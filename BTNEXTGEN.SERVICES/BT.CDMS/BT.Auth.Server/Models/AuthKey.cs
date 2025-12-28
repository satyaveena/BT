using BT.Auth.Business.Helpers;
using BT.Auth.Business.Manager.Interface;
using BT.Auth.Business.Models;
using System.Collections.Generic;
using Unity;

namespace BT.Auth.Server.Models
{
    public class AuthKey
    {
        #region Private Members
        private IAuthConfigManager _authConfigManager = null;
        private List<ApplicationKeys> _authConfig = null;
        #endregion

        #region Constructor 

        public AuthKey()
        {
            _authConfigManager = UnityHelper.Container.Resolve<IAuthConfigManager>();
        }
        #endregion

        #region Methods 

        public ApplicationKeys GetConfigValue(string key)
        {
            if (_authConfig == null)
            {
                _authConfig = _authConfigManager.GetAuthConfig();
            }
            var obj = _authConfig.Find(xx => xx.ApiKey == key);
            return obj;
        }

        #endregion 
    }
}