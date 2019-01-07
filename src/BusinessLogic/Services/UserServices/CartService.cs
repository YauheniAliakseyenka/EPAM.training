using BusinessLogic.BusinessModels;
using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.UserServices
{
	internal class CartService : ICartService
	{
		private readonly IWorkUnit _context;

		public CartService(IWorkUnit context)
		{
			_context = context;
		}

		public void AddSeat(int seatId, string userId)
		{
			if (_context.EventSeatRepository.Get(seatId).State == 1)
				throw new CartException("Seat is locked");

			var cart = (from carts in _context.CartRepository.GetList()
						where carts.UserId.Equals(userId)
						select carts).FirstOrDefault();

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
						_context.Save();
						cart = newCart;
					}
					_context.OrderedSeatsRepository.Create(new OrderedSeat
					{
						CartId = cart.Id,
						SeatId = seatId
					});
					var updateSeat = _context.EventSeatRepository.Get(seatId);
					updateSeat.State = 1;
					_context.Save();
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public IEnumerable<SeatModel> GetOrderedSeats(string userId)
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
				return result;

			data.ForEach(x =>
			{
				result.Add(new SeatModel
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
				});
			});

			return result;
		}

		public void DeleteUserCart(string userId)
		{
			_context.CartRepository.DeleteBy(x => x.UserId.Equals(userId));
			_context.Save();
		}

		public void UnlockSeat(int seatId)
		{
			var delete = (from orderedSeats in _context.OrderedSeatsRepository.GetList()
						  where orderedSeats.SeatId == seatId
						  select orderedSeats).FirstOrDefault();

			if (delete == null)
				return;

			_context.OrderedSeatsRepository.Delete(delete.SeatId);
			var updateEventSeat = _context.EventSeatRepository.Get(seatId);
			updateEventSeat.State = 0;
			_context.Save();
		}
	}
}
