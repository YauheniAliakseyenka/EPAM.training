using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfBusinessLogic.Core.Contracts.Data;
using WcfBusinessLogic.Core.Contracts.Exceptions;

namespace WcfBusinessLogic.Core.Contracts.Services
{
	[ServiceContract]
	public interface IWcfPurchaseService
	{
		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task AddSeatToCart(int seatId, int userId);

		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<IEnumerable<SeatBusinessModel>> GetOrderedSeats(int userId);

		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task DeleteSeatFromCart(int seatId);

		/// <summary>
		/// Create an order
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>An amount of order in a string format</returns>
		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task CreateOrder(int userId);

		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<IEnumerable<OrderBusinessModel>> GetPurchaseHistory(int userId);

		/// <summary>
		/// Cancel an order
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>An amount of refund in a string format</returns>
		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task CancelOrderAndRefund(int orderId);
	}
}
