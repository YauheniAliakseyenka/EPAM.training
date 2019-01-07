using BusinessLogic.BusinessModels;
using BusinessLogic.Services.UserServices;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Services
{
    public interface IOrderService
    {
		event EventHandler<EmailEventArgs> OrderCompleted;

		void Create(string userId);
		IEnumerable<OrderModel> GetPurchaseHistory(string userId);
    }
}
