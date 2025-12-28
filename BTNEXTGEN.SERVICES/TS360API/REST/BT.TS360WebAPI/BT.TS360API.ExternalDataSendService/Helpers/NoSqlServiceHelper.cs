using BT.TS360API.ExternalDataSendService.Caching;
using BT.TS360API.ExternalDataSendService.Configration;
using BT.TS360API.ExternalDataSendService.Logging;
using BT.TS360API.ExternalDataSendService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace BT.TS360API.ExternalDataSendService.Helpers
{
    public class NoSqlServiceHelper
    {
        private static string _apiProfilesUrl;
        private static Uri ProfilesUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_apiProfilesUrl))
                {
                    _apiProfilesUrl = AppSettings.NoSQLApiProfilesUrl;
                }
                return new Uri(_apiProfilesUrl);
            }
        }

        public static List<ItemData> GetSiteTerm(string siteTermName)
        {
            var siteTermCacheKey = "__GetSiteTerm" + siteTermName;
            // check cache
            var result = WFECachingController.Instance.Read(siteTermCacheKey) as List<ItemData>;
            if (result != null) return result;

            result = new List<ItemData>();
            var siteTerms = GetSiteTermByCategory(siteTermName);
            if (siteTerms != null && siteTerms.Data != null && siteTerms.Data.SiteTermObjectList != null)
            {
                foreach (var siteTerm in siteTerms.Data.SiteTermObjectList)
                {

                    List<ItemData> Children = new List<ItemData>();
                    if (siteTerm.Children != null && siteTerm.Children.Count > 0)
                    {
                        foreach (var child in siteTerm.Children)
                        {
                            Children.Add(new ItemData(child.Value, child.Name, child.SearchValue, new List<ItemData>()));
                        }
                    }

                    result.Add(new ItemData(siteTerm.Value, siteTerm.Name, siteTerm.SearchValue, Children));
                }

                WFECachingController.Instance.Write(siteTermCacheKey, result);

                return result;
            }
            return null;
        }

        public static NoSqlServiceResult<SiteTermResponse> GetSiteTermByType(string type)
        {
            try
            {
                // Create a WebClient to GET the request
                using (var client = new WebClient())
                {
                    // Set the header so it knows we are sending JSON
                    client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                    //client.Encoding = System.Text.Encoding.UTF8;

                    var jss = new JavaScriptSerializer();
                    // Serialise the data we are sending in to JSON
                    string address = ProfilesUrl + "/siteterm/" + type;
                    // Make the request
                    var response = client.DownloadString(address);

                    // Deserialise the response into a GUID
                    return jss.Deserialize<NoSqlServiceResult<SiteTermResponse>>(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        public static NoSqlServiceResult<SiteTermCategory> GetSiteTermByCategory(string category)
        {
            try
            {
                // Create a WebClient to GET the request
                using (WebClient client = new WebClient())
                {
                    // Set the header so it knows we are sending JSON
                    client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                    var jss = new JavaScriptSerializer();
                    // Serialise the data we are sending in to JSON
                    string address = ProfilesUrl + "/siteterm/id/" + category;
                    // Make the request
                    var response = client.DownloadString(address);

                    // Deserialise the response into a GUID
                    return jss.Deserialize<NoSqlServiceResult<SiteTermCategory>>(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        public static List<ItemData> GetDisabledReasons()
        {
            return GetSiteTerm("DisableReasonCode");
        }

        public static string GetDisabledReasonText(string code)
        {
            var reasonText = code;

            var reasons = GetDisabledReasons();
            if (reasons != null && reasons.Count > 0)
            {
                var disabledReason = reasons.Where(r => r.ItemDataValue == code).FirstOrDefault();
                if (disabledReason != null)
                    reasonText = disabledReason.ItemDataText;
            }

            return reasonText;
        }
    }
}