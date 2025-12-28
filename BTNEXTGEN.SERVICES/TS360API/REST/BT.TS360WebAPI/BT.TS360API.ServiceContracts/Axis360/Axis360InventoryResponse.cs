using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BT.TS360API.ServiceContracts.Axis360
{
    [DataContract]
    public class Axis360InventoryResponse
    {
        [DataMember(Name = "isbn")]
        public string ISBN { get; set; }
        [DataMember(Name = "circulationScope")]
        public string CirculationScope { get; set; }
        [DataMember(Name = "collection")]
        public List<Axis360Inventory> Collection { get; set; }
        [DataMember(Name = "status")]
        public Axis360Status Status { get; set; }
        [DataMember(Name = "circulationType")]
        public string CirculationType { get; set; }
    }

    [DataContract]
    public class Axis360Inventory
    {
        [DataMember(Name = "libraryName")]
        public string LibraryName { get; set; }
        [DataMember(Name = "totalInventory")]
        public int? TotalInventory { get; set; }
        [DataMember(Name = "totalLicenses")]
        public int? TotalLicenses { get; set; }
        [DataMember(Name = "onLoan")]
        public int? OnLoan { get; set; }
        [DataMember(Name = "remainingCheckouts")]
        public int? RemainingCheckouts { get; set; }
        [DataMember(Name = "last12monthsCirculation")]
        public int? Last12monthsCirculation { get; set; }
        [DataMember(Name = "lendingModel")]
        public string LendingModel { get; set; }
        [DataMember(Name = "lastCheckoutDate")]
        public DateTime? LastCheckoutDate { get; set; }
        [DataMember]
        public string LastCheckoutDateString { get; set; }
        [DataMember(Name = "expirationDate")]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public string ExpirationDateString { get; set; }
        [DataMember(Name = "holds")]
        public int? Holds { get; set; }
        [DataMember(Name = "holdRatio")]
        public double? HoldRatio { get; set; }
        [DataMember(Name = "circulationType")]
        public string CirculationType { get; set; }
        [DataMember(Name = "odsPurchaseOptionLiteral")]
        public string ODSPurchaseOptionLiteral { get; set; }

    }

    [DataContract]
    public class Axis360Status
    {
        [DataMember(Name = "Code")]
        public string Code { get; set; }
        [DataMember(Name = "Message")]
        public string Message { get; set; }
    }
}
