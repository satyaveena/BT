using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360.NoSQL.API.Models
{
    public class InventoryUDTRequest
    {
        public string BTKey { get; set; }

        public string Warehouse { get; set; }

        public int OnHandQuantity { get; set; }

        public int OnOrderQuantity { get; set; }

    }

}