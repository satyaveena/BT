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

namespace BT.TS360API.MongoDB.DataAccess
{
    public class OrdersDAO : BaseMongoDAO
    {
        private static OrdersDAO _instance = null;
        private static readonly object SyncRoot = new Object();
        readonly IMongoDatabase _ordersDatabase;

        #region Singleton

        public static OrdersDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new OrdersDAO();
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
            get { return Constants.OrdersDatabaseName; }
        }

        public OrdersDAO()
        {
            var client = new MongoClient(ConnectionString);
            _ordersDatabase = client.GetDatabase(DatabaseName);
        }

        public List<BsonDocument> GetOrderDuplicates(OrdersDupCheckRequest request)
        {
            var bsonDocs = new List<BsonDocument>();

            // get DupCheck collection
            var collection = _ordersDatabase.GetCollection<BsonDocument>(Constants.DupeCheckCollectionName);
            var filterBuilder = Builders<BsonDocument>.Filter;

            // filter by BTKeys and OrgId
            var filter = filterBuilder.In(FieldNames.BTKey, request.BTKeys) &
             filterBuilder.Eq("OrganizationID", request.OrgId);

            // check type is MyAccounts
            if (StringHelper.EqualsIgnoreCase(request.OrderCheckType, DefaultDuplicateOrders.MyAccounts))
            {
                var orderFilter = filterBuilder.In("Accounts.ShipToAccountNumber", request.UserAccounts);
                if (!string.IsNullOrEmpty(request.BasketId) &&
                 (StringHelper.EqualsIgnoreCase(request.OrderCheckType, DefaultDuplicateOrders.MyAccounts)
                  || StringHelper.EqualsIgnoreCase(request.OrderCheckType, DefaultDuplicateOrders.AllAccounts)))
                {
                    orderFilter = filterBuilder.ElemMatch("Accounts", filterBuilder.And(filterBuilder.In("ShipToAccountNumber", request.UserAccounts), filterBuilder.Exists("OrderSummaries.0"), 
                        filterBuilder.ElemMatch("OrderSummaries", filterBuilder.Ne("BasketSummaryID", request.BasketId))));
                }

                if (StringHelper.EqualsIgnoreCase(request.DownloadCheckType, "IncludeWOrders"))
                {
                    filter = filter & filterBuilder.Or(orderFilter, GetDownloadedCartFilter(request));
                }
                else
                {
                    filter = filter & orderFilter;
                }
            }
            else if (StringHelper.EqualsIgnoreCase(request.OrderCheckType, DefaultDuplicateOrders.AllAccounts))
            {
                var orderFilter = filterBuilder.Exists("Accounts.0");
                if (!string.IsNullOrEmpty(request.BasketId) &&
                 (StringHelper.EqualsIgnoreCase(request.OrderCheckType, DefaultDuplicateOrders.MyAccounts)
                  || StringHelper.EqualsIgnoreCase(request.OrderCheckType, DefaultDuplicateOrders.AllAccounts)))
                {
                    orderFilter = orderFilter & filterBuilder.Exists("Accounts.OrderSummaries.0")
                                & filterBuilder.ElemMatch("Accounts.OrderSummaries", filterBuilder.Ne("BasketSummaryID", request.BasketId));
                }
                if (StringHelper.EqualsIgnoreCase(request.DownloadCheckType, "IncludeWOrders"))
                {

                        filter = filter & filterBuilder.Or(orderFilter, GetDownloadedCartFilter(request));
                }
                else
                {
                    filter = filter & orderFilter;
                }
            }
            else
            {
                filter = filter & GetDownloadedCartFilter(request);
            }

            var projection = Builders<BsonDocument>.Projection.Include(FieldNames.BTKey);

            int retries = base.maxRetries;
            while (retries > 0)
            {
                try
                {
                    bsonDocs = collection.Find(filter).Project(projection).ToList();
                    break;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw;
                    }
                }
            }

            return bsonDocs;
        }

        private FilterDefinition<BsonDocument> GetDownloadedCartFilter(OrdersDupCheckRequest request)
        {
            
            FilterDefinition<BsonDocument> filter;
            if (string.Equals(request.CartCheckType, "AllCarts", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(request.BasketId))
                {
                    filter = Builders<BsonDocument>.Filter.Exists("DownloadedBasketSummary.0");
                }
                else
                {
                    filter = Builders<BsonDocument>.Filter.ElemMatch("DownloadedBasketSummary", Builders<BsonDocument>.Filter.Ne("BasketSummaryID", request.BasketId));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(request.BasketId))
                {
                    filter = Builders<BsonDocument>.Filter.Eq("DownloadedBasketSummary.BasketOwnerID", request.UserId);
                }
                else
                {
                    filter = Builders<BsonDocument>.Filter.ElemMatch("DownloadedBasketSummary", Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("BasketOwnerID", request.UserId), Builders<BsonDocument>.Filter.Ne("BasketSummaryID", request.BasketId)));
                }
            }
            return filter;
        }
    }
}
