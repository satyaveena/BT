using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BT.TS360API.Cache;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Search;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Pricing;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360API.Common.Models;
using BT.TS360Constants;
using TS360Constants;
using ProfileService = BT.TS360API.ExternalServices.ProfileService;
using System.Threading.Tasks;
using BT.TS360API.Logging;
using BT.TS360API.Common.Controller;
using BT.TS360API.ExternalServices.NoSqlAPI;

namespace BT.TS360API.Common.Helpers
{
    public static class CommonHelperExt
    {
        public static string[] ToSortStrings(this IList<SortExpression> list)
        {
            var result = new List<string>();

            if (list != null)
            {
                foreach (var exp in list)
                {
                    if (!String.IsNullOrEmpty(exp.SortField))
                    {
                        result.Add(String.Format("{0}{1}", (exp.SortDirection == SortDirection.Ascending) ? "+" : "-", exp.SortField));
                    }
                }
            }

            return result.ToArray();
        }
    }
    public class CommonHelper
    {
        public const string TargetingContextAll = "ALL";

        private static volatile CommonHelper _instance;
        private static readonly object SyncRoot = new Object();

        public static readonly Dictionary<string, string> SortByDict = new Dictionary<string, string>() {
        {QuickCartDetailsSortByUI.title.ToString(), QuickCartDetailsSortByDB.Title.ToString()},
        {QuickCartDetailsSortByUI.responsiblepartyprimary.ToString(), QuickCartDetailsSortByDB.Author.ToString()},
        {QuickCartDetailsSortByUI.pubdate.ToString(), "Publish/Release Date"},
        {QuickCartDetailsSortByUI.listprice.ToString(), QuickCartDetailsSortByDB.ListPrice.ToString()},
        {QuickCartDetailsSortByUI.productformat.ToString(), QuickCartDetailsSortByDB.ProductFormat.ToString()},
        {QuickCartDetailsSortByUI.isbn.ToString(), QuickCartDetailsSortByDB.ISBN.ToString()},
        {QuickCartDetailsSortByUI.Quantity.ToString(), QuickCartDetailsSortByDB.Quantity.ToString()},
        {QuickCartDetailsSortByUI.Popularity.ToString(), QuickCartDetailsSortByDB.Popularity.ToString()},
        {QuickCartDetailsSortByUI.publisher.ToString(), QuickCartDetailsSortByDB.Publisher.ToString()},
        {QuickCartDetailsSortByUI.BasketOrder.ToString(), QuickCartDetailsSortByDB.BasketOrder.ToString()},
        {QuickCartDetailsSortByUI.CartOrder.ToString(), QuickCartDetailsSortByDB.BasketOrder.ToString()},
        {QuickCartDetailsSortByUI.LCClassAuthor.ToString(), QuickCartDetailsSortByDB.lcclassauthor.ToString()},
        {QuickCartDetailsSortByUI.LCClassArtist.ToString(), QuickCartDetailsSortByDB.lcclassartist.ToString()},
        {QuickCartDetailsSortByUI.DeweyAuthor.ToString(), QuickCartDetailsSortByDB.deweyauthor.ToString()},
        {QuickCartDetailsSortByUI.DeweyArtist.ToString(), QuickCartDetailsSortByDB.deweyartist.ToString()},
        {QuickCartDetailsSortByUI.Artist.ToString(), QuickCartDetailsSortByDB.Artist.ToString()},
        {QuickCartDetailsSortByUI.ESPOverallScore.ToString(), QuickCartDetailsSortByDB.ESPOverallScore.ToString()},
        {QuickCartDetailsSortByUI.ESPBisacScore.ToString(), QuickCartDetailsSortByDB.ESPBisacScore.ToString()}
        };

        public static readonly Dictionary<string, string> OrderStatusDict = new Dictionary<string, string>() {
            {"O", "Open"},
            {"C", "Closed"}
        };

        public enum QuickCartDetailsSortByUI
        {
            title = 0,
            responsiblepartyprimary = 1,
            pubdate,
            listprice,
            productformat,
            isbn,
            Quantity,
            Popularity,
            publisher,
            CartOrder,
            LCClassAuthor,
            LCClassArtist,
            DeweyAuthor,
            DeweyArtist,
            Artist,
            ESPOverallScore,
            BasketOrder,
            ESPBisacScore
        }

        private CommonHelper()
        {
        }

        #region Move wcf to api

        public enum QuickCartDetailsSortByDB
        {
            Title = 0,
            Author = 1,
            PublishReleaseDate,
            ListPrice,
            ProductFormat,
            ISBN,
            Quantity,
            Popularity,
            Publisher,
            BasketOrder,
            lcclassauthor,
            lcclassartist,
            deweyauthor,
            deweyartist,
            Artist,
            ESPOverallScore,
            ESPBisacScore
        }

        #endregion


        public static CommonHelper Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CommonHelper();
                }

