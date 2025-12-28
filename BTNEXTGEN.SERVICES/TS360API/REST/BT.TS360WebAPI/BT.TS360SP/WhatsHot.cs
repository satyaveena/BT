using System.Collections.Generic;
using System.Linq;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Search;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class WhatsHot : CMListBase<WhatsHotItem>
    {
        string RequestDomainName { get; set; }
        public WhatsHot(string requestDomainName)
        {
            RequestDomainName = requestDomainName;
            HasIsDefault = true;
            HasAdName = true;
        }
        protected override string GetListName()
        {
            return "WhatsHot";
        }
        protected override string AddWhereCamlTo(string condition)
        {
            string startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.IsNull_TAG_CLOSE);
            string startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            string startDateOr = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;
            if (string.IsNullOrEmpty(condition))
                return startDateOr;
            return CMConstants.AND_TAG_OPEN + condition + startDateOr + CMConstants.AND_TAG_CLOSE;
        }
        protected override void Refine(IList<WhatsHotItem> items)
        {
            if (items == null) return;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                item.WhatSHotImage =
                    ContentManagementHelper.GetRelativePath(item.WhatSHotImage, RequestDomainName);
            }
        }

        //protected override string GetViewFields()
        //{
        //    return CMConstants.DefaultFieldNames + "<FieldRef Name='PromotionTitle' /><FieldRef Name='PromotionText' /><FieldRef Name='WhatHotBTKEY' />" +
        //           "<FieldRef Name='WhatHotImage' /><FieldRef Name='PromotionFolder' /><FieldRef Name='PromotionCode' /><FieldRef Name='ImageWebtrendsTag' />" +
        //           "<FieldRef Name='ButtonWebtrendsTag' />";
        //}
        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["AdName"],
                       item => item["PromotionTitle"],
                       item => item["PromotionText"],
                       item => item["WhatHotBTKEY"],
                       item => item["WhatHotImage"],
                       item => item["PromotionFolder"],
                       item => item["PromotionCode"],
                       item => item["ImageWebtrendsTag"],
                       item => item["ButtonWebtrendsTag"]
                       ));
        }
    }
}
