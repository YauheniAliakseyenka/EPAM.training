using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Entities;
using DataAccess;
using BusinessLogic.Exceptions.EventExceptions;
using BusinessLogic.DTO;

namespace BusinessLogic.Services.EventServices
{
	internal partial class EventService : IEventService
	{
		private IWorkUnit _context;

		public EventService(IWorkUnit context)
		{
			_context = context;
		}

		public void Create(EventDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if(entity.LayoutId == 0)
				throw new EventException("LayoutId equals zero");
			
			if (!isDateValid(entity, true))
				throw new EventException("Invalid date");

			if (isPastDate(entity.Date))
				throw new EventException("Attempt of creating event with a date in the past");

			_context.EventRepository.Create(mapToEventDAL(entity));
			_context.Save();
        }

		public void Delete(int id)
		{
			if (!HasLockedSeats(id))
				throw new EventException("Not allowed to delete. Event has locked seat");

			_context.EventRepository.Delete(id);
			_context.Save();
		}

		public IEnumerable<EventDto> FindBy(Expression<Func<EventDto, bool>> expression)
		{
			var result = new List<EventDto>();
			Expression<Func<Event, bool>> predicate = x => expression.Compile().Invoke(mapToEventDto(x));
			var list = _context.EventRepository.FindBy(predicate);

			if (!list.Any())
				return result;

			list.ToList().ForEach(x =>
			{
				result.Add(mapToEventDto(x));
			});

			return result;
		}

		public EventDto Get(int id)
		{
			var tempEvent = _context.EventRepository.Get(id);

			if (tempEvent == null)
				return null;

			return mapToEventDto(tempEvent);
		}	

		public IEnumerable<EventDto> GetList()
		{
			return (from events in _context.EventRepository.GetList()
					select mapToEventDto(events));
		}

		public void Update(EventDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId == 0)
				throw new EventException("Layout equals zero");

			if (!isDateValid(entity, false))
				throw new EventException("Invalid date");

			if (isPastDate(entity.Date))
				throw new EventException("Attempt of creating event with a date in the past");

			var update = _context.EventRepository.Get(entity.Id);

			if(update.LayoutId != entity.LayoutId && !HasLockedSeats(entity.Id))
				throw new EventException("Not allowed to update layout. Event has locked seats");

			update.Title = entity.Title;
			update.LayoutId = entity.LayoutId;
			update.ImageURL = entity.ImageURL;
			update.Description = entity.Description;
			update.Date = entity.Date;
			_context.EventRepository.Update(update);
			_context.Save();
		}

		private Event mapToEventDAL(EventDto from)
		{
			return new Event
			{
				CreatedBy = from.CreatedBy,
				Date = from.Date,
				Description = from.Description,
				Id = from.Id,
				ImageURL = from.ImageURL,
				LayoutId = from.LayoutId,
				Title = from.Title
			};
		}

		private EventDto mapToEventDto(Event from)
		{
			return new EventDto
			{
				CreatedBy = from.CreatedBy,
				Date = from.Date,
				Description = from.Description,
				Id = from.Id,
				ImageURL = from.ImageURL,
				LayoutId = from.LayoutId,
				Title = from.Title
			};
		}

		private bool isPastDate(DateTime date)
		{
			return date <= DateTime.Now;
		}

		private bool isDateValid(EventDto entity, bool isCreating)
		{
			return isCreating ?
				!(from events in _context.EventRepository.GetList()
				 where events.LayoutId == entity.LayoutId & events.Date == entity.Date
				 select events).Any() :
				!(from events in _context.EventRepository.GetList()
				 where events.Id != entity.Id && (events.LayoutId == entity.LayoutId & events.Date == entity.Date)
				 select events).Any();
		}
	}
}
