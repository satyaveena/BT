using BT.CDMS.Business.Models;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BT.CDMS.API
{
    public partial class Startup
    {
        public void ConfigureAuthServer(IAppBuilder app)
        {
            //Setup Authorization Server
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                ApplicationCanDisplayErrors = true,
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(24),

//#if DEBUG
                AllowInsecureHttp = true,
//#endif
                // Authorization server provider which controls the life cycle of Authorization server
                Provider = new OAuthAuthorizationServerProvider
                {
                    OnValidateClientAuthentication = ValidateClientAuthentication,
                    OnGrantResourceOwnerCredentials = GrantResourceOwnerCredentials,
                    OnGrantClientCredentials = GrantClientCredetails

                },
                RefreshTokenProvider = new AuthenticationTokenProvider
                {
                    OnCreate = CreateRefreshToken,
                    OnReceive = ReceiveRefreshToken
                }
            });
        }
        private Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            AuthKey authKey = new AuthKey();
            string clientId;
            string clientSecret;
            if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                var authData = authKey.GetConfigValue(clientId);
                if (authData != null)
                {
                    if (clientId == authData.ApiKey && clientSecret == authData.ApiPassphrase)
                    {
                        context.Validated();
                    }
                }
            }
            return Task.FromResult(0);
        }

        private Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(
                new GenericIdentity(context.UserName, OAuthDefaults.AuthenticationType), 
                context.Scope.Select(x => new Claim("urn:oauth:scope", x)));
            context.Validated(identity);
            return Task.FromResult(0);
        }

        private Task GrantClientCredetails(OAuthGrantClientCredentialsContext context)
        {
            var identity = new ClaimsIdentity(
                new GenericIdentity(context.ClientId, OAuthDefaults.AuthenticationType), 
                context.Scope.Select(x => new Claim("urn:oauth:scope", x)));
            context.Validated(identity);
            return Task.FromResult(0);
        }

        private void CreateRefreshToken(AuthenticationTokenCreateContext context)
        {
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(24));
            context.SetToken(context.SerializeTicket());
        }

        private void ReceiveRefreshToken(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}