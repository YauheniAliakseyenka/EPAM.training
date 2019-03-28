using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data
{
	[DataContract]
	public class OrderBusinessModel
	{
		[DataMember]
		public Order Order { get; set; }
		[DataMember]
		public List<SeatBusinessModel> PurchasedSeats { get; set; }
	}
}
