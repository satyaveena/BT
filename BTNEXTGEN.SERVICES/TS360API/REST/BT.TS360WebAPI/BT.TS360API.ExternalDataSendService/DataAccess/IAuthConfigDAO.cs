using BT.TS360API.ExternalDataSendService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BT.TS360API.ExternalDataSendService.DataAccess
{
    /// <summary>
    /// IAuthConfigDAO interface.
    /// </summary>
    public interface IAuthConfigDAO
    {
        Task<List<ExternalApiInfoEx>> GetExternalApiInfoList();
    }
}
 