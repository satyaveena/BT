using BT.TS360API.Common.Constants;

namespace BT.TS360Constants
{
    public static class BasketLineItemFacet
    {
        public const string Format = "Format";
        public const string Inventory = "Inventory";
        public const string Quantity = "Quantity";
        public const string Notes = "Notes";
        public const string WithQuantity = "WITH_QUANTITY";
        public const string WithoutQuantity = "WITHOUT_QUANTITY";
        public const string WithNotes = "WITH_NOTE";
        public const string WithoutNotes = "WITHOUT_NOTE";
        public const string OriginalEntry = "OriginalEntry";
        public const string WITH_OE = "WITH_OE";
        public const string WITHOUT_OE = "WITHOUT_OE";
        public const string Grids = "Grids";
        public const string AUDIENCE_LEVEL = "audiencelevel";
        public const string FICTION_NONFICTION = "fictionnonfiction";
        public const string DIVERSITY_TOPIC = "diversitytopic";
    }
    public class CommonConstants
    {
        public static string publishstatus = "OUT OF BUSINESS|APPLY DIRECT|OUT OF PRINT|PERMANENTLY OUT OF STOCK|UNABLE TO LOCATE |PRODUCT CANCELLED";
        public static string publishstatusEnt = "OUT OF BUSINESS|APPLY DIRECT|OUT OF PRINT|PERMANENTLY OUT OF STOCK|UNABLE TO LOCATE |PRODUCT CANCELLED|Out of Stock|Publication Cancelled|Not Available Through BT|Availability Under Investigation";
        public static string exmerchant = "|TEXT STREAM PRINT ON DEMAND|PRINT ON DEMAND";
        public const string ICON_CD = "icon-cd";
        public const string ICON_VINYL = "icon-vinyl";

        public const string ICON_BLUERAY = "icon-blueray";
        public const string ICON_4K = "icon_format_4k";
        public const string ICON_PLAYAWAY_VIEW = "icon-playaway-view";
        public const string ICON_DVD = "icon-dvd";
        public const string ICON_EBOOK = "icon-ebook";
        public const string ICON_POD = "icon-pod";
        public const string ICON_BOOK = "icon-book";
        public const string ICON_INCLUDED_FORMAT = "icon-included-format";
        public const string ICON_PRELOADED_AUDIO = "icon-preloaded-audio";
        public const string ICON_MAKERSPACE = "ico-makerspace";

        public const int MAXIMUM_CART_NAME_LENGTH = 11;
        public const int MAX_TOTALQUANTITY_LENGTH = 6;
        public const int MAX_PRICE_LENGTH = 12;
        public const int MAX_LINE_ITEM_LENGTH = 4;
        public const int MassQuantity_Threshold = 2000;
        public const int DEFAULT_NUMBER_OF_ACTIVE_CARTS = 10;

        #region Content Cafe

        // User Info to Get Content from Content Cafe
        public const string UserIdForContent = "OMNIPROD";
        public const string PasswordForContent = "BT20110104";
        // User Info to Get Muze from Content Cafe
        public const string UserIdForMuze = "CLS";
        public const string PasswordForMuze = "baker";

        // User for Content Cafe

        public const string UserForTest = "NextGenTst";
        public const string PwdForTest = "NGT360";

        public const string UserForProduction = "NextGenPrd";
        public const string PwdForproduction = "NGP360";

        public const string NoImageSmallSize = "no-jacket-small.jpg";
        public const string NoImageMediumSize = "no-jacket-medium.jpg";
        public const string NoImageLargeSize = "no-jacket-large.jpg";

        public const char BisacSeparator = '/';
        public const char ReadingCountSeparator = '#';
        public const char AcceleratedReaderSeparator = '#';

        #endregion
        public const string ItemDetailPageUrl = "/_layouts/CommerceServer/ItemDetailsPage.aspx?";

        public const string StringDelimiter = ",";
        public const string Search_DateTimeRange_Format = "MMddyyyy";
        public const string BatchEntry_AfterSave = "aftersave";
        public const string BatchEntry_Error_Session = "aftersave";
        public const string BatchEntry_Carts_Session = "finalcart";
        public const int BatchEntry_File_Size = 4 * 1024 * 1024;
        public const string BatchEntry_Exception_Message = "errormessage";
        public const string BatchEntry_ErrorInSession = "BatchEntry_ErrorInSession";
        public const string BatchEntry_ErrorInSessionForSplitCarts = "BatchEntry_ErrorInSessionForSplitCarts";
        public const string BatchEntry_SuccessMessageWithoutSplitCart = "BatchEntry_SuccessMessageWithoutSplitCart";
        public const string BatchEntry_SuccessMessageWithSplitCart = "BatchEntry_SuccessMessageWithSplitCart";
        public const string BatchEntry_ResetCartCache = "BatchEntry_ResetCache";
        public const string BatchEntry_FinalCart = "BatchEntry_FinalCart";
        public const string BatchEntry_TotalItems = "BatchEntry_TotalItems";
        public const string BatchEntry_PermissionViolationMessage = "BEPVM";
        public const string BatchEntry_ShowLastItemProcessed = "BatchEntry_ShowLastItemProcessed";
        public const string BatchEntry_SuccessMessageForBackgroundProcessing = "BatchEntry_SuccessMessageForBackgroundProcessing";
        public const string BatchEntryMode = "BATCH_ENTRY";
        public const string INCLUDE_NO_SPECIFIED_AUDIDENCE_SITETERM_TEXT = "Include Products with No Specific Audience";

        public const string ExpresionXmlTemplate = "<Expression><Operator>{0}</Operator><Scope>{1}</Scope><Term>{2}</Term><Name>{3}</Name></Expression>";
        public const string ExpressionXmlOpenRoot = "<Expressions>";
        public const string ExpressionXmlCloseRoot = "</Expressions>";
        public const string AllKeyWord = "All";
        public const string ISBN10_Frame_Name = "loolkupholder10";
        public const string ISBN13_Frame_Name = "lookupholder13";
        public const string ISBN_Lookup_separator = "@@@";
        public const string ISBN_Show13 = "0";
        public const string ISBN_Show10 = "1";
        public const string ISBN_Show_Both = "2";
        public const string True = "True";
        public const string False = "False";

        // RadTooltip manager HideDelay value in second
        public const int TooltipHideDelay = 5000;

        public const int MAX_NUMBER_TO_DISPLAY_IN_BISAC_SEARCH_SUMMARY = 4;

        public const string ScriptManager = "ScriptManager";

        /// <summary>
        /// Deal with result per page cookie value
        /// </summary>
        public const string ResultPerPageCookieName = "ResultPerPage_CookieName";

        public const string DefaultImageUrl = "http://images.btol.com/CC2Images/Image.aspx?SystemID={0}&IdentifierID=I&IdentifierValue={1}&Source=BT&Category=FC&Sequence=1&Size={2}&NotFound={3}";
        public const string DefaultDateTimeFormat = "MM/dd/yyyy";

        public const string ViewAllTitlesDefaultValue = "0";
        public const string IsValidationCartGridsDefaultValue = "0";

        public const int PageRange = 5;

        public const string DisableGridsOption = "DisableGridsOption";
        public const string LongDateTimeFormat = "MM/dd/yyyy hh:mm tt";
        public const string SQLGeneralErrorMessage = "Cart_General_SQL_Exception_Error_Message";
        public const string ProxyUserSuffix = "_PROXYUSER";
        public const string HttpRequestReferer = "Referer";
        public const string LongDateTimeWithSecondFormat = "MM/dd/yyyy hh:mm:ss tt";

        public const string IsGridExpandedOnItemDetails = "IsGridExpandedOnItemDetails";

        //public const VelocityCacheLevel CmCachingLevel = VelocityCacheLevel.WebFrontEnd;
        public const int CmCachingDuration = 30;
        public const int ScriptCacheMinutes = 720;

        public const string SplitCartActionValue = "splitcart";
        public const string ManageCartsPageDefaultKeywordType = "4";
        public const string CartDetailsPageDefaultKeywordType = "6";

        public const string CartDetailsLineItemNoteCacheKey = "___CartDetailsLineItemNoteCacheKey{0}"; // {CartId}
        public const string DefaultLanguageLiteral = "ENGLISH";

        public const int BactEntryDefaultQuantity = -100;
        public const string NALiteral = "N/A";

        public const string ESPRankingWarningMessageFormat_Single = "<b>{0} item</b> has not been ranked. Click {1} to manually rank this item now, or {2} to auto-rank overnight.";
        public const string ESPRankingWarningMessageFormat_Plurals = "<b>{0} items</b> have not been ranked. Click {1} to manually rank these items now, or {2} to auto-rank them overnight.";

        public const string DEFAULT_GRID_CART_DETAIL = "DEFAULT_GRID_CART_DETAIL";
    }

    public class CustomerServiceConstants
    {
        public const string REQUIRE_REQUEST = "Request is required.";
        public const string REQUIRE_ACCOUNT_NUMBER = "Account Number is required.";
        public const string REQUIRE_ACCOUNT_ID = "AccountID is required.";
        public const string REQUIRE_ORDER_DATE = "Order Date is required.";
        public const string REQUIRE_USER_ID = "UserID is required.";
        public const string REQUIRE_DASHBOARD_NAME = "Dashboard Name is required.";
        public const string REQUIRE_ACCOUNT_TYPE = "Account Type is required.";
        public const string REQUIRE_DASHBOARD_ID = "DashboardID is required.";
    }

    public sealed class CartFrameworkConstants
    {
        public const byte MAX_PAGE_SIZE = byte.MaxValue;

        public const byte NORMAL_PAGE_SIZE = 25;

        public const byte LARGE_PAGE_SIZE = 100;

        public const int BookAccountType = 2;

        public const int EntertainmentAccountType = 3;

        public const int PricingBatchWaitingTime = 100;
    }

    public class ProductMerchandiseCategoryConstants
    {
        public const string PRINT_ON_DEMAND = "PRINT ON DEMAND";
        public const string TEXTSTREAM_PRINT_ON_DEMAND = "TEXTSTREAM PRINT ON DEMAND";
        public const string MAKERSPACE = "MAKERSPACE";
    }

