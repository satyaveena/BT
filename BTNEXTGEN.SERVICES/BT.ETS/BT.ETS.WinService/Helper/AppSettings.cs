using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Helper
{
    public class AppSettings
    {
        public static int TimerInterval
        {
            get { return int.Parse(ConfigurationManager.AppSettings["TimerInterval"]); }
        }

        public static string EspReceiveCartReceivedUrl
        {
            get { return ConfigurationManager.AppSettings["EspReceiveCartReceivedUrl"]; }
        }

        public static string EspReceivePricingUrl
        {
            get { return ConfigurationManager.AppSettings["EspReceivePricingUrl"]; }
        }

        public static string EspReceiveDupCheckUrl
        {
            get { return ConfigurationManager.AppSettings["EspReceiveDupCheckUrl"]; }
        }

        public static string ESPVendorKey
        {
            get { return ConfigurationManager.AppSettings["EspVendorKey"]; }
        }

        public static string RetryPeriods
        {
            get { return ConfigurationManager.AppSettings["RetryPeriods"]; }
        }

        public static int MaxQueueItemNumber
        {
            get { return int.Parse(ConfigurationManager.AppSettings["MaxQueueItemNumber"]); }
        }

        //public static string LogFolder 
        //{
        //    get { return ConfigurationManager.AppSettings["LogLocation"]; }
        //}

        public static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"]; }
        }

        public static string SmtpServer
        {
            get { return ConfigurationManager.AppSettings["EmailSMTPServer"]; }
        }

        public static string EmailList
        {
            get { return ConfigurationManager.AppSettings["EmailTo"]; }
        }
    }
}
