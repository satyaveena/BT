using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
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
    public partial class CustomerServiceController : ApiController
    {
        private static readonly Services.CustomerService _customerServiceService = new Services.CustomerService();

        public CustomerServiceController()
        {
            //this._customerServiceService = new Services.CustomerService();
        }

        /// <summary>
        /// Gets Org AccountTypes.
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("customerSupport/accountTypes/{UserID}")]
        public async Task<AppServiceResult<OrgAccountTypesResponse>> GetUserAccountTypes(string userID)
        {
            var result = new AppServiceResult<OrgAccountTypesResponse>();
            result.Status = AppServiceStatus.Success;

            try
            {
                var resultData = new OrgAccountTypesResponse();
                resultData.AccountTypes = await _customerServiceService.GetUserAccountTypes(userID);

                // checks if user has only one dashboard
                resultData.HasOnlyDashboard = _customerServiceService.HasOnlyDashboard(userID);
              
                result.Data = resultData;
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetOrgAccountTypes");
            }

            return result;
        }


        [HttpGet]
        [Route("customerSupport/accounts/{UserId}/{AccountType}/{pageIndex}/{pageSize}")]
        public async Task<AppServiceResult<List<ServiceContracts.CartAccount>>> GetAccountsByAccountType(string userId, string accountType, int pageIndex, int pageSize)
        {
            var result = new AppServiceResult<List<CartAccount>>();
            result.Status = AppServiceStatus.Success;

            try
            {
                result.Data = await _customerServiceService.GetAccountsByAccountType(userId, accountType, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetAccountsByAccountType");
            }

            return result;
        }

        [HttpGet]
        [Route("customerSupport/accounts/{DashboardId}")]
        public async Task<AppServiceResult<List<ServiceContracts.CartAccount>>> GetAccountsByDashboardId(int dashboardId)
        {
            var result = new AppServiceResult<List<CartAccount>>();
            result.Status = AppServiceStatus.Success;

            try
            {
                result.Data = await _customerServiceService.GetAccountsByDashboardId(dashboardId);
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetAccountsByDashboardId");
            }

            return result;
        }
    }
}
