using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using BT.TS360API.Cache;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Axis360;
using AppSettings = BT.TS360API.Common.Configrations.AppSettings;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using BT.TS360API.ServiceContracts.Request;


namespace BT.TS360API.Common.Helpers
{
    public class Axis360InventoryHelper
    {

        #region Members
        private static string VendorUserName
        {
            get
            {
                return AppSettings.Axis360VendorUserName;
            }
        }
        private static string VendorPassword
        {
            get
            {
                return AppSettings.Axis360VendorPassword;
            }
        }
        private static string LibraryId
        {
            get
            {
                return AppSettings.Axis360LibraryId;
            }
        }

        private static string Axis360ApiURL
        {
            get
            {
                return AppSettings.Axis360ApiURL;
            }
        }

        private static string NoSqlAxis360InventoryUrl
        {
            get
            {
                return AppSettings.NoSqlAxis360InventoryUrl;
            }
        }
        #endregion

        private async static Task<Axis360Token> GetAccessToken(bool getNewToken)
        {
            Axis360Token token;
            DateTime now = DateTime.Now;
            if (!getNewToken)
            {
                token = CachingController.Instance.Read("Axis360Token") as Axis360Token;
                if (token != null && !string.IsNullOrWhiteSpace(token.AccessValue))
                {
                    var tokenDurationInHours = TimeSpan.FromSeconds(token.TokenDurationInSeconds).Hours - 1;
                    if (token.CreatedDate > now.AddHours(-tokenDurationInHours))
                    {
                        return token;
                    }
                }
            }

            var authorizationString = string.Format("{0}:{1}:{2}", VendorUserName, VendorPassword, LibraryId);
            var encodedAuthorizationString = Convert.ToBase64String(Encoding.Unicode.GetBytes(authorizationString));
            NameValueCollection parameters = new NameValueCollection();
            var jss = new JavaScriptSerializer();
            using(HttpClient client = new HttpClient())
            {
                token = new Axis360Token();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", encodedAuthorizationString);
                var response = await client.PostAsJsonAsync(Axis360ApiURL + "/accesstoken", parameters);
                if (response.IsSuccessStatusCode)
                {
                    var stringRes = await response.Content.ReadAsStringAsync();
                    var data = jss.Deserialize<IDictionary<string, string>>(stringRes);

                    if (data.ContainsKey("access_token"))
                        token.AccessValue = data["access_token"];
                    var tokenDuration = 0;
                    if (data.ContainsKey("expires_in"))
                        int.TryParse(data["expires_in"], out tokenDuration);
                    token.TokenDurationInSeconds = tokenDuration;
                    if (data.ContainsKey("token_type"))
                        token.TokenType = data["token_type"];
                    token.CreatedDate = now;
                    CachingController.Instance.Write("Axis360Token", token, 360);
                }
            }

            return token;

        }

        public async static Task<Axis360InventoryResponse> GetCirculationByISBN(string isbn, string customerID)
        {
            var axis360InventoryResponse = new Axis360InventoryResponse();
            var jss = new JavaScriptSerializer();
            string address = string.Format(Axis360ApiURL + "/getlicenseinformation/?ISBN={0}&customerID={1}", isbn, customerID);

            var accessToken = await GetAccessToken(false);
            if (accessToken != null && !string.IsNullOrWhiteSpace(accessToken.AccessValue))
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", accessToken.AccessValue);
                    client.DefaultRequestHeaders.Add("Library", "nolibraryId");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(address);
                    if(response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        accessToken = await GetAccessToken(true);
                        if(accessToken != null && !string.IsNullOrWhiteSpace(accessToken.AccessValue))
                        {
                            response = await client.GetAsync(address);
                        }
                    }

                    if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        var stringRes = await response.Content.ReadAsStringAsync();
                        axis360InventoryResponse = jss.Deserialize<Axis360InventoryResponse>(stringRes);

                    }
                }
                if (axis360InventoryResponse != null && axis360InventoryResponse.Collection != null)
                {
                    axis360InventoryResponse = BindAxis360InventoryData(axis360InventoryResponse);
                }
            }
            return axis360InventoryResponse;

        }

        private static Axis360InventoryResponse BindAxis360InventoryData(Axis360InventoryResponse axis360InventoryResponse)
        {
            var hasMeteredData = false;
            var purchaseOptions = SiteTermHelper.Instance.GetSiteTemByName("EBookPurchaseOptions");
            foreach(var collection in axis360InventoryResponse.Collection)
            {
                if (string.Equals(collection.CirculationType, "Metered", StringComparison.OrdinalIgnoreCase))
                {
                    hasMeteredData = true;
                }
                collection.ExpirationDateString = collection.ExpirationDate.HasValue ? collection.ExpirationDate.Value.ToString("MM/dd/yyyy") : "";
                collection.LastCheckoutDateString = collection.LastCheckoutDate.HasValue ? collection.LastCheckoutDate.Value.ToString("MM/dd/yyyy") : "";
                if(purchaseOptions != null && collection.ODSPurchaseOptionLiteral != null)
                {
                    var purchaseOptionLiteral = (from purchaseOption in purchaseOptions
                              where string.Equals(purchaseOption.ItemKey, collection.ODSPurchaseOptionLiteral, StringComparison.OrdinalIgnoreCase)
                                          select purchaseOption.ItemValue).ToList().FirstOrDefault();

                    if (!string.IsNullOrEmpty(purchaseOptionLiteral))
                    {
                        collection.LendingModel = purchaseOptionLiteral;
                    }
                    else
                    {
                        collection.LendingModel = collection.ODSPurchaseOptionLiteral;
                    }
                }
            }

            axis360InventoryResponse.CirculationType = hasMeteredData ? "M" : "P";
            return axis360InventoryResponse;
        }

        public async static Task<Axis360CheckInventoryResponse> CheckForCirculationByISBN(List<string> isbnList, string eSupplierAccountNumber)
        {
            var axis360CheckInventoryResponse = new Axis360CheckInventoryResponse();
            var noSqlServiceResult = new NoSqlServiceResult<Axis360CheckInventoryResponse>();
            var jss = new JavaScriptSerializer();
            string address = NoSqlAxis360InventoryUrl;
            var axis360CheckInventoryRequest = new Axis360CheckInventoryRequest();
            axis360CheckInventoryRequest.Axis360CustomerID = eSupplierAccountNumber;
            axis360CheckInventoryRequest.ISBNList = isbnList;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync(address, axis360CheckInventoryRequest);
                if (response.IsSuccessStatusCode)
                {
                    var stringRes = await response.Content.ReadAsStringAsync();
                    noSqlServiceResult = jss.Deserialize<NoSqlServiceResult<Axis360CheckInventoryResponse>>(stringRes);

                }
            }
            if (noSqlServiceResult!= null && noSqlServiceResult.Data != null)
            {
                axis360CheckInventoryResponse = noSqlServiceResult.Data;
            }
            return axis360CheckInventoryResponse;
        }
    }

}
