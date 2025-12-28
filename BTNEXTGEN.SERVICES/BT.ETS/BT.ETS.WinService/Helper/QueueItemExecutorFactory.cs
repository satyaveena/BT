using BT.ETS.WinService.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Helper
{
    public class QueueItemExecutorFactory
    {
        private readonly QueueItemJobType jobType;

        public QueueItemExecutorFactory(string jobType)
        {
            Enum.TryParse(jobType, true, out this.jobType);
        }

        public IQueueItemExecutor CreateExecutor()
        {
            switch (this.jobType)
            {
                case QueueItemJobType.CartReceived:
                    return new ETSCartReceiveExecutor();
                case QueueItemJobType.DupeCheck:
                    return new ETSDupCheckExecutor();
                case QueueItemJobType.Pricing:
                    return new ETSPricingExecutor();
            }

            return null;
        }
    }
}
