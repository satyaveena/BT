using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class ListItemDetailFieldItem : CMListItem
    {
        //[Microsoft.SharePoint.Linq.ColumnAttribute(Name = "FieldKey", Storage = "_fieldKey", FieldType = "Text")]
        public string FieldKey { get; set; }

        //[Microsoft.SharePoint.Linq.AssociationAttribute(Name = "Section", Storage = "_section", MultivalueType = Microsoft.SharePoint.Linq.AssociationType.Single, List = "ListItemDetailSection")]
        public int? SectionID { get; set; }

        public string SectionValue { get; set; }

        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            this.FieldKey = item["FieldKey"] as string;

            var tmp = item["Section"] as FieldLookupValue;
            SectionID = tmp.LookupId == 0 ? null : (int?)tmp.LookupId;
            SectionValue = tmp.LookupValue;
        }
    }
}
