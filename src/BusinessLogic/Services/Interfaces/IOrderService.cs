using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IOrderService
    {
		event EventHandler<OrderEventArgs> Ordered;

		/// <summary>
		/// Create an order
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>An order's amount</returns>
		Task<decimal> Create(int userId);
		Task<IEnumerable<OrderModel>> GetPurchaseHistory(int userId);

		/// <summary>
		/// Cancel an order
		/// </summary>
		/// <param name="keys"></param>
		/// <returns>An refund's amount</returns>
		Task<decimal> CancelOrderAndRefund(int orderId);
	}
}
