using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace BTNextGen.BizTalk.RefreshSSOCache
{
    public class RefreshSSOData
    {
        static void Main(string[] args)
        {

           BTNextGen.BizTalk.SSO.Utility. CacheManger.Instance.PopulateCacheFromSSO();
            System.Diagnostics.EventLog.WriteEntry("SSO data is cached", "SSO data is cached");
        }
    }
}
 