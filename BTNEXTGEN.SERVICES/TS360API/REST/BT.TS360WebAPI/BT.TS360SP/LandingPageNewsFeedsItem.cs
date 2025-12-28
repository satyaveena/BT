using ListItem = Microsoft.SharePoint.Client.ListItem;

namespace BT.TS360SP
{
    public class LandingPageNewsFeedsItem : CMListItem
    {
        public string NewsFeed { get; set; }

        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            AdName = item["AdName"] as string;
            this.NewsFeed = item["NewsFeed"] as string;
        }
    }
}
