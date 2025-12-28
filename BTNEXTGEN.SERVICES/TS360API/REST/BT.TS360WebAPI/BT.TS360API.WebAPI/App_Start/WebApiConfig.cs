using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using System.Net.Http;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BT.TS360API.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "cart/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }/*,
                constraints: new { url = new LowercaseRouteConstraint() }*/
            );

            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            /*config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "cart/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/

            /*config.Routes.MapHttpRoute(
            name: "Default",
            routeTemplate: "{controller}/{action}/{id}",
            defaults: new { controller = "Ranked", action = "GetRankItem", id = RouteParameter.Optional }
            );*/

            /*config.Routes.MapHttpRoute("DefaultApiWithId", "Api/{controller}/{id}", new { id = RouteParameter.Optional }, new { id = @"\d+" });
            config.Routes.MapHttpRoute("DefaultApiWithAction", "Api/{controller}/{action}");
            */
            /*config.Routes.MapHttpRoute("DefaultApiGet", "Api/{controller}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute("DefaultApiPost", "Api/{controller}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
             */
        }
    }

    /*public class LowercaseRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var path = httpContext.Request.Url.AbsolutePath;
            return path.Equals(path.ToLowerInvariant(), StringComparison.InvariantCulture);
        }
    }*/
}
