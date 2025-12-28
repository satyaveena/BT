using BT.TS360API.Authentication.Models;
using BT.TS360API.Authentication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BT.TS360API.Authentication.Controllers
{
    public class ProfileController : ApiController
    {
        private static readonly ProfileService _profileService = new ProfileService();

        /// <summary>
        /// Gets TS360 user profile by access token.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("UserProfile")]
        public HttpResponseMessage GetUserProfileData()
        {
            var response = new HttpResponseMessage();

            try
            {
                var accessToken = Request.Headers.Authorization.Parameter;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var userProfile = _profileService.GetTS360UserProfile(accessToken);

                    // response
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new ObjectContent<object>(userProfile, Configuration.Formatters.JsonFormatter);
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.Message);
            }

            return response;
        }
    }
}
