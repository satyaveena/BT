using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BT.TS360API.ExternalDataSendService.Models
{
    [DataContract]
    public class SiteTermCategory
    {
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public List<SiteTermObject> SiteTermObjectList { get; set; }
    }

    public class SiteTermResponse
    {
        public List<SiteTermCategory> SiteTermResult { get; set; }
    }
}
