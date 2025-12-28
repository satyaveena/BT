using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BT.TS360API.ServiceContracts.Request
{
    [DataContract]
    public class Axis360CheckInventoryRequest
    {
        [DataMember]
        public string Axis360CustomerID { get; set; }
        [DataMember]
        public List<string> ISBNList { get; set; }
    }

}