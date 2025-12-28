using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Order
{


    public class OrderSearchSummaryResponse
    {

        public int OrderedQuantity { get; set; }

        public int ShippedQuantity { get; set; }

        public int CancelledQuantity { get; set; }

        public int BackOrderQuantity { get; set; }

        public int InProcessQuantity { get; set; }

        public int OnSaleHoldQuantity { get; set; }

        public int InReserveQuantity { get; set; }

        public int OpenQuantity { get; set; }

    }  
}
