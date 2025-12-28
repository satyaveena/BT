using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.Constants;
using BT.TS360API.WebAPI.Common.DataAccess;
using BT.TS360API.WebAPI.ServiceReferenceUserAlerts;
using BT.TS360API.WebAPI.Models;

using Newtonsoft.Json;

namespace BT.TS360API.WebAPI.Services
{
    public class ESPSubmitRepository
    {

        static OrdersDAO ordersDAO = new OrdersDAO();
        static ExceptionLoggingDAO logger = new ExceptionLoggingDAO();

        public ESPServiceResult<ESPServiceResponse> Submit(ESPServiceRequest espServiceRequest)
        {
            var espServiceResult = new ESPServiceResult<ESPServiceResponse> { Status = ESPServiceStatus.Success };

            string userRequestJson = JsonConvert.SerializeObject(espServiceRequest);

            try
            {
                ESPServiceResponse espServiceResponse = new ESPServiceResponse();

                string error = Validate(espServiceRequest);
                if (!string.IsNullOrEmpty(error))
                {
                    espServiceResult = HandleFailedESPServiceResult(error, "Submit", userRequestJson);
                    return espServiceResult;
                }

                string sqlError = ordersDAO.ProcessESPSubmitRequest(espServiceRequest.CartId, espServiceRequest.UserId, espServiceRequest.ESPType, espServiceRequest.ShowJobURL);
                if (!string.IsNullOrEmpty(sqlError))
                {
                    espServiceResult = HandleFailedESPServiceResult(sqlError, "Submit", userRequestJson);
                    return espServiceResult;
                }

                if (espServiceRequest.ESPType == RankConstant.ESPType.DIST)
                {
                    string message = "";
                    string jobURL = "";
                    DoACartDist(espServiceRequest, out message, out jobURL);

                    espServiceResponse.ESPJobURL = jobURL; // "https://esp.collectionhq.com/espweb/50A83E9640D81BEDE050007F01003D8B";

                    if (!string.IsNullOrEmpty(message))
                    {
                        espServiceResult = HandleFailedESPServiceResult(message, "Submit", userRequestJson);
                        return espServiceResult;
                    }
                }
                else if (espServiceRequest.ESPType == RankConstant.ESPType.RANK)
                {
                    string message = "";
                    DoACartRank(espServiceRequest, out message);

                    if (!string.IsNullOrEmpty(message))
                    {
                        espServiceResult = HandleFailedESPServiceResult(message, "Submit", userRequestJson);
                        return espServiceResult;
                    }
                }

                espServiceResult.Data = espServiceResponse;

                string userResponseJson = JsonConvert.SerializeObject(espServiceResult);
                logger.LogRequest("Submit", userRequestJson, userResponseJson, "", "");
            }
            catch (Exception ex)
            {
                espServiceResult = HandleFailedESPServiceResult(ex.Message, "Submit", userRequestJson);
            
            }

            return espServiceResult;
        }

        /*private void DoRank(bool showJobURL)
        {
            // Get rank request count
            long rankRequestCount = ordersDAO.GetESPRankDataCount();

            // if rank request count > 0 then get rank request details
            if (rankRequestCount > 0)
            {
                ESPRankDataRootRequest espRankDataRootRequest = ordersDAO.GetESPRankData();

                ESPRankDataRequest[] arrEspRankDataRequest = espRankDataRootRequest.espRankDataRequests;
                // Generate the JSON
                if (arrEspRankDataRequest != null)
                {
                    foreach (ESPRankDataRequest espRankDataRequest in arrEspRankDataRequest)
                    {
                        DoACartRank(espRankDataRequest, showJobURL);

                    } // end each RankRequest
                }
            }
        }*/

