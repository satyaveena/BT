using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Business
{
    public class CartGridDAOManager
    {
        private static volatile CartGridDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        public static CartGridDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CartGridDAOManager();
                }

                return _instance;
            }
        }

        //private SiteContextObject _siteContext;
        //public SiteContextObject SiteContext
        //{
        //    get { return _siteContext; }
        //    set { if (_siteContext == null) _siteContext = value; }
        //}

        public void AddProductsToCart(List<ProductLineItem> products, Dictionary<string, List<CommonCartGridLine>> cartGridLines, string toCartId, string userId,
            out string PermissionViolationMessage, out int totalAddingQuantity)
        {
            //Refer to FS_Master_Add_NG32_Original_Entry
            List<SqlDataRecord> productsDS = DataConverter.ConvertProductsToMergeCartLineItem(products);
            List<SqlDataRecord> gridLines = DataConverter.ConvertCartGridLinesToDataSet(cartGridLines);
            CartDAO.Instance.AddProductToCart(productsDS, toCartId, gridLines, userId, out PermissionViolationMessage, out totalAddingQuantity);
        }
    }
}
