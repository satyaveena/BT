using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using BT.FSIS;
using BT.FSIS.WCF;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using Microsoft.Security.Application;
using BT.TS360API.ExternalServices;

namespace BT.TS360API.Common.Search.Helpers
{
    public static class SearchHelper
    {
        public static string CreateSearchResultLink(string searchKey, string value)
        {
            if (searchKey == SearchFieldNameConstants.subject)
                searchKey = SearchFieldNameConstants.subject1;
            return String.Format("{0}?{1}={2}&{3}=1", SiteUrl.SearchResults, searchKey, Microsoft.Security.Application.Encoder.UrlEncode(value), QueryStringName.ResetAVCache);
        }

        //public static string CreateItemDetailLink(string btkey, string btkeyValue, string tabName, string tabValue)
        //{
        //    return String.Format("{0}?{1}={2}&{3}={4}", SiteUrl.ItemDetailsAbsolutePath, btkey,
        //                         Encoder.UrlEncode(btkeyValue),
        //                         tabName, Encoder.UrlEncode(tabValue));
        //}

        /// <summary>
        /// Create search executor
        /// </summary>
        /// <param name="search">Search context</param>
        /// <param name="count">Page size of search result</param>
        /// <returns>The search executor</returns>
        public static ISearchExecutor CreateSearchExecutor(BT.FSIS.Search search, int count)
        {
            return CreateSearchExecutor(search, count, true);
        }

        public static ISearchExecutor CreateSearchExecutor(BT.FSIS.Search search, int count, bool useEsp)
        {
            if (search == null)
            {
                throw new ArgumentNullException("search");
            }

            search.spellCheckType = SpellCheckType.None;
            search.Resubmit.Lemmatization = true;

            var searchExecutorPath = ConfigurationManager.AppSettings[GlobalConfigurationKey.SearchExecutorUrlSetting];

            if (String.IsNullOrEmpty(searchExecutorPath))
                throw new InvalidOperationException(
                    "Search executor URL not found. Please check SearchExecutorPath in AppSettings of web.config file");

            var searchExecutor = SearchExecutorFactory.Create(searchExecutorPath, count, useEsp);

            if (searchExecutor != null)
            {
                searchExecutor.DefineSearch(search);
            }

            return searchExecutor;
        }


        public static SearchOperator CreateSearchOperators(SearchExpression exp)
        {
            if (exp == null) return null;
            if (exp is SearchExpressionGroup)
            {
                var group = (SearchExpressionGroup)exp;

                if (group.SearchExpressions.Count == 0) return null;

                if (group.SearchExpressions.Count == 1
                    && string.Compare(group.OperatorGroup, BooleanOperatorConstants.Not, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return CreateSearchOperators(group.SearchExpressions[0]);
                }
                var groupOperator = CreateGroupSearchOperator(group.OperatorGroup);
                foreach (var item in group.SearchExpressions)
                {
                    var itemOperator = CreateSearchOperators(item);
                    if (itemOperator == null) continue;

                    if (itemOperator is GroupOperator)
                    {
                        var itemgroup = (GroupOperator)itemOperator;
                        if (itemgroup.Type != GroupOperatorType.Not && groupOperator.Type == itemgroup.Type)
                        {
                            groupOperator.Operators.AddRange(itemgroup.Operators);
                        }
                        else
                        {
                            groupOperator.Operators.Add(itemgroup);
                        }
                    }
                    else
                    {
                        groupOperator.Operators.Add(itemOperator);
                    }

                }
                return groupOperator;
            }

            if (!exp.IsValid) return null;
            if (exp is RangeExpression)
            {
                var range = (RangeExpression)exp;
                var sizeOperator = new RangeOperator { Min = range.Min, Max = range.Max };
                sizeOperator.Scopes.Add(range.Scope);
                return sizeOperator;
            }
            //if (exp is BoundaryExpression)
            //{
            //    var termsOperator = new BoundaryOperator(exp.Terms, (BoundaryOperatorMode)Enum.Parse(typeof(BoundaryOperatorMode), exp.ComparisionOperator));
            //    termsOperator.Scopes.Add(exp.Scope);
            //    return termsOperator;
            //}
            var terms = CreateSearchOperator(exp.Terms, exp.Scope, exp.ComparisionOperator);
            if (exp.Operator == SearchOperatorConstants.Not)
            {
                var groupOperator = new GroupOperator(GroupOperatorType.Not);
                groupOperator.Operators.Add(terms);
                return groupOperator;
            }
            return terms;
        }



        private static GroupOperator CreateGroupSearchOperator(string operatorType)
        {
            GroupOperatorType type;
            switch (operatorType)
            {
                case "":
                case BooleanOperatorConstants.And:
                    type = GroupOperatorType.And;
                    break;
                case BooleanOperatorConstants.Or:
                    type = GroupOperatorType.Or;
                    break;
                case BooleanOperatorConstants.Not:
                    type = GroupOperatorType.Not;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return new GroupOperator(type);
        }


        ///// <summary>
        ///// Create search operator based on search key and value
        ///// </summary>
        ///// <param name="scope">search key</param>
        ///// <param name="terms">search value</param>
        ///// <returns>actual search operator</returns>
        private static SearchOperator CreateSearchOperator(string terms, string scope, string comparisonOperator = "")
        {
            SearchOperator searchOperator;

            switch (scope)
            {
                case SearchFieldNameConstants.btkey:
                case SearchFieldNameConstants.reviewcode:
                case SearchFieldNameConstants.btprogramcode:
                case SearchFieldNameConstants.ayprogramcode:
                case SearchFieldNameConstants.reviewnonissued:
                case SearchFieldNameConstants.positivecode:
                case SearchFieldNameConstants.starredcode:
                    searchOperator = CreateBTKeySearchOperators(terms, scope);
                    break;
                case SearchFieldNameConstants.standingOrderID://TFS 19437
                case SearchFieldNameConstants.standingOrderName:
                    searchOperator = CreateStandingOrderSearch(terms, SearchFieldNameConstants.standingOrder, comparisonOperator);
                    break;
                case SearchFieldNameConstants.keyword:
                    searchOperator = CreateBasicSearchOperators(terms, comparisonOperator);
                    break;
                case SearchFieldNameConstants.listprice:

                    searchOperator = CreateDecimalRangeOperator(terms, scope);
                    break;
                case SearchFieldNameConstants.pubdate:
                case SearchFieldNameConstants.preorderdaterange:
                //case SearchFieldNameConstants.btprogramdate:
                case SearchFieldNameConstants.reviewdate:
                case SearchFieldNameConstants.odscreateddatetime:
                    searchOperator = CreateDateRangeOperator(terms, scope);
                    break;
                case SearchFieldNameConstants.deweynormalizedfloat:
                    searchOperator = HasAlphabeDeweyValue(terms)
                                         ? CreateGroupOpeForAlphabeDewey(terms,
                                                                         SearchFieldNameConstants.deweynormalized)
                                         : CreateDecimalDeweyRangeOperator(terms, scope);
                    break;
                case SearchFieldNameConstants.readingcountrange:
                    searchOperator = CreateNumericRangeOperatorForARAndRC(terms, SearchFieldNameConstants.readingcount);
                    break;
                case SearchFieldNameConstants.acceleratedreaderrange:
                    searchOperator = CreateNumericRangeOperatorForARAndRC(terms,
                                                                          SearchFieldNameConstants.acceleratedreader);
                    break;
                case SearchFieldNameConstants.futureonsaledate:
                    searchOperator = CreateFutureOnSaleDateOperator(terms, SearchFieldNameConstants.streetdate);
                    break;
                case SearchQueryStringName.KEYWORD_ARINTEREST:
                    searchOperator = CreateANDTermsOperator(terms, SearchFieldNameConstants.acceleratedreader);
                    break;
                case SearchQueryStringName.KEYWORD_RCINTEREST:
                    searchOperator = CreateANDTermsOperator(terms, SearchFieldNameConstants.readingcount);
                    break;
                case SearchFieldNameConstants.pubdaterange:
                    searchOperator = CreatePubDateRange306090(terms, scope);
                    break;
                case SearchFieldNameConstants.title:
                    searchOperator = CreateTitleSearch(terms, SearchFieldNameConstants.title, comparisonOperator);
                    break;
                case SearchFieldNameConstants.begin:
                    searchOperator = CreateBeginWithOperator(terms, scope);
                    break;
                case SearchFieldNameConstants.booktypeliteral:
                case SearchFieldNameConstants.purchaseoption:
                    searchOperator = CreateBoundaryExactOperators(terms, scope);
                    break;
                case SearchFieldNameConstants.lcclass:
                    searchOperator = CreateBoundaryStartWithOperator(terms, scope);
                    break;
                case SearchFieldNameConstants.publisher:
                    searchOperator = CreatePublisherSuplier(terms, scope, comparisonOperator);
                    break;
                case SearchFieldNameConstants.reviewissuecount:
                case SearchFieldNameConstants.lexilerange:
                    searchOperator = CreateIntegerRangeOperator(terms, scope);
                    break;
                case SearchFieldNameConstants.demand:
                    searchOperator = CreateIntegerRangeOperator(terms, scope);
                    break;
                case SearchFieldNameConstants.ivtfaceta:
                case SearchFieldNameConstants.ivtfacetd:
                case SearchFieldNameConstants.ivtfacetle:
                    searchOperator = CreateIvtRefineSearchOperator(terms, scope);
                    break;
                default:
                    searchOperator = CreateANDTermsOperator(terms, scope, comparisonOperator);
                    break;
            }

            return searchOperator;
        }


        public static bool HasAlphabeDeweyValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            var values = value.Split('|');
            if (!values.Any())
            {
                return false;
            }

            foreach (var s in values)
            {
                decimal decimalValue;
                if (!string.IsNullOrEmpty(s) && !decimal.TryParse(s, out decimalValue))
                {
                    return true;
                }
            }
            return false;
        }

        private static SearchOperator CreatePublisherSuplier(string terms, string p, string comparisonOperator = "")
        {
            var opPublisher = CreateANDTermsOperator(terms, p, comparisonOperator);
            var groupOperator = new GroupOperator(GroupOperatorType.Or);
            groupOperator.Operators.Add(opPublisher);
            //
            var opSupplier = CreateANDTermsOperator(terms, SearchFieldNameConstants.supplier, comparisonOperator);
            groupOperator.Operators.Add(opSupplier);
            //
            return groupOperator;
        }

        private static SearchOperator CreateTitleSearch(string terms, string p, string comparisonOperator = "")
        {
            var opTitle = CreateANDTermsOperator(terms, p, comparisonOperator);


            var groupOperator = new GroupOperator(GroupOperatorType.Or);
            groupOperator.Operators.Add(opTitle);

            var opSubTitle = CreateANDTermsOperator(terms, SearchFieldNameConstants.subtitle, comparisonOperator);
            groupOperator.Operators.Add(opSubTitle);

            return groupOperator;
        }

        private static SearchOperator CreateStandingOrderSearch(string terms, string p, string comparisonOperator = "")
        {
            var termsOperator = CreateANDTermsOperator(terms, p, comparisonOperator);
            return termsOperator;
        }

        private static SearchOperator CreatePubDateRange306090(string terms, string p)
        {
            var opFirst = CreateANDTermsOperator(terms, p);
            if (terms.Contains("30")) // return if term is 30
                return opFirst;

            var groupOperator = new GroupOperator(GroupOperatorType.Or);
            groupOperator.Operators.Add(opFirst);

            if (terms.Contains("60"))
            {
                string term30 = terms.Replace("60", "30");
                var opNext = CreateANDTermsOperator(term30, p);
                groupOperator.Operators.Add(opNext);
            }
            else if (terms.Contains("90"))
            {
                string term60 = terms.Replace("90", "60");
                var opNext = CreateANDTermsOperator(term60, p);
                groupOperator.Operators.Add(opNext);

                string term30 = terms.Replace("90", "30");
                opNext = CreateANDTermsOperator(term30, p);
                groupOperator.Operators.Add(opNext);
            }
            else if (terms.Contains("180"))
            {
                string term90 = terms.Replace("180", "90");
                var opNext = CreateANDTermsOperator(term90, p);
                groupOperator.Operators.Add(opNext);

                string term60 = terms.Replace("180", "60");
                opNext = CreateANDTermsOperator(term60, p);
                groupOperator.Operators.Add(opNext);

                string term30 = terms.Replace("180", "30");
                opNext = CreateANDTermsOperator(term30, p);
                groupOperator.Operators.Add(opNext);
            }
            return groupOperator;
        }

        private static SearchOperator CreateBTKeySearchOperators(string searchExpression, string scope)
        {
            var termsOperator = new TermsOperator(searchExpression.Replace("|", " "), TermsOperatorMode.Any);
            termsOperator.Scopes.Add(scope);
            return termsOperator;
        }

        private static SearchOperator CreateORSearchOperators(IEnumerable<string> terms, string scope)
        {
            var searchExpression = new StringBuilder();
            foreach (var term in terms)
            {
                searchExpression.AppendFormat("{0} ", term);
            }
            var termsOperator = new TermsOperator(searchExpression.ToString(), TermsOperatorMode.Any);
            termsOperator.Scopes.Add(scope);
            return termsOperator;
        }

        //private static SearchExpression CreateORSearchExpression(IEnumerable<string> terms, string scope)
        //{
        //    var searchExpression = new StringBuilder();
        //    foreach (var term in terms)
        //    {
        //        searchExpression.AppendFormat("{0} ", term);
        //    }
        //    return new SearchExpression
        //    {
        //        Operator = BooleanOperatorConstants.And,
        //        Scope = scope,
        //        Terms = searchExpression.ToString()
        //    };
        //}
        private static SearchOperator CreateORSearchOperators(string terms, string scope)
        {
            var termsOperator = new TermsOperator(terms, TermsOperatorMode.Any);
            termsOperator.Scopes.Add(scope);
            return termsOperator;
        }

        private static SearchOperator CreateBeginWithOperator(string terms, string p)
        {
            var groupOperator = new GroupOperator(GroupOperatorType.Or);

            var opTitle = new BoundaryOperator(terms, BoundaryOperatorMode.Start);
            opTitle.Scopes.Add(SearchFieldNameConstants.title);

            var opPublisher = new BoundaryOperator(terms, BoundaryOperatorMode.Start);
            opPublisher.Scopes.Add(SearchFieldNameConstants.publisher);

            groupOperator.Operators.Add(opTitle);
            groupOperator.Operators.Add(opPublisher);

            return groupOperator;
        }

        private static SearchOperator CreateFutureOnSaleDateOperator(string value, string scope)
        {
            var rangeOperator = new RangeOperator();
            rangeOperator.Scopes.Add(scope);

            var minValue = DateTime.ParseExact(value, CommonConstants.Search_DateTimeRange_Format, null);
            rangeOperator.Min = minValue;
            rangeOperator.MinInclusive = true;
            return rangeOperator;
        }

        private static SearchOperator CreateBoundaryStartWithOperator(string value, string scope)
        {
            var opStartWith = new BoundaryOperator(value, BoundaryOperatorMode.Start);
            opStartWith.Scopes.Add(scope);
            return opStartWith;
        }


        ////This method covers Anphabe combination searching for specific Dewey Advance Search.
        public static SearchOperator CreateGroupOpeForAlphabeDewey(string value, string scope)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var values = value.Split('|');
            if (!values.Any())
                return null;

            if (values.Length == 2)
            {
                if (string.IsNullOrEmpty(values[0].Trim()) && !string.IsNullOrEmpty(values[1].Trim()))
                {
                    values[0] = values[1].Trim();
                    values[1] = string.Empty;
                }
            }

            TermsOperatorBase groupOperatorFirst = null;
            if (!string.IsNullOrEmpty(values[0]))
            {
                groupOperatorFirst = new TermsOperator(PaddingToIntegerValue(values[0]), TermsOperatorMode.All);
                groupOperatorFirst.Scopes.Add((values[0].ToUpper() == "SC")
                                                  ? SearchFieldNameConstants.deweynative
                                                  : SearchFieldNameConstants.deweynormalized);
            }

            if (values.Length == 2)
            {
                if (!string.IsNullOrEmpty(values[1]))
                {
                    var result = new GroupOperator(GroupOperatorType.Or);
                    var groupOperatorSecond = new TermsOperator(PaddingToIntegerValue(values[1]), TermsOperatorMode.All);
                    groupOperatorSecond.Scopes.Add((values[1].ToUpper() == "SC")
                                                       ? SearchFieldNameConstants.deweynative
                                                       : SearchFieldNameConstants.deweynormalized);

                    result.Operators.Add(groupOperatorFirst);
                    result.Operators.Add(groupOperatorSecond);
                    return result;
                }
            }

            return groupOperatorFirst;

        }

