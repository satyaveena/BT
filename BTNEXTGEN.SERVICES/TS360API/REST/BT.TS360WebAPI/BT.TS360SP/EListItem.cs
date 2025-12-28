using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360SP
{
    public class EListItem : MultipleProductsBaseItem
    {
        public string Description { get; set; }
        public int? EListSubcategoryID { get; set; }
        public string EListSubcategoryValue { get; set; }
        public string DisplayOrder { get; set; }
        
        //public override void SPListItemMapping(SPListItem item)
        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            Description = item["Description"] as string;
            BTKeyList = item["BTKeyList"] as string;
            DisplayOrder = item["DisplayOrder"] as string;

            //var tmp = new SPFieldLookupValue(item["eListSubcategory"] as string);
            var tmp = item["eListSubcategory"] as FieldLookupValue;
            EListSubcategoryID = tmp.LookupId == 0 ? null : (int?)tmp.LookupId;
            EListSubcategoryValue = tmp.LookupValue;
        }

        public string ItemCurrenceURL { get; set; }
    }

    public class ShortEListItem1 : ICMListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ItemCurrenceURL { get; set; }
        public void Initialize(ListItem item)
        {
            SPListItemMapping(item);
        }
        public string GetAdName()
        {
            return string.Empty;
        }

        public virtual void SPListItemMapping(ListItem item)
        {
            Id = item.Id;
            Title = item["Title"] as string;
        }
    }
}
