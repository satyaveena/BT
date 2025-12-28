using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ESPAutoRankingConsole.DataModels
{
    [XmlRoot("RankRequests")]
    public class AutoRankRequests
    {
        [XmlElement("RankRequest")]
        public List<AutoRankRequest> Requests { get; set; }
    }

    public class AutoRankRequest
    {
        [XmlElement("ESPLibraryID")]
        public string ESPLibraryID { get; set; }
        [XmlElement("UserName")]
        public string UserName { get; set; }
        [XmlElement("UserGuid")]
        public string UserGuid { get; set; }
        [XmlElement("CartID")]
        public string CartID { get; set; }
        [XmlElement("BasketName")]
        public string BasketName { get; set; }

        [XmlElement("Detail")]
        public RankRequestDetail Detail { get; set; }
    }

    public class RankRequestDetail
    {
        [XmlElement("items")]
        public List<RankRequestLineItem> LineItems { get; set; }
    }

    public class RankRequestLineItem
    {
        [XmlElement("LineItemID")]
        public string LineItemID { get; set; }
        [XmlElement("BTKey")]
        public string BTKey { get; set; }
        [XmlElement("bisac")]
        public string Bisac { get; set; }

        // new in Release 4.2
        [XmlElement("listPrice")]
        public decimal ListPrice { get; set; }
        [XmlElement("discountedPrice")]
        public decimal DiscountedPrice { get; set; }

    }

    public class ESPRankJsonRequest
    {
        [XmlAttribute("espLibraryId")]
        public string espLibraryId { get; set; }
        [XmlAttribute("userName")]
        public string userName { get; set; }
        [XmlAttribute("cartId")]
        public string cartId { get; set; }
        [XmlAttribute("cartName")]
        public string cartName { get; set; } // new in Release 4.2
        [XmlAttribute("userId")]
        public string userId { get; set; } // new in Release 4.2
       
        public List<ESPRankItemJsonRequest> items { get; set; }
    }

    public class ESPRankItemJsonRequest
    {
        [XmlAttribute("lineItemId")]
        public string lineItemId { get; set; }
        [XmlAttribute("vendorId")]
        public string vendorId { get; set; }
        [XmlAttribute("fundId")]
        public string fundId { get; set; }
        [XmlAttribute("fundCode")]
        public string fundCode { get; set; }
        [XmlAttribute("price")]
        public decimal price { get; set; }
        [XmlAttribute("quantity")]
        public int quantity { get; set; }
        [XmlAttribute("series")]
        public string series { get; set; }
        [XmlAttribute("bisac")]
        public string bisac { get; set; }
        [XmlAttribute("dewey")]
        public string dewey { get; set; }
        [XmlAttribute("publisher")]
        public string publisher { get; set; }

        // new in Release 4.2
        [XmlAttribute("listPrice")]
        public decimal listPrice { get; set; }
        [XmlAttribute("discountedPrice")]
        public decimal discountedPrice { get; set; }

    }
}