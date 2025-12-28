using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.ExternalDataSendService.Models
{
    public class ExternalApiInfo
    {
        public string ApiUrl { get; set; }
        public List<string> SendOrgFields { get; set; }
        public ApiAccessToken ApiAccessToken { get; set; }
    }

    public class ApiAccessToken
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string EndpointUrl { get; set; }
    }

    public class ExternalApiInfoEx : ExternalApiInfo
    {
        public string PremiumServiceCode { get; set; }
    }
}