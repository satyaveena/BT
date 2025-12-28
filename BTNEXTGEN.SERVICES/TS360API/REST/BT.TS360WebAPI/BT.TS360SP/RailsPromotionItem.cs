using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class RailsPromotionItem : CMListItem
    {
        public string SalesTitle { get; set; }

        public string SalesText { get; set; }

        public string SalesPromoCode { get; set; }

        public string PlacementType { get; set; }

        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            AdName = item["AdName"] as string;
            this.SalesTitle = item["SalesTitle"] as string;
            this.SalesText = item["SalesText"] as string;
            this.SalesPromoCode = item["SalesPromoCode"] as string;
            this.PlacementType = item["PlacementType"] as string;
        }
    }
}
