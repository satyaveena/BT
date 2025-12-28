using System.Collections.Generic;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Pricing;

namespace BT.TS360API.Common.Pricing.Interface
{
    public interface IPricingController
    {
        List<ItemPricing> CalculatePrice(List<BasketLineItemUpdated> items, int? overriedQtyForDiscount,
            TargetingValues targetingParam, bool hideNetPrice = false);
        //void BackgroundReprice(int batchWaitingTime);
        void CalculatePrice(string basketSummaryId, int batchWaitingTime, TargetingValues targetingParam, bool isAsync = true);
        void BackgroundRepriceWithSiteContext(int batchWaitingTime, TargetingValues targetingParam, string cartId);
    }
}
