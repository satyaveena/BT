namespace BT.TS360API.ServiceContracts.Pricing
{
    public class TOLASPricingArgument
    {
        public string BasketSummaryID { get; set; }

        public string BTKey { get; set; }

        public decimal AcceptableDiscount { get; set; }

        public string PriceKey { get; set; }

        public string ProductLine { get; set; }

        public bool ReturnFlag { get; set; }

        public string AccountERPNumber { get; set; }

        public string PrimaryWarehouse { get; set; }

        public int TotalLineQuanity { get; set; }

        public decimal? ListPrice { get; set; }

        public string Upc { get; set; }

        public string ItemId { get; set; }
    }
}
