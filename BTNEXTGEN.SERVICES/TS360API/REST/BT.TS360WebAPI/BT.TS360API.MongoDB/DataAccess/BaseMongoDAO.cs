using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.MongoDB.DataAccess
{
    public abstract class BaseMongoDAO
    {
        protected readonly int retryWaitTime = Common.AppSettings.RetryWaitTime;
        protected readonly int maxRetries = Common.AppSettings.MaxConnectionRetries;

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
