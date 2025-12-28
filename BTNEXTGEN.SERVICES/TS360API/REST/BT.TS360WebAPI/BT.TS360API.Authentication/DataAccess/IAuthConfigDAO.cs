using BT.TS360API.Authentication.Models;
using System.Collections.Generic;

namespace BT.TS360API.Authentication.DataAccess
{
    /// <summary>
    /// IAuthConfigDAO interface.
    /// </summary>
    public interface IAuthConfigDAO
    {
        ApplicationAuthKey GetApplicationIdentity(string appId);
        List<ApplicationAuthKey> GetAuthConfigs();
        string GetMongoPremiumServiceCode(string url, out bool domainUrlFound);
    }
}
 