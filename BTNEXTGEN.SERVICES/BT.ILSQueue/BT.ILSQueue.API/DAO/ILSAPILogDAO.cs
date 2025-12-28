using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BT.ILSQueue.API.DAO
{
    public class ILSAPILogDAO : BaseMongoDAO
    {
        private static ILSAPILogDAO _instance = null;
        private static readonly object SyncRoot = new Object();
        readonly IMongoDatabase _ILSAPILogDatabase;

        #region Singleton

        public static ILSAPILogDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ILSAPILogDAO();
                }

                return _instance;
            }
        }

        #endregion

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["MongoDBConnectionString"].ToString(); }
        }

        public override string DatabaseName
        {
            get { return ConfigurationManager.AppSettings["ILSDatabaseName"].ToString(); }
        }

        public ILSAPILogDAO()
        {
            var client = new MongoClient(ConnectionString);
            _ILSAPILogDatabase = client.GetDatabase(DatabaseName);
        }

        public async Task<bool> UpdateILSJobStatus(string pAPIJobID)
        {
            var collection = _ILSAPILogDatabase.GetCollection<BsonDocument>("ILSAPIRequestLog");
            var isSuccessfulUpdate = false;
            int retries = base.maxRetries;

            while (retries > 0)
            {
                try
                { //ProcessingStatus: "Ordering Response Ready" 
                    var filter = Builders<BsonDocument>.Filter.Eq("OrderingResponse.JobGuid", pAPIJobID);
                    var update = Builders<BsonDocument>.Update.Set("ProcessingStatus", "Ordering Response Ready")
                                                                        .Set("FootprintInformation.UpdatedDate", DateTime.Now)
                                                                        .Set("FootprintInformation.UpdatedBy", "ILSQueueAPI");
                    var response = await collection.UpdateOneAsync(filter, update);
                    if (response != null && response.ModifiedCount > 0)
                        isSuccessfulUpdate = true;
                    break;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(base.retryWaitTime);
                    if (retries < 1)
                    {
                        throw;
                    }
                }
            }
            return isSuccessfulUpdate;
        }
    }
}
