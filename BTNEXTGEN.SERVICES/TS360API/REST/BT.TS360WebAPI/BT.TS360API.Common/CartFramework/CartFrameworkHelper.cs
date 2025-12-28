using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Pricing;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using AppSettings = BT.TS360API.Common.Configrations.AppSettings;
using BT.TS360API.Common.DataAccess;

namespace BT.TS360API.Common.CartFramework
{
    public class CartFrameworkHelper
    {
        //private static readonly Dictionary<int, string> CartListSortByTable = new Dictionary<int, string>()
        //                                                         {
        //                                                             {0, "BasketName"},
        //                                                             {1, "Status"},
        //                                                             {2, "LastUpdate"},
        //                                                             {3, "CartTotal"},
        //                                                             {4, "Quantity"},
        //                                                             {5, "CartOwner"},
        //                                                         };

        private static readonly Dictionary<int, string> LineItemListSortByTable = new Dictionary<int, string>()
                                                                 {
                                                                     {0, "Artist"},
                                                                     {1, "Title"},
                                                                     {2, "Popularity"},
                                                                     {3, "Author"},
                                                                     {4, "Publisher"},
                                                                     {5, "BasketOrder"},
                                                                     {6, "Publish/Release Date"},
                                                                     {7, "Quantity"},
                                                                     {8, "ProductFormat"},
                                                                     {9, "ListPrice"},
                                                                     {10, "ISBN"},
                                                                     {11, "lcclassauthor"},
                                                                     {12, "lcclassartist"},
                                                                     {13, "deweyauthor"},
                                                                     {14, "deweyartist"},
                                                                     {15, "ESPOverallScore"},
                                                                     {16, "ESPBisacScore"}
                                                                 };

        //private static List<string> GetBtKeysFromLineItemsInBasket(IEnumerable<LineItem> lineItems)
        //{
        //    return lineItems.Select(lineItem => lineItem.BTKey).ToList();
        //}

        //public static bool IsAllowAddToCart(List<LineItem> lineItems, List<string> listBTKeys)
        //{
        //    if (listBTKeys.Count > MaxLinesPerCart)
        //    {
        //        return false;
        //    }
        //    var existedLineItemIds = GetBtKeysFromLineItemsInBasket(lineItems);
        //    existedLineItemIds.AddRange(listBTKeys);
        //    existedLineItemIds = existedLineItemIds.Distinct().ToList();
        //    return existedLineItemIds.Count <= MaxLinesPerCart;
        //}

        //public static bool IsAllowAddToCartByItemId(List<LineItem> lineItems, List<string> lineItemIds)
        //{
        //    if (lineItemIds.Count > MaxLinesPerCart)
        //    {
        //        return false;
        //    }
        //    var existedLineItemIds = lineItems.Select(lineItem => lineItem.Id).ToList();
        //    existedLineItemIds.AddRange(lineItemIds);
        //    existedLineItemIds = existedLineItemIds.Distinct().ToList();
        //    return existedLineItemIds.Count <= MaxLinesPerCart;
        //}

        public static bool IsAllowAddToCart(CartManager cartManager, Cart cart, List<string> listBTKeys)
        {
            if (listBTKeys.Count > MaxLinesPerCart)
            {
                return false;
            }
            var newItemsCount = 0;
            var existingItemsCount = 0;
            cartManager.CheckBasketForTitles(cart.CartId, listBTKeys, null, out newItemsCount, out existingItemsCount);
            if (cart.LineItemCount + newItemsCount > MaxLinesPerCart)
            {
                return false;
            }
            return true;
        }

        //public static List<string> GenerateUniqueItem(int maxSize, int numberOfItemToGenerate)
        //{
        //    List<string> uniqueNumbers;
        //    bool isUnique;
        //    do
        //    {
        //        uniqueNumbers = new List<string>();
        //        for (int i = 0; i < numberOfItemToGenerate; i++)
        //        {
        //            var uniqueNumber = GenerateUniqueItem(maxSize);

        //            uniqueNumbers.Add(uniqueNumber);
        //        }
        //        var result = (from p in uniqueNumbers
        //                      select new
        //                      {
        //                          Length = uniqueNumbers.Count,
        //                          DistinctCount = uniqueNumbers.Distinct().Count()
        //                      }).Take(1).Single();
        //        isUnique = result.DistinctCount == result.Length;
        //    } while (!isUnique);
        //    return uniqueNumbers;
        //}

