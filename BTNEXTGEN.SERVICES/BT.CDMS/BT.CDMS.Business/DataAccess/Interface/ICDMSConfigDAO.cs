using System.Collections.Generic;
using BT.CDMS.Business.Models;

namespace BT.CDMS.Business.DataAccess.Interface
{
    /// <summary>
    /// Interface 
    /// </summary>
    public interface ICDMSConfigDAO
    {
        List<CDMSConfig> GetCDMSConfigs();
    }
}
 