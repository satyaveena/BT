namespace BT.TS360Constants
{
    public static class StoredProcedureName
    {
        public const string ProcTs360QuickSearchGetActiveCarts = "procTS360QuickSearchGetActiveCarts";
        public const string ProcTs360QuickSearchGetPrimaryBasket = "procTS360QuickSearchGetPrimaryBasket";
        public const string ProcTs360QuickSearchAddProductToCart = "procTS360QuickSearchAddProductToCart";
        #region Grid Related Stored Procedures

        public static string PROC_GET_GRID_CODES = "procTS360GetGridCodes";
        public static string PROC_SET_GRID_CODE = "procTS360SetGridCode";
        public static string PROC_DELETE_GRID_CODE = "procTS360DeleteGridCode";

        public static string PROC_GET_USER_GRID_CODES = "procTS360GetUserGridCodes";
        public static string PROC_SET_USER_GRID_CODE = "procTS360SetUserGridCode";
        public static string PROC_DELETE_USER_GRID_CODE = "procTS360DeleteUserGridCode";

        //public static string PROC_GET_GRID_FIELDS = "procTS360GetGridFields";
        public static string PROC_SET_GRID_FIELD = "procTS360SetGridField";
        public static string PROC_DELETE_GRID_FIELD = "procTS360DeleteGridField";
        public static string PROC_GET_ALL_GRID_FIELD_WITH_CODE = "procTS360GetGridFieldsWithAuthorizedGridCodeForUser";

        public static string PROC_GET_USER_GRID_FIELDS = "procTS360GetUserGridFields";
        public static string PROC_SET_USER_GRID_FIELD = "procTS360SetUserGridField";
        public static string PROC_DELETE_USER_GRID_FIELD = "procTS360DeleteUserGridField";

        public static string PROC_GET_GRID_TEMPLATES = "procTS360GetGridTemplates";
        public static string PROC_GET_SINGLE_GRID_TEMPLATE = "procTS360GetGridTemplate";
        public static string PROC_SET_GRID_TEMPLATE = "procTS360SetGridTemplate";
        public static string PROC_DELETE_GRID_TEMPLATE = "procTS360DeleteGridTemplate";
        public static string PROC_GET_GRID_TEMPLATE_OWNERS = "procTS360GetGridTemplateOwners";
        public static string PROC_GET_SOP_GRID_FIELD_LIST = "procTS360GetSOPGridFieldList";

        public static string PROC_GET_USER_GRID_TEMPLATES = "procTS360GetUserGridTemplates";
        public static string PROC_GET_GRID_TEMPLATES_BY_USER = "procTS360GetGridTemplatesByUser";
        public static string PROC_SET_USER_GRID_TEMPLATE = "procTS360SetUserGridTemplate";
        public static string PROC_DELETE_USER_GRID_TEMPLATE = "procTS360DeleteUserGridTemplate";
        public static string PROC_SET_USER_GRID_TEMPLATES = "procTS360SetUserGridTemplates";

        public static string PROC_GET_USER_DEFAULT_GRID_TEMPLATE = "procTS360GetUserObject";
        public static string PROC_SET_USER_DEFAULT_GRID_TEMPLATE = "procTS360SetUserObject";

        public static string PROC_GET_GRID_TEMPLATE_LINES = "procTS360GetGridTemplateLines";
        public static string PROC_GET_GRID_TEMPLATE_LINES_WITH_PAGING = "procTS360GetGridTemplateLinesWithPaging";
        public static string PROC_SET_GRID_TEMPLATE_LINE = "procTS360SetGridTemplateLine";
        public static string PROC_DELETE_GRID_TEMPLATE_LINE = "procTS360DeleteGridTemplateLine";

        public static string PROC_GET_GRID_FIELD_POSITION = "procTS360GetGridFieldPositionInGridTemplate";

        public static string PROC_COPY_GRID_TEMPLATE = "procTS360CopyGridTemplate";

        // PE
        public static string PROC_GET_ALL_USER_GRID_CODES = "procTS360GetAllUserGridCodes";
        public static string PROC_GET_ALL_USER_GRID_FIELDS = "procTS360GetAllUserGridFields";

        public static string PROC_GET_USER_GRID_TEMPLATES_PAGING = "procTS360GetUserGridTemplatesWithPaging";
        public static string PROC_UPDATE_USER_GRID_TEMPLATES = "procTS360UpdateUserGridTemplates";
        public static string ProcTs360SaveSlipReportForGridField = "procTS360SaveSlipReportForGridField";
        public static string ProcTs360SaveGridFieldGridCode = "procTS360SaveGridFieldGridCode";
        public static string ProcTs360SaveGridTemplate = "procTS360SaveGridTemplate";
        public static string ProcTs360SaveUserGridFieldsCodes = "procTS360SetUserGridFields";
        public static string ProcTS360UpdateUserGridCodes = "procTS360UpdateUserGridCodes";

        public static string ProcTs360ClearDefaultSettingOfGridTemplate = "procTS360ClearDefaultSettingOfGridTemplate";
        public static string ProcTS360GetGridDistribution = "procTS360GetGridDistributionOption";
        public static string ProcTS360SaveGridDistribution = "procTS360SaveGridDistribution";

        #endregion

        #region Cart Grid Related Stored Procedures name

        public static string PROC_GET_CART_GRID_HEADERS = "procTS360GetBasketGridHeaders";
        public static string PROC_CREATE_BASKET_GRID_HEADER = "procTS360CreateBasketGridHeader";

        public static string PROC_GET_ERROR_LINE_ITEMS = "procTS360GetErrorLineItems";
        //public static string PROC_GET_CART_GRID_LINES = "procTS360GetBasketGridLines";
        public static string PROC_SET_CART_GRID_LINE = "procTS360SetBasketGridLine";
        public static string PROC_SET_ALL_CART_GRID_LINE = "procTS360SetAllBasketGridLine";
        public static string PROC_DELETE_CART_GRID_LINE = "procTS360DeleteBasketGridLine";
        //public static string PROC_FIND_GRID_FIELD_CODE_IN_CART = "procTS360FindGridFieldCodeInBasket";

        public static string PROC_GET_CART_LINE_ITEM_NOTE_QUANTITES = "procTS360GetBasketLineItemNoteQuantities";
        public static string PROC_SET_CART_LINE_ITEM_NOTE_QUANTITY = "procTS360SetBasketLineItemNoteQuantity";
        public static string PROC_DELETE_CART_LINE_ITEM_QUANTITY = "procTS360DeleteBasketLineItemNoteQuantity";
        public static string PROC_MODIFY_NOTE_TO_ALL_TITLES = "procTS360UpdateAndRemoveNoteToAllTitle";
        public static string PROC_SEARCH_WITHITN_BASKET_NOTE = "procTS360SearchWithinBasketByNote";
        public static string PROC_GET_NOTES_BY_BTKEYS = "procTS360GetNotesByBtkeys";
        public static string PROC_GET_USER_NOTES_BY_BTKEYS = "procTS360GetUserNoteByBTkey";

        public static string PROC_GET_DEFAULT_CART_GRID_TEMPLATE = "procTS360GetDefaultBasketGridTemplate";
        public static string PROC_HAS_DEFAULT_CART_GRID_TEMPLATE = "procTS360HasDefaultBasketGridTemplate";
        public static string PROC_SAVE_GRID_LINES_AS_GRID_TEMPLATE = "procTS360SaveGridLinesAsAGridTemplate";
        public static string PROC_CART_CHECK_DUPLICATE_DEFAULT_TEMPLATE = "procTS360CheckDuplicateDefaultTemplateForBasket";
        public static string PROC_PASTE_GRID_LINE_FROM_CLIPBOARD = "procTS360PasteGridLineFromClipboard360ToBasket";
        public static string PROC_COPY_CANCELLED_LINE_ITEMS = "procTS360CopyCancelledBasketLineItems";

        //public static string PROC_CHECK_DUPLICATE_GRID_LINE_CART = "procTS360CheckDuplicateGridLineInBasket";
        public static string PROC_COPY_BASKETS = "procTS360CopyBaskets";
        public static string PROC_CHECK_DUPLICATE_GRID_LINE_TEMPLATE_CART = "procTS360CheckDuplicateTemplateForBasket";

        public static string PROC_CREATE_EBOOK_BASKETS = "procTS360CreateeBookBasket";
        
        public static string PROC_APPLY_TEMPLATE_TO_TITLES = "procTS360ApplyTemplateToTitles";

        public static string PROC_REMOVE_GRID_FROM_LINE_ITEMS = "procTS360RemoveGridFromLineItems";

        public static string PROC_ADD_PRODUCT_TO_CART = "procTS360AddMoveCopyLineItems";
        public static string PROC_GET_GRID_FIELD_POSITION_IN_CART = "procTS360GetGridFieldPositionInBasket";
        public static string PROC_GET_GRID_FIELD_POSITION_IN_CART_LineItems = "procTS360GetGridFieldPositionInBasketForLineItems";
        public static string PROC_GET_CART_SUMMARY = "procTS360GetBasketByID";

        public static string PROC_GET_GRID_SUMMARY = "procTS360GetGridSummary";
        public static string PROC_GET_GRID_SUMMARY_DETAIL = "procTS360GetGridSummaryDetail";
        public static string PROC_REMOVE_GRID_FROM_CART = "procTS360RemoveGridFromBasket";

        public static string PROC_TRANSFER_CART_TO_USERS = "procTS360TransferBasketToUsers";
        public static string PROC_CHECK_EBOOK_ACCOUNT_INFO = "procTS360GetAccountInfoForESupplier";

        public static string PROC_DELETE_ZERO_QUANTITY = "procTS360DeleteBasketLineitemsWithZeroQuantity";

        public static string PROC_REPLACE_ALL_GRID_FIELD_CODE_IN_BASKET = "procTS360ReplaceAllGridFieldCodeInBasket";

        public static string PROC_VALIDATE_GRID_IN_CART_FOR_LIBRARY_SYSTEM_ACCOUNT = "procTS360ValidateGridInCartForLibrarySystemAccount";

        public static string PROC_UPDATE_GRID_FIELD_POSITION_IN_TEMPLATE_HEADER = "procTS360UpdateHeaderOrTemplate";

        //public static string PROC_GET_GRID_NOTE_COUNT_FOR_BTKEYS = "procTS360GetCountUserNotesQuantityByBTKeys";
        public static string PROC_GET_GRID_NOTE_COUNT_FOR_LINEITEM = "procTS360GetCountNotesAndGridLineByBLIs";

        public static string PROC_DELETE_DEFAULT_GRID_TEMPLATE = "procTS360DeleteDefaultTemplate";
        public static string PROC_SET_BLANK_GL_TO_CART = "procTS360ConvertSharedLineToGridLine";
        public static string PROC_SPLIT_CART_TO_GRID_AND_NONGRID = "procTS360SplitBasketToGridAndNG";
        public static string PROC_BASKET_MANAGEMENT_CHECK_CONTAIN_MIX = "procTS360CheckBasketContainsAMixOfGridNNonGrid";
        public static string PROC_GET_LENGTH_LONGEST_NOTE = "procTS360GetLengthOfLongestNote";
        public static string PROC_GET_ORDER_DOWNLOADED_USER = "procTS360GetOrderedDownloadedUser";
        public static string PROC_ADD_BATCH_TO_BACKGROUND_PROCESSING = "procTS360RequestBatchEntry";

        #endregion

        #region Shared Cart
        public static string ProcIsSharedCart = "procTS360IsSharedCart";
        public static string ProcIsPremiumSharedCart = "procTS360IsPremiumSharedCart";

        public static string ProcTs360SetBasketUserGroup = "procTS360SetBasketUserGroup";
        public static string ProcTs360DeleteBasketUserGroup = "procTS360DeleteBasketUserGroup";
        public static string ProcTs360GetBasketUserGroups = "procTS360GetBasketUserGroups";

        public static string ProcSetUserGroupMember = "procTS360SetBasketUserGroupMembers";
        public static string ProcDeleteUserGroupMember = "procTS360DeleteBasketUserGroupMember";
        public static string ProcTs360GetBasketUserGroupMembers = "procTS360GetBasketUserGroupMembers";

        public static string ProcSetOrgWorkflowStage = "procTS360SetOrgWorkflowStage";
        public static string ProcDeleteOrgWorkflowStage = "procTS360DeleteOrgWorkflowStage";
        public static string ProcGetOrganizationWorkflowStages = "procTS360GetOrganizationWorkflowStages";

        public static string ProcSetSharedCartMember = "procTS360SetBasketUser";
        public static string ProcDeleteSharedCartMember = "procTS360DeleteBasketUser";
        public static string ProcTs360GetSharedCartMembers = "procTS360GetBasketUsers";

        public static string ProcSetMemberWorkflowStage = "procTS360SetBasketUserWorkflowStage";
        public static string ProcDeleteMemberWorkflowStage = "procTS360DeleteBasketUserWorkflowStage";
        public static string ProcTs360GetSharedCartMemberWorkflowStages = "procTS360GetBasketUserWorkflowStages";

        public static string ProcSetSharedCartWorkflow = "procTS360SetBasketWorkflows";
        public static string ProcDeleteSharedCartWorkflow = "procTS360DeleteBasketWorkflow";
        public static string ProcTs360GetSharedCartWorkflows = "procTS360GetBasketWorkflows";

        public static string ProcSetWorkflowStage = "procTS360SetBasketWorkflowStage";
        public static string ProcDeleteWorkflowStage = "procTS360DeleteBasketWorkflowStage";
        public static string ProcTs360GetCartWorkflowStages = "procTS360GetBasketWorkflowStages";

        public static string ProcShareCart = "procTS360SharedBasket";
        public static string ProcHasSharedCartProfile = "procTS360HasSharedBasketProfile";
        public static string ProcGetSharedCartSummary = "procTS360GetBasketSummary";
        public static string ProcGetSharedCartSummaryDetail = "procTS360GetBasketSummaryDetail";

        #endregion

        #region NEW GRID STORED PROCEDURES
        public static string PROC_GET_GRID_FIELD_GRID_CODE_BY_USER = "procTS360GetGridFieldsAndCodesByUser";
        public static string PROC_GET_GRID_FIELD_BY_USER = "procTS360GetGridFieldsByUser";
        public static string PROC_GET_BASKET_LINE_ITEM_GRIDS = "procTS360GetBasketLineItemGrids";
        public static string PROC_GET_DEFAULT_BASKET_GRID_TEMPLATE = "procTS360GetDefaultBasketGridTemplate";        
        #endregion

        public const string SPLIT_BASKET_TO_GRID_AND_NON_GRID = "procTS360SplitBasketToGridAndNonGrid";
        public const string PROC_SET_SHARED_BASKET_GRID_ENABLED = "procTS360SetBasketIsSharedBasketGridEnabled";

        #region Cart Folder StoreProcedures
        public const string PROC_BASKET_MANAGEMENT_CREATE_USER_FOLDER = "procTS360CreateUserFolder";
        public const string PROC_BASKET_MANAGEMENT_CREATE_USER_SYSTEM_FOLDERS = "procTS360CreateUserFolders";
        public const string PROC_BASKET_MANAGEMENT_GET_FOLDER_BY_USER_ID = "procTS360GetUserFolders";
        public const string PROC_BASKET_MANAGEMENT_GET_FOLDER_BYID = "procTS360GetUserFolder";
        public const string PROC_BASKET_MANAGEMENT_DELETE_USER_FOLDER = "procTS360DeleteUserFolder";
        public const string PROC_BASKET_MANAGEMENT_MOVE_USER_FOLDER = "procTS360MoveUserFolder";
        //public const string PROC_BASKET_MANAGEMENT_ARCHIVE_USER_FOLDER = "procTS360ArchiveUserFolder";
        public const string PROC_BASKET_MANAGEMENT_RENAME_USER_FOLDER = "procTS360RenameUserFolder";
        public const string PROC_BASKET_MANAGEMENT_UPDATE_USER_FOLDER_SEQUENCE = "procTS360ResequenceUserFolder";

        #endregion

        #region Customer Support Account
        public const string PROC_CUSTOMER_SUPPORT_USERDASHBOARD_GETORGACCOUNT_TYPES = "procCustomerSupportUserDashboardGetOrgAccountTypes";
        public const string PROC_CUSTOMER_SUPPORT_USERDASHBOARD_GETUSERACCOUNT_TYPES = "procCustomerSupportUserDashboardGetUserAccountTypes";
        public const string PROC_CUSTOMER_SUPPORT_USERDASHBOARD_GETACCOUNTS_BY_ACCOUNTTYPE = "procCustomerSupportUserDashboardGetAccountsByType";
        public const string PROC_CUSTOMER_SUPPORT_USERDASHBOARD_GETACCOUNTS_BY_DASHBOARD_ID = "procCustomerSupportUserDashboardGet";

        public const string PROC_CUSTOMER_SUPPORT_ACCOUNTS_REMOVE_USERS = "procCustomerSupportAccountsRemoveUsers";
        public const string PROC_CUSTOMER_SUPPORT_USER_REMOVE_ACCOUNTS = "procCustomerSupportUserRemoveAccounts";
        public const string PROC_CUSTOMER_SUPPORT_DISABLED_ACCOUNT_REMOVE = "procCustomerSupportDisabledAccountRemove";

        #endregion

        #region Customer Support Dashboard
        public const string PROC_CUSTOMER_SUPPORT_USER_DASHBOARD_SAVE = "procCustomerSupportUserDashboardSave";
        public const string PROC_CUSTOMER_SUPPORT_USER_DASHBOARD_DELETE = "procCustomerSupportUserDashboardDelete";
        public const string PROC_CUSTOMER_SUPPORT_USER_DASHBOARD_FINDDASHBOARDS = "procCustomerSupportUserDashboardGetList";
        public const string PROC_CUSTOMER_SUPPORT_USER_DASHBOARD_GET = "procCustomerSupportUserDashboardGet";
        public const string PROC_CUSTOMER_SUPPORT_USER_DASHBOARD_GET_DEFAULT = "procCustomerSupportUserDashboardGetDefault";
        #endregion


        #region Cart StoreProcedures
        public const string PROC_BASKET_MANAGEMENT_CREATE_BASKET = "procTS360CreateBasket";

        public const string PROC_BASKET_MANAGEMENT_GET_PRIMARY_BASKET = "procTS360GetPrimaryBasket";
        public const string PROC_BASKET_MANAGEMENT_GET_BASKET_BYID = "procTS360GetBasketByID";
        public const string PROC_BASKET_MANAGEMENT_GET_BASKET_BY_TOP_NEWEST = "procTS360GetBasketTopNewest";
        public const string PROC_BASKET_MANAGEMENT_GET_BASKET_BY_FOLDER = "procTS360GetBasketByFolder";
        public const string PROC_BASKET_MANAGEMENT_GET_BASKET_BY_FOLDER_LITE = "procTS360GetBasketByFolderLite";
        public const string PROC_BASKET_MANAGEMENT_GET_BASKET_SEARCH = "procTS360GetBasketsSearch";
        public const string PROC_BASKET_SEARCH_MANAGE_CART_PAGE = "procTS360GetBasketsSearchManageCarts";
        public const string PROC_BASKET_SEARCH_CART_LIST_PAGE = "procTS360GetBasketsSearchCartList";
        public const string ProcTs360GetBasketSummaryFlat = "procTS360GetBasketSummaryFlat";
        public const string ProcTs360GetBasketSummaryFlatAjax = "procTS360GetBasketSummaryFlat_AJAX";
        public const string ProcTs360GetBasketSummaryFlatCartView = "procTS360GetBasketSummaryFlatCartView";

        public const string PROC_BASKET_MANAGEMENT_SET_PRIMARY_BASKET = "procTS360SetPrimaryBasket";
        public const string PROC_BASKET_MANAGEMENT_SET_ONECLICKMARC_STATUS = "procTS360SetBasketOneClickMARC";

        public const string PROC_BASKET_MANAGEMENT_DELETE_BASKETS = "procTS360DeleteBaskets";
        public const string PROC_BASKET_MANAGEMENT_ARCHIVE_BASKETS = "procTS360ArchiveBaskets";
        public const string PROC_BASKET_MANAGEMENT_RESTORE_BASKETS = "procTS360RestoreBaskets";
        public const string PROC_BASKET_MANAGEMENT_COPY_BASKETS = "procTS360CopyBaskets";
        public const string PROC_BASKET_MANAGEMENT_MERGE_BASKETS = "procTS360MergeBaskets";
        public const string PROC_BASKET_MANAGEMENT_MOVE_BASKETS = "procTS360MoveBaskets";

        public const string PROC_BASKET_MANAGEMENT_DOWNLOAD_EXPORT_BASKET = "procTS360SetBasketStatusDownloaded";
        public const string PROC_BASKET_MANAGEMENT_GET_LINE_ITEMS_WITH_TITLE = "procTS360GetBasketLineItemsWithTitle";
        public const string PROC_BASKET_MANAGEMENT_GET_LINE_ITEMS_ID = "procTS360GetBasketLineItemIDByBasketSummary";
        public const string PROC_BASKET_MANAGEMENT_RENAME_BASKET = "procTS360RenameBasket";
        public const string PROC_BASKET_MANAGEMENT_GET_CART_FACET = "procTS360GetBasketFacets";
        public const string PROC_BASKET_MANAGEMENT_GET_CART_FACET_FOR_ADMIN = "procTS360GetBasketFacetsForAdmin";
        public const string ProcTs360GetBasketSummaryFlatFacets = "procTS360GetBasketSummaryFlatFacets";

        public const string PROC_BASKET_MANAGEMENT_GET_BASKET_BY_NAME = "procTS360GetBasketByName";
        public const string PROC_BASKET_MANAGEMENT_GET_USER_FOLDER_SEQUENCE = "procTS360GetUserFolderSequence";
        public const string PROC_BASKET_MANAGEMENT_UPDATE_BASKET_NOTE_ONLY = "procTS360UpdateBasketNote";

        public const string PROC_BASKET_MANAGEMENT_UPDATE_BASKET_INSTRUCTION = "procTS360UpdateBasketSpecialInstructions";
        public const string PROC_BASKET_MANAGEMENT_IS_PRICING = "procBasketPricingIsBeingProcessed";
        public const string PROC_BASKET_MANAGEMENT_IS_ZERO_QUANTITY_ITEM = "procTS360BasketContainsZeroQuantityItem";
        public const string PROC_BASKET_MANAGEMENT_GET_QUANTITY_BY_BTKEYS = "procTS360GetQuantitesByBtkeys";
        public const string PROC_BASKET_MANAGEMENT_CHECK_BASKET_FOR_TITLES = "procTS360CheckBasketForTitles";
        public const string PROC_BASKET_MANAGEMENT_CHECK_BASKET_FOR_CAL_MERCH = "procTS360CheckBasketforCalMerch";

        public const string PROC_TS360_ADD_ORIGINAL_ENTRY_TO_CART = "procTS360SaveOriginalEntry";

        public const string PROC_TS360_SET_MASS_QUANTITIES = "procTS360SetMassQuantities";

        public const string ProcTs360DragDropFolder = "procTS360DragDropFolder";

        public const string PROC_BASKET_MANAGEMENT_REQUEST_QUOTE = "procTS360SetBasketRequestQuotation";
        public const string PROC_BASKET_MANAGEMENT_RELEASE_QUOTE = "procTS360SetBasketReleaseQuotation";

        public const string PROC_GET_BASKET_BY_ID_WITH_OCS_DATA = "procTS360GetBasketByID_WithOCSData";
        public const string PROC_SET_ACTIVE_OR_INACTIVE_BASKET = "procTS360SetActiveOrInactiveBasket";
        public const string PROC_SET_ILS_SYSTEM_STATUS = "procTS360SetILSSystemStatus";
        public const string PROC_RESET_ILS_CART = "procTS360ReSetILSCart";
        public const string PROC_PRICING_SET_BASKET_ROLLUP_NUMBERS = "procPricingSetBasketRollupNumbers";
        #endregion

        #region Cart Line Store Procedures

        public const string PROC_BASKET_MANAGEMENT_BASKET_ADD_LINEITEMS = "procTS360BasketAddLineItems";
        public const string PROC_BASKET_MANAGEMENT_BASKET_COPY_LINEITEMS = "procTS360BasketCopyLines";
        public const string PROC_BASKET_MANAGEMENT_BASKET_COPY_TITLE = "procTS360CopyTitle";
        public const string PROC_BASKET_MANAGEMENT_BASKET_MOVE_TITLE = "procTS360MoveTitle";

        public const string PROC_TS360_BASKET_MOVE_LINES = "procTS360BasketMoveLines";
        public const string PROC_TS360_BASKET_REMOVE_LINES = "procTS360DeleteBasketLineItems";
        public const string PROC_TS360_BASKET_UPDATE_LINE_ITEM_NOTE = "procTS360UpdateBasketLineItemNotes";
        public const string PROC_TS360_BASKET_UPDATE_LINE_QUANTITIES = "procTS360MergeBasketLineItemQuantities";
        public const string PROC_TS360_BASKET_UPDATE_LINE_INFO = "procTS360UpdateBasketLineItemInfo";
        public const string PROC_TS360_BASKET_UPDATE_LINE_NOTES = "procTS360UpdateBasketUserLineItemNotes";

        public const string PROC_TS360_BASKET_UPDATE_BASKET_ACCOUNTS = "procTS360MergeBasketOrderForm";
        public const string PROC_TS360_BASKET_GET_ACCOUNT_SUMMARIES = "procTS360GetBasketOrderForm";
        public const string PROC_TS360_BASKET_GET_CART_LINE_ITEMS = "procTS360GetBasketLineItems";
        public const string PROC_TS360_BASKET_GET_BT_KEYS = "procTS360GetBasketBTKeys";
        public const string PROC_TS360_BASKET_GET_CART_LINE_FACET = "procTS360GetBasketLineItemFacet";

        public const string PROC_TS360_BASKET_MERGE_LINE_ITEMS = "procTS360AddMoveCopyLineItems";
        public const string PROC_TS360_BASKET_MERGE_EXCEED500_LINE_ITEMS = "procTS360AddItemsToCartTrackingList";

        public const string PROC_TS360_BASKET_GET_LINE_ITEM_BY_ID = "procTS360GetBasketLineItemByID";
        public const string PROC_TS360_BASKET_GET_LINE_ITEM_BY_BTKEY = "procTS360GetBasketLineItemByBTKey";
        public const string PROC_TS360_BASKET_GET_LINE_ITEM_BY_ID_LIST = "procTS360GetBasketLineItemByIDs";
        public const string PROC_TS360_GENERATE_NEW_BASKETNAME = "procTS360GenerateNewBasketName";
        public const string PROC_TS360_GENERATE_NEW_BASKETNAME_FOR_USER = "procTS360GenerateNewBasketNameForUser";
        public const string PROC_TS360_COPY_CANCELLED_LINE_ITEMS = "procTS360CopyCancelledBasketLineItems";

        public const string PROC_TS360_UPDATE_ITEM_ORDERING_INFO = "procTS360UpdateItemOrderingInfo";
        public const string PROC_TS360_BASKET_GET_CART_LINE_ITEMS_BY_NOTE = "procTS360SearchWithinBasketByNote";
        public const string PROC_TS360_BASKET_GET_CART_LINE_ITEMS_BY_GRID_FIELD_CODE = "procTS360GetBasketLineItemsByGridFieldCode";

        public const string ProcTs360CopyLineItemsToNewCart = "procTS360AddMoveCopyLineItems";
        public const string ProcTs360MoveLineItemsToNewCart = "procTS360AddMoveCopyLineItems";
        public const string PROC_TS360_GET_BASKETS_WITH_DUPLICATED_TITLES = "procTS360GetBasketsWithDuplicatedTitles";

        public const string PROC_TS360_GET_BASKETS_LINE_ITEMS_ESP_RANKING = "procTS360GetBasketLineItemESPRanking";
        public const string PROC_TS360_GET_ESP_INVALID_CODES = "procTS360GetESPInvalidCodes";
        public const string PROC_TS360_GET_CART_DETAILS_QUICK_VIEW = "procTS360GetCartDetailsQuickView";
        public const string PROC_TS360_GET_BASKET_TRANSFER_DETAILS = "procTS360GetBasketTransferDetails";
        public const string PROC_TS360_GET_BASKETS_BY_IDS = "procTS360GetBasketsByIDs";
        public const string PROC_TS360_SET_ESP_AUTO_RANK = "procTS360SetAutoRank";
        #endregion

        #region Submit Order Store Procedures
        public const string PROC_TS360_SUBMIT_BASKET = "procTS360SubmitBasket";
        public const string PROC_TS360_GET_BASKET_STORE_CUSTOMER_VIEW = "procTS360GetBasketStoreAndCustomerView";
        #endregion

        #region Check For Duplicates Procedures

        public const string PROC_TS360_CHECK_FOR_DUPLICATES = "procTS360CheckForDuplicates";
        public const string PROC_TS360_DUPLICATE_BASKET_REFERENCES = "procTS360DuplicateBasketReferences";
        public const string PROC_TS360_DUPLICATE_ORDER_REFERENCES = "procTS360DuplicateOrderReferences";
        public const string PROC_TS360_GET_ORDERED_LINE_ITEMS = "procTS360GetOrderedLineItems";
        public const string ProcTs360GetBasketLineItemOrderStatus = "procTS360GetBasketLineItemOrderStatus";
        public const string PROC_TS360_HOLDINGS_DUP_CHECK = "procTS360HoldingsDupCheck";
        public const string PROC_TS360_GET_USER_SHIP_TO_ACCOUNTS = "procTS360UserGetShipToAccounts";

        #endregion

        public const string ProcTs360Searchorders = "ProcTs360Searchorders";
        public const string ProcTs360ViewOrderQueue = "procTS360ViewOrderQueue";
        public const string ProcTs360SplitBasketByAccountType = "procTS360SplitBasketByAcctType";
        public const string ProcTs360GetCartManagementQuickView = "procTS360GetCartManagementQuickView";
        public const string ProcTs360GetCartManagementQuickView_Ajax = "procTS360GetCartManagementQuickView_AJAX";
        public const string ProcTS360GetItemDetailsQuickView = "procTS360GetItemDetailsQuickView";
        public const string ProcTS360SearchQuotations = "procTS360QuotationSearchHome";
        public const string ProcTS360GetCartManagementQuickViewSelectActions = "procTS360GetCartManagementQuickViewSelectActions";
        public const string ProcTS360GetCartManagementQuickViewSelectedCartActions = "procTS360GetCartManagementQuickViewSelectedCartActions";
        public const string ProcTs360ViewESPQueue = "procTS360GetESPQueue";

        #region Pricing
        public const string PROC_GET_BASKET_LINE_ITEM_UPDATED = "procPricingGetBasketLineItemPriceChanges";
        public const string PROC_SET_BASKET_LINE_ITEM_UPDATED = "procPricingSetBasketLineItemPriceChanges";
        public const string PROC_GET_SOP_PRICING_DISCOUNTS = "procPricingGetSOPPriceDiscounts";
        public const string PROC_GET_LIST_PRICE = "procPricingGetListPrice";
        public const string PROC_RESET_BASKET_REPRICING_INDICATOR = "procPricingResetBasketRepricingIndicator";
        public const string PROC_PRICING_REFLAG_BASKET_LINEITEMS = "procPricingReflagBasketLineItems";
        #endregion

        public const string ProcTS360CDMSListGetBTKeys = "procTS360CDMSListGetBTKeys";
        public const string ProcTs360CDMSListGetUsers = "procTS360CDMSListGetUsers";
        public const string ProcTS360CDMSListSearchAdditionalUsers = "procTS360CDMSListSearchAdditionalUsers";
        public const string procTS360CDMSListGetNameByID = "procTS360CDMSListGetNameByID";
        public const string ProcTS360CDMSListSend = "procTS360CDMSListSend";
        public const string ProcTS360CDMSListGetFeaturedList = "procTS360CDMSListGetFeaturedList";
        public const string ProcTS360CDMSListGet = "procTS360CDMSListGet";

        public const string PROC_TS360_SET_BASKET_STATE_ILS_ORDERED = "procTS360SetBasketStateILSOrdered";
        public const string PROC_TS360SET_ILS_BASKET_STATE = "procTS360SetilsBasketState";
        public const string PROC_TS360_SET_BASKET_LINES_ILS_COLUMNS = "procTS360SetBasketLinesILSColumns";
        public const string PROC_TS360_SAVE_ILS_CONFIGURATION = "procTS360SaveIlsConfiguration";
        public const string PROC_TS360_GET_ILS_CONFIGURATION = "procTS360GetIlsConfiguration";
        public const string PROC_TS360_GET_ILS_VENDORCODES = "procTS360IlsVendorCodesGet";
        public const string PROC_TS360_INSERT_ILS_VENDORCODES = "procTS360IlsVendorCodesInsert";
        public const string PROC_TS360_DELETE_ILS_VENDORCODES = "procTS360IlsVendorCodesDelete";
        public const string PROC_TS360_SET_BAKSET_ILS_VENDORCODE = "procTS360ILSVendorCodesSetBasketState";
        public const string PROC_TS360_GET_ILS_ORDERINGCODES = "procTS360ILSOrderingCodesGet";
        public const string PROC_TS360_SET_ILS_ORDERINGCODES = "procTS360ILSOrderingCodesSet";
        public const string PROC_TS360_DELETE_ILS_ORDERINGCODES = "procTS360ILSOrderingCodesDelete";
    }
    public class DistributedCacheKey
    {
        public static string GridFieldsCodesCacheKey = "__{0}_GridFieldsCodes"; // {0}: Org ID
        public static string GridTemplatesCacheKey = "__{0}_GridTemplates"; // {0}: Org ID
        public static string GridCodeUsersCacheKey = "__{0}_GridCodesUsers"; // {0}: Org ID
        public static string ProfileServiceOrgCacheKey = "__ApiPSOrgCK_{0}"; // {0}: Org ID
        public static string ProfileServiceUserCacheKey = "__ApiPSUserCK_{0}"; // {0}: UserID
        public static string ProfileServiceAccountCacheKey = "__ApiPSAccountCK_{0}"; // {0}: Account ID

        public static string ProfileIsHideProductImages = "ProfileIsHideProductImages";
        public static string ProfileIsHideProductImagesForQuickCart = "ProfileIsHideProductImagesForQuickCart";
        public static string IsQuickSearchEnabled = "__IsQuickSearchEnabled";
        public static string IsQuickCartDetailsEnabled = "IsQuickCartDetailsEnabled";
        public static string IsQuickCartsListEnabled = "IsQuickCartsListEnabled";
        public static string IsQuickItemDetailsEnabled = "IsQuickItemDetailsEnabled";

        public static string UserCountCacheKey = "___GetUserAlertCount_{0}"; // {0} User ID
        public const string CurrentUserInventoryType = "__CurrentInventoryType_UserId_{0}"; // {0}: User ID
    }

    public static class TableParameterType
    {
        public const string BasketLineItemUpdatedTable = "[dbo].[utblBasketLineItemUpdated]";
        public const string ListPriceTable = "[dbo].[utblListPrice]";
        public const string SopPriceDiscountTable = "[dbo].[utblprocPricingGetSOPPriceDiscounts]";
    }

    public static class MupoContants
    {
        public static readonly string MupoOption = "MUPO";
        public static readonly string MupoOptionMultiUser = "Multi-User";
        public static readonly string MupoOptionMultiUser1Year = "Multi-User 1 YR";
    }

    public class CMListName
    {
        public const string ItemDetailsConfigList = "ListItemDetailConfiguration";
        public const string ItemDetailsSectionList = "ListItemDetailSection";
        public const string ItemDetailsFieldList = "ListItemDetailField";
    }

    public sealed class MarketingConstants
    {
        public const int ReturnAdItems = 20;
        public const string AdIdKey = "item_id";
        public const string AdNameKey = "name";
        public const string StartDateKey = "date_start";
        public const string EndDateKey = "date_end";
        public const string PigCacheKey = "__ProductInterestGroupCacheKey__";
    }

    public static class SpecialProductAttributes
    {
        public const string PawPrintsProductLine = "PAWPR";
        public const string GardnersSupplierCode = "GRDRS";
        public const string LargePrintMerchCategory = "LARGEPRINT";
        public const string LargePrintEdition = "Large Print";
        public const string LargePrintAltEdition = "Larger Print";
    }
    public static class ServiceRequestHeader
    {
        public const string RequestUserId = "Request-UserId";
        public const string WfeServerName = "Request-WFE";
        public const string HOST = "Host";
    }

    public static class ProductLookupLinkConstant
    {
        public const string UPCDeactivated = "AV Deactivated";
        public const string UPCUseISBN = "Use ISBN";
        public const string OpenProductLookup = "OpenProductLookup";
    }

    public static class ILSVendorType
    {

        public const string Sierra = "Sierra";

        public const string Polaris = "Polaris";

    }
}
