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

		public async Task Create(int userId)
		{
			if (userId <= 0)
				throw new ArgumentException();

			var orderedSeats = await _cartService.GetOrderedSeats(userId);

			if (!orderedSeats.Any())
				throw new OrderException("User has no ordered seats");

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
						Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.Now, user.Timezone),
						UserId = userId
					};
					_context.OrderRepository.Create(order);
					await _context.SaveAsync();

					foreach(var seat in orderedSeats)
					{
						_context.PurchasedSeatRepository.Create(new PurchasedSeat
						{
							SeatId = seat.Seat.Id,
							OrderId = order.Id,
							Price = seat.Area.Price
						});

						var updateSeat = await _context.EventSeatRepository.GetAsync(seat.Seat.Id);
						updateSeat.State = (byte)SeatState.Purchased;
					}
                    await _context.SaveAsync();
					
					user.Amount -= orderTotal;
					await _userService.Update(user);

					await _cartService.DeleteUserCart(userId);
					transaction.Commit();

					Notify(user, order.Id);
				}
				catch(Exception)
				{
					transaction.Rollback();
					throw;
				}
            }
		}

		public Task<IEnumerable<OrderModel>> GetPurchaseHistory(int userId)
		{
            var result = new List<OrderModel>();
			var data = (from users in _context.UserRepository.GetList()
                        join orders in _context.OrderRepository.GetList() on users.Id equals orders.UserId
                        join purchasedSeats in _context.PurchasedSeatRepository.GetList() on orders.Id equals purchasedSeats.OrderId
						join seats in _context.EventSeatRepository.GetList() on purchasedSeats.SeatId equals seats.Id
						join eventAreas in _context.EventAreaRepository.GetList() on seats.EventAreaId equals eventAreas.Id
						join events in _context.EventRepository.GetList() on eventAreas.EventId equals events.Id
						join layouts in _context.LayoutRepository.GetList() on events.LayoutId equals layouts.Id
						join venues in _context.VenueRepository.GetList() on layouts.VenueId equals venues.Id
						where orders.UserId == userId
						select new { order = orders, seat = seats, area = eventAreas, currentEvent = events, layout = layouts,
                            venue = venues, user = users }).ToList();

			if (!data.Any())
				return Task.FromResult(result.AsEnumerable());

			data.ForEach(x =>
			{
				var order = result.Find(y => y.Order.Id == x.order.Id);

				var seatRow = new SeatModel
				{
					Seat = new EventSeatDto
					{
						State = (SeatState)x.seat.State,
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
						Phone = x.venue.Phone,
						Timezone = x.venue.Timezone
					}
				};

				if (order == null)
				{
                    result.Add(new OrderModel
					{
						Order = MapToOrderDtoWithTimezone(x.order, x.user.Timezone),
						PurchasedSeats = new List<SeatModel>() { seatRow }
					});
				}
				else
					order.PurchasedSeats.Add(seatRow);
			});
			result.OrderBy(x => x.Order.Date);

			return Task.FromResult(result.AsEnumerable());
		}

		private OrderDto MapToOrderDtoWithTimezone(Order from, string timezone)
		{
			var timezoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
			return new OrderDto
			{
				Date = TimeZoneInfo.ConvertTimeFromUtc(from.Date.UtcDateTime, timezoneInfo),
				Id = from.Id,
				UserId = from.UserId
			};
		}

		private async void Notify(UserDto user, int orderId)
		{
			var puchases = await GetPurchaseHistory(user.Id);
			var args = new OrderEventArgs(user, puchases.FirstOrDefault(x => x.Order.Id == orderId));
			Ordered?.Invoke(this, args);
		}

		public async Task CancelOrderAndRefund(int orderId)
		{
			var order = await _context.OrderRepository.GetAsync(orderId);

			if (order == null)
				throw new OrderException("Order does not exist");

			using (var transaction = _context.CreateTransaction())
			{
				try
				{
					var data = (from areas in _context.EventAreaRepository.GetList()
								join eventSeats in _context.EventSeatRepository.GetList() on areas.Id equals eventSeats.EventAreaId
								join purchasedSeats in _context.PurchasedSeatRepository.GetList() on eventSeats.Id equals purchasedSeats.SeatId
								where purchasedSeats.OrderId == orderId
								select new { eventSeats, price = areas.Price }).ToList();
					
					var amount = data.Sum(x => x.price);
					var percent = 10;
					var user = await _userService.Get(order.UserId);

					data.ForEach(x =>
					{
						x.eventSeats.State = (byte)SeatState.Available;
					});

					_context.OrderRepository.Delete(order);
					user.Amount += amount - ((amount * percent) / 100);
					await _userService.Update(user);
					await _context.SaveAsync();
					transaction.Commit();
				}
				catch(Exception)
				{
					transaction.Rollback();
					throw;
				}
			}
		}
	}
}
