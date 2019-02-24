using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfBusinessLogic.Core.Contracts.Data;
using WcfBusinessLogic.Core.Contracts.Data.BusinessModels;
using WcfBusinessLogic.Core.Contracts.Exceptions;

namespace WcfBusinessLogic.Core.Contracts.Services
{
    [ServiceContract]
    public interface IWcfEventService
    {
        [OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<int> Create(Event entity);

		[OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task Update(Event entity);

        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task Delete(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<Event> Get(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<IEnumerable<Event>> ToList();

        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<IEnumerable<EventModel>> GetPublishedEvents(FilterEventOptions filter, string filterText, string dateCulture);

        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<EventModel> GetEventInformation(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<IEnumerable<Event>> GetEventManagerEvents(int venueId, int userId);

        [OperationContract]
        bool HasLockedSeats(int id);
	}
}
