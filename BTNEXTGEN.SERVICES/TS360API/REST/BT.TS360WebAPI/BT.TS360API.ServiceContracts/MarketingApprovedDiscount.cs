using System;
using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public class MarketingApprovedDiscount
    {
        public int DiscountIndex { get; set; }
        public string DiscountName { get; set; }
        public string AwardCatalog { get; set; }
        public string AwardCategory { get; set; }
        public string AwardProduct { get; set; }
        public int AwardExpressionId { get; set; }
        public BasketDisplay BasketDisplay { get; set; }
        public OfferType OfferType { get; set; }
        public decimal OfferValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PageGroup { get; set; }
        public List<TargetingItemAction> TargetingExpressions { get; set; }
        public List<string> EligibilityExpressions { get; set; }
    }

    public class MarketingApprovedAd
    {
        public int AdIndex { get; set; }
        public string AdName { get; set; }
        public List<TargetingItemAction> TargetingExpressions { get; set; }
        public List<string> PageGroupList { get; set; }
        public List<string> EligibilityExpressions { get; set; }
    }

    public class TargetingItemAction
    {
        public TargetingAction TargetingAction { get; set; }
        public string TargetingBody { get; set; }
    }

    public class BasketDisplay
    {
        public int DiscountIndex { get; set; }
        public string Culture { get; set; }
        public string DisplayText { get; set; }
    }

    public enum OfferType
    {
        AmountOff = 1,
        PercentOff = 2
    }

    public enum TargetingAction
    {
        Exclude = 3,
        Require = 2,
        Sponsor = 4,
        Target = 1
    }
}
