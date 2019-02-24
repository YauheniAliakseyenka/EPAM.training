using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data
{
    [DataContract]
    public class EventSeat
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int EventAreaId { get; set; }
        [DataMember]
        public int Row { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public SeatState State { get; set; }
    }
}
