using System.Web;
using BT.TS360Constants;

namespace BT.TS360API.Common.Helpers
{
    public class ProxySessionHelper
    {
        public static string ConvertToProxyKeyName(string key)
        {
            return key + CommonConstants.ProxyUserSuffix;
        }

        public static bool IsInProxySession()
        {
            //check param "proxieduserid" on querystring of a http request of a web page
            var proxiedUserId = HttpContext.Current.Request.QueryString[QueryStringName.ProxiedUserId];
            if (!string.IsNullOrEmpty(proxiedUserId))
                return true;
            //check param "proxieduserid" in the referer url of a WCF Ajax service
            var referedUrl = HttpContext.Current.Request.Headers[CommonConstants.HttpRequestReferer];
            if (!string.IsNullOrEmpty(referedUrl) && referedUrl.Contains(QueryStringName.ProxiedUserId))
                return true;

            return false;
        }

        public static string GetProxiedUserIdFromQS()
        {
            var proxiedUserId = HttpContext.Current.Request.QueryString[QueryStringName.ProxiedUserId];
            return proxiedUserId;
        }

        public static string AppendProxyUserId(string url)
        {
            var proxiedUserId = HttpContext.Current.Request.QueryString[QueryStringName.ProxiedUserId];
            var proxyParam = string.Empty;
            if (!string.IsNullOrEmpty(proxiedUserId))
            {
                proxyParam = QueryStringName.ProxiedUserId + "=" + proxiedUserId;
            }
            else
            {
                var referedUrl = HttpContext.Current.Request.Headers[CommonConstants.HttpRequestReferer];
                if (!string.IsNullOrEmpty(referedUrl) && referedUrl.Contains(QueryStringName.ProxiedUserId))
                {
                    proxyParam = QueryStringName.ProxiedUserId + "=" + ExtractProxiedUserId(referedUrl);
                }
            }

            if (!string.IsNullOrEmpty(proxyParam) &&
                !string.IsNullOrEmpty(url) &&
                !url.Contains(QueryStringName.ProxiedUserId))
            {
                if (url.Contains("?"))
                {
                    url += "&" + proxyParam;
                }
                else
                {
                    url += "?" + proxyParam;
                }
            }
            return url;
        }

        private static string ExtractProxiedUserId(string url)
        {
            var questionIndex = url.IndexOf('?');
            var queryString = url.Substring(questionIndex);
            if (!string.IsNullOrEmpty(queryString))
            {
                var qsCollection = HttpUtility.ParseQueryString(queryString);
                if (qsCollection[QueryStringName.ProxiedUserId] != null)
                {
                    return qsCollection[QueryStringName.ProxiedUserId];
                }
            }
            return string.Empty;
        }
    }
}
