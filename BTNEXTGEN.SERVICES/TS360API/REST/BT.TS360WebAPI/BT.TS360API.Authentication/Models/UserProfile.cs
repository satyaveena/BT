using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.Authentication.Models
{
    public class UserProfile
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string UserAlias { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }
        public bool IsCustomerAdmin { get; set; }
    }
}