using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class CollectionGeneral : CMListBase<CollectionGeneralItem>
    {
        public CollectionGeneral()
        {
            HasAdName = true;
        }
        protected override string GetListName()
        {
            return "CollectionGeneral";
        }
        protected string QueryCondition(string condition)
        {
            string startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.IsNull_TAG_CLOSE);
            string startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            string startDateOr = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;
            if (string.IsNullOrEmpty(condition))
                return startDateOr;
            return CMConstants.AND_TAG_OPEN + condition + startDateOr + CMConstants.AND_TAG_CLOSE;
        }

        //protected override string GetViewFields()
        //{
        //    return CMConstants.DefaultFieldNames + "<FieldRef Name='CollectionTitle' /><FieldRef Name='WebTrendsTag' />";
        //}

        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["AdName"],
                       item => item["CollectionTitle"],
                       item => item["WebTrendsTag"]
                       ));
        }
    }
}