    public sealed class ProductFormatConstants
    {
        //book
        public const string Book_Hardcover = "Hardcover";
        public const string Book_Paperback = "Paperback";
        public const string Book_Cdrom = "CD-ROM";
        public const string Book_CompactDisc = "Compact Disc";
        public const string Book_Dvd = "DVD";
        public const string Book_Cassette = "Cassette";
        public const string Book_Bath_Book = "Bath Book";
        public const string Book_Blu_ray_Hi_Def_DVD = "Blu-ray Hi-Def DVD";
        public const string Book_Board_Book = "Board Book";
        public const string Book_Board_Game = "Board Game";
        public const string Book_Booklet = "Booklet";
        public const string Book_Cards = "Cards";
        public const string Book_Chart = "Chart";
        public const string Book_Diskette = "Diskette";
        public const string Book_DVD_ROM = "DVD-ROM";
        public const string Book_Filmstrip = "Filmstrip";
        public const string Book_Gift_Wrap = "Gift Wrap";
        public const string Book_Library = "Library";
        public const string Book_Loose_Leaf = "Loose Leaf";
        public const string Book_Magnet = "Magnet";
        public const string Book_Map = "Map";
        public const string Book_Medical_Equipment = "Medical Equipment";
        public const string Book_Microfiche = "Microfiche";
        public const string Book_Microfilm = "Microfilm";
        public const string Book_MP3_CD = "MP3-CD";
        public const string Book_Otabind = "Otabind";
        public const string Book_Pamphlet = "Pamphlet";
        public const string Book_Pass_Code = "Pass Code";
        public const string Book_Plush = "Plush";
        public const string Book_Poster = "Poster";
        public const string Book_Prebind = "Prebind";
        public const string Book_PreLoaded_Audio_Player = "Pre-Loaded Audio Player";
        public const string Book_Puzzle = "Puzzle";
        public const string Book_Quiz = "Quiz";
        public const string Book_Rag_Book = "Rag Book";
        public const string Book_Reinforced = "Reinforced";
        public const string Book_Slides = "Slides";
        public const string Book_Software = "Software";
        public const string Book_Toy = "Toy";
        public const string Book_Transparencies = "Transparencies";
        public const string Book_Turtleback = "Turtleback";
        public const string Book_Unbound = "Unbound";
        public const string Book_VAS = "VAS";
        public const string Book_VHS = "VHS";
        public const string Book_Makerspace = "Makerspace";
        //movie
        public const string Movie_3_inches_Mini_DVD = "3 Inch Mini DVD";
        public const string Movie_Blu_ray = "Blu-ray";
        public const string Movie_Blu_ray_Hi_Def_DVD = "Blu-ray Hi-Def DVD";
        public const string Ultra_HD_Blu_Ray = "Ultra HD Blu-Ray";
        public const string Movie_Compact_Disc = "Compact Disc";
        public const string Movie_Digital_VHS = "Digital VHS";
        public const string Movie_Dual_Format_CD_DVD = "Dual Format CD/DVD";
        public const string Movie_DVD = "DVD";
        public const string Movie_DVD_Blu_ray_Disc_Dual_Format = "DVD/Blu-ray Disc Dual Format";
        public const string Movie_DVD_HD_DVD_Dual_Format = "DVD/HD-DVD Dual Format";
        public const string Movie_Hi_Definition_DVD = "Hi-Definition DVD";
        public const string Movie_VHS_Cassettes = "VHS Cassettes";
        public const string Movie_Playaway_View = "Playaway View";

        //music
        public const string Music_DVD_Audio = "DVD-Audio";
        public const string Music_Super_Audio_CD = "Super Audio CD";
        public const string Music_Super_Audio_CD_Hybrid = "Super Audio CD Hybrid";
        //vinyl
        public const string Music_Vinyl_12 = "Vinyl 12 Inch";
        public const string Music_Vinyl_7I = "Vinyl 7 Inch";
        public const string Music_Vinyl_LP = "Vinyl LP";

        //ebook
        public const string EBook_Digital = "eBook - Digital";
        public const string EBook_Digital_Download_Online = "eBook - Digital Download and Online";
        public const string EBook_Downloadable_Audio = "eBook - Downloadable Audio";
        public const string EBook_Digital_Download = "eBook - Digital Download";
        public const string EBook_Digital_Online = "eBook - Digital Online";

        //eAudio
        public const string EAudio_Downloadable_Audio = "eAudio - Downloadable Audio";
    }

    public class ProductCodeConstants
    {
        public const string MUSIC = "Music";
        public const string MOVIE = "Movie";
        public const string BOOK = "Book";
        public const string EBOOK = "eBook";
        public const string BD = "BD";
        public const string PL = "PL";
    }

    public sealed class ProductLineConstants
    {
        public const string Digit = "DIGIT";
        public const string DownloadableAudio = "DWNAUD";
        public const string Video = "VIDEO";
    }

    public sealed class ProductIconPathConstants
    {
        public const string MUSIC = "icon_format_cd.png";
        public const string MOVIE = "icon_format_dvd.png";
        public const string BOOK = "icon_format_book.jpg";
        public const string EBOOK = "icon_format_booklelectronic.png";
        public const string PRINT_ON_DEMAND = "icon_format_pod.png";
        public const string TEXT_STREAM_PRINT_ON_DEMAND = "icon_format_pod.png";
        public const string BD = "icon_format_bd.png";
        public const string PL = "icon_format_pl.jpg";
        public const string PA = "icon_format_pa.jpg";
        public const string EMPTY = "empty.png";
        public const string VINYL = "icon_format_vinyl.png";
        public const string ICON_4k = "icon-4k-format.png";
    }

    public sealed class ProductTypeConstants
    {
        public const string Book = "Book";
        public const string Digital = "Digital";
        public const string eBook = "eBook";
        public const string Movie = "Movie";
        public const string Music = "Music";
        public const string All = "All";
        public const string Entertainment = "Entertainment";

        public const int BookIndex = 0;
        public const int MovieIndex = 1;
        public const int MusicIndex = 2;
        public const int AllIndex = 3;
    }

    public class SearchResultsSortField
    {
        public const string SORT_QUANTITY = "quantity";
        public const string SORT_ADD_TO_CART_DATE = "cartorder";
        public const string SORT_TITLE = "title";
        public const string SORT_LISTPRICE = "listprice";
        public const string SORT_PUBDATE = "pubdate";
        public const string SORT_FORMAT = "productformat";
        public const string SORT_ISBN = "isbn";
        public const string SORT_AUTHOR = "responsiblepartyprimary";
        public const string SORT_LCC_CLASS_AUTHOR = "lcclassauthor";
        public const string SORT_DEWEY_AUTHOR = "deweyauthor";
        public const string SORT_LCC_CLASS_ARTIST = "lcclassartist";
        public const string SORT_ARTIST = "artist";
        public const string SORT_DEWEY_ARTIST = "deweyartist";
        public const string SORT_POPULARITY = "popularity";
        public const string SORT_PUBLISHER = "publisher";
        public const string SORT_DEMAND = "demand";
        public const string SORT_ESP_OVERALL_SCORE = "espoverallscore";
        public const string SORT_ESP_BISAC_SCORE = "espbisacscore";
        public const string SORT_BY_POPULARITY = "Popularity";
        public const string SORT_BY_CARTORDER = "CartOrder";
    }



    public class ProductSupportedHtmlTag
    {
        public const string AnnotationImage = "<span class=\"sprite icon-a\" title=\"ANNOTATIONS\"></span>";
        public const string ReviewImage = "<span class=\"sprite icon-r\" title=\"REVIEWS\"></span>";
        public const string TocImage = "<span class=\"sprite icon-t\" title=\"TABLE OF CONTENTS\"></span>";
        public const string NonReturnImage = "<span title=\"Non-Returnable\" class=\"ico-noreturn\"><img src=\"/_layouts/IMAGES/CSDefaultSite/assets/images/ts360-sprite.png?v=346\"></span>";
        //public const string ReturnImage = "<img alt=\"T\" src=\"/_layouts/IMAGES/CSDefaultSite/assets/images/text/Non-Returnable-Icon-20x20.png\" title=\"NON-RETURNABLE \"/>";
        public const string MImage = "<span class=\"sprite icon-m\" title=\"MUSIC & VIDEO DATA\"></span>";
        public const string ExcerptsImage = "<span class=\"sprite icon-e\" title=\"EXCERPTS\"></span>";
        public const string PPCImage = "<span class=\"sprite icon-ppc\" title=\"Pay Per Circulation\"></span>";
        public const string DiversityClassificationImage = "<span class=\"sprite icon-dei\" title=\"Diversity, Equity, and Inclusion\"></span>";

