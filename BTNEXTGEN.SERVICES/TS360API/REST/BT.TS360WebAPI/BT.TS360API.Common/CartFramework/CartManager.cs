using BT.TS360API.Common.Business;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Models;
using BT.TS360API.Common.Search;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.CartFramework
{
    public class CartManager
    {
        /// <summary>
        /// Gets or sets the User id.
        /// </summary>
        /// <value>UserId.</value>
        public string UserId { get; set; }
        public bool IsCartManagerForUser { get; set; }
        public string OrganizationId { get; set; }
        public CartManager(string userId)
        {
            this.UserId = userId;
        }

        public CartManager(string userId, string organizationId, bool isCartManagerForUser)
        {
            if (isCartManagerForUser)
            {
                if (string.IsNullOrEmpty(userId))
                {
                    //throw exception code
                }
                this.UserId = userId;
                this.IsCartManagerForUser = true;
            }
            else
            {
                if (string.IsNullOrEmpty(organizationId))
                {
                    //throw exception code
                }
                this.OrganizationId = organizationId;
                this.IsCartManagerForUser = false;
            }

            //Initial Class
            //this.CartFolders = new CartFolders();
            //this.Carts = new Carts(this.UserId);
        }
        #region Move WCF to api

        public CartManager() { }
        public CartManager GetCartManagerForUser(string userId)
        {

            if (string.IsNullOrEmpty(userId))
                return null;

            return new CartManager(userId, null, true);

        }
        public QuickCart GetCartDetailsQuickView(string cartId, int pageNumber, short pageSize, string sortBy, byte sortDirection, bool recalculateHeader = true, bool getFast = true)
        {
            if (string.IsNullOrEmpty(cartId) || string.IsNullOrEmpty(UserId))
            {
                return null;
            }

            //sortBy = CommonHelper.SortByDict.ContainsKey(sortBy) ? CommonHelper.SortByDict[sortBy] : CommonHelper.QuickCartDetailsSortByDB.Title.ToString();
            sortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : CommonHelper.QuickCartDetailsSortByDB.Title.ToString();

            var quickCart = CartDAOManager.GetCartDetailsQuickView(cartId, UserId, sortBy, sortDirection, pageSize, pageNumber, recalculateHeader);
            if (quickCart != null)
            {
                if (getFast && quickCart.LineItems != null && quickCart.LineItems.Count > 0)
                {
                    var btkeys = quickCart.LineItems.Where(x => !x.IsOEItem).Select(x => x.BTKey);
                    var searchProducts = ProductSearchController.SearchByIdWithoutAnyRules(btkeys.ToList());
                    if (searchProducts != null && searchProducts.Items != null)
                    {
                        foreach (var lineItem in quickCart.LineItems)
                        {
                            var product = searchProducts.Items.FirstOrDefault(x => x.BTKey == lineItem.BTKey);
                            if (product != null)
                            {
                                lineItem.Title = product.Title;
                                lineItem.Author = product.AuthorText;
                                lineItem.ISBN = product.ISBN;
                                lineItem.UPC = product.Upc;
                                lineItem.Format = product.FormatLiteral;
                                lineItem.NumOfDiscs = product.NumOfDiscs;
                                lineItem.ESupplier = product.ESupplier;
                                lineItem.ProductType = product.ProductType;
                                lineItem.PublishedDate = product.PublishDate;
                                lineItem.PurchaseOption = product.PurchaseOption;
                                lineItem.HasJacket = product.HasJacket;
                                lineItem.Publisher = product.Publisher;
                                lineItem.HasFamilyKey = product.HasFamilyKey;
                                lineItem.Catalog = product.Catalog;
                                lineItem.HasAnnotations = product.HasAnnotations;
                                lineItem.HasReview = product.HasReview;
                                lineItem.HasReturn = product.HasReturn;
                                lineItem.HasMuze = product.HasMuze;
                                lineItem.HasExcerpt = product.HasExcerpt;
                                lineItem.HasToc = product.HasToc;
                                lineItem.MerchCategory = product.MerchCategory;
                                lineItem.Edition = product.Edition;
                                lineItem.ProductLine = product.ProductLine;
                                lineItem.SupplierCode = product.SupplierCode;
                                lineItem.IncludedFormatClass = product.IncludedFormatClass;
                            }
                        }
                    }
                }
                //if (quickCart.CartInfo != null)
                //{
                //    var owner = ProfileController.GetUserById(quickCart.CartInfo.CartOwnerID);
                //    if (owner != null)
                //    {
                //        quickCart.CartInfo.CartOwnerName = owner.UserLoginName;
                //    }
                //    WriteCartHeaderToCache(quickCart.CartInfo, quickCart.CartId);
                //}
                //else
                //{
                //    quickCart.CartInfo = ReadCartHeaderFromCache(CartId, UserId);
                //}
            }
            return quickCart;
        }

        public List<AccountSummary> GetAccountsSummary(string cardId)
        {
            return CartFarmCacheHelper.GetAccountsSummary(cardId);
        }

        public bool IsHomeDeliveryCart(List<AccountSummary> accountSummaries)
        {
            var cartAccounts = DataConverter.ConvertListAccountSummaryToListCartAccount(accountSummaries);
            if (cartAccounts == null || cartAccounts.Count < 1)
            {
                return false;
            }

            return cartAccounts.Any(cartAccount => cartAccount.IsHomeDelivery);
        }

        #endregion

        public Cart GetPrimaryCart()
        {
            //return Carts.GetPrimaryCart();
            var cart = CartFarmCacheHelper.GetPrimaryCart(this.UserId);
            //if (cart == null) return cart;
            //var cartdeletedinProcessing = VelocityCacheManager.Read(string.Format(SessionVariableName.CartIdProcessingToBeDeleted, this.UserId), VelocityCacheLevel.Session);
            //if (cartdeletedinProcessing != null && cartdeletedinProcessing.ToString().Equals(cart.CartId, StringComparison.OrdinalIgnoreCase))
            //{
            //    return null;
            //}
            return cart;
        }

        public async Task<Cart> GetCartByName(string cartName)
        {
            return await CartDAOManager.Instance.GetCartByName(cartName, this.UserId);
        }

        /// <summary>
        /// Adds a list of items to cart.
        /// </summary>
        /// <param name="basketId">The basket.</param>
        /// <param name="addLineItems">The list of line items to add to cart.</param>
        public void AddToCartName(string basketId, List<LineItem> addLineItems, out string PermissionViolationMessage,
            out int totalAddingQtyForGridDistribution, bool needToReprice = true)
        {
            PermissionViolationMessage = null;
            totalAddingQtyForGridDistribution = 0;
            if (!string.IsNullOrEmpty(basketId))
            {
                //Cart cart = CartContext.Current.GetCartManagerForUser(SiteContext.Current.UserId).GetCartById(basketId);
                //cart.AddLineItems(addLineItems, out PermissionViolationMessage, out totalAddingQtyForGridDistribution, needToReprice);
                AddLineItems(basketId, addLineItems, out PermissionViolationMessage, out totalAddingQtyForGridDistribution, needToReprice);
            }
        }

        public async Task<AddToCartOuput> AddToCartNameWithSharedUser(string productId, string variantId, int quantity,
                                                            string catalog, string basketName, string productType,
                                                            string note, string isbn, string gtin, string upc,
                                                            string poPerLine, string title, string author, string bibNumber = "", string sharedUserId = "")
        {
            //var basket = GetBasketByName(basketName);
            var basket = await GetCartByName(basketName);

            var addToCartOutput = new AddToCartOuput();
            addToCartOutput.PermissionViolationMessage = "";
            addToCartOutput.totalAddingQtyForGridDistribution = 0;

            var PermissionViolationMessage = "";
            var totalAddingQtyForGridDistribution = 0;

            if (!string.IsNullOrEmpty(productId) && !string.IsNullOrEmpty(catalog) && basket != null)
            {
                if (string.IsNullOrEmpty(variantId))
                {
                    variantId = null;
                }

                productType = CommonHelper.MapProductTypeToNumber(productType);

                var lineItem = new LineItem
                {
                    CatalogName = catalog,
                    ProductId = productId,
                    VariantId = variantId,
                    Quantity = quantity,
                    BTItemType = productType,
                    BTKey = productId,
                    BTGTIN = gtin,
                    BTISBN = isbn,
                    BTUPC = upc,
                    BTLineItemNote = note,
                    PONumber = poPerLine,
                    Bib = bibNumber,
                    Title = title,
                    Author = author
                };
                //var cart = CartContext.Current.GetCartManagerForUser(this.UserId).GetCartById(basket.CartId);
                var cart = GetCartById(basket.CartId);
                if (cart != null)
                {
                    if (!string.IsNullOrEmpty(sharedUserId))
                        cart.UserId = sharedUserId;
                    //cart.AddLineItems(new List<LineItem>() { lineItem }, out PermissionViolationMessage, out totalAddingQtyForGridDistribution);
                    AddLineItems(cart.CartId, new List<LineItem>() { lineItem }, out PermissionViolationMessage, out totalAddingQtyForGridDistribution);

                    addToCartOutput.PermissionViolationMessage = PermissionViolationMessage;
                    addToCartOutput.totalAddingQtyForGridDistribution = totalAddingQtyForGridDistribution;
                }
            }

            if (basket == null)
            {
                addToCartOutput.CartId = "";
                return addToCartOutput;
                //return string.Empty;
            }

            addToCartOutput.CartId = basket.CartId;
            return addToCartOutput;
            //return basket.CartId;
        }

        public void AddProductToCart(List<ProductLineItem> products, Dictionary<string, List<CommonCartGridLine>> cartGridLines, string toCartId,
            out string PermissionViolationMessage, out int totalAddingQuantity)
        {
            //Refer to FS_Master_Add_NG32_Original_Entry
            CartGridDAOManager.Instance.AddProductsToCart(products, cartGridLines, toCartId, this.UserId, out PermissionViolationMessage, out totalAddingQuantity);
        }

        /// <summary>
        /// Get a cart with the specified id
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public Cart GetCartById(string cartId)
        {
            //if cart is primary
            var isPrimary = CartFarmCacheHelper.IsPrimaryCart(cartId, this.UserId);
            if (isPrimary)
            {
                return CartFarmCacheHelper.GetPrimaryCartNotForMinicart(this.UserId, cartId);
            }

            ////normal cart
            //var cartCache = CartCacheManager.GetCartFromCache(cartId);
            //if (cartCache != null && cartCache.BTStatus != CartStatus.Submitted.ToString() &&
            //                cartCache.BTStatus != CartStatus.Ordered.ToString())
            //    return cartCache;

            //not found cart in cache or submitted cart
            var cart = CartDAOManager.Instance.GetCartById(cartId, this.UserId);
            //if (cart != null)
            //{
            //    CartCacheManager.AddCartToCache(cart);
            //}

            return cart;
        }

        /// <summary>
        /// Get Cart Folders.
        /// </summary>
        /// <returns></returns>
        public List<CartFolder> GetCartFolders()
        {
            if (string.IsNullOrEmpty(this.UserId)) throw new CartManagerException(CartManagerException.USER_ID_NULL);

            var cartFoldersCache = CartCacheManager.GetCartFoldersFromCache(this.UserId);
            if (cartFoldersCache != null)
                return cartFoldersCache;

            var cartFolders = CartDAOManager.GetCartFolders(this.UserId);

            if (cartFolders != null)
                CartCacheManager.AddCartFoldersToCache(cartFolders, this.UserId);

            return cartFolders;
        }

        /// <summary>
        /// Gets the cart folder list by parent folder id.
        /// </summary>
        /// <param name="parentFolderId">The parent folder id.</param>
        /// <returns></returns>
        public ICollection<CartFolder> GetCartFolderListByParentFolderId(string parentFolderId, bool includedShared = false)
        {
            return GetNormalBasketFoldersByParentFolderId(parentFolderId, includedShared);
        }

        /// <summary>
        /// Gets the cart folder list by parent folder id.
        /// </summary>
        /// <param name="parentFolderId">The parent folder id.</param>
        /// <param name="allFolders">All folders.</param>
        /// <returns></returns>
        public ICollection<CartFolder> GetCartFolderListByParentFolderId(string parentFolderId,
                                                                         ICollection<CartFolder> allFolders)
        {
            return GetNormalBasketFoldersByParentFolderId(parentFolderId);
        }

        /// <summary>
        /// Gets the normal basket folders by parent folder id.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <returns></returns>
        public ICollection<CartFolder> GetNormalBasketFoldersByParentFolderId(string parentId, bool includedShared = false)
        {
            return GetNormalBasketFoldersByParentFolderId(parentId, this.UserId, includedShared);
        }

        /// <summary>
        /// Gets the normal basket folders by parent folder id.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="includedShared"></param>
        /// <returns></returns>
        public ICollection<CartFolder> GetNormalBasketFoldersByParentFolderId(string parentId, string userId, bool includedShared = false)
        {
            if (string.IsNullOrEmpty(userId)) return new Collection<CartFolder>();
            if (!includedShared)
            {
                return
                GetFoldersByParentFolderId(parentId, userId).Where(
                    item =>
                    item.FolderType == CartFolderType.NormalFolderType ||
                    item.FolderType == CartFolderType.DefaultFolderType ||
                    item.FolderType == CartFolderType.RootFolderType).
                    ToList();
            }
            return
                GetFoldersByParentFolderId(parentId, userId).Where(
                    item =>
                    item.FolderType == CartFolderType.NormalFolderType ||
                    item.FolderType == CartFolderType.DefaultFolderType ||
                    item.FolderType == CartFolderType.SharedReceivedFolderType ||
                    item.FolderType == CartFolderType.RootFolderType).
                    ToList();
        }

        /// <summary>
        /// Gets the folders by parent folder id.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public ICollection<CartFolder> GetFoldersByParentFolderId(string parentId, string userId)
        {
            var cartFolders = this.GetCartFolders();
            if (cartFolders != null)
            {
                return
                    cartFolders.Where(
                        cartFolder =>
                        string.Compare(cartFolder.ParentFolderId, parentId, StringComparison.OrdinalIgnoreCase) == 0).
                        ToList();
            }
            return null;
        }

        public void AddLineItems(string cartId, List<LineItem> lineItems, out string PermissionViolationMessage,
            out int totalAddingQtyForGridDistribution, bool needToReprice = true)
        {
            #region Validation
            if (cartId == null) throw new CartManagerException(CartManagerException.CART_ID_NULL);
            if (string.IsNullOrEmpty(UserId)) throw new CartManagerException(CartManagerException.USER_ID_NULL);
            #endregion

            CartDAOManager.AddLineItemsToCart(lineItems, cartId, this.UserId, out PermissionViolationMessage, out totalAddingQtyForGridDistribution);

            //NotifyCartLineChanged(this.CartId);

            //if (needToReprice)
            //{
            //    this.WcfServiceCalculatePrice(this.CartId);
            //}
        }

        public Dictionary<string, int> GetQuantitiesByBtKeys(string cartId, List<string> btkeys)
        {
            return CartDAOManager.GetQuantitiesByBtKeys(cartId, this.UserId, btkeys);
        }

        public List<List<string>> CheckBasketForTitles(string cartId, List<string> btkeys, List<string> lineItemIds,
            out int newLineCount, out int existingLineCount)
        {
            return CartDAOManager.CheckBasketForTitles(cartId, btkeys, lineItemIds, out newLineCount, out existingLineCount);
        }

        public void SetPrimaryCartChanged()
        {
            NotifyPrimaryCartChanged();
        }

        public void SetCartChanged(string cartId)
        {
            NotifyCartChanged(cartId);
            //this.Carts.ResetCacheCartById(cartId);
        }

        internal virtual void NotifyCartChanged(string cartId)
        {
            CartCacheManager.SetCartCacheExpired(cartId);
            if (CartFarmCacheHelper.IsPrimaryCart(cartId, this.UserId))
            {
                NotifyPrimaryCartChanged();
            }
            CartCacheManager.SetTopNewestCartCacheExpired(this.UserId);
        }

        internal virtual void NotifyPrimaryCartChanged()
        {
            CartFarmCacheHelper.SetExpiredPrimaryCart(this.UserId);
        }
        public string GenerateNewBasketName(string userId, string cartId)
        {

            var cart = GetCartById(cartId);
            if (cart != null)
            {
                return GenerateNewBasketName(cart.CartName);
            }
            return string.Empty;
        }


        private string GenerateNewBasketName(string basketName)
        {
            if (string.IsNullOrEmpty(basketName))
            {
                throw new CartManagerException(CartManagerException.CART_NAME_NULL);
            }
            if (string.IsNullOrEmpty(this.UserId))
            {
                throw new CartManagerException(CartManagerException.USER_ID_NULL);
            }

            return CartDAOManager.GenerateNewBasketName(basketName, this.UserId);
        }
        public void NotifyCartChangeAndReprice(string cartId, string userId = "", bool isAsync = true)
        {
            SetCartChanged(cartId);
            CartFrameworkHelper.CalculatePrice(cartId, isAsync);
        }
        public AppServiceResult<string> CreateNewIIICartWithErrorsItems(string userId, string currentCartId, List<string> selectedLineItemsId, List<string> selectedBtKeys, out string newCartId)
        {
            var result = new AppServiceResult<string> { Status = AppServiceStatus.Success };
            newCartId = string.Empty;
            string PermissionViolationMessage = "";
            try
            {
                var cartManager = new CartManager(userId);
                var currentCart = cartManager.GetCartById(currentCartId);
                string errorCartName = cartManager.GenerateNewBasketName(userId, currentCart.CartId);
                Cart errorCart = cartManager.CreateCart(errorCartName, false, currentCart.CartFolderID, currentCart.CartAccounts);
                newCartId = errorCart.CartId;
                var lineItems = new List<LineItem>();
                for (var i = 0; i < selectedLineItemsId.Count; i++)
                {
                    var lineItem = new LineItem { Id = selectedLineItemsId[i], BTKey = selectedBtKeys[i] };

                    lineItems.Add(lineItem);
                }

                //var cart = new Cart(basketId, userId, string.Empty);
                cartManager.MoveLineItems(currentCart.CartId, lineItems, errorCart.CartId, out PermissionViolationMessage, userId);
                if (!string.IsNullOrEmpty(PermissionViolationMessage))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = PermissionViolationMessage;
                }
                result.Data = errorCartName;

                // #35993 - 
                // Take out the background pricing call from III orgiinal cart, as we call the pricing roll up stored procedure to force the update the orginal cart
                //cartManager.NotifyCartChangeAndReprice(currentCart.CartId, userId);

                cartManager.NotifyCartChangeAndReprice(errorCart.CartId, userId);
            }
            catch (CartManagerException cartManagerException)
            {
                Logger.Write("MoveLineItemsToOtherCart", cartManagerException.ToString(), !cartManagerException.isBusinessError);
                switch (cartManagerException.Message)
                {
                    case "AddToCart_CartLimitation":
                        result.Status = AppServiceStatus.LimitationFail;
                        result.ErrorMessage = "";
                        return result;
                    case CartManagerException.USER_HAS_NO_RIGHTS_TO_MOVE_OR_DELETE_ITEMS:
                        result.Status = AppServiceStatus.Fail;
                        result.ErrorMessage = "You don't have enough rights to move or delete these line items."; //GetLocalizedString("OrderResources","ItemActions_User_Has_No_Rights_To_Move_Delete_Items");
                        return result;
                    case "NoPermissionToAddProductToCart":
                        result.Status = AppServiceStatus.Fail;
                        result.ErrorMessage = "You don't have permission to add product to this cart."; //GetLocalizedString("OrderResources", "NoPermissionToAddProductToCart");
                        return result;
                    case "Baskets not updated, one or more line items were in an ordered basket.":
                        result.Status = AppServiceStatus.Fail;
                        result.ErrorMessage = cartManagerException.Message;
                        return result;
                    default:
                        result.Status = AppServiceStatus.Fail;
                        result.ErrorMessage = cartManagerException.Message;
                        return result;
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Order);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
        public Cart CreateCart(string name, bool isPrimary, string folderID, List<CartAccount> cartAccounts,
          string gridTemplateId = "", int gridOptionId = 0,
          List<CommonGridTemplateLine> gridLines = null)
        {
            #region Validation
            if (string.IsNullOrEmpty(name)) throw new CartManagerException(CartManagerException.CART_NAME_NULL);
            if (string.IsNullOrEmpty(folderID)) throw new CartManagerException(CartManagerException.CART_FOLDER_ID_NULL);
            if (string.IsNullOrEmpty(this.UserId)) throw new CartManagerException(CartManagerException.USER_ID_NULL);
            #endregion

            return CreateCart(name, isPrimary, folderID, cartAccounts, this.UserId, gridTemplateId, gridOptionId, gridLines);
        }

        internal static Cart CreateCart(string name, bool isPrimary, string folderId, List<CartAccount> cartAccounts, string userId, string gridTemplateId, int gridOptionId, List<CommonGridTemplateLine> gridLines)
        {
            var cart = CartDAOManager.CreateCart(name, isPrimary, folderId, cartAccounts, userId, gridTemplateId, gridOptionId, gridLines);

            //Notify carts changed

            CartFarmCacheHelper.SetExpiredPrimaryCart(userId);
            CartFarmCacheHelper.SetTopNewestCartCacheExpired(userId);
            ////Adding to cache
            //if (isPrimary)
            //{
            //    CartCacheManager.AddPrimaryCartToCache(cart, userId);
            //}

            return cart;
        }
        internal virtual void NotifyCartLineChanged(string cartId)
        {
            this.NotifyCartChanged(cartId);
        }
        public void MoveLineItems(string cartId, List<LineItem> lineItems, string destinationCartId, out string PermissionViolationMessage, string userId = "")
        {
            #region Validation
            if (lineItems == null || lineItems.Count == 0) throw new CartManagerException(CartManagerException.CART_LINE_ITEM_NULL);
            if (destinationCartId == null) throw new CartManagerException(CartManagerException.DESTINATION_CART_ID_NULL);
            if (string.IsNullOrEmpty(UserId)) throw new CartManagerException(CartManagerException.USER_ID_NULL);
            #endregion

            var contextUserId = this.UserId;
            if (userId != "" && this.UserId != userId)
            {
                contextUserId = userId;
            }
            CartDAOManager.Instance.MoveLineItems(lineItems, cartId, destinationCartId, contextUserId, out PermissionViolationMessage);

            this.NotifyCartLineChanged(cartId);

            this.NotifyCartLineChanged(destinationCartId);

            // TODO : testing needed if reprising done on page load so these code not loaded 

            ////this.WcfServiceCalculatePrice(cartId);
            ////this.WcfServiceCalculatePrice(destinationCartId);

            //CartFrameworkHelper.CalculateCartPriceInBackground(cartId, targetingValue);
            //CartFrameworkHelper.CalculateCartPriceInBackground(destinationCartId, targetingValue);
        }
        public Basket GetBasketById(string basketId, string userId)
        {
            var cart = GetCartById(basketId, userId);
            if (cart != null)
            {
                return ToBasket(cart, userId);
            }
            return null;
        }
        public Cart GetCartById(string cartId, string userId, bool isSubmitOrder = false)
        {
            CartManager cartManager = GetCartManagerForUser(userId);

            var cart = isSubmitOrder ? GetCartByIdForSubmitting(cartId) : cartManager.GetCartById(cartId);

            //AddCartObjectToSessionCacheForOneRequest(cart);

            return cart;
        }

        internal Cart GetCartByIdForSubmitting(string cartId)
        {
            return CartDAOManager.Instance.GetCartByIdForSubmitting(cartId, this.UserId);
        }
        public Basket ToBasket(Cart cart, string userId = "", bool flatStuff = false)
        {
            var basket = new Basket
            {
                Id = cart.CartId,
                Name = cart.CartName,
                TotalBackOrderQuantity = cart.TotalBackOrderQuantity,
                TotalCancelQuantity = cart.TotalCancelQuantity,
                Total = cart.Total,
                CartTotalListPrice = cart.CartTotalListPrice,
                CartTotalNetPrice = cart.CartTotalNetPrice,
                CartTotal = cart.Total,
                OrganizationId = cart.OrgId,
                UserId = cart.CartOwnerId,
                CartUserSharedId = cart.CartUserSharedId,
                LastUpdated = cart.UpdatedDateTime,
                DateCreated = cart.CreatedDateTime,
                SpecialInstructions = cart.SpecialInstruction,
                BTNote = cart.Note,
                FolderId = cart.CartFolderID,
                FolderName = cart.CartFolderName,
                LineItemsCount = cart.LineItemCount,
                BTStatus = cart.BTStatus,
                BookAccountId = GetDefaulAccountId(cart.CartAccounts, (int)AccountType.Book),
                EntertainmentAccountId = GetDefaulAccountId(cart.CartAccounts, (int)AccountType.Entertainment),
                IsArchived = cart.IsArchived ? 1 : 0,
                ItemsCount = cart.TotalOrderQuantity,
                ShippingTotal = cart.ShippingTotal,
                CartOwner = cart.CartOwnerName,
                HasProfile = !flatStuff ? cart.HasProfile : cart.IsShared,
                IsShared = cart.IsShared,
                HasGridLine = cart.HasGridLine,
                IsPremium = cart.IsPremium,
                HasPermission = cart.HasPermission,
                OneClickMARCIndicator = cart.OneClickMARCIndicator,
                FTPErrorMessage = cart.FTPErrorMessage,
                HasOwner = cart.HasOwner,
                HasContribution = cart.HasContribution,
                HasReview = cart.HasReview,
                HasAcquisition = cart.HasAcquisition,
                CurrentWorkflowStage = cart.CurrentWorkflow,
                HasWorkflow = cart.HasWorkflow,
                HasReviewAcquisitionPermission = cart.HasReviewAcquisitionPermission,
                IsMixedProduct = cart.IsMixedProduct,
                ESPStateTypeId = cart.ESPStateTypeId,
                HasESPRanking = cart.HasESPRanking,
                ESPStateTypeLiteral = cart.ESPStateTypeLiteral,
                ContainsAMixOfGridNNonGrid = cart.ContainsAMixOfGridNNonGrid,
                IsPrimary = !flatStuff ? (string.IsNullOrEmpty(userId)
                                          ? IsPrimaryCart(cart.CartId) ? 1 : 0
                                          : IsPrimaryCartOfUser(cart.CartId, userId) ? 1 : 0)
                                   : (cart.IsPrimary ? 1 : 0)
            };
            return basket;
        }
        private static string GetDefaulAccountId(List<CartAccount> cartAccounts, int accountType)
        {
            if (cartAccounts != null && cartAccounts.Count > 0)
            {
                foreach (var cartAccount in cartAccounts)
                {
                    if (cartAccount.AccountType == accountType)
                    {
                        return cartAccount.ERPAccountGUID;
                    }
                }
            }
            return string.Empty;
        }
        public bool IsPrimaryCart(string cartId)
        {
            return IsPrimaryCart(cartId, this.UserId);
        }

        public bool IsPrimaryCartOfUser(string cartId, string userId)
        {
            return IsPrimaryCart(cartId, userId);
        }
        internal bool IsPrimaryCart(string cartId, string userId)
        {
            return CartFarmCacheHelper.IsPrimaryCart(cartId, userId);
        }

        public void SubmitOrder(string cartID, string loggedInUserId, out string newBasketName, out string newBasketId, out string newOEBasketName, out string newOEBasketID, bool isVIP, bool isOrderAndHold, string orderedDownloadedUserId)
        {

            Cart cart = GetCartById(cartID);
            cart.OrderForm = GetStoreAndCustomerView(cartID);

            CartDAOManager.Instance.SubmitCart(cart, loggedInUserId, loggedInUserId, out newBasketName, out newBasketId, out newOEBasketName, out newOEBasketID, isVIP, isOrderAndHold, orderedDownloadedUserId);
            
        }

        internal static OrderForm GetStoreAndCustomerView(string cartId)
        {
            try
            {
                var orderFormDs = CartDAO.Instance.GetStoreAndCustomerView(cartId);

                return GetStoreAndCustomerViewFromDataSet(orderFormDs);
            }
            catch (SqlException sqlEx)
            {
                HandleSqlException(sqlEx);
            }
            return null;
        }

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

        private static void HandleSqlException(SqlException exception)
        {
            Logger.LogException(exception);
            throw exception;
        }
       
    }
}
