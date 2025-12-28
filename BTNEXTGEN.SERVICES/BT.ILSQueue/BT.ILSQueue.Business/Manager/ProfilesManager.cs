using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.Business.DAO;
using BT.ILSQueue.Business.Models;
using BT.ILSQueue.Business.Helpers;
using BT.ILSQueue.Business.MongDBLogger.ELMAHLogger;


namespace BT.ILSQueue.Business.Manager
{
    public class ProfilesManager
    {
        #region Private Member

        private static volatile ProfilesManager _instance;
        private static readonly object SyncRoot = new Object();
        public static ProfilesManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProfilesManager();
                }

                return _instance;
            }
        }

        #endregion

        public PolarisProfile GetILSProfile(string organizationID)
        {
            ILSValidationRequest orgIls = ProfilesDAO.Instance.GetILSConfiguration(organizationID);

            try
            {
                if (!string.IsNullOrEmpty(orgIls.ILSApiKey))
                    orgIls.ILSApiKey = APIEncryptionHelper.Decrypt(orgIls.ILSApiKey);

                if (!string.IsNullOrEmpty(orgIls.ILSApiSecret))
                    orgIls.ILSApiSecret = APIEncryptionHelper.Decrypt(orgIls.ILSApiSecret);
            }
            catch (Exception ex)
            {
                FileLogger.LogException("GetILSProfile - " + ex.Message, ex);

                ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));
            }

            /*PolarisProfile polarisProfile = new PolarisProfile();

           polarisProfile.papiURL = "https://qa-polaris.polarislibrary.com/PAPIService/rest";
           polarisProfile.papiID = "TS360API";
           polarisProfile.papiAccesskey = "C987543D-8D2E-4C59-8B6F-4D9E01C70B97";
           polarisProfile.domain = "QA-Polaris";
           polarisProfile.account = "VendorAccount";
           polarisProfile.password = "VendorTesting01!";

           return polarisProfile;*/

           PolarisProfile polarisProfile = new PolarisProfile();

           polarisProfile.papiURL = orgIls.ILSUrl;
           polarisProfile.papiID = orgIls.ILSLogin;
           polarisProfile.papiAccesskey = orgIls.ILSApiKey;
           polarisProfile.domain = orgIls.ILSUserDomain;
           polarisProfile.account = orgIls.ILSUserAccount;
           polarisProfile.password = orgIls.ILSApiSecret;

           return polarisProfile;
           
        }

        public MARCJsonRequest GetMARCJsonRequestParameter(ILSQueueRequest request, DateTime requestDateTime)
        {
            MARCJsonRequest marcRequestParam = ProfilesDAO.Instance.GetMARCRequestParameter(request.UpdatedBy);

            marcRequestParam.BasketSummaryID = request.ExternalID;
            marcRequestParam.IsCancelled = false;
            marcRequestParam.IsOrdered = false;
            marcRequestParam.ProfileID = request.MARCProfileID;
            marcRequestParam.RequestDateTime = requestDateTime;

          
            bool marcGridSort;
            List<MARCProfile> marcProfileList = ProductDAO.Instance.GetMARCProfiles(request.OrganizationID, out marcGridSort);
            var selectedProfile = marcProfileList.Where(x => x.MARCProfileId == request.MARCProfileID).FirstOrDefault();
            if (selectedProfile != null)
            {
                marcRequestParam.HasInventoryRules = selectedProfile.HasInventoryRules;
            }

            return marcRequestParam;
        }
    }
}
