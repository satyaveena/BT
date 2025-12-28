using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Common.Configuration
{
    public class AppSetting
    {
        # region connection string
        public static string ExceptionLoggingDatabaseConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ExceptionLogging"].ToString(); }
        }

        public static string OrdersDatabaseConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Orders"].ToString(); }
        }

        public static string ProfilesDatabaseConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Profiles"].ToString(); }
        }

        public static string ProductCatalogDatabaseConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ProductCatalog"].ToString(); }
        }

        public static string NextGenProfilesDatabaseConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["NextGenProfiles"].ToString(); }
        }

        # endregion connection string

        public static string VendorAPIKey
        {
            get { return ConfigurationManager.AppSettings["VendorAPIKey"].ToString(); }
        }

        public static string UserAlertWebServiceUrl
        {
            get { return ConfigurationManager.AppSettings["UserAlertServiceUrl"].ToString(); }
        }

        public static string ESPRankLogFolder
        {
            get { return ConfigurationManager.AppSettings["ESPRankLogFolder"].ToString(); }
        }

        public static string ESPRankLogFilePrefix
        {
            get { return ConfigurationManager.AppSettings["ESPRankLogFilePrefix"].ToString(); }
        }

        public static string ESPRankEnableTrace
        {
            get { return ConfigurationManager.AppSettings["ESPRankEnableTrace"].ToString(); }
        }

        public static string ESPRankEnableCheckCache
        {
            get { return ConfigurationManager.AppSettings["ESPRankEnableCheckCache"].ToString(); }
        }

        public static string ESPEmailFlag
        {
            get { return ConfigurationManager.AppSettings["ESPEmailFlag"].ToString(); }
        }

        public static string ESPEmailTo
        {
            get { return ConfigurationManager.AppSettings["ESPEmailTo"].ToString(); }
        }

        public static string ESPEmailServer
        {
            get { return ConfigurationManager.AppSettings["ESPEmailServer"].ToString(); }
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

        //Create Cart Controller Configuration Settings


        public static string BasketLineSplitLimit
        {
            get { return ConfigurationManager.AppSettings["BasketLineSplitLimit"].ToString(); }
        }
        public static string BasketLineOverallLimit
        {
            get { return ConfigurationManager.AppSettings["BasketLineOverallLimit"].ToString(); }
        }

        public static string baseURL
        {
            get { return ConfigurationManager.AppSettings["baseURL"].ToString(); }
        }

        public static string passPhrase
        {
            get { return ConfigurationManager.AppSettings["passPhrase"].ToString(); }
        }
        public static string initVector
        {
            get { return ConfigurationManager.AppSettings["initVector"].ToString(); }
        }


        public static string CreateEnableTraceLogFile
        {
            get { return ConfigurationManager.AppSettings["CreateEnableTraceLogFile"].ToString(); }
        }

        public static string CreateEnableTraceRequestFile
        {
            get { return ConfigurationManager.AppSettings["CreateEnableTraceRequestFile"].ToString(); }
        }



        public static string CreateEnableTraceLogSQL
        {
            get { return ConfigurationManager.AppSettings["CreateEnableTraceLogSQL"].ToString(); }
        }


        public static string CreateLogFilePrefix
        {
            get { return ConfigurationManager.AppSettings["CreateLogFilePrefix"].ToString(); }
        }

        public static string CreateLogFolder
        {
            get { return ConfigurationManager.AppSettings["CreateLogFolder"].ToString(); }
        }

        public static string CreateREQFilePrefix
        {
            get { return ConfigurationManager.AppSettings["CreateREQFilePrefix"].ToString(); }
        }

        public static string CreateREQFolder
        {
            get { return ConfigurationManager.AppSettings["CreateREQFolder"].ToString(); }
        }



        public static string CreateEmailExceptionFlag
        {
            get { return ConfigurationManager.AppSettings["CreateEmailExceptionFlag"].ToString(); }
        }

        public static string CreateEmailToExceptions
        {
            get { return ConfigurationManager.AppSettings["CreateEmailToExceptions"].ToString(); }
        }

        public static string CreateEmailServer
        {
            get { return ConfigurationManager.AppSettings["CreateEmailServer"].ToString(); }
        }

        public static string CreateSQLCommandTimeout
        {
            get { return ConfigurationManager.AppSettings["CreateSQLCommandTimeout"].ToString(); }
        }



        public static string PunchOutEnableTraceLogFile
        {
            get { return ConfigurationManager.AppSettings["PunchOutEnableTraceLogFile"].ToString(); }
        }

        public static string PunchOutEnableTraceRequestFile
        {
            get { return ConfigurationManager.AppSettings["PunchOutEnableTraceRequestFile"].ToString(); }
        }

        public static string PunchOutEnableTraceRequestFileDTD
        {
            get { return ConfigurationManager.AppSettings["PunchOutEnableTraceRequestFileDTD"].ToString(); }
        }


        public static string PunchOutEnableTraceResponseFile
        {
            get { return ConfigurationManager.AppSettings["PunchOutEnableTraceResponseFile"].ToString(); }
        }

        public static string PunchOutEnableTraceResponseFileDTD
        {
            get { return ConfigurationManager.AppSettings["PunchOutEnableTraceResponseFileDTD"].ToString(); }
        }

        public static string PunchOutEnableTraceLogSQL
        {
            get { return ConfigurationManager.AppSettings["PunchOutEnableTraceLogSQL"].ToString(); }
        }


        public static string PunchOutLogFilePrefix
        {
            get { return ConfigurationManager.AppSettings["PunchOutLogFilePrefix"].ToString(); }
        }

        public static string PunchOutLogFolder
        {
            get { return ConfigurationManager.AppSettings["PunchOutLogFolder"].ToString(); }
        }

        public static string PunchOutREQFilePrefix
        {
            get { return ConfigurationManager.AppSettings["PunchOutREQFilePrefix"].ToString(); }
        }

        public static string PunchOutREQFolder
        {
            get { return ConfigurationManager.AppSettings["PunchOutREQFolder"].ToString(); }

        }
        public static string PunchOutREQDTDFilePrefix
        {
            get { return ConfigurationManager.AppSettings["PunchOutREQDTDFilePrefix"].ToString(); }
        }

        public static string PunchOutREQDTDFolder
        {
            get { return ConfigurationManager.AppSettings["PunchOutREQDTDFolder"].ToString(); }

        }
        public static string PunchOutEmailExceptionFlag
        {
            get { return ConfigurationManager.AppSettings["PunchOutEmailExceptionFlag"].ToString(); }
        }

        public static string PunchOutEmailToExceptions
        {
            get { return ConfigurationManager.AppSettings["PunchOutEmailToExceptions"].ToString(); }
        }

        public static string PunchOutEmailServer
        {
            get { return ConfigurationManager.AppSettings["PunchOutEmailServer"].ToString(); }
        }

        public static string PunchOutSQLCommandTimeout
        {
            get { return ConfigurationManager.AppSettings["PunchOutSQLCommandTimeout"].ToString(); }
        }

        public static string PunchOutURL
        {
            get { return ConfigurationManager.AppSettings["PunchOutURL"].ToString(); }
        }
        public static string PunchOutSharedSecret
        {
            get { return ConfigurationManager.AppSettings["PunchOutSharedSecret"].ToString(); }
        }
        public static string PunchOutDUNS
        {
            get { return ConfigurationManager.AppSettings["PunchOutDUNS"].ToString(); }
        }

        public static string PunchOutSender
        {
            get { return ConfigurationManager.AppSettings["PunchOutSENDER"].ToString(); }
        }

        public static string PunchOutRESPFilePrefix
        {
            get { return ConfigurationManager.AppSettings["PunchOutRESPFilePrefix"].ToString(); }
        }

        public static string PunchOutRESPFolder
        {
            get { return ConfigurationManager.AppSettings["PunchOutRESPFolder"].ToString(); }

        }
        public static string PunchOutRESPDTDFilePrefix
        {
            get { return ConfigurationManager.AppSettings["PunchOutRESPDTDFilePrefix"].ToString(); }
        }

        public static string PunchOutRESPDTDFolder
        {
            get { return ConfigurationManager.AppSettings["PunchOutRESPDTDFolder"].ToString(); }

        }
        public static string PunchOutPassword
        {
            get { return ConfigurationManager.AppSettings["PunchOutPassword"].ToString(); }
        }

        public static string PunchOutOrgID
        {
            get { return ConfigurationManager.AppSettings["PunchOutOrgID"].ToString(); }

        }

        public static string ESPDistLogFolder
        {
            get { return ConfigurationManager.AppSettings["ESPDistLogFolder"].ToString(); }
        }

        public static string ESPDistLogFilePrefix
        {
            get { return ConfigurationManager.AppSettings["ESPDistLogFilePrefix"].ToString(); }
        }

        public static string ESPDistEnableTrace
        {
            get { return ConfigurationManager.AppSettings["ESPDistEnableTrace"].ToString(); }
        }

        public static string ESPDistEnableCheckCache
        {
            get { return ConfigurationManager.AppSettings["ESPDistEnableCheckCache"].ToString(); }
        }

        public static string Environment
        {
            get { return ConfigurationManager.AppSettings["Environment"].ToString(); }
        }

        #region Distributed Cache

        public static string ClusterServerPortString
        {
            get { return ConfigurationManager.AppSettings["ClusterServerPortString"]; }
        }

        public static string DefaultCacheName
        {
            get { return ConfigurationManager.AppSettings["DefaultCacheName"]; }
        }

        public static string DefaultCmCacheName
        {
            get { return ConfigurationManager.AppSettings["DefaultCmCacheName"]; }
        }

        public static string DefaultCsCacheName
        {
            get { return ConfigurationManager.AppSettings["DefaultCsCacheName"]; }
        }

        public static string DefaultCartCacheName
        {
            get { return ConfigurationManager.AppSettings["DefaultCartCacheName"]; }
        }

        public static string SystemNotificationCacheKey
        {
            get { return ConfigurationManager.AppSettings["SystemNotificationCacheKey"]; }
        }

        public static string DistributedCacheRegion
        {
            get { return ConfigurationManager.AppSettings["DistributedCacheRegion"]; }
        }

        #endregion

        #region SSO OATUH

        public static string SSOOAUTHLogFolder
        {
            get { return ConfigurationManager.AppSettings["SSOOAUTHLogFolder"].ToString(); }
        }

        public static string SSOOAUTHLogFilePrefix
        {
            get { return ConfigurationManager.AppSettings["SSOOAUTHLogFilePrefix"].ToString(); }
        }

        public static string SSOOAUTHEnableTrace
        {
            get { return ConfigurationManager.AppSettings["SSOOAUTHEnableTrace"].ToString(); }
        }

        public static string SSOOAUTHTS360LoginPage
        {
            get { return ConfigurationManager.AppSettings["SSOOAUTHTS360LoginPage"].ToString(); }
        }

        public static string SSOOAUTHSalesForcePage
        {
            get { return ConfigurationManager.AppSettings["SSOOAUTHSalesForcePage"].ToString(); }
        }

        public static string SSOOAUTHExpirationInDays
        {
            get { return ConfigurationManager.AppSettings["SSOOAUTHExpirationInDays"].ToString(); }
        }

        #endregion
    }
}