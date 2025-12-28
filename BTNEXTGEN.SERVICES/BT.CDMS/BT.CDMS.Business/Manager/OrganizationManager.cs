using BT.CDMS.Business.DataAccess.Interface;
using BT.CDMS.Business.Helper;
using BT.CDMS.Business.Helpers;
using BT.CDMS.Business.Manager.Interface;
using BT.CDMS.Business.Models;
using BT.CDMS.Business.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using Unity;
using BT.CDMS.Business.Exceptions;

namespace BT.CDMS.Business.Manager
{
    /// <summary>
    /// Class OrganizationManager
    /// </summary>
    public class OrganizationManager : IOrganizationManager
    {
        #region Private Member
        private IOrganizationDAO _orgDAO;
        private IOrganizationDAO OrganizationDAO
        {
            get
            {
                if (_orgDAO == null)
                    _orgDAO = UnityHelper.Container.Resolve<IOrganizationDAO>();
                return _orgDAO;
            }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// SearchByOrgName
        /// </summary>
        /// <param name="orgName"></param>
        /// <returns></returns>
        public List<OrganizationInfo> SearchByOrgName(string orgName)
        {
            if(string.IsNullOrEmpty(orgName) || orgName.Length<2)
            {
                throw new BusinessException(BusinessExceptionConstants.INVALID_ORGNAME_MESSAGE, BusinessExceptionConstants.INVALID_ORGNAME_CODE);
            }
            var result = new List<OrganizationInfo>();
            var orgInfo = OrganizationDAO.SearchByOrgName(orgName);
            if (orgInfo.Tables.Count > 0)
            {
                if (orgInfo.Tables[0] != null && orgInfo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in orgInfo.Tables[0].Rows)
                    {
                        result.Add(new OrganizationInfo
                        {
                            OrganizationId = DataAccessHelper.ConvertToString(dr[ApplicationConstants.COLUMN_ORG_ID_ALIAS]),
                            OrganizationName = DataAccessHelper.ConvertToString(dr[ApplicationConstants.COLUMN_ORG_NAME_ALIAS]),
                            UserCount = DataAccessHelper.ConvertToInt(dr[ApplicationConstants.COLUMN_USER_COUNT])
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// GetLoginIDsByOrgID
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<UserInfo> GetLoginIDsByOrgId(string orgId)
        {
            var userInfoList = new List<UserInfo>();
            var userInfo = OrganizationDAO.GetLoginIDsByOrgId(orgId);
            if (userInfo.Tables.Count > 0)
            {
                if (userInfo.Tables[0] != null && userInfo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in userInfo.Tables[0].Rows)
                    {
                        var userDetail = new UserInfo()
                        {
                            UserId = DataAccessHelper.ConvertToString(dr[ApplicationConstants.COLUMN_USER_ID]),
                            UserName = DataAccessHelper.ConvertToString(dr[ApplicationConstants.COLUMN_USER_NAME]),
                            UserAlias = DataAccessHelper.ConvertToString(dr[ApplicationConstants.COLUMN_USER_ALIAS])
                        };
                        userInfoList.Add(userDetail);
                    }
                }
            }
            return userInfoList;
        }

        /// <summary>
        /// SearchByLoginId
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public bool SearchByLoginId(string loginId)
        {
            var result = false;
            var userId = OrganizationDAO.SearchByLoginId(loginId);
            if (!String.IsNullOrEmpty(userId))
            {
                result = true;
            }
            return result;
        }
        #endregion
    }
}
