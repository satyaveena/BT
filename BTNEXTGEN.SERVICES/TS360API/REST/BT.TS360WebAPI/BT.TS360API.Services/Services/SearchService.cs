using BT.FSIS;
using BT.TS360API.Cache;
using BT.TS360API.Common;
using BT.TS360API.Common.Business;
using BT.TS360API.Common.CartFramework;
using BT.TS360API.Common.Constants;
using BT.TS360API.Common.Controller;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Grid;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Inventory;
using BT.TS360API.Common.Models;
using BT.TS360API.Common.Pricing;
using BT.TS360API.Common.Search;
using BT.TS360API.Common.Search.Helpers;
using BT.TS360API.ExternalServices;
using BT.TS360API.Logging;
using BT.TS360API.Marketing;
using BT.TS360API.Services.Common.Configuration;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Axis360;
using BT.TS360API.ServiceContracts.Inventory;
using BT.TS360API.ServiceContracts.Pricing;
using BT.TS360API.ServiceContracts.Product;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using BT.TS360SP;
using Microsoft.Security.Application;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using TS360Constants;
using BTStockCheck = BT.TS360API.ExternalServices.BTStockCheckServices;
using BTStockCheckServices = BT.TS360API.ExternalServices.BTStockCheckServices;
using BTStockCheckSvc = BT.TS360API.ExternalServices.BTStockCheckServices;
using BTStockServiceLineItem = BT.TS360API.ExternalServices.BTStockCheckServices.LineItem;
using MarketType = BT.TS360Constants.MarketType;
using ProfileDAOManager = BT.TS360API.Common.Business.ProfileDAOManager;
//using ProductsDAOManager = BT.TS360API.MongoDB.DataAccess.ProductsDAOManager;
using BT.TS360API.MongoDB.DataAccess;

namespace BT.TS360API.Services.Services
{
    public partial class SearchService
    {

        #region Move WCF to Api

        public static InventoryResults FilterVIPWarehousesInventory(InventoryResults inventoryResults, MarketType marketType, string orgId, string countryCode)
        {
            InventoryResults vipInventoryResults = new InventoryResults();
            List<InventoryStockStatus> listVIPInventoryStockStatus = new List<InventoryStockStatus>();

            foreach (InventoryStockStatus inventoryStockStatus in inventoryResults.InventoryStock)
            {
                //if (IsDisplayVIPWarehouse(marketType) &&
                if (IsDisplayVIPWarehouse(marketType, countryCode, orgId) &&
                    (inventoryStockStatus.FormalWareHouseCode == InventoryWareHouseCode.VIM || inventoryStockStatus.FormalWareHouseCode == InventoryWareHouseCode.VIE))
                {
                    listVIPInventoryStockStatus.Add(inventoryStockStatus);
                }
                //else if (IsDisplaySuperWarehouse(marketType) && inventoryStockStatus.FormalWareHouseCode == InventoryWareHouseCode.SUP)
                else if (IsDisplaySuperWarehouse(marketType, countryCode, orgId) && inventoryStockStatus.FormalWareHouseCode == InventoryWareHouseCode.SUP)
                {
                    listVIPInventoryStockStatus.Add(inventoryStockStatus);
                }
            }

            vipInventoryResults.BTKey = inventoryResults.BTKey;
            vipInventoryResults.InventoryStock = listVIPInventoryStockStatus;
            vipInventoryResults.DisplayInventoryForAllWareHouse = inventoryResults.DisplayInventoryForAllWareHouse;
            vipInventoryResults.IsStockCheckInventory = inventoryResults.IsStockCheckInventory;

            return vipInventoryResults;

        }

        public static List<InventoryResults> FilterVIPWarehousesInventory(List<InventoryResults> inventoryResults, MarketType marketType, string orgId, string countryCode)
        {
            List<InventoryResults> vipInventoryResults = new List<InventoryResults>();

            foreach (InventoryResults inventoryResult in inventoryResults)
            {
                InventoryResults vipInventoryResult = FilterVIPWarehousesInventory(inventoryResult, marketType, orgId, countryCode);
                if (vipInventoryResult.InventoryStock.Count > 0)
                    vipInventoryResults.Add(vipInventoryResult);
            }

            return vipInventoryResults;
        }

        public static InventoryResults FilterVIPWarehousesInventory(InventoryResults inventoryResults, MarketType marketType)
        {
            InventoryResults vipInventoryResults = new InventoryResults();
            List<InventoryStockStatus> listVIPInventoryStockStatus = new List<InventoryStockStatus>();

            foreach (InventoryStockStatus inventoryStockStatus in inventoryResults.InventoryStock)
            {
                //if (IsDisplayVIPWarehouse(marketType) &&
                //    (inventoryStockStatus.FormalWareHouseCode == InventoryWareHouseCode.VIM || inventoryStockStatus.FormalWareHouseCode == InventoryWareHouseCode.VIE))
                //{
                //    listVIPInventoryStockStatus.Add(inventoryStockStatus);
                //}
                //else if (IsDisplaySuperWarehouse(marketType) && inventoryStockStatus.FormalWareHouseCode == InventoryWareHouseCode.SUP)
                //{
                //    listVIPInventoryStockStatus.Add(inventoryStockStatus);
                //}
            }

            vipInventoryResults.BTKey = inventoryResults.BTKey;
            vipInventoryResults.InventoryStock = listVIPInventoryStockStatus;
            vipInventoryResults.DisplayInventoryForAllWareHouse = inventoryResults.DisplayInventoryForAllWareHouse;
            vipInventoryResults.IsStockCheckInventory = inventoryResults.IsStockCheckInventory;

            return vipInventoryResults;

        }

        public static bool IsDisplayVIPWarehouse(MarketType marketType, string siteContextCountryCode, string siteContextOrgId)
        {
            return (marketType == MarketType.Retail && IsUSCountry(siteContextCountryCode) && IsVIPEnabled(siteContextOrgId));
        }

        public static bool IsDisplaySuperWarehouse(MarketType marketType, string siteContextCountryCode, string siteContextOrgId)
        {
            if (IsVIPEnabled(siteContextOrgId))
            {

                return
                    (marketType == MarketType.AcademicLibrary ||
                        marketType == MarketType.PublicLibrary ||
                        marketType == MarketType.SchoolLibrary ||
                        (marketType == MarketType.Retail && !IsUSCountry(siteContextCountryCode))
                );
            }

            return false;
        }

        private static bool IsUSCountry(string countryCode)
        {
            bool isUS = false;

            if (!string.IsNullOrEmpty(countryCode))
            {
                isUS = (countryCode == "US" || countryCode == "USA");
            }
            return isUS;
        }

        public static bool IsVIPEnabled(string siteContextOrgId)
        {
            bool retVal = false;
            if (!string.IsNullOrEmpty(siteContextOrgId))
            {
                var orgPremiumServicesStatus = OrganizationDAO.Instance.GetOrganizationPremiumServices(siteContextOrgId);

                if (orgPremiumServicesStatus != null && orgPremiumServicesStatus.vipEnabled)
                {
                    retVal = true;
                }

            }
            return retVal;
        }

        //private void GetStockCheckInventoryStatus2(List<InventoryStockArg> btEKeyInventoryList, string userId,
        //                                          CheckRealTimeInventoryForQuickCartDetailsInfoReponse inventoryRealTime, List<InventoryStockArg> btKeyInventoryList,
        //                                          Cart cart, List<SearchResultInventoryStatusArg> args,
        //                                          MarketType marketType, string scCountryCode, string scOrgId)
        //{
        //    List<AccountSummary> accountSummary = CartDAOManager.GetAccountsSummary(cart.CartId);
        //    List<CartAccount> cartAccounts = DataConverter.ConvertListAccountSummaryToListCartAccount(accountSummary);

        //    List<InventoryResults> lstInventoryResult;

        //    int totalSOPLineErrors = 0;
        //    int totalTOTLASLineErrors = 0;

        //    Hashtable htSOPErrorMessage = new Hashtable();
        //    Hashtable htTOTLASErrorMessage = new Hashtable();
        //    string erpErrorMessage = string.Empty;

        //    var bookSearchArg = new SearchResultInventoryStatusArg { ProductType = ProductTypeConstants.Book };
        //    var entSearchArg = new SearchResultInventoryStatusArg { ProductType = ProductTypeConstants.Music };

        //    var defaultBookAccount = InventoryHelper.GetUserDefaultAccountFromCartDetail(bookSearchArg, userId, cart.CartId);
        //    var defaultEntAccount = InventoryHelper.GetUserDefaultAccountFromCartDetail(entSearchArg, userId, cart.CartId);

        //    var isHomeDeliveryCart = CartFrameworkHelper.IsHomeDeliveryCart(cart.CartAccounts);
        //    var isVIPCart = false;
        //    if (cartAccounts != null && cartAccounts.Count > 1)
        //    {
        //        isVIPCart = cartAccounts.Any(cartAccount => cartAccount.AccountType == (int)AccountType.VIP);

        //        // OneBox account overrides Book & Ent accounts (TFS 22510)
        //        var isOneBoxCart = cartAccounts.Any(acc => acc.AccountType == (int)AccountType.OneBox && !string.IsNullOrEmpty(acc.AccountID));
        //        if (isOneBoxCart)
        //        {
        //            // remove book & ent accounts
        //            cartAccounts = cartAccounts.Where(acc => acc.AccountType != (int)AccountType.Book && acc.AccountType != (int)AccountType.Entertainment)
        //                                        .ToList();
        //        }
        //    }

        //    // book inventory
        //    if (btKeyInventoryList.Count > 0)
        //    {
        //        lstInventoryResult = GetRealTimeInventoryResultForCart(userId, btKeyInventoryList, "BTB",
        //                                                               isHomeDeliveryCart, cartAccounts,
        //                                                               defaultBookAccount, isVIPCart, out totalSOPLineErrors,
        //                                                               out htSOPErrorMessage, args, cart.CartId,
        //                                                               marketType, scCountryCode, scOrgId);
        //        inventoryRealTime.InventoryResultsList.AddRange(lstInventoryResult);
        //    }

        //    // ent inventory
        //    if (btEKeyInventoryList.Count > 0)
        //    {
                
        //        lstInventoryResult = GetRealTimeInventoryResultForCart(userId, btEKeyInventoryList, "BTE",
        //                                                               isHomeDeliveryCart, cartAccounts,
        //                                                               defaultEntAccount, isVIPCart, out totalTOTLASLineErrors,
        //                                                               out htTOTLASErrorMessage, args, cart.CartId,
        //                                                               marketType, scCountryCode, scOrgId);
        //        inventoryRealTime.InventoryResultsList.AddRange(lstInventoryResult);
        //    }

        //    // Fix TFS19876 by assigning ProductType to each result item
        //    foreach (var resultItem in inventoryRealTime.InventoryResultsList)
        //    {
        //        var argFound = args.FirstOrDefault(r => r.BTKey == resultItem.BTKey);
        //        if (argFound != null)
        //            resultItem.ProductType = argFound.ProductType;
        //    }


        //    foreach (string code in htSOPErrorMessage.Keys)
        //    {
        //        erpErrorMessage += string.Format("{0}:{1}, ", code, htSOPErrorMessage[code]);
        //    }

        //    foreach (string code in htTOTLASErrorMessage.Keys)
        //    {
        //        if (!htSOPErrorMessage.ContainsKey(code))
        //            erpErrorMessage += string.Format("{0}:{1}, ", code, htTOTLASErrorMessage[code]);
        //    }

        //    if (!string.IsNullOrEmpty(erpErrorMessage) && erpErrorMessage.LastIndexOf(',') > 0)
        //        erpErrorMessage = erpErrorMessage.Substring(0, erpErrorMessage.LastIndexOf(','));

        //    inventoryRealTime.StockCheckInventoryStatus = string.Format("{0}|{1}|{2}|{3}", true,
        //                                                                      btKeyInventoryList.Count +
        //                                                                      btEKeyInventoryList.Count,
        //                                                                      totalSOPLineErrors + totalTOTLASLineErrors,
        //                                                                      erpErrorMessage);
        //}

        private List<InventoryResults> GetInventoryResultsForCart(string cartId, List<SearchResultInventoryStatusArg> args,MarketType marketType, string countryCode, string orgId)
        {
            //var cacheKey = string.Format("INVENTORY_RESULTS_FOR_CART_{0}", cartId);
            //var data = VelocityCacheManager.Read(cacheKey) as List<InventoryResults>;
            //if (data != null) return data;
            //var dbInventoryResults = InventoryHelper4MongoDb.GetInstance(cartId).GetInventoryResultsForMultipleItems(args);

            var mongoDbInstance = InventoryHelper4MongoDb.GetInstance(cartId, marketType: marketType, countryCode: countryCode, orgId: orgId);
            var dbInventoryResults = mongoDbInstance.GetInventoryResultsForMultipleItems(args);

            //VelocityCacheManager.Write(cacheKey, dbInventoryResults, VelocityCacheLevel.Request);
            return dbInventoryResults;
        }

        private Dictionary<string, BTKeyInventoryResult> GetInventoryWarehouseDemand(string cartId, List<SearchResultInventoryStatusArg> args)
        {
            //var cacheKey = string.Format("INVENTORY_WAREHOUSE_DATA_FOR_CART_{0}", cartId);
            //var data = VelocityCacheManager.Read(cacheKey) as Dictionary<string, BTKeyInventoryResult>;
            //if (data != null) return data;
            var warehouseData = InventoryHelper4MongoDb.GetInstance(cartId).GetInventoryWarehouseData(args);

            //VelocityCacheManager.Write(cacheKey, warehouseData, VelocityCacheLevel.Request);
            return warehouseData;
        }

        private void AddVIPToRealTimeInventory(ref BTStockCheckSvc.CartStockCheckResponse response, MarketType marketType, List<InventoryResults> inventoryResults, string orgId, string countryCode)
        {
            List<InventoryResults> vipInventoryResults = InventoryHelper.FilterVIPWarehousesInventory(inventoryResults, marketType, orgId, countryCode);

            foreach (InventoryResults inventoryResult in vipInventoryResults)
            {
                var responseLineItem = response.LineItems.FirstOrDefault(x => x.ItemID == inventoryResult.BTKey);
                if (responseLineItem == null)
                    continue;

                var currentData = responseLineItem.Warehousehoses.ToList();

                List<InventoryStockStatus> lstInventoryStockStatus = inventoryResult.InventoryStock;

                foreach (InventoryStockStatus inventoryStockStatus in lstInventoryStockStatus)
                {
                    var whs = new BTStockCheckSvc.WHSOHOnly();
                    whs.QTYOnHand = Convert.ToInt32(inventoryStockStatus.OnHandInventory);
                    whs.WHSCode = inventoryStockStatus.FormalWareHouseCode;
                    whs.WHSDescription = inventoryStockStatus.WareHouse;

                    currentData.Add(whs);
                }

                responseLineItem.Warehousehoses = currentData.ToArray();
            }
        }

        #endregion

        //public static SiteContextObject SiteContext;
        private const int PageSize = 25;
        private const string TabString = "&nbsp;&nbsp;&nbsp;&nbsp;";

        #region Home - table tab

        private ProductDetailPopup productDetailPopup = new ProductDetailPopup();
        public Object ProductDetails = null;

        public async Task<AppServiceResult<ProductDetailInfo>> GetItemDetailsTooltipInfo(GetItemDetailsTooltipInfoRequest request)
        {
            var btKey = request.BTKey;
            var userId = request.UserId;
            var marketType = request.MarketType;
            var countryCode = request.CountryCode;
            try
            {
                var result = new AppServiceResult<ProductDetailInfo>();
                if (string.IsNullOrEmpty(btKey))
                {
                    return result;
                }

                bool isReloadData ;

                result.Data = GetProductDetailInfo(btKey, userId, marketType, out isReloadData);

                if (isReloadData)
                {
                    ProductSearchResultItem item = result.Data.SearchResultInfo;

                    var productIds = new List<string> { btKey };

                    DiversityProductsRequest diversityProductsRequest = new DiversityProductsRequest();
                    diversityProductsRequest.BTKeys = productIds;
                    DiversityProductsResponse diversityProductsResponse = await GetDiversityClassificationByBTKeys(diversityProductsRequest);


                    if (diversityProductsResponse.DiversityProducts != null && diversityProductsResponse.DiversityProducts.Count > 0)
                    {
                        item.HasDiversityClassification = true;
                        item.ClassificationName = diversityProductsResponse.DiversityProducts[0].ClassificationName;
                    }
                }

                productDetailPopup.ProductDetail = result.Data;

                result.ImageUrl = ContentCafeHelper.GetJacketImageUrl(productDetailPopup.ProductDetail.SearchResultInfo.ISBN, ImageSize.Medium, productDetailPopup.ProductDetail.SearchResultInfo.HasJacket);
                result.NavigateUrl = SiteUrl.ItemDetailsAbsolutePath + "?" + SearchFieldNameConstants.btkey + "=" + productDetailPopup.ProductDetail.SearchResultInfo.BTKey + "&isfromsearchresults=1";
                //todo
                switch (productDetailPopup.ProductDetail.SearchResultInfo.ProductType)
                {
                    case ProductTypeConstants.Book:
                        {
                            switch (productDetailPopup.ProductDetail.MarketType)
                            {
                                case MarketType.AcademicLibrary:
                                    result.ProductTypeLink = SearchHelper.CreateSearchResultLink(SearchFieldNameConstants.academicsubjects, productDetailPopup.ProductDetail.SearchResultInfo.AcademicSubjects);
                                    break;
                                case MarketType.PublicLibrary:
                                case MarketType.Retail:
                                case MarketType.SchoolLibrary:
                                case MarketType.Any:
                                    result.ProductTypeLink = "/_layouts/CommerceServer/searchresults.aspx?" + SearchFieldNameConstants.subject1 + "=" + Microsoft.Security.Application.Encoder.UrlEncode(productDetailPopup.ProductDetail.SearchResultInfo.Subject);
                                    break;
                            }
                            break;
                        }
                    case ProductTypeConstants.Movie:
                        {
                            result.ProductTypeLink = SearchHelper.CreateSearchResultLink(SearchFieldNameConstants.moviegenre, productDetailPopup.ProductDetail.SearchResultInfo.Genre);
                            break;
                        }
                    case ProductTypeConstants.Music:
                        {
                            result.ProductTypeLink = SearchHelper.CreateSearchResultLink(SearchFieldNameConstants.musicgenre, productDetailPopup.ProductDetail.SearchResultInfo.Genre);
                            break;
                        }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                return null;
            }
        }
        private ProductDetailInfo GetProductDetailInfo(string btKey, string userId, MarketType? marketType, out bool isReloadData)
        {
            isReloadData = false;
 
            var marketTypeValue = marketType ?? MarketType.Any;
            var scope = string.Empty;

            var result = new ProductDetailInfo(marketTypeValue);

            var productSearchResults = GetProductSearchResultsFromCache(userId);
            //
            if (productSearchResults != null)
            {
                var product = productSearchResults.GetProductSearchResultItem(btKey);
                result.SearchResultInfo = product;
            }

            if (result.SearchResultInfo == null)
            {
                var productIds = new List<string> { btKey };
                var searchResults = ProductSearchController.SearchByIdWithoutAnyRules(productIds);
                if (searchResults != null && searchResults.Items != null && searchResults.Items.Count > 0)
                {
                    result.SearchResultInfo = searchResults.Items[0];
                    isReloadData = true;
                }
                else
                {
                    result.SearchResultInfo = new ProductSearchResultItem();
                }
            }

             //            
            return result;
        }


        #endregion

        #region Get real Time Iventory

        public async Task<AppServiceResult<BTStockCheckServices.StockCheckResponse>> GetRealTimeInventoryInfo(GetItemRealTimenventoryRequest request)
        {
            string itemKey = request.BTKey;
            MarketType marketType = request.MarketType;
            string orgId = request.OrgId;
            string countryCode = request.CountryCode;
            string cartId = request.CartId;
            string userId = request.UserId;
            string eSupplier = request.ESupplier;
            string productType = request.ProductType;
            
            bool getVIPInvt = false;

            try
            {
                var result = new AppServiceResult<BTStockCheckServices.StockCheckResponse>()
                {
                    Status = AppServiceStatus.Success,
                    Data = new BTStockCheckServices.StockCheckResponse()//CreateRealTimeInventoryNoData(SiteContext.GetLocalizedString(ResourceName.CatalogResources, "RealTimeInventory_TimeOutMessage"))
                };
                var accountId = "";
                var searchArg = new SearchResultInventoryStatusArg
                {
                    BTKey = itemKey,
                    ProductType = productType,
                    ESupplier = eSupplier
                };
                //var userId = SiteContext.Current.UserId;
                var account = InventoryHelper.GetUserDefaultAccountFromCartDetail(searchArg, userId, cartId);

                if (account != null)
                {
                    accountId = account.AccountNumber;
                }
                else
                {
                    result.ErrorMessage = "You do not have account setup for this product type.";
                    result.Status = AppServiceStatus.Fail;
                    return result;
                }
                //////////////////////////////////////////////////////////////////
                if (!string.IsNullOrEmpty(accountId))
                {
                    var response = new BTStockCheckServices.StockCheckResponse();
                    string primaryWareHouse, secondaryWareHouse;
                    var sortedList = new List<BTStockCheckServices.WHS>();
                    var remain = new List<BTStockCheckServices.WHS>();
                    var isSuccess = true;
                    ProductSearchController.GetPrimarySecondaryWareHouse(out primaryWareHouse, out secondaryWareHouse, account);

                    try
                    {
                        response = InventoryHelper.GetRealTimeInventory(accountId, itemKey);
                        result.Data = response;
                        result.Status = AppServiceStatus.Success;
                    }
                    catch (Exception ex)
                    {
                        Logger.Write("Real Time Inventory", string.Format("Message: {0}, Stack Trace: {1}", ex.Message, ex.StackTrace));
                        isSuccess = false;
                    }

                    if (!isSuccess)
                    {
                        //TFS#6236: get local inventories in case the RealTimeInventory is unavailable
                        InventoryResults inventoryResults = GetInventoryResultForItemDetails(itemKey, marketType, cartId, orgId, countryCode);
                        ProductSearchController.SortWareHouses(out remain, secondaryWareHouse, inventoryResults, out sortedList, primaryWareHouse);
                        response.StatusMessage = string.Empty;
                    }
                    else
                    {
                        if (getVIPInvt)
                        {
                            if (InventoryHelper.IsDisplaySuperWarehouse(marketType, countryCode, orgId) || InventoryHelper.IsDisplayVIPWarehouse(marketType, countryCode, orgId))
                            {
                                AddVIPToRealTimeInventory(ref response, itemKey, marketType,  orgId, countryCode, cartId);
                            }
                        }
                        //sort, primary first then secondary then remain.
                        ProductSearchController.SortWareHouses(out remain, secondaryWareHouse, response, out sortedList, primaryWareHouse);
                    }

                    // TFS 24744
                    var isAvProduct = CommonHelper.IsAVProduct(productType);
                    if (isAvProduct && primaryWareHouse == InventoryWareHouseCode.Com && string.IsNullOrEmpty(secondaryWareHouse)
                        && response != null && response.Warehouses != null)
                    {
                        var inventoryHelper4MongoDb = InventoryHelper4MongoDb.GetInstance(cartId, userId, orgId: orgId);
                        var displayAllWareHouses = inventoryHelper4MongoDb.IsDisplayAllWarehouse();
                        if (!displayAllWareHouses)
                            //display EAST (SOM) only
                            response.Warehouses = response.Warehouses.Where(r => r.WHSCode == InventoryWareHouseCode.Som).ToArray();
                    }

                    #region LoadControl("_layouts/CommerceServer/RealTimeInventoryPopupUserControl.ascx"
                    //var pageHolder = new Page();
                    //var viewControl = (UserControl)pageHolder.LoadControl("_layouts/CommerceServer/RealTimeInventoryPopupUserControl.ascx");
                    ////
                    //var viewControlType = viewControl.GetType();
                    //var field = viewControlType.GetField("RealTimeWareHouse");

                    //var fieldMessage = viewControlType.GetField("StatusMessage");

                    //if (!isSuccess)
                    //{
                    //    var isRealTimeInventoryOffline = viewControlType.GetField("IsRealTimeInventoryOffline");
                    //    isRealTimeInventoryOffline.SetValue(viewControl, false);
                    //}

                    //Temporary fix for time out
                    //response.StatusMessage = RefineInventoryErrorMessage(response.StatusMessage);
                    #endregion

                    var organizationId = "";
                    List<BTStockCheckServices.WHS> refinedWhsForVip = RefineWarehouseForVipLogic(sortedList, productType, organizationId); //TFS #18664

                    #region Load with refinedWhsForVip
                    //fieldMessage.SetValue(viewControl, ExtractErrorMessage(response.StatusMessage));
                    ////
                    //field.SetValue(viewControl, refinedWhsForVip);
                    //pageHolder.Controls.Add(viewControl);

                    //var output = new StringWriter();
                    //HttpContext.Current.Server.Execute(pageHolder, output, false);
                    //

                    //var sPage = output.ToString();
                    //var regex = new Regex("(<div id=\"divGet\">.*)<div id=\"divEndGet\">", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    //var match = regex.Match(sPage);
                    //sPage = match.Success ? match.Groups[1].Value : "";
                    ////
                    //result.Data = sPage;
                    #endregion

                }
                else
                {
                    //result = new AjaxServiceResult<string>
                    //{
                    //    Status = AjaxServiceStatus.Success,
                    //    Data = CreateRealTimeInventoryNoData(SiteContext.GetLocalizedString(ResourceName.CatalogResources, "RealTimeInventory_AccountNotFound"))
                    //};
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                return null;
            }
        }
        private List<BTStockCheckServices.WHS> RefineWarehouseForVipLogic(IEnumerable<BTStockCheckServices.WHS> sortedList, string itemProductType, string organizationId)
        {
            var orgPremium = GetOrgPremiumServicesStatus(organizationId);
            bool isVipCustomer = false;
            if (orgPremium != null)
            {
                isVipCustomer = orgPremium.vipEnabled;
            }

            var result = new List<BTStockCheckServices.WHS>();
            foreach (var w in sortedList)
            {
                if (w.WHSCode == BT.TS360Constants.InventoryWareHouseCode.SUP &&
                    (w.QTYOnHand == 0 ||
                     (isVipCustomer && string.Compare(itemProductType, "book", StringComparison.OrdinalIgnoreCase) != 0)))
                {
                    continue;
                }
                result.Add(w);
            }
            return result;
        }
        private OrganizationPremiumServices GetOrgPremiumServicesStatus(string organizationId)
        {
            //var organizationId = SiteContext.Current.OrganizationId;
            //string cacheKey = string.Format(CacheKeyConstant.ESPPremiumService_CACHE_KEY_PREFIX, organizationId);
            //var orgPremiumServicesStatus = VelocityCacheManager.Read(cacheKey) as OrganizationPremiumServices;
            //if (orgPremiumServicesStatus == null)
            //{
            //    orgPremiumServicesStatus = OrganizationDAO.Instance.GetOrganizationPremiumServices(organizationId);
            //    VelocityCacheManager.Write(cacheKey, orgPremiumServicesStatus, VelocityCacheLevel.Session);
            //}

            var orgPremiumServicesStatus = OrganizationDAO.Instance.GetOrganizationPremiumServices(organizationId);

            return orgPremiumServicesStatus;
        }

        //private InventoryResults GetInventoryResultForItemDetails(string btKey, MarketType? marketType, string userId, string[] eSuppliers, string orgId, string countryCode, string cartId = "")
        //{
        //    //var cacheKey = string.Format("INVENTORY_RESULT_ITEM_DETAILS_{0}", btKey);

        //    //var result = VelocityCacheManager.Read(cacheKey) as InventoryResults;
        //    //if (result != null) return result;

        //    var marketTypeValue = marketType ?? MarketType.Any;
        //    var scope = string.Empty;
        //    string[] ESuppliers = eSuppliers;
        //    bool simonSchusterEnabled = false;

        //    var result = new InventoryResults();
        //    var productSearchResults = this.SearchById(new List<string>() { btKey }, SearchFieldNameConstants.btkey, userId, marketTypeValue, ESuppliers, simonSchusterEnabled, countryCode);
        //    if (productSearchResults != null &&
        //        productSearchResults.Items != null &&
        //        productSearchResults.Items.Count > 0)
        //    {

        //        var inventoryArg = new List<SearchResultInventoryStatusArg>()
        //                           {
        //                               GetSearchResultInventoryStatusArg((productSearchResults.Items[0]), marketType.ToString())
        //                           };
        //        var mongoDbInstance = InventoryHelper4MongoDb.GetInstance(cartId, marketType: marketType, countryCode: countryCode, orgId: orgId);
        //        var results = mongoDbInstance.GetInventoryResultsForMultipleItems(inventoryArg);

        //        if (results != null && results.Any())
        //        {
        //            result = results.First();
        //        }
        //    }
        //    //VelocityCacheManager.Write(cacheKey, result, VelocityCacheLevel.Request);
        //    return result;
        //}

        //private void AddVIPToRealTimeInventory(ref BTStockCheckServices.StockCheckResponse response, string btKey, BT.TS360Constants.MarketType marketType, string userId, string[] eSuppliers, string orgId, string countryCode, string cartId = "")
        //{
        //    InventoryResults inventoryResults = GetInventoryResultForItemDetails(btKey, marketType, cartId, orgId, countryCode); //GetInventoryResultForItemDetails(btKey, marketType, cartId);
        //    InventoryResults vipInventoryResults = InventoryHelper.FilterVIPWarehousesInventory(inventoryResults, marketType);

        //    var currentData = response.Warehouses.ToList();
        //    BTStockCheckServices.WHS whs;

        //    foreach (InventoryStockStatus inventoryStockStatus in vipInventoryResults.InventoryStock)
        //    {
        //        whs = new BTStockCheckServices.WHS();
        //        whs.QTYOnHand = Convert.ToInt32(inventoryStockStatus.OnHandInventory);
        //        whs.QTYOnOrder = Convert.ToInt32(inventoryStockStatus.OnOrderQuantity);
        //        whs.WHSCode = inventoryStockStatus.FormalWareHouseCode;
        //        whs.WHSDescription = inventoryStockStatus.WareHouse;
        //        currentData.Insert(0, whs);// .Add(whs);
        //    }

        //    response.Warehouses = currentData.ToArray();
        //}

        #endregion

        #region Cart Detail - GetItemDetailsTooltipContentForCart


        #endregion

        #region CheckRealTimeInventoryForQuickCartDetails
        public async Task<AppServiceResult<AdditionalCartLineItemsResponse>> CheckRealTimeInventoryForQuickCartDetailsInfo(CheckRealTimeInventoryForQuickCartDetailsInfoRequest qcContextReq)
        {
            var returnResult = new AppServiceResult<AdditionalCartLineItemsResponse>();
            var marketType = qcContextReq.MarketType;

            //var carManager = new CartManager();
            try
            {
                //var result = new CheckRealTimeInventoryForQuickCartDetailsInfoReponse();
                var lineItems = new List<QuickLineItem>();
                //lineItems  = QuickCart.ReadCartLinesFromCache(qcContextReq.CartId, qcContextReq.UserId,
                //                                             qcContextReq.PageNumber, qcContextReq.PageSize,
                //                                             qcContextReq.SortBy, qcContextReq.SortDirection);
                //if (lineItems == null)
                //{
                var cartManager = new CartManager(qcContextReq.UserId);
                var quickCart = cartManager.GetCartDetailsQuickView(qcContextReq.CartId, qcContextReq.PageNumber, qcContextReq.PageSize,
                                                              qcContextReq.SortBy,
                                                              qcContextReq.SortDirection, false, true);
                if (quickCart != null)
                {
                    lineItems = quickCart.LineItems;
                }
                //}
                if (lineItems != null)
                {
                    var inventoryStatusArgList = new List<SearchResultInventoryStatusArg>();
                    var btKeyInventoryList = new List<InventoryStockArg>();
                    var btEKeyInventoryList = new List<InventoryStockArg>();
                    foreach (var item in lineItems)
                    {
                        var isa = new InventoryStockArg(item.BTKey, item.UPC, item.Quantity);
                        if (
                            string.Compare(item.ProductType, ProductType.Book.ToString(),
                                           StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            btKeyInventoryList.Add(isa);
                        }
                        else
                        {
                            btEKeyInventoryList.Add(isa);
                        }

                        inventoryStatusArgList.Add(GetSearchResultInventoryStatusArg(item, marketType));
                    }

                    var response = new AdditionalCartLineItemsResponse();
                    GetStockCheckInventoryStatus(btEKeyInventoryList, qcContextReq.UserId, qcContextReq.CartId, response, btKeyInventoryList, inventoryStatusArgList, marketType, scCountryCode: qcContextReq.CountryCode, scOrgId: qcContextReq.OrgId);
                    
                    //this.GetStockCheckInventoryStatus2(btEKeyInventoryList, qcContextReq.UserId, result,
                    //         btKeyInventoryList, new Cart(qcContextReq.CartId, qcContextReq.UserId, ""), inventoryStatusArgList,
                    //         qcContextReq.MarketType, qcContextReq.CountryCode, qcContextReq.OrgId);

                    RefineRealTimeInventory(response.InventoryResultsList);

                    foreach (var invResult in response.InventoryResultsList)
                    {
                        var btkey = invResult.BTKey;
                        foreach (var searchItem in inventoryStatusArgList)
                        {
                            if (btkey == searchItem.BTKey)
                            {
                                invResult.ProductType = searchItem.ProductType;
                                break;
                            }
                        }
                    }

                    returnResult.Data = response;
                }
                returnResult.Status = AppServiceStatus.Success;
                returnResult.ErrorMessage = "";
            }
            catch (Exception exception)
            {
                //ajaxResult.Status = AjaxServiceStatus.Fail;

                //if (exception.Message == "invalidaccount")
                //{
                //    ajaxResult.ErrorMessage = GetLocalizedString("ProfileResources", "StockCheckAccountNotSetUpError");
                //}
                //else
                //{
                //    ajaxResult.ErrorMessage = GetLocalizedString("ProfileResources", "UnexpectedError");
                //    Logger.RaiseException(exception, ExceptionCategory.Order);

                //}
                returnResult.Status = AppServiceStatus.Fail;
                returnResult.ErrorMessage = exception.Message;
            }

            return returnResult;
        }

        private SearchResultInventoryStatusArg GetSearchResultInventoryStatusArg(QuickLineItem productInfo, MarketType? marketTypeValue)
        {
            var btKey = productInfo.BTKey;
            //            
            var catalogName = productInfo.Catalog /*SiteContext.Current.DefaultCatalogName*/;
            var productType = productInfo.ProductType;
            //var flag = productInfo.;
            var pubDate = productInfo.PublishedDate;
            //var merchandise = productInfo.MerchCategory;
            var marketType = (marketTypeValue ?? BT.TS360Constants.MarketType.Any).ToString();
            var pubCode = productInfo.Publisher;
            var eSupplier = productInfo.ESupplier;
            //var reportCode = productInfo.ReportCode;
            var supplierCode = productInfo.SupplierCode;
            //
            var searchArg = new SearchResultInventoryStatusArg
            {
                CatalogName = catalogName,
                //Flag = flag,
                BTKey = btKey,
                Quantity = 0,
                ProductType = productType,
                VariantId = "",
                PublishDate = pubDate.HasValue ? pubDate.Value : default(DateTime),
                //MerchandiseCategory = merchandise,
                MarketType = marketType,
                PubCodeD = pubCode,
                ESupplier = eSupplier,
                //ReportCode = reportCode,
                SupplierCode = supplierCode
            };
            return searchArg;
        }

        public void GetStockCheckInventoryStatus1(List<InventoryStockArg> btEKeyInventoryList, string userId,
                                          CheckRealTimeInventoryForQuickCartDetailsInfoReponse wCfObjectReturnToClient, List<InventoryStockArg> btKeyInventoryList,
                                          Cart cart, List<SearchResultInventoryStatusArg> args)
        {
            var carManager = new CartManager();
            List<AccountSummary> accountSummary = carManager.GetAccountsSummary(cart.CartId);
            List<CartAccount> cartAccounts = DataConverter.ConvertListAccountSummaryToListCartAccount(accountSummary);

            List<InventoryResults> lstInventoryResult;

            int totalSOPLineErrors = 0;
            int totalTOTLASLineErrors = 0;

            Hashtable htSOPErrorMessage = new Hashtable();
            Hashtable htTOTLASErrorMessage = new Hashtable();
            string erpErrorMessage = string.Empty;

            var bookSearchArg = new SearchResultInventoryStatusArg { ProductType = ProductTypeConstants.Book };
            var entSearchArg = new SearchResultInventoryStatusArg { ProductType = ProductTypeConstants.Music };

            var defaultBookAccount = InventoryHelper.GetUserDefaultAccountFromCartDetail(bookSearchArg, userId, cart.CartId);
            var defaultEntAccount = InventoryHelper.GetUserDefaultAccountFromCartDetail(entSearchArg, userId, cart.CartId);

            var isHomeDeliveryCart = carManager.IsHomeDeliveryCart(accountSummary);
            var isVIPCart = false;
            if (cartAccounts != null && cartAccounts.Count > 1)
            {
                isVIPCart = cartAccounts.Any(cartAccount => cartAccount.AccountType == (int)AccountType.VIP);

                // OneBox account overrides Book & Ent accounts (TFS 22510)
                var isOneBoxCart = cartAccounts.Any(acc => acc.AccountType == (int)AccountType.OneBox && !string.IsNullOrEmpty(acc.AccountID));
                if (isOneBoxCart)
                {
                    // remove book & ent accounts
                    cartAccounts = cartAccounts.Where(acc => acc.AccountType != (int)AccountType.Book && acc.AccountType != (int)AccountType.Entertainment)
                                                .ToList();
                }
            }

            // book inventory
            if (btKeyInventoryList.Count > 0)
            {
                //lstInventoryResult = GetRealTimeInventoryResultForCart(userId, btKeyInventoryList, "BTB",
                //                                                       isHomeDeliveryCart, cartAccounts,
                //                                                       defaultBookAccount, isVIPCart, out totalSOPLineErrors,
                //                                                       out htSOPErrorMessage, args, cart.CartId);
                //wCfObjectReturnToClient.InventoryResultsList.AddRange(lstInventoryResult);
            }

            // ent inventory
            if (btEKeyInventoryList.Count > 0)
            {
                //lstInventoryResult = GetRealTimeInventoryResultForCart(userId, btEKeyInventoryList, "BTE",
                //                                                       isHomeDeliveryCart, cartAccounts,
                //                                                       defaultEntAccount, isVIPCart, out totalTOTLASLineErrors,
                //                                                       out htTOTLASErrorMessage, args, cart.CartId);
                //wCfObjectReturnToClient.InventoryResultsList.AddRange(lstInventoryResult);
            }

            // Fix TFS19876 by assigning ProductType to each result item
            foreach (var resultItem in wCfObjectReturnToClient.InventoryResultsList)
            {
                var argFound = args.FirstOrDefault(r => r.BTKey == resultItem.BTKey);
                if (argFound != null)
                    resultItem.ProductType = argFound.ProductType;
            }


            foreach (string code in htSOPErrorMessage.Keys)
            {
                erpErrorMessage += string.Format("{0}:{1}, ", code, htSOPErrorMessage[code]);
            }

            foreach (string code in htTOTLASErrorMessage.Keys)
            {
                if (!htSOPErrorMessage.ContainsKey(code))
                    erpErrorMessage += string.Format("{0}:{1}, ", code, htTOTLASErrorMessage[code]);
            }

            if (!string.IsNullOrEmpty(erpErrorMessage) && erpErrorMessage.LastIndexOf(',') > 0)
                erpErrorMessage = erpErrorMessage.Substring(0, erpErrorMessage.LastIndexOf(','));

            wCfObjectReturnToClient.StockCheckInventoryStatus = string.Format("{0}|{1}|{2}|{3}", true,
                                                                              btKeyInventoryList.Count +
                                                                              btEKeyInventoryList.Count,
                                                                              totalSOPLineErrors + totalTOTLASLineErrors,
                                                                              erpErrorMessage);
        }

        private void RefineRealTimeInventory(IEnumerable<InventoryResults> lstInventoryResult)
        {
            foreach (var item in lstInventoryResult)
            {
                for (var i = 0; i < item.InventoryStock.Count; i++)
                {
                    if (string.IsNullOrEmpty(item.InventoryStock[i].WareHouse) ||
                        (!item.InventoryStock[i].WareHouse.Contains(GeneralConstants.PrimaryMark) &&
                          !item.InventoryStock[i].WareHouse.Contains(GeneralConstants.SecondaryMark)))
                    {
                        item.InventoryStock.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        #endregion


        public async Task<AppServiceResult<AddToCartStatusObject>> AddProductToCartName(AddProductToCartNameRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<AddToCartStatusObject>();
                var isPrimaryCart = false;
                var targetCartId = string.Empty;
                string PermissionViolationMessage = "";
                //
                if (request.addToNewCartObjects == null || request.addToNewCartObjects.Count <= 0)
                {
                    return new AppServiceResult<AddToCartStatusObject>
                    {
                        Status = AppServiceStatus.Success,
                        Data = null,
                        ErrorMessage = string.Empty
                    };
                }
                var cartManager = new CartManager(request.UserId);
                var cart = await cartManager.GetCartByName(request.cartName);

                if (cart != null)
                {
                    targetCartId = cart.CartId;
                    isPrimaryCart = cart.IsPrimary;
                }
                // End Check cart limitation <= 100

                try
                {
                    var defaultQuantity = 0;
                    int.TryParse(request.DefaultQuantity, out defaultQuantity);
                    int lineCountSuccess = 0;
                    int itemCountSuccess = 0;
                    var listBasketInfo = new List<CartInfo>();
                    var cartId = string.Empty;
                    int totalAddingQtyForGridDistribution = 0;
                    var addWithDefaultSetting = true;

                    var isQuickCartDetailsEnabled = CommonHelper.Instance.GetQuickViewModeInfo(request.UserId,
                    "IsQuickCartDetailsEnabled");

                    //Add products to cart
                    foreach (var product in request.addToNewCartObjects)
                    {
                        int quantity;
                        int.TryParse(product.Quantity, out quantity);

                        if (quantity == -5)
                        {
                            quantity = defaultQuantity; // Quick Search Case
                        }
                        else
                        {
                            if (quantity <= 0)
                            {
                                //if (product.Quantity == "")
                                if (string.IsNullOrEmpty(product.Quantity))
                                {
                                    quantity = -1;
                                }
                                else
                                    quantity = 0;
                            }
                        }

                        if (string.IsNullOrEmpty(product.Quantity))
                        { }
                        else if (product.Quantity.Trim() != "")
                        {
                            addWithDefaultSetting = false;
                        }

                        int temp;
                        // Fixing #5080 for shared user
                        var addToCartOutput = await cartManager.AddToCartNameWithSharedUser(product.BTKey, null, quantity, product.Catalog,
                                                                        request.cartName, product.ProductType, product.Note,
                                                                        product.ISBN, product.GTIN, product.UPC,
                                                                        product.POPerline, product.Title, product.Author, product.BIB, request.UserId);

                        cartId = addToCartOutput.CartId;
                        PermissionViolationMessage = addToCartOutput.PermissionViolationMessage;
                        temp = addToCartOutput.totalAddingQtyForGridDistribution;

                        if (!string.IsNullOrEmpty(PermissionViolationMessage) && PermissionViolationMessage != "")
                        {
                            ajaxResult.Status = AppServiceStatus.Fail;
                            ajaxResult.ErrorMessage = PermissionViolationMessage;
                            return ajaxResult;
                        }

                        totalAddingQtyForGridDistribution += temp;

                        if (isPrimaryCart)
                        {
                            //as primary cart is special, quantity is counted by default grid template or enter from UI.
                            quantity = GetQuantitiesByBtkeys(request.UserId, cartId, product.BTKey);
                        }
                        else
                        {
                            if (quantity == -1) //in case of user does not input from UI
                            {
                                if (request.defaultGridQuantity.HasValue && request.defaultGridQuantity.Value >= 0)
                                    quantity = request.defaultGridQuantity.Value;
                                else
                                    quantity = defaultQuantity;
                            }
                        }

                        lineCountSuccess += 1;
                        itemCountSuccess += quantity;
                        var cartinfor = new CartInfo
                        {
                            Name = request.cartName,
                            ID = cartId,
                            URL = isQuickCartDetailsEnabled //siteContext.IsQuickCartDetailsEnabled
                                ? SiteUrl.QuickCartDetailsPage
                                : SiteUrl.CartDetailsUrl
                        };
                        listBasketInfo.Add(cartinfor);
                    }

                    if (isPrimaryCart)
                    {
                        cartManager.SetPrimaryCartChanged();
                        CartFrameworkHelper.CalculatePrice(targetCartId, request.Targeting, false);
                    }
                    else
                    {
                        cartManager.SetCartChanged(targetCartId);
                        CartFrameworkHelper.CalculateCartPriceInBackground(targetCartId, request.Targeting);
                    }

                    var addingQty = GetAddingQty(cart, totalAddingQtyForGridDistribution, itemCountSuccess, addWithDefaultSetting,
                        lineCountSuccess, request.DefaultQuantity);

                    //
                    ajaxResult.Status = AppServiceStatus.Success;
                    var addCartStatusObject = new AddToCartStatusObject
                    {
                        IsPrimary = isPrimaryCart,
                        LineCountSuccess = lineCountSuccess,
                        ItemCountSuccess = addingQty,
                        CartInfo = listBasketInfo
                    };

                    ajaxResult.Data = addCartStatusObject;
                }
                catch (Exception exception)
                {
                    Logger.Write("AddProductToCartName", string.Format("{0}, {1}", exception.Message, exception.StackTrace));
                    ajaxResult.Status = AppServiceStatus.Fail;
                    //ajaxResult.ErrorMessage = GetLocalizedString("ProfileResources", "UnexpectedError");
                    ajaxResult.ErrorMessage = "Unexpected error! Please contact administrator.";
                }

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                return null;
            }
        }

        //public AppServiceResult<string> AddProductToPrimaryCart(AddProductToPrimaryCartRequest request )
        //{
        //    try
        //    {
        //        List<AddToNewCartObject> addToNewCartObjects = request.AddToNewCartObjects;
        //        var ajaxResult = new AppServiceResult<string>();
        //        string PermissionViolationMessage;
        //        if (addToNewCartObjects == null || addToNewCartObjects.Count <= 0)
        //        {
        //            return new AppServiceResult<string>
        //            {
        //                Status = AppServiceStatus.Success,
        //                Data = false.ToString(),
        //                ErrorMessage = string.Empty
        //            };
        //        }
        //        var cartManager = new CartManager(request.UserId);
        //        //Add products to cart
        //        var primaryCart = cartManager.GetPrimaryCart();
        //        if (primaryCart != null)
        //        {
        //            if (!IsValidCartLimitation(addToNewCartObjects, cartManager, primaryCart))
        //            {
        //                return new AppServiceResult<string>
        //                {
        //                    Status = AppServiceStatus.LimitationFail,
        //                    Data = null,
        //                    ErrorMessage = "The maximum number of lines allowed in a cart is 9,999"
        //                };
        //            }

        //            var lineItems = new List<LineItem>();
        //            foreach (var product in addToNewCartObjects)
        //            {
        //                int quantity;
        //                int.TryParse(product.Quantity, out quantity);
        //                if (quantity < 0)
        //                    quantity = 0;

        //                var lineItem = new LineItem
        //                {
        //                    ProductId = product.BTKey,
        //                    BTKey = product.BTKey,
        //                    VariantId = null,
        //                    Quantity = quantity,
        //                    CatalogName = product.Catalog,
        //                    BTItemType = product.ProductType,
        //                    BTLineItemNote = product.Note,
        //                    BTISBN = product.ISBN,
        //                    BTGTIN = product.GTIN,
        //                    BTUPC = product.UPC,
        //                    PONumber = product.POPerline,
        //                    Title = product.Title,
        //                    Author = product.Author
        //                };

        //                lineItems.Add(lineItem);
        //            }
        //            int temp;
        //            cartManager.AddToCartName(primaryCart.CartId, lineItems, out PermissionViolationMessage, out temp);

        //            cartManager.SetPrimaryCartChanged();
        //            //                                    
        //            ajaxResult.Data = primaryCart.CartId;
        //            ajaxResult.Status = AppServiceStatus.Success;
        //        }
        //        else
        //        {
        //            ajaxResult.Status = AppServiceStatus.Fail;
        //            ajaxResult.ErrorMessage = SearchResources.SelectPrimaryCartMessage;
        //        }
        //        return ajaxResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Search);
        //        return null;
        //    }
        //}

        public AppServiceResult<AddToCartStatusObject> AddProductWithGridToPrimaryCart(AddProductWithGridToPrimaryCartRequest request)
        {
            var ajaxResult = new AppServiceResult<AddToCartStatusObject>();
            string PermissionViolationMessage = "";
            if (request.AddToNewCartObjects == null || request.AddToNewCartObjects.Count <= 0)
            {
                return new AppServiceResult<AddToCartStatusObject>
                {
                    Status = AppServiceStatus.Fail,
                    Data = null,
                    ErrorMessage = string.Empty
                };
            }

            try
            {
                //var currentUserId = siteContext.UserId;
                //var cartManager = CartContext.Current.GetCartManagerForUser(currentUserId);
                var cartManager = new CartManager(request.UserId);

                //Add products to cart
                var primaryCart = cartManager.GetPrimaryCart(); // cartMapping.GetPrimaryCart(currentUserId);
                if (primaryCart != null)
                {
                    var listBtKeyQuantity = new Dictionary<string, int>();
                    var listBTKey = new HashSet<string>();
                    var addProducts = new List<ProductLineItem>();
                    var dicCartGridLine = new Dictionary<string, List<CommonCartGridLine>>();

                    int lineCountSuccess = 0;
                    int itemCountSuccess = 0;
                    var addWithDefaultSetting = true;

                    var isUsingZeroQty = primaryCart.GridDistributionOption == (int)GridDistributionOption.UseZeroQty;

                    // TFS #7806 - Remove duplicate gridlines
                    request.GridTitleProperties = GridHelper.RemoveDuplicateDCGridLines(request.GridTitleProperties);
                    foreach (var product in request.AddToNewCartObjects)
                    {
                        if (string.IsNullOrEmpty(product.BTKey.Trim()) || listBTKey.Contains(product.BTKey))
                            continue;
                        listBTKey.Add(product.BTKey);

                        int quantity;
                        if (!int.TryParse(product.Quantity, out quantity) || string.IsNullOrEmpty(product.Quantity))
                        {
                            quantity = -100;
                        }

                        // dancao - 01/06/2016: we allow to send negative quantity since we will convern to DBNULL and the store procs will get default value 
                        // from Grid Distribution set up

                        //// case -100: apply Default Grid Template information in store proc, MUST send NULL to DB - TFS #11620
                        //if (quantity < 0 && quantity != -100)
                        //    quantity = 0;

                        listBtKeyQuantity.Add(product.BTKey, quantity);

                        var gridTitleProperty = GetTitlePropertyByBTKey(product.BTKey, request.GridTitleProperties);

                        // Prepare for Grid
                        // Will merge to one method: Convert DCGridLine to cartGridLines
                        var dcGridLines = GetDCGridLinesFromTitleProperty(product.BTKey, gridTitleProperty);

                        var cartGridLines = new List<CommonCartGridLine>();
                        if (dcGridLines != null)
                        {
                            cartGridLines = GridHelper.AddDCGridLinesToCartGridLines(dcGridLines, string.Empty);

                            dicCartGridLine.Add(product.BTKey, cartGridLines);
                        }

                        if (string.IsNullOrEmpty(product.Quantity))
                        { }
                        else if (product.Quantity.Trim() != "" || (cartGridLines != null && cartGridLines.Count > 0))
                        {
                            addWithDefaultSetting = false;
                        }

                        addProducts.Add(new ProductLineItem()
                        {
                            BTKey = product.BTKey,
                            BibNumber = product.BIB,
                            Note = product.Note,//noteText,
                            PONumber = product.POPerline,
                            Quantity = quantity,
                            Title = product.Title,
                            Author = product.Author
                        });

                        lineCountSuccess += 1;

                        if ((dcGridLines == null || dcGridLines.Count == 0) && quantity == -100 && isUsingZeroQty)
                        {
                            itemCountSuccess += 0;
                        }
                        else
                        {
                            itemCountSuccess += quantity < 0 ? 0 : quantity;
                        }
                    }

                    int totalAddingForGridDistribution;

                    cartManager.AddProductToCart(addProducts, dicCartGridLine, primaryCart.CartId,
                        out PermissionViolationMessage, out totalAddingForGridDistribution);
                    if (!string.IsNullOrEmpty(PermissionViolationMessage) && PermissionViolationMessage != "")
                    {
                        ajaxResult.Status = AppServiceStatus.Fail;
                        ajaxResult.ErrorMessage = PermissionViolationMessage;
                        return ajaxResult;
                    }

                    var isQuickCartDetailsEnabled = CommonHelper.Instance.GetQuickViewModeInfo(request.UserId,
                    "IsQuickCartDetailsEnabled");

                    var listBasketInfo = new List<CartInfo>();
                    //TFS11268
                    var cartinfor = new CartInfo
                    {
                        Name = primaryCart.CartName,
                        ID = primaryCart.CartId,
                        URL = isQuickCartDetailsEnabled //siteContext.IsQuickCartDetailsEnabled
                            ? SiteUrl.QuickCartDetailsPage
                            : SiteUrl.CartDetailsUrl
                    };
                    listBasketInfo.Add(cartinfor);

                    var addingQty = GetAddingQty(primaryCart, totalAddingForGridDistribution, itemCountSuccess, addWithDefaultSetting,
                        lineCountSuccess, request.DefaultQuantity);

                    var addCartStatusObject = new AddToCartStatusObject
                    {
                        IsPrimary = true,
                        LineCountSuccess = lineCountSuccess,
                        ItemCountSuccess = addingQty,
                        CartInfo = listBasketInfo,
                        UserID = request.UserId,
                        OrgID = request.OrgId
                    };

                    ajaxResult.Data = addCartStatusObject;
                    ajaxResult.Status = AppServiceStatus.Success;

                    cartManager.SetPrimaryCartChanged();
                    CartFrameworkHelper.CalculatePrice(primaryCart.CartId, request.Targeting, false);
                }
                else
                {
                    ajaxResult.Status = AppServiceStatus.Fail;
                    ajaxResult.ErrorMessage = SearchResources.SelectPrimaryCartMessage;
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                ajaxResult.Status = AppServiceStatus.Fail;
                ajaxResult.ErrorMessage = ProfileResources.UnexpectedError;
            }

            return ajaxResult;
        }

        public AppServiceResult<AddToCartStatusObject> AddProductWithGridToSelectedCart(AddProductWithGridToSelectedCartRequest request)
        {
            var ajaxResult = new AppServiceResult<AddToCartStatusObject>();
            string PermissionViolationMessage = "";
            if (request.AddToNewCartObjects == null || request.AddToNewCartObjects.Count <= 0)
            {
                return new AppServiceResult<AddToCartStatusObject>
                {
                    Status = AppServiceStatus.Fail,
                    Data = null,
                    ErrorMessage = string.Empty
                };
            }

            try
            {
                //var currentUserId = siteContext.UserId;
                //var cartManager = CartContext.Current.GetCartManagerForUser(currentUserId);
                var cartManager = new CartManager(request.UserId);

                //Add products to cart
                var selectedCart = cartManager.GetCartById(request.CartId); // cartMapping.GetPrimaryCart(currentUserId);
                if (selectedCart != null)
                {
                    var listBtKeyQuantity = new Dictionary<string, int>();
                    var listBTKey = new HashSet<string>();
                    var addProducts = new List<ProductLineItem>();
                    var dicCartGridLine = new Dictionary<string, List<CommonCartGridLine>>();

                    int lineCountSuccess = 0;
                    int itemCountSuccess = 0;
                    var addWithDefaultSetting = true;

                    var isUsingZeroQty = selectedCart.GridDistributionOption == (int)GridDistributionOption.UseZeroQty;

                    // TFS #7806 - Remove duplicate gridlines
                    request.GridTitleProperties = GridHelper.RemoveDuplicateDCGridLines(request.GridTitleProperties);
                    foreach (var product in request.AddToNewCartObjects)
                    {
                        if (string.IsNullOrEmpty(product.BTKey.Trim()) || listBTKey.Contains(product.BTKey))
                            continue;
                        listBTKey.Add(product.BTKey);

                        int quantity;
                        if (!int.TryParse(product.Quantity, out quantity) || string.IsNullOrEmpty(product.Quantity))
                        {
                            quantity = -100;
                        }

                        // dancao - 01/06/2016: we allow to send negative quantity since we will convern to DBNULL and the store procs will get default value 
                        // from Grid Distribution set up

                        //// case -100: apply Default Grid Template information in store proc, MUST send NULL to DB - TFS #11620
                        //if (quantity < 0 && quantity != -100)
                        //    quantity = 0;

                        listBtKeyQuantity.Add(product.BTKey, quantity);

                        var gridTitleProperty = GetTitlePropertyByBTKey(product.BTKey, request.GridTitleProperties);

                        // Prepare for Grid
                        // Will merge to one method: Convert DCGridLine to cartGridLines
                        var dcGridLines = GetDCGridLinesFromTitleProperty(product.BTKey, gridTitleProperty);

                        var cartGridLines = new List<CommonCartGridLine>();
                        if (dcGridLines != null)
                        {
                            cartGridLines = GridHelper.AddDCGridLinesToCartGridLines(dcGridLines, string.Empty);

                            dicCartGridLine.Add(product.BTKey, cartGridLines);
                        }

                        if (string.IsNullOrEmpty(product.Quantity))
                        { }
                        else if (product.Quantity.Trim() != "" || (cartGridLines != null && cartGridLines.Count > 0))
                        {
                            addWithDefaultSetting = false;
                        }

                        addProducts.Add(new ProductLineItem()
                        {
                            BTKey = product.BTKey,
                            BibNumber = product.BIB,
                            Note = product.Note,//noteText,
                            PONumber = product.POPerline,
                            Quantity = quantity,
                            Title = product.Title,
                            Author = product.Author
                        });

                        lineCountSuccess += 1;

                        if ((dcGridLines == null || dcGridLines.Count == 0) && quantity == -100 && isUsingZeroQty)
                        {
                            itemCountSuccess += 0;
                        }
                        else
                        {
                            itemCountSuccess += quantity < 0 ? 0 : quantity;
                        }
                    }

                    int totalAddingForGridDistribution;

                    cartManager.AddProductToCart(addProducts, dicCartGridLine, selectedCart.CartId,
                        out PermissionViolationMessage, out totalAddingForGridDistribution);
                    if (!string.IsNullOrEmpty(PermissionViolationMessage) && PermissionViolationMessage != "")
                    {
                        ajaxResult.Status = AppServiceStatus.Fail;
                        ajaxResult.ErrorMessage = PermissionViolationMessage;
                        return ajaxResult;
                    }

                    var isQuickCartDetailsEnabled = CommonHelper.Instance.GetQuickViewModeInfo(request.UserId,
                    "IsQuickCartDetailsEnabled");

                    var listBasketInfo = new List<CartInfo>();
                    //TFS11268
                    var cartinfor = new CartInfo
                    {
                        Name = selectedCart.CartName,
                        ID = selectedCart.CartId,
                        URL = isQuickCartDetailsEnabled //siteContext.IsQuickCartDetailsEnabled
                            ? SiteUrl.QuickCartDetailsPage
                            : SiteUrl.CartDetailsUrl
                    };
                    listBasketInfo.Add(cartinfor);

                    var addingQty = GetAddingQty(selectedCart, totalAddingForGridDistribution, itemCountSuccess, addWithDefaultSetting,
                        lineCountSuccess, request.DefaultQuantity);

                    var addCartStatusObject = new AddToCartStatusObject
                    {
                        IsPrimary = true,
                        LineCountSuccess = lineCountSuccess,
                        ItemCountSuccess = addingQty,
                        CartInfo = listBasketInfo,
                        UserID = request.UserId,
                        OrgID = request.OrgId
                    };

                    ajaxResult.Data = addCartStatusObject;
                    ajaxResult.Status = AppServiceStatus.Success;

                    cartManager.SetPrimaryCartChanged();
                    CartFrameworkHelper.CalculatePrice(selectedCart.CartId, request.Targeting, false);
                }
                else
                {
                    ajaxResult.Status = AppServiceStatus.Fail;
                    ajaxResult.ErrorMessage = SearchResources.SelectPrimaryCartMessage;
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                ajaxResult.Status = AppServiceStatus.Fail;
                ajaxResult.ErrorMessage = ProfileResources.UnexpectedError;
            }

            return ajaxResult;
        }

        public AddToCartStatusObject AddSimpleProductToCart(SimpleProductToCartRequest request)
        {
            var status = new AddToCartStatusObject();
            string permissionViolationMessage;
            int totalAddingQtyForGridDistribution;

            // convert product to lineitem
            var lineItem = new LineItem
            {
                Id = null,  // Add indicator
                BTKey = request.BTKey,
                Quantity = request.Quantity,
                Title = request.Title,
                Author = request.Author
            };

            var cartManager = new CartManager(request.UserId);
            cartManager.AddLineItems(request.CartId, new List<LineItem>() { lineItem }, out permissionViolationMessage, out totalAddingQtyForGridDistribution, false);

            status.LineCountSuccess = 1;
            status.ItemCountSuccess = GetQuantitiesByBtkeys(request.UserId, request.CartId, request.BTKey);

            return status;
        }

        public AppServiceResult<DemandInfoForItemContract> GetProductDemandDataForItem(PrimaryInfoItemDetailArg arg, int pageIndex, string userId)
        {
            var result = new AppServiceResult<DemandInfoForItemContract>();

            if (arg == null)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                return result;
            }

            try
            {
                var dataReturn = new DemandInfoForItemContract();

                var searchArg = new SearchResultInventoryStatusArg
                {
                    CatalogName = arg.InventoryArg.CatalogName,
                    Flag = arg.InventoryArg.Flag,
                    BTKey = arg.BTKey,
                    Quantity = arg.InventoryArg.Quantity,
                    ProductType = arg.InventoryArg.ProductType,
                    VariantId = arg.InventoryArg.VariantId,
                    PublishDate = arg.InventoryArg.PublishDate,
                    MerchandiseCategory = arg.InventoryArg.MerchandiseCategory,
                    MarketType = arg.InventoryArg.MarketType,
                    PubCodeD = arg.InventoryArg.PubCodeD,
                    ESupplier = arg.InventoryArg.ESupplier,
                    ReportCode = arg.InventoryArg.ReportCode,
                    SupplierCode = arg.InventoryArg.SupplierCode
                };

                var account = string.IsNullOrEmpty(arg.CartId) ? ProductSearchController.GetUserDefaultAccount(searchArg, userId) :
                    InventoryHelper.GetUserDefaultAccountFromCartDetail(searchArg, userId, arg.CartId);

                string priWarehouse, secWarehouse;
                ProductSearchController.GetPrimarySecondaryWareHouse(out priWarehouse, out secWarehouse, account);

                int prePublicationDemandNumber;
                int postPublicationDemandNumber;

                var demandInfos = GetDemandInventoryMongoDb(arg.BTKey, pageIndex, priWarehouse, secWarehouse,
                    out prePublicationDemandNumber, out postPublicationDemandNumber);
                dataReturn.DemandInfos = demandInfos;

                dataReturn.PrePublicationDemandNumber = prePublicationDemandNumber;
                dataReturn.PostPublicationDemandNumber = postPublicationDemandNumber;

                result.Status = AppServiceStatus.Success;
                result.Data = dataReturn;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
            }
            return result;
        }

        public List<ItemDataContract> QuickSearchGetActiveCarts(string userId)
        {
            var result = new List<ItemDataContract>();

            try
            {
                var activeCarts = QuickSearchDAOManager.Instance.GetActivesCart(userId);
                if (activeCarts != null && activeCarts.Count > 0)
                {
                    foreach (var activeCart in activeCarts)
                    {
                        var item = new ItemDataContract { ItemValue = activeCart.CartName, ItemKey = activeCart.CartId };
                        result.Add(item);
                    }
                    //TFS #5477: sort combobox data
                    return result.OrderBy(x => x.ItemValue).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        public List<ItemDataContract> QuickSearchGetFolderList(string userId)
        {
            var result = new List<ItemDataContract>();
            try
            {
                var cartManager = new CartManager(userId);
                var allFolders = cartManager.GetCartFolders();

                var normalFolders = GetNormalFolders(allFolders, userId);
                if (normalFolders != null && normalFolders.Count > 0)
                {
                    foreach (var folder in normalFolders)
                    {
                        var item = new ItemDataContract { ItemValue = folder.CartFolderName, ItemKey = folder.CartFolderId };
                        result.Add(item);
                    }

                    result.Insert(0, new ItemDataContract { ItemValue = "Select Folder", ItemKey = "" });
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }

            return result;
        }

        private List<CartFolder> GetNormalFolders(List<CartFolder> allFolders, string userId)
        {
            var result = new List<CartFolder>();

            if (allFolders != null && allFolders.Count > 0)
            {
                foreach (var folder in allFolders.Where(folder => (folder.FolderType == CartFolderType.NormalFolderType || folder.FolderType == CartFolderType.DefaultFolderType)))
                {
                    result.Add(folder);
                }
            }

            var listRoot = result.Where(o => o.FolderType == CartFolderType.RootFolderType || o.ParentFolderId == null)
                .OrderBy(o => o.FolderType);

            var refinedResult = new List<CartFolder>();
            foreach (var root in listRoot)
            {
                refinedResult.Add(root);
                refinedResult.AddRange(GetCartFolderHiearchy(root, 1, userId, false));
            }

            return refinedResult;
        }

        private IEnumerable<CartFolder> GetCartFolderHiearchy(CartFolder root, int level, string userId, bool includedShared = false)
        {
            var cartManager = new CartManager(userId);
            var folderList = cartManager.GetCartFolderListByParentFolderId(root.CartFolderId, includedShared);
            var result = new List<CartFolder>();
            foreach (var folder in folderList)
            {
                var temp = new CartFolder
                {
                    CartFolderId = folder.CartFolderId,
                    CartFolderName = folder.CartFolderName
                };

                for (int i = 0; i < level; i++)
                {
                    temp.CartFolderName = HttpContext.Current.Server.HtmlDecode(TabString) + temp.CartFolderName;
                }

                result.Add(temp);

                if (root.CartFolderId != folder.CartFolderId && level < 2)
                {
                    result.AddRange(GetCartFolderHiearchy(folder, level + 1, userId));
                }
            }
            return result;
        }

        private List<DemandInventoryContract> GetDemandInventoryMongoDb(string btkey, int pageIndex, string priWarehouse, string secWarehouse,
            out int prePublicationDemand, out int postPublicationDemand)
        {
            var result = new List<DemandInventoryContract>();
            prePublicationDemand = postPublicationDemand = 0;
            var demandHistoryResponse = InventoryHelper4MongoDb.GetInstance()
                .GetDemandHistory(btkey, pageIndex, priWarehouse, secWarehouse);
            if (demandHistoryResponse != null)
            {
                prePublicationDemand = demandHistoryResponse.PrePublicationDemand.HasValue
                    ? demandHistoryResponse.PrePublicationDemand.Value
                    : 0;
                postPublicationDemand = demandHistoryResponse.PostPublicationDemand.HasValue
                    ? demandHistoryResponse.PostPublicationDemand.Value
                    : 0;
                var demandHistoryResults = demandHistoryResponse.DemandHistoryResults;
                if (demandHistoryResults != null && demandHistoryResults.Any())
                {
                    var dictDemandInvData =
                        new Dictionary<string, List<DemandInventory>>(StringComparer.OrdinalIgnoreCase);
                    foreach (var period in demandHistoryResults)
                    {
                        if (!dictDemandInvData.ContainsKey(period.DemandPeriod))
                        {
                            dictDemandInvData[period.DemandPeriod] = new List<DemandInventory>();
                        }
                        foreach (var wh in period.WareHouses)
                        {
                            var demandInvObject = new DemandInventory
                            {
                                WhsId = wh.WarehouseID,
                                WhsType = wh.WarehouseType,
                                DemandQuantity = wh.DemandQuantity
                            };

                            dictDemandInvData[period.DemandPeriod].Add(demandInvObject);
                        }
                    }
                    result = RefineDictToListInventory(dictDemandInvData);
                }

            }
            return result;
        }

        private List<DemandInventoryContract> RefineDictToListInventory(Dictionary<string, List<DemandInventory>> dictDemandInvData)
        {
            var result = new List<DemandInventoryContract>();

            foreach (var key in dictDemandInvData.Keys)
            {
                var item = new DemandInventoryContract { DemandPeriod = RefineKeyForUI(key) };

                var results = new List<DemandInventory>();
                results.Add(new DemandInventory { WhsId = "COM", WhsName = "South", DemandQuantity = -1 });
                results.Add(new DemandInventory { WhsId = "MOM", WhsName = "Central", DemandQuantity = -1 });
                results.Add(new DemandInventory { WhsId = "REN", WhsName = "West", DemandQuantity = -1 });
                results.Add(new DemandInventory { WhsId = "SOM", WhsName = "East", DemandQuantity = -1 });

                foreach (var demandInv in dictDemandInvData[key])
                {
                    var whsId = demandInv.WhsId;

                    switch (whsId.ToUpper())
                    {
                        case "COM":
                            results[0].WhsName = GetWhsIndicator(demandInv.WhsType, whsId);
                            results[0].DemandQuantity = demandInv.DemandQuantity;
                            break;
                        case "MOM":
                            results[1].WhsName = GetWhsIndicator(demandInv.WhsType, whsId);
                            results[1].DemandQuantity = demandInv.DemandQuantity;
                            break;
                        case "REN":
                            results[2].WhsName = GetWhsIndicator(demandInv.WhsType, whsId);
                            results[2].DemandQuantity = demandInv.DemandQuantity;
                            break;
                        case "SOM":
                            results[3].WhsName = GetWhsIndicator(demandInv.WhsType, whsId);
                            results[3].DemandQuantity = demandInv.DemandQuantity;
                            break;
                    }
                }

                item.Inv1Name = results[0].WhsName;
                item.Inv1Value = int.Parse(results[0].DemandQuantity.ToString());

                item.Inv2Name = results[1].WhsName;
                item.Inv2Value = int.Parse(results[1].DemandQuantity.ToString());

                item.Inv3Name = results[2].WhsName;
                item.Inv3Value = int.Parse(results[2].DemandQuantity.ToString());

                item.Inv4Name = results[3].WhsName;
                item.Inv4Value = int.Parse(results[3].DemandQuantity.ToString());

                result.Add(item);
            }

            return result;
        }

        private string RefineKeyForUI(string key)
        {
            return string.Format("{0} / {1}", key.Substring(key.Length - 2), key.Substring(0, 4));
        }

        private string GetWhsIndicator(string whsType, string whsId)
        {
            var whsName = whsId;
            if (whsName == "COM") whsName = "South";
            else if (whsName == "MOM") whsName = "Central";
            else if (whsName == "REN") whsName = "West";
            else whsName = "East";

            switch (whsType.Trim().ToUpper())
            {
                case "P":
                    whsName += " *";
                    break;
                case "S":
                    whsName += " **";
                    break;
            }
            return whsName;
        }

        private DCGridTitleProperty GetTitlePropertyByBTKey(string btKey, List<DCGridTitleProperty> gridTitleProperties)
        {
            foreach (var gridProp in gridTitleProperties)
            {
                if (gridProp.BTKey == btKey)
                {
                    return gridProp;
                }
            }

            return null;
        }

        private List<DCGridLine> GetDCGridLinesFromTitleProperty(string btKey, DCGridTitleProperty gridTitleProperty)
        {
            if (gridTitleProperty != null)
            {
                return gridTitleProperty.DCGridLines;
            }

            return new List<DCGridLine>();
        }

        private static bool IsValidCartLimitation(IEnumerable<AddToNewCartObject> addToNewCartObjects, CartManager cartManager, Cart cart)
        {
            if (cart != null)
            {
                // Check cart limitation <= 9,999
                // Get list of BTKeys to be added to Cart
                var lstAddBtKeys = addToNewCartObjects.Select(product => product.BTKey).ToList();
                //validation limitation prior to adding to cart, this should be performed by stored procedure
                if (!CartFrameworkHelper.IsAllowAddToCart(cartManager, cart, lstAddBtKeys))
                {
                    return false;
                }
            }
            return true;
        }

        int GetQuantitiesByBtkeys(string userId, string cartId, string btkey)
        {
            //var cartManager = CartContext.Current.GetCartManagerForUser(SiteContext.Current.UserId);
            var cartManager = new CartManager(userId);
            if (cartManager != null)
            {
                var quantiies = cartManager.GetQuantitiesByBtKeys(cartId, new List<String>() { btkey });
                if (quantiies != null && quantiies.ContainsKey(btkey))
                {
                    return quantiies[btkey];
                }
            }
            return -1;
        }

        public AppServiceResult<LandingPageResponse> GetCartFolderById(string folderId, string userId)
        {
            var cartFolder = CartDAOManager.Instance.GetCartFolderById(folderId, userId);
            return null;
        }

        public async Task<AppServiceResult<string>> TestAsync()
        {
            var stringResult = await CartDAOManager.Instance.TestAsync();

            var result = new AppServiceResult<string>();

            result.Data = stringResult;
            result.Status = AppServiceStatus.Success;

            return result;
        }

        public async Task<AppServiceResult<string>> GetDataSetAsync()
        {
            var stringResult = await CartDAOManager.Instance.GetDataSetAysnc();

            var result = new AppServiceResult<string>();

            result.Data = stringResult;
            result.Status = AppServiceStatus.Success;

            return result;
        }

        internal async Task<AppServiceResult<List<string>>> GetBasketByFolderIdAsync(string folderId)
        {
            try
            {
                var ajaxResult = new AppServiceResult<List<string>>();
                //
                var carts = await CartDAOManager.Instance.GetBasketByFolderIdAsync(folderId);
                var results = carts.Select(cart => cart.CartName).ToList();
                //
                ajaxResult.Data = results;
                ajaxResult.Status = AppServiceStatus.Success;
                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        internal async Task<AppServiceResult<BasketInformationOfMiniCart>> GetQuantityOfCartAsync(string cartName,
            string cartId, string userId)
        {
            try
            {
                var result = new AppServiceResult<BasketInformationOfMiniCart>
                {
                    Data = new BasketInformationOfMiniCart
                    {
                        ListPrices = "0.00",
                        DateModified = "",
                        NumberOfItems = "0"
                    }
                };
                if (string.IsNullOrEmpty(userId))
                    return null;

                //return new CartManager(userId, null, true);
                var cart = await CartDAOManager.Instance.GetCartByIdAsync(cartId, userId);
                if (cart != null)
                {
                    result.Data.NumberOfItems = cart.LineItemCount.ToString();
                    result.Data.ListPrices = cart.CartTotalListPrice.ToString("0,0.00");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        private const string GetSuggestionListCacheKey = "__getSuggestionListCacheKey{0}";

        internal List<string> GetSuggestionList(string prefixText, int startRowIndex, int pageSize)
        {
            try
            {
                // key must be in lower case to match with FAST result

                var documentKey = SearchFieldNameConstants.queryterm.ToLower();

                var cacheKey = string.Format(GetSuggestionListCacheKey, prefixText.ToLower());

                var results = CachingController.Instance.Read(cacheKey) as List<string>;

                if (results != null) return results;

                var suggestionList = new List<string>();
                var search = SearchHelper.CreateSearchForSearchTypeAhead(prefixText);
                var sortExpression = new string[] { SuggestionItemNameConstants.QueryTermsRank };
                var searchResult = ProductSearchController.ExecuteSearchForTypeAhead(search, sortExpression,
                    startRowIndex, pageSize);
                if (searchResult != null &&
                    searchResult.ResultDocuments != null &&
                    searchResult.ResultDocuments.Count > 0)
                {
                    foreach (var document in searchResult.ResultDocuments)
                    {
                        if (document.DocumentFields != null &&
                            document.DocumentFields.Keys.Contains(documentKey))
                        {
                            string fieldValue = document.DocumentFields[documentKey];
                            //set empty for value of "null"
                            if (!string.IsNullOrEmpty(fieldValue) && !fieldValue.ToLower().Equals("null"))
                            {
                                suggestionList.Add(fieldValue);
                            }
                        }
                    }
                }

                CachingController.Instance.Write(cacheKey, suggestionList);

                return suggestionList;
            }
            catch (Exception ex)
            {
                //Log to ELMAH
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
            }
            return null;
        }

        public AppServiceResult<string> RemoveSearchSelection(string key, string value, string queryString, string userId)
        {
            var ajaxResult = new AppServiceResult<string>();
            var data = "0";
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    Logger.Write(ExceptionCategory.Search.ToString(), "Remove Refine Search Selection, key is null or empty");
                    ajaxResult.Data = "1";
                    return ajaxResult;
                }
                var AdvSession = new AdvSearchController(userId);
                if (string.Compare(key, SearchFieldNameConstants.SearchTerms, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var searchTerm = AdvSession.SearchTerms;
                    if (string.IsNullOrEmpty(value))
                    {
                        AdvSession.SearchTerms = null;
                        AdvSession.SearchTermsObj = null;
                        AdvSession.SetDirty();
                    }
                    else
                    {
                        for (var i = 0; i < searchTerm.Count; i++)
                        {
                            var expression = searchTerm[i];
                            string expValue = string.Format("{0}|{1}|{2}|{3}", expression.Operator,
                                                       expression.Scope,
                                                       System.Security.SecurityElement.Escape(expression.Terms),
                                                       expression.DisplayName);
                            if (string.Compare(expValue, value, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                searchTerm.RemoveAt(i);
                                AdvSession.SearchTermsObj.Remove(expression);
                                AdvSession.SetDirty();

                                break;
                            }
                        }
                    }
                }
                else
                {
                    //var queryString = HttpContext.Current.Request.UrlReferrer.Query;
                    var updatedParameters = HttpUtility.ParseQueryString(queryString);
                    if (string.Compare(key, SearchFieldNameConstants.producttype, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (string.IsNullOrEmpty(value)
                            || string.Compare(value, ProductTypeConstants.Book, StringComparison.OrdinalIgnoreCase) == 0
                            || string.Compare(value, ProductTypeConstants.Music, StringComparison.OrdinalIgnoreCase) == 0
                            || string.Compare(value, ProductTypeConstants.Movie, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            AdvSession.OriginalProductType = string.Empty;
                            // PBI 31141 :Clicking [x] next to Book Product Type should remove Include Format[x] and Exclude Format[x] 
                            // and also should reset the selection made for Product Type under Refine Your Search
                            AdvSession.AddQueryStrFilter(SearchFieldNameConstants.includedproducttype, string.Empty);
                            AdvSession.AddQueryStrFilter(SearchFieldNameConstants.excludedproducttype, string.Empty);
                        }
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        AdvSession.AddQueryStrFilter(key, string.Empty);
                        updatedParameters.Remove(key);

                        var facets = CachingController.Instance.Read(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet)) as List<KeyValuePair<string, string>> ??
                                     new List<KeyValuePair<string, string>>();

                        //var facets = AdvSession.GetFacets();
                        for (var i = 0; i < facets.Count; i++)
                        {
                            if (facets[i].Key == key)
                            {
                                facets.RemoveAt(i);
                            }
                        }
                        CachingController.Instance.Write(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet), facets);
                    }
                    else
                    {
                        var searchValue = AdvSession.GetQueryStrFilter(key);
                        var values = searchValue.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        var newList = new List<string>();
                        for (var i = 0; i < values.Length; i++)
                        {
                            var val = values[i];
                            if (string.Compare(val, value, StringComparison.OrdinalIgnoreCase) == 0)
                                continue;

                            newList.Add(val);
                        }
                        if (newList.Count > 0)
                        {
                            var newvalue = string.Join("|", newList.ToArray());

                            AdvSession.AddQueryStrFilter(key, newvalue);

                            foreach (string viewProperty in updatedParameters.Keys)
                            {
                                if (string.Compare(viewProperty, key, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    updatedParameters[viewProperty] = newvalue;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            AdvSession.AddQueryStrFilter(key, string.Empty);
                            updatedParameters.Remove(key);

                        }
                        removeBreadCrum(key, value, userId);
                    }
                    //AdvSession.CommitChanges();
                    data = QueryStringHelper.BuildQueryString(updatedParameters);
                }
                AdvSession.CommitChanges();

                ajaxResult.Data = data;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                ajaxResult.Data = "1";
            }
            return ajaxResult;
        }
        private void removeBreadCrum(string key, string value, string userId)
        {
            var facets = CachingController.Instance.Read(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet)) as List<KeyValuePair<string, string>> ??
                                    new List<KeyValuePair<string, string>>();

            //var facets = AdvSession.GetFacets();
            for (var i = 0; i < facets.Count; i++)
            {
                if (facets[i].Key == key)
                {
                    var values = facets[i].Value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    var newList = new List<string>();
                    foreach (var value1 in values)
                    {
                        if (string.Compare(value1, value, StringComparison.OrdinalIgnoreCase) == 0)
                            continue;
                        newList.Add(value1);
                    }
                    if (newList.Count > 0)
                    {
                        facets[i] = new KeyValuePair<string, string>(key, string.Join("|", newList.ToArray()));
                    }
                    else
                    {
                        facets.RemoveAt(i);
                        break;
                    }
                }
            }
            CachingController.Instance.Write(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet), facets);

        }

        public AppServiceResult<bool> ToggleMyPreferences(string userId)
        {
            var ajaxResult = new AppServiceResult<bool>();
            try
            {
                var AdvSession = new AdvSearchController(userId);
                AdvSession.IsUsingMyPreferencesValues = !AdvSession.IsUsingMyPreferencesValues;
                AdvSession.CommitChanges();
                ajaxResult.Data = true;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                ajaxResult.Data = false;
            }
            return ajaxResult;
        }
        public List<SearchFacetNode> LoadSearchFacet(string nodeValue, string orgId, string userId, MarketType? marketType, string[] productType, SearchByIdData searchData,
            SearchArguments searchArgs, bool isWfeFarmCacheAvailable)
        {
            var result = new List<SearchFacetNode>();
            try
            {
                FacetNode root = null;
                if (isWfeFarmCacheAvailable)
                {
                    // get search facet data from Farm Cache
                    root = CachingController.Instance.Read(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SearchFacet)) as FacetNode;
                }

                // do search again
                if (root == null)
                {
                    var sr = ProductSearchController.Search(searchArgs, marketType, searchData.SimonSchusterEnabled,
                        searchData.CountryCode, searchData.ESuppliers);

                    if (sr != null)
                    {
                        root = BuildClusterTree(sr.Clusters, productType, marketType);
                    }
                }

                if (root != null)
                {
                    var filterable = false;
                    var isStockCheckFacet = false;
                    var showAllWhs = false;
                    List<string> validWhs = null;
                    var anode = root;
                    var isRoot = false;
                    if (!string.IsNullOrEmpty(nodeValue))
                    {
                        filterable = true;
                        isStockCheckFacet = nodeValue.Contains("ngstockfacet");
                        if (isStockCheckFacet)
                        {
                            showAllWhs = ShowAllWhs(orgId);
                            validWhs = GetWhsInfoForFilteringStockCheck(userId);
                        }

                        anode = root.FindNode(nodeValue);
                        if (anode == null)
                            return result;
                    }
                    else
                        isRoot = true;

                    foreach (var item in anode.Nodes)
                    {
                        var itemValue = item.Value;

                        if (isStockCheckFacet && !showAllWhs && validWhs != null && validWhs.Count > 0)
                        {
                            var continueFlag = true;
                            foreach (var whs in validWhs)
                            {
                                if (itemValue.IndexOf(whs, StringComparison.OrdinalIgnoreCase) != 0) continue;

                                continueFlag = false;
                                break;
                            }

                            if (continueFlag) continue;
                        }

                        var i = new SearchFacetNode
                        {
                            Text = item.Text,
                            Value = itemValue,
                            Checkable = item.IsShowCheckBox,
                            Filteralbe = filterable,
                            Level = isRoot ? 0 : item.Level,
                            Expandable = item.Nodes.Count > 0
                        };

                        result.Add(i);
                    }

                    if (string.Compare(nodeValue, SearchFieldNameConstants.audience, StringComparison.OrdinalIgnoreCase) == 0)
                        result = result.OrderBy(obj => obj.Text).ToList();
                }

            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        private FacetNode BuildClusterTree(IEnumerable<Cluster> clusterlist, string[] scProductType, MarketType? scMarketType)
        {
            FacetNode facetTreeRoot = null;
            if (clusterlist != null)
            {
                facetTreeRoot = new FacetNode();
                //
                FacetNode productTypeNode = null;
                FacetNode eContentNode = null;
                FacetNode includedFormatNode = null;
                FacetNode audienceNode = null;
                FacetNode movieRatingNode = null;
                FacetNode deweyRangeNode = null;
                FacetNode lcClassificationNode = null;
                FacetNode subjectNode = null;
                FacetNode movieGenreNode = null;
                FacetNode musicGenreNode = null;
                FacetNode pubDateNode = null;
                FacetNode reviewPubNode = null;
                FacetNode languageNode = null;
                FacetNode merchantNode = null;
                FacetNode productFeatureNode = null;
                //FacetNode readingCountNode = null;
                //FacetNode acceleratedNode = null;

                FacetNode eSupplierNode = null;
                FacetNode formDetailsNode = null;
                FacetNode purchaseOptionNode = null;
                FacetNode productAttributeNode = null;
                FacetNode positivereviewNode = null;
                FacetNode starredreviewNode = null;
                FacetNode btprogramsNode = null;
                FacetNode ayprogramsNode = null;
                //FacetNode btpublicationsNode = null;
                FacetNode bookclassificationNode = null;
                FacetNode ivtFacetNode = null;
                FacetNode ivtDFacetNode = null;
                FacetNode ivtLEFacetNode = null;
                FacetNode demandFacetNode = null;
                FacetNode btpublicationFacetNode = null;
                FacetNode childrensFormatNode = null;
                //
                var productTypes = GetProductTypes(scProductType);
                //
                foreach (var cluster in clusterlist)
                {
                    if (IsInclude(cluster, productTypes, scMarketType))
                    {
                        switch (cluster.NavigatorName)
                        {
                            case NavigatorNameConstants.producttype:
                                productTypeNode = CreateClusterNode(cluster, SearchFieldNameConstants.producttype, true, true, true);
                                break;
                            case NavigatorNameConstants.econtentplatform:
                                eContentNode = CreateClusterNode(cluster, SearchFieldNameConstants.econtentplatform, false, true, false);
                                break;
                            case NavigatorNameConstants.includedformat:
                                //PBI 26210 Change 1
                                includedFormatNode = CreateClusterNode(cluster, SearchFieldNameConstants.includedformats, true, true, true);
                                break;
                            case NavigatorNameConstants.audience:
                                var validNumberAudienceType = 0;
                                audienceNode = CreateClusterNode(cluster, SearchFieldNameConstants.audience, true, true, true);
                                if (audienceNode != null && audienceNode.Nodes != null && audienceNode.Nodes.Count > 0)
                                {
                                    var sortedAudienceNode = new FacetNode { Nodes = new List<FacetNode>() };
                                    //Re-order the AudienceType
                                    var stAudienceTypes = SiteTermHelper.Instance.GetFlattenAudienceTypesSiteTem();
                                    for (var i = 0; i < stAudienceTypes.Count; i++)
                                    {
                                        var target = stAudienceTypes[i];

                                        for (var j = 0; j < audienceNode.Nodes.Count; j++)
                                        {
                                            var subNode = audienceNode.Nodes[j];
                                            if (subNode != null && subNode.Nodes != null && subNode.Nodes.Count > 0)
                                            {
                                                var item = subNode.Nodes.FirstOrDefault(f => f.Value == target.SearchValue);
                                                if (item != null)
                                                {
                                                    subNode.Nodes.Remove(item);
                                                    sortedAudienceNode.Nodes.Add(ReplaceTextByValue(item));
                                                    validNumberAudienceType += GetProductNumberOfNode(item);
                                                }
                                            }
                                            else
                                            {
                                                var item = audienceNode.Nodes.FirstOrDefault(f => f.Value == target.SearchValue);
                                                if (item != null)
                                                {
                                                    audienceNode.Nodes.Remove(item);
                                                    sortedAudienceNode.Nodes.Add(item);
                                                    validNumberAudienceType += GetProductNumberOfNode(item);
                                                }
                                            }
                                        }
                                    }
                                    if (validNumberAudienceType == 0) audienceNode = null;
                                    else
                                    {
                                        audienceNode.Nodes.Clear();
                                        audienceNode.Text = string.Format("{0} ({1})", cluster.Name,
                                                                          validNumberAudienceType);
                                        audienceNode.Nodes.AddRange(sortedAudienceNode.Nodes);
                                    }
                                }
                                break;
                            case NavigatorNameConstants.movierating:
                                movieRatingNode = CreateClusterNode(cluster, SearchFieldNameConstants.rating, false, true, true);
                                break;
                            case NavigatorNameConstants.deweyrange:
                                //PBI 26210 Change 2
                                deweyRangeNode = CreateClusterNode(cluster, SearchFieldNameConstants.deweyrange, true, true, true);
                                break;
                            case NavigatorNameConstants.lcclasssification:
                                //PBI 26210 Change 3
                                lcClassificationNode = CreateClusterNode(cluster, SearchFieldNameConstants.lcclassification, true, true, true);
                                break;
                            case NavigatorNameConstants.subject:
                                //PBI 26210 Change 4
                                subjectNode = CreateClusterNode(cluster, SearchFieldNameConstants.subject1, true, true, true);
                                break;
                            case NavigatorNameConstants.moviegenre:
                                movieGenreNode = CreateClusterNode(cluster, SearchFieldNameConstants.moviegenre, true, true, true);
                                break;
                            case NavigatorNameConstants.musicgenre:
                                musicGenreNode = CreateClusterNode(cluster, SearchFieldNameConstants.musicgenre, true, true, true);
                                break;
                            case NavigatorNameConstants.pubdaterange:
                                ClusterNode[] next = new ClusterNode[4];
                                ClusterNode[] prev = new ClusterNode[4];

                                foreach (var clusterNode in cluster.Nodes)
                                {
                                    if (clusterNode.Path.Contains("next"))
                                    {
                                        if (clusterNode.Path.Contains("30"))
                                            next[0] = clusterNode;
                                        else if (clusterNode.Path.Contains("60"))
                                            next[1] = clusterNode;
                                        else if (clusterNode.Path.Contains("90"))
                                            next[2] = clusterNode;
                                        else if (clusterNode.Path.Contains("180"))
                                            next[3] = clusterNode;
                                    }
                                    else if (clusterNode.Path.Contains("previous"))
                                    {
                                        if (clusterNode.Path.Contains("30"))
                                            prev[0] = clusterNode;
                                        else if (clusterNode.Path.Contains("60"))
                                            prev[1] = clusterNode;
                                        else if (clusterNode.Path.Contains("90"))
                                            prev[2] = clusterNode;
                                        else if (clusterNode.Path.Contains("180"))
                                            prev[3] = clusterNode;
                                    }
                                }
                                int count = 0;
                                foreach (var clusterNode in next)
                                {
                                    if (clusterNode == null) continue;
                                    clusterNode.SetTotalDocumentCount(clusterNode.TotalDocumentCount + count);
                                    count = clusterNode.TotalDocumentCount;
                                }
                                count = 0;
                                foreach (var clusterNode in prev)
                                {
                                    if (clusterNode == null) continue;
                                    clusterNode.SetTotalDocumentCount(clusterNode.TotalDocumentCount + count);
                                    count = clusterNode.TotalDocumentCount;
                                }
                                pubDateNode = CreateClusterNode(cluster, SearchFieldNameConstants.pubdaterange, false, true, false);
                                break;
                            case NavigatorNameConstants.reviewpub:
                                reviewPubNode = CreateClusterNode(cluster, SearchFieldNameConstants.reviewpub, true, true, true);
                                reviewPubNode = FilterOldFiveYearsData(reviewPubNode);
                                break;
                            case NavigatorNameConstants.btpubliterals:
                                btpublicationFacetNode = CreateClusterNode(cluster, SearchFieldNameConstants.btpubliterals, true, true, true);
                                btpublicationFacetNode = FilterOldFiveYearsData(btpublicationFacetNode);
                                break;
                            case NavigatorNameConstants.language:
                                languageNode = CreateClusterNode(cluster, SearchFieldNameConstants.languageliteral, true, true, true);
                                break;
                            case NavigatorNameConstants.merchantcategory:
                                //PBI 26210 Change 5
                                merchantNode = CreateMerchantCategoryClusterNode(cluster, SearchFieldNameConstants.merchcategory, true, true, true);
                                break;
                            case NavigatorNameConstants.productfeatures:
                                productFeatureNode = CreateClusterNode(cluster, SearchFieldNameConstants.productfeatures, true, true, true);
                                break;
                            case NavigatorNameConstants.esupplier:
                                eSupplierNode = CreateClusterNode(cluster, SearchFieldNameConstants.esupplier, true, true, true);
                                break;
                            case NavigatorNameConstants.formdetails:
                                formDetailsNode = CreateClusterNode(cluster, SearchFieldNameConstants.formdetails, true, true, true);
                                break;
                            case NavigatorNameConstants.purchaseoption:
                                purchaseOptionNode = CreateClusterNode(cluster, SearchFieldNameConstants.purchaseoption, true, true, true);
                                break;
                            case NavigatorNameConstants.productattribute:
                                productAttributeNode = CreateClusterNode(cluster, SearchFieldNameConstants.productattribute, true, true, true);
                                break;
                            case NavigatorNameConstants.positivereview:
                                positivereviewNode = CreateClusterNode(cluster, SearchFieldNameConstants.positivecode, true, true, true);
                                break;
                            case NavigatorNameConstants.starredreview:
                                starredreviewNode = CreateClusterNode(cluster, SearchFieldNameConstants.starredcode, true, true, true);
                                break;
                            case NavigatorNameConstants.btprograms:
                                btprogramsNode = CreateClusterNode(cluster, SearchFieldNameConstants.btprograms, true, true, true);
                                break;
                            case NavigatorNameConstants.ayprograms:
                                ayprogramsNode = CreateClusterNode(cluster, SearchFieldNameConstants.ayprograms, true, true, true);
                                break;
                            //case NavigatorNameConstants.btpublications:
                            //    btpublicationsNode = CreateClusterNode(cluster, SearchFieldNameConstants.btpublications, true, true, true);
                            //    break;
                            case NavigatorNameConstants.bookclassification:
                                bookclassificationNode = CreateBookClassificationClusterNode(cluster, SearchFieldNameConstants.booktypeliteral, true, true, true);
                                break;
                            case NavigatorNameConstants.ivtfacet:
                                if (cluster.Nodes != null && cluster.Nodes.Count > 0)
                                {
                                    foreach (var node in cluster.Nodes)
                                    {
                                        if (node.SubNodes != null)
                                        {
                                            var correctTotalCount = 0;//correctTotalCount should be the maximum value.
                                            foreach (var subNode in node.SubNodes)
                                            {
                                                if (correctTotalCount < subNode.TotalDocumentCount)
                                                    correctTotalCount = subNode.TotalDocumentCount;
                                            }
                                            node.SetTotalDocumentCount(correctTotalCount);
                                        }
                                    }
                                }

                                ivtFacetNode = CreateClusterNode(cluster, SearchFieldNameConstants.ivtfaceta, true, true, true);
                                break;
                            case NavigatorNameConstants.ivtfacetd:
                                if (cluster.Nodes != null && cluster.Nodes.Count > 0)
                                {
                                    foreach (var node in cluster.Nodes)
                                    {

                                        if (node.SubNodes != null)
                                        {
                                            var correctTotalCount = 0;//correctTotalCount should be the maximum value.
                                            foreach (var subNode in node.SubNodes)
                                            {
                                                if (correctTotalCount < subNode.TotalDocumentCount)
                                                    correctTotalCount = subNode.TotalDocumentCount;
                                            }
                                            node.SetTotalDocumentCount(correctTotalCount);
                                        }
                                    }
                                }

                                ivtDFacetNode = CreateClusterNode(cluster, SearchFieldNameConstants.ivtfacetd, true, true, true);
                                break;
                            case NavigatorNameConstants.ivtfacetle:
                                if (cluster.Nodes != null && cluster.Nodes.Count > 0)
                                {
                                    foreach (var node in cluster.Nodes)
                                    {

                                        if (node.SubNodes != null)
                                        {
                                            var correctTotalCount = 0;//correctTotalCount should be the maximum value.
                                            foreach (var subNode in node.SubNodes)
                                            {
                                                if (correctTotalCount < subNode.TotalDocumentCount)
                                                    correctTotalCount = subNode.TotalDocumentCount;
                                            }
                                            node.SetTotalDocumentCount(correctTotalCount);
                                        }
                                    }
                                }

                                ivtLEFacetNode = CreateClusterNode(cluster, SearchFieldNameConstants.ivtfacetle, true, true, true);
                                break;
                            case NavigatorNameConstants.demandfacet:
                                demandFacetNode = CreateClusterNode(cluster, SearchFieldNameConstants.demandfacetnew, true, true, true);
                                break;
                            case NavigatorNameConstants.childrensformat:
                                childrensFormatNode = CreateClusterNode(cluster, SearchFieldNameConstants.childrensformat, true, true, true);
                                break;
                        }
                    }
                }

                //All those node above have to added in sequence.
                /*
                    + Product Type  (5058)
                    + eContent Platform  (250)
                    + Book Audience  (4500)
                    + Movie/Music Rating  (400)
                    + Dewey Range  (2500)
                    + LC Classification   (2000)
                    + Book Subject  (4700)
                    + Movie Genre  (200)
                    + Music Genre  (100)
                    + Date Published / Released   (4900)
                    + Reviews Publications   (1000)
                    + Language  (1200)
                    + Merchandise Category  (500)
                    + Product Features   (5058)
                    + Reading Counts   (130)
                    + Accelerated Reader   (130)
                    + Included Format  (50)
                    + Book Classification
                 */
                if (productTypeNode != null)
                {
                    facetTreeRoot.Add(productTypeNode);
                }
                if (eContentNode != null)
                {
                    facetTreeRoot.Nodes.Add(eContentNode);
                }
                if (purchaseOptionNode != null)
                {
                    facetTreeRoot.Nodes.Add(purchaseOptionNode);
                }
                if (productAttributeNode != null)
                {
                    facetTreeRoot.Nodes.Add(productAttributeNode);
                }
                if (audienceNode != null)
                {
                    facetTreeRoot.Nodes.Add(audienceNode);
                }
                if (childrensFormatNode != null)
                {
                    facetTreeRoot.Nodes.Add(childrensFormatNode);
                }
                if (ivtFacetNode != null)
                {
                    facetTreeRoot.Nodes.Add(ivtFacetNode);
                }
                if (ivtDFacetNode != null)
                {
                    facetTreeRoot.Nodes.Add(ivtDFacetNode);
                }
                if (ivtLEFacetNode != null)
                {
                    facetTreeRoot.Nodes.Add(ivtLEFacetNode);
                }
                if (demandFacetNode != null)
                {
                    foreach (var node in demandFacetNode.Nodes)
                    {
                        node.Text = node.Text.Replace(">=", "&ge;");
                    }
                    facetTreeRoot.Nodes.Add(demandFacetNode);
                }
                if (movieRatingNode != null)
                {
                    facetTreeRoot.Nodes.Add(movieRatingNode);
                }
                if (deweyRangeNode != null)
                {
                    facetTreeRoot.Nodes.Add(deweyRangeNode);
                }
                if (lcClassificationNode != null)
                {
                    facetTreeRoot.Nodes.Add(lcClassificationNode);
                }
                if (subjectNode != null)
                {
                    facetTreeRoot.Nodes.Add(subjectNode);
                }
                if (movieGenreNode != null)
                {
                    facetTreeRoot.Nodes.Add(movieGenreNode);
                }
                if (musicGenreNode != null)
                {
                    facetTreeRoot.Nodes.Add(musicGenreNode);
                }
                if (pubDateNode != null)
                {
                    facetTreeRoot.Nodes.Add(pubDateNode);
                }
                if (reviewPubNode != null)
                {
                    facetTreeRoot.Nodes.Add(reviewPubNode);
                }
                if (btpublicationFacetNode != null)
                {
                    facetTreeRoot.Nodes.Add(btpublicationFacetNode);
                }

                if (btprogramsNode != null)
                {
                    facetTreeRoot.Nodes.Add(btprogramsNode);
                }
                if (ayprogramsNode != null)
                {
                    facetTreeRoot.Nodes.Add(ayprogramsNode);
                }
                if (positivereviewNode != null)
                {
                    facetTreeRoot.Nodes.Add(positivereviewNode);
                }
                if (starredreviewNode != null)
                {
                    facetTreeRoot.Nodes.Add(starredreviewNode);
                }
                if (languageNode != null)
                {
                    facetTreeRoot.Nodes.Add(languageNode);
                }
                if (merchantNode != null)
                {
                    facetTreeRoot.Nodes.Add(merchantNode);
                }
                if (productFeatureNode != null)
                {
                    facetTreeRoot.Nodes.Add(productFeatureNode);
                }

                if (eSupplierNode != null)
                {
                    facetTreeRoot.Nodes.Add(eSupplierNode);
                }
                if (formDetailsNode != null)
                {
                    facetTreeRoot.Nodes.Add(formDetailsNode);
                }
                if (includedFormatNode != null) //TFS 9141 Move "included Format" Facet to bottom of list
                {
                    facetTreeRoot.Nodes.Add(includedFormatNode);
                }
                if (bookclassificationNode != null)
                {
                    facetTreeRoot.Nodes.Add(bookclassificationNode);
                }
            }
            //            
            return facetTreeRoot;
        }
        private FacetNode CreateBookClassificationClusterNode(Cluster cluster, string navigatorName, bool isMultiple, bool isShowFirstLevelCheckBox,
            bool isShowOtherLevelCheckbox, int maxLevel = 3)
        {
            /* TFS 11649: show 6 nodes by order as below
                    Adult Fiction
                    Adult Non-Fiction
                    Easy Fiction
                    Easy Non-Fiction
                    Juvenile Fiction
                    Juvenile Non-Fiction
             
             *  TFS 18522 confirms 6 values only, just simply remove NULL value if any.
             */
            var childNodes = cluster.Nodes.Where(node => !string.IsNullOrEmpty(node.Name))
                                            .OrderBy(node => node.Name).ToList();

            if (childNodes.Count == 0)
                return null;

            cluster.Nodes = childNodes;

            // re-calculate document count
            var overwriteTotalCount = childNodes.Sum(node => node.TotalDocumentCount);

            return CreateClusterNode(cluster, navigatorName, isMultiple, isShowFirstLevelCheckBox, isShowOtherLevelCheckbox, maxLevel, overwriteTotalCount);
        }
        private FacetNode CreateMerchantCategoryClusterNode(Cluster cluster, string navigatorName, bool isMultiple, bool isShowFirstLevelCheckBox, bool isShowOtherLevelCheckbox)
        {
            var merchantSubNodes = cluster.Nodes;
            if (merchantSubNodes == null) return null;

            if (merchantSubNodes.Count > 0)
            {
                // TFS12577 - only display specific subfacets
                string[] validSubNodeNames =
                {
                    "AUDIO", "CALENDAR", "FOREIGN LANGUAGE", "GENERAL",
                    "GRAPHIC NOVELS", "JUVENILE", "JUVENILE NOVELTY",
                    "LARGEPRINT", "MASS MARKET", "PRINT ON DEMAND",
                    "TEXTBOOK", "TEXTSTREAM PRINT ON DEMAND", "TEXT",
                    "TRAVEL", "UKG", "UNIVERSITY PRESS", "MAKERSPACE"
                };

                // remove all invalid nodes
                merchantSubNodes.RemoveAll(node => !validSubNodeNames.Contains(node.Name, StringComparer.InvariantCultureIgnoreCase));
            }

            if (merchantSubNodes.Count == 0)
                return null;

            // re-calculate document count
            var overwriteTotalCount = merchantSubNodes.Sum(node => node.TotalDocumentCount);

            return CreateClusterNode(cluster, navigatorName, isMultiple, isShowFirstLevelCheckBox, isShowOtherLevelCheckbox, 3, overwriteTotalCount);
        }
        private FacetNode FilterOldFiveYearsData(FacetNode primaryReviewNode)
        {
            FacetNode output = new FacetNode(primaryReviewNode, false);

            int oldYear = DateTime.Now.Year - 5;
            foreach (var node in primaryReviewNode.Nodes)
            {
                FacetNode newSubNote = new FacetNode(node);
                output.Nodes.Add(newSubNote);
                if (node.Nodes == null || node.Nodes.Count == 0)
                    continue;

                var regex = new Regex(@"\d\d\d\d");
                foreach (var subnote in node.Nodes)
                {
                    var match = regex.Match(subnote.Text);
                    if (match.Success)
                    {
                        int collectedYear = 0;
                        if (!int.TryParse(match.Groups[0].Value, out collectedYear) || collectedYear >= oldYear)
                        {
                            newSubNote.Nodes.Add(subnote);
                        }
                    }
                    else // case there is no Year information in review facet.
                    {
                        newSubNote.Nodes.Add(subnote);
                    }
                }
            }
            return output;
        }
        private int GetProductNumberOfNode(FacetNode item)
        {
            var productNumber = 0;
            string text = item.Text;
            int startIndex = text.IndexOf("(");
            int endIndex = text.IndexOf(")");
            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                int.TryParse(text.Substring(startIndex + 1, endIndex - startIndex - 1), out productNumber);
            return productNumber;
        }
        private FacetNode ReplaceTextByValue(FacetNode item)
        {
            string value = item.Value;
            string text = item.Text;
            int startIndex = text.IndexOf("(");
            int endIndex = text.IndexOf(")");
            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                item.Text = string.Format("{0} {1}", value, text.Substring(startIndex, endIndex - startIndex + 1));
            return item;
        }
        private bool IsInclude(Cluster cluster, Dictionary<ProductType, int> productTypes, MarketType? siteContextMarketType)
        {
            var marketType = siteContextMarketType ?? MarketType.Any;
            switch (cluster.NavigatorName)
            {
                case NavigatorNameConstants.producttype:
                case NavigatorNameConstants.subject:
                case NavigatorNameConstants.publisher:
                case NavigatorNameConstants.pubdaterange:
                case NavigatorNameConstants.pubdate:
                case NavigatorNameConstants.reviewpub:
                case NavigatorNameConstants.btpubliterals:
                case NavigatorNameConstants.language:
                case NavigatorNameConstants.productfeatures:
                case NavigatorNameConstants.musicgenre:
                case NavigatorNameConstants.moviegenre:
                case NavigatorNameConstants.productattribute:
                case NavigatorNameConstants.childrensformat:
                    return true;
                case NavigatorNameConstants.includedformat:
                    if (marketType != MarketType.Retail)
                    {
                        return true;
                    }
                    else if (productTypes.ContainsKey(ProductType.Book))
                    {
                        return true;
                    }
                    break;
                case NavigatorNameConstants.audience:
                case NavigatorNameConstants.merchantcategory:
                case NavigatorNameConstants.award:
                case NavigatorNameConstants.econtentplatform:
                case NavigatorNameConstants.series:
                    if (productTypes.ContainsKey(ProductType.Book))
                        return true;
                    break;
                case NavigatorNameConstants.deweyrange:
                    if (marketType != MarketType.Retail)
                        return true;
                    break;
                case NavigatorNameConstants.lcclasssification:
                    if (marketType == MarketType.PublicLibrary || marketType == MarketType.AcademicLibrary)
                        return true;
                    break;
                case NavigatorNameConstants.movierating:
                    if (productTypes.ContainsKey(ProductType.Music) || productTypes.ContainsKey(ProductType.Movie))
                        return true;
                    break;
                case NavigatorNameConstants.acceleratedreader:
                case NavigatorNameConstants.readingcount:
                    if (productTypes.ContainsKey(ProductType.Book) && marketType == MarketType.SchoolLibrary)
                        return true;
                    break;
                case NavigatorNameConstants.purchaseoption:
                case NavigatorNameConstants.bookclassification:
                    return true;
                case NavigatorNameConstants.demandfacet:
                    return true;
                case NavigatorNameConstants.ivtfacet:
                    // if place this statement out of switch command, it will be called 28 times.
                    var tmp1 = DistributedCacheHelper.GetCurrentUserReservedType();
                    if (tmp1 == "a")
                        return true;
                    break;
                case NavigatorNameConstants.ivtfacetd:
                    var tmp2 = DistributedCacheHelper.GetCurrentUserReservedType();
                    if (tmp2 == "d")
                        return true;
                    break;
                case NavigatorNameConstants.ivtfacetle:
                    var tmp3 = DistributedCacheHelper.GetCurrentUserReservedType();
                    if (tmp3 == "le")
                        return true;
                    break;
            }
            return false;
        }
        private Dictionary<ProductType, int> GetProductTypes(string[] siteContextProductType)
        {
            //var siteContextProductType = SiteContext.Current.ProductType;
            var result = new Dictionary<ProductType, int>();
            foreach (var item in siteContextProductType)
            {
                ProductType productType;
                switch (item)
                {
                    case ProductTypeConstants.Book:
                    case "0":
                        productType = ProductType.Book;
                        break;
                    case ProductTypeConstants.Movie:
                    case "3":
                        productType = ProductType.Movie;
                        break;
                    case ProductTypeConstants.Music:
                    case "2":
                        productType = ProductType.Music;
                        break;
                    default:
                        productType = ProductType.Book;
                        break;
                }
                if (!result.ContainsKey(productType))
                    result.Add(productType, 1);
            }
            return result;
        }

        private FacetNode CreateClusterNode(Cluster cluster, string navigatorName, bool isMultiple, bool isShowFirstLevelCheckBox, bool isShowOtherLevelCheckbox, int maxLevel = 3, int? overwriteTotalCount = null)
        {
            var totalCount = overwriteTotalCount.HasValue ? overwriteTotalCount.Value : cluster.TotalDocumentCount;
            string displayName = string.Format("{0} ({1})", cluster.Name, totalCount);

            var node = new FacetNode
            {
                Text = displayName,
                Value = navigatorName
            };

            foreach (var clusterNode in cluster.Nodes)
            {
                AddClusterNodes(node, clusterNode, navigatorName, isMultiple, isShowFirstLevelCheckBox, isShowOtherLevelCheckbox, maxLevel);
            }
            return node;
        }

        private void AddClusterNodes(FacetNode parentNode, ClusterNode clusterNode, string navigatorName, bool isMultiple, bool isShowFirstLevelCheckBox, bool isShowOtherLevelCheckbox, int maxLevel)
        {
            var treeNode = new FacetNode
            {
                Text = string.Format("{0} ({1})", clusterNode.Name, clusterNode.TotalDocumentCount),
                IsShowCheckBox = parentNode.Level == 0 ? isShowFirstLevelCheckBox : isShowOtherLevelCheckbox,
                Value = clusterNode.Path
                //NavigateUrl = UpdateQueryToSearch(navigatorName, clusterNode.Path, isMultiple),
            };

            parentNode.Add(treeNode);

            if (treeNode.Level < maxLevel)
            {
                var nodes = clusterNode.SubNodes;
                if (navigatorName.Contains("ngstockfacet"))
                {
                    nodes.Sort(delegate(ClusterNode x, ClusterNode y)
                    {
                        var xname = x.Name;
                        var yname = y.Name;
                        int i = 0;
                        int j = 0;
                        int.TryParse(xname.Replace(">", ""), out i);
                        int.TryParse(yname.Replace(">", ""), out j);
                        if (i > j) return 1;
                        if (i == j) return 0;
                        return -1;
                    });
                }

                foreach (var node in nodes)
                {
                    AddClusterNodes(treeNode, node, navigatorName, isMultiple, isShowFirstLevelCheckBox, isShowOtherLevelCheckbox, maxLevel);
                }
            }
        }

        private List<string> GetWhsInfoForFilteringStockCheck(string userId)
        {
            var cacheKey = string.Format(CacheKeyConstant.WhsInfoUserIdCacheKey, userId);
            var cacheValue = CachingController.Instance.Read(cacheKey) as List<string>;

            if (cacheValue == null) return new List<string>();

            return cacheValue;
        }
        private bool ShowAllWhs(string orgId)
        {
            var orgObj = ProfileService.Instance.GetOrganizationById(orgId);
            return orgObj.AllWarehouse.HasValue && orgObj.AllWarehouse.Value;
        }

        public AppServiceResult<bool> ClearSearchFilterSession(string userId)
        {
            var ajaxResult = new AppServiceResult<bool>();
            try
            {

                CachingController.Instance.SetExpired(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.BookSearchFilter));
                CachingController.Instance.SetExpired(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.MusicSearchFilter));
                CachingController.Instance.SetExpired(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.MovieSearchFilter));
                CachingController.Instance.SetExpired(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet));
                var AdvSession = new AdvSearchController(userId);
                AdvSession.ResetCriteria();
                AdvSession.CommitChanges();

                ajaxResult.Data = true;
            }
            catch (Exception ex)
            {
                //result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(ex, ExceptionCategory.Search);
                ajaxResult.Data = false;
            }
            return ajaxResult;
        }
        public AppServiceResult<Dictionary<string, string>> GetAdvSearchFilter(string producttype, string userId)
        {
            var result = new AppServiceResult<Dictionary<string, string>>();
            try
            {
                Dictionary<string, string> dataReturn = null;
                if (producttype == "book")
                {
                    //var book = AdvSession.Book;
                    //if (book.BookSearchFilter != null)
                    dataReturn = CachingController.Instance.Read(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.BookSearchFilter)) as Dictionary<string, string>;
                    //dataReturn = book.BookSearchFilter;
                }
                else if (producttype == "movie")
                {
                    dataReturn = CachingController.Instance.Read(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.MovieSearchFilter)) as Dictionary<string, string>;
                    //var movie = AdvSession.Movie;
                    //if (movie.MovieSearchFilter != null)
                    //    dataReturn = movie.MovieSearchFilter;
                }
                else if (producttype == "music")
                {
                    dataReturn = CachingController.Instance.Read(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.MusicSearchFilter)) as Dictionary<string, string>;
                    //var music = AdvSession.Music;
                    //if (music.MusicSearchFilter != null)
                    //    dataReturn = music.MusicSearchFilter;
                }

                if (dataReturn == null) dataReturn = new Dictionary<string, string>();

                result.Status = AppServiceStatus.Success;
                result.Data = dataReturn;
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            } return result;
        }

        public AppServiceResult<string> RefineSearch(List<TwoParam> choices, string userId, bool reset = false)
        {
            var ajaxResult = new AppServiceResult<string>();
            ajaxResult.Data = string.Empty;
            try
            {
                var AdvSession = new AdvSearchController(userId);
                if (reset)
                {
                    CachingController.Instance.SetExpired(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.BookSearchFilter));
                    CachingController.Instance.SetExpired(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.MusicSearchFilter));
                    CachingController.Instance.SetExpired(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.MovieSearchFilter));
                    CachingController.Instance.SetExpired(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet));

                    AdvSession.ResetAdvancedSearch();
                }

                foreach (var choice in choices)
                {
                    var values = AdvSession.GetQueryStrFilter(choice.Param1);
                    if (!string.IsNullOrEmpty(values))
                        AdvSession.AddQueryStrFilter(choice.Param1, string.Empty);

                    switch (choice.Param1)
                    {
                        case SearchFieldNameConstants.purchaseoption:
                            AdvSession.AddQueryStrFilter(SearchFieldNameConstants.includepurchaseoption, string.Empty);//remove include PO from adv search
                            break;
                        case SearchFieldNameConstants.childrensformat:
                            AdvSession.AddQueryStrFilter(SearchFieldNameConstants.childrensformat, string.Empty);
                            break;
                        case SearchFieldNameConstants.producttype:
                            AdvSession.AddQueryStrFilter(SearchFieldNameConstants.includedproducttype, string.Empty);
                            //AdvSession.AddQueryStrFilter(SearchFieldNameConstants.excludedproducttype, string.Empty);
                            break;
                        case SearchFieldNameConstants.productattribute:
                            AdvSession.AddQueryStrFilter(SearchFieldNameConstants.includeproductattribute, string.Empty);
                            //AdvSession.AddQueryStrFilter(SearchFieldNameConstants.excludeproductattribute, string.Empty);
                            break;
                     }
                }
                var dict = new Dictionary<string, string>();
                foreach (var choice in choices)
                {
                    var navigatorName = choice.Param1;
                    var value = choice.Param2;

                    if (!dict.ContainsKey(navigatorName))
                    {
                        if (IsValidSearch(AdvSession.GetQueryStrFilter(navigatorName), value))
                            dict.Add(navigatorName, value);
                    }
                    else
                    {
                        switch (navigatorName)
                        {
                            case SearchFieldNameConstants.producttype:
                            case SearchFieldNameConstants.audience:
                            case SearchFieldNameConstants.acceleratedreader:
                            case SearchFieldNameConstants.readingcount:
                            case SearchFieldNameConstants.reviewpub:
                            case SearchFieldNameConstants.moviegenre:
                            case SearchFieldNameConstants.musicgenre:
                            case SearchFieldNameConstants.rating:
                            case SearchFieldNameConstants.languageliteral:
                            case SearchFieldNameConstants.languagecode:
                            case SearchFieldNameConstants.subject1:
                            case SearchFieldNameConstants.subject:
                            case SearchFieldNameConstants.econtentplatform:
                            case SearchFieldNameConstants.booktypeliteral:
                            case SearchFieldNameConstants.ivtfaceta:
                            case SearchFieldNameConstants.ivtfacetd:
                            case SearchFieldNameConstants.ivtfacetle:
                            case SearchFieldNameConstants.demandfacetnew:
                            case SearchFieldNameConstants.purchaseoption:
                            case SearchFieldNameConstants.childrensformat:
                            case SearchFieldNameConstants.includedformats:
                            case SearchFieldNameConstants.deweyrange:
                            case SearchFieldNameConstants.merchcategory:
                            case SearchFieldNameConstants.lcclassification:
                            case SearchFieldNameConstants.btpubliterals:
                            case SearchFieldNameConstants.productattribute:
                            case SearchFieldNameConstants.pubdaterange:
                                if (IsValidSearch(AdvSession.GetQueryStrFilter(navigatorName), value))
                                {
                                    dict[navigatorName] = dict[navigatorName] + QueryStringValue.OR_VALUE_SEPERATOR +
                                                          value;
                                }
                                break;
                            case SearchFieldNameConstants.productfeatures:
                                if (IsValidSearch(AdvSession.GetQueryStrFilter(navigatorName), value))
                                {
                                    dict[navigatorName] = dict[navigatorName] + QueryStringValue.AND_VALUE_SEPERATOR +
                                                          value;
                                }
                                break;
                            default: //Not Mutiple
                                dict[navigatorName] = value;
                                AdvSession.AddQueryStrFilter(navigatorName, string.Empty);
                                break;
                        }
                    }
                }
                if (dict.Count == 0)
                {
                    ajaxResult.Data = "0";
                }
                else
                {
                    var sitemapFacet = CachingController.Instance.Read(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet)) as List<KeyValuePair<string, string>> ??
                                       new List<KeyValuePair<string, string>>();

                    foreach (KeyValuePair<string, string> keyvalue in dict)
                    {
                        var currentValue = AdvSession.GetQueryStrFilter(keyvalue.Key);
                        if (!string.IsNullOrEmpty(currentValue))
                            currentValue += QueryStringValue.AND_VALUE_SEPERATOR;

                        AdvSession.AddQueryStrFilter(keyvalue.Key, currentValue + keyvalue.Value);
                        //AdvSession.AddFacet(keyvalue.Key, keyvalue.Value);
                        sitemapFacet.Add(new KeyValuePair<string, string>(keyvalue.Key, keyvalue.Value));
                    }
                    //CachingController.Instance.Write(SessionVariableName.RefineSearchFlag, true);
                    CachingController.Instance.Write(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet), sitemapFacet);
                }
                AdvSession.CommitChanges();
                ajaxResult.Status = AppServiceStatus.Success;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                ajaxResult.Status = AppServiceStatus.Fail;
                ajaxResult.ErrorMessage = ex.Message;
            }
            return ajaxResult;
        }
        private bool IsValidSearch(string currentValue, string value)
        {
            var currentFilter = currentValue.Split(new string[] { QueryStringValue.AND_VALUE_SEPERATOR, QueryStringValue.OR_VALUE_SEPERATOR },
                    StringSplitOptions.RemoveEmptyEntries);

            var newFilterDepth = value.Split(QueryStringValue.NAVIGATOR_SEPERATOR).Length;
            foreach (var facet in currentFilter)
            {
                if (string.Compare(facet, value, StringComparison.OrdinalIgnoreCase) == 0) return false;
                if (facet.Split(QueryStringValue.NAVIGATOR_SEPERATOR).Length > newFilterDepth)
                    return false;
            }
            return true;
        }
        public AppServiceResult<string> RemoveSearchFacet(int key, string userId)
        {
            var ajaxResult = new AppServiceResult<string>();
            try
            {
                var facets = CachingController.Instance.Read(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet)) as List<KeyValuePair<string, string>> ??
                                     new List<KeyValuePair<string, string>>();

                var AdvSession = new AdvSearchController(userId);
                //var facets = AdvSession.GetFacets();
                if (key < facets.Count)
                {
                    for (var i = facets.Count - 1; i > key; i--)
                    {
                        var facet = facets[i];
                        var currentValue = AdvSession.GetQueryStrFilter(facet.Key);
                        if (currentValue.Length == facet.Value.Length)
                        {
                            AdvSession.AddQueryStrFilter(facet.Key, string.Empty);
                        }
                        else
                        {
                            AdvSession.AddQueryStrFilter(facet.Key, currentValue.Replace(QueryStringValue.AND_VALUE_SEPERATOR + facet.Value, string.Empty));
                        }
                        facets.Remove(facet);
                    }
                }
                CachingController.Instance.Write(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet), facets);
                AdvSession.CommitChanges();
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                ajaxResult.Data = "1";
            }
            return ajaxResult;
        }
        public async Task<bool> ResetCacheAndSearch(string suggestionText, string userId)
        {
            try
            {
                CachingController.Instance.SetExpired(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SiteMapFacet));
                var AdvSession = new AdvSearchController(userId);
                AdvSession.ResetAdvancedSearch();
                AdvSession.OriginalProductType = string.Empty;
                AdvSession.CommitChanges();

                return true;
                //return await ProductCatalogDAO.Instance.UpdateHitCount(suggestionText);
            }
            catch (Exception e)
            {
                //Handle error

                //Log to ELMAH
                Logger.WriteLog(e, ExceptionCategory.Catalog.ToString());
                return false;
            }
        }

        #region SavedSearch

        public async Task<AppServiceResult<string>> InsertSavedSearch(string strName, string userId)
        {
            var result = new AppServiceResult<string>();
            try
            {
                var AdvSession = new AdvSearchController(userId);
                var obj = AdvSession.ToSearchCriteria();
                var serializer = new JavaScriptSerializer();
                var entity = new SavedSearchEntity
                {
                    UserId = userId,//SiteContext.Current.UserId,
                    SavedSearchId = Guid.NewGuid().ToString("b"),
                    SavedSearchName = strName,
                    SearchCriteria = serializer.Serialize(obj),
                    SearchFrom = 3,
                    DateCreated = DateTime.Today,
                    DateModified = DateTime.Today
                };
                var resultObj = await SavedSearchDAO.Instance.Insert(entity);
                result.Data = resultObj.Data;
                result.ErrorMessage = resultObj.ErrorMessage;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.SavedSearch);
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public async Task<AppServiceResult<bool>> DeleteSavedSearch(string id, string userId)
        {
            var ajaxResult = new AppServiceResult<bool>();
            try
            {
                var resultObj = await SavedSearchDAO.Instance.Delete(userId, id);
                ajaxResult.Data = resultObj.Success;
                ajaxResult.ErrorMessage = resultObj.ErrorMessage;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.SavedSearch);
                ajaxResult.ErrorMessage = ex.Message;
                ajaxResult.Data = false;
            }
            return ajaxResult;
        }
        public async Task<AppServiceResult<bool>> EditSavedSearch(string id, string[] productType, string userId)
        {
            var ajaxResult = new AppServiceResult<bool>();
            try
            {
                var resultObj = await GetSavedSearchById(id, userId, productType);
                ajaxResult.Data = resultObj.Success;
                ajaxResult.ErrorMessage = resultObj.ErrorMessage;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.SavedSearch);
                ajaxResult.ErrorMessage = ex.Message;
                ajaxResult.Data = false;
            }
            return ajaxResult;
        }

        private async Task<Result<SavedSearchEntity>> GetSavedSearchById(string id, string userId, string[] productType)
        {
            var resultObj = await SavedSearchDAO.Instance.GetByID(id);
            SavedSearchObjDeserialize objFromDB = null;
            if (resultObj.Success)
            {
                var savedS = resultObj.Data;
                if (savedS != null && !string.IsNullOrEmpty(savedS.SearchCriteria))
                {
                    NameValueCollection nv = null;
                    if (savedS.SearchFrom != 3)
                    {
                        nv = HttpUtility.ParseQueryString(savedS.SearchCriteria);
                    }
                    else
                    {
                        var serializer = new JavaScriptSerializer();
                        objFromDB = serializer.Deserialize<SavedSearchObjDeserialize>(savedS.SearchCriteria);
                    }
                    CachingController.Instance.Write(CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SavedSearchName), savedS.SavedSearchName);
                    var AdvSession = new AdvSearchController(userId);
                    AdvSession.Load(objFromDB, nv, productType);
                    AdvSession.CommitChanges();
                    //AdvSession = new AdvancedSearchSession(objFromDB, nv);
                }
                else
                {
                    resultObj.ErrorMessage = "Search Criteria is empty.";
                }
            }
            return resultObj;
        }

        public async Task<AppServiceResult<string>> RunSavedSearch(string id, string[] productType, string userId)
        {
            var ajaxResult = new AppServiceResult<string>();
            try
            {
                var resultObj = await GetSavedSearchById(id, userId, productType);
                ajaxResult.Data = resultObj.Success.ToString();// ? (isQuickSearchEnabled ? SiteUrl.QuickSearchView : SiteUrl.SearchResults) : string.Empty;
                ajaxResult.ErrorMessage = resultObj.ErrorMessage;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.SavedSearch);
                ajaxResult.ErrorMessage = ex.Message;
                ajaxResult.Data = string.Empty;
            }
            return ajaxResult;
        }

        public async Task<AppServiceResult<bool>> UpdateSavedSearchById(string id, string userId)
        {
            var ajaxResult = new AppServiceResult<bool>();
            try
            {
                var AdvSession = new AdvSearchController(userId);
                var obj = AdvSession.ToSearchCriteria();
                var serializer = new JavaScriptSerializer();
                var entity = new SavedSearchEntity
                {
                    SavedSearchId = id,
                    SearchCriteria = serializer.Serialize(obj),
                    SearchFrom = 3,
                    DateModified = DateTime.Today
                };
                var resultObj = await SavedSearchDAO.Instance.Update(entity);
                ajaxResult.Data = resultObj.Success;
                ajaxResult.ErrorMessage = resultObj.ErrorMessage;

            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                ajaxResult.ErrorMessage = ex.Message;
                ajaxResult.Data = false;
            }
            return ajaxResult;
        }
        #endregion

        internal async Task<AppServiceResult<WCFObjectReturnToClient>> GetNotesAsync(string cartId, List<string> btkeys,
            string userId)
        {
            var ajaxResult = new AppServiceResult<WCFObjectReturnToClient>();
            var wCfObjectReturnToClient = new WCFObjectReturnToClient { };

            try
            {
                var notes = await GridDAOManager.Instance.GetNotesByBTKeysAsync(cartId, userId, btkeys);
                foreach (var btkey in btkeys)
                {
                    var i = notes.FindIndex(note => note.BTKey == btkey);
                    if (i < 0)
                    {
                        notes.Add(new NoteClientObject { BTKey = btkey });
                    }
                }

                wCfObjectReturnToClient.NotesList = notes;
                ajaxResult.Data = wCfObjectReturnToClient;
                ajaxResult.Status = AppServiceStatus.Success;
                ajaxResult.ErrorMessage = "";
            }
            catch (Exception exception)
            {
                ajaxResult.Status = AppServiceStatus.Fail;

                ajaxResult.ErrorMessage = exception.Message;
                ajaxResult.Data = wCfObjectReturnToClient;
                //Log to ELMAH
                Logger.WriteLog(exception, ExceptionCategory.Order.ToString());
            }
            return ajaxResult;
        }

        public AppServiceResult<List<SiteTermObject>> GetProductReviewIndicator(List<string> btKeyHasReviewList, string orgId)
        {
            var result = new AppServiceResult<List<SiteTermObject>>();

            if (string.IsNullOrEmpty(orgId) || btKeyHasReviewList == null) return result;

            try
            {
                var siteTermObjects = ProductSearchController.CheckProductReviewsFromOds(btKeyHasReviewList.Where(x => x.Length <= 10).ToList(),
                    orgId); //skip OE
                result.Data = siteTermObjects;
                result.Status = AppServiceStatus.Success;
            }
            catch (Exception ex)
            {
                result.Data = new List<SiteTermObject>();
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        public async Task<AppServiceResult<List<SiteTermObject>>> GetProductDuplicateIndicator(List<string> btKeyList, List<string> btEKeyList, string basketId,
            bool isRequiredCheckDupCarts, bool isRequiredCheckDupOrder, string userId, string defaultDownloadedCarts, string orgId)
        {
            var result = new AppServiceResult<List<SiteTermObject>>();
            var siteTermObjects = await CartDAOManager.Instance.GetProductDuplicateIndicator(btKeyList, btEKeyList, basketId,
                    isRequiredCheckDupCarts, isRequiredCheckDupOrder, userId, defaultDownloadedCarts,orgId);

            result.Data = siteTermObjects;
            result.Status = AppServiceStatus.Success;
            return result;
        }

        public WCFObjectReturnToClient GetPricingAndPromoForSearch(SearchRequest request)
        {
            var wCfObjectReturnToClient = new WCFObjectReturnToClient { Message = "Sequential" };
            //

            //var siteContextObject = request.SiteContext;

            //if (siteContextObject == null)
            //{
            //    return null;
            //}

            var btKeyList = new List<string>();
            var btKeyHasReviewList = new List<string>();
            var btEKeyList = new List<string>();
            var inventoryStatusArgList = new List<SearchResultInventoryStatusArg>();
            var pricingClientArg = new List<PricingClientArg>();
            var promotionClientArg = new List<PromotionClientArg>();

            var isExternalAPICall = (request.IsFromExternalAPI.HasValue && request.IsFromExternalAPI == true);

            var productSearchResults = (!isExternalAPICall) ? GetProductSearchResultsFromCache(request.UserId) : null;

            if (productSearchResults == null)
            {
                if (!request.IsAltFormatCall.HasValue && request.SearchArguments != null)
                {
                    productSearchResults = ProductSearchController.Search(request.SearchArguments, request.Targeting.MarketType,
                        request.SearchData.SimonSchusterEnabled, request.SearchData.CountryCode, request.SearchData.ESuppliers);
                }
                else if (request.IsAltFormatCall.HasValue && request.IsAltFormatCall.Value)
                {
                    var productIdList = ProductCatalogDAO.Instance.GetRelatedProductIds(request.AltFormatCallBtKey);
                    if (productIdList != null && productIdList.Count > 0)
                    {
                        productSearchResults = ProductSearchController.SearchByIdWithoutAnyRules(productIdList);

                        if (productSearchResults != null && productSearchResults.Items != null && productSearchResults.Items.Count > 0)
                        {
                            CalculatePrice(productSearchResults, request.UserId, request.SearchData.ESuppliers,
                                request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData);
                        }
                    }
                }
            }

            if (productSearchResults == null)
            {
                var logMsg = string.Format("SearchArguments == NULL: {0}, User Name: {1}", request.SearchArguments == null, request.LoginId);
                PricingLogger.LogDebug("GetPricingAndPromoForSearch - Debug", string.Format("productSearchResults == null, Details: {0}", logMsg));
                return wCfObjectReturnToClient;
            }

            foreach (var item in productSearchResults.Items)
            {
                btKeyList.Add(item.BTKey);
                btEKeyList.Add(item.BTEKey);
                if (item.HasReview)
                {
                    btKeyHasReviewList.Add(item.BTKey);
                }

                inventoryStatusArgList.Add(GetSearchResultInventoryStatusArg(item, request.MarketTypeString));
                pricingClientArg.Add(GetPricingClientArg(item));
                promotionClientArg.Add(GetPromotionClientArg(item));
            }

            request.btKeyList = btKeyList;

            if (request.ShowExpectedDiscountPriceForSearch)
            {
                wCfObjectReturnToClient.Pricing = GetProductPricing(pricingClientArg, request.UserId, request.SearchData.ESuppliers
                , request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData).Data;

                // we will bind Promotion data in UI Server side due to the limitation of Marketing Context
                wCfObjectReturnToClient.Promotion = GetProductPromotion(promotionClientArg, request.Targeting);
            }
            else
            {
                var priceList = GetProductPricingForGale(pricingClientArg, request.UserId, request.SearchData.ESuppliers,
                                request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData);
                foreach (var price in priceList)
                {
                    price.DisPercent = price.Price = string.Empty;
                }
                wCfObjectReturnToClient.Pricing = priceList;
            }

            return wCfObjectReturnToClient;
        }

        public async Task<WCFObjectReturnToClient> GetEnhancedContentIconsForSearch(SearchRequest request)
        {
            var wCfObjectReturnToClient = new WCFObjectReturnToClient { Message = "Sequential" };
            //

            if (request == null) return null;

            //var siteContextObject = request.SiteContext;

            //if (siteContextObject == null)
            //{
            //    return null;
            //}

            var btKeyHasReviewList = new List<string>();

            var productSearchResults = GetProductSearchResultsFromCache(request.UserId);

            if (productSearchResults == null)
            {
                if (!request.IsAltFormatCall.HasValue && request.SearchArguments != null)
                {
                    productSearchResults = ProductSearchController.Search(request.SearchArguments, request.Targeting.MarketType,
                        request.SearchData.SimonSchusterEnabled, request.SearchData.CountryCode, request.SearchData.ESuppliers);
                }
                else if (request.IsAltFormatCall.HasValue && request.IsAltFormatCall.Value)
                {
                    var productIdList = ProductCatalogDAO.Instance.GetRelatedProductIds(request.AltFormatCallBtKey);
                    if (productIdList != null && productIdList.Count > 0)
                    {
                        productSearchResults = ProductSearchController.SearchByIdWithoutAnyRules(productIdList);

                        if (productSearchResults != null && productSearchResults.Items != null && productSearchResults.Items.Count > 0)
                        {
                            //CalculatePricePresenter.CalculatePrice(product);
                            CalculatePrice(productSearchResults, request.UserId, request.SearchData.ESuppliers,
                                request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData);
                        }
                    }
                }
            }

            if (productSearchResults == null)
            {
                //Logger.Write("GetEnhancedContentIconsForSearch - Debug", "productSearchResults == null");
                return wCfObjectReturnToClient;
            }

            foreach (var item in productSearchResults.Items)
            {
                if (item.HasReview)
                {
                    btKeyHasReviewList.Add(item.BTKey);
                }
            }

            if (request.ShowEnhancedContentIconsForSearch)
            {
                var reviewInd = GetProductReviewIndicator(btKeyHasReviewList, request.Targeting.OrgId);

                wCfObjectReturnToClient.ContentIndicator = btKeyHasReviewList.Count > 0
                    ? reviewInd == null ? new List<SiteTermObject>() : reviewInd.Data
                    : new List<SiteTermObject>();
            }

            return wCfObjectReturnToClient;
        }

        public async Task<WCFObjectReturnToClient> GetUserEditableFieldsForSearch(SearchRequest request)
        {
            var wCfObjectReturnToClient = new WCFObjectReturnToClient { Message = "Sequential" };
            //

            if (request == null) return null;

            //var siteContextObject = request.SiteContext;

            //if (siteContextObject == null)
            //{
            //    return null;
            //}

            var btKeyList = new List<string>();

            var productSearchResults = GetProductSearchResultsFromCache(request.UserId);

            if (productSearchResults == null)
            {
                if (!request.IsAltFormatCall.HasValue && request.SearchArguments != null)
                {
                    productSearchResults = ProductSearchController.Search(request.SearchArguments, request.Targeting.MarketType,
                        request.SearchData.SimonSchusterEnabled, request.SearchData.CountryCode, request.SearchData.ESuppliers);
                }
                else if (request.IsAltFormatCall.HasValue && request.IsAltFormatCall.Value)
                {
                    var productIdList = ProductCatalogDAO.Instance.GetRelatedProductIds(request.AltFormatCallBtKey);
                    if (productIdList != null && productIdList.Count > 0)
                    {
                        productSearchResults = ProductSearchController.SearchByIdWithoutAnyRules(productIdList);

                        if (productSearchResults != null && productSearchResults.Items != null && productSearchResults.Items.Count > 0)
                        {
                            //CalculatePricePresenter.CalculatePrice(product);
                            CalculatePrice(productSearchResults, request.UserId, request.SearchData.ESuppliers,
                                request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData);
                        }
                    }
                }
            }

            if (productSearchResults == null)
            {
                //Logger.Write("GetUserEditableFieldsForSearch - Debug", "productSearchResults == null");
                return wCfObjectReturnToClient;
            }

            foreach (var item in productSearchResults.Items)
            {
                btKeyList.Add(item.BTKey);
            }

            if (request.ShowUserEditableFieldsForSearch)
            {
                //var cartManager = CartContext.Current.GetCartManagerForUser(SiteContext.UserId);
                var notes = new List<NoteClientObject>();
                //if (cartManager != null)
                //{
                var cart = await CartDAOManager.Instance.GetPrimaryCartAsync(request.UserId);// cartManager.GetPrimaryCart();
                if (cart == null)
                {
                    notes.AddRange(btKeyList.Select(btkey => new NoteClientObject { BTKey = btkey }));
                }
                else
                {
                    notes = await GridDAOManager.Instance.GetNotesByBTKeysAsync(cart.CartId, request.UserId, btKeyList);
                    if (notes != null)
                    {
                        //notes = GridHelper.GetNotes(cart.CartId, siteContextObject.UserId, btKeyList);
                        foreach (var btkey in btKeyList)
                        {
                            var i = notes.FindIndex(note => note.BTKey == btkey);
                            if (i < 0)
                            {
                                notes.Add(new NoteClientObject { BTKey = btkey });
                            }
                        }
                    }
                }
                //}

                wCfObjectReturnToClient.NotesList = notes;
            }

            return wCfObjectReturnToClient;
        }

        public WCFObjectReturnToClient GetInventoryForSearch(SearchRequest request)
        {
            var wCfObjectReturnToClient = new WCFObjectReturnToClient { Message = "Sequential" };
            var inventoryStatusArgList = new List<SearchResultInventoryStatusArg>();

            var productSearchResults = GetProductSearchResultsFromCache(request.UserId);

            if (productSearchResults == null)
            {
                if (!request.IsAltFormatCall.HasValue && request.SearchArguments != null)
                {
                    productSearchResults = ProductSearchController.Search(request.SearchArguments, request.Targeting.MarketType,
                        request.SearchData.SimonSchusterEnabled, request.SearchData.CountryCode, request.SearchData.ESuppliers);
                }
                else if (request.IsAltFormatCall.HasValue && request.IsAltFormatCall.Value)
                {
                    var productIdList = ProductCatalogDAO.Instance.GetRelatedProductIds(request.AltFormatCallBtKey);
                    if (productIdList != null && productIdList.Count > 0)
                    {
                        productSearchResults = ProductSearchController.SearchByIdWithoutAnyRules(productIdList);

                        if (productSearchResults != null && productSearchResults.Items != null && productSearchResults.Items.Count > 0)
                        {
                            //CalculatePricePresenter.CalculatePrice(product);
                            CalculatePrice(productSearchResults, request.UserId, request.SearchData.ESuppliers,
                                request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData);
                        }
                    }
                }
            }

            if (productSearchResults == null)
            {
                //Logger.Write("GetInventoryForSearch - Debug", "productSearchResults == null");
                return wCfObjectReturnToClient;
            }

            foreach (var item in productSearchResults.Items)
            {
                inventoryStatusArgList.Add(GetSearchResultInventoryStatusArg(item, request.MarketTypeString));
            }

            if (request.ShowInventoryForSearch) //SiteContext.Current.ShowInventoryForSearch)
            {
                var inventoryHelper = InventoryHelper4MongoDb.GetInstance(userId: request.UserId,
                    countryCode: request.SearchData.CountryCode, marketType: request.Targeting.MarketType, orgId: request.Targeting.OrgId);

                //Inventory Result
                wCfObjectReturnToClient.InventoryResultsList.AddRange(
                    inventoryHelper.GetInventoryResultsForMultipleItems(inventoryStatusArgList));

                //Inventory Status
                wCfObjectReturnToClient.InventoryStatus = inventoryHelper.InventoryStatusList;

            }

            return wCfObjectReturnToClient;
        }

        public async Task<WCFObjectReturnToClient> GetDupCheckForSearch(SearchRequest request)
        {
            var wCfObjectReturnToClient = new WCFObjectReturnToClient { Message = "Sequential" };
            //

            //var siteContextObject = request.SiteContext;

            //if (siteContextObject == null)
            //{
            //    return null;
            //}

            var btKeyList = new List<string>();
            var btEKeyList = new List<string>();

            var productSearchResults = GetProductSearchResultsFromCache(request.UserId);

            if (productSearchResults == null)
            {
                if (!request.IsAltFormatCall.HasValue && request.SearchArguments != null)
                {
                    productSearchResults = ProductSearchController.Search(request.SearchArguments, request.Targeting.MarketType,
                        request.SearchData.SimonSchusterEnabled, request.SearchData.CountryCode, request.SearchData.ESuppliers);
                }
                else if (request.IsAltFormatCall.HasValue && request.IsAltFormatCall.Value)
                {
                    var productIdList = ProductCatalogDAO.Instance.GetRelatedProductIds(request.AltFormatCallBtKey);
                    if (productIdList != null && productIdList.Count > 0)
                    {
                        productSearchResults = ProductSearchController.SearchByIdWithoutAnyRules(productIdList);

                        if (productSearchResults != null && productSearchResults.Items != null && productSearchResults.Items.Count > 0)
                        {
                            //CalculatePricePresenter.CalculatePrice(product);
                            CalculatePrice(productSearchResults, request.UserId, request.SearchData.ESuppliers,
                                request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData);
                        }
                    }
                }
            }

            if (productSearchResults == null)
            {
                //Logger.Write("GetDupCheckForSearch - Debug", "productSearchResults == null");
                return wCfObjectReturnToClient;
            }

            foreach (var item in productSearchResults.Items)
            {
                btKeyList.Add(item.BTKey);
                btEKeyList.Add(item.BTEKey);
            }

            if (request.ShowDupCheckCartsForSearch || request.ShowDupCheckOrdersForSearch)
            {
                var userProfile = ProfileService.Instance.GetUserById(request.UserId);
                //var userProfile = CSObjectProxy.GetUserProfileForSearchResult();
                var defaultDuplicateOrders = userProfile.DefaultDuplicateOrders;
                var defaultDuplicateCarts = userProfile.DefaultDuplicateCarts;

                var dupResult = await GetProductDuplicateIndicator(btKeyList, btEKeyList, string.Empty,
                    request.ShowDupCheckCartsForSearch &&
                    !string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase),
                    request.ShowDupCheckOrdersForSearch &&
                    !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase), request.UserId,
                            request.DefaultDownloadedCarts, request.Targeting.OrgId);

                wCfObjectReturnToClient.Duplicate = dupResult.Data;
            }

            return wCfObjectReturnToClient;
        }

        public PricingAndPromoForAltFormatsResponse GetPricingAndPromoForAltFormats(PricingForProductsRequest request)
        {
            var response = new PricingAndPromoForAltFormatsResponse();

            if (!string.IsNullOrEmpty(request.UserId) && request.Products != null && request.Products.Count() > 0)
            {
                var pricingClientArg = new List<PricingClientArg>();
                var promotionClientArg = new List<PromotionClientArg>();

                foreach (var item in request.Products)
                {
                    // promotion arg
                    var promotionArg = new PromotionClientArg
                    {
                        BTKey = item.BTKey,
                        Catalog = item.Catalog,
                        ProductType = item.ProductType
                    };
                    promotionClientArg.Add(promotionArg);

                    // pricing arg
                    var pricingArg = new PricingClientArg
                    {
                        AcceptableDiscount = item.AcceptableDiscount,
                        BTGTIN = item.BTGTIN,
                        ISBN = item.ISBN,
                        BTKey = item.BTKey,
                        BTUPC = item.BTUPC,
                        Catalog = item.Catalog,
                        HasReturn = item.HasReturn.ToString(),
                        ListPrice = item.ListPrice.ToString(),
                        PriceKey = item.PriceKey,
                        ProductLine = item.ProductLine,
                        ProductType = item.ProductType,
                        ProductFormat = item.ProductFormat,
                        ESupplier = item.ESupplier,
                        PurchaseOption = item.PurchaseOption.Trim()
                    };
                    pricingClientArg.Add(pricingArg);
                }

                if (request.ShowExpectedDiscountPriceForSearch)
                {
                    response.Pricing = GetProductPricing(request.Products, request.UserId, request.ESuppliers, request.IsHideNetPriceDiscountPercentage,
                                                            request.Targeting, request.AccountPricingData).Data;

                    // we will bind Promotion data in UI Server side due to the limitation of Marketing Context
                    response.Promotion = GetProductPromotion(promotionClientArg, request.Targeting);
                }
                else
                {
                    var priceList = GetProductPricingForGale(pricingClientArg, request.UserId, request.ESuppliers,
                                        request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData);
                    foreach (var price in priceList)
                    {
                        price.DisPercent = price.Price = string.Empty;
                    }
                    response.Pricing = priceList;
                }
            }

            return response;
        }

        public EnhancedContentIconsForAltFormatsResponse GetEnhancedContentIconsForAltFormats(EnhancedContentIconsForAltFormatsRequest request)
        {
            var wCfObjectReturnToClient = new EnhancedContentIconsForAltFormatsResponse
            {
                ContentIndicator = new List<SiteTermObject>()
            };

            if (request != null && request.HasReviewBTKeyList != null && request.HasReviewBTKeyList.Count > 0)
            {
                var reviewInd = GetProductReviewIndicator(request.HasReviewBTKeyList, request.OrgId);

                if (reviewInd != null)
                    wCfObjectReturnToClient.ContentIndicator = reviewInd.Data;
            }

            return wCfObjectReturnToClient;
        }

        public async Task<UserEditableFieldsForAltFormatsResponse> GetUserEditableFieldsForAltFormats(UserEditableFieldsForAltFormatsRequest request)
        {
            var response = new UserEditableFieldsForAltFormatsResponse
            {
                NotesList = new List<NoteClientObject>()
            };

            if (request.BTKeyList != null && request.BTKeyList.Count > 0)
            {
                var notes = new List<NoteClientObject>();
                var cart = await CartDAOManager.Instance.GetPrimaryCartAsync(request.UserId);
                if (cart == null)
                {
                    notes.AddRange(request.BTKeyList.Select(btkey => new NoteClientObject { BTKey = btkey }));
                }
                else
                {
                    notes = await GridDAOManager.Instance.GetNotesByBTKeysAsync(cart.CartId, request.UserId, request.BTKeyList);
                    if (notes != null)
                    {
                        foreach (var btkey in request.BTKeyList)
                        {
                            var i = notes.FindIndex(note => note.BTKey == btkey);
                            if (i < 0)
                            {
                                notes.Add(new NoteClientObject { BTKey = btkey });
                            }
                        }
                    }
                }

                response.NotesList = notes;
            }

            return response;
        }

        public InventoryForAltFormatsResponse GetInventoryForAltFormats(InventoryForAltFormatsRequest request)
        {
            var response = new InventoryForAltFormatsResponse
            {
                InventoryResultsList = new List<InventoryResults>(),
                InventoryStatus = new List<SiteTermObject>()
            };

            if (request.InventoryArgs != null)
            {
                var inventoryHelper = InventoryHelper4MongoDb.GetInstance(userId: request.UserId,
                    countryCode: request.CountryCode, marketType: request.MarketType, orgId: request.OrgId);

                // Inventory Results
                var inventoryResults = inventoryHelper.GetInventoryResultsForMultipleItems(request.InventoryArgs);
                response.InventoryResultsList.AddRange(inventoryResults);

                // Inventory Status
                response.InventoryStatus = inventoryHelper.InventoryStatusList;
            }

            return response;
        }

        public async Task<List<SiteTermObject>> GetDupCheckForAltFormats(DupCheckForAltFormatsRequest request)
        {
            var response = new List<SiteTermObject>();
            if (request.ShowDupCheckCartsForSearch || request.ShowDupCheckOrdersForSearch)
            {
                var userProfile = ProfileService.Instance.GetUserById(request.UserId);
                var defaultDuplicateOrders = userProfile.DefaultDuplicateOrders;
                var defaultDuplicateCarts = userProfile.DefaultDuplicateCarts;
                var isRequiredCheckDupCarts = (request.ShowDupCheckCartsForSearch && !string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase));
                var isRequiredCheckDupOrder = (request.ShowDupCheckOrdersForSearch && !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase));
                var dupResult = await GetProductDuplicateIndicator(request.BTKeyList, request.BTEKeyList, string.Empty,
                                                                   isRequiredCheckDupCarts, isRequiredCheckDupOrder, request.UserId,
                                                                   request.DefaultDownloadedCarts, request.OrgId);
                if (dupResult != null && dupResult.Data != null)
                {
                    foreach (var item in dupResult.Data)
                    {
                        // remove cb div element for appending Series icon
                        if (!string.IsNullOrEmpty(item.Value))
                            item.Value = item.Value.Replace(ProductSupportedHtmlTag.DivCb, "");
                    }
                }

                response = dupResult.Data;
            }

            return response;
        }

        public async Task<ProductAltFormatsResponse> GetProductAltFormats(ProductAltFormatsRequest request)
        {
            var responseData = new ProductAltFormatsResponse();
            List<string> altFormatBTKeys;
            var userId = request.UserId;
            var user = ProfileService.Instance.GetUserById(userId);
            SortExpression sortExpression = null;
            if (user == null)
            {
                sortExpression = new SortExpression { SortField = SearchFieldNameConstants.format, SortDirection = SortDirection.Ascending };
            }
            else
            {
                if (request.UserContext.FromCartDetailPage)
                {
                    sortExpression = new SortExpression
                    {
                        SortField = user.AltFormatCartSortBy,
                        SortDirection = user.AltFormatCartSortOrder == SortDirection.Descending.ToString() ? SortDirection.Descending : SortDirection.Ascending
                    };
                }
                else
                {
                    sortExpression = new SortExpression
                    {
                        SortField = user.AltFormatSearchSortBy,
                        SortDirection = user.AltFormatSearchSortOrder == SortDirection.Descending.ToString() ? SortDirection.Descending : SortDirection.Ascending
                    };
                }
            }

            if (request.RemainingBTKeys != null && request.RemainingBTKeys.Count > 0)
                altFormatBTKeys = request.RemainingBTKeys;
            else
                altFormatBTKeys = ProductCatalogDAO.Instance.GetRelatedProductIds(request.BTKey, sortExpression);
            
            if (altFormatBTKeys != null && altFormatBTKeys.Count > 0)
            {
                var requestContext = request.UserContext;
                var marketType = requestContext.MarketType;
                var maxItemNumber = request.MaxItemNumber > 0 ? request.MaxItemNumber : altFormatBTKeys.Count;
                int searchBatchSize = 30;
                
                var searchArg = new SearchByBTKeysArgument
                {
                    BTKeyList = altFormatBTKeys,
                    PageSize = maxItemNumber,
                    MarketType = marketType,
                    ESuppliers = requestContext.ESuppliers,
                    SimonSchusterEnabled = requestContext.SimonSchusterEnabled,
                    CountryCode = requestContext.CountryCode,
                    IncludeProductFilter = true,
                    SortExpression = sortExpression,
                    UserId = userId
                };

                // search products from FAST by batch of btkeys
                var searchItems = SearchBTKeysByBatch(searchArg, searchBatchSize);

                //if (product == null || product.Items == null || product.Items.Count == 0)
                //{
                //    result.Status = AppServiceStatus.Fail;
                //    return result;
                //}
                
                if (searchItems.Count > 0)
                {
                    var resultBTKeys = new List<string>();

                    // get product qty in Primary Cart
                    var productsWithQty = new Dictionary<string, int>();
                    if (!string.IsNullOrEmpty(request.PrimaryCartId))
                    {
                        var btKeys = searchItems.Select(r => r.BTKey).ToList();
                        var cartManager = new CartManager(userId);
                        productsWithQty = cartManager.GetQuantitiesByBtKeys(request.PrimaryCartId, btKeys);
                    }

                    // convert to response item
                    var responseItems = new List<ProductAltFormatItem>();
                    var orgEntity = BT.TS360API.ExternalServices.ProfileService.Instance.GetOrganizationById(requestContext.Targeting.OrgId);

                    //Get PPC Duplicates for Search Titles
                    var _ppcDuplicates = await BT.TS360API.Services.Common.Helper.CommonHelper.CheckSearchResultsForPPCDuplicates(searchItems, requestContext.Targeting.OrgId, request.isOrgPPCEnabled);
                    
                    foreach (var searchItem in searchItems)
                    {
                        resultBTKeys.Add(searchItem.BTKey);
                        var productInPrimaryCart = productsWithQty.FirstOrDefault(r => r.Key == searchItem.BTKey);
                        var allowAddToPrimaryCart = !string.IsNullOrEmpty(request.PrimaryCartId) && string.IsNullOrEmpty(productInPrimaryCart.Key);
                        int? qty = !string.IsNullOrEmpty(productInPrimaryCart.Key) ? productInPrimaryCart.Value : (int?)null;
                        var isMakerspace = CommonHelper.IsMakerspaceProductFormat(searchItem.ProductFormat, searchItem.MerchCategory);

                        bool tempIsPPCDup = false;
                        if (request.isOrgPPCEnabled && _ppcDuplicates != null && _ppcDuplicates.Count > 0)
                            tempIsPPCDup = _ppcDuplicates[searchItem.BTKey];

                        var responseItem = new ProductAltFormatItem
                        {
                            BTKey = searchItem.BTKey,
                            BTEKey = searchItem.BTEKey,
                            ISBN = searchItem.ISBN,
                            ISBN10 = searchItem.ISBN10,
                            ISBNLookUpLink = ProductSearchController.CreateIsbnLookupLink(searchItem.ISBN, searchItem.ISBN10, requestContext.Targeting.OrgId, orgEntity),
                            UPCLookUpLink = ProductSearchController.CreateUpcLookupLink(searchItem.Upc, requestContext.Targeting.OrgId, orgEntity),
                            ProductFormat = isMakerspace ? ProductFormatConstants.Book_Makerspace : searchItem.ProductFormat,
                            //FormatIconPath = searchItem.FormatIconPath,
                            FormatIconType = Path.GetFileNameWithoutExtension(searchItem.FormatIconPath),
                            IncludedFormatClass = searchItem.IncludedFormatClass,
                            ESupplier = searchItem.ESupplier,
                            FormDetails = searchItem.FormDetails,
                            Title = searchItem.Title,
                            Upc = searchItem.Upc,
                            CPSIAMessage = searchItem.CPSIAMessage,
                            Author = searchItem.Author,
                            Publisher = searchItem.Publisher,
                            PublishDateText = searchItem.PublishDate.ToString(CommonConstants.DefaultDateTimeFormat),
                            LabelOrStudio = searchItem.Label,
                            StreetDate = searchItem.StreetDate,
                            PreOrderDate = searchItem.PreOrderDate,
                            LastUpdated = searchItem.LastUpdated.ToString(CommonConstants.DefaultDateTimeFormat),
                            Edition = searchItem.Edition,
                            ReportCode = searchItem.ReportCode,
                            LanguageLiteral = searchItem.LanguageLiteral,
                            LanguageLiteralForUI = searchItem.LanguageLiteralForUI,
                            ListPrice = searchItem.ListPrice,
                            ListPriceText = searchItem.ListPriceText,
                            PurchaseOption = searchItem.PurchaseOption,
                            ProductType = searchItem.ProductType,
                            GTIN = searchItem.GTIN,
                            Catalog = searchItem.Catalog,
                            PriceKey = searchItem.PriceKey,
                            HasReview = searchItem.HasReview,
                            ProductLine = searchItem.ProductLine,
                            HasReturn = searchItem.HasReturn,
                            AcceptableDiscount = searchItem.AcceptableDiscount,
                            MerchCategory = searchItem.MerchCategory,
                            SupplierCode = searchItem.SupplierCode,
                            BlockedExportCountryCodes = searchItem.BlockedExportCountryCodes,
                            AllowAddToPrimaryCart = allowAddToPrimaryCart,
                            IsFollettbound = CommonHelper.IsFollettBoundProduct(searchItem.ProductLine, searchItem.SupplierCode),
                            IsPPCDup = tempIsPPCDup,
                            Quantity = qty,
                            ImageUrl = ContentCafeHelper.GetJacketImageUrl(searchItem.ISBN, ImageSize.Small, searchItem.HasJacket)
                        };

                        var urlObject = responseItem.ISBNLookUpLink;
                        if (urlObject != null)
                        {
                            if (!string.IsNullOrEmpty(urlObject.ISBN13Link) || !string.IsNullOrEmpty(urlObject.ISBN10Link))
                            {
                                if (!string.IsNullOrEmpty(urlObject.ISBN13Link))
                                {
                                    urlObject.ISBN13Link = string.Format("javascript:OpenProductLookup('{0}','{1}');",
                                     urlObject.ISBN13Link,
                                     ProductLookupLinkConstant.OpenProductLookup);
                                }

                                if (!string.IsNullOrEmpty(urlObject.ISBN10Link))
                                {
                                    urlObject.ISBN10Link = string.Format("javascript:OpenProductLookup('{0}','{1}');",
                                     urlObject.ISBN10Link,
                                     ProductLookupLinkConstant.OpenProductLookup);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(responseItem.UPCLookUpLink))
                        {
                            responseItem.UPCLookUpLink = string.Format("javascript:OpenProductLookup('{0}','{1}');",
                                responseItem.UPCLookUpLink,
                                ProductLookupLinkConstant.OpenProductLookup);
                        }

                        responseItems.Add(responseItem);
                    }

                    // define remaining btkeys to response
                    if (maxItemNumber < altFormatBTKeys.Count && resultBTKeys.Count() == maxItemNumber)
                    {
                        var remainingBTKeys = altFormatBTKeys.Where(r => !resultBTKeys.Contains(r));
                        responseData.RemainingBTKeys = remainingBTKeys.ToList();
                    }

                    responseData.ProductSearchItems = responseItems;
                }
            }

            return responseData;
        }

        private IList<ProductSearchResultItem> SearchBTKeysByBatch(SearchByBTKeysArgument searchArg, int batchSize)
        {
            var resultItems = new List<ProductSearchResultItem>();

            if (searchArg != null)
            {
                var btKeysArg = searchArg.BTKeyList.ToList();   //clone
                var maxResultCount = searchArg.PageSize;
                bool isNextBatchNeeded = false;

                do
                {
                    searchArg.BTKeyList = btKeysArg.Take(batchSize).ToList();

                    // search products from FAST by batch of btkeys
                    var searchProducts = ProductSearchController.SearchByBTKeys(searchArg);

                    // add items to results
                    if (searchProducts != null && searchProducts.Items != null)
                    {
                        var addingCount = maxResultCount - resultItems.Count;
                        resultItems.AddRange(searchProducts.Items.Take(addingCount));
                    }

                    if (resultItems.Count < maxResultCount)
                    {
                        btKeysArg = btKeysArg.Skip(batchSize).ToList();

                        isNextBatchNeeded = (btKeysArg.Count > 0);
                    }
                    else
                    {
                        isNextBatchNeeded = false;
                    }
                } while (isNextBatchNeeded);
            }

            return resultItems;
        }

        private List<PricingReturn4ClientObject> GetProductPricingForGale(List<PricingClientArg> pricingArgList, string userId,
             string[] ESuppliers, bool hideNetPriceDiscountPercentage, TargetingValues targeting, AccountInfoForPricing accountPricingData)
        {
            try
            {
                //var siteContext = SiteContext.Current;
                var marketType = targeting.MarketType ?? MarketType.Any;
                var audienceType = CommonHelper.Instance.ConvertAudienceTypeAsString(targeting.AudienceType);
                var galeLiteral = CommonHelper.GetESupplierTextFromSiteTerm(AccountType.GALEE.ToString());
                //            
                var listBasketLineItemUpdated = new List<BasketLineItemUpdated>();
                var realtimePricingHelper = new RealTimePricingHelper();
                foreach (var pricingItem in pricingArgList)
                {
                    if (string.Compare(pricingItem.ESupplier, galeLiteral, StringComparison.OrdinalIgnoreCase) != 0)
                        continue;
                    pricingItem.ToUpdateListPrice = true.ToString();
                    //get account info
                    //var accountInfo = realtimePricingHelper.GetAccountInfoForPricing(pricingItem.ProductType, pricingItem.ESupplier, string.Empty, 
                    //    siteContext.EnableProcessingCharges);
                    var accountInfo = realtimePricingHelper.GetAccountInfoForPricing(pricingItem.ProductType, pricingItem.ESupplier,
                        string.Empty, userId, accountPricingData);
                    if (accountInfo == null)
                        continue;
                    //                
                    var lineItemUpdated = new BasketLineItemUpdated
                    {
                        ISBN = pricingItem.ISBN,
                        BTKey = pricingItem.BTKey,
                        SoldToId = userId,
                        AccountId = accountInfo.AccountId,
                        ProductType = pricingItem.ProductType,
                        TotalLineQuantity = 1,
                        TotalOrderQuantity = 1,
                        PriceKey = pricingItem.PriceKey,
                        AccountPricePlan = accountInfo.AccountPricePlanId,
                        ProductLine = pricingItem.ProductLine,
                        ReturnFlag = pricingItem.HasReturn.ToLower() == "true",
                        AccountERPNumber = accountInfo.ErpAccountNumber,
                        PrimaryWarehouse = accountInfo.PrimaryWarehouseCode,
                        ProductCatalog = pricingItem.Catalog,
                        MarketType = marketType.ToString(),
                        AudienceType = audienceType,
                        ESupplier = pricingItem.ESupplier,
                        EMarket = accountInfo.EMarketType,
                        ETier = accountInfo.ETier,
                        ProductPriceChangedIndicator = true,
                        ContractChangedIndicator = true,
                        PromotionChangedIndicator = false,
                        PromotionActiveIndicator = false,
                        QuantityChanged = true,
                        IsHomeDelivery = accountInfo.IsHomeDelivery,
                        Upc = pricingItem.BTUPC,
                        AcceptableDiscount = pricingItem.AcceptableDiscount,
                        PurchaseOption = pricingItem.PurchaseOption, // MUPO+processing charger
                        NumberOfBuildings = accountInfo.BuildingNumbers, // MUPO+processing charger
                        ProcessingCharges = accountInfo.ProcessingCharges, // MUPO+processing charger
                        SalesTax = accountInfo.SalesTax,
                        IsVIPAccount = accountInfo.IsVIPAccount
                    };
                    decimal listPrice;
                    if (Decimal.TryParse(pricingItem.ListPrice, out listPrice))
                    {
                        lineItemUpdated.ListPrice = listPrice;
                    }
                    listBasketLineItemUpdated.Add(lineItemUpdated);
                }
                //Get Prices
                var pricingController = new PricingController();
                //var productItemPrices = pricingController.CalculatePrice(listBasketLineItemUpdated
                //                                                        , (SiteContext.Current.MarketType == BT.TS360Constants.MarketType.Retail) ? 5 : 1
                //                                                        , true);

                var productItemPrices = pricingController.CalculatePrice(listBasketLineItemUpdated
                                                                        , (marketType == MarketType.Retail) ? 5 : 1
                                                                        , targeting, hideNetPriceDiscountPercentage);
                UpdatePriceForSearchResult(productItemPrices, userId, ESuppliers);
                //            
                return BuildPricingObjectsToReturn(pricingArgList, marketType, productItemPrices, userId, ESuppliers);
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Pricing);
                return new List<PricingReturn4ClientObject>();
            }
        }

        private List<PromotionReturn4ClientObject> GetProductPromotion(List<PromotionClientArg> promotionArgList,
            TargetingValues siteContext)
        {
            var returnObjects = new List<PromotionReturn4ClientObject>();

            var listDiscount = MarketingController.GetDiscountsForMultipleItem(promotionArgList, siteContext);

            foreach (var btKeyDiscountObject in listDiscount)
            {
                var returnObject = new PromotionReturn4ClientObject { BTKey = btKeyDiscountObject.BTKey, Content = btKeyDiscountObject.DiscountName };
                returnObjects.Add(returnObject);
            }

            return returnObjects;
        }

        public async Task<AppServiceResult<List<ItemDataContract>>> CheckForAltFormatAsync(AltFormatRequest altFormatRequest)
        {
            var result = new AppServiceResult<List<ItemDataContract>> { Status = AppServiceStatus.Success };
            try
            {
                var dictionary = await ProductDAO.Instance.CheckFamilyKeysAsync(altFormatRequest.btKeys);
                var data = new List<ItemDataContract>();

                foreach (var item in dictionary)
                {
                    data.Add(new ItemDataContract
                    {
                        ItemKey = item.Key,
                        ItemValue = item.Value.ToString().ToLower(),
                    });
                }
                result.Data = data;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                //Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
                Logger.WriteLog(exception, ExceptionCategory.ItemDetails.ToString());
            }
            return result;
        }

        public AppServiceResult<AllInfoForQuickItemDetailsResponse> GetAllInfoForQuickItemDetailPage(AllInfoForQuickItemDetailsRequest request)
        {
            var result = new AppServiceResult<AllInfoForQuickItemDetailsResponse>();
            try
            {
                var dataReturn = new AllInfoForQuickItemDetailsResponse();

                //// get note
                //var btkeys = new List<string> { request.BTKey };
                //var notes = await GridHelper.GetNotes(request.CartId, userId, btkeys);
                //if (notes != null && notes.Count > 0)
                //    dataReturn.Note = notes[0];

                var resultStatus = GetInventoryStatusForItemDetails(request.UserId, request.MarketType, request.InventoryArg);
                if (resultStatus != null && resultStatus.Count == 1)
                {
                    dataReturn.InventoryStatus = resultStatus[0].Value;
                }
                else
                {
                    dataReturn.InventoryStatus = string.Empty;
                }

                dataReturn.NavBarInfo = GetCartItemDetailsNavBarInfoResponse(request.UserId, request.CartId, request.LineItemId
                                                                    , request.SearchCartCriteria);

                result.Status = AppServiceStatus.Success;
                result.Data = dataReturn;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
            }
            return result;
        }

        private List<SiteTermObject> GetInventoryStatusForItemDetails(string userId, MarketType marketType, InventoryStatusArgContract inventoryArg)
        {
            try
            {
                //var invStatusFromCache = GetInventoryStatusFromCache(inventoryArg.BTKey, inventoryArg.CartID);
                //if (invStatusFromCache != null) return invStatusFromCache;

                var siteTermObjects = new List<SiteTermObject>();
                var listArg = new List<SearchResultInventoryStatusArg>();
                //
                var inventoryStatusArg = new SearchResultInventoryStatusArg
                {
                    CatalogName = inventoryArg.CatalogName,
                    UserId = userId,
                    BTKey = inventoryArg.BTKey,
                    VariantId = "",
                    Flag = inventoryArg.ReportCode,
                    Quantity = Convert.ToDecimal("0"),
                    ProductType = inventoryArg.ProductType,
                    PublishDate = inventoryArg.PublishDate,
                    MerchandiseCategory = inventoryArg.MerchandiseCategory,
                    MarketType = marketType.ToString(),
                    PubCodeD = inventoryArg.PubCodeD,
                    ESupplier = inventoryArg.ESupplier,
                    ReportCode = inventoryArg.ReportCode
                };
                listArg.Add(inventoryStatusArg);
                siteTermObjects = InventoryHelper4MongoDb.GetInstance(inventoryArg.CartID, userId).GetInventoryStatus(listArg);
                return siteTermObjects;
            }
            catch (Exception exception)
            {
                Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
                return null;
            }
        }

        public async Task<AppServiceResult<EnhancedContentsForCartDetailsResponse>> GetEnhancedContentsForCartDetails(EnhancedContentsForCartDetailsRequest request)
        {
            var result = new AppServiceResult<EnhancedContentsForCartDetailsResponse>();
            var responseData = new EnhancedContentsForCartDetailsResponse();
            var promotionClientArg = new List<PromotionClientArg>();

            //var userProfile = ProfileService.Instance.GetUserById(ServiceRequestContext.Current.UserId);
            //string defaultDuplicateOrders = userProfile.DefaultDuplicateOrders;
            //string defaultDuplicateCarts = userProfile.DefaultDuplicateCarts;

            var requestContext = request.UserContext;
            try
            {
                var cartId = HttpUtility.UrlDecode(request.CartId);
                var userId = ServiceRequestContext.Current.UserId;

                var cartLineItem = request.LineItem;
                var searchResultItem = ConvertLineItemToProductSearchResultItem(cartLineItem);
                if (searchResultItem == null || searchResultItem.BTKey != request.BTKey || cartLineItem.Id != request.LineItemId)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = ProfileResources.UnexpectedError;
                    return result;
                }

                if (cartLineItem.IsOriginalEntryItem)
                {
                    result.Data = responseData;
                    result.Status = AppServiceStatus.Success;
                    return result;
                }

                // SettingForShowingInventoryCartDetailsPage
                if (!requestContext.ShowInventoryForCart)
                {
                    var inventoryHelper = InventoryHelper4MongoDb.GetInstance(cartId, userId);
                    var listProductArgItems = new List<SearchResultInventoryStatusArg>();
                    listProductArgItems.Add(GetSearchResultInventoryStatusArg(searchResultItem, requestContext.MarketType.ToString()));

                    var resultList = inventoryHelper.GetInventoryResultsForMultipleItems(listProductArgItems);
                    responseData.InventoryResultsList.AddRange(resultList);

                    responseData.InventoryStatus = inventoryHelper.InventoryStatusList;
                }

                // show Enhanced Content Icons
                if (!requestContext.ShowEnhancedContentIconsForCart)
                    SettingForShowEnhancedContentIconsForCartDetails(cartLineItem, request.OrgId, responseData, searchResultItem.HasReview, promotionClientArg, searchResultItem, requestContext.Targeting);

                // Pricing
                if (!requestContext.ShowExpectedDiscountPriceForCart)
                    SettingForShowExpectedDiscountPriceForCartDetailsPage(cartId, searchResultItem, cartLineItem, userId, responseData, requestContext.MarketType);

                // Product Lookup
                if (!requestContext.ShowProductLookupForCart)
                    SettingProductLookupLink(cartLineItem, requestContext.ProductLookupDeactivated, request.OrgId, responseData);

                // check Dup cart and order
                //bool isRequiredCheckDupCart = (!requestContext.ShowDupCheckCartsForCart && !string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase));
                //bool isRequiredCheckDupOrder = (!requestContext.ShowDupCheckOrdersForCart && !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase));
                //if (isRequiredCheckDupCart || isRequiredCheckDupOrder)
                //{
                //    responseData.Duplicate = (await GetProductDuplicateIndicator(new List<string> { searchResultItem.BTKey }, new List<string> { searchResultItem.BTEKey }, cartId, isRequiredCheckDupCart, isRequiredCheckDupOrder, userId, requestContext.DefaultDownloadedCarts, requestContext.CollectionAnalysisEnabled, request.OrgId)).Data;
                //}

                // Notes
                var listItemIds = new List<string> { cartLineItem.Id };
                responseData.NotesList = await GridHelper.GetNotes(cartId, userId, null, listItemIds);

                result.Data = responseData;
                result.Status = AppServiceStatus.Success;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ProfileResources.UnexpectedError;
            }
            return result;
        }

        private void SettingForShowEnhancedContentIconsForCartDetails(RequestLineItemEx lineItem, string orgId,
            EnhancedContentsForCartDetailsResponse serviceResponse, bool hasReviewFromFast, List<PromotionClientArg> promotionClientArg,
            ProductSearchResultItem item, TargetingValues targeting)
        {
            if (lineItem == null || lineItem.IsOriginalEntryItem) return;

            if (hasReviewFromFast)
            {
                var btKeyHasReviewList = new List<string> { lineItem.BTKey };
                var siteTermObjects = ProductSearchController.CheckProductReviewsFromOds(btKeyHasReviewList, orgId);

                serviceResponse.ContentIndicator = siteTermObjects;
            }

            serviceResponse.HasPawPrint = !string.IsNullOrEmpty(lineItem.ProductLine) &&
                                                  lineItem.ProductLine.Equals(SpecialProductAttributes.PawPrintsProductLine);

            serviceResponse.HasGardners = !string.IsNullOrEmpty(lineItem.SupplierCode) &&
                                                  lineItem.SupplierCode.Equals(SpecialProductAttributes.GardnersSupplierCode, StringComparison.OrdinalIgnoreCase);

            if ((!string.IsNullOrEmpty(lineItem.MerchCategory) && lineItem.MerchCategory.Equals(SpecialProductAttributes.LargePrintMerchCategory, StringComparison.OrdinalIgnoreCase))
                || (!string.IsNullOrEmpty(lineItem.Edition) && (lineItem.Edition.Equals(SpecialProductAttributes.LargePrintEdition, StringComparison.OrdinalIgnoreCase)
                     || lineItem.Edition.Equals(SpecialProductAttributes.LargePrintAltEdition, StringComparison.OrdinalIgnoreCase))))
            {
                serviceResponse.HasLargePrint = true;
            }

            promotionClientArg.Add(GetPromotionClientArg(item));
            serviceResponse.Promotion = GetProductPromotion(promotionClientArg, targeting);
        }

        private void SettingForShowExpectedDiscountPriceForCartDetailsPage(string cartId, ProductSearchResultItem item,
            RequestLineItemEx lineItem, string userId, EnhancedContentsForCartDetailsResponse serviceResponse, MarketType marketType)
        {
            if (item.IsOriginalEntryItem) return;

            if (lineItem == null) return;

            var lineItemId = lineItem.Id;
            var btKey = lineItem.BTKey;

            var cart = (new CartDAOManager()).GetCartById(cartId, userId);

            var galeLiteral = CommonHelper.GetESupplierTextFromSiteTerm(AccountType.GALEE.ToString());
            var isGaleAccountInCart = CartFrameworkHelper.IsGaleAccountInCart(cart, AccountType.GALEE.ToString());
            var isBeingPricing = CartFrameworkHelper.IsPricing(cart.CartId, userId, needToReprice: true); //cart.IsPricing(true);
            var lineItemsToReturn = new List<PricingReturn4ClientObject>();

            SetPricingListClientObject(isBeingPricing, isGaleAccountInCart, lineItemsToReturn, galeLiteral,
                lineItem.BTDiscountPercent, lineItem.ListPrice, lineItem.SalePrice, lineItem.ESupplier, lineItemId, btKey, lineItem.Quantity, marketType.ToString());

            serviceResponse.Pricing = lineItemsToReturn;
        }

        private void SettingProductLookupLink(RequestLineItemEx lineItem, bool productLookupDeactivated, string orgId, EnhancedContentsForCartDetailsResponse serviceResponse)
        {
            if (!string.IsNullOrEmpty(lineItem.ISBN) || !string.IsNullOrEmpty(lineItem.Upc))
            {
                string UPCLink = ProductSearchController.CreateUpcLookupLink(lineItem.Upc, orgId);
                var urlObject = ProductSearchController.CreateIsbnLookupLink(lineItem.ISBN, lineItem.ISBN10, orgId);
                if (lineItem.ProductType == ProductTypeConstants.Book || lineItem.ProductType == ProductTypeConstants.eBook || UPCLink == ProductLookupLinkConstant.UPCUseISBN)
                {
                    serviceResponse.UPCProductLookupLink = "";
                    if (urlObject != null)
                    {
                        if (lineItem.ProductType == ProductTypeConstants.Book || lineItem.ProductType == ProductTypeConstants.eBook)
                        {
                            if (productLookupDeactivated)
                            {
                                serviceResponse.ProductLookupLink13HasOnClick = false;
                                serviceResponse.ProductLookupLink13 = "";
                                serviceResponse.ProductLookupLinkHasOnClick = false;
                                serviceResponse.ProductLookupLink = "";
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(urlObject.ISBN13Link) || !string.IsNullOrEmpty(urlObject.ISBN10Link))
                                {
                                    if (!string.IsNullOrEmpty(urlObject.ISBN13Link))
                                    {
                                        serviceResponse.ProductLookupLink13HasOnClick = true;
                                        serviceResponse.ProductLookupLink13 = string.Format("OpenProductLookup('{0}','{1}');",
                                     urlObject.ISBN13Link,
                                     ProductLookupLinkConstant.OpenProductLookup);
                                    }

                                    if (!string.IsNullOrEmpty(urlObject.ISBN10Link))
                                    {
                                        serviceResponse.ProductLookupLinkHasOnClick = true;
                                        serviceResponse.ProductLookupLink = string.Format("OpenProductLookup('{0}','{1}');",
                                     urlObject.ISBN10Link,
                                     ProductLookupLinkConstant.OpenProductLookup);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(urlObject.ISBN13Link) || !string.IsNullOrEmpty(urlObject.ISBN10Link))
                            {
                                if (!string.IsNullOrEmpty(urlObject.ISBN13Link))
                                {
                                    serviceResponse.ProductLookupLink13HasOnClick = true;
                                    serviceResponse.ProductLookupLink13 = string.Format("OpenProductLookup('{0}','{1}');",
                                 urlObject.ISBN13Link,
                                 ProductLookupLinkConstant.OpenProductLookup);
                                }

                                if (!string.IsNullOrEmpty(urlObject.ISBN10Link))
                                {
                                    serviceResponse.ProductLookupLinkHasOnClick = true;
                                    serviceResponse.ProductLookupLink = string.Format("OpenProductLookup('{0}','{1}');",
                                 urlObject.ISBN10Link,
                                 ProductLookupLinkConstant.OpenProductLookup);
                                }
                            }
                        }
                    }
                    else
                    {
                        serviceResponse.ProductLookupLink13HasOnClick = false;
                        serviceResponse.ProductLookupLink13 = "";
                        serviceResponse.ProductLookupLinkHasOnClick = false;
                        serviceResponse.ProductLookupLink = "";
                    }
                }
                else
                {
                    serviceResponse.ProductLookupLink13HasOnClick = false;
                    serviceResponse.ProductLookupLink13 = "";
                    serviceResponse.ProductLookupLinkHasOnClick = false;
                    serviceResponse.ProductLookupLink = "";

                    if (UPCLink != ProductLookupLinkConstant.UPCDeactivated)
                    {
                        if (!string.IsNullOrEmpty(UPCLink))
                        {
                            serviceResponse.UPCProductLookupLink = string.Format("OpenProductLookup('{0}','{1}');",
                             UPCLink,
                             ProductLookupLinkConstant.OpenProductLookup);
                        }
                        else
                            serviceResponse.UPCProductLookupLink = "";
                    }
                    else
                    {                        
                        serviceResponse.UPCProductLookupLink = "";
                    }
                }
            }
        }

        public async Task<AppServiceResult<AdditionalCartLineItemsResponse>> GetAdditionalCartLineItemsInfo(AdditionalCartLineItemsInfoRequest request)
        {
            var result = new AppServiceResult<AdditionalCartLineItemsResponse> { Status = AppServiceStatus.Success };
            try
            {
                var response = new AdditionalCartLineItemsResponse();
                //
                var btKeyList = new List<string>();
                var btKeyHasReviewList = new List<string>();
                var btEKeyList = new List<string>();
                var inventoryStatusArgList = new List<SearchResultInventoryStatusArg>();
                var promotionClientArg = new List<PromotionClientArg>();
                var btKeyInventoryList = new List<InventoryStockArg>();
                var btEKeyInventoryList = new List<InventoryStockArg>();

                var userId = ServiceRequestContext.Current.UserId;
                var requestContext = request.UserContext;
                var cart = request.CartSummary;
                var targetingValues = requestContext.Targeting;
                var countryCode = requestContext.CountryCode;
                var marketType = requestContext.MarketType;
                var orgId = requestContext.OrgId;

                if (cart != null && !string.IsNullOrEmpty(cart.CartId))
                {
                    var cartId = cart.CartId;
                    //var userProfile = ProfileService.Instance.GetUserById(userId);
                    //if (userProfile == null) return result;

                    //string defaultDuplicateOrders = userProfile.DefaultDuplicateOrders;
                    //string defaultDuplicateCarts = userProfile.DefaultDuplicateCarts;

                    var lineItems = request.LineItems;
                    if (lineItems != null && lineItems.Count() > 0)
                    {
                        var isBeingPricing = false;
                        //TFS15595
                        if (requestContext.ShowExpectedDiscountPriceForCart)
                        {
                            var needToReprice = true;
                            var isPricing = CartDAOManager.IsCartPricing(cartId, userId);
                            if (isPricing)
                            {
                                if (needToReprice)
                                {
                                    CartFrameworkHelper.CalculatePrice(cartId, targetingValues);
                                }
                                isBeingPricing = true;
                            }
                        }

                        var lineItemsToReturn = new List<PricingReturn4ClientObject>();
                        var listProductItems = new List<ProductSearchResultItem>();

                        var eBooklistToIgnoreInvCheck = new List<string>
                           {
                               BT.TS360Constants.ProductFormatConstants.EBook_Digital_Download,
                               BT.TS360Constants.ProductFormatConstants.EBook_Digital_Download_Online,
                               BT.TS360Constants.ProductFormatConstants.EBook_Digital_Online,
                               BT.TS360Constants.ProductFormatConstants.EBook_Downloadable_Audio
                           };

                        foreach (var cartLineItem in lineItems)
                        {
                            var item = ConvertLineItemToProductSearchResultItem(cartLineItem);

                            btKeyList.Add(item.BTKey);
                            btEKeyList.Add(item.BTEKey);
                            if (item.HasReview)
                            {
                                btKeyHasReviewList.Add(item.BTKey);
                            }

                            // Inventory arg list
                            var inventoryStatusArg = GetSearchResultInventoryStatusArg(item, requestContext.MarketType.ToString());
                            inventoryStatusArgList.Add(inventoryStatusArg);

                            if (!item.IsOriginalEntryItem)  //if IsNormalLineItem
                            {
                                var galeLiteral = CommonHelper.GetESupplierTextFromSiteTerm(AccountType.GALEE.ToString());
                                //SET Line Items Pricing
                                SetPricingListClientObject(isBeingPricing, cart.HasGaleAccount, lineItemsToReturn,
                                                           cartLineItem, galeLiteral, requestContext.MarketType.ToString());
                            }

                            if (!string.IsNullOrEmpty(item.Catalog))
                                promotionClientArg.Add(GetPromotionClientArg(item));

                            var prodType = CommonHelper.GetProductType(item.ProductType);
                            if (prodType == ProductType.Book && item.IsOriginalEntryItem) // original entry
                                continue;

                            //tfs15595
                            if (request.IsCheckCartInventory && requestContext.ShowInventoryForCart)
                            {
                                var isa = new InventoryStockArg(item.BTKey, item.Upc, item.Quantity);
                                if (prodType == ProductType.Book)
                                {
                                    // TFS 28271: ignore ebooks when checking cart inventory
                                    if (!eBooklistToIgnoreInvCheck.Contains(item.ProductFormat, StringComparer.CurrentCultureIgnoreCase))
                                        btKeyInventoryList.Add(isa);
                                }
                                else
                                    btEKeyInventoryList.Add(isa);
                            }
                            else
                            {
                                listProductItems.Add(item);
                            }
                        }

                        // get inventory data & status
                        if (requestContext.ShowInventoryForCart)
                        {
                            var mongoDbInstance = InventoryHelper4MongoDb.GetInstance(cartId, userId, marketType, countryCode, orgId);

                            if (request.IsCheckCartInventory)
                            {
                                // Get Realtime Inventory list from WCF BTStockCheckServices
                                GetStockCheckInventoryStatus(btEKeyInventoryList, userId, cartId, response, btKeyInventoryList, inventoryStatusArgList,
                                                             requestContext.MarketType, requestContext.CountryCode, requestContext.OrgId);
                            }
                            else
                            {
                                // get non-realtime Inventory list from NoSql service
                                var invResultsList = mongoDbInstance.GetInventoryResultsForMultipleItems(inventoryStatusArgList);
                                response.InventoryResultsList.AddRange(invResultsList);
                            }

                            // get inventory status
                            response.InventoryStatus = mongoDbInstance.GetInventoryStatus(inventoryStatusArgList);
                        }

                        //
                        response.ContentIndicator = btKeyHasReviewList.Count > 0
                                                                       ? (GetProductReviewIndicator(btKeyHasReviewList, string.Empty)).Data
                                                                       : new List<SiteTermObject>();
                        // check Dup Carts or Orders
                        //var isRequiredCheckDupCarts = requestContext.ShowDupCheckCartsForCart && !string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase);
                        //var isRequiredCheckDupOrder = requestContext.ShowDupCheckOrdersForCart && !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase);
                        //if (isRequiredCheckDupCarts || isRequiredCheckDupOrder)
                        //{
                        //    response.Duplicate = (await GetProductDuplicateIndicator(btKeyList, btEKeyList, cartId, isRequiredCheckDupCarts, isRequiredCheckDupOrder, userId, requestContext.DefaultDownloadedCarts, requestContext.CollectionAnalysisEnabled, requestContext.OrgId)).Data;
                        //}

                        // prices and promotion
                        response.Pricing = lineItemsToReturn;
                        response.Promotion = GetProductPromotion(promotionClientArg, requestContext.Targeting);

                        // get lineitem notes
                        if (requestContext.ShowUserEditableFieldsForCart)
                        {
                            var listItemIds = lineItems.Select(lineItem => lineItem.Id).ToList();
                            response.NotesList = await GridHelper.GetNotes(cartId, userId, null, listItemIds);
                        }

                        if (cart.IsShared)
                        {
                            //add my quantity to Pricing Data
                            foreach (var item in response.Pricing)
                            {
                                var noteItem = response.NotesList.FirstOrDefault(x => x.LineItemId == item.LineItemId);
                                if (noteItem != null) item.MyQuantity = noteItem.MyQty;
                            }
                        }
                        response.HasPermissionWF = cart.HasPermission;
                    }
                }

                result.Data = response;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ProfileResources.UnexpectedError;
                Logger.RaiseException(exception, ExceptionCategory.Order);
                result.Data = null;
            }
            return result;
        }

        private void GetStockCheckInventoryStatus(List<InventoryStockArg> btEKeyInventoryList, string userId, string cartId,
                                                  AdditionalCartLineItemsResponse additionalCartLineItemsResponse, List<InventoryStockArg> btKeyInventoryList,
                                                  List<SearchResultInventoryStatusArg> args,
                                                  MarketType marketType, string scCountryCode, string scOrgId)
        {
            List<AccountSummary> accountSummary = CartDAOManager.GetAccountsSummary(cartId);
            List<CartAccount> cartAccounts = DataConverter.ConvertListAccountSummaryToListCartAccount(accountSummary);

            List<InventoryResults> lstInventoryResult;

            int totalSOPLineErrors = 0;
            int totalTOTLASLineErrors = 0;

            Hashtable htSOPErrorMessage = new Hashtable();
            Hashtable htTOTLASErrorMessage = new Hashtable();
            string erpErrorMessage = string.Empty;
            var isHomeDeliveryCart = CartFrameworkHelper.IsHomeDeliveryCart(cartAccounts);
            var isVIPCart = false;
            bool hasBookAccount = false;
            bool hasEntAccount = false;

            if (cartAccounts != null && cartAccounts.Count > 0)
            {
                isVIPCart = cartAccounts.Any(cartAccount => cartAccount.AccountType == (int)AccountType.VIP);

                // OneBox account overrides Book & Ent accounts (TFS 22510)
                var isOneBoxCart = cartAccounts.Any(acc => acc.AccountType == (int)AccountType.OneBox && !string.IsNullOrEmpty(acc.AccountID));
                if (isOneBoxCart)
                {
                    // remove book & ent accounts
                    cartAccounts = cartAccounts.Where(acc => acc.AccountType != (int)AccountType.Book && acc.AccountType != (int)AccountType.Entertainment)
                                                .ToList();
                }

                // book inventory
                if (btKeyInventoryList.Count > 0)
                {
                    var bookSearchArg = new SearchResultInventoryStatusArg { ProductType = ProductTypeConstants.Book };
                    var defaultBookAccount = InventoryHelper.GetUserDefaultAccountFromCartDetail(bookSearchArg, userId, cartId);

                    lstInventoryResult = GetRealTimeInventoryResultForCart(userId, btKeyInventoryList, "BTB",
                                                                           isHomeDeliveryCart, cartAccounts,
                                                                           defaultBookAccount, isVIPCart, out totalSOPLineErrors,
                                                                           out htSOPErrorMessage, args, cartId,
                                                                           marketType, scCountryCode, scOrgId, out hasBookAccount);
                    additionalCartLineItemsResponse.InventoryResultsList.AddRange(lstInventoryResult);
                }

                // ent inventory
                if (btEKeyInventoryList.Count > 0)
                {
                    var entSearchArg = new SearchResultInventoryStatusArg { ProductType = ProductTypeConstants.Music };
                    var defaultEntAccount = InventoryHelper.GetUserDefaultAccountFromCartDetail(entSearchArg, userId, cartId);

                    lstInventoryResult = GetRealTimeInventoryResultForCart(userId, btEKeyInventoryList, "BTE",
                                                                           isHomeDeliveryCart, cartAccounts,
                                                                           defaultEntAccount, isVIPCart, out totalTOTLASLineErrors,
                                                                           out htTOTLASErrorMessage, args, cartId,
                                                                           marketType, scCountryCode, scOrgId, out hasEntAccount);
                    additionalCartLineItemsResponse.InventoryResultsList.AddRange(lstInventoryResult);
                }
            }

            // Fix TFS19876 by assigning ProductType to each result item
            foreach (var resultItem in additionalCartLineItemsResponse.InventoryResultsList)
            {
                var argFound = args.FirstOrDefault(r => r.BTKey == resultItem.BTKey);
                if (argFound != null)
                    resultItem.ProductType = argFound.ProductType;
            }


            foreach (string code in htSOPErrorMessage.Keys)
            {
                erpErrorMessage += string.Format("{0}:{1}, ", code, htSOPErrorMessage[code]);
            }

            foreach (string code in htTOTLASErrorMessage.Keys)
            {
                if (!htSOPErrorMessage.ContainsKey(code))
                    erpErrorMessage += string.Format("{0}:{1}, ", code, htTOTLASErrorMessage[code]);
            }

            if (!string.IsNullOrEmpty(erpErrorMessage) && erpErrorMessage.LastIndexOf(',') > 0)
                erpErrorMessage = erpErrorMessage.Substring(0, erpErrorMessage.LastIndexOf(','));

            additionalCartLineItemsResponse.StockCheckInventoryStatus = string.Format("{0}|{1}|{2}|{3}", true,
                                                                              btKeyInventoryList.Count +
                                                                              btEKeyInventoryList.Count,
                                                                              totalSOPLineErrors + totalTOTLASLineErrors,
                                                                              erpErrorMessage);

            // TFS 28271: show error if No account found
            var hasAccount = true;
            if ((cartAccounts == null || cartAccounts.Count == 0) || (!hasBookAccount && !hasEntAccount))
            {
                hasAccount = false;
                erpErrorMessage = "No account found. Inventory check was unsuccessful.";
            }

            additionalCartLineItemsResponse.StockCheckInventoryStatus = string.Format("{0}|{1}|{2}|{3}", hasAccount,
                                                                              btKeyInventoryList.Count +
                                                                              btEKeyInventoryList.Count,
                                                                              totalSOPLineErrors + totalTOTLASLineErrors,
                                                                              erpErrorMessage);
        }

        private List<InventoryResults> GetRealTimeInventoryResultForCart(string userID, List<InventoryStockArg> ltBTKey
                                    , string targetERP, bool isHomeDeliveryCart
                                    , List<CartAccount> cartAccounts, Account defaultAccount, bool isVIPCart
                                    , out int totalLineErrors, out Hashtable htlineErrorMessage, List<SearchResultInventoryStatusArg> args, string cartId
                                    , MarketType marketType, string scCountryCode, string scOrgId, out bool hasAccount)
        {
            var displayInventoryForAllWareHouse = true;
            var isStockCheckInventory = true;
            totalLineErrors = 0;
            htlineErrorMessage = new Hashtable();

            var inventoryHelper4MongoDb = InventoryHelper4MongoDb.GetInstance(cartId, userID, orgId: scOrgId);
            if (inventoryHelper4MongoDb != null)
            {
                displayInventoryForAllWareHouse = inventoryHelper4MongoDb.IsDisplayAllWarehouse();
            }

            List<InventoryResults> resultList = new List<InventoryResults>();

            BTStockServiceLineItem[] lineItems = new BTStockServiceLineItem[ltBTKey.Count];

            if (ltBTKey.Count > 0)
            {
                for (int i = 0; i < ltBTKey.Count; i++)
                {
                    InventoryStockArg item = (InventoryStockArg)ltBTKey[i];

                    BTStockServiceLineItem lineItem = new BTStockServiceLineItem();
                    lineItem.ItemIDType = (targetERP == "BTB") ? BTStockCheckSvc.ProductIDTypes.BTKey : BTStockCheckSvc.ProductIDTypes.UPC;
                    lineItem.ItemID = (targetERP == "BTB") ? item.BTKey : item.UPC;
                    lineItem.OrderQuantity = item.Quantity;

                    lineItems[i] = lineItem;
                }
            }

            // check if cart is OneBox
            var isOneBoxCart = (cartAccounts != null && cartAccounts.Any(r => r.AccountType == (int)AccountType.OneBox && !string.IsNullOrEmpty(r.AccountID)));

            string defaultAcountID = "";
            string cartAccountERPNumber = "";
            foreach (CartAccount cartAccount in cartAccounts)
            {
                if (
                    (targetERP == "BTB" && cartAccount.AccountType == (int)AccountType.Book) ||
                    (targetERP == "BTE" && cartAccount.AccountType == (int)AccountType.Entertainment) ||
                    (targetERP == "BTB" && cartAccount.AccountType == (int)AccountType.VIP) ||
                    (cartAccount.AccountType == (int)AccountType.OneBox)
                    )
                {
                    if (isOneBoxCart && targetERP == "BTB")
                    {
                        cartAccountERPNumber = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.StockCheckDefaultOneBoxSOPAccountID); // "204286C5093933000000";
                        defaultAcountID = cartAccount.ERPAccountGUID;
                    }
                    else if ((isHomeDeliveryCart || isVIPCart) && targetERP == "BTB")
                    {
                        var StockCheckDefaultSOPAccountID = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.StockCheckDefaultSOPAccountID);
                        cartAccountERPNumber = StockCheckDefaultSOPAccountID; // "404286T6626933000000";
                    }
                    else
                    {
                        cartAccountERPNumber = cartAccount.AccountERPNumber;
                        defaultAcountID = cartAccount.ERPAccountGUID;
                    }

                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(cartAccountERPNumber))
            {
                var user = ProfileService.Instance.GetUserById(userID);
                if (user != null)
                {
                    // get default account from user level
                    var accountID = (targetERP == "BTB")
                                           ? user.DefaultBookAccountId
                                           : user.DefaultEntAccountId;
                    if (!string.IsNullOrWhiteSpace(accountID))
                    {
                        var userDefaultAccount = ProfileService.Instance.GetAccountById(accountID);
                        if (defaultAccount != null)
                        {
                            cartAccountERPNumber = userDefaultAccount.AccountNumber;
                            defaultAcountID = defaultAccount.AccountId;
                        }
                    }
                }
            }

            // get realtime Inventory from StockCheck service
            var response = InventoryHelper.GetRealTimeCartInventory(userID, lineItems, targetERP, cartAccountERPNumber);
            hasAccount = response.StatusMessage != AccountConstants.INVALID_ACCOUNT;

            if (InventoryHelper.IsDisplaySuperWarehouse(marketType, scCountryCode, scOrgId) || InventoryHelper.IsDisplayVIPWarehouse(marketType, scCountryCode, scOrgId))
            {
                var dbInventoryResults = GetInventoryResultsForCart(cartId, args, marketType, scCountryCode, scOrgId);
                AddVIPToRealTimeInventory(ref response, marketType, dbInventoryResults, scOrgId, scCountryCode);
            }

            if (response.LineItems.Length > 0)
            {
                var demandData = GetInventoryWarehouseDemand(cartId, args);

                foreach (var responseLineItem in response.LineItems)
                {
                    string btKey = responseLineItem.ItemID;
                    string erpLineStatusCode = responseLineItem.ERPLineStatusCode;
                    string erpLineStatusMessage = responseLineItem.StatusMessage;

                    if (!string.IsNullOrEmpty(erpLineStatusCode) && erpLineStatusCode != "00000")
                    {
                        totalLineErrors += 1;
                        if (!htlineErrorMessage.ContainsKey(erpLineStatusCode))
                            htlineErrorMessage.Add(erpLineStatusCode, erpLineStatusMessage);
                    }

                    if (responseLineItem.ItemIDType == BTStockCheckSvc.ProductIDTypes.UPC)
                    {
                        string upc = responseLineItem.ItemID;
                        foreach (var requestItem in ltBTKey)
                        {
                            if (requestItem.UPC == upc)
                            {
                                btKey = requestItem.BTKey;
                                break;
                            }
                        }
                    }

                    BTStockCheckSvc.WHSOHOnly[] warehouses = responseLineItem.Warehousehoses;
                    List<InventoryStockStatus> stockStatusList = new List<InventoryStockStatus>();

                    BTKeyInventoryResult whDemand = null;
                    if (demandData.ContainsKey(btKey))
                    {
                        whDemand = demandData[btKey];
                    }

                    if (warehouses.Length > 0)
                    {
                        foreach (var warehouse in warehouses)
                        {
                            if (string.IsNullOrEmpty(warehouse.WHSCode))
                                continue;

                            InventoryStockStatus stockStatus = new InventoryStockStatus(0, 0, 0, 0, 0, 0, 0, warehouse.QTYOnHand.ToString(), warehouse.WHSDescription, warehouse.WHSCode);
                            var inventoryHelper = InventoryHelper4MongoDb.GetInstance(cartId, userID);

                            stockStatus.WareHouse = inventoryHelper.WarehouseDesc.ContainsKey(warehouse.WHSCode) ? inventoryHelper.WarehouseDesc[warehouse.WHSCode] : warehouse.WHSCode;
                            stockStatus.FormalWareHouseCode = warehouse.WHSCode;

                            if (whDemand != null)
                            {
                                var wh = whDemand.Warehouses.FirstOrDefault(r => r.WarehouseId == warehouse.WHSCode);
                                if (wh != null)
                                {
                                    stockStatus.InvDemandNumber = wh.Last30DayDemand;
                                    stockStatus.OnOrderQuantity = wh.OnOrderQuantity;
                                }
                            }

                            stockStatusList.Add(stockStatus);
                        }
                    }

                    // TFS 24744
                    var isAvProduct = (targetERP == "BTE");
                    var isEastWarehouseSet = false;
                    if (isAvProduct && !displayInventoryForAllWareHouse)
                    {
                        var account = ProfileController.Instance.GetAccountById(defaultAcountID, false, false);
                        if (account != null && account.PrimaryWarehouseCode == InventoryWareHouseCode.Com && string.IsNullOrEmpty(account.SecondaryWarehouseCode))
                        {
                            // display only SOM (East) warehouse
                            stockStatusList = stockStatusList.Where(r => r.WareHouseCode == InventoryWareHouseCode.Som).ToList();
                            isEastWarehouseSet = true;
                        }
                    }

                    if (isEastWarehouseSet == false)
                    {
                        stockStatusList = InventoryHelper.CorrectAndFilterInventory(stockStatusList, marketType);
                        stockStatusList = InventoryHelper.SortRealTimeInventoryList(stockStatusList, userID, defaultAccount, marketType);
                        // filter blank item
                        stockStatusList = stockStatusList.Where(whs => !string.IsNullOrEmpty(whs.WareHouse)).ToList();
                    }

                    var inventoryResults = new InventoryResults
                    {
                        DisplayInventoryForAllWareHouse = isEastWarehouseSet ? true : displayInventoryForAllWareHouse,
                        InventoryStock = stockStatusList,
                        BTKey = btKey,
                        ERPLineStatusCode = erpLineStatusCode,
                        ERPLineStatusMessage = erpLineStatusMessage,
                        IsStockCheckInventory = isStockCheckInventory,
                        TotalLast30Demand = whDemand == null ? 0 : whDemand.TotalLast30Demand
                    };

                    resultList.Add(inventoryResults);
                }
            }
            else
            {
                foreach (var requestItem in ltBTKey)
                {
                    string btKey = requestItem.BTKey;
                    string erpLineStatusCode = response.ERPResponseStatusCode;

                    string erpLineStatusMessage = string.IsNullOrEmpty(response.AccountID) ? 
                                                    "Account is missing":
                                                    string.Format("AccountID:{0} {1}", response.AccountID, response.StatusMessage);
                    List<InventoryStockStatus> stockStatusList = new List<InventoryStockStatus>();

                    if (!string.IsNullOrEmpty(erpLineStatusCode) && erpLineStatusCode != "00000")
                    {
                        totalLineErrors += 1;
                        if (!htlineErrorMessage.ContainsKey(erpLineStatusCode))
                            htlineErrorMessage.Add(erpLineStatusCode, erpLineStatusMessage);
                    }

                    stockStatusList = InventoryHelper.CorrectAndFilterInventory(stockStatusList, marketType);

                    if (stockStatusList.Count > 0)
                    {
                        var inventoryHelper = InventoryHelper4MongoDb.GetInstance(cartId, userID);
                        foreach (InventoryStockStatus stockStatus in stockStatusList)
                        {
                            stockStatus.WareHouse = inventoryHelper.WarehouseDesc.ContainsKey(stockStatus.WareHouseCode) ? inventoryHelper.WarehouseDesc[stockStatus.WareHouseCode] : stockStatus.WareHouseCode;
                            stockStatus.FormalWareHouseCode = stockStatus.WareHouseCode;
                        }
                    }

                    stockStatusList = InventoryHelper.SortRealTimeInventoryList(stockStatusList, userID, defaultAccount, marketType);

                    // filter blank item
                    stockStatusList = stockStatusList.Where(whs => !string.IsNullOrEmpty(whs.WareHouse)).ToList();

                    var inventoryResults = new InventoryResults
                    {
                        DisplayInventoryForAllWareHouse = displayInventoryForAllWareHouse,
                        InventoryStock = stockStatusList,
                        BTKey = btKey,
                        ERPLineStatusCode = erpLineStatusCode,
                        ERPLineStatusMessage = erpLineStatusMessage,
                        IsStockCheckInventory = isStockCheckInventory
                    };

                    resultList.Add(inventoryResults);
                }
            }
            return resultList;
        }

        private ProductSearchResultItem ConvertLineItemToProductSearchResultItem(RequestLineItem item)
        {
            if (item == null) return null;

            var productItem = new ProductSearchResultItem();
            productItem.BTKey = item.BTKey;
            productItem.BTEKey = item.BTEKey;
            productItem.ProductType = item.ProductType;
            productItem.Catalog = item.CatalogName;
            productItem.ReportCode = item.ReportCode;
            productItem.Quantity = item.Quantity.HasValue ? item.Quantity.Value : 0;
            if (item.PublishDate.HasValue)
            {
                productItem.PublishDate = item.PublishDate.Value;
            }
            productItem.MerchCategory = item.MerchCategory;
            productItem.ESupplier = item.ESupplier;
            productItem.AcceptableDiscount = item.BTDiscountPercent;
            productItem.GTIN = item.BTGTIN;
            productItem.Upc = item.Upc;
            productItem.HasReturn = item.HasReturn;
            productItem.ListPrice = item.ListPrice.HasValue ? item.ListPrice.Value : 0;
            productItem.PriceKey = item.PriceKey;
            productItem.ProductLine = item.ProductLine;
            productItem.ProductFormat = item.FormatLiteral;
            productItem.Publisher = item.Publisher;
            productItem.SupplierCode = item.SupplierCode;
            productItem.BasketOriginalEntryId = item.BasketOriginalEntryId;
            productItem.HasReview = item.HasReview;
            productItem.BlockedExportCountryCodes = item.BlockedExportCountryCodes;

            return productItem;
        }

        private void SetPricingListClientObject(bool isBeingPricing, bool isGaleAccountInCart, List<PricingReturn4ClientObject> lineItemsToReturn,
                                                RequestLineItem cartLineItem, string galeLiteral, string marketType)
        {
            SetPricingListClientObject(isBeingPricing, isGaleAccountInCart, lineItemsToReturn, galeLiteral, cartLineItem.BTDiscountPercent,
                cartLineItem.ListPrice, cartLineItem.SalePrice, cartLineItem.ESupplier, cartLineItem.Id, cartLineItem.BTKey, cartLineItem.Quantity, marketType);
        }

        private void SetPricingListClientObject(bool isBeingPricing, bool isGaleAccountInCart,
            List<PricingReturn4ClientObject> lineItemsToReturn, string galeLiteral, decimal discountPercent, decimal? listPriceDecimal, decimal? salePrice,
            string eSupplier, string lineItemId, string btKey, int? qty, string marketType)
        {
            var discountPercentage = CommonHelper.FormatDiscount(discountPercent) + " % Discount";
            var listPrice = CommonHelper.GetCurrencyFormat(listPriceDecimal.ToString());
            var na = CommonResources.NA;

            var price = CommonHelper.GetDisplayValueOfDiscountPrice(salePrice, true);
            if (isBeingPricing)
            {
                price = CommonResources.PricingStatusText;
            }

            if (eSupplier == galeLiteral)
            {
                price = CommonHelper.DeterminePriceForGaleProductInCart(price, isGaleAccountInCart);
                if (price == na)
                {
                    listPrice = price;
                    discountPercentage = null;
                }
            }

            if (marketType != BT.TS360Constants.MarketType.Retail.ToString())
            {
                discountPercentage = null;
            }
            var pricingReturn4ClientObject = new PricingReturn4ClientObject
            {
                LineItemId = lineItemId,
                BTKey = btKey,
                Price = price,
                ListPrice = listPrice,
                DisPercent = discountPercentage,
                Quantity = isBeingPricing ? CommonResources.PricingStatusText : qty.ToString()
            };

            lineItemsToReturn.Add(pricingReturn4ClientObject);
        }

        public AppServiceResult<string> QuickItemDetailStockCheck(InventoryStatusArgRequest request)
        {

            var result = new AppServiceResult<string>();
            try
            {
                result.Status = AppServiceStatus.Success;
                int last30DaysDemand;
                bool hasDemand;
                string inventoryStatus;
                var inventory = GetInventoryForItemDetail(request.arg, out last30DaysDemand,
                    out inventoryStatus, out hasDemand, request.MarketType, request.UserId, request.CountryCode,
                    request.OrgId);

                result.Data = inventory.Data;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                //Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
                Logger.WriteLog(exception, ExceptionCategory.ItemDetails.ToString());
            }
            return result;
        }

        public async Task<AppServiceResult<ItemDetailReturn>> GetProductDetailsInfoForQuickItemDetailPageAsync(ProductDetailsInfoForQuickItemDetailRequest request)
        {
            var result = new AppServiceResult<ItemDetailReturn>();
            try
            {
                //var userProfile = ProfileService.Instance.GetUserById(request.UserId);
                //string defaultDuplicateOrders = userProfile.DefaultDuplicateOrders;
                //string defaultDuplicateCarts = userProfile.DefaultDuplicateCarts;
                var dataReturn = new ItemDetailReturn();

                var productDetail = await GetProductDetail(request.BTKey, request.OCLCCatalogingPlusEnabled, request.UserId,
                        request.AllowBTEmployee);
                dataReturn.DetailInfo = productDetail;

                //if (!string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase) || !string.Equals(defaultDuplicateOrders, "none",
                //    StringComparison.OrdinalIgnoreCase))
                //{
                //    var dupIndicator = await GetProductDuplicateIndicator(new List<string> { request.BTKey }, new List<string>(), request.CartId,
                //        !string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase),
                //        !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase), request.UserId,
                //            request.DefaultDownloadedCarts, request.CollectionAnalysisEnabled, request.OrgId);
                //    dataReturn.DuplicateIndicator = dupIndicator.Data;
                //}

                result.Status = AppServiceStatus.Success;
                result.Data = dataReturn;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(exception, ExceptionCategory.ItemDetails.ToString());
            }
            return result;
        }

        public async Task<AppServiceResult<string>> DataFixSendEmailToBtAsync(DataFixSendEmailToBtRequest request)
        {
            var result = new AppServiceResult<string>();
            string btKey = request.btKey;
            string userNote = request.userNote;
            var userEmail = request.UserEmail;
            var userId = request.UserId;
            try
            {
                btKey = HttpUtility.UrlDecode(btKey);
                ProductSearchResultItem currentProduct = null;

                var searchResults = ProductSearchController.SearchByIdWithoutAnyRules(new List<string> { btKey });
                if (searchResults != null && searchResults.Items != null && searchResults.Items.Count > 0)
                {
                    currentProduct = searchResults.Items[0];
                }

                if (currentProduct == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                    return result;
                }
                result.ErrorMessage = "";
                var emailSubject = "TS 360 Product Data Error/Issue";
                var emailBody = GetDataFixEmailBody(currentProduct, userNote, request.UserName, request.LoginId, request.MarketType, request.OrganizationName);
                string mailTo;
                if (string.IsNullOrEmpty(userEmail) || userEmail.Trim().EndsWith("@ts360.com", StringComparison.OrdinalIgnoreCase))
                {
                    result.ErrorMessage =
                        //"You do not have a valid email address setup in &lt;b&gt;My Preferences&lt;/b&gt;. The error will be sent to Datafix, but you will be unable to receive confirmation or relevant feedback. Please update your email address.";
                    "You do not have a valid email address setup in <b>My Preferences</b>. The error will be sent to Datafix, but you will be unable to receive confirmation or relevant feedback. Please update your email address.";
                    mailTo = AppSetting.DataFixSendToMail;
                }
                else
                {
                    mailTo = AppSetting.DataFixSendToMail + ";" + userEmail;
                }
                try
                {
                    //Call send mail function
                    string smtpServer = AppSetting.SmtpServer;
                    Emailer emailer = new Emailer(smtpServer);
                    var sendResult = emailer.Send(mailTo, emailSubject, emailBody);

                    if (!sendResult)
                    {
                        result.ErrorMessage =
                            //"You do not have a valid email address setup in &lt;b&gt;My Preferences&lt;/b&gt;. The error will be sent to Datafix, but you will be unable to receive confirmation or relevant feedback. Please update your email address.";
                            "You do not have a valid email address setup in <b>My Preferences</b>. The error will be sent to Datafix, but you will be unable to receive confirmation or relevant feedback. Please update your email address.";
                    }
                }
                catch (Exception exception)
                {
                    //result.ErrorMessage = GetLocalizedString("SearchResources", "DataFixEmailInvalid");
                    result.ErrorMessage =
                        //"You do not have a valid email address setup in &lt;b&gt;My Preferences&lt;/b&gt;. The error will be sent to Datafix, but you will be unable to receive confirmation or relevant feedback. Please update your email address.";
                    "You do not have a valid email address setup in <b>My Preferences</b>. The error will be sent to Datafix, but you will be unable to receive confirmation or relevant feedback. Please update your email address.";
                    Logger.WriteLog(exception, ExceptionCategory.ContenManagement.ToString());
                }

                result.Status = AppServiceStatus.Success;
                //result.Data = GetLocalizedString("SearchResources", "DataFixSuccess");
                result.Data =
                "Success. The error notification has been sent to Datafix. If your email address is valid you will receive a copy of the email, as well as, follow-up emails as Datafix receives and addresses the error notification.";
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(exception, ExceptionCategory.ContenManagement.ToString());

            }
            return result;
        }

        private string GetDataFixEmailBody(ProductSearchResultItem productDetail, string userNote, string userName, string loginId, MarketType? marketType, string organizationName)
        {
            string emailBody;
            var strNotes = userNote.StripHTML();

            using (var reader = new StreamReader(Emailer.GetDataFixEmailContentTemplate()))
            {
                emailBody = reader.ReadToEnd();
            }

            var isBook = productDetail.ProductType.ToLower() != "movie" &&
                         productDetail.ProductType.ToLower() != "music";


            emailBody = emailBody.Replace("{TITLE}", productDetail.Title);
            emailBody = emailBody.Replace("{AuthorLiteral}", isBook ? "Author" : "Artist");
            emailBody = emailBody.Replace("{AUTHOR}", productDetail.AuthorText);
            emailBody = emailBody.Replace("{ISBN}", productDetail.ISBN);
            emailBody = emailBody.Replace("{BTKEY}", productDetail.BTKey);
            emailBody = emailBody.Replace("{FORMAT}", productDetail.FormatLiteral);
            emailBody = emailBody.Replace("{PublisherLiteral}", isBook ? "Publisher" : "Supplier");
            emailBody = emailBody.Replace("{PUBLISHER}", productDetail.Publisher);
            emailBody = emailBody.Replace("{PublishDateLiteral}", isBook ? "Publish Date" : "Release Date");
            emailBody = emailBody.Replace("{PUBLISHDATE}", productDetail.PublishDate.ToString(CommonConstants.DefaultDateTimeFormat));
            emailBody = emailBody.Replace("{MARKET}", marketType.ToString());
            emailBody = emailBody.Replace("{ORGANIZATIONNAME}", organizationName);
            emailBody = emailBody.Replace("{USERNAME}", userName);
            emailBody = emailBody.Replace("{LOGINID}", loginId);
            emailBody = emailBody.Replace("{USERNOTE}", strNotes);

            return emailBody;
        }

        public async Task<AppServiceResult<string>> DataFixPersitUserNoteToSessionAsync(DataFixPersitUserNoteRequest request)
        {
            var result = new AppServiceResult<string>();
            try
            {
                string btKey = request.btKey;
                string userNote = request.userNote;
                btKey = HttpUtility.UrlDecode(btKey);
                var userId = request.UserId;
                var cacheKeySession = string.Format("__DataFixUserNoteCacheKey{0}", btKey);
                var cacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, cacheKeySession);
                CachingController.Instance.Write(cacheKey, userNote, 15);
                result.Status = AppServiceStatus.Success;
                result.Data = "";
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(exception, ExceptionCategory.ContenManagement.ToString());
            }
            return result;
        }
        public async Task<AppServiceResult<string>> SaveGridFieldsMyPreferenceForNewGridAsync(GridFieldsMyPreferenceForNewGridRequest request)
        {
            var result = new AppServiceResult<string>();
            try
            {

                var userGridFieldObjects = request.userGridFieldObjects;
                var defaultQuantity = request.defaultQuantity;
                var userId = request.UserId;
                var _defaultQuantity = 0;
                int.TryParse(defaultQuantity, out _defaultQuantity);

                var userGridFields = ConvertUserGridFieldObject(userGridFieldObjects);

                UserGridFieldsCodesManager.Instance.SaveUserGridFieldsCodes(userId, userGridFields,
                    _defaultQuantity);

                result.Data = "Your information has been saved successfully!";
                result.Status = AppServiceStatus.Success;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = "Unexpected error! Please contact administrator.";
            }
            return result;
        }
        private List<CommonBaseGridUserControl.UIUserGridField> ConvertUserGridFieldObject(List<UserGridFieldObject> userGridFieldObjects)
        {
            var results = new List<CommonBaseGridUserControl.UIUserGridField>();
            foreach (var userGridFieldObject in userGridFieldObjects)
            {
                var result = new CommonBaseGridUserControl.UIUserGridField();
                result.GridFieldID = userGridFieldObject.GridFieldId;
                result.DefaultGridCodeID = userGridFieldObject.DefaultGridCodeId;
                result.DefaultGridText = userGridFieldObject.DefaultGridText;
                result.DisplayType = userGridFieldObject.DisplayType;
                result.UserGridFieldID = userGridFieldObject.UserGridFieldId;

                results.Add(result);
            }
            return results;
        }
        public async Task<AppServiceResult<ObjectWithTotalPage<List<SiteTermObject>>>> GetSiteTermWithSortParameter(SiteTermRequest request)
        {
            var ajaxResult = new AppServiceResult<ObjectWithTotalPage<List<SiteTermObject>>>();
            var objectWithTotalPage = new ObjectWithTotalPage<List<SiteTermObject>>();
            var siteTermName = GetSiteTerm(request.st);
            if (siteTermName == SiteTermName.None)
                return null;

            var siteTerm = await SiteTermHelper.Instance.GetSiteTemByNameAsync(request.st);

            var siteTermObjects = new List<SiteTermObject>();
            //
            if (request.nSize <= 0)
                request.nSize = PageSize;
            //
            if (siteTerm != null)
            {
                siteTermObjects.AddRange(siteTerm.Select(siteTermElement => new SiteTermObject(siteTermElement.ItemKey, siteTermElement.ItemValue)));
                if (request.nPage > -1)
                {
                    siteTermObjects = siteTermObjects.Skip(request.nPage * request.nSize).Take(request.nSize).ToList();
                }
            }
            //
            if (request.nSize == 0)
                if (siteTerm != null) request.nSize = siteTerm.Count;
            //

            if (request.bSort)
            {
                if (request.byValue)
                    siteTermObjects = new List<SiteTermObject>(siteTermObjects.OrderBy(t => t.Value));
                else
                    siteTermObjects = new List<SiteTermObject>(siteTermObjects.OrderBy(t => t.Name));
            }

            objectWithTotalPage.Data = siteTermObjects;
            if (siteTerm != null)
                objectWithTotalPage.TotalSize = (int)Math.Ceiling((decimal)siteTerm.Count / request.nSize);
            //
            ajaxResult.Data = objectWithTotalPage;
            return ajaxResult;
        }

        public async Task<AppServiceResult<ItemDetailPrimaryInfoReturn>> GetInventoryDemandItemDetail(InventoryDemandItemDetail request)
        {
            var result = new AppServiceResult<ItemDetailPrimaryInfoReturn>();


            var dataReturn = new ItemDetailPrimaryInfoReturn();

            int last30DaysDemand;
            bool hasDemand;
            string inventoryStatus;
            var inventory = GetInventoryForItemDetail(request.InventoryArg, out last30DaysDemand,
                out inventoryStatus, out hasDemand, request.MarketType, request.UserId, request.CountryCode,
                request.OrgId);

            dataReturn.InventoryData = inventory.Data;
            dataReturn.Last30DaysDemandInfo = last30DaysDemand;
            dataReturn.InventoryStatus = inventoryStatus;
            dataReturn.HasDemand = hasDemand;

            result.Status = AppServiceStatus.Success;
            result.Data = dataReturn;

            return result;
        }

        public async Task<AppServiceResult<ItemDetailPrimaryInfoReturn>> GetPricingItemDetail(PrimaryInfoItemDetailRequest request)
        {
            var result = new AppServiceResult<ItemDetailPrimaryInfoReturn>();

            if (request == null)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = "Request is empty";
            }
            else
            {
                var dataReturn = new ItemDetailPrimaryInfoReturn();
                dataReturn.PricingInfo = await GetItemPrice(request.CartId, request.BTKey, request.LineItemId, request.UserId, request.SearchData.ESuppliers,
                                request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData);

                result.Status = AppServiceStatus.Success;
                result.Data = dataReturn;
            }

            return result;
        }

        public async Task<AppServiceResult<ItemDetailPrimaryInfoReturn>> GetGridLineCount(GridLineCount request)
        {
            var result = new AppServiceResult<ItemDetailPrimaryInfoReturn>();

            var dataReturn = new ItemDetailPrimaryInfoReturn();

            // get grid header count
            dataReturn.GridLineCount = GridHelper.GetGridLineCount(request.CartId, request.LineItemId, request.IsGridEnabled);

            dataReturn.TitleDetailSelectingTab = request.TitleDetailSelectingTab;

            result.Status = AppServiceStatus.Success;
            result.Data = dataReturn;

            return result;
        }

        public static string QUOTED = "Quoted";

        private async Task<PricingContract> GetItemPrice(string cartId, string btkey, string lineItemId, string userId,
             string[] ESuppliers, bool hideNetPriceDiscountPercentage, TargetingValues targeting, AccountInfoForPricing accountPricingData)
        {
            //var userId = siteContext.UserId;

            var pricingContract = new PricingContract();
            pricingContract.IsRetail = (targeting.MarketType == MarketType.Retail);
            if (!string.IsNullOrEmpty(cartId))
            {

                Cart cart = CartDAOManager.Instance.GetCartById(cartId, true, userId, targeting);

                if (cart != null)
                {
                    var lineItem = await CartDAOManager.Instance.GetCartLineById(lineItemId, userId);
                    //Check BTKey to ensure user not input an invalid LineItemId
                    if (lineItem != null && string.Compare(lineItem.BTKey, btkey, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        var netPrice = lineItem.SalePrice.HasValue ? lineItem.SalePrice.Value : 0;

                        pricingContract.ListPrice =
                            CommonHelper.GetDisplayValueOfDiscountPrice(
                                lineItem.ListPrice.HasValue ? lineItem.ListPrice.Value : 0, true);
                        pricingContract.NetPrice = CommonHelper.GetDisplayValueOfDiscountPrice(netPrice, true);
                        pricingContract.DiscountPercentage = CommonHelper.FormatDiscount(lineItem.BTDiscountPercent) + " % Discount";

                        bool isQuoted = string.Compare(cart.BTStatus, QUOTED, StringComparison.OrdinalIgnoreCase) == 0;
                        pricingContract.QuotedPriceIndicator = (isQuoted ? OrderResources.QuotedPrice : "");

                    }
                }
            }
            else
            {
                var itemCache = GetProductDetailsFromFast(btkey);

                if (itemCache == null)
                {
                    var logMsg = string.Format("userId: {0}", userId);
                    PricingLogger.LogDebug(ExceptionCategory.ItemDetails.ToString(), string.Format("BTKEY:{0} is not in cache. {1}", btkey, logMsg));
                    return pricingContract;
                }

                var prodItem = itemCache;

                var realtimePricingHelper = new RealTimePricingHelper();
                var marketType = targeting.MarketType ?? MarketType.Any;
                var audienceType = CommonHelper.Instance.ConvertAudienceTypeAsString(targeting.AudienceType);
                //get account info
                var accountInfo = realtimePricingHelper.GetAccountInfoForPricing(prodItem.ProductType, prodItem.ESupplier,
                        prodItem.ProductFormat, userId, accountPricingData);

                var listBasketLineItemUpdated = new List<BasketLineItemUpdated>
                                                    {
                                                        new BasketLineItemUpdated 
                                                        {
                                                            ISBN = prodItem.ISBN,
                                                            BTKey = prodItem.BTKey,
                                                            SoldToId = userId,
                                                            AccountId = accountInfo.AccountId,
                                                            ProductType = prodItem.ProductType,
                                                            TotalLineQuantity = 1,
                                                            TotalOrderQuantity = 1,
                                                            PriceKey = prodItem.PriceKey,
                                                            ListPrice = prodItem.ListPrice,
                                                            AccountPricePlan = accountInfo.AccountPricePlanId,
                                                            ProductLine = prodItem.ProductLine,
                                                            ReturnFlag = prodItem.HasReturn,
                                                            AccountERPNumber = accountInfo.ErpAccountNumber,
                                                            PrimaryWarehouse = accountInfo.PrimaryWarehouseCode,
                                                            ProductCatalog = prodItem.Catalog,
                                                            MarketType = marketType.ToString(),                                                            
                                                            AudienceType = audienceType,
                                                            ESupplier = prodItem.ESupplier,
                                                            EMarket = accountInfo.EMarketType,
                                                            ETier = accountInfo.ETier,
                                                            ProductPriceChangedIndicator = true,
                                                            ContractChangedIndicator = true,
                                                            PromotionChangedIndicator = false,
                                                            PromotionActiveIndicator = false,
                                                            QuantityChanged = true,
                                                            IsHomeDelivery = accountInfo.IsHomeDelivery,
                                                            Upc = prodItem.Upc,
                                                            AcceptableDiscount = prodItem.AcceptableDiscount,
                                                            PurchaseOption = prodItem.PurchaseOption, // MUPO+processing charger
                                                            NumberOfBuildings = accountInfo.BuildingNumbers,
                                                            ProcessingCharges = accountInfo.ProcessingCharges,
                                                            SalesTax = accountInfo.SalesTax,
                                                            IsVIPAccount = accountInfo.IsVIPAccount
                                                        }
                                                    };
                var pricingController = new PricingController();
                //4428: For Search Page Only and for Retail Customers Only we will pass a Static order quantity of 5 
                //and a line quantity of 5 in the product search.  Applicable to both Basic Search and Advanced Search.  Hardcoded to Pricing.
                var listItemPricing = pricingController.CalculatePrice(listBasketLineItemUpdated, (marketType == MarketType.Retail)
                                                                           ? 5
                                                                           : 1, targeting, hideNetPriceDiscountPercentage);
                var galeLiteral = CommonHelper.GetESupplierTextFromSiteTerm(AccountType.GALEE.ToString());
                if (listItemPricing != null && listItemPricing.Count > 0 && listItemPricing[0] != null)
                {

                    var listPrice = listItemPricing[0].ListPrice.HasValue ? listItemPricing[0].ListPrice.Value : 0;
                    var netPrice = listItemPricing[0].SalePrice.HasValue ? listItemPricing[0].SalePrice.Value : 0;

                    var discount = listItemPricing[0].DiscountPercent.HasValue ? listItemPricing[0].DiscountPercent.Value : 0;
                    var na = CommonResources.NA;

                    if (string.Compare(listItemPricing[0].ESupplier, galeLiteral, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        var isExistGaleAccount = CommonHelper.IsESupplierAccountExisted(AccountType.GALEE.ToString(), userId);
                        var priceText = CommonHelper.Instance.DeterminePriceForGaleProduct(netPrice.ToString(),
                                                                                  isExistGaleAccount, ESuppliers);
                        if (priceText == na)
                        {
                            listPrice = 0;
                            netPrice = 0;
                            discount = 0;
                        }
                    }

                    pricingContract.ListPrice = CommonHelper.GetDisplayValueOfDiscountPrice(listPrice, true);
                    pricingContract.NetPrice = CommonHelper.GetDisplayValueOfDiscountPrice(netPrice, true);
                    pricingContract.DiscountPercentage = CommonHelper.FormatDiscount(discount) + " % Discount";
                }
            }
            //pricingContract.IsRetail = (siteContext.MarketType == MarketType.Retail);
            return pricingContract;
        }

        //public async Task<AppServiceResult<ItemDetailSecondaryInfoReturn>> GetDupIcons(DupIcons request)
        //{
        //    var result = new AppServiceResult<ItemDetailSecondaryInfoReturn>();
        //    try
        //    {
        //        var dataReturn = new ItemDetailSecondaryInfoReturn();
        //        if (request != null)
        //        {
        //            var userProfile = ProfileService.Instance.GetUserById(request.UserId); // CSObjectProxy.GetUserProfileForSearchResult();
        //            if (userProfile != null)
        //            {
        //                string defaultDuplicateOrders = userProfile.DefaultDuplicateOrders;
        //                string defaultDuplicateCarts = userProfile.DefaultDuplicateCarts;
        //                if (!string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase) ||
        //                    !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    var dupIndicator = await GetProductDuplicateIndicator(new List<string> { request.BTKey },
        //                        new List<string>(), request.CartId,
        //                        !string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase),
        //                        !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase), request.UserId,
        //                        request.DefaultDownloadedCarts, request.CollectionAnalysisEnabled, request.OrgId);

        //                    dataReturn.DuplicateIndicator = dupIndicator.Data;
        //                }
        //            }
        //        }

        //        result.Status = AppServiceStatus.Success;
        //        result.Data = dataReturn;
        //    }
        //    catch (Exception exception)
        //    {
        //        result.Status = AppServiceStatus.Fail;
        //        result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
        //        Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
        //    }
        //    return result;
        //}

        public async Task<AppServiceResult<ItemDetailSecondaryInfoReturn>> GetAdditionalInfoItemDetails(SecondaryInfoItemDetailRequest request)
        {
            var result = new AppServiceResult<ItemDetailSecondaryInfoReturn>();
            try
            {
                var dataReturn = new ItemDetailSecondaryInfoReturn();
                if (request != null)
                {
                    var relatedProduct = GetRelatedProductData(request.Author, request.BTKey, request.IsPrimaryCartSet,
                        request.UserId, request.IsHideNetPriceDiscountPercentage, request.AccountPricingData,
                        request.Targeting, request.SearchData);
                    dataReturn.RelatedProductData = relatedProduct;

                    var note = await GetNotesAsync(request.CartId, new List<string> { request.BTKey }, request.UserId);
                    if (note.Data.NotesList != null && note.Data.NotesList.Count > 0)
                        dataReturn.Note = note.Data.NotesList[0];

                    dataReturn.PromotionInfo = CheckPromotion(request.BTKey, request.Catalog, request.Targeting);

                    dataReturn.ESPRankInfo = await GetESPRank(request.LineItemId);
                    dataReturn.PrimaryCartGridLink = await GeneratePrimaryGridDataLink(request.BTKey, request.UserId, request.Targeting.OrgId,
                        true, request.CartId);
                    dataReturn.AdditionalVersion = await GetAdditionalVersions(request.BTKey, request.ESupplier, request.DeafaultESuppliersAccount);

                    dataReturn.UpdateItemResult = CheckItemUpdateResult();

                    if (!string.IsNullOrEmpty(request.CartId))
                    {
                        var currentCart = await CartDAOManager.Instance.GetCartByIdAsync(request.CartId, request.UserId); // cartManager.GetCartById(arg.CartId);
                        if (currentCart != null)
                        {
                            if (dataReturn.ESPRankInfo != null)
                            {
                                dataReturn.ESPRankInfo.HasESPRanking = currentCart.HasESPRanking;
                            }
                        }
                    }

                    //var userProfile = ProfileService.Instance.GetUserById(request.UserId); // CSObjectProxy.GetUserProfileForSearchResult();
                    //if (userProfile != null)
                    //{
                    //    string defaultDuplicateOrders = userProfile.DefaultDuplicateOrders;
                    //    string defaultDuplicateCarts = userProfile.DefaultDuplicateCarts;
                    //    if (!string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase) ||
                    //        !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase))
                    //    {
                    //        var dupIndicator = await GetProductDuplicateIndicator(new List<string> { request.BTKey },
                    //            new List<string>(), request.CartId,
                    //            !string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase),
                    //            !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase), request.UserId,
                    //            request.DefaultDownloadedCarts, request.CollectionAnalysisEnabled, request.Targeting.OrgId);

                    //        dataReturn.DuplicateIndicator = dupIndicator.Data;
                    //    }
                    //}
                }

                result.Status = AppServiceStatus.Success;
                result.Data = dataReturn;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
            }
            return result;
        }

        private PromotionContract CheckPromotion(string btkey, string catalog, TargetingValues siteContext)
        {
            var promotionClientArg = new PromotionClientArg
            {
                BTKey = btkey,
                Catalog = catalog,
                ProductType = ""
            };

            var promoContract = new PromotionContract();
            string contentMsg = string.Empty;
            try
            {
                //contentMsg = MarketingController.GetProductPromotion(btkey, catalog);
                var listDiscounts = MarketingController.GetDiscountsForMultipleItem(new List<PromotionClientArg>() { promotionClientArg }, siteContext);
                if (listDiscounts != null && listDiscounts.Count > 0)
                {
                    contentMsg = listDiscounts[0].DiscountName;
                }
            }
            catch (Exception exception)
            {
                Logger.RaiseException(exception, ExceptionCategory.Marketing);
            }

            if (!string.IsNullOrEmpty(contentMsg))
            {
                // format of contentMsg: ItemDataText#ItemDataDescription
                string[] arrContent = contentMsg.Split('#');

                if (arrContent.Length == 2)
                {
                    promoContract.HasPromotion = true;

                    if (!string.IsNullOrEmpty(arrContent[1]))
                    {
                        promoContract.PromoText = Microsoft.Security.Application.Encoder.HtmlEncode(arrContent[1]);
                    }

                    if (!string.IsNullOrEmpty(arrContent[0]))
                    {
                        string link = GetLinkfromContent(arrContent[0]);
                        if (string.Empty != link)
                        {
                            promoContract.PromoLink = link;
                        }
                    }
                }
                return promoContract;
            }
            return null;
        }

        private string GetLinkfromContent(string content)
        {
            var charIndex = content.IndexOf('/');
            if (charIndex != -1)
            {
                var temp = content.Substring(charIndex);
                charIndex = temp.IndexOf('"');
                if (charIndex != -1)
                {
                    temp = temp.Substring(0, charIndex);
                    return temp;
                }
            }

            return "";
        }

        public RelatedProductListContract GetRelatedProductData(string author, string btkey, bool isPrimaryCartSet, string userId, bool hideNetPriceDiscountPercentage, AccountInfoForPricing accountPricingData, TargetingValues targeting, SearchByIdData searchData)
        {
            var twilightContract = new RelatedProductListContract();
            var listItems = new List<RelatedProductContract>();
            var searchResult = FindRelatedItems(author, btkey, userId, hideNetPriceDiscountPercentage, accountPricingData, targeting, searchData);
            if (searchResult == null)
            {
                twilightContract.ListTitle = "0";
                twilightContract.ProductItems = listItems;
                return twilightContract;
            }

            var quantites = InitItemQuantities(searchResult, userId);
            foreach (var item in searchResult.Items)
            {
                var itemData = new RelatedProductContract();
                var itemDetailsUrl = string.Format("{0}?{1}={2}", SiteUrl.ItemDetails, SearchFieldNameConstants.btkey, item.BTKey);
                var ccUrl = ContentCafeHelper.GetJacketImageUrl(item.ISBN, ImageSize.Small, item.HasJacket);
                var quantity = 1;
                if (quantites.ContainsKey(item.BTKey))
                {
                    quantity = quantites[item.BTKey];
                }

                itemData.BTKey = item.BTKey;
                itemData.Author = item.Author;
                itemData.Title = item.Title;
                itemData.ListPriceText = item.ListPriceText;
                itemData.DiscountPriceText = item.DiscountPriceText;
                itemData.FormatIconPath = item.FormatIconPath;
                itemData.ProductFormat = (item.FormatClass == CommonConstants.ICON_MAKERSPACE ? ProductFormatConstants.Book_Makerspace : item.ProductFormat);
                itemData.ProductType = item.ProductType;
                itemData.ISBN = item.ISBN;
                itemData.GTIN = item.GTIN;
                itemData.Upc = item.Upc;
                itemData.Catalog = item.Catalog;
                itemData.Quantity = quantity;
                itemData.ItemDetailsUrl = itemDetailsUrl;
                itemData.CCUrl = ccUrl;
                itemData.HasQuantityInPrimaryCart = !quantites.ContainsKey(item.BTKey);
                itemData.FormatIconClass = item.FormatClass;
                itemData.IncludedFormatClass = item.IncludedFormatClass;
                listItems.Add(itemData);
            }

            twilightContract.ProductItems = listItems;
            twilightContract.ListTitle = listItems.Count.ToString();
            twilightContract.IsPrimaryCartSet = isPrimaryCartSet;

            return twilightContract;
        }

        private Dictionary<string, int> InitItemQuantities(ProductSearchResults searchResults, string userId)
        {
            if (searchResults != null)
            {
                var primaryCart = CartFarmCacheHelper.GetPrimaryCart(userId);
                if (primaryCart != null)
                {
                    var searchResultItems = searchResults.Items;
                    var btKeys = searchResultItems.Select(productSearchResultItem => productSearchResultItem.BTKey).ToList();

                    var cartManager = new CartManager(userId);
                    return cartManager.GetQuantitiesByBtKeys(primaryCart.CartId, btKeys);
                }
            }
            return new Dictionary<string, int>();
        }

        private ProductSearchResults FindRelatedItems(string author, string btkey, string userId, bool hideNetPriceDiscountPercentage,
            AccountInfoForPricing accountPricingData, TargetingValues targeting, SearchByIdData searchData)
        {
            if (targeting == null || searchData == null)
                return null;
            var sAuthor = author;
            var searchArgs = new SearchArguments { PageSize = 6, StartRowIndex = 0 };

            var searchExpAuthor = new SearchExpression(SearchFieldNameConstants.responsiblepartyprimary, sAuthor, BooleanOperatorConstants.And);
            var searchExpStatus = new SearchExpression(SearchFieldNameConstants.productstatus, "A", BooleanOperatorConstants.And);

            searchArgs.SearchExpressionGroup.AddSearchExpress(searchExpAuthor);
            searchArgs.SearchExpressionGroup.AddSearchExpress(searchExpStatus);

            var sortExpression = new SortExpression { SortField = CommonHelper.Instance.ConvertSortFromGridToFast(SearchResultsSortField.SORT_POPULARITY) };
            sortExpression.SortDirection = SortDirection.Descending;
            searchArgs.SortExpressions.Add(sortExpression);

            var result = ProductSearchController.Search(searchArgs, targeting.MarketType, searchData.SimonSchusterEnabled, searchData.CountryCode,
                searchData.ESuppliers);

            //
            if (result != null && result.Items != null && result.Items.Count > 0)
            {
                foreach (var item in result.Items)
                {
                    if (item.BTKey == btkey)
                    {
                        result.Items.Remove(item);
                        break;
                    }
                }

                if (result.Items.Count > 5)
                {
                    result.Items.Remove(result.Items[result.Items.Count - 1]);
                }

                CalculatePrice(result, userId, searchData.ESuppliers, hideNetPriceDiscountPercentage, targeting,
                    accountPricingData);
            }
            else
                result = null;

            return result;
        }

        private void CalculatePrice(ProductSearchResults result, string userId, string[] ESuppliers, bool hideNetPriceDiscountPercentage, TargetingValues targeting, AccountInfoForPricing accountPricingData)
        {
            try
            {
                if (result == null) return;
                var lineItemUpdateds = new List<BasketLineItemUpdated>();
                var searchResultItems = result.Items;
                var realtimePricingHelper = new RealTimePricingHelper();
                var marketType = targeting.MarketType ?? MarketType.Any;
                var audienceType = CommonHelper.Instance.ConvertAudienceTypeAsString(targeting.AudienceType);
                foreach (var prodItem in searchResultItems)
                {
                    var accountInfo = realtimePricingHelper.GetAccountInfoForPricing(prodItem.ProductType, prodItem.ESupplier, prodItem.ProductFormat,
                        userId, accountPricingData);

                    lineItemUpdateds.Add(new BasketLineItemUpdated()
                    {
                        ISBN = prodItem.ISBN,
                        BTKey = prodItem.BTKey,
                        SoldToId = userId,
                        AccountId = accountInfo.AccountId,
                        ProductType = prodItem.ProductType,
                        TotalLineQuantity = 1,
                        TotalOrderQuantity = 1,
                        PriceKey = prodItem.PriceKey,
                        ListPrice = prodItem.ListPrice,
                        AccountPricePlan = accountInfo.AccountPricePlanId,
                        ProductLine = prodItem.ProductLine,
                        ReturnFlag = prodItem.HasReturn,
                        AccountERPNumber = accountInfo.ErpAccountNumber,
                        PrimaryWarehouse = accountInfo.PrimaryWarehouseCode,
                        ProductCatalog = prodItem.Catalog,
                        MarketType = marketType.ToString(),
                        AudienceType = audienceType,
                        EMarket = accountInfo.EMarketType,
                        ETier = accountInfo.ETier,
                        ProductPriceChangedIndicator = true,
                        ContractChangedIndicator = true,
                        PromotionChangedIndicator = false,
                        PromotionActiveIndicator = false,
                        QuantityChanged = true,
                        IsHomeDelivery = accountInfo.IsHomeDelivery,
                        Upc = prodItem.Upc,
                        AcceptableDiscount = prodItem.AcceptableDiscount,
                        NumberOfBuildings = accountInfo.BuildingNumbers,
                        ProcessingCharges = accountInfo.ProcessingCharges,
                        SalesTax = accountInfo.SalesTax,
                        IsVIPAccount = accountInfo.IsVIPAccount
                    });
                }
                var pricingController = new PricingController();
                //4428: For Search Page Only and for Retail Customers Only we will pass a Static order quantity of 5 
                //and a line quantity of 5 in the product search.  Applicable to both Basic Search and Advanced Search.  Hardcoded to Pricing.            
                //TFS#4949 - Show or hide net price   
                //10297: apply 4428 for any market type
                var itemPricings = pricingController.CalculatePrice(lineItemUpdateds, (marketType == MarketType.Retail)
                                                                               ? 5
                                                                               : 1, targeting, hideNetPriceDiscountPercentage);
                //
                var galeLiteral = CommonHelper.GetESupplierTextFromSiteTerm(AccountType.GALEE.ToString());
                if (itemPricings != null)
                {
                    var isExistGaleAccount = CommonHelper.IsESupplierAccountExisted(AccountType.GALEE.ToString(), userId);
                    foreach (var itemPricing in itemPricings)
                    {
                        var prodItem = searchResultItems.FirstOrDefault(x => x.BTKey == itemPricing.BTKey);
                        if (prodItem != null)
                        {
                            prodItem.ListPrice = itemPricing.ListPrice.HasValue ? itemPricing.ListPrice.Value : 0;
                            prodItem.DiscountPrice = itemPricing.SalePrice.HasValue ? itemPricing.SalePrice.Value : 0;
                            prodItem.DiscountPercent = itemPricing.DiscountPercent.HasValue ? itemPricing.DiscountPercent.Value : 0;
                            if (prodItem.ESupplier == galeLiteral)
                            {
                                var na = CommonResources.NA; // ResourceHelper.GetLocalizedString(ResourceName.Common.ToString(), "NA");
                                var salePrice = itemPricing.SalePrice.HasValue ? itemPricing.SalePrice.Value : 0;
                                var priceText = CommonHelper.Instance.DeterminePriceForGaleProduct(salePrice.ToString(), isExistGaleAccount,
                                    ESuppliers);
                                if (priceText == na)
                                {
                                    prodItem.ListPrice = -1;
                                    prodItem.DiscountPrice = -1;
                                    prodItem.DiscountPercent = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.CommonControl);
            }
        }

        #region private

        private UpdateItemResult CheckItemUpdateResult()
        {
            try
            {
                var updateItemResult = CachingController.Instance.Read("UpdateItemOrderingInformationResult") as UpdateItemResult;
                return updateItemResult;
            }
            finally
            {
                CachingController.Instance.SetExpired("UpdateItemOrderingInformationResult");
            }
        }

        private async Task<List<AdditionalVersionContract>> GetAdditionalVersions(string btKey, string eSupplier, string[] scDeafaultESuppliersAccount)
        {
            var result = new List<AdditionalVersionContract>();

            if (string.IsNullOrEmpty(eSupplier)) return result;

            if (scDeafaultESuppliersAccount != null && scDeafaultESuppliersAccount.Length > 0)
            {
                foreach (var eSupplierAccount in scDeafaultESuppliersAccount)
                {
                    if (string.IsNullOrEmpty(eSupplierAccount))
                    {
                        continue;
                    }
                    var account = ProfileController.Instance.GetAccountById(eSupplierAccount);// AdministrationProfileController.Current.GetAccountById(eSupplierAccount);

                    var eSupplierKey = CommonHelper.ConvertESupplierNameToValue(eSupplier);

                    if (account != null &&
                        (string.Compare(account.ESupplier, eSupplierKey, StringComparison.OrdinalIgnoreCase) == 0 ||
                         string.Compare(account.ESupplier, eSupplier, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        var additionalVersions = await ProductCatalogDAOManager.Instance.GetAdditionalVersions(btKey, account.EMarketType, account.ETier);
                        if (additionalVersions != null)
                        {
                            result.AddRange(
                                additionalVersions.Select(
                                    additionalVersion =>
                                    new AdditionalVersionContract(additionalVersion.ESupplier,
                                                                  additionalVersion.PhysicalFormat,
                                                                  additionalVersion.FormDetail,
                                                                  additionalVersion.ListPrice)));
                        }
                        break;
                    }
                }
            }
            return result;
        }

        private async Task<string> GeneratePrimaryGridDataLink(string btKey, string userId, string orgId, bool isItemDetailsContext, string cartId)
        {
            var primaryCart = CartFarmCacheHelper.GetPrimaryCart(userId);
            if (primaryCart == null) return string.Empty;
            var isPrimaryCart = string.Compare(cartId, primaryCart.CartId, StringComparison.OrdinalIgnoreCase) == 0;
            string span = "";
            var primaryCartBtkeyLineItemId = await CartDAOManager.Instance.GetLineItemBtKeys(primaryCart.CartId);
            string lineItemId;
            if (primaryCartBtkeyLineItemId != null &&
                (!string.IsNullOrEmpty(btKey) && primaryCartBtkeyLineItemId.TryGetValue(btKey, out lineItemId)))
            {
                if (!string.IsNullOrEmpty(lineItemId) && isItemDetailsContext && !isPrimaryCart)
                {
                    //var currentContext = SiteContext.Current;
                    //var userId = currentContext.UserId;
                    //var orgid = currentContext.OrganizationId;

                    span = string.Format("<span style=\"cursor: pointer;\" onclick=\"showPrimaryGridData(\'{0}\',\'{1}\',\'{2}\',\'{3}\',\'{4}\')\">{5}{6}{7}</span>",
                        primaryCart.CartId, userId, btKey, lineItemId, orgId, ProductSupportedHtmlTag.DupBeginImage, ProductSupportedHtmlTag.PrimaryCartGridData,
                        ProductSupportedHtmlTag.DupEndImage);
                }
                else if (!string.IsNullOrEmpty(lineItemId) && !isPrimaryCart)
                {
                    //var currentContext = SiteContext.Current;
                    //var userId = currentContext.UserId;
                    //var orgid = currentContext.OrganizationId;

                    span = string.Format(
                    "<span style=\"cursor: pointer;\" onclick=\"showPrimaryGridData(\'{0}\',\'{1}\',\'{2}\',\'{3}\',\'{4}\')\">{5}{6}{7}</span>",
                     primaryCart.CartId, userId, btKey, lineItemId, orgId, ProductSupportedHtmlTag.DupBeginImage,
                    ProductSupportedHtmlTag.PrimaryCartGridData, ProductSupportedHtmlTag.DupEndImage);
                }
            }
            return span;
        }

        /// <summary>
        /// Gets Primary Cart LineItems For AltFormats. Helps to show P icons in ALT Formats tab.
        /// </summary>
        /// <param name="btKeyList"></param>
        /// <param name="primaryCartId"></param>
        /// <returns></returns>
        public async Task<List<SiteTermObject>> GetPrimaryCartLineItemsForAltFormats(List<string> btKeyList, string primaryCartId)
        {
            var resultList = new List<SiteTermObject>();

            if (!string.IsNullOrEmpty(primaryCartId))
            {
                // get lineitem IDs from primary cart
                var primaryLineItems = await CartDAOManager.Instance.GetLineItemBtKeys(primaryCartId);
                if (primaryLineItems != null && primaryLineItems.Count > 0)
                {
                    string lineItemId = "";
                    foreach (var btKey in btKeyList)
                    {
                        if (!string.IsNullOrEmpty(btKey) && primaryLineItems.TryGetValue(btKey, out lineItemId))
                        {
                            var item = new SiteTermObject
                            {
                                Name = btKey,
                                Value = lineItemId
                            };
                            resultList.Add(item);
                        }
                    }
                }
            }

            return resultList;
        }

        private async Task<ESPRankContract> GetESPRank(string lineItemId)
        {
            var espRank = new ESPRankContract();
            //var cartManager = CartContext.Current.GetCartManagerForUser(SiteContext.Current.UserId);
            //if (cartManager != null)
            {
                var espRankDetailInfo = await CartDAOManager.Instance.GetESPRankItemDetailsByID(lineItemId);
                if (espRankDetailInfo != null)
                {
                    espRank.HasESPRanking = espRankDetailInfo.HasESPRanking;
                    espRank.OverallRank = espRankDetailInfo.OverallRank;
                    espRank.BisacRank = espRankDetailInfo.BisacRank;
                    espRank.ESPDetailUrl = espRankDetailInfo.DetailUrl;
                    espRank.ESPDetailWidth = espRankDetailInfo.ESPDetailWidth;
                    espRank.ESPDetailHeight = espRankDetailInfo.ESPDetailHeight;
                    espRank.ESPCategoryName = CommonHelper.GetESPCategoryName(espRankDetailInfo.ESPCategoryType);
                }
            }

            return espRank;
        }

        //public async Task<AppServiceResult<ProductDetail>> GetProductDetail(string btKey, SiteContextObject siteContext)
        //{
        //    var result = new AppServiceResult<ProductDetail>();
        //    var product = new ProductDetail();
        //    try
        //    {
        //        // TFS 20719. Ivor approved to get annotation from DB instead of FAST
        //        // because FAST uses '#' as delimiter between annotations but content of annotation may contain '#' character.
        //        string btAnno = await ProductDAO.Instance.GetFirstProductAnnotation(btKey);
        //        product.Annotaion = Sanitizer.GetSafeHtmlFragment(HttpUtility.HtmlDecode(btAnno));

        //        // Get product DAO object
        //        var cacheKey = string.Format("ProductDetailsInfo_{0}", btKey);
        //        var productDetailsObject = CachingController.Instance.Read(cacheKey) as ProductDetailsObject;
        //        if (productDetailsObject == null)
        //        {
        //            productDetailsObject = await ProductDAO.Instance.GetProductInformation(btKey);
        //            CachingController.Instance.Write(cacheKey, productDetailsObject);
        //        }

        //        if (productDetailsObject != null)
        //        {
        //            GetDataForSections(product, productDetailsObject, btKey, OCLCCatalogingPlusEnabled, userId);
        //        }

        //        result.Status = AppServiceStatus.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Status = AppServiceStatus.Fail;
        //        result.ErrorMessage = ex.Message;
        //        result.Data = product;
        //        Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
        //    }
        //    result.Data = product;
        //    return result;
        //}

        private void GetDataForSections(ProductDetail product, ProductDetailsObject productDetailsObject, string btKey,
            bool OCLCCatalogingPlusEnabled, string userId, bool allowBTEmployee)
        {
            var list = ListItemDetailSettings.GetItemDetailConfigVisibleSettings();
            if (list == null || list.Count < 1)
                return;

            foreach (ListItemDetailConfigurationItem item in list)
            {
                SetupSection(product, btKey, item, productDetailsObject, OCLCCatalogingPlusEnabled, userId, allowBTEmployee);
            }
        }

        private void SetupSection(ProductDetail product, string btKey, ListItemDetailConfigurationItem item, ProductDetailsObject productDetailsObject, bool OCLCCatalogingPlusEnabled, string userId, bool allowBTEmployee)
        {
            if (productDetailsObject != null)
            {
                IList<ListItemDetailFieldItem> listFieldId = ListItemDetailSettings.GetItemDetailField(item.SectionID ?? 1);
                var section = item.SectionValue;
                bool hasValue;
                switch (section)
                {
                    case ProductDetailSectionName.PRODUCT_BTPUBLICATIONS:
                        var btPublications = RealDataForOtherSections(listFieldId, productDetailsObject.BTPublications, "BTPublications");
                        if (btPublications != null && btPublications.Count > 0)
                        {
                            var sb = new StringBuilder();
                            foreach (string att in btPublications)
                            {
                                var strLink = String.Format("{0}?{1}={2}", SiteUrl.PublicationProducts, SearchFieldNameConstants.publicationsubcategoryid, att);
                                sb.AppendFormat("<a href=\"{0}\">{1}</a><br />", strLink, att);
                            }
                            product.BTPublications = sb.ToString().TrimEnd(new char[] { '<', 'b', 'r', ' ', '/', '>' });
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_BTPROGRAMS:
                        var btPrograms = RealDataForOtherSections(listFieldId, productDetailsObject.BTPrograms, "BTPrograms");
                        if (btPrograms != null && btPrograms.Count > 0)
                        {
                            string btProgram = string.Empty;
                            foreach (string att in btPrograms)
                            {
                                btProgram += string.Format("{0}<br />", att);
                            }
                            product.BTPrograms = btProgram;
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_INFORMATION:
                        var pi = RealDataForGeneralProductInfo(listFieldId, productDetailsObject.GeneralInfo,
                            out hasValue, OCLCCatalogingPlusEnabled);
                        if (pi != null && pi.Count > 0 && hasValue)
                            product.ProductInformation = pi;
                        break;
                    case ProductDetailSectionName.PRODUCT_ACADEMICMODIFIERS:
                        var am = RealDataForGeneralProductInfo(listFieldId, productDetailsObject.GeneralInfo,
                            out hasValue, OCLCCatalogingPlusEnabled);
                        if (am != null && am.Count > 0 && hasValue)
                            product.AcademicModifiers = am;
                        break;
                    case ProductDetailSectionName.PRODUCT_ACADEMICSUBJECTS: // "AcademicSubjects":
                        var listAS = RealDataForOtherSections(listFieldId, productDetailsObject.AcademicSubjects, "AcademicSubjects");
                        if (listAS != null && listAS.Count > 0)
                        {
                            foreach (string att in listAS)
                            {
                                string strLink = SearchHelper.CreateSearchResultLink(SearchFieldNameConstants.academicsubjects, att);
                                product.AcademicSubjects += string.Format("<a href={0}>{1}</a><br />", strLink, att);
                            }
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_ACCELERATEDREADER:
                        var lstAR = RealDataForGeneralProductInfo(listFieldId, productDetailsObject.GeneralInfo, out hasValue, OCLCCatalogingPlusEnabled);
                        if (lstAR != null && lstAR.Count > 0 && hasValue)
                        {
                            // add Topic section to ACCELERATEDREADER section
                            if (productDetailsObject.AcceleratedTopics != null && productDetailsObject.AcceleratedTopics.Count > 0)
                            {
                                string topic = string.Join(", ", productDetailsObject.AcceleratedTopics.ToArray());
                                var topicItemData = new ItemData(topic, "Topics");
                                lstAR.Add(topicItemData);
                            }
                            product.AcceleratedReader = lstAR;
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_AWARDS:
                        var lstA = RealDataForOtherSections(listFieldId,
                            productDetailsObject.Awards, "Awards");
                        if (lstA != null && lstA.Count > 0)
                        {
                            string award = string.Empty;
                            foreach (string att in lstA)
                                award += string.Format("{0}<br />", att);
                            product.Awards = award;
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_BIBLIOGRAPHY:
                        var lstB = RealDataForOtherSections(listFieldId, productDetailsObject.Bibliographies, "Bibliographies");
                        if (lstB != null && lstB.Count > 0)
                        {
                            string bibliography = string.Empty;
                            foreach (string att in lstB)
                                bibliography += string.Format("{0}<br />", att);
                            product.Bibliography = bibliography;
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_BISACSUBJECTS:
                        var lstBS = RealDataForOtherSections(listFieldId, productDetailsObject.BISACSubjects, "BISACSubjects");
                        if (lstBS != null && lstBS.Count > 0)
                        {
                            var sb = new StringBuilder();
                            foreach (string att in lstBS)
                            {
                                string strLink = SearchHelper.CreateSearchResultLink(SearchFieldNameConstants.subject1, att);
                                sb.AppendFormat("<a href={0}>{1}</a><br />", strLink, att);
                            }
                            product.BISACSubjects = sb.ToString().TrimEnd(new char[] { '<', 'b', 'r', ' ', '/', '>' });
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_BTSPECIFICDATA:
                        var lstBTSD = RealDataForGeneralProductInfo(listFieldId, productDetailsObject.GeneralInfo, out hasValue, OCLCCatalogingPlusEnabled);
                        if (lstBTSD != null && lstBTSD.Count > 0 && hasValue)
                            product.BTSpecificData = lstBTSD;
                        break;
                    case ProductDetailSectionName.PRODUCT_CLASSIFICATION:
                        var lstCLS = RealDataForGeneralProductInfo(listFieldId, productDetailsObject.GeneralInfo,
                            out hasValue, OCLCCatalogingPlusEnabled);
                        if (lstCLS != null && lstCLS.Count > 0 && hasValue)
                            product.Classification = lstCLS;
                        break;
                    case ProductDetailSectionName.PRODUCT_GENERALSUBJECTS:
                        var lstGS = RealDataForGeneralProductInfo(listFieldId, productDetailsObject.GeneralInfo, out hasValue, OCLCCatalogingPlusEnabled);
                        if (lstGS != null && lstGS.Count > 0 && hasValue)
                        {
                            string strGenSubject = string.Empty;
                            foreach (var gs in lstGS)
                            {
                                if (gs != null && !string.IsNullOrEmpty(gs.ItemDataValue))
                                {
                                    var strLink = SearchHelper.CreateSearchResultLink(SearchFieldNameConstants.generalsubjects, gs.ItemDataValue);
                                    strGenSubject += string.Format("<a href={0}>{1}</a>; ", strLink, gs.ItemDataValue);
                                }
                            }
                            if (!string.IsNullOrEmpty(strGenSubject))
                            {
                                product.GeneralSubjects = strGenSubject.Trim().TrimEnd(';');
                            }
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_PAYPERCIRCCOLLECTION:
                        var lstPPC = RealDataForOtherSections(listFieldId, productDetailsObject.PayPerCirCollections, "PayPerCircCollection");
                        if (lstPPC != null && lstPPC.Count > 0)
                        {
                            var sb = new StringBuilder();
                            foreach (var ppc in lstPPC)
                            {
                                string strLink = SearchHelper.CreateSearchResultLink(SearchFieldNameConstants.PPCAuxCodes, ppc.AuxCode);
                                sb.AppendFormat("<a href={0}>{1}</a><br />", strLink, ppc.AuxDescription);
                            }
                            product.PayPerCircCollections = sb.ToString().TrimEnd(new char[] { '<', 'b', 'r', ' ', '/', '>' });
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_LIBRARYSUBJECTS:
                        var lstLSub = RealDataForOtherSections(listFieldId, productDetailsObject.LibrarySubjects, "LibrarySubjects");
                        if (lstLSub != null && lstLSub.Count > 0)
                        {
                            var sb = new StringBuilder();
                            for (var i = 0; i < lstLSub.Count; i++)
                            {
                                var strLink = SearchHelper.CreateSearchResultLink(SearchFieldNameConstants.librarysubjects, lstLSub[i]);
                                if (i < lstLSub.Count - 1)
                                    sb.AppendFormat("<a href={0}>{1}</a><br /> ", strLink, lstLSub[i]);
                                else
                                    sb.AppendFormat("<a href={0}>{1}</a>", strLink, lstLSub[i]);
                            }
                            product.LibrarySubjects = sb.ToString();
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_LOCATION:
                        var lstDL = RealDataForGeneralProductInfo(listFieldId, productDetailsObject.GeneralInfo, out hasValue, OCLCCatalogingPlusEnabled);
                        if (lstDL != null && lstDL.Count > 0 && hasValue)
                            product.DetailLocation = lstDL;
                        break;
                    case ProductDetailSectionName.PRODUCT_OTHERCITATIONS:
                        // check if BT User
                        if (allowBTEmployee)
                        {
                            var lstOC = RealDataForOtherSections(listFieldId, productDetailsObject.OtherCitations, "OtherCitations");

                            if (lstOC != null && lstOC.Count > 0)
                            {
                                string otherCitations = string.Empty;
                                foreach (string att in lstOC)
                                    otherCitations += string.Format("{0}<br />", att);
                                product.OtherCitations = otherCitations;
                            }
                        }

                        break;
                    case ProductDetailSectionName.PRODUCT_PHYSICAL:
                        var lstPP = RealDataForGeneralProductInfo(listFieldId, productDetailsObject.GeneralInfo, out hasValue, OCLCCatalogingPlusEnabled);
                        if (lstPP != null && lstPP.Count > 0 && hasValue)
                        {
                            // add Attributes section to PRODUCT_PHYSICAL section
                            if (productDetailsObject.Attributes != null && productDetailsObject.Attributes.Count > 0)
                            {
                                string atts = string.Join("<br />", productDetailsObject.Attributes.ToArray());
                                var attItemData = new ItemData(atts, "Attributes");
                                lstPP.Add(attItemData);
                            }
                            for (int i = 0; i < lstPP.Count; i++)
                                if (lstPP[i].IsChild)
                                    lstPP[i].ItemDataText = "<span class='pad-left10'>" + lstPP[i].ItemDataText + "</span>";
                            product.Physical = lstPP;
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_READINGCOUNT:
                        var lstRC = RealDataForGeneralProductInfo(listFieldId, productDetailsObject.GeneralInfo, out hasValue, OCLCCatalogingPlusEnabled);
                        if (lstRC != null && lstRC.Count > 0 && hasValue)
                            product.ReadingCount = lstRC;
                        break;
                    case ProductDetailSectionName.PRODUCT_REVIEWCITATIONS:
                        //Product Reviews
                        var lstRCi = RealDataForOtherSections(listFieldId, productDetailsObject.ReviewCitations, "ReviewCitations");

                        //Organization Reviews sorted by Sequence in MyPreferences
                        var reviews = GetProductDetailReview(btKey, userId);

                        if (lstRCi != null && lstRCi.Count > 0)
                        {
                            //Sort lstRCi
                            var resultRci = new List<string>();
                            var proccessedRci = new List<ReviewCitationObject>();

                            foreach (var content in reviews)
                            {
                                var lstRCiItems = lstRCi.Where(r => !proccessedRci.Contains(r) && r.ReviewId == content.ReviewTypeId).ToList();
                                if (lstRCiItems.Count > 0)
                                {
                                    //Sort duplicated items
                                    lstRCiItems.Sort(SortReviewCitationsByName);

                                    //add active items sorted by Sequence
                                    resultRci.AddRange(lstRCiItems.Select(x => x.ReviewCitation));
                                    proccessedRci.AddRange(lstRCiItems);
                                }
                            }

                            //Get inactive items
                            var inactiveRCis = lstRCi.Where(x => !proccessedRci.Contains(x)).ToList();
                            if (inactiveRCis.Count > 0)
                            {
                                //Sort inactive items by name
                                inactiveRCis.Sort(SortReviewCitationsByName);
                                //Add to result
                                resultRci.AddRange(inactiveRCis.Select(x => x.ReviewCitation));
                            }

                            //Add to GUI
                            for (int j = 0; j < resultRci.Count; j++)
                            {
                                var bookmarkIndex = 0;
                                var linkCaption = resultRci[j];
                                var hasReview = false;

                                if (reviews != null)
                                {
                                    for (var i = 0; i < reviews.Count; i++)
                                    {
                                        if (linkCaption.ToUpper().Equals(reviews[i].Title.ToUpper()))
                                        {
                                            hasReview = true;
                                            bookmarkIndex = i;
                                        }
                                    }
                                }

                                if (hasReview)
                                {
                                    var strScripts = string.Format("javascript:ToReviewTab({0}); return false;", bookmarkIndex);
                                    resultRci[j] = string.Format("<a onclick=\"{0}\" href=''>{1}</a>", strScripts, linkCaption);
                                }
                            }
                            product.ReviewCitations = resultRci;
                        }
                        break;
                    case ProductDetailSectionName.PRODUCT_SERIES:
                        var lstSeries = RealDataForGeneralProductInfo(listFieldId, productDetailsObject.ContinuationsSeries, out hasValue, OCLCCatalogingPlusEnabled);
                        if (lstSeries != null && lstSeries.Count > 0 && hasValue)
                            product.Series = lstSeries;
                        break;
                }
            }
        }

        /// <summary>
        /// Reals the data for other sections.
        /// </summary>
        /// <param name="listField">The list field.</param>
        /// <param name="sectionData">The section data.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        private static List<string> RealDataForOtherSections(IEnumerable<ListItemDetailFieldItem> listField, List<string> sectionData,
            string sectionName)
        {
            var result = new List<string>();

            if (sectionData != null && listField != null)
            {
                if (listField.Any(item => item.FieldKey == sectionName))
                {
                    return sectionData;
                }
            }

            return result;
        }

        private static List<PPCSubscription> RealDataForOtherSections(IEnumerable<ListItemDetailFieldItem> listField, List<PPCSubscription> sectionData, string sectionName)
        {
            var result = new List<PPCSubscription>();
            if (sectionData != null && listField != null)
            {
                if (listField.Any(item => item.FieldKey == sectionName))
                {
                    return sectionData;
                }
            }
            return result;
        }
        /// <summary>
        /// Reals the data for other sections.
        /// </summary>
        /// <param name="listField">The list field.</param>
        /// <param name="sectionData">The section data.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        private static List<ReviewCitationObject> RealDataForOtherSections(IEnumerable<ListItemDetailFieldItem> listField, List<ReviewCitationObject> sectionData, string sectionName)
        {
            var result = new List<ReviewCitationObject>();

            if (sectionData != null && listField != null)
            {
                if (listField.Any(item => item.FieldKey == sectionName))
                {
                    return sectionData;
                }
            }

            return result;
        }

        private static void GetFormatAttr(List<ItemData> result, ListItemDetailFieldItem item, string itemValue)
        {
            var doc = new XmlDocument();
            doc.LoadXml(itemValue);
            var physicalNodes = doc.GetElementsByTagName("PhysicalFormat");

            for (var i = 0; i < physicalNodes.Count; i++)
            {
                var isPrimary = false;
                var attrCollection = physicalNodes[i].Attributes;
                if (attrCollection != null)
                    foreach (XmlAttribute xmlAttr in attrCollection)
                    {
                        if (xmlAttr.Name.ToLower() == "primary" && xmlAttr.Value.ToLower() == "y")
                        {
                            isPrimary = true;
                        }
                    }

                result.AddRange(from XmlNode child in physicalNodes[i].ChildNodes
                                where child.Name.Equals("FormatName")
                                let strTitle = isPrimary ? item.Title : "Included Format"
                                select new ItemData
                                {
                                    ItemDataValue = child.InnerText,
                                    ItemDataText = strTitle,
                                    IsChild = false
                                });
            }
        }

        private static List<ItemData> RealDataForGeneralProductInfo(IEnumerable<ListItemDetailFieldItem> listField, Dictionary<string, string> sectionData,
            out bool hasValue, bool OCLCCatalogingPlusEnabled)
        {
            var result = new List<ItemData>();

            hasValue = false;

            if (sectionData != null && listField != null)
            {
                foreach (var item in listField)
                {
                    if (sectionData.ContainsKey(item.FieldKey))
                    {
                        // if sectionData contains key, value of this key certainly is not null or empty string
                        // because sectionData has filtered before
                        string itemValue = sectionData[item.FieldKey].Trim();
                        if (!hasValue)// && !string.IsNullOrEmpty(itemValue))
                        {
                            hasValue = true;
                        }

                        if (!string.IsNullOrEmpty(itemValue))
                        {
                            switch (item.FieldKey)
                            {
                                case "BTPhysicalFormatsAttributesEditions":
                                    GetFormatAttr(result, item, itemValue);
                                    break;
                                case "RetailPrice":
                                    result.Add(new ItemData
                                    {
                                        ItemDataValue = CommonHelper.GetCurrencyFormat(itemValue),
                                        ItemDataText = item.Title,
                                        IsChild = false
                                    });
                                    break;
                                case "InitialPrintRun":
                                case "RunTime":
                                case "Volumes":
                                    if (itemValue != "0")
                                    {
                                        result.Add(new ItemData
                                        {
                                            ItemDataValue = itemValue,
                                            ItemDataText = item.Title,
                                            IsChild = false
                                        });
                                    }
                                    break;
                                case "ProductStatus":
                                    result.Add(new ItemData
                                    {
                                        ItemDataValue = itemValue,
                                        ItemDataText = item.Title,
                                        IsChild = false
                                    });
                                    break;
                                case "OCLCNumber":
                                    //if (SiteContext.Current.OCLCCatalogingPlusEnabled)
                                    if (OCLCCatalogingPlusEnabled)
                                    {
                                        result.Add(new ItemData
                                        {
                                            ItemDataValue = itemValue,
                                            ItemDataText = item.Title,
                                            IsChild = false
                                        });
                                    }
                                    break;
                                case "PriceKey":// Discount key
                                    result.Add(new ItemData
                                    {
                                        ItemDataValue = itemValue,
                                        ItemDataText = item.Title,
                                        IsChild = false
                                    });
                                    break;
                                case "LastReturnDate":
                                    if (!string.IsNullOrEmpty(itemValue) && itemValue != "01/01/1900")
                                    {
                                        result.Add(new ItemData
                                        {
                                            ItemDataValue = itemValue,
                                            ItemDataText = item.Title,
                                            IsChild = false
                                        });
                                    }
                                    break;
                                case "ContinuationSeries":
                                    string standingOrders = string.Empty;
                                    var arrIDName = itemValue.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var idname in arrIDName)
                                    {
                                        var group = idname.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (group.Length > 1)
                                        {
                                            var id = group[0];
                                            var name = group[1];
                                            string strLink = SearchHelper.CreateSearchResultLink(SearchFieldNameConstants.standingOrderID, id);
                                            standingOrders += string.Format("<a href={0}>{1}</a>", strLink, id);
                                            standingOrders += " / " + name + "</BR>";
                                        }
                                    }
                                    if (standingOrders.Length > 0)
                                        standingOrders.Remove(standingOrders.Length - 4);
                                    result.Add(new ItemData
                                    {
                                        ItemDataValue = standingOrders,
                                        ItemDataText = item.Title,
                                        IsChild = false
                                    });
                                    break;
                                default:
                                    result.Add(new ItemData
                                    {
                                        ItemDataValue = itemValue,
                                        ItemDataText = item.Title,
                                        IsChild = false
                                    });
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private List<AdditionContent> GetProductDetailReview(string btKey, string userId)
        {
            var userProfile = ProfileService.Instance.GetUserById(userId);
            if (userProfile == null)
            {
                return new List<AdditionContent>();
            }
            var organization = ProfileService.Instance.GetOrganizationById(userProfile.OrgId);// userProfile.OrganizationEntity;

            string[] reviewTypeList;
            if (organization != null && organization.ReviewTypeList != null)
            {
                reviewTypeList = organization.ReviewTypeList;
            }
            else
            {
                reviewTypeList = new string[] { };
            }
            //Encode
            var reviewTypeListTmp = new string[reviewTypeList.Length];
            for (var i = 0; i < reviewTypeList.Length; i++)
            {
                reviewTypeListTmp[i] = CommonHelper.Instance.Decode(reviewTypeList[i]);
            }
            //Encode
            var result = ProductDAO.Instance.GetProductReviews(btKey, reviewTypeListTmp);

            //Assign the sequence
            userProfile.MyReviewTypes = ProfileService.Instance.GetUserReviewTypes(userId, userProfile.MyReviewTypeIds);
            var userReviewTypes = userProfile.MyReviewTypes;
            if (result != null && result.Count > 0 && userReviewTypes != null)
            {
                foreach (var rv in result)
                {
                    var rt =
                        userReviewTypes.FirstOrDefault(r => CommonHelper.Instance.Decode(r.ReviewTypeId) == rv.ReviewTypeId);

                    if (rt != null)
                    {
                        rv.Sequence = rt.Ordinal;
                        rv.DisplayName = SiteTermHelper.Instance.GetSiteTermName(SiteTermName.ReviewType,
                                                                           CommonHelper.Instance.Encode(rt.ReviewTypeId));
                    }
                }

                result = SortReviewTypes(result, true /*Add items with null Sequences to top*/);
            }
            return result;
        }
        /// <summary>
        /// Sorts the review types.
        /// </summary>
        /// <param name="reviews">The reviews.</param>
        /// <param name="reverse"></param>
        /// <returns></returns>
        public List<AdditionContent> SortReviewTypes(List<AdditionContent> reviews, bool reverse = false)
        {
            //Check null
            if (reviews == null || reviews.Count <= 0) return new List<AdditionContent>();

            //Separate into 2 list: 1. a list has Sequence & 2. a list has no Sequence
            var unRecognizeItem = reviews.Where(r => string.IsNullOrEmpty(r.Sequence)).ToList(); //get List of item has no sequence
            reviews.RemoveAll(r => string.IsNullOrEmpty(r.Sequence));//get list has Sequence

            //Initialize DisplayName
            foreach (var additionContent in unRecognizeItem)
            {
                additionContent.DisplayName = SiteTermHelper.Instance.GetSiteTermName(SiteTermName.ReviewType,
                                                                               additionContent.ReviewTypeId);
            }

            //sort items by DisplayName
            unRecognizeItem.Sort(CompareMyReviewTypeByDisplayName);
            //Sort by Sequence Number
            reviews.Sort(CompareMyReviewType);
            //Combine 2 list
            if (reverse) reviews.InsertRange(0, unRecognizeItem);
            else reviews.AddRange(unRecognizeItem);

            return reviews;
        }
        /// <summary>
        /// Compares the display name of my review type by.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static int CompareMyReviewTypeByDisplayName(AdditionContent x, AdditionContent y)
        {
            if (string.IsNullOrEmpty(x.DisplayName))
            {
                if (string.IsNullOrEmpty(y.DisplayName))
                {
                    return 0; //equal
                }
                return -1; // x is less than y
            }
            if (string.IsNullOrEmpty(y.DisplayName))
            {
                return 1; //x is greater than y
            }

            return x.DisplayName.CompareTo(y.DisplayName);
        }
        /// <summary>
        /// Compares the type of my review.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static int CompareMyReviewType(AdditionContent x, AdditionContent y)
        {
            if (string.IsNullOrEmpty(x.Sequence))
            {
                if (string.IsNullOrEmpty(y.Sequence))
                {
                    return CompareMyReviewTypeByDisplayName(x, y); //equal
                }
                return -1; // x is less than y
            }
            if (string.IsNullOrEmpty(y.Sequence))
            {
                return 1; //x is greater than y
            }
            var xOrdinal = Int32.Parse(x.Sequence);
            var yOrdinal = Int32.Parse(y.Sequence);
            return xOrdinal.CompareTo(yOrdinal);
        }

        /// <summary>
        /// Sorts the name of the review citations by.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        private static int SortReviewCitationsByName(ReviewCitationObject x, ReviewCitationObject y)
        {
            if (x == null && y == null) return 0;
            if (x != null && y == null) return 1;
            if (x != null)
            {
                if (string.IsNullOrEmpty(x.ReviewCitation) && string.IsNullOrEmpty(y.ReviewCitation)) return 0;
                if (!string.IsNullOrEmpty(x.ReviewCitation) && string.IsNullOrEmpty(y.ReviewCitation)) return 1;
                if (string.IsNullOrEmpty(x.ReviewCitation) && !string.IsNullOrEmpty(y.ReviewCitation)) return -1;
                return string.Compare(x.ReviewCitation, y.ReviewCitation);
            }
            return 0;
        }

        private AppServiceResult<string> GetInventoryForItemDetail(InventoryStatusArgContract arg, out int last30DaysDemand, out string inventoryStatus,
            out bool hasDemand, MarketType? marketType, string userId, string countryCode, string orgId)
        {
            last30DaysDemand = 0;
            hasDemand = false;
            inventoryStatus = "";
            var result = new AppServiceResult<string>();
            try
            {
                bool displayInventoryForAllWareHouse = false;
                string[] blockedCountryCodes = !string.IsNullOrEmpty(arg.BlockedExportCountryCodes)
                                                    ? arg.BlockedExportCountryCodes.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                    : null;

                var searchArg = new SearchResultInventoryStatusArg();
                searchArg.CatalogName = arg.CatalogName;
                searchArg.Flag = arg.Flag;
                searchArg.BTKey = arg.BTKey;
                searchArg.Quantity = arg.Quantity;
                searchArg.ProductType = arg.ProductType;
                searchArg.VariantId = arg.VariantId;
                searchArg.PublishDate = arg.PublishDate;
                searchArg.MerchandiseCategory = arg.MerchandiseCategory;
                searchArg.MarketType = arg.MarketType;
                searchArg.PubCodeD = arg.PubCodeD;
                searchArg.ESupplier = arg.ESupplier;
                searchArg.ReportCode = arg.ReportCode;
                searchArg.SupplierCode = arg.SupplierCode;
                searchArg.ForSingleItem = true;
                searchArg.BlockedExportCountryCodes = blockedCountryCodes;
                var inventoryHelper = InventoryHelper4MongoDb.GetInstance(arg.CartID, userId);
                inventoryHelper.MarketType = marketType;
                inventoryHelper.UserId = userId;
                inventoryHelper.CountryCode = countryCode;
                inventoryHelper.OrgId = orgId;

                List<InventoryStockStatus> inventoryStockStatus =
                    inventoryHelper.GetInventoryResultsForSingleItem(searchArg, false,
                        out displayInventoryForAllWareHouse, out last30DaysDemand, out hasDemand);


                if (inventoryStockStatus == null)
                {
                    result.Status = AppServiceStatus.Success;
                    result.ErrorMessage = "no data.";
                    result.ErrorCode = hasDemand ? "1" : "0";
                    result.Data = string.Empty;
                    return result;
                }
                var invStatus = inventoryHelper.GetInventoryStatus(new List<SearchResultInventoryStatusArg>() { searchArg });
                if (invStatus != null && invStatus.Any())
                {
                    inventoryStatus = invStatus[0].Value;
                }

                var odd = "row odd";
                var even = "row";
                var isOdd = false;
                var sb = new StringBuilder();

                var superWarehouseToolTip = "VIP Inventory available through B&T supply network";
                foreach (var item in inventoryStockStatus)
                {
                    if (item == null)
                    {
                    }
                    else
                    {
                        var oddRow = isOdd ? odd : even;

                        var isVipWhs = item.FormalWareHouseCode == InventoryWareHouseCode.SUP;


                        if (isVipWhs &&
                            (item.OnHandInventory == "0" && item.OnOrderQuantity == 0))
                        {
                            continue;
                        }

                        isOdd = !isOdd;

                        sb.Append(
                            new StringBuilder(HTMLTemplate.InventoryItemHTMLTemplate).Replace("{WareHouse}", item.WareHouse)
                                .Replace("{OnHandInventory}", item.OnHandInventory)
                                .Replace("{OnOrderQuantity}", item.OnOrderQuantity.ToString())
                                .Replace("{OddRow}", oddRow)
                                .Replace("{WarehouseToolTip}", isVipWhs ? superWarehouseToolTip : ""));
                    }
                }

                result.ErrorCode = hasDemand ? "1" : "0";
                result.Status = AppServiceStatus.Success;
                result.Data = sb.ToString();
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                result.Data = string.Empty;
            }
            return result;
        }

        private static ProductSearchResults GetProductSearchResultsFromCache(string userId)
        {
            var cacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SearchResult);
            var productSearchResults = CachingController.Instance.Read(cacheKey) as ProductSearchResults;
            return productSearchResults;
        }

        private SearchResultInventoryStatusArg GetSearchResultInventoryStatusArg(ProductSearchResultItem productInfo, string userMarketType)
        {
            return CommonHelper.Instance.GetSearchResultInventoryStatusArg(productInfo, userMarketType);
        }

        private static PricingClientArg GetPricingClientArg(ProductSearchResultItem item)
        {
            return new PricingClientArg
            {
                AcceptableDiscount = item.AcceptableDiscount,
                BTGTIN = item.GTIN,
                ISBN = item.ISBN,
                BTKey = item.BTKey,
                BTUPC = item.Upc,
                Catalog = item.Catalog,
                HasReturn = item.HasReturn.ToString(),
                ListPrice = item.ListPrice.ToString(),
                PriceKey = item.PriceKey,
                ProductLine = item.ProductLine,
                ProductType = item.ProductType,
                ProductFormat = item.ProductFormat,
                ESupplier = item.ESupplier,
                PurchaseOption = item.PurchaseOption.Trim()
            };
        }

        private static PromotionClientArg GetPromotionClientArg(ProductSearchResultItem item)
        {
            return new PromotionClientArg
            {
                BTKey = item.BTKey,
                Catalog = item.Catalog,
                ProductType = item.ProductType
            };
        }

        private static SiteTermName GetSiteTerm(string strSiteTerm)
        {
            SiteTermName st;
            switch (strSiteTerm)
            {
                case "AdvSearchBookFilterAttribute":
                    st = SiteTermName.AdvSearchBookFilterAttribute;
                    break;
                case "AdvSearchBookFilterARInterest":
                    st = SiteTermName.AdvSearchBookFilterARInterestLevel;
                    break;
                case "AdvSearchBookFilterFormat":
                    st = SiteTermName.AdvSearchBookFilterFormat;
                    break;
                case "AdvSearchBookFilterLanguage":
                    st = SiteTermName.AdvSearchBookFilterLanguague;
                    break;
                case "AdvSearchBookFilterLexileScale":
                    st = SiteTermName.AdvSearchBookFilterLexileScale;
                    break;
                case "AdvSearchBookFilterPublishDate":
                    st = SiteTermName.AdvSearchBookFilterPublishDate;
                    break;
                case "AdvSearchBookFilterPublisherStatus":
                    st = SiteTermName.AdvSearchBookFilterPublisherStatus;
                    break;
                case "AudienceTypes":
                    st = SiteTermName.AudienceTypes;
                    break;
                case "AdvSearchBookFilterReviewSource":
                    st = SiteTermName.ReviewType;
                    break;
                case "AdvSearchMusicFilterFormat":
                    st = SiteTermName.AdvSearchMusicFilterFormat;
                    break;
                case "AdvSearchMusicFilterGenre":
                    st = SiteTermName.AdvSearchMusicFilterGenre;
                    break;
                case "AdvSearchMovieFilterFormat":
                    st = SiteTermName.AdvSearchMovieFilterFormat;
                    break;
                case "AdvSearchMovieFilterGenre":
                    st = SiteTermName.AdvSearchMovieFilterGenre;
                    break;
                case "AdvSearchBookFilterProductAttribute":
                    st = SiteTermName.AdvSearchBookFilterProductAttribute;
                    break;
                case "AdvSearchEBookFilterProductAttribute":
                    st = SiteTermName.AdvSearchEBookFilterProductAttribute;
                    break;
                case "AdvSearchMusicMovieFilterProductAttribute":
                    st = SiteTermName.AdvSearchMusicMovieFilterProductAttribute;
                    break;
                case "AdvSearchBookFilterPurchaseOption":
                    st = SiteTermName.AdvSearchBookFilterPurchaseOption;
                    break;
                default:
                    st = SiteTermName.None;
                    break;
            }
            return st;
        }
        #endregion

        public AppServiceResult<InventoryDataContract> GetHomeLandingInventoryData(InventoryStatusClientRequest request)
        {
            var result = new AppServiceResult<InventoryDataContract>();

            var inventoryStatusArgList = request.inventoryStatusArgList;

            var dataReturn = new InventoryDataContract();

            if (inventoryStatusArgList == null || inventoryStatusArgList.Count == 0)
            {
                result.Status = AppServiceStatus.Fail;
                result.Data = dataReturn;
                result.ErrorMessage = "No data";
                return result;
            }

            var listArg = new List<SearchResultInventoryStatusArg>();
            foreach (var inventoryStatusArg in inventoryStatusArgList)
            {
                if (string.IsNullOrEmpty(inventoryStatusArg.Quantity))
                {
                    inventoryStatusArg.Quantity = "0";
                }
                var arg = new SearchResultInventoryStatusArg
                {
                    CatalogName = inventoryStatusArg.Catalog,
                    UserId = request.UserId,
                    BTKey = inventoryStatusArg.BTKey,
                    VariantId = "",
                    Flag = inventoryStatusArg.Pub,
                    Quantity = Convert.ToDecimal(inventoryStatusArg.Quantity),
                    ProductType = inventoryStatusArg.BTType,
                    PublishDate = Convert.ToDateTime(String.IsNullOrEmpty(inventoryStatusArg.PublishDate) ? null : inventoryStatusArg.PublishDate),
                    MerchandiseCategory = inventoryStatusArg.MerchandiseCategory,
                    MarketType = (request.MarketType ?? BT.TS360Constants.MarketType.Any).ToString(),
                    PubCodeD = inventoryStatusArg.PubCodeD,
                    ESupplier = inventoryStatusArg.ESupplier,
                    ReportCode = inventoryStatusArg.ReportCode
                };
                listArg.Add(arg);
            }

            var inventoryHelper = InventoryHelper4MongoDb.GetInstance(userId: request.UserId,
                    countryCode: request.CountryCode, marketType: request.MarketType, orgId: request.OrgId);

            dataReturn.InventoryResultsList = inventoryHelper.GetInventoryResultsForMultipleItems(listArg);
            dataReturn.InventoryStatus = inventoryHelper.InventoryStatusList;

            result.Status = AppServiceStatus.Success;
            result.Data = dataReturn;

            return result;
        }

        public AppServiceResult<bool> ToggleQuickView(ToggleQuickViewRequest request)
        {
            var result = new AppServiceResult<bool>();
            result.Data = true;


            var updatedValue = request.IsQuickMode;
            string cacheKey;

            switch (request.InPageEnumValue)
            {
                case 2: //InPage.SearchResults:
                    ProfileService.Instance.ToggleQuickSearchView(request.UserId, updatedValue);

                    cacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(request.UserId, "IsQuickSearchViewEnabled");
                    CachingController.Instance.Write(cacheKey, updatedValue);

                    break;
                case 3: //InPage.ItemDetails:
                    ProfileService.Instance.ToggleQuickItemDetails(request.UserId, updatedValue);

                    cacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(request.UserId, "IsQuickItemDetailsEnabled");
                    CachingController.Instance.Write(cacheKey, updatedValue);

                    break;
                case 1: //InPage.CartDetails:
                    ProfileService.Instance.ToggleQuickCartDetails(request.UserId, updatedValue);

                    cacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(request.UserId, "IsQuickCartDetailsEnabled");
                    CachingController.Instance.Write(cacheKey, updatedValue);
                    break;
                case 4: //InPage.CartsList:
                    ProfileService.Instance.ToggleQuickCartList(request.UserId, updatedValue);

                    cacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(request.UserId, "IsQuickCartsListEnabled");
                    CachingController.Instance.Write(cacheKey, updatedValue);
                    break;
            }

            return result;
        }

        public void ToggleProductImages(string userId, int inPageValue)
        {
            string pageKey = "";

            switch (inPageValue)
            {
                case 2: //InPage.SearchResults:
                    pageKey = DistributedCacheKey.ProfileIsHideProductImages;
                    break;
                case 1: //InPage.CartDetails:
                    pageKey = DistributedCacheKey.ProfileIsHideProductImagesForQuickCart;
                    break;
            }

            if (string.IsNullOrEmpty(pageKey)) return;

            var cacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, pageKey);
            var runtimeO = CachingController.Instance.Read(cacheKey);
            if (runtimeO != null)
            {
                var updatedvalue = !(bool)runtimeO;

                switch (inPageValue)
                {
                    case 2: //InPage.SearchResults:
                        ProfileService.Instance.ToggleProductImagesForQuickSearch(userId, updatedvalue);
                        break;
                    case 1: //InPage.CartDetails:
                        ProfileService.Instance.ToggleProductImagesForQuickCart(userId, updatedvalue);
                        break;
                }

                CachingController.Instance.Write(cacheKey, updatedvalue);
            }
            else
            {
                CachingController.Instance.Write(cacheKey, false);
            }
        }

        public async Task<string> GetEncryptQueryString(GetEncryptQueryRequest request)
        {
            var userId = request.UserId;

            var userPref = await ProfileDAOManager.Instance.GetUserPreferenceById(userId);
            if (userPref != null)
            {
                var qs = string.Format("iden={0}&userId={1}&token={2}", AppSetting.OCSIdentifier, userId, userPref.OCSUserToken);

                return HttpUtility.UrlEncode(CommonHelper.Instance.CompressString(qs));
            }
            return string.Empty;
        }

        public AppServiceResult<List<PricingReturn4ClientObject>> GetProductPricing(List<PricingClientArg> pricingArgList, string userId,
             string[] ESuppliers, bool hideNetPriceDiscountPercentage, TargetingValues targeting, AccountInfoForPricing accountPricingData)
        {
            var ajaxResult = new AppServiceResult<List<PricingReturn4ClientObject>>();
            try
            {
                var marketType = targeting.MarketType ?? MarketType.Any;
                var audienceType = CommonHelper.Instance.ConvertAudienceTypeAsString(targeting.AudienceType);
                var galeLiteral = ConfigurationManager.AppSettings["GaleLiteral"]; //CommonHelper.GetESupplierTextFromSiteTerm(AccountType.GALEE.ToString());
                //            
                var listBasketLineItemUpdated = new List<BasketLineItemUpdated>();
                var realtimePricingHelper = new RealTimePricingHelper();

                foreach (var pricingItem in pricingArgList)
                {
                    if (string.Compare(pricingItem.ESupplier, galeLiteral, StringComparison.OrdinalIgnoreCase) == 0)
                        pricingItem.ToUpdateListPrice = true.ToString();

                    //get account info
                    var accountInfo = realtimePricingHelper.GetAccountInfoForPricing(pricingItem.ProductType, pricingItem.ESupplier,
                        pricingItem.ProductFormat, userId, accountPricingData);

                    if (accountInfo != null)
                    {
                        //                
                        var lineItemUpdated = new BasketLineItemUpdated
                        {
                            ISBN = pricingItem.ISBN,
                            BTKey = pricingItem.BTKey,
                            SoldToId = userId,
                            AccountId = accountInfo.AccountId,
                            ProductType = pricingItem.ProductType,
                            TotalLineQuantity = 1,
                            TotalOrderQuantity = 1,
                            PriceKey = pricingItem.PriceKey,
                            AccountPricePlan = accountInfo.AccountPricePlanId,
                            ProductLine = pricingItem.ProductLine,
                            ReturnFlag = pricingItem.HasReturn.ToLower() == "true",
                            AccountERPNumber = accountInfo.ErpAccountNumber,
                            PrimaryWarehouse = accountInfo.PrimaryWarehouseCode,
                            ProductCatalog = pricingItem.Catalog,
                            MarketType = marketType.ToString(),
                            AudienceType = audienceType,
                            ESupplier = pricingItem.ESupplier,
                            EMarket = accountInfo.EMarketType,
                            ETier = accountInfo.ETier,
                            ProductPriceChangedIndicator = true,
                            ContractChangedIndicator = true,
                            PromotionChangedIndicator = false,
                            PromotionActiveIndicator = false,
                            QuantityChanged = true,
                            IsHomeDelivery = accountInfo.IsHomeDelivery,
                            Upc = pricingItem.BTUPC,
                            AcceptableDiscount = pricingItem.AcceptableDiscount,
                            PurchaseOption = pricingItem.PurchaseOption, // MUPO+processing charger
                            NumberOfBuildings = accountInfo.BuildingNumbers, // MUPO+processing charger
                            ProcessingCharges = accountInfo.ProcessingCharges, // MUPO+processing charger
                            SalesTax = accountInfo.SalesTax,
                            IsVIPAccount = accountInfo.IsVIPAccount
                        };
                        decimal listPrice;
                        if (Decimal.TryParse(pricingItem.ListPrice, out listPrice))
                        {
                            lineItemUpdated.ListPrice = listPrice;
                        }
                        listBasketLineItemUpdated.Add(lineItemUpdated);
                    }
                }
                //Get Prices
                var pricingController = new PricingController();
                //4428: For Search Page Only and for Retail Customers Only we will pass a Static order quantity of 5 
                //and a line quantity of 5 in the product search.  Applicable to both Basic Search and Advanced Search.  Hardcoded to Pricing.
                //10297: apply 4428 for any market type
                var productItemPrices = pricingController.CalculatePrice(listBasketLineItemUpdated
                                                                        , (marketType == MarketType.Retail) ? 5 : 1
                                                                        , targeting, hideNetPriceDiscountPercentage);
                //update price back to search result
                UpdatePriceForSearchResult(productItemPrices, userId, ESuppliers);
                //            
                ajaxResult.Data = BuildPricingObjectsToReturn(pricingArgList, marketType, productItemPrices, userId, ESuppliers);
                ajaxResult.Status = AppServiceStatus.Success;

            }
            catch (Exception ex)
            {
                ajaxResult.Data = new List<PricingReturn4ClientObject>();
                ajaxResult.Status = AppServiceStatus.Fail;
                ajaxResult.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Pricing);
            }
            return ajaxResult;
        }

        private List<PricingReturn4ClientObject> BuildPricingObjectsToReturn(IEnumerable<PricingClientArg> pricingArgList,
            MarketType marketType, List<ItemPricing> itemPriceResults, string userId, string[] eSupplier)
        {
            var returnObjects = new List<PricingReturn4ClientObject>();
            if (itemPriceResults == null || itemPriceResults.Count == 0) return returnObjects;
            var galeLiteral = CommonHelper.GetESupplierTextFromSiteTerm(AccountType.GALEE.ToString());
            foreach (var priceArg in pricingArgList)
            {
                var returnObject = new PricingReturn4ClientObject { BTKey = priceArg.BTKey };

                var itemPriceResult = itemPriceResults.FirstOrDefault(x => x.BTKey == priceArg.BTKey);
                if (itemPriceResult != null)
                {
                    var salePrice = CommonHelper.GetDisplayValueOfDiscountPrice(itemPriceResult.SalePrice, true);
                    returnObject.Price = salePrice;
                    returnObject.ListPrice = CommonHelper.FormatPrice(itemPriceResult.ListPrice.HasValue ? itemPriceResult.ListPrice.Value : 0);
                    if (!string.IsNullOrEmpty(priceArg.ESupplier) && priceArg.ESupplier == galeLiteral)
                    {
                        var isExistGaleAccount = CommonHelper.IsESupplierAccountExisted(AccountType.GALEE.ToString(), userId);
                        var na = CommonResources.NA;
                        salePrice = CommonHelper.Instance.DeterminePriceForGaleProduct(salePrice, isExistGaleAccount, eSupplier);
                        if (salePrice == na)
                        {
                            returnObject.Price = na;
                            returnObject.ListPrice = na;
                        }
                    }

                    returnObject.DisPercent = "-1";
                    if (marketType == MarketType.Retail)
                    {
                        if (itemPriceResult.DiscountPercent != null)
                            returnObject.DisPercent = CommonHelper.FormatDiscount(itemPriceResult.DiscountPercent.Value) + " % Discount";
                    }
                }
                returnObject.ToUpdateListPrice = priceArg.ToUpdateListPrice;
                returnObjects.Add(returnObject);
            }
            return returnObjects;
        }

        private void UpdatePriceForSearchResult(IEnumerable<ItemPricing> itemPricings, string userId, string[] eSupplier)
        {
            var cacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.SearchResult);

            var searchResultItems = CachingController.Instance.Read(cacheKey) as ProductSearchResults; //SiteContext.Current.Session[SessionVariableName.SearchResult] as ProductSearchResults;
            if (searchResultItems != null)
            {
                var galeLiteral = CommonHelper.GetESupplierTextFromSiteTerm(AccountType.GALEE.ToString());
                if (itemPricings != null)
                {
                    var isExistGaleAccount = CommonHelper.IsESupplierAccountExisted(AccountType.GALEE.ToString(), userId);
                    foreach (var itemPricing in itemPricings)
                    {
                        var prodItem = searchResultItems.GetProductSearchResultItem(itemPricing.BTKey);
                        if (prodItem != null)
                        {
                            prodItem.ListPrice = itemPricing.ListPrice.HasValue ? itemPricing.ListPrice.Value : 0;
                            prodItem.DiscountPrice = itemPricing.SalePrice.HasValue ? itemPricing.SalePrice.Value : 0;
                            prodItem.DiscountPercent = itemPricing.DiscountPercent.HasValue ? itemPricing.DiscountPercent.Value : 0;
                            if (prodItem.ESupplier == galeLiteral)
                            {
                                var na = CommonResources.NA;
                                var salePrice = itemPricing.SalePrice.HasValue ? itemPricing.SalePrice.Value : 0;
                                var priceText = CommonHelper.Instance.DeterminePriceForGaleProduct(salePrice.ToString(), isExistGaleAccount, eSupplier);
                                if (priceText == na)
                                {
                                    prodItem.ListPrice = -1;
                                    prodItem.DiscountPrice = -1;
                                    prodItem.DiscountPercent = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        public LandingPageContract GetDataForLandingFirstLoad(TargetingValues siteContext)
        {
            var result = new LandingPageContract();

            result.InTheNews = GetInTheNewsData(siteContext);

            result.ListCarouselItem = GetListCarouselsData(siteContext);

            result.SalesPromoText = GetSalesPromoText(siteContext);

            result.RailPromoText = GetRailsPromoText(siteContext);

            return result;
        }

        public AppServiceResult<TwilightListContract> GetComingSoonData(CollectionData request)
        {
            var result = new AppServiceResult<TwilightListContract>();
            var commingSoonContract = new TwilightListContract();
            if (result == null)
            {
                return result;
            }
            //var siteContext = request.SiteContext;
            //if (siteContext == null)
            //{
            //    result.Status = AppServiceStatus.Fail;
            //    result.ErrorMessage = "There is no context.";
            //    result.Data = commingSoonContract;
            //    return result;
            //}
            try
            {

                IList<ComingSoonCarouselItem> comingSoonCarouselItemsFromSharePoint;

                var targeting = MarketingHelper.Instance.GenerateTargetingQueryStringNoOrgInfo(request.Targeting);
                const string postName = "ComingSoonCarouselItem";

                targeting = targeting ?? "";

                var cacheKey = string.Format("{0}__{1}", targeting, postName);
                var cache = CachingController.Instance.Read(cacheKey) as IList<ComingSoonCarouselItem>;
                if (cache != null)
                {
                    comingSoonCarouselItemsFromSharePoint = cache;
                    // refine qunatity
                    var listBtKey = comingSoonCarouselItemsFromSharePoint.Select(item => item.BTKEY).Distinct();
                    var quantites = CartDAOManager.Instance.FindQuantity(request.UserId, listBtKey);
                    foreach (
                        var item in
                            comingSoonCarouselItemsFromSharePoint.Where(item => quantites.ContainsKey(item.BTKEY)))
                    {
                        item.Quantity = quantites[item.BTKEY];
                    }
                }
                else
                {
                    var requestDomainName = AppSetting.Ts360SiteUrl;
                    comingSoonCarouselItemsFromSharePoint =
                        ContentManagementController.Current.GetComingSoonCarouselItems(request.SearchData, request.Targeting, requestDomainName);
                    if (comingSoonCarouselItemsFromSharePoint == null ||
                        comingSoonCarouselItemsFromSharePoint.Count <= 0)
                    {
                        result.Status = AppServiceStatus.Fail;
                        result.ErrorMessage = "Cannot get data.";
                        result.Data = commingSoonContract;
                        return result;
                    }

                    var pricingClientArg = new List<PricingClientArg>();
                    var listBTKey = comingSoonCarouselItemsFromSharePoint.Select(item => item.BTKEY).Distinct();
                    var quantites = CartDAOManager.Instance.FindQuantity(request.UserId, listBTKey);
                    foreach (
                        var item in
                            comingSoonCarouselItemsFromSharePoint.Where(item => quantites.ContainsKey(item.BTKEY)))
                    {
                        item.Quantity = quantites[item.BTKEY];
                        var productItem = item.ProductSearchResultItem; // ConvertToProductItemForComingSoon(item);
                        pricingClientArg.Add(GetPricingClientArg(productItem));
                    }

                    RefinePricingForComingSoon(comingSoonCarouselItemsFromSharePoint, pricingClientArg, request);
                    CachingController.Instance.Write(cacheKey, comingSoonCarouselItemsFromSharePoint, CommonConstants.CmCachingDuration);
                }

                var listItems = new List<ItemDataContract>();
                var isProxied = ProxySessionHelper.IsInProxySession();
                var strProxiedId = string.Empty;
                if (isProxied)
                    strProxiedId = "&proxieduserid=" + request.ProxiedUserId;

                foreach (var item in comingSoonCarouselItemsFromSharePoint)
                {
                    var itemData = new ItemDataContract();
                    var itemDetailsUrl = string.Format("{0}?{1}={2}", SiteUrl.ItemDetails, SearchFieldNameConstants.btkey, item.BTKEY + strProxiedId);
                    itemDetailsUrl = SearchHelper.CreateUrlWebtrendsQueryString(itemDetailsUrl, item.WebTrendsTag);

                    itemData.ItemKey = item.BTKEY;
                    itemData.ItemValue = new StringBuilder(TwilightComingSoonItemHTMLTemplate).Replace("{BTKey}", item.BTKEY)
                        .Replace("{ITEMDETAILSLINK}", itemDetailsUrl)
                        .Replace("{ImageUrl}", item.ComingSoonImage).Replace("{ITEMTITLE}", item.Title).Replace("{CONTENTOWNER}", item.ContentOwner)
                        .Replace("{CONTENTOWNER_ENCODE}", HttpUtility.UrlEncode(item.ContentOwner))
                        .Replace("{OLDPRICE}", item.OldPrice).Replace("{DISCOUNTPRICE}", item.NewPrice)
                        .Replace("{FormatClass}", item.FormatClass).Replace("{ProductFormat}", item.ProductFormat)
                        .Replace("{FormatIconTooltip}", DetermineProductFormatTooltip(item.FormatClass, item.ProductFormat))
                        .Replace("{IncludedFormatIconClass}", item.IncludedFormatClass)
                        .Replace("{ProductType}", item.ProductType.ToString()).Replace("{ISBN}", item.ISBN)
                        .Replace("{GTIN}", item.GTIN).Replace("{UPC}", item.Upc).Replace("{Catalog}", item.Catalog)
                        .Replace("{DefaultQuantity}", item.Quantity).ToString();

                    if (request.isPrimaryCartSet && string.IsNullOrEmpty(item.Quantity))
                    {
                        itemData.ItemValue = itemData.ItemValue.Replace("{disable}", string.Empty)
                            .Replace("{onclick}",
                                     "AddProductToPrimaryCart(this,'" + item.BTKEY + "', '', '" + item.ProductType + "', '" +
                                     item.ISBN + "', '" + item.GTIN
                                     + "', '" + item.Upc + "', '" + item.Catalog + "', '" + CommonHelper.Instance.ReplaceSingleQuote(item.Title) + "', '" + CommonHelper.Instance.ReplaceSingleQuote(item.ContentOwner) + "'); return false;");
                    }
                    else
                    {
                        itemData.ItemValue = itemData.ItemValue.Replace("{disable}", "disable")
                            .Replace("{onclick}", "return false;");
                    }

                    listItems.Add(itemData);
                }

                commingSoonContract.ProductItems = listItems;
                IEnumerable<SingleProductBaseItem> parentList = comingSoonCarouselItemsFromSharePoint.Cast<SingleProductBaseItem>().ToList();
                var combinedBtKeys = ContentManagementHelper.CombineBtKeysForSearch(parentList);
                var viewAllUrl = SearchHelper.CreateUrlBTKeys(combinedBtKeys);

                // put webtrends tag to ViewAll link
                var header = ContentManagementController.Current.GetHeaderTitlesItems();
                if (header != null && !string.IsNullOrEmpty(header.ComingSoonCarousel))
                    viewAllUrl = SearchHelper.CreateUrlWebtrendsQueryString(viewAllUrl, header.NewReleaseViewAllWTTag);

                commingSoonContract.ViewAllLink = viewAllUrl;
                commingSoonContract.ReleaseCalendarLink = ProxySessionHelper.AppendProxyUserId("/_layouts/CommerceServer/NewReleaseCalendarPage.aspx");


                result.Status = AppServiceStatus.Success;
                result.Data = commingSoonContract;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }
        private const string TwilightComingSoonItemHTMLTemplate = @"<div class=""product-item fl"" id=""ComingSoonItem{BTKey}"">
	<div class=""product-image"">
		<a href=""{ITEMDETAILSLINK}"">
			<img width=""108px"" class=""product-thumb fl"" id=""LINKIMAGE{BTKey}"" src=""{ImageUrl}""  />
		</a>
	</div>
	<div class=""product-info"">
		<div class=""product-info-inner"">
			<span class=""product-name fl cb"">
				<span>{ITEMTITLE}</span></span>
			<span class=""product-author cb"">
                <a href=""/_layouts/CommerceServer/SearchResults.aspx?ngresponsiblepartyprimary={CONTENTOWNER_ENCODE}&rc=1"" class=""twilight-collection-author"">{CONTENTOWNER}</a></span>
			<span class=""product-price db fl"">{OLDPRICE}</span>
            <span class=""ico-email"" style=""float:right; width:18px;"">
                <a href=""javascript:void(0)"">
                    <img class=""HiddenTrackChange pointer fr db pad-top5"" 
                         src=""/_layouts/IMAGES/CSDefaultSite/assets/images/ts360-sprite.png?v=346"" alt=""Email Content"" 
				         onclick=""openEmailWindow('{BTKey}');"" />                                                    
                </a>
            </span>
            <span class=""product-list-price db fl"" style=""width: 96px;"">(Est. Net: {DISCOUNTPRICE})</span>
		</div>
        <div style=""clear:both;""></div>
		<div class=""product-action"">
            <span class=""sprite {FormatClass} fl margin-right5"" title=""{FormatIconTooltip}""></span>
            <span class=""sprite {IncludedFormatIconClass}""></span>
			<input type=""text"" value=""{DefaultQuantity}"" maxlength=""3"" class=""BT_Input fr""
				id=""txtQuantity_text_{BTKey}"" 
				style=""width: 15px; margin: 0 3px 0 0px;"">
		</div>
        <div style=""clear:both;""></div>
        <div class=""btn-add"">
            <div class=""sprite btnAddToCartLeftSprite {disable} pointer fl db pad-top5""
                    onmouseenter=""showAddToCartTooltip(event)"" onmouseleave=""addBtnMouseLeave()""
                    onclick=""{onclick}""></div>
            <div class=""sprite btnAddToCartRightSprite pointer fl db pad-top5"" onmouseleave=""showActiveCartsMouseLeave();"" 
					onmouseover=""ShowAddCartTooltip_CoverFlowViewOnHomePage(event,this, 
					'{BTKey}', '{ProductType}', '{ISBN}', '{GTIN}', '{UPC}', '{Catalog}'); return false;""></div>
        </div>
        <div style=""clear:both;""></div>
	</div>
</div>";
        private void RefinePricingForComingSoon(IEnumerable<ComingSoonCarouselItem> comingSoonCarouselItems,
            List<PricingClientArg> pricingClientArg, CollectionData siteContext)
        {
            var pricingData = GetProductPricing(pricingClientArg, siteContext.UserId, siteContext.SearchData.ESuppliers
                , siteContext.IsHideNetPriceDiscountPercentage, siteContext.Targeting, siteContext.AccountPricingData).Data;
            foreach (var item in comingSoonCarouselItems)
            {
                PricingReturn4ClientObject priceItem = null;
                foreach (var pricingReturn4ClientObject in pricingData)
                {
                    if (string.Compare(item.BTKEY, pricingReturn4ClientObject.BTKey, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        priceItem = pricingReturn4ClientObject;
                        break;
                    }
                }
                if (priceItem == null) continue;
                item.OldPrice = priceItem.ListPrice;
                item.NewPrice = priceItem.Price;
                item.PercentDiscount = priceItem.DisPercent;
            }
        }
        private string GetRailsPromoText(TargetingValues siteContext)
        {
            var items = ContentManagementController.Current.GetRailsPromotionItems(siteContext);
            return items.Count == 0 ? string.Empty : items[0].SalesText;
        }

        private string GetSalesPromoText(TargetingValues siteContext)
        {

            var items = ContentManagementController.Current.GetOpenPromotionItems(siteContext);
            return items.Count == 0 ? string.Empty : items[0].SalesText;
        }

        private const string TwilightComingSoonItemHtmlTemplate = @"<div class=""product-item fl"" id=""ComingSoonItem{BTKey}"">
	<div class=""product-image"">
		<a href=""{ITEMDETAILSLINK}"">
			<img width=""108px"" class=""product-thumb fl"" id=""LINKIMAGE{BTKey}"" src=""{ImageUrl}""  />
		</a>
	</div>
	<div class=""product-info"">
		<div class=""product-info-inner"">
			<span class=""product-name fl cb"">
				<span>{ITEMTITLE}</span></span>
			<span class=""product-author cb"">
                <a href=""/_layouts/CommerceServer/SearchResults.aspx?ngresponsiblepartyprimary={CONTENTOWNER_ENCODE}&rc=1"" class=""twilight-collection-author"">{CONTENTOWNER}</a></span>
			<span class=""product-price db fl"">{OLDPRICE}</span>
            <span class=""ico-email"" style=""float:right; width:18px;"">
                <a href=""javascript:void(0)"">
                    <img class=""HiddenTrackChange pointer fr db pad-top5"" 
                         src=""/_layouts/IMAGES/CSDefaultSite/assets/images/ts360-sprite.png?v=346"" alt=""Email Content"" 
				         onclick=""openEmailWindow('{BTKey}');"" />                                                    
                </a>
            </span>
            <span class=""product-list-price db fl"" style=""width: 96px;"">(Est. Net: {DISCOUNTPRICE})</span>
		</div>
        <div style=""clear:both;""></div>
		<div class=""product-action"">
            <span class=""sprite {FormatClass} fl margin-right5"" title=""{FormatIconTooltip}""></span>
            <span class=""sprite {IncludedFormatIconClass}""></span>
			<input type=""text"" value=""{DefaultQuantity}"" maxlength=""3"" class=""BT_Input fr""
				id=""txtQuantity_text_{BTKey}"" 
				style=""width: 15px; margin: 0 3px 0 0px;"">
		</div>
        <div style=""clear:both;""></div>
        <div class=""btn-add"">
            <div class=""sprite btnAddToCartLeftSprite {disable} pointer fl db pad-top5""
                    onmouseenter=""showAddToCartTooltip(event)"" onmouseleave=""addBtnMouseLeave()""
                    onclick=""{onclick}""></div>
            <div class=""sprite btnAddToCartRightSprite pointer fl db pad-top5"" onmouseleave=""showActiveCartsMouseLeave();"" 
					onmouseover=""ShowAddCartTooltip_CoverFlowViewOnHomePage(event,this, 
					'{BTKey}', '{ProductType}', '{ISBN}', '{GTIN}', '{UPC}', '{Catalog}'); return false;""></div>
        </div>
        <div style=""clear:both;""></div>
	</div>
</div>";
        private AdditionalCartLineItemsResponse additionalCartLineItemsResponse;

        private void RefinePricing(IEnumerable<CollectionCarouselItem> collectionCarouselItemsFromSharePoint,
            List<PricingClientArg> pricingClientArg, CollectionData siteContext)
        {
            var pricingData = GetProductPricing(pricingClientArg, siteContext.UserId, siteContext.SearchData.ESuppliers
                , siteContext.IsHideNetPriceDiscountPercentage, siteContext.Targeting, siteContext.AccountPricingData)
                .Data;
            foreach (var item in collectionCarouselItemsFromSharePoint)
            {
                PricingReturn4ClientObject priceItem = null;
                foreach (var pricingReturn4ClientObject in pricingData)
                {
                    if (string.Compare(item.BTKEY, pricingReturn4ClientObject.BTKey, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        priceItem = pricingReturn4ClientObject;
                        break;
                    }
                }

                if (priceItem == null) continue;

                item.OldPrice = priceItem.ListPrice;
                item.NewPrice = priceItem.Price;
                item.PercentDiscount = priceItem.DisPercent;
            }
        }

        public TwilightListContract GetCollectionData(CollectionData siteContext)
        {
            var twilightContract = new TwilightListContract();

            var targetingParam = MarketingHelper.Instance.ToTargetingParam(siteContext.Targeting);

            var collectionGeneralItemsFromSharePoint = ContentManagementController.Current.GetCollectionGeneralItems(targetingParam);
            if (collectionGeneralItemsFromSharePoint == null || collectionGeneralItemsFromSharePoint.Count <= 0)
            {
                return twilightContract;
            }

            IList<CollectionCarouselItem> collectionCarouselItemsFromSharePoint;

            var collectionTitle = collectionGeneralItemsFromSharePoint[0].CollectionTitle;
            var requestDomainName = AppSetting.Ts360SiteUrl;

            //var targeting = CommonHelper.Instance.GenerateTargetingQueryStringNoOrgInfo(siteContext);
            var postName = string.Format("CollectionCarouselItem{0}", collectionTitle);
            var targetingText = MarketingHelper.Instance.GenerateTargetingValuesNoOrgInfo(siteContext.Targeting);
            var cacheKey = string.Format("{0}__{1}", targetingText, postName);
            var cache = CachingController.Instance.Read(cacheKey) as IList<CollectionCarouselItem>;
            if (cache != null)
            {
                collectionCarouselItemsFromSharePoint = cache;
                // refine qunatity
                var listBtKey = collectionCarouselItemsFromSharePoint.Select(item => item.BTKEY).Distinct();
                var quantites = CartDAOManager.Instance.FindQuantity(siteContext.UserId, listBtKey);// CartMapping.Instance.FindQuantity(listBtKey);
                foreach (var item in collectionCarouselItemsFromSharePoint.Where(item => quantites.ContainsKey(item.BTKEY)))
                {
                    item.Quantity = quantites[item.BTKEY];
                }
            }
            else
            {
                //targetingText = MarketingHelper.Instance.GenerateTargetingQueryString(siteContext);

                collectionCarouselItemsFromSharePoint = ContentManagementController.Current.GetCollectionCarouselItems(collectionTitle, targetingParam);
                RefineCollectionCarouselItem(collectionCarouselItemsFromSharePoint, siteContext.SearchData, siteContext.Targeting.MarketType, siteContext.Targeting.OrgId, requestDomainName);

                if (collectionCarouselItemsFromSharePoint == null || collectionCarouselItemsFromSharePoint.Count <= 0)
                {
                    return twilightContract;
                }

                var pricingClientArg = new List<PricingClientArg>();
                var listBtKey = collectionCarouselItemsFromSharePoint.Select(item => item.BTKEY).Distinct();
                var quantites = CartDAOManager.Instance.FindQuantity(siteContext.UserId, listBtKey);
                foreach (var item in collectionCarouselItemsFromSharePoint.Where(item => quantites.ContainsKey(item.BTKEY)))
                {
                    item.Quantity = quantites[item.BTKEY];
                    var productItem = item.ProductSearchResultItem; // ConvertToProductItem(item);
                    pricingClientArg.Add(GetPricingClientArg(productItem));
                }

                RefinePricing(collectionCarouselItemsFromSharePoint, pricingClientArg, siteContext);

                CachingController.Instance.Write(cacheKey, collectionCarouselItemsFromSharePoint, CommonConstants.CmCachingDuration);
            }

            var listItems = new List<ItemDataContract>();
            var isProxied = ProxySessionHelper.IsInProxySession();
            var strProxiedId = string.Empty;
            if (isProxied)
                strProxiedId = "&proxieduserid=" + siteContext.ProxiedUserId;

            foreach (var item in collectionCarouselItemsFromSharePoint)
            {
                item.Image = ContentManagementHelper.GetRelativePath(item.Image, requestDomainName);

                var itemData = new ItemDataContract();
                var itemDetailsUrl = string.Format("{0}?{1}={2}", SiteUrl.ItemDetails, SearchFieldNameConstants.btkey, item.BTKEY + strProxiedId);
                itemDetailsUrl = SearchHelper.CreateUrlWebtrendsQueryString(itemDetailsUrl, item.WebTrendsTag);

                itemData.ItemKey = item.BTKEY;
                itemData.ItemValue =
                    new StringBuilder(TwilightComingSoonItemHtmlTemplate).Replace("{BTKey}", item.BTKEY)
                        .Replace("{ITEMDETAILSLINK}", itemDetailsUrl)
                        .Replace("{ImageUrl}", item.Image).Replace("{ITEMTITLE}", item.Title).Replace(
                            "{CONTENTOWNER}", item.ContentOwner)
                        .Replace("{CONTENTOWNER_ENCODE}", HttpUtility.UrlEncode(item.ContentOwner))
                        .Replace("{OLDPRICE}", item.OldPrice).Replace("{DISCOUNTPRICE}", item.NewPrice)
                        .Replace("{FormatClass}", item.FormatClass).Replace("{ProductFormat}",
                                                                                  item.ProductFormat)
                        .Replace("{FormatIconTooltip}", DetermineProductFormatTooltip(item.FormatClass, item.ProductFormat))
                        .Replace("{IncludedFormatIconClass}", item.IncludedFormatClass)
                        .Replace("{ProductType}", item.ProductType.ToString()).Replace("{ISBN}", item.ISBN)
                        .Replace("{GTIN}", item.GTIN).Replace("{UPC}", item.Upc).Replace("{Catalog}", item.Catalog)
                        .Replace("{DefaultQuantity}", item.Quantity).ToString();

                if (siteContext.isPrimaryCartSet && string.IsNullOrEmpty(item.Quantity))
                {
                    itemData.ItemValue = itemData.ItemValue.Replace("{disable}", string.Empty)
                        .Replace("{onclick}",
                                 "AddProductToPrimaryCart(this,'" + item.BTKEY + "', '', '" + item.ProductType + "', '" +
                                 item.ISBN + "', '" + item.GTIN
                                 + "', '" + item.Upc + "', '" + item.Catalog + "', '" + CommonHelper.Instance.ReplaceSingleQuote(item.Title) + "', '"
                                 + CommonHelper.Instance.ReplaceSingleQuote(item.ContentOwner) + "'); return false;");
                }
                else
                {
                    itemData.ItemValue = itemData.ItemValue.Replace("{disable}", "disable")
                        .Replace("{onclick}", "return false;");
                }

                listItems.Add(itemData);
            }

            twilightContract.ListTitle = collectionGeneralItemsFromSharePoint[0].CollectionTitle;
            twilightContract.ProductItems = listItems;

            IEnumerable<SingleProductBaseItem> parentList = collectionCarouselItemsFromSharePoint.Cast<SingleProductBaseItem>().ToList();
            var combinedBtKeys = ContentManagementHelper.CombineBtKeysForSearch(parentList);

            var webUrl = SearchHelper.CreateUrlBTKeys(combinedBtKeys);
            twilightContract.ViewAllLink = SearchHelper.CreateUrlWebtrendsQueryString(webUrl, collectionGeneralItemsFromSharePoint[0].WebTrendsTag);

            return twilightContract;
        }

        // same tooltip logic on SearchResultsUserControl.ascx
        private string DetermineProductFormatTooltip(string formatClass, string productFormat)
        {
            var result = string.Empty;
            switch (formatClass)
            {
                case CommonConstants.ICON_POD:
                    result = "Print on Demand";
                    break;
                case CommonConstants.ICON_MAKERSPACE:
                    result = ProductFormatConstants.Book_Makerspace;
                    break;
                default:
                    result = productFormat;
                    break;
            }

            return result;
        }

        private ProductSearchResultItem FindItem(ProductSearchResults res, string btKey)
        {
            if (res != null && res.Items != null)
            {
                return res.Items.FirstOrDefault(item => item.BTKey == btKey);
            }
            return null;
        }

        private void RefineCollectionCarouselItem(IList<CollectionCarouselItem> items, SearchByIdData siteContext, MarketType? marketType, string orgId, string requestDomain)
        {
            var btkeys = items.Select(i => i.BTKEY).ToList();
            var searchResult = ProductSearchController.SearchByIdForContentManagement(btkeys, marketType,
                siteContext.SimonSchusterEnabled, siteContext.CountryCode, siteContext.ESuppliers);

            var orgEntity = ProfileService.Instance.GetOrganizationById(orgId);

            //            
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var found = FindItem(searchResult, item.BTKEY);

                if (found != null)
                {
                    if (string.IsNullOrEmpty(item.Image))
                    {
                        // get image url from Content Cafe                    
                        item.Image = ContentCafeHelper.GetJacketImageUrl(found.ISBN, ImageSize.Medium, found.HasJacket);
                    }
                    item.Image = ContentManagementHelper.GetRelativePath(item.Image, requestDomain);
                    item.ProductSearchResultItem = found;

                    item.ISBN = found.ISBN;
                    item.ISBN10 = found.ISBN10;
                    item.PublishedDate = found.PublishDate;
                    item.OldPrice = CommonHelper.FormatPrice(found.ListPrice);
                    item.Format = found.ProductFormat;
                    item.FormatIconPath = found.FormatIconPath;
                    item.FormatClass = found.FormatClass;
                    item.FormatIconClass = found.FormatIconClass;
                    item.IncludedFormatClass = found.IncludedFormatClass;
                    item.BTProductType = found.ProductType;
                    item.ContentOwner = found.Author;
                    item.NewPrice = found.DiscountPrice.ToString();
                    item.DiscountText = found.DiscountText;
                    item.PercentDiscount = found.DiscountPercent.ToString();
                    item.ProductFormat = found.ProductFormat;
                    item.Upc = found.Upc;
                    item.GTIN = found.GTIN;

                    item.HasAnnotations = found.HasAnnotations;
                    item.HasExcerpts = found.HasExcerpt;
                    item.HasMuze = found.HasMuze;
                    item.HasReviews = found.HasReview;
                    item.HasTOC = found.HasToc;
                    item.ReportCode = found.ReportCode;
                    item.HasReturn = found.HasReturn;
                    item.Catalog = found.Catalog;
                    item.ProductLine = found.ProductLine;
                    item.AcceptableDiscount = found.AcceptableDiscount;
                    item.BTEKey = found.BTEKey;
                    item.ContentIndicator = found.ContentIndicatorText;
                    item.ISBNLookUpLink = ProductSearchController.CreateIsbnLookupLink(found.ISBN, found.ISBN10, orgId, orgEntity); //found.ISBNLookUpLink;

                    item.ESupplier = found.ESupplier;
                    item.FormDetails = found.FormDetails;
                    item.PurchaseOption = found.PurchaseOption;

                    item.PriceKey = found.PriceKey;
                }
                else
                {
                    items.Remove(item);
                    i--;
                }
            }
        }

        private List<ListCarouselItemData> GetListCarouselsData(TargetingValues siteContext)
        {
            var listCarousel = new List<ListCarouselItemData>();

            var targetingParam = MarketingHelper.Instance.ToTargetingParamNoOrg(siteContext);

            var requestDomainName = AppSetting.Ts360SiteUrl;
            var collections = ContentManagementController.Current.GetListCarouselItems(targetingParam, requestDomainName);
            if (collections.Count <= 0)
            {
                return listCarousel;
            }

            var selectedColl = new List<ListCarouselItem>();
            var temp = collections[0];
            selectedColl.Add(temp);
            selectedColl.AddRange(collections.Where(carousel => carousel.PromotionFolder != temp.PromotionFolder));

            foreach (var carouselItem in selectedColl)
            {
                var item = new ListCarouselItemData
                {
                    Id = carouselItem.Id,
                    Title = carouselItem.Title,
                    PromotionFolder =
                        carouselItem.PromotionFolder.HasValue
                            ? carouselItem.PromotionFolder.Value.ToString()
                            : "",
                    PromotionName = carouselItem.PromotionName,
                    PromotionLinkImage = carouselItem.PromotionLinkImage,
                    BackgroundImageUrl = carouselItem.PromoBoxBackgroundImage,
                    PromoDesc = carouselItem.PromotionDescription,
                    ListCarouselUrl = carouselItem.ListCarouselURL,
                    JacketCarouselItems = new List<JacketCarouselItemData>()
                };

                listCarousel.Add(item);
            }

            return listCarousel;
        }

        private InTheNewsContract GetInTheNewsData(TargetingValues siteContext)
        {
            var requestDomainName = AppSetting.Ts360SiteUrl;
            var items = ContentManagementController.Current.GetInTheNewsItems(siteContext, requestDomainName);
            if (items != null && items.Count > 0)
            {
                var inTheNewsContract = new InTheNewsContract
                {
                    WachVideoVisible = !string.IsNullOrEmpty(items[0].NewsVideo),
                    NewsVideo = items[0].NewsVideo,
                    NewsTitle = items[0].NewsTitle,
                    NewsText = items[0].NewsText,
                    NewsImage = items[0].NewsImage,
                    ViewDetail =
                        SearchHelper.CreateUrlWebtrendsQueryString(
                            CombineQuerySearch(items[0]), items[0].WebTrendsTag),
                    IsVisible = true,
                    NewsDocument =
                        string.IsNullOrEmpty(items[0].NewsDocument)
                            ? ""
                            : items[0].NewsDocument.Trim()
                };

                return inTheNewsContract;
            }

            return null;
        }

        private string CombineQuerySearch(InTheNewsItem item)
        {
            var result = string.Empty;
            if (item != null)
            {
                if (!string.IsNullOrEmpty(item.PromotionCode))
                    return SearchHelper.CreateUrlPromoCode(item.PromotionCode);

                return string.IsNullOrEmpty(item.BTKEY) ? result : SearchHelper.CreateUrlBTKeys(item.BTKEY.Trim());
            }
            return result;
        }

        public async Task<ProductDetail> GetProductDetail(string btKey, bool OCLCCatalogingPlusEnabled, string userId, bool allowBTEmployee)
        {
            var product = new ProductDetail();
            string btAnno = await ProductDAOManager.Instance.GetFirstProductAnnotation(btKey);
            product.Annotaion = Sanitizer.GetSafeHtmlFragment(HttpUtility.HtmlDecode(btAnno));
            //var product1 = await ProductDAOManager.Instance.GetProductDetail(btKey, OCLCCatalogingPlusEnabled, userId, productDetailsObject);

            var productDetailsObject = new ProductDetailsObject();
            var cacheKey = string.Format("ProductDetailsInfo_{0}", btKey);

            productDetailsObject = CachingController.Instance.Read(cacheKey) as ProductDetailsObject;

            if (productDetailsObject == null)
            {
                productDetailsObject = await ProductDAOManager.Instance.GetProductInformation(btKey);

                CachingController.Instance.Write(cacheKey, productDetailsObject);
            }
            if (productDetailsObject != null)
            {
                GetDataForSections(product, productDetailsObject, btKey, OCLCCatalogingPlusEnabled, userId, allowBTEmployee);
            }

            return product;
        }

        public async Task<string> GetActiveCarts(SearchTooltipConextData context)
        {
            var contextValue = context.Value; // BTKey;UserId;CartId;NumberOfCatr;copyMoveAction
            var arrayString = contextValue.Split(';');

            var userId = arrayString[1];
            var currentCartId = arrayString[2];
            var nuberOfCartsString = arrayString[3];

            int numberOfCarts;
            if (string.IsNullOrEmpty(nuberOfCartsString) || int.TryParse(nuberOfCartsString, out numberOfCarts))
            {
                numberOfCarts = CommonConstants.DEFAULT_NUMBER_OF_ACTIVE_CARTS;
            }

            var copyMoveAction = arrayString[4];

            string currentAction = "Add";
            if (!string.IsNullOrEmpty(copyMoveAction))
            {
                if (copyMoveAction == "copyTitle")
                    currentAction = "Copy";
                if (copyMoveAction == "moveTitle")
                    currentAction = "Move";
            }

            string strAddToACart = string.Empty;
            string strAddNewCart = string.Empty;
            string activeCartTemplate = string.Empty;
            string disabledCartTemplate = string.Empty;
            string activeCart = string.Empty;
            string onclickTemplate = string.Empty;
            string disableCartPopup = string.Empty;

            if (copyMoveAction == "AddAllSelected")
            {
                strAddToACart = "<li><a href='javascript:void(0)' onclick=\"ShowAddAllToAnotherCart(); return false;\">" + "See all of my carts</a></li>";
                strAddNewCart = "<li><a href='javascript:void(0)' name='header' onclick=\"BTNextGenJS.OpenAddAllToNewCartPopup(this); return false;\">" + currentAction + " To new Cart</a></li>";
                activeCartTemplate = "<li class='activeCart'><a href='javascript:void(0)' {0} cartid=\"{1}\">{2}</a></li>";
                disabledCartTemplate = "<li><a {0}>{1}</a></li>";
                onclickTemplate = "onclick=\"AddAllSelectedToActiveCart({0}, '{1}'); return false;\"";
                disableCartPopup = "class=\"disableCartPopup\"";
            }
            else
            {
                strAddToACart = "<li><a href='javascript:void(0)' onclick=\"ShowAddToAnotherCart(); return false;\">" + currentAction + " To a Cart</a></li>";
                strAddNewCart = "<li><a href='javascript:void(0)' onclick=\"BTNextGenJS.OpenAddToNewCartPopup(this); return false;\">" + currentAction + " To new Cart</a></li>";
                activeCartTemplate = "<li class='activeCart'><a href='javascript:void(0)' {0} cartid=\"{1}\">{2}</a></li>";
                disabledCartTemplate = "<li><a {0}>{1}</a></li>";
                onclickTemplate = "onclick=\"AddToActiveCart({0}, '{1}'); return false;\"";
                disableCartPopup = "class=\"disableCartPopup\"";
            }

            Cart primaryCart = null;
            if (string.IsNullOrEmpty(currentCartId))
                primaryCart = CartDAOManager.Instance.GetPrimaryCart(userId);// CartMapping.Instance.GetPrimaryCart();
            var primaryCartId = (primaryCart != null) ? primaryCart.CartId : string.Empty;

            //var cartManager = CartContext.Current.GetCartManagerForUser(userId);
            Carts topNewestCarts = await CartDAOManager.Instance.GetTopNewestCarts(numberOfCarts, userId);
            //if (cartManager != null) topNewestCarts = Ca cartManager.GetTopNewestCarts(numberOfCarts);

            if (topNewestCarts != null)
            {
                foreach (var cart in topNewestCarts)
                {
                    var cartId = cart.CartId;
                    if (primaryCartId == cartId) continue;

                    var cartName = cart.CartName;
                    var encodedName = HttpUtility.HtmlEncode(cartName);

                    if (currentCartId == cartId)
                        activeCart += string.Format(disabledCartTemplate, disableCartPopup, encodedName);
                    else
                        activeCart += string.Format(activeCartTemplate,
                                                    string.Format(onclickTemplate, Microsoft.Security.Application.Encoder.JavaScriptEncode(cartName), cartId),
                                                    cartId, encodedName);
                }
            }

            return string.Format("<div class='list-carts'><div class='list-carts-inner'><div style='height: 23px;text-align: center;padding-top: 8px'>{3}</div><ul id='ulListCart'>{0}{1}{2}</ul></div></div>",
                                activeCart,
                                strAddToACart,
                                strAddNewCart,
                                currentAction.ToUpper());
        }

        public async Task<ActiveNewestBasketsResponse> GetActiveNewestBaskets(ActiveNewestBasketsRequest request)
        {
            var response = new ActiveNewestBasketsResponse();
            response.Baskets = new List<ActiveNewestBasket>();
            if (request != null && !string.IsNullOrEmpty(request.UserId))
            {
                // get carts including primary
                Carts topNewestCarts = await CartDAOManager.Instance.GetTopNewestCarts(CommonConstants.DEFAULT_NUMBER_OF_ACTIVE_CARTS, request.UserId);
                if (topNewestCarts != null)
                {
                    foreach (var cart in topNewestCarts)
                    {
                        // exclude request cart
                        if (!string.Equals(cart.CartId, request.ExcludeBasketId, StringComparison.CurrentCultureIgnoreCase))
                        {
                            var basket = new ActiveNewestBasket
                            {
                                BasketId = cart.CartId,
                                BasketName = cart.CartName
                            };

                            response.Baskets.Add(basket);
                        }
                    }
                }
            }

            return response;
        }

        public async Task<AppServiceResult<AddToCartStatusObject>> AddTitlesToCartNameWithGrid(AddTitlesToCartNameWithGridRequest request)
        {
            var addToNewCartObjects = request.addToNewCartObjects;
            var gridTitleProperties = request.gridTitleProperties;

            var ajaxResult = new AppServiceResult<AddToCartStatusObject>();
            var isPrimaryCart = false;
            string PermissionViolationMessage = "";
            //
            if (addToNewCartObjects == null || addToNewCartObjects.Count == 0)
            {
                return new AppServiceResult<AddToCartStatusObject>
                {
                    Status = AppServiceStatus.Success,
                    Data = null,
                    ErrorMessage = string.Empty
                };
            }
            try
            {
                var userId = request.UserId;

                var cartManager = new CartManager(userId);
                var cart = await cartManager.GetCartByName(addToNewCartObjects[0].BasketName); // cartManager.GetCartByName(addToNewCartObjects[0].BasketName);

                int lineCountSuccess = 0;
                int itemCountSuccess = 0;
                int totalAddingQtyForGridDistribution = 0;
                var listBasketInfo = new List<CartInfo>();
                // TFS #7806 - Remove duplicate gridlines
                GridHelper.RemoveDuplicateDCGridLines(gridTitleProperties);

                var addWithDefaultSetting = true;

                var isQuickCartDetailsEnabled = CommonHelper.Instance.GetQuickViewModeInfo(userId, "IsQuickCartDetailsEnabled");

                //Add products to cart
                foreach (var product in addToNewCartObjects)
                {
                    int quantity;
                    int.TryParse(product.Quantity, out quantity);
                    if (quantity <= 0)
                    {
                        //if (product.Quantity == "")
                        if (string.IsNullOrEmpty(product.Quantity))
                        {
                            quantity = -1;
                        }
                        else
                            quantity = 0;
                    }
                    var currentBasket = cart;
                    var cartGridLines = new List<CommonCartGridLine>();
                    var dcNotes = new List<DCNote>();
                    var gridTitleProperty = gridTitleProperties.FirstOrDefault(gridProp => gridProp.BTKey == product.BTKey);
                    if (gridTitleProperty != null)
                    {
                        // Grid Information
                        cartGridLines = GridHelper.AddDCGridLinesToCartGridLines(gridTitleProperty.DCGridLines, string.Empty);
                        dcNotes = gridTitleProperty.DCNotes;
                    }

                    if (string.IsNullOrEmpty(product.Quantity))
                    { }
                    else if (product.Quantity.Trim() != "" || (cartGridLines != null && cartGridLines.Count > 0))
                    {
                        addWithDefaultSetting = false;
                    }

                    var addProducts = new List<ProductLineItem>
                                      {
                                          new ProductLineItem()
                                          {
                                              BTKey = product.BTKey,
                                              BibNumber = product.BIB,
                                              Note = GetDCNoteFromTitleProperty(userId, dcNotes),
                                              PONumber = product.POPerline,
                                              Quantity = quantity,
                                              Title = product.Title,
                                              Author = product.Author
                                          }
                                      };

                    int tempTotal;

                    var dicCartGridLine = new Dictionary<string, List<CommonCartGridLine>> { { product.BTKey, cartGridLines } };
                    ////////BEGIN ADD TO CART PROCESS///////////////
                    cartManager.AddProductToCart(addProducts, dicCartGridLine,
                        currentBasket.CartId, out PermissionViolationMessage, out tempTotal);
                    //CartGridContext.Current.CartGridManager.AddProductToCart(addProducts, dicCartGridLine,
                    //    currentBasket.CartId, out PermissionViolationMessage, out tempTotal);
                    ////////END ADD TO CART PROCESS///////////////

                    totalAddingQtyForGridDistribution += tempTotal;

                    if (!string.IsNullOrEmpty(PermissionViolationMessage) && PermissionViolationMessage != "")
                    {
                        ajaxResult.Status = AppServiceStatus.Fail;
                        ajaxResult.ErrorMessage = PermissionViolationMessage;
                        return ajaxResult;
                    }
                    //
                    var basketName = product.BasketName;
                    var basketInfo = new CartInfo();
                    var basketId = string.Empty;
                    if (currentBasket != null)//Check if the adding cart is primary cart
                    {
                        basketId = currentBasket.CartId;
                        basketInfo.Name = basketName;
                        basketInfo.ID = basketId;
                        basketInfo.URL = isQuickCartDetailsEnabled
                                                         ? SiteUrl.QuickCartDetailsPage
                                                         : SiteUrl.CartDetailsUrl;

                        if (currentBasket.IsPrimary)
                        {
                            isPrimaryCart = true;
                            cartManager.SetPrimaryCartChanged();
                            var lineitem = await CartDAOManager.Instance.GetCartLineByBtKey(product.BTKey, currentBasket.CartId);
                            if (lineitem != null)
                                basketInfo.LineItemID = lineitem.Id;
                            CartFrameworkHelper.CalculatePrice(basketId, request.Targeting, false);
                        }
                        else
                        {
                            cartManager.SetCartChanged(currentBasket.CartId);
                            CartFrameworkHelper.CalculateCartPriceInBackground(currentBasket.CartId, request.Targeting);
                        }
                    }
                    lineCountSuccess += 1;
                    itemCountSuccess += quantity;
                    listBasketInfo.Add(basketInfo);
                }

                var addingQty = GetAddingQty(cart, totalAddingQtyForGridDistribution, itemCountSuccess, addWithDefaultSetting, lineCountSuccess,
                    request.DefaultQuantity);
                //
                ajaxResult.Status = AppServiceStatus.Success;
                var addCartStatusObject = new AddToCartStatusObject
                {
                    IsPrimary = isPrimaryCart,
                    LineCountSuccess = lineCountSuccess,
                    ItemCountSuccess = addingQty,
                    CartInfo = listBasketInfo,
                    UserID = userId,
                    OrgID = request.Targeting.OrgId
                };

                ajaxResult.Data = addCartStatusObject;
            }
            catch (CartManagerException ex)
            {
                ajaxResult.Status = AppServiceStatus.Fail;
                if (ex.isBusinessError)
                {
                    if (ex.Message.Contains(CartManagerException.INVALID_BASKET_NAME_USER_COMBINATION))
                    {
                        var message = string.Format("Failed to add title(s). The \"{0}\" cart is renamed.", addToNewCartObjects[0].BasketName);
                        ajaxResult.ErrorMessage = message;

                        //var ulsLogMessage = string.Format("{0} {1} (UserId: {2})", message, ex.Message, userId);
                        //Logger.Write("AddTitlesToCartFailed", ulsLogMessage, false);
                    }
                    else
                    {
                        Logger.RaiseException(ex, ExceptionCategory.Search);
                        ajaxResult.ErrorMessage = ProfileResources.UnexpectedError; // GetLocalizedString("ProfileResources", "UnexpectedError");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                ajaxResult.Status = AppServiceStatus.Fail;
                ajaxResult.ErrorMessage = ProfileResources.UnexpectedError; // GetLocalizedString("ProfileResources", "UnexpectedError");
            }

            return ajaxResult;
        }

        private string GetDCNoteFromTitleProperty(string userId, IEnumerable<DCNote> dcNotes)
        {
            if (dcNotes == null) return string.Empty;

            foreach (var dcNote in dcNotes)
            {
                if (dcNote.UserId == userId) return dcNote.NoteText;
            }

            return string.Empty;
        }

        private int GetAddingQty(Cart cart, int totalAddingQtyForGridDistribution, int itemCountSuccess, bool addWithDefaultSetting,
            int lineCountSuccess, string siteContextDefaultQty)
        {
            if (!addWithDefaultSetting) return itemCountSuccess;

            return GetAddingQty(cart.GridDistributionOption, totalAddingQtyForGridDistribution, lineCountSuccess, siteContextDefaultQty);
        }

        private int GetAddingQty(int cartGridDistributionOption, int totalAddingQtyForGridDistribution, int lineCountSuccess, string siteContextDefaultQty)
        {
            var addingQty = 0;
            switch (cartGridDistributionOption)
            {
                case (int)GridDistributionOption.UseGridDistribution:
                case (int)GridDistributionOption.UseGridtemplate:
                    addingQty = totalAddingQtyForGridDistribution;
                    break;
                case (int)GridDistributionOption.UseZeroQty:
                    addingQty = 0;
                    break;
                default:
                    int userDefaultQty;
                    if (!int.TryParse(siteContextDefaultQty, out userDefaultQty))
                    {
                        userDefaultQty = 0;
                    }
                    addingQty = lineCountSuccess * userDefaultQty;
                    break;
            }
            return addingQty;
        }

        private bool ValidForAddAllToCart(Cart chosenBasket, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (chosenBasket == null)
            {
                errorMessage = SearchResources.BENotExistedCart;// GetLocalizedString("SearchResources", "BENotExistedCart");
                return false;
            }

            if (chosenBasket.BTStatus == "Ordered")
            {
                errorMessage = SearchResources.BEOrderedCart;// GetLocalizedString("SearchResources", "BEOrderedCart");
                return false;
            }
            if (chosenBasket.BTStatus == "Deleted")
            {
                errorMessage = SearchResources.BEDeletedCart; // GetLocalizedString("SearchResources", "BEDeletedCart");
                return false;
            }

            //if (chosenBasket.HasPermission)
            //{
            //    errorMessage = GetLocalizedString("SearchResources", "BEDeletedCart");
            //    return false;
            //}
            return true;

        }

        private void ProcessAddAllToCart(ProductSearchResults searchItems, int defaultQuantityValue, string cartId, string userId,
            out string PermissionViolationMessage, out int totalAddingQtyForGridDistribution, TargetingValues siteContext)
        {
            var lineItemsToBeAdded = new List<LineItem>();

            foreach (var itm in searchItems.Items)
            {
                lineItemsToBeAdded.Add(new LineItem
                {
                    BTKey = itm.BTKey,
                    Quantity = defaultQuantityValue,
                    ISBN = itm.ISBN,
                    PONumber = itm.POLine,
                    Id = itm.LineItemID,
                    Bib = itm.Bib,
                    Title = itm.Title,
                    Author = itm.Author
                });
            }

            AddListToCartEx(lineItemsToBeAdded, cartId, userId, out PermissionViolationMessage, out totalAddingQtyForGridDistribution, siteContext);
        }

        private void AddListToCartEx(List<LineItem> listAdded, string cartId, string userId, out string PermissionViolationMessage,
            out int totalAddingQtyForGridDistribution, TargetingValues siteContext)
        {
            PermissionViolationMessage = null;
            totalAddingQtyForGridDistribution = 0;

            if (listAdded == null)
                return;

            var temp = string.Empty;
            CartDAOManager.Instance.AddToCartName4BatchEntry(listAdded, cartId, out temp, out PermissionViolationMessage,
                out totalAddingQtyForGridDistribution, userId, siteContext);
            //CartMapping.Instance.AddToCartName4BatchEntry(listAdded, cartId, out temp, out PermissionViolationMessage, out totalAddingQtyForGridDistribution);
        }

        public AppServiceResult<AddToCartStatusObject> AddAllToCart(AddAllToCartRequest request)
        {
            var ajaxResult = new AppServiceResult<AddToCartStatusObject>();
            string PermissionViolationMessage = null;
            string defaultQuantity = request.clientQuantity;
            try
            {
                var sessionCacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(request.UserId, SearchResultsConstants.SearchArgumentsForAddAll);
                var args = CachingController.Instance.Read(sessionCacheKey); // SiteContext.Current.Session[sessionCacheKey];
                if (args != null && (args as SearchArguments) != null)
                {
                    var searchArguments = args as SearchArguments;

                    var primaryCart = CartDAOManager.Instance.GetPrimaryCart(request.UserId);// CartMapper.CartMapping.Instance.GetPrimaryCartObject();
                    // Invalid Cart
                    string errorAddAll = string.Empty;
                    if (!ValidForAddAllToCart(primaryCart, out errorAddAll))
                    {
                        ajaxResult.Status = AppServiceStatus.Fail;
                        ajaxResult.ErrorMessage = errorAddAll;
                        return ajaxResult;
                    }

                    searchArguments.PageSize = 500;
                    searchArguments.StartRowIndex = 0;
                    var searchResults = ProductSearchController.Search(searchArguments, request.Targeting.MarketType,
                        request.SearchData.SimonSchusterEnabled, request.SearchData.CountryCode, request.SearchData.ESuppliers);

                    // No data return from search
                    if (searchResults == null || searchResults.Items == null || searchResults.Items.Count < 1)
                    {
                        ajaxResult.Status = AppServiceStatus.Fail;
                        ajaxResult.ErrorMessage = SearchResources.BatchEntry_ProvideDataToValidate;// GetLocalizedString("SearchResources", "BatchEntry_ProvideDataToValidate");
                        return ajaxResult;
                    }

                    int totalAddingQtyForGridDistribution;
                    int intDefaultQty = string.IsNullOrEmpty(defaultQuantity) ? -100 : int.Parse(defaultQuantity);

                    var addingWithDefault = string.IsNullOrEmpty(defaultQuantity) || intDefaultQty < 0;

                    ProcessAddAllToCart(searchResults, intDefaultQty, primaryCart.CartId, request.UserId, out PermissionViolationMessage,
                        out totalAddingQtyForGridDistribution, request.Targeting);
                    if (!string.IsNullOrEmpty(PermissionViolationMessage))
                    {
                        ajaxResult.Status = AppServiceStatus.Fail;
                        ajaxResult.ErrorMessage = PermissionViolationMessage;
                        return ajaxResult;
                    }

                    var isQuickCartDetailsEnabled = CommonHelper.Instance.GetQuickViewModeInfo(request.UserId, "IsQuickCartDetailsEnabled");

                    var listBasketInfo = new List<CartInfo>();
                    //TFS#11268
                    var cartinfor = new CartInfo
                    {
                        Name = primaryCart.CartName,
                        ID = primaryCart.CartId,
                        URL = isQuickCartDetailsEnabled //SiteContext.Current.IsQuickCartDetailsEnabled
                            ? SiteUrl.QuickCartDetailsPage
                            : SiteUrl.CartDetailsUrl
                    };
                    listBasketInfo.Add(cartinfor);

                    var userDefaultQty = string.IsNullOrEmpty(request.DefaultQuantity)
                        ? 0
                        : int.Parse(request.DefaultQuantity);

                    var itemCountSuccessForSiteContext = searchResults.Items.Count * userDefaultQty;
                    var addingQty = GetAddingQty(primaryCart, totalAddingQtyForGridDistribution,
                        itemCountSuccessForSiteContext,
                        addingWithDefault, searchResults.Items.Count, request.DefaultQuantity);

                    var addCartStatusObject = new AddToCartStatusObject
                    {
                        IsPrimary = true,
                        LineCountSuccess = searchResults.Items.Count,
                        ItemCountSuccess = addingQty,
                        CartInfo = listBasketInfo,
                        UserID = request.UserId,
                        OrgID = request.Targeting.OrgId
                    };

                    ajaxResult.Status = AppServiceStatus.Success;
                    ajaxResult.Data = addCartStatusObject;

                    return ajaxResult;
                }
                else
                {
                    // Invalid search agrs
                    ajaxResult.Status = AppServiceStatus.Fail;
                    ajaxResult.ErrorMessage = SearchResources.BatchEntry_ProvideDataToValidate;// GetLocalizedString("SearchResources", "BatchEntry_ProvideDataToValidate");
                    return ajaxResult;
                }

            }
            catch (Exception exception)
            {
                //var systemError = string.Format("{0}, {1}", exception.Message, exception.StackTrace);
                Logger.RaiseException(exception, ExceptionCategory.Search);
                ajaxResult.Status = AppServiceStatus.Fail;
                ajaxResult.ErrorMessage = ProfileResources.UnexpectedError; // GetLocalizedString("ProfileResources", "UnexpectedError");
                //ajaxResult.SystemError = systemError;
            }

            return ajaxResult;
        }
        public AppServiceResult<bool> ProcessAddAllExceed500ToCart(AddAllToCartRequest request)
        {
            var ajaxResult = new AppServiceResult<bool>();
            int maxpageSize = request.maxpageSize;
            var defaultQuantity = request.clientQuantity;
            try
            {
                var sessionCacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(request.UserId, SearchResultsConstants.SearchArgumentsForAddAll);
                var args = CachingController.Instance.Read(sessionCacheKey); // SiteContext.Current.Session[sessionCacheKey];
                if (args != null && (args as SearchArguments) != null)
                {
                    var primaryCart = CartDAOManager.Instance.GetPrimaryCart(request.UserId);
                    var searchArguments = args as SearchArguments;
                    searchArguments.PageSize = maxpageSize;
                    searchArguments.StartRowIndex = 0;
                    var searchResults = ProductSearchController.Search(searchArguments, request.Targeting.MarketType,
                        request.SearchData.SimonSchusterEnabled, request.SearchData.CountryCode, request.SearchData.ESuppliers);

                    // No data return from search
                    if (searchResults == null || searchResults.Items == null || searchResults.Items.Count < 1)
                    {
                        ajaxResult.Status = AppServiceStatus.Fail;
                        ajaxResult.ErrorMessage = SearchResources.BatchEntry_ProvideDataToValidate;// GetLocalizedString("SearchResources", "BatchEntry_ProvideDataToValidate");
                        //Logger.RaiseException(ajaxResult.ErrorMessage.ToString(), ExceptionCategory.Search);
                        //Logger.Write(ExceptionCategory.Search.ToString(), ajaxResult.ErrorMessage);
                        PricingLogger.LogDebug(ExceptionCategory.Search.ToString(), ajaxResult.ErrorMessage);
                        return ajaxResult;
                    }
                    // if  data return > 1000

                    if (searchResults.Items.Count > maxpageSize)
                    {
                        ajaxResult.Status = AppServiceStatus.Fail;
                        ajaxResult.ErrorMessage = string.Format("The search result exceeds the {0} item limit. These items cannot be added to your cart.", maxpageSize.ToString());
                        PricingLogger.LogDebug(ExceptionCategory.Search.ToString(), ajaxResult.ErrorMessage);
                        return ajaxResult;
                    }
                    int intDefaultQty = string.IsNullOrEmpty(defaultQuantity) ? -100 : int.Parse(defaultQuantity);
                    ProcessAddAllExceed500ToCart(searchResults, request.UserId, primaryCart.CartId, intDefaultQty, maxpageSize);
                    ajaxResult.Status = AppServiceStatus.Success;
                    return ajaxResult;
                }
                else
                {
                    // Invalid search agrs
                    ajaxResult.Status = AppServiceStatus.Fail;
                    ajaxResult.ErrorMessage = SearchResources.BatchEntry_ProvideDataToValidate;// GetLocalizedString("SearchResources", "BatchEntry_ProvideDataToValidate");
                    PricingLogger.LogDebug(ExceptionCategory.Search.ToString(), ajaxResult.ErrorMessage);
                    return ajaxResult;
                }

            }
            catch (Exception exception)
            {
                //var systemError = string.Format("{0}, {1}", exception.Message, exception.StackTrace);
                Logger.RaiseException(exception, ExceptionCategory.Search);
                ajaxResult.Status = AppServiceStatus.Fail;
                ajaxResult.ErrorMessage = ProfileResources.UnexpectedError; // GetLocalizedString("ProfileResources", "UnexpectedError");
                //ajaxResult.SystemError = systemError;
            }
            return ajaxResult;
        }
        private void ProcessAddAllExceed500ToCart(ProductSearchResults searchItems, string userId, string cartId, int intDefaultQty, int maxBatchSize)
        {
            List<LineItem> lineItemsToBeAdded = new List<LineItem>();

            foreach (var itm in searchItems.Items)
            {
                lineItemsToBeAdded.Add(new LineItem
                {
                    BTKey = itm.BTKey,
                    Quantity = intDefaultQty,
                    ISBN = itm.ISBN,
                    PONumber = itm.POLine,
                    Id = itm.LineItemID,
                    Bib = itm.Bib,
                    Title = itm.Title,
                    Author = itm.Author
                });
            }

            CartDAOManager.Instance.AddExceed500LineItemsToCart(lineItemsToBeAdded, userId, cartId, maxBatchSize);

        }
        public async Task<string> GetCartListPopUpForOriginalEntry(string userId, int numberOfCarts)
        {
            Carts topNewestCarts = await CartDAOManager.Instance.GetTopNewestCarts(numberOfCarts, userId);

            const string strAddToACart = "<li><a href='javascript:void(0)' onclick=\"BTNextGenJS.CartDrawerIconTabClick(true);\">+&nbsp;<u>Add To a Cart</u></a></li>";
            const string strAddNeACart = "<li><a href='#' onclick=\"BTNextGenJS.OpenAddToNewCartPopup(this); return false;\">+&nbsp;<u>Add To new Cart</u></a></li>";
            const string activeCartTemplate = "<li><a href='javascript:void(0)' onclick=\"OriginalEntry.AddToActiveCart({0},'{1}');\">+&nbsp;<u>{2}</u></a></li>";
            var activeCart = string.Empty;
            if (topNewestCarts != null)
                foreach (var cart in topNewestCarts)
                {
                    activeCart += string.Format(activeCartTemplate,
                                                Microsoft.Security.Application.Encoder.JavaScriptEncode(cart.CartName),
                                                cart.CartId,
                                                cart.CartName);
                }

            return
                string.Format("<div class='list-carts'><div class='list-carts-inner'><ul id='ulListCart'>{0}{1}{2}</ul></div></div>",
                                activeCart,
                                strAddToACart,
                                strAddNeACart);
        }

        public async Task<WCFObjectReturnToClient> GetEnhancedContentForSearch(GetEnhancedContentForSearchRequest request)
        {
            var wCfObjectReturnToClient = new WCFObjectReturnToClient();

            var btKeyList = new List<string>() { request.btKey };
            var btKeyHasReviewList = new List<string>();
            var btEKeyList = new List<string>();
            var pricingClientArg = new List<PricingClientArg>();
            var promotionClientArg = new List<PromotionClientArg>();

            var productSearchResults = GetProductSearchResultsFromCache(request.UserId) ??
                                           ProductSearchController.SearchByIdWithoutAnyRules(btKeyList);

            if (productSearchResults == null || productSearchResults.Items == null)
            {
                return wCfObjectReturnToClient;
            }

            ProductSearchResultItem productItem = productSearchResults.Items.FirstOrDefault(item => item.BTKey == request.btKey);
            if (productItem == null)
            {
                return wCfObjectReturnToClient;
            }

            if (!request.ShowInventoryForSearch)
            {
                var invArgs = GetSearchResultInventoryStatusArg(productItem, request.MarketTypeString);
                var inventoryStatusArgList = new List<SearchResultInventoryStatusArg>() { invArgs };
                var inventoryHelper = InventoryHelper4MongoDb.GetInstance(userId: request.UserId,
                    countryCode: request.SearchData.CountryCode, marketType: request.Targeting.MarketType, orgId: request.Targeting.OrgId);
                //InventoryHelper4MongoDb.GetInstance();
                if (!request.isTileView)
                {
                    wCfObjectReturnToClient.InventoryResultsList.AddRange(
                        inventoryHelper.GetInventoryResultsForMultipleItems(inventoryStatusArgList));

                    wCfObjectReturnToClient.InventoryStatus = inventoryHelper.InventoryStatusList;
                }
                else
                {
                    //
                    wCfObjectReturnToClient.InventoryStatus = inventoryHelper.GetInventoryStatus(inventoryStatusArgList);
                }
            }
            if (!request.ShowEnhancedContentIconsForSearch)
            {
                productItem.ContentIndicatorText = SearchHelper.GetContentIndicatorText(request.UserId, productItem.HasAnnotations, productItem.HasExcerpt,
                    productItem.HasReturn, productItem.HasMuze, productItem.HasReview, productItem.HasToc, request.btKey, productItem.ProductType);
                wCfObjectReturnToClient.ContentIndicatorHtml = productItem.ContentIndicatorText;

                if (productItem.HasReview)
                {
                    btKeyHasReviewList.Add(productItem.BTKey);
                }

                if (btKeyHasReviewList.Count > 0)
                {
                    var ci = GetProductReviewIndicator(btKeyHasReviewList, request.Targeting.OrgId);
                    wCfObjectReturnToClient.ContentIndicator = ci.Data;
                }
                else
                {
                    wCfObjectReturnToClient.ContentIndicator = new List<SiteTermObject>();
                }
            }

            var userProfile = ProfileService.Instance.GetUserById(request.UserId); // CSObjectProxy.GetUserProfileForSearchResult();
            string defaultDuplicateOrders = userProfile.DefaultDuplicateOrders;
            string defaultDuplicateCarts = userProfile.DefaultDuplicateCarts;

            if ((!request.ShowDupCheckCartsForSearch && !string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase)) ||
                (!request.ShowDupCheckOrdersForSearch && !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase)))
            {
                pricingClientArg.Add(GetPricingClientArg(productItem));
                promotionClientArg.Add(GetPromotionClientArg(productItem));

                //var dupData = await GetProductDuplicateIndicator(btKeyList, btEKeyList, string.Empty,
                //    !string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase),
                //    !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase),
                //    request.UserId, request.DefaultDownloadedCarts, request.CollectionAnalysisEnabled, request.Targeting.OrgId);

                //wCfObjectReturnToClient.Duplicate = dupData.Data;
            }

            if (!request.ShowExpectedDiscountPriceForSearch)
            {
                wCfObjectReturnToClient.Pricing = GetProductPricing(pricingClientArg, request.UserId, request.SearchData.ESuppliers
                , request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData).Data;
                wCfObjectReturnToClient.Promotion = GetProductPromotion(promotionClientArg, request.Targeting);
            }

            if (!request.ShowUserEditableFieldsForSearch)
            {
                var notes = new List<NoteClientObject>();

                var cart = await CartDAOManager.Instance.GetPrimaryCartAsync(request.UserId);// cartManager.GetPrimaryCart();
                if (cart == null)
                {
                    foreach (var key in btKeyList)
                        notes.Add(new NoteClientObject { BTKey = key });
                }
                else
                {
                    notes = await GridDAOManager.Instance.GetNotesByBTKeysAsync(cart.CartId, request.UserId, btKeyList);
                    if (notes != null)
                    {
                        //notes = GridHelper.GetNotes(cart.CartId, siteContextObject.UserId, btKeyList);
                        foreach (var key in btKeyList)
                        {
                            var i = notes.FindIndex(note => note.BTKey == key);
                            if (i < 0)
                            {
                                notes.Add(new NoteClientObject { BTKey = key });
                            }
                        }
                    }
                }
                //}

                wCfObjectReturnToClient.NotesList = notes;
            }

            if (!request.ShowEnhancedContentIconsForSearch)
            {
                wCfObjectReturnToClient.HasCPSIAWarning = productItem.HasCPSIA;
                wCfObjectReturnToClient.HasPawPrint = !string.IsNullOrEmpty(productItem.ProductLine) &&
                                  productItem.ProductLine.Equals(SpecialProductAttributes.PawPrintsProductLine);
                wCfObjectReturnToClient.HasGardners = !string.IsNullOrEmpty(productItem.SupplierCode) &&
                                productItem.SupplierCode.Equals(SpecialProductAttributes.GardnersSupplierCode);
                wCfObjectReturnToClient.HasLargePrint = (!string.IsNullOrEmpty(productItem.MerchCategory) &&
                                     productItem.MerchCategory.Equals(
                                         SpecialProductAttributes.LargePrintMerchCategory))
                                    ||
                                    (!string.IsNullOrEmpty(productItem.Edition) &&
                                     (productItem.Edition.Equals(SpecialProductAttributes.LargePrintEdition)
                                      ||
                                      productItem.Edition.Equals(
                                          SpecialProductAttributes.LargePrintAltEdition)));
            }

            productItem.ISBNLookUpLink = ProductSearchController.CreateIsbnLookupLink(productItem.ISBN, productItem.ISBN10, request.Targeting.OrgId);
            string UPCLink = ProductSearchController.CreateUpcLookupLink(productItem.Upc, request.Targeting.OrgId);
            if (!request.ShowProductLookupForSearch)
            {
                //Create product lookup link
                if (productItem.ProductType == ProductTypeConstants.Book || productItem.ProductType == ProductTypeConstants.eBook || UPCLink == ProductLookupLinkConstant.UPCUseISBN)
                {
                    var urlObject = productItem.ISBNLookUpLink;
                    if (urlObject != null)
                    {
                        if (!string.IsNullOrEmpty(urlObject.ISBN13Link) || !string.IsNullOrEmpty(urlObject.ISBN10Link))
                        {
                            if (!string.IsNullOrEmpty(urlObject.ISBN13Link))
                            {
                                wCfObjectReturnToClient.ProductLookupLink13HasOnClick = true;
                                wCfObjectReturnToClient.ProductLookupLink13 = string.Format("OpenProductLookup('{0}','{1}');",
                                 urlObject.ISBN13Link,
                                 ProductLookupLinkConstant.OpenProductLookup);
                            }

                            if (!string.IsNullOrEmpty(urlObject.ISBN10Link))
                            {
                                wCfObjectReturnToClient.ProductLookupLinkHasOnClick = true;
                                wCfObjectReturnToClient.ProductLookupLink = string.Format("OpenProductLookup('{0}','{1}');",
                                 urlObject.ISBN10Link,
                                 ProductLookupLinkConstant.OpenProductLookup);
                            }
                        }
                    }
                }
                else
                {
                    if (UPCLink != ProductLookupLinkConstant.UPCDeactivated)
                    {
                        if (!string.IsNullOrEmpty(UPCLink))
                        {
                            wCfObjectReturnToClient.UPCProductLookupLink = string.Format("OpenProductLookup('{0}','{1}');",
                             UPCLink,
                             ProductLookupLinkConstant.OpenProductLookup);
                        }
                    }
                }
            }

            return wCfObjectReturnToClient;
        }

        public string GetUserAlertCount(string userId)
        {
            //var cacheKey = string.Format(DistributedCacheKey.UserCountCacheKey, userId);

            //var result = CachingController.Instance.Read(cacheKey) as string;
            //if (!string.IsNullOrEmpty(result))
            //{
            //    return result;
            //}

            int unReadAlertsCount;
            int hasReadAlertsCount;

            BTAlertDAOManager.Instance.GetUserAlertsCount(userId, out unReadAlertsCount, out hasReadAlertsCount);

            var result = string.Format("{0}#{1}", unReadAlertsCount + hasReadAlertsCount, unReadAlertsCount);
            //CachingController.Instance.Write(cacheKey, result, 7);

            return result;
        }

        public async Task<AppServiceResult<bool>> UpdateTSSONotificationCartUsers(List<string> activeNotificationCartUsers, List<string> removeNotificationCartUsers)
        {
            var ajaxResult = new AppServiceResult<bool>();
            try
            {
                var updateStatus = await ProfileDAOManager.Instance.UpdateTSSONotificationCartUsers(activeNotificationCartUsers, removeNotificationCartUsers);
                ajaxResult.Data = true;

            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.User);
                ajaxResult.ErrorMessage = ex.Message;
                ajaxResult.Data = false;
            }
            return ajaxResult;
        }

        public async Task<AppServiceResult<bool>> UpdateUserNRCProductTypes(string userID, string[] NRCProductTypes)
        {
            var ajaxResult = new AppServiceResult<bool>();
            try
            {
                string strNRCProductTypes = string.Join(";", NRCProductTypes);

                var updateStatus = await ProfileDAOManager.Instance.UpdateUserNRCProductTypes(userID, strNRCProductTypes);
                ajaxResult.Data = true;

            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.User);
                ajaxResult.ErrorMessage = ex.Message;
                ajaxResult.Data = false;
            }
            return ajaxResult;
        }

        public WCFObjectReturnToClient GetInventoryForTitleList(SearchRequest request)
        {
            var wCfObjectReturnToClient = new WCFObjectReturnToClient { Message = "Sequential" };
            //

            /*var siteContextObject = request.SiteContext;

            if (siteContextObject == null)
            {
                return null;
            }*/

            var inventoryStatusArgList = new List<SearchResultInventoryStatusArg>();
            ProductSearchResults productSearchResults = null; ;
            if (request.btKeyList != null && request.btKeyList.Count > 0)
            {
                productSearchResults = ProductSearchController.SearchByIdWithoutAnyRules(request.btKeyList);
            }

            if (productSearchResults != null)
            {
                foreach (var item in productSearchResults.Items)
                {
                    inventoryStatusArgList.Add(GetSearchResultInventoryStatusArg(item, request.MarketTypeString));
                }

                var inventoryHelper = InventoryHelper4MongoDb.GetInstance(userId: request.UserId,
                    countryCode: request.CountryCode, marketType: request.MarketType, orgId: request.OrgId);

                //Inventory Result
                wCfObjectReturnToClient.InventoryResultsList.AddRange(
                    inventoryHelper.GetInventoryResultsForMultipleItems(inventoryStatusArgList));

                //Inventory Status
                wCfObjectReturnToClient.InventoryStatus = inventoryHelper.InventoryStatusList;
            }
            return wCfObjectReturnToClient;
        }

        public async Task<BTKeyListForTitleListResponse> GetBTKeyListForTitleList(BTKeyListForTitleListRequest request)
        {
            var result = new BTKeyListForTitleListResponse();           
            result.BTKeys = new List<string>();
            result.BTEKeys = new List<string>();
            result.PrimaryCartTitleDetails = new List<PrimaryCartTitleDetail>();

            ProductSearchResults productSearchResults = null;

            productSearchResults = ProductSearchController.SearchByIdWithoutAnyRules(request.BTKeys);

            if (productSearchResults == null)
            {
                return result;
            }

            foreach (var item in productSearchResults.Items)
            {
                result.BTKeys.Add(item.BTKey);
                result.BTEKeys.Add(item.BTEKey);
            }
            var cartManager = new CartManager(request.UserId);
            var primaryCart = cartManager.GetPrimaryCart();
            if (primaryCart != null)
            {
                var primaryCartQuantities = cartManager.GetQuantitiesByBtKeys(primaryCart.CartId, result.BTKeys);
                var lineitems = await CartDAOManager.Instance.GetLineItemIDs(primaryCart.CartId);
                var lineItemDictionary = new Dictionary<string, string>();
                if (lineitems != null)
                {
                    foreach (var lineItem in lineitems)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(lineItem.BTKey))
                                lineItemDictionary.Add(lineItem.BTKey, lineItem.Id);
                        }
                        catch
                        {
                            continue;
                        }

                    }
                }
                foreach (var btkey in result.BTKeys)
                {
                    var primaryCartTitleDetail = new PrimaryCartTitleDetail();
                    primaryCartTitleDetail.BTKey = btkey;


                    if (primaryCart != null)
                    {
                        if (primaryCartQuantities != null && primaryCartQuantities.ContainsKey(btkey))
                        {
                            primaryCartTitleDetail.PrimaryQuantity = primaryCartQuantities[btkey];
                        }
                        if (primaryCartQuantities.ContainsKey(btkey))
                        {
                            primaryCartTitleDetail.IsInPrimaryCart = true;
                        }
                        if (lineItemDictionary != null && lineItemDictionary.ContainsKey(btkey))
                        {
                            primaryCartTitleDetail.LineItemId = lineItemDictionary[btkey];
                        }
                    }
                    result.PrimaryCartTitleDetails.Add(primaryCartTitleDetail);
                }
            }

            //var userProfile = ProfileService.Instance.GetUserById(request.UserId);
            //var defaultDuplicateOrders = userProfile.DefaultDuplicateOrders;
            //var defaultDuplicateCarts = userProfile.DefaultDuplicateCarts;

            //var dupResult = await GetProductDuplicateIndicator(btKeyList, btEKeyList, string.Empty,
            //    !string.Equals(defaultDuplicateCarts, "none", StringComparison.OrdinalIgnoreCase),
            //    !string.Equals(defaultDuplicateOrders, "none", StringComparison.OrdinalIgnoreCase), request.UserId,
            //            request.DefaultDownloadedCarts, request.CollectionAnalysisEnabled, request.OrgId);

            //wCfObjectReturnToClient.Duplicate = dupResult.Data;
            return result;
        }

        public async Task<GetCalendarDataResponse> GetCalendarData(GetCalendarDataRequest request)
        {
            var result = new GetCalendarDataResponse();
            result.CalendarData = new List<CalendarData>();

            var notInCacheProductTypes = new List<string>();

            var nrcController = new NewReleaseCalendarController(request.ProductTypesList,
                                                                 request.CalendarView,
                                                                 request.Month,
                                                                 request.Year);
            // get cached data first
            var nrcItems = nrcController.GetCachedData(out notInCacheProductTypes);

            if (notInCacheProductTypes.Count > 0)
            {
                // get data from SharePoint
                var productTypeGroups = nrcController.GetSharePointItems(notInCacheProductTypes);
                
                if (productTypeGroups.Count > 0)
                {
                    foreach (var groupItem in productTypeGroups)
                    {
                        // put items by each ProductType to cache
                        var productTypeEnum = (ProductTypeEx)Enum.Parse(typeof(ProductTypeEx), groupItem.Key);
                        nrcController.WriteItemsToCache(groupItem.Value, productTypeEnum);

                        nrcItems.AddRange(groupItem.Value);
                    }
                }
            }

            // filter items by calendar view (streetdate/preorderdate)
            var listFilterByCalendarView = nrcController.FilterByCalendarView(nrcItems);

            if (listFilterByCalendarView.Count > 0)
            {
                var itemsGroupByActiveDate = listFilterByCalendarView.GroupBy(q => q.ActiveDate).ToList();

                foreach (var itemGroupByActiveDate in itemsGroupByActiveDate)
                {
                    int totalBooks = 0;
                    int totalDigitals = 0;
                    int totalMovies = 0;
                    int totalMusic = 0;

                    foreach (var item in itemGroupByActiveDate)
                    {
                        switch (item.ProductType)
                        {
                            case ProductTypeConstants.Book:
                                totalBooks += item.BTKeys.Count;
                                break;
                            case ProductTypeConstants.Digital:
                                totalDigitals += item.BTKeys.Count;
                                break;
                            case ProductTypeConstants.Movie:
                                totalMovies += item.BTKeys.Count;
                                break;
                            case ProductTypeConstants.Music:
                                totalMusic += item.BTKeys.Count;
                                break;
                        }
                    }

                    var calendarItem = new CalendarData
                    {
                        ActiveDate = itemGroupByActiveDate.Key.Date.ToShortDateString(),
                        TotalBooks = totalBooks,
                        TotalDigitals = totalDigitals,
                        TotalMovies = totalMovies,
                        TotalMusics = totalMusic
                    };
                    result.CalendarData.Add(calendarItem);
                }
            }
            return result;
        }

        public GetNRCFeaturedProductsResponse GetNRCFeaturedProducts(GetNRCFeaturedProductsRequest request)
        {
            var response = new GetNRCFeaturedProductsResponse
            {
                ProductItems = new List<ProductItem>(),
                RemainingBTKeys = new List<string>()
            };

            if (request.ProductTypesList == null || request.ProductTypesList.Count() == 0)
                return response;

            var productTypesList = request.ProductTypesList.ToList();
            var digitalTypeId = ((int)ProductTypeEx.Digital).ToString();
            var digitalIndex = productTypesList.FindIndex(p => p == digitalTypeId);
            if (digitalIndex >= 0)
            {
                var eList = new List<string>(2);
                eList.Add(((int)ProductTypeEx.DigitaleBook).ToString());
                eList.Add(((int)ProductTypeEx.DigitaleAudio).ToString());

                productTypesList.RemoveAt(digitalIndex);
                productTypesList.InsertRange(digitalIndex, eList);
            }

            List<string> notInCacheProductTypes;
            var featuredController = new FeaturedTitlesController(productTypesList.ToArray(),
                                                                 request.CalendarView,
                                                                 request.Month,
                                                                 request.Year);

            var featuredTitlesResult = featuredController.GetCachedData(out notInCacheProductTypes);

            if (notInCacheProductTypes.Count > 0)
            {
                // get data from SharePoint
                var productTypeGroups = featuredController.GetSharePointItems(notInCacheProductTypes);

                if (productTypeGroups.Count > 0)
                {
                    foreach (var groupItem in productTypeGroups)
                    {
                        // put items by each ProductType to cache
                        var productTypeEnum = (ProductTypeEx)Enum.Parse(typeof(ProductTypeEx), groupItem.Key);
                        featuredController.WriteItemsToCache(groupItem.Value, productTypeEnum);

                        featuredTitlesResult.AddRange(groupItem.Value);
                    }
                }
            }

            int maxBtKeysPerType = request.MaxItemsPerProductType;
            if (maxBtKeysPerType <= 0)
                maxBtKeysPerType = 10;

            // filter items by calendar view (streetdate/preorderdate)
            var listFilterByCalendarView = featuredController.FilterByCalendarView(featuredTitlesResult, maxBtKeysPerType);

            if (listFilterByCalendarView.Count > 0)
            {
                var bookBTKeysSearch = new List<string>();
                var listBTkeySearch = new List<string>();

                // sort follow request
                foreach (var productTypeId in productTypesList)
                {
                    var productType = (ProductTypeEx)Enum.Parse(typeof(ProductTypeEx), productTypeId);
                    var list = listFilterByCalendarView.FirstOrDefault(x => x.ProductType == productType.ToString());
                    if (list != null && list.BTKeys != null)
                    {
                        if (productType == ProductTypeEx.Book)
                            bookBTKeysSearch.AddRange(list.BTKeys.Take(maxBtKeysPerType));
                        else
                            listBTkeySearch.AddRange(list.BTKeys.Take(maxBtKeysPerType));
                    }
                }

                List<ProductItem> fastProducts;
                List<string> remainingBTKeys;

                if (bookBTKeysSearch.Count > 0)
                {
                    // Fast search by book BTKeys first
                    fastProducts = CommonHelper.GetNRCFeaturedProductsByFastSearch(bookBTKeysSearch, bookBTKeysSearch.Count, out remainingBTKeys, ignoreBookCheck: true);
                    remainingBTKeys = listBTkeySearch;
                }
                else
                {
                    // Fast search by BTKeys
                    var maxItem = 10;
                    fastProducts = CommonHelper.GetNRCFeaturedProductsByFastSearch(listBTkeySearch, maxItem, out remainingBTKeys);
                }

                // response data
                response.ProductItems = fastProducts;
                response.RemainingBTKeys = remainingBTKeys;
            }

            return response;
        }

        public GetNRCFeaturedProductsResponse GetNRCFeaturedProductsByBTKeys(List<string> btKeys, int batchSize)
        {
            var response = new GetNRCFeaturedProductsResponse();

            // Fast search by BTKeys
            List<string> remainingBTKeys;
            var fastProducts = CommonHelper.GetNRCFeaturedProductsByFastSearch(btKeys, batchSize, out remainingBTKeys);

            // response data
            response.ProductItems = fastProducts;
            response.RemainingBTKeys = remainingBTKeys;

            return response;
        }

        public NRCAltFormatsResponse GetNRCAltFormats(NRCAltFormatsRequest request)
        {
            var response = new NRCAltFormatsResponse();

            var requestContext = request.UserContext;
            var userId = request.UserId;
            var marketType = requestContext.MarketType;
            List<string> productBtKeyList; 

            if (request.RemainingBTKeys != null && request.RemainingBTKeys.Count > 0)
                productBtKeyList = request.RemainingBTKeys;
            else
            {
                // add alt format BTkeys from DB
                productBtKeyList = ProductCatalogDAO.Instance.GetRelatedProductIds(request.BTKey);
                productBtKeyList.Insert(0, request.BTKey); // add itself first
            }

            if (productBtKeyList != null && productBtKeyList.Count > 0)
            {
                var maxItemNumber = request.MaxItemNumber > 0 ? request.MaxItemNumber : productBtKeyList.Count;
                int searchBatchSize = 30;

                var searchArg = new SearchByBTKeysArgument
                {
                    BTKeyList = productBtKeyList,
                    PageSize = maxItemNumber,
                    MarketType = marketType,
                    ESuppliers = requestContext.ESuppliers,
                    SimonSchusterEnabled = requestContext.SimonSchusterEnabled,
                    CountryCode = requestContext.CountryCode,
                    IncludeProductFilter = true,
                    UserId = userId
                };

                // search products from FAST by batch of btkeys
                var products = SearchBTKeysByBatch(searchArg, searchBatchSize);

                if (products.Count > 0)
                {
                    // get product qty in Primary Cart
                    var productsWithQty = new Dictionary<string, int>();
                    if (!string.IsNullOrEmpty(request.PrimaryCartId))
                    {
                        var btKeys = products.Select(r => r.BTKey).ToList();
                        var cartManager = new CartManager(userId);
                        productsWithQty = cartManager.GetQuantitiesByBtKeys(request.PrimaryCartId, btKeys);
                    }

                    // convert to response items
                    var responseItems = new List<NRCAltFormatItem>();
                    var resultBTKeys = new List<string>();
                    foreach (var searchItem in products)
                    {
                        resultBTKeys.Add(searchItem.BTKey);
                        var productInPrimaryCart = productsWithQty.FirstOrDefault(r => r.Key == searchItem.BTKey);
                        var allowAddToPrimaryCart = !string.IsNullOrEmpty(request.PrimaryCartId) && string.IsNullOrEmpty(productInPrimaryCart.Key);
                        int? qty = !string.IsNullOrEmpty(productInPrimaryCart.Key) ? productInPrimaryCart.Value : (int?)null;
                        var isMakerspace = CommonHelper.IsMakerspaceProductFormat(searchItem.ProductFormat, searchItem.MerchCategory);

                        var altFormatItem = new NRCAltFormatItem
                        {
                            BTKey = searchItem.BTKey,
                            ProductType = searchItem.ProductType,
                            ProductCode = searchItem.ProductCode,
                            ISBN = searchItem.ISBN,
                            ISBN10 = searchItem.ISBN10,
                            ProductFormat = isMakerspace ? ProductFormatConstants.Book_Makerspace : searchItem.ProductFormat,
                            ProductFormatForUI = searchItem.ProductFormatForUI,
                            FormatIconType = Path.GetFileNameWithoutExtension(searchItem.FormatIconPath),
                            Title = searchItem.Title,
                            Upc = searchItem.Upc,
                            Author = searchItem.Author,
                            Publisher = searchItem.Publisher,
                            PublishDateText = searchItem.PublishDate.ToString(CommonConstants.DefaultDateTimeFormat),
                            LabelOrStudio = searchItem.Label,
                            Edition = searchItem.Edition,
                            ListPrice = searchItem.ListPrice,
                            ListPriceText = searchItem.ListPriceText,
                            AllowAddToPrimaryCart = allowAddToPrimaryCart,
                            Quantity = qty,

                            // values for pricing
                            AcceptableDiscount = searchItem.AcceptableDiscount,
                            GTIN = searchItem.GTIN,
                            Catalog = searchItem.Catalog,
                            HasReturn = searchItem.HasReturn,
                            PriceKey = searchItem.PriceKey,
                            ProductLine = searchItem.ProductLine,
                            ESupplier = searchItem.ESupplier,
                            PurchaseOption = searchItem.PurchaseOption.Trim(),
                        };

                        responseItems.Add(altFormatItem);
                    }

                    response.NRCAltFormats = responseItems;

                    // define remaining btkeys to response
                    if (maxItemNumber < productBtKeyList.Count && resultBTKeys.Count() == maxItemNumber)
                    {
                        var remainingBTKeys = productBtKeyList.Where(r => !resultBTKeys.Contains(r));
                        response.RemainingBTKeys = remainingBTKeys.ToList();
                    }
                }
            }

            return response;
        }

        public PricingForProductsResponse GetPricingForProducts(PricingForProductsRequest request)
        {
            var response = new PricingForProductsResponse();

            if (!string.IsNullOrEmpty(request.UserId) &&
                request.Products != null && request.Products.Count() > 0)
            {
                response.ProductPrices = GetProductPricing(request.Products, request.UserId, request.ESuppliers
                    , request.IsHideNetPriceDiscountPercentage, request.Targeting, request.AccountPricingData).Data;
            }

            return response;
        }

        public async Task<Axis360InventoryResponse> GetCirculationByISBN(Axis360CirculationRequest request)
        {
            var axis360InventoryResponse = new Axis360InventoryResponse();

            if (request != null)
            {
                axis360InventoryResponse = await Axis360InventoryHelper.GetCirculationByISBN(request.ISBN, request.CustomerID);
            }


            return axis360InventoryResponse;
        }

        public async Task<Axis360CheckInventoryResponse> CheckForCirculationByISBN(Axis360CheckCirculationRequest request)
        {
            var axis360InventoryResponse = new Axis360CheckInventoryResponse();

            if (request != null)
            {
                var eSupplierAccountNumber = await ProfileDAOManager.Instance.GeteSupplierAccountNumber(request.UserID, request.BasketSummaryID);
                if (!string.IsNullOrWhiteSpace(eSupplierAccountNumber))
                {
                    axis360InventoryResponse = await Axis360InventoryHelper.CheckForCirculationByISBN(request.ISBN, eSupplierAccountNumber);
                    axis360InventoryResponse.ESupplierAccountNumber = eSupplierAccountNumber;
                }
            }


            return axis360InventoryResponse;
        }

        public async Task<string> GeteSupplierAccountNumber(Axis360CheckCirculationRequest request)
        {
            var eSupplierAccountNumber = string.Empty;

            if (request != null)
            {
                eSupplierAccountNumber = await ProfileDAOManager.Instance.GeteSupplierAccountNumber(request.UserID, request.BasketSummaryID);
            }

            return eSupplierAccountNumber;
        }

        public async Task<DiversityProductsResponse> GetDiversityClassificationByBTKeys(DiversityProductsRequest request)
        {
            var getResult = await ProductsDAOManager.Instance.GetDiversityClassificationByBTKeys(request);


            return getResult;
        }


    }
}
