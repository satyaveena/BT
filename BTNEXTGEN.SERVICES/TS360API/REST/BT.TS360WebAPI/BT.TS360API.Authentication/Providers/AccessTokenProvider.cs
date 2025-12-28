using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using BT.TS360API.Authentication.DataAccess;

namespace BT.TS360API.Authentication.Providers
{
    public class AccessTokenProvider : AuthenticationTokenProvider
    {
        public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            context.SetToken(context.SerializeTicket());

            var form = await context.Request.ReadFormAsync();
            var tokenEndpointRequest = new TokenEndpointRequest(form);
            // in case request grant_type is authorization_code (exchange authcode to get access token)
            if (tokenEndpointRequest.IsAuthorizationCodeGrantType)
            {
                var authCode = tokenEndpointRequest.AuthorizationCodeGrant.Code;

                // Update auth Log
                await AuthDAOManager.UpdateAccessTokenByAuthCode(authCode, context);
            }
        }

        public override Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);

            return Task.FromResult(0);
        }
    }
}