        private void DoACartRank(ESPServiceRequest espServiceRequest, out string espMessage)
        {
            espMessage = "";
            string reqBasketName = string.Empty;
            string alertErrMsg;
            bool isSentErrorAlert = false;

            ESPRankDataRootRequest espRankDataRootRequest = ordersDAO.GetESPRankData(espServiceRequest.CartId);

            ESPRankDataRequest[] arrEspRankDataRequest = espRankDataRootRequest.espRankDataRequests;

            if (arrEspRankDataRequest == null || arrEspRankDataRequest.Length == 0)
            { 
                espMessage = "No Ranked Data";
                return;
            }

            ESPRankDataRequest espRankDataRequest = arrEspRankDataRequest[0];

            ESPRankJsonRequest espRankJsonRequest = new ESPRankJsonRequest();
            espRankJsonRequest.espLibraryId = espRankDataRequest.ESPLibraryID;
            espRankJsonRequest.cartId = espRankDataRequest.CartID;
            espRankJsonRequest.userName = espRankDataRequest.UserName;
            // new in 4.2
            reqBasketName = espRankDataRequest.BasketName;
            espRankJsonRequest.cartName = espRankDataRequest.BasketName;
            espRankJsonRequest.userId = espRankDataRequest.UserGuid;

            if (espRankDataRequest.Detail != null)
            {
                espRankJsonRequest.items = new List<ESPRankItemJsonRequest>();

                foreach (ESPRankDataItemRequest espRankDataItemRequest in espRankDataRequest.Detail)
                {
                    ESPRankItemJsonRequest espRankItemJsonRequest = new ESPRankItemJsonRequest();
                    espRankItemJsonRequest.lineItemId = espRankDataItemRequest.LineItemID;
                    espRankItemJsonRequest.vendorId = espRankDataItemRequest.BTKey;
                    espRankItemJsonRequest.bisac = espRankDataItemRequest.Bisac;
                    // new in Release 4.2
                    espRankItemJsonRequest.listPrice = espRankDataItemRequest.ListPrice;
                    espRankItemJsonRequest.discountedPrice = espRankDataItemRequest.DiscountedPrice;

                    espRankJsonRequest.items.Add(espRankItemJsonRequest);
                }
            }

            string output = JsonConvert.SerializeObject(espRankJsonRequest);

            // send json to CHQ

            //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://esp.test.collectionhq.co.uk/esp/jobs/rank");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(AppSetting.ESPBaseURI + "jobs/rank");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = false;
            httpWebRequest.Headers.Add("espKey", AppSetting.ESPVendorKey);
            httpWebRequest.Headers.Add("api-version", AppSetting.ESPVersion);

            //req.ContentLength = buffer.Length

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(output);
                streamWriter.Flush();
                streamWriter.Close();
            }

            ESPRankJsonResponse espRankJsonResponse = new ESPRankJsonResponse();
            var result = "";
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                    espRankJsonResponse = JsonConvert.DeserializeObject<ESPRankJsonResponse>(result);

                    logger.LogRequest("DoACartRank", output, result, AppSetting.ESPVendorKey, "");
                }

                //var result = "{\"statusCode\":0,\"statusMessage\":\"Accepted for processing\",\"jobId\":\"20009\",\"cartId\":\"{2BE9959B-B126-4E15-A580-FBA336F10DBA}\",\"cartName\":\"xxx\",\"jobUrl\":null}";

                // log the response
                // update status
                DataTable dtResponseRanked =
                    ConvertResponseRankedItemToSqlTable(espRankJsonResponse, espServiceRequest.CartId, result, "RANK", "submitted", "No Errors");
                
                ordersDAO.SetESPState(dtResponseRanked, "", "");
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

                        logger.LogRequest("DoACartRank", output, responseText, AppSetting.ESPVendorKey, we.Message);

                        espRankJsonResponse = JsonConvert.DeserializeObject<ESPRankJsonResponse>(responseText);

                        DataTable dtResponseRank =
                            ConvertResponseRankedItemToSqlTable(espRankJsonResponse, espServiceRequest.CartId, result, "RANK", "Failed", we.Message);

                        ordersDAO.SetESPState(dtResponseRank, "", "");

                        espMessage = espRankJsonResponse.statusMessage;

                        HttpWebResponse webResponse = (HttpWebResponse)we.Response;
    
                        if (webResponse.StatusCode == HttpStatusCode.NotFound)
                        {
                            isSentErrorAlert = SendUserAlert(AlertMessageTemplateIDEnum.ESPRankFailLibrary, espServiceRequest.CartId, reqBasketName, espServiceRequest.UserId, out alertErrMsg);
                        }
                        else
                        {
                            isSentErrorAlert = SendUserAlert(AlertMessageTemplateIDEnum.ESPRankFailSystem, espServiceRequest.CartId, reqBasketName, espServiceRequest.UserId, out alertErrMsg);
                        }

