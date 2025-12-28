using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BT.TS360API.SearchService.Common.Configuration
{
    public class AppSetting
    {
        public static string Ts360SiteUrl
        {
            get { return ConfigurationManager.AppSettings["TS360SiteUrl"]; }
        }

        public static string AllowedHeaderForAuthentication
        {
            get { return ConfigurationManager.AppSettings["AllowedHeaderForAuthentication"]; }
        }

        public static string AllowedMethodForAuthentication
        {
            get { return ConfigurationManager.AppSettings["AllowedMethodForAuthentication"]; }
        }

        public static string PassPhraseForAuthentication
        {
            get { return ConfigurationManager.AppSettings["PassPhraseForAuthentication"]; }
        }

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

        public static string DataFixSendToMail
        {
            get { return ConfigurationManager.AppSettings["DataFixSendToMail"]; }
        }
        public static string SmtpServer
        {
            get { return ConfigurationManager.AppSettings["SmtpServer"]; }
        }

        public static string OCSIdentifier
        {
            get { return ConfigurationManager.AppSettings["OCSIdentifier"]; }
        }

        public static string APIPassPhrase
        {
            get { return ConfigurationManager.AppSettings["APIPassPhrase"].ToString(); }
        }
    }
}