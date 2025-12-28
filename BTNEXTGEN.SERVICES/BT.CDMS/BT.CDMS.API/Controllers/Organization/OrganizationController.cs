using BT.CDMS.Business.Helper;
using BT.CDMS.Business.Models;
using BT.CDMS.API.Request;
using BT.CDMS.API.Services.Organization;
using System.Web.Http;
using System.Collections.Generic;

namespace BT.CDMS.API.Controllers.Organization
{
    /// <summary>
    /// Class OrganizationController
    /// </summary>
    [Authorize]
    public class OrganizationController : ApiController
    {
        #region Private Member
        private readonly OrganizationServices _organizationServices;
        #endregion

        #region Constructor
        public OrganizationController()
        {
            this._organizationServices = new OrganizationServices();
        }
        #endregion

        #region Public Method

        [HttpGet]
        [Route("Organization/Search")]
        public AppServiceResult<List<OrganizationInfo>> SearchByOrgName([FromUri]string orgName)
        {
            return _organizationServices.SearchByOrgName(orgName);
        }

        [HttpGet]
        [Route("Organization/Users/{orgId}")]
        public AppServiceResult<List<UserInfo>> GetLoginIDsByOrgId(string orgId)
        {
            return _organizationServices.GetLoginIDsByOrgId(orgId);
        }

        [HttpGet]
        [Route("Organization/User/{loginId}")]
        public AppServiceResult<bool> SearchByLoginId(string loginId)
        {
            return _organizationServices.SearchByLoginId(loginId);
        }

        #endregion
    }
}
