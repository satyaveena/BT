using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Request
{
    public class LineStatusResponse
    {
        public string OrderLineId { get; set; }
        public string LineItemStatus { get; set; }
        public string ShippedExpectedDate { get; set; }
        public string ShippedTrackingNumber { get; set; }
        public string ShippedCarrier { get; set; }
        public string CancelledDate { get; set; }
        public string CancelledReason { get; set; }
        public string BackOrderedReason { get; set; }
        public string BackOrderedNumOfDates { get; set; }
        public int PartialOrderedQty { get; set; }
        public int PartialShippedQty { get; set; }
        public int PartialBackOrderQty { get; set; }
        public int PartialInProcessQty { get; set; }
        public int PartialCancelledQty { get; set; }
    }

   
}
