using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360.NoSQL.API.Models
{
    public class InventoryDemandResponse
    {
        public BTKeyInventoryResult[] InventoryResults { get; set; }
    }

    public class BTKeyInventoryResult
    {
        public string BTKey { get; set; }

        public int LineItem { get; set; }

        //public string StockCondition { get; set; }

        //public string HaveMerchandiseCategory { get; set; }

        public string InventoryStatus { get; set; }

        public int TotalLast30Demand { get; set; }

        public string VIPTitle { get; set; }

        public bool HasDemand { get; set; }

        public BTKeyWarehouseResult[] Warehouses { get; set; }
    }

    public class BTKeyWarehouseResult
    {
        public string WarehouseId { get; set; }

        public int? InStockForRequest { get; set; }

        public int? OnOrderQuantity { get; set; }

        public int? Last30DayDemand { get; set; }

        public BTKeyWarehouseResult() {}
       
        public BTKeyWarehouseResult(string id)
        {
            WarehouseId = id;
            InStockForRequest = 0;
            OnOrderQuantity = 0;
            Last30DayDemand = 0;
        }

        public BTKeyWarehouseResult(string id, int inStockForRequest)
        {
            WarehouseId = id;
            InStockForRequest = inStockForRequest;
            OnOrderQuantity = 0;
            Last30DayDemand = 0;
        }
    }
}