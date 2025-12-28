using ESPAutoRankingConsole.DataAccess;
using ESPAutoRankingConsole.DataModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ESPAutoRankingConsole.Common.Helpers
{
    public class CHQServiceHelper
    {
        public static ESPRankJsonResponse SubmitEspAutoRank(ESPRankJsonRequest submitRequest)
        {
            TextFileLogger.LogInfo(string.Format("Start SubmitEspAutoRank(ESPRankJsonRequest): {0}", submitRequest != null ? submitRequest.cartId : "<NULL>"));

            ESPRankJsonResponse espRankJsonResponse = null;

            if(submitRequest == null)
                throw new ArgumentNullException("ESPRankJsonRequest is required.");

            if (submitRequest.items == null || submitRequest.items.Count == 0)
                return null;

            var exceptionLoggingDAO = new ExceptionLoggingDAO();

            // post data
            string postData = JsonConvert.SerializeObject(submitRequest);
            TextFileLogger.LogDebug("ESP postData: " + postData);

            var result = "";

            try
            {
                // send json postData to CHQ
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(AppSettings.ESPBaseURI + "jobs/rank");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.KeepAlive = false;
                httpWebRequest.Headers.Add("espKey", AppSettings.ESPVendorKey);
                httpWebRequest.Headers.Add("api-version", AppSettings.ESPVersion);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();

                    TextFileLogger.LogDebug("ESP httpResponse: " + result);

                    espRankJsonResponse = JsonConvert.DeserializeObject<ESPRankJsonResponse>(result);

                    // log request info into [ExceptionLogging].[TS360APILog] table
                    exceptionLoggingDAO.LogRequest("SubmitEspAutoRank", postData, result, AppSettings.ESPVendorKey, "");
                }
                
            }
            catch (WebException we)
            {
                string responseText;

                if (we.Response != null)
                {
                    var responseStream = we.Response.GetResponseStream();

                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseText = reader.ReadToEnd();
                        }

                        exceptionLoggingDAO.LogRequest("SubmitEspAutoRank", postData, responseText, AppSettings.ESPVendorKey, we.Message);

                        // Set Failed status in Queue
                        var ordersDAO = new OrdersDAO();
                        ordersDAO.SetESPAutoRankStatus(submitRequest.cartId, submitRequest.items, submitRequest.userId, ESPAutoRankQueueStatus.Failed);
                    }
                }

            }
            catch (Exception ex)
            {
                exceptionLoggingDAO.LogRequest("SubmitEspAutoRank", postData, "", AppSettings.ESPVendorKey, ex.Message);

                // Set Failed status in Queue
                var ordersDAO = new OrdersDAO();
                ordersDAO.SetESPAutoRankStatus(submitRequest.cartId, submitRequest.items, submitRequest.userId, ESPAutoRankQueueStatus.Failed);

                Logger.RaiseException(ex, ExceptionCategory.SetESPAutoRankStatus);
            }

            TextFileLogger.LogInfo("End SubmitEspAutoRank(ESPRankJsonRequest)");

            return espRankJsonResponse;
        }
    }
}
