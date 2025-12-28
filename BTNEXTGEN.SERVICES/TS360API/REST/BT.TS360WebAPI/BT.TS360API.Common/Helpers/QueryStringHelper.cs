using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using BT.TS360Constants;
using Encoder = Microsoft.Security.Application.Encoder;

namespace BT.TS360API.Common.Helpers
{
    /// <summary>
    /// Not yet refactored
    /// </summary>
    public static class QueryStringHelper
    {
        public static string BuildQueryString(NameValueCollection queryParameters)
        {
            if (queryParameters == null)
                return string.Empty;

            const char equals = '=';
            const char ampersand = '&';
            const char questionMark = '?';
            int keyCount = queryParameters.Keys.Count;
            int current = 1;

            var builder = new StringBuilder();
            builder.Append(questionMark);

            foreach (string key in queryParameters.Keys)
            {
                string val = queryParameters[key];
                if (!String.IsNullOrEmpty(val))
                {
                    builder.Append(key);
                    builder.Append(@equals);
                    builder.Append(Encoder.UrlEncode(val));
                    if (current < keyCount)
                    {
                        builder.Append(ampersand);
                        current++;
                    }
                }
            }

            string result = builder.ToString();
            result = result.TrimEnd(ampersand);
            return result;
        }

        /// <summary>
        /// This method updates/adds the specified parameter into the query string
        /// </summary>
        /// <param name="queryString">The search parameters</param>
        /// <param name="key">The parameter key</param>
        /// <param name="value">The parameter value</param>
        /// <returns>An query string with the updated search parameters.</returns>
        public static string UpdateQueryString(NameValueCollection queryString, string key, string value)
        {
            // Create a copy of the current query string
            var updatedParameters = (queryString == null
                                        ? new NameValueCollection()
                                        : new NameValueCollection(queryString));

            if (updatedParameters[key] == null)
            {
                updatedParameters.Add(key, value);
            }
            else
            {
                updatedParameters[key] = value;
            }

            // Generate and return the new query string.
            return BuildQueryString(updatedParameters);
        }

        public static NameValueCollection UpdateQueryStringValue(NameValueCollection queryString, string key, string value)
        {
            // Create a copy of the current query string
            var updatedParameters = (queryString == null
                                        ? new NameValueCollection()
                                        : new NameValueCollection(queryString));

            if (updatedParameters[key] == null)
            {
                updatedParameters.Add(key, value);
            }
            else
            {
                updatedParameters[key] = value;
            }

            // Generate and return the new query string.
            return updatedParameters;
        }

        public static string UpdateQueryString(NameValueCollection queryString, NameValueCollection viewProperties, List<string> parametersToRemove)
        {
            // Create a copy of the current query string
            var updatedParameters = (queryString == null
                                        ? new NameValueCollection()
                                        : new NameValueCollection(queryString));

            if (viewProperties != null)
            {
                // Iterate through the view properties, checking the query string each time.
                foreach (string viewProperty in viewProperties.Keys)
                {
                    string propertyValue = updatedParameters[viewProperty];

                    // If the query string did not contain the property, add it.
                    if (String.IsNullOrEmpty(propertyValue))
                    {
                        updatedParameters.Add(viewProperty, viewProperties[viewProperty]);
                    }
                    else
                    {
                        // If the query string did contain our property, ensure that it has the
                        // correct value.
                        updatedParameters[viewProperty] = viewProperties[viewProperty];
                    }
                }
            }

            if (parametersToRemove != null)
            {
                foreach (var parameterToRemove in parametersToRemove)
                {
                    updatedParameters.Remove(parameterToRemove);
                }
            }

            // Generate and return the new query string.
            return BuildQueryString(updatedParameters);
        }

        public static void RemoveQueryStringItem(NameValueCollection collection, string removeKey)
        {
            // reflect to readonly property
            var isreadonly = typeof(NameValueCollection).GetProperty("IsReadOnly",
                                                                     BindingFlags.Instance | BindingFlags.NonPublic);

            // make collection editable
            isreadonly.SetValue(collection, false, null);

            // remove
            collection.Remove(removeKey);
        }

