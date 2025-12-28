using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360Constants;

namespace BT.TS360SP
{
    public  class CMListBasePreview:ICMListSite
    {
        public string GetWebUrl()
        {
            return AppSettings.AuthURL + AppSettings.CollaborationURL;
        }
        public  string QueryCondition(string condition)
        {
            string result = string.Empty;
            string itemStatusEq = @"<Or>
                                         <Eq>
                                            <FieldRef Name='ItemStatus' />
                                            <Value Type='Choice'>Approved</Value>
                                         </Eq>
                                         <Eq>
                                            <FieldRef Name='ItemStatus' />
                                            <Value Type='Choice'>Published</Value>
                                         </Eq>
                                      </Or>";
            if (string.IsNullOrEmpty(condition))
                result = itemStatusEq;
            else
                result = CMConstants.AND_TAG_OPEN + condition + itemStatusEq + CMConstants.AND_TAG_CLOSE;
            return result;
        }
    }
}
