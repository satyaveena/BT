namespace BT.TS360API.ServiceContracts.Pricing
{
    public class PromotionPrice
    {
        public string BtKey { get; set; }
        public decimal Price { get; set; }
        public string PromotionCode { get; set; }
        public string BasketDisplayText { get; set; }
    }
}
