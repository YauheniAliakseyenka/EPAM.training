using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data
{
    [DataContract]
    public class Seat
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int AreaId { get; set; }
        [DataMember]
        public int Row { get; set; }
        [DataMember]
        public int Number { get; set; }
    }
}
