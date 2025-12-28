using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.Authentication.Models
{
    public class GetAuthCodeRequest
    {
        public string UserId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}