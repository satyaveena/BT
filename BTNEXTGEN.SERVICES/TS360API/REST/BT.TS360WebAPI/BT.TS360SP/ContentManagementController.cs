using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using BT.TS360API.Cache;
using BT.TS360API.Marketing;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class ContentManagementController 
    {
        #region Private

        private static volatile ContentManagementController _instance;
        private static readonly object SyncRoot = new Object();

        private ContentManagementController()
        {
        }

        #endregion

        /// <summary>
        /// Gets the Content Management Controller for the current request.
        /// </summary>
        public static ContentManagementController Current
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ContentManagementController();
                }

                return _instance;
            }
            //get
            //{
            //    var controller = HttpContext.Current.Items[RequestCacheKey.ContentManagementControllerCacheKey] as ContentManagementController;

            //    if (controller == null)
            //    {
            //        controller = new ContentManagementController();
            //        HttpContext.Current.Items[RequestCacheKey.ContentManagementControllerCacheKey] = controller;
            //    }

            //    return controller;
            //}
            //set
            //{
            //    HttpContext.Current.Items[RequestCacheKey.ContentManagementControllerCacheKey] = value;
            //}
        }



        //#region EList
        //public EListItem GeteListItemByID(int listId)
        //{
        //    try
        //    {
        //        var list = new EList() { ID = listId };
        //        var items = list.Get();
        //        if (items != null)
        //        {
        //            return items.FirstOrDefault();
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}
        //public IList<EListCategoryItem> GetEListCategoriesItem(string keyword, string queryStringTargeting)
        //{
        //    try
        //    {
        //        var targeting = queryStringTargeting;
        //        string postName = string.Format("EListCategory_{0}", !string.IsNullOrEmpty(keyword) ? keyword : "");
        //        var cache = VelocityCacheManager.Read(targeting + postName, CommonCacheContant.Ts360FarmCacheName) as IList<EListCategoryItem>;
        //        if (cache != null)
        //            return cache;
        //        var targetingText = ContentManagementHelper.GetTargetingText(SiteContext.Current, ref targeting);

        //        var targetingParam = ContentManagementHelper.ConvertStringToTargetingParam(targetingText);

        //        var eListCates = new EListCategory { Keyword = keyword };
        //        eListCates.Targeting = targetingParam;
        //        var catesResult = eListCates.Get();
        //        foreach (var item in catesResult)
        //        {
        //            item.ItemCurrenceURL = SiteUrl.ExpandedLists + "?" + SearchFieldNameConstants.ecateid + "=" + item.Id;
        //        }

        //        VelocityCacheManager.Write(targeting + postName, catesResult, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //        return catesResult;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<EListCategoryItem>();
        //}

        //public IList<EListSubcategoryItem> GetEListSubCategoriesItem(int categoryID)
        //{
        //    try
        //    {
        //        var subCates = new EListSubcategory() { EListCategoryID = categoryID };
        //        return subCates.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<EListSubcategoryItem>();
        //}
        public IList<EListSubcategoryItem> GetEListSubCategoriesItem(List<int> categoryIDs)
        {
                var subCates = new EListSubcategory() { EListCategoryIDs = categoryIDs };
                return subCates.Get();
            }
        //public EListSubcategoryItem GetEListSubCategoriesItemByID(int eListSubCategoryID)
        //{
        //    try
        //    {
        //        var subCates = new EListSubcategory() { ID = eListSubCategoryID };
        //        var subCategories = subCates.Get();
        //        if (subCategories != null && subCategories.Count > 0)
        //        {
        //            return subCategories.FirstOrDefault();
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}
        //public IList<EListItem> GetEListBySubcategory(int subcatId)
        //{
        //    try
        //    {
        //        var eList = new EList() { EListSubcategoryID = subcatId };
        //        return eList.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<EListItem>();
        //}
        public IList<ShortEListItem> GetEListBySubcategory(List<int> subcatIds)
        {
                var eList = new ShortEList() { EListSubcategoryIDs = subcatIds };
                return eList.Get();
            }
        public IList<ShortEListItem1> GetEListBySubcategoryForHomePage(List<int> subcatIds)
        {
                var eList = new ShortEList1() { EListSubcategoryIDs = subcatIds };
                return eList.Get();
            }

        public IList<ShortEListItem1> GetEListForLandingPage(TargetingValues siteContext)
        {
                var targeting = "GetEListForLandingPage";//do not exist, get real targeting from GetTargetingText method
                MarketingHelper.Instance.GetTargetingText(siteContext, ref targeting);
                //var targetingText = ContentManagementHelper.GetTargetingText(SiteContext.Current, ref targeting);//required to get targeting
                var postName = "ShortEListItem1";
                var cache = CachingController.Instance.Read(targeting + postName) as IList<ShortEListItem1>;
                if (cache != null)
                    return cache;

                var eListCates = new ShortEListCategory();
                var catesItems = eListCates.Get();
                var subCategories = GetEListSubCategoriesItem(catesItems.Select(item => item.Id).ToList());
                var eList = GetEListBySubcategoryForHomePage(subCategories.Select(sub => sub.Id).ToList());
                CachingController.Instance.Write(targeting + postName, eList, CommonConstants.CmCachingDuration);

                return eList;
            }

        //public EListCategoryItem GetEListCategory(int eListCategoryId)
        //{
        //    try
        //    {
        //        var eListCategory = new EListCategory() { ID = eListCategoryId };
        //        var lists = eListCategory.Get();
        //        if (lists != null && lists.Count > 0)
        //        {
        //            return lists.FirstOrDefault();
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}
        //public EListItem GetEListByID(int Id)
        //{
        //    try
        //    {
        //        var eList = new EList() { ID = Id };
        //        var eListItems = eList.Get();
        //        if (eListItems != null && eListItems.Count > 0)
        //        {
        //            return eListItems.FirstOrDefault();
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}


        //public IList<FeaturedBTeListsItem> GetMostPopularItems()
        //{
        //    try
        //    {
        //        var list = new MostPopular();
        //        var result = list.Get();
        //        if (result != null)
        //        {
        //            foreach (FeaturedBTeListsItem item in result)
        //            {
        //                if (item.BTListNameID != null)
        //                {
        //                    //var eListItem = GetEListByID(item.BTListNameID.Value);
        //                    //if (eListItem != null)
        //                    //{
        //                    item.BTKeySearch = String.Format("{0}?{1}={2}", SiteUrl.EListProducts, SearchFieldNameConstants.elistid, item.BTListNameID.Value); //eListItem.ItemCurrenceURL;
        //                    //}
        //                }
        //            }
        //        }
        //        return result;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<FeaturedBTeListsItem>();
        //}
        //public IList<LibrariansCornerItem> GetLibrarianItems()
        //{
        //    try
        //    {
        //        var list = new LibrarianCorner();
        //        var result = list.Get();
        //        if (result != null)
        //        {
        //            foreach (LibrariansCornerItem item in result)
        //            {
        //                if (item.BTListNameID != null)
        //                {
        //                    var eListItem = GetEListByID(item.BTListNameID.Value);
        //                    if (eListItem != null)
        //                    {
        //                        item.BTKeySearch = eListItem.ItemCurrenceURL;
        //                    }
        //                }
        //            }
        //        }
        //        return result;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<LibrariansCornerItem>();
        //}
        //#endregion

        //#region Publication
        //public IList<PublicationItem> GetPublicationItems()
        //{
        //    try
        //    {
        //        var targeting = ContentManagementHelper.GenerateTargetingQueryStringNoOrgInfo(SiteContext.Current);
        //        const string postName = "PublicationItem";
        //        var cacheKey = string.Format("{0}__{1}", targeting, postName);
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<PublicationItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new Publication();
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);

        //        return result;

        //        //Publication list = new Publication();
        //        //return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<PublicationItem>();
        //}
        //public IList<PublicationIssueItem> GetPublicationIssueItems(int publictaionId)
        //{
        //    try
        //    {
        //        var targeting = ContentManagementHelper.GenerateTargetingQueryStringNoOrgInfo(SiteContext.Current);
        //        string postName = string.Format("PublicationIssueItem_{0}", publictaionId);
        //        var cacheKey = string.Format("{0}__{1}", targeting, postName);
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<PublicationIssueItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new PublicationIssue();
        //        list.PublicationId = publictaionId;
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);

        //        return result;

        //        //PublicationIssue list = new PublicationIssue();
        //        //list.PublicationId = publictaionId;
        //        //return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<PublicationIssueItem>();
        //}
        //public IList<PublicationCategoryItem> GetPublicationCategoryItems(int publicationIssueId)
        //{
        //    try
        //    {
        //        PublicationCategory list = new PublicationCategory();
        //        list.PublicationIssueId = publicationIssueId;
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<PublicationCategoryItem>();
        //}
        //public IList<PublicationCategoryItem> GetPublicationCategoryItemsById(int id)
        //{
        //    try
        //    {
        //        PublicationCategory list = new PublicationCategory() { ID = id };
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<PublicationCategoryItem>();
        //}
        //public IList<PublicationSubcategoryItem> GetPublicationSubcategoryItems(int publicationCategoryId)
        //{
        //    try
        //    {
        //        PublicationSubcategory list = new PublicationSubcategory() { PublicationCategoryId = publicationCategoryId };
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<PublicationSubcategoryItem>();
        //}
        //public PublicationItem GetPublicationItemByID(int publicationID)
        //{
        //    try
        //    {
        //        Publication publication = new Publication() { ID = publicationID };
        //        var publicationItems = publication.Get();
        //        if (publicationItems != null)
        //        {
        //            return publicationItems.FirstOrDefault();
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}
        //public PublicationIssueItem GetPublicationIssueItemByID(int publicationIssueID)
        //{
        //    try
        //    {
        //        PublicationIssue publicationIssue = new PublicationIssue() { ID = publicationIssueID };
        //        var publicationIssues = publicationIssue.Get();
        //        if (publicationIssues != null)
        //        {
        //            return publicationIssues.FirstOrDefault();
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}
        //public PublicationCategoryItem GetPublicationCategoryItemByID(int publicationCategoryID)
        //{
        //    try
        //    {
        //        PublicationCategory publicationCategory = new PublicationCategory() { ID = publicationCategoryID };
        //        var publicationCategoryItems = publicationCategory.Get();
        //        if (publicationCategoryItems != null)
        //        {
        //            return publicationCategoryItems.FirstOrDefault();
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}
        //public PublicationSubcategoryItem GetPublicationSubcategoryItemByID(int publicationSubCategoryID)
        //{
        //    try
        //    {
        //        PublicationSubcategory publicationSubCategory = new PublicationSubcategory() { ID = publicationSubCategoryID };
        //        var publicationCategoryItems = publicationSubCategory.Get();
        //        if (publicationCategoryItems != null)
        //        {
        //            return publicationCategoryItems.FirstOrDefault();
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}
        //#endregion

        //#region Promotion
        //public IList<FeaturedPromotionItem> GetFeaturedPromotionItems()
        //{
        //    try
        //    {
        //        var targeting = ContentManagementHelper.GenerateTargetingQueryStringNoOrgInfo(SiteContext.Current);
        //        const string postName = "FeaturedPromotionItem";
        //        var cacheKey = string.Format("{0}__{1}", targeting, postName);
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<FeaturedPromotionItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new FeaturedPromotion();
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //        return result;

        //        //var list = new FeaturedPromotion();
        //        //return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<FeaturedPromotionItem>();
        //}

        //public FeaturedPromotionItem GetFeaturedPromotionItemById(int promoId)
        //{
        //    try
        //    {
        //        var list = new FeaturedPromotion();
        //        var featuredPromotionItems = list.Get();

        //        if (featuredPromotionItems != null && featuredPromotionItems.Count > 0)
        //            return featuredPromotionItems.FirstOrDefault(r => r.Id == promoId);
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }

        //    return null;
        //}

        //public IList<PromotionItem> GetTopSalePromotions(int promotionId)
        //{
        //    try
        //    {
        //        var list = new PromotionTopId();
        //        list.SalePromotionId = promotionId;
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<PromotionItem>();
        //}

        //public IList<PromotionItem> GetPromotionItems()
        //{
        //    try
        //    {
        //        var targeting = ContentManagementHelper.GenerateTargetingQueryStringNoOrgInfo(SiteContext.Current);
        //        const string postName = "PromotionItem";
        //        var cacheKey = string.Format("{0}__{1}", targeting, postName);
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<PromotionItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new Promotion();
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //        return result;

        //        //Promotion list = new Promotion();
        //        //return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<PromotionItem>();
        //}

        public IList<PromotionItem> GetApprovedOrPublishedPromotionItems(string requestDomain)
        {
            var list = new Promotion(requestDomain);
            return list.Get();
        }

        //public PromotionItem GetSalePromotionById(int salePromotionId)
        //{
        //    try
        //    {
        //        var list = new PromotionId() { SalePromotionId = salePromotionId };
        //        var isPublishedOnly = false; //check approved item also
        //        var promoList = list.Get(isPublishedOnly);

        //        if (promoList != null)
        //        {
        //            var promotion = promoList.FirstOrDefault();
        //            return promotion;
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}
        //#endregion

        //#region NewReleaseCalendar
        //public IList<NewReleasesCalendarItem> GetNewReleasesCalendarItems(DateTime date, ProductType productType,
        //     out ProductSearchResults allResults)
        //{
        //    try
        //    {
        //        var targeting = ContentManagementHelper.GenerateTargetingQueryString(SiteContext.Current);
        //        var postName = string.Format("NewReleasesCalendarItem{0}{1}", date.ToLongDateString(), productType.ToString());
        //        var cacheKey = string.Format("{0}__{1}", targeting, postName);
        //        var cacheKeyItems = string.Format("{0}__Items", cacheKey);
        //        var cache = VelocityCacheManager.Read(cacheKey) as NewReleasesCalendar;
        //        var cacheItems = VelocityCacheManager.Read(cacheKeyItems) as IList<NewReleasesCalendarItem>;
        //        if (cache != null && cacheItems != null)
        //        {
        //            allResults = cache.AllProductSearchResults;
        //            return cacheItems;
        //        }

        //        var targetingText = ContentManagementHelper.GetTargetingText(SiteContext.Current, ref targeting);
        //        var targetingParam = ContentManagementHelper.ConvertStringToTargetingParam(targetingText);
        //        var list = new NewReleasesCalendar { Date = date, ProductType = productType, Targeting = targetingParam };
        //        var calendarItems = list.Get();
        //        allResults = list.AllProductSearchResults;

        //        VelocityCacheManager.Write(cacheKey, list, CommonConstants.CmCachingLevel, CommonConstants.CmCachingDuration);
        //        VelocityCacheManager.Write(cacheKeyItems, calendarItems, CommonConstants.CmCachingLevel, CommonConstants.CmCachingDuration);
        //        return calendarItems;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<NewReleasesCalendarItem>();
        //}

        //public IList<HolidaysItem> GetHolidaysItems(DateTime date)
        //{
        //    Holidays list = new Holidays();
        //    list.Date = date;
        //    return list.Get();
        //}

        //public NewReleasesCalendarItem GetReleaseDayInformation(DateTime date, ProductType productType)
        //{
        //    try
        //    {
        //        var list = new NewReleasesCalendar() { Date = date, ProductType = productType };
        //        var newReleases = list.Get();
        //        if (newReleases != null && newReleases.Count > 0)
        //        {
        //            foreach (var newRelease in newReleases)
        //            {
        //                if (newRelease.ReleaseDate.Day == date.Day)
        //                {
        //                    return newRelease;
        //                }
        //            }
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}
        //public ReleaseInformationItem GetReleaseInformation(string month, string year, ProductType productType)
        //{
        //    try
        //    {
        //        var list = new ReleaseInformation();
        //        var releaseInformations = list.Get();
        //        if (releaseInformations != null && releaseInformations.Count > 0)
        //        {
        //            foreach (var releaseInformation in releaseInformations)
        //            {
        //                DateTime releaseMonth = new DateTime();
        //                DateTime.TryParse(releaseInformation.ReleaseMonth, out releaseMonth);

        //                if (releaseMonth.Month.ToString() == month &&
        //                    releaseMonth.Year.ToString() == year &&
        //                    releaseInformation.ProductType == productType)
        //                {
        //                    return releaseInformation;
        //                }
        //            }
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}
        //#endregion

        //#region targeting lists
        public IList<LandingPageNewsFeedsItem> GetAllNewFeedItems(int rowLimit)
        {
                var newFeed = new NewFeedList(rowLimit);
                var newFeeds = newFeed.Get();

                // descending order by ID (TFS #7478)
                //if (newFeeds != null && newFeeds.Count > 0)
                //    newFeeds = newFeeds.OrderByDescending(r => r.Id).ToList();

                return newFeeds;
            }

        //public IList<SalesPromoTextItem> GetFirstPromotionItem()
        //{
        //    try
        //    {
        //        //var promotion = new PromotionList();
        //        //return promotion.Get();

        //        //var targeting = ContentManagementHelper.GenerateTargetingQueryStringNoOrgInfo(SiteContext.Current);
        //        //const string postName = "SalesPromoTextItem";
        //        //var cacheKey = string.Format("{0}__{1}", targeting, postName);
        //        //var cache = VelocityCacheManager.Read(cacheKey) as IList<SalesPromoTextItem>;
        //        //if (cache != null)
        //        //    return cache;

        //        var targetingText = ContentManagementHelper.GenerateTargetingValuesNoOrgInfo(SiteContext.Current);
        //        var targetingParam = ContentManagementHelper.ConvertStringToTargetingParamNoOrgInfo(targetingText);

        //        var promotion = new PromotionList { Targeting = targetingParam };
        //        var results = promotion.Get();

        //        //VelocityCacheManager.Write(cacheKey, results, VelocityCacheLevel.WebFrontEnd, CommonConstants.CmCachingDuration);
        //        return results;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<SalesPromoTextItem>();
        //}

        public IList<ListCarouselItem> GetListCarouselItems(TargetingParam targetingParam, string requestDomainName)
        {
                //var targetingText = ContentManagementHelper.GenerateTargetingValuesNoOrgInfo(siteContext);
                //var targetingParam = ContentManagementHelper.ConvertStringToTargetingParamNoOrgInfo(targetingText);

            var list = new ListCarousel(requestDomainName) { Targeting = targetingParam };
                var result = list.Get();

                //VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);

                return result;
            }

        //public ListCarouselItem GetListCarouselItems(int id)
        //{
        //    try
        //    {
        //        var targeting = ContentManagementHelper.GenerateTargetingQueryString(SiteContext.Current);
        //        //var postName = string.Format("ListCarouselItem_{0}", id);
        //        //var cacheKey = string.Format("{0}__{1}", targeting, postName);
        //        //var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as ListCarouselItem;
        //        //if (cache != null)
        //        //    return cache;

        //        var targetingText = ContentManagementHelper.GetTargetingText(SiteContext.Current, ref targeting);
        //        var targetingParam = ContentManagementHelper.ConvertStringToTargetingParam(targetingText);

        //        var list = new ListCarousel { Targeting = targetingParam, ID = id };
        //        var listCarouselItems = list.Get();
        //        if (listCarouselItems != null)
        //        {
        //            var results = listCarouselItems.FirstOrDefault();
        //            //VelocityCacheManager.Write(cacheKey, results, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //            return results;
        //        }
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return null;
        //}

        public IList<CollectionCarouselItem> GetCollectionCarouselItems(string collectionTitle, TargetingParam targetingParam)
        {
            //var targeting = ContentManagementHelper.GenerateTargetingQueryString(siteContext);
            //var targetingText = ContentManagementHelper.GetTargetingText(siteContext, ref targeting);
            //var targetingParam = ContentManagementHelper.ConvertStringToTargetingParam(targetingText);

            var list = new CollectionCarousel { Targeting = targetingParam, CollectionTitle = collectionTitle };
            var listCarouselItems = list.Get();
            return listCarouselItems;
        }
        public IList<CollectionGeneralItem> GetCollectionGeneralItems(TargetingParam targetingParam)
        {
            //var targetingText = ContentManagementHelper.GenerateTargetingValuesNoOrgInfo(siteContext);
            //var targetingParam = ContentManagementHelper.ConvertStringToTargetingParamNoOrgInfo(targetingText);

            var list = new CollectionGeneral { Targeting = targetingParam };
            var results = list.Get();

            return results;
        }
        public IList<ComingSoonCarouselItem> GetComingSoonCarouselItems(SearchByIdData searchData, TargetingValues targeting, string requestDomainName)
        {
            //var targeting = ContentManagementHelper.GenerateTargetingQueryStringNoOrgInfo(SiteContext.Current);
            //const string postName = "ComingSoonCarouselItem";
            //var cacheKey = string.Format("{0}__{1}", targeting, postName);
            //var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<ComingSoonCarouselItem>;
            //if (cache != null)
            //    return cache;

            var targetingParam = MarketingHelper.Instance.ToTargetingParamNoOrg(targeting);
            //var targetingParam = ContentManagementHelper.ConvertStringToTargetingParamNoOrgInfo(targetingText);

            var list = new ComingSoonCarousel(searchData, targeting, requestDomainName) { Targeting = targetingParam };
            var results = list.Get();

            //VelocityCacheManager.Write(cacheKey, results, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
            return results;
        }

        public IList<NewReleaseCalendarItem> GetNewReleaseCalendarItems(string month, string year, List<string> productTypesList, string requestDomainName)
        {
            var list = new NewReleaseCalendar(month, year, productTypesList, requestDomainName);
            var results = list.Get();
            return results;
        }

        public Dictionary<string, IList<NRCFeaturedTitlesItem>> GetNRCFeaturedTitlesItems(string month, string year, List<string> productTypesList, string requestDomainName)
        {
            var results = new Dictionary<string, IList<NRCFeaturedTitlesItem>>();
            foreach (var productType in productTypesList)
            {
                var pType = productType;
                if (productType == ProductTypeEx.DigitaleBook.ToString())
                    pType = "Digital - eBook";
                else if (productType == ProductTypeEx.DigitaleAudio.ToString())
                    pType = "Digital - eAudio";

                // get items by month, year & a productType
                var list = new NRCFeaturedTitles(month, year, pType, requestDomainName);
                var items = list.Get();
                results.Add(productType, items);
            }

            return results;
        }

        public IList<WhatsHotItem> GetWhatsHotItems(TargetingValues siteContext, string requestDomainName)
        {
                var targetingParam = MarketingHelper.Instance.ToTargetingParamNoOrg(siteContext);
                //var targetingParam = ContentManagementHelper.ConvertStringToTargetingParamNoOrgInfo(targetingText);
                var list = new WhatsHot(requestDomainName) { Targeting = targetingParam };
                return list.Get();

                //var targeting = ContentManagementHelper.GenerateTargetingQueryStringNoOrgInfo(SiteContext.Current);
                //const string postName = "WhatsHotItem";
                //var cacheKey = string.Format("{0}__{1}", targeting, postName);
                //var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<WhatsHotItem>;
                //if (cache != null)
                //    return cache;

                //var targetingText = ContentManagementHelper.GenerateTargetingValuesNoOrgInfo(SiteContext.Current);
                //var targetingParam = ContentManagementHelper.ConvertStringToTargetingParamNoOrgInfo(targetingText);

                //var list = new WhatsHot { Targeting = targetingParam };
                //var results = list.Get();

                //VelocityCacheManager.Write(cacheKey, results, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
                //return results;
            }
        //public IList<JacketCarouselItem> GetJacketCarouselItems(string promationFolderName, string adname)
        //{
        //    try
        //    {
        //        var targeting = ContentManagementHelper.GenerateTargetingQueryString(SiteContext.Current);
        //        var postName = string.Format("JacketCarouselItem_{0}_{1}", promationFolderName, adname);
        //        var cacheKey = string.Format("{0}__{1}", targeting, postName);
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<JacketCarouselItem>;
        //        if (cache != null)
        //            return cache;

        //        var targetingText = ContentManagementHelper.GetTargetingText(SiteContext.Current, ref targeting);
        //        var targetingParam = ContentManagementHelper.ConvertStringToTargetingParam(targetingText);

        //        var list = new JacketCarousel
        //        {
        //            FolderName = ContentManagementHelper.ConvertPromotionFolderToString(promationFolderName),
        //            Adname = adname,
        //            Targeting = targetingParam
        //        };
        //        var results = list.Get();

        //        VelocityCacheManager.Write(cacheKey, results, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //        return results;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<JacketCarouselItem>();
        //}

        public IList<InTheNewsItem> GetInTheNewsItems(TargetingValues siteContext, string requestDomainName)
        {
                var targeting = MarketingHelper.Instance.GenerateTargetingQueryStringNoOrgInfo(siteContext);
                const string postName = "InTheNewsItem";
                var cacheKey = string.Format("{0}__{1}", targeting, postName);
                var cache = CachingController.Instance.Read(cacheKey) as IList<InTheNewsItem>;
                if (cache != null)
                    return cache;

                //var targetingText = ContentManagementHelper.GenerateTargetingValuesNoOrgInfo(siteContext);
                var targetingParam = MarketingHelper.Instance.ToTargetingParamNoOrg(siteContext);

                var list = new InTheNews(requestDomainName) { Targeting = targetingParam };
                var results = list.Get();

                CachingController.Instance.Write(cacheKey, results, CommonConstants.CmCachingDuration);
                return results;
            }
        //public IList<TitleBarItem> GetTitleBarItems()
        //{
        //    try
        //    {
        //        TitleBar list = new TitleBar();
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<TitleBarItem>();
        //}
        //public IList<TitleBarItem> GetTitleBarItemsForNonLogin()
        //{
        //    try
        //    {
        //        const string cacheKey = "NonLoginPage_TitleBarItem";
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<TitleBarItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new TitleBarNonLogin();
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //        return result;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<TitleBarItem>();
        //}
        public IList<SalesPromotionItem> GetOpenPromotionItems(TargetingValues siteContext)
        {
            var targeting =  MarketingHelper.Instance.GenerateTargetingQueryStringNoOrgInfo(siteContext);
            const string postName = "SalesPromotionItem";
            var cacheKey = string.Format("{0}__{1}", targeting, postName);
            var cache = CachingController.Instance.Read(cacheKey) as IList<SalesPromotionItem>;
            if (cache != null)
                return cache;

            //var targetingText = ContentManagementHelper.GenerateTargetingValuesNoOrgInfo(siteContext);
            var targetingParam = MarketingHelper.Instance.ToTargetingParamNoOrg(siteContext);

            var salesPromoList = new SalesPromotion { Targeting = targetingParam };
            var results = salesPromoList.Get();

            CachingController.Instance.Write(cacheKey, results, CommonConstants.CmCachingDuration);
            return results;
        }
        public IList<RailsPromotionItem> GetRailsPromotionItems(TargetingValues siteContext)
        {
            //var targetingText = ContentManagementHelper.GenerateTargetingValuesNoOrgInfo(siteContext);
            var targetingParam = MarketingHelper.Instance.ToTargetingParamNoOrg(siteContext);

            //const string postName = "RailsPromotionItem";
            //var cacheKey = string.Format("{0}__{1}", targetingText, postName);

            //var results = CachingController.Instance.Read(cacheKey) as IList<RailsPromotionItem>;

            //if (results != null) return results;

            var railsPromoList = new RailsPromotion { Targeting = targetingParam };
            var results = railsPromoList.Get();

            //CachingController.Instance.Write(cacheKey, results, CommonConstants.CmCachingDuration);
            return results;
        }
        //#endregion

        //#region untargeting lists
        //public IList<HomeImageItem> GetHomeImageItems()
        //{
        //    try
        //    {
        //        HomeImage list = new HomeImage();
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<HomeImageItem>();
        //}
        //public List<PureSystemNotificationsItem> GetSystemNotificationsItems()
        //{
        //    var list = new SystemNotifications();
        //    var listItem = VelocityCacheManager.Read(GlobalConfigurationKey.Ts360SystemNotification, VelocityCacheLevel.Farm)
        //        as List<PureSystemNotificationsItem>;
        //    if (listItem != null) return listItem;

        //    listItem = new List<PureSystemNotificationsItem>();

        //    var snlistItem = list.Get();
        //    if (snlistItem != null && snlistItem.Count > 0)
        //    {
        //        foreach (var systemNotificationsItem in snlistItem)
        //        {
        //            var pureItem = new PureSystemNotificationsItem
        //            {
        //                Id = systemNotificationsItem.Id,
        //                Title = systemNotificationsItem.Title,
        //                NotificationText =
        //                    systemNotificationsItem.NotificationText,
        //                StartDate = systemNotificationsItem.StartDate,
        //                EndDate = systemNotificationsItem.EndDate,
        //                Priority = systemNotificationsItem.Priority
        //            };

        //            listItem.Add(pureItem);
        //        }
        //    }

        //    VelocityCacheManager.Write(GlobalConfigurationKey.Ts360SystemNotification, listItem, CommonCacheContant.Ts360FarmCacheName,
        //        CommonConstants.CmCachingDuration);
        //    return listItem;
        //}
        //public IList<GeneralInformationItem> GetGeneralInformationItems()
        //{
        //    try
        //    {
        //        GeneralInformationList list = new GeneralInformationList();
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<GeneralInformationItem>();
        //}
        //public IList<NextGenTourAreaItem> GetNextGenTourAreaItems()
        //{
        //    try
        //    {
        //        //NextGenTourArea list = new NextGenTourArea();
        //        //return list.Get();

        //        const string cacheKey = "NonLoginPage_NextGenTourAreaItem";
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<NextGenTourAreaItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new NextGenTourArea();
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //        return result;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<NextGenTourAreaItem>();
        //}

        //public IList<ReviewAndSubmitOrderHelpItem> GetReviewAndSubmitOrderHelpItems()
        //{
        //    try
        //    {
        //        var list = new ReviewAndSubmitOrderHelp();
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<ReviewAndSubmitOrderHelpItem>();
        //}

        //public IList<AdvancedSearchTipsItem> GetAdvancedSearchTipsItems()
        //{
        //    try
        //    {
        //        var list = new AdvancedSearchTipsList();
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<AdvancedSearchTipsItem>();
        //}
        //public IList<RetailLibraryAreaItem> GetRetailLibraryAreaItems()
        //{
        //    try
        //    {
        //        //var list = new RetailLibraryArea();
        //        //return list.Get();

        //        const string cacheKey = "NonLoginPage_RetailLibraryAreaItem";
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<RetailLibraryAreaItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new RetailLibraryArea();
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //        return result;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<RetailLibraryAreaItem>();
        //}
        //public IList<FeaturedTitlesItem> GetFeaturedTitlesItems()
        //{
        //    try
        //    {
        //        //var list = new FeaturedTitles();
        //        //return list.Get();

        //        const string cacheKey = "NonLoginPage_FeaturedTitlesItem";
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<FeaturedTitlesItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new FeaturedTitles();
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //        return result;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<FeaturedTitlesItem>();
        //}
        //public IList<TestimonialItem> GetTestimonialItems()
        //{
        //    try
        //    {
        //        //var list = new Testimonial();
        //        //return list.Get();

        //        const string cacheKey = "NonLoginPage_TestimonialItem";
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<TestimonialItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new Testimonial();
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //        return result;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<TestimonialItem>();
        //}
        //public IList<WelcomeToNextGenItem> GetWelcomeToNextGenItems()
        //{
        //    try
        //    {
        //        //var list = new WelcomeToNextGen();
        //        //return list.Get();

        //        const string cacheKey = "NonLoginPage_WelcomeToNextGenItem";
        //        var cache = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as IList<WelcomeToNextGenItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new WelcomeToNextGen();
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, CommonCacheContant.Ts360FarmCacheName, CommonConstants.CmCachingDuration);
        //        return result;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<WelcomeToNextGenItem>();
        //}
        //#endregion

        //public IList<ListItemDetailConfigurationItem> GetItemDetailConfig()
        //{
        //    try
        //    {
        //        const string cacheKey = "__GetItemDetailConfigCacheKey";
        //        var cache = VelocityCacheManager.Read(cacheKey) as IList<ListItemDetailConfigurationItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new ItemDetailsConfigList();
        //        var result = list.Get();

        //        VelocityCacheManager.Write(cacheKey, result, VelocityCacheLevel.Session, CommonConstants.CmCachingDuration);
        //        return result;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<ListItemDetailConfigurationItem>();
        //}
        public IList<ListItemDetailConfigurationItem> GetItemDetailConfigVisible()
        {
                const string cacheKey = "__GetItemDetailConfigVisibleCacheKey";
                var cache = CachingController.Instance.Read(cacheKey) as IList<ListItemDetailConfigurationItem>;
                if (cache != null)
                    return cache;

                var list = new ItemDetailsConfigListVisible();
                cache = list.Get();

                CachingController.Instance.Write(cacheKey, cache, CommonConstants.CmCachingDuration);
                //VelocityCacheManager.Write(cacheKey, config, VelocityCacheLevel.Session, CommonConstants.CmCachingDuration);
                return cache;
            }
        public IList<ListItemDetailFieldItem> GetItemDetailField(int[] sectionIds)
        {
                var sb = new StringBuilder();
                sb.Append("__GetItemDetailFieldCacheKey");

                foreach (var sectionId in sectionIds)
                {
                    sb.Append(sectionId);
                }

                var cacheKey = sb.ToString();
                var cache = CachingController.Instance.Read(cacheKey) as IList<ListItemDetailFieldItem>;
                if (cache != null)
                    return cache;

                var list = new ItemDetailsFieldList() { SectionIds = sectionIds };
                var config = list.Get();

                CachingController.Instance.Write(cacheKey, config, CommonConstants.CmCachingDuration);
                return config;
            }
        //public IList<ListItemDetailSectionItem> GetItemDetailsection()
        //{
        //    try
        //    {
        //        const string cacheKey = "__GetItemDetailsectionCacheKey";
        //        var cache = VelocityCacheManager.Read(cacheKey) as IList<ListItemDetailSectionItem>;
        //        if (cache != null)
        //            return cache;

        //        var list = new ItemDetailsSectionList();
        //        var config = list.Get();

        //        VelocityCacheManager.Write(cacheKey, config, VelocityCacheLevel.Session, CommonConstants.CmCachingDuration);
        //        return config;
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<ListItemDetailSectionItem>();
        //}

      

        ///// <summary>
        ///// Gets the organization items.
        ///// </summary>
        ///// <returns></returns>
        //public static byte[] GetOrganizationLogo(string key)
        //{
        //    byte[] logo = null;
        //    SPSecurity.RunWithElevatedPrivileges(delegate
        //    {
        //        using (var site = new SPSite(SPContext.Current.Site.RootWeb.Url))
        //        {
        //            using (var web = site.RootWeb)
        //            {
        //                var spList = web.Lists["OrganizationLogo"];
        //                var queryOrgId = string.Format("<Where><Eq><FieldRef Name='OrganizationId' /><Value Type='Text'>{0}</Value></Eq></Where>", key);
        //                var query = new SPQuery
        //                {
        //                    Query = queryOrgId
        //                };

        //                var items = spList.GetItems(query);
        //                if (items.Count <= 0) return;

        //                foreach (SPListItem item in items)
        //                {
        //                    var orgItemId = item["OrganizationId"] != null
        //                        ? item["OrganizationId"].ToString()
        //                        : String.Empty;
        //                    if (string.Compare(key, orgItemId, StringComparison.OrdinalIgnoreCase) == 0)
        //                    {
        //                        logo = item.File != null ? item.File.OpenBinary() : null;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    });

        //    return logo;
        //}

        ///// <summary>
        ///// Updates the organization item.
        ///// </summary>
        ///// <param name="item">The item.</param>
        ///// <returns></returns>
        //public static bool UpdateOrganizationItem(SPOrganizationItem item)
        //{
        //    using (var site = new SPSite(SPContext.Current.Site.RootWeb.Url))
        //    {
        //        using (var web = site.RootWeb)
        //        {
        //            web.AllowUnsafeUpdates = true;
        //            var spList = web.Lists["OrganizationLogo"];
        //            //Delete Exist Item
        //            for (var i = 0; i < spList.Items.Count; i++)
        //            {
        //                if (spList.Items[i]["OrganizationId"].ToString() != item.OrganizationId) continue;
        //                spList.Items.Delete(i);
        //                spList.Update();
        //                break;
        //            }
        //            //Add new Item
        //            var organizationLogoFileContent = item.OrganizationLogo;
        //            var organizationLogoFolder = spList.RootFolder;
        //            string fileName;
        //            if (!String.IsNullOrEmpty(item.OrganizationId))
        //            {
        //                fileName = item.OrganizationId.Replace("{", "");
        //                fileName = fileName.Replace("}", "");
        //                fileName = fileName.Replace(" ", "");
        //                fileName = fileName.Replace("-", "");
        //            }
        //            else
        //            {
        //                fileName = Guid.NewGuid().ToString();
        //                fileName = fileName.Replace("{", "");
        //                fileName = fileName.Replace("}", "");
        //                fileName = fileName.Replace(" ", "");
        //                fileName = fileName.Replace("-", "");
        //            }
        //            //
        //            var organizationLogoFile = organizationLogoFolder.Files.Add(organizationLogoFolder.Url + "/" + fileName + item.OrganizationLogoFileName, organizationLogoFileContent);
        //            //
        //            organizationLogoFile.Item["Title"] = fileName + item.OrganizationLogoFileName;
        //            organizationLogoFile.Item["Name"] = fileName + item.OrganizationLogoFileName;
        //            organizationLogoFile.Item["OrganizationId"] = item.OrganizationId;
        //            organizationLogoFile.Item["Description"] = item.Description;
        //            organizationLogoFile.Item.Update();

        //            spList.Update();
        //            web.AllowUnsafeUpdates = false;
        //        }
        //    }

        //    return true;
        //}

        //public IList<TagRulesExportInstructionItem> GetTagRulesExportInstructionItems()
        //{
        //    try
        //    {
        //        TagRulesExportInstructionList list = new TagRulesExportInstructionList();
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<TagRulesExportInstructionItem>();
        //}


        //public IList<SpecialCodeHelpItem> GetSpecialCodeHelpItems()
        //{
        //    try
        //    {
        //        var list = new SpecialCodeHelp();
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<SpecialCodeHelpItem>();
        //}

        //public IList<OnlineBillPaymentTextItem> GetOnlineBillPaymentTextItem()
        //{
        //    try
        //    {
        //        OnlineBillPaymentText list = new OnlineBillPaymentText();
        //        return list.Get();
        //    }
        //    catch (NextGenValidationException ex) { throw; }
        //    catch (SPException ex) { throw; }
        //    catch (Exception ex) { throw; }
        //    return new List<OnlineBillPaymentTextItem>();
        //}
        public HeaderTitlesItem GetHeaderTitlesItems()
        {
                var list = new HeaderTitlesList();
                var items = list.Get();
                if (items != null && items.Count > 0)
                    return items[0];

                return null;
            }
    }
}
