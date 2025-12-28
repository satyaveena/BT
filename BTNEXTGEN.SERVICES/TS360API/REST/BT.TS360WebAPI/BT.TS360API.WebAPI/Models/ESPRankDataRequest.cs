using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace BT.TS360API.WebAPI.Models
{
    //[XmlRootAttribute("RankRequests", Namespace = "BTNextGen.ESP.RankRequest", IsNullable = false)]  
    [XmlRoot("RankRequests", Namespace = "BTNextGen.ESP.RankRequest")]
    public class ESPRankDataRootRequest
    {
        //[XmlArrayAttribute("RankRequest")] 
        [XmlElement("RankRequest")]
        public ESPRankDataRequest[] espRankDataRequests;
    }


    [XmlRoot("RankRequest")]
    public class ESPRankDataRequest
    {
        public string ESPLibraryID { get; set; }
 
        public string UserName { get; set; }

        public string UserGuid { get; set; }

        public string CartID { get; set; }

        public string BasketName { get; set; }

        //[XmlArrayAttribute("items")] 
        //[XmlArray("items")]
        //[XmlArrayItem("Detail")]
        //[XmlElement("Detail")]
        [XmlArray("Detail")]
        [XmlArrayItem("items")]
        public List<ESPRankDataItemRequest> Detail { get; set; }

    }

    [XmlRoot("items", Namespace = "BTNextGen.ESP.RankRequest")]
    public class ESPRankDataItemRequest
    {
        public string LineItemID { get; set; }
        public string BTKey { get; set; }

        [XmlAttribute("bisac")]
        public string Bisac { get; set; }

        [XmlElement("listPrice")]
        public decimal ListPrice { get; set; }

        [XmlElement("discountedPrice")]
        public decimal DiscountedPrice { get; set; }

    }
}