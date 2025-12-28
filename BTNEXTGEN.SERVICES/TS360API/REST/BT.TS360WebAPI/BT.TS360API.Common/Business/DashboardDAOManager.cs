using BT.TS360API.Common.Helpers;
using BT.TS360API.ServiceContracts;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace BT.TS360API.Common.DataAccess
{
    public class DashboardDAOManager
    {
        private static volatile DashboardDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        public static DashboardDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new DashboardDAOManager();
                }

                return _instance;
            }
        }

        // TODO: create a new SP to set default dashboard
        public async Task SetDefaultDashboard(string userId, string dashboardId)
        {
            // get existing dashboard
            var dashboard = await GetUserDashboard(userId, dashboardId);
            if (dashboard != null && dashboard.Accounts != null && !dashboard.IsDefault)
            {
                var newDefaultDashboard = new DashboardInfo
                {
                    UserId = userId,
                    DashboardId = dashboardId,
                    AccountType = dashboard.AccountType,
                    AccountIds = dashboard.Accounts.Select(r => r.Id).ToList(),
                    Name = dashboard.Name,
                    IsDefault = true
                };

                // save to set default
                await DashboardDAO.Instance.SaveDashboard(newDefaultDashboard);
            }
        }

        public async Task<DashboardInfoResponse> SaveDashboard(DashboardInfo request)
        {
            return await DashboardDAO.Instance.SaveDashboard(request);
        }

        public async Task<bool> DeleteDashboard(string userId, string dashboardId)
        {
            return await DashboardDAO.Instance.DeleteDashboard(userId, dashboardId);
        }

        public async Task<DashboardInfoResponse> GetUserDashboard(string userId, string dashboardId)
        {
            var ds = await DashboardDAO.Instance.GetUserDashboard(userId, dashboardId);

            if (ds == null ||
                ds.Tables.Count == 0 ||
                ds.Tables[0].Rows.Count == 0)
                return null;

            var result = new DashboardInfoResponse();
            result.Accounts = new List<AccountInfoForCustomerDashboard>();

            result.AccountType = DataAccessHelper.ConvertToString(ds.Tables[0].Rows[0]["DashboardType"]);
            result.Name = DataAccessHelper.ConvertToString(ds.Tables[0].Rows[0]["DashboardName"]);
            result.IsDefault = DataAccessHelper.ConvertToBool(ds.Tables[0].Rows[0]["IsDefault"]);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var account = new AccountInfoForCustomerDashboard
                {
                    Id = DataAccessHelper.ConvertToString(row["BTAccountID"]),
                    Name = DataAccessHelper.ConvertToString(row["AccountName"]),
                    Number = DataAccessHelper.ConvertToString(row["AccountNumber"]),
                    Alias = DataAccessHelper.ConvertToString(row["AccountAlias"])
                };
                
                result.Accounts.Add(account);
            }

            return result;
        }

        public List<Dashboard> FindUserDashboards(DashboardSearchRequest request)
        {
            return DashboardDAO.Instance.FindUserDashboards(request);
        }

        public async Task<CreateDefaultDashboardResponse> CreateDefaultDashboards(string userId)
        {
            var dashboardCreateResponse = await DashboardDAO.Instance.CreateDefaultDashboards(userId);
            var result = new CreateDefaultDashboardResponse();
            if (dashboardCreateResponse != null && !string.IsNullOrEmpty(dashboardCreateResponse.ErrorMessage))
            {
                result.ErrorMessage = dashboardCreateResponse.ErrorMessage;
                return result;
            }
            if (dashboardCreateResponse == null || dashboardCreateResponse.DataSet == null ||
                dashboardCreateResponse.DataSet.Tables.Count == 0 ||
                dashboardCreateResponse.DataSet.Tables[0].Rows.Count == 0)
                return null;

            result.Accounts = new List<AccountInfoForCustomerDashboard>();

            result.AccountType = DataAccessHelper.ConvertToString(dashboardCreateResponse.DataSet.Tables[0].Rows[0]["DashboardType"]);
            result.Name = DataAccessHelper.ConvertToString(dashboardCreateResponse.DataSet.Tables[0].Rows[0]["DashboardName"]);
            result.IsDefault = DataAccessHelper.ConvertToBool(dashboardCreateResponse.DataSet.Tables[0].Rows[0]["IsDefault"]);
            result.DashboardId = DataAccessHelper.ConvertToString(dashboardCreateResponse.DataSet.Tables[0].Rows[0]["DashboardID"]);
            foreach (DataRow row in dashboardCreateResponse.DataSet.Tables[0].Rows)
            {
                var account = new AccountInfoForCustomerDashboard
                {
                    Id = DataAccessHelper.ConvertToString(row["BTAccountID"]),
                    Name = DataAccessHelper.ConvertToString(row["AccountName"]),
                    Number = DataAccessHelper.ConvertToString(row["AccountNumber"]),
                    Alias = DataAccessHelper.ConvertToString(row["AccountAlias"])
                };

                result.Accounts.Add(account);
            }

            return result;
        }
    }
}
