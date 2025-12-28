using BT.CDMS.Business.Constants;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace BT.CDMS.Business.Helper
{
    /// <summary>
    /// Class ConfigSectionClass
    /// </summary>
    public class ConfigSectionClass
    {
        #region Method
        /// <summary>
        /// GetConnectionInformation
        /// </summary>
        /// <returns>List<ConfigClass></returns>

        public static List<ConfigClass> GetConnectionInformation()
        {
            List<ConfigClass> configClasses = new List<ConfigClass>();
            NameValueCollection settings =  ConfigurationManager.GetSection("environmentSettingsGroup/environmentSettings") as System.Collections.Specialized.NameValueCollection;
            string keyName = string.Empty;
            if (settings != null)
            {
                foreach (string key in settings.AllKeys)
                {
                    configClasses.Add(new ConfigClass() { ConfigKey = key, Envname = key.Split('_')[0] });
                }
            }
            return configClasses;
        }
        #endregion
    }
}