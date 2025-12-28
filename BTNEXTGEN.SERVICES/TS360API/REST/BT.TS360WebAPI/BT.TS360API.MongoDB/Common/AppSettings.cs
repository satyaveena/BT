using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace BT.TS360API.MongoDB.Common
{
    public class AppSettings
    {
        public static string MongoDBConnectionString
        {
            get { return ConfigurationManager.AppSettings["MongoDBConnectionString"].ToString(); }
        }

        public static int MaxConnectionRetries
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxConnectionRetries"]); }
        }

        public static int RetryWaitTime
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["RetryWaitTime"]); }
        }

        public static string ExceptionLoggingConnectionString
        {
            get { return ConfigurationManager.AppSettings["Logs_ConnectionString"].ToString(); }
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

        public static string AuthValueForAuthentication
        {
            get { return ConfigurationManager.AppSettings["AuthValueForAuthentication"]; }
        }
    }
}