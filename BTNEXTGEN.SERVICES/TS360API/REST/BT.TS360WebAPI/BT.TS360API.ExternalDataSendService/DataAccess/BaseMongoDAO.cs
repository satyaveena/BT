using BT.TS360API.ExternalDataSendService.Configration;
using BT.TS360API.ExternalDataSendService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.ExternalDataSendService.DataAccess
{
    public abstract class BaseMongoDAO
    {
        protected readonly int retryWaitTime = AppSettings.RetryWaitTime;
        protected readonly int maxRetries = AppSettings.MaxConnectionRetries;

        public abstract string ConnectionString
        {
            get;
        }

        public abstract string DatabaseName
        {
            get;
        }
    }
}