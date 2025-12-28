using System;
using System.Collections.Generic;
using System.Web.Http;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.Common.Models;
using BT.TS360API.Common;
using BT.TS360API.Common.Constants;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using BT.TS360API.Common.Business;
using System.Web;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360API.ServiceContracts.Product;
using Microsoft.Security.Application;
using BT.TS360API.Cache;
using System.Threading.Tasks;
using System.Linq;
using BT.TS360SP;
using BT.TS360API.ServiceContracts.Profiles;
using TS360Constants;
using BT.TS360API.Common.Search.Helpers;
using BT.TS360API.Common.CartFramework;
using BT.TS360API.Common.Search;
using BT.TS360API.Common.Pricing;

namespace BT.TS360API.SearchService.Controllers
{
    public partial class SearchController : ApiController
    {
        [HttpPost]
        [Route("appservice/searchservice/QuickSearchAddProductsToCart")]
        public async Task<AppServiceResult<AddToCartStatusObject>> QuickSearchAddProductsToCart(QuickSearchAddProductsToCart request)
        {
            return await _searchService.QuickSearchAddProductsToCart(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductExcerpts")]
        public AppServiceResult<string> GetProductExcerpts(GetProductExcerptsRequest request)
        {
            try
            {
                var result = new AppServiceResult<string>();


                result.Data = ProductDAOManager.Instance.GetProductExcerpts(request.key);
                result.Status = AppServiceStatus.Success;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductTOC")]
        public AppServiceResult<string> GetProductTOC(GetProductTOCRequest request)
        {
            try
            {
                var result = new AppServiceResult<string>();
                // Get TOC from Content Cafe
                var data = ProductDAOManager.Instance.GetProductTOC(request.key);

                result.Data = data;
                result.Status = AppServiceStatus.Success;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductReview")]
        public AppServiceResult<List<AdditionContent>> GetProductReview(GetProductReviewRequest request)
        {
            try
            {
                var result = new AppServiceResult<List<AdditionContent>>();
                result.Data = ProductDAOManager.Instance.GetProductReview(request.key, request.OrgId, request.UserId);
                result.Status = AppServiceStatus.Success;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductMuze")]
        public AppServiceResult<string> GetProductMuze(GetProductMuzeRequest request)
        {
            try
            {
                var result = new AppServiceResult<string>();
                var muze = ProductDAOManager.Instance.GetProductMuzeFromContentCafe(request.key);
                result.Data = muze;
                result.Status = AppServiceStatus.Success;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductAdditionAnnotation")]
        public AppServiceResult<List<AdditionContent>> GetProductAdditionAnnotation(GetProductAdditionAnnotationRequest request)
        {
            try
            {
                var result = new AppServiceResult<List<AdditionContent>>();
                var productAnnotations = ProductDAOManager.Instance.GetProductAnnos(request.key);
                //
                foreach (var additionalContent in productAnnotations)
                {
                    additionalContent.Content = HttpUtility.HtmlDecode(additionalContent.Content);
                }
                //
                result.Data = productAnnotations;
                result.Status = AppServiceStatus.Success;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductAltFormats")]
        public AppServiceResult<ProductAltFormatsResponse> GetProductAltFormats(ProductAltFormatsRequest request)
        {
            var result = new AppServiceResult<ProductAltFormatsResponse>();
            try
            {
                //
                result.Data = _searchService.GetProductAltFormats(request);
                result.Status = AppServiceStatus.Success;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }

            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductFlapCopy")]
        public AppServiceResult<List<AdditionContent>> GetProductFlapCopy(GetProductFlapCopyRequest request)
        {
            try
            {
                var result = new AppServiceResult<List<AdditionContent>>();
                var flapCopy = ProductDAOManager.Instance.GetProductFlapCopy(request.btKey);
                //
                foreach (var additionalContent in flapCopy)
                {
                    additionalContent.Content = HttpUtility.HtmlDecode(additionalContent.Content);
                }
                //
                result.Data = flapCopy;
                result.Status = AppServiceStatus.Success;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                return null;
            }
        }


        [HttpPost]
        [Route("appservice/searchservice/GetProductBiographies")]
        public AppServiceResult<List<AdditionContent>> GetProductBiographies(GetProductBiographiesRequest request)
        {
            try
            {
                var result = new AppServiceResult<List<AdditionContent>>();
                var biographies = ProductDAOManager.Instance.GetProductBiographies(request.btKey);
                if (biographies != null && biographies.Count > 0)
                {
                    foreach (var additionalContent in biographies)
                    {
                        additionalContent.Content = HttpUtility.HtmlDecode(additionalContent.Content);
                    }

                }
                result.Data = biographies;
                result.Status = AppServiceStatus.Success;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Search);
                return null;
            }
        }

        [HttpPost]
        [Route("appservice/searchservice/GetProductReviewIndicator")]
        public AppServiceResult<List<SiteTermObject>> GetProductReviewIndicator(GetProductReviewIndicatorRequest request)
        {
            var result = new AppServiceResult<List<SiteTermObject>>();
            try
            {
                //var siteTermObjects = ProductSearchController.CheckProductReviewsFromOds(btKeyList.Where(x => x.Length <= 10).ToList());//skip OE
                var siteTermObjects = ProductDAOManager.Instance.CheckProductReviewsFromOds(request.BTKeyList.Where(x => x.Length <= 10).ToList(), request.UserId);//skip OE
                result.Data = siteTermObjects;
                result.Status = AppServiceStatus.Success;
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
        [Route("appservice/searchservice/GetArtifactsByWork")]
        public AppServiceResult<List<ArtifactItems>> GetArtifactsByWork(ArtifactRequest request)
        {
            var result = new AppServiceResult<List<ArtifactItems>>();
            try
            {
                var searchService = new BT.TS360API.SearchService.Services.SearchService();
                result = searchService.GetArtifactsByWork(request.WorkId);
            }
            catch (Exception ex)
            {
                result.Data = new List<ArtifactItems>();
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetStockPriceCheckForQuickSearch")]
        public AppServiceResult<WCFObjectReturnToClient> GetStockPriceCheckForQuickSearch(GetStockPriceCheckForQuickSearchRequest request)
        {
            var result = new AppServiceResult<WCFObjectReturnToClient>();
            try
            {
                var searchService = new BT.TS360API.SearchService.Services.SearchService();
                result = searchService.GetStockPriceCheckForQuickSearch(request.btKey, request.productType,
                    request.ObjContext.UserId, request.Targeting.MarketType, request.ObjContext.ESuppliers,
                    request.ObjContext.SimonSchusterEnabled, request.ObjContext.CountryCode, request.Targeting.AudienceType,
                    request.AccountPricing.EnableProcessingCharges, request.ObjContext.IsHideNetPriceDiscountPercentage,
                    request.AccountPricing.BookProcessingCharge, request.AccountPricing.MovieProcessingCharge,
                    request.AccountPricing.PaperbackProcessingCharge, request.AccountPricing.MusicProcessingCharge,
                    request.AccountPricing.SpokenWordProcessingCharge, request.AccountPricing.SalesTax,
                    request.ObjContext.DefaultBookAccountId, request.ObjContext.DefaultEntertainmentAccountId, request.ObjContext.DefaultVIPAccountId,
                    request.ObjContext.OrgId, request.Targeting, request.AccountPricing
                    );
            }
            catch (Exception ex)
            {
                result.Data = new WCFObjectReturnToClient();
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetSearchItemDetailsNavBarInfo")]
        public AppServiceResult<SearchItemDetailsNavBarInfoResponse> GetSearchItemDetailsNavBarInfo(SearchItemDetailsNavBarInfoRequest request)
        {
            var result = new AppServiceResult<SearchItemDetailsNavBarInfoResponse>();
            var searchService = new BT.TS360API.SearchService.Services.SearchService();
            result = searchService.GetSearchItemDetailsNavBarInfo(request);

            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetCartItemDetailsNavBarInfo")]
        public AppServiceResult<CartItemDetailsNavBarInfo> GetCartItemDetailsNavBarInfo(CartItemDetailsNavBarInfoRequest request)
        {
            var result = new AppServiceResult<CartItemDetailsNavBarInfo>();
            try
            {
                var searchService = new BT.TS360API.SearchService.Services.SearchService();
                result = searchService.GetCartItemDetailsNavBarInfo(request);
            }
            catch (Exception ex)
            {
                result.Data = new CartItemDetailsNavBarInfo();
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.Search);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetAllInfoBTListsPage")]
        public AppServiceResult<BTListReturn> GetAllInfoBTListsPage(BTListsArgRequest request)
        {
            var result = new AppServiceResult<BTListReturn>();
            try
            {
                var dataReturn = new BTListReturn();
                var targeting = string.Empty;
                const string postName = "EListSubCategory";
                if (request != null && request.arg != null)
                {
                    foreach (int id in request.arg.EListCatIds)
                        targeting += id;

                    var cache = CachingController.Instance.Read(targeting + postName) as List<EListCategory>;

                if (cache != null)
                {
                    dataReturn.EListCats = cache;
                }
                else
                {
                    var elists = new List<EListCategory>();
                        var subCategories =
                            ContentManagementController.Current.GetEListSubCategoriesItem(request.arg.EListCatIds);
                        var eList =
                            ContentManagementController.Current.GetEListBySubcategory(
                                subCategories.Select(sub => sub.Id).ToList());
                    foreach (var eListCatId in request.arg.EListCatIds)
                    {
                            var id = eListCatId;
                            var subCats = subCategories.Where(sub => sub.EListCategoryID == id).Select(sub => sub.Id);
                            var eItems =
                                eList.Where(
                                    elist =>
                                        elist.EListSubcategoryID != null &&
                                        subCats.Contains(elist.EListSubcategoryID.Value));
                            elists.Add(new EListCategory(id, eItems.Count()));

                    }
                    dataReturn.EListCats = elists;

                        CachingController.Instance.Write(targeting + postName, elists, CommonConstants.CmCachingDuration);
                    }
                }
                result.Status = AppServiceStatus.Success;
                result.Data = dataReturn;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(exception, ExceptionCategory.BTListsPage);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/SearchCdmsAdditionalUsers")]
        public AppServiceResult<List<UserProfileCdms>> SearchCdmsAdditionalUsers(SearchCdmsAdditionalUsersRequest request)
        {
            var result = new AppServiceResult<List<UserProfileCdms>> { Status = AppServiceStatus.Success };
            try
            {
                var cacheKey = string.Format("SearchCdmsAdditionalUsers{0}", request.UserId);
                int batchSize = 100;
                List<UserProfileCdms> userList;

                if (request.batchNo == 1)
                {
                    userList = CdmsDAOManager.Instance.GetAdditionalUsers(request.cdmsListId, request.keyword);
                    Cache.CachingController.Instance.Write(cacheKey, userList);
                }
                else
                {
                    userList = Cache.CachingController.Instance.Read(cacheKey) as List<UserProfileCdms>;
                }

                if (userList == null) userList = new List<UserProfileCdms>();

                double totalBatch = Math.Ceiling(((double)(userList.Count)) / batchSize);

                result.Data = userList.Skip((request.batchNo - 1) * batchSize).Take(batchSize).ToList();
                if (request.batchNo == totalBatch)
                {
                    result.ErrorMessage = "0";
                    Cache.CachingController.Instance.SetExpired(cacheKey);
                }
                else
                {
                    result.ErrorMessage = "1";
                }
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/SendCdmsListDistribution")]
        public AppServiceResult<bool> SendCdmsListDistribution(SendCdmsListDistributionRequest request)
        {
            var result = new AppServiceResult<bool> { Status = AppServiceStatus.Success };
            try
            {
                string additionalUserIdsString = request.additionalUserIds != null
                                                     ? string.Join(";", request.additionalUserIds.ToArray())
                                                     : string.Empty;
                string checkedUserIdsString = request.checkedUserIds != null
                                                     ? string.Join(";", request.checkedUserIds.ToArray())
                                                     : string.Empty;
                //CdmsController.Current.SendCdmsList(cdmsListId, additionalUserIdsString, checkedUserIdsString, allIndicator);
                CdmsDAOManager.Instance.SendCdmsList(request.cdmsListId, additionalUserIdsString, checkedUserIdsString, request.allIndicator);
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
            }
            return result;
        }


        //public AjaxServiceResult<List<UserProfile>> GetCdmsListUserPaging(string cdmsListId, int pageSize, int pageIndex, string sortBy, string sortDirection)
        [HttpPost]
        [Route("appservice/searchservice/GetCdmsListUserPaging")]
        public AppServiceResult<List<UserProfileCdms>> GetCdmsListUserPaging(GetCdmsListUserPagingRequest request)
        {
            //var result = new AjaxServiceResult<List<UserProfile>> { Status = AjaxServiceStatus.Success };
            var result = new AppServiceResult<List<UserProfileCdms>> { Status = AppServiceStatus.Success };
            try
            {
                //var userList = CdmsController.Current.GetUserList(cdmsListId, pageSize, pageIndex, sortBy, sortDirection);
                var userList = CdmsDAOManager.Instance.GetUserList(request.cdmsListId, request.pageSize, request.pageIndex, request.sortBy, request.sortDirection);
                result.Data = userList;
            }
            catch (Exception exception)
            {
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = CommonErrorMessage.UnexpectedErrorMessage;
                Logger.RaiseException(exception, ExceptionCategory.ItemDetails);
            }
            return result;
        }

        [HttpPost]
        [Route("appservice/searchservice/GetWhatHotAndPopularFeedData")]
        public AppServiceResult<GetWhatHotAndPopularFeedDataResponse> GetWhatHotAndPopularFeedData(TargetingRequest request)
        {
            var result = new AppServiceResult<GetWhatHotAndPopularFeedDataResponse>
            {
                Data = null,
                ErrorMessage = string.Empty,
                Status = AppServiceStatus.Fail
            };

            try
            {
                if (request != null)
                {
                    var wh = _searchService.GetWhatHotData(request.Targeting);
                    if (wh == null)
                    {
                        wh = new WhatHotLandingPage();
                    }

                    wh.LitProductDescription1 = wh.LitProductDescription1 ?? "";
                    wh.LitProductDescription1 = wh.LitProductDescription1.Left(300);

                    wh.LitProductDescription2 = wh.LitProductDescription2 ?? "";
                    wh.LitProductDescription2 = wh.LitProductDescription2.Left(300);

                    result.Data = new GetWhatHotAndPopularFeedDataResponse();
                    result.Data.WhatHot = wh;

                    var pf = _searchService.GetPopularFeedData(request.Targeting);
                    if (pf == null)
                    {
                        pf = new PopularFeedLandingPage();
                    }

                    result.Data.PopularFeed = pf;

                    result.Status = AppServiceStatus.Success;
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.General);
                result.Status = AppServiceStatus.Fail;
                //result.ErrorMessage = GetLocalizedString("ProfileResources", "UnexpectedError");
                result.ErrorMessage = ProfileResources.UnexpectedError;
            }

            return result;
        }
    }
}
