
namespace BT.TS360API.ServiceContracts.Pricing
{
    public class RequiredPricingInfo
    {
        public string AccountId { get; set; }
        public string PrimaryWarehouseCode { get; set; }
        public string ErpAccountNumber { get; set; }
        public string EMarketType { get; set; }
        public string ETier { get; set; }
        public string AccountPricePlanId { get; set; }
        public bool IsHomeDelivery { get; set; }
        public int BuildingNumbers { get; set; }
        public decimal ProcessingCharges { get; set; }
        public decimal ProcessingCharges2 { get; set; }
        public decimal SpokenWordCharge { get; set; }
        public float SalesTax { get; set; }
        public bool IsVIPAccount { get; set; }

        public RequiredPricingInfo()
        {
            AccountId = string.Empty;
            PrimaryWarehouseCode = string.Empty;
            ErpAccountNumber = string.Empty;
            EMarketType = string.Empty;
            ETier = string.Empty;
            AccountPricePlanId = string.Empty;
            IsHomeDelivery = false;
            IsVIPAccount = false;
            BuildingNumbers = 1;
            ProcessingCharges = ProcessingCharges2 = SpokenWordCharge = 0;
            SalesTax = 0;
        }
    }
}
