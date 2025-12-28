using System.Collections.Generic;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class InTheNews : CMListBase<InTheNewsItem>
    {
        string RequestDomainName { get; set; }
        public InTheNews(string requestDomainName)
        {
            RequestDomainName = requestDomainName;
            HasIsDefault = true;
            HasAdName = true;
            RowLimit = 1;
        }

        #region Overrides of SharePointListBase<NewFeedItem>

        protected override string GetListName()
        {
            return "InTheNews";
        }

        #endregion

        protected string QueryCondition(string condition)
        {
            string result = "";
            string startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.IsNull_TAG_CLOSE);
            string startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            string startDateOr = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;

            result = CMConstants.AND_TAG_OPEN + result + startDateOr + CMConstants.AND_TAG_CLOSE;

            return result;
        }

        protected override string AddWhereCamlTo(string currentCaml)
        {
            var startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.IsNull_TAG_CLOSE);
            var startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            var startDateOr = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;
            
            if (string.IsNullOrEmpty(currentCaml))
                return startDateOr;
            return CMConstants.AND_TAG_OPEN + currentCaml + startDateOr + CMConstants.AND_TAG_CLOSE;
        }

        protected override void Refine(IList<InTheNewsItem> items)
        {
            if (items == null) return;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                //item.NewsImage = ContentManagementHelper.RefineUrlProtocolToContextProtocol(item.NewsImage, UrlScheme);
                //item.NewsVideo = ContentManagementHelper.RefineUrlProtocolToContextProtocol(item.NewsVideo, UrlScheme);

                item.NewsImage = ContentManagementHelper.GetRelativePath(item.NewsImage, RequestDomainName);
                item.NewsVideo = ContentManagementHelper.GetRelativePath(item.NewsVideo, RequestDomainName);
            }
        }
        //protected override string GetViewFields()
        //{
        //    return "<ViewFields><FieldRef Name='Title' /></ViewFields>";
        //    //return CMConstants.DefaultFieldNames + "<FieldRef Name='eListCategory' /><FieldRef Name='eListCategory' />";
        //}
        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                item => item.Id,
                item => item["Title"],
                       item => item["AdName"],
                item => item["NewsBTKEY"],
                item => item["NewsTitle"],
                item => item["NewsImage"],
                item => item["NewsText"],
                item => item["NewsVideo"],
                item => item["PromotionCode"],
                item => item["WebTrendsTag"],
                item => item["News_x0020_Document"]
                ));
        }
    }
}
