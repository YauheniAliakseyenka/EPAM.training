using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfWebHost.Contracts.Data;
using WcfWebHost.Contracts.Data.BusinessModels;
using WcfWebHost.FaultExceptions;

namespace WcfWebHost.Contracts.Services
{
    [ServiceContract(SessionMode =SessionMode.Allowed)]
	public interface IWcfEventService
    {
        [OperationContract]
		[FaultContract(typeof(EventFault))]
		Task<int> Create(Event entity);

		[OperationContract]
        [FaultContract(typeof(EventFault))]
		Task Update(Event entity);

        [OperationContract]
        [FaultContract(typeof(EventFault))]
		Task Delete(int id);

        [OperationContract]
        [FaultContract(typeof(EventFault))]
		Task<Event> Get(int id);

        [OperationContract]
        [FaultContract(typeof(EventFault))]
		Task<IEnumerable<Event>> ToList();

        [OperationContract]
        [FaultContract(typeof(EventFault))]
        Task<IEnumerable<EventModel>> GetPublishedEvents(FilterEventOptions filter, string filterText = null);

        [OperationContract]
        [FaultContract(typeof(EventFault))]
        Task<EventModel> GetEventInformation(int id);

        [OperationContract]
        [FaultContract(typeof(EventFault))]
        Task<IEnumerable<Event>> GetEventManagerEvents(int venueId, int userId);

        [OperationContract]
        [FaultContract(typeof(EventFault))]
        bool HasLockedSeats(int id);
	}
}
