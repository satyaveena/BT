using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BT.TS360API.Common.Business;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Pricing.Interface;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Pricing;
using BT.TS360Constants;

namespace BT.TS360API.Common.Pricing
{
    public class PricingController : IPricingController
    {
        public class ThreadPamameter
        {
            public string BasketSummaryID { get; set; }
            //public int BatchSize { get; set; }
            public int BatchWaitingTime { get; set; }
            public TargetingValues TargetingParam { get; set; }
        }
        private const string LogCategory = "PricingController";

        #region public method
        //public void BackgroundReprice(int batchWaitingTime)
        //{
        //    var pc = new PricingController();
        //    ThreadPool.QueueUserWorkItem(pc.DoBackgroundPrice,
        //        new ThreadPamameter() { BatchWaitingTime = batchWaitingTime });
        //}

        public void BackgroundRepriceWithSiteContext(int batchWaitingTime, TargetingValues siteContext, string cartId)
        {
            var pc = new PricingController();
            ThreadPool.QueueUserWorkItem(pc.DoCalculateCartPrice,
                new ThreadPamameter() { BatchWaitingTime = batchWaitingTime, TargetingParam = siteContext, BasketSummaryID = cartId });
        }

        public void CalculatePrice(string basketSummaryId, int batchWaitingTime, TargetingValues siteContext, bool isAsync = true)
        {
            var param = new ThreadPamameter()
            {
                BasketSummaryID = basketSummaryId,
                BatchWaitingTime = batchWaitingTime,
                TargetingParam = siteContext
            };

            if (isAsync)
            {
                var pc = new PricingController();
                ThreadPool.QueueUserWorkItem(pc.DoCalculateCartPrice, param);
            }
            else
            {
                DoCalculateCartPrice(param);
            }
        }
        public void CalculatePrice(string basketSummaryId, int batchWaitingTime, bool isAsync = true)
        {
            var param = new ThreadPamameter()
            {
                BasketSummaryID = basketSummaryId,
                //BatchSize = batchSize,
                BatchWaitingTime = batchWaitingTime
            };

            if (isAsync)
            {
                var pc = new PricingController();
                //var newThread = new Thread(pc.DoCalculateCartPrice);
                //newThread.Start(param);
                ThreadPool.QueueUserWorkItem(pc.DoCalculateCartPrice, param);
            }
            else
            {
                DoCalculateCartPrice(param);
            }
        }
        public List<ItemPricing> CalculatePrice(List<BasketLineItemUpdated> items, int? overriedQtyForDiscount,
            TargetingValues targetingParam, bool hideNetPrice = false)
        {
            if (items == null || items.Count == 0) return null;
            List<ItemPricing> retItemPricing = null;
            try
            {
                PricingLogger.LogInfo(LogCategory, "CalculatePrice - Realtime <== BEGIN");
                foreach (var item in items)
                {
                    PricingLogger.LogDebug(LogCategory, item.ToString());
                }
                //
                //re-price
                ProcessRePrice(items, overriedQtyForDiscount, targetingParam, hideNetPrice);
                //
                retItemPricing = ReMapItemPricing(items);

                if (retItemPricing != null && retItemPricing.Count > 0)
                {
                    var retItemPricingLogMessage = string.Empty;
                    foreach (var itemPricing in retItemPricing)
                    {
                        retItemPricingLogMessage += Environment.NewLine + itemPricing;
                    }
                    PricingLogger.LogDebug(LogCategory, "Items Pricing Return:" + retItemPricingLogMessage);
                }
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
            finally
            {
                PricingLogger.LogInfo(LogCategory, "CalculatePrice - Realtime ==> END");
            }
            return retItemPricing;
        }
        #endregion

        #region Thread Worker Methods

        private void DoCalculateCartPrice(object data)
        {
            var threadParam = (ThreadPamameter)data;
            var basketSummaryId = threadParam.BasketSummaryID;
            if (string.IsNullOrEmpty(basketSummaryId)) return;
            PricingLogger.LogInfo(LogCategory, string.Format("CalculatePrice - BasketSummary:{0} <== BEGIN", basketSummaryId));

            var pricingControllerDao = new PricingControllerDAO();
            var hasException = false;
            try
            {
                List<BasketLineItemUpdated> items;

                // Get BasketLineItemUpdated to re-price
                PricingLogger.LogInfo(LogCategory, "GetBasketLineItemUpdatedToRePrice <== BEGIN");
                var ds = pricingControllerDao.GetBasketLineItemUpdatedToRePrice(basketSummaryId);
                items = PricingDAOManager.ConvertToBasketLineItemUpdated(ds);
                PricingLogger.LogInfo(LogCategory, "GetBasketLineItemUpdatedToRePrice ==> END");

                if (items == null) return;
                foreach (var item in items)
                {
                    PricingLogger.LogDebug(LogCategory, item.ToString());
                }

                PricingLogger.LogInfo(LogCategory, "ProcessRePrice <== BEGIN");
                //re-price
                ProcessRePrice(items, threadParam.TargetingParam);
                PricingLogger.LogInfo(LogCategory, "ProcessRePrice ==> END");
                foreach (var item in items)
                {
                    PricingLogger.LogDebug(LogCategory, item.ToString());
                }

                PricingLogger.LogInfo(LogCategory, "UpdateBasketLineItemUpdated <== BEGIN");
                pricingControllerDao.UpdateBasketLineItemUpdated(items);
                PricingLogger.LogInfo(LogCategory, "UpdateBasketLineItemUpdated ==> END");
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                hasException = true;
            }
            finally
            {
                //just in case exception occurs before the SET sp is called, call this to reset the RepricingIndicator to 0.
                if (hasException)
                {
                    PricingLogger.LogInfo(LogCategory, "ResetBasketRepricingIndicator <== BEGIN");
                    pricingControllerDao.ResetBasketRepricingIndicator(basketSummaryId);
                    PricingLogger.LogInfo(LogCategory, "ResetBasketRepricingIndicator ==> END");
                }
                PricingLogger.LogInfo(LogCategory, "CalculatePrice <= END");
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Remap basket lineitem to item pricing
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private List<ItemPricing> ReMapItemPricing(IEnumerable<BasketLineItemUpdated> items)
        {
            return items.Select(item => new ItemPricing
            {
                BTKey = item.BTKey,
                BasketSummaryId = item.BasketSummaryId,
                ListPrice = item.ListPrice,
                ContractPrice = item.ContractPrice,
                PromotionPrice = item.PromotionPrice,
                SalePrice = item.SalePrice,
                ExtendedPrice = item.ExtendedPrice,
                ESupplier = item.ESupplier,
                DiscountPercent = item.DiscountPercent
            }).ToList();
        }

        private void ProcessRePrice(List<BasketLineItemUpdated> items, TargetingValues targetingParam)
        {
            ProcessRePrice(items, null, targetingParam);
        }
        private void ProcessRePrice(List<BasketLineItemUpdated> items, int? overriedQtyForDiscount, TargetingValues targetingParam,
            bool hideNetPrice = false)
        {
            //List price will be reset all at one.
            CalculateListPrice(items);

            if (!hideNetPrice)
            {
                CalculateContractPrice(items, overriedQtyForDiscount);
                //
                CalculatePromotionPrice(items, targetingParam);
            }
            //Re total items
            ReTotal(items);
        }

        /// <summary>
        /// Re update price of line item
        /// </summary>
        /// <param name="items"></param>
        private void ReTotal(IEnumerable<BasketLineItemUpdated> items)
        {
            PricingLogger.LogInfo(LogCategory, "Retotal after pricing");
            foreach (var item in items)
            {
                PricingLogger.LogDebug(LogCategory, "Item before retotal:" + item);
                if (!item.ContractPrice.HasValue || item.ContractPrice == 0)
                {
                    item.ContractPrice = item.ListPrice;
                }
                if (item.PromotionActiveIndicator && item.PromotionChangedIndicator)
                {
                    item.SalePrice = Lowest(item.PromotionPrice, item.ContractPrice);

                    item.DiscountPercent = (item.SalePrice == item.PromotionPrice &&
                                            item.PromotionDiscountPercent > 0)
                                               ? item.PromotionDiscountPercent
                                               : item.ContractDiscountPercent;

                    item.PromotionChangedIndicator = false;
                }
                else
                {
                    item.SalePrice = item.ContractPrice;
                    item.DiscountPercent = item.ContractDiscountPercent;
                }
                item.ExtendedPrice = item.TotalLineQuantity * item.SalePrice;

                ApplyMupoAndProcessingChargesToItem(item);

                PricingLogger.LogDebug(LogCategory, "Item after retotal:" + item);
            }
        }

        private void ApplyMupoAndProcessingChargesToItem(BasketLineItemUpdated item)
        {
            if (string.Compare(item.PurchaseOption, MupoContants.MupoOption, StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(item.PurchaseOption, MupoContants.MupoOptionMultiUser, StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(item.PurchaseOption, MupoContants.MupoOptionMultiUser1Year, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (item.NumberOfBuildings < 1) item.NumberOfBuildings = 1;

                if (item.ListPrice <= item.ContractPrice)
                {
                    item.ExtendedPrice = item.ContractPrice * item.NumberOfBuildings;
                    item.SalePrice = item.ContractPrice * item.NumberOfBuildings;
                }
                else
                {
                    item.ExtendedPrice = item.ListPrice * item.NumberOfBuildings;
                    item.SalePrice = item.ContractPrice * item.NumberOfBuildings;
                }
            }

            item.SalePrice += item.ProcessingCharges;
            if (item.SalesTax > 0)
            {
                item.SalePrice += item.SalePrice * (decimal)item.SalesTax / 100;
            }
        }

        /// <summary>
        /// get the low price b/w two
        /// </summary>
        /// <param name="firstPrice"></param>
        /// <param name="secondPrice"></param>
        /// <returns></returns>
        private decimal? Lowest(decimal? firstPrice, decimal? secondPrice)
        {
            if (firstPrice == 0 && secondPrice > 0) return secondPrice;
            if (firstPrice > 0 && secondPrice == 0) return firstPrice;
            if (firstPrice <= secondPrice) return firstPrice;
            if (secondPrice <= firstPrice) return secondPrice;
            return 0;
        }

        /// <summary>
        /// Calculate Commerce Server promotion price.
        /// </summary>
        /// <param name="items"></param>
        private static void CalculatePromotionPrice(List<BasketLineItemUpdated> items, TargetingValues targetingParam)
        {
            PricingLogger.LogInfo(LogCategory, "CalculatePromotionPrice => BEGIN");
            //Collect all item need to be reset promotion price.
            var itemsToResetPromotionPrice =
                items.Where(item => item.PromotionChangedIndicator && item.PromotionActiveIndicator).ToList();
            var itemPricings = PricingCalculator.Instance.CalculatePromotionPrice(itemsToResetPromotionPrice, targetingParam);
            if (itemPricings != null && itemPricings.Count > 0)
            {
                ReUpdateItemPromotionPrice(items, itemPricings);
            }
            PricingLogger.LogInfo(LogCategory, "CalculatePromotionPrice <= END");
        }

        /// <summary>
        /// Calculate Contract Price (SOP, TOLAS)
        /// </summary>
        /// <param name="items"></param>
        private static void CalculateContractPrice(List<BasketLineItemUpdated> items, int? overriedQtyForDiscount)
        {
            PricingLogger.LogInfo(LogCategory, "CalculateContractPrice => BEGIN");
            //Collect all item need to be reset contract price.
            var itemsToResetContractPrice = items.Where(item => item.ContractChangedIndicator).ToList();
            var itemPricings = PricingCalculator.Instance.CalculateContractPrice(itemsToResetContractPrice, overriedQtyForDiscount);
            if (itemPricings != null && itemPricings.Count > 0)
            {
                ReUpdateItemContractPrice(items, itemPricings);
            }
            PricingLogger.LogInfo(LogCategory, "CalculateContractPrice <= END");
        }

        /// <summary>
        /// Calculate List Price
        /// </summary>
        /// <param name="items"></param>
        private static void CalculateListPrice(IEnumerable<BasketLineItemUpdated> items)
        {
            PricingLogger.LogInfo(LogCategory, "CalculateListPrice => BEGIN");
            //Collect all item need to be reset list price.
            if (items == null) return;
            var itemsToResetListPrice = items.Where(item => item.ProductPriceChangedIndicator).ToList();
            var itemPricings = PricingCalculator.Instance.CalculateListPrice(itemsToResetListPrice);
            if (itemPricings != null && itemPricings.Count > 0)
            {
                PricingLogger.LogDebug(LogCategory,
                                       "==================ItemPricing after get list price:==================");
                foreach (var pricing in itemPricings)
                {
                    PricingLogger.LogDebug(LogCategory, pricing.ToString());
                }
                ReUpdateItemListPrice(items, itemPricings);
            }
            PricingLogger.LogInfo(LogCategory, "CalculateListPrice <= END");
        }

        /// <summary>
        /// Re update list price back to line item and reset PriceChangedIndicator
        /// </summary>
        /// <param name="items"></param>
        /// <param name="itemPricings"></param>
        private static void ReUpdateItemListPrice(IEnumerable<BasketLineItemUpdated> items, List<ItemPricing> itemPricings)
        {
            if (itemPricings == null || itemPricings.Count == 0) return;
            PricingLogger.LogDebug(LogCategory, "ReUpdateItemListPrice");
            foreach (var item in items)
            {
                var pricingItem = itemPricings.Where(x => x.BTKey == item.BTKey).FirstOrDefault();

                if (pricingItem != null)
                {
                    PricingLogger.LogDebug(LogCategory, pricingItem.ToString());
                    item.ListPrice = pricingItem.ListPrice;
                }
                item.QuantityChanged = true;
                item.ProductPriceChangedIndicator = false;
                PricingLogger.LogDebug(LogCategory, item.ToString());
            }
        }

        /// <summary>
        /// Re update contract price back to line item and reset ContractChangedIndicator
        /// </summary>
        /// <param name="items"></param>
        /// <param name="itemPricings"></param>
        private static void ReUpdateItemContractPrice(IEnumerable<BasketLineItemUpdated> items, List<ItemPricing> itemPricings)
        {
            if (itemPricings == null || itemPricings.Count == 0) return;
            PricingLogger.LogDebug(LogCategory, "ReUpdateItemContractPrice");
            foreach (var item in items)
            {
                var pricingItem = itemPricings.Where(x => x.BTKey == item.BTKey).FirstOrDefault();

                if (pricingItem != null)
                {
                    PricingLogger.LogDebug(LogCategory, pricingItem.ToString());
                    item.ContractPrice = pricingItem.ContractPrice;
                    item.ContractDiscountPercent = pricingItem.ContractDiscountPercent.HasValue
                                                       ? pricingItem.ContractDiscountPercent.Value
                                                       : 0;

                }
                item.QuantityChanged = true;
                item.ContractChangedIndicator = false;
                PricingLogger.LogDebug(LogCategory, item.ToString());
            }
        }

        /// <summary>
        /// Re update contract price back to line item and reset PromotionChangedIndicator
        /// </summary>
        /// <param name="items"></param>
        /// <param name="itemPricings"></param>
        private static void ReUpdateItemPromotionPrice(IEnumerable<BasketLineItemUpdated> items,
            List<ItemPricing> itemPricings)
        {
            if (itemPricings == null || itemPricings.Count == 0) return;
            PricingLogger.LogDebug(LogCategory, "ReUpdateItemPromotionPrice");
            foreach (var item in items)
            {
                var pricingItem = itemPricings.Where(x => x.BTKey == item.BTKey).FirstOrDefault();

                if (pricingItem != null)
                {
                    PricingLogger.LogDebug(LogCategory, pricingItem.ToString());
                    item.PromotionPrice = pricingItem.PromotionPrice;
                    item.PromotionDiscountPercent = pricingItem.PromotionDiscountPercent.HasValue
                                                        ? pricingItem.PromotionDiscountPercent.Value
                                                        : 0;
                    item.PromotionCode = pricingItem.PromotionCode;
                }
                item.QuantityChanged = true;
                //item.PromotionChangedIndicator = false;
            }
        }
        #endregion
    }
}
