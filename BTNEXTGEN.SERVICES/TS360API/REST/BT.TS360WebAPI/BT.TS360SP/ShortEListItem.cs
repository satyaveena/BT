using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class ShortEListItem : ICMListItem
    {
        public int Id { get; set; }
        public int? EListSubcategoryID { get; set; }
        
        //public void Initialize(SPListItem item)
        public void Initialize(ListItem item)
        {
            SPListItemMapping(item);
        }
        public string GetAdName()
        {
            return string.Empty;
        }

        //public virtual void SPListItemMapping(SPListItem item)
        public virtual void SPListItemMapping(ListItem item)
        {
            //var tmp = new SPFieldLookupValue(item["eListSubcategory"] as string);
            var tmp = item["eListSubcategory"] as FieldLookupValue;

            EListSubcategoryID = tmp.LookupId == 0 ? null : (int?)tmp.LookupId;
        }
    }
}
