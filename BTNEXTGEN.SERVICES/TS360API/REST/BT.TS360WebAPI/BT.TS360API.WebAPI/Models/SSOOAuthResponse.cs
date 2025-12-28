using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class SSOOAuthResponse
    {
        public SSOOAuthUser User;
        public string SSOUrl {get;set;}
        public string ErrorMessage { get; set; }

    }

    public class SSOOAuthUser
    {
        public string UserToken { get; set; }
        public string OrganizationName { get; set; }
        public string MarketType { get; set; }
        public string UserName { get; set; }
        public string UserAlias { get; set; }
        public string EmailAddress { get; set; }
        public string CIPEnabled { get; set; }
        public string BillToAccountID { get; set; }
        public DateTime CIPLastLogin { get; set; }
    }
}