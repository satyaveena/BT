using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public class PricingReturn4ClientObject
    {
        public string LineItemId { get; set; }
        
        public string BTKey
        { get; set; }
        
        public string Price
        { get; set; }
        
        public string DisPercent
        { get; set; }
        
        public string ListPrice
        { get; set; }
        
        public string Catalog
        { get; set; }
        
        public string TotalPrice { get; set; }
        
        public string Quantity { get; set; }
        
        public string MyQuantity { get; set; }
        
        public string ToUpdateListPrice { get; set; }
        public PricingReturn4ClientObject()
        {
            LineItemId = "";
            BTKey = "";
            Price = "";
            DisPercent = "0";
            ListPrice = "0";
            Catalog = "";
            TotalPrice = "";
            Quantity = "";
            MyQuantity = "";
            ToUpdateListPrice = "";
        }
    }

    public class PromotionReturn4ClientObject
    {
        public string BTKey
        { get; set; }

        public string Text
        { get; set; }
        
        public string Content
        { get; set; }

        public PromotionReturn4ClientObject()
        {
            this.BTKey = "";
            this.Text = "";
            this.Content = "0";
        }

        public PromotionReturn4ClientObject(string btkey, string text, string cont)
        {
            this.BTKey = btkey;
            this.Text = text;
            this.Content = cont;
        }
    }
}
