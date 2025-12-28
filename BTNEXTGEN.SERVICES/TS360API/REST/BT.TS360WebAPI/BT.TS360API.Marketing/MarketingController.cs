using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BT.TS360API.Marketing.DataAccess;
using BT.TS360API.ServiceContracts;

namespace BT.TS360API.Marketing
{
    public class MarketingController
    {
        public static List<BtKeyDiscountObject> GetDiscountsForMultipleItem(List<PromotionClientArg> promotionArgList, TargetingValues siteContext)
        {
            var results = new List<BtKeyDiscountObject>();

            var activeDis = MarketingDAOManager.Instance.GetApprovedDiscounts();

            if (activeDis == null || activeDis.Count == 0) return new List<BtKeyDiscountObject>();

            const bool isVirtualCatalog = true;
            var listProducts = ConvertToBtKeyProduct(promotionArgList, isVirtualCatalog);
            var parentCatFromDb = CsProductCatalogDAO.Instance.GetParentCategories(listProducts, isVirtualCatalog);

            if (parentCatFromDb == null || parentCatFromDb.Count == 0) return new List<BtKeyDiscountObject>();

            //var targetingText = MarketingHelper.Instance.GenerateTargetingValues(siteContext);
            var targetingParam = MarketingHelper.Instance.ToTargetingParam(siteContext);

            foreach (var promotionClientArg in promotionArgList)
            {
                var parentCat = parentCatFromDb.Find(item => item.BTKey == promotionClientArg.BTKey);

                if (parentCat == null) continue;

                var dis = MarketingHelper.Instance.GetAppropriateDiscount(activeDis, targetingParam, parentCat);
                if (dis == null) continue;

                var disCountObject = new BtKeyDiscountObject();
                disCountObject.BTKey = promotionClientArg.BTKey;
                disCountObject.DiscountName = string.Concat("#", dis.BasketDisplay.DisplayText);

                results.Add(disCountObject);
            }

            return results;
        }

        public static string GeneratePigNameAllValue(string pigName)
        {
            const string delimiter = ";";
            var lstPig = ProfileDAOManager.Instance.GetProductInterestGroup();// ProfileController.Current.GetProductInterestGroup();
            if (lstPig == null || lstPig.Count == 0) return pigName;

            var result = new StringBuilder();
            result.AppendFormat("{0}{1}", MarketingHelper.TargetingContextAll, delimiter);
            foreach (var btProductInterestGroup in lstPig)
            {
                result.AppendFormat("{0}{1}", btProductInterestGroup.PIGName, delimiter);
            }
            return result.ToString();
        }

        public static string ConvertToMultipleValue(string[] items)
        {
            const string delimiter = ";";

            if (items == null || !items.Any())
                return string.Empty;

            string list = MarketingHelper.TargetingContextAll + delimiter;
            for (int i = 0; i < items.Count(); i++)
            {
                if (i == items.Count() - 1)
                    list += items[i];
                else
                    list += items[i] + delimiter;
            }

            return list;
        }

        public static Collection<CampaignAdItem> GetCampaignAdItem(string pageGroupName)
        {
            return GetCampaignAdItem(pageGroupName, null);
        }

        public static Collection<CampaignAdItem> GetCampaignAdItem(string pageGroupName, TargetingParam targetingParam)
        {
            var activeAds = MarketingDAOManager.Instance.GetApprovedAds();

            if (activeAds == null || activeAds.Count == 0) return new Collection<CampaignAdItem>();

            var appropriatedAds = new List<MarketingApprovedAd>();
            foreach (var marketingApprovedAd in activeAds)
            {
                if (marketingApprovedAd.PageGroupList == null)
                {
                    appropriatedAds.Add(marketingApprovedAd);
                    continue;
                }

                var pgStr = ConvertToMultipleValue(marketingApprovedAd.PageGroupList.ToArray());

                if (pgStr.IndexOf("ALL") != -1||pgStr.IndexOf(pageGroupName) != -1)
                {
                    appropriatedAds.Add(marketingApprovedAd);
                }
            }

            if (appropriatedAds.Count == 0) return new Collection<CampaignAdItem>();

            if (targetingParam == null)
            {
                return MarketingHelper.Instance.ConvertListToCollection(appropriatedAds);
            }

            var results = MarketingHelper.Instance.GetAppropriateAds(appropriatedAds, targetingParam);
            if (results == null || results.Count == 0) return new Collection<CampaignAdItem>(); ;

            return MarketingHelper.Instance.ConvertListToCollection(results);
        }


        #region Private

        private static List<BtKeyCatalogObject> ConvertToBtKeyProduct(IEnumerable<PromotionClientArg> promotionArgList, bool isVirtualCatalog)
        {
            var results = new List<BtKeyCatalogObject>();

            var dictBtKey = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            foreach (var promotionClientArg in promotionArgList)
            {
                if (dictBtKey.ContainsKey(promotionClientArg.BTKey)) continue;

                var catalogName = isVirtualCatalog ? "MarketingCatalog" : promotionClientArg.Catalog;
                results.Add(new BtKeyCatalogObject(promotionClientArg.BTKey, catalogName, promotionClientArg.Catalog));

                dictBtKey.Add(promotionClientArg.BTKey, true);
            }
            return results;
        }

        #endregion
    }
}
