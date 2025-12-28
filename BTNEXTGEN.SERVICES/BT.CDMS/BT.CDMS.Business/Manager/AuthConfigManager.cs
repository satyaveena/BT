using BT.CDMS.Business.DataAccess.Interface;
using BT.CDMS.Business.Helpers;
using BT.CDMS.Business.Manager.Interface;
using BT.CDMS.Business.Models;
using System.Collections.Generic;
using Unity;
namespace BT.CDMS.Business.Manager
{
    /// <summary>
    /// AuthConfigManager Class
    /// </summary>
    public class AuthConfigManager : IAuthConfigManager
    {
        #region Private Member and Property
        private IAuthConfigDAO _portalConfigDAO;
        private IAuthConfigDAO AuthConfigDAO
        {
            get
            {
                if (_portalConfigDAO == null)
                    _portalConfigDAO = UnityHelper.Container.Resolve<IAuthConfigDAO>();
                return _portalConfigDAO;
            }
        }
        #endregion

        #region Public Method
        public List<ApplicationKeys> GetAuthConfig()
        {
            return AuthConfigDAO.GetAuthConfigs();
        }
        #endregion
    }
}
