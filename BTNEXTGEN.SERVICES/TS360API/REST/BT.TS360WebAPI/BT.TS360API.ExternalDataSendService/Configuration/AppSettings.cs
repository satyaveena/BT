using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.ExternalDataSendService.Configration
{
    public class AppSettings
    {
        public static string NoSQLApiProfilesUrl
        {
            get { return ConfigurationManager.AppSettings["NoSQLApiUrl_Profiles"]; }
        }

        public static string NextGenProfilesConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["NextGenProfilesConnStr"].ConnectionString; }
        }

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

        public static string EmailTo
        {
            get { return ConfigurationManager.AppSettings["EmailTo"]; }
        }

        public static string CurrentEnvironment
        {
            get { return ConfigurationManager.AppSettings["CurrentEnvironment"]; }
        }
    }
}
