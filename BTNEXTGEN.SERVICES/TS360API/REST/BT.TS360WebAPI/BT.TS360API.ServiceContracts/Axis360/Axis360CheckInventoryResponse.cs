using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BT.TS360API.ServiceContracts.Axis360
{
    [DataContract]
    public class Axis360CheckInventoryResponse
    {
        [DataMember]
        public List<Axis360InventoryItem> Axis360InventoryItemList { get; set; }
        [DataMember]
        public string ESupplierAccountNumber { get; set; }
    }

    [DataContract]
    public class Axis360InventoryItem
    {
        [DataMember]
        public string ISBN { get; set; }
        [DataMember]
        public bool HasInventory { get; set; }
    }
}
