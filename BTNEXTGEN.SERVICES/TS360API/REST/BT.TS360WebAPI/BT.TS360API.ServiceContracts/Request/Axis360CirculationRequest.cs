using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BT.TS360API.ServiceContracts.Request
{
    public class Axis360CirculationRequest : BaseRequest
    {
        public string ISBN { get; set; }
        public string CustomerID { get; set; }
    }
}

