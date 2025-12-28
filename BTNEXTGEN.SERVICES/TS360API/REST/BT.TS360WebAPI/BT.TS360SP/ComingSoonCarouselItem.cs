using BT.TS360API.ServiceContracts.Search;
using Microsoft.SharePoint.Client;
using ListItem = Microsoft.SharePoint.Client.ListItem;

namespace BT.TS360SP
{
    public partial class ComingSoonCarouselItem : SingleProductBaseItem
    {
        public string NewPrice { get; set; }
        public string OldPrice { get; set; }
        public string Format { get; set; }
        public string PercentDiscount { get; set; }
        public string StockIndicator { get; set; }
        public string FormatClass { get; set; }
        public string FormatIconClass { get; set; }
        public string FormatIconPath { get; set; }
        public string IncludedFormatClass { get; set; }
        public string BTProductType { get; set; }
        public string ISBN { get; set; }
        public string ISBN10 { get; set; }
        public string Promotion { get; set; }
        public string DiscountText { get; set; }
        public string ProductFormat { get; set; }
        public string Upc { get; set; }
        public string GTIN { get; set; }
        public string Catalog { get; set; }
        public string ProductLine { get; set; }
        public decimal AcceptableDiscount { get; set; }
        public string BTEKey { get; set; }
        public bool HasAnnotations { get; set; }
        public bool HasExcerpts { get; set; }
        public bool HasMuze { get; set; }
        public bool HasReviews { get; set; }
        public bool HasTOC { get; set; }
        public bool HasReturn { get; set; }
        public string ReportCode { get; set; }
        public ProductSearchResultItem ProductSearchResultItem { get; set; }
        public string Quantity { get; set; }
        public ISBNLookUpLink ISBNLookUpLink { get; set; }
        public string ContentIndicator { get; set; }
        public string ESupplier { get; set; }
        public string FormDetails { get; set; }
        public string PurchaseOption { get; set; }
        public string PriceKey { get; set; }
        public string IsEBook { get { return string.IsNullOrEmpty(ESupplier) ? "false" : "true"; } }
    }

    /// <summary>
    /// Managed Content for BTNG Content Management
    /// </summary>
    public partial class ComingSoonCarouselItem : SingleProductBaseItem
    {
        public string PromotionCode { get; set; }

        public string ComingSoonImage { get; set; }

        public string WebTrendsTag { get; set; }

        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            AdName = item["AdName"] as string;
            this.BTKEY = item["ComingSoonBTKEY"] as string;
            var temObject = item["ComingSoonImage"] as FieldUrlValue;
            this.ComingSoonImage = GetUrlFromFieldUrlValue(temObject == null ? "" : temObject.Url);
            this.WebTrendsTag = item["WebTrendsTag"] as string;
        }
    }
}
