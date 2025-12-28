using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Configuration;

namespace BTNextGen.Services.Common
{
    public class Config
    {

        # region Public Methods


        /// <summary>
        /// Product Definition for Bulk Product Import
        /// </summary>
        public static string ProductDefinition
        {
            get
            {
                return GetAppSetting("ProductDefinition");
            }
        }


        /// <summary>
        /// Threshold (in Days) for purging baskets
        /// </summary>
        public static string CSSiteName
        {
            get
            {
                return GetAppSetting("SiteName");
            }
        }


        /// <summary>
        /// Threshold (in Days) for purging baskets
        /// </summary>
        public static string PurgeBasketThresholdInDays
        {
            get
            {
                return GetAppSetting("PurgeBasketThresholdInDays");
            }
        }


        /// <summary>
        /// CS Marketing Web Service endpoint from Web.Config
        /// </summary>
        public static string MarketingServiceUrl
        {
            get
            {
                return GetAppSetting("CSMarketingService");
            }
        }

        /// <summary>
        /// CS Profiles Web Service endpoint from Web.Config
        /// </summary>
        public static string ProfilesServiceUrl
        {
            get
            {
                return GetAppSetting("CSProfilesService");
            }
        }

        /// <summary>
        /// inventory demand Web Service endpoint from Web.Config
        /// </summary>
        public static string InventoryServiceUrl
        {
            get
            {
                return GetAppSetting("InventoryServiceUrl");
            }
        }

        /// <summary>
        /// CS Orders Web Service endpoint from Web.Config
        /// </summary>
        public static string OrdersServiceUrl
        {
            get
            {
                return GetAppSetting("CSOrdersService");
            }
        }

        /// <summary>
        /// CS Orders Web Service endpoint from Web.Config
        /// </summary>
        public static string CatalogServiceUrl
        {
            get
            {
                return GetAppSetting("CSCatalogService");
            }
        }

        public static string CyberSourceReportsURL
        {
            get
            {
                return GetAppSetting("CyberSourceReportsURL");
            }
        }

        public static bool IsLoggingOn
        {
            get
            {
                return GetAppSettingFlag("IsLogging");
            }
        }

        public static string TS360SharepointUrl
        {
            get
            {
                return GetAppSetting("TS360SharepointUrl");
            }
        }

        public static string CSSqlConnString
        {
            get
            {
                return GetAppSetting("CSSqlConnString");
            }
        }

        public static string FTPDropoffFolder
        {
            get
            {
                return GetAppSetting("FTPDropoffFolder");
            }
        }

        public static string RegistryKey
        {
            get
            {
                return GetAppSetting("RegistryKey");
            }
        }
        public static string SQLCommandTimeout
        {
            get
            {
                return GetAppSetting("SQLCommandTimeout");
            }
        }

        public static string EmailToExceptions
        {
            get
            {
                return GetAppSetting("EmailToExceptions");
            }
        }
        public static string EmailServer
        {
            get
            {
                return GetAppSetting("EmailServer");
            }
        }


        public static string SPLocalFolder
        {
            get
            {
                return GetAppSetting("SPLocalFolder");
            }
        }

        public static string SPSiteURLUpload
        {
            get
            {
                return GetAppSetting("SPSiteURLUpload");
            }
        }

        public static string SPSiteURLDownload
        {
            get
            {
                return GetAppSetting("SPSiteURLDownload");
            }
        }
        public static string SPMarcFileName
        {
            get
            {
                return GetAppSetting("SPMarcFileName");
            }
        }

        public static string SPLogFileName
        {
            get
            {
                return GetAppSetting("SPLogFileName");
            }
        }


        public static string SPDocLibraryName
        {
            get
            {
                return GetAppSetting("SPDocLibraryName");
            }
        }
        public static string SPUser
        {
            get
            {
                return GetAppSetting("SPUser");
            }
        }
        public static string SPPassword
        {
            get
            {
                return GetAppSetting("SPPassword");
            }
        }
        public static string SPDomain
        {
            get
            {
                return GetAppSetting("SPDomain");
            }
        }

        public static string SourceSystem
        {
            get
            {
                return GetAppSetting("SourceSystem");
            }
        }

        public static string SPMessageTemplateID
        {
            get
            {
                return GetAppSetting("SPMessageTemplateID");
            }
        }

        public static string FTPLogFileName
        {
            get
            {
                return GetAppSetting("FTPLogFileName");
            }
        }

        public static string FTPLocalFolder
        {
            get
            {
                return GetAppSetting("FTPLocalFolder");
            }
        }


        public static string DOWNLOADLogFileName
        {
            get
            {
                return GetAppSetting("DOWNLOADLogFileName");
            }
        }

        public static string DOWNLOADLocalFolder
        {
            get
            {
                return GetAppSetting("DOWNLOADLocalFolder");
            }
        }

        public static string AlertLogFileName
        {
            get
            {
                return GetAppSetting("AlertLogFileName");
            }
        }



        public static string LogDetails
        {
            get
            {
                return GetAppSetting("LogDetails");
            }
        }

        public static string ArchiveInventoryMessage
        {
            get
            {
                return GetAppSetting("ArchiveInventoryMessage");
            }
        }

        public static string VendorAPIKey
        {
            get
            {
                return GetAppSetting("VendorAPIKey");
            }
        }

        public static string Environment
        {
            get
            {
                return GetAppSetting("Environment");
            }
        }

        public static string MarcFTPTESTSourceFile
        {
            get
            {
                return GetAppSetting("MarcFTPTESTSourceFile");
            }
        }

        #endregion





        # region Private Methods
        /// <summary>
        /// Read String Setting in Config file
        /// </summary>
        /// <param name="appSettingName"></param>
        /// <returns></returns>
        private static string GetAppSetting(string appSettingName)
        {
            string iConfig = string.Empty;

            try
            {
                return WebConfigurationManager.AppSettings[appSettingName] != null ?
                    WebConfigurationManager.AppSettings[appSettingName].ToString() : "not found";
            }
            catch
            {
                // log exception
                iConfig = "not found";
            }

            return iConfig;
        }

        /// <summary>
        /// Read Boolean Setting in Config file
        /// </summary>
        /// <param name="appSettingName"></param>
        /// <returns></returns>
        private static bool GetAppSettingFlag(string appSettingName)
        {
            try
            {
                return bool.Parse(WebConfigurationManager.AppSettings[appSettingName] != null ?
                   WebConfigurationManager.AppSettings[appSettingName].ToString() : "false");
            }
            catch
            {
                // log exception
                return false;
            }
        }
        #endregion
    }
}