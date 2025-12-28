using System;
using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public class WCFObjectReturnToClient
    {
        public List<PricingReturn4ClientObject> Pricing { get; set; }
        
        public List<SiteTermObject> InventoryStatus { get; set; }
        
        public List<PromotionReturn4ClientObject> Promotion { get; set; }
        
        public List<SiteTermObject> Duplicate { get; set; }
        
        public List<SiteTermObject> ContentIndicator { get; set; }

        public List<PrimaryCartTitleDetail> PrimaryCartTitleDetails { get; set; }
        
        public string ContentIndicatorHtml { get; set; }
        
        public string Message { get; set; }
        
        public List<InventoryResults> InventoryResultsList { get; set; }
        
        public List<NoteClientObject> NotesList { get; set; }
        
        public string StockCheckInventoryStatus { get; set; }
        
        public bool HasPermissionWF { get; set; }
        
        public string ProductDetailsUrl { get; set; }
        
        public bool HasPawPrint { get; set; }
        
        public bool HasGardners { get; set; }
        
        public string ProductLookupLink { get; set; }
        
        public string ProductLookupLink13 { get; set; }
        
        public bool HasCPSIAWarning { get; set; }
        
        public bool ProductLookupLinkHasOnClick { get; set; }
        
        public bool ProductLookupLink13HasOnClick { get; set; }
        
        public bool HasLargePrint { get; set; }

        public string UPCProductLookupLink { get; set; }

        public WCFObjectReturnToClient()
        {
            Pricing = new List<PricingReturn4ClientObject>();
            InventoryStatus = new List<SiteTermObject>();
            Promotion = new List<PromotionReturn4ClientObject>();
            Duplicate = new List<SiteTermObject>();
            ContentIndicator = new List<SiteTermObject>();
            InventoryResultsList = new List<InventoryResults>();
            NotesList = new List<NoteClientObject>();
            HasPermissionWF = false;
            ProductLookupLink = "";
            ProductLookupLink13 = "";
            ProductLookupLinkHasOnClick = false;
            ProductLookupLink13HasOnClick = false;
            HasLargePrint = false;
            UPCProductLookupLink = "";
        }
    }

    public class WCFObjectReturnToClientDupCheck
    {
        
        public string Message { get; set; }

        
        public List<SiteTermObject> Duplicate { get; set; }

        public WCFObjectReturnToClientDupCheck()
        {
            Duplicate = new List<SiteTermObject>();
        }
    }

    public class SearchResultInventoryStatusArg
    {
        public string BTKey;
        public string BTUPC;
        public string BTGTIN;
        public string Catalog;
        public string ProductLine;
        public string HasReturn;
        public string VariantId;
        public string CatalogName;
        public decimal Quantity;
        public string ProductType;
        public string Flag;
        public string UserId;
        public DateTime PublishDate;
        public string MerchandiseCategory;
        public string MarketType;
        public string PubCodeD;
        public string ESupplier;
        public string ReportCode;
        public string SupplierCode;

        public bool ForSingleItem = false;
        public string[] BlockedExportCountryCodes;
    }

    public class PricingClientArg
    {
        public string ISBN { get; set; }

        public string BTKey { get; set; }
        
        public string PriceKey { get; set; }
        
        public string ProductType { get; set; }
        
        public string ListPrice { get; set; }
        
        public string Quantity { get; set; }
        
        public string Catalog { get; set; }
        
        public string BTGTIN { get; set; }
        
        public string BTUPC { get; set; }
        
        public string ProductLine { get; set; }
        
        public string HasReturn { get; set; }
        
        public decimal AcceptableDiscount { get; set; }
        
        public string ProductFormat { get; set; }
        
        public string ESupplier { get; set; }
        
        public string PurchaseOption { get; set; }
        
        public string ToUpdateListPrice { get; set; }
    }

    public class PromotionClientArg
    {
        public string PIGName { get; set; }
        public string BTKey { get; set; }
        public string ProductType { get; set; }
        public string Catalog { get; set; }
        public string SiteBranding { get; set; }
        public string MarketType { get; set; }
        public string[] AudienceType { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public string[] SiteContextProductType { get; set; }

        public PromotionClientArg()
        { }
    }

    public class PrimaryCartTitleDetail
    {
        public bool IsInPrimaryCart { get; set; }
        public int PrimaryQuantity { get; set; }
        public string BTKey { get; set; }
        public string LineItemId { get; set; }
    }
}
