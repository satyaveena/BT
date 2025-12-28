using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.DataAccess;
using BT.TS360API.WebAPI.Models;
using BT.TS360API.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http;

namespace BT.TS360API.WebAPI.Controllers
{
    [Authorize]
    public class SSOUserProfileController : ApiController
    {
        private SSOOAuthRepository ssoOAuthRepository;

        public SSOUserProfileController()
        {
            this.ssoOAuthRepository = new SSOOAuthRepository();
        }
        public HttpResponseMessage Get()
        {
            try
            {
                var userName = User.Identity.Name;
                LogRequest(Request, userName);
                var userProfile = this.ssoOAuthRepository.GetUserInfo(userName);   
                HttpResponseMessage response = Request.CreateResponse<SSOUserInfoResponse>(HttpStatusCode.OK, userProfile);
                Log(response);
                return response;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return Request.CreateResponse<SSOUserInfoResponse>(HttpStatusCode.OK, null);
        }

        private void LogException(Exception ex)
        {
            FileLogRepository fileLog = new FileLogRepository(AppSetting.SSOOAUTHLogFolder, AppSetting.SSOOAUTHLogFilePrefix);
            fileLog.Write(ex.ToString(), FileLogRepository.Level.ERROR);
        }

        private void Log(HttpResponseMessage response)
        {
            string responseCode = response.StatusCode.ToString();
            string responseBody = response.Content.ReadAsStringAsync().Result;

            FileLogRepository fileLog = new FileLogRepository(AppSetting.SSOOAUTHLogFolder, AppSetting.SSOOAUTHLogFilePrefix);
            string logFileMessage = string.Empty;
            bool enableTrace = (AppSetting.SSOOAUTHEnableTrace.ToUpper() == "ON");           

            // Log SSO OAUTH to file if the file log flag is ON
            if (enableTrace)
            {
                logFileMessage = string.Format("[{0}] {1}", responseCode, responseBody);
                fileLog.Write(logFileMessage, FileLogRepository.Level.INFO);
            }
        }

        private void LogRequest(HttpRequestMessage request, string userName)
        {
            FileLogRepository fileLog = new FileLogRepository(AppSetting.SSOOAUTHLogFolder, AppSetting.SSOOAUTHLogFilePrefix);
            string logFileMessage = string.Empty;
            bool enableTrace = (AppSetting.SSOOAUTHEnableTrace.ToUpper() == "ON");

            string authHeader = request.Headers.GetValues("Authorization").FirstOrDefault();
            // Log SSO OAUTH to file if the file log flag is ON
            if (enableTrace)
            {
                logFileMessage = string.Format("User Info Request: Headers: {0}, Logged-in User:{1}", authHeader, userName);
                fileLog.Write(logFileMessage, FileLogRepository.Level.INFO);
            }
        }

        private IPrincipal User
        {
            get { return HttpContext.Current.User; }
        }

        private string AccessToken
        {
            get
            {
                return HttpContext.Current.Session["AccessToken"] as string;
            }
        }
    }
}
