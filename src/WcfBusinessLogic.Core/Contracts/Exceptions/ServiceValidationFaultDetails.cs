using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Exceptions
{
	[DataContract]
	public class ServiceValidationFaultDetails
	{
		[DataMember]
		public string Message { get; set; }
	}
}
