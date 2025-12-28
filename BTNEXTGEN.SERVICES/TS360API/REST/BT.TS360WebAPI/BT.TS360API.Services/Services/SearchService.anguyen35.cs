using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.Common.Models;
using BT.TS360API.Cache;
using BT.TS360API.ServiceContracts.Product;
using BT.TS360Constants;
using BT.TS360API.Common.Search;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360API.ExternalServices;
using BT.TS360API.Common.Inventory;
using BT.TS360API.ServiceContracts.Inventory;
using BT.TS360SP;
using BT.TS360API.Common.Search.Helpers;
using TS360Constants;
using BT.TS360API.Marketing;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360API.Common.Controller;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360API.Common.CartFramework;
using BT.TS360API.Common;
using BT.TS360API.Common.Constants;
using BT.TS360API.Services.Common.Configuration;

namespace BT.TS360API.Services.Services
{
    public partial class SearchService
    {
        public int GetTotalQuantityFromDefaultGridTemplate(string cartId, bool isGridEnabled, string userId, string orgId, bool isAuthorizedtoUseAllGridCodes)
        {
            //copied from OrdersService.GetDefaultFromDefaultGridTemplate
            try
            {
                //if (SiteContext.Current.IsGridEnabled == false)
                if (isGridEnabled == false)
                {
                    return -1;
                }
                var qty = -1; // imply no default grid line
                //var currentContext = SiteContext.Current;
                //var orgId = currentContext.OrganizationId;
                //var userId = currentContext.UserId;

                //var manager = new GridTemplateManager(userId, orgId);
                //string gridTemplateId = manager.GetDefaultGridTemplateId(userId, cartId);
                string gridTemplateId = GridDAOManager.Instance.GetDefaultGridTemplateId(userId, cartId);

                if (!string.IsNullOrEmpty(gridTemplateId))
                {
                    //var dCGridLines = GridHelper.GetDCGridLinesForEditTemplateNewGrid(gridTemplateId, orgId, userId);
                    var dCGridLines = GetDCGridLinesForEditTemplateNewGrid(gridTemplateId, orgId, userId, isAuthorizedtoUseAllGridCodes);

                    if (dCGridLines != null && dCGridLines.Count > 0)
                    {
                        qty = 0;
                        foreach (var dCGridLine in dCGridLines)
                        {
                            int gridLineqty;
                            if (int.TryParse(dCGridLine.Quantity, out gridLineqty))
                                qty += gridLineqty;
                        }
                    }
                }
                return qty;
            }
            catch
            {
                return -1;
            }
        }

        public static List<DCGridLine> GetDCGridLinesForEditTemplateNewGrid(string templateId, string orgId, string userId, bool isAuthorizedtoUseAllGridCodes)
        {
            //var manager = new GridTemplateManager(userId, orgId);
            //var templateLines = manager.LoadGridTemplateLines(templateId);

            var templateLines = GridDAOManager.Instance.LoadGridTemplateLines(templateId, userId, orgId, isAuthorizedtoUseAllGridCodes);

            return ConvertGridTemplateLinesToDCGridLines(templateLines, false);
        }

        private static List<DCGridLine> ConvertGridTemplateLinesToDCGridLines(List<CommonGridTemplateLine> templateLines, bool isAuthorizedGridLinesOnly = false)
        {
            if (templateLines == null || !templateLines.Any()) return null;
            var dcGridLines = new List<DCGridLine>();

            foreach (var templateLine in templateLines)
            {
                var dcGridFieldCodes = new List<DCGridFieldCode>();
                var fieldCodes = templateLine.GridFieldCodeList;
                var hasUnauthorizedGridCode = false;
                var isExpiredOrFutureDate = false;
                var hasDisabledGridCode = false;

                foreach (var fieldCode in fieldCodes)
                {
                    dcGridFieldCodes.Add(new DCGridFieldCode()
                    {
                        GridCode = fieldCode.GridCode,
                        GridCodeId = fieldCode.GridCodeId,
                        GridCodeText = fieldCode.GridText,
                        GridFieldId = fieldCode.GridFieldId,
                        GridFieldType = fieldCode.GridFieldType.ToString(),
                        IsFreeText = fieldCode.IsFreeText.ToString(),
                        IsAuthorized = fieldCode.IsAuthorized.ToString(),
                        IsExpired = fieldCode.IsExpired.ToString(),
                        IsFutureDate = fieldCode.IsFutureDate.ToString(),
                        IsDisabled = fieldCode.IsDisabled.ToString()
                    });
                    if (fieldCode.IsDisabled) hasDisabledGridCode = true;
                    if (fieldCode.IsExpiredOrFutureDate) isExpiredOrFutureDate = true;
                    if (!fieldCode.IsAuthorized) hasUnauthorizedGridCode = true;
                }

                if (isAuthorizedGridLinesOnly && hasUnauthorizedGridCode) continue;

                var gridLineAuthorized = !hasDisabledGridCode && !isExpiredOrFutureDate && !hasUnauthorizedGridCode;

                dcGridLines.Add(new DCGridLine()
                {
                    ID = templateLine.ID,
                    DCGridFieldCodes = dcGridFieldCodes,
                    IsAuthorized = gridLineAuthorized.ToString(),
                    IsTempDisabled = templateLine.IsTempDisabled.ToString(),
                    Quantity = templateLine.Qty.ToString(),
                    Sequence = templateLine.Sequence.ToString()
                });
            }

            return dcGridLines;
        }

