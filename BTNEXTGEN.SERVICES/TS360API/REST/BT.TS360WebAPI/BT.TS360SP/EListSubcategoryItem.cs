using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360SP
{
    public class EListSubcategoryItem : CMListItem
    {
        public int? EListCategoryID { get; set; }
        //public string EListCategoryValue { get; set; }
        //public string DisplayOrder { get; set; }

        //public override void SPListItemMapping(SPListItem item)
        public override void SPListItemMapping(ListItem item)
        {
            base.SPListItemMapping(item);
            //DisplayOrder = item["DisplayOrder"] as string;
            //var tmp = new SPFieldLookupValue(item["eListCategory"] as string);
            var tmp = item["eListCategory"] as FieldLookupValue;
            EListCategoryID = tmp.LookupId == 0 ? null : (int?)tmp.LookupId;
            //EListCategoryValue = tmp.LookupValue;
        }

        public IList<EListItem> EListItems { get; set; }
    }
}
