using System.Runtime.Serialization;

namespace WcfWebHost.Contracts.Data
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
