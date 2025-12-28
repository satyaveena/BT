using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Common.Configrations
{
    public static class DemandBucketSettings
    {

        

        public static Dictionary<string, int> GetSettings()
        {
           // var siteUrl = SPContext.Current == null ? HttpContext.Current.Request.Url.Authority : SPContext.Current.Site.RootWeb.Url;

            var settings = new Dictionary<string, int>();
//            SPSecurity.RunWithElevatedPrivileges(delegate
//            {
//                using (var site = new SPSite(siteUrl))
//                {
//                    using (var web = site.OpenWeb())
//                    {
//                        var spList = web.Lists["DemandBucket"];
//                        foreach (SPListItem item in spList.Items)
//                        {
//                            var key = item["Title"].ToString();
//                            var value = Int32.Parse(item["Value"].ToString());
//
//                            if (!settings.ContainsKey(key))
//                            {
//                                settings.Add(key, value);
//                            }
//                        }
//                    }
//                }
//            });
            return settings;
        }
    }
}
