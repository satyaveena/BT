using System.Collections.Generic;
using System.Linq;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Search;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;

namespace BT.TS360SP
{
    public class ComingSoonCarousel : CMListBase<ComingSoonCarouselItem>
    {
        string RequestDomainName { get; set; }
        SearchByIdData SearchData { get; set; }
        TargetingValues Targeting { get; set; }
        public ComingSoonCarousel(SearchByIdData search, TargetingValues targeting, string requestDomainName)
        {
            SearchData = search;
            Targeting = targeting;
            RequestDomainName = requestDomainName;
            HasAdName = true;
            HasDisplayOrder = true;
        }
        protected override string GetListName()
        {
            return "ComingSoonCarousel";
        }
        protected override string AddWhereCamlTo(string condition)
        {
            string startDateNull = string.Format("{0}<FieldRef Name='{1}' />{2}", CMConstants.IsNull_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.IsNull_TAG_CLOSE);
            string startDateLtoday = string.Format("{0}<FieldRef Name='{1}' /><Value Type='DateTime'><Today /></Value>{2}", CMConstants.LEQ_TAG_OPEN, CMFieldNameConstants.StartDate, CMConstants.LEQ_TAG_CLOSE);
            string startDateOr = CMConstants.OR_TAG_OPEN + startDateNull + startDateLtoday + CMConstants.OR_TAG_CLOSE;
            if (string.IsNullOrEmpty(condition))
                return startDateOr;
            return CMConstants.AND_TAG_OPEN + condition + startDateOr + CMConstants.AND_TAG_CLOSE;
        }

        private ProductSearchResultItem FindItemFromSearchResults(ProductSearchResults res, string btKey)
        {
            if (res != null && res.Items != null)
            {
                return res.Items.FirstOrDefault(item => item.BTKey == btKey);
            }
            return null;
        }

        // refine data: currenty find product image from Content cafe
        protected override void Refine(IList<ComingSoonCarouselItem> items)
        {
            var btkeys = items.Select(i => i.BTKEY).ToList();
            var searchResult = ProductSearchController.SearchByIdForContentManagement(btkeys, Targeting.MarketType,
                SearchData.SimonSchusterEnabled, SearchData.CountryCode, SearchData.ESuppliers);
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var found = FindItemFromSearchResults(searchResult, item.BTKEY);
                if (found != null)
                {
                    if (string.IsNullOrEmpty(item.ComingSoonImage))
                    {
                        // get image url from Content Cafe                     
                        item.ComingSoonImage = ContentCafeHelper.GetJacketImageUrl(found.ISBN, ImageSize.Medium, found.HasJacket);
                    }

                    item.ComingSoonImage = ContentManagementHelper.GetRelativePath(item.ComingSoonImage, RequestDomainName);
                    item.ProductSearchResultItem = found;

                    item.PublishedDate = found.PublishDate;
                    item.OldPrice = CommonHelper.FormatPrice(found.ListPrice);
                    item.Format = found.ProductFormat;
                    item.FormatIconPath = found.FormatIconPath;
                    item.FormatClass = found.FormatClass;
                    item.FormatIconClass = found.FormatIconClass;
                    item.IncludedFormatClass = found.IncludedFormatClass;
                    item.BTProductType = found.ProductType;
                    item.ContentOwner = found.Author;
                    item.NewPrice = found.DiscountPrice.ToString();
                    item.ISBN = found.ISBN;
                    item.ISBN10 = found.ISBN10;
                    item.DiscountText = found.DiscountText;
                    item.Promotion = found.Promotion;
                    item.ProductFormat = found.ProductFormat;

                    item.Upc = found.Upc;
                    item.GTIN = found.GTIN;
                    item.HasAnnotations = found.HasAnnotations;
                    item.HasExcerpts = found.HasExcerpt;
                    item.HasMuze = found.HasMuze;
                    item.HasReviews = found.HasReview;
                    item.HasTOC = found.HasToc;
                    item.ReportCode = found.ReportCode;
                    item.HasReturn = found.HasReturn;
                    item.Catalog = found.Catalog;
                    item.ProductLine = found.ProductLine;
                    item.AcceptableDiscount = found.AcceptableDiscount;
                    item.BTEKey = found.BTEKey;
                    item.ContentIndicator = found.ContentIndicatorText;
                    item.ISBNLookUpLink = ProductSearchController.CreateIsbnLookupLink(found.ISBN, found.ISBN10, Targeting.OrgId);// found.ISBNLookUpLink;

                    item.ESupplier = found.ESupplier;
                    item.FormDetails = found.FormDetails;
                    item.PurchaseOption = found.PurchaseOption;

                    item.PriceKey = found.PriceKey;
                }
                else
                {
                    items.Remove(item);
                    i--;
                }
            }
        }
        protected override void IncludeFields(ClientContext clientContext, ListItemCollection listItems)
        {
            clientContext.Load(listItems, items => items.Include(
                       item => item.Id,
                       item => item["Title"],
                       item => item["AdName"],
                       item => item["ComingSoonBTKEY"],
                       item => item["ComingSoonImage"],
                       item => item["WebTrendsTag"]
                       ));
        }
      
    }
}
