using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BT.TS360API.Common.Controller;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Inventory;
using BT.TS360API.Common.Search;
using BT.TS360API.ExternalServices;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360Constants;
using AppSettings = BT.TS360API.Common.Configrations.AppSettings;
using BT.TS360API.ExternalServices.BTStockCheckServices;
using BTStockServiceLineItem = BT.TS360API.ExternalServices.BTStockCheckServices.LineItem;

namespace BT.TS360API.Common.Helpers
{
    public class InventoryHelper
    {
        #region Move wcf to api 
        
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

        #endregion 


        private static readonly string[] DefaultWhList = new string[] { InventoryWareHouseCode.Com, 
                                                InventoryWareHouseCode.Mom, 
                                                InventoryWareHouseCode.Ren, 
                                                InventoryWareHouseCode.Som };

        //public static List<InventoryStockStatus> GetInventoryResultsForSearchPage(ProductSearchResultItem product, string userId,
        //    MarketType? market, bool isHorizontalMode, out bool displayInventoryForAllWareHouse,
        //    Account account, List<Warehouse> listWarehouse, List<DataRow> dataRows)
        //{
        //    try
        //    {
        //        displayInventoryForAllWareHouse = false;

        //        var productType = product.ProductType;
        //        var reportCode = product.ReportCode;
        //        var publishDate = product.PublishDate;
        //        var eSupplier = product.ESupplier;
        //        var supplierCode = product.SupplierCode;
        //        var publisherCode = product.Publisher;

        //        var determineInventory = new DetermineInventory();
        //        var listInventoryStockStatus = determineInventory.GetOnhandInventoryForSearchResultPage(account, listWarehouse,
        //            dataRows, productType, reportCode, publishDate, eSupplier, reportCode);

        //        //Get primary and secondary ware house
        //        string primaryWareHouse = String.Empty;
        //        string secondWareHouse = String.Empty;

        //        if (account != null && account.PrimaryWarehouse != null)
        //        {
        //            primaryWareHouse = account.PrimaryWarehouse.Code; //((Warehouse)account.PrimaryWarehouse.Target).Code;
        //        }
        //        if (account != null && account.SecondaryWarehouse != null)
        //        {
        //            secondWareHouse = account.SecondaryWarehouse.Code; //((Warehouse)account.SecondaryWarehouse.Target).Code;
        //        }
        //        if (isHorizontalMode)
        //        {
        //            AddWareHouseIfNeed(listInventoryStockStatus, primaryWareHouse, secondWareHouse);
        //        }

        //        //TFS 1624:Inventory Display for Paw Print Titles//
        //        InventoryStockStatus additionalWarehouse = null;
        //        if (!String.IsNullOrEmpty(publisherCode))
        //        {
        //            if (publisherCode.Contains(SearchFieldValue.PawPrintsPublisherName))
        //            {
        //                additionalWarehouse = ShowInventoryIfPawPrints(listInventoryStockStatus, primaryWareHouse,
        //                                                               secondWareHouse, supplierCode, market);
        //            }
        //        }

        //        listInventoryStockStatus = SortInventoryList(listInventoryStockStatus, primaryWareHouse, secondWareHouse,
        //                                                     additionalWarehouse, userId, account,
        //                                                     out displayInventoryForAllWareHouse);

        //        return listInventoryStockStatus;
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.RaiseException(e, ExceptionCategory.Search);
        //        throw;
        //    }
        //}

