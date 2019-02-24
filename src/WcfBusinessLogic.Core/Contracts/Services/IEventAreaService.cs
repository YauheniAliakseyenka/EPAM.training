using System.ServiceModel;
using System.Threading.Tasks;
using WcfBusinessLogic.Core.Contracts.Data;
using WcfBusinessLogic.Core.Contracts.Exceptions;

namespace WcfBusinessLogic.Core.Contracts.Services
{
	[ServiceContract]
	public interface IWcfEventAreaService
    {
		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<int> Create(EventArea entity);

		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task Update(EventArea entity);

		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task Delete(int id);

		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<EventArea> Get(int id);
	}
}