                return _instance;
            }
        }

        public string ConvertSortFromGridToFast(string sort)
        {
            switch (sort.ToLower())
            {
                case SearchResultsSortField.SORT_TITLE:
                    return SearchFieldNameConstants.shorttitle;
                case SearchResultsSortField.SORT_AUTHOR:
                case SearchResultsSortField.SORT_ARTIST:
                    return SearchFieldNameConstants.responsiblepartyprimary;
                case SearchResultsSortField.SORT_FORMAT:
                    return SearchFieldNameConstants.format;
                case SearchResultsSortField.SORT_ISBN:
                    return SearchFieldNameConstants.isbn13;
                case SearchResultsSortField.SORT_PUBDATE:
                    return SearchFieldNameConstants.pubdate;
                case SearchResultsSortField.SORT_DEWEY_ARTIST:
                case SearchResultsSortField.SORT_DEWEY_AUTHOR:
                    return SearchFieldNameConstants.deweyprefix;
                case SearchResultsSortField.SORT_LCC_CLASS_ARTIST:
                case SearchResultsSortField.SORT_LCC_CLASS_AUTHOR:
                    return SearchFieldNameConstants.lcclass;
                case SearchResultsSortField.SORT_PUBLISHER:
                    return SearchFieldNameConstants.publisher;
                case SearchResultsSortField.SORT_LISTPRICE:
                    return SearchFieldNameConstants.listprice;
                case SearchResultsSortField.SORT_POPULARITY:
                    return SearchFieldNameConstants.popularity;
                default:
                    return SearchFieldNameConstants.shorttitle;

            }
        }
        public static string GetCartManagerURL()
        {
            return GetCurrentWebFrontEndUrl() + "/_layouts/CommerceServer/CartDetailsPage.aspx?CartId={0}";
        }
        public static string GetCurrentWebFrontEnd()
        {
            return BT.TS360API.Common.Configrations.AppSettings.InternetURL;
        }

        public static string GetCurrentWebFrontEndUrl()
        {
            //var webFrontEnd = GetCurrentWebFrontEnd();
            //var readAppSetting = GlobalConfiguration.ReadAppSetting(webFrontEnd);
            var readAppSetting = GetCurrentWebFrontEnd();
            if (readAppSetting != null)
                return readAppSetting;
            //
            return null;
        }
        //public static string SynchronizeSortExpression(string sort)
        //{
        //    switch (sort.ToLower())
        //    {
        //        case SearchResultsSortField.SORT_ARTIST:
        //            return SearchResultsSortField.SORT_AUTHOR;

        //        default:
        //            return sort;
        //    }
        //}

        ///// <summary>
        ///// Convert a string to array with certain separator
        ///// </summary>
        ///// <param name="param"></param>
        ///// <param name="separator"></param>
        ///// <returns></returns>
        //public static string[] ConvertToArray(string param, char separator)
        //{
        //    var result = new List<string>();
        //    var function = param.Split(separator);
        //    if (function.Length > 1)
        //    {
        //        //  Example for param : 
        //        //  14;Child_7_8;Child_2_4;Child_0_2;Professional;Scholarly_Graduate;
        //        //  Vocatinal_Technical;GeneralAdult;Teen_12_14;Child_9_11;Child_6_7;
        //        //  Child_5_6;Child_8_9;Scholarly_Associcate;Teen_15_18
        //        for (var i = 1; i < function.Length; i++)
        //        {
        //            result.Add(function[i]);
        //        }
        //    }
        //    return result.ToArray();
        //}

        ///// <summary>
        ///// Get Site Term array from a string with certain separator
        ///// </summary>
        ///// <param name="siteTermName"></param>
        ///// <param name="param"></param>
        ///// <param name="separator"></param>
        ///// <returns></returns>
        //public static string[] GetSiteTerm(SiteTermName siteTermName, string param, char separator)
        //{
        //    var temp = ConvertToArray(param, separator);
        //    var result = temp.Select(ele => ProfileController.Current.GetSiteTermName(siteTermName, ele)).ToList();
        //    return result.ToArray();
        //}

        ///// <summary>
        ///// Get date time format from resx
        ///// </summary>
        ///// <returns></returns>
        //public static string GetDateTimeFormat()
        //{
        //    var format = ResourceHelper.GetLocalizedString(ResourceName.Common.ToString(), "DateTimeFormat");
        //    return !String.IsNullOrEmpty(format) ? format : CommonConstants.DefaultDateTimeFormat;
        //}

        ///// <summary>
        ///// Get currency
        ///// </summary>
        ///// <returns></returns>
        public static string FormatPrice(decimal price)
        {
            if (price == -1)
                return CommonResources.NA;
            return GetCurrencyFormat(price);
        }

        /// <summary>
        /// Map product type string to number
        /// </summary>
        /// <param name="strProductType"></param>
        /// <returns></returns>
        public static string MapProductTypeToNumber(string strProductType)
        {
            string productType;
            if (String.Compare(strProductType, ProductTypeConstants.Book, StringComparison.OrdinalIgnoreCase) == 0 ||
                strProductType == "0")
            {
                productType = "0";
            }
            else
                productType = "1";
            return productType;
        }

        public static ProductType GetProductType(string strProductType)
        {
            ProductType productType;
            if (String.Compare(strProductType, ProductTypeConstants.Book, StringComparison.OrdinalIgnoreCase) == 0)
            {
                productType = ProductType.Book;
            }
            else if (String.Compare(strProductType, ProductTypeConstants.Movie, StringComparison.OrdinalIgnoreCase) == 0)
            {
                productType = ProductType.Movie;
            }
            else if (String.Compare(strProductType, ProductTypeConstants.Music, StringComparison.OrdinalIgnoreCase) == 0)
            {
                productType = ProductType.Music;
            }
            else
            {
                productType = ProductType.Book;
            }

            return productType;
        }

        //public static string GetCurrentWebFrontEnd()
        //{
        //    return ConfigurationManager.AppSettings["WebFrontEndName"];
        //}

        //public static string GetCurrentWebFrontEndUrl()
        //{
        //    var webFrontEnd = GetCurrentWebFrontEnd();
        //    var readAppSetting = GlobalConfiguration.ReadAppSetting(webFrontEnd);
        //    if (readAppSetting != null)
        //        return readAppSetting.Value;
        //    //
        //    return null;
        //}

        //public static int GetMaxLineLimitForSetQuantities()
        //{
        //    var maxLineLimit = 10;
        //    var readAppSetting = GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.MaxLineLimitForSetQuantities);
        //    if (readAppSetting != null)
        //    {
        //        Int32.TryParse(readAppSetting.Value, out maxLineLimit);
        //    }

        //    return maxLineLimit;
        //}

        //public static string GetWebFrontEndUrl(string webFrontEnd)
        //{
        //    var readAppSetting = GlobalConfiguration.ReadAppSetting(webFrontEnd);
        //    if (readAppSetting != null)
        //        return readAppSetting.Value;
        //    return null;
        //}

        public static string GetCurrencyFormat(string value)
        {
            try
            {
                return GetCurrencyFormat(Decimal.Parse(value));
            }
            catch (Exception)
            {
                return value;   //Don't need to format if it's exception
            }
        }

        public static string GetCurrencyFormat(decimal value)
        {
            try
            {
                // round currency value
                return value.ToString("c");
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        //public static string GetCurrencyFormatWithBlank(decimal value)
        //{
        //    try
        //    {
        //        var result = value.ToString("c");
        //        result = String.Format("$ {0}", result.Substring(1));
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        return String.Empty;
        //    }
        //}

        //public static bool IsInternetSite()
        //{
        //    //Cache for current request
        //    HttpContext context = HttpContext.Current;
        //    if (context.Items[RequestCacheKey.IS_INTERNET_SITE_KEY] != null)
        //        return (bool)context.Items[RequestCacheKey.IS_INTERNET_SITE_KEY];

        //    //6503 only Default is for Auth site, other sites should public.
        //    var isInternetSite = SPContext.Current.Site.Zone != SPUrlZone.Default;
        //    context.Items[RequestCacheKey.IS_INTERNET_SITE_KEY] = isInternetSite;
        //    return isInternetSite;
        //}

        //public static bool IsMarketingUsers()
        //{
        //    //Cache for current request
        //    HttpContext httpContext = HttpContext.Current;
        //    if (httpContext.Items[RequestCacheKey.IS_MARKET_USERS_KEY] != null)
        //        return (bool)httpContext.Items[RequestCacheKey.IS_MARKET_USERS_KEY];
        //    //
        //    if (!IsInternetSite())
        //    {
        //        SPContext spContext = SPContext.Current;
        //        if (spContext != null &&
        //            spContext.Web != null &&
        //            spContext.Web.CurrentUser != null)
        //        {
        //            var groups = spContext.Web.CurrentUser.Groups;
        //            foreach (SPGroup spGroup in groups)
        //            {
        //                if (spGroup.Name == "Marketing")
        //                {
        //                    httpContext.Items[RequestCacheKey.IS_MARKET_USERS_KEY] = true;
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    httpContext.Items[RequestCacheKey.IS_MARKET_USERS_KEY] = false;
        //    return false;
        //}

        ///// <summary>
        ///// Override the Redirect function of HttpContext
        ///// </summary>
        ///// <param name="url"></param>
        //public static void Redirect(string url)
        //{
        //    url = ProxySessionHelper.AppendProxyUserId(url);
        //    RedirectWithNoProxyAppending(url);
        //}

        //public static void RedirectWithNoProxyAppending(string url)
        //{
        //    HttpContext.Current.Response.Redirect(url, false);
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="upc"></param>
        ///// <remarks>
        ///// If user input is 11, 12, or 14 numeric - then it is a UPC. 
        ///// If user input is 13 digits numeric and does not begin with '978' or '979' - then it is a UPC. 
        ///// </remarks>
        ///// <returns></returns>
        //public static bool IsUPC(string upc)
        //{
        //    var result = false;

        //    if (!String.IsNullOrEmpty(upc))
        //    {
        //        if (upc.IndexOf(' ') != -1)
        //            return false;

        //        //check upc is numeric
        //        double upcNumber;
        //        if (!Double.TryParse(upc, out upcNumber))
        //            return false;

        //        switch (upc.Length)
        //        {
        //            case 11:
        //                result = true;
        //                break;
        //            case 12:
        //                result = true;
        //                break;
        //            case 13:
        //                if (!upc.Substring(0, 3).Equals("978") && !upc.Substring(0, 3).Equals("979"))
        //                    result = true;
        //                break;
        //            case 14:
        //                result = true;
        //                break;
        //        }
        //    }

        //    return result;
        //}

        public static bool IsISBN(string value)
        {
            if (String.IsNullOrEmpty(value))
                return false;
            if (value.IndexOf(' ') != -1)
                return false;

            double isbnNumber;
            if (Double.TryParse(value, out isbnNumber))
            {
                switch (value.Length)
                {
                    case 10:
                        //if user input 10 digits numeric, then it is an ISBN.
                        return true;
                        break;
                    case 13:
                        //If user input is 13 digits numeric and starts with 978 or 979. This is an ISBN.
                        if (value.StartsWith("978") || value.StartsWith("979"))
                            return true;
                        break;
                }
            }
            else
            {
                switch (value.Length)
                {
                    //If user input is 10 digits alphanumeric with the 10th position = 'x' then it is an ISBN.
                    case 10:
                        if (value[9].CompareTo('x') == 0)
                            return true;
                        break;
                }
                //isbn with dashes
                bool match = Regex.IsMatch(value, @"^((978[\--– ])?[0-9][0-9\--– ]{10}[\--– ][0-9xX])|((978)?[0-9]{9}[0-9Xx])$");
                return match;
            }

            return false;
        }

        ///// <summary>
        ///// Get valid UPC code
        ///// </summary>
        ///// <returns></returns>
        //public static string GetValidUPCString(string upc)
        //{
        //    if (!IsUPC(upc)) return upc;
        //    switch (upc.Length)
        //    {
        //        case 11:
        //            int checkDigit = CalculateCheckDigit(upc);
        //            return String.Format("00{0}{1}", upc, checkDigit.ToString());
        //        case 12:
        //            return String.Format("00{0}", upc);
        //        case 13:
        //            return String.Format("0{0}", upc);
        //        case 14:
        //            return upc;
        //        default:
        //            return upc;
        //    }
        //}

        ///// <summary>
        ///// Calculating the UPC Check Digit
        ///// </summary>
        ///// <param name="upc"></param>
        ///// <returns></returns>
        //private static int CalculateCheckDigit(string upc)
        //{
        //    var checkDigit = 0;
        //    if (!String.IsNullOrEmpty(upc) && upc.Length == 11)
        //    {
        //        var number = upc.ToCharArray();

        //        var oddTotal = 0;
        //        var evenTotal = 0;
        //        for (var i = 0; i < number.Count(); i++)
        //        {
        //            int intNo;
        //            if (Int32.TryParse(number[i].ToString(), out intNo))
        //            {
        //                if ((i + 1) % 2 == 0)
        //                {
        //                    evenTotal = evenTotal + intNo;
        //                }
        //                else
        //                {
        //                    oddTotal = oddTotal + intNo;
        //                }
        //            }
        //        }

        //        var sumOfOddnEven = 3 * oddTotal + evenTotal;

        //        checkDigit = 10 - (sumOfOddnEven % 10);
        //    }
        //    return checkDigit;
        //}


        ///// <summary>
        ///// Binds the properties.
        ///// </summary>
        ///// <param name="controls">The controls.</param>
        ///// <param name="attributes">The attributes.</param>
        //public static void BindProperties(List<object> controls, List<ItemData> attributes)
        //{
        //    foreach (var control in controls)
        //    {
        //        if (control is HtmlControl)
        //        {
        //            foreach (var attribute in attributes)
        //            {
        //                ((HtmlControl)control).Attributes[attribute.ItemDataValue] = attribute.ItemDataText;
        //            }
        //        }
        //        else if (control is WebControl)
        //        {
        //            foreach (var attribute in attributes)
        //            {
        //                ((WebControl)control).Attributes[attribute.ItemDataValue] = attribute.ItemDataText;
        //            }
        //        }

        //    }
        //}

        ///// <summary>
        ///// Binds the web control properties.
        ///// </summary>
        ///// <param name="controls">The controls.</param>
        ///// <param name="attributes">The attributes.</param>
        //public static void BindWebControlProperties(List<WebControl> controls, List<ItemData> attributes)
        //{
        //    foreach (var control in controls)
        //    {
        //        foreach (var attribute in attributes)
        //        {
        //            control.Attributes[attribute.ItemDataValue] = attribute.ItemDataText;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Decimal Place No Rounding
        ///// </summary>
        ///// <param name="d"></param>
        ///// <param name="decimalPlaces"></param>
        ///// <returns></returns>
        //public static string DecimalPlaceNoRounding(decimal decValue, int decimalPlaces = 2)
        //{

        //    decValue = decValue * (decimal)Math.Pow(10, decimalPlaces);
        //    decValue = Math.Truncate(decValue);
        //    decValue = decValue / (decimal)Math.Pow(10, decimalPlaces);

        //    return String.Format("{0:N" + Math.Abs(decimalPlaces) + "}", decValue);
        //}

        ///// <summary>
        ///// DiscountRound
        ///// </summary>
        public static string FormatDiscount(decimal decValue)
        {
            if (decValue == 0)
                return "0";

            return Math.Round(decValue, 1).ToString("N1");
        }

        ///// <summary>
        ///// If url contain domain, change it to relative url
        ///// </summary>
        ///// <param name="url"></param>
        ///// <returns></returns>
        //public static string ChangeToRelativeUrl(string url)
        //{
        //    if (url != null)
        //    {
        //        if (url.StartsWith(CMConstants.HTTP) || url.StartsWith(CMConstants.HTTPS))
        //        {
        //            // tempIndex: index after "http://" and "https://"
        //            int tempIndex = url.IndexOfAny(new char[] { '/' }, 8);
        //            if (tempIndex > 0)
        //                url = url.Substring(tempIndex);
        //        }
        //    }
        //    return url;
        //}

        public string ReplaceSingleQuote(string p)
        {
            if (!String.IsNullOrEmpty(p))
            {
                p = p.Replace("'", "\\'");
            }
            return p;
        }

        //public static string ReplaceSingleQuoteCXml(string p)
        //{
        //    if (!String.IsNullOrEmpty(p))
        //    {
        //        p = p.Replace("'", "&#39;");
        //    }
        //    return p;
        //}

        //public static string ReplaceAmpersandCXml(string p)
        //{
        //    if (!String.IsNullOrEmpty(p))
        //    {
        //        p = p.Replace("&", "&amp;");
        //    }
        //    return p;
        //}

        ///// <summary>
        ///// Get ResultPerPage from Cookie By User
        ///// </summary>
        ///// <returns></returns>
        //public static int GetResultPerPageByUser()
        //{
        //    var resultTable = ProfileController.Current.GetSiteTerm(SiteTermName.ResultPerPageTable, true);
        //    var itemsTable = new ItemsCollection(resultTable);
        //    //
        //    var resultPerPageCookieName = SiteContext.Current.UserId + CommonConstants.ResultPerPageCookieName;
        //    var resultPerPageValue = GetResultPerPageUserCookie(resultPerPageCookieName, true);
        //    if (!String.IsNullOrEmpty(resultPerPageValue) && IsContainItem(itemsTable, resultPerPageValue))
        //    {
        //        var ret = 0;
        //        Int32.TryParse(resultPerPageValue, out ret);
        //        return ret;
        //    }
        //    return 0;
        //}

        //private static bool IsContainItem(ItemsCollection itemsTable, string itemValue)
        //{
        //    if (itemsTable.Count <= 0)
        //        return false;
        //    foreach (var item in itemsTable)
        //    {
        //        if (item.ItemDataValue.Trim().Equals(itemValue.Trim()))
        //            return true;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Save User ResultPerPage to Cookie
        ///// </summary>
        ///// <param name="param"></param>
        //public static void SaveUserResultPerPageToCookie(string param)
        //{
        //    var resultPerPageName = SiteContext.Current.UserId + CommonConstants.ResultPerPageCookieName;
        //    SetResultPerPageUserCookie(resultPerPageName, param, true);
        //}

        /// <summary>
        /// Replace all occurences of a specified string in original string with a specified string regardless their case
        /// </summary>
        /// <param name="original">String to search</param>
        /// <param name="oldValue">A string to be replaced</param>
        /// <param name="newValue">A string to replace all occurences of oldVlaue</param>
        /// <returns>Replaced string</returns>
        public static string ReplaceInsensitiveString(string original, string oldValue, string newValue)
        {
            if (String.IsNullOrEmpty(original))
                return original;

            int count, position0, position1;

            count = position0 = position1 = 0;
            var upperString = original.ToUpper();
            var upperOldValue = oldValue.ToUpper();
            var inc = (original.Length / oldValue.Length) *
                      (newValue.Length - oldValue.Length);
            var chars = new char[original.Length + Math.Max(0, inc)];

            while ((position1 = upperString.IndexOf(upperOldValue,
                                              position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];

                for (int i = 0; i < newValue.Length; ++i)
                    chars[count++] = newValue[i];

                position0 = position1 + oldValue.Length;
            }

            if (position0 == 0)
                return original;

            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];

            return new string(chars, 0, count);
        }

        public static string Decode_ProductReviewTypeList(string input)
        {
            var rex = new Regex(@"_[\d]+_");
            var matches = rex.Matches(input);
            foreach (var match in matches)
            {
                var temp = Encoding.ASCII.GetString(new[]
                                               {
                                                   Byte.Parse(match.ToString()
                                                    .Replace("_", string.Empty)
                                                    )
                                               });
                input = input.Replace(match.ToString(), temp);
            }
            return input;
        }

        public static string Encode_ProductReviewTypeList(string input)
        {
            var temp = input;
            var rex = new Regex(@"[a-zA-Z0-9-_]");
            var matches = rex.Matches(input);
            temp = matches.Cast<object>().Aggregate(temp, (current, match) => current.Replace(match.ToString(), string.Empty));
            foreach (var item in temp)
            {
                var decodeString = Encoding.ASCII.GetBytes(new[] { item });
                input = input.Replace(item.ToString(), "_" + decodeString[0] + "_");
            }
            return input;
        }

        public bool IsStringInList(string s, string[] list)
        {
            if (s == null || list == null)
            {
                throw new ArgumentNullException();
            }

            foreach (string str in list)
            {
                if ((str.Trim().ToLower() == s.Trim().ToLower()) ||
                    (((ProductType)Enum.Parse(typeof(ProductType), str, true)).ToString().ToLower() == s.ToLower().Trim()))
                {
                    return true;
                }
            }

            return false;
        }

        //public static string DetermineURLForSearchAndBrowseResult(string currentPageURL)
        //{
        //    if (string.IsNullOrEmpty(currentPageURL)) return string.Empty;

        //    if (currentPageURL.ToLower().Contains(SiteUrl.SearchResults.ToLower()))
        //    {
        //        return SiteUrl.SearchResults;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.QuickSearchView.ToLower()))
        //    {
        //        return SiteUrl.QuickSearchView;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.QuickItemDetailsPage.ToLower()))
        //    {
        //        return SiteUrl.QuickItemDetailsPage;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.ItemDetails.ToLower()))
        //    {
        //        return SiteUrl.ItemDetails;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.AdvancedSearchAbsolutePath.ToLower()))
        //    {
        //        return SiteUrl.AdvancedSearchAbsolutePath;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.SavedSearch.ToLower()))
        //    {
        //        return SiteUrl.SavedSearch;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.ReleaseCalendarProducts.ToLower()))
        //    {
        //        return SiteUrl.ReleaseCalendarProducts;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.EListProducts.ToLower()))
        //    {
        //        return SiteUrl.EListProducts;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.PromotionProducts.ToLower()))
        //    {
        //        return SiteUrl.PromotionProducts;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.PublicationProducts.ToLower()))
        //    {
        //        return SiteUrl.PublicationProducts;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.BisacBrowsingPage.ToLower()))
        //    {
        //        return SiteUrl.BisacBrowsingPage;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.MovieGenrePage.ToLower()))
        //    {
        //        return SiteUrl.MovieGenrePage;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.MusicGenrePage.ToLower()))
        //    {
        //        return SiteUrl.MusicGenrePage;
        //    }
        //    return String.Empty;
        //}

        //public static string DetermineQueryStringToTrackForItemDetails(string currentPageURL)
        //{
        //    if (currentPageURL.ToLower().Contains(SiteUrl.SearchResults.ToLower()) ||
        //        currentPageURL.ToLower().Contains(SiteUrl.QuickSearchView.ToLower()) ||
        //        currentPageURL.ToLower().Contains(SiteUrl.CdmsListDetailView.ToLower()))
        //    {
        //        return QueryStringName.IsFromSearchResults;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.ReleaseCalendarProducts.ToLower()))
        //    {
        //        return QueryStringName.IsFromReleaseCalendarProducts;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.EListProducts.ToLower()))
        //    {
        //        return QueryStringName.IsFromEListProducts;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.PromotionProducts.ToLower()))
        //    {
        //        return QueryStringName.IsFromPromotionProducts;
        //    }
        //    else if (currentPageURL.ToLower().Contains(SiteUrl.PublicationProducts.ToLower()))
        //    {
        //        return QueryStringName.IsFromPublicationProducts;
        //    }
        //    return String.Empty;
        //}

        

        //public static string GetResultPerPageUserCookie(string cookieName, bool decryptValue)
        //{
        //    return SiteContext.Current.ReadCookie(cookieName, decryptValue);
        //}

        //public static void SetResultPerPageUserCookie(string cookieName, string value, bool encryptValue)
        //{
        //    SiteContext.Current.WriteCookie(cookieName, value, encryptValue);
        //}

        //public static Dictionary<string, Dictionary<string, string>> GeteSuppliersList(Organization organization)
        //{
        //    var eSuppliersList = new Dictionary<string, Dictionary<string, string>>();

        //    if (organization == null)
        //    {
        //        return null;
        //    }

        //    var eSuppliers = organization.ESuppliers;
        //    var accounts = organization.Accounts;

        //    if (eSuppliers != null && eSuppliers.Length > 0 && accounts != null && accounts.Count > 0)
        //    {
        //        foreach (var eSupplier in eSuppliers)
        //        {
        //            if (!eSuppliersList.ContainsKey(eSupplier))
        //            {
        //                var accountList = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        //                foreach (var account in accounts)
        //                {
        //                    var accountTarget = (Account)account.Target;
        //                    if (String.Compare(accountTarget.ESupplier, eSupplier, StringComparison.OrdinalIgnoreCase) == 0)
        //                    {
        //                        //AddAccountAlias
        //                        if (accountTarget.AccountAlias != null)
        //                        {
        //                            if (accountTarget.AccountAlias.Trim() != "")
        //                            {
        //                                if (!accountTarget.AccountNumber.Contains(accountTarget.AccountAlias))
        //                                {
        //                                    accountTarget.AccountNumber = accountTarget.AccountAlias + "(" + accountTarget.AccountNumber + ")";
        //                                }
        //                            }
        //                        } //AddAccountAlias

        //                        if (!accountList.ContainsKey(accountTarget.Id))
        //                            accountList.Add(accountTarget.Id, accountTarget.AccountNumber);
        //                    }
        //                }
        //                eSuppliersList.Add(eSupplier, accountList);
        //            }
        //        }
        //    }
        //    return eSuppliersList;
        //}

        ///// <summary>
        ///// Get default account of each eSupplier
        ///// </summary>
        ///// <param name="userProfile"></param>
        ///// <param name="organization"></param>
        ///// <returns></returns>
        //public static Dictionary<string, string> GetDefaultESuppliersAccountListOfUser(UserProfile userProfile, Organization organization)
        //{
        //    if (organization == null || userProfile == null)
        //    {
        //        return null;
        //    }

        //    var defaultESuppliersAccountList =
        //        VelocityCacheManager.Read(SessionVariableName.ESupplierDefaultAccountListCacheKey)
        //        as Dictionary<string, string>;

        //    if (defaultESuppliersAccountList != null)
        //    {
        //        return defaultESuppliersAccountList;
        //    }

        //    defaultESuppliersAccountList = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        //    var eSuppliers = organization.ESuppliers;
        //    var defaultESuppliersAccounts = GetDefaultESuppliersAccountOfUser(userProfile);

        //    if (eSuppliers == null || eSuppliers.Length <= 0 || defaultESuppliersAccounts == null ||
        //        defaultESuppliersAccounts.Length <= 0)
        //    {
        //        return null;
        //    }

        //    foreach (var eSupplier in eSuppliers)
        //    {
        //        var accountId = String.Empty;
        //        foreach (var defaultESuppliersAccount in defaultESuppliersAccounts)
        //        {
        //            var administrationProfileController = AdministrationProfileController.Current;
        //            administrationProfileController.AccountPropertiesToReturn.Add(Account.PropertyName.eSupplier);
        //            var account = administrationProfileController.GetAccountById(defaultESuppliersAccount);
        //            if (account != null && String.Compare(account.ESupplier, eSupplier,
        //                                                  StringComparison.OrdinalIgnoreCase) == 0)
        //            {
        //                accountId = account.Id;
        //            }
        //        }

        //        if (!defaultESuppliersAccountList.ContainsKey(eSupplier))
        //            defaultESuppliersAccountList.Add(eSupplier, accountId);
        //    }

        //    VelocityCacheManager.Write(SessionVariableName.ESupplierDefaultAccountListCacheKey,
        //                               defaultESuppliersAccountList, VelocityCacheLevel.Session);
        //    return defaultESuppliersAccountList;
        //}

        ///// <summary>
        ///// Get from user first, if null, lookup in org (use siteContext instead of org object)
        ///// </summary>
        ///// <param name="userProfile"></param>
        ///// <returns></returns>
        //private static string[] GetDefaultESuppliersAccountOfUser(UserProfile userProfile)
        //{
        //    if (userProfile != null && userProfile.Properties[UserProfile.RelationshipName.DefaulteSuppliersAccount] != null)
        //    {
        //        var defaulteSuppliersAccount = (object[])userProfile.Properties[UserProfile.RelationshipName.DefaulteSuppliersAccount];
        //        return defaulteSuppliersAccount.Select(o => o.ToString()).ToArray();
        //    }

        //    //if (organization != null && organization.Properties[Organization.RelationshipName.DefaulteSuppliersAccount] != null)
        //    //{
        //    //    var defaulteSuppliersAccount = (object[])organization.Properties[Organization.RelationshipName.DefaulteSuppliersAccount];
        //    //    return defaulteSuppliersAccount.Select(o => o.ToString()).ToArray();
        //    //}
        //    return null;
        //}

        //public static Dictionary<string, Dictionary<string, string>> GeteSuppliersList(Organization organization, UserProfile userProfile)
        //{
        //    if (userProfile == null || organization == null)
        //    {
        //        return null;
        //    }

        //    var eSuppliersList = VelocityCacheManager.Read(SessionVariableName.ESupplierAccountListCacheKey)
        //                                as Dictionary<string, Dictionary<string, string>>;

        //    if (eSuppliersList != null) return eSuppliersList;

        //    eSuppliersList = new Dictionary<string, Dictionary<string, string>>();

        //    var eSuppliers = organization.ESuppliers;
        //    var accounts = organization.Accounts;

        //    if (eSuppliers != null && eSuppliers.Length > 0 && accounts != null && accounts.Count > 0)
        //    {
        //        foreach (var eSupplier in eSuppliers)
        //        {
        //            var accountList = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        //            foreach (var account in accounts)
        //            {
        //                var accountTarget = (Account)account.Target;

        //                var administrationProfileController = AdministrationProfileController.Current;
        //                administrationProfileController.AccountPropertiesToReturn.Add(Account.PropertyName.eSupplier);
        //                administrationProfileController.AccountPropertiesToReturn.Add(Account.PropertyName.AccountName);
        //                administrationProfileController.AccountPropertiesToReturn.Add(Account.PropertyName.AccountNumber);
        //                var account1 = administrationProfileController.GetAccountById(accountTarget.Id);

        //                if (String.Compare(account1.ESupplier, eSupplier, StringComparison.OrdinalIgnoreCase) == 0 &&
        //                    IsAccountExistedInCreateCartsAccount(accountTarget, userProfile.AccountCreateCarts))
        //                {
        //                    //AddAccountAlias
        //                    if (account1.AccountAlias != null)
        //                    {
        //                        if (account1.AccountAlias.Trim() != "")
        //                        {
        //                            if (!account1.AccountNumber.Contains(account1.AccountAlias))
        //                            {
        //                                account1.AccountNumber = account1.AccountAlias + "(" + account1.AccountNumber + ")";
        //                            }
        //                        }
        //                    }//AddAccountAlias

        //                    if (!accountList.ContainsKey(account1.Id))
        //                        accountList.Add(account1.Id, account1.AccountNumber);
        //                }
        //            }

        //            if (!eSuppliersList.ContainsKey(eSupplier))
        //                eSuppliersList.Add(eSupplier, accountList);
        //        }
        //    }

        //    VelocityCacheManager.Write(SessionVariableName.ESupplierAccountListCacheKey, eSuppliersList,
        //        VelocityCacheLevel.Session);

        //    return eSuppliersList;
        //}

        //public static void AddESupplierPropertyTo(Account account)
        //{
        //    if (account != null && account.IsShippingAccount.HasValue && account.IsShippingAccount.Value)
        //    {
        //        var administrationProfileController = AdministrationProfileController.Current;
        //        administrationProfileController.AccountPropertiesToReturn.Add(Account.PropertyName.eSupplier);
        //        var account1 = administrationProfileController.GetAccountById(account.Id);
        //        if (account1 != null) account.ESupplier = account1.ESupplier;
        //    }
        //}

        public static List<string> GetOrgDefaultESupplierAccount(Organization organization)
        {
            if (organization != null && organization.DefaulteSuppliersAccountList != null && organization.DefaulteSuppliersAccountList.Count > 0) // organization.Properties[Organization.RelationshipName.DefaulteSuppliersAccount] != null)
            {
                var defaulteSuppliersAccount = organization.DefaulteSuppliersAccountList; // (object[])organization.Properties[Organization.RelationshipName.DefaulteSuppliersAccount];
                //if (defaulteSuppliersAccount != null && defaulteSuppliersAccount.Length > 0)
                //{
                //    return defaulteSuppliersAccount.Select(o => o.ToString()).ToList();
                //}

                return defaulteSuppliersAccount.ToList();
            }
            return new List<string>();
        }

        ///// <summary>
        ///// Check an account is exist in AccountCreateCarts
        ///// </summary>
        ///// <param name="account"></param>
        ///// <param name="accountCreateCarts"></param>
        ///// <returns></returns>
        //public static bool IsAccountExistedInCreateCartsAccount(Account account, CommerceRelationshipList accountCreateCarts)
        //{
        //    if (account == null || accountCreateCarts == null || accountCreateCarts.Count <= 0)
        //    {
        //        return false;
        //    }

        //    foreach (var accountCreateCart in accountCreateCarts)
        //    {
        //        var accountCreateCartTarget = (Account)accountCreateCart.Target;
        //        if (String.Compare(accountCreateCartTarget.Id, account.Id, StringComparison.OrdinalIgnoreCase) == 0)
        //        {
        //            return true;
        //        }

        //    }
        //    return false;
        //}



        ///// <summary>
        ///// return a list (eSupplier, (account id, account number)) of an organization
        ///// </summary>
        ///// <param name="org"></param>
        ///// <param name="eSupplierNames"></param>
        ///// <returns></returns>
        //public static Dictionary<string, Dictionary<string, string>> GetOrgESupplierAccountList(Organization org, string[] eSupplierValues)
        //{
        //    var eSupplierAccountList = CreateESupplierEmptyAccountList(eSupplierValues);
        //    foreach (var relationShip in org.Accounts)
        //    {
        //        var account = (Account)relationShip.Target;
        //        if (account != null && account.AccountType == AccountType.Book.ToString()
        //               && account.IsShippingAccount != null
        //               && (bool)account.IsShippingAccount
        //               && (account.Disabled == null || account.Disabled != true))
        //        {
        //            AdministrationProfileController.Current.AccountPropertiesToReturn.Add(Account.PropertyName.eSupplier);
        //            var account1 = AdministrationProfileController.Current.GetAccountById(account.Id);
        //            if (account1 != null && !String.IsNullOrEmpty(account1.ESupplier))
        //            {
        //                foreach (var es in eSupplierValues)
        //                {
        //                    if (String.Compare(account1.ESupplier, es, StringComparison.OrdinalIgnoreCase) == 0)
        //                    {
        //                        Dictionary<string, string> sta;
        //                        if (eSupplierAccountList.TryGetValue(es, out sta))
        //                        {
        //                            if (!sta.ContainsKey(account1.Id))
        //                            {
        //                                //AddAccountAlias
        //                                if (account1.AccountAlias != null)
        //                                {
        //                                    if (account1.AccountAlias.Trim() != "")
        //                                    {
        //                                        if (!account1.AccountNumber.Contains(account1.AccountAlias))
        //                                        {
        //                                            account1.AccountNumber = account1.AccountAlias + "(" + account1.AccountNumber + ")";
        //                                        }
        //                                    }
        //                                }//AddAccountAlias
        //                                sta.Add(account1.Id, account1.AccountNumber);
        //                            }
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return eSupplierAccountList;
        //}

        ///// <summary>
        ///// Create a Dictionary<ESupplier, Dictionary<(1),(2)>> with Dictionary<(1),(2)> is empty
        ///// </summary>
        ///// <param name="eSupplierValues"></param>
        ///// <returns></returns>
        //public static Dictionary<string, Dictionary<string, string>> CreateESupplierEmptyAccountList(string[] eSupplierValues)
        //{
        //    var eSupplierAccountList = new Dictionary<string, Dictionary<string, string>>();
        //    foreach (var es in eSupplierValues)
        //    {
        //        if (!eSupplierAccountList.ContainsKey(es))
        //        {
        //            var sta = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        //            eSupplierAccountList.Add(es, sta);
        //        }
        //    }
        //    return eSupplierAccountList;
        //}

        //public static Dictionary<string, string> GetOrgDefaultESupplierAccountListForRelatedAccount(Organization org)
        //{
        //    var defaultESupplierAccList = new Dictionary<string, string>();
        //    var orgEsuppliers = org.ESuppliers;
        //    if (org.DefaulteSuppliersAccount != null)
        //    {
        //        foreach (var defaultESupplierAcc in org.DefaulteSuppliersAccount)
        //        {
        //            var account = (Account)defaultESupplierAcc.Target;
        //            if (account != null)
        //            {
        //                AdministrationProfileController.Current.AccountPropertiesToReturn.Add(Account.PropertyName.eSupplier);
        //                account = AdministrationProfileController.Current.GetAccountById(((Account)defaultESupplierAcc.Target).Id);
        //                var eSupplier = account.ESupplier;
        //                var eSupplierName = (from es in orgEsuppliers
        //                                     where String.Compare(es, eSupplier, StringComparison.OrdinalIgnoreCase) == 0
        //                                     select es).FirstOrDefault();
        //                if (eSupplierName != null && !defaultESupplierAccList.ContainsKey(eSupplierName))
        //                {
        //                    defaultESupplierAccList.Add(eSupplierName, account.Id);
        //                }
        //            }
        //        }
        //    }
        //    return defaultESupplierAccList;
        //}

        ///// <summary>
        ///// Get default values collection
        ///// </summary>
        ///// <param name="termName"></param>
        ///// <returns></returns>
        //public static ItemsCollection GetSiteTerm(SiteTermName termName)
        //{
        //    var result = PreferencesProfileController.Current.GetSiteTerm(termName);
        //    if (result != null)
        //    {
        //        var items = new ItemsCollection(result);
        //        return items;
        //    }
        //    return null;
        //}

        //public static ItemsCollection GetSiteTermWithFirstItem(SiteTermName termName, string firstItemText)
        //{
        //    var result = GetSiteTerm(termName);
        //    if (result != null)
        //    {
        //        var firstItem = new ItemData("-1", firstItemText);
        //        result.Insert(0, firstItem);

        //        return result;
        //    }
        //    return null;
        //}
        ///// <summary>
        ///// Get Dictionary<eSupplier value, eSupplier name> from eSupplier value list
        ///// </summary>
        ///// <param name="eSupplier"></param>
        ///// <returns></returns>
        public static Dictionary<string, string> GetESupplierValueName(string[] eSupplierValues)
        {
            //var eSuppliers = GetSiteTerm(SiteTermName.eSuppliers);
            var eSuppliers = SiteTermHelper.Instance.GetSiteTemByName(SiteTermName.eSuppliers.ToString());

            var eSupplierValueName = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var esValue in eSupplierValues)
            {
                var es = (from e in eSuppliers
                          //where e.ItemDataValue.ToLower() == esValue.ToLower()
                          where e.ItemKey.ToLower() == esValue.ToLower()
                          select e).ToList();
                //if (es != null && es.Count > 0 && !eSupplierValueName.ContainsKey(es[0].ItemDataValue))
                if (es.Count > 0 && !eSupplierValueName.ContainsKey(es[0].ItemValue))
                {
                    //eSupplierValueName.Add(es[0].ItemDataValue, es[0].ItemDataText);
                    eSupplierValueName.Add(es[0].ItemValue, es[0].ItemValue);
                }
            }
            return eSupplierValueName;
        }

        public static string ConvertESupplierNameToValue(string eSupplierDisplayText)
        {
            var eSupplierName = new string[] { eSupplierDisplayText };
            var eSupplierValues = ConvertESupplierFromDisplayNameToKey(eSupplierName);
            var eSupplierValue = eSupplierValues != null && eSupplierValues.Count > 0
                                ? eSupplierValues[0]
                                : String.Empty;
            return eSupplierValue;
        }

        private static List<string> ConvertESupplierFromDisplayNameToKey(string[] eSupplierNames)
        {
            var eSuppliers = SiteTermHelper.Instance.GetSiteTemByName(SiteTermName.eSuppliers.ToString());// GetSiteTerm(SiteTermName.eSuppliers);
            var eSupplierValues = new List<string>();
            foreach (var esName in eSupplierNames)
            {
                var es = (from e in eSuppliers
                          where e.ItemValue.ToLower() == esName.ToLower()
                          select e).ToList();
                if (es.Count > 0)
                {
                    eSupplierValues.Add(es[0].ItemKey);
                }
            }
            return eSupplierValues;
        }

        /// <summary>
        /// Get esupplier text from esupplier code
        /// </summary>
        /// <param name="eSupplierKey"></param>
        /// <returns></returns>
        public static string GetESupplierTextFromSiteTerm(string eSupplierKey)
        {
            var eSuppliers = SiteTermHelper.Instance.GetSiteTemByName(SiteTermName.eSuppliers.ToString());

            foreach (var itemDataContract in eSuppliers)
            {
                if (string.Compare(itemDataContract.ItemKey, eSupplierKey, true) == 0)
                {
                    var eSupplierItem = eSuppliers.First(x => x.ItemKey.ToLower() == eSupplierKey.ToLower());
                    if (eSupplierItem != null)
                    {
                        return eSupplierItem.ItemValue;
                    }
                }
            }

            //if (eSuppliers.Contains(eSupplierKey))
            //{
            //    var eSupplierItem = eSuppliers.First(x => x.ItemDataValue.ToLower() == eSupplierKey.ToLower());
            //    if (eSupplierItem != null)
            //    {
            //        return eSupplierItem.ItemDataText;
            //    }
            //}
            return String.Empty;
        }

        //public static bool IsValidESupplier(string eSupplierCode)
        //{
        //    var eSupplierText = GetESupplierTextFromSiteTerm(eSupplierCode);
        //    if (!String.IsNullOrEmpty(eSupplierText))
        //        return true;
        //    return false;
        //}

        public static bool IsESupplierAccountExisted(string eSupplier, string userId)
        {
            var accountId = GetAccountIdAssociatedWithAnESupplier(eSupplier, userId);
            if (String.IsNullOrEmpty(accountId))
                return false;
            return true;
        }

        public static bool IsFollettBoundSupplierCode(string supplierCode)
        {
            if (!String.IsNullOrEmpty(supplierCode) &&
                       (supplierCode.Equals(SearchFieldValue.SupplierCodePPBTC, StringComparison.CurrentCultureIgnoreCase) ||
                        supplierCode.Equals(SearchFieldValue.SupplierCodePPBTR, StringComparison.CurrentCultureIgnoreCase) ||
                        supplierCode.Equals(SearchFieldValue.SupplierCodePPBTB, StringComparison.CurrentCultureIgnoreCase) ||
                        supplierCode.Equals(SearchFieldValue.SupplierCodePPBTM, StringComparison.CurrentCultureIgnoreCase)))
                return true;

            return false;
        }

        public static bool IsFollettBoundProduct(string productLine, string supplierCode)
        {
            if (!string.IsNullOrEmpty(productLine) && productLine.Equals(SpecialProductAttributes.PawPrintsProductLine))
            {
                if (CommonHelper.IsFollettBoundSupplierCode(supplierCode))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsMakerspaceProductFormat(string productFormat, string merchCategory)
        {
            if (string.Equals(productFormat, ProductFormatConstants.Book_Hardcover, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(merchCategory, ProductMerchandiseCategoryConstants.MAKERSPACE, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        ///// <summary>
        ///// Get account id of an esupplier
        ///// </summary>
        ///// <param name="eSupplier"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        public static string GetAccountIdAssociatedWithAnESupplier(string eSupplier, string userId)
        {
            var defaultESupplierAccount = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            string temp;
            return GetAccountIdAssociatedWithAnESupplier(eSupplier, userId, defaultESupplierAccount, out temp);
        }

        /// <summary>
        /// Get account id of an esupplier
        /// </summary>
        /// <param name="eSupplier"></param>
        /// <param name="defaultESupplierAccounts"></param>
        /// <returns></returns>
        public static string GetAccountIdAssociatedWithAnESupplier(string eSupplier, string[] defaultESupplierAccounts)
        {
            if (defaultESupplierAccounts != null && defaultESupplierAccounts.Length > 0)
            {
                foreach (var defaultESupplierAccount in defaultESupplierAccounts)
                {
                    var account = ProfileController.Instance.GetAccountById(defaultESupplierAccount);
                    //var administrationProfileController = AdministrationProfileController.Current;
                    //administrationProfileController.AccountPropertiesToReturn.Add(Account.PropertyName.eSupplier);
                    //var account = administrationProfileController.GetAccountById(defaultESupplierAccount);
                    if (account != null)
                    {
                        if (string.Compare(eSupplier, account.ESupplier, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            return account.AccountId;
                        }
                    }
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eSupplierTermName"></param>
        /// <param name="userId"></param>
        /// <param name="defaultESupplierAccount">must not be null</param>
        /// <returns></returns>
        public static string GetAccountIdAssociatedWithAnESupplier(string eSupplierTermName, string userId, Dictionary<string, string> defaultESupplierAccount,
            out string processingChargeAccId)
        {
            processingChargeAccId = string.Empty;
            if (defaultESupplierAccount.ContainsKey(eSupplierTermName))
            {
                processingChargeAccId = defaultESupplierAccount[eSupplierTermName];
                return processingChargeAccId;
            }
            if (defaultESupplierAccount.Count < AccountConstants.ESupplierAccountQuantity)
            {
                GetDefaultESupplierAccountsFromUser(userId, defaultESupplierAccount);
                if (defaultESupplierAccount.ContainsKey(eSupplierTermName))
                {
                    processingChargeAccId = defaultESupplierAccount[eSupplierTermName];
                    return processingChargeAccId;
                }
            }
            if (defaultESupplierAccount.Count < AccountConstants.ESupplierAccountQuantity)
            {
                GetDefaultESupplierAccountsFromOrgOfUser(userId, defaultESupplierAccount);
                if (defaultESupplierAccount.ContainsKey(eSupplierTermName))
                {
                    // processing charges don't take accounts at org level
                    return defaultESupplierAccount[eSupplierTermName];
                }
            }

            if (defaultESupplierAccount != null && defaultESupplierAccount.Count > 0)
            {
                var logString = "";
                foreach (var esupplier in defaultESupplierAccount)
                {
                    logString += string.Format("Key: {0}, Value: {1}, ", esupplier.Key, esupplier.Value);
                }
                PricingLogger.LogDebug("defaultESupplierAccount", string.Format("defaultESupplierAccount: {0}", logString));
            }

            return String.Empty;
        }

        public static void GetDefaultESupplierAccountsFromUser(string userId, Dictionary<string, string> result)
        {
            var user = ProfileService.Instance.GetUserById(userId);// CSObjectProxy.GetUserProfileForSearchResult(); //profilecontroller.GetUserById(userId);
            if (user != null)
            {
                var defaultESupplierAccountIds = user.DefaultESupplierAccountsList;// UserProfileHelper.GetUserDefaultESupplierAccount(user);
                if (defaultESupplierAccountIds != null && defaultESupplierAccountIds.Count > 0)
                {
                    foreach (var defaultESupplierAccount in defaultESupplierAccountIds)
                    {
                        //var administrationProfileController = AdministrationProfileController.Current;
                        //administrationProfileController.AccountPropertiesToReturn.Add(Account.PropertyName.eSupplier);
                        //var account = administrationProfileController.GetAccountById(defaultESupplierAccount);

                        var account = ProfileController.Instance.GetAccountById(defaultESupplierAccount);

                        if (account != null)
                        {
                            if (!String.IsNullOrEmpty(account.ESupplier) && !result.ContainsKey(account.ESupplier))
                            {
                                result.Add(account.ESupplier, account.AccountId);
                            }
                        }
                    }
                }
            }
        }

        public static void GetDefaultESupplierAccountsFromOrgOfUser(string userId, Dictionary<string, string> result)
        {
            var user = ProfileService.Instance.GetUserById(userId);// CSObjectProxy.GetUserProfileForSearchResult(); //profilecontroller.GetUserById(userId);
            if (user != null)
            {
                var organization = ProfileService.Instance.GetOrganizationById(user.OrgId);// (Organization)user.Organization.Target;
                if (organization != null)
                {
                    //var orgObject = ProfileController.Current.GetOrganizationDaoByIdForAdditionalInfo(organization.Id);
                    var orgDefaultEsupplierAccount = GetOrgDefaultESupplierAccount(organization);
                    if (orgDefaultEsupplierAccount != null && orgDefaultEsupplierAccount.Count > 0)
                    {
                        foreach (var defaultESupplierAccount in orgDefaultEsupplierAccount)
                        {
                            //if (!String.IsNullOrEmpty(defaultESupplierAccount.ESupplier) && !result.ContainsKey(defaultESupplierAccount.ESupplier))
                            //{
                            //    result.Add(defaultESupplierAccount.ESupplier, defaultESupplierAccount.Id);
                            //}

                            //var administrationProfileController = AdministrationProfileController.Current;
                            //administrationProfileController.AccountPropertiesToReturn.Add(Account.PropertyName.eSupplier);
                            var account = ProfileController.Instance.GetAccountById(defaultESupplierAccount);// administrationProfileController.GetAccountById(defaultESupplierAccount);
                            if (account != null)
                            {
                                if (!String.IsNullOrEmpty(account.ESupplier) && !result.ContainsKey(account.ESupplier))
                                {
                                    result.Add(account.ESupplier, account.AccountId);
                                }
                            }
                        }
                    }
                }
            }
        }

        ///// <summary>
        ///// Get SiteTerm ESuppliers <value, name>
        ///// </summary>
        ///// <returns></returns>
        //public static Dictionary<string, string> GetSiteTermESuppliers()
        //{
        //    var eSuppliers = GetSiteTerm(SiteTermName.eSuppliers);
        //    if (eSuppliers == null) return null;
        //    var result = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        //    foreach (var es in eSuppliers)
        //    {
        //        if (!result.ContainsKey(es.ItemDataValue))
        //            result.Add(es.ItemDataValue, es.ItemDataText);
        //    }
        //    return result;
        //}

        //public static int GetSortByNumber(string p)
        //{
        //    switch (p)
        //    {
        //        case CartSortByConstants.Name:
        //            return 0;
        //        case CartSortByConstants.BTStatus:
        //            return 1;
        //        case CartSortByConstants.LastUpdated:
        //            return 2;
        //        case CartSortByConstants.CartTotalNetPrice:
        //            return 3;
        //        case CartSortByConstants.ItemsCount:
        //            return 4;
        //        case CartSortByConstants.CartOwner:
        //            return 5;
        //    }
        //    return 0;
        //}

        //public static bool CheckCartStatus(string cartStatus, CartStatus cartStatusEnum)
        //{
        //    var cartStatusInt = -1;
        //    Int32.TryParse(cartStatus, out cartStatusInt);
        //    return cartStatusInt == (int)cartStatusEnum;
        //}

        //public static string ConvertCartStatusToString(string cartStatus)
        //{
        //    var cartStatusInt = -1;
        //    Int32.TryParse(cartStatus, out cartStatusInt);
        //    if (cartStatusInt == (int)CartStatus.Open)
        //    {
        //        return CartStatus.Open.ToString();
        //    }
        //    if (cartStatusInt == (int)CartStatus.Downloaded)
        //    {
        //        return CartStatus.Downloaded.ToString();
        //    }
        //    if (cartStatusInt == (int)CartStatus.Ordered)
        //    {
        //        return CartStatus.Ordered.ToString();
        //    }
        //    if (cartStatusInt == (int)CartStatus.Submitted)
        //    {
        //        return CartStatus.Submitted.ToString();
        //    }
        //    return String.Empty;
        //}

        public static bool IsEBookAccount(int accountType)
        {
            return accountType != (int)AccountType.Book && accountType != (int)AccountType.Entertainment && accountType != (int)AccountType.VIP;
        }

        public string ConvertAudienceTypeAsString(string[] audienceTypes)
        {
            string audienceType = audienceTypes.Aggregate("", (current, s) => current + (s + ","));
            audienceType = audienceType.TrimEnd(',');
            return audienceType;
        }

        /// <summary>
        /// Return N/A if discount price is 0
        /// </summary>
        /// <param name="discountPrice"></param>
        /// <param name="useCurrencyFormat"></param>
        /// <returns></returns>
        public static string GetDisplayValueOfDiscountPrice(decimal discountPrice, bool useCurrencyFormat)
        {
            if (discountPrice == 0)
                return CommonResources.NA;// ResourceHelper.GetLocalizedString(ResourceName.Common.ToString(), "NA");
            if (useCurrencyFormat)
                return GetCurrencyFormat(discountPrice);
            return discountPrice.ToString();
        }

        /// <summary>
        /// Return N/A if discount price is null or 0
        /// </summary>
        /// <param name="discountPrice"></param>
        /// <param name="useCurrencyFormat"></param>
        /// <returns></returns>
        public static string GetDisplayValueOfDiscountPrice(string discountPrice, bool useCurrencyFormat)
        {
            var na = CommonResources.NA;//ResourceHelper.GetLocalizedString(ResourceName.Common.ToString(), "NA");
            try
            {
                if (String.IsNullOrEmpty(discountPrice))
                    return na;
                else
                {
                    var dp = Decimal.Parse(discountPrice);
                    return GetDisplayValueOfDiscountPrice(dp, useCurrencyFormat);
                }
            }
            catch { return na; }
        }

        /// <summary>
        /// Return N/A if discount price is null or 0
        /// </summary>
        /// <param name="discountPrice"></param>
        /// <param name="useCurrencyFormat"></param>
        /// <returns></returns>
        public static string GetDisplayValueOfDiscountPrice(decimal? discountPrice, bool useCurrencyFormat)
        {
            if (discountPrice == null)
                return CommonResources.NA;// ResourceHelper.GetLocalizedString(ResourceName.Common.ToString(), "NA");
            return GetDisplayValueOfDiscountPrice(discountPrice.Value, useCurrencyFormat);
        }

        //public static bool IsCurrentOrgHasGaleESupplierAccount()
        //{
        //    AdministrationProfileController.Current.OrganizationRelated.AccountsNeeded = true;
        //    var org = AdministrationProfileController.Current.GetOrganization(SiteContext.Current.OrganizationId);
        //    if (org != null && org.Accounts != null)
        //    {
        //        foreach (var account in org.Accounts)
        //        {
        //            if (account != null)
        //            {
        //                AdministrationProfileController.Current.AccountPropertiesToReturn.Add(Account.PropertyName.eSupplier);
        //                var account1 = AdministrationProfileController.Current.GetAccountById(account.Id);
        //                if (account1 != null && String.Compare(account1.ESupplier, AccountType.GALEE.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
        //                    return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        public string DeterminePriceForGaleProduct(string price, bool isGaleAccountExisted, string[] eSuppliers)
        {
            var galeValue = AccountType.GALEE.ToString();
            //var galeLiteral = GetESupplierTextFromSiteTerm(galeValue);
            //// check if product is Gale product
            //var isGaleProduct = false;
            //if (productESupplier == galeLiteral)
            //    isGaleProduct = true;
            // check if org has eSupplier
            //var eSuppliers = SiteContext.Current.ESuppliers;
            var orgHasGaleESupplier = eSuppliers != null && eSuppliers.Contains(galeValue);

            if (orgHasGaleESupplier && !isGaleAccountExisted)
                return CommonResources.NA;//ResourceHelper.GetLocalizedString(ResourceName.Common.ToString(), "NA");
            return price;
        }

        public static string DeterminePriceForGaleProductInCart(string price, bool isGaleAccountInCart)
        {
            if (!isGaleAccountInCart)
                return CommonResources.NA;
            return price;
        }

        //public static UserControl LoadControl(Page pageHolder, string UserControlPath, params object[] constructorParameters)
        //{
        //    var userControl = pageHolder.LoadControl(UserControlPath) as UserControl;
        //    if (userControl != null)
        //    {
        //        try
        //        {
        //            var constParamTypes = new List<Type>();
        //            foreach (object constParam in constructorParameters)
        //            {
        //                constParamTypes.Add(constParam.GetType());
        //            }
        //            ConstructorInfo constructor =
        //                userControl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

        //            if (constructor != null)
        //            {
        //                constructor.Invoke(userControl, constructorParameters);
        //            }
        //        }
        //        catch { }
        //    }
        //    return userControl;
        //}
        //public static bool ValidatePhysicalFormat(string physicalFormat)
        //{
        //    if (!String.IsNullOrEmpty(physicalFormat))
        //    {
        //        var physicalFormats = OriginalEntryController.GetOEPhysicalFormat();
        //        if (physicalFormats != null)
        //        {
        //            foreach (var format in physicalFormats)
        //            {
        //                if (String.Compare(format.ItemDataValue, physicalFormat) == 0)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}

        //public static string GetOriginalEntryFormat(string p, IEnumerable<ItemData> oEFormats)
        //{
        //    if (oEFormats == null)
        //    {
        //        return String.Empty;
        //    }
        //    foreach (var oEFormat in oEFormats)
        //    {
        //        if (String.Compare(oEFormat.ItemDataValue, p, StringComparison.OrdinalIgnoreCase) == 0)
        //        {
        //            return oEFormat.ItemDataText;
        //        }
        //    }
        //    return String.Empty;
        //}

        //public static string GetOriginalEntryFormat(string p)
        //{
        //    var oEFormats = GetSiteTerm(SiteTermName.OriginalEntryPhysicalFormat);
        //    return GetOriginalEntryFormat(p, oEFormats);
        //}

        //public static string RefineGuidIdToUseInClient(string id)
        //{
        //    return id.Replace("{", "").Replace("}", "");
        //}

        //public static List<string> GetBtkeysByIsbnOrUpc(string isbnUpc, string marketType)
        //{
        //    var results = new List<string>();
        //    if (String.IsNullOrEmpty(isbnUpc)) return results;

        //    if (!IsISBN(isbnUpc) && !IsUPC(isbnUpc)) return results;
        //    if (marketType != "0" && marketType != "1") return results;

        //    return ProductDAO.Instance.GetBtkeysByIsbnOrUpc(isbnUpc, marketType);
        //}

        //#region Parser
        //public static bool ToBool(object value)
        //{
        //    var resultString = value.ToString();
        //    switch (resultString)
        //    {
        //        case "1":
        //            resultString = "True";
        //            break;
        //        case "0":
        //            resultString = "False";
        //            break;
        //    }

        //    bool result;
        //    return Boolean.TryParse(resultString, out result) && result;
        //}
        //#endregion

        public string EncryptMd5(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return GetMd5Hash(md5Hash, input);
            }
        }
        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        //// Verify a hash against a string. 
        //static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        //{
        //    // Hash the input. 
        //    string hashOfInput = GetMd5Hash(md5Hash, input);

        //    // Create a StringComparer an compare the hashes.
        //    StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        //    if (0 == comparer.Compare(hashOfInput, hash))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //private static readonly Regex RegexBetweenTags = new Regex(@">(?! )\s+", RegexOptions.Compiled);
        //private static readonly Regex RegexLineBreaks = new Regex(@"([\n\s])+?(?<= {2,})<", RegexOptions.Compiled);

        //public static string RemoveWhitespaceFromHtml(string html)
        //{
        //    html = RegexBetweenTags.Replace(html, ">");
        //    html = RegexLineBreaks.Replace(html, "<");

        //    return html.Trim();
        //}

        //public static ItemsCollection GetPageSizeFromResource()
        //{
        //    var listItemData = new List<ItemData>();

        //    ItemsCollection items;
        //    var resValue = ResourceHelper.GetLocalizedString("Common", "PageSizeValue");
        //    if (!string.IsNullOrEmpty(resValue))
        //    {
        //        var arr = resValue.Split('#');
        //        if (arr.Length == 3)
        //        {
        //            listItemData.Add(new ItemData(arr[0], arr[0]));
        //            listItemData.Add(new ItemData(arr[1], arr[1]));
        //            listItemData.Add(new ItemData(arr[2], arr[2]));
        //            items = new ItemsCollection(listItemData);
        //            return items;
        //        }
        //    }

        //    listItemData.Add(new ItemData("9", "9"));
        //    listItemData.Add(new ItemData("15", "15"));
        //    listItemData.Add(new ItemData("30", "30"));
        //    items = new ItemsCollection(listItemData);
        //    return items;
        //}

        //public static ItemsCollection GetPageSizeFromResourceForManageCartsPage()
        //{
        //    var listItemData = new List<ItemData>();

        //    ItemsCollection items;
        //    var resValue = ResourceHelper.GetLocalizedString("Common", "PageSizeValueForManageCart");
        //    if (!string.IsNullOrEmpty(resValue))
        //    {
        //        var arr = resValue.Split('#');
        //        if (arr.Length == 3)
        //        {
        //            listItemData.Add(new ItemData(arr[0], arr[0]));
        //            listItemData.Add(new ItemData(arr[1], arr[1]));
        //            listItemData.Add(new ItemData(arr[2], arr[2]));
        //            items = new ItemsCollection(listItemData);
        //            return items;
        //        }
        //    }

        //    listItemData.Add(new ItemData("10", "10"));
        //    listItemData.Add(new ItemData("15", "15"));
        //    listItemData.Add(new ItemData("25", "25"));
        //    items = new ItemsCollection(listItemData);
        //    return items;
        //}

        //public static ItemsCollection GetPageSizeFromResourceForCDMSListPage()
        //{
        //    var listItemData = new List<ItemData>();

        //    ItemsCollection items;
        //    var resValue = ResourceHelper.GetLocalizedString("Common", "PageSizeValueForCDMS");
        //    if (!string.IsNullOrEmpty(resValue))
        //    {
        //        var arr = resValue.Split('#');
        //        if (arr.Length == 3)
        //        {
        //            listItemData.Add(new ItemData(arr[0], arr[0]));
        //            listItemData.Add(new ItemData(arr[1], arr[1]));
        //            listItemData.Add(new ItemData(arr[2], arr[2]));
        //            items = new ItemsCollection(listItemData);
        //            return items;
        //        }
        //    }

        //    listItemData.Add(new ItemData("50", "50"));
        //    listItemData.Add(new ItemData("100", "100"));
        //    listItemData.Add(new ItemData("150", "150"));
        //    items = new ItemsCollection(listItemData);
        //    return items;
        //}
        //public static string ToQueryString(NameValueCollection queryStringCollection)
        //{
        //    return BuildQueryString(queryStringCollection);
        //}

        //public static NameValueCollection UpdateQueryStringAsCollection(NameValueCollection queryString, string key, string value)
        //{
        //    // Create a copy of the current query string
        //    var updatedParameters = (queryString == null
        //                                ? new NameValueCollection()
        //                                : new NameValueCollection(queryString));

        //    if (updatedParameters[key] == null)
        //    {
        //        updatedParameters.Add(key, value);
        //    }
        //    else
        //    {
        //        updatedParameters[key] = value;
        //    }

        //    // Generate and return the new query string.
        //    return updatedParameters;
        //}

        //public static NameValueCollection RemoveQueryStringAsCollection(NameValueCollection queryString, string key)
        //{
        //    if (queryString == null)
        //        return new NameValueCollection();
        //    // Create a copy of the current query string
        //    var updatedParameters = (queryString == null
        //                                ? new NameValueCollection()
        //                                : new NameValueCollection(queryString));

        //    if (updatedParameters[key] == null)
        //    {
        //        return updatedParameters;
        //    }

        //    updatedParameters.Remove(key);
        //    return updatedParameters;
        //}

        //public static NameValueCollection AppendvalueQueryStringAsCollection(NameValueCollection queryString, string key, string value)
        //{
        //    // Create a copy of the current query string
        //    var updatedParameters = (queryString == null
        //                                ? new NameValueCollection()
        //                                : new NameValueCollection(queryString));

        //    if (updatedParameters[key] == null)
        //    {
        //        updatedParameters.Add(key, value);
        //    }
        //    else
        //    {
        //        var orgValue = updatedParameters[key];
        //        if (string.Compare(value, orgValue, StringComparison.OrdinalIgnoreCase) != 0)
        //        {
        //            updatedParameters[key] = string.Format("{0},{1}", orgValue, value);
        //        }
        //    }

        //    // Generate and return the new query string.
        //    return updatedParameters;
        //}

        //public static string UpdateQueryStringAsString(NameValueCollection queryString, string key, string value)
        //{
        //    return ToQueryString(UpdateQueryStringAsCollection(queryString, key, value));
        //}

        //public static string AppendQueryStringValueAsString(NameValueCollection queryString, string key, string value)
        //{
        //    return ToQueryString(AppendvalueQueryStringAsCollection(queryString, key, value));
        //}

        //private static string BuildQueryString(NameValueCollection queryParameters)
        //{
        //    if (queryParameters == null)
        //        return string.Empty;

        //    const char equals = '=';
        //    const char ampersand = '&';
        //    const char questionMark = '?';
        //    int keyCount = queryParameters.Keys.Count;
        //    int current = 1;

        //    var builder = new StringBuilder();
        //    builder.Append(questionMark);

        //    foreach (string key in queryParameters.Keys)
        //    {
        //        string val = queryParameters[key];
        //        if (string.IsNullOrEmpty(val))
        //            continue;
        //        builder.Append(key);
        //        builder.Append(equals);
        //        builder.Append(Microsoft.Security.Application.Encoder.UrlEncode(val));
        //        if (current < keyCount)
        //        {
        //            builder.Append(ampersand);
        //            current++;
        //        }
        //    }

        //    string result = builder.ToString();
        //    result = result.TrimEnd(ampersand);
        //    return result;
        //}

        //private static int CountNumberOfDigits(int number)
        //{
        //    return number.ToString().Length;
        //    //
        //    //return (int)(Math.Log10(number) + 1);
        //}
        //private static int CalculateFreeSpaceForCartNameLength(int lineItemCount, int totalQuantity, decimal price)
        //{
        //    return (CommonConstants.MAX_LINE_ITEM_LENGTH - CountNumberOfDigits(lineItemCount)) +
        //            (CommonConstants.MAX_TOTALQUANTITY_LENGTH - CountNumberOfDigits(totalQuantity)) +
        //            (CommonConstants.MAX_PRICE_LENGTH - CountNumberOfDigits((int)price));
        //}

        //public static string GetBasketNameTruncated(string cartName, int lineItemCount, int totalQuantity, decimal price, bool isPricing)
        //{
        //    var addtionalLength = CalculateFreeSpaceForCartNameLength(lineItemCount, totalQuantity, isPricing ? 10 : price);

        //    var index = CommonConstants.MAXIMUM_CART_NAME_LENGTH - 3 + addtionalLength;
        //    if (index >= cartName.Length || index <= 0)
        //    {
        //        return cartName;
        //    }

        //    return cartName.Substring(0, index) + "...";
        //}

        //public static string GetSearchResultsPageUrl()
        //{
        //    return SiteContext.Current.IsQuickSearchEnabled ? SiteUrl.QuickSearchView : SiteUrl.SearchResults;
        //}
        //public static string GetCartDetailsPageUrl()
        //{
        //    return SiteContext.Current.IsQuickCartDetailsEnabled ? SiteUrl.QuickCartDetailsPage : SiteUrl.CartDetailsUrl;
        //}

        //public static string GetManageCartsPageUrl()
        //{
        //    return SiteContext.Current.IsQuickCartsListEnabled ? SiteUrl.QuickManageCartsPage : SiteUrl.ManageCartsPage;
        //}

        //public static string GetItemDetailsPageUrl()
        //{
        //    return SiteContext.Current.IsQuickItemDetailsEnabled ? SiteUrl.QuickItemDetailsPage : SiteUrl.ItemDetails;
        //}

        //public static int GetMARCCartLineThreshold()
        //{
        //    var maxItem = 100;
        //    var gcMarcCartLineThreshold =
        //            GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.MARCCartLineThreshold);

        //    if (gcMarcCartLineThreshold != null)
        //        maxItem = (gcMarcCartLineThreshold.Value != "")
        //                                    ? Convert.ToInt32(gcMarcCartLineThreshold.Value)
        //                                    : 100;
        //    return maxItem;
        //}

        //public static string GetMARCFullRecordIndicator()
        //{
        //    return SiteContext.Current.IsFullMarcProfile.HasValue
        //                                        ? SiteContext.Current.IsFullMarcProfile.Value ? "F" : "A"
        //                                        : "A";
        //}

        public static Dictionary<string, List<DataRow>> ConvertInventoryDataToDictionary(DataSet dsInventoryResults)
        {
            if (dsInventoryResults == null || dsInventoryResults.Tables.Count == 0) return null;

            var results = new Dictionary<string, List<DataRow>>();
            var rows = dsInventoryResults.Tables[0].Rows;

            foreach (DataRow row in rows)
            {
                var btKey = row["BTKey"].ToString();
                if (!results.ContainsKey(btKey))
                {
                    results[btKey] = new List<DataRow> { row };
                }
                else
                {
                    results[btKey].Add(row);
                }
            }
            return results;
        }

        public static GridFieldType ConvertToGridFieldType(string gridFieldType)
        {
            if (string.IsNullOrEmpty(gridFieldType)) return GridFieldType.Unknown;

            try
            {
                var result = (GridFieldType)Enum.Parse(typeof(GridFieldType), gridFieldType);
                return result;
            }
            catch
            {

            }
            return GridFieldType.Unknown;
        }

        //public static string GenerateGuidWithBracket()
        //{
        //    return Guid.NewGuid().ToString("B");
        //}

        //public static bool IsAuthorizeToUseAllGridCodes(string organizationId)
        //{
        //    if (string.Compare(organizationId, SiteContext.Current.OrganizationId, StringComparison.OrdinalIgnoreCase) == 0)
        //    {
        //        return SiteContext.Current.IsAuthorizedtoUseAllGridCodes;
        //    }

        //    var current = AdministrationProfileController.Current;
        //    current.OrganizationPropertiesToReturn.Add(Organization.PropertyName.IsAuthorizedtoUseAllGridCodes);
        //    var organization = current.GetOrganization(organizationId);

        //    return organization != null &&
        //        organization.IsAuthorizedtoUseAllGridCodes.HasValue &&
        //        organization.IsAuthorizedtoUseAllGridCodes.Value;
        //}

        //private const string AttributesLeft = "<span class=\"attributes-left\"></span>";
        //private const string AttributesRight = "<span class=\"attributes-right\"></span>";

        //public static string BuildContentIndicatorText(ProductContent productContent, string btKey)
        //{
        //    var result = new StringBuilder();
        //    var resultNonReturnable = "";
        //    var url = "";

        //    if (productContent.HasAnnotation)
        //        result.Append(CreateItemDetailsLink(btKey, "A", url));
        //    //AnnotationImage;
        //    //
        //    if (productContent.HasExcerpts)
        //        result.Append(CreateItemDetailsLink(btKey, "E", url));
        //    //ExcerptsImage;
        //    //
        //    if (productContent.HasReviews)
        //        result.Append(CreateItemDetailsLink(btKey, "R", url));
        //    //ReviewImage;
        //    //
        //    if (productContent.HasTOC)
        //        result.Append(CreateItemDetailsLink(btKey, "T", url)); //TOCImage;
        //    //
        //    if (!productContent.HasReturnKey)
        //    {
        //        resultNonReturnable = CreateItemDetailsLink(btKey, "N", url);
        //        //returnImage;
        //    }
        //    //
        //    if (productContent.HasMuze)
        //        result.Append(CreateItemDetailsLink(btKey, "M", url)); //TOCImage;
        //    //

        //    if (result.Length <= 0 && string.IsNullOrEmpty(resultNonReturnable))
        //    {
        //        return result.ToString();
        //    }

        //    if (result.Length > 0)
        //    {
        //        result.Insert(0, AttributesLeft);
        //        result.Append(AttributesRight);
        //    }

        //    if (!String.IsNullOrEmpty(resultNonReturnable))
        //    {
        //        result.Append(resultNonReturnable + ProductSupportedHtmlTag.DivCb);
        //    }
        //    else
        //    {
        //        result.Append(ProductSupportedHtmlTag.DivCb);
        //    }
        //    return result.ToString();
        //}

        //private static string CreateItemDetailsLink(string btKey, string tab, string webUrl)
        //{
        //    if (tab == "N")
        //    {
        //        return ProductSupportedHtmlTag.NonReturnImage;
        //    }
        //    var tooltip = "";
        //    switch (tab)
        //    {
        //        case "A":
        //            tooltip = "ANNOTATIONS";
        //            break;
        //        case "E":
        //            tooltip = "EXCERPTS";
        //            break;
        //        case "R":
        //            tooltip = "REVIEWS";
        //            break;
        //        case "M":
        //            tooltip = "MUSIC & VIDEO DATA";
        //            break;
        //        case "T":
        //            tooltip = "TABLE OF CONTENTS";
        //            break;
        //        default:
        //            tooltip = "";
        //            break;
        //    }
        //    string spanTab = string.Format("<span class=\"attributes-{0}-middle\"></span>", tab.ToLower());
        //    var url = webUrl;
        //    url += CreateUrlBTKeys(btKey);
        //    var result = "<a title=\"" + tooltip + "\" href=\"" + url + "&tab=" + tab + "\">" + spanTab + "</a>";
        //    return result;
        //}

        //public static string CreateUrlBTKeys(string value)
        //{
        //    if (String.IsNullOrEmpty(value))
        //        return String.Empty;

        //    var btkeys = value.Replace(',', '|').Replace('\n', '|');
        //    var result = SearchFieldNameConstants.btkey + "=" + btkeys;
        //    result = SiteUrl.ItemDetails + "?" + result;
        //    //for R2.6 BTR
        //    result = ProxySessionHelper.AppendProxyUserId(result);
        //    return result;
        //}

        //public static readonly Dictionary<string, string> SortByDict = new Dictionary<string, string>() {
        //{QuickCartDetailsSortByUI.title.ToString(), QuickCartDetailsSortByDB.Title.ToString()},
        //{QuickCartDetailsSortByUI.responsiblepartyprimary.ToString(), QuickCartDetailsSortByDB.Author.ToString()},
        //{QuickCartDetailsSortByUI.pubdate.ToString(), "Publish/Release Date"},
        //{QuickCartDetailsSortByUI.listprice.ToString(), QuickCartDetailsSortByDB.ListPrice.ToString()},
        //{QuickCartDetailsSortByUI.productformat.ToString(), QuickCartDetailsSortByDB.ProductFormat.ToString()},
        //{QuickCartDetailsSortByUI.isbn.ToString(), QuickCartDetailsSortByDB.ISBN.ToString()},
        //{QuickCartDetailsSortByUI.Quantity.ToString(), QuickCartDetailsSortByDB.Quantity.ToString()},
        //{QuickCartDetailsSortByUI.Popularity.ToString(), QuickCartDetailsSortByDB.Popularity.ToString()},
        //{QuickCartDetailsSortByUI.publisher.ToString(), QuickCartDetailsSortByDB.Publisher.ToString()},
        //{QuickCartDetailsSortByUI.BasketOrder.ToString(), QuickCartDetailsSortByDB.BasketOrder.ToString()},
        //{QuickCartDetailsSortByUI.CartOrder.ToString(), QuickCartDetailsSortByDB.BasketOrder.ToString()},
        //{QuickCartDetailsSortByUI.LCClassAuthor.ToString(), QuickCartDetailsSortByDB.lcclassauthor.ToString()},
        //{QuickCartDetailsSortByUI.LCClassArtist.ToString(), QuickCartDetailsSortByDB.lcclassartist.ToString()},
        //{QuickCartDetailsSortByUI.DeweyAuthor.ToString(), QuickCartDetailsSortByDB.deweyauthor.ToString()},
        //{QuickCartDetailsSortByUI.DeweyArtist.ToString(), QuickCartDetailsSortByDB.deweyartist.ToString()},
        //{QuickCartDetailsSortByUI.Artist.ToString(), QuickCartDetailsSortByDB.Artist.ToString()},
        //{QuickCartDetailsSortByUI.ESPOverallScore.ToString(), QuickCartDetailsSortByDB.ESPOverallScore.ToString()},
        //{QuickCartDetailsSortByUI.ESPBisacScore.ToString(), QuickCartDetailsSortByDB.ESPBisacScore.ToString()}
        //};

        //public enum QuickCartDetailsSortByUI
        //{
        //    title = 0,
        //    responsiblepartyprimary = 1,
        //    pubdate,
        //    listprice,
        //    productformat,
        //    isbn,
        //    Quantity,
        //    Popularity,
        //    publisher,
        //    CartOrder,
        //    LCClassAuthor,
        //    LCClassArtist,
        //    DeweyAuthor,
        //    DeweyArtist,
        //    Artist,
        //    ESPOverallScore,
        //    BasketOrder,
        //    ESPBisacScore
        //}

        //public enum QuickCartDetailsSortByDB
        //{
        //    Title = 0,
        //    Author = 1,
        //    PublishReleaseDate,
        //    ListPrice,
        //    ProductFormat,
        //    ISBN,
        //    Quantity,
        //    Popularity,
        //    Publisher,
        //    BasketOrder,
        //    lcclassauthor,
        //    lcclassartist,
        //    deweyauthor,
        //    deweyartist,
        //    Artist,
        //    ESPOverallScore,
        //    ESPBisacScore
        //}

        //public static bool ReGetUserProfileForQuickViewProp(string userId)
        //{
        //    UserProfileHelper.SetupDefaultUserSharedValue(userId);
        //    return false;
        //}

        //public static bool CheckUserFunction(string function, string[] functions)
        //{
        //    if (functions != null)
        //    {
        //        return functions.Any(item => string.Compare(item, function, StringComparison.OrdinalIgnoreCase) == 0);
        //    }
        //    return false;
        //}
        //public static string GetCountryCode(string orgId)
        //{
        //    var current = AdministrationProfileController.Current;
        //    current.OrganizationRelated.AddressNeeded = true;
        //    var org = current.GetOrganization(orgId);
        //    //Get addresses
        //    var addresses = org.Address != null ? (Address)org.Address.Target : null;
        //    //Bind addresses
        //    if (addresses == null) return string.Empty;
        //    return addresses.CountryRegionCode ?? string.Empty;
        //}

        public static bool TryParseEnum<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            bool isSuccess = false;
            result = default(TEnum);

            if (!string.IsNullOrEmpty(value))
            {
                var enumType = typeof(TEnum);
                if (enumType.IsEnum)
                {
                    if (Enum.IsDefined(enumType, value))
                    {
                        result = (TEnum)Enum.Parse(enumType, value);
                        isSuccess = true;
                    }
                    else
                    {
                        foreach (string name in Enum.GetNames(enumType))
                        {
                            if (name.Equals(value, StringComparison.OrdinalIgnoreCase))
                            {
                                result = (TEnum)Enum.Parse(enumType, value, true);
                                isSuccess = true;
                                break;
                            }
                        }
                    }
                }
            }

            return isSuccess;
        }

        //public static string GetEmailRegularExpression()
        //{
        //    const string cachekey = "EmailRegularExpressionCacheKey";
        //    var emailexpression = CachingController.Instance.Get(cachekey) as string;
        //    if (string.IsNullOrEmpty(emailexpression))
        //    {
        //        if (ConfigurationManager.AppSettings["EmailRegularExpression"] != null)
        //        {
        //            emailexpression = ConfigurationManager.AppSettings["EmailRegularExpression"];
        //            CachingController.Instance.Write(cachekey, emailexpression);
        //        }
        //    }
        //    return emailexpression;
        //}
        //public static void SetCookiesExpire(HttpContext currentContext)
        //{
        //    foreach (string key in currentContext.Request.Cookies.AllKeys)
        //    {
        //        if (key == "UserNameCookie") continue;

        //        var cookie = new HttpCookie(key);
        //        cookie.Expires = DateTime.UtcNow.AddDays(-7);
        //        currentContext.Response.Cookies.Add(cookie);
        //    }
        //}

        public SearchResultInventoryStatusArg GetSearchResultInventoryStatusArg(ProductSearchResultItem productInfo, string userMarketType)
        {
            var btKey = productInfo.BTKey;
            //            
            var catalogName = productInfo.Catalog /*SiteContext.Current.DefaultCatalogName*/;
            var productType = productInfo.ProductType;
            var flag = productInfo.ReportCode;
            var pubDate = productInfo.PublishDate;
            var merchandise = productInfo.MerchCategory;
            var marketType = string.IsNullOrEmpty(userMarketType) ? "-1" : userMarketType; // ?? (SiteContext.Current.MarketType ?? MarketType.Any).ToString();
            var pubCode = productInfo.Publisher;
            var eSupplier = productInfo.ESupplier;
            var reportCode = productInfo.ReportCode;
            var supplierCode = productInfo.SupplierCode;
            string[] blockedCountryCodes = !string.IsNullOrEmpty(productInfo.BlockedExportCountryCodes)
                ? productInfo.BlockedExportCountryCodes.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                : null;
            //
            var searchArg = new SearchResultInventoryStatusArg
            {
                CatalogName = catalogName,
                Flag = flag,
                BTKey = btKey,
                Quantity = 0,
                ProductType = productType,
                VariantId = "",
                PublishDate = pubDate,
                MerchandiseCategory = merchandise,
                MarketType = marketType,
                PubCodeD = pubCode,
                ESupplier = eSupplier,
                ReportCode = reportCode,
                SupplierCode = supplierCode,
                BlockedExportCountryCodes = blockedCountryCodes
            };
            return searchArg;
        }

        //public static string GetCurrentTimeZone(string orgId)
        //{
        //    const string defaultTimeZone = "Eastern Standard Time";
        //    if (string.Compare(orgId, SiteContext.Current.OrganizationId, StringComparison.OrdinalIgnoreCase) == 0)
        //    {
        //        return string.IsNullOrEmpty(SiteContext.Current.TimeZone) ? defaultTimeZone : SiteContext.Current.TimeZone;
        //    }

        //    var profileControllerForAdmin = AdministrationProfileController.Current;
        //    return profileControllerForAdmin.GetOrganizationTimeZone(orgId);
        //}

        //public static DateTime GetCurrentDateTimeByTimeZone()
        //{
        //    const string defaultTimeZone = "Eastern Standard Time";
        //    try
        //    {
        //        var timeZoneId = string.IsNullOrEmpty(SiteContext.Current.TimeZone)
        //            ? defaultTimeZone
        //            : SiteContext.Current.TimeZone;
        //        var userTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        //        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZoneInfo);
        //    }
        //    catch (TimeZoneNotFoundException tzException)
        //    {
        //        Logger.RaiseException(tzException, ExceptionCategory.General);
        //        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
        //            TimeZoneInfo.FindSystemTimeZoneById(defaultTimeZone));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.General);
        //        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
        //            TimeZoneInfo.FindSystemTimeZoneById(defaultTimeZone));
        //    }
        //}

        //public static void SetExpiredForOrganizationAccounts(string orgId)
        //{
        //    var cacheKey = string.Format(CacheKeyConstant.OrganizationAccouts, orgId);
        //    VelocityCacheManager.SetExpired(cacheKey, CommonCacheContant.Ts360FarmCacheName);

        //    ProfileController.Current.SetProfileExpiredForAdditionalInfo(NextGenProfilesTableName.UserObject, orgId);
        //}

        //public static bool IsOrderedCart(string cartStatus)
        //{
        //    return cartStatus == CartStatus.Ordered.ToString() || cartStatus == Basket.VIPORDERED || cartStatus == Basket.ORDEREDQUOTE;
        //}

        //public static string CreateHelpLink()
        //{
        //    var configuredLink = GlobalConfiguration.HelpTrainingLink;
        //    var passPhrase = ConfigurationManager.AppSettings["PassPhrase"];
        //    var initVector = ConfigurationManager.AppSettings["InitVector"];
        //    var result = "";
        //    if (SiteContext.Current == null || string.IsNullOrEmpty(configuredLink))
        //    {
        //    }
        //    else
        //    {
        //        var encryptedUserName = PunchoutHelper.TSecE(SiteContext.Current.LoginId, passPhrase, initVector);
        //        var encryptedGuid = PunchoutHelper.TSecE(SiteContext.Current.UserId, passPhrase, initVector);
        //        result = string.Format(configuredLink, encryptedUserName, encryptedGuid);
        //    }

        //    return result;
        //}

        //public static string GetSearchTypeForCartAdminPage(string keywordType)
        //{
        //    var resourceName = (ResourceName)Enum.Parse(typeof(ResourceName), "OrderResources", true);
        //    switch (keywordType)
        //    {
        //        case "0":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeAccountID");
        //        case "1":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeAuthorArtist");
        //        case "2":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeBISAC");
        //        case "3":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeBTKey");
        //        case "4":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeCartName");
        //        case "5":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeCartStatus");
        //        case "6":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeISBN");
        //        case "7":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeNotes");
        //        case "8":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypePONumber");
        //        case "9":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeTitle");
        //        case "10":
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeUPC");
        //        default:
        //            return ResourceHelper.GetLocalizedString(resourceName, "KeywordTypeQuoteID");
        //    }
        //}

        ///// <summary>
        ///// Encodes a string to be represented as a string literal. The format
        ///// is essentially a JSON string.
        ///// 
        ///// The string returned includes outer quotes 
        ///// Example Output: "Hello \"Rick\"!\r\nRock on"
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public static string EncodeJsString(string s)
        //{
        //    var sb = new StringBuilder();
        //    //sb.Append("\"");
        //    foreach (char c in s)
        //    {
        //        switch (c)
        //        {
        //            case '\"':
        //                sb.Append("\\\"");
        //                break;
        //            case '\\':
        //                sb.Append("\\\\");
        //                break;
        //            case '\b':
        //                sb.Append("\\b");
        //                break;
        //            case '\f':
        //                sb.Append("\\f");
        //                break;
        //            case '\n':
        //                sb.Append("\\n");
        //                break;
        //            case '\r':
        //                sb.Append("\\r");
        //                break;
        //            case '\t':
        //                sb.Append("\\t");
        //                break;
        //            default:
        //                var i = (int)c;
        //                if (i < 32 || i > 127)
        //                {
        //                    sb.AppendFormat("\\u{0:X04}", i);
        //                }
        //                else
        //                {
        //                    sb.Append(c);
        //                }
        //                break;
        //        }
        //    }
        //    //sb.Append("\"");

        //    return sb.ToString();
        //}

        //public static string GetSeqGridOptionCacheKey(string orgId)
        //{
        //    return string.Format(CacheKeyConstant.SeqGridOptionCacheKey, orgId);
        //}

        //public static string CompressString(string text)
        //{
        //    var buffer = Encoding.UTF8.GetBytes(text);
        //    var ms = new MemoryStream();
        //    using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
        //    {
        //        zip.Write(buffer, 0, buffer.Length);
        //    }

        //    ms.Position = 0;

        //    var compressed = new byte[ms.Length];
        //    ms.Read(compressed, 0, compressed.Length);

        //    var gzBuffer = new byte[compressed.Length + 4];
        //    Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
        //    Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
        //    return Convert.ToBase64String(gzBuffer);
        //}

        //public static string DecompressString(string compressedText)
        //{
        //    var gzBuffer = Convert.FromBase64String(compressedText);
        //    using (var ms = new MemoryStream())
        //    {
        //        var msgLength = BitConverter.ToInt32(gzBuffer, 0);
        //        ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

        //        var buffer = new byte[msgLength];

        //        ms.Position = 0;
        //        using (var zip = new GZipStream(ms, CompressionMode.Decompress))
        //        {
        //            zip.Read(buffer, 0, buffer.Length);
        //        }

        //        return Encoding.UTF8.GetString(buffer);
        //    }
        //}

        //public static List<NestedProfile> PrepareFullUserObjectNestedProfiles()
        //{
        //    var nestedProfiles = new List<NestedProfile>();
        //    var pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_accounts_create_carts"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_accounts_view_orders"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_book_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_entertainment_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_billing_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_shipping_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.OrganizationObject.ToString(),
        //        ProfileColumnName = "u_org_id",
        //        ParentColumnName = "u_org_id"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTSavedSearches.ToString(),
        //        ProfileColumnName = "u_saved_search_id",
        //        ParentColumnName = "u_saved_searches"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTInvalidLoginAttempts.ToString(),
        //        ProfileColumnName = "u_login_attempt_id",
        //        ParentColumnName = "u_invalid_login_attempts"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTProductInterestGroup.ToString(),
        //        ProfileColumnName = "u_id",
        //        ParentColumnName = "u_pig"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_esuppliers_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTSalesRep.ToString(),
        //        ProfileColumnName = "u_salesrep_id",
        //        ParentColumnName = "u_salesrep_id"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_VIP_account"
        //    };
        //    nestedProfiles.Add(pro);
        //    return nestedProfiles;
        //}

        //public static List<NestedProfile> PrepareUserObjectNestedProfilesForPricing()
        //{
        //    var nestedProfiles = new List<NestedProfile>();

        //    var pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_book_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_entertainment_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_billing_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_shipping_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.OrganizationObject.ToString(),
        //        ProfileColumnName = "u_org_id",
        //        ParentColumnName = "u_org_id"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_esuppliers_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_default_VIP_account"
        //    };
        //    nestedProfiles.Add(pro);

        //    pro = new NestedProfile
        //    {
        //        ProfileKey = NextGenProfilesTableName.BTAccounts.ToString(),
        //        ProfileColumnName = "u_bt_account_id",
        //        ParentColumnName = "u_accounts_view_orders"
        //    };
        //    nestedProfiles.Add(pro);

        //    return nestedProfiles;
        //}

        //#region Moving code from PromotionService

        //private static byte[] _basketBinary;
        //private const string TargetingContextAll = "ALL";
        //private static string _marketType = "";
        //private static string _myPrefProductType = "";
        //private static string _myPrefAudienceType = "";

        //private static string _pig = "";
        //private static string _siteBranding = "";
        //private static string _orgId = "";
        //private static string _orgName = "";

        //private static Microsoft.CommerceServer.Marketing.MarketingContext _marketingSystem;
        //public static Microsoft.CommerceServer.Marketing.MarketingContext MarketingSystem
        //{
        //    get
        //    {
        //        return _marketingSystem ??
        //               (_marketingSystem = Microsoft.CommerceServer.Marketing.MarketingContext.Create(CommerceContext.Current.SiteName, null,
        //                                                           AuthorizationMode.NoAuthorization));
        //    }
        //}

        public static List<PromotionPrice> CalculatePromotionPrices(List<LineRepricedInfo> lineItemRepricedList)
        {
            var result = new List<PromotionPrice>();
            foreach (var lineRepricedInfo in lineItemRepricedList)
            {
                var item = new PromotionPrice { BtKey = lineRepricedInfo.BTKey };
                string promoCode;
                item.Price = GetDiscountPrice(lineRepricedInfo, out promoCode);
                item.PromotionCode = promoCode;
                result.Add(item);
            }
            return result;
        }

        //private static MemoryStream BasketMemoryStream()
        //{
        //    if (_basketBinary == null)
        //    {
        //        var basket = CommerceContext.Current.OrderSystem.GetBasket(Guid.NewGuid());
        //        using (var stream = new MemoryStream())
        //        {
        //            var bformatter = new BinaryFormatter();
        //            //            
        //            bformatter.Serialize(stream, basket);
        //            _basketBinary = stream.ToArray();
        //        }
        //    }
        //    var basketMemoryStream = new MemoryStream(_basketBinary);
        //    return basketMemoryStream;
        //}

        //private static Microsoft.CommerceServer.Runtime.Orders.Basket CreateEmptyBasket()
        //{
        //    using (var basketMemoryStream = BasketMemoryStream())
        //    {
        //        var binaryFormatter = new BinaryFormatter();
        //        var emptyBasket = (Microsoft.CommerceServer.Runtime.Orders.Basket)binaryFormatter.Deserialize(basketMemoryStream);

        //        //
        //        emptyBasket.OrderForms.Add(new OrderFormEx());
        //        return emptyBasket;
        //    }
        //}

        private static decimal GetDiscountPrice(LineRepricedInfo lineRepricedInfo, out string promoCode)
        {
            promoCode = "";

            //var basket = CreateEmptyBasket();
            //var discountPrice = (decimal)0.0;
            //promoCode = string.Empty;
            //var step = 0;
            //try
            //{
            //    // Setting for TargetingContext
            //    _marketType = lineRepricedInfo.MarketType;
            //    if (!string.IsNullOrEmpty(lineRepricedInfo.ProductType))
            //    {
            //        var productType = Enum.Parse(typeof(ProductType), lineRepricedInfo.ProductType);
            //        _myPrefProductType = ((int)productType).ToString(CultureInfo.InvariantCulture);
            //    }
            //    else
            //    {
            //        _myPrefProductType = lineRepricedInfo.ProductType;
            //    }
            //    _myPrefAudienceType = lineRepricedInfo.AudienceType ?? string.Empty;
            //    _pig = lineRepricedInfo.Pig ?? string.Empty;
            //    _siteBranding = lineRepricedInfo.SiteBranding ?? string.Empty;
            //    _orgId = lineRepricedInfo.OrgId;
            //    _orgName = lineRepricedInfo.OrgName;
            //    var targetingParam = new TargetingParam()
            //    {
            //        AudienceType = AddAllValue(_myPrefAudienceType),
            //        ProductType = AddAllValue(_myPrefProductType),
            //        MarketType = lineRepricedInfo.MarketType,
            //        PIG = AddAllValue(_pig),
            //        SiteBranding = lineRepricedInfo.SiteBranding,
            //        OrgId = AddAllValue(_orgId),
            //        OrgName = AddAllValue(_orgName)
            //    };
            //    step = 1;
            //    promoCode = MarketingController.TargetProduct(lineRepricedInfo.BTKey, lineRepricedInfo.ProductCatalog,
            //        targetingParam);
            //    step = 2;
            //    if (!string.IsNullOrEmpty(promoCode))
            //    {
            //        var lineItemEx = AddProductToBasket(basket, lineRepricedInfo.BTKey, lineRepricedInfo.ProductCatalog,
            //            lineRepricedInfo.TotalLineQuantity);
            //        var pipelineInfo = CreateBasketPipelineInfo(lineRepricedInfo.UserId);
            //        //            
            //        step = 3;
            //        basket.RunPipeline(pipelineInfo);
            //        step = 4;
            //        discountPrice = lineItemEx.BTSalePrice;
            //        //
            //        ClearAllLineItemsInBasket(basket); //Clear all line items in basket for basket reusing.
            //        //
            //    }

            //}
            //catch (Exception ex)
            //{
            //    var param = string.Format("BTKey:{0};UserID:{1};Step:{2}", lineRepricedInfo.BTKey, lineRepricedInfo.UserId, step);
            //    Logger.Write(ExceptionCategory.Pricing.ToString(), string.Format("{2}-{0}, {1}", ex.Message, ex.StackTrace, param));
            //}
            //finally
            //{
            //    basket.Delete();
            //}
            return 12; //discountPrice;
        }

        //private static void ClearAllLineItemsInBasket(Microsoft.CommerceServer.Runtime.Orders.Basket basket)
        //{
        //    basket.OrderForms[0].LineItems.Clear();
        //}

        //private static LineItemEx AddProductToBasket(Microsoft.CommerceServer.Runtime.Orders.Basket basket, string productId,
        //    string productCatalog, int quantity)
        //{
        //    var lineItemEx = new LineItemEx { ProductId = productId, ProductCatalog = productCatalog, Quantity = quantity };
        //    basket.OrderForms[0].LineItems.Add(lineItemEx);
        //    return lineItemEx;
        //}

        //private static PipelineInfo CreateBasketPipelineInfo(string userId)
        //{
        //    var pipelineInfo = new PipelineInfo("Basket", OrderPipelineType.Basket);
        //    var user = GetUserProfileObject(userId);
        //    pipelineInfo.Profiles.Add("UserObject", user);

        //    var ctx = CommerceContext.Current;

        //    ctx.TargetingSystem.TargetingContextProfile.Properties["ProductInterestGroup"].Value = AddAllValue(_pig);
        //    ctx.TargetingSystem.TargetingContextProfile.Properties["SiteBranding"].Value = _siteBranding;

        //    ctx.TargetingSystem.TargetingContextProfile.Properties["market_type"].Value = _marketType;
        //    ctx.TargetingSystem.TargetingContextProfile.Properties["product_type"].Value = AddAllValue(_myPrefProductType);
        //    ctx.TargetingSystem.TargetingContextProfile.Properties["audience_type"].Value = AddAllValue(_myPrefAudienceType);
        //    ctx.TargetingSystem.TargetingContextProfile.Properties["org_id"].Value = AddAllValue(_orgId);
        //    ctx.TargetingSystem.TargetingContextProfile.Properties["org_name"].Value = AddAllValue(_orgName);

        //    pipelineInfo.Profiles.Add("targetingContext", ctx.TargetingSystem.TargetingContextProfile);

        //    return pipelineInfo;
        //}

        //private static Profile GetUserProfileObject(string userId)
        //{
        //    CommerceContext.Current.UserID = userId;

        //    var user = CommerceContext.Current.UserProfile;

        //    return user;
        //}

        //private static string AddAllValue(string item)
        //{
        //    const string delimiter = ";";

        //    //if (item == "")
        //    //    return string.Empty;

        //    return TargetingContextAll + delimiter + item;
        //}

        //#endregion

        public string GetCacheKeyFromSessionKey(string userId, string sessionKey)
        {
            return string.Format("{0}{1}", sessionKey, userId);
        }

        public string RefineProductTypeToMusicIfMovie(string productType)
        {
            if (string.Compare(productType, ProductType.Movie.ToString()) == 0)
            {
                productType = ProductType.Music.ToString();
            }
            return productType;
        }

        public static bool IsAVProduct(string productType)
        {
            bool isAV = string.Compare(productType, ProductType.Music.ToString(), StringComparison.OrdinalIgnoreCase) == 0
                        || string.Compare(productType, ProductType.Movie.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
            return isAV;
        }

        public Account GetAccountForSOPPricePlan(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
                return null;

            //Current.AccountRelated.BillToAccountNeeded = true;
            var bookAcc = ProfileController.Instance.GetAccountById(accountId); // Current.GetAccountById(accountId);

            if (bookAcc != null)
            {
                if (bookAcc.IsBillingAccount == true)
                {
                    return bookAcc;
                }
                //bookAcc.IsBillingAccount = false
                //Check to see if the ship-to-account already has a price plan
                if (!string.IsNullOrEmpty(bookAcc.SOPPricePlanId))
                {
                    return bookAcc;
                }

                //Get the bill-to-account associated with this ship-to-account.
                var billToAccount = ProfileController.Instance.GetAccountById(bookAcc.BillToAccountNumber);
                return billToAccount;
            }
            return null;
        }

        //public static void GetDefaulAccountId(string eSupplier, string cartId, string userId, out string entAccountId, 
        //    out string bookAccountId, out Dictionary<string, string> defaultESupplierAccountIds, out string vipAccountId)
        public static DefaultAccountObject GetDefaulAccountId(string eSupplier, string cartId, string userId, string entAccountId,
            string bookAccountId, Dictionary<string, string> defaultESupplierAccountIds, string vipAccountId)
        {
            defaultESupplierAccountIds = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            bookAccountId = string.Empty;
            entAccountId = string.Empty;
            vipAccountId = string.Empty;

            if (string.IsNullOrEmpty(cartId)) return null;
            if (string.IsNullOrEmpty(userId)) return null;

            Cart currentCart = CartDAOManager.Instance.GetCartById(cartId, userId);// CartContext.Current.GetCartManagerForUser(userId).GetCartById(cartId);

            if (currentCart != null && currentCart.CartAccounts != null)
            {
                bool hasOneBoxAccount = false;
                var cartAccounts = currentCart.CartAccounts.Where(acct => acct != null);
                foreach (var cartAccount in cartAccounts)
                {
                    if (cartAccount.AccountType == (int)AccountType.VIP)
                    {
                        vipAccountId = cartAccount.ERPAccountGUID;
                    }
                    else if (!string.IsNullOrEmpty(cartAccount.ESupplierID))
                    {
                        if (!string.IsNullOrEmpty(cartAccount.AccountID))
                        {
                            if (!defaultESupplierAccountIds.ContainsKey(cartAccount.ESupplierID))
                                defaultESupplierAccountIds.Add(cartAccount.ESupplierID, cartAccount.ERPAccountGUID);
                        }
                    }
                    else if (cartAccount.AccountType == (int)AccountType.OneBox)
                    {
                        bookAccountId = entAccountId = cartAccount.ERPAccountGUID;
                        hasOneBoxAccount = true;
                    }
                    else if (cartAccount.AccountType == (int)AccountType.Book)
                        bookAccountId = cartAccount.ERPAccountGUID;
                    else if (cartAccount.AccountType == (int)AccountType.Entertainment)
                        entAccountId = cartAccount.ERPAccountGUID;
                }

                if (!hasOneBoxAccount)
                {
                var isHomeDeliveryOnly = false;

                ProductSearchController.ApplyHomeDelivery(entAccountId, ref entAccountId, ref bookAccountId, out isHomeDeliveryOnly);
                if (!isHomeDeliveryOnly)
                    ProductSearchController.ApplyHomeDelivery(bookAccountId, ref entAccountId, ref bookAccountId, out isHomeDeliveryOnly);
            }
            }

            var result = new DefaultAccountObject();
            result.BookAccountId = bookAccountId;
            result.EntAccountId = entAccountId;
            result.VIPAccountId = vipAccountId;
            result.DefaultESupplierAccountIds = defaultESupplierAccountIds;

            return result;
        }
        public string Decode(string input)
        {
            var rex = new Regex(@"_[\d]+_");
            var matches = rex.Matches(input);
            foreach (var match in matches)
            {
                var temp = Encoding.ASCII.GetString(new[]
                                               {
                                                   Byte.Parse(match.ToString()
                                                    .Replace("_", string.Empty)
                                                    )
                                               });
                input = input.Replace(match.ToString(), temp);
            }
            return input;
        }
        /// <summary>
        /// Encodes the specified input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns></returns>
        public string Encode(string input)
        {
            var temp = input;
            var rex = new Regex(@"[a-zA-Z0-9-_]");
            var matches = rex.Matches(input);
            temp = matches.Cast<object>().Aggregate(temp, (current, match) => current.Replace(match.ToString(), string.Empty));
            foreach (var item in temp)
            {
                var decodeString = Encoding.ASCII.GetBytes(new[] { item });
                input = input.Replace(item.ToString(), "_" + decodeString[0] + "_");
            }
            return input;
        }

        public bool GetQuickViewModeInfo(string userId, string quickViewPage)
        {
            var cacheKey = GetCacheKeyFromSessionKey(userId, quickViewPage);
            var oValue = CachingController.Instance.Read(cacheKey);
            if (oValue == null) return false;

            return (bool)oValue;
        }

        public string CompressString(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            var ms = new MemoryStream();
            using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;

            var compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            var gzBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        static public string DecompressString(string compressedText)
        {
            var gzBuffer = Convert.FromBase64String(compressedText);
            using (var ms = new MemoryStream())
            {
                var msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                var buffer = new byte[msgLength];

                ms.Position = 0;
                using (var zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static string GetESPCategoryName(string espCategoryType)
        {
            if (string.IsNullOrEmpty(espCategoryType))
            {
                return ESPCategoryConstants.LEGACY_NAME;
            }
            else
            {
                espCategoryType = espCategoryType.ToUpper();
            }


            string retVal = "";
            switch (espCategoryType)
            {
                case ESPCategoryConstants.LEGACY_CODE:
                    retVal = ESPCategoryConstants.LEGACY_NAME;
                    break;
                case ESPCategoryConstants.ADULT_FICTION_CODE:
                    retVal = ESPCategoryConstants.ADULT_FICTION_NAME;
                    break;
                case ESPCategoryConstants.ADULT_NON_FICTION_CODE:
                    retVal = ESPCategoryConstants.ADULT_NON_FICTION_NAME;
                    break;
                case ESPCategoryConstants.JUVENILE_FICTION_CODE:
                    retVal = ESPCategoryConstants.JUVENILE_FICTION_NAME;
                    break;
                case ESPCategoryConstants.JUVENILE_NON_FICTION_CODE:
                    retVal = ESPCategoryConstants.JUVENILE_NON_FICTION_NAME;
                    break;
                case ESPCategoryConstants.YOUNG_ADULT_FICTION_CODE:
                    retVal = ESPCategoryConstants.YOUNG_ADULT_FICTION_NAME;
                    break;
                case ESPCategoryConstants.YOUNG_ADULT_NON_FICTION_CODE:
                    retVal = ESPCategoryConstants.YOUNG_ADULT_NON_FICTION_NAME;
                    break;
                default:
                    retVal = ESPCategoryConstants.LEGACY_NAME;
                    break;
            }

            return retVal;
        }

        public static List<ProductInfo> GetNRCProductInfo(List<string> btKeys)
        {
            var result = new List<ProductInfo>();

            try
            {
                var response = NoSqlProductsService.Instance.GetNewReleaseCalendarProducts(btKeys);
                if (response != null)
                {
                    if (response.Status == NoSqlServiceStatus.Success)
                    {
                        if (response.Data != null && response.Data.Any())
                        {
                            result = response.Data;
                        }
                        else
                            Logger.Write("General", "No Product Info from MongoDb WebAPI Call. BTKeys: " + string.Join("|", btKeys.ToArray()));
                    }
                    else
                    {
                        Logger.Write("General",
                            string.Format(
                                "MongoDb WebAPI Call For Product Info {0}, Error Code: {1}, Error Message {2}",
                                response.Status,
                                response.ErrorCode, response.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.General);
            }
            return result;
        }

        public static List<ProductItem> GetNRCFeaturedProductsByFastSearch(List<string> btKeys, int maxItem, out List<string> remainingBTKeys, bool ignoreBookCheck = false)
        {
            var resultProducts = new List<ProductItem>();
            remainingBTKeys = new List<string>();

            if (btKeys == null || btKeys.Count == 0)
                return resultProducts;

            var btKeysToSearch = new List<string>();

            if (btKeys.Count > maxItem)
            {
                btKeysToSearch = btKeys.Select(x => x).Take(maxItem).ToList();
                remainingBTKeys = btKeys.Skip(maxItem).ToList();
            }
            else
                btKeysToSearch = btKeys;

            // Fast search by BTKeys
            var searchResults = ProductSearchController.SearchByIdWithoutAnyRules(btKeysToSearch);
            if (searchResults != null && searchResults.Items != null)
            {
                foreach (var fastProduct in searchResults.Items)
                {
                    var prodItem = new ProductItem();
                    var productType = fastProduct.ProductType;
                    if (ignoreBookCheck == false && productType == ProductTypeConstants.Book)
                    {
                        if (fastProduct.ProductFormat.StartsWith("eBook - Digital", StringComparison.OrdinalIgnoreCase))
                            productType = "Digital - eBook";
                        else if (fastProduct.ProductFormat.StartsWith("eAudio - Downloadable Audio", StringComparison.OrdinalIgnoreCase))
                            productType = "Digital - eAudio";
                    }

                    prodItem.BTKey = fastProduct.BTKey;
                    prodItem.ProductType = productType;
                    prodItem.Title = fastProduct.Title;
                    prodItem.ImageUrl = ContentCafeHelper.GetJacketImageUrl(fastProduct.ISBN, ImageSize.Small, fastProduct.HasJacket);
                    prodItem.Author = fastProduct.Author;

                    resultProducts.Add(prodItem);
                }
            }

            return resultProducts;
        }

        public static bool IsAuthorizeToUseAllGridCodes(string organizationId)
        {
            var organization = ProfileService.Instance.GetOrganizationById(organizationId);
            return organization != null && organization.IsAuthorizedtoUseAllGridCodes.HasValue && organization.IsAuthorizedtoUseAllGridCodes.Value;
        }

        public static string ConvertOrderStatusCodeToText(string orderStatusCode)
        {
            var statusText = orderStatusCode;

            if (!string.IsNullOrEmpty(orderStatusCode) && OrderStatusDict.ContainsKey(orderStatusCode))
                statusText = OrderStatusDict[orderStatusCode];

            return statusText;
        }
    }
}
