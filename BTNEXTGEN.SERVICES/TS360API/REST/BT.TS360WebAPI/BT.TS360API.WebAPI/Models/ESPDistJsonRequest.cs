using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class ESPDistJsonRequest
    {
        public string espLibraryId { get; set; }
        public string userName { get; set; }
        public string fundMonitoring { get; set; }
        public string cartId { get; set; }
        public string cartName { get; set; } // new in Release 4.2
        public string userId { get; set; } // new in Release 4.2
        public string showJobURL { get; set; } // new in Release 4.2

        public List<ESPDistBranchJsonRequest> branches { get; set; }
        public List<ESPDistItemJsonRequest> items { get; set; }
    }

    public class ESPDistBranchJsonRequest
    {
        public string branchId { get; set; }
        public string code { get; set; }

    }

    public class ESPDistItemJsonRequest
    {
        public string lineItemId { get; set; }
        public string vendorId { get; set; }
        public string fundId { get; set; }
        public string fundCode { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public string series { get; set; }
        public string bisac { get; set; }
        public string dewey { get; set; }
        public string publisher { get; set; }

        // new in Release 4.2
        public decimal listPrice { get; set; }
        public decimal discountedPrice { get; set; }
    }
}