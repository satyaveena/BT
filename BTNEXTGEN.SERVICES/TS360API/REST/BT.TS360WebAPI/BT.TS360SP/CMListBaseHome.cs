using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360Constants;

namespace BT.TS360SP
{
    public  class CMListBaseHome : ICMListSite
    {
        public string GetWebUrl()
        {
            return AppSettings.AuthURL;
        }
        public string QueryCondition(string condition)
        {
            return condition;
        }

    }
}
