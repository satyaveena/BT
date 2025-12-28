using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Common
{
    public class ServiceRequestContext
    {
        private static ServiceRequestContext _instance;
        private static readonly object SyncRoot = new Object();

        public static ServiceRequestContext Current
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new ServiceRequestContext();
                    }

                }

                return _instance;
            }
        }

        public string Host
        {
            get
            {
                var host = string.Empty;

                var requestHeaders = HttpContext.Current.Request.Headers;
                if (requestHeaders != null && requestHeaders.Count > 0)
                {
                    host = requestHeaders[HttpRequestHeader.Host.ToString()];
                }

                return host;
            }
        }

        public string UserId
        {
            get
            {
                var userId = string.Empty;

                var requestHeaders = HttpContext.Current.Request.Headers;
                if (requestHeaders != null && requestHeaders.Count > 0)
                {
                    userId = requestHeaders[ServiceRequestHeader.RequestUserId];
                }

                return userId;
            }
        }

        public bool IsFromTitleSource360Site
        {
            get
            {
                var isValid = false;
                var refererUrl = this.RefererUrl;
                var titleSourceConfigUrl = Configrations.AppSettings.TitleSourceSiteUrl;
                if (!string.IsNullOrWhiteSpace(refererUrl) && !string.IsNullOrWhiteSpace(titleSourceConfigUrl)
                    && string.Compare(titleSourceConfigUrl, refererUrl, true) >= 0) // refererUrl contains titleSourceConfigUrl
                {
                    isValid = true;
                }

                return isValid;
            }
        }

        public string RefererUrl
        {
            get
            {
                var refererUrl = string.Empty;

                var requestHeaders = HttpContext.Current.Request.Headers;
                if (requestHeaders != null && requestHeaders.Count > 0)
                {
                    refererUrl = requestHeaders[HttpRequestHeader.Referer.ToString()];
                }

                return refererUrl;
            }
        }

        //public string WfeServerName
        //{
        //    get
        //    {
        //        var wfeServerName = string.Empty;

        //        var requestHeaders = HttpContext.Current.Request.Headers;
        //        if (requestHeaders != null && requestHeaders.Count > 0)
        //        {
        //            wfeServerName = requestHeaders[ServiceRequestHeader.WfeServerName];
        //        }

        //        return wfeServerName;
        //    }
        //}
    }
}
