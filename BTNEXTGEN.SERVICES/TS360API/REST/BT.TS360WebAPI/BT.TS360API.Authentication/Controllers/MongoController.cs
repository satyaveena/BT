using BT.TS360API.Authentication;
using BT.TS360API.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Logging;

namespace BT.TS360API.Services.Controllers
{
    public class MongoController : ApiController
    {
        private readonly Services.MongoService _mongoActionService = new Services.MongoService();

        [Route("api/Mongo")]
        [HttpGet]
        public HttpResponseMessage GetPremiumServiceCode(string url)
        {
            var response = new HttpResponseMessage();

            if (string.IsNullOrEmpty(url))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("Url parameter is required.");
            }
            else
            {
                try
                {
                    bool domainUrlFound;
                    var result = _mongoActionService.GetPremiumServiceCode(url, out domainUrlFound);
                    if (domainUrlFound == false)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Content = new StringContent("Domain URL not found.");
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Content = new ObjectContent<object>(new
                        {
                            PremiumServiceCode = result
                        }, Configuration.Formatters.JsonFormatter);
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Content = new StringContent(ex.Message);

                    Startup.Logger.WriteError("GetPremiumServiceCode Error", ex);
                }
            }
            return response;
        }

        [Route("api/GetAllSiteBrandings")]
        [HttpGet]
        public HttpResponseMessage GetAllSiteBrandings()
        {
            var response = new HttpResponseMessage();

            try
            {
                var result = _mongoActionService.GetAllSiteBrandings();
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new ObjectContent<List<RefererBranding>>(result, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.Message);

                Startup.Logger.WriteError("GetAllSiteBrandings Error", ex);
            }
            return response;
        }
    }
}
