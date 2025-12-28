using BT.CDMS.Business.DataAccess.Interface;
using BT.CDMS.Business.Helpers;
using BT.CDMS.Business.Manager.Interface;
using BT.CDMS.Business.Models;
using System.Collections.Generic;
using Unity;
namespace BT.CDMS.Business.Manager
{
    /// <summary>
    /// CDMSConfigManager Class
    /// </summary>
    public class CDMSConfigManager : ICDMSConfigManager
    {
        #region Private Member and Property
        private ICDMSConfigDAO _portalConfigDAO;
        private ICDMSConfigDAO CDMSConfigDAO
        {
            get
            {
                if (_portalConfigDAO == null)
                    _portalConfigDAO = UnityHelper.Container.Resolve<ICDMSConfigDAO>();
                return _portalConfigDAO;
            }
        }
        #endregion

        #region Public Method
        public List<CDMSConfig> GetCDMSConfig()
        {
            return CDMSConfigDAO.GetCDMSConfigs();
        }
        #endregion
    }
}
