using BT.TS360API.MongoDB.Common;
using BT.TS360API.ServiceContracts;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BT.TS360Constants;
using BT.TS360API.ServiceContracts.Product;

namespace BT.TS360API.MongoDB.DataAccess
{
    public class ProductsDAO : BaseMongoDAO
    {
        private static ProductsDAO _instance = null;
        private static readonly object SyncRoot = new Object();
        readonly IMongoDatabase _productsDatabase;
        readonly IMongoCollection<BsonDocument> _productsCollection;

        #region Singleton

        public static ProductsDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProductsDAO();
                }

                return _instance;
            }
        }

        #endregion
        public override string ConnectionString
        {
            get { return Common.AppSettings.MongoDBConnectionString; }
        }

        public override string DatabaseName
        {
            get { return Constants.ProductsDatabaseName; }
        }

        public ProductsDAO()
        {
            var client = new MongoClient(ConnectionString);
            _productsDatabase = client.GetDatabase(DatabaseName);
            _productsCollection = _productsDatabase.GetCollection<BsonDocument>(Constants.ProductsCollectionName);
        }

        public async Task<DiversityProductsResponse> GetDiversityClassificationByBTKeys(DiversityProductsRequest request)
        {
            int retryWaitTime = base.retryWaitTime;
            int retries = base.maxRetries;

            var filter = Builders<BsonDocument>.Filter.In("_id", request.BTKeys)
                        & Builders<BsonDocument>.Filter.Exists("DiversityClassification")
                        & Builders<BsonDocument>.Filter.Eq("DiversityClassification.RegionCode", "NA");

            var projection = Builders<BsonDocument>.Projection.Include("_id").Include("DiversityClassification.ClassificationName");

            var diversityProductsResponse = new DiversityProductsResponse();
            diversityProductsResponse.DiversityProducts = new List<DiversityProduct>();

            var diversityProductsList = new List<BsonDocument>();
            while (retries > 0)
            {
                try
                {
                    diversityProductsList = await _productsCollection.Find<BsonDocument>(filter).Project(projection).ToListAsync();
                
                    foreach (BsonDocument bsonDiversityProduct in diversityProductsList)
                    {
                        diversityProductsResponse.DiversityProducts.Add(GetDiversityProductsResponse(bsonDiversityProduct));
                    }

                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw;
                    }
                }
            }

            return diversityProductsResponse;

        
        }

        private DiversityProduct GetDiversityProductsResponse(BsonDocument diversityProduct)
        {
            var diversityProductItem = new DiversityProduct();


            if (diversityProduct.Contains("_id"))
                diversityProductItem.BTKey = diversityProduct["_id"].ToString();

            if (diversityProduct.Contains("DiversityClassification"))
            {
                var arrClassificationName = diversityProduct["DiversityClassification"] as BsonArray;
                if (arrClassificationName != null)
                {
                    diversityProductItem.ClassificationName = arrClassificationName.Select(a => a[0].ToString()).ToList();
                }
            }

            return diversityProductItem;
        }

    }
}
