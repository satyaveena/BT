using BT.TS360API.Authentication.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.Authentication.DataAccess
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