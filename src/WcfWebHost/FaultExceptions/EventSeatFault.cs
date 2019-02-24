using System.Runtime.Serialization;

namespace WcfWebHost.FaultExceptions
{
    [DataContract]
    public class EventSeatFault
    {
        [DataMember]
        public string Message { get; set; }
    }
}
