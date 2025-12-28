using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESPAutoRankingConsole.DataModels
{
    public class ESPRankJsonResponse
    {
        public string cartId { get; set; }
        public string statusCode { get; set; }
        public string statusMessage { get; set; }
        public string jobId { get; set; }

        public string jobUrl { get; set; }
    }
}