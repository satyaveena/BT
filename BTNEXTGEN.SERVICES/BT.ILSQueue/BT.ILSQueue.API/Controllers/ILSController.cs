using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using BT.ILSQueue.API.Models;
using BT.ILSQueue.API.Providers;
using BT.ILSQueue.API.Results;
using System.Net;

using BT.ILSQueue.API.DAO;
using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.Business.MongDBLogger.ELMAHLogger;

namespace BT.ILSQueue.API.Controllers
{

    public class ILSController : ApiController
    {
        [HttpPost]
        [Route("SetJobStatusReady")]
        public async Task<HttpResponseMessage> SetJobStatusReady([FromUri]string PAPIJobID)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                if (string.IsNullOrEmpty(PAPIJobID))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    var isUpdated = await ILSAPILogDAO.Instance.UpdateILSJobStatus(PAPIJobID);
                    if(!isUpdated)
                        response.StatusCode = HttpStatusCode.BadRequest;
                }
                
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_API;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));
                //Logger.WriteLog(ex, "SetJobStatusReady");
            }

            response.RequestMessage = Request;
            return await Task.FromResult(response);
        }
    }
}
