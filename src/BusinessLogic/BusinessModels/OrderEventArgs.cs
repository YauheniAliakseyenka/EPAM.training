using BusinessLogic.DTO;
using System;

namespace BusinessLogic.BusinessModels
{
	public class OrderEventArgs : EventArgs
	{
		public UserDto User { get; set; }
		public OrderModel OrderModel { get; set; }

		public OrderEventArgs(UserDto user, OrderModel orderModel)
		{
			User = user;
			OrderModel = orderModel;
		}
	}
}
