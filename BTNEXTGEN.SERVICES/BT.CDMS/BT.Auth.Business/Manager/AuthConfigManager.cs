using BT.Auth.Business.DataAccess.Interface;
using BT.Auth.Business.Helpers;
using BT.Auth.Business.Manager.Interface;
using BT.Auth.Business.Models;
using System.Collections.Generic;
using Unity;
namespace BT.Auth.Business.Manager
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
