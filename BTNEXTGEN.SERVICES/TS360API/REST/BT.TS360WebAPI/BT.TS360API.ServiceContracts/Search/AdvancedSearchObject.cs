//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Web;
//using System.Web.Script.Serialization;
//using System.Xml.Linq;
//using BT.TS360API.Common.Search;

namespace BT.TS360API.ServiceContracts.Search
{
    //using Constants;
    //using Search;
    //using Helpers;
    //public class AdvancedSearchObject
    //{
    //    private BookAdvancedSearch _book;
    //    private MovieAdvancedSearch _movie;
    //    private MusicAdvancedSearch _music;
    //    private Dictionary<string, string> SearchFilter;
    //    private List<KeyValuePair<string,string>> Facets;

    //    public bool IsUsingMyPreferencesValues { get; set; }
    //    public bool IsFromSearchResult { get; set; }
    //    public int CurrentSlectedTabIndex { get; set; }

    //    public string OriginalProductType
    //    {
    //        get;
    //        set;
    //    }

    //    public SearchExpressionGroup SearchTermsObj { get; set; }
    //    public List<SearchExpression> SearchTerms { get; set; }
    //    public string SAVED_SEARCH_NAME { get; set; }

    //    public BookAdvancedSearch Book
    //    {
    //        get
    //        {
    //            if (_book == null)
    //                _book = new BookAdvancedSearch();
    //            return _book;
    //        }
    //        set { _book = value; }
    //    }

    //    public MovieAdvancedSearch Movie
    //    {
    //        get
    //        {
    //            if (_movie == null)
    //                _movie = new MovieAdvancedSearch();
    //            return _movie;
    //        }
    //        set { _movie = value; }
    //    }
    //    public MusicAdvancedSearch Music
    //    {
    //        get
    //        {
    //            if (_music == null)
    //                _music = new MusicAdvancedSearch();
    //            return _music;
    //        }
    //        set { _music = value; }
    //    }


    //    public AdvancedSearchSession()
    //        : this(string.Empty, string.Empty)
    //    {
    //    }
    //    public AdvancedSearchSession(string name, string criteria, int? from = 3)
    //    {
    //        SAVED_SEARCH_NAME = name ?? string.Empty;
    //        IsFromSearchResult = false;
    //        SearchFilter = new Dictionary<string, string>();
    //        Facets=new List<KeyValuePair<string, string>>();
    //        this.SearchTerms = new List<SearchExpression>();
    //        this.SearchTermsObj = new SearchExpressionGroup();

    //        if (string.IsNullOrEmpty(criteria))
    //        {
    //            IsUsingMyPreferencesValues = true;
    //            CurrentSlectedTabIndex = -1;
    //            OriginalProductType = string.Empty;
    //            return;
    //        }

    //        if (from != 3)
    //        {
    //            string advanceTerm;
    //            string searchCriteria;
    //            RemoveStructureCriteria(criteria, out advanceTerm, out searchCriteria);

    //            var nv = HttpUtility.ParseQueryString(criteria);
    //            var strTerm = nv[SearchFieldNameConstants.SearchTerms];
    //            nv.Remove(SearchFieldNameConstants.SearchTerms);

    //            IsUsingMyPreferencesValues = true;
    //            CurrentSlectedTabIndex = -1;
    //            RemoveParametersFromQueryString(nv);

    //            var temp = nv[SearchFieldNameConstants.producttype];
    //            OriginalProductType = GetProductType(temp);

    //            foreach (var key in nv.AllKeys)
    //            {
    //                if (string.Compare(key, SearchFieldNameConstants.keyword, StringComparison.OrdinalIgnoreCase) == 0)
    //                {
    //                    AddSearchTerm(string.Empty, string.Empty, SearchFieldNameConstants.keyword, nv[key]);
    //                }
    //                else
    //                {
    //                    if (string.Compare(key, "excludereportcode", StringComparison.OrdinalIgnoreCase) == 0)
    //                        AddQueryStrFilter(SearchFieldNameConstants.excludereportcode, nv[key]);
    //                    else if (string.Compare(key, "excludeproductattribute", StringComparison.OrdinalIgnoreCase) == 0)
    //                        AddQueryStrFilter(SearchFieldNameConstants.excludeproductattribute, nv[key]);
    //                    else
    //                        AddQueryStrFilter(key, nv[key]);
    //                }
    //            }

