using BT.TS360API.Cache;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BT.TS360API.Services
{
    public class AdvSearchController
    {
        private bool IsDirty { get; set; }
        private string CacheKey { get; set; }

        private AdvancedSearchSession _objSearch;
        private AdvancedSearchSession AdvSession
        {
            get
            {
                if (_objSearch == null)
                {
                    _objSearch = CachingController.Instance.Read(CacheKey) as AdvancedSearchSession ??
                                new AdvancedSearchSession();
                }

                return _objSearch;
            }
        }
        public AdvSearchController(string userId)
        {
            IsDirty = false;
            CacheKey = CommonHelper.Instance.GetCacheKeyFromSessionKey(userId, SessionVariableName.AdvancedSearchSession);
        }

        public bool IsUsingMyPreferencesValues
        {
            get { return AdvSession.IsUsingMyPreferencesValues; }
            set
            {
                if (!IsDirty && AdvSession.IsUsingMyPreferencesValues != value)
                    IsDirty = true;
                AdvSession.IsUsingMyPreferencesValues = value;
            }
        }

        public int CurrentSlectedTabIndex
        {
            get { return AdvSession.CurrentSlectedTabIndex; }
            set
            {
                if (!IsDirty && AdvSession.CurrentSlectedTabIndex != value)
                    IsDirty = true;
                AdvSession.CurrentSlectedTabIndex = value;
            }
        }

        public string OriginalProductType
        {
            get { return AdvSession.OriginalProductType; }
            set
            {
                if (!IsDirty && AdvSession.OriginalProductType != value)
                    IsDirty = true;
                AdvSession.OriginalProductType = value;
            }
        }

        public SearchExpressionGroup SearchTermsObj
        {
            get { return AdvSession.SearchTermsObj; }
            set
            {
                if (!IsDirty)
                    IsDirty = true;
                AdvSession.SearchTermsObj = value;
            }
        }

        public List<SearchExpression> SearchTerms
        {
            get { return AdvSession.SearchTerms; }
            set
            {
                if (!IsDirty)
                    IsDirty = true;
                AdvSession.SearchTerms = value;
            }
        }
        public string GetQueryStrFilter(string key)
        {
            return AdvSession.GetQueryStrFilter(key);
        }
        public Dictionary<string, string>.KeyCollection GetQueryStrFilterKey()
        {
            return AdvSession.GetQueryStrFilterKey();
        }
        public bool HasQueryStrFilter()
        {
            return AdvSession.HasQueryStrFilter();
        }

        public void AddQueryStrFilter(string key, string value)
        {
            if (!IsDirty)
                IsDirty = true;
            AdvSession.AddQueryStrFilter(key, value);
        }

        public void ResetAdvancedSearch()
        {
            if (!IsDirty)
                IsDirty = true;
            AdvSession.ResetAdvancedSearch();
        }

        public void ResetCriteria()
        {
            if (!IsDirty)
                IsDirty = true;
            AdvSession.ResetCriteria();
        }

        public void Load(SavedSearchObjDeserialize obj, NameValueCollection nv, string[] productType)
        {
            _objSearch = new AdvancedSearchSession();
            InitializeAdvancedSearchSession(obj, nv, productType);
            IsDirty = true;
        }

        private void InitializeAdvancedSearchSession(SavedSearchObjDeserialize obj, NameValueCollection nv, string[] productType)
        {
            //if (obj == null && nv == null)
            //{
            //    IsUsingMyPreferencesValues = true;
            //    CurrentSlectedTabIndex = -1;
            //    OriginalProductType = string.Empty;
            //    return;
            //}

            if (nv != null)
            {
                var strTerm = nv[SearchFieldNameConstants.SearchTerms];
                nv.Remove(SearchFieldNameConstants.SearchTerms);

                IsUsingMyPreferencesValues = true;
                CurrentSlectedTabIndex = -1;
                RemoveParametersFromQueryString(nv);

                var temp = nv[SearchFieldNameConstants.producttype];
                OriginalProductType = GetProductType(temp, productType);

                foreach (var key in nv.AllKeys)
                {
                    if (string.Compare(key, SearchFieldNameConstants.keyword, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        AddSearchTerm(string.Empty, string.Empty, SearchFieldNameConstants.keyword, nv[key]);
                    }
                    else
                    {
                        if (string.Compare(key, "excludereportcode", StringComparison.OrdinalIgnoreCase) == 0)
                            AddQueryStrFilter(SearchFieldNameConstants.excludereportcode, nv[key]);
                        else if (string.Compare(key, "excludeproductattribute", StringComparison.OrdinalIgnoreCase) == 0)
                            AddQueryStrFilter(SearchFieldNameConstants.excludeproductattribute, nv[key]);
                        else
                            AddQueryStrFilter(key, nv[key]);
                    }
                }

                var searchTerm = GetSearchTerms(strTerm);
                foreach (var exp in searchTerm)
                {
                    if (string.IsNullOrEmpty(exp.Terms) || string.IsNullOrEmpty(exp.Scope)) continue;

                    var param = new SearchExpression
                    {
                        Operator = exp.Operator,
                        ComparisionOperator = exp.ComparisionOperator ?? string.Empty,
                        Scope = exp.Scope,
                        Terms = exp.Terms
                    };
                    this.SearchTerms.Add(param);
                    this.SearchTermsObj.AddSearchExpress(param);
                }

            }
            else
            {
                IsUsingMyPreferencesValues = obj.IsUsingMyPreferencesValues;
                CurrentSlectedTabIndex = obj.CurrentSlectedTabIndex;
                OriginalProductType = GetProductType(obj.OriginalProductType, productType);

                if (obj.Criteria != null && obj.Criteria.Length > 0)
                {
                    var length = obj.Criteria.Length / 2;
                    for (var i = 0; i < length; i++)
                    {
                        var index = i * 2;
                        var key = obj.Criteria[index];
                        if (string.Compare(key, SearchFieldNameConstants.keyword, StringComparison.OrdinalIgnoreCase) == 0)
                            continue;

                        AddQueryStrFilter(key, obj.Criteria[index + 1]);
                    }
                }

                if (obj.SearchTerms != null && obj.SearchTerms.Length > 0)
                {
                    var length = obj.SearchTerms.Length / 4;
                    for (var i = 0; i < length; i++)
                    {
                        var index = i * 4;
                        var param = new SearchExpression
                        {
                            Operator = obj.SearchTerms[index + 3],
                            ComparisionOperator = obj.SearchTerms[index + 1],
                            Scope = obj.SearchTerms[index + 0],
                            Terms = obj.SearchTerms[index + 2]
                        };

                        if (string.IsNullOrEmpty(param.Terms) || string.IsNullOrEmpty(param.Scope)) continue;
                        this.SearchTerms.Add(param);
                        this.SearchTermsObj.AddSearchExpress(param);
                    }
                }
            }

            _objSearch.SearchFilter = HandleReviewPublication(_objSearch.SearchFilter);

            var dict = _objSearch.SearchFilter;
            if (dict.ContainsKey(SearchFieldNameConstants.ReviewJournalSource) && !dict.ContainsKey(SearchFieldNameConstants.ReviewJournalDateFromTo))
            {
                var date = DateTime.Now.AddYears(-5).ToString("d") + "|" + DateTime.Now.ToString("d");
                dict.Add(SearchFieldNameConstants.ReviewJournalDateFromTo, date);
            }
            if (dict.ContainsKey(SearchFieldNameConstants.BtProgramSource) && !dict.ContainsKey(SearchFieldNameConstants.BTProgramsDateFromTo))
            {
                var date = DateTime.Now.AddYears(-5).ToString("d") + "|" + DateTime.Now.ToString("d");
                dict.Add(SearchFieldNameConstants.BTProgramsDateFromTo, date);
            }
        }
        private void AddSearchTerm(string op, string comparision, string scope, string term)
        {
            if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(scope)) return;

            if (SearchTerms == null)
            {
                this.SearchTerms = new List<SearchExpression>();
                this.SearchTermsObj = new SearchExpressionGroup();
            }
            var param = new SearchExpression
            {
                Operator = op,
                ComparisionOperator = comparision,
                Scope = scope,
                Terms = term
            };
            this.SearchTerms.Add(param);
            this.SearchTermsObj.AddSearchExpress(param);
        }
        private static void RemoveParametersFromQueryString(NameValueCollection nv)
        {
            nv.Remove(QueryStringName.SEARCH_VIEW_PARA_NAME);
            nv.Remove(SearchQueryStringName.KEYWORD_FROM);
            nv.Remove(SearchQueryStringName.KEYWORD_SAVEDSEARCH_NAME);
            nv.Remove(QueryStringName.SEARCH_BASIC_PARA_NAME);
            nv.Remove(SearchQueryStringName.KEYWORD_SEARCH_ADVANCE);
            nv.Remove(SearchQueryStringName.KEYWORD_SEARCH_PREFERENCES);
            nv.Remove(QueryStringName.PAGE_PARAM_NAME);
            nv.Remove(QueryStringName.SEARCH_SORT_ORDER);
            nv.Remove(QueryStringName.SEARCH_SORT_BY);
            nv.Remove(QueryStringName.SEARCH_PAGE_SIZE);
            nv.Remove(QueryStringName.ProxiedUserId);
            nv.Remove(QueryStringName.SearchPageChangeView);

            //Remove parametter for Browse SearchResults pages
            //ReleaseCalendarProducts page
            nv.Remove(SearchFieldNameConstants.releaseday);
            nv.Remove(SearchFieldNameConstants.releasemonth);
            nv.Remove(SearchFieldNameConstants.releaseyear);
            nv.Remove(SearchFieldNameConstants.releaseproducttype);

            //PromotionProducts page
            nv.Remove(SearchFieldNameConstants.promotionid);

            //EListProducts page
            nv.Remove(SearchFieldNameConstants.elistid);

            //PublicationProducts page
            nv.Remove(SearchFieldNameConstants.publicationsubcategoryid);
            nv.Remove(SearchFieldNameConstants.publicationcategoryid);

            // view in search result from batch entry
            nv.Remove(SearchQueryStringName.KEYWORD_SEARCH_BATCH_ENTRY);
            //For WebTrends
            nv.Remove(QueryStringName.WebTrendAC);
            nv.Remove(QueryStringName.ResetAVCache);
            //remove because its codes are in session
            //nv.Remove(SearchFieldNameConstants.reviewcode);
            nv.Remove(SearchFieldNameConstants.reviewnonissued);
            nv.Remove(SearchFieldNameConstants.positivecode);
            nv.Remove(SearchFieldNameConstants.starredcode);
            nv.Remove(SearchFieldNameConstants.reviewissuecount);
            nv.Remove(SearchFieldNameConstants.btprogramcode);
            nv.Remove(SearchFieldNameConstants.ayprogramcode);
        }
        private static List<SearchExpression> GetSearchTerms(string text)
        {
            var searchTerms = new List<SearchExpression>();
            if (string.IsNullOrEmpty(text))
                return searchTerms;
            XElement ele = XElement.Parse(text);
            SearchExpression param;
            foreach (XElement child in ele.Elements())
            {
                param = new SearchExpression();
                foreach (XElement node in child.Elements())
                {
                    var val = node.Value;
                    var name = node.Name;
                    if (name.ToString().ToLower() == "operator")
                        param.Operator = val;
                    else if (name.ToString().ToLower() == "scope")
                    {
                        param.Scope = val;
                    }
                    else
                        if (name.ToString().ToLower() == "term")
                            param.Terms = val;
                        else

                            if (name.ToString().ToLower() == "name")
                                param.DisplayName = val;
                }
                searchTerms.Add(param);
            }

            return searchTerms;
        }
        private static Dictionary<string, string> HandleReviewPublication(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey(SearchFieldNameConstants.reviewpub)) return dict;
            var newDict = new Dictionary<string, string>(dict);

            var value = newDict[SearchFieldNameConstants.reviewpub];
            newDict.Remove(SearchFieldNameConstants.reviewpub);

            var values = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < values.Length; i++)
            {
                var level = values[i].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                values[i] = level[0];
            }
            var reviewTypes = ProductCatalogDAO.Instance.GetReviewType(values);

            var NIRVSC = AdvancedSearchFilter.NonIssuedReviewJournal.ToString();
            var Positive = AdvancedSearchFilter.PositiveReviews.ToString();
            var Starred = AdvancedSearchFilter.StarredReviews.ToString();
            var AYPRGM = AdvancedSearchFilter.AyPrograms.ToString();
            var op = String.Empty;
            foreach (var type in reviewTypes)
            {
                if (string.IsNullOrEmpty(type.Key) || string.IsNullOrEmpty(type.Value)) continue;

                var key = string.Empty;
                switch (type.Key)
                {
                    case "RVSC":
                        key = SearchFieldNameConstants.ReviewJournalSource;
                        break;
                    case "NIRVSC":
                        key = NIRVSC;
                        op = AdvancedSearchFilter.IssuedReviewJournalOperator.ToString();
                        break;
                    case "Positive":
                        key = Positive;
                        op = AdvancedSearchFilter.NonIssuedReviewJournalOperator.ToString();
                        break;
                    case "Starred":
                        key = Starred;
                        op = AdvancedSearchFilter.PositiveReviewsOperator.ToString();
                        break;
                    case "PRGM":
                        key = SearchFieldNameConstants.BtProgramSource;
                        break;
                    case "AYPRGM":
                        key = AYPRGM;
                        op = AdvancedSearchFilter.BtProgramsOperator.ToString();
                        break;
                }
                if (string.IsNullOrEmpty(key)) continue;
                if (newDict.ContainsKey(key))
                {
                    newDict[key] += "|" + type.Value;
                }
                else
                {
                    newDict.Add(key, type.Value);
                    if (key != SearchFieldNameConstants.ReviewJournalSource && key != SearchFieldNameConstants.BtProgramSource
                        && !newDict.ContainsKey(op))
                        newDict.Add(op, BooleanOperatorConstants.Or);
                }
            }

            return newDict;
        }
        public SavedSearchObj ToSearchCriteria()
        {
            var obj = new SavedSearchObj
            {
                IsUsingMyPreferencesValues = this.IsUsingMyPreferencesValues,
                CurrentSlectedTabIndex = this.CurrentSlectedTabIndex,
                OriginalProductType = this.OriginalProductType
            };

            var index = 0;
            var dic = HandleReviewPublication(_objSearch.SearchFilter);
            if (dic.Count > 0)
            {

                var count = dic.Count;
                obj.Criteria = new string[count, 2];
                foreach (var str in dic)
                {
                    if (string.Compare(str.Key, SearchFieldNameConstants.keyword, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        AddSearchTerm(string.Empty, string.Empty, SearchFieldNameConstants.keyword, str.Value);
                    }
                    obj.Criteria[index, 0] = str.Key;
                    obj.Criteria[index, 1] = str.Value;
                    index++;
                }
            }

            if (SearchTerms != null && SearchTerms.Count > 0)
            {
                var countSearchTerms = this.SearchTerms.Count;
                obj.SearchTerms = new string[countSearchTerms, 4];
                index = 0;
                foreach (var str in this.SearchTerms)
                {
                    obj.SearchTerms[index, 0] = str.Scope;
                    obj.SearchTerms[index, 1] = str.ComparisionOperator;
                    obj.SearchTerms[index, 2] = str.Terms;
                    obj.SearchTerms[index, 3] = str.Operator;
                    index++;
                }
            }
            return obj;
        }

        private string GetProductType(string str, string[] prodType)
        {
            var result = ProductTypeConstants.Book;
            if (string.IsNullOrEmpty(str))
            {
                //string[] prodType = SiteContext.Current.ProductType;
                //visible tab if based on user's product type
                if (CommonHelper.Instance.IsStringInList(ProductTypeConstants.Book, prodType))
                    result = ProductTypeConstants.Book;
                else if (CommonHelper.Instance.IsStringInList(ProductTypeConstants.Music, prodType))
                    result = ProductTypeConstants.Music;
                else if (CommonHelper.Instance.IsStringInList(ProductTypeConstants.Movie, prodType))
                    result = ProductTypeConstants.Movie;
            }
            else
            {
                if (str.IndexOf(ProductTypeConstants.Book, StringComparison.OrdinalIgnoreCase) != -1)
                    result = ProductTypeConstants.Book;
                else if (str.IndexOf(ProductTypeConstants.Music, StringComparison.OrdinalIgnoreCase) != -1)
                    result = ProductTypeConstants.Music;
                else if (str.IndexOf(ProductTypeConstants.Movie, StringComparison.OrdinalIgnoreCase) != -1)
                    result = ProductTypeConstants.Movie;
            }
            return result;
        }

        public void SetDirty()
        {
            IsDirty = true;
        }
        public void CommitChanges()
        {
            if (IsDirty)
            {
                CachingController.Instance.Write(CacheKey, AdvSession);
                IsDirty = false;
            }
        }
    }
}