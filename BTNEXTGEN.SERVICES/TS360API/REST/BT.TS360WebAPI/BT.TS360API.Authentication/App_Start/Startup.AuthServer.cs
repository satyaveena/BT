using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using BT.TS360API.Authentication.Constants;
using BT.TS360API.Authentication.DataAccess;
using System.Collections.Concurrent;
using BT.TS360API.Authentication.Providers;
using System.Collections.Generic;
using Microsoft.Owin.Logging;

namespace BT.TS360API.Authentication
{
    public partial class Startup
    {
        public static ILogger Logger;
        public static OAuthAuthorizationServerOptions OAuthServerOptions { get; set; }

        public static OAuthBearerAuthenticationOptions OAuthBearerOptions = new OAuthBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                AuthenticationType = "Bearer"
            };
        
        static Startup()
        {
            
            OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString(Paths.TokenPath),
                ApplicationCanDisplayErrors = true,
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(24),

                AuthorizeEndpointPath = new PathString(Paths.AuthorizePath),
                AuthorizationCodeExpireTimeSpan = TimeSpan.FromMinutes(10),
//#if DEBUG
                AllowInsecureHttp = true,
//#endif
                // Authorization server provider which controls the life cycle of Authorization server
                Provider = new OAuthAuthorizationServerProvider
                {
                    OnValidateClientAuthentication = ValidateClientAuthentication,
                    OnValidateTokenRequest = OnValidateTokenRequest,
                    OnValidateClientRedirectUri = OnValidateClientRedirectUri,
                    OnGrantResourceOwnerCredentials = OnGrantResourceOwnerCredentials,
                    OnGrantClientCredentials = OnGrantClientCredetails,

                    OnGrantAuthorizationCode = OnGrantAuthorizationCode,
                    OnMatchEndpoint = OnMatchEndpoint,
                    OnValidateAuthorizeRequest = OnValidateAuthorizeRequest,
                    OnAuthorizeEndpoint = OnAuthorizeEndpoint,
                    OnAuthorizationEndpointResponse = OnAuthorizationEndpointResponse
                },

                // Authorization code provider which creates and receives the authorization code.
                AuthorizationCodeProvider = new AuthorizationCodeProvider(),

                AccessTokenProvider = new AccessTokenProvider(),

                // Refresh token provider which creates and receives referesh token
                RefreshTokenProvider = new RefreshTokenProvider()
            };
        }

        public void ConfigureAuthServer(IAppBuilder app)
        {
            Logger = app.CreateLogger("AuthenticationAPI");

            //app.UseOAuthBearerAuthentication(OAuthBearerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                AccessTokenFormat = OAuthServerOptions.AccessTokenFormat,
                AccessTokenProvider = OAuthServerOptions.AccessTokenProvider,
            });

            // Setup Authorization Server
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        private static Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                var authConfigDAO = new AuthConfigDAO();
                var application = authConfigDAO.GetApplicationIdentity(clientId);
                if (application != null)
                {
                    if (clientId == application.ApiKey && clientSecret == application.ApiPassphrase)
                    {
                        context.Validated();
                    }
                }
            }
            return Task.FromResult(0);
        }

        public static Task OnValidateTokenRequest(OAuthValidateTokenRequestContext context)
        {
            context.Validated();

            return Task.FromResult(0);
        }

        public static Task OnValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            context.Validated();

            return Task.FromResult(0);
        }

        private static Task OnValidateAuthorizeRequest(OAuthValidateAuthorizeRequestContext context)
        {
            if (string.IsNullOrEmpty(context.Request.Query["user_id"]))
                Logger.WriteError("Authorize endpoint request missing required user_id parameter");
            else
                context.Validated();

            return Task.FromResult(0);
        }

        /// <summary>
        /// Called when grant_type is password.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task OnGrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(
                new GenericIdentity(context.UserName, OAuthDefaults.AuthenticationType), 
                context.Scope.Select(x => new Claim("urn:oauth:scope", x)));
            context.Validated(identity);
            return Task.FromResult(0);
        }

        private static Task OnGrantClientCredetails(OAuthGrantClientCredentialsContext context)
        {
            var identity = new ClaimsIdentity(
                new GenericIdentity(context.ClientId, OAuthDefaults.AuthenticationType), 
                context.Scope.Select(x => new Claim("urn:oauth:scope", x)));
            context.Validated(identity);
            return Task.FromResult(0);
        }

        public static Task OnGrantAuthorizationCode(OAuthGrantAuthorizationCodeContext context)
        {
            context.Validated();

            return Task.FromResult(0);
        }

        public static Task OnMatchEndpoint(OAuthMatchEndpointContext context)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// Called before the AuthorizationEndpoint redirects its response to the caller. The response could be the
        /// token, when using implicit flow or the AuthorizationEndpoint when using authorization code flow.  
        /// An application may implement this call in order to do any final modification of the claims being used 
        /// to issue access or refresh tokens. This call may also be used in order to add additional 
        /// response parameters to the authorization endpoint's response.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>Task to enable asynchronous execution</returns>
        private static Task OnAuthorizationEndpointResponse(OAuthAuthorizationEndpointResponseContext context)
        {
            return Task.FromResult(0);
        }

        public static Task OnAuthorizeEndpoint(OAuthAuthorizeEndpointContext context)
        {
            var ci = new System.Security.Claims.ClaimsIdentity(OAuthBearerOptions.AuthenticationType);
            context.OwinContext.Authentication.SignIn(ci);  // this will trigger AuthorizationCodeProvider.Create()

            context.RequestCompleted();

            return Task.FromResult(0);
        }        
    }
}