    //            var searchTerm = GetSearchTerms(strTerm);
    //            foreach (var exp in searchTerm)
    //            {
    //                if (string.IsNullOrEmpty(exp.Terms) || string.IsNullOrEmpty(exp.Scope)) continue;

    //                var param = new SearchExpression
    //                            {
    //                                Operator = exp.Operator,
    //                                ComparisionOperator = exp.ComparisionOperator ?? string.Empty,
    //                                Scope = exp.Scope,
    //                                Terms = exp.Terms
    //                            };
    //                this.SearchTerms.Add(param);
    //                this.SearchTermsObj.AddSearchExpress(param);
    //            }

    //        }
    //        else
    //        {
    //            var serializer = new JavaScriptSerializer();
    //            var obj = serializer.Deserialize<SavedSearchObjDeserialize>(criteria);

    //            IsUsingMyPreferencesValues = obj.IsUsingMyPreferencesValues;
    //            CurrentSlectedTabIndex = obj.CurrentSlectedTabIndex;
    //            OriginalProductType = GetProductType(obj.OriginalProductType);

    //            if (obj.Criteria != null && obj.Criteria.Length > 0)
    //            {
    //                var length = obj.Criteria.Length / 2;
    //                for (var i = 0; i < length; i++)
    //                {
    //                    var index = i * 2;
    //                    var key = obj.Criteria[index];
    //                    if (string.Compare(key, SearchFieldNameConstants.keyword, StringComparison.OrdinalIgnoreCase) == 0)
    //                        continue;

    //                    AddQueryStrFilter(key, obj.Criteria[index + 1]);
    //                }
    //            }

    //            if (obj.SearchTerms != null && obj.SearchTerms.Length > 0)
    //            {
    //                var length = obj.SearchTerms.Length / 4;
    //                for (var i = 0; i < length; i++)
    //                {
    //                    var index = i * 4;
    //                    var param = new SearchExpression
    //                    {
    //                        Operator = obj.SearchTerms[index + 3],
    //                        ComparisionOperator = obj.SearchTerms[index + 1],
    //                        Scope = obj.SearchTerms[index + 0],
    //                        Terms = obj.SearchTerms[index + 2]
    //                    };

    //                    if (string.IsNullOrEmpty(param.Terms) || string.IsNullOrEmpty(param.Scope)) continue;
    //                    this.SearchTerms.Add(param);
    //                    this.SearchTermsObj.AddSearchExpress(param);
    //                }
    //            }
    //        }

    //        SearchFilter = HandleReviewPublication(SearchFilter);

