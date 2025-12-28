using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class EListSubcategory : CMListBase<EListSubcategoryItem>
    {
        public EListSubcategory() : this(string.Empty)
        {
            HasDisplayOrder = true;
            HasIsDefault = false;
        }
        public EListSubcategory(string site) : base(site) { }

        #region Overrides of SharePointListBase<NewFeedItem>

        protected override string GetListName()
        {
            return "eListSubcategory";
        }

        #endregion
        private string QueryCondition(string condition)
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
            else if (!string.IsNullOrEmpty(_camlGetByParentId))
            {
                if (string.IsNullOrEmpty(result))
                    result = _camlGetByParentId;
                else result = CMConstants.AND_TAG_OPEN
                       + _camlGetByParentId + result
                       + CMConstants.AND_TAG_CLOSE;
            }
            if (!string.IsNullOrEmpty(_camlGetById))
            {
                if (string.IsNullOrEmpty(result))
                    result = _camlGetById;
                else result = CMConstants.AND_TAG_OPEN
                       + _camlGetById + result
                       + CMConstants.AND_TAG_CLOSE;
            }
            return result;
        }

        private string _camlGetByParentId;
        private string _camlGetByParentIds;
        private int _eListCategoryID;
        public int EListCategoryID
        {
            get { return _eListCategoryID; }
            set
            {
                _eListCategoryID = value;
                _camlGetByParentId = ContentManagementHelper.CreateCamlGetByParentId("eListCategory", value.ToString());
            }
        }
        public List<int> EListCategoryIDs
        {
            set
            {
                foreach (var i in value)
                {
                    if (string.IsNullOrEmpty(_camlGetByParentIds))
                        _camlGetByParentIds = ContentManagementHelper.CreateCamlGetByParentId("eListCategory", i.ToString());
                    else
                    {
                        _camlGetByParentIds = CMConstants.OR_TAG_OPEN
                       + _camlGetByParentIds + ContentManagementHelper.CreateCamlGetByParentId("eListCategory", i.ToString())
                       + CMConstants.OR_TAG_CLOSE;
                    }
                }
            }
        }
        private string _camlGetById;
        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                _camlGetById = ContentManagementHelper.CreateCamlGetById(value.ToString());
            }
        }
        //protected override string GetViewFields()
        //{
        //    return "<ViewFields><FieldRef Name='Title' /><FieldRef Name='eListCategory' /></ViewFields>";
        //    //return CMConstants.DefaultFieldNames + "<FieldRef Name='eListCategory' /><FieldRef Name='eListCategory' />";
        //}
        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["eListCategory"]
                       ));
        }
        protected override string GetSortFields()
        {
            return "<FieldRef Name='DisplayOrder' Ascending='True' />";
        }
    }    
}
