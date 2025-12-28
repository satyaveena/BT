using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360Constants;

namespace BT.TS360SP
{
    public  class CMListBasePublishing : ICMListSite
    {

        public string GetWebUrl()
        {
            return AppSettings.AuthURL + AppSettings.PublishingURL;
        }
        public string QueryCondition(string condition)
        {
            string result = string.Empty;
            string expiredDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.ExpirationDate, CMConstants.IsNull_TAG_CLOSE);
            string expiredDateGtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.GT_TAG_OPEN, CMFieldNameConstants.ExpirationDate, CMConstants.GT_TAG_CLOSE);
            string expiredDateOr = CMConstants.OR_TAG_OPEN + expiredDateNull + expiredDateGtoday + CMConstants.OR_TAG_CLOSE;

            string itemStatusEq = string.Format("{0}<FieldRef Name='{1}' /><Value Type='Text'>{2}</Value>{3}", CMConstants.EQ_TAG_OPEN, CMFieldNameConstants.ItemStatus, ItemStatus.Published, CMConstants.EQ_TAG_CLOSE);
            string expiredPublishedAnd = CMConstants.AND_TAG_OPEN + expiredDateOr + itemStatusEq + CMConstants.AND_TAG_CLOSE;

            if (string.IsNullOrEmpty(condition))
                result = expiredPublishedAnd;
            else
                result = CMConstants.AND_TAG_OPEN + expiredPublishedAnd + condition + CMConstants.AND_TAG_CLOSE;
            return result;
        }

    }
}
