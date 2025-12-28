using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Inventory
{
    public class InventoryStockArg
    {
        public string BTKey { get; set; }
        public string UPC { get; set; }
        public int Quantity { get; set; }

        public InventoryStockArg() { }

        public InventoryStockArg(string btKey, string upc, int quantity)
        {
            BTKey = btKey;
            UPC = upc;
            Quantity = quantity;
        }
    }
}
