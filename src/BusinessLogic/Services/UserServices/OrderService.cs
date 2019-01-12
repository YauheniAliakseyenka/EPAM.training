using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Exceptions;
using DataAccess;
using DataAccess.Entities;
using BusinessLogic.BusinessModels;
using BusinessLogic.DTO;
using System.Threading.Tasks;

namespace BusinessLogic.Services.UserServices
{
    internal class OrderService : IOrderService
    {
		public event EventHandler<OrderEventArgs> Ordered;

		private readonly IWorkUnit _context;
		private ICartService _cartService;
		private IUserService _userService;

		public OrderService(IWorkUnit context, ICartService cartService, IUserService userService)
		{
			_context = context;
			_cartService = cartService;
			_userService = userService;
		}

		public async Task Create(string userId)
		{
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException();

			var orderedSeats = await _cartService.GetOrderedSeats(userId);

			if (!orderedSeats.Any())
				return;

			using (var transaction = _context.CreateTransaction())
            {
                try
				{
					var user = await _userService.Get(userId);
					var orderTotal = orderedSeats.Sum(x => x.Area.Price);

					if (user.Amount < orderTotal)
						throw new OrderException("Balance of user is less than total amount of order");

					var order = new Order
					{
						Date = DateTime.Now,
						UserId = userId
					};
					_context.OrderRepository.Create(order);
					await _context.SaveAsync();

					orderedSeats.ToList().ForEach(x =>
					{
						_context.PurchasedSeatRepository.Create(new PurchasedSeat
						{
							SeatId = x.Seat.Id,
							OrderId = order.Id,
							Price = x.Area.Price
						});
					});
                    await _context.SaveAsync();
					
					user.Amount -= orderTotal;
					await _userService.Update(user);

					notify(user, order.Id);

					await _cartService.DeleteUserCart(userId);
					transaction.Commit();
				}
				catch(Exception)
				{
					transaction.Rollback();
					throw;
				}
            }
		}

		public Task<IEnumerable<OrderModel>> GetPurchaseHistory(string userId)
		{
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException();

            var result = new List<OrderModel>();
			var data = (from orders in _context.OrderRepository.GetList()
						join purchasedSeats in _context.PurchasedSeatRepository.GetList() on orders.Id equals purchasedSeats.OrderId
						join seats in _context.EventSeatRepository.GetList() on purchasedSeats.SeatId equals seats.Id
						join eventAreas in _context.EventAreaRepository.GetList() on seats.EventAreaId equals eventAreas.Id
						join events in _context.EventRepository.GetList() on eventAreas.EventId equals events.Id
						join layouts in _context.LayoutRepository.GetList() on events.LayoutId equals layouts.Id
						join venues in _context.VenueRepository.GetList() on layouts.VenueId equals venues.Id
						where orders.UserId.Equals(userId, StringComparison.Ordinal)
						select new { order = orders, seat = seats, area = eventAreas, currentEvent = events, layout = layouts, venue = venues }).ToList();

			if (!data.Any())
				return Task.FromResult(result.AsEnumerable());

			data.ForEach(x =>
			{
				var order = result.Find(y => y.Order.Id == x.order.Id);

				var seatRow = new SeatModel
				{
					Seat = new EventSeatDto
					{
						State = x.seat.State,
						EventAreaId = x.seat.EventAreaId,
						Id = x.seat.Id,
						Number = x.seat.Number,
						Row = x.seat.Row
					},
					Area = new EventAreaDto
					{
						Seats = new List<EventSeatDto>(),
						AreaDefaultId = x.area.AreaDefaultId,
						CoordX = x.area.CoordX,
						CoordY = x.area.CoordY,
						Description = x.area.Description,
						EventId = x.area.EventId,
						Id = x.area.Id,
						Price = x.area.Price
					},
					Event = new EventDto
					{
						CreatedBy = x.currentEvent.CreatedBy,
						Date = x.currentEvent.Date,
						Description = x.currentEvent.Description,
						Id = x.currentEvent.Id,
						ImageURL = x.currentEvent.ImageURL,
						LayoutId = x.currentEvent.LayoutId,
						Title = x.currentEvent.Title
					},
					Layout = new LayoutDto
					{
						AreaList = new List<AreaDto>(),
						Description = x.layout.Description,
						Id = x.layout.Id,
						VenueId = x.layout.VenueId
					},
					Venue = new VenueDto
					{
						Address = x.venue.Address,
						Description = x.venue.Description,
						Id = x.venue.Id,
						LayoutList = new List<LayoutDto>(),
						Name = x.venue.Name,
						Phone = x.venue.Phone
					}
				};

				if (order == null)
				{
					result.Add(new OrderModel
					{
						Order = mapToOrderDto(x.order),
						PurchasedSeats = new List<SeatModel>() { seatRow }
					});
				}
				else
					order.PurchasedSeats.Add(seatRow);
			});

			return Task.FromResult(result.AsEnumerable());
		}

		private OrderDto mapToOrderDto(Order from)
		{
			return new OrderDto
			{
				Date = from.Date,
				Id = from.Id,
				UserId = from.UserId
			};
		}

		private async void notify(UserDto user, int orderId)
		{
			var puchases = await GetPurchaseHistory(user.Id);
			var args = new OrderEventArgs(user, puchases.FirstOrDefault(x => x.Order.Id == orderId));

			Ordered?.Invoke(this, args);
		}
	}
}
