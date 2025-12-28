using System.Collections.Generic;
using BT.Auth.Business.Models;

namespace BT.Auth.Business.DataAccess.Interface
{
    /// <summary>
    /// Interface 
    /// </summary>
    public interface IAuthConfigDAO
    {
        List<ApplicationKeys> GetAuthConfigs();
    }
}
 