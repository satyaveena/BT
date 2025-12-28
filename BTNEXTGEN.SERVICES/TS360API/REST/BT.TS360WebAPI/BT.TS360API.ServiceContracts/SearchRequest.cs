using System.Collections.Generic;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using System;
using BT.TS360API.ServiceContracts.Request;

namespace BT.TS360API.ServiceContracts
{
    public class SearchRequest : BaseRequest
    {
        public List<string> btKeyList { get; set; }
        public string url { get; set; }
        public bool? IsAltFormatCall { get; set; }
        public string AltFormatCallBtKey { get; set; }

        public string UserId { get; set; }
        public string LoginId { get; set; }
        public bool IsHideNetPriceDiscountPercentage { get; set; }
        public string MarketTypeString { get; set; }
        public bool ShowEnhancedContentIconsForSearch { get; set; }
        public bool ShowExpectedDiscountPriceForSearch { get; set; }
        public bool ShowUserEditableFieldsForSearch { get; set; }
        public bool ShowInventoryForSearch { get; set; }
        public bool ShowDupCheckCartsForSearch { get; set; }
        public bool ShowDupCheckOrdersForSearch { get; set; }
        public string DefaultDownloadedCarts { get; set; }
        public TargetingValues Targeting { get; set; }
        public SearchByIdData SearchData { get; set; }
        public AccountInfoForPricing AccountPricingData { get; set; }
        public SearchArguments SearchArguments { get; set; }

        public string CountryCode { get; set; }
        public string OrgId { get; set; }

        public MarketType? MarketType { get; set; }

        public bool? IsFromExternalAPI { get; set; }
    }

    public class ProductDuplicateIndicatorRequest : BaseRequest
    {
        public List<string> btKeyList { get; set; }
        public List<string> btEKeyList { get; set; }
        public string basketId { get; set; }
        public bool isRequiredCheckDupCarts { get; set; }
        public bool isRequiredCheckDupOrder { get; set; }

        public string UserId { get; set; }
        public string OrgId { get; set; }
        public string DefaultDownloadedCarts { get; set; }
    }



    public class SecondaryInfoItemDetailRequest : BaseRequest
    {
        public string BTKey { get; set; }
        public string CartId { get; set; }
        public string LineItemId { get; set; }
        public string Author { get; set; }
        public bool IsPrimaryCartSet { get; set; }
        public string Catalog { get; set; }
        public string ESupplier { get; set; }
        public RecordIndexArgs RecordIndexArg { get; set; }
        public string Tab { get; set; }
        public string HdfTabLoaded { get; set; }
        public bool AllowBTEmployee { get; set; }

        public string UserId { get; set; }
        public bool IsHideNetPriceDiscountPercentage { get; set; }
        public string[] DeafaultESuppliersAccount { get; set; }
        public string DefaultDownloadedCarts { get; set; }
        public TargetingValues Targeting { get; set; }
        public SearchByIdData SearchData { get; set; }
        public AccountInfoForPricing AccountPricingData { get; set; }
    }

    public class GridLineCount
    {
        public string BTKey { get; set; }
        public string CartId { get; set; }
        public string LineItemId { get; set; }
        public bool AllowBTEmployee { get; set; }

        public string UserId { get; set; }
        public bool IsGridEnabled { get; set; }
        public string TitleDetailSelectingTab { get; set; }
        public bool OCLCCatalogingPlusEnabled { get; set; }

    }
    public class DupIcons : BaseRequest
    {
        public string BTKey { get; set; }
        public string CartId { get; set; }

        public string UserId { get; set; }
        public string DefaultDownloadedCarts { get; set; }
        public string OrgId { get; set; }

    }
    public class PrimaryInfoItemDetailRequest : BaseRequest
    {
        public string BTKey { get; set; }
        public string CartId { get; set; }
        public string LineItemId { get; set; }
        //public bool IsVIP { get; set; }

