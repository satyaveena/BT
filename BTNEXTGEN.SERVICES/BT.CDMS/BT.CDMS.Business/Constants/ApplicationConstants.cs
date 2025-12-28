using System;

namespace BT.CDMS.Business.Constants
{
    /// <summary>
    /// Class ApplicationConstants
    /// </summary>
    public static class ApplicationConstants
    {
        #region Public Member
        public const String APPLICATION_NAME = "CDMS API";
        public const String APPLICATION_LOGDB = "ApplicationLog";
        public const String APPLICATION_AUTHKEY_COLLECTION = "ApplicationAuthKeys";
        public const String MONGO_COMMON_DB_NAME = "Common";
        public const String ELMAH_ERROR_SOURCE = "CDMS API";
        public const String LOG_TABLE_EXCEPTIONS = "ExceptionLog";
        public const String LOG_TABLE_INFORMATION = "InformationLog";
        #endregion

        #region Public Column
        public const String COLUMN_ORG_ID_ALIAS = "OrganizationID";
        public const String COLUMN_ORG_NAME_ALIAS = "OrganizationName";
        public const String COLUMN_USER_COUNT = "UserCount";
        public const String COLUMN_USER_ID = "u_user_id";
        public const String COLUMN_USER_NAME = "u_user_name";
        public const String COLUMN_USER_ALIAS = "u_user_alias";
        public const String COLUMN_GRID_TEMPLATE_ID = "GridTemplateID";
        public const String COLUMN_GRID_TEMPLATE_NAME = "GridTemplateName";
        public const String COLUMN_GRID_TEMPLATE_DESCRIPTION = "Description";
        public const String COLUMN_USER_ID_ALIAS = "UserID";
        #endregion
    }
}
