using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPAutoRankingConsole.Common
{
    public static class CommonConstants
    {
        public const string CART_ID_AUTORANK_PREFIX = "AutoRank";
    }

    public class StoredProcedure
    {
        public const string UPDATE_LOG_REQUEST = "procTS360APILogRequests";

        public const string UPDATE_CART_RANK = "procTS360APIPostCartRanked";

        public const string TS360_SET_ESP_AUTO_RANK_QUEUE_STATUS = "procTS360SetESPAutoRankQueueStatus";

        public const string ESP_GET_AUTO_RANK_REQUESTS = "procESPGetAutoRankRequests";

    }
}
