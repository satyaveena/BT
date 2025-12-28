using BT.TS360API.Cache;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Models;
using BT.TS360API.Logging;
using System.Collections.Generic;

namespace BT.TS360API.Common.Grid
{
    public class UserGridFieldsCodesManager
    {
        private static UserGridFieldsCodesManager _instance = null;
        private static readonly string USER_GRID_FIELDS_CACHE_KEY = "USER_GRID_FIELDS_CACHE_KEY_{0}";
        private static readonly string USER_OBJECT_CACHE_KEY = "USER_OBJECT_CACHE_KEY_{0}";

      

        public static UserGridFieldsCodesManager Instance
        {
            get { return _instance ?? (_instance = new UserGridFieldsCodesManager()); }
        }


        public void SaveUserGridFieldsCodes(string userId, List<CommonBaseGridUserControl.UIUserGridField> userGridFieldObjects, int defaultQuantity)
        {
            GridDAOManager.Instance.SaveUserGridFieldsCodes(userId, userGridFieldObjects, defaultQuantity);
            var cacheKey = string.Format(USER_GRID_FIELDS_CACHE_KEY, userId);
            //VelocityCacheManager.SetExpired(cacheKey);
            CachingController.Instance.SetExpired(cacheKey);
            var userObjCacheKey = string.Format(USER_OBJECT_CACHE_KEY, userId);
            //VelocityCacheManager.SetExpired(userObjCacheKey);
            CachingController.Instance.SetExpired(userObjCacheKey);
        }

        public UserGridFieldsCodes GetUserGridFieldsCodesIII(string userId, string orgId)
        {
            var userGridFieldCodes = new UserGridFieldsCodes();

           // userGridFieldCodes = GetUserGridFieldsCodesFromAppCache(userId, orgId);
            //if (userGridFieldCodes != null) return userGridFieldCodes;

            //userGridFieldCodes = new UserGridFieldsCodes();
            if (CommonHelper.IsAuthorizeToUseAllGridCodes(orgId))
            {
                var gridCodes = new List<CommonBaseGridUserControl.UIGridCode>();
                var fieldCodeDs = DistributedCacheHelper.GetGridFieldsCodesForOrgFromDao(orgId);
                var orgGridFieldCodes = DistributedCacheHelper.RefineDsAndGetActiveUiGridField(fieldCodeDs, true);
               // var orgGridFieldCodes = DistributedCacheHelper.GetActiveGridFieldsForOrg(orgId, true);
                foreach (var gf in orgGridFieldCodes)
                {
                    gridCodes.AddRange(gf.UIGridCodes);
                }

                gridCodes.ForEach(item => item.IsAuthorized = true);

                userGridFieldCodes.UserGridCodes = gridCodes;
                userGridFieldCodes.UserGridFields = GetUserGridFields(userId);
                userGridFieldCodes.DefaultQuantity = GetDefaultQuantity(userId);
            }
            else
            {
                userGridFieldCodes = GridDataAccessManager.Instance.GetUserGridFieldsCodes(userId, orgId);
            }

            //StoreUserGridFieldsCodesToAppCache(userId, orgId, userGridFieldCodes);

            return userGridFieldCodes;
        }

        private static UserGridFieldsCodes GetUserGridFieldsCodesFromAppCache(string userId, string orgId)
        {
            var cacheKey = string.Format("GetUserGridFieldsCodesCacheKey_{0}_{1}", userId, orgId);

            // return VelocityCacheManager.Read(cacheKey, VelocityCacheLevel.Request) as UserGridFieldsCodes;
            return CachingController.Instance.Read(cacheKey) as UserGridFieldsCodes;
        }
        private List<CommonBaseGridUserControl.UIUserGridField> GetUserGridFields(string userId)
        {
            var cacheKey = string.Format(USER_GRID_FIELDS_CACHE_KEY, userId);
            var userGridFields = CachingController.Instance.Read(cacheKey) as List<CommonBaseGridUserControl.UIUserGridField>;

            if (userGridFields == null)
            {
                userGridFields = GridDAOManager.Instance.GetGridFieldsByUser(userId);
                CachingController.Instance.Write(cacheKey, userGridFields);
            }
            return userGridFields;
        }
        private int GetDefaultQuantity(string userId)
        {
            try
            {
                var cacheKey = string.Format(USER_OBJECT_CACHE_KEY, userId);
                var userObj = CachingController.Instance.Read(cacheKey) as UserPreference;
                if (userObj == null)
                {
                    userObj = GridDataAccessManager.Instance.GetUserPreference(userId);
                    CachingController.Instance.Write(cacheKey, userObj);
                }
                if (userObj != null)
                    return userObj.DefaultQuantity;
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex);
            }
            return 0;
        }
        private static void StoreUserGridFieldsCodesToAppCache(string userId, string orgId, UserGridFieldsCodes userGridFieldsCodes)
        {
            var cacheKey = string.Format("GetUserGridFieldsCodesCacheKey_{0}_{1}", userId, orgId);

            CachingController.Instance.Write(cacheKey, userGridFieldsCodes);
        }
    }

    public class UserGridFieldsCodes
    {
        public List<CommonBaseGridUserControl.UIUserGridField> UserGridFields { get; set; }

        public List<CommonBaseGridUserControl.UIGridCode> UserGridCodes { get; set; }

        public int DefaultQuantity { get; set; }

        public UserGridFieldsCodes()
        {
            DefaultQuantity = 0;
        }
    }
   
}

