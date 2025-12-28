using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Helpers
{
    public class AppConfigHelper
    {
        #region AppSettings keys
       
        public const string Orders_ConnectionString = "Orders_ConnectionString";
        public const string NextGenProfiles_ConnectionString = "NextGenProfiles_ConnectionString";
        public const string Profiles_ConnectionString = "Profiles_ConnectionString";
        public const string ProductCatalog_ConnectionString = "ProductCatalog_ConnectionString";

        public const string SqlCommandTimeOut = "SqlCommandTimeOut";

        public const string MongoDBConnectionString = "MongoDBConnectionString";
        public const string MongoDBMaxConnectionRetries = "MaxConnectionRetries";
        public const string MongoDBRetryWaitTime = "RetryWaitTime";

        public const string LogFolder = "LogFolder";
        public const string PurchaseOrderDirectory = "PurchaseOrderDirectory";
        public const string ResultDirectory = "ResultDirectory";
        public const string PostbackURL = "PostbackURL";

        public const string InternetSiteURL = "InternetSiteURL";

        public const string PolarisVersion = "PolarisVersion";
        public const string PolarisLangID = "PolarisLangID";
        public const string PolarisAppID = "PolarisAppID";
        public const string PolarisOrgID = "PolarisOrgID";

        public const string MaxLinesPerCart = "MaxLinesPerCart";

        public const string MARC_SERVICE_URL = "MARC_Service_Url";
        public const string MARC_SERVICE_TIMEOUT = "MARC_Service_Timeout";
        
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
