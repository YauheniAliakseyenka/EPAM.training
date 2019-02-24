using BusinessLogic.BusinessModels;
using BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services.EventServices
{
	internal partial class EventService
	{
		public Task<IEnumerable<EventModel>> GetPublishedEvents(FilterEventOptions filter, string filterText = null, string dateCulture = null)
		{
			var result = new List<EventModel>();

			var eventsList = _context.EventRepository.GetList();
			var areasList = _context.EventAreaRepository.GetList();

			var data = from events in eventsList
					   join areas in areasList on events.Id equals areas.EventId
					   where areas.Price > 0
					   group areas by areas.EventId into areaGroup
					   where areaGroup.Count() == (from areas in areasList
												   where areas.EventId == areaGroup.Key
												   select areas.EventId).Count()
					   join events in eventsList on areaGroup.Key equals events.Id
					   join layouts in _context.LayoutRepository.GetList() on events.LayoutId equals layouts.Id
					   join venues in _context.VenueRepository.GetList() on layouts.VenueId equals venues.Id
					   select new { venue = venues, currentEvent = events };

			switch (filter)
			{
				case FilterEventOptions.Title:
					if (!string.IsNullOrEmpty(filterText))
						data = from events in data
							   where events.currentEvent.Title.IndexOf(filterText,0, StringComparison.OrdinalIgnoreCase) != -1
							   select events;
					break;
				case FilterEventOptions.Date:
					if (!string.IsNullOrEmpty(filterText) && !string.IsNullOrEmpty(dateCulture))
					{
						DateTime date;
						try { date = DateTime.Parse(filterText, new CultureInfo(dateCulture)); }
						catch (FormatException) { break; }

						data = from events in data
							   where events.currentEvent.Date.Year == date.Date.Year &&
									 events.currentEvent.Date.Month == date.Date.Month &&
									 events.currentEvent.Date.Day == date.Date.Day
							   select events;
					}
					break;
			}

			//gets events which don't happened by venue's local date
			var list = (from events in data.AsEnumerable()
						where !IsPastDate(events.currentEvent.Date, events.venue.Timezone)
						select events).ToList();

			if (!list.Any())
				return Task.FromResult(result.AsEnumerable());

			list.ToList().ForEach(x =>
			{
				if (!result.Any(y => y.Event.Id == x.currentEvent.Id))
				{
					var add = new EventModel
					{
						Event = MapToEventDto(x.currentEvent),
						Venue = new VenueDto
						{
							Address = x.venue.Address,
							Name = x.venue.Name,
							Phone = x.venue.Phone,
							Timezone = x.venue.Timezone
						}
					};
					result.Add(add);
				}
			});
			result.OrderBy(x => x.Event.Date);

			return Task.FromResult(result.AsEnumerable());
		}

		public Task<EventModel> GetEventInformation(int id)
		{
			var result = new EventModel();

			var data = (from venues in _context.VenueRepository.GetList()
						join layouts in _context.LayoutRepository.GetList() on venues.Id equals layouts.VenueId
						join events in _context.EventRepository.GetList() on layouts.Id equals events.LayoutId
						join areas in _context.EventAreaRepository.GetList() on events.Id equals areas.EventId
						join seats in _context.EventSeatRepository.GetList() on areas.Id equals seats.EventAreaId
                        where events.Id == id
						select new { venue = venues, layoutName = layouts.Description,
                            currentEvent = events, eventArea = areas, eventSeat = seats }).ToList();

			if (!data.Any())
				return Task.FromResult(result);

			bool isPublished = true;
			var first = data.FirstOrDefault();
			result.LayoutName = first.layoutName;
			result.Venue = new VenueDto
			{
				Address = first.venue.Address,
				Description = first.venue.Description,
				Name = first.venue.Name,
				Phone = first.venue.Phone,
				Timezone = first.venue.Timezone
			};
			result.Event = MapToEventDto(first.currentEvent);

			var areaList = new List<EventAreaDto>();
			foreach (var area in data)
			{
				var areaFromList = areaList.Find(x => x.Id == area.eventArea.Id);

				if (isPublished && area.eventArea.Price <= 0)
					isPublished = false;

				if (areaFromList == null)
				{
                    var add = new EventAreaDto
                    {
                        Seats = new List<EventSeatDto>
                        {
                            new EventSeatDto
                            {
                                EventAreaId = area.eventSeat.EventAreaId,
                                Id = area.eventSeat.Id,
                                Number = area.eventSeat.Number,
                                Row  = area.eventSeat.Row,
                                State = (SeatState)area.eventSeat.State
                            }
						},
						AreaDefaultId = area.eventArea.AreaDefaultId,
						CoordX = area.eventArea.CoordX,
						CoordY = area.eventArea.CoordY,
						Description = area.eventArea.Description,
						EventId = area.eventArea.EventId,
						Id = area.eventArea.Id,
						Price = area.eventArea.Price
					};
					areaList.Add(add);
				}
				else
				{
					areaFromList.Seats.Add(new EventSeatDto
					{
						EventAreaId = area.eventSeat.EventAreaId,
						Id = area.eventSeat.Id,
						Number = area.eventSeat.Number,
						Row = area.eventSeat.Row,
						State = (SeatState)area.eventSeat.State
					});
				}
			}
			areaList.Sort();
			areaList.ForEach(x => x.Seats.Sort());
			result.Areas = areaList;
			result.IsPublished = isPublished;

			return Task.FromResult(result);
		}

        public Task<IEnumerable<EventDto>> GetEventManagerEvents(int venueId, int userId)
        {
            var result = new List<EventDto>();

            var data = (from venues in _context.VenueRepository.GetList()
                        join layouts in _context.LayoutRepository.GetList() on venues.Id equals layouts.VenueId
                        join events in _context.EventRepository.GetList() on layouts.Id equals events.LayoutId
                        where venues.Id == venueId && events.CreatedBy == userId
                        select events).ToList();

			if (!data.Any())
				return Task.FromResult(result.AsEnumerable());

            data.ForEach(x =>
			{
				result.Add(MapToEventDto(x));
			});
			result.OrderBy(x => x.Title);

			return Task.FromResult(result.AsEnumerable());
		}

		public bool HasLockedSeats(int eventId)
		{
			var data = (from events in _context.EventRepository.GetList()
						join eventAreas in _context.EventAreaRepository.GetList() on events.Id equals eventAreas.EventId
						join eventSeats in _context.EventSeatRepository.GetList() on eventAreas.Id equals eventSeats.EventAreaId
						where events.Id == eventId && !eventSeats.State.Equals((int)SeatState.Available)
						select eventSeats).ToList();

			if (data.Any())
				return true;

			return false;
		}

		private string VenueTimezoneByLayoutId(int layoutId)
		{
			return (from layouts in _context.LayoutRepository.GetList()
					join venues in _context.VenueRepository.GetList() on layouts.VenueId equals venues.Id
					where layouts.Id == layoutId
					select venues.Timezone).FirstOrDefault();
		}

		private bool IsPastDate(DateTime date, string timezoneId)
		{
			//getting venue's local time
			var venueLocalTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, timezoneId);

			return date <= venueLocalTime;
		}
	}
}
