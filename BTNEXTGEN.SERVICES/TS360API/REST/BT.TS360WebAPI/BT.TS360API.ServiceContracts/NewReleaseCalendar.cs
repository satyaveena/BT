using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class GetCalendarDataRequest : BaseRequest
    {
        public string UserId { get; set; }
        public string[] ProductTypesList { get; set; }
        public string CalendarView { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }

    public class GetNRCFeaturedProductsRequest : BaseRequest
    {
        public string[] ProductTypesList { get; set; }
        public string CalendarView { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public byte MaxItemsPerProductType { get; set; }
    }

    public class FeaturedProductsByBTKeysRequest : BaseRequest
    {
        public List<string> BTKeys { get; set; }
        public int BatchSize { get; set; }
    }

    public class GetCalendarDataResponse
    {
        public List<CalendarData> CalendarData { get; set; }
    }

    public class GetNRCFeaturedProductsResponse
    {
        //public FeaturedTitlesData FeaturedTitlesData { get; set; }
        public List<string> RemainingBTKeys { get; set; }
        public List<ProductItem> ProductItems { get; set; }
    }

    public class NRCList
    {
        public string ProductType { get; set; }
        public DateTime StreetDate { get; set; }
        public DateTime PreOrderDate { get; set; }
        public List<string> BTKeys { get; set; }
    }

    public class NRCListFilterView
    {
        public DateTime ActiveDate { get; set; }
        public string ProductType { get; set; }
        public List<string> BTKeys { get; set; }
    }

    public class CalendarData
    {
        public string ActiveDate { get; set; }
        public int TotalBooks { get; set; }
        public int TotalDigitals { get; set; }
        public int TotalMovies { get; set; }
        public int TotalMusics { get; set; }
    }

    public class ProductInfo
    {
        public string BTKey { get; set; }
        public string ProductType { get; set; }
        public DateTime PreOrderDate { get; set; }
    }

    public class ProductItem
    {
        public string BTKey { get; set; }
        public string ProductType { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Author { get; set; }
    }

    public class NRCProductInfoRequest
    {
        public List<string> BTKeys { get; set; }
    }

    public class NRCAltFormatsRequest
    {
        public string UserId { get; set; }
        public string BTKey { get; set; }
        public string PrimaryCartId { get; set; }
        public ProductAltFormatsRequestContext UserContext { get; set; }

        /// <summary>
        /// Max number of items to response.
        /// Zero means all.
        /// </summary>
        public int MaxItemNumber { get; set; }
        public List<string> RemainingBTKeys { get; set; }
    }

    public class NRCAltFormatsResponse
    {
        public List<NRCAltFormatItem> NRCAltFormats { get; set; }
        public List<string> RemainingBTKeys { get; set; }
    }

    public class NRCAltFormatItem
    {
        public string BTKey { get; set; }
        public string Upc { get; set; }
        public string ISBN { get; set; }
        public string ISBN10 { get; set; }
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public string ProductFormat { get; set; }
        public string ProductFormatForUI { get; set; }
        public string FormatIconType { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string PublishDateText { get; set; }
        public string LabelOrStudio { get; set; }
        public string Edition { get; set; }
        public decimal ListPrice { get; set; }
        public string ListPriceText { get; set; }
        public bool AllowAddToPrimaryCart { get; set; }
        public int? Quantity { get; set; }

        // properties for pricing
        public decimal AcceptableDiscount { get; set; }
        public string GTIN { get; set; }
        public string Catalog { get; set; }
        public bool HasReturn { get; set; }
        public string PriceKey { get; set; }
        public string ProductLine { get; set; }
        public string ESupplier { get; set; }
        public string PurchaseOption { get; set; }
    }

    public class PricingForProductsRequest
    {
        public string UserId { get; set; }
        public List<PricingClientArg> Products { get; set; }
        public string[] ESuppliers { get; set; }
        public bool IsHideNetPriceDiscountPercentage { get; set; }
        public bool ShowExpectedDiscountPriceForSearch { get; set; }
        public TargetingValues Targeting { get; set; }
        public AccountInfoForPricing AccountPricingData { get; set; }
    }

    public class PricingForProductsResponse
    {
        public List<PricingReturn4ClientObject> ProductPrices { get; set; }
    }

    public class NRCProductType
    {
        public string ID { get; set; }
        public bool ActiveIndicator { get; set; }
        public string Name { get; set; } 
    }
}
