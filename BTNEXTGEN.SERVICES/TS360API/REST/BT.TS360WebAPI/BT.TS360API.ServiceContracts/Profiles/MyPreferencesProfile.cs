using System;

namespace BT.TS360API.ServiceContracts.Profiles
{
    public class MyPreferencesProfile
    {
        public string UserID { get; set; }
        public bool DisplayQuotationDisclaimer { get; set; }
        public bool IsInitialDisplayQuotationDisclaimer { get; set; }
        public string CIPUserToken { get; set; }
        public DateTime? CIPLastLoginDateTime { get; set; }
        public string ESPPortalUserToken { get; set; }
        public string OCSUserToken { get; set; }
    }
}
