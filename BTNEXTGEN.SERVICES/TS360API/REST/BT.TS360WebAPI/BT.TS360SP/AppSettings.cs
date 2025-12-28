using System.Configuration;

namespace BT.TS360SP
{
    public class AppSettings
    {
        public static string DemandBucketCachingMinutes
        {
            get { return ConfigurationManager.AppSettings["DemandBucketCachingMinutes"]; }
        }
        public static string PublishingURL
        {
            get { return ConfigurationManager.AppSettings["PublishingURL"]; }
        }
        public static string CollaborationURL
        {
            get { return ConfigurationManager.AppSettings["CollaborationURL"]; }
        }
        public static string InternetURL
        {
            get { return ConfigurationManager.AppSettings["InternetSiteURL"]; }
        }
        public static string AuthURL
        {
            get { return ConfigurationManager.AppSettings["AuthSiteURL"]; }
        }
    }
}
