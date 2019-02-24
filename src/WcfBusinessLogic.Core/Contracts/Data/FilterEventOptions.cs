using System.Runtime.Serialization;

namespace WcfBusinessLogic.Core.Contracts.Data
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
