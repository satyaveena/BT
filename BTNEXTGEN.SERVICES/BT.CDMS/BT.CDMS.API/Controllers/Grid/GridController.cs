using BT.CDMS.Business.Helper;
using BT.CDMS.Business.Models;
using BT.CDMS.API.Request;
using BT.CDMS.API.Services.Grid;
using System.Web.Http;
using System.Collections.Generic;

namespace BT.CDMS.API.Controllers.Grid
{
    /// <summary>
    /// Class GridController
    /// </summary>
    [Authorize]
    public class GridController : ApiController
    {
        #region Private Member
        private readonly GridServices _gridServices;
        #endregion

        #region Constructor
        public GridController()
        {
            this._gridServices = new GridServices();
        }
        #endregion

        #region Public Method

        [HttpGet]
        [Route("Organization/GridTemplates/{orgId}")]
        public AppServiceResult<List<GridTemplate>> GetGridTemplatesByOrgId(string orgId)
        {
            return _gridServices.GetGridTemplatesByOrgId(orgId);
        }

        [HttpPost]
        [Route("Organization/GridTemplateAccess")]
        public AppServiceResult<List<CheckGridTemplateAccessResponse>> CheckIfTemplateIsAccessibleToListOfUsers(CheckGridTemplateAccessRequest request)
        {
            return _gridServices.CheckIfTemplateIsAccessibleToListOfUsers(request);
        }
        #endregion
    }
}
