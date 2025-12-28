using System.Collections.Generic;
using BT.CDMS.Business.Models;

namespace BT.CDMS.Business.Manager.Interface
{
    public interface IGridManager
    {
        /// <summary>
        /// GetGridTemplatesByOrgId
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        List<GridTemplate> GetGridTemplatesByOrgId(string orgId);

        /// <summary>
        /// CheckIfTemplateIsAccessibleToListOfUsers
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<CheckGridTemplateAccessResponse> CheckIfTemplateIsAccessibleToListOfUsers(CheckGridTemplateAccessRequest request);
    }
}