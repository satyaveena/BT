using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BT.TS360API.Cache;
using BT.TS360API.Common.Configrations;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ExternalServices;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Inventory;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360Constants;
using TS360Constants;
using AppSettings = BT.TS360API.Common.Configrations.AppSettings;

namespace BT.TS360API.Common.Inventory
{
    public class InventoryHelper4MongoDb
    {
        const string FALSE = "0";
        const string TRUE = "1";
        private const string LogCategory = "MONGODB";
        public const string Not_Available_for_Shipping = "Not_Available_for_Shipping";
        public const string Available_in_Primary_Warehouse = "Available_in_Primary_Warehouse";
        public const string Available_in_Secondary_Warehouse = "Available_in_Secondary_Warehouse";
        public const string Available_in_Other_Warehouses = "Available_in_Other_Warehouses";
        public const string Available_in_VIP_warehouses = "Available_in_VIP_warehouses";

        #region Members
        private Dictionary<string, Account> _defaultAccountsDict;
        //private Dictionary<string, AccountDaoObject> _defaultAccountsDaoDict;
        private Dictionary<string, BTKeyInventoryResult> _inventoryResultsDict;
        private List<SiteTermObject> _inventoryStatusList;
        private static InventoryHelper4MongoDb _instance;
        private string _userId;
        private MarketType? _marketType;
        private List<string> _warehouseIdsList;
        private Account _homeDeliveryAccount;
        //private AccountDaoObject _homeDeliveryAccountDao;
        #endregion

