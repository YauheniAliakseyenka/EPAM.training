using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data
{
    [DataContract]
    public class Area
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int LayoutId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int CoordX { get; set; }
        [DataMember]
        public int CoordY { get; set; }
		[DataMember]
        public List<Seat> SeatList { get; set; }
    }
}
