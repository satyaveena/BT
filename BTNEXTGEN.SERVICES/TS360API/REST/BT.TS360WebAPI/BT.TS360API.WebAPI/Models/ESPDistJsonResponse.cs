using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class ESPDistJsonResponse
    {
        public string cartId { get; set; }
        
        public string jobId { get; set; }

        public List<ESPDistStatusCodeJsonResponse> statusCodes { get; set; }

        public List<ESPDistBranchJsonResponse> branches { get; set; }

        public List<ESPDistFundJsonResponse> fundCodes { get; set; }
        
        // release 4.2
        public string jobURL { get; set; } // https://esp.collectionhq.com/espweb/50A83E9640D81BEDE050007F01003D8B",
        
        public string jobURLText { get; set; }

        public string statusCode { get; set; }
        public string statusMessage { get; set; }

    }

    public class ESPDistStatusCodeJsonResponse
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class ESPDistBranchJsonResponse
    {
        public string branchId { get; set; }
        public string code { get; set; }
        public string status { get; set; }
    }

    public class ESPDistFundJsonResponse
    {
        public string fundId { get; set; }
        public string code { get; set; }
        public string status { get; set; }
    }
}