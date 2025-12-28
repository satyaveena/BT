using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BT.TS360API.ExternalServices.NoSqlAPI
{
    public class NoSqlProductsService
    {
        private static volatile NoSqlProductsService _instance;
        private static readonly object SyncRoot = new Object();

        private NoSqlProductsService()
        {
        }

        public static NoSqlProductsService Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new NoSqlProductsService();
                }

                return _instance;
            }
        }

        public NoSqlServiceResult<List<ProductInfo>> GetNewReleaseCalendarProducts(List<string> btKeys)
        {
            var request = new NRCProductInfoRequest();
            request.BTKeys = btKeys;

            var productInfoUrl = new Uri(AppSettings.NoSQLApiUrlNRCProductInfo);
            var response = GetNRCProductInfoResults(request, productInfoUrl);

            return response;
        }

        public NoSqlServiceResult<List<ProductInfo>> GetNRCProductInfoResults(NRCProductInfoRequest request, Uri webApiUri)
        {
            // Create a WebClient to POST the request
            using (var client = new WebClient())
            {
                // Set the header so it knows we are sending JSON
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                var jss = new JavaScriptSerializer();
                // Serialise the data we are sending in to JSON
                string serialisedData = jss.Serialize(request);

                // Make the request
                var response = client.UploadString(webApiUri, serialisedData);

                // Deserialise the response into a GUID
                return jss.Deserialize<NoSqlServiceResult<List<ProductInfo>>>(response);
            }
        }
    }
}
