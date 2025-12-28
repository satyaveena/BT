using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class HeaderTitlesItem : CMListItem
    {
        public string ComingSoonCarousel { get; set; }
        public string WhatsHot { get; set; }
        public string InTheNews { get; set; }
        public string PopularBTeLists { get; set; }
        public string NewReleaseViewAllWTTag { get; set; }

        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            AdName = item["AdName"] as string;
            this.ComingSoonCarousel = item["ComingSoonCarousel"] as string;
            this.WhatsHot = item["WhatsHot"] as string;
            this.InTheNews = item["InTheNews"] as string;
            this.PopularBTeLists = item["PopularBTeLists"] as string;
            this.NewReleaseViewAllWTTag = item["NewReleaseViewAllWTTag"] as string;
        }
    }
}
