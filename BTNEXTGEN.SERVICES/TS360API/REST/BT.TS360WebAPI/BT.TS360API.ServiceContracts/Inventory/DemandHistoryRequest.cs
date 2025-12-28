using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Inventory
{
    public class DemandHistoryRequest
    {
        public string BTKey { get; set; }
        public int? PageIndex { get; set; }
        public string PrimaryWareHouseCode { get; set; }
        public string SecondaryWareHouseCode { get; set; }
    }
}