        public const string BeginImage = "<span class=\"sprite icon-content-indicator-begin\"></span>";
        public const string EndImage = "<span class=\"sprite icon-content-indicator-end\"></span>";
        public const string DupEndImage = "<span class=\"dupe-right\"></span></span>";
        public const string DupBeginImage = "<span class=\"dupe\"><span class=\"dupe-left\"></span>";
        public const string DupC = "<span class=\"dupe-c-middle\" title=\"Carts\"></span>";
        //public const string DupC = "<img alt=\"C\" src=\"/_layouts/IMAGES/CSDefaultSite/assets/images/text/dupe_cart.jpg\" title=\"Carts\" id=\"DupCimg\">";
        public const string PrimCart = "<img alt=\"P\" class=\"fl\" src=\"/_layouts/IMAGES/CSDefaultSite/assets/images/text/dupe_primary_cart.jpg\" title=\"Primary Cart Grid Data\" >";
        public const string PrimaryCartGridData = "<span class=\"sprite icon-primary-cart-grid-data fl\" title=\"Primary Cart Grid Data\"></span>";
        public const string DupO = "<span class=\"dupe-o-middle\" title=\"Orders\"></span>";
        //public const string DupO = "<img alt=\"O\" src=\"/_layouts/IMAGES/CSDefaultSite/assets/images/text/dupe_ordered.jpg\" title=\"Orders\" id=\"DupOimg\">";
        public const string DivCb = "<div class=\"cb\"></div>";
        public const string DupH = "<img alt=\"H\" src=\"/_layouts/IMAGES/CSDefaultSite/assets/images/text/dupe_holdings.jpg\" title=\"Holdings\">";
        public const string DupHOrg = "<img alt=\"H\" src=\"/_layouts/IMAGES/CSDefaultSite/assets/images/text/dupe_holdings.jpg\" title=\"Against Organization Holdings\" class=\"DupHimg\">";
        public const string DupHUser = "<img alt=\"H\" src=\"/_layouts/IMAGES/CSDefaultSite/assets/images/text/dupe_holdings.jpg\" title=\"Against My Holdings\" class=\"DupHimg\">";
    }
    public class ProductDetailSectionName
    {
        public const string PRODUCT_INFORMATION = "ProductInformation";
        public const string PRODUCT_PHYSICAL = "Physical";
        public const string PRODUCT_CLASSIFICATION = "Classification";
        public const string PRODUCT_BTSPECIFICDATA = "BTSpecificData";
        public const string PRODUCT_READINGCOUNT = "ReadingCount";
        public const string PRODUCT_ACCELERATEDREADER = "AcceleratedReader";
        public const string PRODUCT_ACADEMICMODIFIERS = "AcademicModifiers";
        public const string PRODUCT_OTHERCITATIONS = "OtherCitations";
        public const string PRODUCT_REVIEWCITATIONS = "ReviewCitations";
        public const string PRODUCT_BIBLIOGRAPHY = "Bibliography";
        public const string PRODUCT_BISACSUBJECTS = "BISACSubjects";
        public const string PRODUCT_ACADEMICSUBJECTS = "AcademicSubjects";
        public const string PRODUCT_AWARDS = "Awards";
        public const string PRODUCT_GENERALSUBJECTS = "GeneralSubjects";
        public const string PRODUCT_PAYPERCIRCCOLLECTION = "PayPerCircCollection";
        public const string PRODUCT_LIBRARYSUBJECTS = "LibrarySubjects";
        public const string PRODUCT_LOCATION = "Location";
        public const string PRODUCT_SERIES = "Series";
        public const string PRODUCT_ATTRIBUTES = "Attributes";
        public const string PRODUCT_BTPUBLICATIONS = "BTPublications";
        public const string PRODUCT_BTPROGRAMS = "BTPrograms";

    }
    public class QueryStringName
    {
        public static string SEARCH_VIEW_PARA_NAME = "sv";
        public static string SIZE_PARAM_NAME = "size";
        public static string PAGE_PARAM_NAME = "page";
        public static string SORT_PARAM_NAME = "sort";
        public static string PRODUCT_ID_PARAM_NAME = "pid";
        public static string SEARCH_BASIC_PARA_NAME = "basic";
        public static string SEARCH_SORT_ORDER = "direction";
        public static string SEARCH_SORT_BY = "sort";
        public static string SEARCH_PAGE_SIZE = "size";
        public static string CART_SEARCH_QUANTITY = "quantity";
        public static string CART_SEARCH_NOTE = "notes";
        public static string CART_SEARCH_FORMAT = "format";
        public static string CART_SEARCH_INVENTORY = "inventory";
        public static string CART_SEARCH_GRID = "grids";
        public static string CARTID = "cartId";
        public static string LINE_ITEM_ID = "lineItemId";
        public static string EDIT_SAVED_SEARCH = "editss";
        public const string ListValidationOptions = "ListValidationOptions";
        public const string IsFromCartDetails = "isfromcartdetails";
        public const string IsFromSearchResults = "isfromsearchresults";
        public const string CreateNewFolder = "CreateNewFolder";

        public const string WITH_NOTES = "WITH_NOTE";
        public const string WITHOUT_NOTES = "WITHOUT_NOTE";
        public const string WITH_QUANTITY = "WITH_QUANTITY";
        public const string WITHOUT_QUANTITY = "WITHOUT_QUANTITY";

        public const string Next30days = "Next30days";
        public const string Next60days = "Next60days";
        public const string Next90days = "Next90days";
        public const string Next180days = "Next180days";
        public const string Prev30days = "Prev30days";
        public const string Prev60days = "Prev60days";
        public const string Prev90days = "Prev90days";
        public const string Prev120days = "Prev120days";
        public const string Prev180days = "Prev180days";
        public const string Prev365days = "Prev365days";
        public const string Prev12months = "Prev12months";
        public const string Prev24months = "Prev24months";
        public const string Prev7days = "Prev7days";
        public const string Prev14days = "Prev14days";
        public const string Prev28days = "Prev28days";
        public const string Next7days = "Next7days";
        public const string Next14days = "Next14days";
        public const string Next28days = "Next28days";
        public const string IsFromReleaseCalendarProducts = "IsFromReleaseCalendarProducts";
        public const string IsFromEListProducts = "IsFromEListProducts";
        public const string IsFromPromotionProducts = "IsFromPromotionProducts";
        public const string IsFromPublicationProducts = "IsFromPublicationProducts";
        public const string SortBy = "SortBy";

        public const string CartId = "CartId";
        public const string UserId = "UserId";
        public const string OrganizationId = "OrganizationId";
        public const string OrgId = "orgId";
        public const string FolderId = "FolderId";
        public const string CartDetailTab = "cdt";

        public const string ViewAllTitles = "ViewAllTitles";
        public const string IsValidationCartGrids = "IsValidationCartGrids";
        public const string GridFieldID = "GridFieldID";
        public const string GridFieldType = "GridFieldType";
        public const string GridCodeID = "GridCodeID";
        public const string GridTemplateId = "gTemplateId";
        public const string ReplaceGridCodeID = "ReplaceGridCodeID";
        public const string IsFreeText = "IsFreeText";
        public const string TotalReplaced = "TotalReplaced";
        public const string UserControlName = "UserControlName";
        public const string UserControlCode = "UserControlCode";
        public const string ProxiedUserId = "proxieduserid";
        public const string SearchPageChangeView = "switchview";
        public const string RedirectToItemDetails = "redirectToItemDetails";

        public const string MyPreferenceTabType = "CT";
        public const string SearchHomeTabType = "SEARCHT";
        public const string Targeting = "targeting";
        public const string IsSecureFlag = "sss";
        public const string DirectByApplication = "dba";

        public const string KeywordType = "_kt";

        public const string IsMixedGridNonGrid = "IsMixedGridNonGrid";
        public const string IsMixedProduct = "IsMixedProduct";

        public const string ViewAllCarts = "_vac";
        public const string SelectedFolderId = "_fid";
        public const string ManageCartsSearchKeyWork = "_kw";
        public const string ManageCartsFilterMode = "_fm";
        public const string ManageCartsActionMessage = "__am";

        public const string NeedToLogout = "__ntl";
        public const string WebTrendAC = "wt.ac";
        public const string ResetAVCache = "rc";

        public const string ESPCodeManagementTabType = "ESPCodeManagementTT";
        public const string ShareGroupId = "sharegroupid";
        public const string FindReplaceQuantity = "FindReplaceQuantity";

        public const string ContentListName = "contentListName";
        public const string IS_BACK_TO_NRC = "IsBackToNRC";
        public const string BRANDING_ID = "brandingid";
    }

    public class SearchQueryStringName
    {
        public const string KEYWORD_REMOVE_SEARCH_TERM = "RemoveSearchTerm";
        public const string KEYWORD_REMOVE_MY_PREFERENCES_EXCLUDE = "RemoveMyPreferenceExclude";
        public const string KEYWORD_REMOVE_MY_PREFERENCES_EXPRESSION = "RemoveMyPreferenceExpression";
        public const string KEYWORD_REMOVE_MY_PREFERENCES_TERM = "RemoveMyPreferenceTerm";
        public const string KEYWORD_REMOVE_SEARCH_EXPRESION = "RemoveSearchExpression";
        public static string KEYWORD_ATTRIBUTE = "attribute";
        public const string KEYWORD_ARINTEREST = "arinterest";
        public const string KEYWORD_RCINTEREST = "rcinterest";
        public const string KEYWORD_ARREADINGLEVEL = "arreadinglevel";
        public static string KEYWORD_SESSION_SEARCHCONDITION = "QueryString";
        public static string KEYWORD_DATE_RANGE = "1000";
        public static string KEYWORD_SAVEDSEARCH_NAME = "savedsearchname";
        public static string KEYWORD_SEARCH_ADVANCE = "av";
        public static string KEYWORD_SEARCH_PREFERENCES = "pref";
        public static string KEYWORD_FROM = "from";
        public static char KEYWORD_BTKEYSEPARATOR = '|';
        public const char Start_Seperator = '#';
        public const string QUOT_REPLACE = "aaaa";
        public const string BTKey = "ngbtkey";
        public static string KEYWORD_SEARCH_BATCH_ENTRY = "isfrombatchentry";
    }

