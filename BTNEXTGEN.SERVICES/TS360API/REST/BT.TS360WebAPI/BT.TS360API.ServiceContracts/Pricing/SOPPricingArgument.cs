namespace BT.TS360API.ServiceContracts.Pricing
{
    public class SOPPricingArgument
    {
        //public string BasketSummaryID { get; set; }

        public string BTKey { get; set; }

        public string PlanID { get; set; }

        public string PriceKey { get; set; }

        public int? TotalQuanity { get; set; }

        public int? OrderLineQuanity { get; set; }

        public decimal? ListPrice { get; set; }

        public SOPPricingArgument(SOPPricingArgument source)
            : this(source.BTKey, source.PlanID, source.PriceKey, source.TotalQuanity, source.OrderLineQuanity, source.ListPrice)
        {
        }

        public SOPPricingArgument(string btkey, string planid, string pricekey, int? totalqty, int? orderqty, decimal? listprice)
        {
            BTKey = btkey;
            PlanID = planid;
            PriceKey = pricekey;
            TotalQuanity = totalqty;
            OrderLineQuanity = orderqty;
            ListPrice = listprice;
        }
        public SOPPricingArgument()
        {
        }
    }
}
