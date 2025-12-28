using BT.ETS.Business.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Helper
{
    public class HTTP
    {
        public async static Task<T> Post<T>(string URL, string RequestData, Dictionary<string, string> keyValuePairs = null)
        {
            dynamic responseObj = null;
            string address = string.Format(URL);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                if (keyValuePairs != null)
                {
                    foreach (var item in keyValuePairs)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(RequestData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(address, content);

                if (response.IsSuccessStatusCode)
                {
                    var stringRes = await response.Content.ReadAsStringAsync();
                    responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(stringRes);
                }
            }

            return responseObj;
        }

        public async static Task<string> Post(string URL, string RequestData, Dictionary<string, string> keyValuePairs = null)
        {
            string stringRes = string.Empty;

            string address = string.Format(URL);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                if (keyValuePairs != null)
                {
                    foreach (var item in keyValuePairs)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(RequestData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(address, content);

                if (response.IsSuccessStatusCode)
                {
                    stringRes = await response.Content.ReadAsStringAsync();
                }
            }

            return stringRes;
        }


        public static EtsServiceResult<string> PostQueue(string URL, string RequestData, Dictionary<string, string> keyValuePairs = null)
        {
            // send json postData to CHQ
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = false;

            var result = "";

            var etsResult = new EtsServiceResult<string>();

            if (keyValuePairs != null)
            {
                foreach (var item in keyValuePairs)
                {
                    httpWebRequest.Headers.Add(item.Key, item.Value);
                }
            }

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(RequestData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse httpResponse = null;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (WebException webEx)
            {
                if (webEx.Status == WebExceptionStatus.ProtocolError && webEx.Response != null)
                {
                    using (var streamReader = new StreamReader(webEx.Response.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                        TextFileLogger.LogDebug(" ETS PostQueue ProtocolError Response Text: " + result);
                        etsResult = JsonConvert.DeserializeObject<EtsServiceResult<string>>(result);
                    }
                }
                else
                    throw;
            }

            if (httpResponse == null)
                return etsResult;

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
                TextFileLogger.LogDebug(" ETS PostQueue Response Text: " + result);
                etsResult = JsonConvert.DeserializeObject<EtsServiceResult<string>>(result);
            }
            return etsResult;
        }
    }
}
