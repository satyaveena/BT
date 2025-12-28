using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BTNextGen.Services.IOrders
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class BasketDetails
    {
        //[DataMember]
        //public string BasketSummaryId { get; set; }

        [DataMember]
        public string CSUserId { get; set; }

        [DataMember]
        public string BasketStatus { get; set; }

        [DataMember]
        public decimal BasketTotal { get; set; }

        [DataMember]
        public DateTime BasketCreatedDate { get; set; }

        [DataMember]
        public DateTime BasketModifiedDate { get; set; }

        [DataMember]
        public string CSBookAccountId { get; set; }

        [DataMember]
        public string CSEntertainmentAccountId { get; set; }

        [DataMember]
        public bool IsHomeDeliveryIndicator { get; set; }

        [DataMember]
        public string HomeDeliveryAccountId { get; set; }
        
        [DataMember]
        public string HomeDeliveryAddressType { get; set; }

        [DataMember]
        public string BasketPONumber { get; set; }

        [DataMember]
        public string BasketSpecialInstructions { get; set; }

        [DataMember]
        public string BasketIsTolas { get; set; }

        [DataMember]
        public decimal BasketStoreShippingFee { get; set; }

        [DataMember]
        public decimal BasketStoreGiftWrapFee { get; set; }

        [DataMember]
        public decimal BasketStoreProcessingFee { get; set; }

        [DataMember]
        public decimal BasketStoreOrderFee { get; set; }

        [DataMember]
        public string AccountWarehouseList { get; set; }

        [DataMember]
        public bool BasketIsBTCart { get; set; }

        [DataMember]
        public string BasketNote { get; set; }

        [DataMember]
        public string AccountInventoryReserveNumber { get; set; }

        [DataMember]
        public string AccountInventoryType { get; set; }

        [DataMember]
        public string LegacyBasketId { get; set; }

        [DataMember]
        public string LegacySourceSystem { get; set; }

        [DataMember]
        public string LegacyBasketName { get; set; }

        [DataMember]
        public Collection<LineItemDetails> LineItemList { get; set; }

    }

    /// <summary>
    /// Data structure definition
    /// </summary>
    [CollectionDataContract]
    public class BasketDetailsCollection : List<BasketDetails>
    {
        [DataMember]
        public List<BasketDetails> BasketCollection { get; set; }
    }

}