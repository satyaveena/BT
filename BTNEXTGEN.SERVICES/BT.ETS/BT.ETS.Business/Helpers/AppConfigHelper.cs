using BT.ETS.Business.Constants;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace BT.ETS.Business.Helpers
{
    /// <summary>
    /// Class ConfigSectionClass
    /// </summary>
    public class AppConfigHelper
    {
        #region AppSettings keys
        public const string ProductCatalog_ConnectionString = "ProductCatalog_ConnectionString";
        public const string Orders_ConnectionString = "Orders_ConnectionString";
        public const string Logs_ConnectionString = "Logs_ConnectionString";
        public const string Profiles_ConnectionString = "Profiles_ConnectionString";
        public const string AppSearchServiceURL = "AppSearchServiceURL";
        public const string SqlCommandTimeOut = "SqlCommandTimeOut";
        public const string WebRequestTimeOutInSec = "WebRequestTimeOutInSec";

        public const string NoSqlApiUrlSeries = "NoSqlApiUrlSeries";
        public const string NoSqlApiUrl = "NoSqlApiUrl";
        public const string BackgroundQueuePriority = "BackgroundQueuePriority";
        public const string MaxConnectionRetries = "MaxConnectionRetries";
        public const string RetryWaitTime = "RetryWaitTime";
        public const string MongoDBConnectionString = "MongoDBConnectionString";
        public const string MongoRetryValues = "MongoRetryValues";
        public const string SearchServiceRetryValues = "SearchServiceRetryValues";
        public const string MaxDupeCheckBatchSize_CH = "MaxDupeCheckBatchSize_CH";
        public const string MaxDupeCheckBatchSize_O = "MaxDupeCheckBatchSize_O";
        public const string MaxDupeCheckBatchSize_S = "MaxDupeCheckBatchSize_S";
        public const string MaxPricingBatchSize = "MaxPricingBatchSize";
        #endregion AppSettings keys

        #region Method

        /// <summary>
        /// Method will return default value if the keyname does NOT exist in config file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyname"></param>
        /// <param name="defaultValueIfKeyMissing"></param>
        /// <returns></returns>
        public static T RetriveAppSettings<T>(string keyname, T defaultValueIfKeyMissing)
        {
            var tmp = ConfigurationManager.AppSettings[keyname];
            if (tmp == null)
            {
                var msg = string.Format("App Config file - Key NOT found: '{0}', use default value '{1}'", keyname, defaultValueIfKeyMissing);
                //Logger.WriteLog(Logger.LogType.INFO, msg);
                return defaultValueIfKeyMissing;
            }

            object outputValue;
            try
            {
                outputValue = Convert.ChangeType(tmp, typeof(T));
            }
            catch (InvalidCastException)
            {
                throw new Exception(string.Format("Value of '{0}' is not expected data type '{1}'.", keyname, typeof(T)));
            }
            catch (FormatException)
            {
                throw new Exception(string.Format("Value of '{0}' is not expected data format '{1}'.", keyname, typeof(T)));
            }

            return (T)outputValue;

        }

        /// <summary>
        /// Method throws exception if config file doesn't contain given keyname.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyname"></param>
        /// <returns></returns>
        public static T RetriveAppSettings<T>(string keyname)
        {
            var tmp = ConfigurationManager.AppSettings[keyname];
            if (tmp == null)
                throw new Exception("Value of " + keyname + " not set in application config file.");

            object outputValue;
            try
            {
                outputValue = Convert.ChangeType(tmp, typeof(T));
            }
            catch (InvalidCastException)
            {
                throw new Exception(string.Format("Value of '{0}' is not expected data type '{1}'.", keyname, typeof(T)));
            }
            catch (FormatException)
            {
                throw new Exception(string.Format("Value of '{0}' is not expected data format '{1}'.", keyname, typeof(T)));
            }

            return (T)outputValue;

        }
        #endregion

    }
}