        public AppServiceResult<SearchItemDetailsNavBarInfoResponse> GetSearchItemDetailsNavBarInfo(SearchItemDetailsNavBarInfoRequest request)
        {
            var result = new AppServiceResult<SearchItemDetailsNavBarInfoResponse>();
            try
            {
                var response = new SearchItemDetailsNavBarInfoResponse();
                var dataFromSearchResults = request.DataFromSearchResults;

                List<string> btKeyList = null;
                int titleTotal = 0;
                if (dataFromSearchResults != null && dataFromSearchResults.CurrentPageBTKeys != null && dataFromSearchResults.CurrentPageBTKeys.Count() > 0)
                {
                    titleTotal = dataFromSearchResults.SearchTotalCount;
                    btKeyList = dataFromSearchResults.CurrentPageBTKeys.ToList();
                }

                bool foundItem = (btKeyList != null && btKeyList.Contains(request.BTKey));
                if (!foundItem && request.SearchArgs != null)
                {
                    // perform FAST search
                    var searchResults = ProductSearchController.Search(request.SearchArgs, request.MarketType, request.SimonSchusterEnabled, request.CountryCode, request.ESuppliers);
                    if (searchResults != null && searchResults.Items != null)
                    {
                        titleTotal = searchResults.TotalRowCount;
                        btKeyList = searchResults.Items.Select(r => r.BTKey).ToList();
                    }
                }

                // response data
                response.TitleTotal = titleTotal;
                if (btKeyList != null && btKeyList.Count() > 0)
                {
                    // calculate page total
                    var totalPage = titleTotal / request.SearchArgs.PageSize;
                    if (titleTotal % request.SearchArgs.PageSize != 0)
                    {
                        totalPage += 1;
                    }

                    var index = btKeyList.FindIndex(r => r == request.BTKey);
                    var isFirstPage = dataFromSearchResults.CurrentPageIndex <= 1;
                    response.TitleIndex = request.SearchArgs.StartRowIndex + index + 1;

                    // Previous Item data
                    if (index > 0)
                    {
                        response.PreviousBTKey = btKeyList[index - 1];
                        response.PageForPreviousBTKey = dataFromSearchResults.CurrentPageIndex;
                    }
                    else
                    {
                        // get items in previous page
                        int prePage = isFirstPage ? totalPage : (dataFromSearchResults.CurrentPageIndex - 1);
                        var btKeysInPrePage = GetProductBTKeysInPage(prePage, request);
                        if (btKeysInPrePage != null && btKeysInPrePage.Count > 0)
                        {
                            response.PreviousBTKey = btKeysInPrePage[btKeysInPrePage.Count - 1];
                            response.PageForPreviousBTKey = prePage;
                        }
                    }

                    // Next Item data
                    var nextIndex = index + 1;
                    var isLastPage = dataFromSearchResults.CurrentPageIndex == totalPage;
                    if (nextIndex == btKeyList.Count)
                    {
                        // get items in first or next page
                        var nextPage = isLastPage ? 1 : dataFromSearchResults.CurrentPageIndex + 1;
                        var btKeysInNextPage = GetProductBTKeysInPage(nextPage, request);
                        if (btKeysInNextPage != null && btKeysInNextPage.Count > 0)
                        {
                            response.NextBTKey = btKeysInNextPage[0];
                            response.PageForNextBTKey = nextPage;
                        }
                    }
                    else if (nextIndex < btKeyList.Count)
                    {
                        response.NextBTKey = btKeyList[nextIndex];
                        response.PageForNextBTKey = dataFromSearchResults.CurrentPageIndex;
                    }
                }

                result.Status = AppServiceStatus.Success;
                result.Data = response;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
            }
            return result;
        }

        private List<string> GetProductBTKeysInPage(int pageIndex, SearchItemDetailsNavBarInfoRequest request)
        {
            var btkeys = new List<string>();
            var searchAgr = request.SearchArgs;
            if (searchAgr != null)
            {
                searchAgr.StartRowIndex = (pageIndex - 1) * searchAgr.PageSize;
                var searchResults = ProductSearchController.Search(request.SearchArgs, request.MarketType, request.SimonSchusterEnabled, request.CountryCode, request.ESuppliers);
                if (searchResults != null && searchResults.Items != null && searchResults.Items.Count > 0)
                {
                    btkeys = searchResults.Items.Select(prod => prod.BTKey).ToList();
                }
            }

            return btkeys;
        }

        public AppServiceResult<CartItemDetailsNavBarInfo> GetCartItemDetailsNavBarInfo(CartItemDetailsNavBarInfoRequest request)
        {
            var result = new AppServiceResult<CartItemDetailsNavBarInfo>();
            try
            {
                var response = GetCartItemDetailsNavBarInfoResponse(request.UserId, request.CartId, request.LineItemId
                                                                    , request.SearchCartCriteria);
                result.Status = AppServiceStatus.Success;
                result.Data = response;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
            }
            return result;
        }

        private CartItemDetailsNavBarInfo GetCartItemDetailsNavBarInfoResponse(string userId, string cartId, string lineItemId, SearchCartCriteria searchCartCriteria, bool isQuickItemDetail = false)
        {
            var response = new CartItemDetailsNavBarInfo();

            if (!string.IsNullOrEmpty(cartId))
            {
                var cartManager = new CartManager(userId);
                var currentCart = cartManager.GetCartById(cartId);
                if (currentCart != null)
                {
                    response.CartName = currentCart.CartName;
                    response.IsPrimary = currentCart.IsPrimary;
                    response.LineItemCount = currentCart.LineItemCount;
                    response.TotalOrderQuantity = currentCart.TotalOrderQuantity;

                    // don't need to return cart price if cart is primary
                    if (!currentCart.IsPrimary)
                    {
                        var isPricing = CartFrameworkHelper.IsPricing(cartId, userId);
                        response.IsPricing = isPricing;

                        if (isPricing)
                            response.CartPrice = CommonResources.PricingStatusText;
                        else
                            response.CartPrice = CommonHelper.GetCurrencyFormat(currentCart.CartTotalNetPrice);
                    }

                    //var requestCriteria = request.SearchCartCriteria;
                    // cart has more than one line item
                    if (currentCart.LineItemCount > 1 && searchCartCriteria != null)
                    {
                        int totalLines;
                        var criteria = new SearchCartLinesCriteria(cartId, userId, searchCartCriteria);

                        // search and get line items
                        var cartLineList = CartDAOManager.Instance.GetCartLineIdAndBTKeyPairList(criteria, out totalLines);
                        if (cartLineList != null && cartLineList.Count > 0)
                        {
                            // return title index
                            var relativeIndex = cartLineList.FindIndex(r => r.LineItemID.Equals(lineItemId, StringComparison.CurrentCultureIgnoreCase));
                            var realIndexInCart = ((searchCartCriteria.PageNumber - 1) * searchCartCriteria.PageSize) + relativeIndex;
                            response.TitleIndex = realIndexInCart + 1;

                            // get previous item from result
                            response.PreviousLineItem = GetProductLinePreviousItem(cartLineList, criteria, totalLines, relativeIndex);

                            // get next item from result
                            response.NextLineItem = GetProductLineNextItem(cartLineList, criteria, totalLines, relativeIndex);
                        }
                    }
                }
            }

            return response;
        }

