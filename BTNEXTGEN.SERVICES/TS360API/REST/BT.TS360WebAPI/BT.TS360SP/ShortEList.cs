using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class ShortEList : CMListBase<ShortEListItem>
    {
        public ShortEList()
            : this(string.Empty)
        {
            HasIsDefault = false;
            RowLimit = 2000;
        }
        public ShortEList(string site) : base(site) { }

        protected override string GetListName()
        {
            return "eList";
        }

        protected override string GetViewFields()
        {
            return "<ViewFields><FieldRef Name='Title' /><FieldRef Name='eListSubcategory' /></ViewFields>";
        }
        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["eListSubcategory"]
                       ));
        }
        protected override string GetSortFields()
        {
            return "<FieldRef Name='DisplayOrder' Ascending='True' />";
        }
        //protected override string QueryCondition(string condition)
        private string QueryCondition(string condition)
        {
            //string result = base.QueryCondition(condition);
            string startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.IsNull_TAG_CLOSE);
            string startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            string startDateOr = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;

            //result = CMConstants.AND_TAG_OPEN + result + startDateOr + CMConstants.AND_TAG_CLOSE;
            //return result;
            
            if (string.IsNullOrEmpty(condition))
                return startDateOr;
            return CMConstants.AND_TAG_OPEN + condition + startDateOr + CMConstants.AND_TAG_CLOSE;
        }
        protected override string AddWhereCamlTo(string currentCaml)
        {
            //string result = currentCaml;
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
