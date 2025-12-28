using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360Constants;
using System.Linq;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using Microsoft.SqlServer.Server;
using BT.TS360API.Logging;

namespace BT.TS360API.Common.DataAccess
{
    public class ProfileDAO : BaseDAO
    {
        private static volatile ProfileDAO _instance;
        private static readonly object SyncRoot = new Object();

        private ProfileDAO()
        { // prevent init object outside
        }

        public static ProfileDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProfileDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["NextGenProfilesConnectionString"]; }
        }

        public List<Warehouse> GetWareHouses()
        {
            var wareHouses = new List<Warehouse>();
            var dbConnection = CreateSqlConnection();
            
            var command = CreateSqlSpCommandNoErrorMessage(DBStores.CONST_GET_WAREHOUSES, dbConnection);            
            
            try
            {
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //SqlDataReader reader = await taskReader;
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var result = new Warehouse
                            {
                                Id = DataAccessHelper.ConvertToString(reader["u_warehouse_id"]),
                                Code = DataAccessHelper.ConvertToString(reader["u_warehouse_erp_code"]),
                                Description = DataAccessHelper.ConvertToString(reader["u_warehouse_description"]),
                                DateCreated = DataAccessHelper.ConvertToDateTime(reader["dt_date_created"])
                            };
                            wareHouses.Add(result);
                        }
                    }
                }
            }
            finally
            {
                dbConnection.Close();
            }

            return wareHouses;
        }

        public List<BTProductInterestGroup> GetProductInterestGroup()
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var result = new List<BTProductInterestGroup>();
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.BTNG_ProductInterestGroup, dbConnection);

                //Open Connection
                dbConnection.Open();

                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //
                        while (reader.Read())
                        {
                            var pigObj = new BTProductInterestGroup
                            {
                                Id = DataAccessHelper.ConvertToString(reader["ID"]),
                                PIGName = DataAccessHelper.ConvertToString(reader["Name"]),
                                Description = DataAccessHelper.ConvertToString(reader["Description"]),
                                MarketTypeList = DataAccessHelper.ConvertToStringArray(reader["MarketType"].ToString()),
                                ProductTypeList = DataAccessHelper.ConvertToStringArray(reader["ProductType"].ToString())
                            };

                            result.Add(pigObj);
                        }
                    }
                }

                return result;
            }
        }

        public async Task<MyPreferencesProfile> GetUserPreferenceById(string userId)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var result = new MyPreferencesProfile();
                var command = CreateSqlSpCommand(DBStores.CONST_GET_USER_PREFERENCE, dbConnection);

                var sqlParamaters = CreateSqlParamaters(1);
                sqlParamaters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50);
                sqlParamaters[0].Direction = ParameterDirection.Input;
                sqlParamaters[0].Value = userId;
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);

                try
                {
                   await dbConnection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                result.UserID = userId;
                                result.DisplayQuotationDisclaimer = DataAccessHelper.ConvertToBool(reader["b_display_quotation_disclaimer"]);
                                result.IsInitialDisplayQuotationDisclaimer = reader["b_display_quotation_disclaimer"] == DBNull.Value;
                                result.CIPUserToken = DataAccessHelper.ConvertToString(reader["CIPUserToken"]);
                                result.CIPLastLoginDateTime = DataAccessHelper.ConvertToDateTime(reader["CIPLastLoginDateTime"]);
                                result.ESPPortalUserToken = DataAccessHelper.ConvertToString(reader["ESPUserToken"]);
                                result.OCSUserToken = DataAccessHelper.ConvertToString(reader["u_ocs_token"]);
                            }
                        }
                    }
                }
                finally
                {
                    dbConnection.Close();
                }

                return result;
            }
        }

        public async Task<bool> SetTSSONotificationCartUsers(List<string> addedUsers, List<string> removedUsers)
        {
            bool retVal = false;

            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(DBStores.CONST_SET_NOTIFICATION_CART_USERS, dbConnection);
                //
                var sqlParamaters = CreateSqlParamaters(2);
                sqlParamaters[0] = CreateTableParameter("@UsersAdded", "utblCSGuids",
                                                    ConvertToListStringArgumentTable(addedUsers, CreateGUIDArgumentTable()));
                sqlParamaters[1] = CreateTableParameter("@UsersRemoved", "utblCSGuids",
                                                    ConvertToListStringArgumentTable(removedUsers, CreateGUIDArgumentTable()));

                command.Parameters.AddRange(sqlParamaters);
                //
                try
                {
                    await dbConnection.OpenAsync();

                    await command.ExecuteNonQueryAsync();

                    retVal = true;
                }
                finally
                {
                    dbConnection.Close();
                }

                return retVal;
            }
        }

        public async Task<bool> SetUserNRCProductTypes(string userID, string NRCProductTypes)
        {
            bool retVal = false;

            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(DBStores.CONST_SET_USER_NRC_PRODUCT_TYPES, dbConnection);
                //
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@UserID", userID));
                sqlParameters.Add(new SqlParameter("@NRCProductTypes", NRCProductTypes));


                command.Parameters.AddRange(sqlParameters.ToArray());
                //
                try
                {
                    await dbConnection.OpenAsync();

                    await command.ExecuteNonQueryAsync();

                    retVal = true;
                }
                finally
                {
                    dbConnection.Close();
                }

                return retVal;
            }
        }

        public void SaveILSDetail(string orgId, string ilsLogin, string ilsUrl, string ilsKey, string ilsSecret, int validationStatusId, string validationLogError, DateTime? validationDateTime, string ilsType, string ilsUserDomain, string ilsUserAccount)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_SAVE_ILS_CONFIGURATION, dbConnection);
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@ILSVendorID", ilsType));
            sqlParameters.Add(new SqlParameter("@OrgID", orgId));
            sqlParameters.Add(new SqlParameter("@ILSLogin", ilsLogin));
            sqlParameters.Add(new SqlParameter("@ILSAcquisitionsApiURL", ilsUrl));
            sqlParameters.Add(new SqlParameter("@ILSAcquisitionsApiKey", ilsKey));
            sqlParameters.Add(new SqlParameter("@ILSAcquisitionsApiPassphrase", ilsSecret));
            sqlParameters.Add(new SqlParameter("@ILSValidationMessageId", validationStatusId));
            sqlParameters.Add(new SqlParameter("@ILSValidationUpdatedDateTime", validationDateTime));
            sqlParameters.Add(new SqlParameter("@ILSValidationLogError", string.IsNullOrEmpty(validationLogError)?string.Empty:validationLogError));
           
            if (string.Equals(ilsType, ILSVendorType.Polaris))
            {
                sqlParameters.Add(new SqlParameter("@ILSUserDomain", ilsUserDomain));
                sqlParameters.Add(new SqlParameter("@ILSUserAccount", ilsUserAccount));
            }
            else
            {
                sqlParameters.Add(new SqlParameter("@ILSUserDomain", string.Empty));
                sqlParameters.Add(new SqlParameter("@ILSUserAccount", string.Empty));
            }

            command.Parameters.AddRange(sqlParameters.ToArray());

            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public void SaveILSDetail(string orgId, string ilsLogin, string ilsUrl, string ilsKey, string ilsSecret, int validationStatusId, string validationLogError, DateTime? validationDateTime)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_SAVE_ILS_CONFIGURATION, dbConnection);

            //<Parameter>
            SqlParameter[] sqlParameters = {
                                                new SqlParameter("@OrgID", orgId),
                                                new SqlParameter("@ILSLogin", ilsLogin),
                                                new SqlParameter("@ILSAcquisitionsApiURL", ilsUrl),
                                                new SqlParameter("@ILSAcquisitionsApiKey", ilsKey),
                                                new SqlParameter("@ILSAcquisitionsApiPassphrase", ilsSecret),
                                                new SqlParameter("@ILSValidationMessageId", validationStatusId),
                                                new SqlParameter("@ILSValidationUpdatedDateTime", validationDateTime),
                                                new SqlParameter("@ILSValidationLogError", string.IsNullOrEmpty(validationLogError)?string.Empty:validationLogError),
                                          };
            command.Parameters.AddRange(sqlParameters);

            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public ILSValidationRequest GetILSConfiguration(string orgId)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var result = new List<Organization>();
                var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_GET_ILS_CONFIGURATION, dbConnection);
                //<Parameter>
                var sqlParamaters = CreateSqlParamaters(1);
                sqlParamaters[0] = new SqlParameter("@OrgId", orgId);
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                //Open Connection
                ILSValidationRequest orgObj = null;

                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //
                        while (reader.Read())
                        {
                            orgObj = new ILSValidationRequest
                            {
                                TSOrgId = DataAccessHelper.ConvertToString(reader["u_org_id"]),
                                ILSUrl = DataAccessHelper.ConvertToString(reader["u_ILS_acquisitions_api_url"]),
                                ILSApiKey = DataAccessHelper.ConvertToString(reader["u_ILS_acquisitions_api_key"]),
                                ILSApiSecret = DataAccessHelper.ConvertToString(reader["u_ILS_acquisitions_api_passphrase"]),
                                ILSLogin = DataAccessHelper.ConvertToString(reader["u_ILS_login"]),
                                ILSValidationStatus = DataAccessHelper.ConvertToString(reader["ILSValidationStatusLiteral"]),
                                ILSValidationStatusId = DataAccessHelper.ConvertToInt(reader["ILSValidationStatusID"]),
                                ILSValidationDateTime = DataAccessHelper.ConvertToDateTime(reader["ILSValidationUpdatedDateTime"]),
                                ILSValidationErrorMessage = DataAccessHelper.ConvertToString(reader["ILSValidationLogError"]),
                                ILSUserAccount = DataAccessHelper.ConvertToString(reader["u_ils_user_account"]),
                                ILSUserDomain = DataAccessHelper.ConvertToString(reader["u_ils_user_domain"])
                            };

                        }
                    }
                }

                dbConnection.Close();
                return orgObj;
            }
        }

        public async Task<string> GeteSupplierAccountNumber(string userId, string basketSummaryId)
        {
            string retVal = "";

            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(DBStores.procTS360GeteSupplierAccountNumber, dbConnection);
                //
                var sqlParamaters = CreateSqlParamaters(3);
                sqlParamaters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50);
                sqlParamaters[0].Direction = ParameterDirection.Input;
                sqlParamaters[0].Value = userId;

                sqlParamaters[1] = new SqlParameter("@BasketSummaryID ", SqlDbType.NVarChar, 50);
                sqlParamaters[1].Direction = ParameterDirection.Input;
                if (string.IsNullOrEmpty(basketSummaryId))
                    sqlParamaters[1].Value = DBNull.Value;
                else
                    sqlParamaters[1].Value = basketSummaryId;

                sqlParamaters[2] = new SqlParameter("@eSupplierAccountNumber", SqlDbType.NVarChar, 12);
                sqlParamaters[2].Direction = ParameterDirection.Output;

                command.Parameters.AddRange(sqlParamaters);
                //
                try
                {
                    await dbConnection.OpenAsync();

                    await command.ExecuteNonQueryAsync();
                    if (command.Parameters["@eSupplierAccountNumber"].Value != DBNull.Value)
                    {
                        retVal = command.Parameters["@eSupplierAccountNumber"].Value.ToString();
                    }
                }
                finally
                {
                    dbConnection.Close();
                }

                return retVal;
            }
        }

        public List<string> GetUserAccounts(string userId)
        {
            var result = new List<string>();
            var dbConnection = CreateSqlConnection();

            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_GET_USER_SHIP_TO_ACCOUNTS, dbConnection);
            //<Parameter>
            var sqlParamaters = CreateSqlParamaters(1);
            sqlParamaters[0] = new SqlParameter("@UserID", userId);
            //</Parameter>
            command.Parameters.AddRange(sqlParamaters);

            try
            {
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //SqlDataReader reader = await taskReader;
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(DataAccessHelper.ConvertToString(reader["u_erp_account_number"]));
                        }
                    }
                }
            }
            finally
            {
                dbConnection.Close();
            }

            return result;
        }

        private SqlParameter CreateTableParameter(string parameterName, string parameterTypeName, DataTable value)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = SqlDbType.Structured,
                TypeName = parameterTypeName,
                Value = value
            };
        }

        private DataTable ConvertToListStringArgumentTable(IEnumerable<string> items, DataTable dt)
        {
            if (items != null)
            {
                var list = items.ToList();
                if (list.Any())
                    list.ForEach(r => dt.Rows.Add(r));
            }
            return dt;
        }

        private DataTable ConvertToListBigIntArgumentTable(IEnumerable<Int64> items, DataTable dt)
        {
            if (items != null)
            {
                var list = items.ToList();
                if (list.Any())
                    list.ForEach(r => dt.Rows.Add(r));
            }
            return dt;
        }

        private DataTable CreateGUIDArgumentTable()
        {
            var dt = new DataTable("utblCSGuids");
            dt.Columns.Add("GUID", typeof(string));
            return dt;
        }

        private DataTable CreateVendorCodeArgumentTable()
        {
            var dt = new DataTable("utbVendorCodes");
            dt.Columns.Add("VendorCode", typeof(string));
            return dt;
        }

        private DataTable CreateILSOrderingCodeArgumentTable()
        {
            var dt = new DataTable("utblILSOrderingCodes");
            dt.Columns.Add("OrderingCode", typeof(string));
            return dt;
        }
        private DataTable CreateBigIntsArgumentTable()
        {
            var dt = new DataTable("utblBigInts");
            dt.Columns.Add("ID", typeof(Int64));
            return dt;
        }

        public List<VendorCode> GetIlsVendorCodes(string orgId, string searchkeyword, int pageIndex, int pageSize, int sortDirection, out int lastResultset)
        {
            List<VendorCode> codeList = new List<VendorCode>();
            lastResultset = 0;
            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_GET_ILS_VENDORCODES, dbConnection);
                //<Parameter>
                var sqlParamaters = CreateSqlParamaters(6);
                sqlParamaters[0] = new SqlParameter("@OrganizationID", orgId);
                sqlParamaters[1] = new SqlParameter("@Keyword", searchkeyword);
                sqlParamaters[2] = new SqlParameter("@PageIndex", pageIndex);
                sqlParamaters[3] = new SqlParameter("@PageSize", pageSize);
                sqlParamaters[4] = new SqlParameter("@SortDirection ", sortDirection);
                sqlParamaters[5] = new SqlParameter("@LargeResultSet", SqlDbType.Int, -1) { Direction = ParameterDirection.Output };
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                //Open Connection
              
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //
                        while (reader.Read())
                        {
                            var codeObj = new VendorCode
                            {
                                Code = DataAccessHelper.ConvertToString(reader["VendorCode"]),
                                Id = DataAccessHelper.ConvertToInt(reader["ILSVendorCodeID"]),
                            };
                            codeList.Add(codeObj);
                        }
                    }
                }
                var paramValue = command.Parameters["@LargeResultSet"].Value;
                if (paramValue != null)
                {
                    lastResultset = DataAccessHelper.ConvertToBool(paramValue) ? 0 : 1;
                }


                dbConnection.Close();

            }

            return codeList;
        }

        public List<VendorCode> GetIlsCodes(string orgId, string searchkeyword, int pageIndex, int pageSize, int sortDirection, int ilsOrderingType, string vendorID, out int lastResultset)
        {
            List<VendorCode> codeList = new List<VendorCode>();
            lastResultset = 0;
            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_GET_ILS_ORDERINGCODES, dbConnection);
                //<Parameter>
                var sqlParamaters = CreateSqlParamaters(8);
                sqlParamaters[0] = new SqlParameter("@OrganizationID", orgId);
                sqlParamaters[1] = new SqlParameter("@Keyword", searchkeyword);
                sqlParamaters[2] = new SqlParameter("@PageIndex", pageIndex);
                sqlParamaters[3] = new SqlParameter("@PageSize", pageSize);
                sqlParamaters[4] = new SqlParameter("@SortDirection ", sortDirection);
                sqlParamaters[5] = new SqlParameter("@ILSOrderingTypeID ", ilsOrderingType);
                sqlParamaters[6] = new SqlParameter("@VendorID ", vendorID);
                sqlParamaters[7] = new SqlParameter("@LargeResultSet", SqlDbType.Int, -1) { Direction = ParameterDirection.Output };
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                //Open Connection

                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //
                        while (reader.Read())
                        {
                            var codeObj = new VendorCode
                            {
                                Code = DataAccessHelper.ConvertToString(reader["OrderingCode"]),
                                Id = DataAccessHelper.ConvertToInt(reader["ILSOrderingCodeID"]),
                            };
                            codeList.Add(codeObj);
                        }
                    }
                }
                var paramValue = command.Parameters["@LargeResultSet"].Value;
                if (paramValue != null)
                {
                    lastResultset = DataAccessHelper.ConvertToBool(paramValue) ? 0 : 1;
                }


                dbConnection.Close();

            }

            return codeList;
        }
        public List<VendorCode> AddIlsCodes(string orgId, string userId, List<string> codeList, int isImport, int orderingType, string vendorID)
        {
            List<VendorCode> resultList = new List<VendorCode>();

            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_SET_ILS_ORDERINGCODES, dbConnection);
                //<Parameter>
                var sqlParamaters = CreateSqlParamaters(7);
                sqlParamaters[0] = new SqlParameter("@OrganizationID", orgId);
                sqlParamaters[1] = new SqlParameter("@UserID", userId);
                sqlParamaters[2] = CreateTableParameter("@utblILSOrderingCodes", "utblILSOrderingCodes", ConvertToListStringArgumentTable(codeList, CreateILSOrderingCodeArgumentTable()));
                sqlParamaters[3] = new SqlParameter("@Overwrite", isImport);
                sqlParamaters[4] = new SqlParameter("@ILSOrderingTypeID ", orderingType);
                sqlParamaters[5] = new SqlParameter("@ILSVendorID", vendorID);
                sqlParamaters[6] = new SqlParameter("@AffectedRecordCount", SqlDbType.Int, -1) { Direction = ParameterDirection.Output };
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //
                        while (reader.Read())
                        {
                            var codeObj = new VendorCode
                            {
                                Code = DataAccessHelper.ConvertToString(reader["OrderingCode"]),
                                Id = DataAccessHelper.ConvertToInt(reader["ILSOrderingCodeID"]),
                            };
                            resultList.Add(codeObj);
                        }
                    }
                }

                //handle exception
                var paramValue = command.Parameters["returnVal"].Value;
                var returnCode = -1;
                if (paramValue != null)
                {
                    returnCode = DataAccessHelper.ConvertToInt(paramValue);
                }

                if (returnCode == -1)
                {
                    paramValue = command.Parameters["@ErrorMessage"].Value;
                    var errorMessage = paramValue != null ? paramValue.ToString() : "";
                    throw new Exception(errorMessage);
                }

                dbConnection.Close();
            }

            return resultList;
        }

        public List<VendorCode> AddIlsVendorCodes(string orgId, string userId, List<string> codeList, int isImport)
        {
            List<VendorCode> resultList = new List<VendorCode>();

            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_INSERT_ILS_VENDORCODES, dbConnection);
                //<Parameter>
                var sqlParamaters = CreateSqlParamaters(5);
                sqlParamaters[0] = new SqlParameter("@OrganizationID", orgId);
                sqlParamaters[1] = new SqlParameter("@UserID", userId);
                sqlParamaters[2] = CreateTableParameter("@utblVendorCodes", "utblVendorCodes", ConvertToListStringArgumentTable(codeList, CreateVendorCodeArgumentTable()));
                sqlParamaters[3] = new SqlParameter("@Overwrite", isImport);
                sqlParamaters[4] = new SqlParameter("@AffectedRecordCount", SqlDbType.Int, -1) { Direction = ParameterDirection.Output };
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //
                        while (reader.Read())
                        {
                            var codeObj = new VendorCode
                            {
                                Code = DataAccessHelper.ConvertToString(reader["VendorCode"]),
                                Id = DataAccessHelper.ConvertToInt(reader["ILSVendorCodeID"]),
                            };
                            resultList.Add(codeObj);
                        }
                    }
                }

                //handle exception
                var paramValue = command.Parameters["returnVal"].Value;
                var returnCode = -1;
                if (paramValue != null)
                {
                    returnCode = DataAccessHelper.ConvertToInt(paramValue);
                }

                if (returnCode == -1)
                {
                    paramValue = command.Parameters["@ErrorMessage"].Value;
                    var errorMessage = paramValue != null ? paramValue.ToString() : "";
                    throw new Exception(errorMessage);
                }

                dbConnection.Close();
            }

            return resultList;
        }

        public void DeleteIlsVendorCodes(List<Int64> idList)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_DELETE_ILS_VENDORCODES, dbConnection);
                //<Parameter>
                var sqlParamaters = CreateSqlParamaters(1);
                sqlParamaters[0] = CreateTableParameter("@ILSVendorCodesToDelete", "utblBigInts", ConvertToListBigIntArgumentTable(idList, CreateBigIntsArgumentTable()));
                
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                dbConnection.Open();
                command.ExecuteNonQuery();
                //handle exception
                var paramValue = command.Parameters["returnVal"].Value;
                var returnCode = -1;
                if (paramValue != null)
                {
                    returnCode = DataAccessHelper.ConvertToInt(paramValue);
                }

                if (returnCode == -1)
                {
                    paramValue = command.Parameters["@ErrorMessage"].Value;
                    var errorMessage = paramValue != null ? paramValue.ToString() : "";
                    throw new Exception(errorMessage);
                }
                
                dbConnection.Close();
            }
        }

        public void DeleteIlsCodes(List<Int64> idList)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_DELETE_ILS_ORDERINGCODES, dbConnection);
                //<Parameter>
                var sqlParamaters = CreateSqlParamaters(1);
                sqlParamaters[0] = CreateTableParameter("@ILSOrderingCodeToDelete", "utblBigInts", ConvertToListBigIntArgumentTable(idList, CreateBigIntsArgumentTable()));

                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                dbConnection.Open();
                command.ExecuteNonQuery();
                //handle exception
                var paramValue = command.Parameters["returnVal"].Value;
                var returnCode = -1;
                if (paramValue != null)
                {
                    returnCode = DataAccessHelper.ConvertToInt(paramValue);
                }

                if (returnCode == -1)
                {
                    paramValue = command.Parameters["@ErrorMessage"].Value;
                    var errorMessage = paramValue != null ? paramValue.ToString() : "";
                    throw new Exception(errorMessage);
                }

                dbConnection.Close();
            }
        }
    }
}
