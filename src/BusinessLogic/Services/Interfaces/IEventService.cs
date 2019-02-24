using BusinessLogic.BusinessModels;
using BusinessLogic.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IEventService : IStoreService<EventDto, int>
	{
		Task<IEnumerable<EventModel>> GetPublishedEvents(FilterEventOptions filter, string filterText = null, string dateCulture = null);
        Task<EventModel> GetEventInformation(int eventId);
        Task<IEnumerable<EventDto>> GetEventManagerEvents(int venueId, int userId);
        bool HasLockedSeats(int eventId);
	}
}
