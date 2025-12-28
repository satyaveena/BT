using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.DataAccess
{
    public class AccountDAO : BaseDAO
    {

        private static volatile AccountDAO _instance;
        private static readonly object SyncRoot = new Object();

        private AccountDAO()
        { // prevent init object outside
        }

        public static AccountDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new AccountDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["NextGenProfilesConnectionString"]; }
        }

        /// <summary>
        /// Get Org Account Types by orgId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal async Task<DataSet> GetUserAccountTypes(string userId)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                using (var command = CreateSqlSpCommandNoErrorMessage(StoredProcedureName.PROC_CUSTOMER_SUPPORT_USERDASHBOARD_GETUSERACCOUNT_TYPES, dbConnection))
                {
                    var sqlParamater = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
                    command.Parameters.Add(sqlParamater);
                    var paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(paramErrorMessage);

                    var da = new SqlDataAdapter(command);
                    var ds = new DataSet();
                    await dbConnection.OpenAsync();
                    await Task.Run(() => da.Fill(ds));
                    //HandleException(command);

                    return ds;
                }
            }
        }

        internal async Task<DataSet> GetAccountsByAccountType(string userId, string accountType, int pageIndex, int pageSize)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                using (var command = CreateSqlSpCommand(StoredProcedureName.PROC_CUSTOMER_SUPPORT_USERDASHBOARD_GETACCOUNTS_BY_ACCOUNTTYPE, dbConnection))
                {
                    var sqlParamaters = CreateSqlParamaters(4);
                    sqlParamaters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
                    sqlParamaters[1] = new SqlParameter("@AccountType", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = accountType };
                    sqlParamaters[2] = new SqlParameter("@PageIndex", SqlDbType.Int) { Value = pageIndex };
                    sqlParamaters[3] = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };

                    command.Parameters.AddRange(sqlParamaters);

                    var da = new SqlDataAdapter(command);
                    var ds = new DataSet();
                    await dbConnection.OpenAsync();

                    await Task.Run(() => da.Fill(ds));

                    //HandleException(command);

                    return ds;
                }
            }
        }

        internal async Task<DataSet> GetAccountsByDashboardId(int dashboardId)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                using (var command = CreateSqlSpCommand(StoredProcedureName.PROC_CUSTOMER_SUPPORT_USERDASHBOARD_GETACCOUNTS_BY_DASHBOARD_ID, dbConnection))
                {
                    var sqlParamater = new SqlParameter("@DashboardID", SqlDbType.Int, 50) { Direction = ParameterDirection.Input, Value = dashboardId };
                    command.Parameters.Add(sqlParamater);
                    
                    var da = new SqlDataAdapter(command);
                    var ds = new DataSet();
                    await dbConnection.OpenAsync();

                    await Task.Run(() => da.Fill(ds));
                   
                    HandleException(command);
                    return ds;

                }
            }
        }

    }
}

