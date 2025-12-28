using System.Configuration;

namespace BT.TS360API.Cache
{
    public class AppSettings
    {
        public static string DistributedCacheName
        {
            get { return ConfigurationManager.AppSettings["DistributedCacheName"]; }
        }

        public static string DistributedCacheRegion
        {
            get { return ConfigurationManager.AppSettings["DistributedCacheRegion"]; }
        }

        public static string DistributedCacheDuration
        {
            get { return ConfigurationManager.AppSettings["DistributedCacheDuration"]; }
        }
    }
}