    public class GlobalConfigurationKey
    {
        public const string FastUrl = "FAST_Url";
        public const string Sessiontimeout = "SessionTimeOut";
        public const string Enabledsingleactiveusersession = "EnabledSingleActiveUserSession";
        public const string RememberMeDuration = "Remember_Me_Duration";
        public const string Auto_Suggest_Collection = "Auto_Suggest_Collection";
        public const string Search_View_Item = "Search_View_Item";
        public const string CmCachingDuration = "CM_Caching_Duration";
        public const string RealtimeWsSysid = "RealTime_WS_SysID";
        public const string RealtimeWsSyspass = "RealTime_WS_SysPass";
        public const string CsprofileConnectionstring = "CSProfile_ConnectionString";
        public const string NgConnectionstring = "NG_ConnectionString";
        public const string ProductcatalogConnectionstring = "ProductCatalog_ConnectionString";
        public const string OrdersConnectionstring = "Orders_ConnectionString";
        public const string CsproductcatalogConnectionstring = "CSProductCatalog_ConnectionString";
        public const string NgFarmactiveusersessionConnectionstring = "NG_FarmActiveUserSession_ConnectionString";
        public const string ProfilesConnectionstring = "Profiles_ConnectionString";
        public const string CcCloudImageConnectionstring = "ContenCafeCloudImage_ConnectionString";
        public const string CcCloudImageNextgenSystemid = "ContenCafeCloudImage_NextGen_SystemId";
        public const string MAX_LINES_PER_CART_TITLE = "Max_Lines_Per_Cart";
        public const string SearchExecutorUrlSetting = "FAST_Url";
        public const string DebugTrace = "DebugTrace";
        public const string CsMarketingUserid = "CS_Marketing_UserId";
        public const string StockcheckserviceUrl = "StockCheckService_URL";
        public const string CcmuzeUrl = "CCMuze_URL";
        public const string LogsConnectionstring = "Logs_ConnectionString";
        public const string Createretailuseremailsubject = "CreateRetailUserEmailSubject";
        public const string Createlibraryuseremailsubject = "CreateLibraryUserEmailSubject";
        public const string Cybersourceurl = "CyberSourceUrl";
        public const string CPSIA_WARNING_BASE_URL = "CPSIA_Warning_Base_URL";
        public const string CPSIA_WARNING_USERID = "CPSIA_Warning_User_ID";
        public const string CUSTOMER_SERVICE_LINK = "Customer_Service_Link";
        public const string CHECK_USER_ID_HEX_VALUE = "CheckUserIdHexValue";
        public const string ShowListcarouselbracket = "Show_ListCarouselBracket";
        public const string ShowJacketCarouselBracket = "Show_JacketCarouselBracket";
        public const string Publicationsubscriptionlink = "PublicationSubscriptionLink";
        public const string Commercesitename = "CommerceSiteName";
        public const string Suggestion_Items_Limit = "Suggestion_Items_Limit";
        public const string Suggestion_Delay_Time = "Suggestion_Delay_Time";
        public const string SubmitorderRetry = "SubmitOrder_Retry";
        //public const string DemandBucketCachingMinutesKey = "DemandBucketCachingMinutes";
        public const string BasicMARCAverageDownloadSpeed = "BasicMARCAverageDownloadSpeed";
        public const string MARCWebServiceUrl = "MARCWebServiceURL";
        public const string MARCBizTalkWebServiceUrl = "MARCBizTalkWebServiceUrl";
        public const string CartCacheExpireDuration = "Cart_Cache_Expire_Duration";
        public const string PricingBatchWaitingTime = "Pricing_Batch_WaitingTime";
        public const string PromotionServiceUrl = "PromotionService_Url";
        public const string CartActionsServiceUrl = "CartActionsService_Url";
        public const string PricingTimerInterval = "PricingTimerInterval";
        public const string WcfServiceAddress = "WcfServiceAddress";
        public const string MaxLineItemsForPrint = "MaxLineItemsForPrint";
        public const string RegisterUrl = "RegisterURL";
        public const string OnlineBillPaymentUrl = "OnlineBillPayURL";
        public const string OnlineBillPaymentAccountID = "OnlineBillPayAccountID";
        public const string OnlineBillPaymentEncryptionKey = "OnlineEncryptionKey";
        public const string ForgotPasswordEncryptionKey = "ForgotPasswordEncryptionKey";
        public const string ForgotPasswordEncryptionVector = "ForgotPasswordEncryptionVector";
        public const string ForgotPasswordExpirationPeriod = "ForgotPasswordExpirationPeriod";
        public const string MARCCartLineThreshold = "MARCCartLineThreshold";
        public const string MaxLineLimitForSetQuantities = "MaxLineLimitForSetQuantities";
        public const string catURL = "CAT_URL";
        public const string BTAlertCacheExpireDuration = "BTAlert_Cache_Expire_Duration";
        public const string SlipReportStorageSiteUrl = "SlipReportStorageSiteUrl";
        public const string SlipReportStorageSPDocLibraryName = "SlipReportStorageSPDocLibraryName";
        public const string SlipReportSiteURLUserName = "SlipReportSiteURLUserName";
        public const string SlipReportSiteURLPassword = "SlipReportSiteURLPassword";
        public const string SlipReportSiteURLDomain = "SlipReportSiteURLDomain";
        public const string SlipUserAlertServiceURL = "SlipUserAlertServiceURL";
        public const string SlipReportUploadLimit = "SlipReportUploadLimit";
        public const string SlipReportStorageSiteUrlForAlert = "SlipReportStorageSiteUrlForAlert";
        public const string SlipReportLayoutFolderPath = "SlipReportLayoutFolderPath";
        public const string LawsonExportLayoutFolderPath = "LawsonExportLayoutFolderPath";
        public const string SingleLineCartThreshold = "SingleLineCartThreshold";
        public const string QuotationCartLineThreshold = "QuotationCartLineThreshold";
        public const string QuotationDocLibraryName = "QuotationDocLibraryName";
        public const string SingleLineCartDocLibraryName = "SingleLineCartDocLibraryName";
        public const string QuotationReportLayoutFolderPath = "QuotationReportLayoutFolderPath";
        public const string SingleLineCartReportLayoutFolderPath = "SingleLineCartReportLayoutFolderPath";
        public const string ReportServerURL = "SlipReportServerUrl";
        public const string StockCheckDefaultSOPAccountID = "StockCheckDefaultSOPAccountID";
        public const string StockCheckDefaultOneBoxSOPAccountID = "StockCheckDefaultOneBoxSOPAccountID";
        public const string ApplyDuplicateThresholdLimit = "ApplyDuplicateThresholdLimit";
        public const string CartActionsThresholdLimit = "CartActionsThresholdLimit";
        public const string ReturnTemplateFolderPath = "ReturnTemplateFolderPath";
        public const string BatchEntryItemsThreshold = "BatchEntryItemsThreshold";
        public const string BatchEntryFileSizeThreshold = "BatchEntryFileSizeThreshold";
        public const string BatchEntryLayoutFolderPath = "BatchEntryLayoutFolderPath";
        public const string BatchEntrySharePointLibrary = "BatchEntrySharePointLibrary";

        public const string CyberSourceAccessKey = "CyberSourceAccessKey";
        public const string CyberSourceProfileID = "CyberSourceProfileID";
        public const string CyberSourceSecretKey = "CyberSourceSecretKey";

        public const string PoLineNumberRegExp = "PoLineNumberRegExp";


        public const string EmailReturn = "EmailReturn";
        public const string EmailLeasing = "EmailLeasing";
        public const string EmailCustomerService = "EmailCustomerService";

        public const string DistributedCacheName = "DistributedCacheName";
        public const string Ts360SystemNotification = "SystemNotificationCacheKey";
        public const string DistributedCache_Env_Region = "DistributedCache_Env_Region";
        public const string DistributedCacheAdminUsers = "DistributedCacheAdminUsers";
        public const string DistributedCacheDuration = "FarmCacheDuration";

        public const string TS360WebAPIURL = "TS360WebAPIURL";
        public const string TS360WebAPISalesForceAPIKey = "TS360WebAPISalesForceAPIKey";

        public const string SuperWarehouseInventoryThreshold = "SuperWarehouseInventoryThreshold";

        public const string ESPPortalURL = "ESPPortal_URL";
        public const string ESPWebServiceURL = "ESPWebServiceURL";
        public const string ESPAPIKey = "ESPAPIKey";
        public const string DataFixSendToMail = "DataFixSendToEmail";
        public const string MaxCartsPerMergeRequest = "MaxCartsPerMergeRequest";
        public const string OCSIdentifier = "OCSIdentifier";
        public const string MergeCartThreshold = "MergeCartThreshold";
        public const string HelpTrainingLink = "HelpTrainingLink";
        public const string UserProfileDurationCache = "UserProfileDurationCache";

        public const string CcCloudImage_Size_Small = "ContenCafeCloudImage_Size_Small";
        public const string CcCloudImage_Size_Medium = "ContenCafeCloudImage_Size_Medium";
        public const string CcCloudImage_Size_Large = "ContenCafeCloudImage_Size_Large";

        public const string TitleSourceSiteUrl = "TitleSourceSiteUrl";

        public const string ILSBaseAddress = "ILSBaseAddress";
        public const string ILSOrderValidatePath = "ILSOrderValidatePath";
        public const string ILSSubmitOrderPath = "ILSSubmitOrderPath";
        public const string ILSAuthorizePath = "ILSAuthorizePath";
        public const string ILSTokenPath = "ILSTokenPath";
        public const string ILSGetLogApiUrl = "ILSGetLogApiUrl";
        public const string ILSInsertLogApiUrl = "ILSInsertLogApiUrl";
        public const string ILSVendor = "ILSVendor";

        public const string PAPIURL = "PAPIURL";
        public const string PAPIID = "PAPIID";
        public const string PAPIAccesskey = "PAPIAccesskey";
        public const string PAPIDomain = "PAPIDomain";
        public const string PAPIAccount = "PAPIAccount";
        public const string PAPIPassword = "PAPIPassword";

    }


    public class QueryStringValue
    {
        // Define the View Value for Search Results
        public const string TABLE_VIEW = "table";
        public const string STACKED_VIEW = "stack";
        public const string TILE_VIEW = "tile";
        public const string SORT_ASC_TEXT = "Ascending";
        public const string SORT_DESC_TEXT = "Descending";
        public const char NAVIGATOR_SEPERATOR = '/';
        public const string AND_VALUE_SEPERATOR = "||";
        public const string OR_VALUE_SEPERATOR = "|";

        public const int CartSummaryTabIndex = 0;
        public const int AccountSummaryTabIndex = 1;
        public const int ShareSummaryTabIndex = 2;
        public const int GridSummaryTabIndex = 3;
        public const int ShareProfileTabIndex = 4;
        public const int NotesTabIndex = 5;
        public const int OrderStatusTabIndex = 6;
    }

    public class SessionVariableName
    {
        public const string AdditionalProductInfo = "__AdditionalProductInfo";
        public const string SearchResult = "__SearchResult";
        public const string SEARCH_RESULT_LIST_ID = "SearchResultListProductId";
        public const string SEARCH_RESULT_LIST_QTY = "SearchResultListProductQtyInCart";
        public const string RefererQueryString = "RefererQueryString";
        public const string RefererPageAbsolutePath = "RefererPageAbsolutePath";
        public const string SearchResultsTotalCount = "SearchResultsTotalCount";
        public const string SearchResultsPageIndex = "SearchResultsPageIndex";
        public const string SearchResultsPageSize = "SearchResultsPageSize";
        public const string SearchResultsSortBy = "SearchResultsSortBy";
        public const string SearchResultsSortDirection = "SearchResultsSortDirection";
        public const string SearchResultsSearchArguments = "SearchResultsSearchArguments";
        public const string SearchResultsDictionayProductId = "SearchResultsDictionayProductId";
        public const string OriginalAudienceType = "OriginalAudienceType";
        public const string OriginalProductType = "OriginalProductType";
        public const string ItemDetailsParentNodeUrl = "ItemDetailsParentNodeUrl";
        public const string ItemDetailsParentNodePath = "ItemDetailsParentNodePath";
        public const string SearchTerms = "SEARCH_TERMS";
        public const string SearchTermsObj = "SEARCH_TERMS_Obj";
        public const string AdvancedSearchFilters = "AdvancedSearchFilters";
        public const string ViewAllCarts = "ViewAllCarts";
        public const string OrganizationDetailsSelectedIndex = "OrganizationDetailsSelectedIndex";
        public const string JustCreatedOrganization = "JustCreatedOrganization";
        public const string DupsDictionary = "DupsDictionary";
        public const string PromoCodeFlag = "PromoCodeFlag";
        public const string ReleaseMonth = "ReleaseMonth";
        public const string ReleaseYear = "ReleaseYear";
        public const string ProductType = "ProductType";
        public const string CurrentSlectedTabIndex = "CurrentSlectedTabIndex";
        public const string DictionaryBtKeyIsOriginalEntry = "DictionaryBTKeyIsOriginalEntry";
        public const string DictionaryBtKeyLineItemId = "DictionaryBtKeyLineItemId";
        public const string RecordIndexSourcePageName = "RecordIndexSourcePageName";
        public const string UserGridTemplateCacheKey = "UserGridTemplateCacheKey";
        public const string BTGridTemplateCacheKey = "BTGridTemplateCacheKey";
        public const string BatchEntryGridLinesSessionKey = "BatchEntryGridLinesSessionKey";
        public const string BatchEntryGridLinesSessionKeyForServer = "BatchEntryGridLinesSessionKeyForServer";
        public const string BatchEntryNoteSessionKeyForServer = "BatchEntryNoteSessionKeyForServer";
        public const string BatchEntryIsGridEnabledSessionKeyForServer = "BatchEntryIsGridEnabledSessionKeyForServer";
        public const string BatchEntryViewInSearchResult = "_BatchEntryViewInSearchResult_{0}";

