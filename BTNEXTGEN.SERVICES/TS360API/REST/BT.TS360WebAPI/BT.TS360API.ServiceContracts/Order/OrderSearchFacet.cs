using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Order
{

    public static class FacetNameConstants
    {
        public const string Format="Format";
        public const string LineItemStatus = "LineItemStatus";
        public const string OrderDateRange = "OrderDateRange";
        public const string PubDateRange = "PubDateRange";
        public const string Warehouse = "Warehouse";
    }
    public class SearchLineFacetsResponse
    {
        public TextFacet Format { get; set; }
        public TextFacet LineItemStatus { get; set; }
        public TextFacet Warehouse { get; set; }
        public DateRangeFacet OrderDate { get; set; }
        public DateRangeFacet PubDate { get; set; }
    }

    public class SearchOrderFacetsResponse
    {
        public DateRangeFacet OrderDate { get; set; }
    }

    public class TextFacet
    {
        public string Name { get; set; }
        public List<TextFacetData> Data { get; set; }
    }

    public class DateRangeFacet
    {
        public string Name { get; set; }
        public DateRangeFacetData Data { get; set; }
    }

    public class TextFacetData
    {
        public string _id { get; set; }
        public int count { get; set; }
    }

    public class DateRangeFacetData
    {
        public string _id { get; set; }
        public int Day15 { get; set; }
        public int Day30 { get; set; }
        public int Day90 { get; set; }
    }

    
}