        public string UserId { get; set; }
        public bool IsHideNetPriceDiscountPercentage { get; set; }
        public TargetingValues Targeting { get; set; }
        public AccountInfoForPricing AccountPricingData { get; set; }
        public SearchByIdData SearchData { get; set; }

    }
    public class InventoryDemandItemDetail : BaseRequest
    {
        public InventoryStatusArgContract InventoryArg { get; set; }
        public MarketType? MarketType { get; set; }
        public string UserId { get; set; }
        public string CountryCode { get; set; }
        public string OrgId { get; set; }
    }
    public class QuickSearchToggleProductImagesRequest : BaseRequest
    {
        public string UserId { get; set; }
    }

    public class ProductDemandDataForItemRequest
    {
        public PrimaryInfoItemDetailArg arg { get; set; }
        public int pageIndex { get; set; }
        public string UserId { get; set; }
    }

    public class ToggleQuickViewRequest : BaseRequest
    {
        public int InPageEnumValue { get; set; }
        public bool IsQuickMode { get; set; }

        public string UserId { get; set; }
    }

    public class ToggleProductImageRequest : BaseRequest
    {
        public int inPageEnumValue { get; set; }

        public string UserId { get; set; }
    }
    public class ItemDetailArgRequest : BaseRequest
    {
        public ItemDetailArg arg { get; set; }

        public string UserId { get; set; }
        public string OrgId { get; set; }
        public bool OCLCCatalogingPlusEnabled { get; set; }
        public string DefaultDownloadedCarts { get; set; }
    }
    public class DataFixSendEmailRequest : BaseRequest
    {
        public string btKey { get; set; }
        public string userNote { get; set; }
    }

    public class GetEncryptQueryRequest : BaseRequest
    {
        public string UserId { get; set; }
    }

    public class GetProductPricingRequest : BaseRequest
    {
        public List<PricingClientArg> pricingArgList { get; set; }
    }

    public class GetActiveCartsRequest : BaseRequest
    {
        public SearchTooltipConextData context { get; set; }
    }

    public class ActiveNewestBasketsRequest
    {
        public string UserId { get; set; }
        public string ExcludeBasketId { get; set; }
    }

    public class AddTitlesToCartNameWithGridRequest : BaseRequest
    {
        public List<AddToNewCartObject> addToNewCartObjects { get; set; }
        public List<DCGridInputData> gridTitleProperties { get; set; }

        public string UserId { get; set; }
        public string DefaultQuantity { get; set; }
        public TargetingValues Targeting { get; set; }
    }

    public class AddAllToCartRequest : BaseRequest
    {
        public string clientQuantity { get; set; }
        public int maxpageSize { get; set; }

        public string UserId { get; set; }
        public string DefaultQuantity { get; set; }
        public TargetingValues Targeting { get; set; }
        public SearchByIdData SearchData { get; set; }
    }


    public class GetEnhancedContentForSearchRequest : BaseRequest
    {
        public string btKey { get; set; }
        public bool isTileView { get; set; }

        public string UserId { get; set; }
        public string MarketTypeString { get; set; }
        public bool ShowEnhancedContentIconsForSearch { get; set; }
        public bool ShowExpectedDiscountPriceForSearch { get; set; }
        public bool ShowUserEditableFieldsForSearch { get; set; }
        public bool ShowInventoryForSearch { get; set; }
        public bool ShowDupCheckCartsForSearch { get; set; }
        public bool ShowDupCheckOrdersForSearch { get; set; }
        public bool ShowProductLookupForSearch { get; set; }
        //public string DefaultDownloadedCarts { get; set; }
        public bool IsHideNetPriceDiscountPercentage { get; set; }
        public TargetingValues Targeting { get; set; }
        public AccountInfoForPricing AccountPricingData { get; set; }
        public SearchByIdData SearchData { get; set; }
    }

    public class QuickSearchActiveCarts : BaseRequest
    {
        public string PassPhrase { get; set; }
        public string UserId { get; set; }

    }
    public class GetUserAlertCountRequest : BaseRequest
    {
        public string userId { get; set; }
    }

    public class AdditionalCartLineItemsInfoRequest : BaseRequest
    {
        public bool IsCheckCartInventory { get; set; }
        public RequestCartSummary CartSummary { get; set; }
        public List<RequestLineItem> LineItems { get; set; }