        public const string NumberItemWithQuantity = "NumberItemWithQuantity";
        public const string NumberItemWithoutQuantity = "NumberItemWithoutQuantity";
        public const string NumberItemWithNote = "NumberItemWithNote";
        public const string NumberItemWithoutNote = "NumberItemWithoutNote";

        public const string DictionaryLineItemListId = "DictionaryLineItemListId";
        public const string CartDetailsContext = "CartDetailsContext";
        public const string QuickCartDetailsContext = "QuickCartDetailsContext";
        public const string CartDrawerData = "__{0}_cartDrawerData";
        public const string AdditionalLineItemInfo = "__AdditionalLineItemInfo";
        public const string ESupplierAccountListCacheKey = "__ESupplierAccountListCacheKey";
        public const string ESupplierAccountListCacheKeyForNewCart = "__ESupplierAccountListCacheKeyForNewCart";
        public const string ESupplierDefaultAccountListCacheKey = "__ESupplierDefaultAccountListCacheKey";
        public const string CartDetailsLineItemsCacheKey = "__CartDetailsLineItemsCacheKey_{0}";
        public const string CartDetailsCartCacheKey = "__CartDetailsCartCacheKey";
        public const string CartDetailsGetPrimaryCartForUser = "__CartDetailsGetPrimaryBasketForUser";
        public const string BtUserIdProxy = "BtUserIdProxy";
        public const string ProxiedUserId = "ProxiedUserId";
        public const string ProxiedUserName = "ProxiedUserName";
        public const string IsProxyActive = "IsProxyActive";
        public const string MassQuantity = "MassQuantity";
        public const string MassQuantityCartIDs = "MassQuantityCartIDs";
        public const string MassQuantityLineItemIDs = "MassQuantityLineItemIDs";
        public const string ItemDetailsLinkAfterDeletedATitle = "ItemDetailsLinkAfterDeletedATitle";
        public const string CmBackgroundInfo = "__CmBackgroundInfo";
        public const string GridBackgroundInfo = "__GridBackgroundInfo";
        public const string GridBackgroundSessionObject = "__GridBackgroundSessionObject";
        public const string DictionaryLineItemListQuantity = "DictionaryLineItemListId";
        public const string CurrentItemIndexSessionStringName = "CurrentItemIndex";

        public const string ProductDetails = "__ProductDetailsCacheKey";
        public const string InventoryStatusForCart = "__InventoryStatusForCart{0}";
        public const string InventoryStatusForSearch = "__InventoryStatusForSearch";
        public const string FeaturedPromtionBtKeyListCacheKey = "__FeaturePromtionBtKeyListCacheKey_{0}";
        public const string RemainingPublicationUnreadItemsCacheKey = "__RemainingPublicationUnreadItemsCacheKey";
        public const string PasteClipBoardSuccessMessageCacheKey = "__PasteClipBoardSuccessMessageCacheKey";
        public const string ManageCartPageSize = "ManageCartPageSize";
        public const string ManageCartSortBy = "ManageCartSortBy";
        public const string ManageCartSortDirection = "ManageCartSortDirection";
        public const string RefineSearchFlag = "RefineSearchFlag";

        public const string ShareGroupSavedSuccess = "ShareGroupSavedSuccess";
        public const string ShareGroupsDeletedSuccess = "ShareGroupsDeletedSuccess";

        public const string CartDetailsLineItemsIneligibleNonVIPItemsCacheKey = "__CartDetailsLineItemsIneligibleNonVIPItemCacheKey_{0}";
        public const string MergeCartList = "__MergeCartList_{0}";
        public const string SelectedTargetCart = "__SelectedTargetCart_{0}";
        public const string NewCreatedTargetCart = "__NewCreatedTargetCart_{0}";
        public const string CartIdProcessingToBeDeleted = "__CartIdProcessingToBeDeleted_{0}";

        public const string UserAccountsCachePrefix = "_userAccountLists_";
        public const string UserDefaultAccountCachePrefix = "_userDefaultAccounts_";
        public const string UserESupplierAccountCachePrefix = "_userESupplierAccounts_{0}";

        public const string SearchFacet = "__SearchFacet";
        public const string AdvancedSearchSession = "AdvancedSearchSession";
        public const string AdvancedSearchData = "AdvancedSearchData";
        public const string BookSearchFilter = "BookSearchFilter";
        public const string MusicSearchFilter = "MusicSearchFilter";
        public const string MovieSearchFilter = "MovieSearchFilter";
        public const string SavedSearchName = "SavedSearchName";
        public const string IsFromSearchResult = "IsFromSearchResult";
        //public const string IsUsingMyPreferencesValues = "IsUsingMyPreferencesValues";
        public const string SiteMapFacet = "SiteMapFacet";
        public const string ControlMoveRemoveRowID = "ControlMoveRemoveRowID";
    }

    public sealed class CacheKeyConstant
    {
        public const string CART_MANAGEMENT_CACHE_KEY_PREFIX = "CART_MANAGEMENT_CACHE_KEY";
        public const string GRIDFIELD_CACHE_FORCURRENTUSER = "GRIDFIELD_CACHE_FORCURRENTUSER";
        public const string TOP_NEWEST_CART_CACHE_KEY_SUFFIX = "TOP_NEWEST_CART";
        public const string USERGRIDFIELD_CACHE_CURRENTUSER = "USERGRIDFIELD_CACHE_CURRENTUSER";
        public const string GRID_USER_PREFERENCE_CACHEKEY = "GRID_USER_PREFERENCE_CACHEKEY";
        public const string GRIDTEMPLATE_USERPREFERENCE_DEFAULT = "GRIDTEMPLATE_USERPREFERENCE_DEFAULT";
        //public const string ORGANIZATION_DETAIL_GRIDTEMPLATE = "ORGANIZATION_DETAIL_GRIDTEMPLATE";
        public const string ORGANIZATION_DETAIL_GRIDTEMPLATE_POPUP = "ORGANIZATION_DETAIL_GRIDTEMPLATE_POPUP";
        public const string SHARE_CART_PROFILE_CACHE_KEY_PREFIX = "SHARE_CART_PROFILE_CACHE_KEY_PREFIX";
        public const string CART_GET_ORIGINALCART = "CART_GET_ORIGINALCART";

        // For Cache Primary Cart in Cart Details
        // We will change this approach later, this way isnot good
        public const string IS_CART_DETAILS_PAGE = "IS_CART_DETAILS_PAGE";
        public const string IS_CART_DETAILS_LAUNCH_MINI_CART = "IS_CART_DETAILS_LAUNCH_MINI_CART";
        public const string IS_CART_DETAILS_LAUNCH_ADDITIONAL_PRODUCT = "IS_CART_DETAILS_LAUNCH_ADDITIONAL_PRODUCT";
        public const string IS_CART_DETAILS_LAUNCH_CART_FILTER_DATA = "IS_CART_DETAILS_LAUNCH_CART_FILTER_DATA";
        public const string IS_CART_DETAILS_LAUNCH_UPPDATE_ALL_ITEMS = "IS_CART_DETAILS_LAUNCH_UPPDATE_ALL_ITEMS";
        public const string CART_DETAILS_PAGE_PRIMARY_CART = "CART_DETAILS_PAGE_PRIMARY_CART";
        public const string IS_MINI_CART_FIRST_TIME = "IS_MINI_CART_FIRST_TIME";
        public const string IS_ADDITIONAL_PRODUCT_FIRST_TIME = "IS_ADDITIONAL_PRODUCT_FIRST_TIME";
        public const string IS_CART_FILTER_DATA_FIRST_TIME = "IS_CART_FILTER_DATA_FIRST_TIME";
        public const string IS_UPPDATE_ALL_ITEMS_FIRST_TIME = "IS_UPPDATE_ALL_ITEMS_FIRST_TIME";
        //public const string PRIMARY_CART_CACHE_KEY_SUFFIX = "PRIMARY_CART";

        public const string USER_ALERT_MESSAGE_CACHE_KEY_PREFIX = "USER_ALERT_MESSAGE_CACHE_KEY_PREFIX";
        public const string SYSTEM_NOTIFICATION_MESSAGE_CACHE_KEY_PREFIX = "SYSTEM_NOTIFICATION_MESSAGE_CACHE_KEY_PREFIX";

        public const string NewReleasesCalendarSessionKeyForListBtKeys = "__NewReleasesCalendarSessionKeyForListBTKeys";
        public const string NewReleasesCalendarSessionKeyForFastProduct = "__NewReleasesCalendarSessionKeyFastProduct";

        public const string ManageCartsPageChanged = "__ManageCartsPageIndexChanged";
        public const string ManageCartsPageQueryString = "__ManageCartsPage_PageQueryString{0}";
        public const string ManageCartsSelectedFolderId = "__ManageCartsPage_SelectedFolderId{0}";
        public const string ManageCartsPageKeyword = "__Keyword{0}";
        public const string ManageCartsActionMessage = "__ActionMessage{0}";
        public const string ManageCartsPageKeywordType = "__KeywordType{0}";
        public const string GridCodesPageGridFieldList = "__GridFieldList{0}";

