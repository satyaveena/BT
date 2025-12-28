using System;
using System.Text;
using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts.Search
{
    public class FastSearchResultItem
    {
        private FSIS.Document document;

        public string BTKey { get; set; }
        public string PartNumber { get; set; }
        public string ISBN { get; set; }
        public string ISBN10 { get; set; }
        //protected string NGFormat { get; set; }
        protected string NGProductType { get; set; }
        public string IncludedFormat { get; set; }
        public string DeweyRange { get; set; }
        public string DeweyNormalized { get; set; }
        public string DeweyPrefix { get; set; }
        public string Deweynative { get; set; }
        public string LCClassification { get; set; }
        public string Subject { get; set; }
        public string Publisher { get; set; }
        public string SupplierCode { get; set; }
        public DateTime PublishDate { get; set; }
        public string PublishDateRange { get; set; }
        public string eContentPlatform { get; set; }
        public string Series { get; set; }
        public string MerchCategory { get; set; }
        public string Upc { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ResponsibleParty { get; set; }
        public string ResponsiblePartyPrimary { get; set; }
        public decimal ListPrice { get; set; }
        public string Edition { get; set; }
        public string ReadingCount { get; set; }
        public string AcceleratedReader { get; set; }
        public string ProductFeatures { get; set; }
        public bool HasAnnotations { get; set; }
        public bool HasExcerpt { get; set; }
        public bool HasToc { get; set; }
        public bool HasMuze { get; set; }
        public bool HasJacket { get; set; }
        public bool HasReview { get; set; }
        public bool HasAward { get; set; }
        public bool HasFlap { get; set; }
        public bool HasBibliography { get; set; }
        public bool HasReturn { get; set; }
        public bool HasCPSIA { get; set; }
        public bool HasPPC { get; set; }
        public string Annotation { get; set; }
        public string Bibliography { get; set; }
        public string Illustrator { get; set; }
        public string Narrator { get; set; }
        public string Toc { get; set; }
        public string Lccn { get; set; }
        public string ReviewText { get; set; }
        public string Lexile { get; set; }
        public string Label { get; set; }
        public DateTime LastUpdated { get; set; }
        public string MusicGenre { get; set; }
        public string MovieGenre { get; set; }
        public string Audience { get; set; }
        public string ProductType { get; set; }
        public string ProductFormat { get; set; }
        public string PrimaryAttribute { get; set; }
        public string PriceKey { get; set; }
        public string ReportCode { get; set; }
        public string LCClass { get; set; }
        public string GTIN { get; set; }
        public string Oclc { get; set; }
        public string StreetDate { get; set; }
        public string Rating { get; set; }
        public string Catalog { get; set; }
        public string ProductLine { get; set; }
        public decimal AcceptableDiscount { get; set; }
        public string BTEKey { get; set; }
        public string CPSIACode { get; set; }
        public string VolumnNumber { get; set; }
        public string Supplier { get; set; }
        public string FormatLiteral { get; set; }
        public string AcademicSubjects { get; set; }
        public string LibrarySubjects { get; set; }
        public string ReplacementBTKey { get; set; }
        public bool HasFamilyKey { get; set; }

        public bool HasDemandHistory { get; set; }
        public string LanguageLiteral { get; set; }

        public int NumOfDiscs { get; set; }
        public string ChildrenFormat { get; set; }
        /*
         * Code	Literal
            01	CHOKING HAZARD -- Small parts
            02	CHOKING HAZARD -- Balloon
            03	CHOKING HAZARD -- Small ball
            04	CHOKING HAZARD -- Contains small ball
            05	CHOKING HAZARD -- Marble
            06	CHOKING HAZARD – Contains marble
            07	No Hazard
         */
        public string CPSIAMessage
        {
            get
            {

                switch (CPSIACode)
                {
                    case "01":
                        return "CHOKING HAZARD:Small parts - Not for children under 3 yrs";
                    case "02":
                        return "CHOKING HAZARD:Balloon - Children under 8 yrs. can choke or suffocate on uninflated or broken balloons" + "<br>Adult supervision required." +
                        "<br>Keep uninflated balloons from children." + "<br>Discard broken balloons at once";
                    case "03":
                        return "CHOKING HAZARD:Small ball - Not for children under 3 yrs";
                    case "04":
                        return "CHOKING HAZARD:Contains small ball-Not for children under 3 yrs";
                    case "05":
                        return "CHOKING HAZARD:Marble-Not for children under 3 yrs";
                    case "06":
                        return "CHOKING HAZARD:Contains marble-Not for children under 3 yrs";
                    case "07":
                        return "";
                    default:
                        return "CHOKING HAZARD:Balloon - Children under 8 yrs. can choke or suffocate on uninflated or broken balloons" + "<br>Adult supervision required." +
                        "<br>Keep uninflated balloons from children." + "<br>Discard broken balloons at once"; ;
                }
            }
        }
        public string Author
        {
            get
            {
                return this.ResponsiblePartyPrimary;
            }
            set
            {
                this.ResponsiblePartyPrimary = value;
            }
        }

        public string Genre
        {
            get
            {
                switch (this.ProductType)
                {
                    case ProductTypeConstants.Movie:
                        return this.MovieGenre;
                    case ProductTypeConstants.Music:
                        return this.MusicGenre;
                    default:
                        return string.Empty;
                }
            }
        }

        public string ReleaseDate
        {
            get
            {
                switch (this.ProductType)
                {
                    case ProductTypeConstants.Movie:
                    case ProductTypeConstants.Music:
                        return this.StreetDate;
                    default:
                        if (this.PublishDate != null)
                        {
                            var pubDate = this.PublishDate.Date.ToShortDateString();
                            return pubDate;
                        }
                        return string.Empty;
                }
            }
        }

        public string EditionVolumeString
        {
            get
            {
                var sbEditionVolume = new StringBuilder(string.Empty);

                var strVolume = !string.IsNullOrEmpty(this.VolumnNumber)
                ? GetVolumnNumber(this.VolumnNumber)
                : string.Empty;

                sbEditionVolume.Append(string.Format("{0}{1}",
                                this.Edition,
                                !string.IsNullOrEmpty(strVolume)
                                    ? string.Format("{0}{1}",
                                        !string.IsNullOrEmpty(this.Edition) ? " / " : string.Empty, strVolume)
                                    : string.Empty));

                return sbEditionVolume.ToString();
            }
        }

        public string FormatIconPath
        {
            get
            {
                string path = "/_layouts/IMAGES/CSDefaultSite/assets/images/icons/";

                //ProductCode = ngproductcode(FAST), ProductCodeFast(Products table)
                //ProductFormat = ngformatliteral(FAST), FormatLiteral(Products table)
                //ProductLine = ngproductline(FAST), ProductLineLiteral(Products table), ProductLine (FAST View).

                switch (this.ProductCode)
                {
                    //case ProductCodeConstants.BD:
                    //    path += ProductIconPathConstants.BD;
                    //    break;
                    //case ProductCodeConstants.PL:
                    //    path += ProductIconPathConstants.PL;
                    //    break;
                    case ProductCodeConstants.MUSIC:
                        if (this.FormatLiteral == ProductFormatConstants.Music_Vinyl_12 || this.FormatLiteral == ProductFormatConstants.Music_Vinyl_7I || this.FormatLiteral == ProductFormatConstants.Music_Vinyl_LP)
                        {
                            path += ProductIconPathConstants.VINYL;
                        }
                        else
                        {
                            path += ProductIconPathConstants.MUSIC;
                        }
                        break;
                    case ProductCodeConstants.MOVIE:
                        if (this.ProductLine.ToUpper() == ProductLineConstants.Video.ToUpper())
                        {
                            switch (this.ProductFormat)
                            {
                                case ProductFormatConstants.Movie_Blu_ray:
                                case ProductFormatConstants.Movie_Blu_ray_Hi_Def_DVD:
                                    path += ProductIconPathConstants.BD; //Blu-ray icon
                                    break;
                                case ProductFormatConstants.Movie_Playaway_View:
                                    path += ProductIconPathConstants.PL; //Playaway View icon 
                                    break;
                                case ProductFormatConstants.Ultra_HD_Blu_Ray:
                                    path += ProductIconPathConstants.ICON_4k; // 4K icon
                                    break;
                                default:
                                    path += ProductIconPathConstants.MOVIE;
                                    break;
                            }
                        }
                        else
                        {
                            path += ProductIconPathConstants.MOVIE;
                        }
                        break;
                    case ProductCodeConstants.EBOOK:
                        path += ProductIconPathConstants.EBOOK;
                        break;
                    case ProductCodeConstants.BOOK:
                        if (this.ProductLine.ToUpper() == ProductLineConstants.Digit.ToUpper()
                                        && this.ProductFormat == ProductFormatConstants.Book_PreLoaded_Audio_Player)
                        {
                            path += ProductIconPathConstants.PA;
                        }
                        else if (this.ProductLine.ToUpper() == ProductLineConstants.DownloadableAudio.ToUpper()
                            && this.ProductFormat == ProductFormatConstants.EBook_Downloadable_Audio)
                        {
                            path += ProductIconPathConstants.EBOOK;
                        }
                        else
                        {
                            switch (this.MerchCategory)
                            {
                                case ProductMerchandiseCategoryConstants.PRINT_ON_DEMAND:
                                case ProductMerchandiseCategoryConstants.TEXTSTREAM_PRINT_ON_DEMAND:
                                    path += ProductIconPathConstants.PRINT_ON_DEMAND;
                                    break;
                                default:
                                    if (this.ProductLine.ToLower() == ProductCodeConstants.EBOOK.ToLower())
                                        path += ProductIconPathConstants.EBOOK;
                                    else
                                        path += ProductIconPathConstants.BOOK;
                                    break;
                            }
                        }
                        break;
                    default:
                        // return blank ICO
                        path += ProductIconPathConstants.EMPTY;
                        break;
                }

                return path;
            }

        }

        private string formatClass = null;
        public string FormatClass
        {
            get
            {
                if (formatClass == null)
                {
                    formatClass = string.Empty;
                    switch (this.ProductCode)
                    {
                        case ProductCodeConstants.MUSIC:
                            if (this.FormatLiteral == ProductFormatConstants.Music_Vinyl_12 || this.FormatLiteral == ProductFormatConstants.Music_Vinyl_7I || this.FormatLiteral == ProductFormatConstants.Music_Vinyl_LP)
                            {
                                formatClass = CommonConstants.ICON_VINYL;
                            }
                            else
                            {
                                formatClass = CommonConstants.ICON_CD;
                            }
                            break;
                        case ProductCodeConstants.MOVIE:
                            if (this.ProductLine.ToUpper() == ProductLineConstants.Video.ToUpper())
                            {
                                switch (this.FormatLiteral)
                                {
                                    case ProductFormatConstants.Movie_Blu_ray:
                                    case ProductFormatConstants.Movie_Blu_ray_Hi_Def_DVD:
                                        formatClass = CommonConstants.ICON_BLUERAY; //Blu-ray icon
                                        break;
                                    case ProductFormatConstants.Movie_Playaway_View:
                                        formatClass = CommonConstants.ICON_PLAYAWAY_VIEW; //Playaway View icon 
                                        break;
                                    case ProductFormatConstants.Ultra_HD_Blu_Ray:
                                        formatClass = CommonConstants.ICON_4K;  // 4K icon
                                        break;
                                    default:
                                        formatClass = CommonConstants.ICON_DVD;
                                        break;
                                }
                            }
                            else
                            {
                                formatClass = CommonConstants.ICON_DVD;
                            }
                            break;
                        case ProductCodeConstants.EBOOK:
                            formatClass = CommonConstants.ICON_EBOOK;
                            break;
                        case ProductCodeConstants.BOOK:
                            if (this.FormatLiteral == ProductFormatConstants.Book_PreLoaded_Audio_Player)
                            {
                                formatClass = CommonConstants.ICON_PRELOADED_AUDIO;
                                break;
                            }

                            if (this.FormatLiteral == ProductFormatConstants.Book_Hardcover && this.MerchCategory.Equals(ProductMerchandiseCategoryConstants.MAKERSPACE, StringComparison.OrdinalIgnoreCase))
                            {
                                formatClass = CommonConstants.ICON_MAKERSPACE;
                                break;
                            }

                            switch (this.MerchCategory)
                            {
                                case ProductMerchandiseCategoryConstants.PRINT_ON_DEMAND:
                                case ProductMerchandiseCategoryConstants.TEXTSTREAM_PRINT_ON_DEMAND:
                                    formatClass = CommonConstants.ICON_POD;
                                    break;
                                default:
                                    if (this.ProductLine.ToLower() == ProductCodeConstants.EBOOK.ToLower())
                                        formatClass = CommonConstants.ICON_EBOOK;
                                    else
                                        formatClass = CommonConstants.ICON_BOOK;
                                    break;
                            }
                            break;
                        default:
                            // return blank ICO
                            formatClass = string.Empty;
                            break;
                    }
                }
                return formatClass;
            }
        }

        public string FormatIconClass
        {
            get
            {
                var cssClass = string.Empty;
                switch (this.ProductCode)
                {
                    case ProductCodeConstants.MUSIC:
                        if (this.FormatLiteral == ProductFormatConstants.Music_Vinyl_12 || this.FormatLiteral == ProductFormatConstants.Music_Vinyl_7I || this.FormatLiteral == ProductFormatConstants.Music_Vinyl_LP)
                        {
                            cssClass = "icon_format_vinyl";
                        }
                        else
                        {
                            cssClass = "icon_format_cd";
                        }
                        break;
                    case ProductCodeConstants.MOVIE:
                        if (this.ProductLine.ToUpper() == ProductLineConstants.Video.ToUpper())
                        {
                            switch (this.FormatLiteral)
                            {
                                case ProductFormatConstants.Movie_Blu_ray:
                                case ProductFormatConstants.Movie_Blu_ray_Hi_Def_DVD:
                                    cssClass = "icon_format_bd"; //Blu-ray icon
                                    break;
                                case ProductFormatConstants.Movie_Playaway_View:
                                    cssClass = "icon_format_pv"; //Playaway View icon 
                                    break;
                                case ProductFormatConstants.Ultra_HD_Blu_Ray:
                                    cssClass = "ico-4k";//4K icon
                                    break;
                                default:
                                    cssClass = "icon_format_dvd";
                                    break;
                            }
                        }
                        else
                        {
                            cssClass = "icon_format_dvd";
                        }
                        break;
                    case ProductCodeConstants.EBOOK:
                        cssClass = "icon_format_bookelectronic";
                        break;
                    case ProductCodeConstants.BOOK:
                        if (this.FormatLiteral == ProductFormatConstants.Book_PreLoaded_Audio_Player)
                        {
                            cssClass = "icon_format_pa";
                            break;
                        }
                        switch (this.MerchCategory)
                        {
                            case ProductMerchandiseCategoryConstants.PRINT_ON_DEMAND:
                            case ProductMerchandiseCategoryConstants.TEXTSTREAM_PRINT_ON_DEMAND:
                                cssClass = "icon_format_pod";
                                break;
                            default:
                                if (this.ProductLine.ToLower() == ProductCodeConstants.EBOOK.ToLower())
                                    cssClass = "icon_format_bookelectronic";
                                else
                                    cssClass = "icon_format_book";
                                break;
                        }
                        break;
                    default:
                        // return blank ICO
                        cssClass = string.Empty;
                        break;
                }
                return cssClass;
            }
        }

        public string IncludedFormatClass
        {
            get
            {
                var cssClass = string.Empty;
                if (!string.IsNullOrEmpty(this.IncludedFormat) && this.IncludedFormat.Trim() != string.Empty)
                    cssClass = CommonConstants.ICON_INCLUDED_FORMAT;

                return cssClass;
            }
        }

        public string PreOrderDate { get; set; }

        public bool HasAcceleratedReader { get; set; }
        public bool HasReadingCounts { get; set; }

        public string ResponsiblePartys { get; set; }

        public string ProductCode { get; set; }

        public string ContinuationSeries { get; set; }

        public string ESupplier { get; set; }
        public string FormDetails { get; set; }
        public string PurchaseOption { get; set; }

        public string Versions { get; set; }
        public string BlockedExportCountryCodes { get; set; }

        public string StarredAuxCodes { get; set; }
        public string PositiveAuxCodes { get; set; }
        public string PPCAuxCodes { get; set; }


        /// <summary>
        /// Initializes a new instance of the FastSearchResultItem class. Default constructor.
        /// </summary>
        public FastSearchResultItem()
        {
            // has jacket by default
            this.HasJacket = true;
        }

        /// <summary>
        /// Initializes a new instance of the FastSearchResultItem class.
        /// </summary>
        /// <param name="document">Search result document</param>
        public FastSearchResultItem(BT.FSIS.Document document)
        {
            this.document = document;
            this.InitializeProperties();
        }

        /// <summary>
        /// Initialize properties
        /// </summary>
        private void InitializeProperties()
        {
            this.BTKey = this.GetDocumentField(SearchFieldNameConstants.btkey);
            this.PartNumber = this.GetDocumentField(SearchFieldNameConstants.partNumber);
            this.ISBN = this.GetDocumentField(SearchFieldNameConstants.isbn13);
            this.ISBN10 = this.GetDocumentField(SearchFieldNameConstants.isbn10);
            ProductFormat = this.FormatLiteral = this.GetDocumentField(SearchFieldNameConstants.format);
            this.NGProductType = this.GetDocumentField(SearchFieldNameConstants.producttype);
            this.IncludedFormat = this.GetDocumentField(SearchFieldNameConstants.includedformats);
            this.DeweyRange = this.GetDocumentField(SearchFieldNameConstants.deweyrange);
            this.DeweyPrefix = this.GetDocumentField(SearchFieldNameConstants.deweyprefix);
            this.Deweynative = this.GetDocumentField(SearchFieldNameConstants.deweynative);
            this.LCClassification = this.GetDocumentField(SearchFieldNameConstants.lcclassification);
            this.Subject = this.GetDocumentField(SearchFieldNameConstants.subject1);
            this.SupplierCode = this.GetDocumentField(SearchFieldNameConstants.suppliercode);
            this.Publisher = this.GetDocumentField(SearchFieldNameConstants.publisher);
            this.PublishDate = this.ToUniversalTime(this.GetDocumentField(SearchFieldNameConstants.pubdate));
            this.PublishDateRange = this.GetDocumentField(SearchFieldNameConstants.pubdaterange);
            this.LanguageLiteral = this.GetDocumentField(SearchFieldNameConstants.languageliteral);
            this.eContentPlatform = this.GetDocumentField(SearchFieldNameConstants.econtentplatform);
            this.Series = this.GetDocumentField(SearchFieldNameConstants.series);
            this.MerchCategory = this.GetDocumentField(SearchFieldNameConstants.merchcategory);
            this.Upc = this.GetDocumentField(SearchFieldNameConstants.upc);
            this.Title = this.GetDocumentField(SearchFieldNameConstants.title);
            this.SubTitle = this.GetDocumentField(SearchFieldNameConstants.subtitle);
            this.ResponsibleParty = this.GetDocumentField(SearchFieldNameConstants.responsibleparty);
            this.ResponsiblePartyPrimary = this.GetDocumentField(SearchFieldNameConstants.responsiblepartyprimary);
            this.ListPrice = this.ToDecimal(this.GetDocumentField(SearchFieldNameConstants.listprice));
            this.AcceptableDiscount = this.ToDecimal(this.GetDocumentField(SearchFieldNameConstants.acceptablediscount));
            this.Edition = this.GetDocumentField(SearchFieldNameConstants.edition);
            this.ReadingCount = this.GetDocumentField(SearchFieldNameConstants.readingcount);
            this.AcceleratedReader = this.GetDocumentField(SearchFieldNameConstants.acceleratedreader);
            this.ProductFeatures = this.GetDocumentField(SearchFieldNameConstants.productfeatures);
            this.HasAnnotations = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasannotations));
            this.HasExcerpt = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasexcerpt));
            this.HasToc = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hastoc));
            this.HasMuze = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasmuze));
            this.HasJacket = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasjacket));
            this.HasReview = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasreview));
            this.HasAward = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasaward));
            this.HasFlap = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasflap));
            this.HasBibliography = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasbibliography));
            this.HasCPSIA = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hascpsia));
            this.HasPPC = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.HasPPC));
            this.Annotation = this.GetDocumentField(SearchFieldNameConstants.annotations);
            this.Bibliography = this.GetDocumentField(SearchFieldNameConstants.bibliography);
            this.Illustrator = this.GetDocumentField(SearchFieldNameConstants.illustrator);
            this.Narrator = this.GetDocumentField(SearchFieldNameConstants.narrator);
            this.Toc = this.GetDocumentField(SearchFieldNameConstants.toc);
            this.Lccn = this.GetDocumentField(SearchFieldNameConstants.lccn);
            this.ReviewText = this.GetDocumentField(SearchFieldNameConstants.reviewtext);
            this.Label = this.GetDocumentField(SearchFieldNameConstants.label);
            this.MusicGenre = this.GetDocumentField(SearchFieldNameConstants.musicgenre);
            this.MovieGenre = this.GetDocumentField(SearchFieldNameConstants.moviegenre);
            this.PriceKey = this.GetDocumentField(SearchFieldNameConstants.pricekey);
            this.Lexile = this.GetDocumentField(SearchFieldNameConstants.lexile);
            this.LastUpdated = this.ToUniversalTime(this.GetDocumentField(SearchFieldNameConstants.lastupdate));
            this.Audience = this.GetDocumentField(SearchFieldNameConstants.audience);
            this.ReportCode = this.GetDocumentField(SearchFieldNameConstants.reportcode);
            this.LCClass = this.GetDocumentField(SearchFieldNameConstants.lcclass);
            this.GTIN = this.GetDocumentField(SearchFieldNameConstants.gtin);
            this.Oclc = this.GetDocumentField(SearchFieldNameConstants.oclc);
            this.BTEKey = this.GetDocumentField(SearchFieldNameConstants.btekey);
            this.VolumnNumber = this.GetDocumentField(SearchFieldNameConstants.volumenumber);
            this.Supplier = this.GetDocumentField(SearchFieldNameConstants.supplier);
            this.DeweyNormalized = this.GetDocumentField(SearchFieldNameConstants.deweynormalized);
            DateTime dt = this.ToUniversalTime(this.GetDocumentField(SearchFieldNameConstants.streetdate));
            this.NumOfDiscs = this.ToInt(this.GetDocumentField(SearchFieldNameConstants.numofdiscs));
            this.NumOfDiscs = this.ToInt(this.GetDocumentField(SearchFieldNameConstants.numofdiscs));
            this.ChildrenFormat = this.GetDocumentField(SearchFieldNameConstants.childrensformat);

            if (dt == DateTime.MinValue)
                this.StreetDate = "";
            else
                this.StreetDate = dt.ToString(@"MM/dd/yyyy");
            this.Rating = this.GetDocumentField(SearchFieldNameConstants.rating);
            this.Catalog = this.GetDocumentField(SearchFieldNameConstants.catalog);
            this.ProductLine = this.GetDocumentField(SearchFieldNameConstants.productline);
            var s = this.NGProductType.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            ProductType = s.Length > 0 ? s[0] : "Book";

            this.HasReturn = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasreturn));
            DateTime preOrderDate = this.ToUniversalTime(this.GetDocumentField(SearchFieldNameConstants.preorderdate));
            if (preOrderDate == DateTime.MinValue || preOrderDate.Date == new DateTime(1900, 1, 1))
                this.PreOrderDate = "";
            else
                this.PreOrderDate = preOrderDate.ToString(@"MM/dd/yyyy");

            this.PrimaryAttribute = this.GetDocumentField(SearchFieldNameConstants.productattribute);

            this.HasAcceleratedReader = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasacceleratedreader));
            this.HasReadingCounts = this.ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasreadingcounts));
            this.ResponsiblePartys = this.GetDocumentField(SearchFieldNameConstants.responsiblepartys);
            this.ProductCode = this.GetDocumentField(SearchFieldNameConstants.productcode);
            this.ContinuationSeries = this.GetDocumentField(SearchFieldNameConstants.continuationseries);

            this.AcademicSubjects = this.GetDocumentField(SearchFieldNameConstants.academicsubjects);
            this.LibrarySubjects = this.GetDocumentField(SearchFieldNameConstants.librarysubjects);
            this.ReplacementBTKey = this.GetDocumentField(SearchFieldNameConstants.replacementbtkey);
            //this.ReplacementBTKey = "0003456284";

            ESupplier = GetDocumentField(SearchFieldNameConstants.esupplier);
            FormDetails = GetDocumentField(SearchFieldNameConstants.formdetails);
            PurchaseOption = GetDocumentField(SearchFieldNameConstants.purchaseoption);
            HasFamilyKey = ToBoolean(this.GetDocumentField(SearchFieldNameConstants.hasfamilykeys));
            this.HasDemandHistory = ToBoolean(this.GetDocumentField(SearchFieldNameConstants.HasDemandHistory));
            Versions = this.GetDocumentField(SearchFieldNameConstants.ngversions);
            this.BlockedExportCountryCodes = this.GetDocumentField(SearchFieldNameConstants.blockedexportcountrycodes);

            this.StarredAuxCodes = this.GetDocumentField(SearchFieldNameConstants.StarredAuxCodesNew);
            this.PositiveAuxCodes = this.GetDocumentField(SearchFieldNameConstants.PositiveAuxCodesNew);
            this.PPCAuxCodes = this.GetDocumentField(SearchFieldNameConstants.PPCAuxCodes);
        }

        private bool ToBoolean(string value)
        {
            return value == "1";
        }

        private decimal ToDecimal(string value)
        {
            decimal result = 0;
            decimal.TryParse(value, out result);
            return result;
        }

        private int ToInt(string value)
        {
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }

        private DateTime ToUniversalTime(string value)
        {
            DateTime result = DateTime.MinValue;
            if (DateTime.TryParse(value, out result))
            {
                result = result.ToUniversalTime();
            }
            return result;
        }

        private DateTime ToDateTime(string value)
        {
            DateTime result = DateTime.MinValue;
            DateTime.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Get value of a field in search document 
        /// </summary>
        /// <param name="key">field key parameter</param>
        /// <returns>field value string</returns>
        private string GetDocumentField(string key)
        {
            string fieldValue = string.Empty;
            if (!string.IsNullOrEmpty(key))
            {
                // key must be in lower case to match with FAST result
                key = key.ToLower();
                if (this.document != null &&
                    this.document.DocumentFields != null &&
                    this.document.DocumentFields.Keys.Contains(key))
                {
                    fieldValue = this.document.DocumentFields[key];
                    //set empty for value of "null"
                    if (fieldValue == null)
                    {
                        return string.Empty;
                    }
                    //set empty for value of "null"
                    if (fieldValue.ToLower().Equals("null"))
                    {
                        fieldValue = string.Empty;
                    }
                }
            }
            return fieldValue;
        }

        private string GetVolumnNumber(string p)
        {
            p = p.Trim();
            if (!p.Contains("Vol."))
            {
                p = "Vol." + p;
            }
            return p;
        }
    }
}