        private static SearchOperator CreateDateRangeOperator(string value, string scope)
        {
            RangeOperator rangeOperator = null;
            var values = value.Split(new string[] { "|" }, StringSplitOptions.None);
            if (values.Length == 1)
            {
                return CreateANDTermsOperator(value, scope);
            }
            if (values.Length == 2)
            {
                rangeOperator = new RangeOperator();
                rangeOperator.Scopes.Add(scope);

                try
                {
                    if (!string.IsNullOrEmpty(values[0]))
                    {
                        var minValue = DateTime.ParseExact(values[0], CommonConstants.Search_DateTimeRange_Format, null);

                        rangeOperator.Min = minValue;
                        rangeOperator.MinInclusive = true;
                    }// let rangeOperator.Min = null in else case.

                    if (!string.IsNullOrEmpty(values[1]))
                    {
                        var maxValue = DateTime.ParseExact(values[1], CommonConstants.Search_DateTimeRange_Format, null);

                        // Maxvalue should add hours information, otherwise data in this day doesn't return.
                        maxValue = maxValue.AddHours(24);
                        rangeOperator.Max = maxValue;
                        rangeOperator.MaxInclusive = false;
                    }// let rangeOperator.Max = null in else case.
                }
                catch
                {
                    return null; //handle case end-user to input invalid value to Browser address bar.
                }
            }
            return rangeOperator;
        }

        private static string PaddingToDecimalValue(string input)
        {
            string result = string.Empty;
            string[] arrInput = input.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            result = arrInput[0];
            if (arrInput.Length > 1)
            {
                result += ".";
                bool isZeroLeading = false;
                var strFraction = arrInput[1];
                if (strFraction.Length > 4)
                    strFraction = strFraction.Substring(0, 4);
                if (strFraction[0] == '0')
                {
                    strFraction = '1' + strFraction;
                    isZeroLeading = true;
                }
                int fraction = 0;
                Int32.TryParse(strFraction, out fraction);
                int beforeLength = strFraction.Length;
                if (fraction > 0)
                {
                    fraction++;
                    if (fraction.ToString().Length > beforeLength)
                    {
                        int wholeNumber = 0;
                        Int32.TryParse(arrInput[0], out wholeNumber);
                        wholeNumber++;
                        result = wholeNumber.ToString();
                        strFraction = string.Empty;
                    }
                    else
                    {
                        strFraction = fraction.ToString();
                        if (isZeroLeading)
                            strFraction = strFraction.Substring(1);
                    }
                }
                else
                {
                    int wholeNumber = 0;
                    Int32.TryParse(arrInput[0], out wholeNumber);
                    wholeNumber++;
                    result = wholeNumber.ToString();
                    strFraction = string.Empty;
                }
                result += strFraction;
            }
            else
            {
                int wholeNumber = 0;
                Int32.TryParse(arrInput[0], out wholeNumber);
                wholeNumber++;
                result = wholeNumber.ToString();
            }
            return result;
        }

        private static string PaddingToIntegerValue(string input)
        {
            string result = string.Empty;

            string[] arrInput = input.Split('.');

            int value; // add '0' to front if input value is a number.
            if (int.TryParse(arrInput[0], out value))
                result = arrInput[0].PadLeft(3, '0');
            else
                result = arrInput[0];

            if (arrInput.GetLength(0) > 1)
            {
                result += ".";
                result += arrInput[1];
            }
            return result;
        }

        ////This method covers Decimal Range searching for specific Dewey Advance Search.
        private static SearchOperator CreateDecimalDeweyRangeOperator(string value, string scope)
        {
            RangeOperator rangeOperator = null;
            var values = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length > 0)
            {
                rangeOperator = new RangeOperator();
                rangeOperator.Scopes.Add(scope);

                double minValue;
                Double.TryParse(values[0], out minValue);
                rangeOperator.Min = minValue;
                rangeOperator.MinInclusive = true;

                if (values.Length > 1)
                {
                    values[1] = PaddingToDecimalValue(values[1]);

                    double maxValue;
                    Double.TryParse(values[1], out maxValue);
                    rangeOperator.Max = maxValue;
                    rangeOperator.MaxInclusive = false;
                }
            }
            return rangeOperator;
        }

