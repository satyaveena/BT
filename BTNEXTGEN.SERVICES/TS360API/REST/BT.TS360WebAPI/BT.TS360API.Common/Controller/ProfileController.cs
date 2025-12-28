using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Cache;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ExternalServices;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360Constants;

namespace BT.TS360API.Common.Controller
{
    public class ProfileController
    {
        private static volatile ProfileController _instance;
        private static readonly object SyncRoot = new Object();

        public static ProfileController Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProfileController();
                }

                return _instance;
            }
        }

        public Account GetAccountById(string accountId, bool primaryWarehouseNeeded = false, bool secondaryWarehouseNeeded = false)
        {
            if (string.IsNullOrEmpty(accountId)) return null;

            var cacheKey = string.Format(DistributedCacheKey.ProfileServiceAccountCacheKey, accountId);

            var account = CachingController.Instance.Read(cacheKey) as Account;

            if (account != null)
            {
                if (primaryWarehouseNeeded == false || (primaryWarehouseNeeded && account.PrimaryWarehouse != null))
                    return account;
            }

            account = ProfileService.Instance.GetAccountById(accountId);

            if (account != null)
            {
                //Site Terms name
                account.ProductTypeName = SiteTermHelper.Instance.GetSiteTermName(SiteTermName.AccountType, account.ProductType);

                // Get DisabledReason SiteTerm
                account.DisabledReasonText = SiteTermHelper.Instance.GetSiteTermName(SiteTermName.DisableReasonCode, account.DisabledReasonCode);
                
                if (!string.IsNullOrEmpty(account.BillToAccountNumber))
                {
                    if (account.AccountType == AccountType.Book.ToString())
                    {
                        var billToAccount = ProfileService.Instance.GetAccountById(account.BillToAccountNumber);

                        if (billToAccount != null)
                        {
                            account.ESupplier = billToAccount.ESupplier;
                            account.EMarketType = billToAccount.EMarketType;
                            account.ETier = billToAccount.ETier;
                        }
                    }
                }

                //Warehouse
                if (primaryWarehouseNeeded && !string.IsNullOrWhiteSpace(account.PrimaryWarehouseCode))
                {
                    var warehouse = ProfileService.Instance.GetWarehouseById(account.PrimaryWarehouseCode);
                    account.PrimaryWarehouse = warehouse;
                    account.PrimaryWarehouseName = warehouse.Code;
                }

                if (secondaryWarehouseNeeded && !string.IsNullOrWhiteSpace(account.SecondaryWarehouseCode))
                {
                    var warehouse = ProfileService.Instance.GetWarehouseById(account.SecondaryWarehouseCode);
                    account.SecondaryWarehouse = warehouse;
                    account.SecondaryWarehouseName = warehouse.Code;
                }
            }

            CachingController.Instance.Write(cacheKey, account, 5);
            return account;
        }
    }
}
