namespace BT.TS360API.ServiceContracts
{
    public class AccountSummary
    {
        public string BasketOrderFormId { get; set; }

        public string AccountID { get; set; }

        public string AccountAlias { get; set; }

        public string AccountERPNumber { get; set; }

        public int AccountType { get; set; }

        public string PONumber { get; set; }

        public int TotalLines { get; set; }

        public int TotalItems { get; set; }

        public decimal TotalListPrice { get; set; }

        public decimal TotalNetPrice { get; set; }

        public decimal EstimateProcessingChange { get; set; }

        public decimal EstimateTotalCartPrice { get; set; }

        public string ESupplierID { get; set; }

        public string ERPAccountGUID { get; set; }

        public bool IsHomeDelivery { get; set; }
    }
}
