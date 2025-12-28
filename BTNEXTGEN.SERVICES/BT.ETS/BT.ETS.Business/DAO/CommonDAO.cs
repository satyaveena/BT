
using BT.ETS.Business.Constants;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Models;
using BT.TS360API.ServiceContracts;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using etsModels = BT.ETS.Business.Models;

namespace BT.ETS.Business.DAO
{
    public class CommonDAO : BaseMongoDAO
    {
        private static CommonDAO _instance = null;
        private static readonly object SyncRoot = new Object();
        readonly IMongoDatabase _commonDatabase, _ordersDatabase;
        IMongoCollection<ETSQueueItem> _etsQueueCollection;
        IMongoCollection<LineItemInput> _etsQueueItemsCollection;

        string retryWaitTimes = AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.MongoRetryValues);
        List<string> retryWaitTimesList;

        #region Singleton

        public static CommonDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CommonDAO();
                }

                return _instance;
            }
        }

        #endregion

        public CommonDAO()
        {
            retryWaitTimesList = retryWaitTimes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var client = new MongoClient(AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.MongoDBConnectionString));
            _commonDatabase = client.GetDatabase(ApplicationConstants.MONGO_COMMON_DB_NAME);
            _ordersDatabase = client.GetDatabase(ApplicationConstants.MONGO_ORDERS_DB_Name);
            
            _etsQueueCollection = _commonDatabase.GetCollection<ETSQueueItem>(ApplicationConstants.MONGO_ETS_QUEUE_COLLECTION_NAME);
            _etsQueueItemsCollection = _commonDatabase.GetCollection<LineItemInput>(ApplicationConstants.MONGO_ETS_QUEUE_ITEMS_COLLECTION_NAME);
        }

        public async Task InsertETSQueueItem(ETSQueueItem etsQueueItem)
        {
            int retryCount = 0;
            int maxRetries = retryWaitTimesList.Count;     
            while (retryCount < maxRetries)
            {
                try
                {
                    await _etsQueueCollection.InsertOneAsync(etsQueueItem);
                    break;
                }
                catch (Exception)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }

                }
            }
        }

        public async Task InsertETSQueueLineItems(LineItemInput etsQueueItems)
        {
            int retryCount = 0;
            int maxRetries = retryWaitTimesList.Count;
            while (retryCount < maxRetries)
            {
                try
                {
                    await _etsQueueItemsCollection.InsertOneAsync(etsQueueItems);
                    break;
                }
                catch (Exception)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }

                }
            }
        }

        public async Task<List<ETSQueueItem>> GetQueueItems(int maxItemNumber)
        {
            var filter = Builders<ETSQueueItem>.Filter.Eq("InProcessState", (int)QueueProcessState.New);
            var sort = Builders<ETSQueueItem>.Sort.Descending("Priority");

            var queueIdBsonDoucmentList = new List<ETSQueueItem>();
            int maxRetries = retryWaitTimesList.Count;     
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    queueIdBsonDoucmentList = await _etsQueueCollection.Find<ETSQueueItem>(filter).Sort(sort).Limit(maxItemNumber).ToListAsync();
                    break;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }

                }
            }
            return queueIdBsonDoucmentList;
        }

        // GetQueueItemsCount
        public async Task<int> GetQueueItemCount(int queueProcessState)
        {
            var filter = Builders<ETSQueueItem>.Filter.Eq("InProcessState", queueProcessState);
        
            long count = 0;
            
            int maxRetries = retryWaitTimesList.Count;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    count = await _etsQueueCollection.Find<ETSQueueItem>(filter).CountDocumentsAsync();
                    break;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }

                }
            }
            return Convert.ToInt32(count);
        }

        public async Task<List<LineItemInput>> GetQueueLineItems(ObjectId requestId)
        {
            var filter = Builders<LineItemInput>.Filter.Eq("RequestID", requestId);

            var queueIdBsonDoucmentList = new List<LineItemInput>();
            int maxRetries = retryWaitTimesList.Count;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    queueIdBsonDoucmentList = await _etsQueueItemsCollection.Find<LineItemInput>(filter).ToListAsync();
                    break;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }

                }
            }
            return queueIdBsonDoucmentList;
        }
        public async Task<bool> UpdateETSQueueStatus(ObjectId id, int fromState, int newState)
        {
            int retryCount = 0;
            int maxRetries = retryWaitTimesList.Count;     
            while (retryCount < maxRetries)
            {
                try
                {
                    var filter = Builders<ETSQueueItem>.Filter.Eq("_id", id);
                                //& Builders<ETSQueueItem>.Filter.Eq("InProcessState", fromState);

                    var update = Builders<ETSQueueItem>.Update
                        .Set("FootprintInformation.UpdatedBy", "ETS Service")
                        .Set("FootprintInformation.UpdatedDate", DateTime.Now)
                        .Set("InProcessState", newState);

                    var updateOptions = new UpdateOptions();
                    updateOptions.IsUpsert = true;

                    var result = await _etsQueueCollection.UpdateOneAsync(filter, update, updateOptions);

                    break;
                }
                catch (Exception)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }

                }
            }
            return true;
        }

        public async Task<bool> UpdateETSQueueItem<T>(ObjectId id, string responseType, T response)
        {
            int retryCount = 0;
            int maxRetries = retryWaitTimesList.Count;     
            while (retryCount < maxRetries)
            {
                try
                {
                    var filter = Builders<ETSQueueItem>.Filter.Eq("_id", id);

                    var update = Builders<ETSQueueItem>.Update
                        .Set("FootprintInformation.UpdatedBy", "ETS Service")
                        .Set("FootprintInformation.UpdatedDate", DateTime.Now)
                        .Set(responseType, response);

                    var updateOptions = new UpdateOptions();
                    updateOptions.IsUpsert = true;

                    var result = await _etsQueueCollection.UpdateOneAsync(filter, update, updateOptions);

                    break;
                }
                catch (Exception)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }
                }
            }
            return true;
        }

        public async Task<bool> SetETSRequestStatus(ObjectId id, string statusCode, string statusMessage)
        {
            int retryCount = 0;
            int maxRetries = retryWaitTimesList.Count;     
            while (retryCount < maxRetries)
            {
                try
                {
                    var filter = Builders<ETSQueueItem>.Filter.Eq("_id", id);

                    var update = Builders<ETSQueueItem>.Update
                        .Set("ETSRequestStatusMessage", statusMessage)
                        .Set("ETSRequestStatusCode", statusCode);

                    var updateOptions = new UpdateOptions();
                    updateOptions.IsUpsert = true;

                    var result = await _etsQueueCollection.UpdateOneAsync(filter, update, updateOptions);

                    break;
                }
                catch (Exception)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }
                }
            }
            return true;
        }

        public async Task<bool> SetETSResponseStatus(ObjectId id, string statusCode, string statusMessage)
        {
            int retryCount = 0;
            int maxRetries = retryWaitTimesList.Count;     
            while (retryCount < maxRetries)
            {
                try
                {
                    var filter = Builders<ETSQueueItem>.Filter.Eq("_id", id);

                    var update = Builders<ETSQueueItem>.Update
                        .Set("ETSResponseStatusMessage", statusMessage)
                        .Set("ETSResponseStatusCode", statusCode);

                    var updateOptions = new UpdateOptions();
                    updateOptions.IsUpsert = true;

                    var result = await _etsQueueCollection.UpdateOneAsync(filter, update, updateOptions);

                    break;
                }
                catch (Exception)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }
                }
            }
            return true;
        }

        public async Task<bool> SetETSQueueItemRequestStatusFailed(ObjectId id, string errorCode, string errorMessage)
        {
            int retryCount = 0;
            int maxRetries = retryWaitTimesList.Count;     
            while (retryCount < maxRetries)
            {
                try
                {
                    var filter = Builders<ETSQueueItem>.Filter.Eq("_id", id)
                                & Builders<ETSQueueItem>.Filter.Eq("InProcessState", (int)QueueProcessState.InProcess);

                    var update = Builders<ETSQueueItem>.Update
                        .Set("FootprintInformation.UpdatedBy", "ETS Service")
                        .Set("FootprintInformation.UpdatedDate", DateTime.Now)
                        .Set("ETSRequestStatusCode", errorCode)
                        .Set("ETSRequestStatusMessage", errorMessage)
                        .Set("InProcessState", (int)QueueProcessState.Failed);

                    var updateOptions = new UpdateOptions();
                    updateOptions.IsUpsert = true;

                    var result = await _etsQueueCollection.UpdateOneAsync(filter, update, updateOptions);

                    break;
                }
                catch (Exception)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }
                }
            }
            return true;
        }

        public async Task<List<BsonDocument>> GetOrderDuplicates(etsModels.OrdersDupCheckRequest request)
        {
            var bsonDocs = new List<BsonDocument>();
            int retryCount = 0;
            int maxRetries = retryWaitTimesList.Count; 
            // get DupCheck collection
            var collection = _ordersDatabase.GetCollection<BsonDocument>(ApplicationConstants.MONGO_DUP_CHECK_COLLECTION_NAME);
            var filterBuilder = Builders<BsonDocument>.Filter;

            // filter by BTKeys and OrgId
            var filter = filterBuilder.In("BTKey", request.BTKeys) &
                         filterBuilder.Eq("OrganizationID", request.OrgId);

            // check type is MyAccounts
            if (string.Equals(request.OrderCheckType, "myaccounts", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(request.DownloadedCheckType, "includeworders", StringComparison.OrdinalIgnoreCase))
                {
                    filter = filter & filterBuilder.Or(filterBuilder.In("Accounts.ShipToAccountNumber", request.UserAccounts), GetDownloadedCartFilter(request));
                }
                else
                {
                    filter = filter & filterBuilder.In("Accounts.ShipToAccountNumber", request.UserAccounts);
                }
            }
            else if (string.Equals(request.OrderCheckType, "allaccounts", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(request.DownloadedCheckType, "includeworders", StringComparison.OrdinalIgnoreCase))
                {
                    filter = filter & filterBuilder.Or(filterBuilder.Exists("Accounts.0"), GetDownloadedCartFilter(request));
                }
                else
                {
                    filter = filter & filterBuilder.Exists("Accounts.0");
                }
            }
            else
            {
                filter = filter & GetDownloadedCartFilter(request);
            }

            var projection = Builders<BsonDocument>.Projection.Include("BTKey");

            while (retryCount < maxRetries)
            {
                try
                {
                    bsonDocs = await collection.Find(filter).Project(projection).ToListAsync();
                    break;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }
                }
            }

            return bsonDocs;
        }
        private FilterDefinition<BsonDocument> GetDownloadedCartFilter(etsModels.OrdersDupCheckRequest request)
        {

            FilterDefinition<BsonDocument> filter;
            if (string.Equals(request.CartCheckType, "allcarts", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrEmpty(request.BasketId))
                {
                    filter = Builders<BsonDocument>.Filter.Exists("DownloadedBasketSummary.0");
                }
                else
                {
                    filter = Builders<BsonDocument>.Filter.ElemMatch("BasketSummary", Builders<BsonDocument>.Filter.Ne("BasketSummaryID", request.BasketId));
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
                    filter = Builders<BsonDocument>.Filter.ElemMatch("BasketSummary", Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("BasketOwnerID", request.UserId), Builders<BsonDocument>.Filter.Ne("BasketSummaryID", request.BasketId)));
                }
            }
            return filter;
        }
    }
}