                        if (!isSentErrorAlert)
                        {
                            espMessage += (" " + alertErrMsg);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogRequest("DoACartRank", output, "", AppSetting.ESPVendorKey, ex.Message);

                DataTable dtResponseRanked =
                    ConvertResponseRankedItemToSqlTable(espRankJsonResponse, espServiceRequest.CartId, result, "RANK", "Failed", ex.Message);
                
                ordersDAO.SetESPState(dtResponseRanked, "", "");

                espMessage = ex.Message;

                isSentErrorAlert = SendUserAlert(AlertMessageTemplateIDEnum.ESPRankFailSystem, espServiceRequest.CartId, reqBasketName, espServiceRequest.UserId, out alertErrMsg);
                if (!isSentErrorAlert)
                {
                    espMessage += (" " + alertErrMsg);
                }

            }
            
            // handle fail response and send alert

        }

        private DataTable ConvertResponseRankedItemToSqlTable(ESPRankJsonResponse espRankJsonResponse, string basketSummaryID, string jsonResponse, string jobType, string stateType, string errorMessage)
        {
            DataTable tblESPStatus = new DataTable("ESPStatus");

            tblESPStatus.Columns.Add("BasketSummaryID", typeof(String));
            tblESPStatus.Columns.Add("ESPJobType", typeof(String));
            tblESPStatus.Columns.Add("ESPStateType", typeof(String));
            tblESPStatus.Columns.Add("ESPJobID", typeof(String));
            tblESPStatus.Columns.Add("CartStatus", typeof(String));
            tblESPStatus.Columns.Add("ErrorMessage", typeof(String));
            tblESPStatus.Columns.Add("ESPResponseJSON", typeof(String));
            //tblESPStatus.Columns.Add("JobURL", typeof(String));
            //tblESPStatus.Columns.Add("JobURLText", typeof(String));

            DataRow workRow = tblESPStatus.NewRow();

            workRow["BasketSummaryID"] = espRankJsonResponse.cartId ?? basketSummaryID;
            workRow["ESPJobType"] = jobType;
            workRow["ESPStateType"] = stateType; // submitted
            workRow["ESPJobID"] = espRankJsonResponse.jobId;
            workRow["CartStatus"] = espRankJsonResponse.statusMessage;
            workRow["ErrorMessage"] = errorMessage;
            workRow["ESPResponseJSON"] = jsonResponse;
            //workRow["JobURL"] = "";
            //workRow["JobURLText"] = "";

            tblESPStatus.Rows.Add(workRow);

            return tblESPStatus;
        }

       
        /*private void DoDistribute(bool showJobURL)
        {
            // Get distribute request count
            long rankDistributeCount = ordersDAO.GetESPDistributeDataCount();

            // if rank request count > 0 then get rank request details
            if (rankDistributeCount > 0)
            {
                ESPDistDataRootRequest espDistDataRootRequest = ordersDAO.GetESPDistributionData();

                ESPDistDataRequest[] arrESPDistDataRequest = espDistDataRootRequest.espDistDataRequests;
                // Generate the JSON
                if (arrESPDistDataRequest != null)
                {
                    foreach (ESPDistDataRequest espDistDataRequest in arrESPDistDataRequest)
                    {
                        DoACartDist(espDistDataRequest, showJobURL);

                    } // end each RankRequest
                }
            }
        }*/

        //private void DoACartDist(ESPDistDataRequest espDistDataRequest, bool showJobURL)
        private void DoACartDist(ESPServiceRequest espServiceRequest, out string espMessage, out string espJobURL)
        {
            espMessage = "";
            espJobURL = "";
            string reqBasketName = string.Empty;
            string alertErrMsg;
            bool isSentErrorAlert = false;

            ESPServiceResponse response = new ESPServiceResponse();

            ESPDistDataRootRequest espDistDataRootRequest = ordersDAO.GetESPDistributionData(espServiceRequest.CartId);

            ESPDistDataRequest[] arrESPDistDataRequest = espDistDataRootRequest.espDistDataRequests;
            // Generate the JSON

            if (arrESPDistDataRequest == null || arrESPDistDataRequest.Length == 0)
            {
                espMessage = "No Distribution Data";
                return;
            }

            ESPDistDataRequest espDistDataRequest = arrESPDistDataRequest[0];

            ESPDistJsonRequest espDistJsonRequest = new ESPDistJsonRequest();
            espDistJsonRequest.espLibraryId = espDistDataRequest.ESPLibraryID; //"610"; 
            espDistJsonRequest.cartId = espDistDataRequest.CartID;
            espDistJsonRequest.userName = espDistDataRequest.UserName;
            espDistJsonRequest.fundMonitoring = espDistDataRequest.FundMonitoring;

            // new in 4.2
            reqBasketName = espDistDataRequest.BasketName;
            espDistJsonRequest.cartName = espDistDataRequest.BasketName;
            espDistJsonRequest.userId = espDistDataRequest.UserGuid;
            espDistJsonRequest.showJobURL = espServiceRequest.ShowJobURL ? "Y" : "N";

            if (espDistDataRequest.Items != null)
            {
                espDistJsonRequest.items = new List<ESPDistItemJsonRequest>();

                foreach (ESPDistDataItemRequest espDistDataItemRequest in espDistDataRequest.Items)
                {
                    ESPDistItemJsonRequest espDistItemJsonRequest = new ESPDistItemJsonRequest();
                    espDistItemJsonRequest.lineItemId = espDistDataItemRequest.LineItemID;
                    espDistItemJsonRequest.vendorId = espDistDataItemRequest.BTKey;
                    espDistItemJsonRequest.bisac = espDistDataItemRequest.Bisac;
                    espDistItemJsonRequest.price = espDistDataItemRequest.Price;
                    espDistItemJsonRequest.fundId = espDistDataItemRequest.Fundid;
                    espDistItemJsonRequest.fundCode = espDistDataItemRequest.Fundcode;

                    // new in Release 4.2
                    espDistItemJsonRequest.listPrice = espDistDataItemRequest.ListPrice;
                    
                    // Jan 11 2018: Confirmed that current price returned from database is the discounted price that we need to send
                    espDistItemJsonRequest.discountedPrice = espDistDataItemRequest.Price;

                    espDistJsonRequest.items.Add(espDistItemJsonRequest);
                }

                espDistJsonRequest.branches = new List<ESPDistBranchJsonRequest>();

                foreach (ESPDistDataBranchRequest espDistDataBranchRequest in espDistDataRequest.Branches)
                {
                    ESPDistBranchJsonRequest espDistBranchJsonRequest = new ESPDistBranchJsonRequest();

                    espDistBranchJsonRequest.branchId = espDistDataBranchRequest.Branchid;
                    espDistBranchJsonRequest.code = espDistDataBranchRequest.Code;

                    espDistJsonRequest.branches.Add(espDistBranchJsonRequest);
                }
            }

            string output = JsonConvert.SerializeObject(espDistJsonRequest);

            // send json to CHQ

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(AppSetting.ESPBaseURI + "jobs/distribute");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = false;
            httpWebRequest.Headers.Add("espKey", AppSetting.ESPVendorKey);
            httpWebRequest.Headers.Add("api-version", AppSetting.ESPVersion);

            //req.ContentLength = buffer.Length

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(output);
                streamWriter.Flush();
                streamWriter.Close();
            }

            ESPDistJsonResponse espDistJsonResponse = new ESPDistJsonResponse();
            var result = "";
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                    espDistJsonResponse = JsonConvert.DeserializeObject<ESPDistJsonResponse>(result);

                    logger.LogRequest("DoACartDist", output, result, AppSetting.ESPVendorKey, "");
                }

                //var result = "{\"statusCode\":0,\"statusMessage\":\"Accepted for processing\",\"jobId\":\"20009\",\"cartId\":\"{2BE9959B-B126-4E15-A580-FBA336F10DBA}\",\"cartName\":\"xxx\",\"jobUrl\":null}";

                // log the response
                // update status

                espJobURL = espDistJsonResponse.jobURL;
                
                string stateType = string.IsNullOrEmpty(espJobURL) ? "submitted" : "wizard";

                DataSet dsResponseDist = ConvertResponseDistItemToSqlTable(espDistJsonResponse, espServiceRequest.CartId, result, "DIST", stateType, "No Errors");

                ordersDAO.SetESPState(dsResponseDist, espJobURL ?? "", espDistJsonResponse.jobURLText ?? "");
                
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


                        logger.LogRequest("DoACartDist", output, responseText, AppSetting.ESPVendorKey, we.Message);

                        espDistJsonResponse = JsonConvert.DeserializeObject<ESPDistJsonResponse>(responseText);

                        DataSet dsResponseDist =
                            ConvertResponseDistItemToSqlTable(espDistJsonResponse, espServiceRequest.CartId, result, "DIST", "Failed", we.Message);

                        ordersDAO.SetESPState(dsResponseDist, "", "");

                        espMessage = espDistJsonResponse.statusMessage ?? espDistJsonResponse.statusCodes[0].message;

                        isSentErrorAlert = SendUserAlert(AlertMessageTemplateIDEnum.ESPDistWOFundFail, espServiceRequest.CartId, reqBasketName, espServiceRequest.UserId, out alertErrMsg);
                        if (!isSentErrorAlert)
                        {
                            espMessage += (" " + alertErrMsg);
                        }
                        
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogRequest("DoACartDist", output, "", AppSetting.ESPVendorKey, ex.Message);

                DataSet dsResponseDist =
                       ConvertResponseDistItemToSqlTable(espDistJsonResponse, espServiceRequest.CartId, result, "DIST", "Failed", ex.Message);

                ordersDAO.SetESPState(dsResponseDist, "", "");

                espMessage = ex.Message;
                
                isSentErrorAlert = SendUserAlert(AlertMessageTemplateIDEnum.ESPDistWOFundFail, espServiceRequest.CartId, reqBasketName, espServiceRequest.UserId, out alertErrMsg);
                if (!isSentErrorAlert)
                {
                    espMessage += (" " + alertErrMsg);
                }
                
            }
            // handle fail response and send alert
        }

        private DataSet ConvertResponseDistItemToSqlTable(ESPDistJsonResponse espDistJsonResponse, string basketSummaryID, string jsonResponse, string jobType, string stateType, string errorMessage)
        {
            DataSet dsResult = new DataSet();

            DataTable tblESPStatus = new DataTable("ESPStatus");
            DataTable tblInvalidBranchCodes = new DataTable("InvalidBranchCodes");
            DataTable tblInvalidFundCodes = new DataTable("InvalidFundCodes");

            //Declare: tblESPStatus 
            tblESPStatus.Columns.Add("BasketSummaryID", typeof(String));
            tblESPStatus.Columns.Add("ESPJobType", typeof(String));
            tblESPStatus.Columns.Add("ESPStateType", typeof(String));
            tblESPStatus.Columns.Add("ESPJobID", typeof(String));
            tblESPStatus.Columns.Add("CartStatus", typeof(String));
            tblESPStatus.Columns.Add("ErrorMessage", typeof(String));
            tblESPStatus.Columns.Add("ESPResponseJSON", typeof(String));
            //tblESPStatus.Columns.Add("JobURL", typeof(String));
            //tblESPStatus.Columns.Add("JobURLText", typeof(String));

            //Declare: tblInvalidBranchCodes
            tblInvalidBranchCodes.Columns.Add("BasketSummaryID", typeof(String));
            tblInvalidBranchCodes.Columns.Add("GridCodeID", typeof(String));

            //Declare: tblInvalidFundCodes
            tblInvalidFundCodes.Columns.Add("BasketSummaryID", typeof(String));
            tblInvalidFundCodes.Columns.Add("GridCodeID", typeof(String));

            //
            //Process tblESPStatus 
            //
            DataRow workRow = tblESPStatus.NewRow();

            workRow["BasketSummaryID"] = espDistJsonResponse.cartId ?? basketSummaryID;
            workRow["ESPJobType"] = jobType;
            workRow["ESPStateType"] = stateType; // submitted
            workRow["ESPJobID"] = espDistJsonResponse.jobId;
            workRow["CartStatus"] = espDistJsonResponse.statusMessage ?? espDistJsonResponse.statusCodes[0].message; ;
            workRow["ErrorMessage"] = errorMessage;
            workRow["ESPResponseJSON"] = jsonResponse;
            //workRow["JobURL"] = espDistJsonResponse.jobURL;
            //workRow["JobURLText"] = espDistJsonResponse.jobURLText;

            tblESPStatus.Rows.Add(workRow);

            dsResult.Tables.Add(tblESPStatus);

            //
            //Process tblInvalidBranchCodes 
            //

            if (espDistJsonResponse.branches.Count == 0)
            {
                dsResult.Tables.Add(tblInvalidBranchCodes);
            }
            else
            {
                foreach ( ESPDistBranchJsonResponse branch in espDistJsonResponse.branches)
                {
                    if (branch.status != "0")
                    {
                        DataRow invalidBranch = tblInvalidBranchCodes.NewRow();

                        invalidBranch["BasketSummaryID"] = espDistJsonResponse.cartId ?? basketSummaryID;
                        invalidBranch["GridCodeID"] = branch.branchId;

                        tblInvalidBranchCodes.Rows.Add(invalidBranch);
                    }
                }

                dsResult.Tables.Add(tblInvalidBranchCodes);
            }

            //
            //Process tblInvalidFundCodes 
            //
            if (espDistJsonResponse.fundCodes.Count == 0)
            {
                dsResult.Tables.Add(tblInvalidFundCodes);
            }
            else
            {
                foreach (ESPDistFundJsonResponse fund in espDistJsonResponse.fundCodes)
                {
                    if (fund.status != "0")
                    {
                        DataRow invalidFund = tblInvalidFundCodes.NewRow();

                        invalidFund["BasketSummaryID"] = espDistJsonResponse.cartId ?? basketSummaryID;
                        invalidFund["GridCodeID"] = fund.fundId;

                        tblInvalidFundCodes.Rows.Add(invalidFund);
                    }
                }

                dsResult.Tables.Add(tblInvalidFundCodes);
            }

            return dsResult;
        }
        private string Validate(ESPServiceRequest request)
        {
            string error = "";

            if (string.IsNullOrEmpty(request.CartId))
                return RankConstant.ESPSubmissionValidationMessage.REQUIRED_CART_ID;

            if (string.IsNullOrEmpty(request.UserId))
                return RankConstant.ESPSubmissionValidationMessage.REQUIRED_USER_ID;

            if (string.IsNullOrEmpty(request.ESPType))
                return RankConstant.ESPSubmissionValidationMessage.INVALID_ESP_TYPE;

            if (!string.IsNullOrEmpty(request.ESPType) && request.ESPType != RankConstant.ESPType.DIST && request.ESPType != RankConstant.ESPType.RANK)
                return RankConstant.ESPSubmissionValidationMessage.INVALID_ESP_TYPE;


            return error;
        }

        private ESPServiceResult<ESPServiceResponse> HandleFailedESPServiceResult(string errorMessage, string webMethod, string userRequestJson)
        {
            logger.LogRequest("ESP: " + webMethod, userRequestJson, "", "", errorMessage);

            var espServiceResult = new ESPServiceResult<ESPServiceResponse> { Status = ESPServiceStatus.Fail };

            espServiceResult.Data = new ESPServiceResponse();
            espServiceResult.ErrorCode = "1"; // will define the error code
            espServiceResult.ErrorMessage = errorMessage;

            return espServiceResult;
        }

        private bool SendUserAlert(AlertMessageTemplateIDEnum alertTemplateEnum, string cartID, string cartName, string userID, out string errorMessage)
        {
            bool isSent = false;
            errorMessage = string.Empty;

            try
            {
                // Create the alert
                UserAlertsClient userAlert = new UserAlertsClient();

                GetUserAlertMessageTemplateResponse svc1Getresp = new GetUserAlertMessageTemplateResponse();
                svc1Getresp = userAlert.GetUserAlertMessageTemplate(alertTemplateEnum);

                string alertMessage = svc1Getresp.AlertMessageTemplate;
                string configReferenceValue = svc1Getresp.ConfigReferenceValue;

                alertMessage = alertMessage.Replace("@cartname", cartName);
                //alertMessage = alertMessage.Replace("@URL", configReferenceValue + cartID);

                CreateUserAlertMessageResponse svc1resp = new CreateUserAlertMessageResponse();

                svc1resp = userAlert.CreateUserAlertMessage(alertMessage, userID, alertTemplateEnum, "WebAPI");

                isSent = (svc1resp.Status == "OK");

                if (!string.IsNullOrEmpty(svc1resp.ErrorMessage))
                    errorMessage = svc1resp.ErrorMessage;

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                isSent = false;
            }

            return isSent;
        }
    }
}
