using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace BT.TS360API.Authentication.Helpers
{
    public class TokenHelper
    {
        public static AuthenticationTicket GenerateAuthTicket(string userId, double expiredSeconds)
        {
            var identity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, userId.ToString()));

            var currentUtc = new SystemClock().UtcNow;
            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromSeconds(expiredSeconds));

            //var token = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            return ticket;
        }
    }
}