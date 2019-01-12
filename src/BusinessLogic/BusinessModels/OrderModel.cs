using BusinessLogic.DTO;
using System.Collections.Generic;

namespace BusinessLogic.BusinessModels
{
	public class OrderModel
	{
	    public OrderDto Order { get; set; }
		public List<SeatModel> PurchasedSeats { get; set; }
	}
}
