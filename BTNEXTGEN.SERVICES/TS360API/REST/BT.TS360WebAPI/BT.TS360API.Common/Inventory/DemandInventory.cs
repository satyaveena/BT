using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Inventory
{
    public class DemandInventory
    {
        public string WhsId { get; set; }
        public string WhsName { get; set; }
        public string WhsType { get; set; }
        public int DemandQuantity { get; set; }
    }
}
