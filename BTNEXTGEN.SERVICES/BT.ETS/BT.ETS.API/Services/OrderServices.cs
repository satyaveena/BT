using BT.ETS.Business.Constants;
using BT.ETS.Business.DAO;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Manager;
using BT.ETS.Business.Models;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace BT.ETS.API.Services
{
    public class OrderServices
    {
        #region Constructor
        public OrderServices()
        {

        }
        #endregion

        #region Public Method
        internal async Task<string> InsertEtsCartToBackgroundQueue(CartReceivedRequestInput inputData)
        {
            if (inputData == null || inputData.Items == null)
                throw new BusinessException(520);
            if (string.IsNullOrEmpty(inputData.UserId))
                throw new BusinessException(105);
            if (string.IsNullOrEmpty(inputData.ESPLibraryId))
                throw new BusinessException(106);
            if (string.IsNullOrEmpty(inputData.ETSCartId))
                throw new BusinessException(107);
            if (string.IsNullOrEmpty(inputData.CartName))
                throw new BusinessException(108);

            var etsQueueItem = new ETSQueueItem(ApplicationConstants.ETS_JOB_CART_RECEIVED, (int)QueueProcessState.Loading);

            CartReceivedRequest cartReceivedInputItem = new CartReceivedRequest();
            CommonHelper.CopyProperties(cartReceivedInputItem, inputData);

            etsQueueItem.CartReceivedRequest = cartReceivedInputItem;

            await CommonDAO.Instance.InsertETSQueueItem(etsQueueItem);

            foreach (LineItemInput lineItemInput in inputData.Items)
            {
                lineItemInput.RequestID = etsQueueItem.JobID;
                await CommonDAO.Instance.InsertETSQueueLineItems(lineItemInput);

            }

            await CommonDAO.Instance.UpdateETSQueueStatus(etsQueueItem.JobID, (int)QueueProcessState.Loading, (int)QueueProcessState.New);


            return etsQueueItem.JobID.ToString();

        }

        internal async Task<string> InsertDupChecksToBackgroundQueue(DupCheckRequest inputData)
        {
            if (inputData == null || inputData.Products == null || inputData.Products.Count == 0)
                throw new BusinessException(520);
            if (string.IsNullOrEmpty(inputData.UserId))
                throw new BusinessException(105);
            
            var dupCheckDownloadCartStatus = new List<string> { "default", "includeworders", "includewcarts" };
            var dupCheckSStatus = new List<string> { "default", "series", "none" };
            string dupCStatus = ApplicationConstants.DupCheckCStatus.FirstOrDefault(x => string.Equals(x, inputData.DupCheckC, StringComparison.OrdinalIgnoreCase));
            string dupOStatus = ApplicationConstants.DupCheckOStatus.FirstOrDefault(x => string.Equals(x, inputData.DupCheckO, StringComparison.OrdinalIgnoreCase));
            string dupHStatus = ApplicationConstants.DupCheckHStatus.FirstOrDefault(x => string.Equals(x, inputData.DupCheckH, StringComparison.OrdinalIgnoreCase));
            string dupDownloadCartStatus = dupCheckDownloadCartStatus.FirstOrDefault(x => string.Equals(x, inputData.DupCheckDownloadCart, StringComparison.OrdinalIgnoreCase));
            string dupSStatus = dupCheckSStatus.FirstOrDefault(x => string.Equals(x, inputData.DupCheckS, StringComparison.OrdinalIgnoreCase));
            if (dupCStatus == null || dupOStatus==null || dupHStatus == null || dupDownloadCartStatus == null || dupSStatus == null)
                throw new BusinessException(211);

            var etsQueueItem = new ETSQueueItem(ApplicationConstants.ETS_JOB_DUPECHECK);
            etsQueueItem.DupCheckRequest = inputData;

            await CommonDAO.Instance.InsertETSQueueItem(etsQueueItem);
            return etsQueueItem.JobID.ToString();

        }

        internal async Task<string> InsertPricingToBackgroundQueue(PricingRequest inputData)
        {
            if (inputData == null || inputData.BTKeys == null || inputData.BTKeys.Count == 0)
                throw new BusinessException(520);

            if (string.IsNullOrEmpty(inputData.UserId))
                throw new BusinessException(105);

            var etsQueueItem = new ETSQueueItem(ApplicationConstants.ETS_JOB_PRICING);
            etsQueueItem.PricingRequest = inputData;

            await CommonDAO.Instance.InsertETSQueueItem(etsQueueItem);
            return etsQueueItem.JobID.ToString();

        }

        internal async Task<DupCheckDetailResult> GetDupCheckDetails(DupCheckDetailRequest inputData)
        {
            if (inputData == null)
                throw new BusinessException(520);

            if (string.IsNullOrEmpty(inputData.BTKey) || inputData.BTKey.Length != 10)
                throw new BusinessException(202);

            inputData.DupCheckStatusType = inputData.DupCheckStatusType.ToUpper();
            if (string.IsNullOrEmpty(inputData.DupCheckStatusType) || "COS".IndexOf(inputData.DupCheckStatusType) < 0)
                throw new BusinessException(212);

            //37969: Override Dup preference
            if ("CO".IndexOf(inputData.DupCheckStatusType) > -1)
            {
                if (inputData.DupCheckPreference == "" || string.Equals(inputData.DupCheckPreference, "default", StringComparison.OrdinalIgnoreCase))
                    inputData.DupCheckPreference = null;
                if (inputData.DupCheckDownloadCartType == "" || string.Equals(inputData.DupCheckDownloadCartType, "default", StringComparison.OrdinalIgnoreCase))
                    inputData.DupCheckDownloadCartType = null;
            }
            if (inputData.DupCheckStatusType.Equals("C"))
            {
                string dupCheckCPreference = ApplicationConstants.DupCheckCPreference.FirstOrDefault(x => string.Equals(x, inputData.DupCheckPreference, StringComparison.OrdinalIgnoreCase));
                if (dupCheckCPreference == null && inputData.DupCheckPreference != null)
                    throw new BusinessException(213);
            }
            if (inputData.DupCheckStatusType.Equals("O"))
            {
                string dupCheckOPreference = ApplicationConstants.DupCheckOPreference.FirstOrDefault(x => string.Equals(x, inputData.DupCheckPreference, StringComparison.OrdinalIgnoreCase));
                if (dupCheckOPreference == null && inputData.DupCheckPreference != null)
                    throw new BusinessException(213);
            }

            var dupCheckResult = new DupCheckDetailResult();
            dupCheckResult.BTKey = inputData.BTKey;
            dupCheckResult.DupCheckStatusType = inputData.DupCheckStatusType;

            switch (inputData.DupCheckStatusType) { 
                case "C":
                case "O":
                    if (string.IsNullOrEmpty(inputData.UserId))
                        throw new BusinessException(105);

                    //37969: Override Dup preference
                    string dupCheckDownloadCartType = ApplicationConstants.DupCheckDownloadCartType.FirstOrDefault(x => string.Equals(x, inputData.DupCheckDownloadCartType, StringComparison.OrdinalIgnoreCase));
                    if (dupCheckDownloadCartType == null && inputData.DupCheckDownloadCartType != null)
                        throw new BusinessException(214);

                    dupCheckResult.DupCheckCartInfo = await OrderManager.Instance.GetCheckDupDetails(inputData.UserId, inputData.BTKey, inputData.DupCheckStatusType, inputData.DupCheckPreference, inputData.DupCheckDownloadCartType);
                    break;
                    
                case "S":
                    if (string.IsNullOrEmpty(inputData.OrganizationId))
                        throw new BusinessException(101);
                    bool resultCheck = await OrderManager.Instance.ValidateUserAndOrg(string.Empty, inputData.OrganizationId);
                    if (resultCheck)
                        dupCheckResult.DupCheckSeriesInfo = await GetDupCheckDetails_Series(inputData.BTKey, inputData.OrganizationId);
                    break;
            }
            return dupCheckResult;
        }

        private async Task<List<DupCheckSeriesInfo>> GetDupCheckDetails_Series(string btkey, string organizationId)
        {
            List<ProfiledSeries> profileSeries = await SeriesHelper.GetInstance().GetDuplicateSeriesDetail(btkey, organizationId);
            if (profileSeries == null || profileSeries.Count == 0)
                throw new BusinessException(202);

            List<DupCheckSeriesInfo> result = new List<DupCheckSeriesInfo>();
            foreach (var itm in profileSeries)
                result.Add(ConvertToDupCheckSeriesInfo(itm));

            return result;
        }

        private DupCheckSeriesInfo ConvertToDupCheckSeriesInfo(ProfiledSeries profiledSerie)
        { 
            DupCheckSeriesInfo dupCheckSeriesInfo = new DupCheckSeriesInfo();
            dupCheckSeriesInfo.ProfileId = profiledSerie.ProfileID != null ? profiledSerie.ProfileID.ToString() : "";
            if (profiledSerie.RedundantProfileInformation != null)
            {
                dupCheckSeriesInfo.ProfileName = profiledSerie.RedundantProfileInformation.Name;
                dupCheckSeriesInfo.ProfileType = profiledSerie.RedundantProfileInformation.ProfileType;
                dupCheckSeriesInfo.AccountNumber = profiledSerie.RedundantProfileInformation.ShippingAccountNumber;
                if (profiledSerie.RedundantProfileInformation.Programs != null && profiledSerie.RedundantProfileInformation.Programs.Count > 0)
                    dupCheckSeriesInfo.ProgramType = string.Join(",", profiledSerie.RedundantProfileInformation.Programs);
                if (!string.IsNullOrEmpty(profiledSerie.RedundantProfileInformation.Status))
                    dupCheckSeriesInfo.IsEnabled = profiledSerie.RedundantProfileInformation.Status[0];
            }
            
            dupCheckSeriesInfo.SeriesId = profiledSerie.SeriesID != null ? profiledSerie.SeriesID.ToString() : "";
            dupCheckSeriesInfo.SeriesName = profiledSerie.RedundantSeriesInformation != null ? profiledSerie.RedundantSeriesInformation.Name : "";
            dupCheckSeriesInfo.Quantity = profiledSerie.TotalPrimaryQuantity;


            if (profiledSerie.PurchaseOrders != null && profiledSerie.PurchaseOrders.Count > 0)
            {
                dupCheckSeriesInfo.Orders = new List<OrderSeriesInfor>();
                foreach(var po in profiledSerie.PurchaseOrders)
                {
                    var order = new OrderSeriesInfor();
                    order.FormatPreferencesPrimary = po.FormatPreferencePrimary;
                    order.FormatPreferencesPrimaryQuantity = po.FormatPreferencePrimaryQuantity;
                    order.FormatPreferencesSecondary = po.FormatPreferenceSecondary;
                    order.FormatPreferencesSecondaryQuantity = po.FormatPreferenceSecondaryQuantity;
                    order.PO = po.POLineNumber;
                    order.StartDate = po.StartDate;
                    order.ShippingPreference = po.ShippingPreference;

                    dupCheckSeriesInfo.Orders.Add(order);
                }
            }

            return dupCheckSeriesInfo;
        }

        internal async Task<GridTemplateResult> GetGridTemplatesByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new BusinessException(103);

            return await OrderManager.Instance.GetGridTemplatesByUserId(userId);
        }

        
        #endregion
    }
}