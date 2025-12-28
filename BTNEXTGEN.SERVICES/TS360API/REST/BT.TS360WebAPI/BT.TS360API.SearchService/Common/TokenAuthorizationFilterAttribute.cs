using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using System.Net.Http;
using BT.TS360API.ServiceContracts;
using Newtonsoft.Json;
using BT.TS360API.Common.Helpers;
using BT.TS360API.SearchService.Common.Configuration;

namespace BT.TS360API.SearchService.Common
{
    public class TokenAuthorizationFilterAttribute : AuthorizationFilterAttribute 
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                var content = actionContext.Request.Content;
                string jsonContent = content.ReadAsStringAsync().Result;

                var rq = JsonConvert.DeserializeObject<BaseRequest>(jsonContent);

                if (rq == null || string.IsNullOrEmpty(rq.PassPhrase)) return;

                var pp = CommonHelper.DecompressString(rq.PassPhrase);

                var decom_pp = pp.Split('$');

                if (decom_pp[0] == AppSetting.APIPassPhrase && decom_pp[1] == DateTime.Now.ToShortDateString())
                { }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.WriteLog(ex, "WebAPI PassPhrase Exception");
            }
        }
        private void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
    }
}