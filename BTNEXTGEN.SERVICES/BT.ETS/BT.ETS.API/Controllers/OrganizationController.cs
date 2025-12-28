using BT.ETS.API.Services;
using BT.ETS.Business.Constants;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BT.ETS.API.Controllers.Organization
{
    /// <summary>
    /// Organization Methods
    /// </summary>
    [Authorize]
    public class OrganizationController : ApiController
    {
        #region Private Member
        private readonly OrganizationServices _organizationServices;
        private readonly OrderServices _orderServices;
        #endregion

        #region Constructor
        /// <summary>
        /// Organization Constructor
        /// </summary>
        public OrganizationController()
        {
            this._organizationServices = new OrganizationServices();
            this._orderServices = new OrderServices();
        }
        #endregion

        #region Public Method

                /// <summary>
        /// Return the ESP enabled organizations in TS360.
        /// </summary>
        /// <param name="since">If provided, returned only those ESP premium organizations have been added since date provided in {since} </param>

        [HttpGet]
        [Route("organization/espOrgInfo/")]
        public async Task<EtsServiceResult<List<OrganizationInfo>>> GetEspOrgsByDate()
        {
            return await GetEspOrgsByDate("");
        }

        /// <summary>
        /// Return the ESP enabled organizations in TS360.
        /// </summary>
        /// <param name="since">If provided, returned only those ESP premium organizations have been added since date provided in {since} </param>
        
        [HttpGet]
        [Route("organization/espOrgInfo/")]
        public async Task<EtsServiceResult<List<OrganizationInfo>>> GetEspOrgsByDate(string since)
        {
            var result = new EtsServiceResult<List<OrganizationInfo>>();
            try
            {
                result.Data = await _organizationServices.GetEspOrgsByDate(since);
                return result;
            }
            catch (BusinessException be)
            {
                result.StatusCode = be.ErrorCode;
                result.StatusMessage = be.Message;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Organization);
                result.StatusCode = "500";
                result.StatusMessage = BusinessExceptionConstants.Message(result.StatusCode);
                return result;
            }
        }

        /// <summary>
        /// Return the login information for corresponding organization.
        /// </summary>
        /// <param name="organizationId">TS360 Organization ID {GUID} </param>
        [HttpGet]
        [Route("organization/loginInfo/")]
        public async Task<EtsServiceResult<List<UserInfo>>> GetLoginIDsByOrgId(string organizationId)
        {
            var result = new EtsServiceResult<List<UserInfo>>();
            try
            {
                result.Data = await _organizationServices.GetLoginIDsByOrgId(organizationId);
                return result;
            }
            catch (BusinessException be)
            {
                result.StatusCode = be.ErrorCode;
                result.StatusMessage = be.Message;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Organization);
                result.StatusCode = "500";
                result.StatusMessage = BusinessExceptionConstants.Message(result.StatusCode);
                return result;
            }

            
        }

        /// <summary>
        /// Return the Grid Templates for the Organization that setup in TS360.
        /// </summary>
        /// <param name="userId">Return the Grid Templates for the Organization that setup in TS360 </param>
        [HttpGet]
        [Route("organization/gridTemplates/")]
        public async Task<EtsServiceResult<GridTemplateResult>> GetGridTemplates(string userId)
        {
            var result = new EtsServiceResult<GridTemplateResult>();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new BusinessException(105);

                result.Data = await _orderServices.GetGridTemplatesByUserId(userId);
                return result;
            }
            catch (BusinessException be)
            {
                result.StatusCode = be.ErrorCode;
                result.StatusMessage = be.Message;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Organization);
                result.StatusCode = "500";
                result.StatusMessage = BusinessExceptionConstants.Message(result.StatusCode);
                return result;
            }


        }

        #endregion
    }
}
