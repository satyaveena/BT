using System.Runtime.Serialization;

namespace BT.TS360API.ServiceContracts.Pricing
{
    [DataContract]
    public class LineRepricedInfo
    {
        [DataMember]
        public string BTKey { get; set; }

        [DataMember]
        public string ProductCatalog { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public int TotalLineQuantity { get; set; }

        [DataMember]
        public int TotalOrderQuantity { get; set; }

        [DataMember]
        public string MarketType { get; set; }

        [DataMember]
        public string ProductType { get; set; }

        [DataMember]
        public string AudienceType { get; set; }

        [DataMember]
        public string Pig { get; set; }

        [DataMember]
        public string SiteBranding { get; set; }

        [DataMember]
        public string OrgId { get; set; }

        [DataMember]
        public string OrgName { get; set; }

        public decimal? ListPrice { get; set; }
    }
}
