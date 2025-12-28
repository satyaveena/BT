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
using BT.TS360API.MongoDB.Contracts;

namespace BT.TS360API.MongoDB.DataAccess
{
    public class CommonDAO : BaseMongoDAO
    {
        private static CommonDAO _instance = null;
        private static readonly object SyncRoot = new Object();
        readonly IMongoDatabase _commonDatabase;

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

        public override string ConnectionString
        {
            get { return Common.AppSettings.MongoDBConnectionString; }
        }

        public override string DatabaseName
        {
            get { return Constants.CommonDatabaseName; }
        }

        public CommonDAO()
        {
            var client = new MongoClient(ConnectionString);
            _commonDatabase = client.GetDatabase(DatabaseName);
        }

        public async Task InsertBackgroundQueueItem(BackgroundQueue backgroundQueueItem)
        {
            var backgroundQueue  = _commonDatabase.GetCollection<BackgroundQueue>(Constants.BackgroundQueueCollectionName);
            int retryWaitTime = base.retryWaitTime;
            int retries = base.maxRetries;
            while (retries > 0)
            {
                try
                {
                    await backgroundQueue.InsertOneAsync(backgroundQueueItem);
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
    }
}