        private GoToCartLineItem GetProductLinePreviousItem(List<SimpleLineItem> cartLineList, SearchCartLinesCriteria searchCriteria, int totalLines, int relativeIndex)
        {
            var previousItem = new SimpleLineItem();
            int pageNumber = searchCriteria.PageNumber;

            if (relativeIndex > 0)
            {
                previousItem = cartLineList[relativeIndex - 1];
            }
            else if (relativeIndex == 0)    // current index is first, get last item for prevItem
            {
                if (searchCriteria.PageNumber > 1)
                {
                    pageNumber = searchCriteria.PageNumber - 1;
                }
                else
                {
                    // last page number
                    pageNumber = totalLines / searchCriteria.PageSize;
                    if (totalLines % searchCriteria.PageSize > 0)
                        pageNumber++;
                }

                // query from DB
                if (pageNumber != searchCriteria.PageNumber)
                {
                    var tempCriteria = new SearchCartLinesCriteria(searchCriteria.CartId, searchCriteria.UserId, searchCriteria);
                    tempCriteria.PageNumber = pageNumber;

                    // search and get line items
                    var tempCartLineList = CartDAOManager.Instance.GetCartLineIdAndBTKeyPairList(tempCriteria, out totalLines);
                    if (tempCartLineList != null && tempCartLineList.Count > 0)
                    {
                        //last item for response prevItem
                        previousItem = tempCartLineList[tempCartLineList.Count - 1];
                    }
                }
                else // cart has 1 page only
                {
                    previousItem = cartLineList[cartLineList.Count - 1]; 
                }
            }

            GoToCartLineItem result = null;
            if (!string.IsNullOrEmpty(previousItem.LineItemID))
            {
                result = new GoToCartLineItem
                {
                    LineItemID = previousItem.LineItemID,
                    BTKey = previousItem.BTKey,
                    BasketOriginalEntryID = previousItem.BasketOriginalEntryID,
                    PageNumber = pageNumber
                };
            }

            return result;
        }

        private GoToCartLineItem GetProductLineNextItem(List<SimpleLineItem> cartLineList, SearchCartLinesCriteria searchCriteria, int cartTotalLines, int relativeIndex)
        {
            var nextItem = new SimpleLineItem();
            var pageNumber = searchCriteria.PageNumber;

            // current line is not last in list
            if (relativeIndex < cartLineList.Count - 1)
            {
                nextItem = cartLineList[relativeIndex + 1];
            }
            else if (relativeIndex == cartLineList.Count - 1)    // current index is last in list
            {
                // last page number
                var lastPageNumber = cartTotalLines / searchCriteria.PageSize;
                if (cartTotalLines % searchCriteria.PageSize > 0)
                    lastPageNumber++;

                if (searchCriteria.PageNumber == lastPageNumber)
                {
                    pageNumber = 1;  // first page
                }
                else
                {
                    pageNumber = searchCriteria.PageNumber + 1;
                }

                // query from DB
                if (pageNumber != searchCriteria.PageNumber)
                {
                    var tempCriteria = new SearchCartLinesCriteria(searchCriteria.CartId, searchCriteria.UserId, searchCriteria);
                    tempCriteria.PageNumber = pageNumber;

                    // search and get line items
                    var tempCartLineList = CartDAOManager.Instance.GetCartLineIdAndBTKeyPairList(tempCriteria, out cartTotalLines);
                    if (tempCartLineList != null && tempCartLineList.Count > 0)
                    {
                        //first item from list
                        nextItem = tempCartLineList[0];
                    }
                }
                else // cart has 1 page only
                {
                    nextItem = cartLineList[0];
                }
            }

            GoToCartLineItem result = null;
            if (!string.IsNullOrEmpty(nextItem.LineItemID))
            {
                result = new GoToCartLineItem
                {
                    LineItemID = nextItem.LineItemID,
                    BTKey = nextItem.BTKey,
                    BasketOriginalEntryID = nextItem.BasketOriginalEntryID,
                    PageNumber = pageNumber
                };
            }

            return result;
        }

