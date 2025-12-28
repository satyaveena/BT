using System;
using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public class QuickCart
    {
        public string CartId { get; set; }
        public string UserId { get; set; }
        public QuickCartInfo CartInfo { get; set; }
        public List<QuickLineItem> LineItems { get; set; }
        public int NonRankedCount { get; set; }

        public QuickCart(string cartId, string userId)
        {
            CartId = cartId;
            UserId = userId;
        }

    }
    public class QuickCartInfo
    {
        public string CartStatus { get; set; }
        public string CartOwnerID { get; set; }
        public string CartName { get; set; }
        public string CartOwnerName { get; set; }
        public int TotalLines { get; set; }
        public long TotalQuantity { get; set; }
        public decimal TotalListPrice { get; set; }
        public decimal TotalNetPrice { get; set; }
        public bool IsShared { get; set; }
        public bool IsPricingComplete { get; set; }
        public int ESPStateTypeId { get; set; }
        public bool OneClickMARCIndicator { get; set; }
        public string FTPErrorMessage { get; set; }
        public int ESPFundStateTypeID { get; set; }
        public int ESPDistStateTypeID { get; set; }
        public int ESPRankStateTypeId { get; set; }
        public string LastESPStateTypeLiteral { get; set; }
        public int FreezeLevel { get; set; }
        public bool IsMixedProduct { get; set; }
        public bool IsBasketActive { get; set; }
        public string CartFolderId { get; set; }
        public bool IsArchived { get; set; }
    }
    public class QuickLineItem
    {
        private string _btKey;
        private bool _hasJacket = true;
        //From database
        public string Title { get; set; }
        public string Author { get; set; }
        public string LineItemID { get; set; }
        public string Publisher { get; set; }
        public Decimal ListPrice { get; set; }
        public Decimal NetPrice { get; set; }
        public bool IsGridded { get; set; }
        public int Quantity { get; set; }
        public Decimal Discount { get; set; }
        public string BasketOriginalEntryID { get; set; }

        public string BTKey
        {
            get
            {
                if (!string.IsNullOrEmpty(_btKey)) return _btKey;
                return BasketOriginalEntryID;
            }
            set { _btKey = value; }
        }

        public bool HasJacket
        {
            get { return _hasJacket; }
            set { _hasJacket = value; }
        }

        public bool IsOEItem
        {
            get { return !string.IsNullOrEmpty(BasketOriginalEntryID); }
        }

        //From FAST
        public string ISBN { get; set; }
        public string UPC { get; set; }
        public string Format { get; set; }
        public string ESupplier { get; set; }
        public string ProductType { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string PurchaseOption { get; set; }
        public bool HasFamilyKey { get; set; }
        public string Catalog { get; set; }

        public bool HasCpsiaWarning { get; set; }
        public bool HasAnnotations { get; set; }
        public bool HasExcerpt { get; set; }
        public bool HasReturn { get; set; }
        public bool HasMuze { get; set; }
        public bool HasReview { get; set; }
        public bool HasToc { get; set; }

        public string MerchCategory { get; set; }
        public string Edition { get; set; }
        public string ProductLine { get; set; }
        public string SupplierCode { get; set; }
        public string IncludedFormatClass { get; set; }
        public int NumOfDiscs { get; set; }
    }
}
