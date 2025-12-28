using BT.TS360API.Common.Business;
using BT.TS360API.Common.Constants;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helper;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using BT.TS360SP;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BT.TS360API.MongoDB;

namespace BT.TS360API.Services.Services
{
    public class OrdersService
    {
        #region SubmitTS360ILSOrder
        public AppServiceResult<bool> SubmitILSOrder(ILSOrderRequest request)
        {
            AppServiceResult<bool> result = null;
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSBaseAddress, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSBaseAddress));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSOrderValidatePath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSOrderValidatePath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSSubmitOrderPath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSSubmitOrderPath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSAuthorizePath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSAuthorizePath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSTokenPath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSTokenPath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSGetLogApiUrl, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSGetLogApiUrl));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSInsertLogApiUrl, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSInsertLogApiUrl));

            var val = OrdersDAOManager.Instance.ProcessILSOrder(request);
            if (val)
                result = new AppServiceResult<bool>() { Data = val, Status = AppServiceStatus.Success };
            else
                result = new AppServiceResult<bool>() { Data = val, Status = AppServiceStatus.Fail, ErrorMessage = "Unexpected Error Occured." };
            return result;
        }
        #endregion

        #region ValidateILSDetail
        public AppServiceResult<ILSValidationRequest> ValidateILSDetail(ILSValidationRequest request)
        {
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSBaseAddress, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSBaseAddress));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSOrderValidatePath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSOrderValidatePath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSSubmitOrderPath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSSubmitOrderPath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSAuthorizePath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSAuthorizePath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSTokenPath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSTokenPath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSGetLogApiUrl, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSGetLogApiUrl));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSInsertLogApiUrl, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSInsertLogApiUrl));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSVendor, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSVendor));

            string message = OrdersDAOManager.Instance.ValidateILSDetail(request);
            AppServiceStatus status = AppServiceStatus.Success;
            if (message == OrderResources.ILS_ValidationSucess)
            {
                request.ILSValidationStatusId = (int)ILSValidationStatus.ValidationSuccessful;
            }
            else
            {
                request.ILSValidationStatusId = (int)ILSValidationStatus.ValidationFailed;
                status = AppServiceStatus.Fail;
            }
            request.ILSValidationDateTime = DateTime.Now;
            request.ILSValidationErrorMessage = message;
            if (!ProfileDAOManager.Instance.SaveILSDetail(request))
            {
                status = AppServiceStatus.Fail;
                message = OrderResources.ILS_SaveUnexpectedError;
                request.ILSValidationStatusId = 0;
            }
            return new AppServiceResult<ILSValidationRequest>() { Data = request, Status = status, ErrorMessage = message };
        }

        public AppServiceResult<ILSValidationRequest> ValidatePolarisILSDetail(ILSValidationRequest request)
        {
            var error = OrdersDAOManager.Instance.ValidatePolarisILSDetail(request);
            string message = error.ErrorMessage;
            AppServiceStatus status = AppServiceStatus.Success;
            if (string.IsNullOrEmpty(message))
            {
                request.ILSValidationStatusId = (int)ILSValidationStatus.ValidationSuccessful;
                request.ILSValidationErrorMessage = OrderResources.ILS_ValidationSucess;
            }
            else
            {
                request.ILSValidationStatusId = (int)ILSValidationStatus.ValidationFailed;
                status = AppServiceStatus.Fail;
                request.ILSValidationErrorMessage = message;
            }
            request.ILSValidationDateTime = DateTime.Now;

            if (!ProfileDAOManager.Instance.SaveILSDetail(request))
            {
                status = AppServiceStatus.Fail;
                message = OrderResources.ILS_SaveUnexpectedError;
                request.ILSValidationStatusId = 0;

                error.PAPIErrorCode = "-1";
            }
            return new AppServiceResult<ILSValidationRequest>() { Data = request, Status = status, ErrorMessage = message, ErrorCode = error.PAPIErrorCode };
        }
        #endregion

        public AppServiceResult<ILSValidationRequest> SaveILSDetail(ILSValidationRequest request)
        {
            AppServiceStatus status = AppServiceStatus.Fail;
            AppServiceResult<ILSValidationRequest> result = null;

            string message = "";
            request.ILSValidationStatusId = (int)ILSValidationStatus.ValidationPending;
            request.ILSValidationDateTime = DateTime.Now;
            request.ILSValidationStatus = "Validation Pending";
            if (ProfileDAOManager.Instance.SaveILSDetail(request))
            {
                status = AppServiceStatus.Success;
                message = "Data has been saved successfully";
            }
            else
            {
                status = AppServiceStatus.Fail;
                message = OrderResources.ILS_SaveUnexpectedError;
            }

            result = new AppServiceResult<ILSValidationRequest>() { Data = request, Status = status, ErrorMessage = message };
            return result;
        }

        public AppServiceResult<ILSValidationRequest> GetILSDetail(ILSValidationRequest request)
        {
            AppServiceStatus status = AppServiceStatus.Fail;
            string message = "";
            ILSValidationRequest orgIls = ProfileDAOManager.Instance.GetILSConfiguration(request.TSOrgId);
            if (orgIls != null)
            {
                status = AppServiceStatus.Success;
                message = "Data has been saved successfully";
            }
            else
            {
                status = AppServiceStatus.Fail;
                message = OrderResources.ILS_SaveUnexpectedError;
            }

            AppServiceResult<ILSValidationRequest> result = new AppServiceResult<ILSValidationRequest>();
            result.Data = orgIls;
            result.Status = status;
            result.ErrorMessage = message;
            return result;
        }

        public AppServiceResult<VendorCodesRequest> GetIlsVendorCodes(VendorCodesRequest request)
        {
            AppServiceResult<VendorCodesRequest> result = new AppServiceResult<VendorCodesRequest>();
            try
            { 
                AppServiceStatus status = AppServiceStatus.Fail;
                string message = "";
                int lastResultset = 0;
                List<VendorCode> listCode = ProfileDAO.Instance.GetIlsVendorCodes(request.OrganizationId, request.SearchKeyword, request.PageIndex, request.PageSize, request.SortDirection, out lastResultset);

                if (listCode != null)
                {
                    status = AppServiceStatus.Success;
                    message = "Data has been loaded successfully";
                }
                else
                {
                    status = AppServiceStatus.Fail;
                    message = OrderResources.ILS_SaveUnexpectedError;
                }
                VendorCodesRequest data = new VendorCodesRequest();
                data.VendorCodes = listCode;
                data.LastResultset = lastResultset;

            
                result.Data = data;
                result.Status = status;
                result.ErrorMessage = message;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Order);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public AppServiceResult<VendorCodesRequest> GetIlsCodes(VendorCodesRequest request)
        {
            AppServiceResult<VendorCodesRequest> result = new AppServiceResult<VendorCodesRequest>();
            try
            {
                AppServiceStatus status = AppServiceStatus.Fail;
                string message = "";
                int lastResultset = 0;
                List<VendorCode> listCode = ProfileDAO.Instance.GetIlsCodes(request.OrganizationId, request.SearchKeyword, request.PageIndex, request.PageSize, request.SortDirection, request.OrderingType, request.VendorID, out lastResultset);

                if (listCode != null)
                {
                    status = AppServiceStatus.Success;
                    message = "Data has been loaded successfully";
                }
                else
                {
                    status = AppServiceStatus.Fail;
                    message = OrderResources.ILS_SaveUnexpectedError;
                }
                VendorCodesRequest data = new VendorCodesRequest();
                data.VendorCodes = listCode;
                data.LastResultset = lastResultset;


                result.Data = data;
                result.Status = status;
                result.ErrorMessage = message;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Order);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public AppServiceResult<VendorCodesRequest> AddIlsVendorCodes(VendorCodeAddRequest request)
        {
            AppServiceResult<VendorCodesRequest> result = new AppServiceResult<VendorCodesRequest>();
            try
            {
                List<VendorCode> listCode = ProfileDAOManager.Instance.AddIlsVendorCodes(request);
                VendorCodesRequest data = new VendorCodesRequest();
                data.VendorCodes = listCode;
                data.IsImport = request.IsImport;

                result.Data = data;
                result.Status = AppServiceStatus.Success; 
                result.ErrorMessage = "Data has been saved successfully";
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Order);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public AppServiceResult<VendorCodesRequest> AddIlsCodes(VendorCodeAddRequest request)
        {
            AppServiceResult<VendorCodesRequest> result = new AppServiceResult<VendorCodesRequest>();
            try
            {
                List<VendorCode> listCode = ProfileDAOManager.Instance.AddIlsCodes(request);
                VendorCodesRequest data = new VendorCodesRequest();
                data.VendorCodes = listCode;
                data.IsImport = request.IsImport;

                result.Data = data;
                result.Status = AppServiceStatus.Success;
                result.ErrorMessage = "Data has been saved successfully";
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Order);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public AppServiceResult<bool> DeleteIlsVendorCodes(VendorCodesDeleteRequest request)
        {
            var result = new AppServiceResult<bool>();

            try
            {
                ProfileDAO.Instance.DeleteIlsVendorCodes(request.IdList);
                result.Status = AppServiceStatus.Success;
                result.Data = true;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Order);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                result.Data = false;
            }
            return result;
        }

        public AppServiceResult<bool> DeleteIlsCodes(VendorCodesDeleteRequest request)
        {
            var result = new AppServiceResult<bool>();

            try
            {
                ProfileDAO.Instance.DeleteIlsCodes(request.IdList);
                result.Status = AppServiceStatus.Success;
                result.Data = true;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Order);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                result.Data = false;
            }
            return result;
        }
        
        public async Task<CartsDupCheckResponse> GetCartsAndHoldingDuplicates(CartsDupCheckRequest request)
        {
            var response = new CartsDupCheckResponse();
            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.UserId) && request.BTKeys != null)
                {
                    //if (!string.Equals(request.CartCheckType, "none", StringComparison.OrdinalIgnoreCase))
                    //{
                        // check carts duplicates
                        var cartDuplicates = await CartDAOManager.Instance.CheckForCartDuplicates(request.OrgId, request.UserId, request.BasketId,
                                                                                request.BTKeys, request.BTEKeys, request.CartCheckType,
                                                                                request.DownloadCheckType);
                        // check Holding Duplicates
                        if (cartDuplicates != null)
                        {
                            var dicDuplicateHoldings = await CartDAOManager.Instance.CheckForHoldingsDuplicates(request.OrgId, request.UserId, request.BTKeys);
                            if (dicDuplicateHoldings != null)
                            {
                                // update results for Holding dupe
                                foreach (var dupItem in cartDuplicates)
                                {
                                    if (dicDuplicateHoldings.ContainsKey(dupItem.BTKey))
                                    {
                                        dupItem.IsDupHolding = dicDuplicateHoldings[dupItem.BTKey];
                                    }
                                }
                            }
                        }

                        // result
                        response.DuplicateItems = cartDuplicates;
                    //}
                }
            }

            return response;
        }

        public async Task<OrdersDupCheckResponse> GetOrdersDuplicates(OrdersDupCheckRequest request)
        {
            var response = new OrdersDupCheckResponse();
            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.UserId) && request.BTKeys != null)
                {
                    //if (!string.Equals(request.OrderCheckType, "none", StringComparison.OrdinalIgnoreCase))
                    //{
                        if (request.UserAccounts == null && request.OrderCheckType == DefaultDuplicateOrders.MyAccounts.ToString())
                        {
                            request.UserAccounts = ProfileDAOManager.Instance.GetUserAccounts(request.UserId);
                        }

                        var dicOrderDuplicates = MongoOrdersDAOManager.GetOrderDuplicates(request);

                        if (dicOrderDuplicates != null)
                        {
                            var dupItems = new List<DuplicateItem>();
                            foreach (var item in dicOrderDuplicates)
                            {
                                dupItems.Add(new DuplicateItem { BTKey = item.Key, IsDuplicated = item.Value });
                            }

                            response.DuplicateItems = dupItems;
                        }
                    //}
                }
            }

            return response;
        }

        public AppServiceResult<CartOrderedDownloadedUser> GetOrderedDownloadedUser(CartOrderedDownloadedUser request)
        {
            var result = new AppServiceResult<CartOrderedDownloadedUser>();
            try
            {
                string userInfor = CartDAOManager.GetOrderedDownloadedUser(request.CartId);
                result.Status = AppServiceStatus.Success;
                result.Data = new CartOrderedDownloadedUser() { CartId = request.CartId, OrderedDownloadedUser = userInfor };
            }
            catch (Exception exception)
            {
                Logger.RaiseException(exception, ExceptionCategory.Order);
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = "Can not retrieve Ordered/Downloaded user information. " + exception.Message;
            }
            return result;
        }

    }
}
