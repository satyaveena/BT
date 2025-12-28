using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.Business.DAO;
using BT.ILSQueue.Business.Models;
using BT.ILSQueue.Business.Helpers;
using BT.ILSQueue.Business.MongDBLogger.ELMAHLogger;

using Newtonsoft.Json;

namespace BT.ILSQueue.Business.Manager
{
    public class OrderManager
    {
        #region Private Member

        private static volatile OrderManager _instance;
        private static readonly object SyncRoot = new Object();
        public static OrderManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new OrderManager();
                }

                return _instance;
            }
        }

        #endregion

        #region Public Method

        public string PurchaseOrderFolderPath
        {
            get { return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.LogFolder) + 
                    AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.PurchaseOrderDirectory); }
        }

        public string ResultFolderPath
        {
            get { return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.LogFolder) + 
                    AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.ResultDirectory); }
        }


        public int GetILSQueuePendingOrderCount()
        {
            return OrderDAO.Instance.GetILSQueuePendingOrderCount();
        }

        public List<ILSQueueRequest> GetILSQueuePendingOrder()
        {
            DataSet dsILSRequest = OrderDAO.Instance.GetILSQueuePendingOrders(true);

            return HandleILSQueueValidationResult(dsILSRequest);      
        }

        public async Task SubmitILSValidation(ILSQueueRequest request)
        {
            // Change cart status to ILS submitted and ILS status to Validation in Process
            OrderDAO.Instance.SetILSBasketState(request.ExternalID, request.UpdatedBy, CartStatus.Submitted, ILSState.ILSValidationInProcess,
                   request.MARCProfileID, request.Vendor, request.OrderedAtLocation, request.OrderedDownloadedUserID);

            // Change cart ILS System Status to In progress
            OrderDAO.Instance.SetILSSystemState(request.ExternalID, request.UpdatedBy, ILSSystemStatus.InProgress);

            PolarisProfile pp = ProfilesManager.Instance.GetILSProfile(request.OrganizationID);
            
            StaffUserResponse staffUserResponse = AuthHashManager.Instance.Authenicate(pp);

            FileLogger.LogInfo(string.Format("\r\nBasketSummaryID: {0}\r\nPAPIUrl: {1}\r\n AccessToken: {2} \r\n AccessSecret: {3}", 
                request.ExternalID, pp.papiURL, staffUserResponse.AccessToken, staffUserResponse.AccessSecret));

            //example - https://qa-polaris.polarislibrary.com/PAPIService/rest/protected/v1/1033/100/3/{AccessToken}/jobs/purchaseorders?preordervalidation=1"

            string papiOrderBaseUrl = string.Format("{0}/{1}/jobs/purchaseorders", CommonHelper.GetPolarisAppRelativePath(pp.papiURL), staffUserResponse.AccessToken); 
            string papiValidationUrl = papiOrderBaseUrl += "?preordervalidation=1";

            String dateTimeFormatRFC = DateTime.UtcNow.ToString("R");
            string signature = AuthHashManager.Instance.GetPAPIHash(pp.papiAccesskey, "PUT", papiValidationUrl, dateTimeFormatRFC, staffUserResponse.AccessSecret);

            FileLogger.LogInfo(string.Format("\r\nBasketSummaryID: {0} \r\nPAPIUrl: {1}\r\n Hashkey: {2} \r\n DateTimeFormatRFC: {3}", 
                request.ExternalID, papiValidationUrl, signature, dateTimeFormatRFC));

            PolarisValidationRequest ilsValidationRequest = new PolarisValidationRequest();
            CommonHelper.CopyProperties(ilsValidationRequest, request);

            var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

            string ILSValidationRequestJson = JsonConvert.SerializeObject(ilsValidationRequest, settings);

            var joRequest = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(ILSValidationRequestJson);
            
            DateTime requestDateTime = DateTime.Now;

            // log file
            string folderName = PurchaseOrderFolderPath + "\\";
            string PreFileName = request.ExternalID + "_" + requestDateTime.ToString("yyyyMMddHHmmss");
            string fileName = PreFileName + "_val_request.json";
            CommonHelper.CreateFile(folderName, fileName, joRequest.ToString());
            // end log file

            // log Mongo ILSAPIRequestLog
            ILSAPIRequestLog ilsRequestQueueLog = new ILSAPIRequestLog();
            
            CommonHelper.CopyProperties(ilsRequestQueueLog, ilsValidationRequest);

            ilsRequestQueueLog.ValidationRequest = new ValidationRequest();
            ilsRequestQueueLog.ValidationRequest.LineItems = new List<ILSOrderLineItem>();
            ilsRequestQueueLog.ValidationRequest.LineItems.AddRange(ilsValidationRequest.LineItems);

            ilsRequestQueueLog.ILSVendorID = ApplicationConstants.POLARIS_VENDOR_CODE;
            ilsRequestQueueLog.ProcessingStatus = ILSProcessingStatus.ValidationRequest;
           
            ilsRequestQueueLog.MARCProfileID = request.MARCProfileID;
            ilsRequestQueueLog.OrganizationID = request.OrganizationID;
            ilsRequestQueueLog.UpdatedBy = request.UpdatedBy;
            ilsRequestQueueLog.OrderedDownloadedUserID = request.OrderedDownloadedUserID;
            ilsRequestQueueLog.BasketName = request.BasketName;

            ObjectId queueID = await CommonDAO.Instance.AddILSAPIRequestQueueLog(ilsRequestQueueLog);

            // prepare validation
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(papiValidationUrl);

            // append Header 

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "PUT";
            httpWebRequest.Headers.Add("Authorization", string.Format("PWS {0}:{1}", pp.papiID, signature));
            httpWebRequest.Headers.Add("PolarisDate", dateTimeFormatRFC);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(ILSValidationRequestJson);
                streamWriter.Flush();
                streamWriter.Close();
            }

            PolarisPOResponse ilsValidationResponse = new PolarisPOResponse();

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {

                    string result = streamReader.ReadToEnd();
                    var joResponse = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(result);

                    // file logging
                    fileName = PreFileName + "_val_response.json";
                    CommonHelper.CreateFile(folderName, fileName, joResponse.ToString());

                    ilsValidationResponse = JsonConvert.DeserializeObject<PolarisPOResponse>(result);

                    if (Convert.ToInt32(ilsValidationResponse.PAPIErrorCode) >= 0)
                    {
                        // Mongo logging
                        await CommonDAO.Instance.UpdateILSValidationResponseLog(queueID, ilsValidationResponse, ILSProcessingStatus.ValidationResponse);

                        BTAlertManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSValidationSucess, request.ExternalID, request.UpdatedBy, request.BasketName );

                        await SubmitILSOrder(request, queueID, pp, papiOrderBaseUrl, staffUserResponse.AccessSecret, requestDateTime);
                    }
                    else
                    {
                        // Mongo logging
                        await CommonDAO.Instance.UpdateILSValidationResponseLog(queueID, ilsValidationResponse, ILSProcessingStatus.ValidationFailed);

                        OrderDAO.Instance.ResetILSStatus(request.ExternalID, request.UpdatedBy);
                        OrderDAO.Instance.SetILSBasketState(request.ExternalID, request.UpdatedBy, CartStatus.Open, ILSState.ILSValidationCompleted, "", "", "", "");

                        BTAlertManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSValidationError, request.ExternalID, request.UpdatedBy, request.BasketName);
                    }
                }
            }

            catch (Exception ex)
            {
                ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));
            }

          
        }

       
        public async Task CheckJobStatus(int numberOfQueueItems)
        {
            DateTime resultDateTime = DateTime.Now;

            List<ILSAPIRequestLog> jobList = await CommonDAO.Instance.GetOrderQueueItems(numberOfQueueItems, ILSProcessingStatus.OrderingResponse);

            foreach (ILSAPIRequestLog requestLog in jobList) {

                PolarisProfile pp = ProfilesManager.Instance.GetILSProfile(requestLog.OrganizationID);
                StaffUserResponse staffUserResponse = AuthHashManager.Instance.Authenicate(pp);

                string folderName = ResultFolderPath + "\\";
                string PreFileName = requestLog.OrderResponse.JobGuid + "_" + resultDateTime.ToString("yyyyMMddHHmmss");
                string fileName = PreFileName + "_order_status.json";

                FileLogger.LogInfo(string.Format("\r\nQueueID: {0}\r\nJobID: {1}\r\nPAPIUrl: {2}\r\n AccessToken: {3} \r\n AccessSecret: {4}",
                   requestLog.ILSQueueID,
                   requestLog.OrderResponse.JobGuid, 
                   pp.papiURL, staffUserResponse.AccessToken, staffUserResponse.AccessSecret));

                // Example - https://qa-polaris.polarislibrary.com/PAPIService/rest/protected/v1/1033/100/3/{AccessToken}/jobs/purchaseorders/{JobGuid}/status
                string papiOrderBaseUrl = string.Format("{0}/{1}/jobs/purchaseorders/{2}/status", CommonHelper.GetPolarisAppRelativePath(pp.papiURL), staffUserResponse.AccessToken, requestLog.OrderResponse.JobGuid);

                String dateTimeFormatRFC = DateTime.UtcNow.ToString("R");
                string signature = AuthHashManager.Instance.GetPAPIHash(pp.papiAccesskey, "GET", papiOrderBaseUrl, dateTimeFormatRFC, staffUserResponse.AccessSecret);

                FileLogger.LogInfo(string.Format("\r\nQueueID: {0}\r\nJobID: {1} \r\nPAPIUrl: {2}\r\n Hashkey: {3} \r\n DateTimeFormatRFC: {4}",
                    requestLog.ILSQueueID,
                    requestLog.OrderResponse.JobGuid, 
                    papiOrderBaseUrl, signature, dateTimeFormatRFC));

                // prepare validation
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(papiOrderBaseUrl);

                // append Header 

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", string.Format("PWS {0}:{1}", pp.papiID, signature));
                httpWebRequest.Headers.Add("PolarisDate", dateTimeFormatRFC);

                PolarisOrderResult polarisOrderResult = new PolarisOrderResult();

                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {

                        string result = streamReader.ReadToEnd();
                        var joResponse = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(result);

                        // file logging
                        CommonHelper.CreateFile(folderName, fileName, joResponse.ToString());

                        // Mongo logging
                        PolarisPOResponse ilsJobStatusResponse = JsonConvert.DeserializeObject<PolarisPOResponse>(result);

                        if ( ilsJobStatusResponse.JobStatusID == ((int)PolarisJobStatus.Succeeded).ToString() )
                        {
                            await CommonDAO.Instance.UpdateILSProcessingStatus(requestLog.ILSQueueID, ILSProcessingStatus.OrderingResponseReady);
                        }
                         
                         
                    }
                }

                catch (Exception ex)
                {
                    FileLogger.LogException("CheckJobStatus Failed - " + ex.Message, ex);

                    ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                    ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));
                }
            }
        }

        public async Task GetILSOrderResult(int numberOfQueueItems)
        {
            // log
            DateTime resultDateTime = DateTime.Now;

            List<ILSAPIRequestLog> jobReadyList = await CommonDAO.Instance.GetOrderQueueItems(numberOfQueueItems, ILSProcessingStatus.OrderingResponseReady);

            foreach (ILSAPIRequestLog jobReadyRequestLog in jobReadyList)
            {
                string folderName = ResultFolderPath + "\\";
                string PreFileName = jobReadyRequestLog.ExternalID + "_" + resultDateTime.ToString("yyyyMMddHHmmss");
                string fileName = PreFileName + "_order_result.json";

                PolarisProfile pp = ProfilesManager.Instance.GetILSProfile(jobReadyRequestLog.OrganizationID);
                StaffUserResponse staffUserResponse = AuthHashManager.Instance.Authenicate(pp);

                FileLogger.LogInfo(string.Format("\r\nBasketSummaryID: {0}\r\nPAPIUrl: {1}\r\n AccessToken: {2} \r\n AccessSecret: {3}",
                   jobReadyRequestLog.ExternalID, pp.papiURL, staffUserResponse.AccessToken, staffUserResponse.AccessSecret));

                // exmaple string papiOrderBaseUrl = string.Format("https://qa-polaris.polarislibrary.com/PAPIService/rest/protected/v1/1033/100/3/{0}/jobs/purchaseorders/{1}/result", staffUserResponse.AccessToken, jobID);
                string papiOrderBaseUrl = string.Format("{0}/{1}/jobs/purchaseorders/{2}/result", CommonHelper.GetPolarisAppRelativePath(pp.papiURL), staffUserResponse.AccessToken, jobReadyRequestLog.OrderResponse.JobGuid);

                String dateTimeFormatRFC = DateTime.UtcNow.ToString("R");
                string signature = AuthHashManager.Instance.GetPAPIHash(pp.papiAccesskey, "GET", papiOrderBaseUrl, dateTimeFormatRFC, staffUserResponse.AccessSecret);

                FileLogger.LogInfo(string.Format("\r\nBasketSummaryID: {0} \r\nPAPIUrl: {1}\r\n Hashkey: {2} \r\n DateTimeFormatRFC: {3}",
                   jobReadyRequestLog.ExternalID, papiOrderBaseUrl, signature, dateTimeFormatRFC));

                // prepare validation
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(papiOrderBaseUrl);

                // append Header 

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", string.Format("PWS {0}:{1}", pp.papiID, signature));
                httpWebRequest.Headers.Add("PolarisDate", dateTimeFormatRFC);

                PolarisOrderResult polarisOrderResult = new PolarisOrderResult();

                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    await CommonDAO.Instance.UpdateILSProcessingStatus(jobReadyRequestLog.ILSQueueID, ILSProcessingStatus.OrderingResultRequest);

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {

                        string result = streamReader.ReadToEnd();
                        var joResponse = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(result);

                        // file logging
                        CommonHelper.CreateFile(folderName, fileName, joResponse.ToString());

                        // Mongo logging
                        polarisOrderResult = JsonConvert.DeserializeObject<PolarisOrderResult>(result);
                        await CommonDAO.Instance.UpdateILSOrderResultLog(jobReadyRequestLog.ILSQueueID, polarisOrderResult);

                        List<ILSLineItemDetail> successOrders = new List<ILSLineItemDetail>();
                        List<ILSLineItemDetail> errorOrders = new List<ILSLineItemDetail>();
                       

                        HandlePolarisOrderResult(polarisOrderResult, successOrders, errorOrders);

                        if (successOrders.Count == 0)
                        {

                            OrderDAO.Instance.ResetILSStatus(jobReadyRequestLog.ExternalID, jobReadyRequestLog.UpdatedBy);
                            OrderDAO.Instance.SetILSBasketState(jobReadyRequestLog.ExternalID, jobReadyRequestLog.UpdatedBy, CartStatus.Open, ILSState.ILSOrderValidationCompleted, "", "", "", "");

                            BTAlertManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSOrderValidationError, jobReadyRequestLog.ExternalID, jobReadyRequestLog.UpdatedBy, jobReadyRequestLog.BasketName);

                        }
                        else
                        {
                            if (errorOrders.Count > 0)
                            {
                                // split cart next....
                                MoveErrorItemstoNewILSCart(errorOrders, jobReadyRequestLog.ExternalID, jobReadyRequestLog.UpdatedBy);

                                // rollup original cart
                                OrderDAO.Instance.SetPricingBasketRollupNumbers(jobReadyRequestLog.ExternalID);
                            }

                            OrderDAO.Instance.SaveILSLineItemDetails(jobReadyRequestLog.UpdatedBy, successOrders);

                            OrderDAO.Instance.SetILSBasketState(jobReadyRequestLog.ExternalID, jobReadyRequestLog.UpdatedBy, CartStatus.Submitted, ILSState.ILSOrderValidationCompleted,
                            jobReadyRequestLog.MARCProfileID, jobReadyRequestLog.Vendor, jobReadyRequestLog.OrderedAtLocation, jobReadyRequestLog.OrderedDownloadedUserID);
                            
                            // Change cart ILS System Status to Completed
                            OrderDAO.Instance.SetILSSystemState(jobReadyRequestLog.ExternalID, jobReadyRequestLog.UpdatedBy, ILSSystemStatus.Completed);

                            // Alert
                            BTAlertManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSOrderValidationSuccess, jobReadyRequestLog.ExternalID, jobReadyRequestLog.UpdatedBy, jobReadyRequestLog.BasketName);

                            //III customer in Library Org. NOT using retail VIP functionality to TOLAS  and Bin and Hold premium services
                            string newBasketName, newBasketID, newOEBasketName, newOEBasketID;

                            CartManager.Instance.SubmitOrder(jobReadyRequestLog.ExternalID, jobReadyRequestLog.UpdatedBy,
                                out newBasketName, out newBasketID, out newOEBasketName, out newOEBasketID, false, false, jobReadyRequestLog.OrderedDownloadedUserID);
                             
                        }
                    
                    }
                }

                catch (Exception ex)
                {
                    FileLogger.LogException("CheckJobStatus Failed - " + ex.Message, ex);

                    ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                    ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));
                }

            }
           
        }
        #endregion

        #region Private Method
        private List<ILSQueueRequest> HandleILSQueueValidationResult(DataSet dsInput)
        {
            List<ILSQueueRequest> ilsQueueServiceOrderValidation = new List<ILSQueueRequest>();

            foreach (DataRow row in dsInput.Tables[0].Rows)
            {
                var validateCart = new ILSQueueRequest();
                validateCart.ExternalID = DataAccessHelper.SqlDataConvertTo<string>(row, "ExternalID");
                validateCart.Vendor = DataAccessHelper.SqlDataConvertTo<string>(row, "Vendor");
                validateCart.OrderedAtLocation = DataAccessHelper.SqlDataConvertTo<string>(row, "OrderedAtLocation");
                validateCart.OrderType = DataAccessHelper.SqlDataConvertTo<int>(row, "OrderType");
                validateCart.PaymentMethod = DataAccessHelper.SqlDataConvertTo<int>(row, "PaymentMethod");
                validateCart.Copies = DataAccessHelper.SqlDataConvertTo<int>(row, "Copies");

                validateCart.PONumber = DataAccessHelper.SqlDataConvertTo<string>(row, "PONumber");
                validateCart.PostbackURL =  AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.PostbackURL);

                validateCart.OrganizationID = DataAccessHelper.SqlDataConvertTo<string>(row, "u_org_id");
                validateCart.MARCProfileID = DataAccessHelper.SqlDataConvertTo<string>(row, "ILSMarcProfileID");
                validateCart.UpdatedBy = DataAccessHelper.SqlDataConvertTo<string>(row, "UpdatedBy");
                validateCart.OrderedDownloadedUserID = DataAccessHelper.SqlDataConvertTo<string>(row, "OrderedDownloadedUserID");
                validateCart.BasketName = DataAccessHelper.SqlDataConvertTo<string>(row, "BasketName");

                #region Basket Line Items
                if (dsInput.Tables.Count > 1)
                {
                    string qryFilter = string.Format("BasketSummaryID = '{0}'",  validateCart.ExternalID);
                    string qrySortOrder = "ExternalLineItemID ASC";

                    DataRow[] dataSetGrids = dsInput.Tables[1].Select(qryFilter, qrySortOrder);

                    string o_externalLineItemID = "";
                    string n_externalLineItemID = "";
                    var validateCartLineItem= new ILSOrderLineItem();
                    int totalCopiesPerLineItem = 0;

                    foreach (DataRow row1 in dataSetGrids)
                    {
                        bool isFirstLineItem = (o_externalLineItemID == "");// first row

                        if (isFirstLineItem)
                        { 
                            validateCart.LineItems = new List<ILSOrderLineItem>();
                        }

                        n_externalLineItemID = DataAccessHelper.SqlDataConvertTo<string>(row1, "ExternalLineItemID");

                        if (n_externalLineItemID != o_externalLineItemID)
                        {
                            if (!isFirstLineItem)
                            {
                                validateCartLineItem.Copies = totalCopiesPerLineItem;
                                validateCart.LineItems.Add(validateCartLineItem);
                            }  
                            
                            validateCartLineItem = new ILSOrderLineItem();
                            validateCartLineItem.LineItemSegments = new List<ILSLineItemSegment>();

                            // validateCartLineItems.ExternalLineItemID = CommonHelper.SqlDataConvertTo<string>(row1, "ExternalLineItemID");
                            validateCartLineItem.ExternalLineItemID = n_externalLineItemID;
                            //validateCartLineItem.Copies = CommonHelper.SqlDataConvertTo<int>(row1, "Copies");
                            validateCartLineItem.BTKey = DataAccessHelper.SqlDataConvertTo<string>(row1, "BTKey");

                            o_externalLineItemID = n_externalLineItemID;
                            totalCopiesPerLineItem = 0;
                        }

                        var validateCartLineItemsSegment = new ILSLineItemSegment();

                        validateCartLineItemsSegment.Fund = DataAccessHelper.SqlDataConvertTo<string>(row1, "Fund");
                        validateCartLineItemsSegment.Collection = DataAccessHelper.SqlDataConvertTo<string>(row1, "Collection");
                        validateCartLineItemsSegment.Location = DataAccessHelper.SqlDataConvertTo<string>(row1, "Location");

                        validateCartLineItemsSegment.Copies = DataAccessHelper.SqlDataConvertTo<int>(row1, "Copies");
                        totalCopiesPerLineItem += validateCartLineItemsSegment.Copies;

                        validateCartLineItem.LineItemSegments.Add(validateCartLineItemsSegment);

                        //validateCart.LineItems.Add(validateCartLineItems);
                    } // end for each cart
                    // added last line item
                    validateCartLineItem.Copies = totalCopiesPerLineItem;
                    validateCart.LineItems.Add(validateCartLineItem);
                } // end table-1

                #endregion
                ilsQueueServiceOrderValidation.Add(validateCart);
            } // end for the basket summary dataset

            return ilsQueueServiceOrderValidation;
        }

        /* private PolarisProfile GetILSProfile()
        {
            PolarisProfile polarisProfile = new PolarisProfile();

            polarisProfile.papiURL = "https://qa-polaris.polarislibrary.com/PAPIService/rest";
            polarisProfile.papiID = "TS360API";
            polarisProfile.papiAccesskey = "C987543D-8D2E-4C59-8B6F-4D9E01C70B97";
            polarisProfile.domain = "QA-Polaris";
            polarisProfile.account = "VendorAccount";
            polarisProfile.password = "VendorTesting01!";

            return polarisProfile;
        }*/

        /*private void SetILSSystemState(string basketSummaryID, string userId, ILSSystemStatus ilsSystemStatusID)
        {
            OrderDAO.Instance.SetILSSystemState(basketSummaryID, userId, ilsSystemStatusID);
        }*/

        private async Task SubmitILSOrder(ILSQueueRequest request, ObjectId queueID,
               PolarisProfile pp, string papiOrderUrl, string accessSecret, DateTime requestDateTime)
        {
            OrderDAO.Instance.SetILSBasketState(request.ExternalID, request.UpdatedBy, CartStatus.Submitted, ILSState.ILSOrderValidationInProcess,
                  request.MARCProfileID, request.Vendor, request.OrderedAtLocation, request.OrderedDownloadedUserID);

            /*string sortColumn = "Cart Order";
            string sortDirection = "Descending";
            string FullIndicator = "F";
            bool isOrdered = true;
            bool isCancelled = false;
            bool isOCLCEnabled = true;
            bool isBTEmployee = true;
            bool hasInventoryRules = false;
            string marketType = "1";*/

            MARCJsonRequest marcJsonRequest = ProfilesManager.Instance.GetMARCJsonRequestParameter(request, requestDateTime);

            SortedDictionary<string, MARC21> ltMarc21 = MarcManager.Instance.GenerateOrderMARC(
                  marcJsonRequest.SortColumn, request.ExternalID, marcJsonRequest.SortDirection, request.MARCProfileID, marcJsonRequest.FullIndicator,
                  marcJsonRequest.IsOrdered, marcJsonRequest.IsCancelled, marcJsonRequest.IsOCLCEnabled, marcJsonRequest.IsBTEmployee,
                  marcJsonRequest.HasInventoryRules, marcJsonRequest.MarketType,
                  requestDateTime);


           // string pause;

            PolarisOrderRequest ilsOrderRequest = new PolarisOrderRequest();

            CommonHelper.CopyProperties(ilsOrderRequest, request);

            ilsOrderRequest.MARCLineItems = new List<MARCLineItem>();

            foreach (ILSOrderLineItem lineItem in request.LineItems)
            {

                MARCLineItem marcLineItem = new MARCLineItem();
                marcLineItem.ExternalLineItemID = lineItem.ExternalLineItemID;
                marcLineItem.Copies = lineItem.Copies;
                marcLineItem.MARC = ltMarc21[lineItem.BTKey];//ltMarc21[0];

                ilsOrderRequest.MARCLineItems.Add(marcLineItem);
            }

            String dateTimeFormatRFC = DateTime.UtcNow.ToString("R");
            string signature = AuthHashManager.Instance.GetPAPIHash(pp.papiAccesskey, "POST", papiOrderUrl, dateTimeFormatRFC, accessSecret);

            string ILSOrderRequestJson = JsonConvert.SerializeObject(ilsOrderRequest);
            var joRequest = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(ILSOrderRequestJson);

            // log
            string folderName = PurchaseOrderFolderPath + "\\";
            string PreFileName = request.ExternalID + "_" + requestDateTime.ToString("yyyyMMddHHmmss");
            string fileName = PreFileName + "_order_request.json";

            CommonHelper.CreateFile(folderName, fileName, joRequest.ToString());
            // end log

            // log Mongo ILSAPIRequestLog
            ILSAPIRequestLog ilsRequestQueueLog = new ILSAPIRequestLog();

            CommonHelper.CopyProperties(ilsRequestQueueLog, ilsOrderRequest);

            ilsRequestQueueLog.OrderRequest = new OrderRequest();
            ilsRequestQueueLog.OrderRequest.MARCLineItems = new List<MARCLineItem>();
            ilsRequestQueueLog.OrderRequest.MARCLineItems.AddRange(ilsOrderRequest.MARCLineItems);

            await CommonDAO.Instance.UpdateILSOrderRequestLog(queueID, ilsRequestQueueLog);
            //

            // prepare order request
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(papiOrderUrl);

            // append Header 

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", string.Format("PWS {0}:{1}", pp.papiID, signature));
            httpWebRequest.Headers.Add("PolarisDate", dateTimeFormatRFC);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(ILSOrderRequestJson);
                streamWriter.Flush();
                streamWriter.Close();
            }

            PolarisPOResponse ilsOrderResponse = new PolarisPOResponse();

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();

                ilsOrderResponse = JsonConvert.DeserializeObject<PolarisPOResponse>(result);

                var joResponse = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(result);
                // file logging
                fileName = PreFileName + "_order_response.json";
                CommonHelper.CreateFile(folderName, fileName, joResponse.ToString());

                if (Convert.ToInt32(ilsOrderResponse.PAPIErrorCode) >= 0)
                {
                    // Mongo logging
                    await CommonDAO.Instance.UpdateILSOrderResponseLog(queueID, ilsOrderResponse, ILSProcessingStatus.OrderingResponse);
                
                }
                else
                {
                    await CommonDAO.Instance.UpdateILSOrderResponseLog(queueID, ilsOrderResponse, ILSProcessingStatus.OrderValidationFailed);

                    OrderDAO.Instance.ResetILSStatus(request.ExternalID, request.UpdatedBy);
                    OrderDAO.Instance.SetILSBasketState(request.ExternalID, request.UpdatedBy, CartStatus.Open, ILSState.ILSValidationCompleted, "", "", "", "");

                    BTAlertManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSOrderValidationError, request.ExternalID, request.UpdatedBy, request.BasketName);

                }

            }
        }

        private void HandlePolarisOrderResult(PolarisOrderResult orderResultResponse, List<ILSLineItemDetail> successOrders, List<ILSLineItemDetail> errorOrders)
        {
            string purchaseOrderID = orderResultResponse.PurchaseOrderID;

            foreach (ILSAckLineItem lineItem in orderResultResponse.LineItems)
            {
                foreach (ILSAckLineItemSegment lineItemSegment in lineItem.LineItemSegments )
                {
                    successOrders.Add(new ILSLineItemDetail() {
                        BasketLineItemID = lineItem.ExternalLineItemID,
                        ILSBIBNumber = lineItem.BibRecordID,
                        ILSOrderNumber = lineItem.POLineItemID,
                        LocationCode = lineItemSegment.Location,
                        FundCode= lineItemSegment.Fund,
                        CollectionCode = lineItemSegment.Collection,
                        ILSPolySegID = lineItemSegment.EDIPOLISegNum
                    });

                }
            }

            if (orderResultResponse.LineItemErrors != null)
            {
                foreach (ILSAckLineItemError lineItem in orderResultResponse.LineItemErrors)
                {
                    errorOrders.Add(new ILSLineItemDetail()
                    {
                        BasketLineItemID = lineItem.ExternalLineItemID
                    });
                }
            }


        }

        private void MoveErrorItemstoNewILSCart(List<ILSLineItemDetail> errorLines, string basketSummaryID, string userId)
        {
            DataSet dsBTKeyLineItemIDs = OrderDAO.Instance.GetBTKeysByLineItemIDs(basketSummaryID, errorLines.Select(p => p.BasketLineItemID).ToList());

            if (!(dsBTKeyLineItemIDs == null || dsBTKeyLineItemIDs.Tables.Count == 0))
            {
                /*  List<string> lineIds = (from x in dsBTKeyLineItemIDs.Tables[0].AsEnumerable()
                                       select x.Field<string>("BasketLineItemID")).ToList();

               List<string> btkeys = (from x in dsBTKeyLineItemIDs.Tables[0].AsEnumerable()
                                       select x.Field<string>("BTKey")).ToList();*/

                var dictLineItems = new Dictionary<string, Dictionary<string, string>>();

                foreach (DataRow row in dsBTKeyLineItemIDs.Tables[0].Rows)
                {
                    var dicValues = new Dictionary<string, string>
                                    {
                                        {"BasketLineItemID", DataAccessHelper.ConvertToString(row["BasketLineItemID"])},
                                        {"BTKey", DataAccessHelper.ConvertToString(row["BTKey"])},
                                        {"quantity", ""},
                                        {"POLineItemNumber", ""},
                                        {"Note", ""},
                                        {"BibNumber", ""},
                                        {"PrimaryResponsiblePartyRedundant", ""},
                                        {"ShortTitleRedundant", ""}
                                    };

                    dictLineItems.Add(DataAccessHelper.ConvertToString(row["BasketLineItemID"]), dicValues);
                }

                CartManager.Instance.CreateNewILSCartWithErrorsItems(userId, basketSummaryID, dictLineItems);

            }
           
            
        }
        #endregion
    }
}
