using BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IOrderService
    {
		event EventHandler<OrderEventArgs> Ordered;

		Task Create(string userId);
		Task<IEnumerable<OrderModel>> GetPurchaseHistory(string userId);
    }
}
