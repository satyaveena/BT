namespace BT.TS360API.ServiceContracts.Inventory
{
    public class InventoryDemandResponse
    {
        public BTKeyInventoryResult[] InventoryResults { get; set; }
    }

    public class BTKeyInventoryResult
    {
        public string BTKey { get; set; }

        public string LineItem { get; set; }

        //public string StockCondition { get; set; }

        //public string HaveMerchandiseCategory { get; set; }

        public string InventoryStatus { get; set; }

        public int TotalLast30Demand { get; set; }

        public string VIPTitle { get; set; }

        public BTKeyWarehouseResult[] Warehouses { get; set; }

        public bool HasDemand { get; set; }
    }

    public class BTKeyWarehouseResult
    {
        public string WarehouseId { get; set; }

        public int InStockForRequest { get; set; }

        public int OnOrderQuantity { get; set; }

        public int Last30DayDemand { get; set; }

    }
}