        public AdditionalCartLineItemsInfoRequestContext UserContext { get; set; }
    }

    public class AdditionalCartLineItemsInfoRequestContext
    {
        public MarketType MarketType { get; set; }
        public bool ShowExpectedDiscountPriceForCart { get; set; }
        public bool ShowInventoryForCart { get; set; }
        //public bool ShowDupCheckCartsForCart { get; set; }
        //public bool ShowDupCheckOrdersForCart { get; set; }
        public bool ShowUserEditableFieldsForCart { get; set; }
        public string CountryCode { get; set; }
        public string OrgId { get; set; }
        //public string DefaultDownloadedCarts { get; set; }
        public TargetingValues Targeting { get; set; }
    }

    public class RequestCartSummary
    {
        public string CartId { get; set; }
        public bool IsShared { get; set; }
        public bool HasPermission { get; set; }
        public bool HasGaleAccount { get; set; }
    }

    public class RequestLineItem
    {
        private string _btKey;
        private string _espDetailUrl;

        public string Id { get; set; }

        public string BTKey
        {
            get
            {
                if (IsOriginalEntryItem && !string.IsNullOrEmpty(BasketOriginalEntryId))
                    _btKey = BasketOriginalEntryId;

                return _btKey;
            }
            set { _btKey = value; }
        }

        public string BTEKey { get; set; }
        public string ProductType { get; set; }
        public string CatalogName { get; set; }
        public string ReportCode { get; set; }

        public string BasketOriginalEntryId { get; set; }
        public bool IsOriginalEntryItem
        {
            get { return !string.IsNullOrEmpty(BasketOriginalEntryId); }
        }

        public decimal BTDiscountPercent { get; set; }
        public decimal? ListPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public string ESupplier { get; set; }
        public int? Quantity { get; set; }
        public DateTime? PublishDate { get; set; }
        public string Publisher { get; set; }
        public string MerchCategory { get; set; }
        public string BTGTIN { get; set; }
        public string Upc { get; set; }
        public bool HasReturn { get; set; }
        public string PriceKey { get; set; }
        public string ProductLine { get; set; }
        public string FormatLiteral { get; set; }
        public string SupplierCode { get; set; }
        public bool HasReview { get; set; }
        public string BlockedExportCountryCodes { get; set; }
    }

    public class RequestLineItemEx : RequestLineItem
    {
        public string ISBN { get; set; }
        public string ISBN10 { get; set; }
        public string Edition { get; set; }
    }

    public class EnhancedContentsForCartDetailsRequest : BaseRequest
    {
        public string CartId { get; set; }
        public string LineItemId { get; set; }
        public string BTKey { get; set; }
        public string OrgId { get; set; }
        public RequestLineItemEx LineItem { get; set; }
        //public ProductSearchResultItem SearchResultItem { get; set; }
        public EnhancedContentsForCartDetailsRequestContext UserContext { get; set; }
    }

    public class EnhancedContentsForCartDetailsRequestContext
    {
        public MarketType MarketType { get; set; }
        public bool ShowExpectedDiscountPriceForCart { get; set; }
        public bool ShowInventoryForCart { get; set; }
        public bool ShowEnhancedContentIconsForCart { get; set; }
        //public bool ShowDupCheckCartsForCart { get; set; }
        //public bool ShowDupCheckOrdersForCart { get; set; }
        public bool ShowProductLookupForCart { get; set; }
        public bool ProductLookupDeactivated { get; set; }
        //public string DefaultDownloadedCarts { get; set; }
        public TargetingValues Targeting { get; set; }
    }

    public class AllInfoForQuickItemDetailsRequest : BaseRequest
    {
        public string UserId { get; set; }
        public string CartId { get; set; }
        public string LineItemId { get; set; }
        //public string BTKey { get; set; }
        public MarketType MarketType { get; set; }
        public InventoryStatusArgContract InventoryArg { get; set; }
        public SearchCartCriteria SearchCartCriteria { get; set; }
    }

