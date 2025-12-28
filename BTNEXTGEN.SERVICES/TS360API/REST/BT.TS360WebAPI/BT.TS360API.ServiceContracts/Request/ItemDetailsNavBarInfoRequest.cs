using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BT.TS360Constants;
using BT.TS360API.ServiceContracts.Search;

namespace BT.TS360API.ServiceContracts.Request
{
    //..24235 #1
    public class CartItemDetailsNavBarInfoRequest : BaseRequest
    {
        public string CartId { get; set; }
        public string UserId { get; set; }
        public string LineItemId { get; set; }

        public SearchCartCriteria SearchCartCriteria { get; set; }
    }

    public class ItemDetailsInfo
    {
        public string CartName { get; set; }
        public int CartTitleCount { get; set; }
        public int CartQtyTotal { get; set; }
        public decimal CartPriceTotal { get; set; }
        public string PreviousBTKey { get; set; }
        public string PreviousLineItemID { get; set; }
        public string NextBTKey { get; set; }
        public string NextLineItemID { get; set; }
        public string TitleIndex { get; set; }
    }

    public class SearchCartCriteria
    {
        public bool IsQuickCartDetails { get; set; }
        public string FacetPath { get; set; }
        public string Keyword { get; set; }
        public string KeywordType { get; set; }
        public string MatchingBtkeys { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int SortBy { get; set; }
        public string QuickSortBy { get; set; }
        public int SortDirection { get; set; }
    }

    public class SearchItemDetailsNavBarInfoRequest : BaseRequest
    {
        public string BTKey { get; set; }
        public string UserId { get; set; }
        public MarketType? MarketType { get; set; }
        public bool SimonSchusterEnabled { get; set; }
        public string CountryCode { get; set; }
        public string[] ESuppliers { get; set; }

        public DataFromSearchResultsForItemDetails DataFromSearchResults { get; set; }
        public SearchArguments SearchArgs { get; set; }
    }

    public class DataFromSearchResultsForItemDetails
    {
        public int SearchTotalCount { get; set; }
        public string[] CurrentPageBTKeys { get; set; }
        public int CurrentPageIndex { get; set; }
    }
}
