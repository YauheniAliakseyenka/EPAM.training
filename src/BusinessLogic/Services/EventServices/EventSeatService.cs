using BusinessLogic.DTO;
using BusinessLogic.Exceptions.EventExceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

			if (entity.EventAreaId == 0)
				throw new EventSeatException("EventAreaId equals zero");

			if (!IsSeatUnique(entity, true))
				throw new EventSeatException("Seat already exists");

			var add = mapToEventSeat(entity);
			_context.EventSeatRepository.Create(add);
			await _context.SaveAsync();
            entity.Id = add.Id;
		}

		public async Task Delete(int id)
		{
			if (await isSeatLocked(id))
				throw new EventSeatException("Not allowed to deleted. Seat is locked");

            _context.EventSeatRepository.Delete(id);
			await _context.SaveAsync();
		}

		public Task<IEnumerable<EventSeatDto>> FindBy(Expression<Func<EventSeatDto, bool>> expression)
		{
			var resultList = new List<EventSeatDto>();
			Expression<Func<EventSeat, bool>> predicate = x => expression.Compile().Invoke(mapToEventSeatDto(x));
			var list =  _context.EventSeatRepository.FindBy(predicate);

			return Task.FromResult(list.Select(x => mapToEventSeatDto(x)).AsEnumerable());
		}

		public async Task<EventSeatDto> Get(int id)
		{
			var seat = await _context.EventSeatRepository.GetAsync(id);

			if (seat == null)
				return null;

			return mapToEventSeatDto(seat);
		}

		public async Task<IEnumerable<EventSeatDto>> GetList()
		{
			var tempSeatList = await _context.EventSeatRepository.GetListAsync();

			return tempSeatList.Select(x => mapToEventSeatDto(x));
		}

		public async Task Update(EventSeatDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.EventAreaId == 0)
				throw new EventSeatException("AreaId equals zero");

			if (!IsSeatUnique(entity,false))
				throw new EventSeatException("Seat already exists");
			
			var update = await _context.EventSeatRepository.GetAsync(entity.Id);
			update.Number = entity.Number;
			update.Row = entity.Row;
			update.State = entity.State;
			_context.EventSeatRepository.Update(update);
			await _context.SaveAsync();
		}

		private bool IsSeatUnique(EventSeatDto seat, bool isCreating)
		{
			return isCreating ? !_context.EventSeatRepository.FindBy(c => c.EventAreaId == seat.EventAreaId &&
			(c.Row == seat.Row & c.Number == seat.Number)).Any() :
				!_context.EventSeatRepository.FindBy(c => c.Id != seat.Id && (c.EventAreaId == seat.EventAreaId &&
			(c.Row == seat.Row & c.Number == seat.Number))).Any();
		}

		private EventSeatDto mapToEventSeatDto(EventSeat from)
		{
			return new EventSeatDto
			{
				State = from.State,
				EventAreaId = from.EventAreaId,
				Id = from.Id,
				Number = from.Number,
				Row = from.Row
			};
		}

		private EventSeat mapToEventSeat(EventSeatDto from)
		{
			return new EventSeat
			{
				State = from.State,
				EventAreaId = from.EventAreaId,
				Id = from.Id,
				Number = from.Number,
				Row = from.Row
			};
		}

		private async Task<bool> isSeatLocked(int seatId)
		{
			var seat = await _context.EventSeatRepository.GetAsync(seatId);
			return seat.State == 1;
		}
	}
}
