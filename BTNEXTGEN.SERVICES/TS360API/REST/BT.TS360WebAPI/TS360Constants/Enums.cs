namespace BT.TS360Constants
{
    public enum GridDistributionOption
    {
        UseMyDefaultQty = 1,
        UseZeroQty = 2,
        UseGridtemplate = 3,
        UseGridDistribution = 4
    }
    public enum AdvancedSearchFilter
    {
        IssuedReviewJournal,
        IssuedReviewJournalOperator,
        NonIssuedReviewJournal,
        NonIssuedReviewJournalOperator,
        MinimumReviewsPerTitle,
        PositiveReviews,
        PositiveReviewsOperator,
        StarredReviews,
        StarredReviewsOperator,
        BtPrograms,
        BtProgramsOperator,
        AyPrograms,
    }
    public enum ProfileEntity
    {
        UserObject,
        Organization,
        BTUserAlert,
        BTSavedSearch,
        BTSiteBranding,
        BTProductInterestGroup,
        BTFee,
        BTShippingMethod,
        BTWarehouse,
        BTInvalidLoginAttempts,
        SalesRep,
        Address,
        BTAccount,
        BTProductLookup,
        BTSubscription,
        BTUserReviewType
    }

    public enum ExceptionCategory
    {
        Admin, Account, User, ContenManagement, Search, Order, General, Permission, Menu, Pricing, OriginalEntry, CreditCard,
        CartGrid, CartDetailsTrace, Marketing, SavedSearch,
        ItemDetails, BTListsPage, CDMS, Quote,
        Catalog,
        CommonControl,
        Profiles,
        SiteTerm,
        ILS,
        UserDashboard
    }

    public enum CartStatus
    {
        Open = 1,
        Ordered = 2,
        Downloaded = 3,
        Submitted = 5,
        Archived = 4,
        Deleted = -1,
        Quote_Submitted = 6,
        Quoted = 7,
        Quote_Transmitted = 8,
        Ordered_Quote = 9,
        Processing = 10,
        VIP_Submitted = 11,
        VIP_Ordered = 12
    }

    public enum ILSState
    {
        ILSNew = 1,
        ILSValidationInProcess = 2,
        ILSValidationCompleted = 3,
        ILSOrderValidationInProcess = 4,
        ILSOrderValidationCompleted = 5,
        ILSOrderInProcess = 6,
        ILSOrderCompleted = 7,
        ILSNone = 0
    }

    public enum AppServiceStatus
    {
        Success = 0,
        Fail = 1,
        LimitationFail = 2
    }

    public enum CartFolderType
    {
        RootFolderType = 0,
        OrderedFolderType = 1,
        BtcartFolderType = 2,
        ArchivedFolderType = 3,
        DeletedFolderType = 4,
        NormalFolderType = 5,
        SharedReceivedFolderType = 6,
        ESPFolderType = 7,
        DefaultFolderType = 8,
        ReceivedFolderType = 9,
        OrderAndHoldFolderType = 11,
    }

    public enum OrderStatus
    {
        Submitted = 0,
        Ordered = 1,
        All = 2
    }

    public enum OrderUnitStatus
    {
        Shipped = 0,
        Cancelled = 1,
        BackOrder = 2,
        OnSaleHold = 3,
        InReserve = 4,
        InProcess = 5
    }

    public enum AccountType
    {
        Book = 0,
        Entertainment = 1,
        BTDML = 4,
        EBRRY = 5,
        NETLB = 6,
        GALEE = 7,
        OE = -1,
        eBook = 2, // will be removed
        BookEntertainment = 10,
        VIP = 8,
        OneBox = 11
    }
    public enum ESPStateType
    {
        None = 0,
        Requested = 1,
        InProcess = 2,
        Submitted = 3,
        Failed = 4,
        Successful = 5,
        Wizard = 6
    }
    public enum SiteTermName
    {
        /// <summary>
        /// None site terms
        /// </summary>
        None = -1,

        /// <summary>
        /// Countries site terms
        /// </summary>
        Countries = 0,

        /// <summary>
        /// Regions site terms
        /// </summary>
        Regions = 1,

        /// <summary>
        /// Credit Card Types
        /// </summary>
        CreditCardTypes = 2,

        /// <summary>
        /// Provinces site term
        /// </summary>
        Provinces = 3,

        /// <summary>
        /// States site term
        /// </summary>
        States = 4,

        /// <summary>
        /// Months site term
        /// </summary>
        Months = 5,

        /// <summary>
        /// Years site term
        /// </summary>
        Years = 6,
        #region BTNextGen
        AudienceTypes = 7,
        ProductType = 8,
        MarketType = 9,
        CartFormats = 10,
        SortOrder = 11,
        CartSortBy = 12,
        DefaultDuplicateCarts = 13,
        DefaultDuplicateOrders = 14,
        ResultFormat = 15,
        ResultSortBy = 16,
        ProductTypeFilters = 17,
        UserRole = 18,
        ReviewType = 19,
        OrganizationStatus = 20,
        AdvSearchBook_SearchTerms = 21,
        AdvSearchBookFilterAttribute = 22,
        AdvSearchBookFilterFormat = 23,
        AdvSearchBookFilterLanguague = 24,
        AdvSearchBookFilterPublisherStatus = 25,
        AdvSearchBookFilterLexileScale = 26,
        AdvSearchBookFilterARInterestLevel = 27,
        AdvSearchBookFilterPublishDate = 28,
        DisableReasonCode = 29,
        AdvSearchMusicFilterFormat = 30,
        AdvSearchMusicFilterGenre = 31,
        AdvSearchMovieFilterFormat = 32,
        AdvSearchMovieFilterGenre = 33,
        AdvSearchMusicSearchTerms = 34,
        AdvSearchMovieSearchTerms = 35,
        AdvSearchMusicFilterPublishDate = 36,
        FeeType = 37,
        AddressType = 38,
        CartFolderType = 39,
        UserFunctions = 40,
        OrganizationAdminFunctions = 41,
        BTAdminFunctions = 42,
        BTUserRoleType = 43,
        AdvSearchBookFilterStockStatusServiceCenter = 44,
        AdvSearchBookFilterInventory = 45,
        AdvSearchBookFilterClassification = 46,
        AdvSearchBookFilterStockStatusOnHand = 47,
        AdvSearchBookFilterEdition = 48,
        ResultPerPageTable = 49,
        ResultPerPageTile = 50,
        SecurityQuestions = 51,
        FeeDescription = 52,
        US_en_US = 53,
        OnhandInventoryStatus = 54,
        OrganizationType = 55,
        CartStatus = 56,
        card_type = 57,
        AdvSearchBookFilterPublisherStatusExclude = 58,
        AdvSearchBookFilterRCInterestLevel = 59,
        payment_terms = 60,
        AccountType = 61,
        ISBNLinkDisplayed = 62,
        GiftWrapping = 63,
        OnhandInventoryStatusItemDetails = 64,
        ShippingMethod = 65,
        eSuppliers = 66,
        OriginalEntryPhysicalFormat = 67,
        workflow_timezone = 68,
        //ResultsPerPage
        ResultsPerPage = 69,
        HoldingsType = 70,
        AdvSearchBookFilterPurchaseOption = 71,
        AdvSearchBookFilterProductAttribute = 72,
        AdvSearchMusicMovieFilterProductAttribute = 73,
        BookBTProgram = 74,
        MusicBTProgram = 75,
        MovieBTProgram = 76,
        AYProgram = 77,
        AdvancedSearchComparisionOperator = 78,
        AdvSearchEBookFilterProductAttribute = 79,
        ReturnsPageReasonForReturn_Retail = 80,
        ReturnsPageReasonForReturn_LE = 81,
        ReturnsPageAction = 82,
        DefaultDownloadedCarts = 83,
        AdvSearchBookFilterChildrenFormat,
        AdvSearchMovieBoxOffice,
        AltFormatSearchSortBy,
        AltFormatCartSortBy,
        ILSVendors
        #endregion BTNextGen
    }

    public enum MarketType
    {
        Any = -1,
        Retail = 0,
        PublicLibrary = 1,
        AcademicLibrary = 2,
        SchoolLibrary = 3,
    }

    public enum ProductType
    {
        Book = 0,
        //Entertainment = 1,
        Music = 2,
        Movie = 3,
        Digital = 4
    }

    public enum ProductTypeEx
    {
        Book = 0,
        Music = 2,
        Movie = 3,
        Digital = 4, // 40

        DigitaleBook = 41,
        DigitaleAudio = 42
    }

    public static class InventoryWareHouseCode
    {
        public const string Ren = "REN";
        public const string Com = "COM";
        public const string Mom = "MOM";
        public const string Som = "SOM";
        public const string Rno = "RNO";
        public const string VIM = "VIM";
        public const string VIE = "VIE";
        public const string SUP = "VIP";
    }

    public enum OnhandInventoryStatus
    {
        InStock,
        AvailableToBackorder,
        AvailableToPreorder,
        Available,
        EmptyStock
    }

    public enum BTBPublisherStatus
    {
        PublisherOutOfStock,
        NotYetPublished,
        NotDefined
    }

    public enum BTEInactiveFlag
    {
        SupplierOutOfStock,
        NotYetPublished,
        NotDefined
    }

    public enum DefaultDuplicateOrders
    {
        AllAccounts,
        MyAccounts
    }

    public enum DefaultDuplicateCarts
    {
        AllCarts,
        MyCarts
    }

    public enum NoSqlServiceStatus
    {
        Success = 0,
        Fail = 1
    }
   
    
    public enum PermissionType
    {
        BTAdmin,
        BTUser,
        OrganizationAdmin,
        OrganizationUser,
        IsBTEmployee,
        ViewOrganizationList,
        ViewOrganizationDetail,
        ViewAccounts,
        ViewUsers,
        CreateOrganization,
        SearchOrganization,
        SearchAccounts,
        SearchUsers,
        EditOrganizationDetail,
        ViewAccountDetail,
        ViewAccountAssignment,
        EditAccountAssignment,
        ViewCartAdmin,
        ViewUserDetail,
        CreateUser,
        EditAccounts,
        EditUsers,
        EditAccountDetail,
        EditUserDetail,
        SubmitOrder,
        //Support org addition permission
        OrganizationSupport,
        EditSupportOrganization,
        EditAccountInformation,
        EditAccountAccess,
        OriginalEntry,
        ProductSupport,
        ProxyUser,
        OneClickMARC,
        OnlineBillPay,
        ESPAccess,
        CollectionDevelopment,
        RequestQuotes,
        ReleaseQuotes,
        ESPAdmin,
        StandingOrdersServicesSupport,
        StandingOrderServicesAdmin,
        CustomerServiceDashboard
    }

    /// <summary>
    /// LineStatus
    /// </summary>
    public enum LineStatus
    {
        View = 0,
        Add = 1,
        Edit = 2
    }

    public enum UserFunctions
    {
        SubmitOrder,
        OneClickMARC,
        CreateNewOrganization,
        MaintainOrganization,
        CreateUser,
        MaintainUser,
        SupportOrganization,
        NotDefined,
        OriginalEntry,
        OnlineBillPay,
        ProductSupport,
        ProxyUser,
        ESPAccess,
        CollectionDevelopment,
        RequestQuotes,
        ReleaseQuotes,
        SharedLogin,
        ESPAdmin,
        StandingOrdersServicesSupport,
        StandingOrderServicesAdmin,
        CustomerServiceDashboard
    }

    public enum DownloadExportMode
    {
        Simple = 0,
        POS = 1,
        BasicTitle = 2,
        BasicTitleExpanded = 3,
        BasicMARC = 4,
        BasicTitleWithGrid = 5,
        BasicTitleWithGridExpanded = 6,
        MARCProfilerDownload = 7,
        LawsonDownloadFormat = 8,
        FullASCIIGrid = 9,
        GridSummaryExport = 10
    }

    public enum ControlMode
    {
        /// <summary>
        /// Indicates user control is not set to any mode
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates user control is in edit mode.
        /// </summary>
        Edit = 1,

        /// <summary>
        /// Indicates user control is in create mode.
        /// </summary>
        Create = 2,

        /// <summary>
        /// Indicates user control is in delete mode.
        /// </summary>
        Delete = 3,

        /// <summary>
        /// Indicates user control is in view mode.
        /// </summary>
        View = 4,

        GetInventoryForHorizontalMode = 5,

        Reset = 6,

        ReplaceAll = 7,

        CopyToOtherOrganizationCompleted = 8,
        SetQuantities = 9,
        ApplyDuplicates = 10,
        Print = 11,
        Submit = 12,
        GridSummaryExport = 13
    }

    public enum SplitCartMode
    {
        SplitToSeparateCart = 0,
        SplitNoRequisitionItemsToGridCart = 1,
        SplitNoRequisitionItemsToNonGridCart = 2,
        DesignateAsNonGrid = 3,
        DesignateAsGrid = 4
    }

    public enum CartMixMode
    {
        HasAllType = 123,
        HasNoRequisitionOnly = 1,
        HasNonRequisitionOnly = 2,
        HasGridRequisitionOnly = 3,
        HasNoRequisitionAndNonRequisition = 12,
        HasNoRequisitionAndGridRequistion = 13,
        HasNonRequisitionAndGridRequistion = 23,
        HasNoItem = 0
    }

    public enum CartSplitResult
    {
        HasNoItems = 0,
        HasNotOpen = 1,
        InvalidUser = 2,
        CompletedWithRunBackground = 3,
        CompletedWithoutRunBackground = 4,
        IsSharedCart = 5,
        Failed = 6,
        DesignateAsNonGrid = 7,
        DesignateAsGrid = 8
    }

    /// <summary>
    /// Custom mode
    /// </summary>
    public enum CustomMode
    {
        /// <summary>
        /// Indicates user control will redirect to another page or open new window.
        /// </summary>
        Redirect = 0,

        /// <summary>
        /// Re-use Create of control mode
        /// </summary>
        Create = 1,

        /// <summary>
        /// Re-use view of control mode
        /// </summary>
        View = 2,

        /// <summary>
        /// For Edit User
        /// </summary>
        EditUser = 3,

        /// <summary>
        /// For Edit account
        /// </summary>
        EditAccount = 4,

        /// <summary>
        /// Save new user
        /// </summary>
        SaveNewUser = 5,

        None = 6,

        SearchMode = 7
    }

    /// <summary>
    /// Admin Search mode
    /// </summary>
    public enum AdminSeachMode
    {
        /// <summary>
        /// Search For Account
        /// </summary>
        SearchOrganization = 0,

        /// <summary>
        /// Search For Account that begin with search key
        /// </summary>
        SearchAccountBegin = 1,

        /// <summary>
        /// Search For Account that contains search key
        /// </summary>
        SearchAccountContains = 2,

        /// <summary>
        /// Search For users
        /// </summary>
        SeacrhUser = 3
    }

    public enum NewReleasesCalendarStatus
    {
        ViewNextMonth = 0,
        ViewPreviousMonth = 1,
        ViewCurrentMonth = 2
    }

    public enum FreezeLevel
    {
        /// <summary>
        /// Indicates no freeze and actions are open/blocked based on cart status
        /// </summary>
        Level0 = 0,

        /// <summary>
        /// Indicates all cart actions are frozen
        /// </summary>
        Level1 = 1,

        /// <summary>
        /// Indicates all cart actions are frozen except download cart and submit order
        /// </summary>
        Level2 = 2,
    }

    public enum ItemDetailsMode
    {
        None = 0,
        GetAdditionalVersions = 1,
        GetItemNetPriceWhenItemInCart = 2,
        GetItemNetPriceWhenItemNotInCart = 3
    }

    public enum ProductStatus
    {
        A = 0, //Active
        D = 1, //Deleted
        R = 2, //
        All
    }

    public enum FeeTypeEnum
    {
        PerUnit = 0,
        PerOrder = 1
    }

    public enum AddressType
    {
        Standard = 0,
        APO_FPO = 1
    }

    public enum ItemsViewMode
    {
        CoverFlow = 0,
        Table
    }

    public enum AssociationType
    {
        Direct = 0,
        Indirect = 1
    }

    public enum ImageSize
    {
        Small = 0,
        Medium = 1,
        Large = 2
    }

    public enum OneClickTestEnum
    {
        Untested = 0,
        Successful = 1,
        InProcess = 2,
        Failed = 3
    }
    public enum UserAlertTemplateID
    {
        CopyCart = 22,
        MergeCart = 23,
        MoveCart = 24,
        ApplyGridTemplate = 25,
        ILSOrderError = 44,
        ILSValidationError = 45,
        ILSOrderSucess = 46,
        ILSValidationSucess = 47,
        ILSOrderValidationError = 48,
        ILSOrderValidationSuccess = 49
    }

    public enum ILSSystemStatus
    {
        ReadyForPickUp = 1,
        PickedUp = 2,
        InProgress = 3,
        Completed = 4
    }

    public enum ILSValidationStatus
    {
        ValidationPending = 1,
        ValidationInProgress = 2,
        ValidationFailed = 3,
        ValidationSuccessful = 4
    }

    public enum ProxyErrorCode
    {
        Success = 0,
        Warn = 1,
        Fail = 2,
        Expired = 3,
        ContinueOption = 4
    }

    public enum SearchType
    {
        Contains = 0,
        StartsWith = 1,   // future use if any
        EndsWith = 2,
        Equals = 3
    }
}
