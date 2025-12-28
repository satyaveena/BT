using BT.TS360API.Common.Helpers;
using BT.TS360API.ServiceContracts.Request;
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
    public class OrdersDAO : BaseDAO
    {
        private static volatile OrdersDAO _instance;
        private static readonly object SyncRoot = new Object();

        private OrdersDAO()
        {
        }

        public static OrdersDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new OrdersDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["OrderDbConnString"]; }
        }

        public DataSet GetGridFieldsCodesForOrg(string orgId)
        {
            var ds = new DataSet();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage("procTS360GetGridFieldsAndCodesByOrganization", dbConnection);

            //<Parameter>
            var sqlParameter = new SqlParameter("@u_org_id", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = orgId };
            var paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            command.Parameters.Add(sqlParameter);
            command.Parameters.Add(paramErrorMessage);

            var da = new SqlDataAdapter(command);
            dbConnection.Open();
            try
            {
                da.Fill(ds);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
        }
        public DataTable GetMARCRecordsBTKeys(String basketSummaryID)
        {
            DataTable dt = new DataTable();
            var ds = new DataSet();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage("procMARCGetBasketBTKeys", dbConnection);
            SqlParameter paramBasketSummaryID = new SqlParameter("BasketSummaryID", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = basketSummaryID };
            command.Parameters.Add(paramBasketSummaryID);

            var da = new SqlDataAdapter(command);
            dbConnection.Open();
            try
            {
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }

            finally
            {
                dbConnection.Close();
            }

            return dt;
        }
        public void SaveILSLineItemDetails(string userId, List<ILSLineItemDetail> lstILSLineItemDetail)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_SET_BASKET_LINES_ILS_COLUMNS, dbConnection);

            //<Parameter>
            SqlParameter[] sqlParameter = {
                                              new SqlParameter("@BasketLineItemIDs", SqlDbType.Structured),
                                              new SqlParameter("@UserID", userId)
                                          };

            sqlParameter[0].TypeName = "dbo.utblILSColumns";
            sqlParameter[0].Value = DataConverter.ConvertILSLineItemDetailToDataTable(lstILSLineItemDetail);
            command.Parameters.AddRange(sqlParameter);

            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();
                HandleOrderException(command);
            }
            finally
            {
                dbConnection.Close();
            }
        }
        public void HandleOrderException(SqlCommand command)
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
            var errorMessage = paramValue != null ? paramValue.ToString() : string.Empty;
            //var exception = new OrdersException(errorMessage);

            //if (exception.Message.Equals(OrdersException.ESP_FUND_GRID_FIELD_NOT_FOR_ORG)
            //    || exception.Message.Equals(OrdersException.ESP_BRANCH_GRID_FIELD_NOT_FOR_ORG)
            //    )
            //{
            //    exception.IsBusinessError = true;
            //}
            //else
            //    Logger.RaiseException(exception, ExceptionCategory.Order);

            //throw exception;
        }

    }
}
