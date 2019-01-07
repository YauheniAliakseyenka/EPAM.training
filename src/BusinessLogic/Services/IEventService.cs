using BusinessLogic.BusinessModels;
using BusinessLogic.DTO;
using System.Collections.Generic;

namespace BusinessLogic.Services
{
	public interface IEventService : IStoreService<EventDto, int>
	{
		IEnumerable<EventModel> GetPublishedEvents();
		EventModel GetEventStructure(int id);
		IEnumerable<EventDto> GetEventsByVenueIdForParticularEventManager(int venueId, string userId);
		bool HasLockedSeats(int eventId);
	}
}