    public class ProductAltFormatsRequest : BaseRequest
    {
        public string BTKey { get; set; }
        public string UserId { get; set; }
        public string PrimaryCartId { get; set; }
        public List<string> RemainingBTKeys { get; set; }
        public ProductAltFormatsRequestContext UserContext { get; set; }
        public bool isOrgPPCEnabled { get; set; }
        
        /// <summary>
        /// Max number of items to response.
        /// Zero means all.
        /// </summary>
        public int MaxItemNumber { get; set; }
    }

    public class ProductAltFormatsRequestContext
    {
        public MarketType MarketType { get; set; }
        public bool SimonSchusterEnabled { get; set; }
        public bool IsHideNetPriceDiscountPercentage { get; set; }
        public bool EnableProcessingCharges { get; set; }
        public decimal BookProcessingCharge { get; set; }
        public decimal MovieProcessingCharge { get; set; }
        public decimal MusicProcessingCharge { get; set; }
        public decimal PaperbackProcessingCharge { get; set; }
        public decimal SpokenWordProcessingCharge { get; set; }
        public float SalesTax { get; set; }
        public string[] ESuppliers { get; set; }
        public string CountryCode { get; set; }
        public TargetingValues Targeting { get; set; }
        public bool FromCartDetailPage { get; set; }
    }

    public class GetSuggestionListRequest : BaseRequest
    {
        public string prefixText { get; set; }
        public int startRowIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class UpdateNotificationCartUsersRequest : BaseRequest
    {
        public List<string> ActiveNotificationCartUsers { get; set; }
        public List<string> RemovedNotificationCartUsers { get; set; }

    }
    public class ProductDetailsInfoForQuickItemDetailRequest
    {
        public string BTKey { get; set; }
        public string CartId { get; set; }
        public string UserId { get; set; }
        public bool OCLCCatalogingPlusEnabled { get; set; }
        public bool AllowBTEmployee { get; set; }
        //public string DefaultDownloadedCarts { get; set; }
        public string OrgId { get; set; }

    }

    public class GetItemDetailsTooltipInfoRequest : BaseRequest
    {
        public string BTKey { get; set; }
        public MarketType MarketType { get; set; }
        public string CountryCode { get; set; }
        public string UserId { get; set; }
    }

    public class GetItemRealTimenventoryRequest : BaseRequest
    {
        public string BTKey { get; set; }
        public MarketType MarketType { get; set; }
        public string OrgId { get; set; }
        public string CountryCode { get; set; }
        public string CartId { get; set; }
        public string UserId { get; set; }
        public string ESupplier { get; set; }
        public string ProductType { get; set; }
    }

    public class CheckRealTimeInventoryForQuickCartDetailsInfoRequest : BaseRequest
    {
        public string BTKey { get; set; }
        public MarketType MarketType { get; set; }
        public string CountryCode { get; set; }
        public string CartId { get; set; }
        public string UserId { get; set; }
        public string OrgId { get; set; }
        public string SortBy { get; set; }
        public byte SortDirection { get; set; }
        public int PageNumber { get; set; }
        public short PageSize { get; set; }

    }

    public class DupCheckForAltFormatsRequest : BaseRequest
    {
        public string UserId { get; set; }
        public string OrgId { get; set; }

        public List<string> BTKeyList { get; set; }
        public List<string> BTEKeyList { get; set; }

        public bool ShowDupCheckCartsForSearch { get; set; }
        public bool ShowDupCheckOrdersForSearch { get; set; }
        public string DefaultDownloadedCarts { get; set; }
    }

    public class InventoryForAltFormatsRequest : BaseRequest
    {
        public List<SearchResultInventoryStatusArg> InventoryArgs { get; set; }
        public string UserId { get; set; }
        public string OrgId { get; set; }
        public string CountryCode { get; set; }
        public MarketType? MarketType { get; set; }
    }

    public class UserEditableFieldsForAltFormatsRequest : BaseRequest
    {
        public List<string> BTKeyList { get; set; }
        public string UserId { get; set; }
    }

    public class EnhancedContentIconsForAltFormatsRequest : BaseRequest
    {
        public List<string> HasReviewBTKeyList { get; set; }
        public string UserId { get; set; }
        public string OrgId { get; set; }
    }
}