    //        var dict = SearchFilter;
    //        if (dict.ContainsKey(SearchFieldNameConstants.ReviewJournalSource) && !dict.ContainsKey(SearchFieldNameConstants.ReviewJournalDateFromTo))
    //        {
    //            var date = DateTime.Now.AddYears(-5).ToString("d") + "|" + DateTime.Now.ToString("d");
    //            dict.Add(SearchFieldNameConstants.ReviewJournalDateFromTo, date);
    //        }
    //        if (dict.ContainsKey(SearchFieldNameConstants.BtProgramSource) && !dict.ContainsKey(SearchFieldNameConstants.BTProgramsDateFromTo))
    //        {
    //            var date = DateTime.Now.AddYears(-5).ToString("d") + "|" + DateTime.Now.ToString("d");
    //            dict.Add(SearchFieldNameConstants.BTProgramsDateFromTo, date);
    //        }
    //    }
    //    public void AddFacet(string key, string value)
    //    {
    //        if (string.IsNullOrEmpty(key)||string.IsNullOrEmpty(value)) return;
    //       Facets.Add(new KeyValuePair<string, string>(key,value));
    //    }
    //    public List<KeyValuePair<string,string>> GetFacets()
    //    {
    //        return Facets;
    //    }
    //    public string GetQueryStrFilter(string key)
    //    {
    //        var dic = SearchFilter;
    //        if (dic.ContainsKey(key))
    //            return dic[key];
    //        return string.Empty;
    //    }
    //    public void AddQueryStrFilter(string key, string value)
    //    {
    //        if (string.IsNullOrEmpty(key)) return;
    //        var dic = SearchFilter;
    //        if (string.IsNullOrEmpty(value))
    //        {
    //            if (dic.ContainsKey(key))
    //                dic.Remove(key);
    //        }
    //        else if (dic.ContainsKey(key))
    //            dic[key] = value;
    //        else
    //        {
    //            dic.Add(key, value);
    //        }
    //    }
    //    public bool HasQueryStrFilter()
    //    {
    //        return SearchFilter.Count > 0;
    //    }
    //    public Dictionary<string, string>.KeyCollection GetQueryStrFilterKey()
    //    {
    //        return SearchFilter.Keys;
    //    }
    //    public void ResetAdvancedSearch()
    //    {
    //        ResetCriteria();
    //        if (SearchTerms != null) this.SearchTerms.Clear();
    //        this.SearchTermsObj = new SearchExpressionGroup();
    //        OriginalProductType = string.Empty;
    //        IsUsingMyPreferencesValues = true;
    //    }

    //    public void ResetCriteria()
    //    {
    //        if (SearchFilter != null) SearchFilter.Clear();
    //        if (Facets != null) Facets.Clear();
    //    }
    //    public string ToSearchCriteria()
    //    {
    //        var obj = new SavedSearchObj
    //                  {
    //                      IsUsingMyPreferencesValues = this.IsUsingMyPreferencesValues,
    //                      CurrentSlectedTabIndex = this.CurrentSlectedTabIndex,
    //                      OriginalProductType = this.OriginalProductType
    //                  };

    //        var index = 0;
    //        var dic = HandleReviewPublication(SearchFilter);
    //        if (dic.Count > 0)
    //        {

    //            var count = dic.Count;
    //            obj.Criteria = new string[count, 2];
    //            foreach (var str in dic)
    //            {
    //                if (string.Compare(str.Key, SearchFieldNameConstants.keyword, StringComparison.OrdinalIgnoreCase) == 0)
    //                {
    //                    AddSearchTerm(string.Empty, string.Empty, SearchFieldNameConstants.keyword, str.Value);
    //                }
    //                obj.Criteria[index, 0] = str.Key;
    //                obj.Criteria[index, 1] = str.Value;
    //                index++;
    //            }
    //        }

    //        if (SearchTerms != null && SearchTerms.Count > 0)
    //        {
    //            var countSearchTerms = this.SearchTerms.Count;
    //            obj.SearchTerms = new string[countSearchTerms, 4];
    //            index = 0;
    //            foreach (var str in this.SearchTerms)
    //            {
    //                obj.SearchTerms[index, 0] = str.Scope;
    //                obj.SearchTerms[index, 1] = str.ComparisionOperator;
    //                obj.SearchTerms[index, 2] = str.Terms;
    //                obj.SearchTerms[index, 3] = str.Operator;
    //                index++;
    //            }
    //        }

