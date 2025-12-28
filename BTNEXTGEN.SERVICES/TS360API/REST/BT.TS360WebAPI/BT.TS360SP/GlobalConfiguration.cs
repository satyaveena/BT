using System.Collections.Generic;
//using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;

namespace BT.TS360SP
{
    public static class GlobalConfiguration
    {
        #region Const

        private const string GlobalConfigurationCacheKey = "__GlobalConfigurationCacheKey";
        private const string GlobalFileCacheKey = "__GlobalFileCacheKey";

        #endregion

        #region public methods

        /// <summary>
        /// Read AppSettings
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadAppSetting(string key)
        {
            var settings = GetSettings();
            if (settings == null)
                return null;
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            //
            if (settings.ContainsKey(key))
            {
                return settings[key];
            }
            else
            {
                //Logger.Write("GlobalConfiguration", string.Format("Key: {0} is not existed", key));
                return null;
            }
        }

        /// <summary>
        /// Read Global File Setting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        //public static BinarySetting ReadBinarySetting(string key)
        //{
        //    var settings = GetFileSettings();
        //    return settings.ContainsKey(key) ? settings[key] : null;
        //}

        #endregion

        #region private methods

        private static Dictionary<string, string> GetSettings()
        {
            //var settings = CachingController.Instance.Read(GlobalConfigurationCacheKey) as Dictionary<string, AppSetting>;
            //if (settings == null)
            //{
                //
               var  settings = new Dictionary<string, string>();

                //if (SPContext.Current == null || SPContext.Current.Site == null || SPContext.Current.Site.RootWeb == null)
                //    return settings;
            var list = new GlobalConfigurationList();
            var items = list.Get();
            foreach (var item in items)
            {
                if(!settings.ContainsKey(item.Title))
                    settings.Add(item.Title,item.Value);
            }
            //            //CachingController.Instance.Write(GlobalConfigurationCacheKey, settings);
            
            //}
            return settings;
        }

        //private static Dictionary<string, BinarySetting> GetFileSettings()
        //{
        //    var settings = CacheHelper.Get(GlobalFileCacheKey) as Dictionary<string, BinarySetting>;
        //    if (settings != null)
        //        return settings;
        //    //
        //    settings = new Dictionary<string, BinarySetting>();
        //    SPSecurity.RunWithElevatedPrivileges(delegate
        //    {
        //        using (var site = new SPSite(SPContext.Current.Site.RootWeb.Url))
        //        {
        //            using (var web = site.RootWeb)
        //            {

        //                var spList = web.Lists["GlobalFileSetting"];
        //                foreach (SPListItem item in spList.Items)
        //                {
        //                    var setting = new BinarySetting
        //                    {
        //                        Key = item["Title"] as string,
        //                        Description = item["Description"] as string,
        //                        Data = item.File.OpenBinary()
        //                    };
        //                    //
        //                    if (!settings.ContainsKey(setting.Key))
        //                        settings.Add(setting.Key, setting);
        //                }
        //                CacheHelper.Write(GlobalFileCacheKey, settings);
        //            }
        //        }
        //    });
        //    return settings;
        //}

        #endregion

        //public static int GetSessionTimeout()
        //{
        //    int timeout;
        //    var readAppSetting = ReadAppSetting(GlobalConfigurationKey.Sessiontimeout);
        //    Int32.TryParse(readAppSetting.Value, out timeout);
        //    return timeout;
        //}

        //public static bool IsEnabledSingleActiveUserSession()
        //{
        //    var readAppSetting = ReadAppSetting(GlobalConfigurationKey.Enabledsingleactiveusersession);
        //    if (readAppSetting != null)
        //        return readAppSetting.Value.ToLower() == "true";
        //    return false;
        //}

        //public static int GetRememberMeDurationTimeout()
        //{
        //    int timeout = 0;
        //    var readAppSetting = ReadAppSetting(GlobalConfigurationKey.RememberMeDuration);
        //    if (readAppSetting != null)
        //        Int32.TryParse(readAppSetting.Value, out timeout);
        //    return timeout;
        //}

        //public static long PricingTimerInterval
        //{
        //    get
        //    {
        //        long interval = 30;
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.PricingTimerInterval);
        //        if (readAppSetting != null)
        //            Int64.TryParse(readAppSetting.Value, out interval);
        //        return interval * 1000;
        //    }
        //}

        //public static int MaxLineItemsForPrint
        //{
        //    get
        //    {
        //        int max = 500;
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.MaxLineItemsForPrint);
        //        if (readAppSetting != null)
        //            Int32.TryParse(readAppSetting.Value, out max);
        //        return max;
        //    }
        //}

        //public static string DistributedCacheName
        //{
        //    get
        //    {
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.DistributedCacheName);
        //        return readAppSetting.Value;
        //    }
        //}

        //public static string Ts360SystemNotification
        //{
        //    get
        //    {
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.Ts360SystemNotification);
        //        return readAppSetting.Value;
        //    }
        //}

        //public static string DistributedCache_Env_Region
        //{
        //    get
        //    {
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.DistributedCache_Env_Region);
        //        return readAppSetting.Value;
        //    }
        //}

        //public static string DistributedCacheAdminUsers
        //{
        //    get
        //    {
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.DistributedCacheAdminUsers);
        //        return readAppSetting.Value;
        //    }
        //}

        //public static string HelpTrainingLink
        //{
        //    get
        //    {
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.HelpTrainingLink);
        //        return readAppSetting != null ? readAppSetting.Value : string.Empty;
        //    }
        //}

        //public static string DataFixSendToMail
        //{
        //    get
        //    {
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.DataFixSendToMail);
        //        return readAppSetting.Value;
        //    }
        //}

        //public static string OCSIdentifier
        //{
        //    get
        //    {
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.OCSIdentifier);
        //        return readAppSetting.Value;
        //    }
        //}

        //public static int UserProfileDurationCache
        //{
        //    get
        //    {
        //        int defaultValue = 15;
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.UserProfileDurationCache);
        //        if (readAppSetting != null)
        //            Int32.TryParse(readAppSetting.Value, out defaultValue);
        //        return defaultValue;
        //    }
        //}

        //public static int DistributedCacheDuration
        //{
        //    get
        //    {
        //        var readAppSetting = ReadAppSetting(GlobalConfigurationKey.DistributedCacheDuration);
        //        return string.IsNullOrEmpty(readAppSetting.Value) ? 60 : int.Parse(readAppSetting.Value);
        //    }
        //}
    }
}
