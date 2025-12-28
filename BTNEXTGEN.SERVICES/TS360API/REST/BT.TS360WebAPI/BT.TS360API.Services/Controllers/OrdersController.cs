using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace BT.TS360API.Services.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly Services.OrdersService _orderActionService;

        public OrdersController()
        {
            this._orderActionService = new Services.OrdersService();
        }

        [HttpPost]
        [Route("appservice/Orders/SubmitILSOrder")]
        public AppServiceResult<bool> SubmitILSOrder(ILSOrderRequest request)
        {
            return _orderActionService.SubmitILSOrder(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/ValidateILSDetail")]
        public AppServiceResult<ILSValidationRequest> ValidateILSDetail(ILSValidationRequest request)
        {
            if (request.ILSType == ILSVendorType.Polaris)
            {
                return _orderActionService.ValidatePolarisILSDetail(request);
            }
            else
            {
                return _orderActionService.ValidateILSDetail(request);
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/SaveILSDetail")]
        public AppServiceResult<ILSValidationRequest> SaveILSDetail(ILSValidationRequest request)
        {
            return _orderActionService.SaveILSDetail(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetIlsDetail")]
        public AppServiceResult<ILSValidationRequest> GetIlsDetail(ILSValidationRequest request)
        {
            return _orderActionService.GetILSDetail(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetCartsAndHoldingDuplicates")]
        public async Task<AppServiceResult<CartsDupCheckResponse>> GetCartsAndHoldingDuplicates(CartsDupCheckRequest request)
        {
            var result = new AppServiceResult<CartsDupCheckResponse>();
            result.Status = AppServiceStatus.Success;

            try
            {
                result.Data = await _orderActionService.GetCartsAndHoldingDuplicates(request);
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetProductsDupCheck");
            }

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("Orders/GetOrdersDuplicates")]
        //[Route("appservice/searchservice/GetOrdersDuplicates")]
        public async Task<AppServiceResult<OrdersDupCheckResponse>> GetOrdersDuplicates(OrdersDupCheckRequest request)
        {
            var result = new AppServiceResult<OrdersDupCheckResponse>();
            result.Status = AppServiceStatus.Success;

            try
            {
                result.Data = await _orderActionService.GetOrdersDuplicates(request);
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetOrdersDuplicates");
            }

            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetOrderedDownloadedUser")]
        public AppServiceResult<CartOrderedDownloadedUser> GetOrderedDownloadedUser(CartOrderedDownloadedUser request)
        {
            return _orderActionService.GetOrderedDownloadedUser(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetIlsVendorCodes")]
        public AppServiceResult<VendorCodesRequest> GetIlsVendorCodes(VendorCodesRequest request)
        {
            return _orderActionService.GetIlsVendorCodes(request);
        }


        [HttpPost]
        [Route("appservice/searchservice/GetIlsCodes")]
        public AppServiceResult<VendorCodesRequest> GetIlsBranchCodes(VendorCodesRequest request)
        {
            return _orderActionService.GetIlsCodes(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/AddIlsVendorCodes")]
        public AppServiceResult<VendorCodesRequest> AddIlsVendorCodes(VendorCodeAddRequest request)
        {
            return _orderActionService.AddIlsVendorCodes(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/DeleteIlsVendorCodes")]
        public AppServiceResult<bool> DeleteIlsVendorCodes(VendorCodesDeleteRequest request)
        {
            return _orderActionService.DeleteIlsVendorCodes(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/AddIlsCodes")]
        public AppServiceResult<VendorCodesRequest> AddIlsCodes(VendorCodeAddRequest request)
        {
            return _orderActionService.AddIlsCodes(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/DeleteIlsCodes")]
        public AppServiceResult<bool> DeleteIlsCodes(VendorCodesDeleteRequest request)
        {
            return _orderActionService.DeleteIlsCodes(request);
        }


    }
}