    //        var serializer = new JavaScriptSerializer();
    //        return serializer.Serialize(obj);
    //    }
    //    /// <summary>
    //    /// These field will not be searched
    //    /// </summary>
    //    private static readonly HashSet<string> SkipKeys = new HashSet<string>()
    //                                        {
    //                                            AdvancedSearchFilter.NonIssuedReviewJournalOperator.ToString()
    //                                            ,AdvancedSearchFilter.NonIssuedReviewJournalOperator.ToString()
    //                                            ,AdvancedSearchFilter.PositiveReviewsOperator.ToString()
    //                                            ,AdvancedSearchFilter.BtProgramsOperator.ToString()
    //                                            ,AdvancedSearchFilter.StarredReviewsOperator.ToString()
    //                                            ,AdvancedSearchFilter.IssuedReviewJournalOperator.ToString()
    //                                            ,SearchFieldNameConstants.ReviewJournalDateFromTo
    //                                            ,AdvancedSearchFilter.MinimumReviewsPerTitle.ToString()
    //                                            ,AdvancedSearchFilter.StarredReviews.ToString()
    //                                            ,AdvancedSearchFilter.PositiveReviews.ToString()
    //                                            ,AdvancedSearchFilter.NonIssuedReviewJournal.ToString()
    //                                            ,AdvancedSearchFilter.AyPrograms.ToString()
    //                                            ,SearchFieldNameConstants.BtProgramSource
    //                                            ,SearchFieldNameConstants.ReviewJournalSource
    //                                            ,SearchFieldNameConstants.BTProgramsDateFromTo
    //                                            ,SearchFieldNameConstants.includepurchaseoption
    //                                            ,SearchFieldNameConstants.excludepurchaseoption
    //                                            ,SearchFieldNameConstants.includereportcode
    //                                            ,SearchFieldNameConstants.excludereportcode
    //                                            ,SearchFieldNameConstants.excludeparentaladvisory
    //                                            ,SearchFieldNameConstants.includeproductattribute
    //                                            ,SearchFieldNameConstants.excludeproductattribute
    //                                            ,QueryStringName.WebTrendAC
    //                                            ,QueryStringName.ResetAVCache
    //                                        };
    //    public static bool Skip(string key)
    //    {
    //        return SkipKeys.Contains(key);
    //    }
    //    private void AddSearchTerm(string op, string comparision, string scope, string term)
    //    {
    //        if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(scope)) return;

    //        if (SearchTerms == null)
    //        {
    //            this.SearchTerms = new List<SearchExpression>();
    //            this.SearchTermsObj = new SearchExpressionGroup();
    //        }
    //        var param = new SearchExpression
    //        {
    //            Operator = op,
    //            ComparisionOperator = comparision,
    //            Scope = scope,
    //            Terms = term
    //        };
    //        this.SearchTerms.Add(param);
    //        this.SearchTermsObj.AddSearchExpress(param);
    //    }
    //    private static string RemoveStructureCriteria(string p, out string term, out string criteria)
    //    {
    //        var nv = HttpUtility.ParseQueryString(p);
    //        var strTerm = nv[SearchFieldNameConstants.SearchTerms];
    //        nv.Remove(SearchFieldNameConstants.SearchTerms);
    //        var first = nv.ToString();
    //        var searchTerm = GetSearchTerms(strTerm);
    //        var second = searchTerm.ToQueryString();
    //        criteria = first;
    //        if (!string.IsNullOrEmpty(second))
    //        {
    //            first += "&" + SearchFieldNameConstants.SearchTerms + "=" + second;
    //        }
    //        term = strTerm;
    //        return first;
    //    }
    //    private static void RemoveParametersFromQueryString(NameValueCollection nv)
    //    {
    //        nv.Remove(QueryStringName.SEARCH_VIEW_PARA_NAME);
    //        nv.Remove(SearchQueryStringName.KEYWORD_FROM);
    //        nv.Remove(SearchQueryStringName.KEYWORD_SAVEDSEARCH_NAME);
    //        nv.Remove(QueryStringName.SEARCH_BASIC_PARA_NAME);
    //        nv.Remove(SearchQueryStringName.KEYWORD_SEARCH_ADVANCE);
    //        nv.Remove(SearchQueryStringName.KEYWORD_SEARCH_PREFERENCES);
    //        nv.Remove(QueryStringName.PAGE_PARAM_NAME);
    //        nv.Remove(QueryStringName.SEARCH_SORT_ORDER);
    //        nv.Remove(QueryStringName.SEARCH_SORT_BY);
    //        nv.Remove(QueryStringName.SEARCH_PAGE_SIZE);
    //        nv.Remove(QueryStringName.ProxiedUserId);
    //        nv.Remove(QueryStringName.SearchPageChangeView);

