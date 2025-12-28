using System.Collections.Generic;
using BT.CDMS.Business.Models;

namespace BT.CDMS.Business.Manager.Interface
{
    public interface IOrganizationManager
    {
        /// <summary>
        /// SearchByOrgName
        /// </summary>
        /// <param name="orgName"></param>
        /// <returns></returns>
        List<OrganizationInfo> SearchByOrgName(string orgName);

        /// <summary>
        /// GetLoginIDsByOrgID
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        List<UserInfo> GetLoginIDsByOrgId(string orgId);

        /// <summary>
        /// SearchByLoginId
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        bool SearchByLoginId(string loginId);
    }
}