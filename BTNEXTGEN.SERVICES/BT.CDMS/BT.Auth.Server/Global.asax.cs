using BT.Auth.Business.DataAccess;
using BT.Auth.Business.DataAccess.Interface;
using BT.Auth.Business.Helpers;
using BT.Auth.Business.Logger.ELMAHLogger;
using BT.Auth.Business.Manager;
using BT.Auth.Business.Manager.Interface;
using Elmah;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace BT.Auth.Server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region Unity Mapping 
            //Business Layer
            UnityHelper.Container.RegisterType(typeof(IAuthConfigManager), typeof(AuthConfigManager));
            UnityHelper.Container.RegisterType(typeof(ErrorLog), typeof( ELMAHMongoLogger));
            
            //Data Access Layer 
            UnityHelper.Container.RegisterType(typeof(IAuthConfigDAO), typeof(AuthConfigDAO));
            #endregion 
        }
    }
}