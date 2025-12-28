using BT.TS360API.Cache;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.Services.Services;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BT.TS360API.Services.Controllers
{
    [Authorize]
    public class OrganizationController : ApiController
    {
        private static readonly OrganizationService _orgService = new OrganizationService();

        [HttpGet]
        [Route("organization/PPCSubscriptions/{orgId?}/{pageIndex?}/{pageSize?}")]
        public async Task<AppServiceResult<List<PPCSubscription>>> GetPPCSubscriptions(string orgId = "", int pageIndex = 0, int pageSize = 0)
        {
            var result = new AppServiceResult<List<PPCSubscription>>();
            try
            {
                // get and cache
                var ppcSubsriptions = await _orgService.GetPPCSubscriptions();

                if (!string.IsNullOrEmpty(orgId))
                {
                    if (ppcSubsriptions != null)
                    {
                        // return by page
                        result.Data = (from ps in ppcSubsriptions
                                       select ps).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {
                        result.Data = new List<PPCSubscription>();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetPPCSubscriptions");
            }

            return null;
        }

        [HttpGet]
        [Route("organization/OrganizationPPCSubscriptions/{orgId}/{pageIndex}/{pageSize}")]
        public async Task<AppServiceResult<List<string>>> GetOrganizationPPCSubscriptions(string orgId, int pageIndex, int pageSize)
        {
            var result = new AppServiceResult<List<string>>();

            try
            {
                if (string.IsNullOrEmpty(orgId))
                    throw new ArgumentNullException("orgId");

                // get and cache
                var selectedPPSubscriptions = await _orgService.GetOrganizationPPCSubscriptions(orgId);
                if (selectedPPSubscriptions != null)
                {
                    // return by page
                    result.Data = (from ps in selectedPPSubscriptions
                                   select ps).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetOrganizationPPCSubscriptions");
            }

            return result;
        }
    }
}
