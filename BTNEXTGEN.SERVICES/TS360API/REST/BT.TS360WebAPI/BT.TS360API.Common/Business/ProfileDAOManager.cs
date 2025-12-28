using BT.TS360API.Cache;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Business
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

        public List<AdditionContent> SortReviewTypes(List<AdditionContent> reviews, bool reverse = false)
        {
            //Check null
            if (reviews == null || reviews.Count <= 0) return new List<AdditionContent>();

            //Separate into 2 list: 1. a list has Sequence & 2. a list has no Sequence
            var unRecognizeItem = reviews.Where(r => string.IsNullOrEmpty(r.Sequence)).ToList(); //get List of item has no sequence
            reviews.RemoveAll(r => string.IsNullOrEmpty(r.Sequence));//get list has Sequence

            //Initialize DisplayName
            foreach (var additionContent in unRecognizeItem)
            {
                //additionContent.DisplayName = GetSiteTermName(SiteTermName.ReviewType, additionContent.ReviewTypeId);
                additionContent.DisplayName = SiteTermHelper.Instance.GetSiteTermName(SiteTermName.ReviewType, additionContent.ReviewTypeId);
            }

            //sort items by DisplayName
            unRecognizeItem.Sort(CompareMyReviewTypeByDisplayName);
            //Sort by Sequence Number
            reviews.Sort(CompareMyReviewType);
            //Combine 2 list
            if (reverse) reviews.InsertRange(0, unRecognizeItem);
            else reviews.AddRange(unRecognizeItem);

            return reviews;
        }

        public static int CompareMyReviewType(AdditionContent x, AdditionContent y)
        {
            if (string.IsNullOrEmpty(x.Sequence))
            {
                if (string.IsNullOrEmpty(y.Sequence))
                {
                    return CompareMyReviewTypeByDisplayName(x, y); //equal
                }
                return -1; // x is less than y
            }
            if (string.IsNullOrEmpty(y.Sequence))
            {
                return 1; //x is greater than y
            }
            var xOrdinal = Int32.Parse(x.Sequence);
            var yOrdinal = Int32.Parse(y.Sequence);
            return xOrdinal.CompareTo(yOrdinal);
        }

        public static int CompareMyReviewTypeByDisplayName(AdditionContent x, AdditionContent y)
        {
            if (string.IsNullOrEmpty(x.DisplayName))
            {
                if (string.IsNullOrEmpty(y.DisplayName))
                {
                    return 0; //equal
                }
                return -1; // x is less than y
            }
            if (string.IsNullOrEmpty(y.DisplayName))
            {
                return 1; //x is greater than y
            }

            return x.DisplayName.CompareTo(y.DisplayName);
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
                //CacheHelper.Write(MarketingConstants.PigCacheKey, results);
            }

            return results;
        }

        public async Task<MyPreferencesProfile> GetUserPreferenceById(string userId)
        {
            var myPreferences = await ProfileDAO.Instance.GetUserPreferenceById(userId);
            return myPreferences;
        }

        public async Task<bool> UpdateTSSONotificationCartUsers(List<string> activeNotificationCartUsers, List<string> removedNotificationCartUsers)
        {
            var tssoNotificationCartUsersStatus = await ProfileDAO.Instance.SetTSSONotificationCartUsers(activeNotificationCartUsers, removedNotificationCartUsers);
            return tssoNotificationCartUsersStatus;
        }

        public async Task<bool> UpdateUserNRCProductTypes(string userID, string NRCProductTypes)
        {
            var userNRCProductTypesStatus = await ProfileDAO.Instance.SetUserNRCProductTypes(userID, NRCProductTypes);
            return userNRCProductTypesStatus;
        }

        public bool SaveILSDetail(ILSValidationRequest request)
        {
            try
            {
                
                    var encryedKey = APIEncryptionHelper.Encrypt(request.ILSApiKey);
                    var encryedSecret = APIEncryptionHelper.Encrypt(request.ILSApiSecret);

                    ProfileDAO.Instance.SaveILSDetail(request.TSOrgId, request.ILSLogin, request.ILSUrl, encryedKey, encryedSecret
                        , request.ILSValidationStatusId, request.ILSValidationErrorMessage, request.ILSValidationDateTime, request.ILSType, request.ILSUserDomain, request.ILSUserAccount);

                    return true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex.Message + " - SaveILSDetail : ", ExceptionCategory.ILS.ToString(), ex);
                return false;
            }
        }

        public ILSValidationRequest GetILSConfiguration(string orgId)
        {
            ILSValidationRequest orgIls = ProfileDAO.Instance.GetILSConfiguration(orgId);
            try
            {
                if (!string.IsNullOrEmpty(orgIls.ILSApiKey))
                    orgIls.ILSApiKey = APIEncryptionHelper.Decrypt(orgIls.ILSApiKey);

                if (!string.IsNullOrEmpty(orgIls.ILSApiSecret))
                    orgIls.ILSApiSecret = APIEncryptionHelper.Decrypt(orgIls.ILSApiSecret);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex.Message + " - GetILSConfiguration : ", ExceptionCategory.ILS.ToString(), ex);
            }
            return orgIls;
        }
        public async Task<string> GeteSupplierAccountNumber(string userId, string basketSummaryId)
        {
            return await ProfileDAO.Instance.GeteSupplierAccountNumber(userId, basketSummaryId);
        }

        public List<VendorCode> AddIlsVendorCodes(VendorCodeAddRequest request)
        {
            if (request == null || request.CodeList == null)
                return null;

            List<string> validCodes = new List<string>();
            foreach (var code in request.CodeList)
            {
                if (!string.IsNullOrEmpty(code) && code.Length <= 5)
                    validCodes.Add(code);
            }
            List<VendorCode> resultList = ProfileDAO.Instance.AddIlsVendorCodes(request.OrganizationId, request.UserId, validCodes, request.IsImport);

            return resultList;
        }

        public List<VendorCode> AddIlsCodes(VendorCodeAddRequest request)
        {
            if (request == null || request.CodeList == null)
                return null;

            List<string> validCodes = new List<string>();
            foreach (var code in request.CodeList)
            {
                if (request.VendorID == ILSVendorType.Sierra && request.OrderingType == 1)
                {
                    if (!string.IsNullOrEmpty(code) && code.Length <= 5)
                        validCodes.Add(code);
                }
                else if(request.VendorID == ILSVendorType.Polaris && request.OrderingType == 1)
                {
                    if (!string.IsNullOrEmpty(code) && code.Length <= 50)
                        validCodes.Add(code);
                }
                else if (request.OrderingType == 2)
                {
                    if (!string.IsNullOrEmpty(code) && code.Length <= 15)
                        validCodes.Add(code);
                }
            }
            List<VendorCode> resultList = ProfileDAO.Instance.AddIlsCodes(request.OrganizationId, request.UserId, validCodes, request.IsImport, request.OrderingType, request.VendorID);

            return resultList;
        }


        public List<string> GetUserAccounts(string userId)
        {
            return ProfileDAO.Instance.GetUserAccounts(userId);
        }
    }
}
