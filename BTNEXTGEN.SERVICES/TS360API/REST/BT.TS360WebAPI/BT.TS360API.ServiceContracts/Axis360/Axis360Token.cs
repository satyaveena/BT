using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BT.TS360API.ServiceContracts.Axis360
{
    public class Axis360Token
    {
        public DateTime CreatedDate { get; set; }
        public string AccessValue { get; set; }
        public string TokenType { get; set; }
        public int TokenDurationInSeconds { get; set; }

    }
}
