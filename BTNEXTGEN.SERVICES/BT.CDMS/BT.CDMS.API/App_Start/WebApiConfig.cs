using BT.CDMS.API.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

namespace BT.CDMS.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            config.Filters.Add(new APIExceptionFilter());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());


            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
