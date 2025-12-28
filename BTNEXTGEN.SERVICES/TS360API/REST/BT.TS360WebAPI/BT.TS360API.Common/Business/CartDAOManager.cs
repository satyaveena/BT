using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ExternalServices;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360API.Common.Models;
using BT.TS360API.Common.CartFramework;
using BT.TS360Constants;

namespace BT.TS360API.Common.DataAccess
{
    public class CartDAOManager
    {
        private static volatile CartDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        public static CartDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CartDAOManager();
                }

                return _instance;
            }
        }
        
        #region Move Wcf to Api
        public static bool HideEspAutoRankMessage(HideEspAutoRankMessageRequest request)
        {
            return CartDAO.Instance.HideEspAutoRankMessage(request);
        }

        public static QuickCart GetCartDetailsQuickView(string cartId, string userId, string sortBy, byte sortDirection, short pageSize, int pageNumber, bool recalculateHeader)
        {
            try
            {
                var quickCart = new QuickCart(cartId, userId);
                int nonRankedCount = 0;
                var ds = CartDAO.Instance.GetCartDetailsQuickView(out nonRankedCount, cartId, userId, sortBy, sortDirection, pageSize, pageNumber, recalculateHeader);
                quickCart.NonRankedCount = nonRankedCount;
                if (ds != null && ds.Tables.Count > 0)
                {
                    quickCart.LineItems = new List<QuickLineItem>();
                    var lineItemTable = ds.Tables[0];
                    foreach (DataRow row in lineItemTable.Rows)
                    {
                        var lineItem = new QuickLineItem();
                        lineItem.LineItemID = DataAccessHelper.ConvertToString(row["BasketLineItemID"]);
                        lineItem.BTKey = DataAccessHelper.ConvertToString(row["BTKey"]);
                        lineItem.ListPrice = DataAccessHelper.ConvertTodecimal(row["ListPrice"]);
                        lineItem.NetPrice = DataAccessHelper.ConvertTodecimal(row["NetPrice"]);
                        lineItem.IsGridded = DataAccessHelper.ConvertToBool(row["IsGridded"]);
                        lineItem.Quantity = DataAccessHelper.ConvertToInt(row["Quantity"]);
                        lineItem.Discount = DataAccessHelper.ConvertTodecimal(row["Discount"]);
                        lineItem.BasketOriginalEntryID = DataAccessHelper.ConvertToString(row["BasketOriginalEntryID"]);
                        lineItem.ProductType = string.Empty;
                        if (!string.IsNullOrEmpty(lineItem.BasketOriginalEntryID))
                        {
                            lineItem.Title = DataAccessHelper.ConvertToString(row["Title"]);
                            lineItem.Author = DataAccessHelper.ConvertToString(row["ResponsibleParty"]);
                            lineItem.ISBN = DataAccessHelper.ConvertToString(row["ISBN"]);
                            lineItem.UPC = DataAccessHelper.ConvertToString(row["UPC"]);
                            lineItem.Format = DataAccessHelper.ConvertToString(row["FormatLiteral"]);
                            lineItem.PublishedDate = DataAccessHelper.ConvertToDateTime(row["PublicationDate"]);
                        }
                        quickCart.LineItems.Add(lineItem);
                    }
                    if (ds.Tables.Count > 1)
                    {
                        var cartHeaderTable = ds.Tables[1];
                        if (cartHeaderTable.Rows.Count > 0)
                        {
                            quickCart.CartInfo = new QuickCartInfo();
                            var liRow = cartHeaderTable.Rows[0];
                            quickCart.CartInfo.CartStatus = DataAccessHelper.ConvertToString(liRow["CartStatus"]);
                            quickCart.CartInfo.CartOwnerID = DataAccessHelper.ConvertToString(liRow["CartOwner"]);
                            quickCart.CartInfo.CartName = DataAccessHelper.ConvertToString(liRow["CartName"]);
                            quickCart.CartInfo.TotalLines = DataAccessHelper.ConvertToInt(liRow["TotalLines"]);
                            quickCart.CartInfo.TotalQuantity = DataAccessHelper.ConvertToLong(liRow["TotalQuantity"]);
                            quickCart.CartInfo.TotalListPrice = DataAccessHelper.ConvertTodecimal(liRow["TotalListPrice"]);
                            quickCart.CartInfo.TotalNetPrice = DataAccessHelper.ConvertTodecimal(liRow["TotalNetPrice"]);
                            quickCart.CartInfo.IsShared = DataAccessHelper.ConvertToBool(liRow["IsShared"]);
                            quickCart.CartInfo.IsPricingComplete = DataAccessHelper.ConvertToBool(liRow["IsPricingComplete"]);
                            quickCart.CartInfo.OneClickMARCIndicator = DataAccessHelper.ConvertToBool(liRow["OneClickMARCIndicator"]);
                            quickCart.CartInfo.FTPErrorMessage = DataAccessHelper.ConvertToString(liRow["FTPErrorMessage"]);
                            quickCart.CartInfo.ESPRankStateTypeId = liRow.Table.Columns.Contains("ESPRankStateTypeId") ? DataAccessHelper.ConvertToInt(liRow["ESPRankStateTypeId"]) : (int)ESPStateType.None;
                            quickCart.CartInfo.ESPDistStateTypeID = liRow.Table.Columns.Contains("ESPDistStateTypeID") ? DataAccessHelper.ConvertToInt(liRow["ESPDistStateTypeID"]) : (int)ESPStateType.None;
                            quickCart.CartInfo.ESPFundStateTypeID = liRow.Table.Columns.Contains("ESPFundStateTypeID") ? DataAccessHelper.ConvertToInt(liRow["ESPFundStateTypeID"]) : (int)ESPStateType.None;
                            quickCart.CartInfo.LastESPStateTypeLiteral = liRow.Table.Columns.Contains("LastESPStateType") ? DataAccessHelper.ConvertToString(liRow["LastESPStateType"]) : string.Empty;
                            quickCart.CartInfo.FreezeLevel = liRow.Table.Columns.Contains("BasketFrozenLevelIndicator") ? DataAccessHelper.ConvertToInt(liRow["BasketFrozenLevelIndicator"]) : 0;
                            quickCart.CartInfo.IsMixedProduct = liRow.Table.Columns.Contains("IsMixedProduct") ? DataAccessHelper.ConvertToBool(liRow["IsMixedProduct"]) : false;
                            quickCart.CartInfo.IsBasketActive = liRow.Table.Columns.Contains("IsBasketActive") ? DataAccessHelper.ConvertToBool(liRow["IsBasketActive"]) : false;
                            quickCart.CartInfo.CartFolderId = DataAccessHelper.ConvertToString(liRow["CartFolderID"]);
                            quickCart.CartInfo.IsArchived = liRow.Table.Columns.Contains("IsArchived") ? DataAccessHelper.ConvertToBool(liRow["IsArchived"]) : false;
                        }
                    }
                }
                return quickCart;
            }
            catch (SqlException sqlEx)
            {
                //HandleSqlException(sqlEx);
                return null;
            }
        }

        public static List<AccountSummary> GetAccountsSummary(string cartId)
        {
            List<AccountSummary> accounts = null;
            try
            {
                DataSet result = CartDAO.Instance.GetAccountsSummary(cartId);
                accounts = GetAccountsSummaryFromDataSet(result);
            }
            catch (SqlException sqlEx)
            {
                //HandleSqlException(sqlEx);
            }

            return accounts;
        }

        public static List<AccountSummary> GetAccountsSummaryFromDataSet(DataSet ds)
        {
            if (ds == null ||
                ds.Tables.Count < 1 ||
                ds.Tables[0].Rows.Count == 0)
                return null;

            var list = new List<AccountSummary>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var account = new AccountSummary();
                account.BasketOrderFormId = DataAccessHelper.ConvertToString(row["BasketOrderFormId"]);
                account.AccountID = DataAccessHelper.ConvertToString(row["AccountID"]);
                account.AccountAlias = DataAccessHelper.ConvertToString(row["AccountAlias"]);
                account.AccountType = DataConverter.GetOldAccountTypeID(DataAccessHelper.ConvertToInt(row["BasketAccountTypeID"]));
                account.ESupplierID = DataAccessHelper.ConvertToString(row["ESupplierID"]);
                account.AccountERPNumber = DataAccessHelper.ConvertToString(row["AccountERPNumber"]);
                account.ERPAccountGUID = DataAccessHelper.ConvertToString(row["ERPAccountGUID"]);
                account.PONumber = DataAccessHelper.ConvertToString(row["PONumber"]);
                account.TotalLines = DataAccessHelper.ConvertToInt(row["TotalOrderLineCount"]);
                account.TotalItems = DataAccessHelper.ConvertToInt(row["TotalOrderQuantity"]);
                account.TotalListPrice = DataAccessHelper.ConvertTodecimal(row["TotalListPrice"]);
                account.TotalNetPrice = DataAccessHelper.ConvertTodecimal(row["TotalNetPrice"]);
                account.EstimateProcessingChange = DataAccessHelper.ConvertTodecimal(row["EstimatedProcessingCharges"]);
                account.EstimateTotalCartPrice = DataAccessHelper.ConvertTodecimal(row["EstimatedTotalCartPrice"]);
                account.IsHomeDelivery = DataAccessHelper.ConvertToBool(row["IsHomeDelivery"]);
                list.Add(account);
            }
            return list;
        }

        #endregion

        /// <summary>
        /// Get Primary Cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Cart GetPrimaryCart(string userId)
        {
            Cart cart = null;

            var cartDS = CartDAO.Instance.GetPrimaryCart(userId);            
            cart = GetCartFromDataSet(cartDS, true);

            return cart;
        }

        public static bool IsCartPricing(string cartId, string userId)
        {
            try
            {
                return CartDAO.Instance.IsCartPricing(cartId, userId);
            }
            catch (CartManagerException ex)
            {
                if (ex.isBusinessError)
                    return false;
                else
                    throw ex;
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
                return false;
            }
        }

        private static void HandleSqlException(SqlException exception)
        {
            Logger.LogException(exception);
            throw exception;
        }

        public async Task<Cart> GetPrimaryCartAsync(string userId)
        {
            var cartDs = await CartDAO.Instance.GetPrimaryCartAsync(userId);
            return GetCartFromDataSet(cartDs, true);
        }

        public CartFolder GetCartFolderById(string folderId, string userId)
        {
            var ds = CartDAO.Instance.GetCartFolderById(folderId, userId);
            //convert ds to Cart Folder
            return null;
        }

        /// <summary>
        /// Get Cart Folders
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal static List<CartFolder> GetCartFolders(string userId)
        {
            var cartFoldersDs = CartDAO.Instance.GetCartFolders(userId);
            var result = new List<CartFolder>();
            if (cartFoldersDs.Tables.Count > 0)
            {
                var basketFolderDt = cartFoldersDs.Tables[0];
                if (basketFolderDt != null && basketFolderDt.Rows.Count > 0)
                {
                    foreach (DataRow dr in basketFolderDt.Rows)
                    {
                        var cartFolder = CartFolderHelper.CreateCartFolderFromDataRow(dr);
                        result.Add(cartFolder);
                    }
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Add line items to cart
        /// </summary>
        /// <param name="lineItems"></param>
        /// <param name="cartId"></param>
        /// <param name="userId"></param>
        internal static void AddLineItemsToCart(List<LineItem> lineItems, string cartId, string userId, out string PermissionViolationMessage,
            out int totalAddingQtyForGridDistribution)
        {
            PermissionViolationMessage = "";
            totalAddingQtyForGridDistribution = 0;
            if (lineItems == null || lineItems.Count == 0) return;

            CartDAO.Instance.AddLineItemsToCart(DataConverter.ConvertCartLineItemsToDataset(lineItems), cartId, userId,
                    CartFrameworkHelper.MaxLinesPerCart, out PermissionViolationMessage, out totalAddingQtyForGridDistribution);
        }

        public async Task<string> TestAsync()
        {
            return await CartDAO.Instance.TestAsync();
        }

        public async Task<string> GetDataSetAysnc()
        {
            var ds = await CartDAO.Instance.GetDataSetAsync();
            return "";
        }

        /// <summary>
        /// Gets the baskets by folder id.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <returns>list of basket contained in this folder</returns>
        public async Task<List<Cart>> GetBasketByFolderIdAsync(string folderId)
        {
            if (string.IsNullOrEmpty(folderId)) return null;
            try
            {
                List<Cart> carts = null;
                var cartsDs = await CartDAO.Instance.GetCartsAsync(folderId);
                carts = GetCartsFromDataSet(cartsDs);
                return carts;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
            }
            return null;
        }

        public async Task<Cart> GetCartByIdAsync(string cartId, string userId)
        {
            //if cart is primary
            /* var isPrimary = IsPrimaryCart(cartId, this.UserId);
             if (isPrimary)
             {
                 return CartFarmCacheHelper.GetPrimaryCartNotForMinicart(this.UserId, cartId);
             }

             //normal cart
             var cartCache = CartCacheManager.GetCartFromCache(cartId);
             if (cartCache != null && cartCache.BTStatus != CartStatus.Submitted.ToString() &&
                             cartCache.BTStatus != CartStatus.Ordered.ToString())
                 return cartCache;
 */
            //not found cart in cache or submitted cart
            var cart = CartDAOManager.Instance.GetCartById(cartId, userId);
            /* if (cart != null)
             {
                 CartCacheManager.AddCartToCache(cart);
             }*/

            //// AppFabric
            //var cart = CartDAOManager.GetCartById(cartId, this.UserId);

            return cart;
        }

        #region Partial
        private static List<Cart> GetCartsFromDataSet(DataSet ds)
        {
            //Throw exception if no data returned from DAO
            if (ds == null ||
                ds.Tables.Count == 0 ||
                ds.Tables[0].Rows.Count == 0)
                return null;

            var cartList = new List<Cart>();//new Dictionary<string, Cart>();
            var strUserid = "";

            foreach (DataRow cartRow in ds.Tables[0].Rows)
            {
                var cart = GetCartFromDataRow(cartRow);

                if (cart != null)
                {
                    //if (!cartList.ContainsKey(cart.CartId))
                    //    cartList.Add(cart.CartId, cart);
                    //else
                    cartList.Add(cart);

                    if (String.IsNullOrEmpty(strUserid))
                        strUserid = cart.UserId;
                }
            }

            if (ds.Tables.Count > 1)
            {
                foreach (DataRow accRow in ds.Tables[1].Rows)
                {
                    var account = GetCartAccountFromDataRow(accRow);

                    if (account != null)
                    {
                        foreach (var cart in cartList.Where(cart => cart.CartId == account.BasketSummaryID))
                        {
                            cart.CartAccounts.Add(account);
                        }
                    }
                }
            }

            /*var carts = new Carts(strUserid);
            carts.AddRange(cartList);*/

            return cartList;
        }
        private static Cart GetCartFromDataRow(DataRow row, bool isPrimaryCall = false)
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
                HasESPRanking = row.Table.Columns.Contains("HasESPRanking") && DataAccessHelper.ConvertToBool(row["HasESPRanking"]),
                QuoteID = row.Table.Columns.Contains("QuoteID") ? DataAccessHelper.ConvertToInt(row["QuoteID"]) : 0,
                QuoteExpiredDateTime = row.Table.Columns.Contains("QuoteExpiredDateTime") ? DataAccessHelper.ConvertToDateTime(row["QuoteExpiredDateTime"]) : null,
                IsSplitting = row.Table.Columns.Contains("IsSplitting") && DataAccessHelper.ConvertToBool(row["IsSplitting"]),
                IsSharedBasketGridEnabled = row.Table.Columns.Contains("IsSharedBasketGridEnabled") && DataAccessHelper.ConvertToBool(row["IsSharedBasketGridEnabled"]),
                IsTransferred = row.Table.Columns.Contains("IsTransferred") && DataAccessHelper.ConvertToBool(row["IsTransferred"]),
                BasketProcessingCategoryID = row.Table.Columns.Contains("BasketProcessingCategoryID") ? DataAccessHelper.ConvertToInt(row["BasketProcessingCategoryID"]) : 0,
                ESPRankStateTypeId = row.Table.Columns.Contains("ESPRankStateTypeId") ? DataAccessHelper.ConvertToInt(row["ESPRankStateTypeId"]) : (int)ESPStateType.None,
                ESPDistStateTypeID = row.Table.Columns.Contains("ESPDistStateTypeID") ? DataAccessHelper.ConvertToInt(row["ESPDistStateTypeID"]) : (int)ESPStateType.None,
                ESPFundStateTypeID = row.Table.Columns.Contains("ESPFundStateTypeID") ? DataAccessHelper.ConvertToInt(row["ESPFundStateTypeID"]) : (int)ESPStateType.None,
                LastESPStateTypeLiteral = row.Table.Columns.Contains("LastESPStateType") ? DataAccessHelper.ConvertToString(row["LastESPStateType"]) : string.Empty,
                FreezeLevel = row.Table.Columns.Contains("BasketFrozenLevelIndicator") ? DataAccessHelper.ConvertToInt(row["BasketFrozenLevelIndicator"]) : 0,
                IsActive = row.Table.Columns.Contains("IsBasketActive") && DataAccessHelper.ConvertToBool(row["IsBasketActive"]),
                GridDistributionOption = row.Table.Columns.Contains("GridDistributionOptionID") ?
                    DataAccessHelper.ConvertToInt(row["GridDistributionOptionID"]) : -1
            };

            cart.CurrentWorkflow = row.Table.Columns.Contains("CurrentWorkflow") ? GetCurrentWorkFlow(row["CurrentWorkflow"]) : 0;
            cart.IsPrimary = isPrimaryCall || DataAccessHelper.ConvertToBool(row["IsPrimary"]);

            RefineContainsMixGridNonGridForCart(cart);

            return cart;
        }
        private static void RefineContainsMixGridNonGridForCart(Cart cart)
        {
            cart.ContainsAMixOfGridNNonGrid = cart.HasGridLine && cart.TitlesWithoutGrids > 0 && cart.LineItemCount != cart.TitlesWithoutGrids;
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
        private static int GetNumberOfBuilding(object obj)
        {
            int returnValue = 1;
            if (null != obj)
            {
                Int32.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        internal async Task<Cart> GetCartByName(string cartName, string userId)
        {
            var ds = await CartDAO.Instance.GetCartByName(cartName, userId);
            return GetCartFromDataSet(ds);
        }

        /// <summary>
        /// Gets Cart by Cart's ID
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public Cart GetCartById(string cartId, string userId = null)
        {
            if (string.IsNullOrEmpty(cartId)) return null;

            Cart cart = null;
            int nonRankedCount = 0;
            var cartDs = CartDAO.Instance.GetCart(cartId, userId, out nonRankedCount);
            cart = GetCartSummaryFromDataSet(cartDs);
            cart.NonRankedCount = nonRankedCount;
            return cart;
        }

        public Cart GetCartById(string cartId, bool needToRepice, string userId, TargetingValues siteContext)
        {
            if (string.IsNullOrEmpty(cartId)) throw new CartManagerException(CartManagerException.CART_ID_NULL);

            if (!needToRepice) return GetCartById(cartId, userId);

            //Recalculate Price
            CartFrameworkHelper.CalculatePrice(cartId, siteContext);

            var cart = GetCartById(cartId, userId);
            //if (cart != null)
            //{
            //    CartCacheManager.AddCartToCache(cart);
            //}
            return cart;

            //return this.Carts.GetCartById(cartId, needToRepice);
        }

        internal static Dictionary<string, int> GetQuantitiesByBtKeys(string cartId, string userId, List<string> btKeys)
        {
            if (string.IsNullOrEmpty(cartId))
                throw new CartManagerException(CartManagerException.CART_ID_NULL);
            var retDict = new Dictionary<string, int>();

            var ds = CartDAO.Instance.GetQuantitiesByBtKeys(cartId, userId, btKeys);
            if (ds != null && ds.Tables.Count > 0)
            {
                var table = ds.Tables[0];

                foreach (DataRow row in table.Rows)
                {
                    var btkey = DataConverter.ConvertTo<string>(row, "BTKey");
                    var quantity = DataConverter.ConvertTo<int>(row, "Quantity");
                    if (!retDict.ContainsKey(btkey))
                    {
                        retDict.Add(btkey, quantity);
                    }
                }
            }
            
            return retDict;
        }

        internal static List<List<string>> CheckBasketForTitles(string cartId, List<string> btKeys, List<string> lineItemIds, out int newLineCount, out int existingLineCount)
        {
            if (string.IsNullOrEmpty(cartId))
                throw new CartManagerException(CartManagerException.CART_ID_NULL);
            var retList = new List<List<string>>();
            newLineCount = 0;
            existingLineCount = 0;

            var ds = CartDAO.Instance.CheckBasketForTitles(cartId, btKeys, lineItemIds, out newLineCount, out existingLineCount);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    var tableNewLine = ds.Tables[0];
                    var newList =
                        (from DataRow row in tableNewLine.Rows select DataConverter.ConvertTo<string>(row, "NewBTKey"))
                            .ToList();
                    retList.Add(newList);
                }
                if (ds.Tables.Count > 1)
                {
                    var tableExistingLine = ds.Tables[1];
                    var existingList =
                        (from DataRow row in tableExistingLine.Rows
                         select DataConverter.ConvertTo<string>(row, "ExistingBTKey")).ToList();
                    retList.Add(existingList);
                }
            }
            
            return retList;
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

        public static Cart GetCartFromDataSet(DataSet ds, bool isPrimaryCall = false)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            //Primary Cart information
            var cartInfoRow = ds.Tables[0].Rows[0];

            var cart = GetCartFromDataRow(cartInfoRow, isPrimaryCall);

            if (cart != null)
            {
                if (ds.Tables.Count == 2)
                {
                    //Accounts
                    foreach (DataRow accountRow in ds.Tables[1].Rows)
                    {
                        var cartAccount = GetCartAccountFromDataRow(accountRow);

                        if (cartAccount != null)
                            cart.CartAccounts.Add(cartAccount);
                    }
                }
            }

            return cart;
        }

        #endregion Partiall

        public async Task<List<CartDuplicateItem>> CheckForCartDuplicates(string orgId, string userId, string basketId, List<string> listBtKeys, List<string> listBteKeys,
            string dupCheckCartType, string DownloadedCheckType)
        {
            if (listBtKeys == null || listBtKeys.Count == 0)
                return null;

            var results = new List<CartDuplicateItem>();
            const string delimited = ";";
            string itemBteKeys = "";
            string itemListKeys = listBtKeys.Aggregate("", (current, btKey) => current + (btKey + delimited));
            itemListKeys = itemListKeys.TrimEnd(';');
            
            if (listBteKeys != null)
            {
                foreach (string bteKey in listBteKeys)
                {
                    itemBteKeys += (bteKey ?? "NA") + delimited;
                }

                // remove last delimited char
                if (itemBteKeys.EndsWith(delimited))
                    itemBteKeys = itemBteKeys.Remove(itemBteKeys.Length - 1);
            }

            if (string.Compare(dupCheckCartType, DefaultDuplicateCarts.MyCarts.ToString(), true) == 0)
                orgId = null;

            // Cart Duplicates from database. Result string sample: 'C;C;C;C;N;C;C;C;N;'
            var itemsCheckCart = await CartDAO.Instance.CheckForCartDuplicates(orgId, userId, basketId, itemListKeys, itemBteKeys, dupCheckCartType, DownloadedCheckType);

            // split result string and add to results
            if (!string.IsNullOrEmpty(itemsCheckCart))
            {
                itemsCheckCart = itemsCheckCart.TrimEnd(';');
                string[] arrCart = itemsCheckCart.Split(';');

                if (arrCart.Length == listBtKeys.Count)
                {
                    for (int i = 0; i < listBtKeys.Count; i++)
                    {
                        var duplicateItem = new CartDuplicateItem()
                        {
                            BTKey = listBtKeys[i],
                            IsDuplicated = (arrCart[i].ToLower() == "c")
                        };

                        // add DupC item
                        results.Add(duplicateItem);
                    }
                }
            }

            return results;
        }

        public async Task<Dictionary<string, bool>> CheckForHoldingsDuplicates(string orgId, string userId, List<string> listBtKeys)
        {
            Dictionary<string, bool> results = null;

            if (listBtKeys != null && listBtKeys.Count > 0)
            {
                var btKeys = string.Join(";", listBtKeys);
                var userProfile = ProfileService.Instance.GetUserById(userId);
                if (userProfile != null && !string.IsNullOrEmpty(userProfile.HoldingsFlag))
                {
                    // Check Holdings Duplicates
                    if (userProfile.HoldingsFlag.ToLower() != "none")
                    {
                        results = await CartDAO.Instance.CheckHoldingsDuplicates(userId, orgId, userProfile.HoldingsFlag, btKeys);
                    }
                }
            }

            return results;
        }

        public async Task<CheckForDuplicatesObject> CheckForDuplicates(string userId, List<string> listBtKeys, List<string> listBteKeys,
            string itemsCheckOrder, string itemsCheckCart, string basketId, string holdingsFlag,
            bool isRequiredCheckDupCarts, bool isRequiredCheckDupOrder, string siteContextDefaultDownloadedCarts,
            string siteContextOrgId)
        {
            itemsCheckOrder = string.Empty;
            itemsCheckCart = string.Empty;
            holdingsFlag = "None";

            if (listBtKeys == null || listBtKeys.Count == 0)
                return null;

            string itemBteKeys = "";
            string itemListKeys = listBtKeys.Aggregate("", (current, btKey) => current + (btKey + ";"));
            itemListKeys = itemListKeys.TrimEnd(';');
            const string delimited = ";";

            var userProfile = ProfileService.Instance.GetUserById(userId);

            if (userProfile == null) return null;

            var organizationId = userProfile.OrgId; // .Organization.Target.Id;

            holdingsFlag = userProfile.HoldingsFlag ?? "None";

            string defaultDuplicateOrders;
            string defaultDuplicateCarts;

            if (!isRequiredCheckDupCarts)
                defaultDuplicateCarts = "None";
            else
                defaultDuplicateCarts = userProfile.DefaultDuplicateCarts;
            if (!isRequiredCheckDupOrder)
                defaultDuplicateOrders = "None";
            else
                defaultDuplicateOrders = userProfile.DefaultDuplicateOrders;

            if (listBteKeys != null)
            {
                foreach (string bteKey in listBteKeys)
                {
                    itemBteKeys += (bteKey ?? "NA") + delimited;
                }

                // remove last delimited char
                if (itemBteKeys.EndsWith(delimited))
                    itemBteKeys = itemBteKeys.Remove(itemBteKeys.Length - 1);
            }

            var checkForDupObj = await CheckForDuplicatesOnManager(itemListKeys, itemBteKeys, defaultDuplicateCarts,
                                                                                        defaultDuplicateOrders,
                                                                                        organizationId, userId,
                                                                                        itemsCheckOrder,
                                                                                        itemsCheckCart, basketId,
                                                                                        holdingsFlag, siteContextDefaultDownloadedCarts,
                                                                                        siteContextOrgId);

            checkForDupObj.HoldingsFlag = holdingsFlag;
            return checkForDupObj;
        }

        public async Task<List<SiteTermObject>> GetProductDuplicateIndicator(List<string> listId, List<string> listBteKeys,
                                                                 string basketId, bool isRequiredCheckDupCarts, bool isRequiredCheckDupOrder,
            string siteContextUserId, string siteContextDefaultDownloadedCarts, string siteContextOrgId)
        {
            var list = new List<SiteTermObject>();
            //
            string itemsCheckOrder = "", itemsCheckCart = "";
            var itemsCheckHoldings = new Dictionary<string, bool>();
            string holdingsFlag = "";
            bool runHoldingsDupCheck = false;
            //
            var checkDupObj = await CheckForDuplicates(siteContextUserId, listId, listBteKeys, itemsCheckOrder,
                                             itemsCheckCart, basketId, holdingsFlag,
                                             isRequiredCheckDupCarts, isRequiredCheckDupOrder, siteContextDefaultDownloadedCarts, 
                                             siteContextOrgId);

            if (checkDupObj == null || !checkDupObj.IsDuplication)
                return list;

            itemsCheckOrder = checkDupObj.ItemsCheckOrder;
            itemsCheckCart = checkDupObj.ItemsCheckCart;
            holdingsFlag = checkDupObj.HoldingsFlag;
            itemsCheckHoldings = checkDupObj.ItemsCheckHoldings;

            itemsCheckOrder = itemsCheckOrder.TrimEnd(';');
            itemsCheckCart = itemsCheckCart.TrimEnd(';');
            if (!isRequiredCheckDupCarts)
            {
                itemsCheckCart = itemsCheckOrder.Replace("O", "N").Replace("o", "N");
            }
            if (!isRequiredCheckDupOrder)
            {
                itemsCheckOrder = itemsCheckCart.Replace("C", "N").Replace("c", "N");
            }
            //
            string[] arrOrder = itemsCheckOrder.Split(';');
            string[] arrCart = itemsCheckCart.Split(';');
            //
            if (arrOrder.Length != arrCart.Length || arrOrder.Length != listId.Count || arrCart.Length != listId.Count)
                return list;

            if (holdingsFlag.ToLower() == "user" || holdingsFlag.ToLower() == "org")
            {
                runHoldingsDupCheck = true;
            }

            if (runHoldingsDupCheck)
            {
                if (itemsCheckHoldings.Count > 0)
                {
                    for (int i = 0; i < arrOrder.Length; i++)
                    {
                        var btKey = listId[i];
                        var combinedProduct = CombineProductDuplicated(arrCart[i], arrOrder[i], itemsCheckHoldings[btKey], holdingsFlag);

                        var item = new SiteTermObject(btKey, combinedProduct, basketId);
                        list.Add(item);
                    }
                }
            }
            else
            {
                list.AddRange(
                arrOrder.Select(
                    (t, i) => new SiteTermObject(listId[i], CombineProductDuplicated(arrCart[i], t, false, holdingsFlag), basketId)));
            }

            //
            return list;
        }


        public async Task<CheckForDuplicatesObject> CheckForDuplicatesOnManager(string listItemKeys, string listBTEkeys, string cartCheckType,
            string orderCheckType, string orgId, string soldToId, string itemsCheckOrder, string itemsCheckCart, string cartId,
            string holdingsFlag, string downloadCheckType, string siteContextOrgId)
        {
            string orgIdValue = orgId;
            if (string.Compare(cartCheckType, DefaultDuplicateCarts.MyCarts.ToString(), true) == 0)
                orgId = null;
            if (string.Compare(orderCheckType, DefaultDuplicateOrders.AllAccounts.ToString(), true) == 0)
                orgId = orgIdValue;

            itemsCheckOrder = string.Empty;
            itemsCheckCart = string.Empty;

            var checkForDupObj = await CartDAO.Instance.CheckForDuplicates(listItemKeys, listBTEkeys,
                                                       cartCheckType, orderCheckType, orgId, soldToId,
                                                       itemsCheckOrder, itemsCheckCart, cartId, downloadCheckType);

           if (holdingsFlag.ToLower() != "none")
            {
                var duplicateHoldings = await CartDAO.Instance.CheckHoldingsDuplicates(soldToId, siteContextOrgId, holdingsFlag, listItemKeys);
                checkForDupObj.ItemsCheckHoldings = duplicateHoldings;

            }
            return checkForDupObj;
        }

        public async Task<LineItem> GetCartLineById(string lineItemId, string contextUserId)
        {
            var ds = await CartDAO.Instance.GetCartLineById(lineItemId, contextUserId);

            var result = GetLineItemsFromDataSet(ds);

            if (result != null && result.Count > 0)
            {
                return result[0];
            }
            return null;
        }

        public List<SimpleLineItem> GetCartLineIdAndBTKeyPairList(SearchCartLinesCriteria criteria, out int totalLines)
        {
            List<SimpleLineItem> result = null;
            totalLines = 0;
            try
            {
                DataSet ds;
                if (criteria.IsQuickCartDetails)
                {
                    var sortBy = CommonHelper.QuickCartDetailsSortByDB.Title.ToString();

                    if (!string.IsNullOrEmpty(criteria.QuickSortBy) && CommonHelper.SortByDict.ContainsKey(criteria.QuickSortBy))
                        sortBy = CommonHelper.SortByDict[criteria.QuickSortBy];

                    int nonRankedCount = 0;
                    ds = CartDAO.Instance.GetCartDetailsQuickView(out nonRankedCount, criteria.CartId, criteria.UserId, sortBy, (byte)criteria.SortDirection
                                                                    , (short)criteria.PageSize, criteria.PageNumber, recalculateHeader: true);

                    if (ds.Tables.Count > 1)
                    {
                        var cartHeaderTable = ds.Tables[1];
                        if (cartHeaderTable.Rows.Count > 0)
                        {
                            var liRow = cartHeaderTable.Rows[0];
                            totalLines = DataAccessHelper.ConvertToInt(liRow["TotalLines"]);
                        }
                    }
                }
                else
                {
                    ds = CartDAO.Instance.GetCartLines(criteria, out totalLines);
                }

                // get line items from dataset
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result = new List<SimpleLineItem>();
                    foreach (DataRow dataRow in ds.Tables[0].Rows)
                    {
                        var item = new SimpleLineItem
                        {
                            LineItemID = DataAccessHelper.ConvertTo<string>(dataRow, "BasketLineItemId"),
                            BTKey = DataAccessHelper.ConvertTo<string>(dataRow, "BTKey"),
                            BasketOriginalEntryID = DataAccessHelper.ConvertTo<string>(dataRow, "BasketOriginalEntryID")
                        };

                        result.Add(item);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }

            return result;
        }

        private List<LineItem> GetLineItemsFromDataSet(DataSet ds)
        {
            if (ds == null ||
                ds.Tables.Count < 1 ||
                ds.Tables[0].Rows.Count == 0)
                return null;

            var result = new List<LineItem>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var item = GetLineItemFromDataRow(row);
                //item.PublishDate = DataAccessHelper.ConvertTo<DateTime>(row, "PublishDate");
                item.Acknowledgements = GetLineItemAckFromDataSet(ds, item.Id);
                result.Add(item);
            }

            return result;
        }

        private List<LineItemAcknowledgement> GetLineItemAckFromDataSet(DataSet ds, string lineItemID)
        {
            var acks = new List<LineItemAcknowledgement>();
            if (ds == null ||
                 ds.Tables.Count < 2 ||
                 ds.Tables[1].Rows.Count == 0)
                return null;

            foreach (DataRow row in ds.Tables[1].Rows)
            {
                if (DataAccessHelper.ConvertToString(row["BasketLineItemId"]) == lineItemID)
                {
                    var ack = new LineItemAcknowledgement();
                    ack.OrderNumber = DataAccessHelper.ConvertToInt(row["OrderNumber"]);
                    ack.OrderDate = DateTime.Parse(DataAccessHelper.ConvertToString(row["OrderDate"]));
                    ack.Warehouse = DataAccessHelper.ConvertToString(row["Warehouse"]);
                    ack.ShippedQuantity = DataAccessHelper.ConvertToInt(row["ShippedQuantity"]);
                    ack.CancelledQuantity = DataAccessHelper.ConvertToInt(row["CancelledQuantity"]);
                    ack.InProcessQuantity = DataAccessHelper.ConvertToInt(row["InProcessQuantity"]);
                    ack.BackOrderedQuantity = DataAccessHelper.ConvertToInt(row["BackOrderedQuantity"]);
                    ack.RevervedAwaitingReleaseQuantity = DataAccessHelper.ConvertToInt(row["ReservedQuantity"]);
                    acks.Add(ack);
                }
            }

            return acks;
        }

        private LineItem GetLineItemFromDataRow(DataRow row)
        {

            var res = new LineItem
            {
                Id = DataAccessHelper.ConvertTo<string>(row, "BasketLineItemId"),
                BasketOrderFormId = DataAccessHelper.ConvertTo<string>(row, "BasketOrderFormId"),
                ISBN10 = DataAccessHelper.ConvertTo<string>(row, "ISBN10"),
                BTKey = DataAccessHelper.ConvertTo<string>(row, "BTKey"),
                Title = DataAccessHelper.ConvertTo<string>(row, "Title"),
                Author = DataAccessHelper.ConvertTo<string>(row, "Author"),
                Publisher = DataAccessHelper.ConvertTo<string>(row, "Publisher"),
                ListPrice = DataAccessHelper.ConvertTo<decimal>(row, "ListPrice"),
                ContractPrice = DataAccessHelper.ConvertTo<decimal>(row, "ContractPrice"),
                SalePrice = DataAccessHelper.ConvertTo<decimal>(row, "SalePrice"),
                ExtendedPrice = DataAccessHelper.ConvertTo<decimal>(row, "ExtendedPrice"),
                BTDiscountPercent = DataAccessHelper.ConvertTo<decimal>(row, "DiscountPercent"),
                Lccn = DataAccessHelper.ConvertTo<string>(row, "LCCN"),
                DeweyNormalized = DataAccessHelper.ConvertTo<string>(row, "DeweyNormalized"),
                DeweyNative = DataAccessHelper.ConvertTo<string>(row, "DeweyNative"),
                FormatLiteral = DataAccessHelper.ConvertTo<string>(row, "FormatLiteral"),
                Edition = DataAccessHelper.ConvertTo<string>(row, "Edition"),
                PublishDate = DataAccessHelper.ConvertToDateTimeNull(row, "PublishDate"),
                ReportCode = ReviseReportCode(DataAccessHelper.ConvertTo<string>(row, "ReportCode")),
                Subject = DataAccessHelper.ConvertTo<string>(row, "Subject"),
                PONumber = DataAccessHelper.ConvertTo<string>(row, "POLineItemNumber"),
                Quantity = DataAccessHelper.ConvertTo<int>(row, "Quantity"),
                BTShippedQuantity = DataAccessHelper.ConvertTo<int>(row, "ShippedQuantity"),
                BTBackorderQuantity = DataAccessHelper.ConvertTo<int>(row, "BackOrderQuantity"),
                BTCancelQuantity = DataAccessHelper.ConvertTo<int>(row, "CancelledQuantity"),
                BTLineItemNote = DataAccessHelper.ConvertTo<string>(row, "LineItemNote"),
                LCClass = DataAccessHelper.ConvertTo<string>(row, "LCClass"),
                Upc = DataAccessHelper.ConvertTo<string>(row, "UPC"),
                SubTitle = DataAccessHelper.ConvertTo<string>(row, "SubTitle"),
                ISBN = DataAccessHelper.ConvertTo<string>(row, "ISBN"),
                ProductLine = DataAccessHelper.ConvertTo<string>(row, "ProductLine"),
                ProductType = row.Table.Columns.Contains("ProductType") ? DataAccessHelper.ConvertToString(row["ProductType"]) : string.Empty,
                StreetDate = DataAccessHelper.ConvertTo<DateTime>(row, "StreetDate"),
                VolumeNumber = DataAccessHelper.ConvertTo<string>(row, "VolumeNumber"),
                PromotionCode = DataAccessHelper.ConvertTo<string>(row, "PromotionCode"),
                ESupplier = DataAccessHelper.ConvertTo<string>(row, "ESupplier"),
                Bib = DataAccessHelper.ConvertTo<string>(row, "BIBNumber"),
                BasketOriginalEntryId = DataAccessHelper.ConvertTo<string>(row, "BasketOriginalEntryID"),
                HasGridError = row.Table.Columns.Contains("HasGridError") && DataAccessHelper.ConvertToBool(row["HasGridError"]),
                HasFamilyKey = row.Table.Columns.Contains("HasFamilyKey") && DataAccessHelper.ConvertToBool(row["HasFamilyKey"]),
                PriceKey = row.Table.Columns.Contains("PriceKey") ? DataAccessHelper.ConvertToString(row["PriceKey"]) : string.Empty,
                ContributedBy = row.Table.Columns.Contains("ContributedBy") ? DataAccessHelper.ConvertToString(row["ContributedBy"]) : string.Empty,
                SupplierCode = DataConverter.ConvertTo<string>(row, "PubCodeD"),
                PubStatusCode = DataConverter.ConvertTo<string>(row, "PubStatusCode"),
                AVAttributes = DataConverter.ConvertTo<string>(row, "AVAttributes"),
                BisacSubject1Code = DataConverter.ConvertTo<string>(row, "BisacSubject1Code"),
                Attributes = DataConverter.ConvertTo<string>(row, "Attributes"),
                FormatCode = DataConverter.ConvertTo<string>(row, "FormatCode"),
                EditionNumber = DataConverter.ConvertTo<string>(row, "EditionNumber"),
                Oclc = DataConverter.ConvertTo<string>(row, "OCLCControlNumber"),
                Genre = DataConverter.ConvertTo<string>(row, "Genre"),
                HasReview = DataConverter.ConvertTo<bool>(row, "HasReview"),
                HasAnnotations = DataConverter.ConvertTo<bool>(row, "HasAnnotations"),
                HasExcerpt = DataConverter.ConvertTo<bool>(row, "HasExcerpt"),
                HasReturn = DataConverter.ConvertTo<bool>(row, "HasReturn"),
                HasMuze = DataConverter.ConvertTo<bool>(row, "HasMuze"),
                HasTOC = DataConverter.ConvertTo<bool>(row, "HasTOC"),
                //NumOfDiscs = DataConverter.ConvertTo<int>(row, "NumOfDiscs")
                Version = row.Table.Columns.Contains("Version") ? DataAccessHelper.ConvertTo<string>(row, "Version") : "",
            };
            return res;
        }

        private string ReviseReportCode(string dbValue)
        {
            return (!string.IsNullOrEmpty(dbValue) && dbValue.Equals("PERMANENTLY OUT OF STOCK", StringComparison.OrdinalIgnoreCase)) ?
                            "Publisher Out of Stock Indefinitely" : dbValue;
        }

        public async Task<Carts> GetTopNewestCarts(int topCarts, string userId)
        {
            var carts = new Carts(userId);

            var ds = await CartDAO.Instance.GetCarts(topCarts, userId);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            foreach (DataRow cartRow in ds.Tables[0].Rows)
            {
                var cart = GetCartFromDataRowForTopNewest(cartRow, userId);

                if (cart != null)
                {
                    carts.Add(cart);
                }
            }

            return carts;
        }

        private Cart GetCartFromDataRowForTopNewest(DataRow row, string userId)
        {
            var strCartId = DataAccessHelper.ConvertToString(row["BasketSummaryID"]);

            if (String.IsNullOrEmpty(strCartId))
                return null;

            var cart = new Cart(strCartId, userId)
            {
                CartName = DataConverter.ConvertTo<string>(row, "BasketName"),
            };

            return cart;
        }

        public async Task<LineItem> GetCartLineByBtKey(string btkey, string cartId)
        {
            LineItem result = null;

            var ds = await CartDAO.Instance.GetCartLineByBtKey(btkey, cartId);

            if (ds == null ||
            ds.Tables.Count < 1 ||
            ds.Tables[0].Rows.Count == 0)
                return null;
            var row = ds.Tables[0].Rows[0];
            result = new LineItem
            {
                Id = DataAccessHelper.ConvertTo<string>(row, "BasketLineItemId"),
                BTKey = DataAccessHelper.ConvertTo<string>(row, "BTKey"),
                Upc = DataAccessHelper.ConvertTo<string>(row, "UPC"),
                ISBN = DataAccessHelper.ConvertTo<string>(row, "ISBN"),
                Quantity = DataAccessHelper.ConvertTo<int>(row, "Quantity"),
                ProductType = DataAccessHelper.ConvertTo<string>(row, "ProductType"),
                CatalogName = DataAccessHelper.ConvertTo<string>(row, "ProductCatalog")
            };
            return result;
        }

        public void AddToCartName4BatchEntry(List<LineItem> listItem, string cartId, out string lastIsbnUpcProcessed,
            out string PermissionViolationMessage, out int totalAddingQtyForGridDistribution, string userId, TargetingValues siteContext)
        {
            lastIsbnUpcProcessed = string.Empty;
            PermissionViolationMessage = null;
            totalAddingQtyForGridDistribution = 0;
            if (listItem != null && listItem.Count > 0)
            {
                CartBatchEntryManager.AddLineItemsToCart(listItem, cartId, out PermissionViolationMessage, out totalAddingQtyForGridDistribution, userId);
                lastIsbnUpcProcessed = GetLastItemProcessed(listItem);

                ResetCacheCartById(cartId, userId);

                //HttpHelper.ForceCartRepriceInBackgroundRequest(cartId);
                CartFrameworkHelper.CalculatePrice(cartId, siteContext);
            }
        }

        public void ResetCacheCartById(string cartId, string userId)
        {
            var cartManager = new CartManager(userId);
            if (cartManager == null) return;

            cartManager.SetCartChanged(cartId);
        }

        private static string GetLastItemProcessed(List<LineItem> list)
        {
            if (list == null || list.Count == 0) return string.Empty;

            var lastIndex = list.Count - 1;
            var lastIsbn = list[lastIndex].ISBN;
            if (!string.IsNullOrEmpty(lastIsbn))
            {
                return lastIsbn;
            }
            return list[lastIndex].Upc;
        }
        public void AddExceed500LineItemsToCart(List<LineItem> lineItems, string userId, string cartId, int maxBatchSize)
        {
            if (lineItems == null || lineItems.Count == 0) return;

            CartDAO.Instance.AddExceed500LineItemsToCart(DataConverter.ConvertCartLineItemsToDataset(lineItems), userId, cartId,
                maxBatchSize);
        }
        #region Private

        private void CreateAccountAndAccount8IdList(Account account, string delimited,
                                                    ref bool isContainTolasAccount, ref string accountList, ref string account8List)
        {
            //var account = (Account)commerceRelationship.Target;

            //Huy modified: account.Id => account.AccountNumber
            accountList = accountList + account.AccountNumber + delimited;

            bool isTolasAccount = account.IsTOLAS.HasValue && account.IsTOLAS.Value;
            isContainTolasAccount = isContainTolasAccount || isTolasAccount;

            account8List += account.Account8Id == null ? String.Empty + delimited : account.Account8Id + delimited;
        }

        public string CombineProductDuplicated(string cart, string order, bool isInHoldingsQueue, string holdingsFlag)
        {
            string result = "";
            if (cart.ToLower() == "c")
                result += ProductSupportedHtmlTag.DupC;
            //this is not required for release 1
            if (order.ToLower() == "o")
                result += ProductSupportedHtmlTag.DupO;

            if (isInHoldingsQueue)
            {
                if (holdingsFlag.ToLower() == "user")
                    result += ProductSupportedHtmlTag.DupHUser;
                else if (holdingsFlag.ToLower() == "org")
                    result += ProductSupportedHtmlTag.DupHOrg;
            }

            if (!String.IsNullOrEmpty(result) && isInHoldingsQueue)
                result = ProductSupportedHtmlTag.DupBeginImage + result + ProductSupportedHtmlTag.DupEndImage +
                         ProductSupportedHtmlTag.DivCb;
            else if (!String.IsNullOrEmpty(result))
                result = ProductSupportedHtmlTag.DupBeginImage + result + ProductSupportedHtmlTag.DupEndImage +
                         ProductSupportedHtmlTag.DivCb;
            //
            return result;
        }

        public string CombineProductDuplicated(string cart, string order, bool isInHoldingsQueue)
        {
            string result = "";
            if (cart.ToLower() == "c")
                result += ProductSupportedHtmlTag.DupC;
            //this is not required for release 1
            if (order.ToLower() == "o")
                result += ProductSupportedHtmlTag.DupO;

            if (isInHoldingsQueue)
                result += ProductSupportedHtmlTag.DupH;

            if (!String.IsNullOrEmpty(result))
                result = ProductSupportedHtmlTag.DupBeginImage + result + ProductSupportedHtmlTag.DupEndImage +
                         ProductSupportedHtmlTag.DivCb;
            //
            return result;
        }

        public Dictionary<string, string> FindQuantity(string userId, IEnumerable<string> btKeys)
        {
            var quantities = new Dictionary<string, string>();
            if (btKeys != null && btKeys.Count() > 0)
            {
                var primaryCartId = CartFarmCacheHelper.GetPrimaryCartId(userId);
                if (string.IsNullOrEmpty(primaryCartId))
                {
                    var primaryCart = CartFarmCacheHelper.GetPrimaryCart(userId);
                    if (primaryCart != null)
                        primaryCartId = primaryCart.CartId;
                }

                if (!String.IsNullOrEmpty(primaryCartId)) 
                {
                    var defaultQuantityInPrimaryCart = GetQuantitiesByBtkeys(primaryCartId, userId, btKeys.ToList());
                    if (defaultQuantityInPrimaryCart != null)
                    {
                        foreach (var btKey in btKeys)
                        {
                            quantities.Add(btKey,
                                           defaultQuantityInPrimaryCart.ContainsKey(btKey)
                                               ? defaultQuantityInPrimaryCart[btKey].ToString()
                                               : string.Empty);
                        }
                    }
                }
                else
                {
                    foreach (var btKey in btKeys)
                    {
                        quantities.Add(btKey, string.Empty);
                    }
                }
            }
            return quantities;
        }

        private Dictionary<string, int> GetQuantitiesByBtkeys(string cartId, string userId, List<string> btKeys)
        {
            if (string.IsNullOrEmpty(cartId))
                throw new CartManagerException(CartManagerException.CART_ID_NULL);

            var retDict = new Dictionary<string, int>();

            var ds = CartDAO.Instance.GetQuantitiesByBtKeys(cartId, userId, btKeys);
            if (ds != null && ds.Tables.Count > 0)
            {
                var table = ds.Tables[0];

                foreach (DataRow row in table.Rows)
                {
                    var btkey = DataConverter.ConvertTo<string>(row, "BTKey");
                    var quantity = DataConverter.ConvertTo<int>(row, "Quantity");
                    if (!retDict.ContainsKey(btkey))
                    {
                        retDict.Add(btkey, quantity);
                    }
                }
            }

            return retDict;
        }

        public async Task<ESPRankDetailInfo> GetESPRankItemDetailsByID(string lineItemID)
        {
            ESPRankDetailInfo rankDetail = null;

            if (string.IsNullOrEmpty(lineItemID)) return null;

            var rankDs = await CartDAO.Instance.GetESPRankItemDetails(lineItemID);
            rankDetail = GetESPRankDetail(rankDs);

            return rankDetail;
        }

        private ESPRankDetailInfo GetESPRankDetail(DataSet ds)
        {
            if (ds == null ||
                ds.Tables.Count < 1 ||
                ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            var espRankDetailInfo = new ESPRankDetailInfo();
            var lsRankDetail = new ArrayList();

            var firstRow = ds.Tables[0].Rows[0];
            espRankDetailInfo.OverallRank = DataConverter.ConvertTo<decimal?>(firstRow, "ESPOverallRanking");
            espRankDetailInfo.BisacRank = DataConverter.ConvertTo<decimal?>(firstRow, "ESPBisacRanking");
            espRankDetailInfo.DetailUrl = DataConverter.ConvertTo<string>(firstRow, "ESPDetailUrl");
            espRankDetailInfo.ESPCategoryType = DataConverter.ConvertTo<string>(firstRow, "ESPCategory");
            espRankDetailInfo.ESPDetailWidth = DataConverter.ConvertTo<int>(firstRow, "ESPDetailWidth");
            espRankDetailInfo.ESPDetailHeight = DataConverter.ConvertTo<int>(firstRow, "ESPDetailHeight");

            /*foreach (DataRow row in ds.Tables[0].Rows)
            {
                int espRankTypeID = DataAccessHelper.ConvertToInt(row["ESPRankingTypeID"]);
                string espRankDescription = DataAccessHelper.ConvertToString(row["ESPRankingDescription"]);
                decimal value = DataAccessHelper.ConvertTodecimal(row["Value"]);

                ESPRankDetailInfo.RankDetail rankDetail = new ESPRankDetailInfo.RankDetail();
                rankDetail.Description = espRankDescription;
                rankDetail.Value = value;
                lsRankDetail.Add(rankDetail);
            }*/

            if (lsRankDetail.Count > 0)
                espRankDetailInfo.RankDetails = (ESPRankDetailInfo.RankDetail[])lsRankDetail.ToArray(typeof(ESPRankDetailInfo.RankDetail));

            return espRankDetailInfo;
        }

        public async Task<List<LineItem>> GetLineItemIDs(string cartId)
        {
            if (string.IsNullOrEmpty(cartId))
                throw new CartManagerException(CartManagerException.CART_ID_NULL);
            List<LineItem> lineItems;

            DataSet result = await CartDAO.Instance.GetCartLineItemIDs(cartId);
            lineItems = GetLineItemIDsFromDataSet(result);

            return lineItems;
        }
        private List<LineItem> GetLineItemIDsFromDataSet(DataSet ds)
        {
            if (ds == null ||
                ds.Tables.Count < 1 ||
                ds.Tables[0].Rows.Count == 0)
                return null;

            var result = new List<LineItem>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var item = new LineItem
                {
                    Id = DataAccessHelper.ConvertTo<string>(row, "BasketLineItemId"),
                    BTKey = DataAccessHelper.ConvertTo<string>(row, "BTKey")
                };
                result.Add(item);
            }

            return result;
        }

        public async Task<Dictionary<string, string>> GetLineItemBtKeys(string cartId)
        {
            if (string.IsNullOrEmpty(cartId))
                throw new CartManagerException(CartManagerException.CART_ID_NULL);

            var ret = new Dictionary<string, string>();

            DataSet ds = await CartDAO.Instance.GetCartLineItemIDs(cartId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var btKey = DataAccessHelper.ConvertTo<string>(row, "BTKey");
                    if (!string.IsNullOrEmpty(btKey))
                    {
                        var lineItemId = DataAccessHelper.ConvertTo<string>(row, "BasketLineItemId");
                        ret.Add(btKey, lineItemId);
                    }
                }
            }

            return ret;
        }

        #endregion

        public void SetBasketStateILSOrdered(string basketSummaryID, string userId)
        {
            CartDAO.Instance.SetBasketStateILSOrdered(basketSummaryID, userId);
        }

        public void SetILSBasketState(string basketSummaryID, string userId, CartStatus basketState, ILSState ilsStatusId, string ILSMarcProfileId, string vendorCode, string orderedDownloadedUserId)
        {
            CartDAO.Instance.SetILSBasketState(basketSummaryID, userId, basketState, ilsStatusId, ILSMarcProfileId, vendorCode, orderedDownloadedUserId);
        }

        public void SetILSSystemState(string basketSummaryID, string userId, ILSSystemStatus ilsSystemStatusID)
        {
            CartDAO.Instance.SetILSSystemState(basketSummaryID, userId, ilsSystemStatusID);
        }

        public void ResetILSStatus(string basketSummaryID, string userId)
        {
            CartDAO.Instance.ResetILSStatus(basketSummaryID, userId);
        }

        public void SetPricingBasketRollupNumbers(string basketSummaryID)
        {
            CartDAO.Instance.SetPricingBasketRollupNumbers(basketSummaryID);
        }

        public void SubmitCart(Cart cart, string userId, string loggedInUserId, out string newBasketName, out string newBasketId, out string newOEBasketName, out string newOEBasketID, bool isVIP, bool isOrderAndHold, string orderedDownloadedUserId)
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

                CartDAO.Instance.SubmitCart(cart.CartId, userId, loggedInUserId, orderFormInfo, accountDict, cart.SpecialInstruction,
                                            out newBasketName, out newBasketId, out newOEBasketName, out newOEBasketID, isVIP, isOrderAndHold, orderedDownloadedUserId);
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }
        }
        internal static Cart CreateCart(string name, bool isPrimary, string folderId, List<CartAccount> cartAccounts, string userId, string gridTemplateId, int gridOptionId, List<CommonGridTemplateLine> gridLines)
        {
            #region Input Logging
            //log input data
            var inputLogMess = String.Format("CreateCart(Name:{0}, isPrimary:{1}, FolderId:{2}, CartAccounts:, UserID:{3} ",
                                        name, isPrimary, folderId, userId);
            Logger.Write("CartDAOManager", inputLogMess, false);
            #endregion

            Cart cart = null;
            List<CartAccountSummary> cartAccountSummaries = null;
            if (cartAccounts != null)
            {
                cartAccountSummaries = GetCartAccountDictionary(cartAccounts);
            }

            try
            {
                var cartId = CartDAO.Instance.CreateCart(name, isPrimary, folderId, cartAccountSummaries, userId, gridTemplateId, gridOptionId, gridLines);

                if (String.IsNullOrEmpty(cartId))
                    throw new CartManagerException(CartManagerException.CART_CREATE_FAILED);

                cart = (new CartDAOManager()).GetCartById(cartId, userId);
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }

            #region Output Logging
            //log output data
            Logger.Write("CartDAOManager", cart.GetCartLogString(), false);
            #endregion

            return cart;
        }

        private static List<CartAccountSummary> GetCartAccountDictionary(List<CartAccount> accounts)
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

        internal static string GenerateNewBasketName(string basketName, string userId)
        {
            var result = string.Empty;
            try
            {
                return result = CartDAO.Instance.GenerateNewBasketName(basketName, userId);
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }

            return result;
        }
        internal void MoveLineItems(List<LineItem> lineItems, string sourceCartId, string destinationCartId, string userId, out string PermissionViolationMessage)
        {
            PermissionViolationMessage = string.Empty;
            try
            {
                var lineItemsDict = BuildLineItemDictionary(lineItems);
                CartDAO.Instance.MoveLineItems(lineItemsDict, sourceCartId, destinationCartId, userId, CartFrameworkHelper.MaxLinesPerCart, out PermissionViolationMessage);
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }
        }
        private static Dictionary<string, Dictionary<string, string>> BuildLineItemDictionary(IEnumerable<LineItem> lineItems)
        {
            var dictLineItems = new Dictionary<string, Dictionary<string, string>>();

            foreach (var lineItem in lineItems)
            {
                var dicValues = new Dictionary<string, string>
                                    {
                                        {"BasketLineItemID", lineItem.Id},
                                        {"BTKey", lineItem.BTKey},
                                        {"quantity", lineItem.Quantity.ToString()},
                                        {"POLineItemNumber", lineItem.PONumber},
                                        {"Note", lineItem.IsOriginalEntryItem ? lineItem.BTLineItemNote : string.Empty},
                                        {"BibNumber", lineItem.Bib},
                                        {"PrimaryResponsiblePartyRedundant", lineItem.Author},
                                        {"ShortTitleRedundant", lineItem.Title}
                                    };

                dictLineItems.Add(lineItem.Id, dicValues);
            }

            return dictLineItems;
        }
        public Cart GetCartByIdForSubmitting(string cartId, string userId)
        {
            Cart cart = null;

            if (string.IsNullOrEmpty(cartId)) return null;

            try
            {
                int nonRankedCount = 0;
                var cartDs = CartDAO.Instance.GetCartForSubmitting(cartId, userId, out nonRankedCount);
                cart = GetCartSummaryFromDataSet(cartDs);
                cart.NonRankedCount = nonRankedCount;
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }

            return cart;
        }

        public static string GetOrderedDownloadedUser(string cartId)
        {
            string returnValue = "";
            try
            {
                returnValue = CartDAO.Instance.GetOrderedDownloadedUser(cartId);
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }
            return returnValue;
        }
    }

    public static class CartBatchEntryManager
    {
        public static void AddLineItemsToCart(List<LineItem> lineItems, string cartId, out string PermissionViolationMessage,
            out int totalAddingQtyForGridDistribution, string userId)
        {
            var i = 0;
            const int batchSize = 2000;
            var count = batchSize;
            //CartDAO.Instance.InitializeConnectionString();
            PermissionViolationMessage = null;
            totalAddingQtyForGridDistribution = 0;

            var cartManager = new CartManager(userId);

            while (count == batchSize)
            {
                var items = lineItems.Skip(i).Take(batchSize);
                count = items.Count();
                if (count == 0) break;
                i += count;
                int tempAdding = 0;
                cartManager.AddToCartName(cartId, items.ToList(), out PermissionViolationMessage, out tempAdding, false);
                //CartMapping.Instance.AddToCartName(cartId, items.ToList(), out PermissionViolationMessage, out tempAdding, false);
                totalAddingQtyForGridDistribution += tempAdding;
            }
        }
    }
}
