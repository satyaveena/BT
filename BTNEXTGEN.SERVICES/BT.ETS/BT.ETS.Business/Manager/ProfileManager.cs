using BT.ETS.Business.DAO;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Manager
{
    public class ProfileManager
    {
        #region Private Member

        private static volatile ProfileManager _instance;
        private static readonly object SyncRoot = new Object();
        public static ProfileManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProfileManager();
                }

                return _instance;
            }
        }

        #endregion

        public async Task<List<string>> GetUserAccounts(string userId)
        {
            var result = await ProfileDAO.Instance.GetUserAccounts(userId);
            return HandleUserAccountsResult(result);
        }

        private List<string> HandleUserAccountsResult(DataSet dsInput)
        {
            List<string> result = new List<string>();

            if (dsInput == null || dsInput.Tables == null || dsInput.Tables.Count == 0)
                throw new Exception("HandleUserAccountsResult returns NULL data."); ;

            foreach (DataRow row in dsInput.Tables[0].Rows)
            {
                result.Add(CommonHelper.SqlDataConvertTo<string>(row, "u_erp_account_number"));
            }

            return result;
        }
    }
}
