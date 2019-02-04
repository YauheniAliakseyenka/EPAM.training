using BusinessLogic.DTO;
using BusinessLogic.Exceptions.EventExceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.EventServices
{
	internal class EventSeatService : IStoreService<EventSeatDto, int>
	{
		private readonly IWorkUnit _context;

		public EventSeatService(IWorkUnit context)
		{
			_context = context;
		}

		public async Task Create(EventSeatDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.EventAreaId <= 0)
				throw new EventSeatException("EventAreaId is invalid");

			if (!IsSeatUnique(entity, true))
				throw new EventSeatException("Seat already exists");

			var add = MapToEventSeat(entity);
			_context.EventSeatRepository.Create(add);
			await _context.SaveAsync();
            entity.Id = add.Id;
		}

		public async Task Delete(int id)
		{
			if (id <= 0)
				throw new ArgumentException();

			var delete = await _context.EventSeatRepository.GetAsync(id);

			if (delete.State != (byte)SeatState.Available)
				throw new EventSeatException("Not allowed to delete. Seat is locked");

            _context.EventSeatRepository.Delete(delete);
			await _context.SaveAsync();
		}

		public async Task<EventSeatDto> Get(int id)
		{
			var seat = await _context.EventSeatRepository.GetAsync(id);

			return seat == null ? null : MapToEventSeatDto(seat);
		}

		public async Task<IEnumerable<EventSeatDto>> GetList()
		{
			var tempSeatList = await _context.EventSeatRepository.GetListAsync();

			return tempSeatList.Select(x => MapToEventSeatDto(x));
		}

		public async Task Update(EventSeatDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.EventAreaId <= 0)
				throw new EventSeatException("EventAreaId is invalid");

			if (!IsSeatUnique(entity,false))
				throw new EventSeatException("Seat already exists");
			
			var update = await _context.EventSeatRepository.GetAsync(entity.Id);
			update.Number = entity.Number;
			update.Row = entity.Row;
			update.State = (byte)entity.State;
			_context.EventSeatRepository.Update(update);
			await _context.SaveAsync();
		}

		private bool IsSeatUnique(EventSeatDto seat, bool isCreating)
		{
			var data = from eventSeats in _context.EventSeatRepository.GetList()
					   where eventSeats.EventAreaId == seat.EventAreaId && eventSeats.Row == seat.Row && eventSeats.Number == seat.Number
					   select eventSeats;

			return isCreating ? 
				!data.Any() :
				!(from eventSeats in data
				  where eventSeats.Id != seat.Id
				  select eventSeats).Any();
		}

		private EventSeatDto MapToEventSeatDto(EventSeat from)
		{
			return new EventSeatDto
			{
				State = (SeatState)from.State,
				EventAreaId = from.EventAreaId,
				Id = from.Id,
				Number = from.Number,
				Row = from.Row
			};
		}

		private EventSeat MapToEventSeat(EventSeatDto from)
		{
			return new EventSeat
			{
				State = (byte)from.State,
				EventAreaId = from.EventAreaId,
				Id = from.Id,
				Number = from.Number,
				Row = from.Row
			};
		}
	}
}
