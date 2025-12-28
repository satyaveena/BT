using BT.TS360API.ServiceContracts.Order;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Request
{
    public enum ViewPOOption { 
        ViewAllPos = 0,
        ViewOpenPos = 1
    }
    public class OrderLineRequest
    {
        public List<string> AccountNumbers { get; set; }
        public List<string> ArtistAuthors { get; set; }
        public List<string> BTOrderNumbers { get; set; }
        public List<string> ISBNs { get; set; }
        public List<string> ISBNsOrUPCs { get; set; }
        public DateRange OrderDateRange { get; set; }

        public string POOrder { get; set; }//CustomerPONumber
        public SearchType POOrderSearchType { get; set; }

        public DateRange PubDateRanges { get; set; }
        public List<string> Titles { get; set; }
        public List<string> CustomerItemNumbers { get; set; }
        public List<string> UPC { get; set; }
        public List<string> Formats { get; set; }
        public List<string> Warehouses { get; set; }
        public string POLine { get; set; } //?
        public List<string> CustomerPONumber { get; set; }
        public List<string> Statuses { get; set; }
        public int ViewPoOption { get; set; }
        public bool IsOrderLineSearch { get; set; }

        
    }

    public class DateRange
    {
         public string FromDate { get; set; }
         public string ToDate { get; set; }
    }

    public class OrderSearchLinesRequest : OrderLineRequest
    {
        public string SortBy { get; set; }
        public string SortByDirection { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

    public class SearchOrdersRequest
    {
        public List<string> AccountNumbers { get; set; }
        public string POOrder { get; set; }//CustomerPONumber
        public string SortBy { get; set; }
        public string SortByDirection { get; set; }
    }

    public class SearchOrdersRespond : OrderSearchSummaryResponse
    {
        public string OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public string AccountNumber { get; set; }
    }

    public class SearchOrdersResult 
    {
        public long LinesCount { get; set; }
        public string POOrder { get; set; }//CustomerPONumber
        public List<SearchOrdersRespond> SearchOrdersRespondList { get; set; }
    }

}
