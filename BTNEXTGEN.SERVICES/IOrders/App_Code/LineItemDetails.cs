using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace BTNextGen.Services.IOrders
{
    /// <summary>
    /// Data structure definition
    /// </summary>
    [DataContract]
    public class LineItemDetails
    {
        [DataMember] 
        public string ProductCatalog {get; set;}
        [DataMember]
         public string BTKey { get; set; }
        [DataMember]
         public decimal Quantity { get; set; }
        [DataMember]
         public decimal ListPrice { get; set; }
        [DataMember]
         public decimal PlacedPrice { get; set; }
        [DataMember]
         public string BTTitle { get; set; }            // WS: Map to Description and DisplayName
        [DataMember]
         public string ProductType {get; set;}
        [DataMember]
         public string BTGiftWrapCode {get; set;}
        [DataMember] 
        public string BTGiftWrapMessage {get; set;}    // ADVN: Rename from BTGiftWarpMessage to BTGiftWrapMessage to fix spelling
        [DataMember]
        public string BTItemType {get; set;}           // BTDEV: Join with Commerce Server View for the product related attributes
        [DataMember]
        public string BTVolumeSet {get; set;}
        [DataMember]
        public string BTTitleEditionVersion {get; set;}
        [DataMember]
        public string BTBibNumber {get; set;}
         //public string BTAuthorOrArtist {get; set;}   // ADVN: Remove, why do we keep this in the basket
       [DataMember]
        public string LinePONumber { get; set; }       // ADVN: Add this attribute for tracebility 
        [DataMember]
        public string LegacyLineItemId { get; set; }   // ADVN: Add this attribute for tracebility
        [DataMember]
        public string LegacyBasketId { get; set; }     // ADVN: Add this attribute for tracebility
        [DataMember] 
        public string LegacyCreatedDate { get; set; }  // ADVN: Add this attribute for tracebility
        [DataMember]
        public string LegacyModifiedDate { get; set; } // ADVN: Add this attribute for tracebility
    }

}