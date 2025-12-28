using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class CartsDupCheckRequest
    {
        public string OrgId { get; set; }
        public string UserId { get; set; }
        public string BasketId { get; set; }

        public List<string> BTKeys { get; set; }
        public List<string> BTEKeys { get; set; }

        public string CartCheckType { get; set; }   // MyCarts or AllCarts
        public string DownloadCheckType { get; set; } // IncludeWCarts or IncludeWOrders

    }

    public class CartsDupCheckResponse
    {
        public List<CartDuplicateItem> DuplicateItems { get; set; }
    }

    public class CartDuplicateItem : DuplicateItem
    {
        public bool IsDupHolding { get; set; }
    }

    public class DuplicateItem
    {
        public string BTKey { get; set; }
        public bool IsDuplicated { get; set; }
    }

    public class OrdersDupCheckRequest
    {
        public string OrgId { get; set; }
        public string UserId { get; set; }
        public string BasketId { get; set; }

        public List<string> BTKeys { get; set; }

        public string CartCheckType { get; set; }   // MyCarts or AllCarts
        public string OrderCheckType { get; set; }  // MyAccounts or AllAccounts
        public string DownloadCheckType { get; set; } // IncludeWCarts or IncludeWOrders

        public List<string> UserAccounts { get; set; }
    }

    public class OrdersDupCheckResponse
    {
        public List<DuplicateItem> DuplicateItems { get; set; }
    }

    public class BTKeyListForTitleListRequest
    {
        public string UserId { get; set; }
        public List<string> BTKeys { get; set; }
    }

    public class BTKeyListForTitleListResponse
    {
        public List<string> BTKeys { get; set; }
        public List<string> BTEKeys { get; set; }
        public List<PrimaryCartTitleDetail> PrimaryCartTitleDetails { get; set; }
    }
}