        public const string ViewCartAdminPageQueryString = "__ViewCartAdminPage_PageQueryString{0}";
        public const string IsBTEmployee = "__IsBTEmployee{0}";
        public const string CartSortBy = "__CartSortBy{0}";
        public const string CartSortOrder = "__CartSortOrder{0}";
        public const string WarningDeleteLineItem = "__WarningDeleteLineItem{0}";
        public const string CartFormat = "__CartFormat{0}";

        public const string ESPPremiumService_CACHE_KEY_PREFIX = "__ESPPremiumServices{0}";
        public const string GetGridSummaryFilterByCartId = "__GetGridSummaryFilter_CartId{0}";
        public const string GridSummarySelectedFieldsByCartId = "__GridSummarySelectedFields_CartId{0}";
        public const string GridSummaryUserPreferences = "__GridSummaryUserPreferences";
        public const string TwilightProductsForInventory = "__TwilightProductsForInventory";
        public const string ComingSoonProductsForInventory = "__ComingSoonProductsForInventory";
        public const string OrganizationPremiumService_CACHE_KEY_PREFIX = "__OrgPremiumServices{0}";
        public const string PPCSubscriptions_CACHE_KEY_PREFIX = "PPCSubscription";
        public const string OrganizationPPCSubscriptions_CACHE_KEY_PREFIX = "PPCSubscription_{0}";

        public const string InventoryCacheKey = "__InventoryCacheKey";

        public const string CartDetailsPageSearchItemsCacheKey = "__CartDetailsPageSearchItems{0}"; // {0}: Cart ID
        public const string CartDetailsPageLineItemsCacheKey = "__CartDetailsPageLineItems{0}"; // {0}: Cart ID
        public const string CartDetailsPageShowEnhancedContentCartCacheKey = "___CartDetailsPageShowEnhancedContentCacheKey_{0}"; // {0}: Cart ID
        public const string OrganizationAccouts = "__OrganizationAccouts{0}"; // {0}: ORG ID

        //public const string OcsTokenCacheKey = "__OcsTokenCacheKey{0}"; // {0}: User ID
        public const string SeqGridOptionCacheKey = "__SeqGridOptionCacheKey{0}"; // {0}: ORG ID

        public const string WhsInfoUserIdCacheKey = "__WhsInfoSessionCacheKey_UserId_{0}";
        public const string OrgHasOneBoxAccount = "__OrgHasOneBoxAccount_OrgId_{0}"; // {0}: ORG ID
        public const string NRC_CONTEXT_CACHE_KEY = "NEW_RELEASE_CALENDAR_CONTEXT_CACHE_KEY";
        public const string NRC_CACHE_KEY = "NRC_{0}{1}_{2}";
        public const string PHYSICAL_DIGITAL_SELECTED_FOLDER_CACHE_KEY = "PHYSICAL_TO_DIGITAL_CONVERSION_SELECTED_FOLDER_CACHE_KEY";

        public const string ILSUSerSelectedMARC = "__ILSUSerSelectedMARC_{0}";
        public const string DASHBOARD_ORDER_STATUS_DATE_RANGE = "__DashboardOrderStatusDateRange_{0}";
        public const string DASHBOARD_ORDER_HISTORY_DATE_RANGE = "__DashboardOrderHistoryDateRange_{0}";
    }

    public class SearchFieldValue
    {
        public const string PawPrintsPublisherName = "Paw Prints";
        public const string SupplierCodePPBTB = "PPBTB";
        public const string SupplierCodePPBTM = "PPBTM";
        public const string PawPrintsProductLine = "PAWPR";
        public const string GardnerSupplierCode = "GRDRS";
        public const string SupplierCodePPBTR = "PPBTR";
        public const string SupplierCodePPBTC = "PPBTC";
    }


    public class DBStores
    {
        public static string CONST_GET_ORGANIZATION_EXTENTION = @"GetOrganizationsExtention";
        public static string ConstGetAllOrganizations = "procGetAllOrganizations";
        public static string CONST_GET_ACCOUNT_EXTENTION_CONTAINS = @"GetAccountsExtention";
        public static string CONST_GET_ACCOUNT_EXTENTION_BEGIN_WITH = @"GetAccountsExtentionBeginWith";

        public static string CONST_GET_ORGANIZATION_NAME = @"GetOrganizationName";
        public static string CONST_GET_USER_EXTENTION = @"GetUsersExtention";
        public static string CONST_GET_USER_BY_SEARCH = @"GetUsersBySearch";
        public static string CONST_GET_USERID_BY_ORG_ID = @"GetUsersIDByOrganization";
        public static string CONST_GET_USER_DETAILS_BY_ID = @"procTS360GetUsersDetailByID";

        public static string CONST_ACC_GET_ACCOUNTS = @"GetAccounts";
        public static string CONST_USER_GET_USERS = @"GetUsers";
        public static string CONST_ORG_GET_ORGANIZATION = @"GetOrganizationsEx";
        public static string CONST_SUGGEST_USERS_COPIED = @"SuggestUsersCopied";
        public static string CONST_GET_UNASSIGNED_ACCOUNTS = @"GetUnassignedAccounts";
        public static string CONST_GET_ASSOCIATED_SHIPPING_ACCOUNTS = @"GetAssociatedShippingAccounts";
        public static string CONST_ASSIGN_ACCOUNTS_TO_ORGANIZATION = @"AssignAccountToOrganization";

        public static string CONST_UPDATE_BASKET = @"BTNG_UpdateBasket";
        public static string CONST_UPDATE_BASKET_PROPERTY = @"BTNG_UpdateBasketProperty";
        public static string CONST_INSERT_BASKET = @"BTNG_InsertBasket";
        public static string CONST_GET_USER_BASKETS = @"BTNG_GetBaskets";
        public static string CONST_SEARCH_USER_BASKETS = @"BTNG_SearchUserBaskets";
        public static string CONST_SEARCH_ORGANIZATION_BASKETS = @"BTNG_SearchOrganizationBaskets";
        public static string CONST_GET_USER_PRIMARY_BASKET = @"BTNG_GetPrimaryBasket";
        public static string CONST_CHECK_UNIQUE_BASKET_NAME = @"BTNG_CheckUniqueName";
        //public static string CONST_GET_FAMILY_KEY = @"procTS360GetFamilyKeys";
        public static string CONST_CHECK_FAMILY_KEY = @"procTS360CheckFamilyKeys";
        public static string CONST_GET_ORGANIZATION_BASKET = @"BTNG_GetOrganizationBaskets";
        public static string CONST_GET_LINEITEMS_IN_BASKET = @"BTNG_GetLineItemsInBasket";
        public static string CONST_GET_ORDERED_LINEITEMS = @"BTNG_GetOrderedLineItems";
        public static string CONST_GET_BASKET_BY_ID = @"BTNG_GetBasketById";
        public static string CONST_DELETE_ALL_LINEITEMS_IN_BASKET = @"BTNG_DeleteAllLineItemsInBasket";
        public static string CONST_DELETE_BASKET_BY_ID = @"BTNG_DeleteBasketById";
        public static string CONST_GET_BASKET_BY_NAME_AND_USER_ID = @"BTNG_GetBasketByNameAndUserId";
        public static string CONST_UPDATE_LINEITEM = @"BTNG_UpdateLineItem";
        public static string CONST_INSERT_LINEITEM = @"BTNG_InsertLineItem";
        public static string CONST_UPDATE_LINEITEM_PROPERTY = @"BTNG_UpdateLineItemProperty";
        public static string CONST_DELETE_LINEITEM_IN_BASKET = @"BTNG_DeleteLineItemsInBasket";
        public static string GET_ADVANCE_SEARCH_FILTER = @"GetAdvanceSearchFilter";

        public static string CONST_INSERT_SUBMITTED_LINEITEM = @"BTNG_InsertSubmittedLineItem";
        public static string CONST_INSERT_SUBMITTED_BASKET = @"BTNG_InsertSubmittedBasket";
        public static string CONST_INSERT_BASKET_ORDER_FORM = @"BTNG_InsertBasketOrderForm";

        public const string CheckProductContent = "procTS360CheckProductContent";
        public const string GetProductContent = "sp_GetProductContent";
        public const string DisableSystemNotificationMessage = "BT_DisableSystemNotificationMessage";
        public const string GetSystemNotificationMessageList = "BT_GetSystemNotificationMessageList";
        public const string CheckProductReviews = "procTS360CheckProductReviews";
        public const string GetProductReviews = "procTS360GetReviews";
        public const string GetProductAnnos = "procTS360GetAnnotations";
        public const string GetProductExcerpts = "procTS360GetExcerpt";
        public const string GetProductFlapCopy = "procTS360GetFlapCopies";
        public const string GetProductBiographies = "procTS360GetBiographies";
        public const string GetEdition = "procCSGetEdition";
        public const string GetPriceKeys = "procTS360GetPriceKeys";
        public const string GetReportCodes = "procTS360GetReportCodes";
        public const string GetProductInformation = "procTS360GetProductDetail";

        public const string GetValidIsbnUpc = "procTS360GetValidISBN_UPC";

        public const string GetInventory = "procTS360GetInventory";

        public const string CheckFutureOnSaleDate = "BTNG_CheckFutureOnSaleDate";
        public const string IsUsingAsDefaultAccount = "sp_IsUsingAsDefaultAccount";

        public const string ProcTs360GetBtkeysByIsbnUpc = "procTS360GetBtkeysByISBN_UPC";

        public const string GetBasketUserGroupsForOrganization = "procTS360GetBasketUserGroupsForOrganization";
        public const string GetBasketUserGroups = "procTS360GetBasketUserGroups";
        public const string GetBasketUserGroupMembers = "procTS360GetBasketUserGroupMembers";
        public const string SetBasketUserGroup = "procTS360SetBasketUserGroup";
        public const string DeleteBasketUserGroups = "procTS360DeleteBasketUserGroups";
        public const string IsKeepTransferedCart = "procTS360GetUserObjectKeepTransferedCart";

