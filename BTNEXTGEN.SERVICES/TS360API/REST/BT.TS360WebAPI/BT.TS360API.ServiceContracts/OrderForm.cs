using System;
using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts
{
    public class OrderForm
    {
        //Total number
        /// <summary>
        /// Handling Total
        /// </summary>
        public decimal? HandlingTotal { get; set; }

        /// <summary>
        /// Shipping Total
        /// </summary>
        public decimal? ShippingTotal { get; set; }

        /// <summary>
        /// Sub Total
        /// </summary>
        public decimal? SubTotal { get; set; }

        /// <summary>
        /// Tax Total
        /// </summary>
        public decimal? TaxTotal { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        public decimal? Total { get; set; }

        public bool IsHomeDelivery { get; set; }

        //Address
        /// <summary>
        /// Address ID
        /// </summary>
        public string AddressID { get; set; }

        //Address
        /// <summary>
        /// Address Line #1
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Address Line #2
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Address Line #3
        /// </summary>
        public string AddressLine3 { get; set; }

        /// <summary>
        /// Address Line #3
        /// </summary>
        public string AddressLine4 { get; set; }

        /// <summary>
        /// Flag PO Box
        /// </summary>
        public bool IsPoBox { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Region Code
        /// </summary>
        public string RegionCode { get; set; }

        /// <summary>
        /// Postal Code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Country Code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Telephone Number
        /// </summary>
        public string TelNumber { get; set; }

        /// <summary>
        /// Email Address
        /// </summary>
        public string EmailAddress { get; set; }

        public string BTGiftWrapCode { get; set; }

        public string BTGiftWrapString { get; set; }

        //Fee
        public bool HasStoreShippingFee { get; set; }

        public bool HasStoreGiftWrapFee { get; set; }

        public bool HasStoreProccessingFee { get; set; }

        public bool HasStoreOrderFee { get; set; }

        //Shipping methods
        public string ShippingMethodExtID { get; set; }

        public string BTShippingMethodGuid { get; set; }

        public string BTCarrierCode { get; set; }

        public CartAccount CartAccount { get; set; }

        public CreditCard CreditCard { get; set; }



        public OrderForm()
        {
            CartAccount = new CartAccount();
            CreditCard = new CreditCard();
        }

        public string Name { get; set; }

        public bool IsBackOrder { get; set; }

        public string CostSummaryByIntAdmin { get; set; }

        public string CostSummaryByExtAdmin { get; set; }

        public int TransmisisonNumber { get; set; }
        public string TransmisisonNumberForUiSort
        {
            get { return TransmisisonNumber.ToString(); }
        }

        public DateTime LastUpdated { get; set; }
        public string LastUpdatedForUiSort
        {
            get { return LastUpdated.ToString("MM/dd/yyyy hh:mm tt"); }
        }

        public string CartName { get; set; }

        public string OrderType { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public string OrderStatusStringForUi { get; set; }

        public DateTime SubmittedDatetime { get; set; }
        public string SubmittedDatetimeForUi
        {
            get { return SubmittedDatetime.ToString("MM/dd/yyyy hh:mm tt"); }
        }

        public string PoNumber { get; set; }

        public string ErpAccount { get; set; }

        public string CartOwnersUserName { get; set; }

        public string CartOrganization { get; set; }

        public string CartId { get; set; }

        public string OrderNumber { get; set; }

        public string OrderFormId { get; set; }
        public string BasketOrderFormStateId { get; set; }

        public string CartStatus { get; set; }

        public string CartOwnerUserId { get; set; }

        public string CartOwnerOrgId { get; set; }
    }
}
