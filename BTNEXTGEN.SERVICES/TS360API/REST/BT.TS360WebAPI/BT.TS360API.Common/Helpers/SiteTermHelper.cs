using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BT.TS360API.Cache;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;

namespace BT.TS360API.Common.Helpers
{
    public class SiteTermHelper
    {
        private static volatile SiteTermHelper _instance;
        private static readonly object SyncRoot = new Object();
        private static string _siteTermUrl;
        private static HttpClient client;

        public static SiteTermHelper Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _siteTermUrl = ConfigurationManager.AppSettings["SiteTermApiUrl"];

                        client = new HttpClient();
                        client.BaseAddress = new Uri(_siteTermUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        _instance = new SiteTermHelper();
                    }

                }

                return _instance;
            }
        }

        public async Task<List<ItemDataContract>> GetSiteTemByNameAsync(string siteTemName, bool sort = false)
        {
            try
            {
                var cacheKey = string.Format("__GetSiteTemByNameAsync{0}", siteTemName);

                var result = WFECachingController.Instance.Read(cacheKey) as List<ItemDataContract>;

                if (result != null) return result;

                result = new List<ItemDataContract>();

                var response = await client.GetAsync(string.Format("{0}/profiles/siteterm/id/{1}", _siteTermUrl, siteTemName));
                if (response.IsSuccessStatusCode)
                {
                    var stringRes = await response.Content.ReadAsStringAsync();

                    var jss = new JavaScriptSerializer();
                    var stRes = jss.Deserialize<SiteTermResponse>(stringRes);

                    if (stRes != null && stRes.Data != null && stRes.Data.SiteTermObjectList != null)
                    {
                        foreach (var item in stRes.Data.SiteTermObjectList)
                        {
                            result.Add(new ItemDataContract() { ItemKey = item.Name, ItemValue = item.Value });
                        }
                    }

                    var sortedList = sort ? result.OrderBy(s => s.ItemKey).ToList() : result;


                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.SiteTerm.ToString());
            }
            return null;
        }

        public string GetSiteTermName(SiteTermName siteTermName, string id)
        {
            if (string.IsNullOrEmpty(id))
                return string.Empty;
            //
            var siteterm = GetSiteTemByName(siteTermName.ToString()).FirstOrDefault(c => c.ItemValue == id);
            if (siteterm != null)
            {
                return siteterm.ItemKey;
            }
            return string.Empty;
        }

        public string GetSiteTermValueByKey(SiteTermName siteTermName, string siteTermKey)
        {
            if (string.IsNullOrEmpty(siteTermKey))
                return string.Empty;
            //
            var siteterm = GetSiteTemByName(siteTermName.ToString()).FirstOrDefault(c => c.ItemKey == siteTermKey);
            if (siteterm != null)
            {
                return siteterm.ItemValue;
            }
            return string.Empty;
        }

        public List<ItemDataContract> GetSiteTemByName(string siteTemName, bool sort = false)
        {
            try
            {
                var cacheKey = string.Format("__GetSiteTemByName{0}", siteTemName);

                var result = WFECachingController.Instance.Read(cacheKey) as List<ItemDataContract>;

                if (result != null) return result;

                result = new List<ItemDataContract>();

                var response = client.GetAsync(string.Format("{0}/profiles/siteterm/id/{1}", _siteTermUrl, siteTemName)).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var stringRes = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    return PersitSiteTermToCache(sort, cacheKey, result, stringRes);
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.SiteTerm.ToString());
            }
            return null;
        }

        public List<SiteTermItem> GetFlattenAudienceTypesSiteTem(bool sort = false)
        {
            try
            {
                var siteTemName = SiteTermName.AudienceTypes.ToString();
                var cacheKey = "__GetAudienceTypesSiteTem";

                var result = WFECachingController.Instance.Read(cacheKey) as List<SiteTermItem>;

                if (result != null) return result;

                result = new List<SiteTermItem>();

                var response = client.GetAsync(string.Format("{0}/profiles/siteterm/id/{1}", _siteTermUrl, siteTemName)).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var stringRes = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    var jss = new JavaScriptSerializer();
                    var stRes = jss.Deserialize<SiteTermResponse>(stringRes);

                    if (stRes != null && stRes.Data != null && stRes.Data.SiteTermObjectList != null)
                    {
                        foreach (var item in stRes.Data.SiteTermObjectList)
                        {
                            if (item.Children != null && item.Children.Count > 0)
                            {
                                result.AddRange(item.Children);
                            }
                            else
                            {
                                result.Add(item);
                            }
                        }
                    }

                    var sortedList = sort ? result.OrderBy(s => s.Name).ToList() : result;
                    WFECachingController.Instance.Write(cacheKey, sortedList);

                    return sortedList;
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.SiteTerm.ToString());
            }
            return null;
        }

        private static List<ItemDataContract> PersitSiteTermToCache(bool sort, string cacheKey, List<ItemDataContract> result, string stringRes)
        {
            var jss = new JavaScriptSerializer();
            var stRes = jss.Deserialize<SiteTermResponse>(stringRes);

            if (stRes != null && stRes.Data != null && stRes.Data.SiteTermObjectList != null)
            {
                foreach (var item in stRes.Data.SiteTermObjectList)
                {
                    result.Add(new ItemDataContract() { ItemKey = item.Value, ItemValue = item.Name });
                }
            }

            var sortedList = sort ? result.OrderBy(s => s.ItemKey).ToList() : result;

            WFECachingController.Instance.Write(cacheKey, sortedList);

            return sortedList;
        }
    }
}
