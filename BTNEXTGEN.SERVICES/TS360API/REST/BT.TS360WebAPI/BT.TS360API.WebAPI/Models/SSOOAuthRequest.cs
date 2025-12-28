using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace BT.TS360API.WebAPI.Models
{
    public class SSOOAuthRequest
    {
        public string UserToken { get; set; }

        public override string ToString()
        {
            return string.Format("UserToken:{0}", UserToken);
        }
    }
}