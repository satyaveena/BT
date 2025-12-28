using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using BT.ILSQueue.Business.Models;
using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.Business.Helpers;

using BT.TS360.NoSQL.Data;

namespace BT.ILSQueue.Business.DAO
{
    public class CommonDAO: BaseMongoDAO
    {
        private static CommonDAO _instance = null;
        private static readonly object SyncRoot = new Object();

        readonly IMongoCollection<ILSAPIRequestLog> _ILSAPIRequestLog;

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
            var client = new MongoClient(ConnectionString);
            var iCommonDatabase = client.GetDatabase(ApplicationConstants.CommonDatabaseName);

            _ILSAPIRequestLog = iCommonDatabase.GetCollection<ILSAPIRequestLog>(ApplicationConstants.ILSAPIRequestCollectionName);
        }

        public async Task<ObjectId> AddILSAPIRequestQueueLog(ILSAPIRequestLog queueLog)
        {
            var now = DateTime.Now;
            try
            {
                queueLog.ILSQueueID = ObjectId.GenerateNewId();

                queueLog.FootprintInformation = new FootprintInformation();

                queueLog.FootprintInformation.CreatedDate = now;
                queueLog.FootprintInformation.CreatedBy = "ILSQueueService";
                queueLog.FootprintInformation.UpdatedDate = now;
                queueLog.FootprintInformation.UpdatedBy = "ILSQueueService";

                int retryWaitTime = RetryWaitTime;
                int retries = Retries;

                while (retries > 0)
                {
                    try
                    {
                        await _ILSAPIRequestLog.InsertOneAsync(queueLog);
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

            }
            catch (Exception)
            {
                throw;
            }

            return queueLog.ILSQueueID;
        }

        public async Task<bool> UpdateILSValidationResponseLog(ObjectId id, PolarisPOResponse validationResponse, string processingStatus)
        {
            int retryWaitTime = RetryWaitTime;
            int retries = Retries;

            while (retries > 0)
            {
                try
                {
                    var filter = Builders<ILSAPIRequestLog>.Filter.Eq("_id", id);

                    var update = Builders<ILSAPIRequestLog>.Update
                        .Set("FootprintInformation.UpdatedBy", "ILSQueueService")
                        .Set("FootprintInformation.UpdatedDate", DateTime.Now)
                        .Set("ProcessingStatus", processingStatus)
                        .Set("ValidationResponse", validationResponse);

                    var updateOptions = new UpdateOptions();
                    updateOptions.IsUpsert = true;

                    var result = await _ILSAPIRequestLog.UpdateOneAsync(filter, update, updateOptions);

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
            return true;


        }

        public async Task<bool> UpdateILSOrderRequestLog(ObjectId id, ILSAPIRequestLog queueLog)
        {
            int retryWaitTime = RetryWaitTime;
            int retries = Retries;

            while (retries > 0)
            {
                try
                {
                    var filter = Builders<ILSAPIRequestLog>.Filter.Eq("_id", id);

                    var update = Builders<ILSAPIRequestLog>.Update
                        .Set("FootprintInformation.UpdatedBy", "ILSQueueService")
                        .Set("FootprintInformation.UpdatedDate", DateTime.Now)
                        .Set("ProcessingStatus", ILSProcessingStatus.OrderingRequest)
                        .Set("PONumber", queueLog.PONumber)
                        .Set("PostbackURL", queueLog.PostbackURL)
                        .Set("OrderRequest", queueLog.OrderRequest);


                    var updateOptions = new UpdateOptions();
                    updateOptions.IsUpsert = true;

                    var result = await _ILSAPIRequestLog.UpdateOneAsync(filter, update, updateOptions);

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
            return true;
        }

        public async Task<bool> UpdateILSOrderResponseLog(ObjectId id, PolarisPOResponse orderResponse, string processingStatus)
        {
            int retryWaitTime = RetryWaitTime;
            int retries = Retries;

            while (retries > 0)
            {
                try
                {
                    var filter = Builders<ILSAPIRequestLog>.Filter.Eq("_id", id);

                    var update = Builders<ILSAPIRequestLog>.Update
                        .Set("FootprintInformation.UpdatedBy", "ILSQueueService")
                        .Set("FootprintInformation.UpdatedDate", DateTime.Now)
                        .Set("ProcessingStatus", processingStatus)
                        .Set("OrderResponse", orderResponse);


                    var updateOptions = new UpdateOptions();
                    updateOptions.IsUpsert = true;

                    var result = await _ILSAPIRequestLog.UpdateOneAsync(filter, update, updateOptions);

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
            return true;

        }

        public async Task<bool> UpdateILSOrderResultLog(ObjectId id, PolarisOrderResult orderResult)
        {
            int retryWaitTime = RetryWaitTime;
            int retries = Retries;

            while (retries > 0)
            {
                try
                {
                    var filter = Builders<ILSAPIRequestLog>.Filter.Eq("_id", id);

                    var update = Builders<ILSAPIRequestLog>.Update
                        .Set("FootprintInformation.UpdatedBy", "ILSQueueService")
                        .Set("FootprintInformation.UpdatedDate", DateTime.Now)
                        .Set("ProcessingStatus", ILSProcessingStatus.OrderingResultResponse)
                        .Set("OrderResult", orderResult);


                    var updateOptions = new UpdateOptions();
                    updateOptions.IsUpsert = true;

                    var result = await _ILSAPIRequestLog.UpdateOneAsync(filter, update, updateOptions);

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
            return true;

        }
        public async Task<bool> UpdateILSProcessingStatus(ObjectId id, string ILSProcessingStatus)
        {
            int retryWaitTime = RetryWaitTime;
            int retries = Retries;

            while (retries > 0)
            {
                try
                {
                    var filter = Builders<ILSAPIRequestLog>.Filter.Eq("_id", id);

                    var update = Builders<ILSAPIRequestLog>.Update
                        .Set("FootprintInformation.UpdatedBy", "ILSQueueService")
                        .Set("FootprintInformation.UpdatedDate", DateTime.Now)
                        .Set("ProcessingStatus", ILSProcessingStatus);


                    var updateOptions = new UpdateOptions();
                    updateOptions.IsUpsert = true;

                    var result = await _ILSAPIRequestLog.UpdateOneAsync(filter, update, updateOptions);

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
            return true;

        }

        public async Task<List<ILSAPIRequestLog>> GetOrderQueueItems(int maxItemNumber, string ilsProcessingStatus)
        {
            //var filter = Builders<ILSAPIRequestLog>.Filter.Eq("ProcessingStatus", ILSProcessingStatus.OrderingResponse);
            var filter = Builders<ILSAPIRequestLog>.Filter.Eq("ProcessingStatus", ilsProcessingStatus);
            //var sort = Builders<ILSAPIRequestLog>.Sort.Descending("FootprintInformation.CreatedDate");
            var sort = Builders<ILSAPIRequestLog>.Sort.Descending("Priority");

            var queueIdBsonDoucmentList = new List<ILSAPIRequestLog>();
            int retryWaitTime = RetryWaitTime;
            int retries = Retries;

            while (retries > 0)
            {
                try
                {
                    queueIdBsonDoucmentList = await _ILSAPIRequestLog.Find<ILSAPIRequestLog>(filter).Sort(sort).Limit(maxItemNumber).ToListAsync();
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
            return queueIdBsonDoucmentList;
        }
    }
}
