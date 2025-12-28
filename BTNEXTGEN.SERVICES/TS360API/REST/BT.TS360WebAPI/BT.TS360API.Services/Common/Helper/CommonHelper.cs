using BT.TS360API.Cache;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360API.Services.Services;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Services.Common.Helper
{
    public class CommonHelper
    {
        /// <summary>
        /// Checks SearchResult Items for PPC Duplicates.
        /// </summary>
        /// <param name="searchResultItems"></param>
        /// <returns>Dictionary<BTKey, isDup></returns>
        public static async Task<Dictionary<string, bool>> CheckSearchResultsForPPCDuplicates(IList<ProductSearchResultItem> searchResultItems, string OrgId, bool isOrgPPCEnabled)
        {
            var ppcDuplicates = new Dictionary<string, bool>();

            // PPC Enabled in Premium Services page
            if (isOrgPPCEnabled && searchResultItems != null && searchResultItems.Count > 0)
            {
                // get org PPC Collection from farm cache
                var cacheKey = string.Format(CacheKeyConstant.OrganizationPPCSubscriptions_CACHE_KEY_PREFIX, OrgId);
                var orgPPCSubscriptions = CachingController.Instance.Read(cacheKey) as Dictionary<string, string>;

                if (orgPPCSubscriptions == null || orgPPCSubscriptions.Count == 0)
                {
                    var _orgService = new OrganizationService();
                    var list = await _orgService.GetOrganizationPPCSubscriptions(OrgId);

                    orgPPCSubscriptions = CachingController.Instance.Read(cacheKey) as Dictionary<string, string>;
                }

                foreach (var resultItem in searchResultItems)
                {
                    if (ppcDuplicates.ContainsKey(resultItem.BTKey))
                        continue;

                    var isDuplicate = false;

                    if (orgPPCSubscriptions != null && orgPPCSubscriptions.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(resultItem.PPCAuxCodes))
                        {
                            var ppcAuxCodes = resultItem.PPCAuxCodes.Split('|').ToList();
                            // there must be at least one auxcode active in org PPC Collections subscription
                            isDuplicate = orgPPCSubscriptions.Any(ppc => ppcAuxCodes.Contains(ppc.Value, StringComparer.CurrentCultureIgnoreCase));
                        }
                    }

                    // add to results
                    ppcDuplicates.Add(resultItem.BTKey, isDuplicate);
                }
            }

            return ppcDuplicates;
        }
    }
}