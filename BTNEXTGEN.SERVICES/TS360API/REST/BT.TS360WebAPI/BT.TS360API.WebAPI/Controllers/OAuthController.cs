using BT.TS360API.WebAPI.Common.Helper;
using BT.TS360API.WebAPI.OAuth;
using DotNetOpenAuth.OAuth2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BT.TS360API.WebAPI.Controllers
{
    public class OAuthController : Controller
    {
        private readonly AuthorizationServer authorizationServer = 
            new AuthorizationServer(new OAuth2AuthorizationServer());
        
        /// <summary>
        /// The OAuth 2.0 token endpoint.
        /// </summary>
        /// <returns>The response to the Client.</returns>
        public ActionResult Token()
        {
            LogRequest();
            var response = this.authorizationServer.HandleTokenRequest(this.Request);
            var jsresult = new JsonResult();
            //jsresult.ContentType = "application/json";
            var vr = new ViewResult();
            if (response.Status == HttpStatusCode.OK)
            {
                var accessTokenJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Body);
                accessTokenJson.Add("state", Guid.NewGuid().ToString());
                var idTokenValue = AccessTokenManager.GenerateIdToken(CurrentRequestUrl, ClientId);

                accessTokenJson.Add("id_token", idTokenValue);
                var jsonAccessToken = JsonConvert.SerializeObject(accessTokenJson);
                jsresult.Data = jsonAccessToken;
                LogResponse(jsonAccessToken);

                Response.ContentType = "application/json";
                Response.Headers["Cache-Control"] = "no-store";
                Response.Headers["Pragma"] = "no-cache";
                Response.Write(jsonAccessToken);
            }
            return new EmptyResult();
        }

        private void LogRequest()
        {
            var request = this.Request;
            var headers = new StringBuilder();
            var headerKeys = request.Headers.AllKeys;
            foreach (var key in headerKeys)
            {
                headers.Append(key + ": " + request.Headers[key] + "; ");
            }

            var param = new StringBuilder();
            var paramKeys = request.Params.AllKeys;
            foreach (var key in paramKeys)
            {
                param.Append(key + ": " + request.Params[key] + "; ");
            }

            string message = string.Format("SSO TOKEN PENDING REQUEST- HttpRequest: {0} - Headers: {1}, Params: {2}",
                request.RawUrl, headers, param);

            Logger.Info(message);
        }

        private void LogResponse(string body)
        {
            var headers = new StringBuilder();
            var headerKeys = Response.Headers.AllKeys;
            foreach (var key in headerKeys)
            {
                headers.Append(key + ": " + Response.Headers[key] + "; ");
            }

            string message = string.Format("SSO TOKEN RESPONSE- HttpResponse: Header:{0}; Body: {1}", headers, body);
            Logger.Info(message);
        }
        private string CurrentRequestUrl
        {
            get
            {
                if (this.Request.Url != null) return this.Request.Url.ToString();
                return "";

            }
        }

        private string ClientId
        {
            get
            {
                
                return "Salesforce";

            }
        }
    }
}