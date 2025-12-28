using BT.TS360API.Cache;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Data;

namespace BT.TS360API.Common.Helpers
{
    public static class CartFarmCacheHelper
    {

        #region Move wcf to api

        internal static List<AccountSummary> GetAccountsSummary(string cardId)
        {
            return CartDAOManager.GetAccountsSummary(cardId);
        }

        #endregion

        public static Cart GetPrimaryCart(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            Cart primaryCart;
            DataSet primaryCartDS;

            // get cartId from cache
            string primaryCartId = GetPrimaryCartId(userId);

            if (!string.IsNullOrEmpty(primaryCartId))
            {
                // get cart DS from cache
                primaryCartDS = GetPrimaryCartDataSet(primaryCartId);

                // convert DS to Cart object
                primaryCart = CartDAOManager.GetCartFromDataSet(primaryCartDS);
            }
            else
            {
                // get cart DS from DB
                primaryCartDS = CartDAO.Instance.GetPrimaryCart(userId);

                // convert DS to Cart object
                primaryCart = CartDAOManager.GetCartFromDataSet(primaryCartDS);
                if (primaryCart != null)
                {
                    if (primaryCart.BTStatus == CartStatus.Deleted.ToString()
                        || primaryCart.BTStatus == CartStatus.Submitted.ToString()
                        || primaryCart.BTStatus == CartStatus.Ordered.ToString())
                    {
                        primaryCart = null;
                    }
                    else
                    {
                        // write cartId and dataset into cache
                        WritePrimaryCartToCache(userId, primaryCart.CartId, primaryCartDS);
                    }
                }
            }

            return primaryCart;
        }

        /// <summary>
        /// Get Primary Cart, not for mini cart.
        /// This will return the cart even it's status is Deleted or Submitted.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public static Cart GetPrimaryCartNotForMinicart(string userId, string cartId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(cartId))
                return null;

            // get cart DS from cache by cartId
            var primaryCartDS = GetPrimaryCartDataSet(cartId);

            // convert DS to Cart object
            var primaryCart = CartDAOManager.GetCartFromDataSet(primaryCartDS);

            if (primaryCart == null)
            {
                // get cart DS from DB
                primaryCartDS = CartDAO.Instance.GetPrimaryCart(userId);

                // convert DS to Cart object
                primaryCart = CartDAOManager.GetCartFromDataSet(primaryCartDS);

                if (primaryCart != null)
                {
                    // write cartId and dataset into cache
                    WritePrimaryCartToCache(userId, primaryCart.CartId, primaryCartDS);
                }
            }

            return primaryCart;
        }

        public static bool IsPrimaryCart(string cartId, string userId)
        {
            // get primary cart id from Farm cache
            string primaryCartId = GetPrimaryCartId(userId);

            if (CachingController.Instance.IsCacheAvailable == false)
            {
                // get primary cart from DB
                var primaryCart = CartDAOManager.Instance.GetPrimaryCart(userId);

                if (primaryCart == null) return false;

                return String.Compare(primaryCart.CartId, cartId, true) == 0;
            }

            return String.Compare(primaryCartId, cartId, true) == 0;
        }

        public static void SetExpiredPrimaryCart(string userId, string cartId = null)
        {
            if (string.IsNullOrEmpty(userId))
                return;

            if (string.IsNullOrEmpty(cartId))
                cartId = GetPrimaryCartId(userId);

            // Set expired cartId from cache by userId
            var cacheKey = GetUserPrimaryCartCacheKey(userId);
            CachingController.Instance.SetExpired(cacheKey);

            // Set expired dataset from cache by userId
            cacheKey = GetCartCacheKey(cartId);
            CachingController.Instance.SetExpired(cacheKey);
        }

        private static void WritePrimaryCartToCache(string userId, string primaryCartId, DataSet primaryCartDS)
        {
            // make sure cache expired
            SetExpiredPrimaryCart(userId, primaryCartId);

            // write primary cartID into cache by userId
            var cacheKey = GetUserPrimaryCartCacheKey(userId);
            CachingController.Instance.Write(cacheKey, primaryCartId);

            // write primary cart dataset into cache by primaryCartId
            cacheKey = GetCartCacheKey(primaryCartId);
            CachingController.Instance.Write(cacheKey, primaryCartDS);
        }

        public static string GetPrimaryCartId(string userId)
        {
            // get cartId from cache
            var cacheKey = GetUserPrimaryCartCacheKey(userId);
            return CachingController.Instance.Read(cacheKey) as string;
        }

        private static DataSet GetPrimaryCartDataSet(string primaryCartId)
        {
            // get cart DS from cache by cartId
            var cacheKey = GetCartCacheKey(primaryCartId);
            return CachingController.Instance.Read(cacheKey) as DataSet;
        }

        private static string GetCartCacheKey(string cartId)
        {
            return string.Format("{0}_CartId_{1}", CacheKeyConstant.CART_MANAGEMENT_CACHE_KEY_PREFIX, cartId);
        }

        private static string GetUserPrimaryCartCacheKey(string userId)
        {
            return string.Format("{0}_PrimaryCartUserId_{1}", CacheKeyConstant.CART_MANAGEMENT_CACHE_KEY_PREFIX, userId);
        }

        internal static void SetTopNewestCartCacheExpired(string userId)
        {
            CachingController.Instance.SetExpired(GetTopNewestCartCacheKey(userId));
        }
        private static string GetTopNewestCartCacheKey(string userId)
        {
            return String.Format("{0}_{1}_{2}", CacheKeyConstant.CART_MANAGEMENT_CACHE_KEY_PREFIX,
                                 CacheKeyConstant.TOP_NEWEST_CART_CACHE_KEY_SUFFIX, userId);
        }
    }
}
