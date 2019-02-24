using System.Runtime.Serialization;

namespace WcfWebHost.FaultExceptions
{
    [DataContract]
    public class EventAreaFault
    {
        [DataMember]
        public string Message { get; set; }
    }
}
