using System.Collections.Generic;

namespace Cybersource_rest_samples_dotnetXXX
{
    public class Configuration
    {
        // initialize dictionary object
        private readonly  Dictionary<string, string> _configurationDictionary = new Dictionary<string, string>();

        public Dictionary<string, string> GetConfiguration(string merchantid, string merchantsecretkey, string merchantkeyid, string runenviroment)
        {
            _configurationDictionary.Add("authenticationType", "HTTP_SIGNATURE");
            _configurationDictionary.Add("merchantID", merchantid );
            _configurationDictionary.Add("merchantsecretKey", merchantsecretkey );
            _configurationDictionary.Add("merchantKeyId", merchantkeyid );
            //_configurationDictionary.Add("merchantID", "bt_pp");
            //_configurationDictionary.Add("merchantsecretKey", "VhECEy8MO+0Wlwhvn8UYh19lHZB7si5vEm21fAlvOxQ=");
            //_configurationDictionary.Add("merchantKeyId", "80abe566-db30-4daa-a091-2dce5d430438");
            _configurationDictionary.Add("keysDirectory", "Resource");
            _configurationDictionary.Add("keyFilename",  merchantkeyid);
            _configurationDictionary.Add("runEnvironment", runenviroment );
            _configurationDictionary.Add("keyAlias", merchantkeyid );
            _configurationDictionary.Add("keyPass", merchantkeyid );
            _configurationDictionary.Add("enableLog", "FALSE");
            _configurationDictionary.Add("logDirectory", string.Empty);
            _configurationDictionary.Add("logFileName", string.Empty);
            _configurationDictionary.Add("logFileMaxSize", "5242880");
            _configurationDictionary.Add("timeout", "1000");
            _configurationDictionary.Add("proxyAddress", string.Empty);
            _configurationDictionary.Add("proxyPort", string.Empty);

            return _configurationDictionary;
        }



    }
}
