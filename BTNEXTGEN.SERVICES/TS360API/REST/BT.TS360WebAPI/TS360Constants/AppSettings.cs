using System;
using System.Configuration;

namespace BT.TS360Constants
{
    public class AppSettings
    {
        public static string AutoSuggestCollection
        {
            get { return ConfigurationManager.AppSettings["Auto_Suggest_Collection"]; }
        }

        public static string SearchViewItem
        {
            get { return ConfigurationManager.AppSettings["Search_View_Item"]; }
        }

        public static string NoSQLApiUrlNRCProductInfo
        {
            get { return ConfigurationManager.AppSettings["NoSQLApiUrl_NRCProductInfo"]; }
        }

        public static int OrderSearchExportBackgroundQueuePriority
        {
            get 
            {
                try
                {
                    var priority = ConfigurationManager.AppSettings["OrderSearchExportBackgroundQueuePriority"];
                    return Convert.ToInt32(priority);
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}
