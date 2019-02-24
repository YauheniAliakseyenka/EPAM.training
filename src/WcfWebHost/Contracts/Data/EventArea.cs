using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfWebHost.Contracts.Data
{
    [DataContract]
    public class EventArea
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int EventId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int CoordX { get; set; }
        [DataMember]
        public int CoordY { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public int AreaDefaultId { get; set; }
        [DataMember]
        public List<EventSeat> Seats { get; set; }
    }
}
