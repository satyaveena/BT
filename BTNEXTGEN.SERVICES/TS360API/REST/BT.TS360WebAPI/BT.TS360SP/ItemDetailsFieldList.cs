using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class ItemDetailsFieldList : CMListBase<ListItemDetailFieldItem>
    {
         protected override string GetListName()
        {
            return CMListName.ItemDetailsFieldList; // "Item Detail Field";
        }

        public ItemDetailsFieldList():this("Home")
        {
            HasIsDefault = false;
            HasDisplayOrder = true;
        }
        public ItemDetailsFieldList(string site) : base(site) { }
        //protected override string GetViewFields()
        //{
        //    return "<ViewFields><FieldRef Name='Title' /><FieldRef Name='FieldKey' /><FieldRef Name='Section' /></ViewFields>";
        //}
        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["FieldKey"],
                       item => item["Section"]
                       ));
        }
        protected override string GetSortFields()
        {
            return "<FieldRef Name='DisplayOrder' Ascending='True' />";
        }
        protected override string AddWhereCamlTo(string currentCaml)
        {
            string result = string.Empty;
            foreach (var sectionId in SectionIds)
            {
                string sectionEq = string.Format("{0}<FieldRef Name='{1}' LookupId='true'/><Value Type='Lookup'>{2}</Value>{3}", CMConstants.EQ_TAG_OPEN, "Section", sectionId, CMConstants.EQ_TAG_CLOSE);
                if (string.IsNullOrEmpty(result))
                    result = sectionEq;
                else
                    result = CMConstants.OR_TAG_OPEN + sectionEq + result + CMConstants.OR_TAG_CLOSE;
            }
            if (!string.IsNullOrEmpty(currentCaml))
                result = CMConstants.AND_TAG_OPEN + currentCaml + result + CMConstants.AND_TAG_CLOSE;
            //result = result + "</Where><OrderBy><FieldRef Name='DisplayOrder' Ascending='True' /></OrderBy><GroupBy><FieldRef Name='Section' LookupId='true'/></GroupBy>";
            return result;
        }
       
        public int[] SectionIds { get; set; }
    }
}
