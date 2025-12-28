using BT.TS360API.Cache;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ExternalServices;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Product;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Common.Business
{
    public class ProductDAOManager
    {
        private ProductDAOManager()
        { }

        private static volatile ProductDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        public static ProductDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProductDAOManager();
                }

                return _instance;
            }
        }

        public string GetProductExcerpts(string key)
        {
            var data = ProductDAO.Instance.GetProductExcerpts(key);

            data = RefineExcerpt(data);

            data = HttpUtility.HtmlDecode(data);
            data = Sanitizer.GetSafeHtmlFragment(data);

            return data;
        }

        private static string RefineExcerpt(string data)
        {
            //TRAC 715: Excerpt tab text is unreadable
            //Now we replace <Font size="+2"> with <Font>
            var refined = CommonHelper.ReplaceInsensitiveString(data, "<Font size=\"+2\">", "<Font>");

            //Clear out all html, body tags
            refined = CommonHelper.ReplaceInsensitiveString(refined, "<html>", "");
            refined = CommonHelper.ReplaceInsensitiveString(refined, "<body>", "");
            refined = CommonHelper.ReplaceInsensitiveString(refined, "</html>", "");
            refined = CommonHelper.ReplaceInsensitiveString(refined, "</body>", "");

            return refined;
        }

        public string GetProductTOC(string key)
        {
            // Get TOC from Content Cafe
            //var data = ProductSearchController.GetTocFromCc(key);
            var data = ContentCafeHelper.GetTOCFromContentCafe(key);

            data = HttpUtility.HtmlDecode(data);
            data = Sanitizer.GetSafeHtmlFragment(data);

            return data;
        }

        public List<AdditionContent> GetProductReview(string key, string organizationId, string userId)
        {
            var result = new AppServiceResult<List<AdditionContent>>();
            //var profileController = ProfileController.Current;

            //var siteContext = SiteContext.Current;
            //var organization = AdministrationProfileController.Current.GetOrganization(siteContext.OrganizationId);                                

            var organization = ProfileService.Instance.GetOrganizationById(organizationId);

            var reviewTypeList = organization != null && organization.ReviewTypeList != null
                                          ? organization.ReviewTypeList
                                          : new string[] { };

            //Encodes
            var reviewTypeListTmp = new string[reviewTypeList.Length];
            for (var i = 0; i < reviewTypeList.Length; i++)
            {
                //reviewTypeListTmp[i] = profileController.Decode(reviewTypeList[i]);
                reviewTypeListTmp[i] = CommonHelper.Decode_ProductReviewTypeList(reviewTypeList[i]);
            }
            var reviews = ProductDAO.Instance.GetProductReviews(key, reviewTypeListTmp);
            // need to change later to adapt the UI
            //var reviews = ProductDAO.Instance.GetProductReviews(key, reviewTypeList);

            //PreferencesProfileController.Current.UserProfileRelated.UserReviewTypeNeeded = true;
            //var userProfile = PreferencesProfileController.Current.GetCurrentUserProfile();
            var userProfile = ProfileService.Instance.GetUserById(userId);

            if (userProfile != null && reviews != null && reviews.Count > 0)
            {
                userProfile.MyReviewTypes = ProfileService.Instance.GetUserReviewTypes(userId,
                    userProfile.MyReviewTypeIds);
                var userReviewTypes = userProfile.MyReviewTypes;
                //Get the Sequence of Review item from UserProfile's MyReviewTypes

                //foreach (var reviewItem in userReviewTypes.Select(t => (UserReviewType)t.Target))
                foreach (var reviewItem in userReviewTypes)
                {
                    var item = reviewItem;
                    //foreach (var validReview in reviews.Where(v => v.ReviewTypeId == profileController.Decode(item.ReviewTypeId)))
                    foreach (var validReview in reviews.Where(v => v.ReviewTypeId == CommonHelper.Decode_ProductReviewTypeList(item.ReviewType)))
                    {
                        validReview.Sequence = reviewItem.Ordinal;
                        //validReview.DisplayName = profileController.GetSiteTermName(SiteTermName.ReviewType, profileController.Encode(reviewItem.ReviewTypeId));
                        validReview.DisplayName = SiteTermHelper.Instance.GetSiteTermName(SiteTermName.ReviewType, CommonHelper.Encode_ProductReviewTypeList(reviewItem.ReviewTypeId));
                    }
                }
                foreach (var additionalContent in reviews)
                {
                    additionalContent.Content = HttpUtility.HtmlDecode(additionalContent.Content);
                }

                //reviews = profileController.SortReviewTypes(reviews, true);
                reviews = ProfileDAOManager.Instance.SortReviewTypes(reviews, true);
            }

            return reviews;
        }

        public string GetProductMuzeFromContentCafe(string btKey)
        {
            return ContentCafeHelper.GetProductMuzeFromContentCafe(btKey);
        }

        public List<AdditionContent> GetProductAnnos(string productId)
        {
            return ProductDAO.Instance.GetProductAnnos(productId);
        }

        public async Task<string> GetFirstProductAnnotation(string btkey)
        {
            return await ProductDAO.Instance.GetFirstProductAnnotation(btkey);
        }

        public async Task<ProductDetailsObject> GetProductInformation(string btKey)
        {
            return await ProductDAO.Instance.GetProductInformation(btKey);
        }

        //public async Task<ProductDetail> GetProductDetail(string btKey, bool OCLCCatalogingPlusEnabled, string userId, 
        //    ProductDetailsObject productDetailsObject)
        //{
        //    try
        //    {
        //        // TFS 20719. Ivor approved to get annotation from DB instead of FAST
        //        // because FAST uses '#' as delimiter between annotations but content of annotation may contain '#' character.
        //        //string btAnno = await GetFirstProductAnnotation(btKey);
        //        //product.Annotaion = Sanitizer.GetSafeHtmlFragment(HttpUtility.HtmlDecode(btAnno));

        //        // Get product DAO object
        //        var cacheKey = string.Format("ProductDetailsInfo_{0}", btKey);

        //        //var productDetailsObject = CacheHelper.Get(cacheKey) as ProductDetailsObject;
        //        productDetailsObject = CachingController.Instance.Read(cacheKey) as ProductDetailsObject;

        //        if (productDetailsObject == null)
        //        {
        //            productDetailsObject = await ProductDAOManager.Instance.GetProductInformation(btKey);

        //            //CacheHelper.Write(cacheKey, productDetailsObject);
        //            CachingController.Instance.Write(cacheKey, productDetailsObject);
        //        }

        //        //if (productDetailsObject != null)
        //        //{
        //        //    GetDataForSections(product, productDetailsObject, btKey, OCLCCatalogingPlusEnabled, userId);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        productDetailsObject = new ProductDetailsObject();
        //        Logger.RaiseException(ex, ExceptionCategory.Search);
        //    }

        //    return product;
        //}

        //private List<AdditionContent> GetProductDetailReview(string btKey, string userId)
        //{
        //    //var profileController = ProfileController.Current;
        //    //profileController.UserProfileRelated.UserReviewTypeNeeded = true;

        //    //var userProfile = CSObjectProxy.GetUserProfileForSearchResult();
        //    var userProfile = ProfileService.Instance.GetUserById(userId);

        //    if (userProfile == null)
        //    {
        //        return new List<AdditionContent>();
        //    }
        //    var organization = ProfileService.Instance.GetOrganizationById(userProfile.OrgId);// userProfile.OrganizationEntity;

        //    string[] reviewTypeList;
        //    if (organization != null && organization.ReviewTypeList != null)
        //    {
        //        reviewTypeList = organization.ReviewTypeList;
        //    }
        //    else
        //    {
        //        reviewTypeList = new string[] { };
        //    }
        //    //Encode
        //    var reviewTypeListTmp = new string[reviewTypeList.Length];
        //    for (var i = 0; i < reviewTypeList.Length; i++)
        //    {
        //        //reviewTypeListTmp[i] = profileController.Decode(reviewTypeList[i]);
        //        CommonHelper.Decode_ProductReviewTypeList(reviewTypeList[i]);
        //    }
        //    //Encode
        //    var result = ProductDAO.Instance.GetProductReviews(btKey, reviewTypeListTmp);

        //    //Assign the sequence
        //    userProfile.MyReviewTypes = ProfileService.Instance.GetUserReviewTypes(userId, userProfile.MyReviewTypeIds);
        //    var userReviewTypes = userProfile.MyReviewTypes;

        //    if (result != null && result.Count > 0 && userReviewTypes != null)
        //    {
        //        foreach (var rv in result)
        //        {
        //            var rt =
        //                //userReviewTypes.Select(t => (UserReviewType)t.Target).Where(
        //                userReviewTypes.Where(
        //                //r => profileController.Decode(r.ReviewTypeId) == rv.ReviewTypeId).FirstOrDefault();
        //                    r => CommonHelper.Decode_ProductReviewTypeList(r.ReviewTypeId) == rv.ReviewTypeId).FirstOrDefault();

        //            if (rt != null)
        //            {
        //                rv.Sequence = rt.Ordinal;
        //                //rv.DisplayName = profileController.GetSiteTermName(SiteTermName.ReviewType,
        //                rv.DisplayName = SiteTermHelper.Instance.GetSiteTermName(SiteTermName.ReviewType,
        //                    //profileController.Encode(rt.ReviewTypeId));
        //                                                                   CommonHelper.Encode_ProductReviewTypeList(rt.ReviewTypeId));
        //            }
        //        }

        //        //result = profileController.SortReviewTypes(result, true /*Add items with null Sequences to top*/);
        //        result = ProfileDAOManager.Instance.SortReviewTypes(result, true /*Add items with null Sequences to top*/);

        //    }
        //    return result;
        //}

        //private static List<ReviewCitationObject> RealDataForOtherSections(IEnumerable<ListItemDetailFieldItem> listField, List<ReviewCitationObject> sectionData, string sectionName)
        //{
        //    var result = new List<ReviewCitationObject>();

        //    if (sectionData != null && listField != null)
        //    {
        //        if (listField.Any(item => item.FieldKey == sectionName))
        //        {
        //            return sectionData;
        //        }
        //    }

        //    return result;
        //}

        //private static List<string> RealDataForOtherSections(IEnumerable<ListItemDetailFieldItem> listField, List<string> sectionData,
        //    string sectionName)
        //{
        //    var result = new List<string>();

        //    if (sectionData != null && listField != null)
        //    {
        //        if (listField.Any(item => item.FieldKey == sectionName))
        //        {
        //            return sectionData;
        //        }
        //    }

        //    return result;
        //}

        

        //private static void GetFormatAttr(List<ItemData> result, ListItemDetailFieldItem item, string itemValue)
        //{
        //    var doc = new XmlDocument();
        //    doc.LoadXml(itemValue);
        //    var physicalNodes = doc.GetElementsByTagName("PhysicalFormat");

        //    for (var i = 0; i < physicalNodes.Count; i++)
        //    {
        //        var isPrimary = false;
        //        var attrCollection = physicalNodes[i].Attributes;
        //        if (attrCollection != null)
        //            foreach (XmlAttribute xmlAttr in attrCollection)
        //            {
        //                if (xmlAttr.Name.ToLower() == "primary" && xmlAttr.Value.ToLower() == "y")
        //                {
        //                    isPrimary = true;
        //                }
        //            }

        //        result.AddRange(from XmlNode child in physicalNodes[i].ChildNodes
        //                        where child.Name.Equals("FormatName")
        //                        let strTitle = isPrimary ? item.Title : "Included Format"
        //                        select new ItemData
        //                        {
        //                            ItemDataValue = child.InnerText,
        //                            ItemDataText = strTitle,
        //                            IsChild = false
        //                        });
        //    }
        //}

        public List<AdditionContent> GetProductFlapCopy(string productId)
        {
            return ProductDAO.Instance.GetProductFlapCopy(productId);
        }

        public List<AdditionContent> GetProductBiographies(string productId)
        {
            return ProductDAO.Instance.GetProductBiographies(productId);
        }

        public List<SiteTermObject> CheckProductReviewsFromOds(List<string> btkey, string userId)
        {
            var list = new List<SiteTermObject>();
            
            //var user = CSObjectProxy.GetUserProfileForSearchResult();
            var user = ProfileService.Instance.GetUserById(userId);
            
            if (user != null)
            {
                var organization = ProfileService.Instance.GetOrganizationById(user.OrgId); //user.OrganizationEntity;
                //
                if (organization != null)
                {
                    var reviewType = organization.ReviewTypeList;

                    if (reviewType != null && reviewType.Length == 0) return list;
                    //
                    var productReviewsFromOds = ProductDAO.Instance.CheckProductReviewsFromODS(btkey.ToArray(), reviewType);
                    //
                    if (productReviewsFromOds != null)
                    {
                        list.AddRange(from item in productReviewsFromOds where item.Value select new SiteTermObject(item.Key, item.Value.ToString()));
                    }
                }
            }
            return list;
        }
    }
}
