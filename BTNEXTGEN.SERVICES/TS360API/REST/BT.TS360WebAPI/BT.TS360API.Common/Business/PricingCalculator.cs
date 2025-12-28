using System;
using System.Collections.Generic;
using System.Linq;
using BT.TS360API.Common.Business.Interface;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Pricing;
using BT.TS360API.Logging;
using BT.TS360API.Marketing;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Pricing;
using BT.TS360Constants;
using AppSettings = BT.TS360API.Common.Configrations.AppSettings;

namespace BT.TS360API.Common.Business
{
    public class PricingCalculator : IPricingCalculator
    {
        private static PricingCalculator _instance;

        //private static PromotionServiceClient _promotionServiceClient;

        /// <summary>
        /// Singleton instance of PricingCalculator
        /// </summary>
        public static PricingCalculator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PricingCalculator();
                //
                return _instance;
            }
        }

        private PricingCalculator()
        {

        }

        private int PromotionServiceBatchSize
        {
            get
            {
                var batchSize = AppSettings.PromotionServiceBatchSize;
                int iValue;
                if (string.IsNullOrEmpty(batchSize) || !Int32.TryParse(batchSize, out iValue))
                {
                    iValue = 160; //default
                }
                return iValue;
            }
        }

        /// <summary>
        /// Calculate list price of BasketLineItemUpdateds
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<ItemPricing> CalculateListPrice(List<BasketLineItemUpdated> items)
        {
            if (items == null || items.Count == 0) return null;

            var galeLiteral = PricingConfiguration.Instance.GaleLiteral;
            var listPricingArguments = new List<ListPricingArgument>();
            HashSet<string> uniqueBTKey = new HashSet<string>();
            foreach (BasketLineItemUpdated item in items)
            {
                PricingLogger.LogDebug("PricingCalculator",
                                           string.Format(
                                               "item.AccountERPNumber: {0}, galeLiteral: {1}",
                                               item.AccountERPNumber, galeLiteral));

                //If the item is GALE and there is not account
                if (string.IsNullOrEmpty(item.AccountERPNumber) &&
                    !string.IsNullOrEmpty(galeLiteral) &&
                    string.Compare(item.ESupplier, galeLiteral, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    PricingLogger.LogDebug("PricingCalculator",
                                           string.Format(
                                               "CalculateListPrice: Gale product ({0}, {1}) without Account Number => Set list price to null",
                                               item.BTKey, item.ESupplier));
                    item.ListPrice = null;
                }
                else if (!string.IsNullOrEmpty(item.BTKey))
                {
                    if (uniqueBTKey.Add(item.BTKey))
                        listPricingArguments.Add(new ListPricingArgument()
                        {
                            BTKey = item.BTKey,
                            eMarket = item.EMarket,
                            eTier = item.ETier
                        });
                }
            }
            
            return PricingCalculatorDAO.Instance.GetListPrice(listPricingArguments);
        }

        /// <summary>
        /// Calculate contract price of BasketLineItemUpdateds
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<ItemPricing> CalculateContractPrice(List<BasketLineItemUpdated> items, int? overriedQtyForDiscount)
        {
            var itemPricings = new List<ItemPricing>();
            List<SOPPricingArgument> sopPricingArguments;
            List<TOLASPricingArgument> tolasPricingArguments;
            //create the SOP Arguments and TOLAS Arguments
            GetSOPAndTOLASArgument(items, out sopPricingArguments, out tolasPricingArguments);

            if (sopPricingArguments == null || sopPricingArguments.Count == 0)
            {
                PricingLogger.LogInfo("PricingCalculator", "CalculateContractPrice => There is no item for SOP pricing.");
            }
            else
            {
                var sopItemPricings = PricingCalculatorDAO.Instance.CalculateContractPrice(sopPricingArguments, overriedQtyForDiscount);
                if (sopItemPricings != null && sopItemPricings.Count > 0)
                {
                    itemPricings.AddRange(sopItemPricings);
                }
            }

            if (tolasPricingArguments == null || tolasPricingArguments.Count == 0)
            {
                PricingLogger.LogInfo("PricingCalculator", "CalculateContractPrice => There is no item for TOLAS pricing.");
            }
            else
            {
                var tolasItemPricings = PricingCalculatorDAO.Instance.CalculateContractPrice(tolasPricingArguments);
                if (tolasItemPricings != null && tolasItemPricings.Count > 0)
                {
                    itemPricings.AddRange(tolasItemPricings);
                }
            }

            return itemPricings;
        }

        public List<ItemPricing> CalculatePromotionPrice(List<BasketLineItemUpdated> items, TargetingValues targetingParam)
        {
            var repriceLines = new List<LineRepricedInfo>();
            foreach (var lineItem in items)
            {
                if (!string.IsNullOrEmpty(lineItem.BTKey) && !string.IsNullOrEmpty(lineItem.ProductCatalog) &&
                    !string.IsNullOrEmpty(lineItem.SoldToId) && !string.IsNullOrEmpty(lineItem.MarketType) && lineItem.TotalLineQuantity != 0)
                {
                    repriceLines.Add(new LineRepricedInfo()
                    {
                        AudienceType = lineItem.AudienceType,
                        BTKey = lineItem.BTKey,
                        MarketType = lineItem.MarketType,
                        UserId = lineItem.SoldToId,
                        ProductCatalog = lineItem.ProductCatalog,
                        ProductType = lineItem.ProductType,
                        TotalLineQuantity = lineItem.TotalLineQuantity,
                        TotalOrderQuantity = lineItem.TotalOrderQuantity.HasValue ? lineItem.TotalOrderQuantity.Value : 0,
                        Pig = lineItem.Pig,
                        SiteBranding = lineItem.SiteBranding,
                        OrgId = lineItem.OrgId,
                        OrgName = lineItem.OrgName,
                        ListPrice = lineItem.ListPrice
                    });
                }
            }
            var retList = new List<ItemPricing>();
            if (repriceLines.Count == 0) return retList;

            //call to CS Promotion Service
            //var returnPromoPrices = RunPipeline(repriceLines);

            //Get Promotion Price from Database
            var returnPromoPrices = GetPromotionPrices(repriceLines, targetingParam);

            //validate
            if (returnPromoPrices == null) return retList;
            PricingLogger.LogDebug("PricingCalculator", "==================ItemPricing after get promotion price:==================");
            //calculate after getting promotion prices
            foreach (var lineItem in items)
            {
                if (returnPromoPrices.ContainsKey(lineItem.BTKey))
                {
                    var promotionPrice = returnPromoPrices[lineItem.BTKey];

                    var singlePromoPrice = promotionPrice.Price / lineItem.TotalLineQuantity;
                    var pricingItem = new ItemPricing
                    {
                        BTKey = lineItem.BTKey,
                        BasketSummaryId = lineItem.BasketSummaryId,
                        ContractPrice = lineItem.ContractPrice,
                        ExtendedPrice = lineItem.ExtendedPrice,
                        ListPrice = lineItem.ListPrice,
                        PromotionPrice = singlePromoPrice,
                        PromotionDiscountPercent =
                            (lineItem.ListPrice.HasValue && lineItem.ListPrice.Value != 0 &&
                             singlePromoPrice != 0)
                                ? (lineItem.ListPrice - singlePromoPrice) * 100 / lineItem.ListPrice
                                : 0,
                        PromotionCode = promotionPrice.PromotionCode,
                        BasketDisplay = promotionPrice.BasketDisplayText
                    };
                    PricingLogger.LogDebug("PricingCalculator", pricingItem.ToString());
                    retList.Add(pricingItem);
                }
            }

            return retList;
        }

        private Dictionary<string, PromotionPrice> GetPromotionPrices(List<LineRepricedInfo> items, TargetingValues targeting)
        {
            var activeDis = MarketingDAOManager.Instance.GetApprovedDiscounts();

            if(activeDis == null || activeDis.Count == 0) return new Dictionary<string, PromotionPrice>();

            var promotionArgList = items.Select(lineRepricedInfo => new PromotionClientArg()
                                                                    {
                                                                        BTKey = lineRepricedInfo.BTKey, 
                                                                        Catalog = lineRepricedInfo.ProductCatalog, 
                                                                        ProductType = lineRepricedInfo.ProductType
                                                                    }).ToList();

            const bool isVirtualCatalog = true;
            var listProducts = ConvertToBtKeyProduct(promotionArgList, isVirtualCatalog);
            var parentCatFromDb = ProductCatalogDAO.Instance.GetParentCategories(listProducts, isVirtualCatalog);

            if (parentCatFromDb == null || parentCatFromDb.Count == 0) return new Dictionary<string, PromotionPrice>();

            //var targetingText = MarketingHelper.Instance.GenerateTargetingValues(siteContext);
            //var targetingParam = MarketingHelper.Instance.ConvertStringToTargetingParam(targetingText);
            var targetingParam = MarketingHelper.Instance.ToTargetingParam(targeting);

            var promoPricesDict = new Dictionary<string, PromotionPrice>();

            foreach (var lineRepricedInfo in items)
            {
                var promoPrice = new PromotionPrice();

                var parentCat = parentCatFromDb.Find(item => item.BTKey == lineRepricedInfo.BTKey);

                if(parentCat == null) continue;

                var dis = MarketingHelper.Instance.GetAppropriateDiscount(activeDis, targetingParam, parentCat);
                if(dis == null) continue;

                promoPrice.BtKey = lineRepricedInfo.BTKey;
                promoPrice.Price = GetDiscountPrice(lineRepricedInfo, dis);
                promoPrice.PromotionCode = dis.DiscountName;
                promoPrice.BasketDisplayText = dis.BasketDisplay.DisplayText;

                if (!promoPricesDict.ContainsKey(promoPrice.BtKey))
                {
                    promoPricesDict.Add(promoPrice.BtKey, promoPrice);
                }
                else
                {
                    promoPricesDict[promoPrice.BtKey] = promoPrice;
                }
            }

            return promoPricesDict;
        }

        private decimal GetDiscountPrice(LineRepricedInfo lineRepricedInfo, MarketingApprovedDiscount dis)
        {
            if (!lineRepricedInfo.ListPrice.HasValue) return 0;

            var listPrice = lineRepricedInfo.ListPrice.Value;

            if (dis.OfferType == OfferType.AmountOff)
            {
                return listPrice - dis.OfferValue;
            }

            var discountValue = (listPrice * dis.OfferValue) / 100;
            return listPrice - discountValue;
        }

        private List<BtKeyCatalogObject> ConvertToBtKeyProduct(IEnumerable<PromotionClientArg> promotionArgList, bool isVirtualCatalog)
        {
            var results = new List<BtKeyCatalogObject>();

            foreach (var promotionClientArg in promotionArgList)
            {
                var catalogName = isVirtualCatalog ? "MarketingCatalog" : promotionClientArg.Catalog;
                results.Add(new BtKeyCatalogObject(promotionClientArg.BTKey, catalogName, promotionClientArg.Catalog));
            }
            return results;
        }

        /// <summary>
        /// Build SOP/TOLAS Pricing Arguments.
        /// </summary>
        /// <param name="lineItemUpdateds"></param>
        /// <param name="sopPricingArguments"></param>
        /// <param name="tolasPricingArguments"></param>
        private void GetSOPAndTOLASArgument(IEnumerable<BasketLineItemUpdated> lineItemUpdateds,
            out List<SOPPricingArgument> sopPricingArguments,
            out List<TOLASPricingArgument> tolasPricingArguments)
        {
            sopPricingArguments = new List<SOPPricingArgument>();
            tolasPricingArguments = new List<TOLASPricingArgument>();
            HashSet<string> uniqueBTKeySOP = new HashSet<string>();
            HashSet<string> uniqueBTKeyTOLAS = new HashSet<string>();
            foreach (var lineItem in lineItemUpdateds)
            {
                //If there's no account for pricing, the contrace will be null and displayed on UI as list price
                if (string.IsNullOrEmpty(lineItem.AccountId) && string.IsNullOrEmpty(lineItem.AccountERPNumber))
                {
                    lineItem.ContractPrice = null;
                }
                else
                {
                    string itemId;
                    if (CheckItemForTolas(lineItem, out itemId))
                    {
                        if (!string.IsNullOrEmpty(itemId) && !string.IsNullOrEmpty(lineItem.AccountERPNumber)
                            && !string.IsNullOrEmpty(lineItem.PrimaryWarehouse))
                        {
                            if (uniqueBTKeyTOLAS.Add(lineItem.BTKey))
                                tolasPricingArguments.Add(new TOLASPricingArgument()
                                {
                                    BasketSummaryID = lineItem.BasketSummaryId,
                                    BTKey = lineItem.BTKey,
                                    ItemId = itemId,
                                    PriceKey = lineItem.PriceKey,
                                    AcceptableDiscount = lineItem.AcceptableDiscount,
                                    AccountERPNumber = lineItem.AccountERPNumber,
                                    ListPrice = lineItem.ListPrice,
                                    PrimaryWarehouse = lineItem.PrimaryWarehouse,
                                    ProductLine = lineItem.ProductLine,
                                    ReturnFlag = lineItem.ReturnFlag,
                                    TotalLineQuanity = lineItem.TotalLineQuantity
                                }
                                    );
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(lineItem.BTKey) && !string.IsNullOrEmpty(lineItem.AccountPricePlan)
                            && !string.IsNullOrEmpty(lineItem.PriceKey) && lineItem.TotalOrderQuantity.HasValue)
                        {
                            if (uniqueBTKeySOP.Add(lineItem.BTKey))
                                sopPricingArguments.Add(new SOPPricingArgument()
                                {
                                    BTKey = lineItem.BTKey,
                                    PlanID = lineItem.AccountPricePlan,
                                    PriceKey = lineItem.PriceKey,
                                    TotalQuanity = lineItem.TotalOrderQuantity,
                                    OrderLineQuanity = lineItem.TotalLineQuantity,
                                    ListPrice = lineItem.ListPrice
                                });
                        }
                    }
                }
            }
        }

        private bool CheckItemForTolas(BasketLineItemUpdated lineItem, out string itemId)
        {
            itemId = string.Empty;
            if (string.Compare(lineItem.ProductType, ProductTypeConstants.Entertainment, StringComparison.CurrentCultureIgnoreCase) == 0
                    || string.Compare(lineItem.ProductType, ProductTypeConstants.Movie, StringComparison.CurrentCultureIgnoreCase) == 0
                    || string.Compare(lineItem.ProductType, ProductTypeConstants.Music, StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(lineItem.Upc))
                    itemId = lineItem.Upc;
                else
                    itemId = lineItem.ISBN;

                return true;
            }

            if (lineItem.IsHomeDelivery || lineItem.IsVIPAccount || lineItem.IsOneBoxAccount)
            {
                itemId = lineItem.BTKey;
                return true;
            }

            return false;
        }

        //public void Dispose()
        //{
        //    if (_promotionServiceClient != null &&
        //        (_promotionServiceClient.State == CommunicationState.Opened ||
        //        _promotionServiceClient.State == CommunicationState.Created ||
        //        _promotionServiceClient.State == CommunicationState.Faulted))
        //    {
        //        _promotionServiceClient.Close();
        //        _promotionServiceClient = null;
        //    }
        //}
    }

}
