using System.Runtime.Serialization;

namespace WcfWebHost.FaultExceptions
{
	[DataContract]
	public class EventFault
	{
		[DataMember]
		public string Message { get; set; }
	}
}
