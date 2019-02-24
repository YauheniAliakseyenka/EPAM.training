using System.Runtime.Serialization;

namespace WcfWebHost.FaultExceptions
{
	[DataContract]
	public class OrderFault
	{
		[DataMember]
		public string Message { get; set; }
	}
}
