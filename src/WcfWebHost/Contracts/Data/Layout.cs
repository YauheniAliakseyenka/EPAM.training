using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfWebHost.Contracts.Data
{
    [DataContract]
    public class Layout
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int VenueId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public List<Area> AreaList { get; set; }
    }
}