    //        //Remove parametter for Browse SearchResults pages
    //        //ReleaseCalendarProducts page
    //        nv.Remove(SearchFieldNameConstants.releaseday);
    //        nv.Remove(SearchFieldNameConstants.releasemonth);
    //        nv.Remove(SearchFieldNameConstants.releaseyear);
    //        nv.Remove(SearchFieldNameConstants.releaseproducttype);

    //        //PromotionProducts page
    //        nv.Remove(SearchFieldNameConstants.promotionid);

    //        //EListProducts page
    //        nv.Remove(SearchFieldNameConstants.elistid);

    //        //PublicationProducts page
    //        nv.Remove(SearchFieldNameConstants.publicationsubcategoryid);
    //        nv.Remove(SearchFieldNameConstants.publicationcategoryid);

    //        // view in search result from batch entry
    //        nv.Remove(SearchQueryStringName.KEYWORD_SEARCH_BATCH_ENTRY);
    //        //For WebTrends
    //        nv.Remove(QueryStringName.WebTrendAC);
    //        nv.Remove(QueryStringName.ResetAVCache);
    //        //remove because its codes are in session
    //        //nv.Remove(SearchFieldNameConstants.reviewcode);
    //        nv.Remove(SearchFieldNameConstants.reviewnonissued);
    //        nv.Remove(SearchFieldNameConstants.positivecode);
    //        nv.Remove(SearchFieldNameConstants.starredcode);
    //        nv.Remove(SearchFieldNameConstants.reviewissuecount);
    //        nv.Remove(SearchFieldNameConstants.btprogramcode);
    //        nv.Remove(SearchFieldNameConstants.ayprogramcode);
    //    }
    //    private static List<SearchExpression> GetSearchTerms(string text)
    //    {
    //        var searchTerms = new List<SearchExpression>();
    //        if (string.IsNullOrEmpty(text))
    //            return searchTerms;
    //        XElement ele = XElement.Parse(text);
    //        SearchExpression param;
    //        foreach (XElement child in ele.Elements())
    //        {
    //            param = new SearchExpression();
    //            foreach (XElement node in child.Elements())
    //            {
    //                var val = node.Value;
    //                var name = node.Name;
    //                if (name.ToString().ToLower() == "operator")
    //                    param.Operator = val;
    //                else if (name.ToString().ToLower() == "scope")
    //                {
    //                    param.Scope = val;
    //                }
    //                else
    //                    if (name.ToString().ToLower() == "term")
    //                        param.Terms = val;
    //                    else

    //                        if (name.ToString().ToLower() == "name")
    //                            param.DisplayName = val;
    //            }
    //            searchTerms.Add(param);
    //        }

    //        return searchTerms;
    //    }

    //    private static Dictionary<string, string> HandleReviewPublication(Dictionary<string, string> dict)
    //    {
    //        if (!dict.ContainsKey(SearchFieldNameConstants.reviewpub)) return dict;
    //        var newDict = new Dictionary<string, string>(dict);

    //        var value = newDict[SearchFieldNameConstants.reviewpub];
    //        newDict.Remove(SearchFieldNameConstants.reviewpub);

    //        var values = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
    //        for (var i = 0; i < values.Length; i++)
    //        {
    //            var level = values[i].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
    //            values[i] = level[0];
    //        }
    //        var reviewTypes = ProductCatalogDAO.Instance.GetReviewType(values);

    //        var NIRVSC = AdvancedSearchFilter.NonIssuedReviewJournal.ToString();
    //        var Positive = AdvancedSearchFilter.PositiveReviews.ToString();
    //        var Starred = AdvancedSearchFilter.StarredReviews.ToString();
    //        var AYPRGM = AdvancedSearchFilter.AyPrograms.ToString();
    //        var op = String.Empty;
    //        foreach (var type in reviewTypes)
    //        {
    //            if (string.IsNullOrEmpty(type.Key) || string.IsNullOrEmpty(type.Value)) continue;

