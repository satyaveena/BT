using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Pricing;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS360Constants;

namespace BT.TS360API.Common.Pricing
{
    public class CalculatePricePresenter
    {
        public static void CalculatePrice(ProductSearchResults result, SearchResultsPricingArg pricingArg)
        {
            try
            {
                if (result == null) return;
                var lineItemUpdateds = new List<BasketLineItemUpdated>();
                var searchResultItems = result.Items;
                var realtimePricingHelper = new RealTimePricingHelper();
                foreach (var prodItem in searchResultItems)
                {
                    var accountInfo = realtimePricingHelper.GetAccountInfoForPricing(prodItem.ProductType, prodItem.ESupplier, prodItem.ProductFormat, pricingArg.UserId, pricingArg.InforForPricing);

                    lineItemUpdateds.Add(new BasketLineItemUpdated()
                    {
                        ISBN = prodItem.ISBN,
                        BTKey = prodItem.BTKey,
                        SoldToId = pricingArg.UserId,
                        AccountId = accountInfo.AccountId,
                        ProductType = prodItem.ProductType,
                        TotalLineQuantity = 1,
                        TotalOrderQuantity = 1,
                        PriceKey = prodItem.PriceKey,
                        ListPrice = prodItem.ListPrice,
                        AccountPricePlan = accountInfo.AccountPricePlanId,
                        ProductLine = prodItem.ProductLine,
                        ReturnFlag = prodItem.HasReturn,
                        AccountERPNumber = accountInfo.ErpAccountNumber,
                        PrimaryWarehouse = accountInfo.PrimaryWarehouseCode,
                        ProductCatalog = prodItem.Catalog,
                        MarketType = pricingArg.TargetingValues.MarketType.ToString(),
                        AudienceType = CommonHelper.Instance.ConvertAudienceTypeAsString(pricingArg.TargetingValues.AudienceType),
                        EMarket = accountInfo.EMarketType,
                        ETier = accountInfo.ETier,
                        ProductPriceChangedIndicator = true,
                        ContractChangedIndicator = true,
                        PromotionChangedIndicator = false,
                        PromotionActiveIndicator = false,
                        QuantityChanged = true,
                        IsHomeDelivery = accountInfo.IsHomeDelivery,
                        Upc = prodItem.Upc,
                        AcceptableDiscount = prodItem.AcceptableDiscount,
                        NumberOfBuildings = accountInfo.BuildingNumbers,
                        ProcessingCharges = accountInfo.ProcessingCharges,
                        SalesTax = accountInfo.SalesTax,
                        IsVIPAccount = accountInfo.IsVIPAccount
                    });
                }
                var pricingController = new PricingController();
                //4428: For Search Page Only and for Retail Customers Only we will pass a Static order quantity of 5 
                //and a line quantity of 5 in the product search.  Applicable to both Basic Search and Advanced Search.  Hardcoded to Pricing.            
                //TFS#4949 - Show or hide net price   
                //10297: apply 4428 for any market type
                var itemPricings = pricingController.CalculatePrice(lineItemUpdateds, (pricingArg.TargetingValues.MarketType == MarketType.Retail) ? 5 : 1
                                                                     , pricingArg.TargetingValues, pricingArg.IsHideNetPriceDiscountPercentage);
                //
                var galeLiteral = CommonHelper.GetESupplierTextFromSiteTerm(AccountType.GALEE.ToString());
                if (itemPricings != null)
                {
                    var isExistGaleAccount = CommonHelper.IsESupplierAccountExisted(AccountType.GALEE.ToString(), pricingArg.UserId);
                    foreach (var itemPricing in itemPricings)
                    {
                        var prodItem = searchResultItems.Where(x => x.BTKey == itemPricing.BTKey).FirstOrDefault();
                        if (prodItem != null)
                        {
                            prodItem.ListPrice = itemPricing.ListPrice.HasValue ? itemPricing.ListPrice.Value : 0;
                            prodItem.DiscountPrice = itemPricing.SalePrice.HasValue ? itemPricing.SalePrice.Value : 0;
                            prodItem.DiscountPercent = itemPricing.DiscountPercent.HasValue ? itemPricing.DiscountPercent.Value : 0;
                            if (prodItem.ESupplier == galeLiteral)
                            {
                                var na = CommonResources.NA;
                                var salePrice = itemPricing.SalePrice.HasValue ? itemPricing.SalePrice.Value : 0;
                                var priceText = CommonHelper.Instance.DeterminePriceForGaleProduct(salePrice.ToString(), isExistGaleAccount, pricingArg.ESuppliers);
                                if (priceText == na)
                                {
                                    prodItem.ListPrice = -1;
                                    prodItem.DiscountPrice = -1;
                                    prodItem.DiscountPercent = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.CommonControl);
            }
        }
    }
}
