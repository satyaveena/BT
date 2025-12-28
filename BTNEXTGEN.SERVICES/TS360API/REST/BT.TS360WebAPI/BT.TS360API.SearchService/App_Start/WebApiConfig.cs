using BT.TS360API.SearchService.Common;
using System.Web.Http;
using System.Web.Http.Cors;


namespace BT.TS360API.SearchService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            log4net.Config.XmlConfigurator.Configure();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //// json supporter
            //var json = config.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling =
            //    Newtonsoft.Json.PreserveReferencesHandling.Objects;

            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            //config.Filters.Add(new TokenAuthorizationFilterAttribute());
        }
    }
}
