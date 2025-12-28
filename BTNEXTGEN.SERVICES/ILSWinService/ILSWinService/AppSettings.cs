using System.Configuration;


namespace ILSWinService
{
    public class AppSettings
    {

        #region ServiceSettings

        #endregion ServiceSettings


        #region LoggingSettings
        public static string CurrentEnvironment
        {
            get { return ConfigurationManager.AppSettings["CurrentEnvironment"].ToString(); }
        }

        public static string EmailTo
        {
            get { return ConfigurationManager.AppSettings["EmailTo"].ToString(); }
        }

        public static string EmailSMTPServer
        {
            get { return ConfigurationManager.AppSettings["EmailSMTPServer"].ToString(); }
        }


        public static string ExceptionLoggingConnectionString
        {
            get { return ConfigurationManager.AppSettings["ExceptionLoggingConnectionString"].ToString(); }
        }

        public static string LogFolder
        {
            get { return ConfigurationManager.AppSettings["LogFolder"].ToString(); }
        }

        public static string LogFilePrefix
        {
            get { return ConfigurationManager.AppSettings["LogFilePrefix"].ToString(); }
        }

        //public static string TimeIntervalToCheck
        //{
        //    get { return ConfigurationManager.AppSettings["TimeIntervalToCheck"].ToString(); }
        //}

        //public static string TimeLastUpdated
        //{
        //    get { return ConfigurationManager.AppSettings["TimeLastUpdated"].ToString(); }
        //}

        //public static string TimeToExecuteHour
        //{
        //    get { return ConfigurationManager.AppSettings["TimeToExecuteHour"].ToString(); }
        //}

        #endregion LoggingSettings

        #region MongoSettings

        //public static string MongoDBConnectionString
        //{
        //    get { return ConfigurationManager.AppSettings["MongoDBConnectionString"].ToString(); }
        //}

        //public static int MaxConnectionRetries
        //{
        //    get { return Convert.ToInt32(ConfigurationManager.AppSettings["MaxConnectionRetries"]); }
        //}

        //public static int RetryWaitTime
        //{
        //    get { return Convert.ToInt32(ConfigurationManager.AppSettings["RetryWaitTime"]); }
        //}

        //public static int InventoryBatchSize
        //{
        //    get { return Convert.ToInt32(ConfigurationManager.AppSettings["InventoryBatchSize"]); }
        //}

        #endregion MongoSettings       
    }
}
