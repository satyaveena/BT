using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BT.TS360API.Authentication.Startup))]

namespace BT.TS360API.Authentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            ConfigureAuthServer(app);
            //ConfigureAuthClient(app);
        }
    }
}
