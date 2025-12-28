using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BT.TS360API.ServiceContracts.Request
{
    public class Axis360CheckCirculationRequest : BaseRequest
    {
        public List<string> ISBN { get; set; }
        public string UserID { get; set; }
        public string BasketSummaryID { get; set; }
    }
}

