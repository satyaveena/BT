using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BT.TS360API.ServiceContracts;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.Common.Constants;
using BT.TS360API.Common.Models;
using System.Collections.Generic;
using BT.TS360Constants;

namespace BT.TS360API.Common.DataAccess
{
    public class QuickSearchDAO : BaseDAO
    {
        private static volatile QuickSearchDAO _instance;
        private static readonly object SyncRoot = new Object();

        private QuickSearchDAO()
        {
        }

        public static QuickSearchDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new QuickSearchDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["OrderDbConnString"]; }
        }


        public string AddProductsToCart(string userId, string cartName, string userFolderId, string cartId, List<LineItem> lineItems, out string PermissionViolationMessage, out int totalAddingQuantity)
        {
            var basketSummaryId = cartId;

            var linesDataSet = DataConverter.ConvertCartLineItemsToDataset(lineItems);

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.ProcTs360QuickSearchAddProductToCart, dbConnection);

            var cartNameParam = new SqlParameter("@CartName", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = cartName
            };

            var userFolderIdParam = new SqlParameter("@UserFolderId", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = userFolderId
            };
            var userIdParam = new SqlParameter("@UserId", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = userId
            };
            var lineItemsParam = new SqlParameter("@BasketLineItems", SqlDbType.Structured)
            {
                Value = DataAccessHelper.GenerateDataRecords(linesDataSet)
            };
            var basketSummaryIdParam = new SqlParameter("@BasketSummaryId", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.InputOutput
            };
            var totalAddingQuantityParam = new SqlParameter("@TotalAddingQuantity", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Output
            };
            if (string.IsNullOrEmpty(cartId))
            {
                basketSummaryIdParam.Value = DBNull.Value;
            }
            else
            {
                basketSummaryIdParam.Value = cartId;
            }
            var permissionViolationMessage = new SqlParameter("@PermissionViolationMessage", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
            command.Parameters.AddRange(new[]{
                                        cartNameParam, userFolderIdParam, userIdParam, lineItemsParam, basketSummaryIdParam, totalAddingQuantityParam, permissionViolationMessage
            });

            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();
                HandleCartException(command);
                basketSummaryId = basketSummaryIdParam.Value != null ? basketSummaryIdParam.Value.ToString() : cartId;
                PermissionViolationMessage = command.Parameters["@PermissionViolationMessage"].Value as string;

                if (!int.TryParse(command.Parameters["@TotalAddingQuantity"].Value.ToString(), out totalAddingQuantity))
                {
                    totalAddingQuantity = 0;
                }
            }
            finally
            {
                dbConnection.Close();
            }
            return basketSummaryId;
        }

        public DataSet GetActiveCarts(int topCartsNo, string userId)
        {
            var ds = new DataSet();

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.ProcTs360QuickSearchGetActiveCarts, dbConnection);

            //<Parameter>
            var paramRowCountNo = new SqlParameter("@Rowcount", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = topCartsNo };
            var paramUserId = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
            command.Parameters.AddRange(new[] { paramRowCountNo, paramUserId });

            var da = new SqlDataAdapter(command);

            dbConnection.Open();
            try
            {
                da.Fill(ds);
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
        }
    }
}
