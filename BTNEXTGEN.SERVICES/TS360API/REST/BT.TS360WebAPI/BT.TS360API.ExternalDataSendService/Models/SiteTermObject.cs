using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BT.TS360API.ExternalDataSendService.Models
{
    [DataContract]
    public class SiteTermObject
    {
        [DataMember]
        public string Value
        { get; set; }

        [DataMember]
        public string Name
        { get; set; }

        [DataMember]
        public string SearchValue { get; set; }

        [DataMember]
        public List<SiteTermObject> Children { get; set; }

        public string BasketId { get; set; }

        public SiteTermObject()
        {
            this.Value = "";
            this.Name = "";
        }

        public SiteTermObject(string name, string value)
        {
            this.Value = value;
            this.Name = name;
        }

        public SiteTermObject(string name, string value, string basketId)
        {
            this.Value = value;
            this.Name = name;
            this.BasketId = basketId;
        }

        public SiteTermObject(string name, string value, string basketId, string searchValue, List<SiteTermObject> children)
        {
            this.Value = value;
            this.Name = name;
            this.BasketId = basketId;
            this.SearchValue = searchValue;
            this.Children = children;
        }
        
    }
}