        //private static string GenerateUniqueItem(int maxSize)
        //{
        //    var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        //    var data = new byte[1];
        //    var crypto = new RNGCryptoServiceProvider();
        //    crypto.GetNonZeroBytes(data);
        //    data = new byte[maxSize];
        //    crypto.GetNonZeroBytes(data);
        //    var result = new StringBuilder(maxSize);
        //    foreach (var b in data)
        //    {
        //        result.Append(chars[b % (chars.Length - 1)]);
        //    }
        //    return result.ToString();
        //}

        public static int MaxLinesPerCart
        {
            get
            {
                int maxLinesPerCart = 200;
                //var maxLinesPerCartFromGlobalConfiguration = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.MAX_LINES_PER_CART_TITLE);
                var maxLinesPerCartFromGlobalConfiguration = AppSettings.MaxLinePerCartTitle;
                if (!string.IsNullOrEmpty(maxLinesPerCartFromGlobalConfiguration))
                {
                    Int32.TryParse(maxLinesPerCartFromGlobalConfiguration, out maxLinesPerCart);
                }
                return maxLinesPerCart;
            }
        }

        //internal static string ConvertToCartSortByColumn(int sortBy)
        //{
        //    if (CartListSortByTable.ContainsKey(sortBy))
        //    {
        //        return CartListSortByTable[sortBy];
        //    }
        //    throw new CartManagerException("Invalid sort by column");
        //}

        //public static bool CheckHomeDeliveryCartAccount(Cart cart)
        //{
        //    var homeDelivery = false;

        //    if (cart == null)
        //    {
        //        return false;
        //    }

        //    var cartAccounts = cart.CartAccounts;
        //    if (cartAccounts == null || cartAccounts.Count < 1)
        //    {
        //        return false;
        //    }
        //    var profileControllerForAdmin = AdministrationProfileController.Current;
        //    profileControllerForAdmin.UserProfilePropertiesToReturn.Add(UserProfile.PropertyName.OrganizationId);
        //    var cartOwner = profileControllerForAdmin.GetUserById(cart.CartOwnerId);
        //    if (cartOwner != null)
        //    {
        //        var organization = profileControllerForAdmin.GetOrganization(cartOwner.OrganizationId);
        //        if (organization != null && organization.WebMarketType == ((int)MarketType.Retail).ToString())
        //        {
        //            foreach (var cartAccount in cartAccounts)
        //            {
        //                if (cartAccount.AccountType == (int)AccountType.Book &&
        //                    !string.IsNullOrEmpty(cartAccount.ESupplierID))
        //                {
        //                    homeDelivery = cartAccount.IsHomeDelivery;
        //                }
        //                if (!homeDelivery && cartAccount.AccountType == (int)AccountType.Entertainment)
        //                {
        //                    homeDelivery = cartAccount.IsHomeDelivery;
        //                }
        //            }
        //        }
        //    }

        //    return homeDelivery;
        //}

        //public static bool CheckHomeDeliveryAccountInCart(Cart cart)
        //{
        //    var homeDelivery = false;

        //    if (cart == null)
        //    {
        //        return false;
        //    }

        //    var cartAccounts = cart.CartAccounts;
        //    if (cartAccounts == null || cartAccounts.Count < 1)
        //    {
        //        return false;
        //    }
        //    var profileControllerForAdmin = AdministrationProfileController.Current;
        //    profileControllerForAdmin.UserProfilePropertiesToReturn.Add(UserProfile.PropertyName.OrganizationId);

        //    var cartOwner = profileControllerForAdmin.GetUserById(cart.CartOwnerId);
        //    if (cartOwner != null)
        //    {
        //        var organization = profileControllerForAdmin.GetOrganization(cartOwner.OrganizationId);
        //        if (organization != null && organization.WebMarketType == ((int)MarketType.Retail).ToString())
        //        {
        //            foreach (var cartAccount in cartAccounts)
        //            {
        //                var acc = profileControllerForAdmin.GetAccountById(cartAccount.ERPAccountGUID);
        //                if (acc != null && acc.HomeDeliveryAccount.HasValue &&
        //                    acc.HomeDeliveryAccount.Value)
        //                {
        //                    if (cartAccount.AccountType == (int)AccountType.Book &&
        //                        !string.IsNullOrEmpty(cartAccount.ESupplierID))
        //                    {
        //                        homeDelivery = acc.HomeDeliveryAccount.Value;
        //                    }
        //                    if (!homeDelivery && cartAccount.AccountType == (int)AccountType.Entertainment)
        //                    {
        //                        homeDelivery = acc.HomeDeliveryAccount.Value;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return homeDelivery;
        //}

