using System.Runtime.Serialization;

namespace WcfWebHost.FaultExceptions
{
	[DataContract]
	public class AccessFault
	{
		[DataMember]
		public string Message { get; set; }
	}
}