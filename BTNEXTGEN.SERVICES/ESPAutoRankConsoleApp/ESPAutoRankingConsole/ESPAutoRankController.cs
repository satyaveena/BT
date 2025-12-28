using Elmah;
using ESPAutoRankingConsole.Common;
using ESPAutoRankingConsole.Common.Helpers;
using ESPAutoRankingConsole.DataAccess;
using ESPAutoRankingConsole.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ESPAutoRankingConsole
{
    public class ESPAutoRankController
    {
        OrdersDAO ordersDAO = new OrdersDAO();
        public void DoEspAutoRankSubmit()
        {
            try
            {
                TextFileLogger.LogInfo("=== START DoEspAutoRankSubmit() ===");

                // 1. Get request in XML format from DAO (procESPGetAutoRankRequests)
                var theadId = AppSettings.JobId;
                var espAutoRankRequests = ordersDAO.ESPGetAutoRankRequests(AppSettings.MaxBasketCount, theadId);
                TextFileLogger.LogInfo("Completed OrdersDAO.ESPGetAutoRankRequests()");

                // 2. Deserialize XML to list of requests
                var espRankJsonRequests = DeserializeXmlEspAutoRankRequests(espAutoRankRequests);

                // 3. Loop and submit request to CHQ service
                if (espRankJsonRequests != null)
                {
                    TextFileLogger.LogInfo("START submitting all ESPAutoRankRequests to CHQService");

                    foreach (var espRequest in espRankJsonRequests)
                    {
                        // submit
                        var rankResponse = CHQServiceHelper.SubmitEspAutoRank(espRequest);

                        if (rankResponse != null)
                        {
                            var cartId = espRequest.cartId.TrimStart(CommonConstants.CART_ID_AUTORANK_PREFIX.ToArray());
                            // change queue status to Submitted
                            ordersDAO.SetESPAutoRankStatus(cartId, espRequest.items, espRequest.userId, ESPAutoRankQueueStatus.Submitted);

                            TextFileLogger.LogInfo(string.Format("Completed OrdersDAO.SetESPAutoRankStatus('{0}', '{1}', '{2}'", cartId, espRequest.userId, ESPAutoRankQueueStatus.Submitted));
                        }

                        // Pause for awhile
                        Thread.Sleep(100);
                    }

                    TextFileLogger.LogInfo("END to submit ESPAutoRankRequests to CHQService");
                }

                TextFileLogger.LogInfo("=== END DoEspAutoRankSubmit() ===");

            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.SetESPAutoRankStatus);
            }
        }

        /// <summary>
        /// Deserialize Xml EspAutoRankRequests.
        /// </summary>
        /// <param name="xmlRequests"></param>
        /// <returns></returns>
        private List<ESPRankJsonRequest> DeserializeXmlEspAutoRankRequests(XmlDocument xmlRequests)
        {
            var results = new List<ESPRankJsonRequest>();

            if (xmlRequests != null && xmlRequests.DocumentElement != null)
            {
                TextFileLogger.LogInfo("Deserialize XML to AutoRankRequests object");
                TextFileLogger.LogDebug("ESPAutoRankRequests XML: " + xmlRequests.InnerXml);
                
                // deserialize XML to AutoRankRequests object
                AutoRankRequests autoRankRequests = null;
                using (TextReader textReader = new StringReader(xmlRequests.InnerXml))
                {
                    using (var reader = new XmlTextReader((textReader)))
                    {
                        reader.Namespaces = false;
                        var serializer = new XmlSerializer(typeof(AutoRankRequests));
                        autoRankRequests = (AutoRankRequests)serializer.Deserialize(reader);
                    }
                }

                // convert to ESPRankJsonRequests
                TextFileLogger.LogInfo("Convert to ESPRankJsonRequests");
                if (autoRankRequests != null && autoRankRequests.Requests != null)
                {
                    foreach (var basketRequest in autoRankRequests.Requests)
                    {
                        // convert user basket
                        var espRankJsonRequest = new ESPRankJsonRequest
                        {
                            espLibraryId = basketRequest.ESPLibraryID,
                            cartId = basketRequest.CartID,
                            userName = basketRequest.UserName,
                            cartName = basketRequest.BasketName,
                            userId = basketRequest.UserGuid
                        };

                        // convert LineItems
                        if (basketRequest.Detail != null && basketRequest.Detail.LineItems != null)
                        {
                            espRankJsonRequest.items = new List<ESPRankItemJsonRequest>();

                            foreach (var requestItem in basketRequest.Detail.LineItems)
                            {
                                var espRankItemJsonRequest = new ESPRankItemJsonRequest
                                {
                                    lineItemId = requestItem.LineItemID,
                                    vendorId = requestItem.BTKey,
                                    bisac = requestItem.Bisac,
                                    // new in Release 4.2
                                    listPrice = requestItem.ListPrice,
                                    discountedPrice = requestItem.DiscountedPrice
                                };
                                
                                espRankJsonRequest.items.Add(espRankItemJsonRequest);
                            }
                        }

                        // add to results
                        results.Add(espRankJsonRequest);
                    }
                }
            }

            return results;
        }
    }
}
