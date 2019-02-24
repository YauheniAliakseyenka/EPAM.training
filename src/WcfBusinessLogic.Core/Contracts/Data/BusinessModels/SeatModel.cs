using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data.BusinessModels
{
    [DataContract]
    public class SeatModel
    {
        [DataMember]
        public Event Event { get; set; }
        [DataMember]
        public EventSeat Seat { get; set; }
        [DataMember]
        public EventArea Area { get; set; }
        [DataMember]
        public Layout Layout { get; set; }
        [DataMember]
        public Venue Venue { get; set; }
    }
}
