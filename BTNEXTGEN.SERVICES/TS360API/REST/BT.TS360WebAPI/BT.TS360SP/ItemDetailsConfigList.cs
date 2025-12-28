using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class ItemDetailsConfigListVisible : CMListBase<ListItemDetailConfigurationItem>
    {
        protected override string GetListName()
        {
            return CMListName.ItemDetailsConfigList; // "Item Detail Configuration";
        }

        public ItemDetailsConfigListVisible():this("Home")
        {
            HasIsDefault = false;
        }
        public ItemDetailsConfigListVisible(string site) : base(site) { }
        //protected override string GetViewFields()
        //{
        //    return "<ViewFields><FieldRef Name='Title' /><FieldRef Name='Mode' /><FieldRef Name='Section' /></ViewFields>";
        //}
        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["Mode"],
                       item => item["Section"]
                       ));
        }
        
        protected override string AddWhereCamlTo(string currentCaml)
        {
            string result = string.Empty;
            string itemModeEq = string.Format("{0}<FieldRef Name='{1}' /><Value Type='Choice'>{2}</Value>{3}", CMConstants.EQ_TAG_OPEN, "Mode", "Turn On", CMConstants.EQ_TAG_CLOSE);

            if (string.IsNullOrEmpty(currentCaml))
                result = itemModeEq;
            else
                result = CMConstants.AND_TAG_OPEN + currentCaml + itemModeEq + CMConstants.AND_TAG_CLOSE;
            return result;
            
        }
      
    }
}
