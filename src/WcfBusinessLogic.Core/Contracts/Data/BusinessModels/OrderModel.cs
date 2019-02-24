using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data.BusinessModels
{
	[DataContract]
	public class OrderModel
	{
		[DataMember]
		public Order Order { get; set; }
		[DataMember]
		public List<SeatModel> PurchasedSeats { get; set; }
	}
}
