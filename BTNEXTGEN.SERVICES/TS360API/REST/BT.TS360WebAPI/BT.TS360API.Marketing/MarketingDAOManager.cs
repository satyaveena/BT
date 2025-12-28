using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BT.TS360API.Cache;
using BT.TS360API.Marketing.DataAccess;
using BT.TS360API.ServiceContracts;

namespace BT.TS360API.Marketing
{
    public class MarketingDAOManager
    {
        private const int BasketDisplayTableIndex = 8;
        private const int TargetTableIndex = 6;
        private const int EligibilityTableIndex = 7;
        private const int PageGroupTableIndex = 5;
        private const int MarketingCacheDuration = 30; // in minutes

        private MarketingDAOManager()
        { }

        private static volatile MarketingDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        public static MarketingDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new MarketingDAOManager();
                }

                return _instance;
            }
        }

        public List<MarketingApprovedDiscount> GetApprovedDiscounts()
        {
            var ds = MarketingDAO.Instance.GetApprovedDiscounts();
            if(ds == null || ds.Tables.Count == 0) return null;

            var approvedDiscounts = GetDiscountFromDataSet(ds);

            return approvedDiscounts;
        }

        private List<MarketingApprovedDiscount> GetDiscountFromDataSet(DataSet ds)
        {
            var approvedDiscounts = new List<MarketingApprovedDiscount>();

            var dtDiscount = ds.Tables[0];

            foreach (DataRow dataRow in dtDiscount.Rows)
            {
                var dis = new MarketingApprovedDiscount();

                var discountIdx = DataAccessHelper.ConvertToInt(dataRow["idx"]);

                dis.DiscountIndex = discountIdx;
                dis.DiscountName = DataAccessHelper.ConvertToString(dataRow["name"]);
                dis.AwardCatalog = DataAccessHelper.ConvertToString(dataRow["award_catalog"]);
                dis.AwardCategory = DataAccessHelper.ConvertToString(dataRow["award_category"]);
                dis.AwardProduct = DataAccessHelper.ConvertToString(dataRow["award_product"]);
                dis.AwardExpressionId = DataAccessHelper.ConvertToInt(dataRow["award_expr"]);

                dis.BasketDisplay = GetBasketDisplay(dis.DiscountIndex, ds.Tables[BasketDisplayTableIndex]);

                var offerType = DataAccessHelper.ConvertToInt(dataRow["offer_type"]);
                dis.OfferType = offerType == 1 ? OfferType.AmountOff : OfferType.PercentOff;

                dis.OfferValue = DataAccessHelper.ConvertTodecimal(dataRow["offer_value"]);

                var convertToDateTime = DataAccessHelper.ConvertToDateTime(dataRow["date_start"]);
                if (convertToDateTime != null)
                    dis.StartDate = convertToDateTime.Value;

                convertToDateTime = DataAccessHelper.ConvertToDateTime(dataRow["date_end"]);
                if (convertToDateTime != null)
                    dis.EndDate = convertToDateTime.Value;

                dis.TargetingExpressions = GetTargetingExpression(discountIdx, ds.Tables[TargetTableIndex]);

                dis.EligibilityExpressions = GetEligibilityExpressions(discountIdx, ds.Tables[EligibilityTableIndex]);

                approvedDiscounts.Add(dis);
            }

            return approvedDiscounts;
        }

        private List<string> GetEligibilityExpressions(int discountIdx, DataTable dataTable)
        {
            var results = new List<string>();
            var rows = dataTable.Select(string.Format("re_idx={0}", discountIdx));

            foreach (var dataRow in rows)
            {
                var eligibilityExpr = DataAccessHelper.ConvertToString(dataRow["re_expr_body"]);
                if (!string.IsNullOrEmpty(eligibilityExpr))
                {
                    results.Add(eligibilityExpr);
                }
            }

            return results;
        }

        private static BasketDisplay GetBasketDisplay(int discountIndex, DataTable dataTable)
        {
            DataRow[] foundRows = dataTable.Select("bd_idx=" + discountIndex);

            if (foundRows.Length == 0)
            {
                return null;
            }

            var basketDisplay = new BasketDisplay();
            foreach (DataRow row in foundRows)
            {
                basketDisplay.Culture = DataAccessHelper.ConvertToString(row["culture"]);
                basketDisplay.DiscountIndex = discountIndex;
                basketDisplay.DisplayText = DataAccessHelper.ConvertToString(row["display"]);
            }
            return basketDisplay;
        }

        public string GetExpressionBodyById(int exprId)
        {
            return MarketingDAO.Instance.GetExpressionBodyById(exprId);
        }

        public List<MarketingApprovedAd> GetApprovedAds()
        {
            const string maketingApprovedAdsCacheKey = "___GetApprovedAds";

            var approvedAds =
                CachingController.Instance.Read(maketingApprovedAdsCacheKey) as List<MarketingApprovedAd>;

            if (approvedAds != null) return approvedAds;

            var ds = MarketingDAO.Instance.GetApprovedAds();
            if (ds == null || ds.Tables.Count == 0) return null;

            approvedAds = GetAdsFromDataSet(ds);

            //CachingController.Instance.Write(maketingApprovedAdsCacheKey, approvedAds, MarketingCacheDuration);
            return approvedAds;
        }

        private List<MarketingApprovedAd> GetAdsFromDataSet(DataSet ds)
        {
            var approvedAds = new List<MarketingApprovedAd>();

            var dtDiscount = ds.Tables[0];

            foreach (DataRow dataRow in dtDiscount.Rows)
            {
                var ad = new MarketingApprovedAd();

                var idx = DataAccessHelper.ConvertToInt(dataRow["idx"]);

                ad.AdIndex = idx;
                ad.AdName = DataAccessHelper.ConvertToString(dataRow["name"]);
                ad.PageGroupList = GetAdPageGroupList(idx, ds.Tables[PageGroupTableIndex]);
                ad.TargetingExpressions = GetTargetingExpression(idx, ds.Tables[TargetTableIndex]);
                ad.EligibilityExpressions = GetEligibilityExpressions(idx, ds.Tables[EligibilityTableIndex]);
                approvedAds.Add(ad);
            }

            return approvedAds;
        }

        private List<string> GetAdPageGroupList(int idx, DataTable dataTable)
        {
            var rows = dataTable.Select(string.Format("pg_idx={0}", idx));

            return rows.Select(dataRow => DataAccessHelper.ConvertToString(dataRow["pg_tag"])).ToList();
        }

        private List<TargetingItemAction> GetTargetingExpression(int idx, DataTable dataTable)
        {
            var results = new List<TargetingItemAction>();
            var rows = dataTable.Select(string.Format("ta_idx={0}", idx));

            foreach (var dataRow in rows)
            {
                var adAction =
                    (TargetingAction)
                        Enum.ToObject(typeof (TargetingAction), DataAccessHelper.ConvertToInt(dataRow["ta_action"]));

                var adTa = new TargetingItemAction
                           {
                               TargetingAction = adAction,
                               TargetingBody = DataAccessHelper.ConvertToString(dataRow["ta_expr_body"])
                           };

                results.Add(adTa);
            }

            return results;
        }
    }
}
