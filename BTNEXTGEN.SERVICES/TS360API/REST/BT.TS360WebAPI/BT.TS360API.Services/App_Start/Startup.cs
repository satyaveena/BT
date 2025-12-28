using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;

using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Infrastructure;

[assembly: OwinStartup(typeof(BT.TS360API.Authentication.Startup))]

namespace BT.TS360API.Authentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions { });
        }
    }
}
