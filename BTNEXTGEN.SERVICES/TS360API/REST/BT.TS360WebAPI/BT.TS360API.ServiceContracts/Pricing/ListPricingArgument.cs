namespace BT.TS360API.ServiceContracts.Pricing
{
    public class ListPricingArgument
    {
        public string BTKey { get; set; }

        //public string AccountId { get; set; }

        public string eMarket { get; set; }

        public string eTier { get; set; }
    }
}
