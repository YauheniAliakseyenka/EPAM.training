using System;
using System.Collections.Generic;
using System.Linq;
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

			if (entity.LayoutId <= 0)
				throw new EventException("LayoutId is invalid");

			if (!IsDateValid(entity, true))
				throw new EventException("Invalid date");
			
			if (IsPastDate(entity))
				throw new EventException("Attempt of creating event with a date in the past");

			var addEvent = MapToEventDAL(entity);
			_context.EventRepository.Create(addEvent);
			await _context.SaveAsync();
            entity.Id = addEvent.Id;
        }

		public async Task Delete(int id)
		{
			if (id <= 0)
				throw new ArgumentException();

			var delete = await _context.EventRepository.GetAsync(id);

			if (HasLockedSeats(id))
				throw new EventException("Not allowed to delete. Event has locked seat");

			_context.EventRepository.Delete(delete);
			await _context.SaveAsync();
		}

		public async Task<EventDto> Get(int id)
		{
			var tempEvent = await _context.EventRepository.GetAsync(id);

			return tempEvent == null ? null : MapToEventDto(tempEvent);
		}	

		public async Task<IEnumerable<EventDto>> GetList()
		{
			var list = await _context.EventRepository.GetListAsync();

			return list.Select(x => MapToEventDto(x));
		}

		public async Task Update(EventDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.LayoutId <= 0)
				throw new EventException("LayoutId is invalid");

			if (!IsDateValid(entity, false))
				throw new EventException("Invalid date");

			if (IsPastDate(entity))
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

		private Event MapToEventDAL(EventDto from)
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

		private EventDto MapToEventDto(Event from)
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

		private bool IsPastDate(EventDto entity)
		{
			//getting venue's local time
			var venueLocalTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, VenueTimezoneByLayoutId(entity.LayoutId));

			return entity.Date <= venueLocalTime;
		}

		private bool IsDateValid(EventDto entity, bool isCreating)
		{
			var data = from events in _context.EventRepository.GetList()
						 where events.LayoutId == entity.LayoutId & events.Date == entity.Date
						 select events;

			return isCreating ? 
				!data.Any() :
				!(from events in data
				  where events.Id != entity.Id
				  select events).Any();
		}
	}
}
