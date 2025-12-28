using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class CollectionCarousel : CMListBase<CollectionCarouselItem>
    {
        public CollectionCarousel()
        {
            HasIsDefault = true;
            HasAdName = true;
        }
        #region Overrides of SharePointListBase<PopularItem>

        protected override string GetListName()
        {
            return "CollectionCarousel";
        }

        #endregion

        protected string QueryCondition(string condition)
        {
            string startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, 
                CMConstants.IsNull_TAG_CLOSE);
            string startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", 
                CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            string startDateOr = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;
            if (string.IsNullOrEmpty(condition))
                return startDateOr;
            return CMConstants.AND_TAG_OPEN + condition + startDateOr + CMConstants.AND_TAG_CLOSE;
        }

        //protected string QueryConditionPreview(string condition)
        //{
        //    string result = ""; // base.QueryConditionPreview(condition);
        //    string collectionTitleEq = string.Format("{0}<FieldRef Name='{1}' /><Value Type='Text'>{2}</Value>{3}", 
        //        CMConstants.EQ_TAG_OPEN, "CollectionTitle", CollectionTitle, CMConstants.EQ_TAG_CLOSE);
        //    result = CMConstants.AND_TAG_OPEN + result + collectionTitleEq + CMConstants.AND_TAG_CLOSE;
        //    return result;
        //}

        public string CollectionTitle { get; set; }

        protected override string AddWhereCamlTo(string currentCaml)
        {
            string result = QueryCondition(currentCaml);
            string collectionTitleEq = string.Format("{0}<FieldRef Name='{1}' /><Value Type='Text'>{2}</Value>{3}", CMConstants.EQ_TAG_OPEN,
               "CollectionTitle", CollectionTitle, CMConstants.EQ_TAG_CLOSE);
            if (string.IsNullOrEmpty(result))
                result = collectionTitleEq;
            else
                result = CMConstants.AND_TAG_OPEN + result + collectionTitleEq + CMConstants.AND_TAG_CLOSE;
            return result;
        }

        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["AdName"],
                       item => item["CollectionTitle"],
                       item => item["BTKEY"],
                       item => item["Image"],
                       item => item["WebTrendsTag"]
                       ));
        }
    }
}
