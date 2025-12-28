using BT.CDMS.Business.Models;
using System.Collections.Generic;

namespace BT.CDMS.Business.Manager.Interface
{
    /// <summary>
    /// Interface AuthConfigManager
    /// </summary>
    public interface IAuthConfigManager
    {
        /// <summary>
        /// GetAuthConfig()
        /// </summary>
        /// <returns></returns>
        List<ApplicationKeys> GetAuthConfig();
    }
}
