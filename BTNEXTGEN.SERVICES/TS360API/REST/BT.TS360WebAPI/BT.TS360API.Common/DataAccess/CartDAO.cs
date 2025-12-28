using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using Microsoft.SqlServer.Server;
using BT.TS360API.Common.CartFramework;
using BT.TS360API.Common.Models;
using BT.TS360API.Logging;
using BT.TS360API.Common.Resources;

namespace BT.TS360API.Common.DataAccess
{
    public class CartDAO : BaseDAO
    {
        private static volatile CartDAO _instance;
        private static readonly object SyncRoot = new Object();
        private const int LongStoreRunningTimeOut = 180;
        private const char DEFAULT_ORDER_STATUS = 'A';

        private CartDAO()
        {
        }

        public static CartDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CartDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["OrderDbConnString"]; }
        }


        #region Move wcf to api
        public bool HideEspAutoRankMessage(HideEspAutoRankMessageRequest request)
        {
            bool isSuccess = false;
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_SET_ESP_AUTO_RANK, dbConnection);
            command.CommandTimeout = 300;
                       
            //<Parameter>
            var sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50) { Value = request.CartId }; 
            sqlParameters[1] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Value = request.UserId };
            sqlParameters[2] = new SqlParameter("@AutoRank", SqlDbType.Bit) { Value = request.IsAutoRank };

            command.Parameters.AddRange(sqlParameters);

            var ds = new DataSet();
            var da = new SqlDataAdapter(command);

            dbConnection.Open();

            try
            {
                command.ExecuteScalar();

                HandleCartException(command);                

                var paramValue = command.Parameters["returnVal"].Value;
                var returnCode = -1;

                if (paramValue != null)
                {
                    returnCode = DataAccessHelper.ConvertToInt(paramValue);
                }

                if (returnCode == 0 || returnCode == 1000)
                    isSuccess = true;                
            }
            finally
            {
                dbConnection.Close();
            }

            return isSuccess;
        }

        public DataSet GetCartDetailsQuickView(out int nonRankedCount, string cartId, string userId, string sortBy, byte sortDirection, short pageSize, int pageNumber, bool recalculateHeader)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_GET_CART_DETAILS_QUICK_VIEW, dbConnection);
            command.CommandTimeout = 300;

            if (string.Equals(sortBy, SearchResultsSortField.SORT_ADD_TO_CART_DATE, StringComparison.OrdinalIgnoreCase))
            {
                sortBy = "basketorder";
            }

            //<Parameter>
            var sqlParameters = new SqlParameter[8];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", cartId);
            sqlParameters[1] = new SqlParameter("@UserID", userId);
            sqlParameters[2] = new SqlParameter("@SortBy", sortBy);
            sqlParameters[3] = new SqlParameter("@SortDirection", sortDirection);
            sqlParameters[4] = new SqlParameter("@PageSize", pageSize);
            sqlParameters[5] = new SqlParameter("@PageNumber", pageNumber);
            sqlParameters[6] = new SqlParameter("@RecalculateHeader", recalculateHeader);
            sqlParameters[7] = new SqlParameter("@PendingRankCount", SqlDbType.Int) { Direction = ParameterDirection.Output };

            command.Parameters.AddRange(sqlParameters);

            var ds = new DataSet();
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

            nonRankedCount = DataAccessHelper.ConvertToInt(command.Parameters["@PendingRankCount"].Value);

            return ds;
        }

        #endregion

        public DataSet GetCartFolderById(string folderId, string userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get Cart Folders by userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetCartFolders(string userId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_GET_FOLDER_BY_USER_ID, dbConnection);
            var sqlParamater = new SqlParameter("@Userid", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
            command.Parameters.Add(sqlParamater);
            dbConnection.Open();
            try
            {
                var ds = new DataSet();
                var sqlDa = new SqlDataAdapter(command);
                sqlDa.Fill(ds);

                HandleCartException(command);

                return ds;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public bool IsCartPricing(string cartId, string userId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_IS_PRICING, dbConnection);
            //<Parameter>
            var sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = cartId
            };

            sqlParameters[1] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = userId
            };

            command.Parameters.AddRange(sqlParameters);

            dbConnection.Open();
            try
            {
                var returnObj = command.ExecuteScalar();

                HandleCartException(command);
                bool isProcessing;

                bool.TryParse(returnObj.ToString(), out isProcessing);

                return isProcessing;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public DataSet GetAccountsSummary(string cartId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_BASKET_GET_ACCOUNT_SUMMARIES, dbConnection);

            //<Parameter>
            var sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", cartId);

            command.Parameters.AddRange(sqlParameters);

            var ds = new DataSet();
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

        public async Task<string> TestAsync()
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand("procTS360SetBasketReleaseQuotation", dbConnection);

            //<Parameter>
            var paramBasketID = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = "" };
            var paramUserID = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = "" };

            command.Parameters.AddRange(new[] { paramBasketID, paramUserID });


            await dbConnection.OpenAsync();

            try
            {
                await command.ExecuteNonQueryAsync();
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            return "";
        }

        public async Task<DataSet> GetDataSetAsync()
        {
            var ds = new DataSet();

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_RELEASE_QUOTE, dbConnection);

            //<Parameter>
            var paramBasketID = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = "" };
            var paramUserID = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = "" };

            command.Parameters.AddRange(new[] { paramBasketID, paramUserID });

            var da = new SqlDataAdapter(command);

            await dbConnection.OpenAsync();
            try
            {
                await Task.Run(() => da.Fill(ds));
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        /// <remarks>Used for getting carts by folder's id</remarks>
        internal async Task<DataSet> GetCartsAsync(string folderId)
        {
            var ds = new DataSet();

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_GET_BASKET_BY_FOLDER, dbConnection);

            //<Parameter>
            var sqlParameter = new SqlParameter("@UserFolderID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = folderId };
            command.Parameters.Add(sqlParameter);

            var da = new SqlDataAdapter(command);
            await dbConnection.OpenAsync();
            try
            {
                await Task.Run(() => da.Fill(ds));
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }

        internal async Task<CheckForDuplicatesObject> CheckForDuplicates(string listItemKeys, string listBTEkeys, string cartCheckType,
            string orderCheckType, string orgId, string soldToId, string itemsCheckOrder, string itemsCheckCart, string basketId,
            string downloadCheckType)
        {
            var result = new CheckForDuplicatesObject();

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_CHECK_FOR_DUPLICATES, dbConnection);

            //<Parameter>            
            var sqlParamaters = CreateSqlParamaters(10);

            sqlParamaters[0] = new SqlParameter("@listItemKeys", listItemKeys ?? string.Empty);
            sqlParamaters[1] = new SqlParameter("@listBTEkeys", listBTEkeys ?? string.Empty);
            sqlParamaters[2] = new SqlParameter("@cartCheckType", cartCheckType ?? string.Empty);
            sqlParamaters[3] = new SqlParameter("@orderCheckType", orderCheckType ?? string.Empty);

            if (orgId == null)
                sqlParamaters[4] = new SqlParameter("@orgId", DBNull.Value);
            else
                sqlParamaters[4] = new SqlParameter("@orgId", orgId);
            sqlParamaters[5] = new SqlParameter("@soldToId", soldToId ?? string.Empty);
            sqlParamaters[6] = new SqlParameter("@basketId", basketId ?? string.Empty);
            sqlParamaters[7] = new SqlParameter("@downloadCheckType", downloadCheckType ?? string.Empty);
            sqlParamaters[8] = new SqlParameter("@itemsCheckOrder", SqlDbType.NVarChar, 2000) { Direction = ParameterDirection.Output };
            sqlParamaters[9] = new SqlParameter("@itemsCheckCart", SqlDbType.NVarChar, 2000) { Direction = ParameterDirection.Output };

            command.Parameters.AddRange(sqlParamaters);

            //
            try
            {
                await dbConnection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                HandleCartException(command);

                result.ItemsCheckOrder = command.Parameters["@itemsCheckOrder"].Value.ToString();
                result.ItemsCheckCart = command.Parameters["@itemsCheckCart"].Value.ToString();
            }
            finally
            {
                dbConnection.Close();
            }

            result.IsDuplication = true;
            return result;
        }

        internal async Task<string> CheckForCartDuplicates(string orgId, string soldToId, string basketId, string listItemKeys, string listBTEkeys,
            string cartCheckType, string downloadCheckType)
        {
            string itemsCheckCart = string.Empty;
            using (var dbConnection = CreateSqlConnection())
            {
                using (var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_CHECK_FOR_DUPLICATES, dbConnection))
                {
                    // parameters          
                    var sqlParamaters = CreateSqlParamaters(8);

                    sqlParamaters[0] = new SqlParameter("@listItemKeys", listItemKeys ?? string.Empty);
                    sqlParamaters[1] = new SqlParameter("@listBTEkeys", listBTEkeys ?? string.Empty);
                    sqlParamaters[2] = new SqlParameter("@cartCheckType", cartCheckType ?? string.Empty);

                    if (orgId == null)
                        sqlParamaters[3] = new SqlParameter("@orgId", DBNull.Value);
                    else
                        sqlParamaters[3] = new SqlParameter("@orgId", orgId);
                    sqlParamaters[4] = new SqlParameter("@soldToId", soldToId ?? string.Empty);
                    sqlParamaters[5] = new SqlParameter("@basketId", basketId ?? string.Empty);
                    sqlParamaters[6] = new SqlParameter("@downloadCheckType", downloadCheckType ?? string.Empty);
                    sqlParamaters[7] = new SqlParameter("@itemsCheckCart", SqlDbType.NVarChar, 2000) { Direction = ParameterDirection.Output };

                    command.Parameters.AddRange(sqlParamaters);

                    await dbConnection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    HandleCartException(command);

                    // result
                    itemsCheckCart = command.Parameters["@itemsCheckCart"].Value.ToString(); // Ex: 'C;C;C;C;N;C;C;C;N;'
                }
            }

            return itemsCheckCart;
        }

        public async Task<Dictionary<string, bool>> CheckHoldingsDuplicates(string userId, string orgID, string holdingsFlag, string btKeys)
        {
            var resultSet = new Dictionary<string, bool>();

            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                using (var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_HOLDINGS_DUP_CHECK, dbConnection))
                {
                    var userIdParam = new SqlParameter("@u_user_id", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
                    var orgIdParam = new SqlParameter("@orgId", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = orgID };
                    var holdingsTypeParam = new SqlParameter("@HoldingsDupCheckType", SqlDbType.Char, 4) { Direction = ParameterDirection.Input, Value = holdingsFlag };
                    var btkeysParam = new SqlParameter("@listBTKeys", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Input, Value = btKeys };

                    command.Parameters.Add(userIdParam);
                    command.Parameters.Add(orgIdParam);
                    command.Parameters.Add(holdingsTypeParam);
                    command.Parameters.Add(btkeysParam);

                    await dbConnection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var btKey = reader["BTKey"].ToString();
                            if (!resultSet.ContainsKey(btKey))
                            {
                                var isInHoldingsQueue = DataAccessHelper.ConvertToBool(reader["IsInHoldingsQueue"]);
                                resultSet.Add(btKey, isInHoldingsQueue);
                            }
                        }
                    }

                    return resultSet.Count > 0 ? resultSet : null;
                }
            }
        }

        #region Partial
        /// <summary>
        /// Gest Primary Cart by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal DataSet GetPrimaryCart(string userId)
        {
            var ds = new DataSet();

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_GET_PRIMARY_BASKET, dbConnection);

            //<Parameter>
            var sqlParameter = new SqlParameter("@u_user_id", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
            command.Parameters.Add(sqlParameter);

            var da = new SqlDataAdapter(command);

            dbConnection.Open();
            try
            {
                da.Fill(ds);
                HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
        }

        internal async Task<DataSet> GetPrimaryCartAsync(string userId)
        {
            var ds = new DataSet();

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_GET_PRIMARY_BASKET, dbConnection);

            //<Parameter>
            var sqlParameter = new SqlParameter("@u_user_id", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
            command.Parameters.Add(sqlParameter);

            var da = new SqlDataAdapter(command);

            await dbConnection.OpenAsync();

            try
            {
                await Task.Run(() => da.Fill(ds));
                //da.Fill(ds);
                HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
        }

        internal DataSet GetCart(string cartId, string userId, out int nonRankedCount)
        {
            var ds = new DataSet();

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_GET_BASKET_BYID, dbConnection);

            //<Parameter>
            //var paramBasketID = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = cartId };
            //var paramUserID = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) 
            //                    { Direction = ParameterDirection.Input, Value = string.IsNullOrEmpty(userId) ? DBNull.Value : userId };

            var sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", cartId);
            if (!string.IsNullOrEmpty(userId))
                sqlParameters[1] = new SqlParameter("@UserID", userId);
            else
            {
                sqlParameters[1] = new SqlParameter("@UserID", DBNull.Value);
            }
            sqlParameters[2] = new SqlParameter("@PendingRankCount", SqlDbType.Int) { Direction = ParameterDirection.Output };

            command.Parameters.AddRange(sqlParameters);

            var da = new SqlDataAdapter(command);

            dbConnection.Open();
            try
            {
                da.Fill(ds);
                HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            nonRankedCount = DataAccessHelper.ConvertToInt(command.Parameters["@PendingRankCount"].Value);

            return ds;
        }

        internal async Task<DataSet> GetCartByName(string cartName, string userId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_GET_BASKET_BY_NAME, dbConnection);

            //<Parameter>
            //int total = 0;
            var sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@BasketName", cartName);
            sqlParameters[1] = new SqlParameter("@UserID", userId);

            command.Parameters.AddRange(sqlParameters);

            var ds = new DataSet();
            var da = new SqlDataAdapter(command);

            await dbConnection.OpenAsync();
            try
            {
                await Task.Run(() => da.Fill(ds));
                //da.Fill(ds);
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }

        internal void AddLineItemsToCart(DataSet lineItems, string cartId, string userId, int maxLinesPerCart, out string PermissionViolationMessage,
            out int totalAddingQty)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_BASKET_MERGE_LINE_ITEMS, dbConnection);

            var paramLineItems = DataAccessHelper.GenerateDataRecords(lineItems).Distinct(new SqlDataRecordComparerByBTKey()).ToList();

            var sqlParameters = CreateLineItemParameters(cartId, userId,
                                                         paramLineItems, DEFAULT_ORDER_STATUS, false, maxLinesPerCart);


            command.Parameters.AddRange(sqlParameters);
            command.CommandTimeout = 600;
            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();

                HandleCartException(command);
                PermissionViolationMessage = command.Parameters["@PermissionViolationMessage"].Value as string;
                if (!int.TryParse(command.Parameters["@TotalAddingQuantity"].Value.ToString(), out totalAddingQty))
                {
                    totalAddingQty = 0;
                }
            }
            finally
            {
                dbConnection.Close();
            }
        }
        internal void AddExceed500LineItemsToCart(DataSet lineItems, string userId, string cartId, int maxLinesPerCart)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_BASKET_MERGE_EXCEED500_LINE_ITEMS, dbConnection);

            var paramLineItems = DataAccessHelper.GenerateDataRecords(lineItems).Distinct(new SqlDataRecordComparerByBTKey()).ToList();

            var sqlParameters = CreateExceed500LineItemParameters(cartId, userId, paramLineItems);

            command.Parameters.AddRange(sqlParameters);
            command.CommandTimeout = 600;
            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();

                HandleCartException(command);

            }
            finally
            {
                dbConnection.Close();
            }
        }
        private SqlParameter[] CreateExceed500LineItemParameters(string cartId, string userId, List<SqlDataRecord> lineItems)
        {
            var sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@UserID", userId);
            sqlParameters[1] = new SqlParameter("@BasketSummaryID", cartId);
            sqlParameters[2] = new SqlParameter("@BasketLineItems", SqlDbType.Structured) { Value = lineItems };
            // sqlParameters[3] = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
            return sqlParameters;
        }
        public void AddProductToCart(List<SqlDataRecord> products, string toCartId, List<SqlDataRecord> cartGridLines, string userId, out string PermissionViolationMessage, out int totalAddingQuantity)
        {
            var conn = this.CreateSqlConnection();
            var cmd = this.CreateSqlSpCommand(StoredProcedureName.PROC_ADD_PRODUCT_TO_CART, conn);
            cmd.CommandTimeout = 180;
            var sqlParameter = new SqlParameter[11];
            sqlParameter[0] = new SqlParameter("@DestinationBasketSummaryID", toCartId) { Direction = ParameterDirection.InputOutput };
            sqlParameter[1] = new SqlParameter("@UserID", userId);
            if (products == null || products.Count == 0)
            {
                sqlParameter[2] = new SqlParameter("@BasketLineItems", SqlDbType.Structured);
                sqlParameter[2].Value = null;
            }
            else
            {
                sqlParameter[2] = new SqlParameter("@BasketLineItems", SqlDbType.Structured) { Value = products.Distinct(new SqlDataRecordComparerByBTKey()).ToList() };
            }
            sqlParameter[3] = new SqlParameter("@BasketLineItemOrderStatus", "A");
            sqlParameter[4] = new SqlParameter("@DeleteSourceLineItems", false);
            if (cartGridLines == null || cartGridLines.Count == 0)
            {
                sqlParameter[5] = new SqlParameter("@utblBasketGridLine", SqlDbType.Structured);
                sqlParameter[5].Value = null;
            }
            else
            {
                sqlParameter[5] = new SqlParameter("@utblBasketGridLine", SqlDbType.Structured) { Value = cartGridLines };
            }
            sqlParameter[6] = new SqlParameter("@MaxLinesPerCartNumber", CartFrameworkHelper.MaxLinesPerCart);
            sqlParameter[7] = new SqlParameter("@All_or_MyQty_or_TitleOnly", DBNull.Value);
            sqlParameter[8] = new SqlParameter("@OverRide_Warning", DBNull.Value);
            sqlParameter[9] = new SqlParameter("@PermissionViolationMessage", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
            sqlParameter[10] = new SqlParameter("@TotalAddingQuantity", SqlDbType.Int) { Direction = ParameterDirection.Output };

            cmd.Parameters.AddRange(sqlParameter);
            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                HandleCartException(cmd);
                PermissionViolationMessage = cmd.Parameters["@PermissionViolationMessage"].Value as string;
                if (
                    !int.TryParse(cmd.Parameters["@TotalAddingQuantity"].Value.ToString(),
                        out totalAddingQuantity))
                {
                    totalAddingQuantity = 0;
                }
            }
            finally
            {
                conn.Close();
            }
        }

        internal DataSet GetQuantitiesByBtKeys(string cartId, string userId, List<string> btKeys)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_GET_QUANTITY_BY_BTKEYS, dbConnection);

            var sqlParameters = new SqlParameter[3];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", cartId);

            if (string.IsNullOrEmpty(userId))
                sqlParameters[1] = new SqlParameter("@UserID", DBNull.Value);
            else
                sqlParameters[1] = new SqlParameter("@UserID", userId);

            var btkeysTable = DataConverter.GenerateDataRecords<string>(btKeys, "BTKey", 50);

            sqlParameters[2] = new SqlParameter("@BTKeys", SqlDbType.Structured) { Direction = ParameterDirection.Input, Value = btkeysTable };

            //<Parameter>
            int total = 0;

            command.Parameters.AddRange(sqlParameters);

            var ds = new DataSet();
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

        internal DataSet CheckBasketForTitles(string cartId, List<string> btKeys, List<string> lineItemIds, out int newLineCount, out int existingLineCount)
        {
            newLineCount = 0;
            existingLineCount = 0;
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_CHECK_BASKET_FOR_TITLES, dbConnection);

            var sqlParameters = new SqlParameter[5];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", cartId);
            sqlParameters[3] = new SqlParameter("@CountNew", SqlDbType.Int) { Direction = ParameterDirection.Output };
            sqlParameters[4] = new SqlParameter("@CountExisting", SqlDbType.Int) { Direction = ParameterDirection.Output };


            sqlParameters[1] = CreateTableParameter("@BTKeys", "utblBTKeys",
                                                    ConvertToListBtKeyArgumentTable(btKeys, CreateBtKeyArgumentTable()));

            sqlParameters[2] = CreateTableParameter("@BasketLineItemIDs", "utblCSGuids",
                                                    ConvertToListBtKeyArgumentTable(lineItemIds, CreateLineItemsArgumentTable()));

            //<Parameter>
            int total = 0;

            command.Parameters.AddRange(sqlParameters);

            var ds = new DataSet();
            var da = new SqlDataAdapter(command);

            dbConnection.Open();
            try
            {
                da.Fill(ds);
                HandleCartException(command);
                newLineCount = ConvertToInt(command.Parameters["@CountNew"].Value);
                existingLineCount = ConvertToInt(command.Parameters["@CountExisting"].Value);
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }

        internal async Task<DataSet> GetESPRankItemDetails(string basketLineItemID)
        {
            var ds = new DataSet();

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_GET_BASKETS_LINE_ITEMS_ESP_RANKING, dbConnection);

            var sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@BasketLineItemID", basketLineItemID);

            command.Parameters.AddRange(sqlParameters);

            var da = new SqlDataAdapter(command);

            await dbConnection.OpenAsync();
            try
            {
                await Task.Run(() => da.Fill(ds));
                //da.Fill(ds);
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
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

        private DataTable ConvertToListBtKeyArgumentTable(IEnumerable<string> items, DataTable dt)
        {
            if (items != null)
            {
                var list = items.ToList();
                if (list.Any())
                    list.ForEach(r => dt.Rows.Add(r));
            }
            return dt;
        }

        private DataTable CreateBtKeyArgumentTable()
        {
            var dt = new DataTable("utblBTKeys");
            dt.Columns.Add("BTKey", typeof(string));
            return dt;
        }

        private DataTable CreateLineItemsArgumentTable()
        {
            var dt = new DataTable("utblCSGuids");
            dt.Columns.Add("GUID", typeof(string));
            return dt;
        }

        private SqlParameter[] CreateLineItemParameters(string destCartId, string userId, List<SqlDataRecord> lineItems, char lineItemOrderStatus,
            bool isDelete, int maxLinesPerCart, string sourceCartId = null)
        {
            var sqlParameters = new SqlParameter[11];
            sqlParameters[0] = new SqlParameter("@SourceBasketSummaryID", sourceCartId);
            sqlParameters[1] = new SqlParameter("@DestinationBasketSummaryID", destCartId);
            sqlParameters[2] = new SqlParameter("@UserID", userId);
            sqlParameters[3] = new SqlParameter("@BasketLineItems", SqlDbType.Structured) { Value = lineItems };
            sqlParameters[4] = new SqlParameter("@BasketLineItemOrderStatus", lineItemOrderStatus);
            sqlParameters[5] = new SqlParameter("@DeleteSourceLineItems", isDelete);
            sqlParameters[6] = new SqlParameter("@MaxLinesPerCartNumber", SqlDbType.Int) { Value = maxLinesPerCart };
            sqlParameters[7] = new SqlParameter("@All_or_MyQty_or_TitleOnly", DBNull.Value);
            sqlParameters[8] = new SqlParameter("@OverRide_Warning", DBNull.Value);
            sqlParameters[9] = new SqlParameter("@PermissionViolationMessage", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };
            sqlParameters[10] = new SqlParameter("@TotalAddingQuantity", SqlDbType.Int) { Direction = ParameterDirection.Output };
            return sqlParameters;
        }
        #endregion Partial

        public async Task<DataSet> GetCartLineItemIDs(string cartId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_GET_LINE_ITEMS_ID, dbConnection);

            //<Parameter>
            var sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", cartId);

            command.Parameters.AddRange(sqlParameters);

            var ds = new DataSet();
            var da = new SqlDataAdapter(command);

            await dbConnection.OpenAsync();
            try
            {
                await Task.Run(() => da.Fill(ds));
                //da.Fill(ds);
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }

        public async Task<DataSet> GetCartLineById(string lineItemId, string userId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_BASKET_GET_LINE_ITEM_BY_ID, dbConnection);

            //<Parameter>
            int total = 0;
            var sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@BasketLineItemID", lineItemId);
            sqlParameters[1] = new SqlParameter("@UserID", userId);

            command.Parameters.AddRange(sqlParameters);

            var ds = new DataSet();
            var da = new SqlDataAdapter(command);

            await dbConnection.OpenAsync();
            try
            {
                await Task.Run(() => da.Fill(ds));
                //da.Fill(ds);
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }

        public DataSet GetCartLines(SearchCartLinesCriteria criteria, out int totalLines)
        {
            totalLines = 0;
            if (criteria == null || string.IsNullOrEmpty(criteria.CartId))
                return null;

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_BASKET_GET_CART_LINE_ITEMS, dbConnection);
            command.CommandTimeout = 300;

            if (!string.IsNullOrEmpty(criteria.Keyword) && !string.IsNullOrEmpty(criteria.KeywordType))
            {
                var keywordParam = PrepareKeywordParamForLines(criteria.Keyword, criteria.KeywordType);
                if (keywordParam != null)
                {
                    command.Parameters.Add(keywordParam);
                }
            }

            string sortbyLiteral = CartFrameworkHelper.TransformSortOrderLineItem(criteria.SortBy);

            var sqlParameters = new SqlParameter[9];
            sqlParameters[0] = new SqlParameter("@FacetPath", criteria.FacetPath ?? string.Empty);
            sqlParameters[1] = new SqlParameter("@BasketSummaryID", criteria.CartId);
            sqlParameters[2] = new SqlParameter("@PageNumber", criteria.PageNumber);
            sqlParameters[3] = new SqlParameter("@PageSize", criteria.PageSize);
            sqlParameters[4] = new SqlParameter("@SortBy", sortbyLiteral);
            sqlParameters[5] = new SqlParameter("@Direction", criteria.SortDirection);
            sqlParameters[6] = new SqlParameter("@TotalLines", SqlDbType.Int) { Direction = ParameterDirection.Output };
            sqlParameters[7] = new SqlParameter("@UserID", criteria.UserId);
            if (!string.IsNullOrEmpty(criteria.MatchingBtkeys))
            {
                var btkeys = DataConverter.GenerateDataRecords(criteria.MatchingBtkeys.Split(';').ToList(), "BTKey", 10);
                sqlParameters[8] = new SqlParameter("@BTKeys", SqlDbType.Structured) { Value = btkeys };
            }
            else
            {
                sqlParameters[8] = new SqlParameter("@BTKeys", null);
            }
            command.Parameters.AddRange(sqlParameters);

            var ds = new DataSet();
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
            totalLines = DataAccessHelper.ConvertToInt(command.Parameters["@TotalLines"].Value);

            return ds;
        }

        private static SqlParameter PrepareKeywordParamForLines(string keyword, string keywordType)
        {
            SqlParameter result = null;
            switch (keywordType)
            {
                case "0":
                    result = new SqlParameter("@KeywordAuthor", SqlDbType.VarChar, 80) { Value = keyword };
                    break;
                case "1":
                    result = new SqlParameter("@KeywordBisacSubjectLiteral", SqlDbType.VarChar, 80) { Value = keyword };
                    break;
                case "2":
                    result = new SqlParameter("@KeywordBTKey", SqlDbType.VarChar, 50) { Value = keyword };
                    break;
                case "3":
                    result = new SqlParameter("@KeywordISBN", SqlDbType.VarChar, 80) { Value = keyword };
                    break;
                case "4":
                    result = new SqlParameter("@KeywordNotes", SqlDbType.VarChar, 80) { Value = keyword };
                    break;
                case "5":
                    result = new SqlParameter("@KeywordPOLineNumber", SqlDbType.VarChar, 80) { Value = keyword };
                    break;
                case "6":
                    result = new SqlParameter("@KeywordTitle", SqlDbType.VarChar, 80) { Value = keyword };
                    break;
                case "7":
                    result = new SqlParameter("@KeywordUPC", SqlDbType.VarChar, 80) { Value = keyword };
                    break;
            }
            return result;
        }

        internal async Task<DataSet> GetCarts(int topCartsNo, string userId)
        {
            var ds = new DataSet();

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_GET_BASKET_BY_TOP_NEWEST, dbConnection);

            //<Parameter>
            var paramRowCountNo = new SqlParameter("@Rowcount", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = topCartsNo };
            var paramUserId = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
            command.Parameters.AddRange(new[] { paramRowCountNo, paramUserId });

            var da = new SqlDataAdapter(command);

            await dbConnection.OpenAsync();
            try
            {
                await Task.Run(() => da.Fill(ds));
                //da.Fill(ds);
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
        }

        internal async Task<DataSet> GetCartLineByBtKey(string btkey, string cartId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_BASKET_GET_LINE_ITEM_BY_BTKEY, dbConnection);

            //<Parameter>
            int total = 0;
            var sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", cartId);
            sqlParameters[1] = new SqlParameter("@BTKey", btkey);

            command.Parameters.AddRange(sqlParameters);

            var ds = new DataSet();
            var da = new SqlDataAdapter(command);

            await dbConnection.OpenAsync();
            try
            {
                await Task.Run(() => da.Fill(ds));
                //da.Fill(ds);
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }

        public void SetBasketStateILSOrdered(string basketSummaryID, string userId)
        {
            var conn = CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_SET_BASKET_STATE_ILS_ORDERED, conn);
            cmd.CommandTimeout = LongStoreRunningTimeOut;

            var sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@BasketSummaryID", basketSummaryID);
            sqlParameter[1] = new SqlParameter("@UserID", userId);

            cmd.Parameters.AddRange(sqlParameter);

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                HandleCartException(cmd);
            }
            finally
            {
                conn.Close();
            }
        }

        public void SetILSBasketState(string basketSummaryID, string userId, CartStatus basketState, ILSState ilsStatusId, string ILSMarcProfileId, string vendorCode, string orderedDownloadedUserId)
        {
            var conn = CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_TS360SET_ILS_BASKET_STATE, conn);

            var sqlParameter = new SqlParameter[7];
            sqlParameter[0] = new SqlParameter("@BasketSummaryID", basketSummaryID);
            sqlParameter[1] = new SqlParameter("@UserID", userId);
            sqlParameter[2] = new SqlParameter("@BasketStateID", (int)basketState);
            sqlParameter[3] = new SqlParameter("@ILSStatusID", (int)ilsStatusId);

            if (string.IsNullOrEmpty(ILSMarcProfileId))
                sqlParameter[4] = new SqlParameter("@ILSMarcProfileId", DBNull.Value);
            else
                sqlParameter[4] = new SqlParameter("@ILSMarcProfileId", ILSMarcProfileId);

            if (string.IsNullOrEmpty(vendorCode))
                sqlParameter[5] = new SqlParameter("@VendorCode", DBNull.Value);
            else
                sqlParameter[5] = new SqlParameter("@VendorCode", vendorCode);

            if (string.IsNullOrEmpty(orderedDownloadedUserId))
                sqlParameter[6] = new SqlParameter("@OrderedDownloadedUserID", DBNull.Value);
            else
                sqlParameter[6] = new SqlParameter("@OrderedDownloadedUserID", orderedDownloadedUserId);

            cmd.Parameters.AddRange(sqlParameter);

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                HandleCartException(cmd);
            }
            finally
            {
                conn.Close();
            }
        }

        public void SetILSSystemState(string basketSummaryID, string userId, ILSSystemStatus ilsSystemStatusID)
        {
            var conn = CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_SET_ILS_SYSTEM_STATUS, conn);

            var sqlParameter = new SqlParameter[3];
            sqlParameter[0] = new SqlParameter("@BasketSummaryID", basketSummaryID);
            sqlParameter[1] = new SqlParameter("@UserID", userId);
            sqlParameter[2] = new SqlParameter("@ILSSystemStatusID", (int)ilsSystemStatusID);
    
            cmd.Parameters.AddRange(sqlParameter);

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                HandleCartException(cmd);
            }
            finally
            {
                conn.Close();
            }
        }

        public void ResetILSStatus(string basketSummaryID, string userId)
        {
            var conn = CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_RESET_ILS_CART, conn);

            var sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@BasketSummaryID", basketSummaryID);
            sqlParameter[1] = new SqlParameter("@UserID", userId);
           
            cmd.Parameters.AddRange(sqlParameter);

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                HandleCartException(cmd);
            }
            finally
            {
                conn.Close();
            }

        }

        public void SetPricingBasketRollupNumbers(string basketSummaryID)
        {
            var conn = CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_PRICING_SET_BASKET_ROLLUP_NUMBERS, conn);

            var sqlParameter = new SqlParameter[1];
            sqlParameter[0] = new SqlParameter("@BasketSummaryID", basketSummaryID);
            
            cmd.Parameters.AddRange(sqlParameter);

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                HandleCartException(cmd);
            }
            finally
            {
                conn.Close();
            }

        }

        internal void SubmitCart(string cartId, string userId, string loggedInUserId, Dictionary<string, string> orderForm, List<CartAccountSummary> accountDict, string specialInstruction,
            out string newBasketName, out string newBasketId, out string newOEBasketName, out string newOEBasketID, bool isVIP, bool isOrderAndHold, string orderedDownloadedUserId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_SUBMIT_BASKET, dbConnection);
            var accSqlMetadata = DataConverter.GetUtblBasketOrderFormsDataRecords(accountDict);
            var paramBasketID = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = cartId };
            var paramUserId = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
            var paramLoggedInUserId = new SqlParameter("@LoginUserID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = loggedInUserId };
            var paramAccount = new SqlParameter("@BasketOrderForms", SqlDbType.Structured) { Direction = ParameterDirection.Input, Value = accSqlMetadata };
            var paramSpecialInstruction = new SqlParameter("@SpecialInstructions", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = specialInstruction };

            var paramIsHomeDelivery = new SqlParameter("@HomeDeliveryIndicator", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = Boolean.Parse(orderForm["IsHomeDelivery"])
            };
            var paramHandlingTotal = new SqlParameter("@HandlingTotal", SqlDbType.Money) { Direction = ParameterDirection.Input, Value = 0 };
            if (orderForm.ContainsKey("HandlingTotal"))
            {
                paramHandlingTotal.Value = Decimal.Parse(orderForm["HandlingTotal"]);
            }

            var paramShippingTotal = new SqlParameter("@ShippingTotal", SqlDbType.Money) { Direction = ParameterDirection.Input, Value = 0 };
            if (orderForm.ContainsKey("ShippingTotal"))
            {
                paramShippingTotal.Value = Decimal.Parse(orderForm["ShippingTotal"]);
            }

            var paramSubTotal = new SqlParameter("@SubTotal", SqlDbType.Money) { Direction = ParameterDirection.Input, Value = 0 };
            if (orderForm.ContainsKey("SubTotal"))
            {
                paramSubTotal.Value = Decimal.Parse(orderForm["SubTotal"]);
            }

            var paramTaxTotal = new SqlParameter("@TaxTotal", SqlDbType.Money) { Direction = ParameterDirection.Input, Value = 0 };
            if (orderForm.ContainsKey("TaxTotal"))
            {
                paramTaxTotal.Value = Decimal.Parse(orderForm["TaxTotal"]);
            }

            var paramTotal = new SqlParameter("@Total", SqlDbType.Money) { Direction = ParameterDirection.Input, Value = 0 };
            if (orderForm.ContainsKey("Total"))
            {
                paramTotal.Value = Decimal.Parse(orderForm["Total"]);
            }

            var paramName = new SqlParameter("@Name", SqlDbType.NVarChar, 64) { Direction = ParameterDirection.Input, Value = orderForm["Name"] };
            var paramAddLine1 = new SqlParameter("@Line1", SqlDbType.NVarChar, 80) { Direction = ParameterDirection.Input, Value = orderForm["AddressLine1"] };
            var paramAddLine2 = new SqlParameter("@Line2", SqlDbType.NVarChar, 80) { Direction = ParameterDirection.Input, Value = orderForm["AddressLine2"] };
            var paramAddLine3 = new SqlParameter("@Line3", SqlDbType.NVarChar, 80) { Direction = ParameterDirection.Input, Value = orderForm["AddressLine3"] };
            var paramAddLine4 = new SqlParameter("@Line4", SqlDbType.NVarChar, 80) { Direction = ParameterDirection.Input, Value = orderForm["AddressLine4"] };
            var paramCity = new SqlParameter("@City", SqlDbType.NVarChar, 64) { Direction = ParameterDirection.Input, Value = orderForm["City"] };
            var paramPostalCode = new SqlParameter("@PostalCode", SqlDbType.NVarChar, 20) { Direction = ParameterDirection.Input, Value = orderForm["PostalCode"] };
            var paramCountryCode = new SqlParameter("@CountryCode", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = orderForm["CountryCode"] };
            var paramRegionCode = new SqlParameter("@RegionCode", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = orderForm["RegionCode"] };
            var paramPOBoxInd = new SqlParameter("@POBoxIndicator", SqlDbType.Bit) { Direction = ParameterDirection.Input, Value = bool.Parse(orderForm["IsPoBox"]) };
            var paramPhoneNum = new SqlParameter("@PhoneNumber", SqlDbType.NVarChar, 32) { Direction = ParameterDirection.Input, Value = orderForm["TelNumber"] };
            var paramEMail = new SqlParameter("@eMail", SqlDbType.NVarChar, 64) { Direction = ParameterDirection.Input, Value = orderForm["EmailAddress"] };
            var paramGiftWrapCode = new SqlParameter("@GiftWrapCode", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = orderForm["BTGiftWrapCode"] };
            var paramGiftWrapMessage = new SqlParameter("@GiftWrapMessage", SqlDbType.NVarChar, 300) { Direction = ParameterDirection.Input, Value = orderForm["BTGiftWrapString"] };
            var paramStoreShippingFee = new SqlParameter("@StoreShippingFeeIndicator", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = bool.Parse(orderForm["HasStoreShippingFee"])
            };
            var paramStoreGiftWrapFee = new SqlParameter("@StoreGiftWrapFeeIndicator", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = bool.Parse(orderForm["HasStoreGiftWrapFee"])
            };
            var paramStoreProccessingFee = new SqlParameter("@StoreProccessingFeeIndicator", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = bool.Parse(orderForm["HasStoreProccessingFee"])
            };
            var paramStoreOrderFee = new SqlParameter("@StoreOrderFeeIndicator", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = bool.Parse(orderForm["HasStoreOrderFee"])
            };
            var paramCostSummaryByIntAdmin = new SqlParameter("@CostSummaryByIntAdmin", SqlDbType.NVarChar, 2000) { Direction = ParameterDirection.Input, Value = orderForm["CostSummaryByIntAdmin"] };
            var paramCostSummaryByExtAdmin = new SqlParameter("@CostSummaryByExtAdmin", SqlDbType.NVarChar, 2000) { Direction = ParameterDirection.Input, Value = orderForm["CostSummaryByExtAdmin"] };
            var paramShippingMethodID = new SqlParameter("@ShippingMethodID", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = orderForm["BTShippingMethodGuid"]
            };
            var paramShippingMethodExId = new SqlParameter("@ShippingMethodExId", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = orderForm["ShippingMethodExtID"]
            };
            var paramCarrierCode = new SqlParameter("@CarrierCode", SqlDbType.NVarChar, 50)
            {
                Direction = ParameterDirection.Input,
                Value = orderForm["BTCarrierCode"]
            };
            var paramBackOrderIndicator = new SqlParameter("@BackOrderIndicator", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = bool.Parse(orderForm["BackOrderIndicator"])
            };
            var paramNewBasketName = new SqlParameter("@NewBasketName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
            var paramNewBasketSummaryID = new SqlParameter("@NewBasketSummaryID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
            var paramNewOEBasketName = new SqlParameter("@NewOEBasketName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
            var paramNewOEBasketSummaryID = new SqlParameter("@NewOEBasketSummaryID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };

            var paramIsVIP = new SqlParameter("@IsVIPCart", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = isVIP
            };

            var paramIsOrderAndHold = new SqlParameter("@OrderAndHoldShippingMethodIndicator", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = isOrderAndHold
            };
            var paramOrderedDownloadedUserId = new SqlParameter("@OrderedDownloadedUserId", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = orderedDownloadedUserId };
            command.Parameters.AddRange(new[]
                                            {
                                                paramBasketID, paramUserId, paramAccount, paramSpecialInstruction, paramIsHomeDelivery,
                                                paramHandlingTotal, paramShippingTotal, paramSubTotal, paramTaxTotal,
                                                paramTotal, paramName, paramAddLine1, paramAddLine2, paramAddLine3,
                                                paramAddLine4, paramCity, paramPostalCode, paramCountryCode, paramRegionCode,
                                                paramPOBoxInd, paramPhoneNum, paramEMail, paramGiftWrapCode,
                                                paramGiftWrapMessage, paramStoreShippingFee, paramStoreGiftWrapFee,
                                                paramStoreProccessingFee, paramStoreOrderFee, paramCostSummaryByIntAdmin, 
                                                paramCostSummaryByExtAdmin, paramShippingMethodID,
                                                paramShippingMethodExId, paramCarrierCode, paramBackOrderIndicator,
                                                paramNewBasketSummaryID, paramNewBasketName, 
                                                paramNewOEBasketName, paramNewOEBasketSummaryID, paramLoggedInUserId,
                                                paramIsVIP, paramIsOrderAndHold, paramOrderedDownloadedUserId
                                            });

            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();
                HandleCartException(command);

                newBasketName = command.Parameters["@NewBasketName"].Value as string;
                newBasketId = command.Parameters["@NewBasketSummaryID"].Value as string;
                newOEBasketName = command.Parameters["@NewOEBasketName"].Value as string;
                newOEBasketID = command.Parameters["@NewOEBasketSummaryID"].Value as string;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        internal DataSet GetStoreAndCustomerView(string cartId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_GET_BASKET_STORE_CUSTOMER_VIEW, dbConnection);
            var sqlParamater = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = cartId };
            command.Parameters.Add(sqlParamater);
            dbConnection.Open();
            try
            {
                var ds = new DataSet();
                var sqlDa = new SqlDataAdapter(command);
                sqlDa.Fill(ds);
                HandleCartException(command);
                return ds;
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public DataSet GetUserPreference(string userId)
        {
            var conn = this.CreateSqlConnection();
            var cmd = this.CreateSqlSpCommand(StoredProcedureName.PROC_GET_USER_DEFAULT_GRID_TEMPLATE, conn);

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

        public string GetOrderedDownloadedUser(string cartId)
        {
            var conn = this.CreateSqlConnection();
            var cmd = this.CreateSqlSpCommand(StoredProcedureName.PROC_GET_ORDER_DOWNLOADED_USER, conn);

            var sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", cartId);
            cmd.Parameters.AddRange(sqlParameters);
            conn.Open();
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            var result = string.Empty;
            try
            {
                da.Fill(ds);
                if (ds == null || ds.Tables == null || ds.Tables.Count <= 0
                    || ds.Tables[0].Rows == null || ds.Tables[0].Columns.Count <= 0
                    || ds.Tables[0].Rows.Count <= 0)
                    result = "";
                else
                    result = ds.Tables[0].Rows[0][0].ToString();

                HandleCartGridException(cmd);
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        protected void HandleCartGridException(SqlCommand command, string procName = "")
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
            if (!string.IsNullOrEmpty(procName)) exception.Source = procName + exception.Source;

            if (string.Compare(errorMessage, "Grid codes cannot be deleted because they are in use.", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, "The basket must be grid enabled.", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, CartGridDatabaseException.UNAUTHORIZED_TO_EDIT_OR_REPLACE_GRID_LINES_MESSAGE, StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, "Invalid parameters: Basket is not Open state.", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, "BasketState is not Open.", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, CartGridDatabaseException.CART_DUPLICATE_NAME, StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, "Replacement will create duplicate grid, cannot proceed", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(errorMessage, CartManagerException.INVALID_USERID, StringComparison.OrdinalIgnoreCase) == 0)
            {
                exception.IsBusinessError = true;
            }
            else if (string.Compare(errorMessage, "BTCart Folder is not existing.", StringComparison.OrdinalIgnoreCase) == 0)
            {
                exception = new CartGridDatabaseException(CartResource.BTCart_Folder_Not_Exist);
                exception.IsBusinessError = true;
            }
            else
                Logger.Write("CartGridDatabaseException", errorMessage);

            throw exception;
        }
        internal string GenerateNewBasketName(string basketName, string userId)
        {
            if (string.IsNullOrEmpty(basketName) || string.IsNullOrEmpty(userId))
            {
                return string.Empty;
            }
            string newBasketName = string.Empty;
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_GENERATE_NEW_BASKETNAME, dbConnection);

            //<Parameter>            
            var sqlParamaters = CreateSqlParamaters(3);

            sqlParamaters[0] = new SqlParameter("@BasketName", basketName);
            sqlParamaters[1] = new SqlParameter("@UserId", userId);
            sqlParamaters[2] = new SqlParameter("@NewBasketName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
            command.Parameters.AddRange(sqlParamaters);
            //
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();

                HandleCartException(command);

                newBasketName = command.Parameters["@NewBasketName"].Value.ToString();
            }
            finally
            {
                dbConnection.Close();
            }
            return newBasketName;
        }
        internal string CreateCart(string name, bool isPrimary, string folderId, List<CartAccountSummary> accountList, string userId, string gridTemplateId, int gridOptionId = 1, List<CommonGridTemplateLine> gridLines = null)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_CREATE_BASKET, dbConnection);

            var accSqlMetadata = DataConverter.GetUtblBasketOrderFormsDataRecords(accountList);

            //<Parameter>
            var paramName = new SqlParameter("@Basketname", SqlDbType.VarChar, 80) { Direction = ParameterDirection.Input, Value = name };
            var paramIsPrimary = new SqlParameter("@PrimaryIndicator", SqlDbType.Bit) { Direction = ParameterDirection.Input, Value = isPrimary };
            var paramFolderID = new SqlParameter("@UserFolderID", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = folderId };

            var paramAccounts = new SqlParameter("@BasketOrderForms", SqlDbType.Structured) { Direction = ParameterDirection.Input, Value = accSqlMetadata };

            var paramUserID = new SqlParameter("@BasketOwnerID", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
            var gridTemplateID = new SqlParameter("@GridTemplateId", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = (gridOptionId == 3 ? gridTemplateId : "") };
            //TFS19330 - code to add default grid distribution
            var gridDistributionOptionID = new SqlParameter("@GridDistributionOptionID", SqlDbType.SmallInt) { Direction = ParameterDirection.Input, Value = gridOptionId };
            var gridTemplateLinesInfo = new SqlParameter("@GridTemplateLinesInfo", SqlDbType.Structured) { Direction = ParameterDirection.Input, Value = (gridOptionId == 4 ? BT.TS360API.Common.Helpers.DataConverter.ConvertGridTemplateLinesToDataSetNew(gridLines) : null) };
            //            
            var outBasketUserID = new SqlParameter("@BasketUserID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };

            var cartID = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };

            command.Parameters.AddRange(new[] { paramName, paramIsPrimary, paramFolderID, paramAccounts, paramUserID, outBasketUserID, cartID, gridTemplateID, gridDistributionOptionID, gridTemplateLinesInfo });

            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();
                HandleCartException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            if (cartID.Value != DBNull.Value)
            {
                return DataAccessHelper.ConvertToString(cartID.Value);
            }

            return null;
        }
        internal void MoveLineItems(Dictionary<string, Dictionary<string, string>> lineItems, string sourceCartId, string destinationCartId, string userId, int maxLinesPerCart, out string PermissionViolationMessage)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.ProcTs360MoveLineItemsToNewCart, dbConnection);
            var sqlParameters = new SqlParameter[10];
            sqlParameters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) { Value = userId };
            sqlParameters[1] = new SqlParameter("@SourceBasketSummaryID", SqlDbType.NVarChar, 50) { Value = sourceCartId };
            sqlParameters[2] = new SqlParameter("@DestinationBasketSummaryID", SqlDbType.NVarChar, 50) { Value = destinationCartId };
            sqlParameters[3] = new SqlParameter("@BasketLineItems", SqlDbType.Structured)
            {
                Value = DataConverter.GetUtblProcTs360CopyLineItemsToNewCartDataRecords(lineItems).Distinct(new SqlDataRecordComparerByBTKey()).ToList()
            };
            sqlParameters[4] = new SqlParameter("@DeleteSourceLineItems", true);
            sqlParameters[5] = new SqlParameter("@BasketLineItemOrderStatus", SqlDbType.Char, 1) { Value = DEFAULT_ORDER_STATUS };
            sqlParameters[6] = new SqlParameter("@MaxLinesPerCartNumber", SqlDbType.Int) { Value = maxLinesPerCart };
            sqlParameters[7] = new SqlParameter("@All_or_MyQty_or_TitleOnly", DBNull.Value);
            sqlParameters[8] = new SqlParameter("@OverRide_Warning", DBNull.Value);
            sqlParameters[9] = new SqlParameter("@PermissionViolationMessage", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output };

            command.Parameters.AddRange(sqlParameters);

            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();

                HandleCartException(command);
                PermissionViolationMessage = command.Parameters["@PermissionViolationMessage"].Value as string;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        public DataSet GetCartForSubmitting(string cartId, string userId, out int nonRankedCount)
        {
            var ds = new DataSet();

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_BASKET_MANAGEMENT_GET_BASKET_BYID, dbConnection);

            //<Parameter>
            //var paramBasketID = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = cartId };
            //var paramUserID = new SqlParameter("@UserID", SqlDbType.NVarChar, 50) 
            //                    { Direction = ParameterDirection.Input, Value = string.IsNullOrEmpty(userId) ? DBNull.Value : userId };

            var sqlParameters = new SqlParameter[4];
            sqlParameters[0] = new SqlParameter("@BasketSummaryID", cartId);
            if (!string.IsNullOrEmpty(userId))
                sqlParameters[1] = new SqlParameter("@UserID", userId);
            else
            {
                sqlParameters[1] = new SqlParameter("@UserID", DBNull.Value);
            }

            sqlParameters[2] = new SqlParameter("@SubmitBasket", SqlDbType.Bit);
            sqlParameters[2].Value = true;

            sqlParameters[3] = new SqlParameter("@PendingRankCount", SqlDbType.Int) { Direction = ParameterDirection.Output };

            command.Parameters.AddRange(sqlParameters);

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

            nonRankedCount = DataAccessHelper.ConvertToInt(command.Parameters["@PendingRankCount"].Value);

            return ds;
        }

    }
}
