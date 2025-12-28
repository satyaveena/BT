using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.WinServices.Helper
{
    public class AppSettings
    {
        public static int ILSOrderPickupTimer
        {
            get { return int.Parse(ConfigurationManager.AppSettings["ILSOrderPickupTimer"]); }
        }

        public static int ILSOrderResultTimer
        {
            get { return int.Parse(ConfigurationManager.AppSettings["ILSOrderResultTimer"]); }
        }


        public static int ILJobStatusTimer
        {
            get { return int.Parse(ConfigurationManager.AppSettings["ILJobStatusTimer"]); }
        }

        public static int NumberOfILSThreads
        {
            get { return int.Parse(ConfigurationManager.AppSettings["NumberOfILSThreads"]); }
        }

        public static int MaxQueueJobStatus
        {
            get { return int.Parse(ConfigurationManager.AppSettings["MaxQueueJobStatus"]); }
        }

        public static int MaxQueueJobResult
        {
            get { return int.Parse(ConfigurationManager.AppSettings["MaxQueueJobResult"]); }
        }

        public static bool EnableOrderPickupTimer
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["EnableOrderPickupTimer"]); }
        }

        public static bool EnableOrderStatusTimer
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["EnableOrderStatusTimer"]); }
        }
        public static bool EnableOrderResultTimer
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["EnableOrderResultTimer"]); }
        }

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
