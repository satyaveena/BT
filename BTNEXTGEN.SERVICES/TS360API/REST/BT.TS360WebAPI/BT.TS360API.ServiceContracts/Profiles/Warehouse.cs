using System;
using System.Runtime.Serialization;

namespace BT.TS360API.ServiceContracts.Profiles
{
    [DataContract]
    public class Warehouse
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime? DateCreated { get; set; }
    }
}
