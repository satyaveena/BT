using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace BT.ILSQueue.Business.Models
{
    public class PolarisPORequest
    {
        [JsonProperty(Order = -2)]
        public string ExternalID { get; set; }


        [JsonProperty(Order = -2)]
        public string Vendor { get; set; }


        [JsonProperty(Order = -2)]
        public string OrderedAtLocation { get; set; }


        [JsonProperty(Order = -2)]
        public int OrderType { get; set; }


        [JsonProperty(Order = -2)]
        public int PaymentMethod { get; set; }
    }
}
