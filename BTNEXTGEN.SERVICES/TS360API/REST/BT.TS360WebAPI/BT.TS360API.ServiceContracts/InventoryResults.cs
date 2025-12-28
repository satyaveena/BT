using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public class InventoryResults
    {
        public string BTKey { get; set; }

        public List<InventoryStockStatus> InventoryStock { get; set; }

        public bool DisplayInventoryForAllWareHouse { get; set; }

        public string ERPLineStatusCode { get; set; }

        public string ERPLineStatusMessage { get; set; }

        public bool IsStockCheckInventory { get; set; }

        public string ProductType { get; set; }

        public int TotalLast30Demand { get; set; }

        public InventoryResults(string btKey, List<InventoryStockStatus> inventoryStock, bool displayInventoryForAllWareHouse)
        {
            BTKey = btKey;
            InventoryStock = inventoryStock;
            DisplayInventoryForAllWareHouse = displayInventoryForAllWareHouse;
            ERPLineStatusCode = string.Empty;
            ERPLineStatusMessage = string.Empty;
            IsStockCheckInventory = false;
        }

        public InventoryResults(string btKey, List<InventoryStockStatus> inventoryStock, bool displayInventoryForAllWareHouse,
                                string erpLineStatusCode, string erpLineStatusMessage, bool isStockCheckInventory)
        {
            BTKey = btKey;
            InventoryStock = inventoryStock;
            DisplayInventoryForAllWareHouse = displayInventoryForAllWareHouse;
            ERPLineStatusCode = erpLineStatusCode;
            ERPLineStatusMessage = erpLineStatusMessage;
            IsStockCheckInventory = isStockCheckInventory;
        }

        public InventoryResults()
        {
            BTKey = string.Empty;
            InventoryStock = new List<InventoryStockStatus>();
            DisplayInventoryForAllWareHouse = false;
            ERPLineStatusCode = string.Empty;
            ERPLineStatusMessage = string.Empty;
            IsStockCheckInventory = false;
        }
    }

    public class InventoryStockStatus
    {
        public int LineItem { get; set; }

        public int StockCondition { get; set; }

        public int InStockForRequest { get; set; }

        public int PreorderForRequest { get; set; }

        public int BackorderForRequest { get; set; }

        public int QuantityAvailable { get; set; }

        public int OnOrderQuantity { get; set; }

        public string OnHandInventory { get; set; }

        public string WareHouse { get; set; }

        public string WareHouseCode { get; set; }

        public string FormalWareHouseCode { get; set; }

        public int InvDemandNumber { get; set; }

        public InventoryStockStatus(int lineItem, int stockCondition, int inStockForRequest, int preorderForRequest, int backorderForRequest, int quantityAvailable, int quantityOnOrder, string onHandInventory, string wareHouse, string wareHouseCode)
        {
            LineItem = lineItem;
            StockCondition = stockCondition;
            InStockForRequest = inStockForRequest;
            PreorderForRequest = preorderForRequest;
            BackorderForRequest = backorderForRequest;
            QuantityAvailable = quantityAvailable;
            OnOrderQuantity = quantityOnOrder;
            OnHandInventory = onHandInventory;
            WareHouse = wareHouse;
            WareHouseCode = wareHouseCode;
        }

        public InventoryStockStatus()
        {
            LineItem = 0;
            StockCondition = 0;
            InStockForRequest = 0;
            PreorderForRequest = 0;
            BackorderForRequest = 0;
            QuantityAvailable = 0;
            OnOrderQuantity = 0;
            OnHandInventory = string.Empty;
            WareHouse = string.Empty;
            WareHouseCode = string.Empty;
        }
    }

    public class InventoryStockForDemand
    {
        public string InvCode { get; set; }
        public string InvName { get; set; }
        public string InventoryType { get; set; }
        public int DemandNumber { get; set; }
        public string DemandPeriod { get; set; }
    }

    public class InventoryResultForItem
    {
        public string BtKey { get; set; }
        public List<InventoryStockForDemand> InvStockForDemand { get; set; }
    }

    public class InventoryDataContract
    {
        public List<InventoryResults> InventoryResultsList { get; set; }

        public List<SiteTermObject> InventoryStatus { get; set; }
    }
}
