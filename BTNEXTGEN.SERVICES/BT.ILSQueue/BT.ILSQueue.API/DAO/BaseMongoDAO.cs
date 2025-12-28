using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.API.DAO
{
    public abstract class BaseMongoDAO
    {
        protected readonly int retryWaitTime = Convert.ToInt32(ConfigurationManager.AppSettings["RetryWaitTime"]);
        protected readonly int maxRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MaxConnectionRetries"]);

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
