using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    public class LineItemAcknowledgement
    {
        public int OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public string Warehouse { get; set; }

        public int ShippedQuantity { get; set; }

        public int BackOrderedQuantity { get; set; }

        public int CancelledQuantity { get; set; }

        public int InProcessQuantity { get; set; }

        public int RevervedAwaitingReleaseQuantity { get; set; }
    }
}
