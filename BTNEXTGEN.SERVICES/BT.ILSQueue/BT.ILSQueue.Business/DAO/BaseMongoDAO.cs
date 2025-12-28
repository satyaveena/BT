using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.Business.Helpers;

namespace BT.ILSQueue.Business.DAO
{
    public abstract class BaseMongoDAO
    {
        protected readonly string ConnectionString = AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.MongoDBConnectionString);
        protected readonly int RetryWaitTime = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MongoDBRetryWaitTime);
        protected readonly int Retries = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MongoDBMaxConnectionRetries);
    }
}
