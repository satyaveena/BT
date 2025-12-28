using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Request
{
    public class OrderShippingStatusRequest
    {
        public List<string> ShipTrackingNumbers { get; set; }
    }
}
