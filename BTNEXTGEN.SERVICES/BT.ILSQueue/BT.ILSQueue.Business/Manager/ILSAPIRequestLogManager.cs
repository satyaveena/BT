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

namespace BT.ILSQueue.Business.Manager
{
    public class ILSAPIRequestLogManager
    {
        
        readonly IMongoCollection<ILSAPIRequestLog> _ILSAPIRequestLog;

        public ILSAPIRequestLogManager()
        {
            var client = new MongoClient(ConnectionString);
            var iCommonDatabase = client.GetDatabase(ApplicationConstants.CommonDatabaseName);

            _ILSAPIRequestLog = iCommonDatabase.GetCollection<ILSAPIRequestLog>(ApplicationConstants.ILSAPIRequestCollectionName);
          
        }

        public string ConnectionString
        {
            get { return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.MongoDBConnectionString);  }
        }

        public int RetryWaitTime
        {
            get { return AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MongoDBRetryWaitTime); }
        }
        public int Retries
        {
            get { return AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MongoDBMaxConnectionRetries); }
        }

        //public  ObjectId AddILSAPIRequestQueueLog(ILSAPIRequestLog queueLog)
        public async Task<ObjectId> AddILSAPIRequestQueueLog(ILSAPIRequestLog queueLog)
        {
            var now = DateTime.Now;
            try
            {
                queueLog.ILSQueueID = ObjectId.GenerateNewId();
                
                queueLog.FootprintInformation = new FootprintInformation();

                queueLog.FootprintInformation.CreatedDate = now;
                queueLog.FootprintInformation.CreatedByUserID = "ILSQueueService";
                queueLog.FootprintInformation.UpdatedDate = now;
                queueLog.FootprintInformation.UpdatedByUserID = "ILSQueueService";

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
                
                /*var logger = new LoggerService();
                logger.LogError(exception, "ILSService SaveILSAPIRequest", string.Format("Message: {0}, Stack Trace: {1}",
                    exception.Message, exception.StackTrace));*/
            }

            return queueLog.ILSQueueID;
        }

        public async Task<bool> UpdateILSValidationResponseLog(ObjectId id, PolarisPOResponse validatonResponse)
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
                        .Set("ProcessingStatus", ILSProcessingStatus.ValidatonResponse)
                        .Set("ValidatonResponse", validatonResponse);
                      

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
                        .Set("ProcessingStatus", ILSProcessingStatus.ValidatonResponse)
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

        public async Task<bool> UpdateILSOrderResponseLog(ObjectId id, PolarisPOResponse orderResponse)
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
                        .Set("ProcessingStatus", ILSProcessingStatus.OrderingResponse)
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

        /*public Task<List<ETSQueueItem>> GetQueueItems(int maxItemNumber)
        {
            var filter = Builders<ILSAPIRequestLog>.Filter.Eq("ProcessingStatus", ILSProcessingStatus.OrderingResponse);
            var sort = Builders<ILSAPIRequestLog>.Sort.Descending("FootprintInformation.CreatedDate");

            var queueIdBsonDoucmentList = new List<ILSAPIRequestLog>();
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
        }*/
    }
}
