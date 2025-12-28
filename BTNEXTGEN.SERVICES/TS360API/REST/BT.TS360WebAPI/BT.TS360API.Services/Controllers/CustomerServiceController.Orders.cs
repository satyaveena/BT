using BT.TS360API.Cache;
using BT.TS360API.Logging;
using BT.TS360API.MongoDB.Contracts;
using BT.TS360API.MongoDB.DataAccess;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Order;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BT.TS360API.Services.Controllers
{
    public partial class CustomerServiceController : ApiController
    {

        [HttpPost]
        [Route("orderHistory/SearchLines")]
        public async Task<AppServiceResult<OrderSearchLinesResponseResult>> SearchLines(OrderSearchLinesRequest request)
        {
            var result = new AppServiceResult<OrderSearchLinesResponseResult>();

            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Request is required";
                }
                else if (request.AccountNumbers == null || !request.AccountNumbers.Any())
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Account Number is required";
                }
                else
                {
                    result.Data = await _customerServiceService.GetSearchLines(request);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "SearchLines");
            }
            return result;
        }

        [HttpPost]
        [Route("orderHistory/SearchOrders")]
        public async Task<AppServiceResult<SearchOrdersResult>> GetSearchOrders(OrderSearchLinesRequest request)
        {
            var result = new AppServiceResult<SearchOrdersResult>();

            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Request is required";
                }
                else
                {
                    result.Data = await _customerServiceService.GetSearchOrders(request);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetSearchOrders");
            }
            return result;
        }

        [HttpPost]
        [Route("orderHistory/LinesOrOrdersSearchCount")]
        public async Task<AppServiceResult<long>> GetLinesOrOrdersSearchCount(OrderSearchLinesRequest request)
        {
            var result = new AppServiceResult<long>();

            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Request is required";
                }
                else
                {
                    result.Data = await _customerServiceService.GetLinesOrOrdersSearchCount(request);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetSearchOrders");
            }
            return result;
        }
     
        [HttpPost]
        [Route("orderHistory/SearchSummary")]
        public async Task<AppServiceResult<OrderSearchSummaryResponse>> OrderSearchSummary(OrderLineRequest request)
        {

            AppServiceResult<OrderSearchSummaryResponse> result = new AppServiceResult<OrderSearchSummaryResponse>();
            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Request is required";
                }
                else if (request.AccountNumbers == null || !request.AccountNumbers.Any())
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Account Number is required";
                }
                else
                {
                    result.Data = await _customerServiceService.GetOrderSearchSummary(request);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "OrderSearchSummary");
            }

            return result;
        }

        [HttpGet]
        [Route("orderHistory/lineStatus")]
        public async Task<AppServiceResult<LineStatusResponse>> GetLineStatus(string orderLineId)
        {
            AppServiceResult<LineStatusResponse> result = new AppServiceResult<LineStatusResponse>();
            try
            {
                if (string.IsNullOrEmpty(orderLineId))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "OrderLineId is required";
                }
                else
                {
                    result.Data = await _customerServiceService.GetLineStatus(orderLineId);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetLineStatus");
            }

            return result;
        }

        [HttpPost]
        [Route("orderHistory/SearchLineFacets")]
        public async Task<AppServiceResult<SearchLineFacetsResponse>> SearchLineFacets(OrderLineRequest request)
        {
            AppServiceResult<SearchLineFacetsResponse> result = new AppServiceResult<SearchLineFacetsResponse>();
            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Request is required";
                }
                else if (request.AccountNumbers == null || !request.AccountNumbers.Any())
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Account Number is required";
                }
                else
                {
                    result.Data = await _customerServiceService.SearchLineFacets(request);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "SearchLineFacets");
            }

            return result;
        }

        [HttpPost]
        [Route("orderHistory/SearchOrderFacets")]
        public async Task<AppServiceResult<SearchOrderFacetsResponse>> SearchOrderFacets(OrderLineRequest request)
        {
            AppServiceResult<SearchOrderFacetsResponse> result = new AppServiceResult<SearchOrderFacetsResponse>();
            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Request is required";
                }
                else if (request.AccountNumbers == null || !request.AccountNumbers.Any())
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Account Number is required";
                }
                else
                {
                    result.Data = await _customerServiceService.SearchOrderFacets(request);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "SearchOrderFacets");
            }
            return result;
        }

        [HttpPost]
        [Route("orderHistory/SearchLineExport")]
        public async Task<AppServiceResult<OrderSearchExportResponse>> SearchLineExport(OrderSearchLinesRequest request)
        {
            AppServiceResult<OrderSearchExportResponse> result = new AppServiceResult<OrderSearchExportResponse>();
            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Request is required";
                }
                else if (request.AccountNumbers == null || !request.AccountNumbers.Any())
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Account Number is required";
                }
                else
                {
                    request.PageNumber = 1;
                    request.PageSize = -1; //retrieve and export all data.
                    result.Data = await _customerServiceService.SearchLineExport(request);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "SearchLineExport");
            }
            return result;
        }

        [HttpPost]
        [Route("orderHistory/SearchOrderExport")]
        public async Task<AppServiceResult<OrderSearchExportResponse>> SearchOrderExport(OrderSearchLinesRequest request)
        {
            AppServiceResult<OrderSearchExportResponse> result = new AppServiceResult<OrderSearchExportResponse>();
            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Request is required";
                }
                else if (request.AccountNumbers == null || !request.AccountNumbers.Any())
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Account Number is required";
                }
                else
                {
                    request.PageNumber = 1;
                    request.PageSize = -1; //retrieve and export all data.
                    result.Data = await _customerServiceService.SearchOrderExport(request);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "SearchOrderExport");
            }
            return result;
        }

        [HttpPost]
        [Route("orderHistory/SearchLineExportBackground")]
        public async Task<AppServiceResult<OrderSearchExportResponse>> SearchLineExportBackground(OrderSearchLinesRequest request)
        {
            AppServiceResult<OrderSearchExportResponse> result = new AppServiceResult<OrderSearchExportResponse>();
            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Request is required";
                }
                else if (request.AccountNumbers == null || !request.AccountNumbers.Any())
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Account Number is required";
                }
                else
                {
                    var batchExportbackgroundQueueItem = CreateBackgroundQueueItemForOrderExport(request, "SearchLineExport");
                    await CommonDAO.Instance.InsertBackgroundQueueItem(batchExportbackgroundQueueItem);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "SearchLineExportBackground");
            }
            return result;
        }

        [HttpPost]
        [Route("orderHistory/SearchOrderExportBackground")]
        public async Task<AppServiceResult<OrderSearchExportResponse>> SearchOrderExportBackground(OrderSearchLinesRequest request)
        {
            AppServiceResult<OrderSearchExportResponse> result = new AppServiceResult<OrderSearchExportResponse>();
            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Request is required";
                }
                else if (request.AccountNumbers == null || !request.AccountNumbers.Any())
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "Account Number is required";
                }
                else
                {
                    var batchExportbackgroundQueueItem = CreateBackgroundQueueItemForOrderExport(request, "SearchOrderExport");
                    await CommonDAO.Instance.InsertBackgroundQueueItem(batchExportbackgroundQueueItem);
                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "SearchOrderExportBackground");
            }
            return result;
        }

        private BackgroundQueue CreateBackgroundQueueItemForOrderExport(OrderSearchLinesRequest request, string reportType)
        {
            var now = DateTime.Now;
            var backgroundQueueItem = new BackgroundQueue();
             
            var footprintInformation = new FootprintInformation();
            footprintInformation.CreatedBy = request.UserName;
            footprintInformation.CreatedByUserID = request.UserId;
            footprintInformation.CreatedDate = now;
            footprintInformation.UpdatedBy = request.UserName;
            footprintInformation.UpdatedByUserID = request.UserId;
            footprintInformation.UpdatedDate = now;

            backgroundQueueItem.Priority = BT.TS360Constants.AppSettings.OrderSearchExportBackgroundQueuePriority;
            backgroundQueueItem.InProcessState = "New";
            backgroundQueueItem.JobType = "OrderSearchExport";
            backgroundQueueItem.FootprintInformation = footprintInformation;
            backgroundQueueItem.ReportSettings = new ReportSettings
            {
                ReportType = reportType,
                UserID = request.UserId,
                UserName = request.UserName,
                OrderSearchLinesRequestDetails = request,
            };

            return backgroundQueueItem;
        }

        [HttpGet]
        [Route("orderHistory/GetShippingStatus/{ShipTrackingNumber}")]
        public async Task<AppServiceResult<UPSResponse>> GetShippingStatus(string ShipTrackingNumber)
        {
            var result = new AppServiceResult<UPSResponse>();
            result.Status = AppServiceStatus.Success;

            object cacheResult = null;

            try
            {
                cacheResult = MemoryCacherHelper.GetValue(ShipTrackingNumber);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, "GetShippingStatus");
            }

            if (cacheResult != null)
            {
                result.Data = (UPSResponse)cacheResult;

                return result;
            }
            else
            {
                try
                {
                    result.Data = await _customerServiceService.GetShippingStatus(ShipTrackingNumber);
                    if (result.Data != null)
                    {
                        result.Data.ShipTrackingNumber = ShipTrackingNumber;
                        MemoryCacherHelper.Add(ShipTrackingNumber, result.Data, (TimeSpan.FromHours(24) - DateTime.Now.TimeOfDay).TotalSeconds);
                    }
                }
                catch (Exception ex)
                {
                    if (result.Data != null)
                    {
                        Logger.WriteLog(ex, "GetShippingStatus");
                        return result;
                    }
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                    Logger.WriteLog(ex, "GetShippingStatus");
                }

                return result;
            }
        }

        [HttpPost]
        [Route("orderHistory/GetMultipleShippingStatus")]
        public async Task<AppServiceResult<List<UPSResponse>>> GetShippingStatus(OrderShippingStatusRequest request)
        {
            var result = new AppServiceResult<List<UPSResponse>>();
            result.Status = AppServiceStatus.Success;
            result.Data = new List<UPSResponse>();

            if (request != null && request.ShipTrackingNumbers != null && request.ShipTrackingNumbers.Any())
            {
                foreach (var shipTrackingNumber in request.ShipTrackingNumbers)
                {
                    var cacheResult = MemoryCacherHelper.GetValue(shipTrackingNumber);

                    if (cacheResult != null)
                    {
                        result.Data.Add((UPSResponse)cacheResult);

                    }
                    else
                    {
                        try
                        {
                            var upsResponse = await _customerServiceService.GetShippingStatus(shipTrackingNumber);
                            if (upsResponse != null)
                            {
                                upsResponse.ShipTrackingNumber = shipTrackingNumber;
                                result.Data.Add(upsResponse);

                                MemoryCacherHelper.Add(shipTrackingNumber, upsResponse, (TimeSpan.FromHours(24) - DateTime.Now.TimeOfDay).TotalSeconds);
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Status = AppServiceStatus.Fail;
                            result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                            Logger.WriteLog(ex, "GetShippingStatus");
                        }
                    }
                }
            }
            return result;

        }

    }
}