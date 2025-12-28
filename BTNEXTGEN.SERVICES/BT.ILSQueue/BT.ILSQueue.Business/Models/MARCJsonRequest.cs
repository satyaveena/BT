using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
    public class MARCJsonRequest
    {
        public String SortColumn { get; set; }
        public String BasketSummaryID { get; set; }
        public String SortDirection { get; set; }
        public String ProfileID { get; set; }
        public String FullIndicator { get; set; }
        public Boolean IsOrdered { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsOCLCEnabled { get; set; }
        public Boolean IsBTEmployee { get; set; }
        public Boolean HasInventoryRules { get; set; }
        public String MarketType { get; set; }
        public DateTime RequestDateTime { get; set; }
    }
}
