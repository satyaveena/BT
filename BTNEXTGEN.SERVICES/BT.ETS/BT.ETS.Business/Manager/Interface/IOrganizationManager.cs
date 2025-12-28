using BT.ETS.Business.Models;
using System;
using System.Collections.Generic;


namespace BT.ETS.Business.Manager.Interface
{
    public interface IOrganizationManager
    {
        /// <summary>
        /// GetEspOrgsByDate
        /// </summary>
        /// <param name="orgName"></param>
        /// <returns></returns>
        List<OrganizationInfo> GetEspOrgsByDate(DateTime? since);

        /// <summary>
        /// GetLoginIDsByOrgID
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        List<UserInfo> GetLoginIDsByOrgId(string orgId);
         
    }
}