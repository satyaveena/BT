using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class ShortEListCategoryItem : CMListItem
    {
        public int Id { get; set; }

        public virtual void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            AdName = item["AdName"] as string;
        }
    }
}
