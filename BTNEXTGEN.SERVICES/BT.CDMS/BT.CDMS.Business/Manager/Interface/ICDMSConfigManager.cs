using BT.CDMS.Business.DataAccess;
using BT.CDMS.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BT.CDMS.Business.Manager.Interface
{
    /// <summary>
    /// Interface CDMSConfigManager
    /// </summary>
    public interface ICDMSConfigManager
    {
        /// <summary>
        /// GetCDMSConfig()
        /// </summary>
        /// <returns></returns>
        List<CDMSConfig> GetCDMSConfig();
    }
}
