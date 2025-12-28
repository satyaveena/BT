using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    
     class ShortEList1 : CMListBase<ShortEListItem1>
    {
        public ShortEList1()
        {
            HasDisplayOrder = true;//sort by ID DESC
            HasIsDefault = false;
            RowLimit = 5;
        }

        protected override string GetListName()
        {
            return "eList";
        }

        //protected override string GetViewFields()
        //{
        //    return "<FieldRef Name='ID' /><FieldRef Name='Title' />";
        //}
         protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"]
                       ));
        }
        protected override string GetSortFields()
        {
            return "<FieldRef Name='ID' Ascending='FALSE' />";
        }
        protected  string QueryCondition(string condition)
        {
            string startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.IsNull_TAG_CLOSE);
            string startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            string startDateOr = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;
             if (string.IsNullOrEmpty(condition))
                return startDateOr;
            return CMConstants.AND_TAG_OPEN + condition + startDateOr + CMConstants.AND_TAG_CLOSE;
        }
        protected override string AddWhereCamlTo(string currentCaml)
        {
            string result = QueryCondition(currentCaml);
            if (!string.IsNullOrEmpty(_camlGetByParentIds))
            {
                if (string.IsNullOrEmpty(result))
                    result = _camlGetByParentIds;
                else result = CMConstants.AND_TAG_OPEN
                       + _camlGetByParentIds + result
                       + CMConstants.AND_TAG_CLOSE;
            }

            return result;
        }
        protected override void Refine(IList<ShortEListItem1> items)
        {
            foreach (var eListItem in items)
            {
                //Fix TFS #1151
                eListItem.ItemCurrenceURL = String.Format("{0}?{1}={2}", SiteUrl.EListProducts, SearchFieldNameConstants.elistid, eListItem.Id);
            }
        }
        private string _camlGetByParentIds;

        public List<int> EListSubcategoryIDs
        {
            set
            {
                if (value == null || value.Count == 0) return;

                _camlGetByParentIds += "<In>";
                _camlGetByParentIds += "<FieldRef Name='eListSubcategory' LookupId='true'/>";
                _camlGetByParentIds += "<Values>";
                foreach (var i in value)
                {
                    _camlGetByParentIds += "<Value Type='Integer'>" + i + "</Value>";
                }
                _camlGetByParentIds += "</Values>";
                _camlGetByParentIds += "</In>";
            }
        }
    }
}
