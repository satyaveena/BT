namespace BT.TS360API.ServiceContracts.Inventory
{
    public class InventoryDemandRequest
    {
        public BTKeys[] BTKeys { get; set; }

        public WareHouses[] Warehouses { get; set; }

        public string VIPEnabled { get; set; }

        public string MarketType { get; set; }

        public string CountryCode { get; set; }

        public string BookPrimaryWarehouseCode { get; set; }

        public string BookSecondaryWarehouseCode { get; set; }

        public string EntertainmentPrimaryWarehouseCode { get; set; }

        public string EntertainmentSecondaryWarehouseCode { get; set; }

        public bool OnItemDetail { get; set; }
    }

    public class BTKeys
    {
        public BTKeys()
        {

        }
        public int PagePosition { get; set; }

        public string BTKey { get; set; }

        public string LEIndicator { get; set; }

        public string AccountInventoryType { get; set; }

        public string InventoryReserveNumber { get; set; }

        public BTKeys(int pagePosition, string btkey, string leIndicator, string accountInventoryType,
            string invReserveNumber)
        {
            PagePosition = pagePosition;
            BTKey = btkey;
            LEIndicator = leIndicator;
            AccountInventoryType = accountInventoryType;
            InventoryReserveNumber = invReserveNumber;
        }
    }

    public class WareHouses
    {
        public string WarehouseID { get; set; }
    }
}
