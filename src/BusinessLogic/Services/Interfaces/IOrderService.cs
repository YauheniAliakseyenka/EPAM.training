using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IOrderService
    {
		event EventHandler<OrderEventArgs> Ordered;

		Task Create(int userId);
		Task<IEnumerable<OrderModel>> GetPurchaseHistory(int userId);
		Task CancelOrderAndRefund(int orderId);
	}
}
