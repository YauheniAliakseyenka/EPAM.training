using BusinessLogic.BusinessModels;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.UserServices
{
	internal class CartService : ICartService
	{
		private readonly IWorkUnit _context;
		private readonly IStoreService<EventSeatDto,int> _eventSeatService;

		public CartService(IWorkUnit context, IStoreService<EventSeatDto, int> eventSeatService)
		{
			_context = context;
			_eventSeatService = eventSeatService;
		}

        public async Task AddSeat(int seatId, int userId)
        {
			if (seatId <= 0 || userId <= 0)
				throw new ArgumentException();

			var seat = await _eventSeatService.Get(seatId);
			if (!seat.State.Equals(SeatState.Available))
                throw new CartException("Seat is locked");

			var findCart = await _context.CartRepository.FindByAsync(x => x.UserId == userId);
			var cart = findCart.FirstOrDefault();

            using (var transaction = _context.CreateTransaction())
            {
                try
                {
                    if (cart == null)
                    {
                        var newCart = new Cart
                        {
                            UserId = userId
                        };
                        _context.CartRepository.Create(newCart);
						await _context.SaveAsync();
                        cart = newCart;
                    }

					seat.State = SeatState.Ordered;
					await _eventSeatService.Update(seat);

					_context.OrderedSeatsRepository.Create(new OrderedSeat
                    {
                        CartId = cart.Id,
                        SeatId = seatId
                    });
					await _context.SaveAsync();
					transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

		public Task<IEnumerable<SeatModel>> GetOrderedSeats(int userId)
		{
			var result = new List<SeatModel>();

			var data = (from carts in _context.CartRepository.GetList()
						join orderedSeats in _context.OrderedSeatsRepository.GetList() on carts.Id equals orderedSeats.CartId
						join seats in _context.EventSeatRepository.GetList() on orderedSeats.SeatId equals seats.Id
						join areas in _context.EventAreaRepository.GetList() on seats.EventAreaId equals areas.Id
						join events in _context.EventRepository.GetList() on areas.EventId equals events.Id
						join layouts in _context.LayoutRepository.GetList() on events.LayoutId equals layouts.Id
						join venues in _context.VenueRepository.GetList() on layouts.VenueId equals venues.Id
						where carts.UserId == userId
						select new { seat = seats, currentEvent = events, layout = layouts, area = areas, venue = venues }).ToList();

			if (!data.Any())
				return Task.FromResult(result.AsEnumerable());

			data.ForEach(x =>
			{
				result.Add(new SeatModel
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
				});
			});

			return Task.FromResult(result.AsEnumerable());
		}

		public async Task DeleteUserCart(int userId)
		{
			if (userId <= 0)
				throw new ArgumentException();

			var delete = await _context.CartRepository.FindByAsync(x => x.UserId == userId);
            _context.CartRepository.Delete(delete.FirstOrDefault());
			await _context.SaveAsync();
		}

		public async Task DeleteSeat(int seatId)
		{
			if (seatId <= 0)
				throw new ArgumentException();

			var delete = await _context.OrderedSeatsRepository.GetAsync(seatId);

			if (delete == null)
				return;

			var update = await _eventSeatService.Get(seatId);
			_context.OrderedSeatsRepository.Delete(delete);
			update.State = SeatState.Available;
			await _eventSeatService.Update(update);
			await _context.SaveAsync();
		}
	}
}
