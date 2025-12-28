using BT.TS360API.ExternalDataSendService.Constants;
using BT.TS360API.ExternalDataSendService.Interfaces;
using BT.TS360API.ExternalDataSendService.Logging;
using BT.TS360API.ExternalDataSendService.Models;
using BT.TS360API.ExternalDataSendService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BT.TS360API.ExternalDataSendService.Controllers
{
    public class OrganizationController : ApiController
    {
        private static readonly IOrganizationSendService _profileSendService = new OrganizationSendService();

        [HttpPost]
        [Route("sendOrganizationInfo")]
        public async Task<AppServiceResult<bool>> SendOrganizationInfoAsync(string orgId)
        {
            var result = new AppServiceResult<bool>();

            try
            {
                result.Data = await _profileSendService.SendOrganizationAsync(orgId);
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.WriteLog(ex, "SendOrganizationInfoAsync");
            }

            return result;
        }
    }
}
