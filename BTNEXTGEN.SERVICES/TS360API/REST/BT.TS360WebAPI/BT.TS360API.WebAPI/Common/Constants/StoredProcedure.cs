using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Common.Constants
{
    public class StoredProcedure
    {
        public const string UPDATE_LOG_REQUEST = "procTS360APILogRequests";

        public const string UPDATE_CART_RANK = "procTS360APIPostCartRanked";

        public const string GET_ESP_STATE = "procTS360APIGetESPState";

        public const string GET_ALERT_TEMPATE = "procGetAlertMessageTemplate";

        public const string GET_USER_DETAILS_BY_TOKEN = "procTS360APIGetUserByCIPUserToken";

        public const string UPDATE_CART_DIST = "procTS360APIPostCartDistributed";
		
		public const string GET_USER_PROFILE_BY_NAME = "procTS360OAuthGetUserByName";

        public static string ProcGetSymmetricCryptoKey = @"procGetSymmetricCryptoKey";
        public static string ProcGetSymmetricCryptoKeyByBucket = @"procGetSymmetricCryptoKeyByBucket";
        public static string ProcInsertClientAuthorization = @"procInsertClientAuthorization";
        public static string ProcInsertNonce = @"procInsertNonce";
        public static string ProcInsertSymmetricCryptoKey = @"procInsertSymmetricCryptoKey";
        public static string ProcDeleteSymmetricCryptoKey = @"procDeleteSymmetricCryptoKey";
        public static string ProcGetScope = @"procGetScope";
        public static string ProcGetClient = @"procGetClient";
        public static string ProcInsertUser = @"procInsertUser";
        public static string ProcGetClientAuthorization = @"procGetClientAuthorization";

        // ESP Release 4.2
        
        // LD1
        public const string SET_BASKET_SUMMARY_ESP_STATE = "procTS360SetBasketSummaryESPState";
        public const string GET_RANK_REQUESTS = "procTS360APIESPGetRankRequests";

        public const string GET_DISTRIBUTION_REQUESTS = "procTS360APIESPGetDistributionRequests";

        public const string ESP_SET_STATE = "procTS360APISetESPState";

        
    }
}