using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPAutoRankingConsole.Common
{
    public class AppSettings
    {
        public static string OrdersConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Orders"].ConnectionString; }
        }

        public static string ExceptionLoggingConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ExceptionLogging"].ConnectionString; }
        }

        public static string ESPBaseURI
        {
            get { return ConfigurationManager.AppSettings["ESPBaseURI"].ToString(); }
        }

        public static string ESPVendorKey
        {
            get { return ConfigurationManager.AppSettings["ESPVendorKey"].ToString(); }
        }

        public static string ESPVersion
        {
            get { return ConfigurationManager.AppSettings["ESPVersion"].ToString(); }
        }

        public static int MaxBasketCount
        {
            get { return int.Parse(ConfigurationManager.AppSettings["MaxBasketCount"].ToString()); }
        }

        /// <summary>
        /// JobId (or ThreadId) to pass to procESPGetAutoRankRequests. Value is unique for each task setup in Task Scheduler.
        /// </summary>
        public static string JobId
        {
            get { return ConfigurationManager.AppSettings["JobId"].ToString(); }
        }
    }
}
