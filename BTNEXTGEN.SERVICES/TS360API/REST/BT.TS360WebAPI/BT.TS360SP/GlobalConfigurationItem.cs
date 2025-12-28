using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
  
    public class GlobalConfigurationItem : CMListItem
    {
        public string Value { get; set; }

        public override void SPListItemMapping(ListItem item)
        {
            Title = item["Title"] as string;
            Value = item["Value"] as string;
            //base.SPListItemMapping(item);
        }
    }
}
