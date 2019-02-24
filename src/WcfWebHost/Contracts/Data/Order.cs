using System;
using System.Runtime.Serialization;

namespace WcfWebHost.Contracts.Data
{
	[DataContract]
	public class Order
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public int UserId { get; set; }
		[DataMember]
		public DateTimeOffset Date { get; set; }
	}
}
