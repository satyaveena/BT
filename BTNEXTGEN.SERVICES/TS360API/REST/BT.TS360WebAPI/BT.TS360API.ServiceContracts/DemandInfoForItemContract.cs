using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class DemandInfoForItemContract
    {
        public List<DemandInventoryContract> DemandInfos { get; set; }

        public int PrePublicationDemandNumber { get; set; }

        public int PostPublicationDemandNumber { get; set; }

        public int TotalItemCount { get; set; }
    }

    public class DemandInventoryContract
    {
        public string Inv1Name { get; set; }

        public string Inv2Name { get; set; }

        public string Inv3Name { get; set; }

        public string Inv4Name { get; set; }

        public int Inv1Value { get; set; }

        public int Inv2Value { get; set; }

        public int Inv3Value { get; set; }

        public int Inv4Value { get; set; }

        public string DemandPeriod { get; set; }
    }
}
