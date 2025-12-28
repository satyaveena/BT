using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace BT.TS360API.Authentication.Helpers
{
    public class AppSettings
    {
        public static string MongoDBConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["MongoDBConnStr"].ToString(); }
        }

        public static int MaxConnectionRetries
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxConnectionRetries"]); }
        }

        public static int RetryWaitTime
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["RetryWaitTime"]); }
        }
    }
}