        public AppServiceResult<WCFObjectReturnToClient> GetStockPriceCheckForQuickSearch(string btKey, string productType,
            string ctxUserId, BT.TS360Constants.MarketType? ctxMarketType, string[] ctxESuppliers, bool ctxSimonSchusterEnabled,
            string ctxCountryCode, string[] ctxAudienceType, bool ctxEnableProcessingCharges, bool ctxIsHideNetPriceDiscountPercentage,
            decimal ctxBookProcessingCharge, decimal ctxMovieProcessingCharge, decimal ctxPaperbackProcessingCharge,
            decimal ctxMusicProcessingCharge, decimal ctxSpokenWordProcessingCharge, float ctxSalesTax, string ctxDefaultBookAccountId,
            string ctxDefaultEntertainmentAccountId, string ctxDefaultVipAccountId, string ctxOrgId, TargetingValues targeting, AccountInfoForPricing accountPricing)
        {
            var ajaxResult = new AppServiceResult<WCFObjectReturnToClient>();
            try
            {
                var wCfObjectReturnToClient = new WCFObjectReturnToClient
                {
                    Message = "Sequential",
                    InventoryResultsList = new List<InventoryResults>()
                };

                var pricingClientArg = new List<PricingClientArg>();
                var productItem = GetProductDetailsFromCache(btKey, ctxUserId, ctxMarketType, ctxESuppliers, ctxSimonSchusterEnabled, ctxCountryCode);
                if (productItem != null)
                {
                    pricingClientArg.Add(GetPricingClientArg(productItem));
                    wCfObjectReturnToClient.Pricing = GetProductPricing(pricingClientArg, ctxUserId, ctxESuppliers, ctxIsHideNetPriceDiscountPercentage, targeting, accountPricing).Data;
                }
                var vipAccountId = "";
                var defaultOrgVIPAccountId = "";
                var defaultEntertainmentAccountId = "";
                bool isHomeDeliveryCheck = false;
                bool isVIPCheck = false;
                var accountId = string.Compare(productType, ProductType.Book.ToString(), StringComparison.OrdinalIgnoreCase) == 0 ?
                    ctxDefaultBookAccountId : ctxDefaultEntertainmentAccountId;
                if (string.Compare(productType, ProductType.Book.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                {
                    vipAccountId = ctxDefaultVipAccountId;
                    defaultEntertainmentAccountId = ctxDefaultEntertainmentAccountId;
                }

                Account account = null;
                if (!string.IsNullOrEmpty(defaultEntertainmentAccountId))
                {
                    account = ProfileController.Instance.GetAccountById(accountId);
                    if (account != null && account.HomeDeliveryAccount.HasValue && account.HomeDeliveryAccount.Value)
                    {
                        isHomeDeliveryCheck = true;
                    }
                }

                if (string.IsNullOrEmpty(accountId) && !isHomeDeliveryCheck)
                {
                    account = GetDefaultAccountFromOrg(ctxOrgId, productType, out defaultOrgVIPAccountId, out isHomeDeliveryCheck);
                    if (account != null)
                    {
                        accountId = account.AccountId;
                    }

                    if (string.IsNullOrEmpty(accountId) && string.IsNullOrEmpty(vipAccountId) && string.IsNullOrEmpty(defaultOrgVIPAccountId) && !isHomeDeliveryCheck)
                    {
                        ajaxResult = new AppServiceResult<WCFObjectReturnToClient>
                        {
                            Status = AppServiceStatus.Fail,
                            ErrorMessage = "Default account not found. Cannot get real time inventory.",
                            Data = wCfObjectReturnToClient
                        };

                        return ajaxResult;
                    }
                }

                if (string.IsNullOrEmpty(accountId) && (!string.IsNullOrEmpty(vipAccountId) || !string.IsNullOrEmpty(defaultOrgVIPAccountId)) && string.Compare(productType, ProductType.Book.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                {
                    isVIPCheck = true;
                }

                if (isVIPCheck || isHomeDeliveryCheck)
                {
                    accountId = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.StockCheckDefaultSOPAccountID);
                }
                if (account == null && !isVIPCheck && !isHomeDeliveryCheck)
                {
                    account = ProfileController.Instance.GetAccountById(accountId);
                }
                string primaryWareHouse = "";
                string secondaryWareHouse = "";
                if (account != null)
                {
                    ProductSearchController.GetPrimarySecondaryWareHouse(out primaryWareHouse, out secondaryWareHouse, account);
                }
                else if (!isVIPCheck && !isHomeDeliveryCheck)
                {
                    ajaxResult = new AppServiceResult<WCFObjectReturnToClient>
                    {
                        Status = AppServiceStatus.Fail,
                        ErrorMessage = "Default account not found. Cannot get real time inventory.",
                        Data = wCfObjectReturnToClient
                    };
                    return ajaxResult;
                }

                try
                {
                    var displayInventoryForAllWareHouse = true;
                    if (!isVIPCheck && !isHomeDeliveryCheck)
                        displayInventoryForAllWareHouse = GetDisplayInventoryForAllWareHouse(account, ctxOrgId);
                    BT.TS360API.ExternalServices.BTStockCheckServices.StockCheckResponse response = new BT.TS360API.ExternalServices.BTStockCheckServices.StockCheckResponse();
                    if (isVIPCheck || isHomeDeliveryCheck)
                    {
                        response = InventoryHelper.GetRealTimeInventory(accountId, btKey);
                    }
                    else if (account != null)
                    {
                        response = InventoryHelper.GetRealTimeInventory(account.AccountNumber, btKey);
                    }
                    if (response != null && response.Warehouses != null && response.Warehouses.Length > 0)
                    {
                        var marketType = targeting.MarketType.HasValue ? targeting.MarketType.Value : BT.TS360Constants.MarketType.Any;

                        if (InventoryHelper.IsDisplaySuperWarehouse(marketType, ctxCountryCode, ctxOrgId) || InventoryHelper.IsDisplayVIPWarehouse(marketType, ctxCountryCode, ctxOrgId))
                        {
                            AddVIPToRealTimeInventory(ref response, btKey, marketType, ctxOrgId, ctxCountryCode);
                        }

                        var total30Demand = 0;
                        var listInvStockStatus = GetListInvStockStatusForQuickSearch(btKey, primaryWareHouse, secondaryWareHouse, response, out total30Demand, ctxUserId, ctxMarketType, ctxESuppliers, ctxSimonSchusterEnabled, ctxCountryCode);

                        var inventoryResults = new InventoryResults
                        {
                            DisplayInventoryForAllWareHouse = displayInventoryForAllWareHouse,
                            InventoryStock = listInvStockStatus,
                            BTKey = btKey,
                            ProductType = productType,
                            TotalLast30Demand = total30Demand
                        };

                        wCfObjectReturnToClient.InventoryResultsList.Add(inventoryResults);

                        ajaxResult.Status = AppServiceStatus.Success;
                        ajaxResult.Data = wCfObjectReturnToClient;
                        return ajaxResult;
                    }

                    var hasResponse = response != null;
                    var erroMessage = "No response from Stock Check service";
                    if (hasResponse)
                    {
                        erroMessage = ExtractErrorMessage(response.StatusMessage);
                    }
                    ajaxResult.ErrorMessage = erroMessage;
                    ajaxResult.Status = AppServiceStatus.Fail;
                    ajaxResult.Data = new WCFObjectReturnToClient();

                    return ajaxResult;
                }
                catch (Exception ex)
                {
                    Logger.RaiseException(ex, ExceptionCategory.Search);
                    ajaxResult.ErrorMessage = CatalogResources.RealTimeInventory_UnexpectedException;
                    ajaxResult.Status = AppServiceStatus.Fail;
                    ajaxResult.Data = new WCFObjectReturnToClient();

                    return ajaxResult;
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                ajaxResult.Status = AppServiceStatus.Fail;
            }
            return ajaxResult;
        }

        private static ProductSearchResultItem GetProductDetailsFromCache(string btKey, string userId, BT.TS360Constants.MarketType? marketType, string[] ESuppliers,
            bool simonSchusterEnabled, string countryCode)
        {
            var cacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.ProductDetails);
            var searchResultInfo = CachingController.Instance.Read(cacheKey) as ProductSearchResultItem;
            if (searchResultInfo != null && searchResultInfo.BTKey != btKey)
            {
                // FAST search
                var productIds = new List<string> { btKey };
                var searchResults = ProductSearchController.SearchByIdForContentManagement(productIds, marketType, simonSchusterEnabled, countryCode, ESuppliers);
                if (searchResults != null && searchResults.Items != null &&
                       searchResults.Items.Count > 0)
                {
                    CachingController.Instance.Write(cacheKey, searchResults.Items[0], 5);
                }
            }

            return searchResultInfo;
        }

        private void AddVIPToRealTimeInventory(ref BT.TS360API.ExternalServices.BTStockCheckServices.StockCheckResponse response, string btKey, BT.TS360Constants.MarketType marketType, string orgId, string countryCode, string cartId = "")
        {
            InventoryResults inventoryResults = GetInventoryResultForItemDetails(btKey, marketType, cartId, orgId, countryCode);
            InventoryResults vipInventoryResults = InventoryHelper.FilterVIPWarehousesInventory(inventoryResults, marketType, orgId, countryCode);
            var currentData = response.Warehouses.ToList();
            BT.TS360API.ExternalServices.BTStockCheckServices.WHS whs;

            foreach (InventoryStockStatus inventoryStockStatus in vipInventoryResults.InventoryStock)
            {
                whs = new BT.TS360API.ExternalServices.BTStockCheckServices.WHS();
                whs.QTYOnHand = Convert.ToInt32(inventoryStockStatus.OnHandInventory);
                whs.QTYOnOrder = Convert.ToInt32(inventoryStockStatus.OnOrderQuantity);
                whs.WHSCode = inventoryStockStatus.FormalWareHouseCode;
                whs.WHSDescription = inventoryStockStatus.WareHouse;
                currentData.Insert(0, whs);// .Add(whs);
            }

            response.Warehouses = currentData.ToArray();
        }

        private InventoryResults GetInventoryResultForItemDetails(string btKey, BT.TS360Constants.MarketType marketType, string cartId, string orgId, string countryCode)
        {
            //var cacheKey = string.Format("INVENTORY_RESULT_ITEM_DETAILS_{0}", btKey);

            //var result = CachingController.Instance.Read(cacheKey) as InventoryResults;
            //if (result != null) return result;

            var result = new InventoryResults();
            var productSearchResults = ProductSearchController.SearchByIdWithoutAnyRules(new List<string>() { btKey });
            if (productSearchResults != null &&
                productSearchResults.Items != null &&
                productSearchResults.Items.Count > 0)
            {
                var inventoryArg = new List<SearchResultInventoryStatusArg>()
                                   {
                                       GetSearchResultInventoryStatusArg((productSearchResults.Items[0]), marketType.ToString())
                                   };
                var mongoDbInstance = InventoryHelper4MongoDb.GetInstance(cartId, marketType: marketType, countryCode: countryCode, orgId: orgId);
                var results = mongoDbInstance.GetInventoryResultsForMultipleItems(inventoryArg);

                if (results != null && results.Any())
                {
                    result = results.First();
                }
            }
            //CachingController.Instance.Write(cacheKey, result);
            return result;
        }

        private List<InventoryStockStatus> GetListInvStockStatusForQuickSearch(string btKey, string primaryWareHouse, string secondaryWareHouse, BT.TS360API.ExternalServices.BTStockCheckServices.StockCheckResponse response, out int total30Demand,
            string userId, BT.TS360Constants.MarketType? marketType, string[] ESuppliers, bool simonSchusterEnabled, string countryCode)
        {
            var warehouseDemandData = GetWarehouseDemandData(btKey, userId, marketType, ESuppliers, simonSchusterEnabled, countryCode);

            if (warehouseDemandData == null)
            {
                total30Demand = 0;
            }
            else
            {
                total30Demand = warehouseDemandData.TotalLast30Demand;
            }

            var listInvStockStatus = new List<InventoryStockStatus>();
            var invVIPStockStatus = new List<InventoryStockStatus>();
            foreach (var warehouse in response.Warehouses)
            {
                int index = -1;

                var whsCode = warehouse.WHSCode.ToUpper().Trim();

                var isVipWhs = InventoryHelper.IsVIPWarehouse(whsCode);

                var whsDescription = warehouse.WHSDescription.ToUpper().Trim();
                if (string.IsNullOrEmpty(whsDescription))
                {
                    whsDescription = whsCode;
                }
                var demand = 0;
                if (warehouseDemandData != null)
                {
                    var wh = warehouseDemandData.Warehouses.FirstOrDefault(r => r.WarehouseId == whsCode);
                    demand = wh == null ? 0 : wh.Last30DayDemand;
                }

                if (string.Compare(whsCode, primaryWareHouse, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    whsDescription += " *";
                    whsCode += " *";
                    index = 0;
                }
                else if (string.Compare(whsCode, secondaryWareHouse, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    whsDescription += " **";
                    whsCode += " **";
                    index = 1;
                }
                else if (isVipWhs)
                {
                    if (!whsDescription.EndsWith("***"))
                        whsDescription += " ***";

                    if (!whsCode.EndsWith("***"))
                        whsCode += " ***";
                }

                var invStockStatus = new InventoryStockStatus
                {
                    WareHouse = whsDescription,
                    WareHouseCode = whsCode,
                    OnHandInventory = warehouse.QTYOnHand.ToString(),
                    OnOrderQuantity = warehouse.QTYOnOrder,
                    InvDemandNumber = demand,
                };

                if (index == 0)
                {
                    if (listInvStockStatus.Count == 0)
                    {
                        listInvStockStatus.Add(invStockStatus);
                    }
                    else
                    {
                        listInvStockStatus.Insert(0, invStockStatus);
                    }
                }
                else if (index == 1)
                {
                    if (listInvStockStatus.Count <= 1)
                    {
                        listInvStockStatus.Add(invStockStatus);
                    }
                    else
                    {
                        listInvStockStatus.Insert(1, invStockStatus);
                    }
                }
                else //VIP inventory
                {
                    if (isVipWhs)
                        invVIPStockStatus.Add(invStockStatus);
                    else
                        listInvStockStatus.Add(invStockStatus);
                }

            }
           
            if (invVIPStockStatus.Count > 0)
            {
                // Retail VIP inventory displays on International Org in last position as VIP.
                // Non Retail VIP inventory displays in the first position as VIP 
                if(marketType == BT.TS360Constants.MarketType.Retail)
                    listInvStockStatus.AddRange(invVIPStockStatus);
                else
                    listInvStockStatus.InsertRange(0, invVIPStockStatus);
            } 
 

            return listInvStockStatus;
        }

        private BTKeyInventoryResult GetWarehouseDemandData(string btKey, string userId, BT.TS360Constants.MarketType? marketType, string[] ESuppliers, bool simonSchusterEnabled, string countryCode)
        {
            var searchResultInfo = GetProductDetailsFromCache(btKey, userId, marketType, ESuppliers, simonSchusterEnabled, countryCode);
            if (searchResultInfo != null)
            {
                var warehouseData =
                    InventoryHelper4MongoDb.GetInstance(userId: userId)
                        .GetInventoryWarehouseData(new[] { GetSearchResultInventoryStatusArg(searchResultInfo, marketType.Value.ToString()) });
                if (warehouseData != null && warehouseData.ContainsKey(btKey))
                {
                    return warehouseData[btKey];
                }
            }
            return null;
        }

        private static string ExtractErrorMessage(string statusMessage)
        {
            //Special case for error code 99999
            if (statusMessage.Contains("99999"))
            {
                //return SiteContext.GetLocalizedString(ResourceName.CatalogResources, "RealTimeInventory_UnexpectedException");
                return CatalogResources.RealTimeInventory_UnexpectedException;
            }

            var colonIndex = statusMessage.IndexOf(':');
            if (colonIndex > 0)
            {
                var desStr = statusMessage.Substring(colonIndex + 1);
                return desStr;
            }
            //return SiteContext.GetLocalizedString(ResourceName.CatalogResources, "RealTimeInventory_UnexpectedException");
            return CatalogResources.RealTimeInventory_UnexpectedException;
        }

        //private List<InventoryStockStatus> GetListInvStockStatusForQuickSearch(string btKey, string primaryWareHouse, string secondaryWareHouse, StockCheckResponse response, out int total30Demand)
        //private List<InventoryStockStatus> GetListInvStockStatusForQuickSearch(string btKey, string primaryWareHouse, string secondaryWareHouse, StockCheckResponse response, out int total30Demand,
        //    string userId, MarketType? marketType, string[] ESuppliers, bool simonSchusterEnabled, string countryCode)
        //{
        //    var warehouseDemandData = GetWarehouseDemandData(btKey, userId, marketType, ESuppliers, simonSchusterEnabled, countryCode);

        //    if (warehouseDemandData == null)
        //    {
        //        total30Demand = 0;
        //    }
        //    else
        //    {
        //        total30Demand = warehouseDemandData.TotalLast30Demand;
        //    }

        //    var listInvStockStatus = new List<InventoryStockStatus>();
        //    var hasPrimary = false;
        //    var invVIPStockStatus = new List<InventoryStockStatus>();
        //    foreach (var warehouse in response.Warehouses)
        //    {
        //        int index = -1;

        //        var whsCode = warehouse.WHSCode.ToUpper().Trim();

        //        var isVipWhs = InventoryHelper.IsVIPWarehouse(whsCode);

        //        var whsDescription = warehouse.WHSDescription.ToUpper().Trim();
        //        if (string.IsNullOrEmpty(whsDescription))
        //        {
        //            whsDescription = whsCode;
        //        }

        //        //var demand = warehouseDemandData != null && warehouseDemandData.ContainsKey(whsCode)
        //        //    ? warehouseDemandData[whsCode]
        //        //    : 0;

        //        var demand = 0;
        //        if (warehouseDemandData == null)
        //        {
        //        }
        //        else
        //        {
        //            var wh = warehouseDemandData.Warehouses.FirstOrDefault(r => r.WarehouseId == whsCode);
        //            demand = wh == null ? 0 : wh.Last30DayDemand;
        //        }

        //        if (string.Compare(whsCode, primaryWareHouse, StringComparison.OrdinalIgnoreCase) == 0)
        //        {
        //            whsDescription += " *";
        //            whsCode += " *";
        //            index = 0;
        //        }
        //        else if (string.Compare(whsCode, secondaryWareHouse, StringComparison.OrdinalIgnoreCase) == 0)
        //        {
        //            whsDescription += " **";
        //            whsCode += " **";
        //            index = 1;
        //            hasPrimary = true;
        //        }
        //        else if (isVipWhs)
        //        {
        //            whsDescription += " ***";
        //            whsCode += " ***";
        //        }

        //        var invStockStatus = new InventoryStockStatus
        //        {
        //            WareHouse = whsDescription,
        //            WareHouseCode = whsCode,
        //            OnHandInventory = warehouse.QTYOnHand.ToString(),
        //            OnOrderQuantity = warehouse.QTYOnOrder,
        //            InvDemandNumber = demand,
        //        };
        //        if (index == 1)
        //        {
        //            if (listInvStockStatus.Count == 0)
        //            {
        //                listInvStockStatus.Add(invStockStatus);
        //            }
        //            else
        //            {
        //                listInvStockStatus.Insert(0, invStockStatus);
        //            }
        //        }
        //        else if (index == 0)
        //        {
        //            if (hasPrimary)
        //            {
        //                listInvStockStatus.Insert(1, invStockStatus);
        //            }
        //            else
        //            {
        //                if (listInvStockStatus.Count == 0)
        //                {
        //                    listInvStockStatus.Add(invStockStatus);
        //                }
        //                else
        //                {
        //                    listInvStockStatus.Insert(0, invStockStatus);
        //                }
        //            }
        //        }
        //        else //VIP inventory
        //        {
        //            if (isVipWhs)
        //                invVIPStockStatus.Add(invStockStatus);
        //            else
        //                listInvStockStatus.Add(invStockStatus);
        //        }

        //    }
        //    if (invVIPStockStatus.Count > 0)
        //        listInvStockStatus.InsertRange(0, invVIPStockStatus);
        //    return listInvStockStatus;
        //}

        //private BTKeyInventoryResult GetWarehouseDemandData(string btKey, string userId, MarketType? marketType, string[] ESuppliers, bool simonSchusterEnabled, string countryCode)
        //{
        //    //var productItem = GetProductDetailsFromCache(btKey);
        //    var productItem = GetProductDetailsFromCache(btKey, userId, marketType, ESuppliers, simonSchusterEnabled, countryCode);
        //    if (productItem != null && productItem.SearchResultInfo != null)
        //    {
        //        var warehouseData =
        //            InventoryHelper4MongoDb.GetInstance()
        //                .GetInventoryWarehouseData(new[] { GetSearchResultInventoryStatusArg(productItem.SearchResultInfo, marketType.Value.ToString()) });
        //        if (warehouseData != null && warehouseData.ContainsKey(btKey))
        //        {
        //            return warehouseData[btKey];
        //        }
        //    }
        //    return null;
        //}

        private bool GetDisplayInventoryForAllWareHouse(Account account, string orgId)
        {
            bool displayInventoryForAllWareHouse = false;

            //var profileControllerForAdmin = AdministrationProfileController.Current;
            //profileControllerForAdmin.OrganizationPropertiesToReturn.Add(Organization.PropertyName.AllWarehouse);
            //var organization = profileControllerForAdmin.GetOrganization(SiteContext.Current.OrganizationId);
            var organization = ProfileService.Instance.GetOrganizationById(orgId);

            if ((account == null) || (organization.AllWarehouse.HasValue && organization.AllWarehouse.Value))
            {
                displayInventoryForAllWareHouse = true;
            }
            return displayInventoryForAllWareHouse;
        }

        private Account GetDefaultAccountFromOrg(string organizationId, string productType, out string DefaultVIPAccountId, out bool isHomeDeliveryAccount)
        {
            DefaultVIPAccountId = "";
            isHomeDeliveryAccount = false;
            if (string.IsNullOrEmpty(organizationId)) return null;

            //AdministrationProfileController.Current.OrganizationRelated.DefaultBookAccountNeeded = true;
            //AdministrationProfileController.Current.OrganizationRelated.DefaultEntertainmentAccountNeeded = true;
            //var orgObject = AdministrationProfileController.Current.GetOrganization(organizationId);
            var orgObject = ProfileService.Instance.GetOrganizationById(organizationId);

            if (orgObject != null)
            {
                if (string.Compare(productType, ProductType.Book.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                {
                    DefaultVIPAccountId = orgObject.DefaultVIPAccountId;

                    //if (orgObject.DefaultEntertainmentAccount != null)
                    if (string.IsNullOrEmpty(orgObject.DefaultEntAccountId) == false)
                    {
                        //var account = (Account)orgObject.DefaultEntertainmentAccount.Target;                        
                        //if (account != null && account.HomeDeliveryAccount.HasValue && account.HomeDeliveryAccount.Value)
                        //{
                        //    isHomeDeliveryAccount = true;
                        //    return null;
                        //}

                        var account = ProfileController.Instance.GetAccountById(orgObject.DefaultEntAccountId);
                        if (account != null && account.HomeDeliveryAccount.HasValue && account.HomeDeliveryAccount.Value)
                        {
                            isHomeDeliveryAccount = true;
                            return null;
                        }
                    }

                    //return orgObject.DefaultBookAccount == null ? null : (Account)(orgObject.DefaultBookAccount.Target);
                    return string.IsNullOrEmpty(orgObject.DefaultBookAccountId) ? null
                        : ProfileController.Instance.GetAccountById(orgObject.DefaultBookAccountId);
                }
                //return orgObject.DefaultEntertainmentAccount == null ? null : (Account)(orgObject.DefaultEntertainmentAccount.Target);
                return string.IsNullOrEmpty(orgObject.DefaultEntAccountId) ? null
                        : ProfileController.Instance.GetAccountById(orgObject.DefaultEntAccountId);
            }

            return null;
        }

        //private ProductSearchResultItem GetProductDetailsFromCache(string btKey, string userId, BT.TS360Constants.MarketType? marketType, string[] ESuppliers, bool simonSchusterEnabled, string countryCode)
        private ProductSearchResultItem GetProductDetailsFromFast(string btKey)
        {
            var searchResults = ProductSearchController.SearchByIdWithoutAnyRules(new List<string> { btKey });

            if (searchResults != null && searchResults.Items != null &&
                searchResults.Items.Count > 0)
            {
                return searchResults.Items[0];
            }

            return null;
        }

        public WhatHotLandingPage GetWhatHotData(TargetingValues siteContext)
        {
            var wh = new WhatHotLandingPage();

            string text = string.Empty;
            var header = ContentManagementController.Current.GetHeaderTitlesItems();
            if (header != null)
                text = header.WhatsHot;
            if (string.IsNullOrEmpty(text))
                text = CommonResources.WhatsHot;

            wh.HeaderText = text;
            wh.LearnMore = CommonResources.LearnMore;

            try
            {
                var requestDomain = AppSetting.Ts360SiteUrl;
                var whatsHotItems = ContentManagementController.Current.GetWhatsHotItems(siteContext, requestDomain);
                if (whatsHotItems == null || whatsHotItems.Count <= 0)
                { }
                else
                {
                    var promotionItem = whatsHotItems.Where(i => i.PromotionFolder == PromotionFolder.Promotion1).FirstOrDefault() ??
                                whatsHotItems[0];
                    //
                    wh.LitTitle1 = promotionItem.PromotionTitle;
                    wh.ImgWhatHot1 = promotionItem.WhatSHotImage;
                    wh.LitProductDescription1 = promotionItem.PromotionText;

                    //var lnkWhatHot1 = CombineQuerySearch(promotionItem);
                    var lnkWhatHot1 = CombineQuerySearch(promotionItem, requestDomain);
                    wh.LnkWhatHot1 = SearchHelper.CreateUrlWebtrendsQueryString(lnkWhatHot1, promotionItem.ImageWebtrendsTag);
                    wh.LnkLearnMore1 = SearchHelper.CreateUrlWebtrendsQueryString(lnkWhatHot1, promotionItem.ButtonWebtrendsTag);
                    //
                    if (whatsHotItems.Count > 1)
                    {
                        var item = promotionItem;
                        var promotionItem2 = whatsHotItems.Where(f => f.PromotionFolder != item.PromotionFolder).FirstOrDefault();
                        promotionItem = promotionItem2 ?? whatsHotItems[1];
                        wh.LitTitle2 = promotionItem.PromotionTitle;
                        wh.ImgWhatHot2 = promotionItem.WhatSHotImage;
                        wh.LitProductDescription2 = promotionItem.PromotionText;

                        //var lnkWhatHot2 = CombineQuerySearch(promotionItem);
                        var lnkWhatHot2 = CombineQuerySearch(promotionItem, requestDomain);
                        wh.LnkWhatHot2 = SearchHelper.CreateUrlWebtrendsQueryString(lnkWhatHot2, promotionItem.ImageWebtrendsTag);
                        wh.LnkLearnMore2 = SearchHelper.CreateUrlWebtrendsQueryString(lnkWhatHot2, promotionItem.ButtonWebtrendsTag);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.General);
            }

            ProxySessionHelper.AppendProxyUserId(wh.LnkWhatHot1);
            ProxySessionHelper.AppendProxyUserId(wh.LnkLearnMore1);
            ProxySessionHelper.AppendProxyUserId(wh.LnkWhatHot2);
            ProxySessionHelper.AppendProxyUserId(wh.LnkLearnMore2);

            return wh;
        }

        //private string CombineQuerySearch(WhatsHotItem item)
        private string CombineQuerySearch(WhatsHotItem item, string requestDomain)
        {
            var result = string.Empty;
            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.PromotionCode))
                {
                    var adname = item.AdName;
                    if (!string.IsNullOrEmpty(adname))
                    {
                        //var promotionItem = this.PromotionItems.Where(f => f.AdName == adname && f.PromotionCode == item.PromotionCode).FirstOrDefault();
                        var promotionItem = ContentManagementController.Current.GetApprovedOrPublishedPromotionItems(requestDomain).Where(f => f.AdName == adname && f.PromotionCode == item.PromotionCode).FirstOrDefault();
                        if (promotionItem != null)
                            return SearchHelper.CreateUrlPromotionProductByPromotionId(promotionItem.Id.ToString());//(promotionItem.PromotionCode, promotionItem.AdName); 
                        else
                            return SearchHelper.CreateUrlPromoCode(item.PromotionCode);
                    }
                    else
                        return SearchHelper.CreateUrlPromoCode(item.PromotionCode);

                }
                else
                {
                    //return SearchHelper.CreateUrlBTKeys(item.BTKEY);
                    return SearchHelper.CreateUrlForWhatsHot(item.Id, item.BTKEY);
                }
            }
            return result;
        }

        public PopularFeedLandingPage GetPopularFeedData(TargetingValues siteContext)
        {
            var pf = new PopularFeedLandingPage();

            string text = string.Empty;
            var header = ContentManagementController.Current.GetHeaderTitlesItems();
            if (header != null)
                text = header.PopularBTeLists;
            if (string.IsNullOrEmpty(text))
                text = CommonResources.HotBTELists;

            pf.HeaderTextEList = text;
            pf.MoreELists = CommonResources.MoreELists;

            pf.HeaderTextNewsFeeds = CommonResources.NewsFeeds;
            pf.MoreNewsFeeds = CommonResources.MoreNewsFeeds;

            //targetingText = CommonHelper.Instance.GenerateTargetingQueryString(siteContext);
            pf.TargetingText = MarketingHelper.Instance.GenerateTargetingQueryString(siteContext);

            try
            {
                var eLists = ContentManagementController.Current.GetEListForLandingPage(siteContext);

                if (eLists == null)
                { }
                else
                {
                    pf.EList = eLists.Select(r => new TS_ShortEListItem()
                    {
                        Title = r.Title,
                        PostBackUrl = ProxySessionHelper.AppendProxyUserId(r.ItemCurrenceURL)
                    }).ToList();
                }

                const int rowLimit = 3;
                var newFeedList = ContentManagementController.Current.GetAllNewFeedItems(rowLimit);
                if (newFeedList == null)
                { }
                else
                {
                    pf.NewsFeedsList = newFeedList.Select(r => r.NewsFeed).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.General);
            }

            return pf;
        }

        public async Task<AppServiceResult<AddToCartStatusObject>> QuickSearchAddProductsToCart(QuickSearchAddProductsToCart request)
        {
            var ajaxResult = new AppServiceResult<AddToCartStatusObject>();
            string PermissionViolationMessage = "";
            //
            if (request.addToNewCartObjects == null || request.addToNewCartObjects.Count <= 0)
            {
                return new AppServiceResult<AddToCartStatusObject>
                {
                    Status = AppServiceStatus.Success,
                    Data = new AddToCartStatusObject(),
                    ErrorMessage = string.Empty
                };
            }

            try
            {
                var listBasketInfo = new List<CartInfo>();
                var lineItems = new List<LineItem>();
                foreach (var product in request.addToNewCartObjects)
                {
                    var lineItem = new LineItem
                    {
                        BTKey = product.BTKey,
                        ISBN = product.ISBN,
                        Upc = product.UPC,
                        Quantity = null,
                        PONumber = string.Empty
                    };
                    lineItems.Add(lineItem);
                }

                // add to cart. If cart GridDistributionOption is 3 or 4, totalAddingQtyForGridDistribution has value
                int totalAddingQtyForGridDistribution;
                var returnedCartId = QuickSearchDAOManager.Instance.AddProductsToCart(request.UserId, request.cartName,
                                                                                   request.desFolderId, request.cartId,
                                                                                   lineItems, out PermissionViolationMessage, out totalAddingQtyForGridDistribution);
                if (!string.IsNullOrEmpty(PermissionViolationMessage) && PermissionViolationMessage != "")
                {
                    ajaxResult.Status = AppServiceStatus.Fail;
                    ajaxResult.ErrorMessage = PermissionViolationMessage;
                    return ajaxResult;
                }

                var isQuickCartDetailsEnabled = CommonHelper.Instance.GetQuickViewModeInfo(request.UserId, "IsQuickCartDetailsEnabled");

                var cartInfor = new CartInfo
                {
                    Name = request.cartName,
                    ID = returnedCartId,
                    URL = isQuickCartDetailsEnabled
                        ? SiteUrl.QuickCartDetailsPage
                        : SiteUrl.CartDetailsUrl
                };
                listBasketInfo.Add(cartInfor);

                var userId = request.UserId;

                var cartManager = new CartManager(userId);
                var cart = await cartManager.GetCartByName(request.cartName);
                int lineCountSuccess = request.addToNewCartObjects.Count;

                // get added qty basing on GridDistributionOption
                var addingQty = GetAddingQty(cart.GridDistributionOption, totalAddingQtyForGridDistribution, lineCountSuccess, request.DefaultQuantity);

                // response result
                ajaxResult.Status = AppServiceStatus.Success;
                var addCartStatusObject = new AddToCartStatusObject
                {
                    IsPrimary = false,
                    LineCountSuccess = lineCountSuccess,
                    ItemCountSuccess = addingQty,
                    CartInfo = listBasketInfo
                };

                ajaxResult.Data = addCartStatusObject;

                cartManager.SetCartChanged(request.cartId);
                CartFrameworkHelper.CalculateCartPriceInBackground(request.cartId, request.Targeting);
            }
            catch (CartManagerException cartManagerException)
            {
                ajaxResult.ErrorMessage = cartManagerException.Message == CartManagerException.CART_DUPLICATE_NAME ?
                    OrderResources.CartFolder_CreateCartWithNotUniqueName : CommonErrorMessage.UnexpectedErrorMessage;

                ajaxResult.Status = AppServiceStatus.Fail;
            }
            catch (Exception exception)
            {
                Logger.RaiseException(exception, ExceptionCategory.Search);
                ajaxResult.Status = AppServiceStatus.Fail;
                ajaxResult.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
            }

            return ajaxResult;
        }
    }
}
