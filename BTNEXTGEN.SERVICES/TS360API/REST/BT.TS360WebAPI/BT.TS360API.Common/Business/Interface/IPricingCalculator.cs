using System.Collections.Generic;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Pricing;

namespace BT.TS360API.Common.Business.Interface
{
    public interface IPricingCalculator
    {
        List<ItemPricing> CalculateListPrice(List<BasketLineItemUpdated> items);
        List<ItemPricing> CalculateContractPrice(List<BasketLineItemUpdated> items, int? overriedQtyForDiscount);
        List<ItemPricing> CalculatePromotionPrice(List<BasketLineItemUpdated> items, TargetingValues targetingParam);
    }
}
