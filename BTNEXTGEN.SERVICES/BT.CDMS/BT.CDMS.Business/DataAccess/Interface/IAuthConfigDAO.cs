using BT.CDMS.Business.Models;
using System.Collections.Generic;

namespace BT.CDMS.Business.DataAccess.Interface
{
    /// <summary>
    /// Interface 
    /// </summary>
    public interface IAuthConfigDAO
    {
        List<ApplicationKeys> GetAuthConfigs();
    }
}
 