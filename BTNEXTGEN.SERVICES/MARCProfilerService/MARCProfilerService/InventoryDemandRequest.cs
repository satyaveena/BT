using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360.NoSQL.API.Models
{
    public class InventoryDemandRequest
    {
        public BTKeys[] BTKeys { get; set; }

        public InventoryDemandWareHouses[] Warehouses { get; set; }

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
        public int PagePosition { get; set; }

        public string BTKey { get; set; }

        public string LEIndicator { get; set; }

        public string AccountInventoryType { get; set; }

        public string InventoryReserveNumber { get; set; }

    }

    public class InventoryDemandWareHouses
    {
        public string WarehouseID { get; set; }
    }
}