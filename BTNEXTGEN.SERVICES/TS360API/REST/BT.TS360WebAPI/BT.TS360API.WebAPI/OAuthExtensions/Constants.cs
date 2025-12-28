using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.OAuth
{
    public class Constants
    {
        public const string Success = "1";
        public const string Failure = "0";
        public const string DisabledUser = "401";
        public const string InvalidUser = "402";
        public const string InternalServer = "500";

        public const string ACCESS_TOKEN_EXPIRATION = "SSOAccessTokenExpiration";
        public const string SSO_SESSION_LIFETIME = "SSOSessionLifeTime";
    }
}