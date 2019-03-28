using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data
{
    [DataContract]
    public class EventBusinessModel
    {
        [DataMember]
        public Venue Venue { get; set; }
        [DataMember]
        public string LayoutName { get; set; }
        [DataMember]
        public List<EventArea> Areas { get; set; }
        [DataMember]
        public Event Event { get; set; }
        [DataMember]
        public bool IsPublished { get; set; }
    }
}
