using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class PunchOutSetupSQL
    {
        public string LoginEmail { get; set; }

        public string RequestPayloadID { get; set; }

        public string FromDomain { get; set; }

        public string FromIdentity { get; set; }
        public string ToDomain { get; set; }

        public string ToIdentity { get; set; }
        public string SenderDomain { get; set; }

        public string SenderIdentity { get; set; }
        public string SenderUserAgent { get; set; }

        public string BuyerCookie { get; set; }
        public string Extrinsic { get; set; }

        public string Token { get; set; }

        public string BrowserFormPost { get; set;  }
    }
}
