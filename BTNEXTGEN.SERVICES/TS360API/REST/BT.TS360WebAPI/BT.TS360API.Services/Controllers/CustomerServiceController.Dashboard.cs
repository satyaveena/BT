using BT.TS360API.Common;
using BT.TS360API.Logging;
using BT.TS360API.MongoDB.Contracts;
using BT.TS360API.ServiceContracts;
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
        [Route("orderHistory/recent")]
        public async Task<AppServiceResult<RecentOrdersResponse>> GetRecentOrders(RecentOrdersRequest request)
        {
            var result = new AppServiceResult<RecentOrdersResponse>();

            try
            {
                var response = new RecentOrdersResponse();
                response.RecentOrders = await _customerServiceService.GetRecentOrders(request.AccountNumbers, request.Size);

                result.Status = AppServiceStatus.Success;
                result.Data = response;
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetRecentOrders");
            }

            return result;
        }

        [HttpPost]
        [Route("orderHistory/viewStatus")]
        public async Task<AppServiceResult<OrderHistoryStatusResponse>> OrderHistoryViewStatus(OrderHistoryStatusRequest request)
        {
            var result = new AppServiceResult<OrderHistoryStatusResponse>();

            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_REQUEST;
                }
                else if (request.AccountNumbers == null || request.AccountNumbers.Count == 0)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_ACCOUNT_NUMBER;
                }
                else if (string.IsNullOrEmpty(request.OrderDate))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_ORDER_DATE;
                }
                else
                {
                    result.Status = AppServiceStatus.Success;
                    result.Data = await _customerServiceService.OrderHistoryViewStatus(request);
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "OrderHistoryViewStatus");
            }

            return result;
        }

        [HttpPost]
        [Route("orderHistory/showMonthly")]
        public async Task<AppServiceResult<List<OrderHistoryStatusInfo>>> OrderHistoryShowMonthly(OrderHistoryShowMonthlyRequest request)
        {
            var result = new AppServiceResult<List<OrderHistoryStatusInfo>>();

            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_REQUEST;
                }
                else if (request.AccountNumbers == null || request.AccountNumbers.Count == 0)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_ACCOUNT_NUMBER;
                }
                else if (string.IsNullOrEmpty(request.OrderDate))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_ORDER_DATE;
                }
                else
                {
                    result.Status = AppServiceStatus.Success;
                    result.Data = await _customerServiceService.OrderHistoryShowMonthly(request);
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "OrderHistoryShowMonthly");
            }

            return result;
        }

        [HttpPost]
        [Route("customerSupport/dashboard/save")]
        public async Task<AppServiceResult<DashboardInfoResponse>> SaveDashboard(SaveDashboardRequest request)
        {
            var result = new AppServiceResult<DashboardInfoResponse>();

            try
            {
                if (request == null)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_REQUEST;
                }
                else if (string.IsNullOrEmpty(request.UserId))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_USER_ID;
                }
                else if (string.IsNullOrEmpty(request.Name))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_DASHBOARD_NAME;
                }
                else if (string.IsNullOrEmpty(request.AccountType))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_ACCOUNT_TYPE;
                }
                else if (request.AccountIds == null || request.AccountIds.Count == 0)
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_ACCOUNT_ID;
                }
                else
                {
                    result.Status = AppServiceStatus.Success;
                    result.Data = await _customerServiceService.SaveDashboard(request);
                }
            }
            catch (DAOException ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;

                if(!ex.IsBusinessError)
                    Logger.WriteLog(ex, "SaveDashboard");
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "SaveDashboard");
            }

            return result;
        }

        [HttpDelete]
        [Route("customerSupport/dashboard/delete/{UserId}/{DashboardId}/{selectedDashboardIdInDelete}")]
        public async Task<AppServiceResult<bool>> DeleteDashboard(string userId, string dashboardId, string selectedDashboardIdInDelete)
        {
            var result = new AppServiceResult<bool>();

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_USER_ID;
                }
                else if (string.IsNullOrEmpty(dashboardId))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_DASHBOARD_ID;
                }

                else
                {
                    result.Status = AppServiceStatus.Success;
                    result.Data = await _customerServiceService.DeleteDashboard(userId, dashboardId);
                    if (selectedDashboardIdInDelete != "null")
                    {
                        var dashboard = await _customerServiceService.GetUserDashboard(userId, selectedDashboardIdInDelete);
                        dashboard.IsDefault = true;
                        var dashboardRequest = new SaveDashboardRequest
                        {
                            AccountType = dashboard.AccountType,
                            DashboardId = selectedDashboardIdInDelete,
                            IsDefault = dashboard.IsDefault,
                            Name = dashboard.Name,
                            UserId = userId,
                        };
                        dashboardRequest.AccountIds = new List<string>();
                        dashboard.Accounts.ForEach(a =>
                        {
                            dashboardRequest.AccountIds.Add(a.Id);
                        });
                        await _customerServiceService.SaveDashboard(dashboardRequest);
                    }

                }
            }
            catch (DAOException ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;

                if(!ex.IsBusinessError)
                    Logger.WriteLog(ex, "DeleteDashboard");
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "DeleteDashboard");
            }

            return result;
        }

        [HttpGet]
        [Route("customerSupport/dashboard/{UserId}/{DashboardId}")]
        public async Task<AppServiceResult<DashboardInfoResponse>> GetUserDashboard(string userId, string dashboardId)
        {
            var result = new AppServiceResult<DashboardInfoResponse>();

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_USER_ID;
                }
                else
                {
                    result.Status = AppServiceStatus.Success;
                    result.Data = await _customerServiceService.GetUserDashboard(userId, dashboardId);
                }
            }
            catch (DAOException ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;

                if(!ex.IsBusinessError)
                    Logger.WriteLog(ex, "GetUserDashboard");
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetUserDashboard");
            }

            return result;
        }

        [HttpGet]
        [Route("customerSupport/createdefaultdashboards/{UserId}")]
        public async Task<AppServiceResult<CreateDefaultDashboardResponse>> CreateDefaultDashboards(string userId)
        {
            var result = new AppServiceResult<CreateDefaultDashboardResponse>();

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_USER_ID;
                }
                else
                {
                    result.Status = AppServiceStatus.Success;
                    result.Data = await _customerServiceService.CreateDefaultDashboards(userId);
                    if (result.Data != null)
                    {
                        if(!string.IsNullOrEmpty(result.Data.ErrorMessage))
                        {
                            if(result.Data.ErrorMessage == "NoAccountsFound")
                            {
                                result.ErrorCode = "1";
                                result.ErrorMessage = result.Data.ErrorMessage;
                            }
                            else
                            {
                                result.ErrorCode = "-1";
                                result.Status = AppServiceStatus.Fail;
                                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                            }
                        }
                    }
                    else
                    {
                        result.ErrorCode = "-1";
                        result.Status = AppServiceStatus.Fail;
                        result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                    }
                }
            }
            catch (DAOException ex)
            {
                result.ErrorCode = "-1";
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;

                if (!ex.IsBusinessError)
                    Logger.WriteLog(ex, "CreateDefaultDashboards");
            }
            catch (Exception ex)
            {
                result.ErrorCode = "-1";
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "CreateDefaultDashboards");
            }

            return result;
        }

        [HttpPost]
        [Route("customerSupport/dashboard/FindUserDashboards")]
        public AppServiceResult<DashboardResponse> FindUserDashboards(DashboardSearchRequest request)
        {
            var result = new AppServiceResult<DashboardResponse>();

            try
            {
                var response = new DashboardResponse();
                if (string.IsNullOrEmpty(request.UserId))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_USER_ID;
                }
                else
                {
                    result.Status = AppServiceStatus.Success;
                    response.Dashboards = _customerServiceService.FindUserDashboards(request);
                    result.Data = response;
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "FindUserDashboards");
            }

            return result;
        }

        [HttpGet]
        [Route("customerSupport/dashboard/hasonlyone/{userId}")]
        public AppServiceResult<Boolean> HasOnlyDashboard(string userId)
        {
            Services.CustomerService _customerServiceService = new Services.CustomerService();

             var result = new AppServiceResult<Boolean>();

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = CustomerServiceConstants.REQUIRE_USER_ID;
                }
                else
                {
                    result.Status = AppServiceStatus.Success;
                    result.Data = _customerServiceService.HasOnlyDashboard(userId);
                }
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "HasOnlyDashboard");
            }

            return result;

        }


    }
}
