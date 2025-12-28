using BT.TS360API.MongoDB.Common;
using BT.TS360API.MongoDB.Contracts;
using BT.TS360API.MongoDB.DataAccess;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Order;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.ServiceContracts.Product;


namespace BT.TS360API.MongoDB.DataAccess
{
    public class ProductsDAOManager
    {
        #region Private Member

        private static volatile ProductsDAOManager _instance;
        private static readonly object SyncRoot = new Object();
        public static ProductsDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProductsDAOManager();
                }

                return _instance;
            }
        }

        #endregion



        public async Task<DiversityProductsResponse> GetDiversityClassificationByBTKeys(DiversityProductsRequest request)
        {

            var getResult = await ProductsDAO.Instance.GetDiversityClassificationByBTKeys(request);

            return getResult;
        }
    }
}
