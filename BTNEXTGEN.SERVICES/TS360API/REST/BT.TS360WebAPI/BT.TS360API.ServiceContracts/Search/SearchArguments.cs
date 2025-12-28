using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Search
{
    public class SearchArguments
    {
        /// <summary>
        /// Gets or sets search expression in form of web browser query string syntax
        /// </summary>
        public string SearchExpression { get; set; }
        public int StartRowIndex { get; set; }
        public int PageSize { get; set; }
        public bool ApplyDemandBoosting { get; set; }

        /// <summary>
        /// Gets or sets search expression collection
        /// </summary>
        public IList<SearchExpression> SearchExpressions { get; set; }
        public SearchExpressionGroup SearchExpressionGroup { get; set; }

        public IList<SortExpression> SortExpressions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SearchArguments()
        //: this(string.Empty)
        {
            this.ApplyDemandBoosting = true;
            this.SortExpressions = new List<SortExpression>();
            this.SearchExpressionGroup = new SearchExpressionGroup();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="queryString"></param>
        //public SearchArguments(string queryString)
        //    : this(HttpUtility.ParseQueryString(queryString))
        //{
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="queryString"></param>
        //public SearchArguments(NameValueCollection queryString)
        //{
        //    this.ApplyDemandBoosting = true;
        //    this.SortExpressions = new List<SortExpression>();
        //    this.SearchExpressionGroup = new SearchExpressionGroup();
        //    if (queryString != null && queryString.Count > 0)
        //    {
        //        //this.SearchExpressions = queryString.ToSearchExpressions();
        //        this.SearchExpressionGroup.AddSearchExpress(queryString.ToSearchExpressions());
        //    }
        //}
    }
    /// <summary>
    /// Specifies the direction in which to sort a list of items.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Sort from smallest to largest. For example, from A to Z.
        /// </summary>
        Ascending = 0,
        /// <summary>
        /// Sort from largest to smallest. For example, from Z to A.
        /// </summary>
        Descending = 1
    }

    public class SortExpression
    {
        public string SortField { get; set; }
        public SortDirection SortDirection { get; set; }
    }

    public class SearchByBTKeysArgument
    {
        public SearchByBTKeysArgument()
        {
            ProductStatus = ProductStatus.A;
        }

        public List<string> BTKeyList { get; set; }
        public int PageSize { get; set; }
        public MarketType? MarketType { get; set; }
        public string[] ESuppliers { get; set; }
        public bool SimonSchusterEnabled { get; set; }
        public string CountryCode { get; set; }
        public ProductStatus ProductStatus { get; set; }

        public bool IncludeProductFilter { get; set; }
        public string UserId { get; set; }
        public SortExpression SortExpression { get; set; }

    }
}
