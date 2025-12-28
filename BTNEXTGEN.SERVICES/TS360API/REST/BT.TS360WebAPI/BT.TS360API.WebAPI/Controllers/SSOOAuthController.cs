using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.DataAccess;
using BT.TS360API.WebAPI.Models;
using BT.TS360API.WebAPI.Services;

namespace BT.TS360API.WebAPI.Controllers
{
    public class SSOOAuthController : ApiController
    {
        private SSOOAuthRepository ssoOAuthRepository;

        public SSOOAuthController()
        {
            this.ssoOAuthRepository = new SSOOAuthRepository();
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        /*public void Post([FromBody]string value)
        {
        }*/

        public HttpResponseMessage Post(SSOOAuthRequest oauthRequest)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.Accepted;
            string apiKey;
            string ssoAuthError = "";

            SecurityRepository sp = new SecurityRepository();
            bool isValidVendor = sp.ValidateAPIKeyDB(Request, out apiKey);
            SSOOAuthResponse ssoOAuthResponse;

            if (!isValidVendor)
            {
                ssoAuthError = "Invalid API Key";
                httpStatusCode = HttpStatusCode.Unauthorized;

                ssoOAuthResponse = new SSOOAuthResponse { User = new SSOOAuthUser { UserToken = oauthRequest.UserToken }, ErrorMessage = "Unauthorized" };
            }
            else
            {
                string ts360LoginPage = AppSetting.SSOOAUTHTS360LoginPage;
                string salesForcePage = AppSetting.SSOOAUTHSalesForcePage;
                int SSOOAUTHExpirationInDays = Convert.ToInt32(AppSetting.SSOOAUTHExpirationInDays);

                ssoOAuthResponse = this.ssoOAuthRepository.Authorize(oauthRequest.UserToken, ts360LoginPage, salesForcePage, SSOOAUTHExpirationInDays);
                ssoAuthError = string.IsNullOrEmpty(ssoOAuthResponse.ErrorMessage) ? "" : ssoOAuthResponse.ErrorMessage;

                httpStatusCode = (ssoAuthError == "") ? HttpStatusCode.Created : HttpStatusCode.InternalServerError;
            }

            var response = Request.CreateResponse<SSOOAuthResponse>(httpStatusCode, ssoOAuthResponse);
            string strOAUTHRequest = oauthRequest.ToString();

            string webMethod = response.RequestMessage.RequestUri.LocalPath;
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string responseCode = response.StatusCode.ToString();
            string responseReason = response.ReasonPhrase;
            string responseMessage = string.Format("Version:{0} | ResponseCode:{1} | ResponseReason:{2}", version, responseCode, responseReason);
            string requestMessage = strOAUTHRequest;

            bool hasLoggedToFile = false;
            FileLogRepository fileLog = new FileLogRepository(AppSetting.SSOOAUTHLogFolder, AppSetting.SSOOAUTHLogFilePrefix);
            string logFileMessage = string.Empty;
            bool enableTrace = (AppSetting.SSOOAUTHEnableTrace.ToUpper() == "ON");

            try
            {
                ExceptionLoggingDAO logger = new ExceptionLoggingDAO();
                logger.LogRequest(webMethod, requestMessage, responseMessage, apiKey, ssoAuthError);
            }
            catch (Exception ex)
            {
                // Log rank to file if WEBAPI cannot connect to database
                logFileMessage = string.Format("[{0}] [{1}] [{2}] [{3}] {4}", responseCode, apiKey, ssoAuthError, ex.Message, strOAUTHRequest);
                fileLog.Write(logFileMessage, FileLogRepository.Level.ERROR);
                hasLoggedToFile = true;
            }

            // Log SSO OAUTH to file if the file log flag is ON
            if (enableTrace && !hasLoggedToFile)
            {
                logFileMessage = string.Format("[{0}] {1}", responseCode, strOAUTHRequest);
                fileLog.Write(logFileMessage, FileLogRepository.Level.INFO);
            }

            return response;
        }

        // PUT api/<controller>/5
        /*public void Put(int id, [FromBody]string value)
        {
        }*/

        // DELETE api/<controller>/5
        /*public void Delete(int id)
        {
        }*/
    }
}