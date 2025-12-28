using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class ESPRankJsonResponse
    {
        public string cartId { get; set; }
        public string statusCode { get; set; }
        public string statusMessage { get; set; }
        public string jobId { get; set; }

        public string jobUrl { get; set; }
        

        //Public branches As List(Of distBranch)  'Never used, but included to have in common with other responses
        //Public fundCodes As List(Of distFund)   'Never used, but included to have in common with other responses
    }
}