using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfBusinessLogic.Core.Contracts.Data.BusinessModels;
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
		Task<IEnumerable<SeatModel>> GetOrderedSeats(int userId);

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
		Task<string> CreateOrder(int userId);

		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<IEnumerable<OrderModel>> GetPurchaseHistory(int userId);

		/// <summary>
		/// Cancel an order
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>An amount of refund in a string format</returns>
		[OperationContract]
		[FaultContract(typeof(ServiceValidationFaultDetails))]
		Task<string> CancelOrderAndRefund(int orderId);
	}
}
