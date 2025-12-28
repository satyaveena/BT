using BT.Auth.Business.DataAccess;
using BT.Auth.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BT.Auth.Business.Manager.Interface
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
