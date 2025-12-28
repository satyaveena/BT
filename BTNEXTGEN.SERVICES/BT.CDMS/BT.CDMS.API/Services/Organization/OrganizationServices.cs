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

namespace BT.CDMS.API.Services.Organization
{
    /// <summary>
    /// Class OrganizationServices
    /// </summary>
    public class OrganizationServices
    {
        #region Private Member
        private IOrganizationManager _orgMgr;
        #region Property
        private IOrganizationManager OrgMgr
        {
            get
            {
                if (_orgMgr == null)
                    _orgMgr = UnityHelper.Container.Resolve<IOrganizationManager>();
                return _orgMgr;
            }
        }
        #endregion
        #endregion

        #region Constructor
        public OrganizationServices()
        {

        }
        #endregion

        #region Public Method

        internal AppServiceResult<List<OrganizationInfo>> SearchByOrgName(string orgName)
        {
            var result = new AppServiceResult<List<OrganizationInfo>>();

            result.Data = OrgMgr.SearchByOrgName(orgName);
            result.Status = AppServiceStatus.Success;
           
            return result;
        }

        internal AppServiceResult<List<UserInfo>> GetLoginIDsByOrgId(string orgId)
        {
            var result = new AppServiceResult<List<UserInfo>>();

            result.Data = OrgMgr.GetLoginIDsByOrgId(orgId);
            result.Status = AppServiceStatus.Success;
            
            return result;
        }

        internal AppServiceResult<bool> SearchByLoginId(string loginId)
        {
            var result = new AppServiceResult<bool>();
            
            result.Data = OrgMgr.SearchByLoginId(loginId);
            result.Status = AppServiceStatus.Success;
           
            return result;
        }
        #endregion
    }
}