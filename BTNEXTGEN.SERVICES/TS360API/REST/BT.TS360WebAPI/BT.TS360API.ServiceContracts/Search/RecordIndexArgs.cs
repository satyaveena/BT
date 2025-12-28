using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class RecordIndexArgs
    {

        public string CartId { get; set; }

        public string LineItemId { get; set; }

        public string BtKey { get; set; }

        public bool IsFromCartDetails { get; set; }

        public bool IsFromCartSummary { get; set; }

        public bool Isfromsearchresults { get; set; }

        public bool IsFromDuplicatePopup { get; set; }

        public string IncreasePageIndex { get; set; }

        public string DecreasePageIndex { get; set; }

        public string ToFirst { get; set; }

        public string ToLast { get; set; }

        public string Quantity { get; set; }
    }

}
