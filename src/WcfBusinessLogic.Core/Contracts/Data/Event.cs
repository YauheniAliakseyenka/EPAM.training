using System;
using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data
{
    [DataContract]
    public class Event
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ImageURL { get; set; }
        [DataMember]
        public int LayoutId { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
}
