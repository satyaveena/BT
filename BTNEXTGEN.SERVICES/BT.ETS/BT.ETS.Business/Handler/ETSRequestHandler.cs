using BT.ETS.Business.Constants;
using BT.ETS.Business.DAO;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Manager;
using BT.ETS.Business.Models;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using etsModels = BT.ETS.Business.Models;

namespace BT.ETS.Business.Handler
{
    /// <summary>
    /// Realtime ETS requests handler.
    /// </summary>
    public class ETSRequestHandler
    {
        public static async Task<InsertedCartResult> InsertEtsCart(CartReceivedRequest inputData, ObjectId requestId )
        {
            List<LineItemInput> etsQueueItems = new List<LineItemInput> ();

            // Read Data from ETS QueueItems
            etsQueueItems = await CommonDAO.Instance.GetQueueLineItems(requestId);

            // further logic implement here to load etsQueueItems

            if (inputData == null || etsQueueItems == null)
                throw new BusinessException(520);
            if (string.IsNullOrEmpty(inputData.UserId))
                throw new BusinessException(105);
            if (string.IsNullOrEmpty(inputData.ESPLibraryId))
                throw new BusinessException(106);
            if (string.IsNullOrEmpty(inputData.ETSCartId))
                throw new BusinessException(107);
            if (string.IsNullOrEmpty(inputData.CartName))
                throw new BusinessException(108);

            var errorItems = new List<ErrorItem>();
            if (etsQueueItems != null && etsQueueItems.Any())
            {
                foreach (var item in etsQueueItems)
                {
                    if (string.IsNullOrEmpty(item.BTKey))
                    {
                        errorItems.Add(new ErrorItem() { BTKey = item.BTKey, Message = "BTKey is required." });
                        continue;
                    }

                    if (item.BTKey.Length != 10)
                    {
                        errorItems.Add(new ErrorItem() { BTKey = item.BTKey, Message = BusinessExceptionConstants.INVALID_ITEM });
                        continue;
                    }

                    var itemQuantity = item.Quantity;
                    var gridQuantity = 0;
                    if (item.Grids != null)
                    {
                        foreach (var grid in item.Grids)
                        {
                            gridQuantity += grid.Quantity;
                        }
                    }

                    string errorMessage = string.Empty;

                    // validate Qty
                    if (itemQuantity != gridQuantity)
                    {
                        errorMessage = "Grid quantity does not match item quantity. ";
                    }

                    // validate Ranking
                    if (item.Ranking == null)
                    {
                        errorMessage += "Ranking is required. ";
                    }
                   else
                    {
                        decimal decimalResult;
                        if (!string.IsNullOrEmpty(item.Ranking.Overall))
                        {
                            var isValidOverall = Decimal.TryParse(item.Ranking.Overall, out decimalResult);

                            if (!isValidOverall)
                            {
                                errorMessage += "Ranking Overall is not valid decimal type. ";
                            }
                        }

                        if (!string.IsNullOrEmpty(item.Ranking.Genre_Score))
                        { 
                            var isValidGenreScore = Decimal.TryParse(item.Ranking.Genre_Score, out decimalResult);

                            if (!isValidGenreScore)
                            {
                                errorMessage += "Genre_Score is not valid decimal type. ";
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(errorMessage))
                        errorItems.Add(new ErrorItem() { BTKey = item.BTKey, Message = errorMessage });
                }

                if (errorItems.Any())
                {
                    etsQueueItems.RemoveAll(x => errorItems.Any(y => y.BTKey == x.BTKey));
                }

            }
            var response = new InsertedCartResult();
            if (etsQueueItems.Any())
            {
                response = await OrderManager.Instance.InsertEtsCart(inputData.ESPLibraryId, inputData.ETSCartId, inputData.CartName,
                    inputData.CartNote, inputData.UserId, etsQueueItems);
                response.ErrorItems.AddRange(errorItems);
            }
            else
            {
                response.Carts = new List<CartResult>();
                response.ErrorItems = errorItems;
            }

            return response;
        }

        public static async Task<DupCheckResult> GetDupChecks(DupCheckRequest inputData)
        {
            if (inputData == null || inputData.Products == null || inputData.Products.Count == 0)
                throw new BusinessException(520);
            if (string.IsNullOrEmpty(inputData.UserId))
                throw new BusinessException(105);

            var dupCheckDownloadCartStatus = new List<string> { "default", "includeworders", "includewcarts" };
            var dupCheckSStatus = new List<string> { "default", "series", "none" };
            string dupCStatus = ApplicationConstants.DupCheckCStatus.FirstOrDefault(x => string.Equals(x, inputData.DupCheckC, StringComparison.OrdinalIgnoreCase));
            string dupOStatus = ApplicationConstants.DupCheckOStatus.FirstOrDefault(x => string.Equals(x, inputData.DupCheckO, StringComparison.OrdinalIgnoreCase));
            string dupHStatus = ApplicationConstants.DupCheckHStatus.FirstOrDefault(x => string.Equals(x, inputData.DupCheckH, StringComparison.OrdinalIgnoreCase));
            string dupDownloadCartStatus = dupCheckDownloadCartStatus.FirstOrDefault(x => string.Equals(x, inputData.DupCheckDownloadCart, StringComparison.OrdinalIgnoreCase));
            string dupSStatus = dupCheckSStatus.FirstOrDefault(x => string.Equals(x, inputData.DupCheckS, StringComparison.OrdinalIgnoreCase));
            if (dupCStatus == null || dupOStatus == null || dupHStatus == null || dupDownloadCartStatus == null || dupSStatus == null)
                throw new BusinessException(211);

            var response = new DupCheckDataResult();
            response = await OrderManager.Instance.GetDupChecks(inputData.UserId, inputData.Products, inputData.DupCheckC, inputData.DupCheckDownloadCart, inputData.DupCheckH);
            if (response != null)
            {
                if (response != null && response.DupCheckResult.Items != null && response.DupCheckResult.Items.Any() && !string.Equals(inputData.DupCheckO, "none", StringComparison.OrdinalIgnoreCase))
                {
                    etsModels.OrdersDupCheckRequest dupCheckRequestObj = new etsModels.OrdersDupCheckRequest();
                    dupCheckRequestObj.UserId = inputData.UserId;
                    dupCheckRequestObj.OrgId = response.OrgId;

                    if (string.Equals(inputData.DupCheckO, "default", StringComparison.OrdinalIgnoreCase))
                        dupCheckRequestObj.OrderCheckType = response.OrdersDupeCheckType;
                    else
                        dupCheckRequestObj.OrderCheckType = inputData.DupCheckO;

                    if (string.Equals(inputData.DupCheckDownloadCart, "default", StringComparison.OrdinalIgnoreCase))
                        dupCheckRequestObj.DownloadedCheckType = response.DupCheckPreferenceDownloadCart;
                    else
                        dupCheckRequestObj.DownloadedCheckType = inputData.DupCheckDownloadCart;

                    dupCheckRequestObj.CartCheckType = inputData.DupCheckC;
                    dupCheckRequestObj.BasketId = null;
                    dupCheckRequestObj.BTKeys = response.DupCheckResult.Items.Select(x => x.BTKey).ToList();

                    var dupOGetOrderDuplicatesResponse = await OrderManager.Instance.GetOrderServiceDuplicates(dupCheckRequestObj);

                    if (dupOGetOrderDuplicatesResponse != null)
                    {
                        foreach(var dupOrder in dupOGetOrderDuplicatesResponse.DuplicateItems)
                        {
                            var dupeItem = response.DupCheckResult.Items.Where(x => x.BTKey == dupOrder.BTKey && dupOrder.IsDuplicated).FirstOrDefault();
                            if (dupeItem != null)
                            {
                                if (dupeItem.DupCheckStatus == null)
                                {
                                    dupeItem.DupCheckStatus = new List<string>();
                                }

                                dupeItem.DupCheckStatus.Add("O");
                            }
                        }
                    }
                }
                
                var duplicateSeriesType = "";
                if (string.Equals(inputData.DupCheckS, "default", StringComparison.OrdinalIgnoreCase))
                    duplicateSeriesType = response.SeriesDupeCheckType;
                else
                    duplicateSeriesType = inputData.DupCheckS;

                if (response != null && response.DupCheckResult.Items != null && response.DupCheckResult.Items.Any() && string.Equals(duplicateSeriesType, "series", StringComparison.OrdinalIgnoreCase))
                {
                    var seriesHelper = SeriesHelper.GetInstance();
                    var seriesDuplicateTitleResponse = seriesHelper.GetDuplicateSeriesTitle(response.DupCheckResult.Items.Select(x => x.BTKey).ToList(), response.OrgId);
                    if (seriesDuplicateTitleResponse != null)
                    {
                        foreach (var seriesDupe in seriesDuplicateTitleResponse.SeriesDuplicateTitleList)
                        {
                            if (!string.IsNullOrEmpty(seriesDupe.DuplicateProfiledSeriesIdList))
                            {
                                var dupeItem = response.DupCheckResult.Items.Where(x => x.BTKey == seriesDupe.BTKey).FirstOrDefault();
                                if (dupeItem != null)
                                {
                                    if (dupeItem.DupCheckStatus == null)
                                    {
                                        dupeItem.DupCheckStatus = new List<string>();
                                    }

                                    dupeItem.DupCheckStatus.Add("S");
                                }
                            }
                        }
                    }
                }
            }

            return response.DupCheckResult;
        }

        public static async Task<ProductPricingResult> GetProductPricing(PricingRequest inputData)
        {
            if (inputData == null || inputData.BTKeys == null || inputData.BTKeys.Count == 0)
                throw new BusinessException(520);

            if (string.IsNullOrEmpty(inputData.UserId))
                throw new BusinessException(105);

            ProductPricingResult ppr = new ProductPricingResult();
            int MaxPricingBatchSize = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.MaxPricingBatchSize);

            if (inputData.BTKeys.Count <= MaxPricingBatchSize)
            {
                ppr = await GetProductPricingByBatch(inputData);
            }
            else
            {
                double loopCount = Math.Ceiling((double)inputData.BTKeys.Count / MaxPricingBatchSize);

                for (int i = 0; i < loopCount; i++)
                {
                    PricingRequest pricingRequest = new PricingRequest();
                    pricingRequest.BTKeys = inputData.BTKeys.Skip(i * MaxPricingBatchSize).Take(MaxPricingBatchSize).ToList();
                    pricingRequest.UserId = inputData.UserId;
                    var tempResult = await GetProductPricingByBatch(pricingRequest);

                    if (i == 0)
                        ppr = tempResult;
                    else
                    {
                        if (tempResult.ErrorItems != null)
                            ppr.ErrorItems.AddRange(tempResult.ErrorItems);
                        if (tempResult.Products != null)
                            ppr.Products.AddRange(tempResult.Products);
                    }

                }
            }           

            return ppr;
        }

        private static async Task<ProductPricingResult> GetProductPricingByBatch(PricingRequest pricingReqBatch)
        {
            ProductPricingResult pprTempResultByBatch = new ProductPricingResult();

            var errorItems = new List<ErrorItem>();
            foreach (var item in pricingReqBatch.BTKeys)
            {
                if (item.Length != 10)
                {
                    errorItems.Add(new ErrorItem() { BTKey = item, Message = BusinessExceptionConstants.INVALID_ITEM });
                    continue;
                }
            }
            if (errorItems.Any())
                pricingReqBatch.BTKeys.RemoveAll(x => errorItems.Any(y => y.BTKey == x));

            
            if (pricingReqBatch.BTKeys.Any())
            {
                PricingRequests pr = await OrderManager.Instance.GetProductPricingPreferences(pricingReqBatch.UserId, pricingReqBatch.BTKeys);
                if (pr.ErrorItems != null)
                    pprTempResultByBatch.ErrorItems = pr.ErrorItems;
                pprTempResultByBatch.ErrorItems.AddRange(errorItems);

                SearchRequest request = pr.SearchRequest;
                request.SearchArguments = CreateSearchArguments(pricingReqBatch.BTKeys);
                List<PricingReturn4ClientObject> pricingObjects = await RetrievePricingFromServiceApi(request);

                if (pprTempResultByBatch.Products == null)
                    pprTempResultByBatch.Products = new List<ProductPricing>();

                foreach (var obj in pricingObjects)
                {
                    if (!pr.ProductVasList.ContainsKey(obj.BTKey))
                        continue;

                    var productPricing = pr.ProductVasList[obj.BTKey];

                    decimal netprice;
                    if (Decimal.TryParse(obj.Price, NumberStyles.Currency, CultureInfo.CurrentCulture, out netprice))
                        productPricing.NetPrice = netprice;
                    pprTempResultByBatch.Products.Add(productPricing);
                }
            }
            else
            {
                pprTempResultByBatch.ErrorItems.AddRange(errorItems);
            }

            return pprTempResultByBatch;
        }

        private static SearchArguments CreateSearchArguments(List<string> inputData)
        {
            var group = new SearchExpressionGroup();
            group.AddSearchExpress(new SearchExpression
            {
                Operator = BooleanOperatorConstants.And,
                Scope = SearchFieldNameConstants.btkey,
                Terms = string.Join("|", inputData.ToArray())
            });

            var argument = new SearchArguments();
            argument.SearchExpressionGroup.AddSearchExpress(group);
            argument.SortExpressions.Add(new SortExpression { SortField = string.Empty });
            argument.StartRowIndex = 0;
            argument.PageSize = inputData.Count;

            return argument;
        }

        private static async Task<List<PricingReturn4ClientObject>> RetrievePricingFromServiceApi(SearchRequest request)
        {
            var serializer = new JavaScriptSerializer();

            var servicesApiUrl = new Uri(AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.AppSearchServiceURL) + @"/GetPricingAndPromoForSearch");
            int timeout = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.WebRequestTimeOutInSec, 120);
            string jsonRequest = serializer.Serialize(request);

            FileLogger.LogDebug(string.Format("Service API Pricing Request: {0}.", jsonRequest));

            AppServiceResult<WCFObjectReturnToClient> res = null;
            string retryWaitTimes = AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.SearchServiceRetryValues);
            var retryWaitTimesList = retryWaitTimes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            int retryCount = 0;
            var maxRetries = retryWaitTimesList.Count;
            while (retryCount < maxRetries)
            {

                try
                {
                    using (var client = new ExtendedWebClient(servicesApiUrl, timeout))
                    {
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";

                        var response = await client.UploadStringTaskAsyncEx(servicesApiUrl.AbsoluteUri, "POST", jsonRequest);

                        FileLogger.LogDebug(string.Format("Service API Pricing Response: {0}.", response));

                        res = serializer.Deserialize<AppServiceResult<WCFObjectReturnToClient>>(response);
                    }
                    break;
                }
                catch (Exception)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }
                }

            }

            if (res == null)
            {
                var msg = string.Format("RetrievePricingFromServiceApi(), ~/GetPricingAndPromoForSearch return NULL. Request='{0}'", request.ToString());
                throw new Exception(msg);
            }

            return (res.Status == 0) ? res.Data.Pricing : null;
        }
    }
}
