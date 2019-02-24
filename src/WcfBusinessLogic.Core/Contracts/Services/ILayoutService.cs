using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfBusinessLogic.Core.Contracts.Data;
using WcfBusinessLogic.Core.Contracts.Exceptions;

namespace WcfBusinessLogic.Core.Contracts.Services
{
	[ServiceContract]
	public interface IWcfLayoutService
	{
		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<IEnumerable<Layout>> ToList();

		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<IEnumerable<Layout>> GetLayoutsByVenue(int venueId);
	}
}
