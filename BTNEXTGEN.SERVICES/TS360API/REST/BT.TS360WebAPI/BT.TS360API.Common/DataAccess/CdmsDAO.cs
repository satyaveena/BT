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
    public class CdmsDAO: BaseDAO
    {
        private static volatile CdmsDAO _instance;
        private static readonly object SyncRoot = new Object();

        private const char DEFAULT_ORDER_STATUS = 'A';

        private CdmsDAO()
        {
        }

        public static CdmsDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CdmsDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["OrderDbConnString"]; }
        }

        public DataSet GetAdditionalUsers(string cdmsListId, string keyword)
        {
            var dbConnection = CreateSqlConnection();
            //var command = CreateSqlSpCommandWithErrorHandling(StoreProcedureName.ProcTS360CDMSListSearchAdditionalUsers, dbConnection);
            var command = CreateSqlSpCommand(StoredProcedureName.ProcTS360CDMSListSearchAdditionalUsers, dbConnection);
            var sqlParamaters = CreateSqlParamaters(2);
            sqlParamaters[0] = new SqlParameter("@CDMSListID", cdmsListId);
            sqlParamaters[1] = new SqlParameter("@Keyword", keyword);
            command.Parameters.AddRange(sqlParamaters);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            dbConnection.Open();
            try
            {
                sqlDa.Fill(ds);
                //HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;        
        }

        public void SendCdmsList(string cdmsListId, string additionalUserIds, string checkedUserIds, bool allIndicator)
        {
            var dbConnection = CreateSqlConnection();
            //var command = CreateSqlSpCommand(StoreProcedureName.ProcTS360CDMSListSend, dbConnection);
            var command = CreateSqlSpCommandNoErrorMessage(StoredProcedureName.ProcTS360CDMSListSend, dbConnection);
            var sqlParamaters = CreateSqlParamaters(5);
            sqlParamaters[0] = new SqlParameter("@CDMSListID", cdmsListId);
            sqlParamaters[1] = new SqlParameter("@CDMSListAdditionalUsersID", additionalUserIds);
            sqlParamaters[2] = new SqlParameter("@CDMSListCheckedUsersID", checkedUserIds);
            sqlParamaters[3] = new SqlParameter("@AllIndicator", allIndicator);
            sqlParamaters[4] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            command.Parameters.AddRange(sqlParamaters);
            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();
                //HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public DataSet GetUserList(string cdmsListId, int pageSize, int pageIndex, string sortBy, string sortDirection)
        {
            var dbConnection = CreateSqlConnection();
            //var command = CreateSqlSpCommandWithErrorHandling(StoreProcedureName.ProcTs360CDMSListGetUsers, dbConnection);
            var command = CreateSqlSpCommand(StoredProcedureName.ProcTs360CDMSListGetUsers, dbConnection);
            var sqlParamaters = CreateSqlParamaters(5);
            var sortDirectionValue = sortDirection == QueryStringValue.SORT_ASC_TEXT ? 0 : 1;
            sqlParamaters[0] = new SqlParameter("@CDMSListID", cdmsListId);
            sqlParamaters[1] = new SqlParameter("@PageIndex", pageIndex);
            sqlParamaters[2] = new SqlParameter("@PageSize", pageSize);
            sqlParamaters[3] = new SqlParameter("@SortBy", sortBy);
            sqlParamaters[4] = new SqlParameter("@SortDirection", sortDirectionValue);
            command.Parameters.AddRange(sqlParamaters);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            dbConnection.Open();
            try
            {
                sqlDa.Fill(ds);
                //HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }
   
    }
}
