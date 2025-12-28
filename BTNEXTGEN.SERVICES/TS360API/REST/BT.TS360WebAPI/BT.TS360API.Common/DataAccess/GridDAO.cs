using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360Constants;
using Microsoft.SqlServer.Server;

namespace BT.TS360API.Common.DataAccess
{
    public class GridDAO : BaseDAO
    {
        private static volatile GridDAO _instance;
        private static readonly object SyncRoot = new Object();

        private GridDAO()
        {
        }

        public static GridDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new GridDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["OrderDbConnString"]; }
        }

        public string GetDefaultGridTemplateId(string userId, string cartId)
        {
            var defaultGridTplId = "";

            var conn = CreateSqlConnection();
            var cmd = CreateSqlSpCommand("procTS360GetDefaultGridTempalteID", conn);

            var sqlParameterUser = new SqlParameter("@u_user_id", userId);
            var paramCartId = new SqlParameter("@BasketSummaryID", cartId);
            var paramDefaultGridTplId = new SqlParameter("@DefaultGridTemplateID", SqlDbType.NVarChar, 50);
            paramDefaultGridTplId.Direction = ParameterDirection.Output;

            cmd.Parameters.AddRange(new SqlParameter[] { sqlParameterUser, paramCartId, paramDefaultGridTplId });
            conn.Open();

            try
            {
                cmd.ExecuteNonQuery();

                if (paramDefaultGridTplId.Value != null && paramDefaultGridTplId.Value != DBNull.Value)
                {
                    defaultGridTplId = paramDefaultGridTplId.Value.ToString();
                }
                HandleCartGridException(cmd);
            }
            finally
            {
                conn.Close();
            }

            return defaultGridTplId;
        }

        public DataSet GetGridTemplateLines(string gridTemplateId)
        {
            var conn = CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_GET_GRID_TEMPLATE_LINES, conn);

            var sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@GridTemplateID", gridTemplateId);

            cmd.Parameters.AddRange(sqlParameter);
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            conn.Open();
            try
            {
                da.Fill(ds);
                HandleCartGridException(cmd);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        public DataSet GetGridFieldsByUser(string userId)
        {
            var conn = this.CreateSqlConnection();
            var cmd = this.CreateSqlSpCommand(StoredProcedureName.PROC_GET_GRID_FIELD_BY_USER, conn);

            var sqlParameter = new SqlParameter("@u_user_id", userId);

            cmd.Parameters.Add(sqlParameter);

            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            conn.Open();
            try
            {
                da.Fill(ds);
                HandleCartGridException(cmd);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        public DataSet GetUserPreference(string userId)
        {
            var conn = this.CreateSqlConnection();
            var cmd = this.CreateSqlSpCommand(StoredProcedureName.PROC_GET_USER_DEFAULT_GRID_TEMPLATE, conn);

            //var sqlParameter = new SqlParameter("@u_user_id", UserId);
            var sqlParameter = new SqlParameter("@u_user_id", userId);

            cmd.Parameters.Add(sqlParameter);
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            conn.Open();
            try
            {
                da.Fill(ds);
                HandleCartGridException(cmd);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }
        
        public DataSet GetUserGridFieldsCodes(string userId, string orgId, out int defaultQuantity)
        {
            defaultQuantity = 0;
            var conn = this.CreateSqlConnection();
            var cmd = this.CreateSqlSpCommand(StoredProcedureName.PROC_GET_GRID_FIELD_GRID_CODE_BY_USER, conn);

            var sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@u_org_id", orgId);
            sqlParameter[1] = new SqlParameter("@u_user_id", userId);
            sqlParameter[2] = new SqlParameter("@DefaultQuantity", SqlDbType.Int) { Direction = ParameterDirection.Output };

            cmd.Parameters.AddRange(sqlParameter);

            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            conn.Open();
            try
            {
                da.Fill(ds);
                HandleCartGridException(cmd);

                defaultQuantity = DataAccessHelper.ConvertToInt(cmd.Parameters["@DefaultQuantity"].Value);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        protected void HandleCartGridException(SqlCommand command)
        {
            var paramValue = command.Parameters["returnVal"].Value;
            var returnCode = -1;

            if (paramValue != null)
            {
                returnCode = DataAccessHelper.ConvertToInt(paramValue);
            }

            if (returnCode == 0)
                return;

            //Error
            paramValue = command.Parameters["@ErrorMessage"].Value;
            var errorMessage = paramValue != null ? paramValue.ToString() : "";

            var exception = new CartGridDatabaseException(errorMessage);

            if (string.Compare(errorMessage, "Grid codes cannot be deleted because they are in use.", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, "The basket must be grid enabled.", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, CartGridDatabaseException.UNAUTHORIZED_TO_EDIT_OR_REPLACE_GRID_LINES_MESSAGE, StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, "Invalid parameters: Basket is not Open state.", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, "BasketState is not Open.", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, CartGridDatabaseException.CART_DUPLICATE_NAME, StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, "Replacement will create duplicate grid, cannot proceed", StringComparison.OrdinalIgnoreCase) == 0)
            {
                exception.IsBusinessError = true;
            }
            else
                Logger.Write("CartGridDatabaseException", errorMessage);
            

            throw exception;
        }
        public async Task<DataSet> GetNotesByBTKeysAsync(string cartId, string userId, List<string> btkeys, List<string> lineItemIds = null)
        {
            var conn = CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_GET_USER_NOTES_BY_BTKEYS, conn);

            var sqlParameter = new SqlParameter[4];
            sqlParameter[0] = new SqlParameter("@BasketSummaryID", string.IsNullOrEmpty(cartId) ? "" : cartId);
            sqlParameter[1] = new SqlParameter("@UserID", string.IsNullOrEmpty(userId) ? "" : userId);
            if (btkeys == null) btkeys = new List<string>();
            sqlParameter[2] = new SqlParameter("@BTKeyList", SqlDbType.Structured)
            {
                Value = DataAccessHelper.GenerateDataRecords(btkeys, "BTKey")
            };
            if (lineItemIds == null) lineItemIds = new List<string>();
            sqlParameter[3] = new SqlParameter("@LineItemIdList", SqlDbType.Structured)
            {
                Value = DataAccessHelper.GenerateDataRecords(lineItemIds, "BasketLineItemID")
            };

            cmd.Parameters.AddRange(sqlParameter);
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            await conn.OpenAsync();
            try
            {
                await Task.Run(() => da.Fill(ds));
                HandleCartGridException(cmd);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        public DataSet GetCountForLineItems(string cartId, List<string> lineItemIds)
        {
            var conn = this.CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_GET_GRID_NOTE_COUNT_FOR_LINEITEM, conn);

            var sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@BasketSummaryID", cartId);
            sqlParameter[1] = new SqlParameter("@BasketLineItemIDs", SqlDbType.Structured) { Value = DataConverter.ConverUserIDsToDataSet(lineItemIds) };
            cmd.Parameters.AddRange(sqlParameter);
            var ds = new DataSet();
            var da = new SqlDataAdapter(cmd);
            conn.Open();
            try
            {
                da.Fill(ds);
                HandleCartGridException(cmd);
                return ds;
            }
            finally
            {
                conn.Close();
            }
        }
        public void SaveUserGridFieldsCodes(string userId, List<SqlDataRecord> sqlDataRecords, int defaultQuantity)
        {
            using (var conn = this.CreateSqlConnection())
            {
                var cmd = CreateSqlSpCommand(StoredProcedureName.ProcTs360SaveUserGridFieldsCodes, conn);

                var sqlParameter = new SqlParameter[3];
                sqlParameter[0] = new SqlParameter("@u_user_id", SqlDbType.NVarChar, 50) { Value = userId };

                sqlParameter[1] = new SqlParameter("@GridFieldsInfo", SqlDbType.Structured) { Value = sqlDataRecords };

                sqlParameter[2] = new SqlParameter("@DefaultQuantity", SqlDbType.Int) { Value = defaultQuantity };

                conn.Open();
                cmd.Parameters.AddRange(sqlParameter);
                cmd.ExecuteNonQuery();
                HandleCartGridException(cmd);
            }
        }
        public DataSet GetCartGridLines(List<string> lineItemIdList)
        {
            var conn = this.CreateSqlConnection();
            var cmd = this.CreateSqlSpCommand(StoredProcedureName.PROC_GET_BASKET_LINE_ITEM_GRIDS, conn);

            var sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@BasketLineItemIDs", SqlDbType.Structured)
            {
                Value =
                    DataAccessHelper
                    .GenerateDataRecords(
                        lineItemIdList,
                        "GUID")
            };

            cmd.Parameters.AddRange(sqlParameter);
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            conn.Open();
            try
            {
                da.Fill(ds);
                HandleCartGridException(cmd);
            }
            finally
            {
                conn.Close();
            }
            return ds;
        }
    }
}