        public static string GetFacetPathFromQueryString(string queryString)
        {
            string facetPath = String.Empty;
            string[] split = queryString.Split('?');
            if (split.Length > 0)
            {
                string[] queryItems = split[0].Split('&');
                for (int i = 0; i < queryItems.Length; i++)
                {
                    string[] operands = queryItems[i].Split('=');
                    if (operands.Length == 2)
                    {
                        string leftOp = operands[0];
                        if (leftOp.Contains(QueryStringName.CART_SEARCH_NOTE))
                        {
                            facetPath += queryItems[i];
                            //if (i != queryItems.Length - 1)
                            //{
                            facetPath += ';';
                            //}
                        }
                        else if (leftOp.Contains(QueryStringName.CART_SEARCH_QUANTITY))
                        {
                            facetPath += queryItems[i];
                            //if (i != queryItems.Length - 1)
                            //{
                            facetPath += ';';
                            //}
                        }
                        else if (leftOp.Contains(QueryStringName.CART_SEARCH_FORMAT))
                        {
                            facetPath += queryItems[i];
                            //if (i != queryItems.Length - 1)
                            //{
                            facetPath += ';';
                            //}
                        }
                        else if (leftOp.Contains(QueryStringName.CART_SEARCH_INVENTORY))
                        {
                            facetPath += queryItems[i];
                            //if (i != queryItems.Length - 1)
                            //{
                            facetPath += ';';
                            //}
                        }
                        else if (queryItems[i].Contains(BasketLineItemFacet.OriginalEntry))
                        {
                            facetPath += queryItems[i];
                            //if (i != queryItems.Length - 1)
                            //{
                            facetPath += ';';
                            //}
                        }
                        else if (leftOp.ToLower().Contains(BasketLineItemFacet.Grids.ToLower()))
                        {
                            facetPath += queryItems[i];
                            facetPath += ';';
                        }
                    }
                }
            }
            return HttpUtility.UrlDecode(facetPath);
        }

        public static string GetParamFromQueryString(string param, string queryString)
        {
            string[] queries = queryString.Split('&');
            if (queries.Length > 0)
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    string[] split = queries[i].Split('=');
                    if (split.Length > 1 && string.Compare(split[0], param, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return split[1];
                    }
                }
            }
            return String.Empty;
        }

        public static string ConvertToSortDirection(string direction)
        {
            if (!string.IsNullOrEmpty(direction) && direction == "Descending")
                return "DESC";
            return "ASC";
        }
        public static int ConvertToSortOrder(string sortBy)
        {
            int order = 0;
            if (string.IsNullOrEmpty(sortBy)) return order;
            switch (sortBy.ToLower())
            {
                case SearchResultsSortField.SORT_ARTIST:
                    order = 0;
                    break;
                case SearchResultsSortField.SORT_TITLE:
                    order = 1;
                    break;
                case SearchResultsSortField.SORT_POPULARITY:
                    order = 2;
                    break;
                case SearchResultsSortField.SORT_AUTHOR:
                    order = 3;
                    break;
                case SearchResultsSortField.SORT_PUBLISHER:
                    order = 4;
                    break;
                case SearchResultsSortField.SORT_ADD_TO_CART_DATE:
                    order = 5;
                    break;
                case SearchResultsSortField.SORT_PUBDATE:
                    order = 6;
                    break;
                case SearchResultsSortField.SORT_QUANTITY:
                    order = 7;
                    break;
                case SearchResultsSortField.SORT_FORMAT:
                    order = 8;
                    break;
                case SearchResultsSortField.SORT_LISTPRICE:
                    order = 9;
                    break;
                case SearchResultsSortField.SORT_ISBN:
                    order = 10;
                    break;
                case SearchResultsSortField.SORT_LCC_CLASS_AUTHOR:
                    order = 11;
                    break;
                case SearchResultsSortField.SORT_LCC_CLASS_ARTIST:
                    order = 12;
                    break;
                case SearchResultsSortField.SORT_DEWEY_AUTHOR:
                    order = 13;
                    break;
                case SearchResultsSortField.SORT_DEWEY_ARTIST:
                    order = 14;
                    break;
                case SearchResultsSortField.SORT_ESP_OVERALL_SCORE:
                    order = 15;
                    break;
                case SearchResultsSortField.SORT_ESP_BISAC_SCORE:
                    order = 16;
                    break;
                default:
                    order = 0;
                    break;
            }
            return order;
        }
    }
}
