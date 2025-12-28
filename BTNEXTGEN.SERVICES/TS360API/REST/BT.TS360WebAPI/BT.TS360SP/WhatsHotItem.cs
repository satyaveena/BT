using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class WhatsHotItem : SingleProductBaseItem
    {
        public string PromotionCode { get; set; }

        public string PromotionTitle { get; set; }

        public string PromotionText { get; set; }

        public string WhatSHotImage { get; set; }

        public PromotionFolder? PromotionFolder { get; set; }

        public string ImageWebtrendsTag { get; set; }

        public string ButtonWebtrendsTag { get; set; }

        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            AdName = item["AdName"] as string;
            this.PromotionTitle = item["PromotionTitle"] as string;
            this.PromotionText = item["PromotionText"] as string;
            this.BTKEY = item["WhatHotBTKEY"] as string;
            var temObject = item["WhatHotImage"] as FieldUrlValue;
            this.WhatSHotImage = GetUrlFromFieldUrlValue(temObject == null ? "" : temObject.Url);

            this.PromotionFolder = ToPromotionFolder(item["PromotionFolder"] as string);
            this.PromotionCode = item["PromotionCode"] as string;

            this.ImageWebtrendsTag = item["ImageWebtrendsTag"] as string;
            this.ButtonWebtrendsTag = item["ButtonWebtrendsTag"] as string;
        }
    }
}
