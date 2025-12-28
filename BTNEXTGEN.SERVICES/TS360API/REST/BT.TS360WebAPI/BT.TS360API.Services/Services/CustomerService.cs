using BT.TS360API.Common.Business;
using BT.TS360API.Logging;
using BT.TS360API.MongoDB;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Services.Services
{
    public partial class CustomerService
    {
        public async Task<List<BasketAccountType>> GetUserAccountTypes(string userId)
        {
            return await AccountDAOManager.Instance.GetUserAccountTypes(userId);
        }

        public async Task<List<CartAccount>> GetAccountsByAccountType(string userId, string accountType, int pageIndex, int pageSize)
        {
            return await AccountDAOManager.Instance.GetAccountsByAccountType(userId, accountType, pageIndex, pageSize);
        }

        public async Task<List<CartAccount>> GetAccountsByDashboardId(int dashboardId)
        {
            return await AccountDAOManager.Instance.GetAccountsByDashboardId(dashboardId);
        }
    }
}