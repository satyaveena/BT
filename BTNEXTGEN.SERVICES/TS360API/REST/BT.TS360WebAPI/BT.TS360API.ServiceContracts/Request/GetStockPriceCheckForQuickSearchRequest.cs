using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts.Request
{
    public class GetStockPriceCheckForQuickSearchRequest : BaseRequest
    {
        public string btKey { get; set; }
        public string productType { get; set; }
        public ContextObject ObjContext { get; set; }
        public AccountInfoForPricing AccountPricing { get; set; }
        public TargetingValues Targeting { get; set; }
    }

    public class ContextObject
    {
        public string UserId { get; set; }
        public string OrgId { get; set; }
        public string CountryCode { get; set; }
        public bool IsHideNetPriceDiscountPercentage { get; set; }
        public string[] ESuppliers { get; set; }
        public bool SimonSchusterEnabled { get; set; }
        public string DefaultBookAccountId { get; set; }
        public string DefaultEntertainmentAccountId { get; set; }
        public string DefaultVIPAccountId { get; set; }
    }
    }
