using System;
using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data
{
	[DataContract]
	public class Order
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public int UserId { get; set; }
		[DataMember]
		public DateTime Date { get; set; }
	}
}