        // Basket Folder's store procedure
        public const string procIsFolderAndSubFolderContainsPrimaryCart = "procIsFolderAndSubFolderContainsPrimaryCart";
        public const string procGetRootFolderContainsPrimaryCart = "procGetRootFolderContainsPrimaryCart";
        public const string procGetPrimaryFolder = "procGetPrimaryFolder";
        public const string procGetLastOrderingNumber = "procGetLastOrderingNumber";
        public const string procIsFolderTypeExist = "procIsFolderTypeExist";
        public const string procIsUniqueFolderName = "procIsUniqueFolderName";
        public const string procDeleteFolder = "procDeleteFolder";
        public const string procDeleteAllFolders = "procDeleteAllFolders";
        public const string procChangeFolderType = "procChangeFolderType";
        public const string procChangeParentFolder = "procChangeParentFolder";
        public const string procCreateBasketFolder = "procCreateBasketFolder";
        public const string procGetBasketFolderByFolderType = "procGetBasketFolderByFolderType";
        public const string procGetBasketFolderById = "procGetBasketFolderById";
        public const string procGetBasketFolderByName = "procGetBasketFolderByName";
        public const string procGetBasketFoldersByParentId = "procGetBasketFoldersByParentId";
        public const string procGetBasketsByUserId = "procGetBasketsByUserId";
        public const string procRenameBasketFolder = "procRenameBasketFolder";
        public const string procResequenceFolder = "procResequenceFolder";


        //MARC Profiler store procedures
        public const string procMARCDeleteMarcProfile = "procMARCDeleteMarcProfile";
        //public const string procMARCSetProfileList = "procMARCSetProfileList";
        public const string procMARCCopyProfile = "procMARCCopyProfile";
        public const string procMARCGetProfiles = "procMARCGetProfiles";
        public const string procMARCGetGeneralInfo = "procMARCGetGeneralInfo";
        public const string procMARCSetGeneralInfo = "procMARCSetGeneralInfo";
        public const string procMARCCheckDuplicateProfileName = "procMARCCheckDuplicateProfileName";
        public const string procMARCSetTagRule = "procMARCSetTagRule";
        public const string procMARCGetTagRules = "procMARCGetTagRules";
        public const string procMARCDeleteTagRule = "procMARCDeleteTagRule";
        public const string procMARCGetProfileTagRule = "procMARCGetProfileTagRule";
        public const string procMARCUpdateProfileTagRule = "procMARCUpdateProfileTagRule";
        public const string procMARCGetRuleType = "procMARCGetRuleType";
        public const string procMARCGetAddType = "procMARCGetAddType";
        public const string procMARCGetBibliographicConditionOperator = "procMARCGetBibliographicConditionOperator";
        public const string procMARCSetSpecialCodes = "procMARCSetSpecialCodes ";
        public const string procMARCGetSpecialCodes = "procMARCGetSpecialCodes";
        public const string procMARCGetSpecialCodeLines = "procMARCGetSpecialCodeLines";
        public const string procMARCDeleteSpecialCode = "procMARCDeleteSpecialCode";
        public const string procMARCGetTS360Fields = "procMARCGetTS360Fields";
        public const string procOrdersGetGridFields = "procTS360GetGridFields";
        public const string procMARCGetSpecialCodePartTypes = "procMARCGetSpecialCodePartTypes";
        public const string procMARCSetSpecialCodeLines = "procMARCSetSpecialCodeLines";
        public const string procMARCGetFTPProfiles = "procMARCGetFTPProfiles";
        public const string procMARCGetSpecialCodesForTagRule = "procMARCGetSpecialCodesForTagRule";
        //public const string procMARCSetOrgProfilers = "procMARCSetProfiles";
        public const string procMARCSetProfiles = "procMARCSetProfiles";

        public static string CONST_GET_WAREHOUSES = @"GetWareHouses";
        public static string CONST_GET_ACCOUNT_BY_KEYWORDS = @"GetAccountsBySearch";
        public static string SP_GET_SHIP_OR_BILL_TO_ACCOUNT_ALL = @"GetAccountsWithType";

        //Product Catalogs store procedure
        public static string BTNG_GetFamilyKeys = @"procTS360GetFamilyKeys";

        // Site Branding
        public static string BTNG_GetSiteBranding = @"GetSiteBranding";

        // ProductInterestGroup
        public static string BTNG_ProductInterestGroup = @"GetProductInterestGroup";

        //Update hit count store procedure
        public static string BTNG_UpdateQueryTerm = @"procSetProductQueryTermsHitCount";

        //Update review date
        public static string BTNG_UpdateNGReviewDate = @"spNextGen_UpdateReviewDate";

        //Get Additional Version for EBook
        public static string BTNG_GetAdditionalVersions = @"procTS360GetAdditionaleBookInformation";
        public static string GetReviewPublicationType = "procTS360GetReviewPublicationType";

        #region Original Entry

        public static string procTS360SetBasketOriginalEntry = @"procTS360SetBasketOriginalEntry";
        public static string procTS360GetOriginalEntry = @"procTS360GetBasketOriginalEntry";
        public static string procTS360GetOriginalEntries = @"procTS360GetBasketOriginalEntries";

        #endregion

        //R2.6 Stored Procedures for Proxy/Assignment
        public static string CONST_GET_ASSIGNED_BTUSERS = @"procTS360GetAssignedBTUsers";

        public static string CONST_GET_BTUSERS_COUNT = @"procTS360GetBTUserCount";
        public static string CONST_UPDATE_ASSIGNED_BTUSERS = @"procTS360UpdateAssignedBTUsers";
        public static string CONST_SEARCH_FOR_BTUSERS = @"procTS360SearchForBTUsers";
        public static string CONST_SEARCH_FOR_USER = @"procTS360SearchForUsers";
        public static string CONST_GET_SALES_REP_FOR_ORGANIZATION = @"procTS360GetSalesRepsForOrganization";
        public static string CONST_SEARCH_FOR_SALES_REP = @"procTS360SearchForSalesRep";
        public static string CONST_SEARCH_FOR_ORGANIZATION = @"procTS360SearchForOrganizations";

        public static string procTS360CopyMARCProfileToOrg = @"procTS360CopyMARCProfileToOrg";

        public static string CONST_GET_ASSIGNED_ORGANIZATIONS = @"procTS360GetAssignedOrganizations";
        public static string CONST_GET_ORGANIZATIONS_BY_SEARCH = @"GetOrganizationsBySearch";

        public static string CONST_UPDATE_ASSIGNED_ORGANIZATIONS = @"procTS360UpdateAssignedOrganizations";
        public static string CONST_GET_PROXY_STATE = @"procTS360GetProxyState";
        public static string CONST_UPDATE_PROXY_STATE = @"procTS360UpdateProxyState";
        public static string CONST_GET_NUMBER_OF_ORGS = @"procTS360GetNumberOfOrganizations";
        #region Standing Orders
        //public const string procTS360GetOrganizationStandingOrderStatus = "procTS360GetOrganizationStandingOrderStatus";
        public const string procTS360GetUserStandingOrderStatus = "procTS360GetUserStandingOrderStatus";
        public const string procGetStandingOrders = "procTS360GetStandingOrders";
        public const string procAssignGridTemplateToStandingOrders = "procTS360AssignGridTemplateToStandingOrders";

        #endregion
        // BT Alerts
        public const string procGetUserSystemNotifications = "procGetUserSystemNotifications";
        public const string procUpdateSystemNotifications = "procUpdateSystemNotifications";
        public const string procInsertSystemNotifications = "procInsertSystemNotifications";
        public const string procUpdateUserAlerts = "procTS360SetUserAlerts";
        public const string procGetBTAlerts = "procTS360GetBTAlert";
        public const string ProcTs360GetUserAlerts = "procTS360GetUserAlerts";
        public const string procGetSystemNotification = "procTS360GetSystemNotification";
        public const string procGetUserAlertTotalCount = "procTS360GetUserAlertTotalCount";
        public const string procInsertUserAlert = "procTS360InsertAlertUserMessage";
        public const string procGetAlertMessageTemplate = "procGetAlertMessageTemplate";
        public const string procInsertAlertUserMessage = "procInsertAlertUserMessage";
        // ESP
        public const string CONST_GET_ESP_PREMIUM_SERVICES = "procTS360GetOrgESPPremiumServices";
        public const string CONST_SET_ESP_PREMIUM_SERVICES = "procTS360SetOrgESPPremiumServices";
        public const string CONST_GET_ESP_LIBRARIES = "procTS360GetESPLibraries";

        public const string CONST_GET_ORG_PREMIUM_SERVICES = "procTS360GetOrgPremiumServices";
        public const string CONST_SET_ORG_PREMIUM_SERVICES = "procTS360SetOrgPremiumServices";

        public const string CONST_GET_ACTIVE_PPC_SUBSCRIPTIONS = "procTS360GetActivePPCSubscriptions";
        public const string CONST_GET_ORG_PPC_SUBSCRIPTIONS = "procTS360GetOrganizationPPCSubscriptions";
       
        public static string CONST_GET_ESP_VERIFY_CODES = @"procTS360GetESPVerifiedCodes";
        public static string CONST_SET_ESP_VERIFY_CODES = @"procTS360SetESPVerifiedCodes";

        public static string CONST_GET_ILS_GRID_FIELDS = @"procTS360GetOrgILSGridFields";

        // Account
        public const string procGetAccountByID = "procTS360GetAccountByID";
        public const string procSetAccountByID = "procTS360SetAccountByID";
        public const string procRemoveBillToAccounts = "procTS360RemoveBillToAccounts";

        //Search Home
        public static string CONST_GET_ORGANIZATIONS_BY_SEARCH_HOME = @"GetOrganizationsBySearchHome";
        public static string CONST_GET_USER_BY_SEARCH_HOME = @"GetUsersBySearchHome";
        public static string GET_SHIP_OR_BILL_TO_ACCOUNT_SEARCH_HOME = @"GetAccountsWithTypeSearchHome";

        //Quotations
        public const string CONST_SET_ORG_GENERAL_INFORMATION = "procTS360SetOrganizationByID";
        public const string CONST_GET_ORG_GENERAL_INFORMATION = "procTS360GetOrganizationByID";

        public const string CONST_GET_ORG_ILS_GRID_FIELDS = "procTS360GetOrgILSGridFields";

        // My Preferences
        public const string CONST_GET_USER_PREFERENCE = "procTS360GetUserPreferenceByID";
        public const string CONST_SET_USER_PREFERENCE = "procTS360SetUserPreferenceByID";

        //CIP
        public static string CONST_GET_CIP_USERS = @"procTS360GetCIPUsersByOrg";
        public static string CONST_SET_CIP_USERS = @"procTS360SetCIPUsersByOrg";