        public static Account GetUserDefaultAccountFromCartDetail(SearchResultInventoryStatusArg searchArg, string userId, string cartId)
        {
            //var cacheProductType = searchArg.ProductType;
            //if (string.Compare(cacheProductType, ProductType.Movie.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
            //{
            //    cacheProductType = ProductType.Music.ToString();
            //}

            //var cacheKey = String.Format("UserDefaultAcct_{0}_{1}_{2}_{3}", cacheProductType, searchArg.ESupplier,
            //                             userId, cartId);

            //var account = CachingController.Instance.Read(cacheKey) as Account;// VelocityCacheManager.Read(cacheKey) as Account;

            //if (account == null)
            {
                var productType = searchArg.ProductType;
                var eSupplier = searchArg.ESupplier;
                //var defaultESupplierAccountIds = VelocityCacheManager.Read("InventoryBinding_DefaultESuppliersAccountIds")
                //    as Dictionary<string, string>;
                //var bookAccountId = VelocityCacheManager.Read("InventoryBinding_DefaultBookAccountId") as string;
                //var entAccountId = VelocityCacheManager.Read("InventoryBinding_DefaultEntAccountId") as string;

                //if (bookAccountId == null || entAccountId == null || defaultESupplierAccountIds == null)
                //{
                //    CartAccountHelper.GetDefaulAccountId(eSupplier, cartId, userId, out entAccountId, out bookAccountId,
                //                                     out defaultESupplierAccountIds, out vipAccountId);
                //}

                var defaultESupplierAccountIds = new Dictionary<string, string>();
                string bookAccountId = "", entAccountId = "";
                string vipAccountId = string.Empty;

                var cartAccountObj = CommonHelper.GetDefaulAccountId(eSupplier, cartId, userId, entAccountId, bookAccountId,
                                                     defaultESupplierAccountIds, vipAccountId);

                if (cartAccountObj != null)
                {
                    entAccountId = cartAccountObj.EntAccountId;
                    bookAccountId = cartAccountObj.BookAccountId;
                    defaultESupplierAccountIds = cartAccountObj.DefaultESupplierAccountIds;
                    vipAccountId = cartAccountObj.VIPAccountId;
                }

                bool isVIPAccount;
                string accountId = ProductSearchController.SelectTheAccount(productType, eSupplier, entAccountId,
                                                                            bookAccountId, defaultESupplierAccountIds, vipAccountId, out isVIPAccount, userId);

                if (!string.IsNullOrEmpty(accountId))
                {
                    //var administrationProfileController = AdministrationProfileController.Current;
                    //administrationProfileController.AccountRelated.PrimaryWarehouseNeeded = true;
                    //administrationProfileController.AccountRelated.SecondaryWarehouseNeeded = true;
                    //account = administrationProfileController.GetAccountById(accountId);
                    //VelocityCacheManager.Write(cacheKey, account, VelocityCacheLevel.Request);
                    //return account;
                    return ProfileController.Instance.GetAccountById(accountId, true, true);
                }

                return ProductSearchController.GetUserDefaultAccount(searchArg, userId);
                //var account = ProductSearchController.GetUserDefaultAccount(searchArg, userId);
                //VelocityCacheManager.Write(cacheKey, account, VelocityCacheLevel.Request);
                //return account;
            }
        }

        ////public static AccountDaoObject GetUserDefaultAccountDaoFromCartDetail(SearchResultInventoryStatusArg searchArg, string userId, string cartId)
        ////{
        ////    var cacheProductType = searchArg.ProductType;
        ////    if (string.Compare(cacheProductType, ProductType.Movie.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
        ////    {
        ////        cacheProductType = ProductType.Music.ToString();
        ////    }

        ////    var cacheKey = String.Format("UserDefaultAcctDao_{0}_{1}_{2}_{3}", cacheProductType, searchArg.ESupplier,
        ////                                 userId, cartId);

        ////    var account = VelocityCacheManager.Read(cacheKey) as AccountDaoObject;

        ////    if (account == null)
        ////    {
        ////        var productType = searchArg.ProductType;
        ////        var eSupplier = searchArg.ESupplier;
        ////        var defaultESupplierAccountIds = VelocityCacheManager.Read("InventoryBinding_DefaultESuppliersAccountDaoIds")
        ////            as Dictionary<string, string>;
        ////        string bookAccountId = "";
        ////        var entAccountId = "";
        ////        string vipAccountId = string.Empty;
        ////        if (bookAccountId == null || entAccountId == null || defaultESupplierAccountIds == null)
        ////        {
        ////            CartAccountHelper.GetDefaulAccountId(eSupplier, cartId, userId, out entAccountId, out bookAccountId,
        ////                                             out defaultESupplierAccountIds, out vipAccountId);
        ////        }

        ////        bool isVIPAccount;
        ////        string accountId = ProductSearchController.SelectTheAccount(productType, eSupplier, entAccountId,
        ////                                                                    bookAccountId, defaultESupplierAccountIds, vipAccountId, out isVIPAccount);

        ////        if (!String.IsNullOrEmpty(accountId))
        ////        {
        ////            account = ProfileController.Current.GetAccountDaoByIdForAdditionalInfo(accountId);
        ////            VelocityCacheManager.Write(cacheKey, account, VelocityCacheLevel.Request);
        ////            return account;
        ////        }

        ////        account = ProductSearchController.GetUserDefaultAccountDaoObject(searchArg, userId);
        ////        VelocityCacheManager.Write(cacheKey, account, VelocityCacheLevel.Request);
        ////        return account;
        ////    }

        ////    return account;
        ////}

