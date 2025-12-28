using BT.ETS.Business.Constants;
using BT.ETS.Business.DAO;
using BT.ETS.Business.DAO.Interface;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Manager.Interface;
using BT.ETS.Business.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Manager
{
    /// <summary>
    /// Class OrganizationManager
    /// </summary>
    public class OrganizationManager
    {
       
 
        #region Private Member
        //private IOrganizationDAO _orgDAO;
        //private IOrganizationDAO OrganizationDAO
        //{
        //    get
        //    {
        //        if (_orgDAO == null)
        //            _orgDAO = UnityHelper.Container.Resolve<IOrganizationDAO>();
        //        return _orgDAO;
        //    }
        //}

        private static volatile OrganizationManager _instance;
        private static readonly object SyncRoot = new Object();
        public static OrganizationManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new OrganizationManager();
                }

                return _instance;
            }
        }
        
        #endregion

        #region Public Method
        /// <summary>
        /// SearchByOrgName
        /// </summary>
        /// <param name="orgName"></param>
        /// <returns></returns>
        public async Task<List<OrganizationInfo>> GetEspOrgsByDate(DateTime? since)
        {
            var result = new List<OrganizationInfo>();
            var orgInfo = await OrganizationDAO.Instance.GetEspOrgsByDate(since);
            if (orgInfo.Tables.Count > 0)
            {
                if (orgInfo.Tables[0] != null && orgInfo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in orgInfo.Tables[0].Rows)
                    {
                        result.Add(new OrganizationInfo
                        {
                            OrganizationId = CommonHelper.SqlDataConvertTo<string>(dr, ApplicationConstants.COLUMN_ORG_ID),
                            OrganizationName = CommonHelper.SqlDataConvertTo<string>(dr, ApplicationConstants.COLUMN_ORG_NAME),
                            IsDistributionEnabled = CommonHelper.SqlDataConvertTo<Boolean>(dr, ApplicationConstants.COLUMN_ORG_ESP_DISTRIBUTION_FLAG)? "Y" : "N",
                            IsRankEnabled = CommonHelper.SqlDataConvertTo<Boolean>(dr, ApplicationConstants.COLUMN_ORG_ESP_RANKING_FLAG)? "Y" : "N",
                            LastUpdatedDate = CommonHelper.SqlDataConvertTo<DateTime?>(dr, ApplicationConstants.COLUMN_ORG_ESP_DATE),
                            ESPLibraryId = CommonHelper.SqlDataConvertTo<string>(dr, ApplicationConstants.COLUMN_ORG_ESP_LIBRARY_ID),
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
        public async Task<List<UserInfo>> GetLoginIDsByOrgId(string orgId)
        {
            var userInfoList = new List<UserInfo>();
            var userInfo = await OrganizationDAO.Instance.GetLoginIDsByOrgId(orgId);
            if (userInfo.Tables.Count > 0)
            {
                if (userInfo.Tables[0] != null && userInfo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in userInfo.Tables[0].Rows)
                    {
                        var userDetail = new UserInfo()
                        {
                            UserId = CommonHelper.SqlDataConvertTo<string>(dr, ApplicationConstants.COLUMN_USER_ID),
                            UserName = CommonHelper.SqlDataConvertTo<string>(dr, ApplicationConstants.COLUMN_USER_NAME),
                            UserAlias = CommonHelper.SqlDataConvertTo<string>(dr, ApplicationConstants.COLUMN_USER_ALIAS)
                        };
                        userInfoList.Add(userDetail);
                    }
                }
            }
            return userInfoList;
        }

        #endregion
    }
}
