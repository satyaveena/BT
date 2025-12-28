using System;
using System.Collections.Generic;
using System.Linq;
using BT.TS360API.Cache;
using BT.TS360API.Marketing.DataAccess;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;

namespace BT.TS360API.Marketing
{
    public class ProfileDAOManager
    {
        private ProfileDAOManager()
        { }

        private static volatile ProfileDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        public static ProfileDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProfileDAOManager();
                }

                return _instance;
            }
        }

        public List<BTProductInterestGroup> GetProductInterestGroup()
        {
            // get from Cache
            var results = CachingController.Instance.Read(MarketingConstants.PigCacheKey) as List<BTProductInterestGroup>;

            if (results == null)
            {
                results = ProfileDAO.Instance.GetProductInterestGroup(); //ProductInterestGroupDAO.Instance.GetProductInterestGroup();
                // write to cache
                CachingController.Instance.Write(MarketingConstants.PigCacheKey, results);
            }

            return results;
        }
    }
}
