using System.Runtime.Serialization;

namespace BT.TS360API.ServiceContracts.Profiles
{
    [DataContract]
    public class Account
    {
        public Account(string accId)
        {
            AccountId = accId;
        }

        [DataMember]
        public string AccountId { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string Account8Id { get; set; }
        [DataMember]
        public bool? IsTOLAS { get; set; }
        [DataMember]
        public bool? HomeDeliveryAccount { get; set; }
        [DataMember]
        public Warehouse PrimaryWarehouse { get; set; }
        [DataMember]
        public Warehouse SecondaryWarehouse { get; set; }
        [DataMember]
        public string PrimaryWarehouseCode { get; set; }
        [DataMember]
        public string SecondaryWarehouseCode { get; set; }
        [DataMember]
        public string EMarketType { get; set; }
        [DataMember]
        public string ETier { get; set; }
        [DataMember]
        public string BillToAccountNumber { get; set; }
        [DataMember]
        public string SOPPricePlanId { get; set; }
        [DataMember]
        public bool? IsBillingAccount { get; set; }
        [DataMember]
        public int? NumberOfBuilding { get; set; }
        [DataMember]
        public decimal? ProcessingCharge { get; set; }
        [DataMember]
        public decimal? ProcessingCharges2 { get; set; }
        [DataMember]
        public decimal? ProcessingCharges3 { get; set; }
        [DataMember]
        public float? SalesTax { get; set; }
        [DataMember]
        public string ESupplier { get; set; }
        [DataMember]
        public bool? CheckLEReserve { get; set; }
        [DataMember]
        public string AccountInventoryType { get; set; }
        [DataMember]
        public string InventoryReserveNumber { get; set; }

        public string ProductType { get; set; }

        public string DisabledReasonCode { get; set; }

        public string PrimaryWarehouseName { get; set; }

        public string SecondaryWarehouseName { get; set; }

        public string AccountType { get; set; }

        public string ProductTypeName { get; set; }

        public string DisabledReasonText { get; set; }
    }
}
