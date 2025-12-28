using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.DAO.Interface
{
    /// <summary>
    /// Interface 
    /// </summary>
    public interface IOrganizationDAO
    {
        DataSet GetEspOrgsByDate(DateTime? since);
        DataSet GetLoginIDsByOrgId(string orgId);
        
    }
}
