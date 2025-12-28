using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace BT.ILSQueue.Business.Models
{
    public class MARC21
    {
        public string leader { get; set; }

        public List<MARC21ControlField> controlFields { get; set; }
        public List<MARC21DataField> dataFields { get; set; }

    }

    public class MARC21ControlField
    {
        public string tag { get; set; }
        public string data { get; set; }
    }

    public class MARC21DataField
    {
        [JsonProperty(Order = 1)]
        public string tag { get; set; }
        [JsonProperty(Order = 2)]
        public char ind1 { get; set; }
        [JsonProperty(Order = 3)]
        public char ind2 { get; set; }

        [JsonProperty(Order = 4)]
        public List<MARC21SubField> subFields;
    }

    public class MARC21SubField
    {
        public char code { get; set; }
        public string data { get; set; }
    }
}