    //            var key = string.Empty;
    //            switch (type.Key)
    //            {
    //                case "RVSC":
    //                    key = SearchFieldNameConstants.ReviewJournalSource;
    //                    break;
    //                case "NIRVSC":
    //                    key = NIRVSC;
    //                    op = AdvancedSearchFilter.IssuedReviewJournalOperator.ToString();
    //                    break;
    //                case "Positive":
    //                    key = Positive;
    //                    op = AdvancedSearchFilter.NonIssuedReviewJournalOperator.ToString();
    //                    break;
    //                case "Starred":
    //                    key = Starred;
    //                    op = AdvancedSearchFilter.PositiveReviewsOperator.ToString();
    //                    break;
    //                case "PRGM":
    //                    key = SearchFieldNameConstants.BtProgramSource;
    //                    break;
    //                case "AYPRGM":
    //                    key = AYPRGM;
    //                    op = AdvancedSearchFilter.BtProgramsOperator.ToString();
    //                    break;
    //            }
    //            if (string.IsNullOrEmpty(key)) continue;
    //            if (newDict.ContainsKey(key))
    //            {
    //                newDict[key] += "|" + type.Value;
    //            }
    //            else
    //            {
    //                newDict.Add(key, type.Value);
    //                if (key != SearchFieldNameConstants.ReviewJournalSource && key != SearchFieldNameConstants.BtProgramSource
    //                    && !newDict.ContainsKey(op))
    //                    newDict.Add(op, BooleanOperatorConstants.Or);
    //            }
    //        }

    //        return newDict;
    //    }

    //    private static string GetProductType(string str)
    //    {
    //        var result = ProductTypeConstants.Book;
    //        if (string.IsNullOrEmpty(str))
    //        {
    //            string[] prodType = SiteContext.Current.ProductType;
    //            //visible tab if based on user's product type
    //            if (CommonHelper.IsStringInList(ProductTypeConstants.Book, prodType))
    //                result = ProductTypeConstants.Book;
    //            else if (CommonHelper.IsStringInList(ProductTypeConstants.Music, prodType))
    //                result = ProductTypeConstants.Music;
    //            else if (CommonHelper.IsStringInList(ProductTypeConstants.Movie, prodType))
    //                result = ProductTypeConstants.Movie;
    //        }
    //        else
    //        {
    //            if (str.IndexOf(ProductTypeConstants.Book, StringComparison.OrdinalIgnoreCase) != -1)
    //                result = ProductTypeConstants.Book;
    //            else if (str.IndexOf(ProductTypeConstants.Music, StringComparison.OrdinalIgnoreCase) != -1)
    //                result = ProductTypeConstants.Music;
    //            else if (str.IndexOf(ProductTypeConstants.Movie, StringComparison.OrdinalIgnoreCase) != -1)
    //                result = ProductTypeConstants.Movie;
    //        }
    //        return result;
    //    }
    //}

    //public class BookAdvancedSearch
    //{
    //    public Dictionary<string, string> BookSearchFilter { get; set; }
    //}
    //public class MovieAdvancedSearch
    //{
    //    public Dictionary<string, string> MovieSearchFilter { get; set; }
    //}
    //public class MusicAdvancedSearch
    //{
    //    public Dictionary<string, string> MusicSearchFilter { get; set; }
    //}

    //public class SavedSearchObj
    //{
    //    public int CurrentSlectedTabIndex { get; set; }
    //    public bool IsUsingMyPreferencesValues { get; set; }
    //    public string[,] Criteria { get; set; }
    //    public string OriginalProductType { get; set; }
    //    public string[,] SearchTerms { get; set; }
    //}
    //public class SavedSearchObjDeserialize
    //{
    //    public int CurrentSlectedTabIndex { get; set; }
    //    public bool IsUsingMyPreferencesValues { get; set; }
    //    public string[] Criteria { get; set; }
    //    public string OriginalProductType { get; set; }
    //    public string[] SearchTerms { get; set; }
    //}

}
