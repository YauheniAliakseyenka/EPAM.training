using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data
{
    [DataContract]
    public enum SeatState
    {
        [EnumMember]
        Available = 0,
        [EnumMember]
        Ordered = 1,
        [EnumMember]
		Purchased = 2
    }
}
