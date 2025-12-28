using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BT.TS360API.ServiceContracts.Pricing;
using BT.TS360Constants;
using AppSettings = BT.TS360API.Common.Configrations.AppSettings;

namespace BT.TS360API.Common.DataAccess
{
    public class PricingControllerDAO : BaseDAO
    {
        #region Constants

        internal const string ConnectStringProvider = "provider";
        internal const char Semicolon = ';';

        #endregion

        #region Singleton

        private static PricingControllerDAO _instance = new PricingControllerDAO();
        public static PricingControllerDAO Instance
        {
            get { return _instance ?? (_instance = new PricingControllerDAO()); }
        }

        #endregion

        private SqlTransaction _transaction = null;
        private SqlConnection _connection = null;
        #region Implement BaseDAO
        public override string ConnectionString
        {
            get
            {
                var connectString = AppSettings.OrderDbConnString;
                // Cut out Provider
                if (connectString.ToLower().Contains(ConnectStringProvider))
                {
                    int firstDelimeter = connectString.IndexOf(Semicolon);
                    connectString = connectString.Substring(firstDelimeter + 1);
                }

                return connectString;
            }
        }

        //protected override SqlCommand CreateSqlSpCommand(string spName, SqlConnection sqlConnection)
        //{
        //    var command = base.CreateSqlSpCommand(spName, sqlConnection);

        //    var paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

        //    var paramReturnValue = new SqlParameter("returnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

        //    command.Parameters.Add(paramErrorMessage);
        //    command.Parameters.Add(paramReturnValue);

        //    return command;
        //}
        #endregion

        private int SqlCmdTimeout
        {
            get
            {
                var cmdTimeout = AppSettings.PricingSqlCmdTimeout;
                int iValue;
                if (string.IsNullOrEmpty(cmdTimeout) || !Int32.TryParse(cmdTimeout, out iValue))
                {
                    iValue = 300; //default
                }
                return iValue;
            }
        }

        internal void BeginTransaction()
        {
            _connection = CreateSqlConnection();
            _connection.Open();

            _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        internal void CommitTransaction()
        {
            if (_transaction != null && _transaction.Connection != null)
            {
                _transaction.Commit();
                _transaction = null;
            }
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }

        internal void RollbackTransaction()
        {
            if (_transaction != null && _transaction.Connection != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }

            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }

        public DataSet GetBasketLineItemUpdatedToRePrice(string basketSummaryId, string basketPostfixCharacterSet = null)
        {
            var ds = new DataSet();

            var dbConnection = CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_GET_BASKET_LINE_ITEM_UPDATED, dbConnection);
            cmd.CommandTimeout = SqlCmdTimeout;

            var paramBasketSummaryId = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50)
            {
                IsNullable = true,
                Value = (object)basketSummaryId ?? DBNull.Value
            };
            cmd.Parameters.Add(paramBasketSummaryId);

            if (!string.IsNullOrEmpty(basketPostfixCharacterSet))
            {
                var paramBasketSummaryGroup = new SqlParameter("@BasketSummaryGroup", SqlDbType.NVarChar, 50)
                {
                    Value = basketPostfixCharacterSet
                };
                cmd.Parameters.Add(paramBasketSummaryGroup);
            }
            
            var da = new SqlDataAdapter(cmd);

            try
            {
                dbConnection.Open();
                da.Fill(ds);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
        }

        internal void UpdateBasketLineItemUpdated(List<BasketLineItemUpdated> items)
        {
            var dt = ConvertToBasketLineItemUpdated(items);
            var dbConnection = CreateSqlConnection();
            using (var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_SET_BASKET_LINE_ITEM_UPDATED, dbConnection))
            {
                var parameter = new SqlParameter
                {
                    ParameterName = "@BasketLineItemPriceChanges",
                    SqlDbType = SqlDbType.Structured,
                    Value = dt,
                    TypeName = TableParameterType.BasketLineItemUpdatedTable // important
                };

                cmd.Parameters.Add(parameter);
                try
                {
                    dbConnection.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    dbConnection.Close();
                }
            }
        }


        internal void ResetBasketRepricingIndicator(string basketSummaryId)
        {
            if (string.IsNullOrEmpty(basketSummaryId))
                return;

            var dbConnection = CreateSqlConnection();
            using (var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_RESET_BASKET_REPRICING_INDICATOR, dbConnection))
            {
                var paramBasketSummaryId = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50)
                {
                    Value = basketSummaryId
                };

                cmd.Parameters.Add(paramBasketSummaryId);
                try
                {
                    dbConnection.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    dbConnection.Close();
                }
            }
        }

        #region Helpers

        private DataTable CreateBasketLineItemUpdatedTable()
        {
            var obj = new BasketLineItemUpdated
            {
                BasketLineItemId = string.Empty,
                ListPrice = 0,
                PromotionPrice = 0,
                ContractPrice = 0,
                PromotionCode = string.Empty,
                DiscountPercent = 0
            };

            var dt = new DataTable("utblBasketLineItemUpdated");
            dt.Columns.Add("BasketLineItemID", obj.BasketLineItemId.GetType());
            dt.Columns.Add("ListPrice", obj.ListPrice.HasValue ? obj.ListPrice.GetType() : typeof(decimal));
            dt.Columns.Add("PromotionPrice",
                           obj.PromotionPrice.HasValue ? obj.PromotionPrice.GetType() : typeof(decimal));
            dt.Columns.Add("ContractPrice", obj.ContractPrice.HasValue ? obj.ContractPrice.GetType() : typeof(decimal));
            dt.Columns.Add("PromotionCode", obj.PromotionCode.GetType());
            dt.Columns.Add("ContractDiscountPercent", obj.DiscountPercent.GetType());
            dt.Columns.Add("ProcessingCharges", typeof(decimal));
            dt.Columns.Add("BuildingCount", typeof(int));
            dt.Columns.Add("SalesTax", typeof(decimal));

            return dt;
        }

        private DataTable ConvertToBasketLineItemUpdated(List<BasketLineItemUpdated> listObj)
        {
            var dt = CreateBasketLineItemUpdatedTable();
            if (listObj != null && listObj.Any())
            {
                listObj.ForEach(r => dt.Rows.Add(r.BasketLineItemId,
                                                 r.ListPrice,
                                                 r.PromotionPrice,
                                                 r.ContractPrice,
                                                 r.PromotionCode,
                                                 r.DiscountPercent,
                                                 r.ProcessingCharges,
                                                 r.NumberOfBuildings,
                                                 r.SalesTax));
            }
            return dt;
        }

        #endregion
    }
}
