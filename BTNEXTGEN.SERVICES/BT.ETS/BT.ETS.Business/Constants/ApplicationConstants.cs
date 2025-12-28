using System;
using System.Collections.Generic;

namespace BT.ETS.Business.Constants
{
    /// <summary>
    /// Class ApplicationConstants
    /// </summary>
    public static class ApplicationConstants
    {
        #region Public Member
        public const String APPLICATION_NAME_ETS_BACKGROUND = "ETS Background Service";
        public const String APPLICATION_NAME = "ETS API";
        public const String APPLICATION_LOGDB = "ApplicationLog";
        public const String APPLICATION_AUTHKEY_COLLECTION = "ApplicationAuthKeys";
        public const String MONGO_COMMON_DB_NAME = "Common";
        public const String MONGO_ORDERS_DB_Name = "Orders";
        public const String ELMAH_ERROR_SOURCE = "ETS API";
        public const String ELMAH_ERROR_SOURCE_ETS_BACKGROUND = "ETS Background Service";
        public const String LOG_TABLE_EXCEPTIONS = "ExceptionLog";
        public const String LOG_TABLE_INFORMATION = "InformationLog";
        public const String MONGO_ETS_QUEUE_COLLECTION_NAME = "ETSQueue";
        public const String MONGO_ETS_QUEUE_ITEMS_COLLECTION_NAME = "ETSQueueItems";
        public const String MONGO_DUP_CHECK_COLLECTION_NAME = "DupeCheck";
        public const String ETS_JOB_CART_RECEIVED = "CartReceived";
        public const String ETS_JOB_DUPECHECK = "DupeCheck";
        public const String ETS_JOB_PRICING = "Pricing";
        public const String ELMAH_MONGODB = "ApplicationLog";
        #endregion

        #region Public Column
        public const String COLUMN_ORG_ID = "u_org_id";
        public const String COLUMN_ORG_NAME = "u_name";
        public const String COLUMN_ORG_ESP_RANKING_FLAG = "b_esp_ranking";
        public const String COLUMN_ORG_ESP_DISTRIBUTION_FLAG = "b_esp_distribution";
        public const String COLUMN_ORG_ESP_DATE = "ESPEnabledDate";
        public const String COLUMN_ORG_ESP_LIBRARY_ID = "ESPCollectionHQLibraryID";

        public const String COLUMN_USER_ID = "u_user_id";
        public const String COLUMN_USER_NAME = "u_user_name";
        public const String COLUMN_USER_ALIAS = "u_user_alias";
        public const String COLUMN_GRID_TEMPLATE_ID = "GridTemplateID";
        public const String COLUMN_GRID_TEMPLATE_NAME = "GridTemplateName";
        public const String COLUMN_GRID_TEMPLATE_DESCRIPTION = "Description";
        public const String COLUMN_USER_ID_ALIAS = "UserID";
        #endregion

        #region DupCheck Statuses
        public static List<string> DupCheckCStatus = new List<string> { "default", "mycarts", "allcarts", "none" };
        public static List<string> DupCheckOStatus = new List<string> { "default", "myaccounts", "allaccounts", "none" };
        public static List<string> DupCheckHStatus = new List<string> { "default", "againstmyholdings", "againstorganizationholdings", "none" };
        #endregion

        #region DupCheck Prefernces
        public static List<string> DupCheckCPreference = new List<string> { "mycarts", "allcarts" };
        public static List<string> DupCheckOPreference = new List<string> { "myaccounts", "allaccounts" };
        #endregion

        #region DupCheck Download Cart Types
        public static List<string> DupCheckDownloadCartType = new List<string> { "includeworders", "includewcarts" };
        #endregion
    }


}
