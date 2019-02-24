using System.Runtime.Serialization;

namespace WcfWebHost.Contracts.Data
{
    [DataContract]
    public enum FilterEventOptions
    {
        [EnumMember]
        None,
        [EnumMember]
        Title,
        [EnumMember]
        Date
    }
}