        public static string TransformSortOrderLineItem(int sortBy)
        {
            if (LineItemListSortByTable.ContainsKey(sortBy))
            {
                return LineItemListSortByTable[sortBy];
            }
            throw new CartManagerException("Invalid sort by column");
        }

        public static int GetSortKeyIndex(string sortByValue)
        {
            if (LineItemListSortByTable.ContainsValue(sortByValue))
            {
                return LineItemListSortByTable.FirstOrDefault(x => x.Value == sortByValue).Key;
            }
            return -1;
        }

        public static void CalculatePrice(string cartId, TargetingValues siteContext, bool isAsync = true)
        {
            int batchWaitingTime = GetBatchWaitingTimeConfig();

            var pricingController = new PricingController();
            pricingController.CalculatePrice(cartId, batchWaitingTime, siteContext, isAsync);
        }
        public static void CalculatePrice(string cartId, bool isAsync = true)
        {
            //Logger.DebugTraceLog(ExceptionCategory.Pricing, "Cart Framework calls to Pricing to re-price");
            var batchWaitingTime = GetBatchWaitingTimeConfig();

            var pricingController = new PricingController();
            pricingController.CalculatePrice(cartId, batchWaitingTime, isAsync);
        }
        public static void CalculateCartPriceInBackground(string cartId, TargetingValues siteContext)
        {
            int batchWaitingTime = GetBatchWaitingTimeConfig();

            var pc = new PricingController();
            pc.BackgroundRepriceWithSiteContext(batchWaitingTime, siteContext, cartId);
        }

        public static int GetBatchWaitingTimeConfig()
        {
            var batchWaitingTime = CartFrameworkConstants.PricingBatchWaitingTime;
            try
            {
                var sbatchWaitingTime = AppSettings.PricingBatchWaitingTime;
                int.TryParse(sbatchWaitingTime, out batchWaitingTime);
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }

            return batchWaitingTime;
        }

        public static bool IsPricing(string cartId, string userId, bool needToReprice = false)
        {
            if (cartId == null) throw new CartManagerException(CartManagerException.CART_ID_NULL);
            if (string.IsNullOrEmpty(userId)) throw new CartManagerException(CartManagerException.USER_ID_NULL);

            var isPricing = CartDAOManager.IsCartPricing(cartId, userId);
            if (isPricing)
            {
                if (needToReprice)
                {
                    //this.WcfServiceCalculatePrice(cartId);
                    //this.NotifyCartLineChanged(cartId);
                }
                return true;
            }

            return false;
        }

        public static bool IsHomeDeliveryCart(List<CartAccount> cartAccounts)
        {
            if (cartAccounts == null || cartAccounts.Count < 1)
            {
                return false;
            }

            return cartAccounts.Any(cartAccount => cartAccount.IsHomeDelivery);
        }

        //public static void WcfServiceCalculatePrice(string basketSummaryID)
        //{
        //    try
        //    {
        //        var serviceClient = new PricingServiceClient();
        //        serviceClient.CalculatePriceForBasket(basketSummaryID);
        //    }
        //    catch (Exception exception)
        //    {
        //        Logger.LogException(exception);
        //    }
        //}

        public static bool IsGaleAccountInCart(Cart cart, string galeESupplier)
        {
            if (cart == null) return false;
            var accounts = cart.CartAccounts;
            if (accounts.Any(accountSummary => accountSummary.ESupplierID == galeESupplier))
            {
                return true;
            }
            return false;
        }

        //public static string GetAccountId(List<CartAccount> cartAccounts, AccountType accountType)
        //{
        //    if (cartAccounts != null && cartAccounts.Count > 0)
        //    {
        //        foreach (var cartAccount in cartAccounts)
        //        {
        //            if (cartAccount.AccountType == (int)accountType)
        //            {
        //                return cartAccount.ERPAccountGUID;
        //            }
        //        }
        //    }
        //    return string.Empty;
        //}

        //internal static bool ConvertSortByToBool(int sortBy)
        //{
        //    if (sortBy == 1)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
