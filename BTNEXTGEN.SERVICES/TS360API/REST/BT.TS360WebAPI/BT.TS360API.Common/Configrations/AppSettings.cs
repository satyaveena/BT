using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360Constants;

namespace BT.TS360API.Common.Configrations
{
    public class AppSettings
    {
        public static string PricingBatchSize
        {
            get { return ConfigurationManager.AppSettings["Pricing_Batch_Size"]; }
        }

        public static string PricingBatchWaitingTime
        {
            get { return ConfigurationManager.AppSettings["Pricing_Batch_WaitingTime"]; }
        }
        public static string InternetURL
        {
            get { return ConfigurationManager.AppSettings["InternetSiteURL"]; }
        }
        //public static string NoSQLApiUrlNRCProductInfo
        //{
        //    get { return ConfigurationManager.AppSettings["NoSQLApiUrl_NRCProductInfo"]; }
        //}

        public static string NoSqlApiUrlInventoryDemand
        {
            get { return ConfigurationManager.AppSettings["NoSQLApiUrl_InventoryDemand"]; }
        }

        public static string NoSqlApiUrlDemandHistory
        {
            get { return ConfigurationManager.AppSettings["NoSQLApiUrl_DemandHistory"]; }
        }

        public static string NoSqlApiUrlCartInventoryFacet
        {
            get { return ConfigurationManager.AppSettings["NoSQLApiUrl_CartInventoryFacet"]; }
        }

        public static string SuperWarehouseInventoryThreshold
        {
            get { return ConfigurationManager.AppSettings["SuperWarehouseInventoryThreshold"]; }
        }

        public static string CsproductcatalogConnectionstring
        {
            get { return ConfigurationManager.AppSettings["CSProductCatalog_ConnectionString"]; }
        }

        public static string CsMarketingConnectionstring
        {
            get { return ConfigurationManager.AppSettings["MarketingConnectionString"]; }
        }

        public static string ProductCatalogConnectionString
        {
            get { return ConfigurationManager.AppSettings["ProductCatalog_ConnectionString"]; }
        }

        public static string MaxLinePerCartTitle
        {
            get { return ConfigurationManager.AppSettings["MaxLinesPerCart"]; }
        }

        public static string OrderDbConnString
        {
            get { return ConfigurationManager.AppSettings["OrderDbConnString"]; }
        }

        public static string PricingSqlCmdTimeout
        {
            get { return ConfigurationManager.AppSettings["PricingSqlCmdTimeout"]; }
        }

        public static string PromotionServiceBatchSize
        {
            get { return ConfigurationManager.AppSettings["PromotionServiceBatchSize"]; }
        }

        public static string GaleLiteral
        {
            get { return ConfigurationManager.AppSettings["GaleLiteral"]; }
        }

        public static string RealtimeWsSysid
        {
            get { return ConfigurationManager.AppSettings["RealTime_WS_SysID"]; }
        }

        public static string RealtimeWsSyspass
        {
            get { return ConfigurationManager.AppSettings["RealTime_WS_SysPass"]; }
        }

        public static string TolasPricingServiceUrl
        {
            get { return ConfigurationManager.AppSettings["PricingService_URL"]; }
        }

        public static string PromotionServiceUrl
        {
            get { return ConfigurationManager.AppSettings["PromotionService_Url"]; }
        }

        public static string LogsConnectionstring
        {
            get { return ConfigurationManager.AppSettings["Logs_ConnectionString"]; }
        }

        public static string NextGenProfilesConnectionString
        {
            get { return ConfigurationManager.AppSettings["NextGenProfilesConnectionString"]; }
        }

        public static string DemandBucketCachingMinutes
        {
            get { return ConfigurationManager.AppSettings["DemandBucketCachingMinutes"]; }
        }

        public static string AutoSuggestCollection
        {
            get { return ConfigurationManager.AppSettings["Auto_Suggest_Collection"]; }
        }

        public static string SearchViewItem
        {
            get { return ConfigurationManager.AppSettings["Search_View_Item"]; }
        }

        public static string StockCheckDefaultSOPAccountID
        {
            get { return ConfigurationManager.AppSettings["StockCheckDefaultSOPAccountID"]; }
        }

        public static string TitleSourceSiteUrl
        {
            get { return ConfigurationManager.AppSettings[GlobalConfigurationKey.TitleSourceSiteUrl]; }
        }
        public static string ILSMaxRetryCount
        {
            get { return ConfigurationManager.AppSettings["ILSMaxRetryCount"]; }
        }

        public static string ILSRetryDelayInSecond
        {
            get { return ConfigurationManager.AppSettings["ILSRetryDelayInSecond"]; }
        }

        public static string Axis360VendorUserName
        {
            get { return ConfigurationManager.AppSettings["Axis360VendorUserName"]; }
        }

        public static string Axis360VendorPassword
        {
            get { return ConfigurationManager.AppSettings["Axis360VendorPassword"]; }
        }

        public static string Axis360LibraryId
        {
            get { return ConfigurationManager.AppSettings["Axis360LibraryId"]; }
        }

        public static string Axis360ApiURL
        {
            get { return ConfigurationManager.AppSettings["Axis360ApiURL"]; }
        }

        public static string NoSqlAxis360InventoryUrl
        {
            get { return ConfigurationManager.AppSettings["NoSqlAxis360InventoryUrl"]; }
        }

        public const string ILSDebugMode = "ILSDebugMode";
        public const string ILSLogFolderPath = "ILSLogFolderPath";
        public const string ILSMarcProfileExtName = "ILSMarcProfileExtName";
        public const string ILSMarcJsonExtName = "ILSMarcJsonExtName";

        public const string FTPServer = "FTPServer";
        public const string FTPUserID = "FTPUserID";
        public const string FTPPassword = "FTPPassword";
        public const string FTPFolder = "FTPFolder";


        public static T RetriveAppSettings<T>(string keyname)
        {
            var tmp = ConfigurationManager.AppSettings[keyname];
            if (tmp == null)
                throw new Exception("Value of '" + keyname + "' not set in application config file.");

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
                throw new Exception(string.Format("Value of '{0}' is not expected data type '{1}'.", keyname, typeof(T)));
            }

            return (T)outputValue;

        }

        public static T RetriveAppSettings<T>(string keyname, T defaultValueIfKeyMissing)
        {
            var tmp = ConfigurationManager.AppSettings[keyname];
            if (tmp == null)
            {
                var msg = string.Format("App Config file - Key NOT found: '{0}', use default value '{1}'", keyname, defaultValueIfKeyMissing);
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
                throw new Exception(string.Format("Value of '{0}' is not expected data type '{1}'.", keyname, typeof(T)));
            }

            return (T)outputValue;

        }
    }
}
