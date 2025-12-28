using BT.TS360API.ServiceContracts.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class AdditionalCartLineItemsResponse
    {
        public AdditionalCartLineItemsResponse()
        {
            Pricing = new List<PricingReturn4ClientObject>();
            Promotion = new List<PromotionReturn4ClientObject>();
            //Duplicate = new List<SiteTermObject>();
            ContentIndicator = new List<SiteTermObject>();
            InventoryResultsList = new List<InventoryResults>();
            InventoryStatus = new List<SiteTermObject>();
            NotesList = new List<NoteClientObject>();
            HasPermissionWF = false;
        }

        public List<InventoryResults> InventoryResultsList { get; set; }
        public List<SiteTermObject> InventoryStatus { get; set; }
        public List<SiteTermObject> ContentIndicator { get; set; }
        //public List<SiteTermObject> Duplicate { get; set; }
        public List<PricingReturn4ClientObject> Pricing { get; set; }
        public List<PromotionReturn4ClientObject> Promotion { get; set; }
        public List<NoteClientObject> NotesList { get; set; }
        public bool HasPermissionWF { get; set; }
        public string StockCheckInventoryStatus { get; set; }
    }

    public class EnhancedContentsForCartDetailsResponse
    {
        public EnhancedContentsForCartDetailsResponse()
        {
            Pricing = new List<PricingReturn4ClientObject>();
            Promotion = new List<PromotionReturn4ClientObject>();
            //Duplicate = new List<SiteTermObject>();
            ContentIndicator = new List<SiteTermObject>();
            InventoryResultsList = new List<InventoryResults>();
            InventoryStatus = new List<SiteTermObject>();
            NotesList = new List<NoteClientObject>();
        }

        public List<InventoryResults> InventoryResultsList { get; set; }
        public List<SiteTermObject> InventoryStatus { get; set; }
        public List<SiteTermObject> ContentIndicator { get; set; }
        public bool HasPawPrint { get; set; }
        public bool HasGardners { get; set; }
        public bool HasLargePrint { get; set; }
        public bool ProductLookupLink13HasOnClick { get; set; }
        public bool ProductLookupLinkHasOnClick { get; set; }
        public string ProductLookupLink13 { get; set; }
        public string ProductLookupLink { get; set; }
        public string UPCProductLookupLink { get; set; }
        //public List<SiteTermObject> Duplicate { get; set; }
        public List<PricingReturn4ClientObject> Pricing { get; set; }
        public List<PromotionReturn4ClientObject> Promotion { get; set; }
        public List<NoteClientObject> NotesList { get; set; }

    }

    public class AllInfoForQuickItemDetailsResponse
    {
        //public NoteClientObject Note { get; set; }
        public string InventoryStatus { get; set; }
        public CartItemDetailsNavBarInfo NavBarInfo { get; set; }
    }

    public class ProductAltFormatsResponse
    {
        public List<ProductAltFormatItem> ProductSearchItems { get; set; }
        public List<string> RemainingBTKeys { get; set; }
    }

    public class ProductAltFormatItem
    {
        public string BTKey { get; set; }
        public string BTEKey { get; set; }
        public string ISBN { get; set; }
        public string ISBN10 { get; set; }
        public ISBNLookUpLink ISBNLookUpLink { get; set; }
        public string UPCLookUpLink { get; set; }
        public string ImageUrl { get; set; }
        public string ProductFormat { get; set; }
        //public string FormatIconPath { get; set; }
        public string FormatIconType { get; set; }
        public string IncludedFormatClass { get; set; }
        public string ESupplier { get; set; }
        public string FormDetails { get; set; }
        public string Upc { get; set; }
        public string Title { get; set; }
        public string CPSIAMessage { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string PublishDateText { get; set; }
        public string LabelOrStudio { get; set; }
        public string StreetDate { get; set; }
        public string PreOrderDate { get; set; }
        public string LastUpdated { get; set; }
        public string Edition { get; set; }
        public string ReportCode { get; set; }
        public string LanguageLiteral { get; set; }
        public string LanguageLiteralForUI { get; set; }
        public decimal ListPrice { get; set; }
        public decimal AcceptableDiscount { get; set; }
        public string MerchCategory { get; set; }
        public string SupplierCode { get; set; }
        public string ListPriceText { get; set; }
        public string PurchaseOption { get; set; }
        public string ProductType { get; set; }
        public string GTIN { get; set; }
        public string Catalog { get; set; }
        public string PriceKey { get; set; }
        public bool HasReview { get; set; }
        public bool HasReturn { get; set; }
        public string ProductLine { get; set; }
        public string BlockedExportCountryCodes { get; set; }

        public bool AllowAddToPrimaryCart { get; set; }
        public bool IsFollettbound { get; set; }
        public int? Quantity { get; set; }
        public bool IsPPCDup { get; set; }
    }

    public class InventoryForAltFormatsResponse
    {
        public List<InventoryResults> InventoryResultsList { get; set; }
        public List<SiteTermObject> InventoryStatus { get; set; }
    }

    public class UserEditableFieldsForAltFormatsResponse
    {
        public List<NoteClientObject> NotesList { get; set; }
    }

    public class CartItemDetailsNavBarInfo
    {
        public string CartName { get; set; }
        public int LineItemCount { get; set; }
        public int TotalOrderQuantity { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsPricing { get; set; }
        public string CartPrice { get; set; }

        public int TitleIndex { get; set; }
        public GoToCartLineItem PreviousLineItem { get; set; }
        public GoToCartLineItem NextLineItem { get; set; }
        //public string PreviousBTKey { get; set; }
        //public string PreviousLineItemID { get; set; }
        //public string NextBTKey { get; set; }
        //public string NextLineItemID { get; set; }
        //public int PageNumberForNext { get; set; }
        //public int PageNumberForPrevious { get; set; }
    }

    public class SearchItemDetailsNavBarInfoResponse
    {
        public int TitleTotal { get; set; }
        public int TitleIndex { get; set; }
        public string PreviousBTKey { get; set; }
        public string NextBTKey { get; set; }

        public int PageForPreviousBTKey { get; set; }
        public int PageForNextBTKey { get; set; }
    }

    public class ActiveNewestBasketsResponse
    {
        public List<ActiveNewestBasket> Baskets { get; set; }
    }

    public class ActiveNewestBasket
    {
        public string BasketId { get; set; }
        public string BasketName { get; set; }
    }

    public class GoToProductItem
    {
        public string BTKey { get; set; }
        public int PageNumber { get; set; }
    }

    public class GoToCartLineItem : GoToProductItem
    {
        public string LineItemID { get; set; }
        public string BasketOriginalEntryID { get; set; }
    }

    public class PricingAndPromoForAltFormatsResponse
    {
        public List<PricingReturn4ClientObject> Pricing { get; set; }
        public List<PromotionReturn4ClientObject> Promotion { get; set; }
    }

    public class EnhancedContentIconsForAltFormatsResponse
    {
        public List<SiteTermObject> ContentIndicator { get; set; }
    }
}
