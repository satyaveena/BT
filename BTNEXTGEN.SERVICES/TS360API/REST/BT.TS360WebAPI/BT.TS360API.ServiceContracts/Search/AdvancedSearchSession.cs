using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts.Search
{
    public class AdvancedSearchSession
    {
        public Dictionary<string, string> SearchFilter { get; set; }

        public bool IsUsingMyPreferencesValues { get; set; }
        public int CurrentSlectedTabIndex { get; set; }

        public string OriginalProductType { get; set; }

        public SearchExpressionGroup SearchTermsObj { get; set; }
        public List<SearchExpression> SearchTerms { get; set; }

        public AdvancedSearchSession()
        {
            //SAVED_SEARCH_NAME = name ?? string.Empty;
            //IsFromSearchResult = false;
            SearchFilter = new Dictionary<string, string>();
            //Facets = new List<KeyValuePair<string, string>>();
            this.SearchTerms = new List<SearchExpression>();
            this.SearchTermsObj = new SearchExpressionGroup();

            IsUsingMyPreferencesValues = true;
            CurrentSlectedTabIndex = -1;
            OriginalProductType = string.Empty;
        }

        public string GetQueryStrFilter(string key)
        {
            var dic = SearchFilter;
            if (dic.ContainsKey(key))
                return dic[key];
            return string.Empty;
        }
        public void AddQueryStrFilter(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) return;
            var dic = SearchFilter;
            if (string.IsNullOrEmpty(value))
            {
                if (dic.ContainsKey(key))
                    dic.Remove(key);
            }
            else if (dic.ContainsKey(key))
                dic[key] = value;
            else
            {
                dic.Add(key, value);
            }
        }
        public bool HasQueryStrFilter()
        {
            return SearchFilter.Count > 0;
        }
        public Dictionary<string, string>.KeyCollection GetQueryStrFilterKey()
        {
            return SearchFilter.Keys;
        }
        public void ResetAdvancedSearch()
        {
            ResetCriteria();
            if (SearchTerms != null) this.SearchTerms.Clear();
            this.SearchTermsObj = new SearchExpressionGroup();
            OriginalProductType = string.Empty;
            IsUsingMyPreferencesValues = true;
        }

        public void ResetCriteria()
        {
            if (SearchFilter != null) SearchFilter.Clear();
            //if (Facets != null) Facets.Clear();
        }

        /// <summary>
        /// These field will not be searched
        /// </summary>
        private static readonly HashSet<string> SkipKeys = new HashSet<string>()
                                            {
                                                AdvancedSearchFilter.NonIssuedReviewJournalOperator.ToString()
                                                ,AdvancedSearchFilter.NonIssuedReviewJournalOperator.ToString()
                                                ,AdvancedSearchFilter.PositiveReviewsOperator.ToString()
                                                ,AdvancedSearchFilter.BtProgramsOperator.ToString()
                                                ,AdvancedSearchFilter.StarredReviewsOperator.ToString()
                                                ,AdvancedSearchFilter.IssuedReviewJournalOperator.ToString()
                                                ,SearchFieldNameConstants.ReviewJournalDateFromTo
                                                ,AdvancedSearchFilter.MinimumReviewsPerTitle.ToString()
                                                ,AdvancedSearchFilter.StarredReviews.ToString()
                                                ,AdvancedSearchFilter.PositiveReviews.ToString()
                                                ,AdvancedSearchFilter.NonIssuedReviewJournal.ToString()
                                                ,AdvancedSearchFilter.AyPrograms.ToString()
                                                ,SearchFieldNameConstants.BtProgramSource
                                                ,SearchFieldNameConstants.ReviewJournalSource
                                                ,SearchFieldNameConstants.BTProgramsDateFromTo
                                                ,SearchFieldNameConstants.includepurchaseoption
                                                ,SearchFieldNameConstants.excludepurchaseoption
                                                ,SearchFieldNameConstants.includereportcode
                                                ,SearchFieldNameConstants.excludereportcode
                                                ,SearchFieldNameConstants.excludeparentaladvisory
                                                ,SearchFieldNameConstants.includeproductattribute
                                                ,SearchFieldNameConstants.excludeproductattribute
                                                ,SearchFieldNameConstants.audience
                                                ,QueryStringName.WebTrendAC
                                                ,QueryStringName.ResetAVCache
                                                ,SearchFieldNameConstants.includedproducttype
                                                ,SearchFieldNameConstants.excludedproducttype
                                            };
        public static bool Skip(string key)
        {
            return SkipKeys.Contains(key);
        }

    }

    public class SavedSearchObj
    {
        public int CurrentSlectedTabIndex { get; set; }
        public bool IsUsingMyPreferencesValues { get; set; }
        public string[,] Criteria { get; set; }
        public string OriginalProductType { get; set; }
        public string[,] SearchTerms { get; set; }
    }
    public class SavedSearchObjDeserialize
    {
        public int CurrentSlectedTabIndex { get; set; }
        public bool IsUsingMyPreferencesValues { get; set; }
        public string[] Criteria { get; set; }
        public string OriginalProductType { get; set; }
        public string[] SearchTerms { get; set; }
    }

}
