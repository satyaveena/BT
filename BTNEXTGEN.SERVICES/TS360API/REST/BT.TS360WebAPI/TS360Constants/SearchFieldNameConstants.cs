namespace BT.TS360Constants
{
    public sealed class SearchFieldNameConstants
    {
        /// <summary>
        /// Special search field for Basic Search
        /// </summary>
        public const string keyword = "keyword";

        /// <summary>
        /// Special search field for Advanced Search
        /// </summary>
        public const string SearchTerms = "terms";
        public const string partNumber = "ngpartnumber";
        
        // Search fields from FAST
        public const string acceleratedreader = "ngacceleratedreader";
        public const string acceptablediscount = "ngacceptablediscount";
        public const string annotations = "ngannotations";
        public const string audience = "ngaudience";
        public const string award = "ngawards";
        public const string ayprogram = "ngayprogramliteral";
        public const string ayprogramcode = "ngayprogramauxcodes";
        public const string begin = "Begin";
        public const string bibliography = "ngbibliographies";
        public const string booktypeliteral = "ngbooktypeliteral";
        public const string btekey = "ngbtekey";
        public const string btkey = "ngbtkey";
        public const string btprogram = "ngbtprogramliteral";
        public const string btprogramcode = "ngbtprogramauxcodes";
        public const string btprogramdate = "ngbtprogramdate";
        public const string catalog = "ngproductcatalog";
        public const string content = "ngcontent";
        public const string deweynative = "ngdeweynative";
        public const string deweynormalized = "ngdeweynormalized";
        public const string deweyprefix = "ngdeweyprefix";
        public const string deweyrange = "ngdeweyrange";
        public const string deweynormalizedfloat = "ngdeweynormalizedfloat";
        public const string econtentplatform = "ngecontentplatform";
        public const string edition = "ngedition";
        public const string format = "ngformatliteral";
        public const string formatcode = "ngformatcode";
        public const string gtin = "nggtin";
        public const string hasannotations = "nghasannotation";
        public const string hasaward = "nghasaward";
        public const string hasbibliography = "nghasbibliography";
        public const string hascitiation = "nghascitiation";
        public const string hascpsia = "nghascpsiawarning";
        public const string hasexcerpt = "nghasexcerpt";
        public const string hasfamilykeys = "nghasfamilykeys";
        public const string hasflap = "nghasflap";
        public const string hasjacket = "nghasjacket";
        public const string hasmuze = "nghasmuze";
        public const string hasreturn = "ngreturnindicator";
        public const string hasreview = "nghasreview";
        public const string hastoc = "nghastoc";
        public const string illustrator = "ngillustrator";
        public const string includedformats = "ngincludedformats";
        public const string isbn10 = "ngisbn10";
        public const string isbn13 = "ngisbn";
        public const string label = "nglabel";
        public const string languagecode = "nglanguagecode";
        public const string languageliteral = "nglanguageliteral";
        public const string lastupdate = "ngupdated";
        public const string lcclass = "nglcclass";
        public const string lcclassification = "nglcclassprefix";
        public const string lcclassprefix = "nglcclassprefix";
        public const string lccn = "nglccn";
        public const string leadingarticle = "ngleadingarticle";
        public const string lexile = "nglexile";
        public const string lexileprefix = "nglexileprefix";
        public const string lexilerange = "nglexilevalue";
        public const string listprice = "nglistprice";
        public const string marketcode = "ngmarketcode";
        public const string merchcategory = "ngmerchcategory";
        public const string moviegenre = "ngmoviegenre";
        public const string musicgenre = "ngmusicgenre";
        public const string narrator = "ngnarrator";
        public const string oclc = "ngoclccontrolnumber";
        public const string pricekey = "ngpricekey";
        public const string productfeatures = "ngproductfeatures";
        public const string productline = "ngproductline";
        public const string productstatus = "ngproductstatus";
        public const string producttype = "ngproducttype";
        public const string positivecode = "ngpositiveauxcodes";
        public const string pubdate = "ngpubdate";
        public const string odscreateddatetime = "ngodscreateddatetime";
        public const string pubdaterange = "ngpublicationrange";
        public const string publisher = "ngpublisher";
        public const string suppliercode = "ngsuppliercode";
        public const string rating = "ngrating";
        public const string readingcount = "ngreadingcount";
        public const string reportcode = "ngreportcode";
        public const string responsibleparty = "ngresponsibleparty";
        public const string responsiblepartyprimary = "ngresponsiblepartyprimary";
        public const string reviewcode = "ngreviewauxcodes";
        public const string reviewissuecount = "ngreviewissuecount";
        public const string reviewdate = "ngreviewdate";
        public const string reviewpub = "ngreviewpubissueliterals";
        public const string btpubliterals = "ngbtpubliterals";
        public const string reviewtext = "ngreviewtext";
        public const string reviewtype = "ngreviewtype";
        public const string series = "ngseries";
        public const string continuationseries = "ngcontinuationseries";
        public const string shorttitle = "ngshorttitle";
        public const string size = "ngsize";
        public const string starredcode = "ngstarredauxcodes";
        public const string streetdate = "ngstreetdate";
        public const string subject = "ngbisacsubject";
        public const string subject1 = "ngbisacsubject1";
        public const string librarySubj = "nglibrarysubjects";
        public const string subtitle = "ngsubtitle";
        public const string supplier = "ngsupplier";
        public const string title = "ngtitle";
        public const string toc = "ngtocs";
        public const string upc = "ngupc";
        public const string preorderdate = "ngpreorderdate";
        public const string generalsubjects = "nggeneralsubjects";
        public const string volumenumber = "ngvolumenumber";
        public const string academicsubjects = "ngacademicsubjects";
        public const string librarysubjects = "nglibrarysubjects";
        public const string replacementbtkey = "ngreplacementbtkey";
        public const string standingOrderID = "ngcontinuationseriesid";
        public const string standingOrderName = "ngcontinuationseriesname";
        public const string standingOrder = "ngcontinuationseries";
        public const string actor = "ngactor";
        public const string studio = "ngstudio";
        public const string album = "ngalbum";
        public const string keywords = "ngcontent";
        public const string numofdiscs = "ngnumofdiscs";

        // new fields for AC and RC
        public const string hasacceleratedreader = "nghasacceleratedreader";
        public const string hasreadingcounts = "nghasreadingcounts";

        // new field for multi author
        public const string responsiblepartys = "ngresponsiblepartys";

        // Product Code - using to determine Product Format
        public const string productattribute = "ngprimaryattributes";
        public const string childrensformat = "ngchildrensformat";
        public const string productcode = "ngproductcode";
        public const string btprograms = "ngbtprograms";
        public const string ayprograms = "ngayprograms";
        public const string btpublications = "ngbtpublications";

        public const string productranking = "ngproducts";
        public const string reviewnonissued = "ngnonissueauxcodes";

        public const string ivtfaceta = "ngstockfaceta";
        public const string ivtfacetd = "ngstockfacetd";
        public const string ivtfacetle = "ngstockfacetle";

        public const string demand = "ngdemand";
        public const string demandfacetnew = "ngdemandfacet";

        public const string auxcode = "ngauxcode"; // this value comes from MongoDB - AdvSearchBook_SearchTerms and not exist in FAST
        public const string auxdescription = "ngauxdescription"; // this value comes from MongoDB and not exist in FAST

        public const string boxoffice = "ngboxoffice"; 
        //Query Temrs - for Type Ahead Search
        public const string queryterm = "ngqueryterms";

        public static string autosuggestcontent = "autosuggestcontent";

        public const string releasemonth = "releasemonth";
        public const string releaseyear = "releaseyear";
        public const string releaseday = "releaseday";
        public const string releaseproducttype = "releaseproducttype";
        public const string calendarView = "calendarView";
        public const string isfromNRC = "isfromNRC";
        public const string isfeaturedtitles = "isfeaturedtitles";
        public const string promotionid = "promotionid";
        public const string elistid = "elistid";
        public const string ecateid = "ecateid";
        public const string publicationsubcategoryid = "publicationsubcategoryid";
        public const string publicationcategoryid = "publicationcategoryid";
        public const string publicationid = "pid";
        public const string publicationIssueId = "issueid";
        public const string year = "year";
        public const string month = "month";

        #region Custom search key
        public const string promoCode = "promoCode";
        public const string excludepurchaseoption = "exclpurchaseoption";
        public const string includepurchaseoption = "inclpurchaseoption";
        public const string excludereportcode = "exclreportcode";
        public const string includereportcode = "inclreportcode";
        public const string excludeproductattribute = "exclprodattr";
        public const string includeproductattribute = "inclprodattr";
        public const string excludemerchcategory = "excludemerchcategory";
        public const string excludenonreturnables = "excludenonreturnables";
        public const string excludemerchcategorygardner = "excludemerchcategorygardner";
        public const string excludemerchcategoryprintondemand = "excludemerchcategoryprintondemand";
        public const string excludeparentaladvisory = "excludeparentaladvisory";
        public const string includepawprintonly = "includepawprintonly";
        public const string acceleratedreaderrange = "acceleratedreaderrange";
        public const string readingcountrange = "readingcountrange";
        public const string futureonsaledate = "futureonsaledate";
        public const string cartdetailkeyword = "btkeyword";
        public const string preorderdaterange = "ngpreorderdate";
        public const string searchwithinnote = "searchwithinnote";
        public const string ngdeweyprefix = "ngdeweyprefix";
        #endregion

        //Demand boosting feature
        public const string demandweight = "ngdemandweight";
        public const string popularity = "ngpopularity";//get from DemanWeight in DB View

        //Search result view.
        public const string nextgensppublished = "nextgensppublished";
        public const string searchtypeaheaddemandweight = "ngquerytermdemandweight";

        //Product exclusion
        public const string marketrestrictioncode = "ngmarketrestrictioncode";

        public const string esupplier = "ngesupplier";
        public const string formdetails = "ngformdetail";
        public const string purchaseoption = "ngpurchaseoption";
        public const string adname = "adname";
        public const string featuredpromoid = "featuredpromoid";
        public const string whatsHotId = "whatshotid";

        public const string ReviewJournalDateFromTo = "ReviewJournalDateFromTo";
        public const string ReviewJournalSource = "ReviewJournalSource";
        public const string BTProgramsDateFromTo = "BTProgramsDateFromTo";
        public const string BtProgramSource = "BtProgramSource";

        public const string HasDemandHistory = "nghasdemandhistory";
        public const string HasPPC = "nghaspaypercirc";
        public const string ngversions = "ngversions";
        public const string blockedexportcountrycodes = "ngcountrycodeblockedexports";

        public const string StarredAuxCodesNew = "ngstarredauxcodes";
        public const string PositiveAuxCodesNew = "ngpositiveauxcodes";
        public const string PPCAuxCodes = "ngppcauxcodes";

        public const string discountkeys = "ngpricekey";

        public const string catalogpartnumber = "ngpartnumber";

        public const string includedproducttype = "ngincludedproducttype";
        public const string excludedproducttype = "ngexcludedproducttype";
    }
}
