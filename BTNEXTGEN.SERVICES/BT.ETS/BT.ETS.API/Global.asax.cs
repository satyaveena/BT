using BT.ETS.Business.DAO;
using BT.ETS.Business.DAO.Interface;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Manager;
using BT.ETS.Business.Manager.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BT.ETS.API
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
            ////Business Layer
             //UnityHelper.Container.RegisterType(typeof(IOrganizationManager), typeof(OrganizationManager));

            ////Data Access Layer 
            // UnityHelper.Container.RegisterType(typeof(IOrganizationDAO), typeof(OrganizationDAO));
 
            #endregion 
        }
    }
}
