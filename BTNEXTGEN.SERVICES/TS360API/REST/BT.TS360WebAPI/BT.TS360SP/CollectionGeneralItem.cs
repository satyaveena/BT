using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class CollectionGeneralItem : CMListItem
    {
        public string CollectionTitle { get; set; }

        public string WebTrendsTag { get; set; }

        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            this.AdName = item["AdName"] as string;
            this.CollectionTitle = item["CollectionTitle"] as string;
            this.WebTrendsTag = item["WebTrendsTag"] as string;
        }
    }
}
