using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using BT.TS360API.ServiceContracts;

using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.Business.DAO;
using BT.ILSQueue.Business.Models;
using BT.ILSQueue.Business.Helpers;
using BT.ILSQueue.Business.MongDBLogger.ELMAHLogger;

namespace BT.ILSQueue.Business.Manager
{
    public class CartManager
    {
        #region Private Member

        private static volatile CartManager _instance;
        private static readonly object SyncRoot = new Object();
        public static CartManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CartManager();
                }

                return _instance;
            }
        }

        #endregion
        public void SubmitOrder(string cartID, string loggedInUserId, out string newBasketName, out string newBasketId, out string newOEBasketName, out string newOEBasketID, bool isVIP, bool isOrderAndHold, string orderedDownloadedUserId)
        {

            Cart cart = GetCartById(cartID, loggedInUserId);
            cart.OrderForm = GetStoreAndCustomerView(cartID);

            SubmitCart(cart, loggedInUserId, loggedInUserId, out newBasketName, out newBasketId, out newOEBasketName, out newOEBasketID, isVIP, isOrderAndHold, orderedDownloadedUserId);

        }

        public void  CreateNewILSCartWithErrorsItems(string userId, string currentCartId, Dictionary<string, Dictionary<string, string>> dictLineItems)
        {
            Cart currentCart = GetCartById(currentCartId, userId);
            string errorCartName = OrderDAO.Instance.GenerateNewBasketName(currentCart.CartName, userId);

            List<CartAccountSummary> cartAccountSummaries = null;
            if (currentCart.CartAccounts != null)
            {
                cartAccountSummaries = GetCartAccountDictionary(currentCart.CartAccounts);
            }

            var errorCartId = OrderDAO.Instance.CreateCart(errorCartName, false, currentCart.CartFolderID, cartAccountSummaries, userId, "");

            Cart errorCart = GetCartById(errorCartId, userId);

            string PermissionViolationMessage;

            OrderDAO.Instance.MoveLineItems(dictLineItems, currentCartId, errorCartId, userId,
                AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MaxLinesPerCart), out PermissionViolationMessage);
            
        }
        private void SubmitCart(Cart cart, string userId, string loggedInUserId, out string newBasketName, out string newBasketId, out string newOEBasketName, out string newOEBasketID, bool isVIP, bool isOrderAndHold, string orderedDownloadedUserId)
        {
            newBasketName = string.Empty;
            newBasketId = string.Empty;
            newOEBasketName = string.Empty;
            newOEBasketID = string.Empty;
            try
            {
                var accountDict = GetCartAccountDictionary(cart.CartAccounts);

                var orderForm = cart.OrderForm;
                var orderFormInfo = new Dictionary<string, string>();

                if (orderForm.HandlingTotal.HasValue) orderFormInfo.Add("HandlingTotal", orderForm.HandlingTotal.ToString());
                if (orderForm.ShippingTotal.HasValue) orderFormInfo.Add("ShippingTotal", orderForm.ShippingTotal.ToString());
                if (orderForm.SubTotal.HasValue) orderFormInfo.Add("SubTotal", orderForm.SubTotal.ToString());
                if (orderForm.TaxTotal.HasValue) orderFormInfo.Add("TaxTotal", orderForm.TaxTotal.ToString());
                if (orderForm.Total.HasValue) orderFormInfo.Add("Total", orderForm.Total.ToString());

                orderFormInfo.Add("IsHomeDelivery", orderForm.IsHomeDelivery.ToString());

                //orderFormInfo.Add("AddressID", orderForm.AddressID);
                orderFormInfo.Add("AddressLine1", orderForm.AddressLine1 ?? string.Empty);
                orderFormInfo.Add("AddressLine2", orderForm.AddressLine2 ?? string.Empty);
                orderFormInfo.Add("AddressLine3", orderForm.AddressLine3 ?? string.Empty);
                orderFormInfo.Add("AddressLine4", orderForm.AddressLine4 ?? string.Empty);
                orderFormInfo.Add("IsPoBox", orderForm.IsPoBox.ToString());
                orderFormInfo.Add("City", orderForm.City ?? string.Empty);
                orderFormInfo.Add("RegionCode", orderForm.RegionCode ?? string.Empty);
                orderFormInfo.Add("PostalCode", orderForm.PostalCode ?? string.Empty);
                orderFormInfo.Add("CountryCode", orderForm.CountryCode ?? string.Empty);
                orderFormInfo.Add("TelNumber", orderForm.TelNumber ?? string.Empty);
                orderFormInfo.Add("EmailAddress", orderForm.EmailAddress ?? string.Empty);

                orderFormInfo.Add("BTGiftWrapCode", orderForm.BTGiftWrapCode ?? string.Empty);
                orderFormInfo.Add("BTGiftWrapString", orderForm.BTGiftWrapString ?? string.Empty);

                orderFormInfo.Add("HasStoreShippingFee", orderForm.HasStoreShippingFee.ToString());
                orderFormInfo.Add("HasStoreGiftWrapFee", orderForm.HasStoreGiftWrapFee.ToString());
                orderFormInfo.Add("HasStoreProccessingFee", orderForm.HasStoreProccessingFee.ToString());
                orderFormInfo.Add("HasStoreOrderFee", orderForm.HasStoreOrderFee.ToString());

                orderFormInfo.Add("ShippingMethodExtID", orderForm.ShippingMethodExtID ?? string.Empty);
                orderFormInfo.Add("BTCarrierCode", orderForm.BTCarrierCode ?? string.Empty);
                orderFormInfo.Add("BTShippingMethodGuid", orderForm.BTShippingMethodGuid ?? string.Empty);
                orderFormInfo.Add("BackOrderIndicator", orderForm.IsBackOrder.ToString());
                orderFormInfo.Add("Name", orderForm.Name ?? string.Empty);
                orderFormInfo.Add("CostSummaryByExtAdmin", orderForm.CostSummaryByExtAdmin ?? string.Empty);
                orderFormInfo.Add("CostSummaryByIntAdmin", orderForm.CostSummaryByIntAdmin ?? string.Empty);

                OrderDAO.Instance.SubmitCart(cart.CartId, userId, loggedInUserId, orderFormInfo, accountDict, cart.SpecialInstruction,
                                            out newBasketName, out newBasketId, out newOEBasketName, out newOEBasketID, isVIP, isOrderAndHold, orderedDownloadedUserId);
            }
            catch (SqlException sqlEx)
            {
                sqlEx.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(sqlEx));
            }
        }

        private List<CartAccountSummary> GetCartAccountDictionary(List<CartAccount> accounts)
        {
            if (accounts == null || accounts.Count == 0)
                return null;

            var cartAccountSummaries = new List<CartAccountSummary>();

            foreach (var cartAccount in accounts)
            {
                var cartAccountSummary = new CartAccountSummary()
                {
                    AccountID = cartAccount.AccountID,
                    BasketAccountTypeID = DataConverter.GetNewAccountTypeID(cartAccount.AccountType).ToString(),
                    PONumber = cartAccount.PONumber,
                    AccountAlias = cartAccount.AccountAlias
                };

                cartAccountSummaries.Add(cartAccountSummary);
            }

            return cartAccountSummaries;
        }
        private  OrderForm GetStoreAndCustomerView(string cartId)
        {
            try
            {
                var orderFormDs = OrderDAO.Instance.GetStoreAndCustomerView(cartId);

                //return GetStoreAndCustomerViewFromDataSet(orderFormDs);
                return orderFormDs;
            }
            catch (SqlException sqlEx)
            {
                sqlEx.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(sqlEx));
            }
            return null;
        }

        /*private  OrderForm GetStoreAndCustomerViewFromDataSet(DataSet ds)
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
        }*/

        private Cart GetCartById(string cartId, string userID)
        {

            var cart = GetCart(cartId, userID);
         
            return cart;
        }

        private Cart GetCart(string cartId, string userId = null)
        {
            if (string.IsNullOrEmpty(cartId)) return null;

            Cart cart = null;
            int nonRankedCount = 0;
            var cartDs = OrderDAO.Instance.GetCart(cartId, userId, out nonRankedCount);
            cart = GetCartSummaryFromDataSet(cartDs);
            cart.NonRankedCount = nonRankedCount;
            return cart;
        }

        private static Cart GetCartSummaryFromDataSet(DataSet ds, bool isPrimaryCall = false)
        {
            if (ds == null ||
                ds.Tables.Count < 2 ||
                ds.Tables[0].Rows.Count == 0)
                return null;

            //Primary Cart information
            var cartInfoRow = ds.Tables[0].Rows[0];

            var cart = GetCartSummaryFromDataRow(cartInfoRow, isPrimaryCall);

            if (cart != null)
            {
                //Accounts
                foreach (DataRow accountRow in ds.Tables[1].Rows)
                {
                    var cartAccount = GetCartAccountFromDataRow(accountRow);

                    if (cartAccount != null)
                        cart.CartAccounts.Add(cartAccount);
                }
            }

            return cart;
        }

        private static Cart GetCartSummaryFromDataRow(DataRow row, bool isPrimaryCall = false)
        {
            var strCartId = DataAccessHelper.ConvertToString(row["BasketSummaryID"]);
            var strUserId = DataAccessHelper.ConvertToString(row["BasketOwnerID"]);
            var strFolderId = DataAccessHelper.ConvertToString(row["UserFolderID"]);

            if (String.IsNullOrEmpty(strCartId) || String.IsNullOrEmpty(strUserId))
                return null;

            var cartStatus = DataAccessHelper.ConvertCartStatus(row["BasketStateID"]);

            var cart = new Cart(strCartId, strUserId)
            {
                CartName = DataConverter.ConvertTo<string>(row, "BasketName"),
                CartOwnerId = DataConverter.ConvertTo<string>(row, "BasketOwnerID"),
                CartUserSharedId = DataConverter.ConvertTo<string>(row, "UserID"),
                BTStatus = cartStatus,
                CartFolderID = strFolderId,
                LineItemCount = DataAccessHelper.ConvertToInt(row["TotalOrderLineCount"]),
                CartTotalListPrice = DataAccessHelper.ConvertTodecimal(row["TotalListPrice"]),
                CartTotalNetPrice = DataAccessHelper.ConvertTodecimal(row["TotalNetPrice"]),
                ShippingTotal = DataAccessHelper.ConvertTodecimal(row["ShippingTotal"]),
                FeeTotal = DataAccessHelper.ConvertTodecimal(row["FeeTotal"]),
                Total = DataAccessHelper.ConvertTodecimal(row["Total"]),
                TotalOrderQuantity = DataAccessHelper.ConvertToInt(row["TotalOrderQuantity"]),
                TotalCancelQuantity = DataConverter.ConvertTo<int>(row, "TotalCancelQuantity"),
                TotalBackOrderQuantity = DataConverter.ConvertTo<int>(row, "TotalBackOrderQuantity"),
                TotalInProcessQuantity = DataConverter.ConvertTo<int>(row, "TotalInProcessQuantity"),
                Note = DataAccessHelper.ConvertToString(row["Note"]),
                SpecialInstruction = DataAccessHelper.ConvertToString(row["SpecialInstructions"]),
                CreatedBy = DataAccessHelper.ConvertToString(row["CreatedBy"]),
                UpdatedBy = DataAccessHelper.ConvertToString(row["UpdatedBy"]),
                CreatedDateTime = DataAccessHelper.ConvertToDateTime(row["CreatedDateTime"]),
                UpdatedDateTime = DataAccessHelper.ConvertToDateTime(row["UpdatedDateTime"]),
                NoteUpdatedBy = DataAccessHelper.ConvertToString(row["NoteUpdatedBy"]),
                NoteUpdatedDateTime = DataAccessHelper.ConvertToDateTime(row["NoteUpdatedDateTime"]),
                SpecialInstructionsUpdatedBy = DataAccessHelper.ConvertToString(row["SpecialInstructionsUpdatedBy"]),
                SpecialInstructionsUpdatedDateTime = DataAccessHelper.ConvertToDateTime(row["SpecialInstructionsUpdatedDateTime"]),
                TotalOELines = row.Table.Columns.Contains("TotalOELines") ? DataAccessHelper.ConvertToInt(row["TotalOELines"]) : 0,
                TotalOEQuantity = row.Table.Columns.Contains("TotalOEQuantity") ? DataAccessHelper.ConvertToInt(row["TotalOEQuantity"]) : 0,
                HasProfile = row.Table.Columns.Contains("HasProfile") && DataAccessHelper.ConvertToBool(row["HasProfile"]),
                IsShared = row.Table.Columns.Contains("IsShared") && DataAccessHelper.ConvertToBool(row["IsShared"]),
                HasGridLine = row.Table.Columns.Contains("HasGrids") && DataAccessHelper.ConvertToBool(row["HasGrids"]),
                IsPremium = row.Table.Columns.Contains("IsPremium") && DataAccessHelper.ConvertToBool(row["IsPremium"]),
                HasPermission = row.Table.Columns.Contains("HasPermission") && DataAccessHelper.ConvertToBool(row["HasPermission"]),
                CartOwnerName = DataConverter.ConvertTo<string>(row, "BasketOwnerName"),
                IsArchived = row.Table.Columns.Contains("ArchivedIndicator") && DataAccessHelper.ConvertToBool(row["ArchivedIndicator"]),
                CartFolderName = DataConverter.ConvertTo<string>(row, "FolderName"),
                TitlesWithoutGrids = row.Table.Columns.Contains("TitlesWithoutGrids") ? DataAccessHelper.ConvertToInt(row["TitlesWithoutGrids"]) : 0,
                EntertainmentHasGridLine = row.Table.Columns.Contains("EntertainmentHasGrids") && DataAccessHelper.ConvertToBool(row["EntertainmentHasGrids"]),
                WorkflowTimeZone = DataConverter.ConvertTo<string>(row, "TimeZone"),
                OneClickMARCIndicator = row.Table.Columns.Contains("OneClickMARCIndicator") && DataAccessHelper.ConvertToBool(row["OneClickMARCIndicator"]),
                FTPErrorMessage = row.Table.Columns.Contains("FTPErrorMessage") ? DataAccessHelper.ConvertToString(row["FTPErrorMessage"]) : "",
                HasOwner = row.Table.Columns.Contains("HasOwner") && DataAccessHelper.ConvertToBool(row["HasOwner"]),
                HasContribution = row.Table.Columns.Contains("HasContribution") && DataAccessHelper.ConvertToBool(row["HasContribution"]),
                HasReview = row.Table.Columns.Contains("HasReview") && DataAccessHelper.ConvertToBool(row["HasReview"]),
                HasAcquisition = row.Table.Columns.Contains("HasAquisition") && DataAccessHelper.ConvertToBool(row["HasAquisition"]),
                HasWorkflow = row.Table.Columns.Contains("HasWorkflow") && DataAccessHelper.ConvertToBool(row["HasWorkflow"]),
                HasReviewAcquisitionPermission = row.Table.Columns.Contains("HasReviewAcquisitionPermission") && DataAccessHelper.ConvertToBool(row["HasReviewAcquisitionPermission"]),
                IsMixedProduct = DataAccessHelper.ConvertToBool(row["IsMixedProduct"]),
                ESPStateTypeId = row.Table.Columns.Contains("ESPStateTypeId") ? DataAccessHelper.ConvertToInt(row["ESPStateTypeId"]) : (int)ESPStateType.None,
                SubmittedDate = row.Table.Columns.Contains("SubmittedDate") ? DataAccessHelper.ConvertToDateTime(row["SubmittedDate"]) : null,
                HasESPRanking = row.Table.Columns.Contains("HasESPRanking") && DataAccessHelper.ConvertToBool(row["HasESPRanking"]),
                QuoteID = row.Table.Columns.Contains("QuoteID") ? DataAccessHelper.ConvertToInt(row["QuoteID"]) : 0,
                QuoteExpiredDateTime = row.Table.Columns.Contains("QuoteExpiredDateTime") ? DataAccessHelper.ConvertToDateTime(row["QuoteExpiredDateTime"]) : null,
                IsSplitting = row.Table.Columns.Contains("IsSplitting") && DataAccessHelper.ConvertToBool(row["IsSplitting"]),
                IsSharedBasketGridEnabled = row.Table.Columns.Contains("IsSharedBasketGridEnabled") && DataAccessHelper.ConvertToBool(row["IsSharedBasketGridEnabled"]),
                IsActive = row.Table.Columns.Contains("IsBasketActive") && DataAccessHelper.ConvertToBool(row["IsBasketActive"]),
                BasketProcessingCategoryID = row.Table.Columns.Contains("BasketProcessingCategoryID") ? DataAccessHelper.ConvertToInt(row["BasketProcessingCategoryID"]) : 0,
                ESPRankStateTypeId = row.Table.Columns.Contains("ESPRankStateTypeId") ? DataAccessHelper.ConvertToInt(row["ESPRankStateTypeId"]) : (int)ESPStateType.None,
                ESPDistStateTypeID = row.Table.Columns.Contains("ESPDistStateTypeID") ? DataAccessHelper.ConvertToInt(row["ESPDistStateTypeID"]) : (int)ESPStateType.None,
                ESPFundStateTypeID = row.Table.Columns.Contains("ESPFundStateTypeID") ? DataAccessHelper.ConvertToInt(row["ESPFundStateTypeID"]) : (int)ESPStateType.None,
                LastESPStateTypeLiteral = row.Table.Columns.Contains("LastESPStateType") ? DataAccessHelper.ConvertToString(row["LastESPStateType"]) : string.Empty,
                FreezeLevel = row.Table.Columns.Contains("BasketFrozenLevelIndicator") ? DataAccessHelper.ConvertToInt(row["BasketFrozenLevelIndicator"]) : 0,
                OrderedDownloadedUser = DataAccessHelper.ConvertToString(row["OrderedDownloadedUser"]),
                GridDistributionOption = DataAccessHelper.ConvertToInt(row["GridDistributionOptionID"])
            };

            cart.CurrentWorkflow = row.Table.Columns.Contains("CurrentWorkflow") ? GetCurrentWorkFlow(row["CurrentWorkflow"]) : 0;
            cart.IsPrimary = isPrimaryCall || DataAccessHelper.ConvertToBool(row["IsPrimary"]);

            RefineContainsMixGridNonGridForCart(cart);

            return cart;
        }
        private static CartAccount GetCartAccountFromDataRow(DataRow row)
        {
            var strAccountId = DataAccessHelper.ConvertToString(row["AccountID"]);
            var strBasketSummaryId = DataAccessHelper.ConvertToString(row["BasketSummaryID"]);

            if (String.IsNullOrEmpty(strAccountId) || String.IsNullOrEmpty(strBasketSummaryId))
                return null;

            var cartAcc = new CartAccount()
            {
                AccountID = strAccountId,
                AccountType = DataConverter.GetOldAccountTypeID(DataAccessHelper.ConvertToInt(row["BasketAccountTypeID"])),
                ESupplierID = DataAccessHelper.ConvertToString(row["ESupplierID"]),
                AccountERPNumber = DataAccessHelper.ConvertToString(row["AccountERPNumber"]),
                PONumber = DataAccessHelper.ConvertToString(row["PONumber"]),
                BasketSummaryID = strBasketSummaryId,
                ERPAccountGUID = DataAccessHelper.ConvertToString(row["ERPAccountGUID"]),
                AccountAlias = DataAccessHelper.ConvertToString(row["AccountAlias"]),
                IsHomeDelivery = row.Table.Columns.Contains("IsHomeDelivery") && DataAccessHelper.ConvertToBool(row["IsHomeDelivery"]),
                NumberOfBuilding = GetNumberOfBuilding(row["NumberOfBuilding"]),
                ProcessingCharge = DataAccessHelper.ConvertTodecimal(row["ProcessingCharges"]),
            };

            return cartAcc;
        }

        private static int GetCurrentWorkFlow(object p)
        {
            var valueString = p.ToString();

            if (valueString == "1" ||
                string.Compare(valueString, "Contribution", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return 1;
            }
            else if (valueString == "2" ||
               string.Compare(valueString, "Requisition", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return 2;
            }
            else if (valueString == "3" ||
               string.Compare(valueString, "Review", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return 3;
            }
            else if (valueString == "4" ||
               string.Compare(valueString, "Acquisition", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return 4;
            }
            else
            {
                return 0;
            }

        }

        private static void RefineContainsMixGridNonGridForCart(Cart cart)
        {
            cart.ContainsAMixOfGridNNonGrid = cart.HasGridLine && cart.TitlesWithoutGrids > 0 && cart.LineItemCount != cart.TitlesWithoutGrids;
        }

        private static int GetNumberOfBuilding(object obj)
        {
            int returnValue = 1;
            if (null != obj)
            {
                Int32.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }
    }
}
