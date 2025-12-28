
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using Microsoft.SqlServer.Server;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace BT.TS360API.Common.DataAccess
{
    public class DashboardDAO : BaseDAO
    {
        private static volatile DashboardDAO _instance;
        private static readonly object SyncRoot = new Object();

        private DashboardDAO()
        {
        }

        public static DashboardDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new DashboardDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["NextGenProfilesConnectionString"]; }
        }

        private void HandleDashboardException(SqlCommand command)
        {
            var paramValue = command.Parameters["returnVal"].Value;
            var returnCode = -1;

            if (paramValue != null)
            {
                returnCode = DataAccessHelper.ConvertToInt(paramValue);
            }

            if (returnCode == 0 || returnCode == 1000)
                return;

            // Error
            paramValue = command.Parameters["@ErrorMessage"].Value;
            var errorMessage = paramValue != null ? paramValue.ToString() : "";

            var exception = new DAOException(errorMessage);

            if (errorMessage.Equals("Dashboard name must be unique.", StringComparison.CurrentCultureIgnoreCase)
                || errorMessage.Equals("Invalid user accounts.", StringComparison.CurrentCultureIgnoreCase)
                || errorMessage.Equals("Invalid dashboard type.", StringComparison.CurrentCultureIgnoreCase)
                || errorMessage.StartsWith("Dashboard does not exist", StringComparison.CurrentCultureIgnoreCase)
                || errorMessage.StartsWith("User does not exist", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                // is expected error. No need to write log.
                exception.IsBusinessError = true;
            }
            else
            {
                Logger.WriteLog(exception, ExceptionCategory.UserDashboard.ToString());
            }

            throw exception;
        }

        private int HandleDashboardExceptionWithReturnCode(SqlCommand command)
        {
            var paramValue = command.Parameters["returnVal"].Value;
            var returnCode = -1;

            if (paramValue != null)
            {
                returnCode = DataAccessHelper.ConvertToInt(paramValue);
            }

            if (returnCode == 0 || returnCode == 1000 || returnCode == 1)
                return returnCode;

            // Error
            paramValue = command.Parameters["@ErrorMessage"].Value;
            var errorMessage = paramValue != null ? paramValue.ToString() : "";

            var exception = new DAOException(errorMessage);

            if (errorMessage.Equals("Dashboard name must be unique.", StringComparison.CurrentCultureIgnoreCase)
                || errorMessage.Equals("Invalid user accounts.", StringComparison.CurrentCultureIgnoreCase)
                || errorMessage.Equals("Invalid dashboard type.", StringComparison.CurrentCultureIgnoreCase)
                || errorMessage.StartsWith("Dashboard does not exist", StringComparison.CurrentCultureIgnoreCase)
                || errorMessage.StartsWith("User does not exist", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                // is expected error. No need to write log.
                exception.IsBusinessError = true;
            }
            else
            {
                Logger.WriteLog(exception, ExceptionCategory.UserDashboard.ToString());
            }

            throw exception;
        }

        internal async Task<DashboardInfoResponse> SaveDashboard(DashboardInfo request)
        {
            bool retVal = false;
            var dbConnection = CreateSqlConnection();
            var command = new SqlCommand();
            command = CreateSqlSpCommand(StoredProcedureName.PROC_CUSTOMER_SUPPORT_USER_DASHBOARD_SAVE, dbConnection);            

            var sqlParameters = new SqlParameter[7];

            sqlParameters[0] = new SqlParameter("@DashboardID", SqlDbType.NVarChar, 50) { Value = request.DashboardId != null ? request.DashboardId : (object)DBNull.Value };

            // prevent account IDs from duplicated
            List<string> distinctIds = null;
            if (request.AccountIds != null && request.AccountIds.Count > 0)
                distinctIds = request.AccountIds.Distinct().ToList();
            var accountIds = DataConverter.GenerateDataRecords(distinctIds, "GUID", 50);

            sqlParameters[1] = new SqlParameter("@Accounts", SqlDbType.Structured) { Value = accountIds };
            sqlParameters[2] = new SqlParameter("@DashboardName", SqlDbType.VarChar, 50) { Value = request.Name };
            sqlParameters[3] = new SqlParameter("@DashboardType", SqlDbType.VarChar, 50) { Value = request.AccountType };
            sqlParameters[4] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Value = request.UserId };
            sqlParameters[5] = new SqlParameter("@DefaultDashboard", SqlDbType.Bit) { Value = request.IsDefault };
            sqlParameters[6] = new SqlParameter("@NewDashboardID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
            

            command.Parameters.AddRange(sqlParameters);

            try
            {
                await dbConnection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                HandleDashboardException(command);

                retVal = true;
            }
            finally
            {
                dbConnection.Close();
            }

            var result = new DashboardInfoResponse();
            result.DashboardId = sqlParameters[6].Value != null ? sqlParameters[6].Value.ToString() : null;
            result.ResponseStatus = retVal;

            return result;
        }

        internal async Task<bool> DeleteDashboard(string userId, string dashboardId)
        {
            bool retVal = false;
            var dbConnection = CreateSqlConnection();
            var command = new SqlCommand();
            
            command = CreateSqlSpCommand(StoredProcedureName.PROC_CUSTOMER_SUPPORT_USER_DASHBOARD_DELETE, dbConnection);
           
            var sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Value = userId };
            sqlParameters[1] = new SqlParameter("@DashboardID", SqlDbType.NVarChar, 50) { Value = dashboardId };
            

            command.Parameters.AddRange(sqlParameters);

            try
            {
                await dbConnection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                HandleDashboardException(command);

                retVal = true;
            }
            finally
            {
                dbConnection.Close();
            }

            return retVal;
        }

        internal async Task<DataSet> GetUserDashboard(string userId, string dashboardId)
        {
            var dbConnection = CreateSqlConnection();
            var command = new SqlCommand();

            command = CreateSqlSpCommand(StoredProcedureName.PROC_CUSTOMER_SUPPORT_USER_DASHBOARD_GET, dbConnection);

            var sqlParameters = new SqlParameter[2];

            sqlParameters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Value = userId };
            sqlParameters[1] = new SqlParameter("@DashboardID", SqlDbType.NVarChar, 50) { Value = dashboardId };


            command.Parameters.AddRange(sqlParameters);

            var da = new SqlDataAdapter(command);
            var ds = new DataSet();

            try
            {                
                await dbConnection.OpenAsync();

                await Task.Run(() => da.Fill(ds));

                HandleDashboardException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
        }

        internal async Task<DashboardCreateResponse> CreateDefaultDashboards(string userId)
        {
            var dashboardCreateResponse = new DashboardCreateResponse();
            var errorMessage = "";
            var dbConnection = CreateSqlConnection();
            var command = new SqlCommand();
            command = CreateSqlSpCommand(StoredProcedureName.PROC_CUSTOMER_SUPPORT_USER_DASHBOARD_GET_DEFAULT, dbConnection);

            var sqlParameters = new SqlParameter[1];

            sqlParameters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Value = userId };

            command.Parameters.AddRange(sqlParameters);

            var da = new SqlDataAdapter(command);
            var ds = new DataSet();

            try
            {
                await dbConnection.OpenAsync();

                await Task.Run(() => da.Fill(ds));

                var returnCode = HandleDashboardExceptionWithReturnCode(command);
                if (returnCode == 1)
                {
                    errorMessage = "NoAccountsFound";
                }
            }
            finally
            {
                dbConnection.Close();
            }
            dashboardCreateResponse.DataSet = ds;
            dashboardCreateResponse.ErrorMessage = errorMessage;
            return dashboardCreateResponse;
        }

        internal List<Dashboard> FindUserDashboards(DashboardSearchRequest request)
        {
            var dbConnection = CreateSqlConnection();
            var command = new SqlCommand();

            command = CreateSqlSpCommand(StoredProcedureName.PROC_CUSTOMER_SUPPORT_USER_DASHBOARD_FINDDASHBOARDS, dbConnection);

            var sqlParameters = new SqlParameter[4];

            sqlParameters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Value = request.UserId };
            sqlParameters[1] = new SqlParameter("@Keyword", SqlDbType.VarChar, 50) { Value = request.Keyword };
            sqlParameters[2] = new SqlParameter("@PageIndex", SqlDbType.Int) { Value = request.PageIndex };
            sqlParameters[3] = new SqlParameter("@PageSize", SqlDbType.Int) { Value = request.PageSize };
            
            command.Parameters.AddRange(sqlParameters);
            
            var da = new SqlDataAdapter(command);
            var ds = new DataSet();
            dbConnection.Open();
            try
            {
                da.Fill(ds);
            }
            finally
            {
                dbConnection.Close();
            }

            List<Dashboard> lstDashboards = new List<Dashboard>();

            if (ds != null && ds.Tables!= null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var reviewSourceDt = ds.Tables[0];
                if (reviewSourceDt != null && reviewSourceDt.Rows.Count > 0)
                {
                    foreach (DataRow dr in reviewSourceDt.Rows)
                    {
                        lstDashboards.Add(new Dashboard()
                        {
                            DashboardType = DataAccessHelper.ConvertToString(dr["DashboardType"]),
                            DashboardId = DataAccessHelper.ConvertToString(dr["DashboardID"]),
                            DashboardName = DataAccessHelper.ConvertToString(dr["DashboardName"])
                        });
                    }
                }
            }

            return lstDashboards;
        }
    }
}