        public static InventoryStockStatus ShowInventoryIfPawPrints(IEnumerable<InventoryStockStatus> listInventoryStockStatus,
            string primaryWareHouse, string secondWareHouse, string supplierCode, MarketType? marketType)
        {
            InventoryStockStatus additionalWarehouse = null;
            var pubCode = String.Empty;

            if (marketType == MarketType.Retail)
            {
                foreach (InventoryStockStatus iss in listInventoryStockStatus)
                    iss.OnHandInventory = GeneralConstants.DefaultQuantity;
            }
            else
            {
                if (!String.IsNullOrEmpty(supplierCode))
                {
                    pubCode = supplierCode.Trim();
                    if (pubCode.Equals(SearchFieldValue.SupplierCodePPBTB, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //PPBTB - add the inventory from the Bridgewater warehouse to the customer's primary warehouse
                        additionalWarehouse = RecalculateInventoryIfPawnPrints(listInventoryStockStatus, primaryWareHouse,
                                                                               secondWareHouse, InventoryWareHouseCode.Som);
                    }

                    else if (pubCode.Equals(SearchFieldValue.SupplierCodePPBTM, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //PPBTM - add the inventory from the Momence warehouse to the customer's primary warehouse.
                        additionalWarehouse = RecalculateInventoryIfPawnPrints(listInventoryStockStatus, primaryWareHouse,
                                                                               secondWareHouse, InventoryWareHouseCode.Mom);
                    }
                    else if (pubCode.Equals(SearchFieldValue.SupplierCodePPBTC, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //PPBTC - add the inventory from the Com warehouse to the customer's primary warehouse.
                        additionalWarehouse = RecalculateInventoryIfPawnPrints(listInventoryStockStatus, primaryWareHouse,
                                                                               secondWareHouse, InventoryWareHouseCode.Com);
                    }
                    else if (pubCode.Equals(SearchFieldValue.SupplierCodePPBTR, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //PPBTR - add the inventory from the Ren warehouse to the customer's primary warehouse.
                        additionalWarehouse = RecalculateInventoryIfPawnPrints(listInventoryStockStatus, primaryWareHouse,
                                                                               secondWareHouse, InventoryWareHouseCode.Ren);
                    }
                }
            }
            return additionalWarehouse;
        }

        private static InventoryStockStatus RecalculateInventoryIfPawnPrints(IEnumerable<InventoryStockStatus> listInventoryStockStatus,
            string primaryWareHouse, string secondWareHouse, string warehouseCode)
        {
            InventoryStockStatus additionalWarehouse = null;
            if (IsEqual(warehouseCode, primaryWareHouse) || IsEqual(warehouseCode, secondWareHouse))
                return null;
            InventoryStockStatus primaryInvStockStatus = null;
            InventoryStockStatus invStockStatus = null;
            foreach (InventoryStockStatus st in listInventoryStockStatus)
                if (IsEqual(st.WareHouseCode, primaryWareHouse))
                    primaryInvStockStatus = st;
                else if (IsEqual(st.WareHouseCode, warehouseCode))
                    invStockStatus = st;

            if (invStockStatus != null && !String.IsNullOrEmpty(invStockStatus.OnHandInventory))
            {
                int iquantityAdditional = 0;
                Int32.TryParse(invStockStatus.OnHandInventory, out iquantityAdditional);
                if (iquantityAdditional != 0)
                {
                    if (primaryInvStockStatus == null)
                        return null;
                    int quantity = 0;
                    Int32.TryParse(primaryInvStockStatus.OnHandInventory, out quantity);
                    quantity += iquantityAdditional;
                    additionalWarehouse = invStockStatus;
                    invStockStatus.OnHandInventory = GeneralConstants.DefaultQuantity;
                    primaryInvStockStatus.OnHandInventory = quantity.ToString();
                }
            }
            return additionalWarehouse;
        }

        private static bool CheckIfWareHouseCodeIsExisted(IEnumerable<InventoryStockStatus> listInventoryStockStatus, string wareHouseCode)
        {
            return listInventoryStockStatus.Any(inventoryStockStatus => IsEqual(inventoryStockStatus.WareHouseCode, wareHouseCode));
        }

        public static void AddWareHouseIfNeed(List<InventoryStockStatus> listInventoryStockStatus, string primaryWareHouse, string secondWareHouse, string productType)
        {
            if (listInventoryStockStatus != null)
            {
                bool isAVProduct = CommonHelper.IsAVProduct(productType);
                // ignore SOUTH (COM) if AV product
                if (!String.IsNullOrEmpty(primaryWareHouse) && !(isAVProduct && primaryWareHouse == InventoryWareHouseCode.Com) &&
                    !CheckIfWareHouseCodeIsExisted(listInventoryStockStatus, primaryWareHouse))
                {
                    AddPrimaryWareHouse(listInventoryStockStatus, primaryWareHouse);
                }
                if (!String.IsNullOrEmpty(secondWareHouse) && !(isAVProduct && secondWareHouse == InventoryWareHouseCode.Com) &&
                    !CheckIfWareHouseCodeIsExisted(listInventoryStockStatus, secondWareHouse))
                {
                    AddSecondaryWareHouse(listInventoryStockStatus, secondWareHouse);
                }
            }
        }

        private static void AddSecondaryWareHouse(List<InventoryStockStatus> listInventoryStockStatus, string secondWareHouse)
        {
            //var secondaryWareHouseName = secondWareHouse + GeneralConstants.SecondaryMark;

            var inventoryHelper = InventoryHelper4MongoDb.GetInstance();
            var secondaryWareHouseName = inventoryHelper.WarehouseDesc.ContainsKey(secondWareHouse) ? inventoryHelper.WarehouseDesc[secondWareHouse] : secondWareHouse
                + GeneralConstants.SecondaryMark;

            var secondaryInventoryStockStatus = new InventoryStockStatus
            {
                WareHouseCode = secondWareHouse,
                WareHouse = secondaryWareHouseName
            };
            listInventoryStockStatus.Add(secondaryInventoryStockStatus);
        }

        private static void AddPrimaryWareHouse(List<InventoryStockStatus> listInventoryStockStatus, string primaryWareHouse)
        {
            var inventoryHelper = InventoryHelper4MongoDb.GetInstance();
            var primaryWareHouseName = inventoryHelper.WarehouseDesc.ContainsKey(primaryWareHouse) ? inventoryHelper.WarehouseDesc[primaryWareHouse] : primaryWareHouse
                + GeneralConstants.PrimaryMark;

            //var primaryWareHouseName = primaryWareHouse + GeneralConstants.PrimaryMark;
            var primaryInventoryStockStatus = new InventoryStockStatus
            {
                WareHouseCode = primaryWareHouse,
                WareHouse = primaryWareHouseName
            };
            listInventoryStockStatus.Add(primaryInventoryStockStatus);
        }

        public static List<InventoryStockStatus> SortInventoryList(List<InventoryStockStatus> listInventoryStockStatus,
                                                                    string primaryWareHouse,
                                                                    string secondWareHouse,
                                                                    InventoryStockStatus additionalWarehouse,
                                                                    string userId,
                                                                    Account account,
                                                                    out bool displayInventoryForAllWareHouse,
            MarketType? siteContextMarketType)
        {
            displayInventoryForAllWareHouse = IsDisplayAllWarehouse(account, userId);
            if (listInventoryStockStatus != null && listInventoryStockStatus.Count > 0)
            {
                var sortedList = new List<InventoryStockStatus>();
                var remainList = new List<InventoryStockStatus>();
                var vipList = new List<InventoryStockStatus>();

                //sort and mark primary/secondary
                foreach (InventoryStockStatus st in listInventoryStockStatus)
                {
                    st.FormalWareHouseCode = st.WareHouseCode = ChangeRno2RenIfAny(st.WareHouseCode);
                    st.WareHouse = ChangeRno2RenIfAny(st.WareHouse);
                    if (IsEqual(st.WareHouseCode, primaryWareHouse))
                    {
                        st.WareHouse += GeneralConstants.PrimaryMark;
                        st.WareHouseCode += GeneralConstants.PrimaryMark;
                        sortedList.Insert(0, st);
                    }
                    else if (IsEqual(st.WareHouseCode, secondWareHouse))
                    {
                        st.WareHouse += GeneralConstants.SecondaryMark;
                        st.WareHouseCode += GeneralConstants.SecondaryMark;
                        sortedList.Add(st);
                    }
                    else if (IsVIPWarehouse(st.WareHouseCode))
                    {
                        st.WareHouse += GeneralConstants.VipMark;
                        st.WareHouseCode += GeneralConstants.VipMark;
                        vipList.Add(st);
                    }
                    else
                        remainList.Add(st);
                }

                if (displayInventoryForAllWareHouse)
                {
                    sortedList.AddRange(remainList);
                }

                if (vipList.Count > 0)
                {
                    var marketType = siteContextMarketType;
                    if (marketType != null && (marketType == MarketType.AcademicLibrary
                    || marketType == MarketType.PublicLibrary
                    || marketType == MarketType.SchoolLibrary))
                    {
                        sortedList.InsertRange(0, vipList);
                    }
                    else
                        sortedList.AddRange(vipList);
                }

                return sortedList;
            }
            return null;
        }

        public static List<InventoryStockStatus> SortInventoryList(List<InventoryStockStatus> listInventoryStockStatus,
                                                                    string primaryWareHouse,
                                                                    string secondWareHouse,
                                                                    InventoryStockStatus additionalWarehouse,
                                                                    bool displayInventoryForAllWareHouse, MarketType? siteContextMarketType)
        {
            if (listInventoryStockStatus != null && listInventoryStockStatus.Count > 0)
            {
                var sortedList = new List<InventoryStockStatus>();
                var remainList = new List<InventoryStockStatus>();
                var vipList = new List<InventoryStockStatus>();

                //sort and mark primary/secondary
                foreach (InventoryStockStatus st in listInventoryStockStatus)
                {
                    st.FormalWareHouseCode = st.WareHouseCode = ChangeRno2RenIfAny(st.WareHouseCode);
                    st.WareHouse = ChangeRno2RenIfAny(st.WareHouse);
                    if (IsEqual(st.WareHouseCode, primaryWareHouse))
                    {
                        st.WareHouse += GeneralConstants.PrimaryMark;
                        st.WareHouseCode += GeneralConstants.PrimaryMark;
                        sortedList.Insert(0, st);
                    }
                    else if (IsEqual(st.WareHouseCode, secondWareHouse))
                    {
                        st.WareHouse += GeneralConstants.SecondaryMark;
                        st.WareHouseCode += GeneralConstants.SecondaryMark;
                        sortedList.Add(st);
                    }
                    else if (IsVIPWarehouse(st.WareHouseCode))
                    {
                        st.WareHouse += GeneralConstants.VipMark;
                        st.WareHouseCode += GeneralConstants.VipMark;
                        vipList.Add(st);
                    }
                    else
                        remainList.Add(st);
                }

                if (displayInventoryForAllWareHouse)
                {
                    sortedList.AddRange(remainList);
                }

                if (vipList.Count > 0)
                {
                    var marketType = siteContextMarketType;
                    if (marketType != null && (marketType == MarketType.AcademicLibrary
                    || marketType == MarketType.PublicLibrary
                    || marketType == MarketType.SchoolLibrary))
                    {
                        sortedList.InsertRange(0, vipList);
                    }
                    else
                        sortedList.AddRange(vipList);
                }

                return sortedList;
            }
            return null;
        }

        public static bool IsDisplayAllWarehouse(Account account, string userId)
        {
            //var profileController = ProfileController.Current;
            //profileController.UserProfilePropertiesToReturn.Add(UserProfile.PropertyName.OrganizationId);
            //var user = profileController.GetUserById(userId);

            var user = ProfileService.Instance.GetUserById(userId);// CSObjectProxy.GetUserProfileForSearchResult();

            if (user == null)
            {
                Logger.Write("Inventory", "User not found");
                return false;
            }

            var organizationId = user.OrgId;// user.Organization.Target.Id;

            //var profileControllerForAdmin = AdministrationProfileController.Current;
            //profileControllerForAdmin.OrganizationPropertiesToReturn.Add(Organization.PropertyName.AllWarehouse);
            //var organization = profileControllerForAdmin.GetOrganization(user.OrganizationId);
            var organization = ProfileService.Instance.GetOrganizationById(organizationId);// profileControllerForAdmin.GetOrganization(organizationId);

            if ((account == null) || (organization != null && organization.AllWarehouse.HasValue && organization.AllWarehouse.Value))
            {
                return true;
            }
            return false;
        }

        public static List<InventoryStockStatus> CorrectAndFilterInventory(List<InventoryStockStatus> listInventoryStockStatus,
            MarketType? siteContextMarketType)
        {
            //try
            //{
                var refinedList = new List<InventoryStockStatus>();
                var vipList = new List<InventoryStockStatus>();

                if (listInventoryStockStatus != null && listInventoryStockStatus.Count > 0)
                {
                    foreach (InventoryStockStatus st in listInventoryStockStatus)
                    {
                        if (InventoryHelper.IsVIPWarehouse(st.FormalWareHouseCode))
                        {
                            vipList.Add(st);
                        }
                        else if (ValidateInventoryStockStatus(st))
                        {
                            st.OnHandInventory = CorrectQuantity(st.OnHandInventory);
                            refinedList.Add(st);
                        }
                    }
                }
                if (refinedList.Count == 0)
                {
                    AddDefaultFourItems(refinedList);
                }
                if (refinedList.Count == 1)
                {
                    AddMoreThreeItems(refinedList);
                }
                else if (refinedList.Count == 2)
                {
                    AddMoreTwoItems(refinedList);
                }
                else if (refinedList.Count == 3)
                {
                    AddMoreOneItem(refinedList);
                }
                
                // remove no name warehouse
                refinedList.RemoveAll(r => string.IsNullOrEmpty(r.WareHouse));

                if (vipList.Count > 0)
                {
                    if (siteContextMarketType != null && (siteContextMarketType == MarketType.AcademicLibrary
                        || siteContextMarketType == MarketType.PublicLibrary
                        || siteContextMarketType == MarketType.SchoolLibrary))
                    {
                        refinedList.InsertRange(0, vipList);
                    }
                    else
                        refinedList.AddRange(vipList);
                }

                return refinedList;
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        private static void AddDefaultFourItems(List<InventoryStockStatus> listInventoryStockStatus)
        {
            listInventoryStockStatus.AddRange(
                       from s in DefaultWhList
                       select new InventoryStockStatus()
                       {
                           WareHouseCode = s,
                           OnHandInventory = GeneralConstants.DefaultQuantity
                       });
        }

        private static void AddMoreThreeItems(List<InventoryStockStatus> listInventoryStockStatus)
        {
            var inventoryHelper = InventoryHelper4MongoDb.GetInstance();

            var warehouseFirstItem = listInventoryStockStatus[0].FormalWareHouseCode;
            listInventoryStockStatus.AddRange(
                from s in DefaultWhList
                where String.Compare(s, warehouseFirstItem, StringComparison.OrdinalIgnoreCase) != 0
                select new InventoryStockStatus()
                {
                    WareHouseCode = s,
                    OnHandInventory = GeneralConstants.DefaultQuantity,
                    WareHouse = inventoryHelper.WarehouseDesc.ContainsKey(s) ? inventoryHelper.WarehouseDesc[s] : s
                });
        }

        private static void AddMoreTwoItems(List<InventoryStockStatus> listInventoryStockStatus)
        {
            var warehouseFirstItem = listInventoryStockStatus[0].FormalWareHouseCode;
            var warehouseSecItem = listInventoryStockStatus[1].FormalWareHouseCode;
            listInventoryStockStatus.AddRange(
                from s in DefaultWhList
                where String.Compare(s, warehouseFirstItem, StringComparison.OrdinalIgnoreCase) != 0 &&
                String.Compare(s, warehouseSecItem, StringComparison.OrdinalIgnoreCase) != 0
                select new InventoryStockStatus()
                {
                    WareHouseCode = s,
                    OnHandInventory = GeneralConstants.DefaultQuantity
                });
        }

        private static void AddMoreOneItem(List<InventoryStockStatus> listInventoryStockStatus)
        {
            var warehouseFirstItem = listInventoryStockStatus[0].FormalWareHouseCode;
            var warehouseSecItem = listInventoryStockStatus[1].FormalWareHouseCode;
            var warehouseThirdItem = listInventoryStockStatus[2].FormalWareHouseCode;
            listInventoryStockStatus.AddRange(
                from s in DefaultWhList
                where String.Compare(s, warehouseFirstItem, StringComparison.OrdinalIgnoreCase) != 0
                && String.Compare(s, warehouseSecItem, StringComparison.OrdinalIgnoreCase) != 0
                && String.Compare(s, warehouseThirdItem, StringComparison.OrdinalIgnoreCase) != 0
                select new InventoryStockStatus()
                {
                    WareHouseCode = s,
                    OnHandInventory = GeneralConstants.DefaultQuantity
                });
        }

        private static bool ValidateInventoryStockStatus(InventoryStockStatus item)
        {
            return !String.IsNullOrEmpty(item.WareHouse) &&
                        (String.Compare(item.FormalWareHouseCode, InventoryWareHouseCode.Com, StringComparison.OrdinalIgnoreCase) == 0 ||
                         String.Compare(item.FormalWareHouseCode, InventoryWareHouseCode.Mom, StringComparison.OrdinalIgnoreCase) == 0 ||
                         String.Compare(item.FormalWareHouseCode, InventoryWareHouseCode.Ren, StringComparison.OrdinalIgnoreCase) == 0 ||
                         String.Compare(item.FormalWareHouseCode, InventoryWareHouseCode.Som, StringComparison.OrdinalIgnoreCase) == 0 ||
                         String.Compare(item.FormalWareHouseCode, InventoryWareHouseCode.VIM, StringComparison.OrdinalIgnoreCase) == 0 ||
                         String.Compare(item.FormalWareHouseCode, InventoryWareHouseCode.VIE, StringComparison.OrdinalIgnoreCase) == 0 ||
                         String.Compare(item.FormalWareHouseCode, InventoryWareHouseCode.SUP, StringComparison.OrdinalIgnoreCase) == 0);
        }

        private static string CorrectQuantity(string quantity)
        {
            int iQuantity;
            if (Int32.TryParse(quantity, out iQuantity))
            {
                return quantity;
            }
            return GeneralConstants.DefaultQuantity;
        }

        /// <summary>
        /// Compare Warehouse
        /// </summary>
        /// <param name="warehouseA"></param>
        /// <param name="warehouseB"></param>
        /// <returns></returns>
        private static bool IsEqual(string warehouseA, string warehouseB)
        {
            warehouseA = ChangeRno2RenIfAny(warehouseA);
            warehouseB = ChangeRno2RenIfAny(warehouseB);
            return String.Compare(warehouseA, warehouseB, StringComparison.OrdinalIgnoreCase) == 0;
        }

        private static string ChangeRno2RenIfAny(string warehouse)
        {
            if (String.Compare(warehouse, InventoryWareHouseCode.Rno, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return InventoryWareHouseCode.Ren;
            }
            return warehouse;
        }

        public static List<InventoryStockStatus> SortRealTimeInventoryList(List<InventoryStockStatus> listInventoryStockStatus,
                                                                            string userId, Account account, MarketType marketType)
        {
            try
            {
                var sortedList = new List<InventoryStockStatus>();
                if (listInventoryStockStatus != null && listInventoryStockStatus.Count > 0)
                {
                    var remainList = new List<InventoryStockStatus>();
                    var vipList = new List<InventoryStockStatus>();

                    string primaryWareHouse = String.Empty;
                    string secondaryWareHouse = String.Empty;
                    if (account != null)
                    {
                        if (account.PrimaryWarehouse != null)
                            primaryWareHouse = account.PrimaryWarehouse.Code;
                        else if (!string.IsNullOrEmpty(account.PrimaryWarehouseCode))
                            primaryWareHouse = account.PrimaryWarehouseCode;

                        if (account.SecondaryWarehouse != null)
                            secondaryWareHouse = account.SecondaryWarehouse.Code;
                        else if (!string.IsNullOrEmpty(account.SecondaryWarehouseCode))
                            secondaryWareHouse = account.SecondaryWarehouseCode;
                    }

                    //sort and mark primary/secondary
                    foreach (InventoryStockStatus st in listInventoryStockStatus)
                    {
                        if (IsEqual(st.WareHouseCode, primaryWareHouse))
                        {
                            st.WareHouse += GeneralConstants.PrimaryMark;
                            st.WareHouseCode += GeneralConstants.PrimaryMark;
                            sortedList.Insert(0, st);
                        }
                        else if (IsEqual(st.WareHouseCode, secondaryWareHouse))
                        {
                            st.WareHouse += GeneralConstants.SecondaryMark;
                            st.WareHouseCode += GeneralConstants.SecondaryMark;
                            sortedList.Add(st);
                        }
                        else if (IsVIPWarehouse(st.WareHouseCode))
                        {
                            st.WareHouse += GeneralConstants.VipMark;
                            st.WareHouseCode += GeneralConstants.VipMark;
                            vipList.Add(st);
                        }
                        else
                        {
                            remainList.Add(st);
                        }
                    }

                    sortedList.AddRange(remainList);

                    if (vipList.Count > 0)
                    {
                        if (marketType != null && (marketType == MarketType.AcademicLibrary
                        || marketType == MarketType.PublicLibrary
                        || marketType == MarketType.SchoolLibrary))
                        {
                            sortedList.InsertRange(0, vipList);
                        }
                        else
                            sortedList.AddRange(vipList);
                    }
                }
                return sortedList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public static List<string> GetSkusList(IList<ProductSearchResultItem> productInfos,
        //                                        IDictionary<string, Account> dictAccounts, bool fromReviewAndSubmitOrderPage = true)
        //{
        //    if (productInfos == null || productInfos.Count == 0) return null;

        //    const string FALSE = "0";
        //    const string TRUE = "1";

        //    var results = new List<string>();
        //    foreach (var product in productInfos)
        //    {
        //        var productType = RefineProductTypeToMusicIfMovie(product.ProductType);

        //        if (fromReviewAndSubmitOrderPage)
        //        {
        //            if (string.Compare(productType, ProductType.Book.ToString(), StringComparison.OrdinalIgnoreCase) == 0
        //            && !string.IsNullOrEmpty(product.ESupplier))
        //                continue;
        //        }

        //        var checkLeReserve = FALSE;
        //        var accountInventoryType = String.Empty;
        //        var inventoryReserveNumber = String.Empty;

        //        if (dictAccounts == null || dictAccounts.Count == 0)
        //        {
        //            results.Add(String.Format("{0}@{1}@{2}@{3}", product.BTKey, checkLeReserve, accountInventoryType, inventoryReserveNumber));
        //            continue;
        //        }

        //        if (dictAccounts.ContainsKey(productType))
        //        {
        //            var account = dictAccounts[productType];
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
        //        results.Add(String.Format("{0}@{1}@{2}@{3}", product.BTKey, checkLeReserve, accountInventoryType, inventoryReserveNumber));
        //    }

        //    return results;
        //}

        public static string RefineProductTypeToMusicIfMovie(string productType)
        {
            if (string.Compare(productType, ProductType.Movie.ToString()) == 0)
            {
                productType = ProductType.Music.ToString();
            }
            return productType;
        }

        public static bool IsVIPWarehouse(string warehouse)
        {
            return (warehouse == "VIE" || warehouse == "VIM" || warehouse == "VIP");
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

        public static bool IsDisplayVIPWarehouse(MarketType marketType, string siteContextCountryCode, string siteContextOrgId)
        {
            return (marketType == MarketType.Retail && IsUSCountry(siteContextCountryCode) && IsVIPEnabled(siteContextOrgId));
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

        public static void AddSuperWarehouse(ref DataSet dsInventory)
        {
            var dictBTKey = new Dictionary<string, List<DataRow>>();

            var rows = dsInventory.Tables[0].Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];

                var btKey = row[PropertyName.BTKey].ToString();

                var wareHouse = row[PropertyName.WarehouseId].ToString();
                if (InventoryHelper.IsVIPWarehouse(wareHouse))
                {
                    if (!dictBTKey.ContainsKey(btKey))
                    {
                        dictBTKey[btKey] = new List<DataRow> { row };
                    }
                    else
                    {
                        dictBTKey[btKey].Add(row);
                    }
                }
            }


            foreach (KeyValuePair<string, List<DataRow>> kvpVIPInventory in dictBTKey)
            {
                int stockCondition = 0;
                int inStockForRequest = 0;
                int onOrderQuantity = 0;
                int invDemandNumber = 0;
                int lineItem = 0;

                string btKey = kvpVIPInventory.Key;

                foreach (DataRow datarow in kvpVIPInventory.Value)
                {
                    stockCondition += int.Parse(datarow[PropertyName.StockCondition].ToString());
                    inStockForRequest += int.Parse(datarow[PropertyName.InStockForRequest].ToString());
                    onOrderQuantity += int.Parse(datarow[PropertyName.OnOrderQuantity].ToString());
                    invDemandNumber += datarow[PropertyName.Last30DayDemand] == null ? 0 : int.Parse(datarow[PropertyName.Last30DayDemand].ToString());
                    lineItem += int.Parse(datarow[PropertyName.LineItem].ToString());
                }

                if (inStockForRequest >= GetSuperWarehouseInventoryThreshold())
                {
                    DataRow drSuperWarehouse = dsInventory.Tables[0].NewRow();
                    drSuperWarehouse[PropertyName.WarehouseId] = InventoryWareHouseCode.SUP;
                    drSuperWarehouse[PropertyName.LineItem] = lineItem;
                    drSuperWarehouse[PropertyName.StockCondition] = stockCondition;
                    drSuperWarehouse[PropertyName.InStockForRequest] = inStockForRequest;
                    drSuperWarehouse[PropertyName.OnOrderQuantity] = onOrderQuantity;
                    drSuperWarehouse[PropertyName.Last30DayDemand] = invDemandNumber;
                    drSuperWarehouse[PropertyName.BTKey] = btKey;

                    dsInventory.Tables[0].Rows.Add(drSuperWarehouse);
                }
            }

        }

        private static int GetSuperWarehouseInventoryThreshold()
        {
            var inventoryThreshold = AppSettings.SuperWarehouseInventoryThreshold;//GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.SuperWarehouseInventoryThreshold);
            if (inventoryThreshold != null)
            {
                if (!string.IsNullOrEmpty(inventoryThreshold))
                    return Convert.ToInt32(inventoryThreshold);
            }
            return 0;
        }

        public static StockCheckResponse GetRealTimeInventory(string accId, string btKey)
        {
            //var stockCheck = new StockCheck();
            var stockCheck = new StockCheckSoapClient();

            //var sysId = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.RealtimeWsSysid).Value;
            var sysId = BT.TS360API.Common.Configrations.AppSettings.RealtimeWsSysid;

            //var pass = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.RealtimeWsSyspass).Value;
            var pass = BT.TS360API.Common.Configrations.AppSettings.RealtimeWsSyspass;

            var result = stockCheck.StockCheckByParameters(sysId, pass, accId, btKey);

            //result.Warehouses = new WHS[4]
            //        {
            //            new WHS(){QTYOnHand = 152,WHSCode = "MOM",WHSDescription = "MOMDes",QTYOnOrder = 51}, 
            //            new WHS(){QTYOnHand = 153,WHSCode = "COM",WHSDescription = "COMDes",QTYOnOrder = 52}, 
            //            new WHS(){QTYOnHand = 154,WHSCode = "REN",WHSDescription = "RENDes",QTYOnOrder = 53}, 
            //            new WHS(){QTYOnHand = 155,WHSCode = "SOM",WHSDescription = "SOMDes",QTYOnOrder = 54}, 
            //        };

            //var whs = CSProfileDAO.Instance.GetWareHouses();
            var whs = ProfileDAO.Instance.GetWareHouses();

            if (result.Warehouses == null)
            { }
            else
            {
                result.Warehouses
                    .ToList()
                    .ForEach(r =>
                    {
                        var wh = whs.FirstOrDefault(r1 => r1.Code == r.WHSCode);
                        if (wh == null) { }
                        else
                        {
                            r.WHSDescription = wh.Description;
                        }
                    }
                    );
            }

            return result;
        }

        public static CartStockCheckResponse GetRealTimeCartInventory(string userID, BTStockServiceLineItem[] lineItems, string targetERP, string cartAccountERPNumber)
        {
            var sysId = BT.TS360API.Common.Configrations.AppSettings.RealtimeWsSysid;
            var pass = BT.TS360API.Common.Configrations.AppSettings.RealtimeWsSyspass;

            int totalQuantity = lineItems.Sum(lineItem => lineItem.OrderQuantity);

            if (string.IsNullOrWhiteSpace(cartAccountERPNumber))
            {
                return new CartStockCheckResponse
                {
                    StatusMessage = AccountConstants.INVALID_ACCOUNT,
                    ERPResponseStatusCode = "-1",
                    LineItems = new ResponseLineItem[0]
                };
            }

            var cartStockCheckRequest = new CartStockCheckRequest
            {
                SystemID = sysId,
                SystemPassword = pass,
                AccountIDType = AccountTypes.AccountNumber,
                AccountID = cartAccountERPNumber,
                TargetERP = targetERP,
                TotalOrderQuantity = totalQuantity,
                LineItems = lineItems
            };

            var stockCheck = new StockCheckSoapClient();
            var cartStockCheckResponse = stockCheck.StockCheckByCart(cartStockCheckRequest);

            var whs = ProfileDAO.Instance.GetWareHouses();
            if (cartStockCheckResponse.LineItems != null)
            {
                cartStockCheckResponse.LineItems
                    .SelectMany(r => r.Warehousehoses)
                    .ToList()
                    .ForEach(r =>
                    {
                        var wh = whs.FirstOrDefault(r1 => r1.Code == r.WHSCode);
                        if (wh != null)
                        {
                            r.WHSDescription = wh.Description;
                        }
                    });
            }

            return cartStockCheckResponse;
        }
    }
}
