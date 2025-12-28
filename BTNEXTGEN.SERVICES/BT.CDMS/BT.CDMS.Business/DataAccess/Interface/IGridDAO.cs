using System.Collections.Generic;
using System.Data;
using BT.CDMS.Business.Models;

namespace BT.CDMS.Business.DataAccess.Interface
{
    /// <summary>
    /// Interface 
    /// </summary>
    public interface IGridDAO
    {
        DataSet GetGridTemplatesByOrgId(string orgId);
        DataSet CheckIfTemplateIsAccessibleToListOfUsers(CheckGridTemplateAccessRequest request);
    }
}