        ////This method covers Decimal Range searching
        private static SearchOperator CreateDecimalRangeOperator(string value, string scope)
        {
            RangeOperator rangeOperator = null;
            var values = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length > 0)
            {
                rangeOperator = new RangeOperator();
                rangeOperator.Scopes.Add(scope);

                double minValue;
                Double.TryParse(values[0], out minValue);
                rangeOperator.Min = minValue;
                rangeOperator.MinInclusive = true;

                if (values.Length > 1)
                {
                    double maxValue;
                    Double.TryParse(values[1], out maxValue);
                    rangeOperator.Max = maxValue;
                    rangeOperator.MaxInclusive = true;
                }
            }
            return rangeOperator;
        }

        private static SearchOperator CreateNumericRangeOperatorForARAndRC(string value, string scope)
        {
            var groupOperator = new GroupOperator(GroupOperatorType.Or);
            //The passing value will have format 1|2|3|etc. If it contain 1 we will search from 1 to 2 (exclusive)
            var values = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length > 0)
            {
                foreach (string s in values)
                {
                    var rangeOperator = new RangeOperator();
                    rangeOperator.Scopes.Add(scope);

                    var minValue = Double.Parse(s);
                    rangeOperator.Min = minValue;
                    rangeOperator.MinInclusive = true;

                    var maxValue = minValue + 1;
                    rangeOperator.Max = maxValue;
                    rangeOperator.MaxInclusive = false;

                    groupOperator.Operators.Add(rangeOperator);
                }
            }
            return groupOperator;
        }

        private static SearchOperator CreateBasicSearchOperators(string value, string comparisonOperator = "")
        {
            return CreateANDTermsOperator(value, SearchFieldNameConstants.content, comparisonOperator);
        }

        public static SearchOperator CreateANDTermsOperator(string terms, string scope, string comparisonOperator = "")
        {
            SearchOperator result;

            var andValues = terms.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

            if (andValues.Length > 1)
            {
                var groupOperator = new GroupOperator(GroupOperatorType.And);
                foreach (var andValue in andValues)
                {
                    result = CreateORTermOperator(andValue, scope, comparisonOperator);
                    groupOperator.Operators.Add(result);
                }

                result = groupOperator;
            }
            else
            {
                result = CreateORTermOperator(terms, scope, comparisonOperator);
            }
            return result;
        }

        private static SearchOperator CreateORTermOperator(string terms, string scope, string comparisonOperator = "")
        {
            var values = terms.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            SearchOperator result = values.Length > 1
                                        ? CreateTermsOperators(new List<string>(values), scope)
                                        : CreateSearchTermOperator(terms, scope, comparisonOperator);
            return result;
        }

        private static SearchOperator CreateSearchTermOperator(string terms, string scope, string comparisonOperator = "")
        {
            if (string.IsNullOrEmpty(comparisonOperator))
            {
                var termsOperator = new TermsOperator(terms, TermsOperatorMode.All);
                termsOperator.Scopes.Add(scope);
                if (scope == SearchFieldNameConstants.content)
                {
                    termsOperator.Linguistics = true;
                }
                if (scope == SearchFieldNameConstants.producttype
                    || scope == SearchFieldNameConstants.readingcount
                    || scope == SearchFieldNameConstants.reviewpub
                    || scope == SearchFieldNameConstants.acceleratedreader
                    || scope == SearchFieldNameConstants.rating)
                {
                    termsOperator.Mode = TermsOperatorMode.Phrase;
                }
                return termsOperator;
            }
            else
            {
                var termsOperator = new BoundaryOperator(terms, BoundaryOperatorMode.Exact);
                termsOperator.Scopes.Add(scope);
                termsOperator.Mode =
                    (BoundaryOperatorMode)Enum.Parse(typeof(BoundaryOperatorMode), comparisonOperator);

                return termsOperator;
            }
        }

        internal static SearchOperator CreateTermsOperatorsForISBNAndUPC(List<string> terms)
        {
            var searchExpression = new StringBuilder();
            foreach (var term in terms)
            {
                searchExpression.AppendFormat("{0} ", term);
            }
            var groupOperator = new GroupOperator(GroupOperatorType.Or);

            string searchTerms = searchExpression.ToString();

            groupOperator.Operators.Add(CreateTermsOperators(searchTerms, SearchFieldNameConstants.title));
            groupOperator.Operators.Add(CreateTermsOperators(searchTerms, SearchFieldNameConstants.isbn13));
            groupOperator.Operators.Add(CreateTermsOperators(searchTerms, SearchFieldNameConstants.upc));

            return groupOperator;
        }

        internal static SearchOperator CreateTermsOperators(string terms, string scope)
        {
            return CreateORSearchOperators(terms, scope);
        }
        internal static SearchOperator CreateTermsOperators(List<string> terms, string scope)
        {
            var groupOperator = new GroupOperator(GroupOperatorType.Or);
            switch (scope)
            {
                case SearchFieldNameConstants.btkey:
                case SearchFieldNameConstants.isbn13:
                case SearchFieldNameConstants.isbn10:
                case SearchFieldNameConstants.upc:
                    return CreateORSearchOperators(terms, scope);
                case SearchFieldNameConstants.subtitle:
                case SearchFieldNameConstants.content:
                    groupOperator = new GroupOperator(GroupOperatorType.And);
                    break;
            }
            foreach (string term in terms)
            {
                groupOperator.Operators.Add(CreateSearchTermOperator(term, scope));
            }
            return groupOperator;
        }

        public static BT.FSIS.Search CreateSearchForSearchTypeAhead(string prefixText)
        {
            if (string.IsNullOrEmpty(prefixText))
            {
                throw new ArgumentNullException("searchArguments");
            }

            var searchArguments = new SearchArguments();

            var searchExpression = new SearchExpression
            {
                Operator = BooleanOperatorConstants.And,
                Scope = SearchFieldNameConstants.autosuggestcontent,
                Terms = string.Format("{0}*", prefixText.Trim())
            };
            //searchArguments.SearchExpressions.Add(searchExpression);
            searchArguments.SearchExpressionGroup.AddSearchExpress(searchExpression);

            var sortExpression = new SortExpression
            {
                SortField = SuggestionItemNameConstants.QueryTermsRank,
                SortDirection = SortDirection.Descending
            };
            searchArguments.SortExpressions.Add(sortExpression);


            var op1 = CreateSearchOperators(searchArguments.SearchExpressionGroup);

            var boostingOperator = CreateDemandBoosting(SearchFieldNameConstants.searchtypeaheaddemandweight);

            BT.FSIS.Search search = null;
            if (op1 != null)
            {
                var groupOp = new GroupOperator(GroupOperatorType.Rank);
                groupOp.Operators.Add(op1);
                if (boostingOperator != null)
                    groupOp.Operators.Add(boostingOperator);

                search = new BT.FSIS.Search(groupOp) { ResultView = SuggestionItemNameConstants.AutoSuggestCollection };
            }
            return search;
        }



        public static SearchOperator CreateDemandBoosting(string field)
        {
            var settings = DistributedCacheHelper.GetDemandBucket();

            if (settings == null || settings.Count == 0) return null;

            var gOp = new GroupOperator(GroupOperatorType.Or);

            foreach (var setting in settings)
            {
                var xRankOp = new GroupOperator(GroupOperatorType.XRank) { Boost = setting.Value };

                var termsOp = new TermsOperator(setting.Key);
                termsOp.Scopes.Add(field);

                xRankOp.Operators.Add(termsOp);
                gOp.Operators.Add(xRankOp);
            }

            return gOp;
        }

        internal static SearchOperator CreateBoundaryExactOperators(string terms, string scope, bool offWildCard = false)
        {
            var values = terms.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            var groupOperator = new GroupOperator(GroupOperatorType.Or);

            foreach (var term in values)
            {
                var boundaryOperator = new BoundaryOperator(term, BoundaryOperatorMode.Exact);
                boundaryOperator.Scopes.Add(scope);
                boundaryOperator.OffWildCard = offWildCard;

                groupOperator.Operators.Add(boundaryOperator);
            }

            return groupOperator;
        }

        //public static void GetRcInterestLevel(string readingcount, out string myRcInterestLevel,
        //                                      out string myRcInterestLevelLink, out string myRcReadinglevel,
        //                                      out string myRcReadinglevelLink)
        //{
        //    myRcInterestLevel = "";
        //    myRcInterestLevelLink = "";
        //    myRcReadinglevel = "";
        //    myRcReadinglevelLink = "";

        //    if (readingcount == null) return;
        //    var arr = readingcount.Split(CommonConstants.ReadingCountSeparator);
        //    if (arr.Length != 2)
        //        return;
        //    var interest = arr[1].Split(CommonConstants.BisacSeparator);
        //    if (interest.Length > 1)
        //    {
        //        myRcInterestLevel = interest[1];
        //        myRcInterestLevelLink = arr[1];
        //    }
        //    var reading = arr[0].Split(CommonConstants.BisacSeparator);
        //    if (reading.Length > 1)
        //    {
        //        myRcReadinglevel = reading[1];
        //        myRcReadinglevelLink = arr[0];
        //    }
        //}

        //public static void GetArInterestLevel(string arreader, out string myArInterestLevel,
        //                                      out string myArInterestLevelLink, out string myArReadinglevel,
        //                                      out string myArReadinglevelLink)
        //{
        //    myArInterestLevel = "";
        //    myArInterestLevelLink = "";
        //    myArReadinglevel = "";
        //    myArReadinglevelLink = "";

        //    if (arreader == null) return;
        //    var arr = arreader.Split(CommonConstants.AcceleratedReaderSeparator);
        //    if (arr.Length != 2)
        //        return;
        //    var interest = arr[1].Split(CommonConstants.BisacSeparator);
        //    if (interest.Length > 1)
        //    {
        //        myArInterestLevel = interest[1];
        //        myArInterestLevelLink = arr[1];
        //    }
        //    var reading = arr[0].Split(CommonConstants.BisacSeparator);
        //    if (reading.Length > 1)
        //    {
        //        myArReadinglevel = reading[1];
        //        myArReadinglevelLink = arr[0];
        //    }
        //}

        //public static string ToQueryString(this List<SearchExpression> list)
        //{
        //    var result = new StringBuilder();
        //    foreach (var exp in list)
        //    {
        //        if (exp.IsValid)
        //        {
        //            result.AppendFormat("{0} {1} EQUALS {2} ", (result.Length > 0) ? exp.Operator : String.Empty,
        //                                exp.DisplayName, exp.Terms);
        //        }
        //    }

        //    return result.ToString().Trim();
        //}

        //public static string ToQueryString(this NameValueCollection qscoll)
        //{
        //    var results = new List<string>();
        //    foreach (var key in qscoll.AllKeys)
        //    {
        //        if (string.Compare(key, QueryStringName.SearchPageChangeView, StringComparison.OrdinalIgnoreCase) == 0)
        //            continue;

        //        results.Add(string.Format("{0}={1}", key, HttpUtility.UrlEncode(qscoll[key])));
        //    }
        //    return string.Join("&", results.ToArray());
        //}

        public static string CreateUrlBTKeys(string value)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            var btkeys = value.Replace(',', '|').Replace('\n', '|');
            var result = SearchFieldNameConstants.btkey + "=" + btkeys;

            if (btkeys.Contains(SearchQueryStringName.KEYWORD_BTKEYSEPARATOR.ToString()))
            {

                //AdvSession.IsUsingMyPreferencesValues = false;
                //SiteContext.Current.Session[SessionVariableName.IsUsingMyPreferencesValues] = false;
                result = SiteUrl.SearchResults + "?" + result;
            }
            else
            {
                result = SiteUrl.ItemDetails + "?" + result;
            }

            //for R2.6 BTR
            result = ProxySessionHelper.AppendProxyUserId(result);
            return result;
        }

        public static string CreateUrlWebtrendsQueryString(string webUrl, string webtrensTagValue)
        {
            if (String.IsNullOrEmpty(webUrl))
                return String.Empty;

            if (String.IsNullOrEmpty(webtrensTagValue))
                return webUrl;

            webtrensTagValue = HttpUtility.UrlEncode(webtrensTagValue);

            if (webUrl.IndexOf('?') == -1)
                webUrl += string.Format("?{0}={1}", QueryStringName.WebTrendAC, webtrensTagValue);
            else
                webUrl += string.Format("&{0}={1}", QueryStringName.WebTrendAC, webtrensTagValue);

            return webUrl;
        }

        //public static string CreateUrlBTKeysForBrowse(string value, string browsePage)
        //{
        //    if (String.IsNullOrEmpty(value))
        //        return String.Empty;

        //    var btkeys = value.Replace(',', '|').Replace('\n', '|');
        //    var result = SearchFieldNameConstants.btkey + "=" + btkeys;

        //    AdvSession.IsUsingMyPreferencesValues = false;
        //    //SiteContext.Current.Session[SessionVariableName.IsUsingMyPreferencesValues] = false;
        //    result = browsePage + "?" + result;

        //    //for R2.6 BTR
        //    result = ProxySessionHelper.AppendProxyUserId(result);
        //    return result;
        //}

        public static string CreateUrlPromoCode(string value)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;
            return ProxySessionHelper.AppendProxyUserId(SiteUrl.SearchResults + "?" + SearchFieldNameConstants.promoCode +
                                                     "=" + value);
        }

        public static string CreateUrlForWhatsHot(int whatsHotId, string btKeys)
        {
            if (String.IsNullOrEmpty(btKeys))
                return String.Empty;

            btKeys = btKeys.Replace(',', '|').Replace('\n', '|');
            string result;

            if (btKeys.Contains(SearchQueryStringName.KEYWORD_BTKEYSEPARATOR.ToString()))
            {
                // SearchResults Url
                result = SearchFieldNameConstants.whatsHotId + "=" + whatsHotId;
                result = SiteUrl.SearchResults + "?" + result;
            }
            else
            {
                // ItemDetails Url
                result = SearchFieldNameConstants.btkey + "=" + btKeys;
                result = SiteUrl.ItemDetails + "?" + result;
            }

            result = ProxySessionHelper.AppendProxyUserId(result);
            return result;
        }

        //public static bool IsSearchResultOrCartDetailsContext()
        //{
        //    var absoluteUri = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        //    return absoluteUri.Contains("cartdetails.aspx") || absoluteUri.Contains("searchresults.aspx") ||
        //           absoluteUri.Contains("BackgroundProcessHandler.ashx") || absoluteUri.Contains("CartDetailsPage.aspx");
        //}

        //public static bool IsSearchResultContext()
        //{
        //    var absoluteUri = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        //    return absoluteUri.Contains("searchresults.aspx") || absoluteUri.Contains("quicksearch.aspx");
        //}

        //public static bool IsHomeContext()
        //{
        //    var absoluteUri = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        //    return absoluteUri.Contains("pages/default.aspx");
        //}

        //public static bool IsItemDetailsContext()
        //{
        //    var absoluteUri = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        //    if (absoluteUri.Contains("ajax"))
        //    {
        //        var referUrl = HttpContext.Current.Request.UrlReferrer;

        //        return referUrl != null && referUrl.AbsoluteUri.ToLower().Contains("itemdetailspage.aspx");
        //    }

        //    return absoluteUri.Contains("itemdetailspage.aspx");
        //}

        //public static bool IsPrimaryCart(string primaryCartID)
        //{
        //    var absoluteUri = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        //    if (absoluteUri.Contains("ajax"))
        //    {
        //        var referUrl = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToLower();
        //        string urlDecoded = HttpUtility.UrlDecode(referUrl);
        //        return referUrl.Contains("itemdetailspage.aspx") && urlDecoded.Contains(primaryCartID.ToLower());

        //    }

        //    string uriDecoded = HttpUtility.UrlDecode(absoluteUri);
        //    return absoluteUri.Contains("itemdetailspage.aspx") && uriDecoded.Contains(primaryCartID.ToLower());
        //}


        private static string CreateItemDetailsLink(string btKey, string img, string tab, string webUrl)
        {
            if (tab == "N")
            {
                return img;
            }
            var url = webUrl;
            url += CreateUrlBTKeys(btKey);
            var result = "<a  href=\"" + url + "&tab=" + tab + "\">" + img + "</a>";
            return result;
        }

        private static string CreateItemDetailsReviewLink(string btKey, string img, string tab, string webUrl, bool onlyReview)
        {
            string result;
            var url = webUrl;
            url += CreateUrlBTKeys(btKey);
            if (onlyReview == false)
                result = "<a href=\"" + url + "&tab=" + tab + "\"" + "id='divIconReview" + btKey + "'>" + img + "</a>";
            else
                result = "<a href=\"" + url + "&tab=" + tab + "\"" + "id='divIconReview" + btKey + "'>" +
                         ProductSupportedHtmlTag.BeginImage + img + ProductSupportedHtmlTag.EndImage + "</a>";
            return result;
        }

        public static string CombineContentIndicator(ProductContent productContent, string btKey, string url)
        {
            var result = "";
            var resultNonReturnable = "";

            bool onlyReview = false;
            if (!productContent.HasAnnotation
                && !productContent.HasExcerpts
                && !productContent.HasMuze
                && productContent.HasReturnKey
                && !productContent.HasTOC)
                onlyReview = true;

            if (productContent.HasAnnotation)
                result += CreateItemDetailsLink(btKey, ProductSupportedHtmlTag.AnnotationImage, "A", url);
            //AnnotationImage;
            //
            if (productContent.HasExcerpts)
            {
                result += CreateItemDetailsLink(btKey, ProductSupportedHtmlTag.ExcerptsImage, "E", url);
            }
            //ExcerptsImage;
            //
            if (productContent.HasReviews)
            {
                result += CreateItemDetailsReviewLink(btKey, ProductSupportedHtmlTag.ReviewImage, "R", url, onlyReview);
            }
            //ReviewImage;
            //
            if (productContent.HasTOC)
            {
                result += CreateItemDetailsLink(btKey, ProductSupportedHtmlTag.TocImage, "T", url); //TOCImage;
            }
            //
            if (!productContent.HasReturnKey)
            {
                resultNonReturnable = CreateItemDetailsLink(btKey, ProductSupportedHtmlTag.NonReturnImage, "N", url);
                //returnImage;
            }
            //
            if (productContent.HasMuze)
            {
                result += CreateItemDetailsLink(btKey, ProductSupportedHtmlTag.MImage, "M", url); //TOCImage;
            }
            //
            if (!String.IsNullOrEmpty(result) || !String.IsNullOrEmpty(resultNonReturnable))
            {
                if (!String.IsNullOrEmpty(resultNonReturnable))
                {
                    if (result != "")
                    {
                        result = ProductSupportedHtmlTag.BeginImage + result + ProductSupportedHtmlTag.EndImage + resultNonReturnable;
                    }
                    else
                        result = resultNonReturnable + ProductSupportedHtmlTag.DivCb;
                }
                else if (productContent.HasReviews)
                {
                    if (onlyReview == false)
                    {
                        result = ProductSupportedHtmlTag.BeginImage + result + ProductSupportedHtmlTag.EndImage +
                                 ProductSupportedHtmlTag.DivCb;
                    }
                    else
                    {
                        result = result + ProductSupportedHtmlTag.DivCb;
                    }
                }
                else
                {
                    result = ProductSupportedHtmlTag.BeginImage + result + ProductSupportedHtmlTag.EndImage +
                             ProductSupportedHtmlTag.DivCb;
                }
            }
            return result;
        }

        //public static GroupOperator CreateGroupOpeForNonSimonSchuster()
        //{
        //    //exclude SIMEB titles
        //    var mrcNotTermFirst = new TermsOperator("SIMEB", TermsOperatorMode.All);
        //    mrcNotTermFirst.Scopes.Add(SearchFieldNameConstants.suppliercode);

        //    var result = new GroupOperator(GroupOperatorType.Not);
        //    result.Operators.Add(mrcNotTermFirst);

        //    return result;
        //}
        public static SearchExpression CreateGroupExpressionForNonSimonSchuster()
        {
            //exclude SIMEB titles
            return new SearchExpression
            {
                Operator = BooleanOperatorConstants.Not,
                Scope = SearchFieldNameConstants.suppliercode,
                Terms = "SIMEB"
            };
        }

        //public static GroupOperator CreateGroupOpeForMarketRestrictionCodes()
        //{
        //    var siteContext = SiteContext.Current;

        //    if (siteContext.MarketType.HasValue)
        //    {
        //        var userMarketType = siteContext.MarketType.Value;
        //        var mrcNotTermFirst = userMarketType == MarketType.Retail
        //                                  ? new TermsOperator(MarketRestrictionCodeConstants.INT, TermsOperatorMode.All)
        //                                  : new TermsOperator(MarketRestrictionCodeConstants.NIN, TermsOperatorMode.All);
        //        mrcNotTermFirst.Scopes.Add(SearchFieldNameConstants.marketrestrictioncode);

        //        var mrcNotTermSecond = userMarketType == MarketType.Retail
        //                                   ? new TermsOperator(MarketRestrictionCodeConstants.NRE, TermsOperatorMode.All)
        //                                   : new TermsOperator(MarketRestrictionCodeConstants.RET, TermsOperatorMode.All);
        //        mrcNotTermSecond.Scopes.Add(SearchFieldNameConstants.marketrestrictioncode);


        //        var groupOperatorFirst = new GroupOperator(GroupOperatorType.Not);
        //        groupOperatorFirst.Operators.Add(mrcNotTermFirst);

        //        var groupOperatorSecond = new GroupOperator(GroupOperatorType.Not);
        //        groupOperatorSecond.Operators.Add(mrcNotTermSecond);

        //        var result = new GroupOperator(GroupOperatorType.And);
        //        result.Operators.Add(groupOperatorFirst);
        //        result.Operators.Add(groupOperatorSecond);
        //        return result;
        //    }

        //    return null;
        //}
        public static SearchExpressionGroup CreateGroupExpressionForMarketRestrictionCodes(MarketType? marketType)
        {
            //var siteContext = SiteContext.Current;
            //if (siteContext.MarketType.HasValue)
            if (marketType.HasValue)
            {
                var group = new SearchExpressionGroup();

                //var userMarketType = siteContext.MarketType.Value;
                var userMarketType = marketType.Value;

                group.AddSearchExpress(new SearchExpression
                {
                    Operator = BooleanOperatorConstants.Not,
                    Scope = SearchFieldNameConstants.marketrestrictioncode,
                    Terms = (userMarketType == MarketType.Retail)
                                          ? MarketRestrictionCodeConstants.INT
                                          : MarketRestrictionCodeConstants.NIN
                });

                group.AddSearchExpress(new SearchExpression
                {
                    Operator = BooleanOperatorConstants.Not,
                    Scope = SearchFieldNameConstants.marketrestrictioncode,
                    Terms = (userMarketType == MarketType.Retail)
                                          ? MarketRestrictionCodeConstants.NRE
                                          : MarketRestrictionCodeConstants.RET
                });

                return group;
            }

            return null;
        }
        //public static GroupOperator CreateGroupOpeForGeographicExclusive()
        //{
        //    var siteContext = SiteContext.Current;

        //    if (siteContext.MarketType.HasValue && !string.IsNullOrEmpty(siteContext.CountryCode))
        //    {
        //        var userMarketType = siteContext.MarketType.Value;
        //        //var marketName = siteContext.MarketTypeName;
        //        var country = siteContext.CountryCode;
        //        var salesExclusion = BuildSalesExclusion(userMarketType.ToString());

        //        var mrcNotTermFirst = new TermsOperator(country, TermsOperatorMode.All);
        //        mrcNotTermFirst.Scopes.Add(salesExclusion);

        //        var mrcNotTermSecond = new TermsOperator("WORLD", TermsOperatorMode.All);
        //        mrcNotTermSecond.Scopes.Add(salesExclusion);

        //        var groupOperatorFirst = new GroupOperator(GroupOperatorType.Not);
        //        groupOperatorFirst.Operators.Add(mrcNotTermFirst);

        //        var groupOperatorSecond = new GroupOperator(GroupOperatorType.Not);
        //        groupOperatorSecond.Operators.Add(mrcNotTermSecond);

        //        var result = new GroupOperator(GroupOperatorType.And);
        //        result.Operators.Add(groupOperatorFirst);
        //        result.Operators.Add(groupOperatorSecond);
        //        return result;
        //    }

        //    return null;
        //}
        public static SearchExpressionGroup CreateGroupExpressionForGeographicExclusive(MarketType? marketType, string countryCode)
        {
            //var siteContext = SiteContext.Current;
            //if (siteContext.MarketType.HasValue && !string.IsNullOrEmpty(siteContext.CountryCode))

            if (marketType.HasValue && !string.IsNullOrEmpty(countryCode))
            {
                var group = new SearchExpressionGroup();

                //var userMarketType = siteContext.MarketType.Value;
                //var country = siteContext.CountryCode;
                var userMarketType = marketType.Value;
                var country = countryCode;

                var salesExclusion = BuildSalesExclusion(userMarketType.ToString());

                group.AddSearchExpress(new SearchExpression
                {
                    Operator = BooleanOperatorConstants.Not,
                    Scope = salesExclusion,
                    Terms = country
                });
                group.AddSearchExpress(new SearchExpression
                {
                    Operator = BooleanOperatorConstants.Not,
                    Scope = salesExclusion,
                    Terms = "WORLD"
                });

                return group;
            }

            return null;
        }
        //public static GroupOperator CreateGroupOpeForSimonEnabledAndPublicLib()
        //{
        //    // Sample for the ORG has country = US ->retrieve title that has exlcusion:US
        //    //  OR( and(not SIMEB, not US, not WORLD)
        //    //      and(    SIMEB)

        //    GroupOperator line1 = null;
        //    var grcNotSimonTitle = SearchHelper.CreateGroupOpeForNonSimonSchuster();
        //    var grcGeoExclusive = SearchHelper.CreateGroupOpeForGeographicExclusive();
        //    if (grcGeoExclusive != null && grcNotSimonTitle != null)
        //    {
        //        line1 = new GroupOperator(GroupOperatorType.And);
        //        line1.Operators.Add(grcNotSimonTitle);
        //        line1.Operators.Add(grcGeoExclusive);
        //    }

        //    var line2 = new TermsOperator("SIMEB", TermsOperatorMode.All);
        //    line2.Scopes.Add(SearchFieldNameConstants.suppliercode);

        //    if (line1 != null)
        //    {
        //        var final = new GroupOperator(GroupOperatorType.Or);
        //        final.Operators.Add(line1);
        //        final.Operators.Add(line2);
        //        return final;
        //    }
        //    return null;
        //}
        public static SearchExpressionGroup CreateGroupExpressionForSimonEnabledAndPublicLib(MarketType? marketType, string countryCode)
        {
            // Sample for the ORG has country = US ->retrieve title that has exlcusion:US
            //  OR( and(not SIMEB, not US, not WORLD)
            //      and(    SIMEB)

            var group = new SearchExpressionGroup();

            group.AddSearchExpress(SearchHelper.CreateGroupExpressionForNonSimonSchuster());

            var grcGeoExclusive = SearchHelper.CreateGroupExpressionForGeographicExclusive(marketType, countryCode);
            if (grcGeoExclusive != null)
            {
                group.AddSearchExpress(grcGeoExclusive);
            }

            group.AddSearchExpress(new SearchExpression
            {
                Operator = BooleanOperatorConstants.Or,
                Scope = SearchFieldNameConstants.suppliercode,
                Terms = "SIMEB"
            });

            return group;
        }
        private static string BuildSalesExclusion(string market)
        {
            //return "ngstring5";
            string result = string.Format("NG{0}Exclusions", market).ToLower();
            return result;
        }

        public static bool ContainEBookSearchExp(SearchArguments searchArguments)
        {
            if (searchArguments == null) return false;
            var list = new List<string>
                       {
                           ProductFormatConstants.EBook_Digital_Download ,
                           ProductFormatConstants.EBook_Digital_Download_Online ,
                           ProductFormatConstants.EBook_Digital_Online ,
                           ProductFormatConstants.EBook_Downloadable_Audio
                       };
            return searchArguments.SearchExpressionGroup.ContainScopeTerms(SearchFieldNameConstants.format, list);
            //return searchArguments.SearchExpressions.Any(searchExpression =>
            //                                             searchExpression.Scope == SearchFieldNameConstants.format
            //                                             &&
            //                                             (searchExpression.Terms.ToLower().Contains(
            //                                                 ProductFormatConstants.EBook_Digital_Download) ||
            //                                              searchExpression.Terms.ToLower().Contains(
            //                                                  ProductFormatConstants.EBook_Digital_Download_Online) ||
            //                                              searchExpression.Terms.ToLower().Contains(
            //                                                  ProductFormatConstants.EBook_Digital_Online) ||
            //                                              searchExpression.Terms.ToLower().Contains(
            //                                                  ProductFormatConstants.EBook_Downloadable_Audio)));
        }

        //private static SearchOperator CreateEBookSearchOpe(bool isIncluded)
        //{
        //    if (isIncluded)
        //    {
        //        var gOp = new GroupOperator(GroupOperatorType.And);
        //        var groupOperator = new GroupOperator(GroupOperatorType.Or);
        //        var termsEBookDigitalDownload = new TermsOperator(ProductFormatConstants.EBook_Digital_Download);
        //        termsEBookDigitalDownload.Scopes.Add(SearchFieldNameConstants.format);
        //        var termsEBookDigitalDownloadOnline =
        //            new TermsOperator(ProductFormatConstants.EBook_Digital_Download_Online);
        //        termsEBookDigitalDownloadOnline.Scopes.Add(SearchFieldNameConstants.format);
        //        var termsEBookDigitalOnline = new TermsOperator(ProductFormatConstants.EBook_Digital_Online);
        //        termsEBookDigitalOnline.Scopes.Add(SearchFieldNameConstants.format);
        //        var termsEBookDownloadableAudio = new TermsOperator(ProductFormatConstants.EBook_Downloadable_Audio);
        //        termsEBookDownloadableAudio.Scopes.Add(SearchFieldNameConstants.format);

        //        //groupOperator.Scopes.Add(SearchFieldNameConstants.format);
        //        groupOperator.Operators.Add(termsEBookDigitalDownload);
        //        groupOperator.Operators.Add(termsEBookDigitalDownloadOnline);
        //        groupOperator.Operators.Add(termsEBookDigitalOnline);
        //        groupOperator.Operators.Add(termsEBookDownloadableAudio);
        //        gOp.Operators.Add(groupOperator);

        //        return gOp;
        //    }
        //    else
        //    {
        //        var gOp = new GroupOperator(GroupOperatorType.Not);
        //        var groupOperator = new GroupOperator(GroupOperatorType.Or);
        //        //var groupOperator = new GroupOperator(GroupOperatorType.And);
        //        var termsEBookDigitalDownload = new TermsOperator(ProductFormatConstants.EBook_Digital_Download);
        //        termsEBookDigitalDownload.Scopes.Add(SearchFieldNameConstants.format);
        //        var termsEBookDigitalDownloadOnline =
        //            new TermsOperator(ProductFormatConstants.EBook_Digital_Download_Online);
        //        termsEBookDigitalDownloadOnline.Scopes.Add(SearchFieldNameConstants.format);
        //        var termsEBookDigitalOnline = new TermsOperator(ProductFormatConstants.EBook_Digital_Online);
        //        termsEBookDigitalOnline.Scopes.Add(SearchFieldNameConstants.format);
        //        var termsEBookDownloadableAudio = new TermsOperator(ProductFormatConstants.EBook_Downloadable_Audio);
        //        termsEBookDownloadableAudio.Scopes.Add(SearchFieldNameConstants.format);

        //        //groupOperator.Scopes.Add(SearchFieldNameConstants.format);
        //        groupOperator.Operators.Add(termsEBookDigitalDownload);
        //        groupOperator.Operators.Add(termsEBookDigitalDownloadOnline);
        //        groupOperator.Operators.Add(termsEBookDigitalOnline);
        //        groupOperator.Operators.Add(termsEBookDownloadableAudio);
        //        gOp.Operators.Add(groupOperator);

        //        return gOp;
        //    }
        //}
        private static SearchExpressionGroup CreateEBookSearchExpression(bool isIncluded)
        {
            //var gOp = new GroupOperator(GroupOperatorType.And);

            var groupOr = new SearchExpressionGroup(BooleanOperatorConstants.Or);
            //var groupOperator = new GroupOperator(GroupOperatorType.Or);

            //var termsEBookDigitalDownload = new TermsOperator(ProductFormatConstants.EBook_Digital_Download);
            //termsEBookDigitalDownload.Scopes.Add(SearchFieldNameConstants.format);
            groupOr.AddSearchExpress(new SearchExpression
            {
                Operator = BooleanOperatorConstants.Or,
                Scope = SearchFieldNameConstants.format,
                Terms = ProductFormatConstants.EBook_Digital
            });

            //var termsEBookDigitalDownloadOnline =
            //    new TermsOperator(ProductFormatConstants.EBook_Digital_Download_Online);
            //termsEBookDigitalDownloadOnline.Scopes.Add(SearchFieldNameConstants.format);
            //groupOr.AddSearchExpress(new SearchExpression
            //{
            //    Operator = BooleanOperatorConstants.Or,
            //    Scope = SearchFieldNameConstants.format,
            //    Terms = ProductFormatConstants.EBook_Digital_Download_Online
            //});

            //var termsEBookDigitalOnline = new TermsOperator(ProductFormatConstants.EBook_Digital_Online);
            //termsEBookDigitalOnline.Scopes.Add(SearchFieldNameConstants.format);
            //groupOr.AddSearchExpress(new SearchExpression
            //{
            //    Operator = BooleanOperatorConstants.Or,
            //    Scope = SearchFieldNameConstants.format,
            //    Terms = ProductFormatConstants.EBook_Digital_Online
            //});

            //var termsEBookDownloadableAudio = new TermsOperator(ProductFormatConstants.EBook_Downloadable_Audio);
            //termsEBookDownloadableAudio.Scopes.Add(SearchFieldNameConstants.format);
            //groupOr.AddSearchExpress(new SearchExpression
            //{
            //    Operator = BooleanOperatorConstants.Or,
            //    Scope = SearchFieldNameConstants.format,
            //    Terms = ProductFormatConstants.EBook_Downloadable_Audio
            //});

            groupOr.AddSearchExpress(new SearchExpression
            {
                Operator = BooleanOperatorConstants.Or,
                Scope = SearchFieldNameConstants.format,
                Terms = ProductFormatConstants.EAudio_Downloadable_Audio
            });

            if (isIncluded)
                return groupOr;
            var groupNot = new SearchExpressionGroup(BooleanOperatorConstants.Not);
            groupNot.AddSearchExpress(groupOr);
            return groupNot;
            //{
            //    var gOp = new GroupOperator(GroupOperatorType.Not);
            //    var groupOperator = new GroupOperator(GroupOperatorType.Or);
            //    //var groupOperator = new GroupOperator(GroupOperatorType.And);
            //    var termsEBookDigitalDownload = new TermsOperator(ProductFormatConstants.EBook_Digital_Download);
            //    termsEBookDigitalDownload.Scopes.Add(SearchFieldNameConstants.format);
            //    var termsEBookDigitalDownloadOnline =
            //        new TermsOperator(ProductFormatConstants.EBook_Digital_Download_Online);
            //    termsEBookDigitalDownloadOnline.Scopes.Add(SearchFieldNameConstants.format);
            //    var termsEBookDigitalOnline = new TermsOperator(ProductFormatConstants.EBook_Digital_Online);
            //    termsEBookDigitalOnline.Scopes.Add(SearchFieldNameConstants.format);
            //    var termsEBookDownloadableAudio = new TermsOperator(ProductFormatConstants.EBook_Downloadable_Audio);
            //    termsEBookDownloadableAudio.Scopes.Add(SearchFieldNameConstants.format);

            //    //groupOperator.Scopes.Add(SearchFieldNameConstants.format);
            //    groupOperator.Operators.Add(termsEBookDigitalDownload);
            //    groupOperator.Operators.Add(termsEBookDigitalDownloadOnline);
            //    groupOperator.Operators.Add(termsEBookDigitalOnline);
            //    groupOperator.Operators.Add(termsEBookDownloadableAudio);
            //    gOp.Operators.Add(groupOperator);

            //    return gOp;
            //}
        }
        //private static SearchOperator CreateESuppliersSearchOpe()
        //{
        //    // should be get from SiteContext.Current.ESuppliers property
        //    var eSuppliers = SiteContext.Current.ESuppliers;

        //    if (eSuppliers == null || eSuppliers.Count() == 0) return null;

        //    var eSuppliersText = CommonHelper.GetESupplierValueName(eSuppliers);
        //    if (eSuppliersText == null || eSuppliersText.Count == 0)
        //    {
        //        return null;
        //    }
        //    var gOp = new GroupOperator(GroupOperatorType.Or);
        //    foreach (var eSupplier in eSuppliersText)
        //    {
        //        var termsOp = new TermsOperator(eSupplier.Value);
        //        termsOp.Scopes.Add(SearchFieldNameConstants.esupplier);
        //        gOp.Operators.Add(termsOp);
        //    }

        //    return gOp;
        //}
        private static SearchExpressionGroup CreateESuppliersSearchExpression(string[] ESuppliers)
        {
            // should be get from SiteContext.Current.ESuppliers property
            //var eSuppliers = SiteContext.Current.ESuppliers;
            var eSuppliers = ESuppliers;

            if (eSuppliers == null || eSuppliers.Count() == 0) return null;

            var eSuppliersText = CommonHelper.GetESupplierValueName(eSuppliers);
            if (eSuppliersText == null || eSuppliersText.Count == 0)
            {
                return null;
            }
            var gOp = new GroupOperator(GroupOperatorType.Or);
            var groupOr = new SearchExpressionGroup(BooleanOperatorConstants.Or);
            foreach (var eSupplier in eSuppliersText)
            {
                var termsOp = new TermsOperator(eSupplier.Value);
                termsOp.Scopes.Add(SearchFieldNameConstants.esupplier);
                gOp.Operators.Add(termsOp);
                groupOr.AddSearchExpress(new SearchExpression
                {
                    Operator = BooleanOperatorConstants.Or,
                    Scope = SearchFieldNameConstants.esupplier,
                    Terms = eSupplier.Value
                });
            }

            return groupOr;
        }
        public static SearchExpressionGroup ApplyEBookSearchExpression(MarketType? marketType, string[] ESuppliers)
        {
            #region without ebook
            var notEBookOpe = CreateEBookSearchExpression(false);

            // Logic: If Market Type  = Retail => Not Ebook
            //if (SiteContext.Current.MarketType.HasValue && SiteContext.Current.MarketType.Value == MarketType.Retail)
            if (marketType.HasValue && marketType.Value == MarketType.Retail)
            {
                return notEBookOpe;
            }

            //if (ContainEBookSearchExp(searchArguments)) return null;

            var eSupplierOpe = CreateESuppliersSearchExpression(ESuppliers);

            if (eSupplierOpe == null) return notEBookOpe;
            #endregion
            #region with ebook
            // Logic: (originalOpe AND notEbook ope) OR ((originalOpe AND Ebook ope) AND ngsupplier = sitecontext.ESUppliers)
            // = originalOpe AND (notEbook ope OR (Ebook ope AND ngsupplier = sitecontext.ESUppliers))
            //var ebookOpe = CreateEBookSearchExpression(true);
            //var onlyEBookGroup = new SearchExpressionGroup();

            //onlyEBookGroup.AddSearchExpress(ebookOpe);
            //onlyEBookGroup.AddSearchExpress(eSupplierOpe);

            var notEBookOrWithESupplierGroupOpe = new SearchExpressionGroup(BooleanOperatorConstants.Or);
            notEBookOrWithESupplierGroupOpe.AddSearchExpress(notEBookOpe);
            notEBookOrWithESupplierGroupOpe.AddSearchExpress(eSupplierOpe);

            return notEBookOrWithESupplierGroupOpe;
            #endregion
        }

        public static string CreateUrlPromotionProductByPromotionId(string promocodeid) //, string adname)
        {
            if (String.IsNullOrEmpty(promocodeid)) //
                return String.Empty;
            return ProxySessionHelper.AppendProxyUserId(SiteUrl.PromotionProducts + "?" +
                                                     SearchFieldNameConstants.promotionid + "=" + promocodeid);
            // + "&" + SearchFieldNameConstants.adname + "=" + adname);
        }


        public static ServiceContracts.Profiles.UserProfile CreateSearchExpressionForProductTypeFilter(SearchExpressionGroup SearchArguments, string userId)
        {
            //Add product type and audience type             
            //var user = CSObjectProxy.GetUserProfileForSearchResult();
            var user = ProfileService.Instance.GetUserById(userId);

            if (user != null)
            {
                if (user.ProductTypeFilter == null) return user;

                SearchExpression searchExpression;

                if (user.ProductTypeFilter.Contains("OutofPrint")
                    && user.ProductTypeFilter.Contains("OutofPrintEnt"))
                {
                    var exp = new SearchExpression(SearchFieldNameConstants.reportcode, CommonConstants.publishstatus, BooleanOperatorConstants.Not);
                    SearchArguments.AddSearchExpress(exp);
                }
                else if (user.ProductTypeFilter.Contains("OutofPrint")) //ng publish status
                {
                    var exp = new SearchExpression(SearchFieldNameConstants.reportcode, CommonConstants.publishstatus, BooleanOperatorConstants.Not);

                    var isBook = new SearchExpression(SearchFieldNameConstants.producttype, "Book", BooleanOperatorConstants.And);
                    var isNotBook = new SearchExpression(SearchFieldNameConstants.producttype, "Book", BooleanOperatorConstants.Not);

                    var g1 = new SearchExpressionGroup();
                    g1.AddSearchExpress2(isBook);
                    g1.AddSearchExpress2(exp);

                    var g2 = new SearchExpressionGroup(BooleanOperatorConstants.Or);
                    g2.AddSearchExpress2(g1);
                    g2.AddSearchExpress2(isNotBook);

                    SearchArguments.AddSearchExpress(g2);
                }
                else if (user.ProductTypeFilter.Contains("OutofPrintEnt"))
                {
                    var exp = new SearchExpression(SearchFieldNameConstants.reportcode, CommonConstants.publishstatusEnt, BooleanOperatorConstants.Not);

                    var isBook = new SearchExpression(SearchFieldNameConstants.producttype, "Book", BooleanOperatorConstants.And);

                    var isNotBook = new SearchExpression(SearchFieldNameConstants.producttype, "Book", BooleanOperatorConstants.Not);

                    var g1 = new SearchExpressionGroup();
                    g1.AddSearchExpress2(isNotBook);
                    g1.AddSearchExpress2(exp);

                    var g2 = new SearchExpressionGroup(BooleanOperatorConstants.Or);
                    g2.AddSearchExpress2(g1);
                    g2.AddSearchExpress2(isBook);

                    SearchArguments.AddSearchExpress(g2);
                }

                string exmerchant = "";
                if (user.ProductTypeFilter.Contains("PrintonDemand"))//ngmerchcategory
                {
                    exmerchant += CommonConstants.exmerchant;
                }

                if (user.ProductTypeFilter.Contains("Gardner")) //ngmerchcategory
                {
                    //TFS 9916 check against more reliable fielzd for gardner
                    searchExpression = new SearchExpression(SearchFieldNameConstants.suppliercode, SearchFieldValue.GardnerSupplierCode, BooleanOperatorConstants.Not);
                    //SearchArguments.SearchExpressions.Add(searchExpression);
                    SearchArguments.AddSearchExpress(searchExpression);

                }
                if (!string.IsNullOrEmpty(exmerchant))
                {
                    searchExpression = new SearchExpression(SearchFieldNameConstants.merchcategory, exmerchant.TrimStart('|'), BooleanOperatorConstants.Not);
                    //SearchArguments.SearchExpressions.Add(searchExpression);
                    SearchArguments.AddSearchExpress(searchExpression);
                }

                if (user.ProductTypeFilter.Contains("PawPrints")) //pub code 
                {
                    //TFS 9916 Search against product line for Paw Prints
                    searchExpression = new SearchExpression(SearchFieldNameConstants.productline, SearchFieldValue.PawPrintsProductLine, BooleanOperatorConstants.And);
                    //SearchArguments.SearchExpressions.Add(searchExpression);
                    SearchArguments.AddSearchExpress(searchExpression);
                }


                if (user.ProductTypeFilter.Contains("NonReturnables")) //return indicator
                {
                    const string nonreturn = "1";
                    searchExpression = new SearchExpression(SearchFieldNameConstants.hasreturn, nonreturn, BooleanOperatorConstants.And);
                    //SearchArguments.SearchExpressions.Add(searchExpression);
                    SearchArguments.AddSearchExpress(searchExpression);
                }
            }
            return user;
        }

        public static List<SearchExpression> GetSearchExpressionsForProductTypesAndFormats(string userId, ServiceContracts.Profiles.UserProfile userProfile, string[] eSuppliers)
        {
            var result = new List<SearchExpression>();
            if (userProfile == null)
                userProfile = ProfileService.Instance.GetUserById(userId);

            if (userProfile != null)
            {
                var digitalExpressions = new List<SearchExpression>();
                var productTypes = new List<string>();
                var siteTermHelper = SiteTermHelper.Instance;

                // expression for selected book formats
                var bookTypeId = ((int)BT.TS360Constants.ProductType.Book).ToString();
                var userProductTypes = userProfile.ProductTypeList;
                var selectedBookType = userProductTypes.Contains(bookTypeId);
                var hasBookIncludeFormat = !String.IsNullOrEmpty(userProfile.BookIncludeFilter);
                var hasBookExcludeFormat = !String.IsNullOrEmpty(userProfile.BookExcludeFilter);

                if (selectedBookType)
                {
                    var bookSiteTerm = siteTermHelper.GetSiteTemByName(SiteTermName.AdvSearchBookFilterFormat.ToString());
                    if (bookSiteTerm != null && bookSiteTerm.Count > 0)
                    {
                        // if any include book format, ignore exclude formats
                        if (hasBookIncludeFormat)
                        {
                            var includeFilters = userProfile.BookIncludeFilter.Split(';');
                            var listIncBookFormat = bookSiteTerm.Where(x => includeFilters.Contains(x.ItemKey));
                            foreach (var item in listIncBookFormat)
                            {
                                var term = string.Format("{0}/{1}", ProductTypeConstants.Book, item.ItemValue.Replace("\"", ""));
                                var searchExpression = new SearchExpression(SearchFieldNameConstants.producttype, term, BooleanOperatorConstants.Or);
                                searchExpression.ComparisionOperator = "Exact";

                                if (item.ItemValue == ProductFormatConstants.EBook_Digital_Download
                                    || item.ItemValue == ProductFormatConstants.EAudio_Downloadable_Audio)
                                {
                                    // add eBook/eAudio digital searchExpression
                                    digitalExpressions.Add(searchExpression);
                                }
                                else
                                {
                                    result.Add(searchExpression);
                                }
                            }
                        }
                        else
                        {
                            productTypes.Add(ProductTypeConstants.Book);

                            if (hasBookExcludeFormat)
                            {
                                var digitalList = new List<string> {
                                       ProductFormatConstants.EBook_Digital_Download ,
                                       ProductFormatConstants.EBook_Digital_Download_Online ,
                                       ProductFormatConstants.EBook_Digital_Online ,
                                       ProductFormatConstants.EAudio_Downloadable_Audio
                                   };

                                var excludeFilters = userProfile.BookExcludeFilter.Split(';');
                                var listExcBookFormat = bookSiteTerm.Where(x => excludeFilters.Contains(x.ItemKey));
                                foreach (var item in listExcBookFormat)
                                {
                                    var term = string.Format("{0}/{1}", ProductTypeConstants.Book, item.ItemValue.Replace("\"", ""));
                                    var searchExpression = new SearchExpression(SearchFieldNameConstants.producttype, term, BooleanOperatorConstants.Not);
                                    searchExpression.ComparisionOperator = "Exact";
                                    result.Add(searchExpression);

                                    //
                                    if (digitalList.Contains(item.ItemValue))
                                        digitalList.Remove(item.ItemValue);
                                }

                                // create digital searchExpression
                                if (digitalList.Count > 0)
                                {
                                    foreach (var digitalItem in digitalList)
                                    {
                                        var term = string.Format("{0}/{1}", ProductTypeConstants.Book, digitalItem);
                                        var searchExpression = new SearchExpression(SearchFieldNameConstants.producttype, term, BooleanOperatorConstants.Or) { ComparisionOperator = "Exact" };

                                        digitalExpressions.Add(searchExpression);
                                    }
                                }

                            }
                        }
                    }
                }

                // expression for selected DIGITAL formats
                var hasDigitalIncludeFromBookType = (result.Count == 0) || (digitalExpressions.Count > 0);
                var digitalTypeId = ((int)BT.TS360Constants.ProductType.Digital).ToString();
                var selectedDigitalType = userProductTypes.Contains(digitalTypeId);
                //var eSuppliers = SiteContext.Current.ESuppliers;
                var hasESuppliers = eSuppliers != null && eSuppliers.Count() > 0;
                if (hasDigitalIncludeFromBookType && selectedDigitalType && hasESuppliers)
                {
                    // add all digital types
                    if (digitalExpressions.Count == 0)
                    {
                        var digitalEBookExp = new SearchExpression(SearchFieldNameConstants.format, ProductFormatConstants.EBook_Digital, BooleanOperatorConstants.Or);
                        var digitalEAudioExp = new SearchExpression(SearchFieldNameConstants.format, ProductFormatConstants.EAudio_Downloadable_Audio, BooleanOperatorConstants.Or);
                        digitalExpressions.Add(digitalEBookExp);
                        digitalExpressions.Add(digitalEAudioExp);
                    }

                    if (String.IsNullOrEmpty(userProfile.DigitalIncludeFilter) && String.IsNullOrEmpty(userProfile.DigitalExcludeFilter))
                    {
                        // case of no purchase option selected
                        result.AddRange(digitalExpressions);
                    }

                    var digitalSearchGrp = new SearchExpressionGroup(BooleanOperatorConstants.And);
                    var formatLiteralGrp = new SearchExpressionGroup(BooleanOperatorConstants.Or);
                    formatLiteralGrp.AddSearchExpress(digitalExpressions);
                    digitalSearchGrp.AddSearchExpress(formatLiteralGrp);

                    var digitalSiteTerm = siteTermHelper.GetSiteTemByName(SiteTermName.AdvSearchBookFilterPurchaseOption.ToString());
                    // if any include format, ignore exclude formats
                    if (!String.IsNullOrEmpty(userProfile.DigitalIncludeFilter))
                    {
                        var includeFilters = userProfile.DigitalIncludeFilter.Split(';');
                        var arrayIncDigitalFormat = digitalSiteTerm.Where(x => includeFilters.Contains(x.ItemKey))
                                                                  .Select(x => x.ItemValue).ToArray();
                        if (arrayIncDigitalFormat.Count() > 0)
                        {
                            // no book format selected
                            if (productTypes.Contains(ProductTypeConstants.Book) && !hasBookIncludeFormat)
                            {
                                var bookGroupAnd = new SearchExpressionGroup(BooleanOperatorConstants.And);
                                bookGroupAnd.AddSearchExpress(new SearchExpression(SearchFieldNameConstants.producttype, ProductTypeConstants.Book));

                                var firstDigExpr = digitalExpressions[0];
                                var arrTerms = digitalExpressions.Select(x => x.Terms).ToArray();
                                var terms = string.Join("|", arrTerms);
                                var expression = new SearchExpression(firstDigExpr.Scope, terms, BooleanOperatorConstants.Not);
                                expression.ComparisionOperator = firstDigExpr.ComparisionOperator;
                                bookGroupAnd.AddSearchExpress(expression);
                                //
                                result.Add(bookGroupAnd);

                                // trick to split 2 "And" groups
                                result.Add(new SearchExpression(string.Empty, string.Empty, BooleanOperatorConstants.Or));

                                productTypes.Remove(ProductTypeConstants.Book);
                            }

                            var term = string.Join("|", arrayIncDigitalFormat);
                            var searchExpression = new SearchExpression(SearchFieldNameConstants.purchaseoption, term, BooleanOperatorConstants.Or);
                            var searchGroup = new SearchExpressionGroup(BooleanOperatorConstants.Or);
                            searchGroup.AddSearchExpress(searchExpression);
                            digitalSearchGrp.AddSearchExpress(searchGroup);

                            //
                            result.Add(digitalSearchGrp);
                        }
                    }
                    else
                    {
                        productTypes.Add(ProductTypeConstants.Digital);

                        if (!String.IsNullOrEmpty(userProfile.DigitalExcludeFilter))
                        {
                            var excludeFilters = userProfile.DigitalExcludeFilter.Split(';');
                            var arrayExcDigitalFormat = digitalSiteTerm.Where(x => excludeFilters.Contains(x.ItemKey))
                                                                       .Select(x => x.ItemValue).ToArray();
                            if (arrayExcDigitalFormat.Count() > 0)
                            {
                                if (selectedBookType && !hasBookIncludeFormat)
                                {
                                    foreach (var dExp in digitalExpressions)
                                        formatLiteralGrp.Remove(dExp);

                                    formatLiteralGrp.AddSearchExpress(new SearchExpression(SearchFieldNameConstants.producttype, ProductTypeConstants.Book));
                                    productTypes.Remove(ProductTypeConstants.Book);
                                }

                                var term = string.Join("|", arrayExcDigitalFormat);
                                var digitalExcludeExpression = new SearchExpression(SearchFieldNameConstants.purchaseoption, term, BooleanOperatorConstants.Not);
                                digitalSearchGrp.AddSearchExpress(digitalExcludeExpression);

                                result.Add(digitalSearchGrp);
                            }
                        }
                    }
                }
                else
                {
                    // Book is selected without include format, but Digital is not
                    if (selectedBookType && !hasBookIncludeFormat && !selectedDigitalType && hasESuppliers)
                    {
                        // exclude digital
                        var digitalEBookExp = new SearchExpression(SearchFieldNameConstants.format, ProductFormatConstants.EBook_Digital, BooleanOperatorConstants.Not);
                        var digitalEAudioExp = new SearchExpression(SearchFieldNameConstants.format, ProductFormatConstants.EAudio_Downloadable_Audio, BooleanOperatorConstants.Not);
                        result.Add(digitalEBookExp);
                        result.Add(digitalEAudioExp);

                    }
                    else if (digitalExpressions.Count > 0)
                        result.AddRange(digitalExpressions);
                }

                // expression for selected music formats
                var musicTypeId = ((int)BT.TS360Constants.ProductType.Music).ToString();
                if (userProductTypes.Contains(musicTypeId))
                {
                    var musicSiteTerm = siteTermHelper.GetSiteTemByName(SiteTermName.AdvSearchMusicFilterFormat.ToString());
                    if (musicSiteTerm != null && musicSiteTerm.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(userProfile.MusicIncludeFilter))
                        {
                            var includeFilters = userProfile.MusicIncludeFilter.Split(';');
                            var listIncMusicFormat = musicSiteTerm.ToList().Where(x => includeFilters.Contains(x.ItemKey));
                            foreach (var item in listIncMusicFormat)
                            {
                                var term = string.Format("{0}/{1}", ProductTypeConstants.Music, item.ItemValue.Replace("\"", ""));
                                var searchExpression = new SearchExpression(SearchFieldNameConstants.producttype, term, BooleanOperatorConstants.Or);
                                searchExpression.ComparisionOperator = "Exact";
                                result.Add(searchExpression);
                            }
                        }
                        else
                        {
                            productTypes.Add(ProductTypeConstants.Music);

                            if (!String.IsNullOrEmpty(userProfile.MusicExcludeFilter))
                            {
                                var excludeFilters = userProfile.MusicExcludeFilter.Split(';');
                                var listExcMusicFormat = musicSiteTerm.ToList().Where(x => excludeFilters.Contains(x.ItemKey));
                                foreach (var item in listExcMusicFormat)
                                {
                                    var term = string.Format("{0}/{1}", ProductTypeConstants.Music, item.ItemValue.Replace("\"", ""));
                                    var searchExpression = new SearchExpression(SearchFieldNameConstants.producttype, term, BooleanOperatorConstants.Not);
                                    searchExpression.ComparisionOperator = "Exact";
                                    result.Add(searchExpression);
                                }
                            }
                        }
                    }
                }

                // expression for selected movie formats
                var movieTypeId = ((int)BT.TS360Constants.ProductType.Movie).ToString();
                if (userProductTypes.Contains(movieTypeId))
                {
                    var movieSiteTerm = siteTermHelper.GetSiteTemByName(SiteTermName.AdvSearchMovieFilterFormat.ToString());
                    if (movieSiteTerm != null && movieSiteTerm.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(userProfile.MovieIncludeFilter))
                        {
                            var includeFilters = userProfile.MovieIncludeFilter.Split(';');
                            var listIncMovieFormat = movieSiteTerm.ToList().Where(x => includeFilters.Contains(x.ItemKey));
                            foreach (var item in listIncMovieFormat)
                            {
                                var term = string.Format("{0}/{1}", ProductTypeConstants.Movie, item.ItemValue.Replace("\"", ""));
                                var searchExpression = new SearchExpression(SearchFieldNameConstants.producttype, term, BooleanOperatorConstants.Or);
                                searchExpression.ComparisionOperator = "Exact";
                                result.Add(searchExpression);
                            }
                        }
                        else
                        {
                            productTypes.Add(ProductTypeConstants.Movie);

                            if (!String.IsNullOrEmpty(userProfile.MovieExcludeFilter))
                            {
                                var excludeFilters = userProfile.MovieExcludeFilter.Split(';');
                                var listExcMovieFormat = movieSiteTerm.ToList().Where(x => excludeFilters.Contains(x.ItemKey));
                                foreach (var item in listExcMovieFormat)
                                {
                                    var term = string.Format("{0}/{1}", ProductTypeConstants.Movie, item.ItemValue.Replace("\"", ""));
                                    var searchExpression = new SearchExpression(SearchFieldNameConstants.producttype, term, BooleanOperatorConstants.Not);
                                    searchExpression.ComparisionOperator = "Exact";
                                    result.Add(searchExpression);
                                }
                            }
                        }
                    }
                }

                // ignore if "ALL" product type is selected but does not has any selected format
                if (!(productTypes.Count == 4 && result.Count == 0))
                {
                    // Digital is kind of Book, ignore Digital
                    var term = String.Join("|", productTypes.Where(p => p != ProductTypeConstants.Digital).ToArray());
                    var searchExpression = new SearchExpression(SearchFieldNameConstants.producttype, term, BooleanOperatorConstants.Or);
                    result.Add(searchExpression);
                }
            }

            return result;
        }

        //public static string ExtractInventoryValue(string val)
        //{
        //    if (string.IsNullOrEmpty(val))
        //        return string.Empty;
        //    var whs = string.Empty;
        //    var valeles = val.Split('|');
        //    var invtRange = "";
        //    foreach (var valele in valeles)
        //    {
        //        if (!string.IsNullOrEmpty(valele))
        //        {
        //            var temp = valele.Replace("/>", "#");
        //            var vals = temp.Split('#');
        //            if (string.IsNullOrEmpty(invtRange))
        //                invtRange = vals[1];
        //            whs = vals[0] + '|' + whs;
        //        }
        //    }
        //    return invtRange + '#' + whs;
        //}
        //public static string CreateUrlFeaturedPromotion(int featuredPromoId, string btKeys, out string fastBtKeys)
        //{
        //    fastBtKeys = string.Empty;
        //    if (String.IsNullOrEmpty(btKeys))
        //        return String.Empty;

        //    btKeys = btKeys.Replace(',', '|').Replace('\n', '|');
        //    string result;

        //    if (btKeys.Contains(SearchQueryStringName.KEYWORD_BTKEYSEPARATOR.ToString()))
        //    {

        //        AdvSession.IsUsingMyPreferencesValues = false;
        //        //SiteContext.Current.Session[SessionVariableName.IsUsingMyPreferencesValues] = false;
        //        fastBtKeys = btKeys;

        //        // SearchResults Url
        //        result = SearchFieldNameConstants.featuredpromoid + "=" + featuredPromoId;
        //        result = SiteUrl.SearchResults + "?" + result;
        //    }
        //    else
        //    {
        //        // ItemDetails Url
        //        result = SearchFieldNameConstants.btkey + "=" + btKeys;
        //        result = SiteUrl.ItemDetails + "?" + result;
        //    }

        //    result = ProxySessionHelper.AppendProxyUserId(result);
        //    return result;
        //}

        //public static SearchExpression CreateSearchExpressionForPublishDateRange(string searchTerms)
        //{
        //    if (!string.IsNullOrEmpty(searchTerms))
        //    {
        //        return new SearchExpression(SearchFieldNameConstants.pubdaterange, searchTerms, BooleanOperatorConstants.Not);
        //    }
        //    return null;
        //}

        //public static SearchExpression CreateSearchExpressionForExcludeParentalAdvisory(string searchTerms)
        //{
        //    if (searchTerms == "1")
        //    {
        //        var searchExpression = new SearchExpression(SearchFieldNameConstants.rating,
        //                                                    "Parental Advisory (Explicit Music)",
        //                                                    BooleanOperatorConstants.Not);
        //    }
        //    return null;
        //}

        //public static SearchExpression CreateSearchExpressionForExcludePublisherStatus(string searchTerms)
        //{
        //    return new SearchExpression(SearchFieldNameConstants.reportcode, searchTerms, BooleanOperatorConstants.Not);
        //}

        //public static SearchExpression CreateSearchExpressionForDateRange(string searchScope, string searchTerms)
        //{
        //    string pubDate = searchTerms;

        //    string newPubDate = string.Empty;
        //    if (!string.IsNullOrEmpty(pubDate))
        //    {
        //        string[] arr = pubDate.Split('|');
        //        DateTime beginDate = DateTime.Now.ToUniversalTime();
        //        if (arr.Length < 2)
        //        {
        //            DateTime nextDate;
        //            switch (pubDate)
        //            {
        //                case QueryStringName.Next30days:
        //                    nextDate = AddDateTo(30);
        //                    break;
        //                case QueryStringName.Next60days:
        //                    nextDate = AddDateTo(60);
        //                    break;
        //                case QueryStringName.Next90days:
        //                    nextDate = AddDateTo(90);
        //                    break;
        //                case QueryStringName.Next180days:
        //                    nextDate = AddDateTo(180);
        //                    break;
        //                case QueryStringName.Prev30days:
        //                    nextDate = AddDateTo(-30);
        //                    break;
        //                case QueryStringName.Prev60days:
        //                    nextDate = AddDateTo(-60);
        //                    break;
        //                case QueryStringName.Prev90days:
        //                    nextDate = AddDateTo(-90);
        //                    break;
        //                case QueryStringName.Prev180days:
        //                    nextDate = AddDateTo(-180);
        //                    break;
        //                case QueryStringName.Prev7days:
        //                    nextDate = AddDateTo(-7);
        //                    break;
        //                case QueryStringName.Prev14days:
        //                    nextDate = AddDateTo(-14);
        //                    break;
        //                case QueryStringName.Prev28days:
        //                    nextDate = AddDateTo(-28);
        //                    break;
        //                case QueryStringName.Next7days:
        //                    nextDate = AddDateTo(7);
        //                    break;
        //                case QueryStringName.Next14days:
        //                    nextDate = AddDateTo(14);
        //                    break;
        //                case QueryStringName.Next28days:
        //                    nextDate = AddDateTo(28);
        //                    break;
        //                default:
        //                    nextDate = beginDate;
        //                    break;
        //            }
        //            if (nextDate != beginDate)
        //            {
        //                if (beginDate > nextDate)
        //                {
        //                    var tempDate = nextDate;
        //                    nextDate = beginDate;
        //                    beginDate = tempDate;
        //                }
        //                newPubDate = beginDate.ToString(CommonConstants.Search_DateTimeRange_Format) + "|"
        //                             + nextDate.ToString(CommonConstants.Search_DateTimeRange_Format);
        //            }
        //            else
        //                newPubDate = pubDate;
        //        }
        //        else
        //        {
        //            if (arr.Length > 3)
        //            {
        //                string monFrom = arr[0].PadLeft(2, '0');
        //                string monTo = arr[2].PadLeft(2, '0');

        //                string fromDate = monFrom + "01" + arr[1];
        //                string toDate = monTo + DateTime.DaysInMonth(int.Parse(arr[3]), int.Parse(arr[2])) + arr[3];

        //                newPubDate = fromDate + "|" + toDate;
        //            }
        //        }
        //    }

        //    return new SearchExpression
        //    {
        //        Operator = BooleanOperatorConstants.And,
        //        Scope = searchScope,
        //        Terms = searchTerms,
        //    };
        //}

        //private static DateTime AddDateTo(int p)
        //{
        //    return DateTime.Now.AddDays(p).ToUniversalTime();
        //}

        private static SearchOperator CreateIntegerRangeOperator(string value, string scope, bool inclusive = true)
        {
            RangeOperator rangeOperator = null;
            var values = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length > 0)
            {
                rangeOperator = new RangeOperator();
                rangeOperator.Scopes.Add(scope);

                Int32 minValue;
                Int32.TryParse(values[0], out minValue);
                rangeOperator.Min = minValue;
                rangeOperator.MinInclusive = inclusive;

                if (values.Length > 1)
                {
                    Int32 maxValue;
                    Int32.TryParse(values[1], out maxValue);
                    rangeOperator.Max = maxValue;
                    rangeOperator.MaxInclusive = inclusive;
                }
            }
            return rangeOperator;
        }

        private static SearchOperator CreateIvtRefineSearchOperator(string value, string scope, bool inclusive = true)
        {
            //pairs case mutliselect, value chould be: Commerce/>500|Reno/>25|Momence/>100
            var pairs = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            if (pairs != null)
            {
                var groupOperator = new GroupOperator(GroupOperatorType.Or);
                foreach (var pair in pairs)
                {
                    //pair=Commerce/>500
                    var temp = pair.Split(new string[] { "/>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length > 1) //case click/checked on child of Commerce/Reno/Mom/Bridgewater nodes
                    {
                        var newScope = BuildNewSearchScope(temp[0].ToLower());
                        value = temp[1];
                        groupOperator.Operators.Add(CreateIntegerRangeOperator(value, newScope, false));
                    }
                    else //case click on Commerce/Reno/Mom/Bridgewater nodes themself
                    {
                        groupOperator.Operators.Add(CreateANDTermsOperator(temp[0], scope));
                    }
                }
                return groupOperator;
            }
            return null;
        }

        private static string BuildNewSearchScope(string oldScope)
        {
            // This method to return field search name in FAST Index profile.
            string userReservedType = DistributedCacheHelper.GetCurrentUserReservedType();
            oldScope = oldScope.ToLower();
            var warehouse = "";
            if (oldScope.Contains("south"))
                warehouse = "com";
            else if (oldScope.Contains("central"))
                warehouse = "mom";
            else if (oldScope.Contains("west"))
                warehouse = "rno";
            else if (oldScope.Contains("east"))
                warehouse = "som";

            var suffix = "";
            // from Search Refine box: scope name has suffix (a, le, d) already.
            // from Adv Search page, it doesn't
            if (!oldScope.EndsWith(userReservedType))
                suffix = userReservedType;

            return "ngstock" + warehouse + suffix;
        }

        public static string GetContentIndicatorText(string userId, bool hasAnnotations, bool hasExcerpt, bool hasReturn, bool hasMuze,
            bool hasReview, bool hasToc, string btKey, string productType)
        {
            var flagObject = ProductSearchController.GetFlagObject(userId); // ProductSearchController.GetFlagObject(SiteContext.Current.UserId);
            //
            var hasMuzeLocal = false;
            var hasTocLocal = false;
            if (flagObject != null)
            {
                hasMuzeLocal = flagObject.ShowMuze;
                hasTocLocal = flagObject.ShowToc;
            }

            var prodContent = new ProductContent
            {
                HasAnnotation = hasAnnotations,
                HasExcerpts = hasExcerpt,
                HasReturnKey = hasReturn
            };

            if (hasMuzeLocal && productType != ProductTypeConstants.Book)
                prodContent.HasMuze = hasMuze;
            //
            prodContent.HasReviews = hasReview;
            //
            if (hasTocLocal)
                prodContent.HasTOC = hasToc;
            //
            var content = CombineContentIndicator(prodContent, btKey, string.Empty);
            return content;
        }
    }
}
