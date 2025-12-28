using BT.TS360API.Authentication.DataAccess;
using BT.TS360API.Authentication.Helpers;
using BT.TS360API.Authentication.Models;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace BT.TS360API.Authentication.Controllers
{
    //[RoutePrefix("OAuth")]
    public class AuthenticationController : ApiController
    {
        [HttpPost]
        [Route("GetAuthCode")]
        public HttpResponseMessage GetAuthCode(GetAuthCodeRequest request)
        {
            var response = new HttpResponseMessage();

            // validate inputs
            if (request == null || string.IsNullOrWhiteSpace(request.UserId) || 
                string.IsNullOrWhiteSpace(request.ClientId) || string.IsNullOrWhiteSpace(request.ClientSecret))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("All input parameters are required.");
            }
            else
            {
                try
                {
                    // validate clientId and Secret
                    if (ValidateHelper.ValidateAuthClient(request.ClientId, request.ClientSecret))
                    {
                        // generate authentication code
                        var authTicket = TokenHelper.GenerateAuthTicket(request.UserId, 60);
                        var authCode = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(authTicket);

                        // save to MongoDB
                        AuthenticationLogDAO.Instance.AddAuthCodeInfo(request.UserId, request.ClientId, authCode, authTicket.Properties.ExpiresUtc.Value);

                        // response
                        response.StatusCode = HttpStatusCode.OK;
                        response.Content = new ObjectContent<object>(new
                                            {
                                                AuthenticationCode = authCode
                                            }, Configuration.Formatters.JsonFormatter);
                    }
                    
                }
                catch (Exception ex)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Content = new StringContent(ex.Message);
                }
            }

            return response;
        }
    }
}
