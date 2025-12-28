using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
  
    public class GlobalConfigurationList : CMListBase<GlobalConfigurationItem>
    {
        protected override string GetListName()
        {
            return "GlobalConfiguration";
        }

        public GlobalConfigurationList()
            : this("Home")
        {
            HasIsDefault = false;
        }
        public GlobalConfigurationList(string site) : base(site) { }
        protected override string GetViewFields()
        {
            return "<ViewFields><FieldRef Name='Title' /><FieldRef Name='Value' /></ViewFields>";
        }
        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["Value"]
                       ));
        }
    }
}
