using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.Common.Models;
using BT.TS360Constants;

namespace BT.TS360API.Common.DataAccess
{
    public class QuickSearchDAOManager
    {
        private static volatile QuickSearchDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        private QuickSearchDAOManager()
        { }

        public static QuickSearchDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new QuickSearchDAOManager();
                }

                return _instance;
            }
        }

        public string AddProductsToCart(string userId, string cartName, string userFolderId, string cartId, List<LineItem> lineItems, out string PermissionViolationMessage, out int totalAddingQuantity)
        {
            var basketSummaryId = QuickSearchDAO.Instance.AddProductsToCart(userId, cartName, userFolderId, cartId, lineItems, out PermissionViolationMessage, out totalAddingQuantity);

            // clear distributed cache
            //var cartManager = CartContext.Current.GetCartManagerForUser(userId);
            //cartManager.SetPrimaryCartChanged();

            CartFarmCacheHelper.SetExpiredPrimaryCart(userId);

            PermissionViolationMessage = "";
            return basketSummaryId;
        }

        public Carts GetActivesCart(string userId)
        {
            var ds = QuickSearchDAO.Instance.GetActiveCarts(CommonConstants.DEFAULT_NUMBER_OF_ACTIVE_CARTS, userId);
            return GetCartsFromDataSet(ds, userId);
        }

        private static Carts GetCartsFromDataSet(DataSet ds, string userId)
        {
            //Throw exception if no data returned from DAO
            if (ds == null ||
                ds.Tables[0].Rows.Count == 0)
                return null;

            var cartList = new List<Cart>();
            var strUserid = "";

            foreach (DataRow cartRow in ds.Tables[0].Rows)
            {
                var cart = GetCartFromDataRow(cartRow, userId);

                if (cart != null)
                {
                    cartList.Add(cart);

                    if (String.IsNullOrEmpty(strUserid))
                        strUserid = cart.UserId;
                }
            }

            var carts = new Carts(strUserid);
            carts.AddRange(cartList);

            return carts;
        }

        private static Cart GetCartFromDataRow(DataRow row, string userId)
        {
            var cartId = row["BasketSummaryID"].ToString();
            var cartName = row["BasketName"].ToString();

            if (String.IsNullOrEmpty(cartId) || String.IsNullOrEmpty(cartName))
                return null;

            var cart = new Cart(cartId, userId, string.Empty) { CartName = cartName };
            return cart;
        }
    }
}
