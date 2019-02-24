using System.Runtime.Serialization;

namespace WcfWebHost.FaultExceptions
{
	[DataContract]
	public class CartFault
	{
		[DataMember]
		public string Message { get; set; }
	}
}
