using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Entities;
using DataAccess;
using BusinessLogic.Exceptions.EventExceptions;
using BusinessLogic.DTO;
using System.Threading.Tasks;

namespace BusinessLogic.Services.EventServices
{
	internal partial class EventService : IEventService
	{
		private IWorkUnit _context;

		public EventService(IWorkUnit context)
		{
			_context = context;
		}

		public async Task Create(EventDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId == 0)
				throw new EventException("LayoutId equals zero");

			if (!isDateValid(entity, true))
				throw new EventException("Invalid date");

			if (isPastDate(entity.Date))
				throw new EventException("Attempt of creating event with a date in the past");

			var addEvent = mapToEventDAL(entity);
            _context.EventRepository.Create(addEvent);
			await _context.SaveAsync();
			entity.Id = addEvent.Id;
        }

		public async Task Delete(int id)
		{
			if (HasLockedSeats(id))
				throw new EventException("Not allowed to delete. Event has locked seat");

			_context.EventRepository.Delete(id);
			await _context.SaveAsync();
		}

		public Task<IEnumerable<EventDto>> FindBy(Expression<Func<EventDto, bool>> expression)
		{
			Expression<Func<Event, bool>> predicate = x => expression.Compile().Invoke(mapToEventDto(x));
			var list = _context.EventRepository.FindBy(predicate);

			return Task.FromResult(list.Select(x => mapToEventDto(x)).AsEnumerable());
		}

		public async Task<EventDto> Get(int id)
		{
			var tempEvent = await _context.EventRepository.GetAsync(id);

			if (tempEvent == null)
				return null;

			return mapToEventDto(tempEvent);
		}	

		public async Task<IEnumerable<EventDto>> GetList()
		{
			var list = await _context.EventRepository.GetListAsync();

			return list.Select(x => mapToEventDto(x));
		}

		public async Task Update(EventDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId == 0)
				throw new EventException("Layout equals zero");

			if (!isDateValid(entity, false))
				throw new EventException("Invalid date");

			if (isPastDate(entity.Date))
				throw new EventException("Attempt of updating event with a date in the past");

			var update = await _context.EventRepository.GetAsync(entity.Id);

			if (update.LayoutId != entity.LayoutId && HasLockedSeats(entity.Id))
				throw new EventException("Not allowed to update layout. Event has locked seats");

			update.Title = entity.Title;
			update.LayoutId = entity.LayoutId;
			update.ImageURL = entity.ImageURL;
			update.Description = entity.Description;
			update.Date = entity.Date;
			_context.EventRepository.Update(update);
			await _context.SaveAsync();
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
