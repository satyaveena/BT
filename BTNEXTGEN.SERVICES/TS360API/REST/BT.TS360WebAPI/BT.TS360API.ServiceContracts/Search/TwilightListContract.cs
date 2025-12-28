using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts.Search
{
    public class TwilightListContract
    {
        
        public string ListTitle { get; set; }

        
        public List<ItemDataContract> ProductItems { get; set; }

        
        public string ViewAllLink { get; set; }

        
        public string ReleaseCalendarLink { get; set; }
    }

   
    public class RelatedProductListContract
    {
        
        public string ListTitle { get; set; }

        
        public List<RelatedProductContract> ProductItems { get; set; }

        
        public string ViewAllLink { get; set; }

        
        public string ReleaseCalendarLink { get; set; }
        
        public bool IsPrimaryCartSet { get; set; }
    }

   
    public class RelatedProductContract
    {
        
        public string CCUrl { get; set; }
        
        public string ISBN { get; set; }
        
        public string BTKey { get; set; }
        
        public string Author { get; set; }
        
        public string Title { get; set; }
        
        public string ListPriceText { get; set; }
        
        public string DiscountPriceText { get; set; }
        
        public string FormatIconPath { get; set; }
        
        public string ProductFormat { get; set; }
        
        public string ProductType { get; set; }
        
        public string GTIN { get; set; }
        
        public string Upc { get; set; }
        
        public string Catalog { get; set; }
        
        public int Quantity { get; set; }
        
        public string ItemDetailsUrl { get; set; }
        
        public bool HasQuantityInPrimaryCart { get; set; }
        
        public string FormatIconClass { get; set; }
        
        public string IncludedFormatClass { get; set; }
    }
}
