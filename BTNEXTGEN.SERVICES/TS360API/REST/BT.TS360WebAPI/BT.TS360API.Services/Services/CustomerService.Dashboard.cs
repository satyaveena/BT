using BT.TS360API.Common.Business;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.MongoDB;
using BT.TS360API.MongoDB.Contracts;
using BT.TS360API.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Services.Services
{
    public partial class CustomerService
    {
        internal async Task<List<RecentOrder>> GetRecentOrders(List<string> accountNumbers, int maxResultCount)
        {
            var results = new List<RecentOrder>();

            if (accountNumbers != null && accountNumbers.Count > 0 && maxResultCount > 0)
                results = await OrderLinesDAOManager.GetRecentOrders(accountNumbers, maxResultCount);

            foreach (var order in results)
            {
                // convert code to text
                order.OrderStatus = CommonHelper.ConvertOrderStatusCodeToText(order.OrderStatus);
            }
            return results;
        }

        internal async Task<OrderHistoryStatusResponse> OrderHistoryViewStatus(OrderHistoryStatusRequest request)
        {
            return await OrderLinesDAOManager.OrderHistoryViewStatus(request);
        }

        internal async Task<List<OrderHistoryStatusInfo>> OrderHistoryShowMonthly(OrderHistoryShowMonthlyRequest request)
        {
            return await OrderLinesDAOManager.OrderHistoryShowMonthly(request);
        }

        internal async Task<DashboardInfoResponse> SaveDashboard(SaveDashboardRequest request)
        {
            var daoManager = DashboardDAOManager.Instance;
            var response = await daoManager.SaveDashboard(request);

            if (!string.IsNullOrWhiteSpace(request.NewDefaultDashboardId))
            {
                // set new default dashboard
                await daoManager.SetDefaultDashboard(request.UserId, request.NewDefaultDashboardId);
            }

            return response;
        }

        internal async Task<bool> DeleteDashboard(string userId, string dashboardId)
        {
            return await DashboardDAOManager.Instance.DeleteDashboard(userId, dashboardId);
        }

        internal async Task<DashboardInfoResponse> GetUserDashboard(string userId, string dashboardId)
        {
            return await DashboardDAOManager.Instance.GetUserDashboard(userId, dashboardId);
        }

        internal async Task<CreateDefaultDashboardResponse> CreateDefaultDashboards(string userId)
        {
            return await DashboardDAOManager.Instance.CreateDefaultDashboards(userId);
        }

        /// <summary>
        /// checks if user has only one dashboard.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal bool HasOnlyDashboard(string userId)
        {
            var hasOnlyDashboard = false;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                // find if user has 2 dashboards max.
                var request = new DashboardSearchRequest
                {
                    UserId = userId,
                    Keyword = string.Empty,
                    PageIndex = 1,
                    PageSize = 2
                };

                var dashboardList = DashboardDAOManager.Instance.FindUserDashboards(request);
                if (dashboardList != null && dashboardList.Count == 1)
                    hasOnlyDashboard = true;
            }

            return hasOnlyDashboard;
        }

        internal List<Dashboard> FindUserDashboards(DashboardSearchRequest request)
        {
            return DashboardDAOManager.Instance.FindUserDashboards(request);
        }

        //internal async Task<bool> AddAccountToDashboard(AccountsToDashboardRequest request)
        //{
        //    return await DashboardDAOManager.Instance.SaveDashboard(request);
        //}
    }
}
