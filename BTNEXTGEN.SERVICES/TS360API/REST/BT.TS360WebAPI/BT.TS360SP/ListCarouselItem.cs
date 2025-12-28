using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public partial class ListCarouselItem : CMListItem
    {
        //additional properties
        public string FolderName { get; set; }
    }

    /// <summary>
    /// Managed Content for BTNG Content Management
    /// </summary>
    public partial class ListCarouselItem : CMListItem
    {
        public string PromotionName { get; set; }
        public string PromotionDescription { get; set; }
        public string PromotionLinkImage { get; set; }
        public string PromoBoxBackgroundImage { get; set; }
        public string ListCarouselURL { get; set; }
        public PromotionFolder? PromotionFolder { get; set; }
        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            AdName = item["AdName"] as string;
            this.PromotionName = item["PromotionName"] as string;
            this.PromotionDescription = item["PromotionDescription"] as string;
            var temObject = item["PromotionLinkImage"] as FieldUrlValue;
            this.PromotionLinkImage = GetUrlFromFieldUrlValue(temObject == null ? "" : temObject.Url);
            temObject = item["PromoBoxBackground"] as FieldUrlValue;
            this.PromoBoxBackgroundImage = GetUrlFromFieldUrlValue(temObject == null ? "" : temObject.Url);
            temObject = item["ListCarouselUrl"] as FieldUrlValue;
            this.ListCarouselURL = GetUrlFromFieldUrlValue(temObject == null ? "" : temObject.Url);
            this.PromotionFolder = ToPromotionFolder(item["PromotionFolder"] as string);
        }
    }

    public enum PromotionFolder : int
    {
        None = 0,
        Invalid = 1,
        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Promotion 1")]
        Promotion1 = 2,
        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Promotion 2")]
        Promotion2 = 4,
        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Promotion 3")]
        Promotion3 = 8,
        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Promotion 4")]
        Promotion4 = 16,
        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Promotion 5")]
        Promotion5 = 32,
        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Promotion 6")]
        Promotion6 = 64,
        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Promotion 7")]
        Promotion7 = 128,
        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Promotion 8")]
        Promotion8 = 256,
        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Promotion 9")]
        Promotion9 = 512,
        //[Microsoft.SharePoint.Linq.ChoiceAttribute(Value = "Promotion 10")]
        Promotion10 = 1024,
    }
}
