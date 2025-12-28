namespace BT.TS360API.ServiceContracts
{
    public class CartAccount
    {
        public string AccountID { get; set; } //key

        public string AccountName { get; set; }

        public int AccountType { get; set; }

        public string AccountTypeName { get; set; }

        public string ESupplierID { get; set; }

        public string AccountERPNumber { get; set; }

        public string PONumber { get; set; }

        //Not yet data binded
        public string AccountAlias { get; set; }

        public int LineItemCount { get; set; }

        public int TotalQuantity { get; set; }

        public decimal TotalListPrice { get; set; }

        public decimal TotalNetPrice { get; set; }

        public decimal EstimatedProcessingCharges { get; set; }

        public decimal EstimatedTotalCartPrice { get; set; }

        public string BasketSummaryID { get; set; }

        public bool IsHomeDelivery { get; set; }

        public string ERPAccountGUID { get; set; }

        public bool IsEntertainmentGridAccount { get; set; }

        public bool IsLibrarySystemAccount { get; set; }

        public int NumberOfBuilding { get; set; }

        public decimal ProcessingCharge { get; set; }

    }
}
