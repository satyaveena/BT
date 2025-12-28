using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.Authentication.Models
{
    /// <summary>
    /// Application Key and Passphrase.
    /// </summary>
    public class ApplicationAuthKey
    {
        public string ApiKey { get; set; }
        public string ApiPassphrase { get; set; }
        public string ApiDescription { get; set; }
        public string DomainURL { get; set; }
        public string PremiumServiceCode { get; set; }
    }
}