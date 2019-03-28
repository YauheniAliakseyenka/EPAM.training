using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfBusinessLogic.Core.Contracts.Data;
using WcfBusinessLogic.Core.Contracts.Exceptions;

namespace WcfBusinessLogic.Core.Contracts.Services
{
    [ServiceContract]
    public interface IWcfVenueService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<int> Create(Venue entity);

        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task Update(Venue entity);

        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task Delete(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<Venue> Get(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceValidationFaultDetails))]
        Task<Venue> GetFullModel(int id);

        [OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<IEnumerable<Venue>> ToList();
	}
}
