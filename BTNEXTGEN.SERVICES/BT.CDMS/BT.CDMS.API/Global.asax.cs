using BT.CDMS.Business.DataAccess;
using BT.CDMS.Business.DataAccess.Interface;
using BT.CDMS.Business.Helpers;
using BT.CDMS.Business.Logger.ELMAHLogger;
using BT.CDMS.Business.Manager;
using BT.CDMS.Business.Manager.Interface;
using Elmah;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace BT.CDMS.API
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
            UnityHelper.Container.RegisterType(typeof(ICDMSConfigManager), typeof(CDMSConfigManager));
            UnityHelper.Container.RegisterType(typeof(IOrganizationManager), typeof(OrganizationManager));
            UnityHelper.Container.RegisterType(typeof(IGridManager), typeof(GridManager));
            UnityHelper.Container.RegisterType(typeof(ErrorLog), typeof( ELMAHMongoLogger));

            //Data Access Layer 
            UnityHelper.Container.RegisterType(typeof(IAuthConfigDAO), typeof(AuthConfigDAO));
            UnityHelper.Container.RegisterType(typeof(ICDMSConfigDAO), typeof(CDMSConfigDAO));
            UnityHelper.Container.RegisterType(typeof(IOrganizationDAO), typeof(OrganizationDAO));
            UnityHelper.Container.RegisterType(typeof(IGridDAO), typeof(GridDAO));
            #endregion 
        }
    }
}