using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BT.TS360.Services.Cybersource.Common.Configuration
{
    public class AppSetting
    {
        # region connection string
        public static string CompassDatabaseConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Compass"].ToString(); }
        }

        public static string MongoDatabaseConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["MongoProducts"].ToString(); }
        }

        public static string ElmahConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Elmah"].ToString(); }
        }

        # endregion connection string

        public static string LogFolder
        {
            get { return ConfigurationManager.AppSettings["LogFolder"].ToString(); }
        }

        public static string LogFilePrefix
        {
            get { return ConfigurationManager.AppSettings["LogFilePrefix"].ToString(); }
        }

        public static string ArchiveFolder
        {
            get { return ConfigurationManager.AppSettings["ArchiveFolder"].ToString(); }
        }

        public static string TolasFolderPB
        {
            get { return ConfigurationManager.AppSettings["TolasFolderPB"].ToString(); }
        }

        public static string TolasFolderTR
        {
            get { return ConfigurationManager.AppSettings["TolasFolderTR"].ToString(); }
        }

        public static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"].ToString(); }
        }


        public static string EmailToRecipients
        {
            get { return ConfigurationManager.AppSettings["EmailToRecipients"].ToString(); }
        }


        public static string EmailServer
        {
            get { return ConfigurationManager.AppSettings["EmailServer"].ToString(); }
        }


        public static string RunTransactionRequest_btpp
        {
            get { return ConfigurationManager.AppSettings["RunTransactionRequest_btpp"].ToString(); }
        }


        public static string RunTransactionRequest_bt
        {
            get { return ConfigurationManager.AppSettings["RunTransactionRequest_bt"].ToString(); }
        }


        public static string RunTransactionRequest_majors
        {
            get { return ConfigurationManager.AppSettings["RunTransactionRequest_majors"].ToString(); }
        }


        public static string RunPaymentBatchRequest_btpp
        {
            get { return ConfigurationManager.AppSettings["RunPaymentBatchRequest_btpp"].ToString(); }
        }

        public static string RunPaymentBatchRequest_bt
        {
            get { return ConfigurationManager.AppSettings["RunPaymentBatchRequest_bt"].ToString(); }
        }

        public static string RunPaymentBatchRequest_majors
        {
            get { return ConfigurationManager.AppSettings["RunPaymentBatchRequest_majors"].ToString(); }
        }

        public static string DaysBack
        {
            get { return ConfigurationManager.AppSettings["DaysBack"].ToString(); }
        }

        public static string merchantsecretKey_btpp
        {
            get { return ConfigurationManager.AppSettings["merchantsecretKey_btpp"].ToString(); }
        }

        public static string merchantsecretKey_bt
        {
            get { return ConfigurationManager.AppSettings["merchantsecretKey_bt"].ToString(); }
        }

        public static string merchantsecretKey_majors
        {
            get { return ConfigurationManager.AppSettings["merchantsecretKey_majors"].ToString(); }
        }

        public static string merchantKeyid_btpp
        {
            get { return ConfigurationManager.AppSettings["merchantKeyid_btpp"].ToString(); }
        }

        public static string merchantKeyid_bt
        {
            get { return ConfigurationManager.AppSettings["merchantKeyid_bt"].ToString(); }
        }

        public static string merchantKeyid_majors
        {
            get { return ConfigurationManager.AppSettings["merchantKeyid_majors"].ToString(); }
        }

        public static string runEnvironment
        {
            get { return ConfigurationManager.AppSettings["runEnvironment"].ToString(); }
        }

    }

    
}
