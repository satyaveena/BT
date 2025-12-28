using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPAutoRankingConsole.Common
{
    public enum ESPAutoRankQueueStatus
    {
        Requested = 1,
        InProcess = 2, // picked up from queue
        Submitted = 3,       // sent to CHQ but no response yet
        Failed = 4,
        Successful = 5,
        Removed = 6
    }

    public enum ExceptionCategory
    {
        ESPGetAutoRankRequests,
        SetESPAutoRankStatus,
        SubmitEspAutoRank
    }
}
