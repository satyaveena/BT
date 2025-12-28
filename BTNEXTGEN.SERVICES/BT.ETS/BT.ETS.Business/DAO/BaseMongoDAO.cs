using BT.ETS.Business.Helpers;
using System;
using System.Configuration;
namespace BT.ETS.Business.DAO
{
    public abstract class BaseMongoDAO
    {
        protected readonly int retryWaitTime = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.RetryWaitTime);
        protected readonly int maxRetries = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MaxConnectionRetries);
    }
}
