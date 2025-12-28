using BT.TS360API.Cache;
using BT.TS360API.Common.Helpers;
using System;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using System.Collections.Generic;

namespace BT.TS360API.Common.CartFramework
{
    static internal class CartCacheManager
    {
        #region Constants

        //internal const string PRIMARY_CART_CACHE_KEY_SUFFIX = "PRIMARY_CART";
        internal const string TOP_NEWEST_CART_CACHE_KEY_SUFFIX = "TOP_NEWEST_CART";
        internal const string CART_CACHE_KEY_SUFFIX = "CART";
        internal const string CART_FOLDER_LIST_KEY_SUFFIX = "CART_FOLDER_LIST";
        internal const string CART_FOLDER_KEY_SUFFIX = "CART_FOLDER";
        internal const string CART_ORGANIZATION_PERMISSION_KEY_SUFFIX = "CART_ORGANIZATION_PERMISSION";
        internal const string CART_USER_PERMISSION_KEY_SUFFIX = "CART_USER_PERMISSION";
        #endregion

        /// <summary>
        /// Set expired for specified user's primary cart.
        /// </summary>
        /// <param name="userId"></param>
        internal static void SetPrimaryCartCacheExpired(string userId)
        {
            //VelocityCacheManager.SetExpired(GetPrimaryCartCacheKey(userId));
            CartFarmCacheHelper.SetExpiredPrimaryCart(userId);
        }

        internal static void SetTopNewestCartCacheExpired(string userId)
        {
            CachingController.Instance.SetExpired(GetTopNewestCartCacheKey(userId));
        }

        internal static void SetCartCacheExpired(string cartId)
        {
            CachingController.Instance.SetExpired(GetCartCacheKey(cartId));
        }

        /// <summary>
        /// Get Cart Folders From Cache
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal static List<CartFolder> GetCartFoldersFromCache(string userId)
        {
            // TODO: apply Farm Cache at UI side
            return CachingController.Instance.Read(GetCartFolderListCacheKey(userId)) as List<CartFolder>;
        }

        /// <summary>
        /// Add Cart Folders To Cache
        /// </summary>
        /// <param name="cartFolders"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal static void AddCartFoldersToCache(List<CartFolder> cartFolders, string userId)
        {
            // TODO: apply Farm Cache at UI side
            CachingController.Instance.Write(GetCartFolderListCacheKey(userId), cartFolders);
        }

        private static string GetCartFolderListCacheKey(string userId)
        {
            return String.Format("{0}_{1}_{2}", CacheKeyConstant.CART_MANAGEMENT_CACHE_KEY_PREFIX,
                                 CART_FOLDER_LIST_KEY_SUFFIX, userId);
        }

        private static string GetTopNewestCartCacheKey(string userId)
        {
            return String.Format("{0}_{1}_{2}", CacheKeyConstant.CART_MANAGEMENT_CACHE_KEY_PREFIX,
                                 TOP_NEWEST_CART_CACHE_KEY_SUFFIX, userId);
        }

        private static string GetCartCacheKey(string cartID)
        {
            return string.Format("{0}_{1}_{2}", CacheKeyConstant.CART_MANAGEMENT_CACHE_KEY_PREFIX, CART_CACHE_KEY_SUFFIX, cartID);
        }
    }
}
