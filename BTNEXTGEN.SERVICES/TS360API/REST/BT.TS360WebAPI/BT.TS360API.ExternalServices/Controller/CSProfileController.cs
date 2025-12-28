using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.ExternalServices.DataAccess;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Profiles;

namespace BT.TS360API.ExternalServices.Controller
{
    public class CSProfileController
    {
        private static volatile CSProfileController _instance;
        private static readonly object SyncRoot = new Object();

        private CSProfileController()
        { // prevent init object outside
        }

        public static CSProfileController Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CSProfileController();
                }

                return _instance;
            }
        }

        public List<Account> GetAccounts(List<string> accountIds)
        {
            var ds = CSProfileDAO.Instance.GetAccounts(accountIds);

            List<Account> accounts = ConvertDataSetToAccount(ds);

            return accounts;
        }

        public List<UserReviewType> GetReviewTypes(List<string> reviewTypeIds)
        {
            var ds = CSProfileDAO.Instance.GetUserReviewTypes(reviewTypeIds);

            var reviewTypes = ConvertDataSetToReviewTypes(ds);

            return reviewTypes;
        }

        private List<UserReviewType> ConvertDataSetToReviewTypes(DataSet ds)
        {
            var results = new List<UserReviewType>();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return results;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var row = ds.Tables[0].Rows[i];

                UserReviewType rt = ConvertDataRowToUserReviewType(row);
                results.Add(rt);
            }

            return results;
        }

        private UserReviewType ConvertDataRowToUserReviewType(DataRow row)
        {
            var rt = new UserReviewType
            {
                Ordinal = ExtSvcDataAccessHelper.ConvertToString(row["u_ordinal"]),
                ReviewTypeId = ExtSvcDataAccessHelper.ConvertToString(row["u_userReview_id"]),
                ReviewType = ExtSvcDataAccessHelper.ConvertToString(row["u_review_type_id"])
            };

            return rt;
        }

        private List<Account> ConvertDataSetToAccount(DataSet ds)
        {
            var listAccs = new List<Account>();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return listAccs;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var row = ds.Tables[0].Rows[i];

                Account account = ConvertDataRowToAccount(row);
                listAccs.Add(account);
            }

            return listAccs;
        }

        private Account ConvertDataRowToAccount(DataRow row)
        {
            var account = new Account(ExtSvcDataAccessHelper.ConvertToString(row["u_bt_account_id"]));

            account.IsTOLAS = ExtSvcDataAccessHelper.ConvertToBoolean(row["b_is_TOLAS"]);
            account.Account8Id = ExtSvcDataAccessHelper.ConvertToString(row["u_account8_id"]);
            account.ProductType = ExtSvcDataAccessHelper.ConvertToString(row["u_product_type"]);
            account.AccountType = ExtSvcDataAccessHelper.ConvertToString(row["u_account_type"]);
            account.AccountNumber = ExtSvcDataAccessHelper.ConvertToString(row["u_erp_account_number"]);
            account.DisabledReasonCode = ExtSvcDataAccessHelper.ConvertToString(row["u_disable_reason_code"]);
            account.PrimaryWarehouseCode = ExtSvcDataAccessHelper.ConvertToString(row["u_primary_warehouse"]);
            account.SecondaryWarehouseCode = ExtSvcDataAccessHelper.ConvertToString(row["u_secondary_warehouse"]);
            account.EMarketType = ExtSvcDataAccessHelper.ConvertToString(row["u_esupplier_market_type"]);
            account.ETier = ExtSvcDataAccessHelper.ConvertToString(row["u_esupplier_market_tier"]);
            account.SOPPricePlanId = ExtSvcDataAccessHelper.ConvertToString(row["u_sop_price_plan_list"]);
            account.IsBillingAccount = ExtSvcDataAccessHelper.ConvertToBoolean(row["b_is_billing_account"]);
            account.HomeDeliveryAccount = ExtSvcDataAccessHelper.ConvertToBoolean(row["b_is_home_delivery"]);
            account.ESupplier = ExtSvcDataAccessHelper.ConvertToString(row["u_esupplier"]);
            account.CheckLEReserve = ExtSvcDataAccessHelper.ConvertToBoolean(row["b_check_le_reserve"]);
            account.AccountInventoryType = ExtSvcDataAccessHelper.ConvertToString(row["u_inventory_type"]);
            account.InventoryReserveNumber = ExtSvcDataAccessHelper.ConvertToString(row["u_reserve_inventory_number"]);
            account.BillToAccountNumber = ExtSvcDataAccessHelper.ConvertToString(row["u_bill_to_account_number"]);

            account.NumberOfBuilding = ExtSvcDataAccessHelper.ConvertToInt(row["BuildingCount"]);

            account.ProcessingCharge = ExtSvcDataAccessHelper.ConvertToDecimal(row["ProcessingCharges"]);
            account.ProcessingCharges2 = ExtSvcDataAccessHelper.ConvertToDecimal(row["ProcessingCharges2"]);
            account.ProcessingCharges3 = ExtSvcDataAccessHelper.ConvertToDecimal(row["ProcessingCharges3"]);
            account.SalesTax = ExtSvcDataAccessHelper.ConvertToFloat(row["SalesTax"]);

            return account;
        }
    }
}
