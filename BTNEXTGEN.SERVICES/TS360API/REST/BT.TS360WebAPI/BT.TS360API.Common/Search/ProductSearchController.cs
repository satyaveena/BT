using BT.FSIS;
using BT.TS360API.Common.Controller;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Search.Helpers;
using BT.TS360API.ExternalServices;
using BT.TS360API.ExternalServices.BTStockCheckServices;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Pricing;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
//using System.Web.ApplicationServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Search
{
    public sealed class ProductSearchController
    {
        #region Move WCF to Api

        public static void SortWareHouses(out List<WHS> remain, string secondaryWareHouse, InventoryResults inventoryResults, out List<WHS> sortedList, string primaryWareHouse)
        {
            sortedList = new List<WHS>();
            remain = new List<WHS>();

            if (inventoryResults != null &&
                inventoryResults.InventoryStock != null)
            {
                foreach (InventoryStockStatus item in inventoryResults.InventoryStock)
                {
                    var onHandInventory = 0;
                    int.TryParse(item.OnHandInventory, out onHandInventory);
                    var wHSItem = new WHS()
                    {
                        QTYOnHand = onHandInventory,
                        QTYOnOrder = item.OnOrderQuantity,
                        WHSCode = item.FormalWareHouseCode,
                        WHSDescription = item.WareHouseCode
                    };
                    if (item.FormalWareHouseCode == primaryWareHouse)
                    {
                        sortedList.Insert(0, wHSItem);
                    }
                    else if (item.FormalWareHouseCode == secondaryWareHouse)
                    {
                        sortedList.Add(wHSItem);
                    }
                }

                sortedList.AddRange(remain);

                //mart primary/secondarry
                foreach (WHS item in sortedList)
                {
                    if (item.WHSCode == primaryWareHouse)
                        item.WHSCode = item.WHSCode + " *";
                    if (item.WHSCode == secondaryWareHouse)
                        item.WHSCode = item.WHSCode + " **";
                }
            }
        }

        public static void SortWareHouses(out List<WHS> remain, string secondaryWareHouse, StockCheckResponse response, out List<WHS> sortedList, string primaryWareHouse)
        {
            sortedList = new List<WHS>();
            remain = new List<WHS>();

            if (response != null && response.Warehouses != null && response.Warehouses.Length > 0)
            {
                foreach (WHS item in response.Warehouses)
                {
                    if (item.WHSCode == primaryWareHouse)
                        sortedList.Insert(0, item);
                    else if (item.WHSCode == secondaryWareHouse)
                        sortedList.Add(item);
                    else
                        remain.Add(item);
                }

                sortedList.AddRange(remain);

                //mart primary/secondarry
                foreach (WHS item in sortedList)
                {
                    if (item.WHSCode == primaryWareHouse)
                        item.WHSCode = item.WHSCode + " *";
                    if (item.WHSCode == secondaryWareHouse)
                        item.WHSCode = item.WHSCode + " **";
                    // remove region in description
                    string[] desc = item.WHSDescription.Split(',');
                    if (desc.Length > 0)
                        item.WHSDescription = desc[0].Trim();
                }
            }
        }

        #endregion

        //#region Static Methods

        /// <summary>
        /// Perform search against FAST
        /// </summary>
        /// <param name="searchArguments">The search arguments</param>
        /// <returns>The search results: need to call the method CalculatePrice of UI\CommonControllers 
        /// after call this method</returns>
        public static ProductSearchResults Search(SearchArguments searchArguments,
            MarketType? marketType,
            bool simonSchusterEnabled,
            string countryCode,
            string[] ESuppliers,
            bool includeMarketRestriction = true, bool notDeleted = true)
        {
            if (searchArguments == null)
            {
                PricingLogger.LogDebug("Search", string.Format("searchArguments is null"));
                return null;
            }
            if (includeMarketRestriction)
            {
                // get the MRC GroupOperator
                var mrcGroupOperator = SearchHelper.CreateGroupExpressionForMarketRestrictionCodes(marketType);
                if (mrcGroupOperator != null)
                {
                    searchArguments.SearchExpressionGroup.AddSearchExpress(mrcGroupOperator);
                }
            }

            if (marketType.HasValue
                && marketType == MarketType.PublicLibrary && simonSchusterEnabled)
            {
                // Updated rule in TFS 9731, comment 4/21 by Ivor.
                // This case will see Simon titles without geographic restriction and non Simon titles with geographic restriction.
                var grcGroupOperator = SearchHelper.CreateGroupExpressionForSimonEnabledAndPublicLib(marketType, countryCode);
                if (grcGroupOperator != null)
                {
                    searchArguments.SearchExpressionGroup.AddSearchExpress(grcGroupOperator);
                }
            }
            else if (marketType.HasValue
                && marketType == MarketType.SchoolLibrary)
            {
                var grcGroupOperator = SearchHelper.CreateGroupExpressionForGeographicExclusive(marketType, countryCode);
                if (grcGroupOperator != null)
                {
                    searchArguments.SearchExpressionGroup.AddSearchExpress(grcGroupOperator);

                }
            }
            else if (marketType.HasValue
                && marketType == MarketType.AcademicLibrary
                && simonSchusterEnabled)
            {
                var grcGroupOperator = SearchHelper.CreateGroupExpressionForGeographicExclusive(marketType, countryCode);
                if (grcGroupOperator != null)
                {
                    searchArguments.SearchExpressionGroup.AddSearchExpress(grcGroupOperator);
                }
            }
            else
            {
                //case Simon OFF
                var grcNotSimonTitle = SearchHelper.CreateGroupExpressionForNonSimonSchuster();
                var grcGeoExclusive = SearchHelper.CreateGroupExpressionForGeographicExclusive(marketType, countryCode);
                if (grcGeoExclusive != null && grcNotSimonTitle != null)
                {
                    searchArguments.SearchExpressionGroup.AddSearchExpress(grcNotSimonTitle);
                    searchArguments.SearchExpressionGroup.AddSearchExpress(grcGeoExclusive);
                }
            }

            if (marketType.HasValue && marketType == MarketType.Retail)
            {
                //TFS33169
                var exp = new SearchExpression(SearchFieldNameConstants.merchcategory, "MAKERSPACE", BooleanOperatorConstants.Not);
                searchArguments.SearchExpressionGroup.AddSearchExpress(exp);
            }

            if (notDeleted)
            {
                var exp = new SearchExpression(SearchFieldNameConstants.productstatus, ProductStatus.D.ToString(), BooleanOperatorConstants.Not);
                searchArguments.SearchExpressionGroup.AddSearchExpress(exp);
            }

            if (!SearchHelper.ContainEBookSearchExp(searchArguments))
                searchArguments.SearchExpressionGroup.AddSearchExpress(SearchHelper.ApplyEBookSearchExpression(marketType, ESuppliers));

            var op1 = SearchHelper.CreateSearchOperators(searchArguments.SearchExpressionGroup);

            BT.FSIS.Search search = null;
            if (op1 != null)
            {
                #region TFS16341:FAST - ADD X Rank
                var isPopularityDesc = searchArguments.SortExpressions.Any(
                    sortex => String.Compare(sortex.SortField, SearchFieldNameConstants.popularity, StringComparison.OrdinalIgnoreCase) == 0
                            && sortex.SortDirection == SortDirection.Descending);
                if (isPopularityDesc)
                {
                    var boostingOperator = SearchHelper.CreateDemandBoosting(SearchFieldNameConstants.popularity);
                    if (boostingOperator != null)
                    {
                        var ranking = new GroupOperator(GroupOperatorType.Rank);
                        ranking.Operators.Add(op1);
                        ranking.Operators.Add(boostingOperator);
                        op1 = ranking;
                    }
                }
                #endregion

                search = new BT.FSIS.Search(op1) { ResultView = SearchViewItemConstants.SearchViewItem, TimeOut = 60000 };
            }
            if (search != null)
            {
                search.ResultView = SearchViewItemConstants.SearchViewItem;
                search.TimeOut = 60000;
                search.spellCheckType = SpellCheckType.Suggest;
            }

            var result = ExecuteSearch(search, searchArguments.SortExpressions.ToSortStrings(), searchArguments.StartRowIndex, searchArguments.PageSize);

            return result;
        }

        public static ProductSearchResults SearchById(List<string> productIdList, string userId, MarketType? marketType, string[] eSuppliers,
            bool simonSchusterEnabled, string countryCode, ProductStatus status = ProductStatus.A, bool includeProductFilter = false)
        {
            var result = SearchByTerms(productIdList, SearchFieldNameConstants.btkey, userId, marketType, eSuppliers, simonSchusterEnabled, countryCode, status, includeProductFilter);
            return result;
        }

        /// <summary>
        /// Similar with SearchByTerms except PageSize.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static ProductSearchResults SearchByBTKeys(SearchByBTKeysArgument arg)
        {
            if (arg == null || arg.BTKeyList == null)
            {
                throw new ArgumentNullException("SearchByBTKeysArgument");
            }

            var group = new SearchExpressionGroup();
            group.AddSearchExpress(new SearchExpression
            {
                Operator = BooleanOperatorConstants.And,
                Scope = SearchFieldNameConstants.btkey,
                Terms = string.Join("|", arg.BTKeyList.ToArray())
            });

            if (arg.ProductStatus == ProductStatus.A)
            {
                group.AddSearchExpress(new SearchExpression
                {
                    Operator = BooleanOperatorConstants.And,
                    Scope = SearchFieldNameConstants.productstatus,
                    Terms = "A"
                });
            }

            if (arg.IncludeProductFilter)
            {
                UserProfile userProfile = SearchHelper.CreateSearchExpressionForProductTypeFilter(group, arg.UserId);

                var formatExpressions = SearchHelper.GetSearchExpressionsForProductTypesAndFormats(arg.UserId, userProfile, arg.ESuppliers);
                if (formatExpressions.Count > 0)
                {
                    // add "Include" format group
                    var includeFormats = formatExpressions.Where(r => r.Operator != BooleanOperatorConstants.Not).ToList();
                    if (includeFormats.Count > 0)
                    {
                        var inclGroup = new SearchExpressionGroup(BooleanOperatorConstants.And);
                        inclGroup.AddSearchExpress(includeFormats);
                        group.AddSearchExpress(inclGroup);
                    }

                    // add "Exclude" format group
                    var exclFormats = formatExpressions.Where(r => r.Operator == BooleanOperatorConstants.Not).ToList();
                    if (exclFormats.Count > 0)
                    {
                        var exclGroup = new SearchExpressionGroup(BooleanOperatorConstants.And);
                        exclGroup.AddSearchExpress(exclFormats);
                        group.AddSearchExpress(exclGroup);
                    }
                }
            }
            group.AddSearchExpress(SearchHelper.ApplyEBookSearchExpression(arg.MarketType, arg.ESuppliers));

            var argument = new SearchArguments();
            argument.SearchExpressionGroup.AddSearchExpress(group);
            if (arg.SortExpression != null)
                argument.SortExpressions.Add(arg.SortExpression);
            else
                argument.SortExpressions.Add(new SortExpression { SortField = string.Empty });
            argument.StartRowIndex = 0;
            argument.PageSize = arg.PageSize;

            return Search(argument, arg.MarketType, arg.SimonSchusterEnabled, arg.CountryCode, arg.ESuppliers, notDeleted: false);
        }

        /// <summary>
        /// This method retrieves data from FAST without any filters, such as: product status, market/sale restriction, my preference.
        /// This is used in Cart Detail page.
        /// </summary>
        /// <param name="btKeys"></param>
        /// <returns></returns>
        public static ProductSearchResults SearchByIdWithoutAnyRules(List<string> btKeys)
        {
            if (btKeys == null)
            {
                PricingLogger.LogDebug("SearchByIdWithoutAnyRules", string.Format("btKeys is null"));
                return null;
            }

            const int startRowIndex = 0;
            int pageSize = btKeys.Count;

            var searchOperator = SearchHelper.CreateTermsOperators(btKeys, SearchFieldNameConstants.btkey);
            var search = new BT.FSIS.Search(searchOperator) { ResultView = SearchViewItemConstants.SearchViewItem };

            ProductSearchResults results = null;

            var searchExecutor = SearchHelper.CreateSearchExecutor(search, pageSize);
            if (searchExecutor != null)
            {
                var searchResults = searchExecutor.ExecuteSearch(startRowIndex);
                results = new ProductSearchResults(searchResults);
            }

            return results;
        }

        public static ProductSearchResults SearchByIdForContentManagement(List<string> productIdList,
            MarketType? marketType,
            bool simonSchusterEnabled,
            string countryCode,
            string[] ESuppliers)
        {
            return SearchByIdForContentManagement(productIdList, "", ProductStatus.A, marketType, simonSchusterEnabled, countryCode,
                ESuppliers);
        }

        public static ProductSearchResults SearchByIdForContentManagement(List<string> productIdList, string productType,
            ProductStatus status,
            MarketType? marketType,
            bool simonSchusterEnabled,
            string countryCode,
            string[] ESuppliers,
            bool includeMarketRestriction = true, bool notDeleted = true)
        {
            ProductSearchResults results = null;
            if (productIdList != null && productIdList.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var btkey in productIdList)
                {
                    sb.AppendFormat("{0}|", btkey);
                }

                var btKeys = sb.ToString().TrimEnd('|');


                var searchArguments = new SearchArguments();
                var exp1 = new SearchExpression(SearchFieldNameConstants.btkey, btKeys, BooleanOperatorConstants.And);
                //searchArguments.SearchExpressions.Add(exp1);
                searchArguments.SearchExpressionGroup.AddSearchExpress(exp1);
                var exp2 = new SearchExpression(SearchFieldNameConstants.productstatus, status.ToString(), BooleanOperatorConstants.And);
                //searchArguments.SearchExpressions.Add(exp2);
                searchArguments.SearchExpressionGroup.AddSearchExpress(exp2);

                if (!string.IsNullOrEmpty(productType))
                {
                    var exp3 = new SearchExpression(SearchFieldNameConstants.producttype, productType, BooleanOperatorConstants.And);
                    //searchArguments.SearchExpressions.Add(exp3);
                    searchArguments.SearchExpressionGroup.AddSearchExpress(exp3);
                }

                searchArguments.StartRowIndex = 0;
                searchArguments.PageSize = productIdList.Count;

                results = Search(searchArguments, marketType, simonSchusterEnabled, countryCode, ESuppliers, includeMarketRestriction, notDeleted);
            }
            return results;
        }

        public static bool IsHomeDeliveryAccount(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return false;
            }

            var account = ProfileController.Instance.GetAccountById(accountId); // AdministrationProfileController.Current.GetAccountById(accountId);
            return account != null && account.HomeDeliveryAccount.HasValue && account.HomeDeliveryAccount.Value;
        }

        public static void ApplyHomeDelivery(string defaultAccountID, ref string entertainmentAccountId, ref string bookAccountId,
            out bool isHomeDeliveryOnly)
        {
            var temp = string.Empty;
            ApplyHomeDelivery(defaultAccountID, ref entertainmentAccountId, ref bookAccountId, out isHomeDeliveryOnly, ref temp, ref temp);
        }
        public static void ApplyHomeDelivery(string defaultAccountID, ref string entertainmentAccountId, ref string bookAccountId,
            out bool isHomeDeliveryOnly, ref string bookProcessingChargeAccId, ref string entertainmentProcessingChargeAccId)
        {
            isHomeDeliveryOnly = false;
            if (!String.IsNullOrEmpty(defaultAccountID))
            {
                if (IsHomeDeliveryAccount(defaultAccountID))
                {
                    bookAccountId = entertainmentAccountId = bookProcessingChargeAccId = entertainmentProcessingChargeAccId = defaultAccountID;
                    isHomeDeliveryOnly = true;
                    return;
                }
            }
        }

        private static void GetAccountIdFromOrgForPricing(out string bookAccountId, out string entertainmentAccountId,
            out bool isHomeDeliveryOnly, out string vipAccountId, out string bookProcessingChargeAccId, out string entertainmentProcessingChargeAccId,
            string userId)
        {
            var user = ProfileService.Instance.GetUserById(userId); // CSObjectProxy.GetUserProfileForSearchResult();

            vipAccountId = bookAccountId = entertainmentAccountId = bookProcessingChargeAccId = entertainmentProcessingChargeAccId = string.Empty;

            isHomeDeliveryOnly = false;
            if (user == null) return;

            // user's default onebox account
            if (!string.IsNullOrEmpty(user.DefaultOneBoxAccountId))
            {
                bookAccountId = entertainmentAccountId = bookProcessingChargeAccId = entertainmentProcessingChargeAccId = user.DefaultOneBoxAccountId;
                return;
            }

            string defaultBookAccountId = string.Empty;
            string defaultEntertainmentAccountId = string.Empty;
            string defaultVIPAccountId = string.Empty;

            if (!string.IsNullOrEmpty(user.DefaultBookAccountId))
            {
                defaultBookAccountId = user.DefaultBookAccountId; // ((Account)user.DefaultBookAccount.Target).Id;
            }

            if (!string.IsNullOrEmpty(user.DefaultEntAccountId))
            {
                defaultEntertainmentAccountId = user.DefaultEntAccountId; //((Account)user.DefaultEntertainmentAccount.Target).Id;
            }

            if (!string.IsNullOrEmpty(user.DefaultVIPAccountId))
            {
                defaultVIPAccountId = user.DefaultVIPAccountId; //((Account)user.DefaultVIPAccount.Target).Id;
            }

            ApplyHomeDelivery(user.DefaultEntAccountId, ref entertainmentAccountId, ref bookAccountId, out isHomeDeliveryOnly, ref bookProcessingChargeAccId,
                ref entertainmentProcessingChargeAccId);

            if (isHomeDeliveryOnly) return;
            entertainmentProcessingChargeAccId = entertainmentAccountId = defaultEntertainmentAccountId;

            ApplyHomeDelivery(user.DefaultBookAccountId, ref entertainmentAccountId, ref bookAccountId, out isHomeDeliveryOnly, ref bookProcessingChargeAccId,
                ref entertainmentProcessingChargeAccId);
            if (isHomeDeliveryOnly) return;
            bookProcessingChargeAccId = bookAccountId = defaultBookAccountId;

            if (!string.IsNullOrEmpty(defaultVIPAccountId))
            {
                bookProcessingChargeAccId = vipAccountId = defaultVIPAccountId;
            }
            //check at org level
            if (String.IsNullOrEmpty(bookAccountId) || String.IsNullOrEmpty(entertainmentAccountId) || String.IsNullOrEmpty(vipAccountId))
            {
                var organizationId = user.OrgId; // .Organization.Target.Id;

                //var profileControllerForAdmin = AdministrationProfileController.Current;
                //profileControllerForAdmin.OrganizationRelated.DefaultBookAccountNeeded = true;
                //profileControllerForAdmin.OrganizationRelated.DefaultEntertainmentAccountNeeded = true;
                //profileControllerForAdmin.OrganizationRelated.DefaultVIPAccountNeeded = true;
                var organization = ProfileService.Instance.GetOrganizationById(organizationId);// profileControllerForAdmin.GetOrganization(organizationId);
                if (organization != null)
                {

                    //If user already have entertainment Acc, do not need to overrride it with organization's acc
                    //if (organization.DefaultEntertainmentAccount != null && String.IsNullOrEmpty(entertainmentAccountId))
                    if (string.IsNullOrEmpty(entertainmentAccountId))
                    {
                        var entertainmentAccountIdFromOrg = organization.DefaultEntAccountId;
                        if (!string.IsNullOrEmpty(organization.DefaultOneBoxAccountId))
                        {
                            entertainmentAccountIdFromOrg = organization.DefaultOneBoxAccountId;
                        }
                        else
                        {
                            var tempAccId = string.Empty;//processing charges don't take accounts at org level
                            ApplyHomeDelivery(entertainmentAccountIdFromOrg, ref entertainmentAccountId, ref bookAccountId, out isHomeDeliveryOnly,
                                ref tempAccId, ref tempAccId);

                            if (isHomeDeliveryOnly) return;
                        }

                        entertainmentAccountId = entertainmentAccountIdFromOrg;
                    }

                    //If user already have book Acc, do not need to overrride it with organization's acc
                    //if (organization.DefaultBookAccount != null && String.IsNullOrEmpty(bookAccountId))
                    if (string.IsNullOrEmpty(bookAccountId))
                    {
                        var bookAccountIdOrg = organization.DefaultBookAccountId;
                        if (!string.IsNullOrEmpty(organization.DefaultOneBoxAccountId))
                        {
                            bookAccountIdOrg = organization.DefaultOneBoxAccountId;
                        }
                        else
                        {
                            var tempAccId = string.Empty;//processing charges don't take accounts at org level
                            ApplyHomeDelivery(bookAccountIdOrg, ref entertainmentAccountId, ref bookAccountId, out isHomeDeliveryOnly, ref tempAccId,
                                ref tempAccId);
                            if (isHomeDeliveryOnly) return;
                        }

                        bookAccountId = bookAccountIdOrg;
                    }

                    //If user already have vip Acc, do not need to overrride it with organization's acc
                    //if (organization.DefaultVIPAccount != null && String.IsNullOrEmpty(vipAccountId))
                    if (string.IsNullOrEmpty(vipAccountId))
                    {
                        vipAccountId = organization.DefaultVIPAccountId; //((Account)(organization.DefaultVIPAccount.Target)).Id;
                        //processing charges don't take accounts at org level
                    }
                }
            }
        }

        /// <summary>
        /// Replace DetermineInventory.GetUserDefaultAccount from InventoryHelper.GetItemInventory
        /// </summary>
        /// <param name="searchArg"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Account GetUserDefaultAccount(SearchResultInventoryStatusArg searchArg, string userId)
        {
            var defaultESupplierAccount = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            return GetUserDefaultAccount(searchArg, userId, defaultESupplierAccount);
        }

        private static Account GetUserDefaultAccount(SearchResultInventoryStatusArg searchArg, string userId,
            Dictionary<string, string> defaultESupplierAccount)
        {
            string accountId = GetAccountInfoForPricing(userId, searchArg.ProductType, searchArg.ESupplier, defaultESupplierAccount);
            //var administrationProfileController = AdministrationProfileController.Current;
            //administrationProfileController.AccountRelated.PrimaryWarehouseNeeded = true;
            //administrationProfileController.AccountRelated.SecondaryWarehouseNeeded = true;
            //return administrationProfileController.GetAccountById(accountId);

            if (string.IsNullOrEmpty(accountId)) return null;

            return ProfileController.Instance.GetAccountById(accountId);
        }

        private static string GetAccountInfoForPricing(string userId, string productType, string eSupplier, Dictionary<string, string> defaultESupplierAccount)
        {
            bool isHomeDelivery;
            bool isVIPAccount;
            string accountId;
            string temp;
            GetAccountInfoForPricing(userId, productType, eSupplier, out accountId, out isHomeDelivery, defaultESupplierAccount, out isVIPAccount, out temp);
            return accountId;
        }

        private static void GetAccountInfoForPricing(string userId, string productType, string eSupplierDisplayText, out string accountId,
            out bool isHomeDelivery, out bool isVIPAccount, out string processingChargeAccId)
        {
            var defaultESupplierAccount = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            GetAccountInfoForPricing(userId, productType, eSupplierDisplayText, out accountId, out isHomeDelivery, defaultESupplierAccount,
                out isVIPAccount, out processingChargeAccId);
        }

        /// <summary>
        /// Get account info for pricing with ebook
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="productType"></param>
        /// <param name="eSupplierDisplayText"></param>
        /// <param name="accountId"></param>
        /// <param name="isHomeDelivery"></param>
        /// <param name="defaultESupplierAccount"></param>
        private static void GetAccountInfoForPricing(string userId, string productType, string eSupplierDisplayText, out string accountId, out bool isHomeDelivery,
            Dictionary<string, string> defaultESupplierAccount, out bool isVIPAccount, out string processingChargeAccId)
        {
            string bookAccountId;
            string entertainmentAccountId;
            string bookProcessingChargeAccId;
            string entertainmentProcessingChargeAccId;
            string vipAccountId;

            GetAccountIdFromOrgForPricing(out bookAccountId, out entertainmentAccountId, out isHomeDelivery, out vipAccountId, out bookProcessingChargeAccId,
                out entertainmentProcessingChargeAccId, userId);
            accountId = SelectTheAccount(productType, eSupplierDisplayText, entertainmentAccountId, bookAccountId, defaultESupplierAccount, vipAccountId,
                out isVIPAccount, bookProcessingChargeAccId, entertainmentProcessingChargeAccId, out processingChargeAccId, userId);

        }

        public static string SelectTheAccount(string productType, string eSupplier, string entertainmentAccountId,
            string bookAccountId, Dictionary<string, string> defaultESupplierAccount,
            string vipAccountId, out bool isVIPAccount, string userId)
        {
            var temp = string.Empty;
            return SelectTheAccount(productType, eSupplier, entertainmentAccountId, bookAccountId, defaultESupplierAccount, vipAccountId,
                out isVIPAccount, temp, temp, out temp, userId);
        }

        public static string SelectTheAccount(string productType, string eSupplierDisplayText, string entertainmentAccountId, string bookAccountId,
            Dictionary<string, string> defaultESupplierAccount, string vipAccountId, out bool isVIPAccount, string bookProcessingChargeAccId,
            string entertainmentProcessingChargeAccId, out string processingChargeAccId, string userId)
        {
            PricingLogger.LogDebug("ProductSearchController.SelectTheAccount", string.Format("productType: {0}, eSupplierDisplayText: {1}, vipAccountId: {2}",
                productType, eSupplierDisplayText, vipAccountId));

            isVIPAccount = false;
            var accountId = processingChargeAccId = string.Empty;
            var prodType = CommonHelper.GetProductType(productType);
            switch (prodType)
            {
                case ProductType.Book:
                    if (!string.IsNullOrEmpty(vipAccountId))
                    {
                        accountId = vipAccountId;
                        if (string.Compare(bookProcessingChargeAccId, vipAccountId, StringComparison.OrdinalIgnoreCase) ==
                            0)
                        {
                            processingChargeAccId = vipAccountId;
                        }
                        else if (string.Compare(bookProcessingChargeAccId, bookAccountId, StringComparison.OrdinalIgnoreCase) ==
                            0)
                        {
                            processingChargeAccId = bookAccountId;
                        }
                        isVIPAccount = true;
                    }
                    else if (!string.IsNullOrEmpty(eSupplierDisplayText))
                    {
                        var eSupplierValue = CommonHelper.ConvertESupplierNameToValue(eSupplierDisplayText);
                        PricingLogger.LogDebug("ProductSearchController.SelectTheAccount", string.Format("eSupplier after converting: {0}", eSupplierValue));
                        accountId = CommonHelper.GetAccountIdAssociatedWithAnESupplier(eSupplierValue, userId, defaultESupplierAccount,
                            out processingChargeAccId);

                    }
                    else
                    {
                        accountId = bookAccountId;
                        if (string.Compare(bookProcessingChargeAccId, bookAccountId, StringComparison.OrdinalIgnoreCase) ==
                            0)
                        {
                            processingChargeAccId = bookAccountId;
                        }
                    }


                    break;
                case ProductType.Movie:
                case ProductType.Music:
                    accountId = entertainmentAccountId;
                    if (string.Compare(entertainmentProcessingChargeAccId, entertainmentAccountId, StringComparison.OrdinalIgnoreCase) ==
                            0)
                    {
                        processingChargeAccId = entertainmentAccountId;
                    }
                    break;
            }
            return accountId;
        }

        public static RequiredPricingInfo GetAccountInfoForPricing(string productType, string eSupplierDisplayText, string userId,
             AccountInfoForPricing accountPricingData)
        {
            var requiredInfor = new RequiredPricingInfo();
            string processingChargeAccId = string.Empty;
            string accountId = string.Empty;
            bool isHomeDelivery;
            bool isVIPAccount;

            GetAccountInfoForPricing(userId, productType, eSupplierDisplayText, out accountId, out isHomeDelivery, out isVIPAccount,
                out processingChargeAccId);

            requiredInfor.AccountId = accountId;
            requiredInfor.IsHomeDelivery = isHomeDelivery;
            requiredInfor.IsVIPAccount = isVIPAccount;

            //var administrationProfileController = AdministrationProfileController.Current;
            //administrationProfileController.AccountRelated.PrimaryWarehouseNeeded = true;
            var account = ProfileController.Instance.GetAccountById(accountId);// administrationProfileController.GetAccountById(accountId);

            if (account != null)
            {
                requiredInfor.ErpAccountNumber = account.AccountNumber;
                if (!string.IsNullOrEmpty(account.PrimaryWarehouseCode))
                {
                    //Warehouse primaryWareHouse = account.PrimaryWarehouse.Target;
                    requiredInfor.PrimaryWarehouseCode = account.PrimaryWarehouseCode;// primaryWareHouse.Code;
                }

                requiredInfor.EMarketType = account.EMarketType;
                requiredInfor.ETier = account.ETier;

                var sopAccount = CommonHelper.Instance.GetAccountForSOPPricePlan(accountId);// administrationProfileController.GetAccountForSOPPricePlan(accountId);
                if (sopAccount != null)
                {
                    requiredInfor.AccountPricePlanId = sopAccount.SOPPricePlanId;
                }

                if (account.NumberOfBuilding.HasValue)
                    requiredInfor.BuildingNumbers = account.NumberOfBuilding.Value;

                if (accountPricingData.EnableProcessingCharges)
                {
                    if (!string.IsNullOrEmpty(processingChargeAccId))
                    {
                        if (string.Compare(accountId, processingChargeAccId, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            requiredInfor.ProcessingCharges = account.ProcessingCharge.HasValue ? account.ProcessingCharge.Value : 0;
                            requiredInfor.ProcessingCharges2 = account.ProcessingCharges2.HasValue
                                ? account.ProcessingCharges2.Value
                                : 0;
                            requiredInfor.SpokenWordCharge = account.ProcessingCharges3.HasValue
                                ? account.ProcessingCharges3.Value
                                : 0;
                            requiredInfor.SalesTax = account.SalesTax.HasValue ? account.SalesTax.Value : 0;
                        }
                        else
                        {
                            var processingChargesAcc =
                                ProfileController.Instance.GetAccountById(processingChargeAccId); // administrationProfileController.GetAccountById(processingChargeAccId);
                            if (processingChargesAcc != null)
                            {
                                requiredInfor.ProcessingCharges = processingChargesAcc.ProcessingCharge.HasValue
                                    ? processingChargesAcc.ProcessingCharge.Value
                                    : 0;
                                requiredInfor.ProcessingCharges2 = processingChargesAcc.ProcessingCharges2.HasValue
                                    ? processingChargesAcc.ProcessingCharges2.Value
                                    : 0;
                                requiredInfor.SpokenWordCharge = processingChargesAcc.ProcessingCharges3.HasValue
                                ? processingChargesAcc.ProcessingCharges3.Value
                                : 0;
                                requiredInfor.SalesTax = processingChargesAcc.SalesTax.HasValue ? processingChargesAcc.SalesTax.Value : 0;
                            }

                        }
                    }
                }
            }
            else
            {
                if (accountPricingData.EnableProcessingCharges)
                {
                    if (!string.IsNullOrEmpty(processingChargeAccId))
                    {
                        var processingChargesAcc = ProfileController.Instance.GetAccountById(processingChargeAccId); // administrationProfileController.GetAccountById(processingChargeAccId);
                        if (processingChargesAcc != null)
                        {
                            requiredInfor.ProcessingCharges = processingChargesAcc.ProcessingCharge.HasValue
                                ? processingChargesAcc.ProcessingCharge.Value
                                : 0;
                            requiredInfor.ProcessingCharges2 = processingChargesAcc.ProcessingCharges2.HasValue
                                ? processingChargesAcc.ProcessingCharges2.Value
                                : 0;
                            requiredInfor.SpokenWordCharge = processingChargesAcc.ProcessingCharges3.HasValue
                                ? processingChargesAcc.ProcessingCharges3.Value
                                : 0;
                            requiredInfor.SalesTax = processingChargesAcc.SalesTax.HasValue ? processingChargesAcc.SalesTax.Value : 0;
                        }

                    }
                }
            }

            if (accountPricingData.EnableProcessingCharges)
            {

                var prodType = CommonHelper.GetProductType(productType);
                if (requiredInfor.ProcessingCharges == 0 && requiredInfor.ProcessingCharges2 == 0 && requiredInfor.SpokenWordCharge == 0
                    && requiredInfor.SalesTax == 0 && string.IsNullOrEmpty(eSupplierDisplayText)) //ebooks don't have org level
                {
                    requiredInfor.ProcessingCharges = prodType == ProductType.Book
                        ? accountPricingData.BookProcessingCharge
                        : accountPricingData.MovieProcessingCharge;

                    requiredInfor.ProcessingCharges2 = prodType == ProductType.Book
                        ? accountPricingData.PaperbackProcessingCharge
                        : accountPricingData.MusicProcessingCharge;
                    requiredInfor.SpokenWordCharge = accountPricingData.SpokenWordProcessingCharge;
                    requiredInfor.SalesTax = accountPricingData.SalesTax;
                }
            }

            return requiredInfor;
        }

        //public static void GetPrimaryWarehouseCodeAndErpAccountNumber(string accountId, out string primaryWarehouseCode, out string erpAccountNumber)
        //{
        //    primaryWarehouseCode = String.Empty;
        //    erpAccountNumber = String.Empty;
        //    // list properties to return
        //    // the relationship that need to be returned
        //    var profileControllerForAdmin = AdministrationProfileController.Current;
        //    profileControllerForAdmin.AccountRelated.PrimaryWarehouseNeeded = true;
        //    var account = profileControllerForAdmin.GetAccountById(accountId);
        //    if (account != null)
        //    {
        //        if (account.PrimaryWarehouse != null)
        //        {
        //            Warehouse primaryWareHouse = account.PrimaryWarehouse.Target;
        //            primaryWarehouseCode = primaryWareHouse.Code;
        //        }
        //        erpAccountNumber = account.AccountNumber;
        //    }
        //}

        //public static string GetProductMuzeFromContentCafe(string btKey)
        //{
        //    return ContentCafeHelper.GetProductMuzeFromContentCafe(btKey);
        //}

        //public static string GetTocFromCc(string btKey)
        //{
        //    return ContentCafeHelper.GetTOCFromContentCafe(btKey);
        //}

        public static bool ShowReturnFlag(string[] filter)
        {
            //Exclude Non-Returnables
            if (filter.Contains("NonReturnables"))
                return false;
            //
            return true;
        }

        //public static async Task<List<SiteTermObject>> GetProductContentIndicator(List<string> btKeyList, string url, string userId, string orgId)
        //{
        //    var list = new List<SiteTermObject>();

        //    //var profileController = ProfileController.Current;
        //    //profileController.UserProfileRelated.OrganizationNeeded = true;
        //    //profileController.OrganizationPropertiesToReturn.Add(Organization.PropertyName.ReviewTypes);
        //    //profileController.OrganizationPropertiesToReturn.Add(Organization.PropertyName.EntertainmentProduct);
        //    //profileController.UserProfilePropertiesToReturn.Add(UserProfile.PropertyName.ProductTypeFilter);
        //    //var user = profileController.GetUserById(SiteContext.Current.UserId);

        //    var user = ProfileService.Instance.GetUserById(userId);
        //    //
        //    if (user != null)
        //    {
        //        var organization = ProfileService.Instance.GetOrganizationById(orgId);// user.OrganizationEntity;
        //        if (organization != null)
        //        {
        //            var reviewType = organization.ReviewTypeList;
        //            //
        //            var filter = user.ProductTypeFilter;
        //            var isShowReturn = ShowReturnFlag(filter);
        //            //
        //            var contentFromOds = await ProductDAO.Instance.CheckProductContentFromODS(btKeyList.ToArray(), reviewType);
        //            //
        //            if (contentFromOds != null)
        //            {
        //                foreach (var item in contentFromOds)
        //                {
        //                    if (isShowReturn == false)
        //                        item.Value.HasReturnKey = false;
        //                    list.Add(new SiteTermObject(item.Key, SearchHelper.CombineContentIndicator(item.Value, item.Key, url)));
        //                }
        //            }
        //        }
        //    }

        //    return list;
        //}

        public static List<SiteTermObject> CheckProductReviewsFromOds(List<string> btkey, string orgId)
        {
            var list = new List<SiteTermObject>();
            //            
            //var user = ProfileService.Instance.GetUserById(userId);// CSObjectProxy.GetUserProfileForSearchResult();
            ////
            //if (user != null)
            {
                var organization = ProfileService.Instance.GetOrganizationById(orgId);// user.OrganizationEntity;
                //
                if (organization != null)
                {
                    var reviewType = organization.ReviewTypeList;

                    if (reviewType != null && reviewType.Length == 0) return list;
                    //
                    var productReviewsFromOds = ProductDAO.Instance.CheckProductReviewsFromODS(btkey.ToArray(), reviewType);
                    //
                    if (productReviewsFromOds != null)
                    {
                        list.AddRange(from item in productReviewsFromOds where item.Value select new SiteTermObject(item.Key, item.Value.ToString()));
                    }
                }
            }
            return list;
        }

        public static void GetPrimarySecondaryWareHouse(out string primaryWareHouse, out string secondaryWareHouse, Account account)
        {
            primaryWareHouse = "";
            secondaryWareHouse = "";

            if (account != null)
            {
                //var profileControllerForAdmin = AdministrationProfileController.Current;
                //profileControllerForAdmin.AccountRelated.PrimaryWarehouseNeeded = true;
                //profileControllerForAdmin.AccountRelated.SecondaryWarehouseNeeded = true;
                ////
                //var myAccount = profileControllerForAdmin.GetAccountById(account.Id);
                //var myAccount = account;

                primaryWareHouse = account.PrimaryWarehouseCode;
                secondaryWareHouse = account.SecondaryWarehouseCode;

                //if (account != null && account.SecondaryWarehouse != null)
                //{
                //    //secondaryWareHouse = ((Warehouse)account.SecondaryWarehouse.Target).Code;
                //    secondaryWareHouse = account.SecondaryWarehouse.Code;
                //}
            }
        }

        public static string CreateUpcLookupLink(string upc, string orgId, Organization orgEntity = null)
        {
           
            if (orgEntity == null)
                orgEntity = ProfileService.Instance.GetOrganizationById(orgId);
            
            if (orgEntity == null) 
                return null;

            if (orgEntity.AVProductLookupDeactivated == true)
                return ProductLookupLinkConstant.UPCDeactivated;
 
            string linkUPC = "";

            if (orgEntity.AVUseISBN == false || orgEntity.AVUseISBN == null)
            {
                if (String.IsNullOrEmpty(upc))
                    return string.Empty;

                string url = "";
                string index = "";
                string suff = "";

                if (orgEntity.AVPersonalProductURL != null)
                    url = orgEntity.AVPersonalProductURL.Trim();

                if (String.IsNullOrEmpty(url))
                    return string.Empty;

                if (orgEntity.AVProductLookupIndex != null)
                    index = orgEntity.AVProductLookupIndex.Trim();

                if (orgEntity.AVProductSuffixLookup != null)
                    suff = orgEntity.AVProductSuffixLookup.Trim();

                bool? useUpc14 = orgEntity.AVUseUPC14;
                if (useUpc14 == null)
                    useUpc14 = false;

                if (useUpc14 == true)
                {
                    if (upc.Length >= 14)
                    {
                        upc = upc.Substring(upc.Length - 14);
                    }
                    else
                    {
                        while (upc.Length < 14)
                        {
                            upc = "0" + upc;
                        }
                    }
                }
                else
                {
                    if (upc.Length >= 12)
                    {
                        upc = upc.Substring(upc.Length - 12);
                    }
                    else
                    {
                        while (upc.Length < 12)
                        {
                            upc = "0" + upc;
                        }
                    }
                }
                linkUPC = url + index + upc + suff;
            }
            else
            {
                linkUPC = ProductLookupLinkConstant.UPCUseISBN;
            }
            return linkUPC;
        }

        public static ISBNLookUpLink CreateIsbnLookupLink(string isbn, string isbn10, string orgId)
        {
            return CreateIsbnLookupLink(isbn, isbn10, orgId, null);
        }

        public static ISBNLookUpLink CreateIsbnLookupLink(string isbn, string isbn10, string orgId, Organization orgEntity = null)
        {
            var linkObject = new ISBNLookUpLink();
            //
            if (orgEntity == null)
                orgEntity = ProfileService.Instance.GetOrganizationById(orgId);
            //
            if (orgEntity == null)
                return null;
            //
            if (orgEntity.ProductLookupDeactivated == true)
                return null;

            string url = orgEntity.PersonalProductURL;
            //
            if (String.IsNullOrEmpty(url))
                return null;
            string index = orgEntity.ProductLookupIndex;
            string suff = orgEntity.ProductSuffixLookup;
            string isbnLookupCode = orgEntity.ISBNLookupCode;
            //
            if (String.IsNullOrEmpty(isbnLookupCode))
                return null;
            //
            string val = url + index + isbnLookupCode + suff;
            if (String.IsNullOrEmpty(val))
                return null;
            //            
            string linkIsbn = val.Replace(isbnLookupCode, isbn);
            string linkIsbn10 = val.Replace(isbnLookupCode, isbn10);
            switch (orgEntity.ISBNLinkDisplayed)
            {
                case CommonConstants.ISBN_Show10:
                    linkObject.ISBN10Link = linkIsbn10;
                    break;
                case CommonConstants.ISBN_Show_Both:
                    linkObject.ISBN13Link = linkIsbn;
                    linkObject.ISBN10Link = linkIsbn10;
                    break;
                default:
                    linkObject.ISBN13Link = linkIsbn;
                    break;
            }
            return linkObject;
        }

        public static FlagObject GetFlagObject(string userId)
        {
            var flagObject = new FlagObject();
            var user = ProfileService.Instance.GetUserById(userId); // CSObjectProxy.GetUserProfileForSearchResult();
            if (user == null)
                return flagObject;
            //
            var organization = ProfileService.Instance.GetOrganizationById(user.OrgId);// user.OrganizationEntity;
            if (organization == null)
                return flagObject;
            //
            var hasMuze = organization.EntertainmentProduct ?? false;
            var hasToc = organization.TableOfContents ?? false;
            //
            flagObject = new FlagObject(hasToc, hasMuze);
            return flagObject;
        }

        public static SearchResults ExecuteSearchForTypeAhead(BT.FSIS.Search search, string[] sortExpressions, int startRowIndex, int pageSize)
        {
            SearchResults results = null;
            try
            {
                if (search != null)
                {
                    search.SortElements = sortExpressions;
                    var searchExecutor = SearchHelper.CreateSearchExecutor(search, pageSize, true);
                    if (searchExecutor != null)
                    {
                        var searchResults = searchExecutor.ExecuteSearch(startRowIndex);
                        results = searchResults;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
            }
            return results;
        }

        //public static ProductSearchResults SearchAlternateFormatItemsAndSorting(List<string> btKeys)
        //{
        //    var searchAgr = SearchHelper.GetAlternateFormatItemsAndSorting(btKeys);
        //    return searchAgr == null ? null : Search(searchAgr);
        //}

        //#endregion

        //#region Private Methods

        public static ProductSearchResults SearchByTerms(List<string> terms, string scope,
            string userId,
            MarketType? marketType,
            string[] ESuppliers,
            bool simonSchusterEnabled,
            string countryCode,
            ProductStatus status = ProductStatus.A,
            bool includeProductFilter = false)
        {
            if (terms == null)
            {
                throw new ArgumentNullException("terms");
            }

            var group = new SearchExpressionGroup();
            group.AddSearchExpress(new SearchExpression
            {
                Operator = BooleanOperatorConstants.And,
                Scope = scope,
                Terms = string.Join("|", terms.ToArray())
            });

            if (status == ProductStatus.A)
            {
                group.AddSearchExpress(new SearchExpression
                {
                    Operator = BooleanOperatorConstants.And,
                    Scope = SearchFieldNameConstants.productstatus,
                    Terms = "A"
                });
            }

            if (includeProductFilter)
            {
                SearchHelper.CreateSearchExpressionForProductTypeFilter(group, userId);
            }
            group.AddSearchExpress(SearchHelper.ApplyEBookSearchExpression(marketType, ESuppliers));

            var argument = new SearchArguments();
            argument.SearchExpressionGroup.AddSearchExpress(group);
            argument.SortExpressions.Add(new SortExpression { SortField = string.Empty });
            argument.StartRowIndex = 0;
            argument.PageSize = terms.Count;

            return Search(argument, marketType, simonSchusterEnabled, countryCode, ESuppliers, notDeleted: false);
        }

        private static ProductSearchResults ExecuteSearch(BT.FSIS.Search search, string[] sortExpressions, int startRowIndex, int pageSize)
        {
            ProductSearchResults results = null;
            try
            {
                if (search != null)
                {
                    search.SortElements = sortExpressions;
                    var searchExecutor = SearchHelper.CreateSearchExecutor(search, pageSize);
                    if (searchExecutor != null)
                    {
                        //var str = getGroup((GroupOperator)search.SearchOperator);
                        var searchResults = searchExecutor.ExecuteSearch(startRowIndex);
                        results = new ProductSearchResults(searchResults);
                    }
                }
            }
            catch (Exception ex)
            {
                //SiteContext.WriteLog("ExecuteFastSearch", ex.ToString());
                Logger.WriteLog(ex, "ExecuteFastSearch");
            }
            return results;
        }

        //#endregion
    }
}
