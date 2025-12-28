using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public partial class PromotionItem : CMListItem
    {
        public string ItemCurrenceURL { get; set; }
        public string TargetPage { get; set; }
        //public string PromoEffectiveDate { get; set; }
        //public string PromoExpirationDate { get; set; }
        public string BtKeyList { get; set; }
    }

    public partial class PromotionItem : CMListItem
    {
        public string ImageUrl { get; set; }
        public string SummaryText { get; set; }
        public string DisplayOrder { get; set; }
        public string DetailText { get; set; }
        public string PromotionCode { get; set; }

        public string ImageWebtrendsTag { get; set; }
        public string ButtonWebtrendsTag { get; set; }

        //public override void SPListItemMapping(SPListItem item)
        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            AdName = item["AdName"] as string;
            
            //ImageUrl = GetUrlFromFieldUrlValue(item["ImageUrl"] as string);
            var temObject = item["ImageUrl"] as FieldUrlValue;
            this.ImageUrl = GetUrlFromFieldUrlValue(temObject == null ? "" : temObject.Url);

            SummaryText = item["SummaryText"] as string;
            DetailText = item["DetailText"] as string;
            DisplayOrder = item["DisplayOrder"] as string;
            PromotionCode = item["PromotionCode"] as string;

            this.ImageWebtrendsTag = item["ImageWebtrendsTag"] as string;
            this.ButtonWebtrendsTag = item["ButtonWebtrendsTag"] as string;
        }
    }
}
