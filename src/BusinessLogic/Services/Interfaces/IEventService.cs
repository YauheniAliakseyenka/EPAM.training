using BusinessLogic.BusinessModels;
using BusinessLogic.DTO;
using BusinessLogic.Services.EventServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
	public interface IEventService : IStoreService<EventDto, int>
	{
		Task<IEnumerable<EventModel>> GetPublishedEvents(FilterEventOptions filter, string filterText = null);
		Task<EventModel> GetEventInformation(int id);
		Task<IEnumerable<EventDto>> GetEventManagerEvents(int venueId, string userId);
		bool HasLockedSeats(int eventId);
	}
}
