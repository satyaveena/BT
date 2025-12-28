using System.Collections.Generic;
using System.Data;

namespace BT.CDMS.Business.DataAccess.Interface
{
    /// <summary>
    /// Interface 
    /// </summary>
    public interface IOrganizationDAO
    {
        DataSet SearchByOrgName(string orgName);
        DataSet GetLoginIDsByOrgId(string orgId);
        string SearchByLoginId(string loginId);
    }
}
