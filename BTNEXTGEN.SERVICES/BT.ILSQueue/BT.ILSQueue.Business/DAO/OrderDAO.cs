using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BT.ILSQueue.Business.Helpers;
using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.Business.Models;

using BT.TS360API.ServiceContracts;

namespace BT.ILSQueue.Business.DAO
{
    class OrderDAO: BaseDAO 
    {
        private static volatile OrderDAO _instance;
        private static readonly object SyncRoot = new Object();
        private const char DEFAULT_ORDER_STATUS = 'A';

        public static OrderDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new OrderDAO();
                }

                return _instance;
            }
        }

        #region Public Property
        public override string ConnectionString
        {
            get { return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.Orders_ConnectionString); }
        }

        public int GetILSQueuePendingOrderCount()
        {
            using (var dbConnection = CreateSqlConnection())
            {
                using (var command = CreateSqlSpCommand(StoredProcedureName.PROC_GET_ILS_QUEUE_PENDING_ORDER_COUNT, dbConnection))
                {
                    dbConnection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return DataAccessHelper.ConvertToInt(reader["ILSPendingOrderCount"]);
                        }
                    }

                    return 0;
                }
            }
        }

        public DataSet GetILSQueuePendingOrders(bool enabledPolaris)
        {
            DataSet result = new DataSet();
            
            if (enabledPolaris)
                result = GetILSQueuePendingOrders(ApplicationConstants.POLARIS_VENDOR_CODE);
            
            return result;
        }

        public DataSet GetILSQueuePendingOrders(string ilsVendorCode)
        {
            var conn = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_GET_ILS_QUEUE_PENDING_ORDERS, conn);
           
            var sqlParamaters = CreateSqlParamaters(2);
            sqlParamaters[0] = new SqlParameter("@ILSVendorID", SqlDbType.VarChar, 25) { SqlValue = ilsVendorCode };

            sqlParamaters[1] = new SqlParameter("@PendingQueueCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            
            command.Parameters.AddRange(sqlParamaters);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            try
            {
                conn.Open();
                sqlDa.Fill(ds);
                HandleException(command);
            }
            catch (SqlException ex)
            {
               
                throw ex;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return ds;

        }

        public void SetILSBasketState(string basketSummaryID, string userId, CartStatus basketState, ILSState ilsStatusId, string ILSMarcProfileId, string vendorCode, 
            string locationCode, string orderedDownloadedUserId)
        {
            var conn = CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_SET_ILS_BASKET_STATE, conn);

            var sqlParameter = new SqlParameter[8];
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

            if (string.IsNullOrEmpty(locationCode))
                sqlParameter[6] = new SqlParameter("@LocationCode", DBNull.Value);
            else
                sqlParameter[6] = new SqlParameter("@LocationCode", locationCode);

            if (string.IsNullOrEmpty(orderedDownloadedUserId))
                sqlParameter[7] = new SqlParameter("@OrderedDownloadedUserID", DBNull.Value);
            else
                sqlParameter[7] = new SqlParameter("@OrderedDownloadedUserID", orderedDownloadedUserId);

            cmd.Parameters.AddRange(sqlParameter);

            conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                //HandleCartException(cmd);
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
               // HandleCartException(cmd);
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
               // HandleCartException(cmd);
            }
            finally
            {
                conn.Close();
            }

        }

        public void SaveILSLineItemDetails(string userId, List<ILSLineItemDetail> lstILSLineItemDetail)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_SET_ILS_BASKET_LINES_GRIDS, dbConnection);

            //<Parameter>
            SqlParameter[] sqlParameter = {
                                              new SqlParameter("@BasketLineItemGrids", SqlDbType.Structured),
                                              new SqlParameter("@UserID", userId)
                                          };

            sqlParameter[0].TypeName = "dbo.utblILSSetBasketLinesGrids";
            sqlParameter[0].Value = DataConverter.ConvertILSLineItemDetailToDataTable(lstILSLineItemDetail);
            command.Parameters.AddRange(sqlParameter);

            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();
                //HandleOrderException(command);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public OrderForm GetStoreAndCustomerView(string cartId)
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
                //HandleCartException(command);
                return GetStoreAndCustomerViewFromDataSet(ds);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        public void SubmitCart(string cartId, string userId, string loggedInUserId, Dictionary<string, string> orderForm, List<CartAccountSummary> accountDict, string specialInstruction,
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
                //HandleCartException(command);

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

        public DataSet GetCart(string cartId, string userId, out int nonRankedCount)
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
        public DataSet GetBTKeysByLineItemIDs(string cartId, List<string> basketLineItemIDs)
        {
            var conn = CreateSqlConnection();
            var cmd = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_GET_BTKEYS_BY_LINEITEMS, conn);

            var sqlParameter = new SqlParameter[2];
            sqlParameter[0] = new SqlParameter("@BasketSummaryID", cartId);
            sqlParameter[1] = new SqlParameter("@BasketLineItemIDs", SqlDbType.Structured) { Value = DataAccessHelper.GenerateDataRecords(basketLineItemIDs, "BasketLineItemID") };

            cmd.Parameters.AddRange(sqlParameter);
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            conn.Open();
            try
            {
                da.Fill(ds);
              //  HandleCartGridException(cmd);
            }
            finally
            {
                conn.Close();
            }

            return ds;
        }

        public string GenerateNewBasketName(string basketName, string userId)
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

               // HandleCartException(command);

                newBasketName = command.Parameters["@NewBasketName"].Value.ToString();
            }
            finally
            {
                dbConnection.Close();
            }
            return newBasketName;
        }

        public string CreateCart(string name, bool isPrimary, string folderId, List<CartAccountSummary> accountList, string userId, string gridTemplateId, int gridOptionId = 1)
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
            var gridTemplateLinesInfo = new SqlParameter("@GridTemplateLinesInfo", SqlDbType.Structured) { Direction = ParameterDirection.Input, Value = null };
            //            
            var outBasketUserID = new SqlParameter("@BasketUserID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };

            var cartID = new SqlParameter("@BasketSummaryID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };

            command.Parameters.AddRange(new[] { paramName, paramIsPrimary, paramFolderID, paramAccounts, paramUserID, outBasketUserID, cartID, gridTemplateID, gridDistributionOptionID, gridTemplateLinesInfo });

            dbConnection.Open();
            try
            {
                command.ExecuteNonQuery();
               // HandleCartException(command);
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

        public void MoveLineItems(Dictionary<string, Dictionary<string, string>> lineItems, string sourceCartId, string destinationCartId, string userId, int maxLinesPerCart, out string PermissionViolationMessage)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.PROC_TS360_MOVE_LINE_ITEMS_TO_NEW_CART, dbConnection);
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

                //HandleCartException(command);
                PermissionViolationMessage = command.Parameters["@PermissionViolationMessage"].Value as string;
            }
            finally
            {
                dbConnection.Close();
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
                //HandleCartException(cmd);
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region private method

        private static OrderForm GetStoreAndCustomerViewFromDataSet(DataSet ds)
        {
            if (ds == null) return null;
            if (ds.Tables.Count < 4) return null;

            var totalTable = ds.Tables[0];
            var addressTable = ds.Tables[1];
            var accountTable = ds.Tables[2];
            var creditCartTable = ds.Tables[3];

            OrderForm orderform = null;
            if (totalTable.Rows.Count > 0)
            {
                var row = totalTable.Rows[0];

                orderform = new OrderForm()
                {
                    HandlingTotal = DataAccessHelper.ConvertTodecimal(row["HandlingTotal"]),
                    ShippingTotal = DataAccessHelper.ConvertTodecimal(row["ShippingTotal"]),
                    SubTotal = DataAccessHelper.ConvertTodecimal(row["SubTotal"]),
                    TaxTotal = DataAccessHelper.ConvertTodecimal(row["TaxTotal"]),
                    Total = DataAccessHelper.ConvertTodecimal(row["Total"]),
                    IsHomeDelivery = DataAccessHelper.ConvertToBool(row["IsHomeDelivery"]),
                };

                if (addressTable.Rows.Count > 0)
                {
                    row = addressTable.Rows[0];
                    orderform.AddressID = DataAccessHelper.ConvertToString(row["Address_ID"]);
                    orderform.AddressLine1 = DataAccessHelper.ConvertToString(row["Line1"]);
                    orderform.AddressLine2 = DataAccessHelper.ConvertToString(row["Line2"]);
                    orderform.AddressLine3 = DataAccessHelper.ConvertToString(row["Line3"]);
                    orderform.AddressLine4 = DataAccessHelper.ConvertToString(row["Line4"]);
                    orderform.IsPoBox = DataAccessHelper.ConvertToBool(row["IsPOBox"]);
                    orderform.City = DataAccessHelper.ConvertToString(row["City"]);
                    orderform.RegionCode = DataAccessHelper.ConvertToString(row["Region_Code"]);
                    orderform.PostalCode = DataAccessHelper.ConvertToString(row["Postal_Code"]);
                    orderform.CountryCode = DataAccessHelper.ConvertToString(row["Country_Code"]);
                    orderform.TelNumber = DataAccessHelper.ConvertToString(row["tel_number"]);
                    orderform.EmailAddress = DataAccessHelper.ConvertToString(row["email_address"]);

                    orderform.ShippingMethodExtID = DataAccessHelper.ConvertToString(row["ShippingMethodExtID"]);
                    orderform.BTCarrierCode = DataAccessHelper.ConvertToString(row["BTCarrierCode"]);
                    orderform.BTShippingMethodGuid = DataAccessHelper.ConvertToString(row["BTShippingMethodGuid"]);

                    orderform.HasStoreShippingFee = DataAccessHelper.ConvertToBool(row["HasStoreShippingFee"]);
                    orderform.HasStoreGiftWrapFee = DataAccessHelper.ConvertToBool(row["HasStoreGiftWrapFee"]);
                    orderform.HasStoreProccessingFee = DataAccessHelper.ConvertToBool(row["HasStoreProccessingFee"]);
                    orderform.HasStoreOrderFee = DataAccessHelper.ConvertToBool(row["HasStoreOrderFee"]);

                    orderform.BTGiftWrapCode = DataAccessHelper.ConvertToString(row["BTGiftWrapCode"]);
                    orderform.BTGiftWrapString = DataAccessHelper.ConvertToString(row["BTGiftWrapMessage"]);
                }

                if (accountTable.Rows.Count > 0)
                {
                    row = accountTable.Rows[0];
                    orderform.CartAccount.AccountERPNumber = DataAccessHelper.ConvertToString(row["AccountERPNumber"]);
                    orderform.CartAccount.AccountAlias = DataAccessHelper.ConvertToString(row["AccountAlias"]);
                    orderform.CartAccount.AccountType = DataAccessHelper.ConvertToInt(row["AccountType"]);
                    orderform.CartAccount.AccountID = DataAccessHelper.ConvertToString(row["AccountId"]);
                }

                if (creditCartTable.Rows.Count > 0)
                {
                    row = creditCartTable.Rows[0];
                    orderform.CreditCard.CreditCardId = DataAccessHelper.ConvertToString(row["CreditCardId"]);
                    orderform.CreditCard.ExpirationYear = DataAccessHelper.ConvertToInt(row["ExpirationYear"]);
                    orderform.CreditCard.ExpirationMonth = DataAccessHelper.ConvertToInt(row["ExpirationMonth"]);
                    orderform.CreditCard.CreditCardNumber = DataAccessHelper.ConvertToString(row["CreditCardNumber"]);
                    orderform.CreditCard.CreditCardIdentifier = DataAccessHelper.ConvertToString(row["CreditCardIdentifier"]);
                    orderform.CreditCard.BTCreditCardToken = DataAccessHelper.ConvertToString(row["BTCreditCardToken"]);
                    orderform.CreditCard.Alias = DataAccessHelper.ConvertToString(row["Alias"]);
                    orderform.CreditCard.CreditCardType = DataAccessHelper.ConvertToString(row["CreditCardType"]);
                    orderform.CreditCard.BillingAddressId = DataAccessHelper.ConvertToString(row["BillingAddressId"]);
                }
            }

            return orderform;
        }

        #endregion
    }
}
