using BT.TS360API.ExternalDataSendService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace BT.TS360API.ExternalDataSendService.Helpers
{
    public class ApiServiceHelper
    {
        public static string GetApiAccessToken(ApiAccessToken requestInfo)
        {
            var accessToken = string.Empty;

            if (requestInfo != null && !string.IsNullOrWhiteSpace(requestInfo.ClientId)
                && !string.IsNullOrWhiteSpace(requestInfo.ClientSecret) && !string.IsNullOrWhiteSpace(requestInfo.EndpointUrl))
            {
                var sspUser = new JObject();
                sspUser.Add("UserName", requestInfo.ClientId);
                sspUser.Add("Password", requestInfo.ClientSecret);

                var response = UploadString(requestInfo.EndpointUrl, "POST", sspUser.ToString(), null);
                var token = JsonConvert.DeserializeObject<AuthToken>(response);
                if (token != null)
                {
                    accessToken = token.Token;
                }
            }

            return accessToken;
        }

        public static string UploadString(string urlAddress, string method, string bodyJsonString, Dictionary<string, string> headers)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            // create a WebClient to POST the request
            using (WebClient client = new WebClient())
            {
                client.Headers.Remove(HttpRequestHeader.ContentType);
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                if (headers != null && headers.Keys != null && headers.Keys.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in headers)
                    {
                        client.Headers.Remove(item.Key);
                        client.Headers.Add(item.Key, item.Value);
                    }
                }

                // call
                var response = client.UploadString(urlAddress, method, bodyJsonString);

                return response;
            }
        }
    }
}