using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts
{
    public class CollectionData : BaseRequest
    {
        public string UserId { get; set; }
        public bool isPrimaryCartSet { get; set; }

        public bool IsHideNetPriceDiscountPercentage { get; set; }
        public string ProxiedUserId { get; set; }
        public TargetingValues Targeting { get; set; }
        public SearchByIdData SearchData { get; set; }
        public AccountInfoForPricing AccountPricingData { get; set; }
    }

    public class QuickSearchGetFolder : BaseRequest
    {
        public string UserId { get; set; }
    }
    public class TargetingRequest : BaseRequest
    {
        public TargetingValues Targeting { get; set; }
    }
    public class AccountInfoForPricing
    {
        public bool EnableProcessingCharges { get; set; }
        public decimal BookProcessingCharge { get; set; }
        public decimal PaperbackProcessingCharge { get; set; }
        public decimal SpokenWordProcessingCharge { get; set; }
        public decimal MovieProcessingCharge { get; set; }
        public decimal MusicProcessingCharge { get; set; }
        public float SalesTax { get; set; }
    }

    public class SearchResultsPricingArg
    {
        public string UserId { get; set; }
        public bool IsHideNetPriceDiscountPercentage { get; set; }
        public string[] ESuppliers { get; set; }
        public TargetingValues TargetingValues { get; set; }
        public AccountInfoForPricing InforForPricing { get; set; }
    }

    public class TargetingValues
    {
        public string PIGName { get; set; }
        public string SiteBranding { get; set; }
        public MarketType? MarketType { get; set; }
        public string[] ProductType { get; set; }
        public string[] AudienceType { get; set; }
        public string OrgId { get; set; }
        public string OrganizationName { get; set; }
    }

    public class SearchByIdData
    {
        public bool SimonSchusterEnabled { get; set; }
        public string CountryCode { get; set; }
        public string[] ESuppliers { get; set; }
    }
    public class SiteContextObject
    {
        //Access Token
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string LoginId { get; set; }
        public string OrgId { get; set; }
        public string OrganizationName { get; set; }
        public string MarketTypeString { get; set; }
        
        public bool ShowInventoryForSearch { get; set; }
        public bool ShowEnhancedContentIconsForSearch { get; set; }
        public bool ShowDupCheckCartsForSearch { get; set; }
        public bool ShowDupCheckOrdersForSearch { get; set; }
        public bool ShowExpectedDiscountPriceForSearch { get; set; }

        public string DefaultDownloadedCarts { get; set; }

        public MarketType? MarketType { get; set; }
        public bool EnableProcessingCharges { get; set; }
        public string[] AudienceType { get; set; }

        public string DefaultQuantity { get; set; }
        //public bool IsQuickCartDetailsEnabled { get; set; }

        public bool IsGridEnabled { get; set; }

        public bool IsAuthorizedtoUseAllGridCodes { get; set; }
        public decimal BookProcessingCharge { get; set; }
        public decimal PaperbackProcessingCharge { get; set; }
        public decimal SpokenWordProcessingCharge { get; set; }
        public decimal MovieProcessingCharge { get; set; }
        public decimal MusicProcessingCharge { get; set; }
        public float SalesTax { get; set; }
        public string CountryCode { get; set; }
        public bool IsHideNetPriceDiscountPercentage { get; set; }
        public string[] ESuppliers { get; set; }
        public bool ShowUserEditableFieldsForSearch { get; set; }
        public bool OCLCCatalogingPlusEnabled { get; set; }
        public string PIGName { get; set; }
        public string SiteBranding { get; set; }
        public string[] ProductType
        {
            get;
            set;
        }

        public bool SimonSchusterEnabled { get; set; }

        public string DefaultBookAccountId { get; set; }
        public string DefaultEntertainmentAccountId { get; set; }
        public string DefaultVIPAccountId { get; set; }
        public string DefaultDashboardId { get; set; }

        public string TitleDetailSelectingTab { get; set; }
        public string[] DeafaultESuppliersAccount { get; set; }
        public string ProxiedUserId { get; set; }
        public bool IsProxyActive { get; set; }
        public string BtUserId { get; set; }
        public bool ShowProductLookupForSearch { get; set; }
        public bool PPCEnabled { get; set; }
        //...
    }
}
