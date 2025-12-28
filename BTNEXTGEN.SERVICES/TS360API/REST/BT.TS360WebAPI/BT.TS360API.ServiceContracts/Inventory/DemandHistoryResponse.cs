using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Inventory
{
    public class DemandHistoryResponse
    {
        public int? PrePublicationDemand { get; set; }
        public int? PostPublicationDemand { get; set; }
        public DemandHistoryPeriod[] DemandHistoryResults { get; set; }
    }

    public class DemandHistoryPeriod
    {
        public string DemandPeriod { get; set; }

        public DemandHistoryWareHouses[] WareHouses { get; set; }
    }

    public class DemandHistoryWareHouses
    {
        public string WarehouseID { get; set; }

        public string WarehouseType { get; set; }

        public int DemandQuantity { get; set; }
    }
}
