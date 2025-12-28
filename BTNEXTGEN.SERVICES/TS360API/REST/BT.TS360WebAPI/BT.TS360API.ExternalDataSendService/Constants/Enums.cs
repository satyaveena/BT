using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.ExternalDataSendService.Constants
{
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

    public enum AppServiceStatus
    {
        Success = 0,
        Fail = 1,
        LimitationFail = 2
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

    public enum NoSqlServiceStatus
    {
        Success = 0,
        Fail = 1
    }
}