        #region Properties
        private string _inventoryDemandUrl;
        private Uri InventoryDemandUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_inventoryDemandUrl))
                {
                    _inventoryDemandUrl = AppSettings.NoSqlApiUrlInventoryDemand;// GlobalConfiguration.ReadAppSetting("NoSQLApiUrl_InventoryDemand").Value;
                }
                return new Uri(_inventoryDemandUrl);
            }
        }

        private string _demandHistoryUrl;
        private Uri DemandHistoryUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_demandHistoryUrl))
                {
                    _demandHistoryUrl = AppSettings.NoSqlApiUrlDemandHistory;// GlobalConfiguration.ReadAppSetting("NoSQLApiUrl_DemandHistory").Value;
                }
                return new Uri(_demandHistoryUrl);
            }
        }

        private string _inventoryFacetUrl;
        private Uri CartInventoryFacetUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_inventoryFacetUrl))
                {
                    _inventoryFacetUrl = AppSettings.NoSqlApiUrlCartInventoryFacet;// GlobalConfiguration.ReadAppSetting("NoSQLApiUrl_CartInventoryFacet").Value;
                }
                return new Uri(_inventoryFacetUrl);
            }
        }

        private List<Warehouse> _warehousesList;
        private List<Warehouse> Warehouses
        {
            get
            {

                return _warehousesList ??
                (_warehousesList = ProfileDAO.Instance.GetWareHouses());
            }
        }

        // Code: Desc
        private Dictionary<string, string> _warehouseDesc;
        public Dictionary<string, string> WarehouseDesc
        {
            get
            {
                if (_warehouseDesc != null) return _warehouseDesc;
                _warehouseDesc = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var warehouse in Warehouses)
                {
                    if (string.IsNullOrEmpty(warehouse.Code)) continue;

                    if (_warehouseDesc.ContainsKey(warehouse.Code))
                    {
                        _warehouseDesc[warehouse.Code] = warehouse.Description;
                    }
                    else
                    {
                        _warehouseDesc.Add(warehouse.Code, warehouse.Description);
                    }
                }
                return _warehouseDesc;
            }
        }

        #endregion

        #region Public Methods

        public string CartId { get; set; }
        public string UserId { get; set; }
        public string CountryCode { get; set; }
        public string OrgId { get; set; }

        public MarketType? MarketType { get; set; }

        public List<SiteTermObject> InventoryStatusList
        {
            get { return _inventoryStatusList; }
        }

        public List<string> WarehouseIDsList
        {
            get
            {
                if (_warehouseIdsList != null && _warehouseIdsList.Any())
                    return _warehouseIdsList;
                _warehouseIdsList = GetWarehouseIDs();
                return _warehouseIdsList;
            }
            set { _warehouseIdsList = value; }
        }

        public static InventoryHelper4MongoDb GetInstance(string cartId="", string userId="", MarketType? marketType=null, string countryCode="",
            string orgId = "")
        {
            if (string.IsNullOrEmpty(userId))
                userId = ServiceRequestContext.Current.UserId; 

            if (_instance == null) return new InventoryHelper4MongoDb() { CartId = cartId, UserId = userId, MarketType = marketType,
                                                                          CountryCode = countryCode,
                                                                          OrgId = orgId
            };

            _instance.CartId = cartId;
            _instance.UserId = userId;
            _instance.MarketType = marketType;
            _instance.CountryCode = countryCode;
            _instance.OrgId = orgId;

            return _instance;
        }

        public List<InventoryResults> GetInventoryResultsForMultipleItems(IList<SearchResultInventoryStatusArg> args)
        {
            var results = new List<InventoryResults>();
            if (args == null || !args.Any()) return results;
            //Call to WebApi to get inventory result for all items from MongoDb
            //Dictionary [BTKEY, InventoryResults]
            GetInventoryResultsDict(args);

            //Set values for individual product item based on result above
            var displayInventoryForAllWareHouse = IsDisplayAllWarehouse();

            foreach (var item in args)
            {
                var productType = CommonHelper.Instance.RefineProductTypeToMusicIfMovie(item.ProductType); // InventoryHelper.RefineProductTypeToMusicIfMovie(item.ProductType);

                //ignore ebooks
                if (string.Compare(productType, ProductType.Book.ToString(), StringComparison.OrdinalIgnoreCase) == 0
                    && !string.IsNullOrEmpty(item.ESupplier))
                    continue;

                var account = GetAccountByProductType(productType);
                var showAllWhs = displayInventoryForAllWareHouse || (account == null);

                int last30Demand;
                bool hasDemand, isEastWarehouseSet;
                var inventoryItems = GetInventoryResultsForSingleItemInternal(item, true, account,
                    showAllWhs, out last30Demand, out hasDemand, out isEastWarehouseSet);

                // TFS 24744 Do not Display South Warehouse for AV Products
                if (isEastWarehouseSet)
                {
                    showAllWhs = true;  // to display East warehouse only
                }
                else
                {
                    inventoryItems = InventoryHelper.CorrectAndFilterInventory(inventoryItems, MarketType);
                }

                var inventoryResults = new InventoryResults
                {
                    DisplayInventoryForAllWareHouse = showAllWhs,
                    InventoryStock = inventoryItems,
                    BTKey = item.BTKey,
                    ProductType = item.ProductType,
                    TotalLast30Demand = last30Demand
                };

                results.Add(inventoryResults);
            }

            return results;
        }

        public List<SiteTermObject> GetInventoryStatus(IList<SearchResultInventoryStatusArg> args)
        {
            if (args == null || !args.Any()) return _inventoryStatusList;
            GetInventoryResultsDict(args);

            return _inventoryStatusList;
        }

        public List<InventoryStockStatus> GetInventoryResultsForSingleItem(SearchResultInventoryStatusArg searchArg,
            bool isHorizontalMode, out bool displayInventoryForAllWareHouse, out int last30DaysDemand, out bool hasDemand)
        {
            //
            var args = new List<SearchResultInventoryStatusArg>() { searchArg };
            //Call to WebApi to get inventory result for all items from MongoDb
            //Dictionary [BTKEY, InventoryResults]
            GetInventoryResultsDict(args);

            displayInventoryForAllWareHouse = IsDisplayAllWarehouse();

            var productType = InventoryHelper.RefineProductTypeToMusicIfMovie(searchArg.ProductType);
            var account = GetAccountByProductType(productType);
            displayInventoryForAllWareHouse = displayInventoryForAllWareHouse || (account == null);

            bool isEastWarehouseSet;
            var inventoryItems = GetInventoryResultsForSingleItemInternal(searchArg, isHorizontalMode, account,
                displayInventoryForAllWareHouse, out last30DaysDemand, out hasDemand, out isEastWarehouseSet);

            return inventoryItems;
        }

        public Dictionary<string, BTKeyInventoryResult> GetInventoryWarehouseData(
            IList<SearchResultInventoryStatusArg> args)
        {
            var result = new Dictionary<string, BTKeyInventoryResult>();
            if (args == null || !args.Any()) return result;

            GetInventoryResultsDict(args);
            //[BTKEY: [WarehouseId: demand]]

            foreach (var arg in args)
            {
                if (string.IsNullOrEmpty(arg.BTKey)) continue;

                if (!result.ContainsKey(arg.BTKey) && _inventoryResultsDict.ContainsKey(arg.BTKey))
                {
                    var invRes = _inventoryResultsDict[arg.BTKey];
                    result.Add(arg.BTKey, invRes);
                }
            }
            return result;
        }

        public DemandHistoryResponse GetDemandHistory(string btkey, int pageIndex, string primaryWarehouse,
            string secondaryWarehouse)
        {
            var request = new DemandHistoryRequest()
            {
                BTKey = btkey,
                PageIndex = pageIndex,
                PrimaryWareHouseCode = primaryWarehouse,
                SecondaryWareHouseCode = secondaryWarehouse
            };

            var response = GetDemandHistory(request, DemandHistoryUrl);
            if (response != null)
            {
                if (response.Status == NoSqlServiceStatus.Success)
                    return response.Data;
                Logger.Write(LogCategory,
                        string.Format("MongoDb WebAPI Call For DemandHistory {0}, Error Code: {1}, Error Message {2}", response.Status,
                            response.ErrorCode, response.ErrorMessage));
            }
            return null;
        }

        //public Dictionary<string, BTKeyInventoryResult> GetRawInventoryResults(
        //    IList<SearchResultInventoryStatusArg> args)
        //{
        //    if (args == null || !args.Any()) return null;
        //    //Call to WebApi to get inventory result for all items from MongoDb
        //    //Dictionary [BTKEY, InventoryResults]
        //    GetInventoryResultsDict(args);

        //    return _inventoryResultsDict;
        //}

        //public List<InventoryStockStatus> GetInventoryForHomeDelivery(IList<SearchResultInventoryStatusArg> args,
        //    Account hdAccount, string userId)
        //{
        //    UserId = userId;
        //    _homeDeliveryAccount = hdAccount;
        //    if (args == null || !args.Any()) return null;

        //    string primaryWh, secondaryWh;
        //    ProductSearchController.GetPrimarySecondaryWareHouse(out primaryWh, out secondaryWh, hdAccount);
        //    _warehouseIdsList = new List<string>() { primaryWh, secondaryWh };

        //    //Call to WebApi to get inventory result for all items from MongoDb
        //    //Dictionary [BTKEY, InventoryResults]
        //    GetInventoryResultsDict(args);
        //    if (_inventoryResultsDict == null) return null;

        //    var result = new List<InventoryStockStatus>();
        //    foreach (var arg in args)
        //    {
        //        var item = new InventoryStockStatus();
        //        if (!string.IsNullOrEmpty(arg.BTKey) && _inventoryResultsDict.ContainsKey(arg.BTKey))
        //        {
        //            var warehouses = _inventoryResultsDict[arg.BTKey].Warehouses;
        //            var sumOnOrderQty = 0;
        //            var sumQuantityAvailable = 0;
        //            foreach (var wh in warehouses)
        //            {
        //                sumOnOrderQty += wh.OnOrderQuantity;
        //                sumQuantityAvailable += wh.InStockForRequest;
        //            }
        //            item.OnOrderQuantity = sumOnOrderQty;
        //            item.QuantityAvailable = sumQuantityAvailable;
        //        }
        //        result.Add(item);
        //    }
        //    return result;
        //}

        //public CartLineFacet GetInventoryFacetForCartDetails(IList<SearchResultInventoryStatusArg> args, string facetPath = "",
        //    int nonInventoryTitleCount = 0)
        //{
        //    string matchingBtkeys;
        //    return GetInventoryFacetForCartDetails(args, out matchingBtkeys, facetPath, nonInventoryTitleCount);
        //}

        //public CartLineFacet GetInventoryFacetForCartDetails(IList<SearchResultInventoryStatusArg> args, out string matchingBtkeys, string facetPath = "", int nonInventoryTitleCount = 0)
        //{
        //    matchingBtkeys = "";
        //    var cartLineFacet = new CartLineFacet()
        //    {
        //        Level = 0,
        //        Text = BasketLineItemFacet.Inventory
        //    };
        //    var request = PrepareInventoryFacetRequest(args, facetPath);
        //    if (request == null) return new CartLineFacet();
        //    //
        //    var response = GetInventoryFacetResults(request, CartInventoryFacetUrl);
        //    if (response != null)
        //    {
        //        if (response.Status == NoSqlServiceStatus.Success)
        //        {
        //            if (response.Data != null)
        //            {
        //                matchingBtkeys = response.Data.MatchingBTKeys;
        //            }
        //            if (response.Data != null && response.Data.CartInventoryFacetsResults != null &&
        //                response.Data.CartInventoryFacetsResults.Any())
        //            {
        //                _inventoryResultsDict = new Dictionary<string, BTKeyInventoryResult>();
        //                _inventoryStatusList = new List<SiteTermObject>();
        //                bool hasNotAvailableItem = false;
        //                foreach (var res in response.Data.CartInventoryFacetsResults)
        //                {
        //                    var node = new CartLineFacet()
        //                    {
        //                        Level = 1,
        //                        Parent = cartLineFacet,
        //                        Text = res.Facet,
        //                        Value = res.ItemCount.ToString()
        //                    };
        //                    //plus OE + Ebook as it's always not available for shipping
        //                    if (node.Text.Equals(Not_Available_for_Shipping))
        //                    {
        //                        var temp = res.ItemCount + nonInventoryTitleCount;
        //                        node.Value = temp.ToString();
        //                        hasNotAvailableItem = true;
        //                    }
        //                    cartLineFacet.Nodes.Add(node);
        //                }
        //                //OE and Ebook
        //                if (!hasNotAvailableItem && nonInventoryTitleCount > 0)
        //                {
        //                    var node = new CartLineFacet()
        //                    {
        //                        Level = 1,
        //                        Parent = cartLineFacet,
        //                        Text = Not_Available_for_Shipping,
        //                        Value = nonInventoryTitleCount.ToString()
        //                    };
        //                    cartLineFacet.Nodes.Insert(0, node);//always the first node for Not_Available_for_Shipping
        //                }
        //                return cartLineFacet;
        //            }
        //        }
        //        else
        //        {
        //            Logger.Write(LogCategory,
        //                string.Format("MongoDb WebAPI Call For InventoryFacet {0}, Error Code: {1}, Error Message {2}", response.Status,
        //                    response.ErrorCode, response.ErrorMessage), false);
        //        }
        //    }
        //    return new CartLineFacet();
        //}

        //#endregion

        //#region Private Methods

        private List<InventoryStockStatus> GetInventoryResultsForSingleItemInternal(SearchResultInventoryStatusArg product,
            bool isHorizontalMode, Account account, bool displayInventoryForAllWareHouse, out int last30Demand, out bool hasDemand, out bool isEastWarehouseSet)
        {
            last30Demand = 0;
            isEastWarehouseSet = false;
            //Get primary and secondary ware house
            string primaryWareHouse, secondWareHouse;
            GetPrimarySecondaryWarehouses(account, out primaryWareHouse, out secondWareHouse);

            WriteWhsInfoToSessionForLoadFacetUsing(primaryWareHouse, secondWareHouse);

            //Get Inventory Results list for each warehouse
            var listInventoryStockStatus = ConvertToInventoryStockStatuses(product.BTKey, out last30Demand, out hasDemand);

            if (isHorizontalMode)
            {
                InventoryHelper.AddWareHouseIfNeed(listInventoryStockStatus, primaryWareHouse, secondWareHouse, product.ProductType);
            }

            //TFS 1624:Inventory Display for Paw Print Titles//
            InventoryStockStatus additionalWarehouse = null;
            if (!String.IsNullOrEmpty(product.PubCodeD))
            {
                if (product.PubCodeD.Contains(SearchFieldValue.PawPrintsPublisherName))
                {
                    additionalWarehouse = InventoryHelper.ShowInventoryIfPawPrints(listInventoryStockStatus, primaryWareHouse,
                                                                   secondWareHouse, product.SupplierCode, MarketType);
                }
            }

            // TFS 24744 Do not Display South Warehouse for AV Products
            var isAvProduct = CommonHelper.IsAVProduct(product.ProductType);
            if (isAvProduct && !displayInventoryForAllWareHouse &&
                primaryWareHouse == InventoryWareHouseCode.Com && string.IsNullOrEmpty(secondWareHouse))
            {
                listInventoryStockStatus = listInventoryStockStatus.Where(r => r.WareHouseCode == InventoryWareHouseCode.Som).ToList();
                isEastWarehouseSet = true;
            }
            else
            {
                listInventoryStockStatus = InventoryHelper.SortInventoryList(listInventoryStockStatus, primaryWareHouse, secondWareHouse,
                                                             additionalWarehouse, displayInventoryForAllWareHouse, MarketType);
            }

            return listInventoryStockStatus;
        }

        private void WriteWhsInfoToSessionForLoadFacetUsing(string primaryWareHouse, string secondWareHouse)
        {
            var userId = !string.IsNullOrEmpty(this.UserId) ? this.UserId : ServiceRequestContext.Current.UserId;
            var newCacheKey = string.Format(CacheKeyConstant.WhsInfoUserIdCacheKey, userId);
            var currentWhs = CachingController.Instance.Read(newCacheKey) as List<string> ?? new List<string>();

            var availableWhs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            availableWhs.Add("COM", "South");
            availableWhs.Add("MOM", "Central");
            availableWhs.Add("REN", "West");
            availableWhs.Add("RNO", "West");
            availableWhs.Add("SOM", "East");

            var needToUpdate = false;
            if (!string.IsNullOrEmpty(primaryWareHouse) && !currentWhs.Contains(primaryWareHouse))
            {
                needToUpdate = true;
                SettingAvailableWhs(primaryWareHouse, currentWhs, availableWhs);
            }

            if (!string.IsNullOrEmpty(secondWareHouse) && !currentWhs.Contains(secondWareHouse))
            {
                needToUpdate = true;
                SettingAvailableWhs(secondWareHouse, currentWhs, availableWhs);
            }

            if (needToUpdate)
            {
                CachingController.Instance.Write(newCacheKey, currentWhs);
            }
        }

        private static void SettingAvailableWhs(string whsCode, List<string> currentWhs, Dictionary<string, string> availableWhs)
        {
            currentWhs.Add(whsCode);
            if (availableWhs.ContainsKey(whsCode) && !currentWhs.Contains(availableWhs[whsCode]))
            {
                currentWhs.Add(availableWhs[whsCode]);
            }

            if (whsCode.ToUpper() == "SOM" && !currentWhs.Contains("Bridgewater"))
            {
                currentWhs.Add("Bridgewater");
            }
        }

        private List<InventoryStockStatus> ConvertToInventoryStockStatuses(string btKey, out int last30Demand, out bool hasDemand)
        {
            last30Demand = 0;
            hasDemand = false;
            var inventoryResult = GetInventoryResultByBTkey(btKey);
            var inventoryStockStatuses = new List<InventoryStockStatus>();

            if (inventoryResult != null && inventoryResult.Warehouses != null && inventoryResult.Warehouses.Any())
            {
                foreach (var wh in inventoryResult.Warehouses)
                {
                    var inventoryStockStatus = new InventoryStockStatus();
                    var wareHouse = wh.WarehouseId;
                    //these 2 fields seem not used in UI
                    //inventoryStockStatus.LineItem = inventoryResult.LineItem;
                    //inventoryStockStatus.StockCondition = inventoryResult.StockCondition;
                    inventoryStockStatus.InStockForRequest = wh.InStockForRequest;
                    inventoryStockStatus.QuantityAvailable = wh.InStockForRequest;
                    inventoryStockStatus.OnOrderQuantity = wh.OnOrderQuantity;
                    inventoryStockStatus.WareHouseCode = wareHouse;
                    inventoryStockStatus.WareHouse = WarehouseDesc.ContainsKey(wareHouse) ? WarehouseDesc[wareHouse] : wareHouse;
                    inventoryStockStatus.InvDemandNumber = wh.Last30DayDemand;
                    inventoryStockStatus.OnHandInventory = wh.InStockForRequest.ToString();

                    inventoryStockStatuses.Add(inventoryStockStatus);
                }
                last30Demand = inventoryResult.TotalLast30Demand;
                hasDemand = inventoryResult.HasDemand;
            }

            return inventoryStockStatuses;
        }

        public bool IsDisplayAllWarehouse()
        {
            var orgId = this.OrgId;
            if (string.IsNullOrEmpty(orgId))
            {
                var user = ProfileService.Instance.GetUserById(UserId);
                if (user == null)
                {
                    Logger.Write("Inventory", "User not found");
                    return false;
                }

                orgId = user.OrgId;
            }

            var organization = ProfileService.Instance.GetOrganizationById(orgId);

            if (organization != null && organization.AllWarehouse.HasValue && organization.AllWarehouse.Value)
            {
                return true;
            }
            return false;
        }

        private Dictionary<string, Account> GetDefaultAccounts(IEnumerable<SearchResultInventoryStatusArg> args)
        {
            var result = new Dictionary<string, Account>(StringComparer.OrdinalIgnoreCase);
            foreach (var searchArg in args)
            {
                var keyProductType = InventoryHelper.RefineProductTypeToMusicIfMovie(searchArg.ProductType);
                if (keyProductType == null || result.ContainsKey(keyProductType)) continue;

                var account = _homeDeliveryAccount ?? InventoryHelper.GetUserDefaultAccountFromCartDetail(searchArg, UserId, CartId);

                result.Add(keyProductType, account);
            }
            return result;
        }

        private InventoryDemandRequest PrepareInventoryRequest(IList<SearchResultInventoryStatusArg> args)
        {
            var request = new InventoryDemandRequest();
            if (args == null || args.Count == 0) return null;

            request.OnItemDetail = args.Count == 1 && args[0].ForSingleItem;

            //get default account
            _defaultAccountsDict = GetDefaultAccounts(args);

            // get user reserved type
            string userReservedType = DistributedCacheHelper.GetCurrentUserReservedType(this.UserId);
            bool isLeUserReservedType = (userReservedType == "le");

            string bookPrimaryWareHouse = string.Empty;
            string bookSecondWareHouse = string.Empty;
            string entPrimaryWareHouse = string.Empty;
            string entSecondWareHouse = string.Empty;
            var btkeys = new List<BTKeys>();
            var pagePosition = 1;
            foreach (var product in args)
            {
                if (string.IsNullOrEmpty(product.BTKey)) continue;//TFS 17944 - ELMAH

                var productType = InventoryHelper.RefineProductTypeToMusicIfMovie(product.ProductType);
                //var checkLeReserve = isLeUserReservedType ? TRUE : FALSE;
                var checkLeReserve = isLeUserReservedType && productType.ToLower() == "book" ? TRUE : FALSE;
                var accountInventoryType = String.Empty;
                var inventoryReserveNumber = String.Empty;
                //no account presented
                if (_defaultAccountsDict == null || _defaultAccountsDict.Count == 0)
                {
                    btkeys.Add(new BTKeys(pagePosition, product.BTKey, checkLeReserve, accountInventoryType, inventoryReserveNumber));
                    continue;
                }

                //there's at least an account
                if (!string.IsNullOrEmpty(productType) && _defaultAccountsDict.ContainsKey(productType))
                {
                    var account = _defaultAccountsDict[productType];
                    if (account != null)
                    {
                        if (account.CheckLEReserve != null && account.CheckLEReserve.Value && productType.ToLower() == "book") //if (account.CheckLEReserve != null && account.CheckLEReserve.Value)
                        {
                            checkLeReserve = TRUE;
                        }
                        if (account.AccountInventoryType != null)
                        {
                            accountInventoryType = account.AccountInventoryType;
                        }
                        if (account.InventoryReserveNumber != null)
                        {
                            inventoryReserveNumber = account.InventoryReserveNumber;
                        }

                        if (productType.ToLower() == "book")
                        {
                            //get primary/secondary warehouse code
                            GetPrimarySecondaryWarehouses(account, out bookPrimaryWareHouse, out bookSecondWareHouse);
                        }
                        else
                        {
                            GetPrimarySecondaryWarehouses(account, out entPrimaryWareHouse, out entSecondWareHouse);
                        }
                    }
                }
                btkeys.Add(new BTKeys(pagePosition, product.BTKey, checkLeReserve, accountInventoryType, inventoryReserveNumber));

                //increase page position by 1
                pagePosition++;
            }

            //BtKeys
            request.BTKeys = btkeys.ToArray();

            //Warehouses
            List<string> listWhsIds = WarehouseIDsList;
            request.Warehouses = listWhsIds.Select(wh => new WareHouses() { WarehouseID = wh }).ToArray();

            //VIPEnabled
            request.VIPEnabled = InventoryHelper.IsVIPEnabled(OrgId) ? TRUE : FALSE;

            //MarketType
            int marketType = MarketType.HasValue
                ? (int)MarketType.Value
                : (int)TS360Constants.MarketType.Any;

            request.MarketType = marketType.ToString();
            //Country Code
            request.CountryCode = CountryCode;

            //primary warehouse
            request.BookPrimaryWarehouseCode = bookPrimaryWareHouse;

            //secondary warehouse
            request.BookSecondaryWarehouseCode = bookSecondWareHouse;

            //primary warehouse
            request.EntertainmentPrimaryWarehouseCode = entPrimaryWareHouse;

            //secondary warehouse
            request.EntertainmentSecondaryWarehouseCode = entSecondWareHouse;

            return request;
        }

        private List<string> GetWarehouseIDs()
        {
            var warehouseIds = new List<string>();
            var warehousesList = Warehouses;
            foreach (var wareHouse in warehousesList)
            {
                if (!warehouseIds.Contains(wareHouse.Id) && wareHouse.Id != InventoryWareHouseCode.Ren)
                {
                    warehouseIds.Add(wareHouse.Id);
                }
            }
            return warehouseIds;
        }

        private static NoSqlServiceResult<InventoryDemandResponse> GetInventoryResults(InventoryDemandRequest data, Uri webApiUri)
        {
            try
            {
                // Create a WebClient to POST the request
                using (var client = new WebClient())
                {
                    // Set the header so it knows we are sending JSON
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";

                    var jss = new JavaScriptSerializer();
                    // Serialise the data we are sending in to JSON
                    string serialisedData = jss.Serialize(data);

                    // Make the request
                    var response = client.UploadString(webApiUri, serialisedData);

                    var logMessge = string.Format("Calling WebApi at: {0}{3}.InventoryDemandRequest: {1}{3}.InventoryDemandResponse: {2}",
                                                    webApiUri, serialisedData, response, Environment.NewLine);

                    PricingLogger.LogDebug(LogCategory, logMessge);
                    // Deserialise the response into a GUID
                    return jss.Deserialize<NoSqlServiceResult<InventoryDemandResponse>>(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        private void GetInventoryResultsDict(IList<SearchResultInventoryStatusArg> args)
        {
            //if the set of args already fetched inventory data, just return
            if (!string.IsNullOrEmpty(args[0].BTKey) //TFS 17944 - ELMAH
                && _inventoryResultsDict != null && _inventoryResultsDict.ContainsKey(args[0].BTKey))
                return;

            var inventoryRequest = PrepareInventoryRequest(args);

            var response = GetInventoryResults(inventoryRequest, InventoryDemandUrl);

            if (response != null)
            {
                if (response.Status == NoSqlServiceStatus.Success)
                {
                    if (response.Data != null && response.Data.InventoryResults != null &&
                        response.Data.InventoryResults.Any())
                    {
                        _inventoryResultsDict = new Dictionary<string, BTKeyInventoryResult>();
                        _inventoryStatusList = new List<SiteTermObject>();
                        var notAvailableSaleInYourCountryText = SearchResources.NotAvailableSaleInYourCountry;// SiteContext.GetLocalizedString(ResourceName.SearchResources, "NotAvailableSaleInYourCountry");
                        var userCountryCode = CountryCode;

                        foreach (var res in response.Data.InventoryResults)
                        {
                            if (!string.IsNullOrEmpty(res.BTKey) && !_inventoryResultsDict.ContainsKey(res.BTKey))
                            {
                                // TFS19551: if blocked-export CountryCodes contain user's country code, show new inventory status
                                var isExportBlocked = args.Any(r => r.BTKey == res.BTKey &&
                                                               r.BlockedExportCountryCodes != null &&
                                                               r.BlockedExportCountryCodes.Contains(userCountryCode, StringComparer.CurrentCultureIgnoreCase));
                                if (isExportBlocked)
                                    res.InventoryStatus = notAvailableSaleInYourCountryText;

                                _inventoryResultsDict.Add(res.BTKey, res);

                                var itemData = new SiteTermObject(res.BTKey, res.InventoryStatus);
                                _inventoryStatusList.Add(itemData);
                            }
                        }
                    }
                }
                else
                {
                    Logger.Write(LogCategory,
                        string.Format(
                            "MongoDb WebAPI Call For InventoryDemand {0}, Error Code: {1}, Error Message {2}",
                            response.Status,
                            response.ErrorCode, response.ErrorMessage));
                }
            }
        }

        private Account GetAccountByProductType(string productType)
        {
            if (string.IsNullOrEmpty(productType)) return null;

            if (_defaultAccountsDict != null && _defaultAccountsDict.ContainsKey(productType))
            {
                return _defaultAccountsDict[productType];
            }
            return null;
        }

        ////private AccountDaoObject GetAccountDaoByProductType(string productType)
        ////{
        ////    if (string.IsNullOrEmpty(productType)) return null;

        ////    if (_defaultAccountsDaoDict != null && _defaultAccountsDaoDict.ContainsKey(productType))
        ////    {
        ////        return _defaultAccountsDaoDict[productType];
        ////    }
        ////    return null;
        ////}

        private BTKeyInventoryResult GetInventoryResultByBTkey(string btKey)
        {
            if (_inventoryResultsDict != null && !string.IsNullOrEmpty(btKey) && _inventoryResultsDict.ContainsKey(btKey))
            {
                return _inventoryResultsDict[btKey];
            }
            return null;
        }

        private void GetPrimarySecondaryWarehouses(Account account, out string primaryWareHouse, out string secondWareHouse)
        {
            //Get primary and secondary ware house
            primaryWareHouse = String.Empty;
            secondWareHouse = String.Empty;

            if (account != null && !string.IsNullOrEmpty(account.PrimaryWarehouseCode))
            {
                primaryWareHouse = account.PrimaryWarehouseCode;//((Warehouse)account.PrimaryWarehouse.Target).Code;
            }
            if (account != null && !string.IsNullOrEmpty(account.SecondaryWarehouseCode))
            {
                secondWareHouse = account.SecondaryWarehouseCode;//((Warehouse)account.SecondaryWarehouse.Target).Code;
            }
        }

        //private void GetPrimarySecondaryWarehouses4DaoObject(AccountDaoObject account, out string primaryWareHouse, out string secondWareHouse)
        //{
        //    //Get primary and secondary ware house
        //    primaryWareHouse = String.Empty;
        //    secondWareHouse = String.Empty;

        //    if (account != null && account.PrimaryWarehouse != null)
        //    {
        //        primaryWareHouse = account.PrimaryWarehouse.Code;
        //    }
        //    if (account != null && account.SecondaryWarehouse != null)
        //    {
        //        secondWareHouse = account.SecondaryWarehouse.Code;
        //    }
        //}

        private static NoSqlServiceResult<DemandHistoryResponse> GetDemandHistory(DemandHistoryRequest data, Uri webApiUri)
        {
            try
            {
                // Create a WebClient to POST the request
                using (WebClient client = new WebClient())
                {
                    PricingLogger.LogDebug(LogCategory, string.Format("Calling WebApi at: {0}", webApiUri));
                    // Set the header so it knows we are sending JSON
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var jss = new JavaScriptSerializer();
                    // Serialise the data we are sending in to JSON
                    string serialisedData = jss.Serialize(data);

                    PricingLogger.LogDebug(LogCategory, string.Format("DemandHistoryRequest: {0}", serialisedData));
                    // Make the request
                    var response = client.UploadString(webApiUri, serialisedData);
                    PricingLogger.LogDebug(LogCategory, string.Format("DemandHistoryResponse: {0}", response));

                    // Deserialise the response into a GUID
                    return jss.Deserialize<NoSqlServiceResult<DemandHistoryResponse>>(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }


        //private static NoSqlServiceResult<InventoryFacetResponse> GetInventoryFacetResults(InventoryFacetRequest data,
        //   Uri webApiUri)
        //{
        //    try
        //    {
        //        // Create a WebClient to POST the request
        //        using (WebClient client = new WebClient())
        //        {
        //            Logger.Write(LogCategory, string.Format("Calling WebApi at: {0}", webApiUri), false);
        //            // Set the header so it knows we are sending JSON
        //            client.Headers[HttpRequestHeader.ContentType] = "application/json";
        //            var jss = new JavaScriptSerializer();
        //            // Serialise the data we are sending in to JSON
        //            string serialisedData = jss.Serialize(data);

        //            Logger.Write(LogCategory, string.Format("InventoryFacetRequest: {0}", serialisedData), false);
        //            // Make the request
        //            var response = client.UploadString(webApiUri, serialisedData);
        //            Logger.Write(LogCategory, string.Format("InventoryFacetResponse: {0}", response), false);

        //            // Deserialise the response into a GUID
        //            return jss.Deserialize<NoSqlServiceResult<InventoryFacetResponse>>(response);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //    }
        //    return null;
        //}

        //private InventoryFacetRequest PrepareInventoryFacetRequest(IList<SearchResultInventoryStatusArg> args, string facetPath)
        //{
        //    var request = new InventoryFacetRequest() { FacetPath = facetPath };
        //    if (args == null || args.Count == 0) return null;
        //    //get default account
        //    _defaultAccountsDict = GetDefaultAccounts(args);

        //    var btkeys = new List<BTKeys>();
        //    var pagePosition = 1;
        //    foreach (var product in args)
        //    {
        //        var productType = InventoryHelper.RefineProductTypeToMusicIfMovie(product.ProductType);
        //        var checkLeReserve = FALSE;
        //        var accountInventoryType = String.Empty;
        //        var inventoryReserveNumber = String.Empty;
        //        //no account presented
        //        if (_defaultAccountsDict == null || _defaultAccountsDict.Count == 0)
        //        {
        //            btkeys.Add(new BTKeys(pagePosition, product.BTKey, checkLeReserve, accountInventoryType, inventoryReserveNumber));
        //            continue;
        //        }

        //        //there's at least an account
        //        if (!string.IsNullOrEmpty(productType) && _defaultAccountsDict.ContainsKey(productType))
        //        {
        //            var account = _defaultAccountsDict[productType];
        //            if (account != null)
        //            {
        //                if (account.CheckLEReserve != null && account.CheckLEReserve.Value)
        //                {
        //                    checkLeReserve = TRUE;
        //                }
        //                if (account.AccountInventoryType != null)
        //                {
        //                    accountInventoryType = account.AccountInventoryType;
        //                }
        //                if (account.InventoryReserveNumber != null)
        //                {
        //                    inventoryReserveNumber = account.InventoryReserveNumber;
        //                }
        //            }
        //        }
        //        btkeys.Add(new BTKeys(pagePosition, product.BTKey, checkLeReserve, accountInventoryType, inventoryReserveNumber));

        //        //increase page position by 1
        //        pagePosition++;
        //    }

        //    //BtKeys
        //    request.BTKeys = btkeys.ToArray();

        //    //VIPEnabled
        //    request.VIPEnabled = InventoryHelper.IsVIPEnabled() ? TRUE : FALSE;

        //    //MarketType
        //    int marketType = MarketType.HasValue
        //        ? (int)MarketType.Value
        //        : (int)BTNextGen.Commerce.Portal.Common.Constants.MarketType.Any;

        //    request.MarketType = marketType.ToString();
        //    //Country Code
        //    request.CountryCode = SiteContext.Current.CountryCode;

        //    if (_defaultAccountsDict != null)
        //    {
        //        if (_defaultAccountsDict.ContainsKey(ProductType.Book.ToString()))
        //        {
        //            string bookprimaryWareHouse, booksecondWareHouse;
        //            GetPrimarySecondaryWarehouses(_defaultAccountsDict[ProductType.Book.ToString()],
        //                out bookprimaryWareHouse, out booksecondWareHouse);
        //            request.BookPrimaryWareHouseCode = bookprimaryWareHouse;
        //            request.BookSecondaryWareHouseCode = booksecondWareHouse;
        //        }
        //        if (_defaultAccountsDict.ContainsKey(ProductType.Music.ToString()))
        //        {
        //            string entprimaryWareHouse, entsecondWareHouse;
        //            GetPrimarySecondaryWarehouses(_defaultAccountsDict[ProductType.Music.ToString()],
        //                out entprimaryWareHouse, out entsecondWareHouse);
        //            request.EntertainmentPrimaryWareHouseCode = entprimaryWareHouse;
        //            request.EntertainmentSecondaryWareHouseCode = entsecondWareHouse;
        //        }
        //    }
        //    return request;
        //}

        #endregion

        public NoSqlServiceResult<InventoryDemandResponse> GetInventoryDemandForMarc(DataTable dtTableMarcBTKeyResults, string marketType)
        {
            //build the request to mongo
            InventoryDemandRequest inventorydemandrequest = new InventoryDemandRequest();
            inventorydemandrequest = BuildMongoRequestForMarc(dtTableMarcBTKeyResults, marketType);
            //call mongo inventory service
            var response = GetInventoryResults(inventorydemandrequest, InventoryDemandUrl);
            return response;
        }

        private InventoryDemandRequest BuildMongoRequestForMarc(DataTable dtBTKEYRecord, string marketType)
        {
            try
            {
                string leindicator = string.Empty;

                int UpperBound = dtBTKEYRecord.Rows.Count;
                InventoryDemandRequest inventorydemandrequest = new InventoryDemandRequest();
                List<BTKeys> btKeyList = new List<BTKeys>();

                // Loop through tags
                //
                for (int Loop = 0; Loop < UpperBound; Loop++)
                {
                    // Array information
                    //
                    DataRow TagInfo = dtBTKEYRecord.Rows[Loop];

                    //COUNT 
                    //int totalColumns = dtBTKEYRecord.Columns.Count;
                    //WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "#x1 count: " + totalColumns.ToString(), Config.LogDetails);

                    // BTKey Data
                    //
                    string tempBTKey;
                    tempBTKey = TagInfo["BTKEY"].ToString();

                    // ProductType Data 
                    string tempProductID;
                    tempProductID = TagInfo["ProductTypeID"].ToString();


                    if (marketType == "1" && tempProductID == "4")
                    { leindicator = "1"; }
                    else
                    { leindicator = "0"; }


                    //List<BTKeys> btKeyList = new List<BTKeys>();
                    btKeyList.Add(new BTKeys { BTKey = tempBTKey, LEIndicator = leindicator });

                }
                inventorydemandrequest.MarketType = marketType;
                inventorydemandrequest.OnItemDetail = false;
                inventorydemandrequest.VIPEnabled = "1";
                inventorydemandrequest.CountryCode = "US";
                List<WareHouses> warehouseList = new List<WareHouses>();
                warehouseList.Add(new WareHouses { WarehouseID = "COM" });
                warehouseList.Add(new WareHouses { WarehouseID = "SOM" });
                warehouseList.Add(new WareHouses { WarehouseID = "MOM" });
                warehouseList.Add(new WareHouses { WarehouseID = "RNO" });
                warehouseList.Add(new WareHouses { WarehouseID = "VIE" });
                warehouseList.Add(new WareHouses { WarehouseID = "VIM" });
                inventorydemandrequest.BTKeys = btKeyList.ToArray();
                inventorydemandrequest.Warehouses = warehouseList.ToArray();

                return inventorydemandrequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
