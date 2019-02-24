using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data
{
    [DataContract]
    public class Venue
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Timezone { get; set; }
        [DataMember]
        public List<Layout> LayoutList { get; set; }
		[DataMember]
		public string NameWithOffset { get; set; }
	}
}
