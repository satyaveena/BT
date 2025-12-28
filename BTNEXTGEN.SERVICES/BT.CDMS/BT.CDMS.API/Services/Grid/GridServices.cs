using BT.CDMS.Business.Constants;
using BT.CDMS.Business.Helper;
using BT.CDMS.Business.Models;
using System;
using BT.CDMS.Business.Logger.ELMAHLogger;
using System.Configuration;
using BT.CDMS.Business.Helpers;
using BT.CDMS.Business.Manager.Interface;
using Unity;
using Elmah;
using System.Collections.Generic;

namespace BT.CDMS.API.Services.Grid
{
    /// <summary>
    /// Class GridServices
    /// </summary>
    public class GridServices
    {
        #region Private Member
        private IGridManager _gridMgr;
        #region Property
        private IGridManager GridMgr
        {
            get
            {
                if (_gridMgr == null)
                    _gridMgr = UnityHelper.Container.Resolve<IGridManager>();
                return _gridMgr;
            }
        }
        #endregion
        #endregion

        #region Constructor
        public GridServices()
        {

        }
        #endregion

        #region Public Method
        internal AppServiceResult<List<GridTemplate>> GetGridTemplatesByOrgId(string orgId)
        {
            var result = new AppServiceResult<List<GridTemplate>>();
            
            result.Data = GridMgr.GetGridTemplatesByOrgId(orgId);
            result.Status = AppServiceStatus.Success;
            
            return result;
        }

        internal AppServiceResult<List<CheckGridTemplateAccessResponse>> CheckIfTemplateIsAccessibleToListOfUsers(CheckGridTemplateAccessRequest request)
        {
            var result = new AppServiceResult<List<CheckGridTemplateAccessResponse>>();
            
            result.Data = GridMgr.CheckIfTemplateIsAccessibleToListOfUsers(request);
            result.Status = AppServiceStatus.Success;
            
            return result;
        }
        #endregion
    }
}