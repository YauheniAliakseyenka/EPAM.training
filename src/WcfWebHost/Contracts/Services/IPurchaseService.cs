using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WcfWebHost.Contracts.Data.BusinessModels;
using WcfWebHost.FaultExceptions;

namespace WcfWebHost.Contracts.Services
{
	[ServiceContract]
	public interface IWcfPurchaseService
	{
		[OperationContract]
		[FaultContract(typeof(CartFault))]
		Task AddSeatToCart(int seatId, int userId);

		[OperationContract]
		[FaultContract(typeof(CartFault))]
		Task<IEnumerable<SeatModel>> GetOrderedSeats(int userId);

		[OperationContract]
		[FaultContract(typeof(CartFault))]
		Task DeleteSeatFromCart(int seatId);

		[OperationContract]
		[FaultContract(typeof(OrderFault))]
		Task CreateOrder(int userId);

		[OperationContract]
		[FaultContract(typeof(OrderFault))]
		Task<IEnumerable<OrderModel>> GetPurchaseHistory(int userId);

		[OperationContract]
		[FaultContract(typeof(OrderFault))]
		Task CancelOrderAndRefund(int orderId);
	}
}
