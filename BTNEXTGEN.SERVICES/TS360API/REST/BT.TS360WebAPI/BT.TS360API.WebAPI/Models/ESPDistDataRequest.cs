using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace BT.TS360API.WebAPI.Models
{
    [XmlRoot("DistributionRequests", Namespace = "BTNextGen.ESP.DistributionRequest")]
    public class ESPDistDataRootRequest
    {
        [XmlElement("DistributionRequest")]
        public ESPDistDataRequest[] espDistDataRequests;
    }

    [XmlRoot("DistributionRequest")]
    public class ESPDistDataRequest
    {
        public string ESPLibraryID { get; set; }
 
        public string UserName { get; set; }

        public string UserGuid { get; set; }
        public string CartID { get; set; }

        public string BasketName { get; set; }
        public string FundMonitoring { get; set; }

        [XmlArray("Branches")]
        [XmlArrayItem("branches")]
        public List<ESPDistDataBranchRequest> Branches { get; set; }

        [XmlArray("Items")]
        [XmlArrayItem("items")]
        public List<ESPDistDataItemRequest> Items { get; set; }

      
    
    }

    [XmlRoot("items", Namespace = "BTNextGen.ESP.DistributionRequest")]
    public class ESPDistDataItemRequest
    {
        public string LineItemID { get; set; }
        public string BTKey { get; set; }

        //[XmlAttribute("fundid")]
        [XmlElement("fundid")]
        public string Fundid { get; set; }

        //[XmlAttribute("fundcode")]
        [XmlElement("fundcode")]
        public string Fundcode { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
        public string Quantity { get; set; }

        //[XmlAttribute("bisac")]
        [XmlElement("bisac")]
        public string Bisac { get; set; }

        [XmlElement("listprice")]
        public decimal ListPrice { get; set; }

        [XmlElement("discountedprice")]
        public decimal DiscountedPrice { get; set; }

    }

    [XmlRoot("branches", Namespace = "BTNextGen.ESP.DistributionRequest")]
    public class ESPDistDataBranchRequest
    {
        [XmlElement("branchid")]
        public string Branchid { get; set; }

        [XmlElement("code")]
        public string Code { get; set; }
    }
}