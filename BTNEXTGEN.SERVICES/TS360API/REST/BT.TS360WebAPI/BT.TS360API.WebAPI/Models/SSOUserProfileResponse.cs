using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class SSOUserProfileResponse
    {
        public SSOUserProfile User;
        public string ErrorMessage { get; set; }
    }

    public class SSOUserProfile
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

    public class SSOUserInfoResponse
    {
        public string sub { get; set; }
        public string user_id { get; set; }
        public string organization_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        
        public string id_token { get; set; }

    }
}