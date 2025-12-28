using System;
using System.Collections.Generic;
using System.Web;

namespace BT.TS360API.Common.Helper
{
    public class GlobalConfigurationHelper
    {
        private static readonly object SyncRoot = new Object();

        private static Dictionary<string, string> _settings = new Dictionary<string, string>();
        static GlobalConfigurationHelper _instance;

        public static GlobalConfigurationHelper Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    if ( _instance == null)
                    {
                        _instance = new GlobalConfigurationHelper();
                    }
                    else if (HttpContext.Current != null )
                    {
                        _instance = HttpContext.Current.Items["GlobalConfigurationHelper"] as GlobalConfigurationHelper;
                        if (_instance == null)
                        {
                            _instance = new GlobalConfigurationHelper();
                            HttpContext.Current.Items.Add("GlobalConfigurationHelper", _instance);
                        }
                    }
                    return _instance;
                }
            }
        }


        public void Add(string key, string value)
        {
            if (_settings.ContainsKey(key))
                _settings[key] = value;
            else
                _settings.Add(key, value);
        }

        public string Get(string key)
        {
            return _settings[key];
        }

    }
}
