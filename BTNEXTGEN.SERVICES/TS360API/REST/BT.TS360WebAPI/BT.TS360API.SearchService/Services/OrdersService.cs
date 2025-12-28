using BT.TS360API.Common.Constants;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helper;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using BT.TS360SP;

namespace BT.TS360API.SearchService.Services
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
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSVendor, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSVendor));

            var val = OrdersDAOManager.Instance.ProcessILSOrder(request);
            if (val)
                result = new AppServiceResult<bool>() { Data = val, Status = AppServiceStatus.Success };
            else
                result = new AppServiceResult<bool>() { Data = val, Status = AppServiceStatus.Fail, ErrorMessage = "Unexpected Error Occured." };
            return result;
        }
        #endregion

        #region ValidateILSDetail
        public AppServiceResult<string> ValidateILSDetail(ILSValidationRequest request)
        {
            AppServiceStatus status = AppServiceStatus.Fail;
            AppServiceResult<string> result = null;
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSBaseAddress, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSBaseAddress));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSOrderValidatePath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSOrderValidatePath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSSubmitOrderPath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSSubmitOrderPath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSAuthorizePath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSAuthorizePath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSTokenPath, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSTokenPath));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSGetLogApiUrl, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSGetLogApiUrl));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSInsertLogApiUrl, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSInsertLogApiUrl));
            GlobalConfigurationHelper.Instance.Add(GlobalConfigurationKey.ILSVendor, GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.ILSVendor));

            string message = OrdersDAOManager.Instance.ValidateILSDetail(request);
            status = (message == OrderResources.ILS_ValidationSucess) ? AppServiceStatus.Success : AppServiceStatus.Fail;
            result = new AppServiceResult<string>() { Data = message, Status = status, ErrorMessage = "" };
            return result;
        }
        #endregion
    }
}