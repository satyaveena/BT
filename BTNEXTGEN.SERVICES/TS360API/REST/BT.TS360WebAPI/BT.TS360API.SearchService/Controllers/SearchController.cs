using BT.TS360API.Common.Controller;
using BT.TS360API.ExternalServices;
using BT.TS360API.ExternalServices.BTStockCheckServices;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Product;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace BT.TS360API.SearchService.Controllers
{
    public partial class SearchController : ApiController
    {
        private readonly Services.SearchService _searchService;
        //private string _defaultScheme = "http";

        public SearchController()
        {
            this._searchService = new Services.SearchService();
        }

        #region move wcf to api

        [HttpPost]
        [Route("appservice/searchservice/GetItemDetailsTooltipInfo")]
        public async Task<AppServiceResult<ProductDetailInfo>> GetItemDetailsTooltipInfo(GetItemDetailsTooltipInfoRequest request)
        {
            return await _searchService.GetItemDetailsTooltipInfo(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetRealTimeInventoryInfo")]
        public async Task<AppServiceResult<StockCheckResponse>> GetRealTimeInventoryInfo(GetItemRealTimenventoryRequest request)
        {
            return await _searchService.GetRealTimeInventoryInfo(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/CheckRealTimeInventoryForQuickCartDetailsInfo")]
        public async Task<AppServiceResult<AdditionalCartLineItemsResponse>> CheckRealTimeInventoryForQuickCartDetailsInfo(CheckRealTimeInventoryForQuickCartDetailsInfoRequest request)
        {
            return await _searchService.CheckRealTimeInventoryForQuickCartDetailsInfo(request);
        }

        #endregion

        [HttpPost]
        [Route("appservice/searchservice/AddProductToCartName")]
        public async Task<AppServiceResult<AddToCartStatusObject>> AddProductToCartName(AddProductToCartNameRequest request)
        {
            return await _searchService.AddProductToCartName(request);
        }

        //[HttpPost]
        //[Route("appservice/searchservice/AddProductToPrimaryCart")]
        //public AppServiceResult<string> AddProductToPrimaryCart(AddProductToPrimaryCartRequest request)
        //{
        //    return _searchService.AddProductToPrimaryCart(request);
        //}

        [HttpPost]
        [Route("appservice/searchservice/AddProductWithGridToPrimaryCart")]
        public AppServiceResult<AddToCartStatusObject> AddProductWithGridToPrimaryCart(AddProductWithGridToPrimaryCartRequest request)
        {
            return _searchService.AddProductWithGridToPrimaryCart(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/AddProductWithGridToSelectedCart")]
        public AppServiceResult<AddToCartStatusObject> AddProductWithGridToSelectedCart(AddProductWithGridToSelectedCartRequest request)
        {
            return _searchService.AddProductWithGridToSelectedCart(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/AddSimpleProductToCart")]
        public AppServiceResult<AddToCartStatusObject> AddSimpleProductToCart(SimpleProductToCartRequest request)
        {
            var result = new AppServiceResult<AddToCartStatusObject>();
            result.Status = AppServiceStatus.Success;
            try
            {
                result.Data = _searchService.AddSimpleProductToCart(request);
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "AddSimpleProductToCart");
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductDemandDataForItem")]
        public AppServiceResult<DemandInfoForItemContract> GetProductDemandDataForItem(ProductDemandDataForItemRequest request)
        {
            return _searchService.GetProductDemandDataForItem(request.arg, request.pageIndex, request.UserId);
        }

        [HttpPost]
        [Route("appservice/searchservice/QuickSearchGetActiveCarts")]
        public AppServiceResult<List<ItemDataContract>> QuickSearchGetActiveCarts(QuickSearchActiveCarts request)
        {
            var result = new AppServiceResult<List<ItemDataContract>>();
            result.Status = AppServiceStatus.Success;

            if ( !string.IsNullOrEmpty(request.UserId))
            {
                result.Data = _searchService.QuickSearchGetActiveCarts(request.UserId);
            }
            else
            {
                Logger.Write("ServiceController.QuickSearchGetActiveCarts", "UserId parameter is null or empty");
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
            }

            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/QuickSearchGetFolderList")]
        public AppServiceResult<List<ItemDataContract>> QuickSearchGetFolderList(QuickSearchGetFolder request)
        {
            var result = new AppServiceResult<List<ItemDataContract>>();
            result.Status = AppServiceStatus.Success;

            if (!string.IsNullOrEmpty(request.UserId))
            {
                result.Data = _searchService.QuickSearchGetFolderList(request.UserId);
            }
            else
            {
                Logger.Write("ServiceController.QuickSearchGetFolderList", "UserId parameter is null or empty");
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
            }
            return result;
        }

        //[HttpPost]
        //[Route("appservice/searchservice/GetCartFolderById")]
        //public AppServiceResult<LandingPageResponse> GetCartFolderById( LandingPageRequest request)
        //{
        //    return _searchService.GetCartFolderById(request.FolderId, request.UserId);
        //}

        [HttpPost]
        [Route("appservice/searchservice/Test")]
        public string Test([FromUri] string id)
        {
            try
            {
                var u = ProfileService.Instance.GetUserById("{0075b804-c5ce-4db6-9b95-553a88d8ca78}");
                u.MyReviewTypes = ProfileService.Instance.GetUserReviewTypes("{0075b804-c5ce-4db6-9b95-553a88d8ca78}", u.MyReviewTypeIds);

                var o = ProfileService.Instance.GetOrganizationById(id);
            }
            catch (Exception)
            {
                throw;
            }

            return "";
        }

        [HttpPost]
        [Route("appservice/searchservice/GetAdditionalCartLineItemsInfo")]
        public async Task<AppServiceResult<AdditionalCartLineItemsResponse>> GetAdditionalCartLineItemsInfo([FromBody] AdditionalCartLineItemsInfoRequest request)
        {
            return await _searchService.GetAdditionalCartLineItemsInfo(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetEnhancedContentsForCartDetails")]
        public async Task<AppServiceResult<EnhancedContentsForCartDetailsResponse>> GetEnhancedContentsForCartDetails([FromBody] EnhancedContentsForCartDetailsRequest request)
        {
            return await _searchService.GetEnhancedContentsForCartDetails(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetAllInfoForQuickItemDetailPage")]
        public AppServiceResult<AllInfoForQuickItemDetailsResponse> GetAllInfoForQuickItemDetailPage([FromBody] AllInfoForQuickItemDetailsRequest request)
        {
            return _searchService.GetAllInfoForQuickItemDetailPage(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetSuggestionList")]
        public List<string> GetSuggestionList(GetSuggestionListRequest request)
        {
            return _searchService.GetSuggestionList(request.prefixText, request.startRowIndex, request.pageSize);
        }

        [HttpPost]
        [Route("appservice/searchservice/CheckForAltFormat")]
        public async Task<AppServiceResult<List<ItemDataContract>>> CheckForAltFormat([FromBody] AltFormatRequest btKeys)
        {
            return await _searchService.CheckForAltFormatAsync(btKeys);
        }

        [HttpPost]
        [Route("appservice/searchservice/QuickItemDetailStockCheck")]
        public AppServiceResult<string> QuickItemDetailStockCheck(InventoryStatusArgRequest request)
        {
            return _searchService.QuickItemDetailStockCheck(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductDetailsInfoForQuickItemDetailPage")]
        public async Task<AppServiceResult<ItemDetailReturn>> GetProductDetailsInfoForQuickItemDetailPage([FromBody] ProductDetailsInfoForQuickItemDetailRequest request)
        {
            return await _searchService.GetProductDetailsInfoForQuickItemDetailPageAsync(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/DataFixSendEmailToBt")]
        public async Task<AppServiceResult<string>> DataFixSendEmailToBt([FromBody] DataFixSendEmailToBtRequest request)
        {
            return await _searchService.DataFixSendEmailToBtAsync(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/DataFixPersitUserNoteToSession")]
        public async Task<AppServiceResult<string>> DataFixPersitUserNoteToSession(DataFixPersitUserNoteRequest request)
        {
            return await _searchService.DataFixPersitUserNoteToSessionAsync(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/SaveGridFieldsMyPreferenceForNewGrid")]
        public async Task<AppServiceResult<string>> SaveGridFieldsMyPreferenceForNewGrid(GridFieldsMyPreferenceForNewGridRequest request)
        {
            return await _searchService.SaveGridFieldsMyPreferenceForNewGridAsync(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/ResetCacheAndSearch")]
        public async Task<bool> ResetCacheAndSearch([FromBody] SimpleOne obj)
        {
            return await _searchService.ResetCacheAndSearch(obj.Param1, obj.UserId);
        }


        [HttpPost]
        [Route("appservice/searchservice/FindAttibute")]
        public async Task<AppServiceResult<ObjectWithTotalPage<List<SiteTermObject>>>> FindAttibute([FromBody] SiteTermRequest request)
        {
            try
            {
                if (request == null) return null;

                request.bSort = false;
                request.byValue = false;
                return await _searchService.GetSiteTermWithSortParameter(request);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetSiteTerm")]
        public async Task<AppServiceResult<ObjectWithTotalPage<List<SiteTermObject>>>> GetSiteTerm([FromBody] SiteTermRequest request)
        {
            try
            {
                if (request == null) return null;

                request.bSort = false;
                request.byValue = false;
                return await _searchService.GetSiteTermWithSortParameter(request);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetSiteTermSortedByValue")]
        public async Task<AppServiceResult<ObjectWithTotalPage<List<SiteTermObject>>>> GetSiteTermSortedByValue([FromBody] SiteTermRequest request)
        {
            try
            {
                if (request == null) return null;

                request.bSort = true;
                request.byValue = true;
                return await _searchService.GetSiteTermWithSortParameter(request);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetSiteTermWithSortParameter")]
        public async Task<AppServiceResult<ObjectWithTotalPage<List<SiteTermObject>>>> GetSiteTermWithSortParameter([FromBody] SiteTermRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<ObjectWithTotalPage<List<SiteTermObject>>>();

                if (request == null) return ajaxResult;

                request.byValue = false;

                return await _searchService.GetSiteTermWithSortParameter(request);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }
        [HttpPost]
        [Route("appservice/searchservice/GetListSiteTermWithSort")]
        public async Task<AppServiceResult<List<KeyValuePair<string, ObjectWithTotalPage<List<SiteTermObject>>>>>> GetListSiteTermWithSort([FromBody] ListSiteTermRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<List<KeyValuePair<string, ObjectWithTotalPage<List<SiteTermObject>>>>>();
                ajaxResult.Data = new List<KeyValuePair<string, ObjectWithTotalPage<List<SiteTermObject>>>>();
                if (request == null) return ajaxResult;

                request.byValue = false;
                foreach (var st in request.StList)
                {
                    var temp = await _searchService.GetSiteTermWithSortParameter(new SiteTermRequest(st, request));
                    if (temp != null)
                        ajaxResult.Data.Add(new KeyValuePair<string, ObjectWithTotalPage<List<SiteTermObject>>>(st, temp.Data));
                }

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetPricingAndPromoForSearch")]
        public AppServiceResult<WCFObjectReturnToClient> GetPricingAndPromoForSearch(SearchRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<WCFObjectReturnToClient>();
                ajaxResult.Data = _searchService.GetPricingAndPromoForSearch(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetDupCheckForSearch")]
        public async Task<AppServiceResult<WCFObjectReturnToClient>> GetDupCheckForSearch(SearchRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<WCFObjectReturnToClient>();
                ajaxResult.Data = await _searchService.GetDupCheckForSearch(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetInventoryForSearch")]
        public AppServiceResult<WCFObjectReturnToClient> GetInventoryForSearch(SearchRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<WCFObjectReturnToClient>();
                ajaxResult.Data = _searchService.GetInventoryForSearch(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetUserEditableFieldsForSearch")]
        public async Task<AppServiceResult<WCFObjectReturnToClient>> GetUserEditableFieldsForSearch(SearchRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<WCFObjectReturnToClient>();
                ajaxResult.Data = await _searchService.GetUserEditableFieldsForSearch(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetEnhancedContentIconsForSearch")]
        public async Task<AppServiceResult<WCFObjectReturnToClient>> GetEnhancedContentIconsForSearch(SearchRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<WCFObjectReturnToClient>();
                ajaxResult.Data = await _searchService.GetEnhancedContentIconsForSearch(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetPricingAndPromoForAltFormats")]
        public AppServiceResult<PricingAndPromoForAltFormatsResponse> GetPricingAndPromoForAltFormats(PricingForProductsRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<PricingAndPromoForAltFormatsResponse>();
                ajaxResult.Data = _searchService.GetPricingAndPromoForAltFormats(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetDupCheckForAltFormats")]
        public async Task<AppServiceResult<List<SiteTermObject>>> GetDupCheckForAltFormats(DupCheckForAltFormatsRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<List<SiteTermObject>>();
                ajaxResult.Data = await _searchService.GetDupCheckForAltFormats(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetInventoryForAltFormats")]
        public AppServiceResult<InventoryForAltFormatsResponse> GetInventoryForAltFormats(InventoryForAltFormatsRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<InventoryForAltFormatsResponse>();
                ajaxResult.Data = _searchService.GetInventoryForAltFormats(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetUserEditableFieldsForAltFormats")]
        public async Task<AppServiceResult<UserEditableFieldsForAltFormatsResponse>> GetUserEditableFieldsForAltFormats(UserEditableFieldsForAltFormatsRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<UserEditableFieldsForAltFormatsResponse>();
                ajaxResult.Data = await _searchService.GetUserEditableFieldsForAltFormats(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetEnhancedContentIconsForAltFormats")]
        public AppServiceResult<EnhancedContentIconsForAltFormatsResponse> GetEnhancedContentIconsForAltFormats(EnhancedContentIconsForAltFormatsRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<EnhancedContentIconsForAltFormatsResponse>();
                ajaxResult.Data = _searchService.GetEnhancedContentIconsForAltFormats(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/RemoveSearchSelection")]
        public AppServiceResult<string> RemoveSearchSelection([FromBody] SimpleThree selection)
        {
            //var sitecontext = selection.SiteContext;
            return _searchService.RemoveSearchSelection(selection.Param1, selection.Param2,selection.Param3, selection.UserId);
        }
        [HttpPost]
        [Route("appservice/searchservice/RemoveSearchFacet")]
        public AppServiceResult<string> RemoveSearchFacet([FromBody] SimpleOneInt obj)
        {
            //var sitecontext = selection.SiteContext;
            return _searchService.RemoveSearchFacet(obj.Param1, obj.UserId);
        }

        [HttpPost]
        [Route("appservice/searchservice/ToggleMyPreferences")]
        public AppServiceResult<bool> ToggleMyPreferences([FromBody] BaseSimple selection)
        {
            return _searchService.ToggleMyPreferences(selection.UserId);
        }

        [HttpPost]
        [Route("appservice/searchservice/ClearSearchFilterSession")]
        public AppServiceResult<bool> ClearSearchFilterSession([FromBody] BaseSimple selection)
        {
            return _searchService.ClearSearchFilterSession(selection.UserId);
        }

        [HttpPost]
        [Route("appservice/searchservice/LoadFacet")]
        public AppServiceResult<List<SearchFacetNode>> LoadFacet(LoadFacetRequest request)
        {
            var ajaxResult = new AppServiceResult<List<SearchFacetNode>>
                             {
                                 Data =
                                     _searchService.LoadSearchFacet(request.Param1, request.OrgId, request.UserId, request.MarketType,
                                         request.ProductType, request.SearchData, request.SearchArguments, request.IsWfeFarmCacheAvailable)
                             };
            return ajaxResult;
        }

        [HttpPost]
        [Route("appservice/searchservice/RefineSearch")]
        public AppServiceResult<string> RefineSearch([FromBody] ListSimpleTwo obj)
        {
            return _searchService.RefineSearch(obj.List, obj.UserId);
        }
        [HttpPost]
        [Route("appservice/searchservice/SearchArtifacts")]
        public AppServiceResult<string> SearchArtifacts([FromBody] ListSimpleTwo obj)
        {
            return _searchService.RefineSearch(obj.List, obj.UserId,true);
        }
        [HttpPost]
        [Route("appservice/searchservice/GetAdvSearchFilter")]
        public AppServiceResult<Dictionary<string, string>> GetAdvSearchFilter([FromBody] SimpleOne obj)
        {
            return _searchService.GetAdvSearchFilter(obj.Param1, obj.UserId);
        }

        [HttpPost]
        [Route("appservice/searchservice/InsertSavedSearch")]
        public async Task<AppServiceResult<string>> InsertSavedSearch([FromBody] KeyValuePair<string, string> obj)
        {
            return await _searchService.InsertSavedSearch(obj.Key, obj.Value);
        }

        [HttpPost]
        [Route("appservice/searchservice/EditSavedSearch")]
        public async Task<AppServiceResult<bool>> EditSavedSearch([FromBody] SimpleTwoArrStr obj)
        {
            return await _searchService.EditSavedSearch(obj.Param1, obj.Param2, obj.UserId);
        }

        [HttpPost]
        [Route("appservice/searchservice/RunSavedSearch")]
        public async Task<AppServiceResult<string>> RunSavedSearch([FromBody] SimpleTwoArrStr obj)
        {
            return await _searchService.RunSavedSearch(obj.Param1, obj.Param2, obj.UserId);
        }

        [HttpPost]
        [Route("appservice/searchservice/DeleteSavedSearch")]
        public async Task<AppServiceResult<bool>> DeleteSavedSearch([FromBody] KeyValuePair<string, string> obj)
        {
            return await _searchService.DeleteSavedSearch(obj.Key, obj.Value);
        }

        [HttpPost]
        [Route("appservice/searchservice/UpdateSavedSearchById")]
        public async Task<AppServiceResult<bool>> UpdateSavedSearchById([FromBody] KeyValuePair<string, string> obj)
        {
            return await _searchService.UpdateSavedSearchById(obj.Key, obj.Value);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductDuplicateIndicator")]
        public async Task<AppServiceResult<List<SiteTermObject>>> GetProductDuplicateIndicator([FromBody] ProductDuplicateIndicatorRequest request)
        {
            var result = new AppServiceResult<List<SiteTermObject>>();
            try
            {

                return await _searchService.GetProductDuplicateIndicator(request.btKeyList, request.btEKeyList, request.basketId,
                    request.isRequiredCheckDupCarts, request.isRequiredCheckDupOrder, request.UserId, request.DefaultDownloadedCarts,
                    request.CollectionAnalysisEnabled, request.OrgId);
            }
            catch (Exception ex)
            {
                result.Data = new List<SiteTermObject>();
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetInventoryDemandItemDetail")]
        public async Task<AppServiceResult<ItemDetailPrimaryInfoReturn>> GetInventoryDemandItemDetail(InventoryDemandItemDetail request)
        {
            var result = new AppServiceResult<ItemDetailPrimaryInfoReturn>();
            
            if (request == null)
            {
            }
            else
            {
                try
                {
                    return await _searchService.GetInventoryDemandItemDetail(request);
                }
                catch (Exception ex)
                {
                    result.Data = null;
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = ex.Message;
                    Logger.RaiseException(ex, ExceptionCategory.Search);
                }
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetPricingItemDetail")]
        public async Task<AppServiceResult<ItemDetailPrimaryInfoReturn>> GetPricingItemDetail(PrimaryInfoItemDetailRequest request)
        {
            var result = new AppServiceResult<ItemDetailPrimaryInfoReturn>();
            try
            {
                return await _searchService.GetPricingItemDetail(request);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetGridLineCount")]
        public async Task<AppServiceResult<ItemDetailPrimaryInfoReturn>> GetGridLineCount(GridLineCount request)
        {
            var result = new AppServiceResult<ItemDetailPrimaryInfoReturn>();
            try
            {
                return await _searchService.GetGridLineCount(request);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetDupIcons")]
        public async Task<AppServiceResult<ItemDetailSecondaryInfoReturn>> GetDupIcons(DupIcons request)
        {
            var result = new AppServiceResult<ItemDetailSecondaryInfoReturn>();
            try
            {
                return await _searchService.GetDupIcons(request);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetAdditionalInfoItemDetails")]
        public async Task<AppServiceResult<ItemDetailSecondaryInfoReturn>> GetAdditionalInfoItemDetails(SecondaryInfoItemDetailRequest request)
        {
            var result = new AppServiceResult<ItemDetailSecondaryInfoReturn>();
            try
            {
                return await _searchService.GetAdditionalInfoItemDetails(request);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetHomeLandingInventoryData")]
        public AppServiceResult<InventoryDataContract> GetHomeLandingInventoryData(InventoryStatusClientRequest request)
        {
            var result = new AppServiceResult<InventoryDataContract>();
            try
            {
                return _searchService.GetHomeLandingInventoryData(request);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/QuickSearchToggleProductImages")]
        public AppServiceResult<bool> QuickSearchToggleProductImages(QuickSearchToggleProductImagesRequest request)
        {
            var result = new AppServiceResult<bool>();
            try
            {
                _searchService.ToggleProductImages(request.UserId, 2);
                result.Data = true;
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/ToggleQuickView")]
        public AppServiceResult<bool> ToggleQuickView(ToggleQuickViewRequest request)
        {
            var result = new AppServiceResult<bool>();
            try
            {
                return _searchService.ToggleQuickView(request);
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/ToggleProductImage")]
        public AppServiceResult<bool> ToggleProductImage(ToggleProductImageRequest request)
        {
            var result = new AppServiceResult<bool>();
            try
            {
                _searchService.ToggleProductImages(request.UserId, request.inPageEnumValue);
                result.Data = true;
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetEncryptQueryString")]
        public async Task<AppServiceResult<string>> GetEncryptQueryString(GetEncryptQueryRequest request)
        {
            var result = new AppServiceResult<string>();
            result.Status = AppServiceStatus.Success;
            try
            {
                result.Data = await _searchService.GetEncryptQueryString(request);
                return result;
            }
            catch (Exception ex)
            {
                result.Data = "";
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetDataForLandingFirstLoad")]
        public AppServiceResult<LandingPageContract> GetDataForLandingFirstLoad(TargetingRequest request)
        {
            var result = new AppServiceResult<LandingPageContract> { Status = AppServiceStatus.Success };
            try
            {
                result.Data = _searchService.GetDataForLandingFirstLoad(request.Targeting);
                return result;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }
        [HttpPost]
        [Route("appservice/searchservice/GetCollectionData")]
        public AppServiceResult<TwilightListContract> GetCollectionData(CollectionData request)
        {
            var result = new AppServiceResult<TwilightListContract> { Status = AppServiceStatus.Success };
            try
            {
                result.Data = _searchService.GetCollectionData(request);
                return result;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }
        [HttpPost]
        [Route("appservice/searchservice/GetComingSoonData")]
        public AppServiceResult<TwilightListContract> GetComingSoonData(CollectionData request)
        {
            return _searchService.GetComingSoonData(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetActiveCarts")]
        public async Task<JsonResult<GetActiveCartsContract>> GetActiveCarts(GetActiveCartsRequest request)
        {
            try
            {
                var activeCartsString = await _searchService.GetActiveCarts(request.context);
                return Json<GetActiveCartsContract>(new GetActiveCartsContract { d = activeCartsString });
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return null;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetActiveNewestBaskets")]
        public async Task<AppServiceResult<ActiveNewestBasketsResponse>> GetActiveNewestBaskets(ActiveNewestBasketsRequest request)
        {
            var result = new AppServiceResult<ActiveNewestBasketsResponse> { Status = AppServiceStatus.Success };
            try
            {
                result.Data = await _searchService.GetActiveNewestBaskets(request);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.General);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetCartListPopUpForOriginalEntry")]
        public async Task<JsonResult<GetActiveCartsContract>> GetCartListPopUpForOriginalEntry(GetActiveCartsRequest request)
        {
            try
            {
                var context = request.context;

                var contextValue = context.Value; // BTKey;UserId;CartId;NumberOfCatr;copyMoveAction
                var arrayString = contextValue.Split(';');

                var userId = arrayString[1];

                var activeCartsString = await _searchService.GetCartListPopUpForOriginalEntry(userId, 5);
                return Json<GetActiveCartsContract>(new GetActiveCartsContract { d = activeCartsString });
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return null;
        }

        [HttpPost]
        [Route("appservice/searchservice/AddTitlesToCartNameWithGrid")]
        public async Task<AppServiceResult<AddToCartStatusObject>> AddTitlesToCartNameWithGrid(AddTitlesToCartNameWithGridRequest request)
        {
            try
            {
                return await _searchService.AddTitlesToCartNameWithGrid(request);
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return null;
        }

        [HttpPost]
        [Route("appservice/searchservice/AddAllToCart")]
        public AppServiceResult<AddToCartStatusObject> AddAllToCart(AddAllToCartRequest request)
        {
            try
            {
                return _searchService.AddAllToCart(request);
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return null;
        }
        [HttpPost]
        [Route("appservice/searchservice/ProcessAddAllExceed500ToCart")]
        public AppServiceResult<bool> ProcessAddAllExceed500ToCart(AddAllToCartRequest request)
        {
            try
            {
                return _searchService.ProcessAddAllExceed500ToCart(request);
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return null;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetEnhancedContentForSearch")]
        public async Task<AppServiceResult<WCFObjectReturnToClient>> GetEnhancedContentForSearch(GetEnhancedContentForSearchRequest request)
        {
            var result = new AppServiceResult<WCFObjectReturnToClient> { Status = AppServiceStatus.Success };
            try
            {
                result.Data = await _searchService.GetEnhancedContentForSearch(request);
                return result;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetUserAlertCount")]
        public AppServiceResult<string> GetUserAlertCount(GetUserAlertCountRequest request)
        {
            try
            {
                string userId;
                if (request != null && !string.IsNullOrEmpty(request.userId))
                    userId = request.userId;
                else
                    userId = ServiceRequestHeader.RequestUserId;

                return new AppServiceResult<string>
                {
                    Data = _searchService.GetUserAlertCount(userId),
                    ErrorMessage = "",
                    Status = AppServiceStatus.Success
                };
            }
            catch (Exception e)
            {
                Logger.RaiseException(e, ExceptionCategory.General);
                return new AppServiceResult<string>
                {
                    Data = null,
                    ErrorMessage = "Cannot get data.",
                    Status = AppServiceStatus.Fail
                };
            }
        }

        //
        // put profile update in searchcontroller, will confirm the approach with the code review design
        //
        [HttpPost]
        [Route("appservice/searchservice/UpdateTSSONotificationCartUsers")]
        public async Task<AppServiceResult<bool>> UpdateTSSONotificationCartUsers([FromBody] UpdateNotificationCartUsersRequest request)
        {
            try
            {
                return await _searchService.UpdateTSSONotificationCartUsers(request.ActiveNotificationCartUsers, request.RemovedNotificationCartUsers);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.User.ToString());
                return null;
        }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetInventoryForTitleList")]
        public AppServiceResult<WCFObjectReturnToClient> GetInventoryForTitleList(SearchRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<WCFObjectReturnToClient>();
                ajaxResult.Data = _searchService.GetInventoryForTitleList(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetDupCheckForTitleList")]
        public async Task<AppServiceResult<WCFObjectReturnToClient>> GetDupCheckForTitleList(SearchRequest request)
        {
            try
            {
                var ajaxResult = new AppServiceResult<WCFObjectReturnToClient>();
                ajaxResult.Data = await _searchService.GetDupCheckForTitleList(request);

                return ajaxResult;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, ExceptionCategory.Search.ToString());
                return null;
            }
        }

        #region private

        #endregion

        [HttpPost]
        [Route("appservice/searchservice/GetDetailsInfoAndGridLineCount")]
        public async Task<AppServiceResult<ItemDetailPrimaryInfoReturn>> GetDetailsInfoAndGridLineCount(SecondaryInfoItemDetailRequest request)
        {
            var result = new AppServiceResult<ItemDetailPrimaryInfoReturn>();
            try
            {
                result.Data = null;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetCalendarData")]
        public AppServiceResult<GetCalendarDataResponse> GetCalendarData(GetCalendarDataRequest request)
        {
            var result = new AppServiceResult<GetCalendarDataResponse>();
            result.Status = AppServiceStatus.Success;
            try
            {
                result.Data = _searchService.GetCalendarData(request);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(ex, ExceptionCategory.General);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetNRCFeaturedProducts")]
        public AppServiceResult<GetNRCFeaturedProductsResponse> GetNRCFeaturedProducts(GetNRCFeaturedProductsRequest request)
        {
            var result = new AppServiceResult<GetNRCFeaturedProductsResponse>();
            result.Status = AppServiceStatus.Success;
            try
            {
                result.Data = _searchService.GetNRCFeaturedProducts(request);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(ex, ExceptionCategory.General);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetNRCFeaturedProductsByBTKeys")]
        public AppServiceResult<GetNRCFeaturedProductsResponse> GetNRCFeaturedProductsByBTKeys(FeaturedProductsByBTKeysRequest request)
        {
            var result = new AppServiceResult<GetNRCFeaturedProductsResponse>();
            result.Status = AppServiceStatus.Success;
            try
            {
                result.Data = _searchService.GetNRCFeaturedProductsByBTKeys(request.BTKeys);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(ex, ExceptionCategory.General);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetNRCAltFormats")]
        public AppServiceResult<NRCAltFormatsResponse> GetNRCAltFormats(NRCAltFormatsRequest request)
        {
            var result = new AppServiceResult<NRCAltFormatsResponse>();
            result.Status = AppServiceStatus.Success;
            try
            {
                result.Data = _searchService.GetNRCAltFormats(request);
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetNRCAltFormats");
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetPricingForProducts")]
        public AppServiceResult<PricingForProductsResponse> GetPricingForProducts(PricingForProductsRequest request)
        {
            var result = new AppServiceResult<PricingForProductsResponse>();
            result.Status = AppServiceStatus.Success;
            try
            {
                result.Data = _searchService.GetPricingForProducts(request);
            }
            catch (Exception ex)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.WriteLog(ex, "GetNRCAltFormats");
            }
            return result;
        }
    }

    public class GetActiveCartsContract
    {
        // Telerik's convention: wrap data in "d" property
        public string d { get; set; }
    }
}
