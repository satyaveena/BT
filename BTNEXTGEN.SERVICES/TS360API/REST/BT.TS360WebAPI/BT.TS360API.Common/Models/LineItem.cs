using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    public class LineItem
    {
        private string _btKey;
        private string _espDetailUrl;

        public string BTKey
        {
            get
            {
                if (IsOriginalEntryItem && !string.IsNullOrEmpty(BasketOriginalEntryId))
                    _btKey = BasketOriginalEntryId.Replace("{", "").Replace("}", "");

                return _btKey;
            }
            set { _btKey = value; }
        }

        public string BTEKey { get; set; }

        public int? Quantity { get; set; }

        public string PONumber { get; set; }

        public string AccountId { get; set; }

        public string ProductType { get; set; }

        public decimal? ListPrice { get; set; }

        public string ISBN10 { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public string Lccn { get; set; }

        public string DeweyNormalized { get; set; }

        public string FormatLiteral { get; set; }

        public string Edition { get; set; }

        public DateTime? PublishDate { get; set; }

        public string ReportCode { get; set; }

        public string Subject { get; set; }

        public int BTPOLineItemNumber { get; set; }

        public int BTShippedQuantity { get; set; }

        public int BTBackorderQuantity { get; set; }

        public int BTCancelQuantity { get; set; }

        public string BTLineItemNote { get; set; }

        public string LCClass { get; set; }

        public string Upc { get; set; }

        public string SubTitle { get; set; }

        public string ISBN { get; set; }

        public string ProductLine { get; set; }

        public string Id { get; set; }

        public string BasketOrderFormId { get; set; }

        public string CatalogName { get; set; }

        public string DisplayName { get; set; }

        public string ProductId { get; set; }

        public string VariantId { get; set; }

        public string BTItemType { get; set; }

        public string BTAuthorOrArtist { get; set; }

        public string BTGTIN { get; set; }

        public string BTUPC { get; set; }

        public string BTISBN { get; set; }

        public decimal ContractPrice { get; set; }

        public decimal BTDiscountPercent { get; set; }

        public int QuantityAvailable { get; set; }

        public int OnOrderQuantity { get; set; }

        public List<LineItemAcknowledgement> Acknowledgements { get; set; }

        public string BasketOriginalEntryId { get; set; }

        public bool IsOriginalEntryItem
        {
            get
            {
                return !string.IsNullOrEmpty(BasketOriginalEntryId);
            }
        }

        public string Bib { get; set; }

        public bool HasGridError { get; set; }

        public bool HasFamilyKey { get; set; }

        public bool HasQuantityError
        {
            get
            {
                return !Quantity.HasValue || Quantity.Value == 0;
            }
        }

        public string MerchCategory { get; set; }
        public bool HasCPSIAWarning { get; set; }
        public bool HasAnnotations { get; set; }
        public bool HasExcerpt { get; set; }
        public bool HasReturn { get; set; }
        public bool HasMuze { get; set; }
        public bool HasReview { get; set; }
        public bool HasTOC { get; set; }
        public bool HasIncludedFormat { get; set; }

        // has jacket by default to get image from ContentCafe
        private bool _hasJacket = true;
        public bool HasJacket
        {
            get { return _hasJacket; }
            set { _hasJacket = value; }
        }

        public string ESupplier { get; set; }
        public string FormDetails { get; set; }
        public string PurchaseOption { get; set; }
        public string ProductCode { get; set; }
        public string PromotionCode { get; set; }
        public string SupplierCode { get; set; }

        private FlagObject _flagObject;
        //public string ContentIndicatorText
        //{
        //    get
        //    {
        //        //
        //        var content = SearchHelper.CombineContentIndicator(ProductContent, this.BTKey, string.Empty);
        //        return content;
        //    }
        //}

        //public ProductContent ProductContent
        //{
        //    get
        //    {
        //        if (this._flagObject == null)
        //            this._flagObject = ProductSearchController.GetFlagObject(SiteContext.Current.UserId);

        //        var hasMuze = false;
        //        var hasToc = false;
        //        if (_flagObject != null)
        //        {
        //            hasMuze = _flagObject.ShowMuze;
        //            hasToc = _flagObject.ShowToc;
        //        }

        //        var prodContent = new ProductContent
        //        {
        //            HasAnnotation = this.HasAnnotations,
        //            HasExcerpts = this.HasExcerpt,
        //            HasReturnKey = this.HasReturn
        //        };
        //        if (hasMuze && this.ProductType != ProductTypeConstants.Book)
        //            prodContent.HasMuze = this.HasMuze;
        //        //
        //        prodContent.HasReviews = this.HasReview;
        //        //
        //        if (hasToc)
        //            prodContent.HasTOC = this.HasTOC;

        //        return prodContent;
        //    }
        //}

        //public string FormatIconPath
        //{
        //    get
        //    {
        //        string path = "/_layouts/IMAGES/CSDefaultSite/assets/images/icons/";
        //        switch (this.ProductCode)
        //        {
        //            case ProductCodeConstants.MUSIC:
        //                if (this.FormatLiteral == ProductFormatConstants.Music_Vinyl_12 || this.FormatLiteral == ProductFormatConstants.Music_Vinyl_7I || this.FormatLiteral == ProductFormatConstants.Music_Vinyl_LP)
        //                {
        //                    path += ProductIconPathConstants.VINYL;
        //                }
        //                else
        //                {
        //                    path += ProductIconPathConstants.MUSIC;
        //                }
        //                break;
        //            case ProductCodeConstants.MOVIE:
        //                if (this.ProductLine.ToUpper() == ProductLineConstants.Video.ToUpper())
        //                {
        //                    switch (this.FormatLiteral)
        //                    {
        //                        case ProductFormatConstants.Movie_Blu_ray_Hi_Def_DVD:
        //                            path += ProductIconPathConstants.BD; //Blu-ray icon
        //                            break;
        //                        case ProductFormatConstants.Movie_Playaway_View:
        //                            path += ProductIconPathConstants.PL; //Playaway View icon 
        //                            break;
        //                        default:
        //                            path += ProductIconPathConstants.MOVIE;
        //                            break;
        //                    }
        //                }
        //                else
        //                {
        //                    path += ProductIconPathConstants.MOVIE;
        //                }
        //                break;
        //            case ProductCodeConstants.EBOOK:
        //                path += ProductIconPathConstants.EBOOK;
        //                break;
        //            case ProductCodeConstants.BOOK:
        //                switch (this.MerchCategory)
        //                {
        //                    case ProductMerchandiseCategoryConstants.PRINT_ON_DEMAND:
        //                    case ProductMerchandiseCategoryConstants.TEXTSTREAM_PRINT_ON_DEMAND:
        //                        path += ProductIconPathConstants.PRINT_ON_DEMAND;
        //                        break;
        //                    default:
        //                        if (this.ProductLine.ToLower() == ProductCodeConstants.EBOOK.ToLower())
        //                            path += ProductIconPathConstants.EBOOK;
        //                        else
        //                            path += ProductIconPathConstants.BOOK;
        //                        break;
        //                }
        //                break;
        //            default:
        //                // return blank ICO
        //                path += ProductIconPathConstants.EMPTY;
        //                break;
        //        }

        //        return path;
        //    }
        //}

        //public string FormatIconClass
        //{
        //    get
        //    {
        //        var cssClass = string.Empty;
        //        switch (this.ProductCode)
        //        {
        //            case ProductCodeConstants.MUSIC:
        //                if (this.FormatLiteral == ProductFormatConstants.Music_Vinyl_12 || this.FormatLiteral == ProductFormatConstants.Music_Vinyl_7I || this.FormatLiteral == ProductFormatConstants.Music_Vinyl_LP)
        //                {
        //                    cssClass = "icon_format_vinyl";
        //                }
        //                else
        //                {
        //                    cssClass = "icon_format_cd";
        //                }
        //                break;
        //            case ProductCodeConstants.MOVIE:
        //                if (this.ProductLine.ToUpper() == ProductLineConstants.Video.ToUpper())
        //                {
        //                    switch (this.FormatLiteral)
        //                    {
        //                        case ProductFormatConstants.Movie_Blu_ray_Hi_Def_DVD:
        //                            cssClass = "icon_format_bd"; //Blu-ray icon
        //                            break;
        //                        case ProductFormatConstants.Movie_Playaway_View:
        //                            cssClass = "icon_format_pv"; //Playaway View icon 
        //                            break;
        //                        default:
        //                            cssClass = "icon_format_dvd";
        //                            break;
        //                    }
        //                }
        //                else
        //                {
        //                    cssClass = "icon_format_dvd";
        //                }
        //                break;
        //            case ProductCodeConstants.EBOOK:
        //                cssClass = "icon_format_bookelectronic";
        //                break;
        //            case ProductCodeConstants.BOOK:
        //                if (this.FormatLiteral == ProductFormatConstants.Book_PreLoaded_Audio_Player)
        //                {
        //                    cssClass = "icon_format_pa";
        //                    break;
        //                }
        //                switch (this.MerchCategory)
        //                {
        //                    case ProductMerchandiseCategoryConstants.PRINT_ON_DEMAND:
        //                    case ProductMerchandiseCategoryConstants.TEXTSTREAM_PRINT_ON_DEMAND:
        //                        cssClass = "icon_format_pod";
        //                        break;
        //                    default:
        //                        if (this.ProductLine.ToLower() == ProductCodeConstants.EBOOK.ToLower())
        //                            cssClass = "icon_format_bookelectronic";
        //                        else
        //                            cssClass = "icon_format_book";
        //                        break;
        //                }
        //                break;
        //            default:
        //                // return blank ICO
        //                cssClass = string.Empty;
        //                break;
        //        }
        //        return cssClass;
        //    }
        //}

        //public string IncludedFormatClass
        //{
        //    get
        //    {
        //        var cssClass = string.Empty;
        //        if (this.HasIncludedFormat)
        //            cssClass = CommonConstants.ICON_INCLUDED_FORMAT;

        //        return cssClass;
        //    }
        //}

        public string DeweyNative { get; set; }

        public DateTime? StreetDate { get; set; }

        public string VolumeNumber { get; set; }

        public decimal? SalePrice { get; set; }

        public decimal? ExtendedPrice { get; set; }
        public string PubStatusCode { get; set; }
        public string AVAttributes { get; set; }
        public string Attributes { get; set; }
        public string EditionNumber { get; set; }
        public string BisacSubject1Code { get; set; }

        public string PriceKey { get; set; }

        public string AgencyCode { get; set; }

        public string ItemTypeCode { get; set; }

        public string CollectionCode { get; set; }

        public string LocalCallNumber { get; set; }

        public string ContributedBy { get; set; }
        public string ReplacementBTKey { get; set; }

        public string AcceleratedReader { get; set; }
        public string ReadingCount { get; set; }
        public string VolumnNumber { get; set; }
        public string Audience { get; set; }
        public string Series { get; set; }
        public string Oclc { get; set; }
        public string Lexile { get; set; }
        public string PreOrderDate { get; set; }
        public string Rating { get; set; }
        public string Genre { get; set; }
        public string MultiPack { get; set; }

        public string ARReadingLevel { get; set; }
        public string ARInterestLevel { get; set; }
        public string RCInterestLevel { get; set; }
        public string RCReadingLevel { get; set; }
        public string FormatCode { get; set; }

        public decimal? ESPOverallRanking { get; set; }
        public decimal? ESPBisacRanking { get; set; }
        public string ESPDetailUrl
        {
            get { return _espDetailUrl != null ? _espDetailUrl.Trim() : string.Empty; }
            set { _espDetailUrl = value; }
        }

        /***********************************************************
         HasESPRanking is implemented within the database logic 
        **********************************************************/
        /*public bool HasESPRanking
        {
            get
            {
                return (ESPOverallRanking.HasValue && ESPOverallRanking.Value > 0) ||
                    (ESPBisacRanking.HasValue && ESPBisacRanking.Value > 0);
            }
        }*/

        public bool IsShared { get; set; }
        public bool IsGridded { get; set; }

        public int GridCount { get; set; }

        public int FreezeLevel { get; set; }

        public string LanguageLiteral { get; set; }

        public int NumOfDiscs { get; set; }
        public string Version { get; set; }
    }
}
