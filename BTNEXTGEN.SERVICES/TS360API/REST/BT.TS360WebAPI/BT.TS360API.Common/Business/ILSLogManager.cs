using BT.TS360API.Common.Helper;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Common.Business
{
    class ILSLogManager
    {
        private static readonly object SyncRoot = new Object();

        public static ILSLogManager Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    ILSLogManager _instance = HttpContext.Current == null ? new ILSLogManager() : HttpContext.Current.Items["ILSLogManager"] as ILSLogManager;
                    if (_instance == null)
                    {
                        _instance = new ILSLogManager();
                        HttpContext.Current.Items.Add("ILSLogManager", _instance);
                    }

                    return _instance;
                }
            }
        }

        public ILSLog GetILSLog(string cartId)
        {

            GlobalConfigurationHelper ILSGlobalSettings = GlobalConfigurationHelper.Instance;
            string ilsAPILogUrl = ILSGlobalSettings.Get(GlobalConfigurationKey.ILSGetLogApiUrl);
            string ilsAPIReqUrl = String.Format("{0}/{1}", ilsAPILogUrl, cartId);
            ILSLog cartILSAPILog;

            using (WebClient client = new WebClient())
            {
                client.Headers["Content-type"] = "application/json";
                string responseJson = client.DownloadString(ilsAPIReqUrl);
                cartILSAPILog = Newtonsoft.Json.JsonConvert.DeserializeObject<ILSLog>(responseJson);
            }

            return cartILSAPILog;
        }

        /// <summary>
        /// logs error to MONGO
        /// </summary>
        /// <param name="cartILSAPILog"></param>
        public void InsertILSLog(ILSLog cartILSAPILog)
        {
            GlobalConfigurationHelper ILSGlobalSettings = GlobalConfigurationHelper.Instance;
            string ilsAPILogUrl = ILSGlobalSettings.Get(GlobalConfigurationKey.ILSInsertLogApiUrl);
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers["Content-type"] = "application/json";
                    string responseJson = client.UploadString(ilsAPILogUrl, Newtonsoft.Json.JsonConvert.SerializeObject(cartILSAPILog));
                }
            }
            catch (Exception Ex)
            {
                Logger.LogException("Method- ProcessILSOrder() - " + Ex.Message + " - CartId : " + cartILSAPILog.BasketSummaryId, ExceptionCategory.ILS.ToString(), Ex);
            }

        }
    }
}
