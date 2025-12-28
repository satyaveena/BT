using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Business
{
    public class AccountDAOManager
    {
        private static volatile AccountDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        public static AccountDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new AccountDAOManager();
                }

                return _instance;
            }
        }

        public async Task<List<BasketAccountType>> GetUserAccountTypes(string userId)
        {
            var result = await AccountDAO.Instance.GetUserAccountTypes(userId);
            return GetAccountTypesFromDataSet(result);
        }

        public async Task<List<CartAccount>> GetAccountsByAccountType(string userId, string accountType, int pageIndex, int pageSize)
        {
            DataSet result = await AccountDAO.Instance.GetAccountsByAccountType(userId, accountType, pageIndex, pageSize);
            return GetAccountsFromDataSet(result);
        }

        public async Task<List<CartAccount>> GetAccountsByDashboardId(int dashboardId)
        {
            DataSet result = await AccountDAO.Instance.GetAccountsByDashboardId(dashboardId);
            return GetAccountsFromDataSet(result);
        }

        private List<BasketAccountType> GetAccountTypesFromDataSet(DataSet ds)
        {
            if (ds == null ||
                ds.Tables.Count < 1 ||
                ds.Tables[0].Rows.Count == 0)
                return null;

            var list = new List<BasketAccountType>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var accountType = new BasketAccountType
                {
                    Code = DataAccessHelper.ConvertToString(row["Code"]),
                    AccountType = DataAccessHelper.ConvertToString(row["AccountType"])
                };

                list.Add(accountType);
            }
            return list;
        }

        private List<CartAccount> GetAccountsFromDataSet(DataSet ds)
        {
            if (ds == null ||
                ds.Tables.Count < 1 ||
                ds.Tables[0].Rows.Count == 0)
                return null;

            var list = new List<CartAccount>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var accounts = new CartAccount();
                accounts.AccountID = DataAccessHelper.ConvertToString(row["BTAccountID"]);
                accounts.AccountName = DataAccessHelper.ConvertToString(row["AccountName"]);
                accounts.AccountAlias = DataAccessHelper.ConvertToString(row["AccountAlias"]);
                accounts.AccountERPNumber = DataAccessHelper.ConvertToString(row["u_erp_account_number"]); 
                //accounts.AccountTypeName = DataAccessHelper.ConvertToString(row["AccountType"]);

                list.Add(accounts);
            }
            return list;
        }

    }
}
