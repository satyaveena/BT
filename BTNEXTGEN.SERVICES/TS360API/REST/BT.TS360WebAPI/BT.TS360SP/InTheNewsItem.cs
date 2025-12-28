using Microsoft.SharePoint.Client;
using ListItem = Microsoft.SharePoint.Client.ListItem;

namespace BT.TS360SP
{
    public class InTheNewsItem : SingleProductBaseItem
    {
        public string PromotionCode { get; set; }
        public string NewsTitle { get; set; }
        public string NewsImage { get; set; }
        public string NewsText { get; set; }
        public string NewsVideo { get; set; }
        public string WebTrendsTag { get; set; }
        public string NewsDocument { get; set; }

        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            AdName = item["AdName"] as string;
            this.BTKEY = item["NewsBTKEY"] as string;
            this.NewsTitle = item["NewsTitle"] as string;
            var temObject = item["NewsImage"] as FieldUrlValue;
            this.NewsImage = GetUrlFromFieldUrlValue(temObject == null ? "" : temObject.Url);
            this.NewsText = item["NewsText"] as string;
            temObject = item["NewsVideo"] as FieldUrlValue;
            this.NewsVideo = GetUrlFromFieldUrlValue(temObject == null ? "" : temObject.Url);
            this.PromotionCode = item["PromotionCode"] as string;
            this.WebTrendsTag = item["WebTrendsTag"] as string;
            temObject = item["News_x0020_Document"] as FieldUrlValue;
            NewsDocument = GetUrlFromFieldUrlValue(temObject == null ? "" : temObject.Url);
        }
    }
}
