using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class UserReviewType
    {
        public string ReviewTypeId { get; set; }
        public string ReviewType { get; set; }
        public string Ordinal { get; set; }
    }
}
