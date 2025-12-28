using Owin;
using System;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using System.Collections.Generic;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(BT.ETS.API.Startup))]
namespace BT.ETS.API
{
    public partial class Startup
    {
        public void ConfigureAuthClient(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions { });
        }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuthClient(app);
        }
    }
}