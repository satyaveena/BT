using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Constants
{
    public static class StoredProcedureName
    {
        #region Public Member

        public const string PROC_GET_ILS_QUEUE_PENDING_ORDER_COUNT = "procTS360ILSQueueGetCount";

        public const string PROC_GET_ILS_QUEUE_PENDING_ORDERS = "procTS360ILSQueueGetOrder";

        public const string PROC_GET_ILS_CONFIGURATION = "procTS360GetIlsConfiguration";

        public const string PROC_SET_ILS_BASKET_STATE = "procTS360SetilsBasketState";

        public const string PROC_SET_ILS_SYSTEM_STATUS = "procTS360SetILSSystemStatus";

        public const string PROC_RESET_ILS_CART = "procTS360ReSetILSCart";

        public const string PROC_GET_ALERT_MESSAGE_TEMPLATE = "procGetAlertMessageTemplate";
        public const string PROC_INSERT_ALERT_USER_MESSAGE = "procInsertAlertUserMessage";

        public const string PROC_GET_ILS_ORG_USER_PROFILE = "procTS360GetILSOrgUserProfile";
        public const string PROC_TS360_GET_BTKEYS_BY_LINEITEMS = "procTS360GetBTKeysByBasketLineItems";
        public const string PROC_TS360_GET_BASKET_STORE_CUSTOMER_VIEW = "procTS360GetBasketStoreAndCustomerView";

        public const string PROC_MARC_GET_PROFILES = "procMARCGetProfiles";

        public const string PROC_BASKET_MANAGEMENT_GET_BASKET_BYID = "procTS360GetBasketByID";
       
        public const string PROC_TS360_SUBMIT_BASKET = "procTS360SubmitBasket";
        public const string PROC_TS360_SET_ILS_BASKET_LINES_GRIDS = "procTS360ILSSetBasketLinesGrids";
        public const string PROC_TS360_GENERATE_NEW_BASKETNAME = "procTS360GenerateNewBasketName";
        public const string PROC_BASKET_MANAGEMENT_CREATE_BASKET = "procTS360CreateBasket";
        public const string PROC_TS360_MOVE_LINE_ITEMS_TO_NEW_CART = "procTS360AddMoveCopyLineItems";
        public const string PROC_PRICING_SET_BASKET_ROLLUP_NUMBERS = "procPricingSetBasketRollupNumbers";
       

        #endregion

        
    }
}
