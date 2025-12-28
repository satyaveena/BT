using BT.TS360API.Cache;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Services.Services
{
    public class OrganizationService
    {
        public async Task<List<PPCSubscription>> GetPPCSubscriptions()
        {
            // read cache
            var ppcSubscriptions = CachingController.Instance.Read(CacheKeyConstant.PPCSubscriptions_CACHE_KEY_PREFIX) as List<PPCSubscription>;

            if (ppcSubscriptions == null)
            {
                ppcSubscriptions = await OrganizationDAO.Instance.GetPPCSubscriptions();

                // write cache
                CachingController.Instance.Write(CacheKeyConstant.PPCSubscriptions_CACHE_KEY_PREFIX, ppcSubscriptions);
            }

            return ppcSubscriptions;
        }

        public async Task<List<string>> GetOrganizationPPCSubscriptions(string orgId)
        {
            // read cache
            var cacheKey = string.Format(CacheKeyConstant.OrganizationPPCSubscriptions_CACHE_KEY_PREFIX, orgId);
            var selectedPPCSubscriptions = CachingController.Instance.Read(cacheKey) as Dictionary<string, string>;
            List<string> orgPPCIDs;

            if (selectedPPCSubscriptions == null)
            {
                orgPPCIDs = await OrganizationDAO.Instance.GetOrgSelectedPPCSubscriptions(orgId);

                // get all PPC from cache
                var ppcSubscriptions = await GetPPCSubscriptions();

                selectedPPCSubscriptions = ppcSubscriptions.Where(p => orgPPCIDs.Contains(p.ID))
                                                            .ToDictionary(p => p.ID, p => p.AuxCode);
                // write Dictionary<PPCID, AuxCode> to cache
                CachingController.Instance.Write(cacheKey, selectedPPCSubscriptions);
            }

            // return PayPerCirculationID list
            orgPPCIDs = selectedPPCSubscriptions.Select(p => p.Key).ToList();

            return orgPPCIDs;
        }
    }
}