        //OAuth
        public static string ProcGetSymmetricCryptoKey = @"procGetSymmetricCryptoKey";
        public static string ProcGetSymmetricCryptoKeyByBucket = @"procGetSymmetricCryptoKeyByBucket";
        public static string ProcInsertClientAuthorization = @"procInsertClientAuthorization";
        public static string ProcInsertNonce = @"procInsertNonce";
        public static string ProcInsertSymmetricCryptoKey = @"procInsertSymmetricCryptoKey";
        public static string ProcDeleteSymmetricCryptoKey = @"procDeleteSymmetricCryptoKey";
        public static string ProcGetScope = @"procGetScope";
        public static string ProcGetClient = @"procGetClient";
        public static string ProcInsertUser = @"procInsertUser";
        public static string ProcGetClientAuthorization = @"procGetClientAuthorization";

        // Saved Search
        public const string ProcSavedSearchDelete = "procTS360SavedSearchDelete";
        public const string ProcSavedSearchInsert = "procTS360SavedSearchInsert";
        public const string ProcSavedSearchUpdate = "procTS360SavedSearchUpdate";
        public const string ProcSavedSearchGet = "procTS360SavedSearchGetAllForUser";
        public const string ProcSavedSearchGetByID = "procTS360SavedSearchGetByID";

        // User Accounts
        public const string CONST_GET_USER_ACCOUNTS = "procTS360GetAccountsByUserID";
        public const string ProcTS360GetEnrichedBISACByAccountId = "procTS360GetEnrichedBISACByAccountId";

        public const string mktg_spRuntimeLoadDiscounts = "mktg_spRuntimeLoadDiscounts";
        public const string mktg_spGetExpression = "mktg_spGetExpression";
        public const string mktg_spRuntimeLoadAdvertisements = "mktg_spRuntimeLoadAdvertisements";

        //User Settings
        public const string CONST_GET_USER_SETTINGS = "procTS360GetUserSettings";
        public const string CONST_SET_USER_SETTINGS = "procTS360SetUserSettings";
        public const string CONST_SET_NOTIFICATION_CART_USERS = "procTS360SetTSSONotificationCartUsers";
        public const string CONST_GET_USER_NRC_PRODUCT_TYPES = "procTS360GetUserNRCProductTypes";
        public const string CONST_SET_USER_NRC_PRODUCT_TYPES = "procTS360SaveUserNRCProductTypes";

        public const string CONST_ORG_HAS_ONEBOX_ACCOUNT = "procTS360OrgHasOneBoxAccount";
        public const string CONST_GET_ONEBOX_SHIPPING_METHODS = "procTS360OneBoxGetShippingMethods";
        public const string CONST_SAVE_ONEBOX_SHIPPING_METHODS = "procTS360OneBoxSaveShippingMethods";
        public const string CONST_GET_ACTIVE_ONEBOX_SHIPPING_METHODS = "procTS360OneBoxGetActiveShippingMethods";
        public const string procMARCGetBasket = "procMARCGetBasket";
        public const string procTS360GeteSupplierAccountNumber = "procTS360GeteSupplierAccountNumber";
    }

    public class DBSortOder
    {
        public static int ASCENDING = 0;
        public static int DESCENDING = 1;
    }

    public class DBSortBy
    {
        public static string AccountId = "u_bt_account_id";
        public static string AccountNumber = "u_erp_account_number";
        public static string AccountName = "u_account_name";
        public static string City = "u_city";
        public static string ZipCode = "u_postal_code";
        public static string StateCode = "u_region_code";
        public static string AccountAlias = "u_account_alias";
        public static string OrganizationName = "u_Name";
        public static string BillToAccountNumber = "u_bill_to_account_number";
        public static string ProductType = "DisplayName";
        public static string LastLoginDateTime = "dt_last_logon";
        public static string PrimaryWarehouse = "u_warehouse_erp_code";
        public static string SecondaryWarehouse = "u_warehouse_erp_code";

        //For searching users
        //public static string UserDisplayName = "u_first_name";
        public static string UserDisplayName = "u_user_alias";
        public static string Email = "u_email_address";
        public static string UserName = "u_user_name";
        public static string AccountCount = "AccountCount";
        public static string SalesRepName = "u_salesrep_name";
    }

    /// <summary>
    /// Using this class to handle the Type Name when passing Table-Values parameter to sql server
    /// </summary>
    public class DBCustomTypeName
    {
        public static string integer_list_tbltype = "integer_list_tbltype";
        public static string udtCSReviewTypes = "udtCSReviewTypes";
        public static string UdtUibtKeys = "udtUIBTKeys";
        public static string UdtWarehouseId = "udtWarehouseID";
        public static string udtBTKeys = "udtBTKeys";
        public static string udtReviewSourceCodes = "udtReviewSourceCodes";
    }

    // General Constants
    public static class GeneralConstants
    {
        public static string StoreProcCheckInventory = @"dbo.inv_CheckInventoryByWarehouse";
        public static string StoreProcUpdateInventory = @"dbo.inv_UpdateInventoryByWarehouse";
        public static string StoreProcOnHandInventory = @"dbo.inv_GetOnhandInventory";
        public static string DatabaseInventory = @"connstr_db_inventory";
        public static string PersistSecurity = @";Persist Security Info=true;";
        public static string ProviderName = @"provider";
        public static char Semicolon = ';';
        public static string ProductCatalog = @"Product Catalog";
        public static string DatabaseCatalog = @"connstr_db_Catalog";
        public static string CatalogCacheKey = @"BTNextGenCatalogConnectionStringCacheKey";
        public static string DefaultQuantity = "0";
        public static string PrimaryMark = "*";
        public static string SecondaryMark = "**";
        public static string VipMark = "***";
    }

    public static class PropertyName
    {
        public const string Items = "Items";
        public const string WarehouseList = "WarehouseList";
        public const string IsTolas = "IsTolas";
        public const string AccountType = "AccountType";
        public const string AccountInventoryType = "AccountInventoryType";
        public const string InventoryReserveNumber = "InventoryReserveNumber";
        public const string CheckReserveFlag = "CheckReserveFlag";
        public const string Inventory = "Inventory";
        public const string ProductCatalog = "product_catalog";
        public const string ProductId = "product_id";
        public const string Quantity = "quantity";
        public const string CommerceResources = "CommerceResources";
        public const string ColumnName = "ColumnName";
        public const string DataType = "DataType";
        public const string RowsAffected = "RowsAffected";
        public const string InStockForRequest = "InStockForRequest";
        public const string WarehouseId = "WarehouseId";
        public const string LineItem = "LineItem";
        public const string StockCondition = "StockCondition";
        public const string OnOrderQuantity = "OnOrderQuantity";
        public const string Last30DayDemand = "Last30DayDemand";
        public const string BTKey = "BTKey";
    }
    public static class CommonErrorMessage
    {
        //public static readonly string UnexpectedErrorMessage = ResourceHelper.GetLocalizedString("ProfileResources", "UnexpectedError");
        public static readonly string UnexpectedErrorMessage = "Unexpected error! Please contact administrator.";

        public static readonly string BackgroundProcessInforMessage = "The services has been called successfully, a notification will be displayed when complete";

        //public static readonly string CART_ID_NULL = ResourceHelper.GetLocalizedString("OrderResources", "CART_ID_NULL");
        public static readonly string CART_ID_NULL = OrderResources.CART_ID_NULL;

        public static readonly string DashboardNameMustBeUnique = "Dashboard name must be unique.";
    }

    public class AccountConstants
    {
        public const int ESupplierAccountQuantity = 4;
        public const string INVALID_ACCOUNT = "Invalid Account";
    }

    public class AdvancedSearchConst
    {
        public const string KeyWordsConstString = "Keywords";
        public const string BeginsWithConst = "Begin";
        public const string KeywordsConst = "ngcontent";

        //Music Search
        public const string AlbumConst = "ngalbum";
        public const string ArtistConst = "ngresponsiblepartyprimary";
        public const string LabelConst = "nglabel";
        public const string PartNumberMusic = "ngpartnumber";

        //Movie search
        public const string TitleConst = "ngtitle";
        public const string ActorConst = "ngactor";
        public const string StudioConst = "ngstudio";
        public const string PartNumberMovie = "ngpartnumber";

        //Book search
        public const string AuthorConst = "ngresponsiblepartyprimary";
        public const string ReviewTextConst = "ngreviewtext";
        public const string SeriesConst = "ngseries";
        public const string BisacSubjectConst = "ngbisacsubject";
        public const string PublisherConst = "ngpublisher";
        public const string NarratorConst = "ngnarrator";
        public const string AnnotationConst = "ngannotation";
        public const string LccnConst = "nglccn";
        public const string MerchCategoryConst = "ngmerchcategory";
        public const string IllustratorConst = "ngillustrator";
        public const string TableofContentsConst = "ngtocs";
    }

    public class SearchResultsConstants
    {
        public const string SearchArgumentsForAddAll = "SearchResults_SearchArgumentsForAddAll";
    }

    public class ESPCategoryConstants
    {
        public const string LEGACY_CODE = "OVERALL";
        public const string LEGACY_NAME = "Overall";

        public const string ADULT_FICTION_CODE = "ADULT_FICTION";
        public const string ADULT_FICTION_NAME = "Adult Fic";

        public const string ADULT_NON_FICTION_CODE = "ADULT_NON_FICTION";
        public const string ADULT_NON_FICTION_NAME = "Adult Non-Fic";

        public const string YOUNG_ADULT_FICTION_CODE = "YOUNG_ADULT_FICTION";
        public const string YOUNG_ADULT_FICTION_NAME = "YA Fic";

        public const string YOUNG_ADULT_NON_FICTION_CODE = "YOUNG_ADULT_NON_FICTION";
        public const string YOUNG_ADULT_NON_FICTION_NAME = "YA Non-Fic";

        public const string JUVENILE_FICTION_CODE = "JUVENILE_FICTION";
        public const string JUVENILE_FICTION_NAME = "Juv Fic";

        public const string JUVENILE_NON_FICTION_CODE = "JUVENILE_NON_FICTION";
        public const string JUVENILE_NON_FICTION_NAME = "Juv Non-Fic";
    }

    public class NewReleaseCalendarConst
    {
        public const string STREET_DATE_VIEW_TITLE = "Street Date";
        public const string PRE_ORDER_VIEW_TITLE = "Pre Order";
        public const string ERROR_MESSAGE_CANNOT_GET_DATA = "Cannot get data.";
    }

    public class UPSConstants
    {
        public const string DELIVERED = "DELIVERED";
        public const string SHIPPED = "SHIPPED";
        public const string NO_TRACKING_AVAILABLE = "NO TRACKING AVAILABLE";
        public const string INVALID_TRACKING_NUMBER = "INVALID TRACKING NUMBER";
    }
}
