using BusinessLogic.DTO;
using BusinessLogic.Exceptions.EventExceptions;
using DataAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BusinessLogic.Services.EventServices
{
	internal class EventSeatService : IStoreService<EventSeatDto, int>
	{
		private readonly IWorkUnit _context;

		public EventSeatService(IWorkUnit context)
		{
			_context = context;
		}

		public void Create(EventSeatDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.EventAreaId == 0)
				throw new EventSeatException("EventAreaId equals zero");

			if (!IsSeatUnique(entity, true))
				throw new EventSeatException("Seat already exists");

			var add = mapToEventSeat(entity);
			_context.EventSeatRepository.Create(add);
			_context.Save();
            entity.Id = add.Id;
		}

		public void Delete(int id)
		{
			_context.EventSeatRepository.Delete(id);
			_context.Save();
		}

		public IEnumerable<EventSeatDto> FindBy(Expression<Func<EventSeatDto, bool>> expression)
		{
			var resultList = new List<EventSeatDto>();
			Expression<Func<EventSeat, bool>> predicate = x => expression.Compile().Invoke(mapToEventSeatDto(x));
			var list = _context.EventSeatRepository.FindBy(predicate).ToList();	

			if (!list.Any())
				return resultList;

			list.ForEach(x =>
			{
				resultList.Add(mapToEventSeatDto(x));
			});

			return resultList;
		}

		public EventSeatDto Get(int id)
		{
			var seat = _context.EventSeatRepository.Get(id);

			if (seat == null)
				return null;

			return mapToEventSeatDto(seat);
		}

		public IEnumerable<EventSeatDto> GetList()
		{
			var tempSeatList = _context.EventSeatRepository.GetList();

			if (tempSeatList == null)
				return null;

			var result = new List<EventSeatDto>();
			tempSeatList.ToList().ForEach(x =>
			{
				result.Add(mapToEventSeatDto(x));
			});

			return result;
		}

		public void Update(EventSeatDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.EventAreaId == 0)
				throw new EventSeatException("AreaId equals zero");

			if (!IsSeatUnique(entity,false))
				throw new EventSeatException("Seat already exists");

			var update = _context.EventSeatRepository.FindBy(x => x.Id == entity.Id).FirstOrDefault();
			update.Number = entity.Number;
			update.Row = entity.Row;
			update.State = entity.State;
			_context.EventSeatRepository.Update(update);
			_context.Save();
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
	}
}
