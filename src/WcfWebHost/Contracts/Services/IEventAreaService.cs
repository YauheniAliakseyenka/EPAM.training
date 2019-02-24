using System.ServiceModel;
using System.Threading.Tasks;
using WcfWebHost.Contracts.Data;
using WcfWebHost.FaultExceptions;

namespace WcfWebHost.Contracts.Services
{
    [ServiceContract]
    public interface IWcfEventAreaService
    {
        [OperationContract]
        [FaultContract(typeof(EventAreaFault))]
        [FaultContract(typeof(EventSeatFault))]
        Task<int> Create(EventArea entity);

        [OperationContract]
        [FaultContract(typeof(EventAreaFault))]
        [FaultContract(typeof(EventSeatFault))]
        Task Update(EventArea entity);

        [OperationContract]
        [FaultContract(typeof(EventAreaFault))]
        [FaultContract(typeof(EventSeatFault))]
        Task Delete(int id);

		[OperationContract]
		[FaultContract(typeof(EventAreaFault))]
		Task<EventArea> Get(int id);
	}
}
