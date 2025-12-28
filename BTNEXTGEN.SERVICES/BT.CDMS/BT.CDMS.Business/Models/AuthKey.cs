using BT.CDMS.Business.Helpers;
using BT.CDMS.Business.Manager.Interface;
using System.Collections.Generic;
using Unity;

namespace BT.CDMS.Business.Models
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