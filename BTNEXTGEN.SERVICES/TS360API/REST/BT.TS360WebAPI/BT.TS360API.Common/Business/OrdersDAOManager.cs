using BT.TS360API.Common.Business;
using BT.TS360API.Common.CartFramework;
using BT.TS360API.Common.Constants;
using BT.TS360API.Common.Helper;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ExternalServices;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using DotNetOpenAuth.OAuth2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;


namespace BT.TS360API.Common.DataAccess
{
    public class OrdersDAOManager
    {
        private static readonly object SyncRoot = new Object();
        public static OrdersDAOManager Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    OrdersDAOManager _instance = HttpContext.Current == null ? new OrdersDAOManager() : HttpContext.Current.Items["OrdersDAOManager"] as OrdersDAOManager;
                    if (_instance == null)
                    {
                        _instance = new OrdersDAOManager();
                        HttpContext.Current.Items.Add("OrdersDAOManager", _instance);
                    }

                    return _instance;
                }
            }
        }

        #region Submit ILS

        /// <summary>
        /// Process ILS Order
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ILSGlobalSettings"></param>
        /// <returns></returns>
        public bool ProcessILSOrder(ILSOrderRequest request)
        {
            bool isSuccess = true;
            MARCJsonRequest marcJsonRequest = new MARCJsonRequest();

            ILSLog ILSRespLog = new ILSLog()
            {
                BasketSummaryId = request.CartId,
                RequestDateTime = DateTime.Now,
                ResponseDateTime = DateTime.Now,
                UserId = request.UserId
            };

            List<ILSLineItemDetail> SuccessOrders = new List<ILSLineItemDetail>();
            List<MARCJsonResponse> marcs = new List<MARCJsonResponse>();

            try
            {
                // Change cart status to ILS submitted and ILS status to Validation in Process
                CartDAOManager.Instance.SetILSBasketState(request.CartId, request.UserId, CartStatus.Submitted, ILSState.ILSValidationInProcess, request.MarcProfileId, request.IlsVendorCode, request.OrderedDownloadedUserId);

                // Change cart ILS System Status to In progress
                CartDAOManager.Instance.SetILSSystemState(request.CartId, request.UserId, ILSSystemStatus.InProgress);

                marcJsonRequest = GetMarcDetails(request.MarcProfileId, request.UserId, request.CartId, request.IlsVendorCode);

                // Get access token 
                string accessToken = RequestILSToken(marcJsonRequest.OrgId, ILSRespLog, marcJsonRequest.ILSAcquisitionsApiKey, marcJsonRequest.ILSAcquisitionsApiPassphrase, marcJsonRequest.ILSBaseAddress);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    //Call Marc Service 
                    marcs = MARCProfilerManager.Instance.GetMarcJson(marcJsonRequest);

                    //Validate all MARC records
                    InvokeILSApiValidation(accessToken, marcJsonRequest.ILSBaseAddress, marcs, marcJsonRequest.TsUserId, ILSRespLog, SuccessOrders);

                    if (ILSRespLog.ILSLineItemLogs.Count == 0)
                    {
                        // Submitt all MARC records
                        CartDAOManager.Instance.SetILSBasketState(marcJsonRequest.BasketSummaryID, marcJsonRequest.TsUserId, CartStatus.Submitted, ILSState.ILSOrderValidationInProcess, marcJsonRequest.ProfileID, request.IlsVendorCode, request.OrderedDownloadedUserId);
                        InvokeILSApiSubmit(accessToken, marcJsonRequest.ILSBaseAddress, marcs, marcJsonRequest.TsUserId, ILSRespLog, SuccessOrders);
                    }
                }

                HandleILSError(ILSRespLog, marcJsonRequest.BasketSummaryID, marcJsonRequest.TsUserId, marcs.Count, marcJsonRequest.ProfileID);

                // log error to MONGO
                if (SuccessOrders != null && SuccessOrders.Count > 0)
                {
                    if (string.IsNullOrEmpty(ILSRespLog.ErrorType))
                    {
                        ILSRespLog.ErrorType = "Information";
                        ILSRespLog.ILSStatus = "SuccessOrders";
                    }
                    
                    foreach (var successOrder in SuccessOrders)
                    {
                        ILSRespLog.ILSLineItemLogs.Add(new ILSLineItemLog()
                        {
                            ILSAPIRequest = successOrder.ILSBIBNumber,
                            ILSAPIResponse = successOrder.ILSOrderNumber,
                            LineItemId = successOrder.BasketLineItemID,
                            ErrorType = "Information"
                        });
                    }
                }
                ILSLogManager.Instance.InsertILSLog(ILSRespLog);
                // log error to MONGO end

                HandleILSSuccessOrders(marcJsonRequest.TsUserId, marcJsonRequest.BasketSummaryID, SuccessOrders, request.MarcProfileId, request.IlsVendorCode, request.OrderedDownloadedUserId);
            }
            catch (Exception ex)
            {
                isSuccess = false;

                ILSRespLog.ErrorDescription = OrderResources.ILS_ErrorOccured;
                ILSRespLog.ErrorType = "Other";
                ILSRespLog.ILSStatus = "Other";

                ILSLogManager.Instance.InsertILSLog(ILSRespLog);
                Logger.LogException("Method- ProcessILSOrder() - " + ex.Message + " - CartId : " + request.CartId, ExceptionCategory.ILS.ToString() + ". Stack Trace: " + ex.StackTrace, ex);
                // Change status to Open
                CartDAOManager.Instance.ResetILSStatus(request.CartId, request.UserId);
                CartDAOManager.Instance.SetILSBasketState(request.CartId, request.UserId, CartStatus.Open, ILSState.ILSValidationCompleted, "", "", "");
            }

            if (!string.IsNullOrEmpty(ILSRespLog.ErrorDescription))
                isSuccess = false;

            return isSuccess;
        }

        /// <summary>
        /// Handle ILS Errors
        /// </summary>
        /// <param name="ILSResponse"></param>
        private void HandleILSError(ILSLog ilsErrors, string basketSummaryID, string userId, int totalLineItemCount, string ilsMarcProfileID)
        {

            ilsErrors.ResponseDateTime = DateTime.Now;

            if (ilsErrors.ILSStatus == "Order")
            {

                if (ilsErrors.ILSLineItemLogs.Count < totalLineItemCount)
                {
                    // Order -- Move to new cart

                    ilsErrors.ErrorType = "Order";
                    string newCartId = string.Empty;
                    var newCart = MoveErrorItemstoNewIIICart(ilsErrors, basketSummaryID, userId, out newCartId);
                    
                    // rollup original cart
                    CartDAOManager.Instance.SetPricingBasketRollupNumbers(basketSummaryID);

                    if (!string.IsNullOrEmpty(newCart))
                    {
                        string errorFormat = OrderResources.ILS_ErrorMoveCartFormat;
                        ilsErrors.ErrorDescription = string.Format(errorFormat, ilsErrors.ILSLineItemLogs.Count, newCart);
                        ilsErrors.NewCartId = newCartId;
                        UserDAOManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSOrderError, newCartId, userId);
                    }
                    else
                    {
                        //TODO : What to do if user have no rights to create cart and error occured during cart creation or moving line items
                    }
                }
                else
                {
                    //ilsErrors.ErrorDescription = OrderResources.ILS_AllItemsCartError;
                    CartDAOManager.Instance.ResetILSStatus(basketSummaryID, userId);
                    CartDAOManager.Instance.SetILSBasketState(basketSummaryID, userId, CartStatus.Open, ILSState.ILSValidationCompleted, "", "", "");
                    UserDAOManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSOrderValidationError, basketSummaryID, userId);
                }

            } // end order

            if (ilsErrors.ILSStatus == "Authentication" || ilsErrors.ILSStatus == "Validation")
            {

                if (ilsErrors.ILSLineItemLogs.Any(p => p.ErrorType.Equals("FundLocation")))
                {
                    ilsErrors.ErrorDescription = OrderResources.ILS_InvalidFundAndLocationCodeError;
                }

                CartDAOManager.Instance.ResetILSStatus(basketSummaryID, userId);
                //Change status to open and validation completed only if validation error
                CartDAOManager.Instance.SetILSBasketState(basketSummaryID, userId, CartStatus.Open, ILSState.ILSValidationCompleted, "", "", "");
            }
        }

        private string MoveErrorItemstoNewIIICart(ILSLog ILSLineItems, string BasketSummaryID, string userId, out string newCartId)
        {
            List<string> lineIds = ILSLineItems.ILSLineItemLogs.Select(p => p.LineItemId).ToList();
            List<string> btkeys = ILSLineItems.ILSLineItemLogs.Select(p => p.BTKey).ToList();
            var cartManager = new CartManager().GetCartManagerForUser(userId);

            var result = cartManager.CreateNewIIICartWithErrorsItems(userId, BasketSummaryID, lineIds, btkeys, out newCartId);
            if (result.Status == AppServiceStatus.Success)
                return result.Data;
            else
                return string.Empty;
        }

        /// <summary>
        /// Handle ILS Success Orders
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="successOrders"></param>
        private void HandleILSSuccessOrders(string userId, string basketSummaryID, List<ILSLineItemDetail> successOrders, string marcProfile, string ilsVendorCode, string orderedDownloadedUserId)
        {
            if (successOrders.Count > 0)
            {
                var newBasketName = string.Empty;
                var newBasketID = string.Empty;
                var newOEBasketName = string.Empty;
                var newOEBasketID = string.Empty;

                OrdersDAO.Instance.SaveILSLineItemDetails(userId, successOrders);

                CartDAOManager.Instance.SetILSBasketState(basketSummaryID, userId, CartStatus.Submitted, ILSState.ILSOrderValidationCompleted, marcProfile, ilsVendorCode, orderedDownloadedUserId);
                // Change cart ILS System Status to Completed
                CartDAOManager.Instance.SetILSSystemState(basketSummaryID, userId, ILSSystemStatus.Completed);

                //III customer in Library Org. NOT using retail VIP functionality to TOLAS  and Bin and Hold premium services
                var cartManager = new CartManager().GetCartManagerForUser(userId);
                cartManager.SubmitOrder(basketSummaryID, userId, out newBasketName, out newBasketID, out newOEBasketName, out newOEBasketID, false, false, orderedDownloadedUserId);
            }
        }

        /// <summary>
        /// Initialize ILS WebServer Client
        /// </summary>
        /// <param name="OrgId"></param>
        /// <param name="ILSGlobalSettings"></param>
        /// <returns></returns>
        private WebServerClient InitializeILSWebServerClient(string OrgId, string ILSAcquisitionsApiKey, string ILSAcquisitionsApiPassphrase, string authorizationServerUri)
        {
            GlobalConfigurationHelper ILSGlobalSettings = GlobalConfigurationHelper.Instance;
            WebServerClient webServerClient;
            string ILSAuthorizePath = ILSGlobalSettings.Get(GlobalConfigurationKey.ILSAuthorizePath);
            var ILSTokenPath = ILSGlobalSettings.Get(GlobalConfigurationKey.ILSTokenPath);
            var authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(new Uri(authorizationServerUri), ILSAuthorizePath),
                TokenEndpoint = new Uri(new Uri(authorizationServerUri), ILSTokenPath)
            };


            var apikey = APIEncryptionHelper.Decrypt(ILSAcquisitionsApiKey);
            var apiphrase = APIEncryptionHelper.Decrypt(ILSAcquisitionsApiPassphrase);
            webServerClient = new WebServerClient(authorizationServer, apikey, apiphrase);

            return webServerClient;
        }

        /// <summary>
        /// Request ILS Token
        /// </summary>
        /// <param name="OrgId"></param>
        /// <param name="ILSGlobalSettings"></param>
        /// <param name="ILSLogs"></param>
        /// <returns></returns>
        private string RequestILSToken(string OrgId, ILSLog ILSLogs, string ILSAcquisitionsApiKey, string ILSAcquisitionsApiPassphrase, string ILSBaseAddress)
        {
            int retryMaxCount = 0;
            int retryDelay = 0;

            if (int.TryParse(BT.TS360API.Common.Configrations.AppSettings.ILSMaxRetryCount, out retryMaxCount))
                retryMaxCount = 3;
            if (int.TryParse(BT.TS360API.Common.Configrations.AppSettings.ILSRetryDelayInSecond, out retryDelay))
                retryDelay = 5;

            TimeSpan delay = TimeSpan.FromSeconds(retryDelay);

            int currentRetry = -1;

            string accessToken = string.Empty;

            do
            {
                currentRetry++;

                try
                {
                    WebServerClient _webServerClient = InitializeILSWebServerClient(OrgId, ILSAcquisitionsApiKey, ILSAcquisitionsApiPassphrase, ILSBaseAddress);
                    var state = _webServerClient.GetClientAccessToken(new[] { "order" });
                    accessToken = state.AccessToken;
                    currentRetry = retryMaxCount; // Exit Loop - No Retry
                }
                catch (DotNetOpenAuth.Messaging.ProtocolException webEx)
                {

                    var response = ((System.Exception)(webEx)).InnerException == null ? ((System.Exception)(webEx)).Message : (((System.Exception)(webEx)).InnerException).Message;

                    if (response.Contains("The remote name could not be resolved")
                        || response.Contains("Unable to connect to the remote server")
                        || response.Contains("The underlying connection was closed")
                        )
                    {
                        // Errors Caught and retried 
                        // 1. The remote name could not be resolved
                        // 2. Unable to connect to the remote server
                        // 3. Timeout
                        ILSErrorParse("Authentication", "AccessToken", response, "Authentication", "", ILSLogs, "TimeOut");
                        Task.Delay(delay);
                    }
                    else
                    {
                        // Errors Caught that are not retried 
                        // 1. Unauthorized error 401 or error in api key or pass phrase
                        // 2. The remote server returned an error: (404) Not Found. when Authorization or Token end point is not valid
                        // 3. The remote name could not be resolved: When III base adderess is not valid
                        // 4. This message can only be sent over HTTPS.
                        // 5. Unexpected response Content-Type text/html : When Authorization or Token end point is Empty
                        // 6. The remote name could not be resolved : When no network 
                        currentRetry = retryMaxCount; // Exit Loop - No Retry
                        ILSErrorParse("AccessToken", "AccessToken", response, "Authentication", "", ILSLogs, "Others");
                    }

                    Logger.LogException("Method- RequestILSToken() - " + webEx.Message + " - CartId : " + ILSLogs.BasketSummaryId, ExceptionCategory.ILS.ToString(), webEx);
                }
                catch (Exception webEx)
                {
                    // Errors Caught that are not retried 
                    // 1. When api key or pass phrase is Empty
                    // 2. Invalid URI: The URI is empty.  When III base adderess is Empty

                    currentRetry = retryMaxCount; // Exit Loop - No Retry
                    var response = webEx.Message;
                    ILSErrorParse("AccessToken", "AccessToken", response, "Authentication", "", ILSLogs, "Others");
                    Logger.LogException("Method- RequestILSToken() - " + webEx.Message + " - CartId : " + ILSLogs.BasketSummaryId, ExceptionCategory.ILS.ToString(), webEx);
                }

            }
            while (currentRetry < retryMaxCount);

            return accessToken;
        }

        /// <summary>
        /// Invoke ILS Api Submit
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="ILSGlobalSettings"></param>
        /// <param name="marcs"></param>
        /// <param name="ILSResponse"></param>
        private void InvokeILSApiSubmit(string accessToken, string ilsBaseAddress, List<MARCJsonResponse> marcs, string tsUserId, ILSLog ILSLogs, List<ILSLineItemDetail> SuccessOrders)
        {
            GlobalConfigurationHelper ILSGlobalSettings = GlobalConfigurationHelper.Instance;
            var ILSBaseAddress = new Uri(ilsBaseAddress);
            var ILSOrderUrl = ILSGlobalSettings.Get(GlobalConfigurationKey.ILSSubmitOrderPath);

            foreach (var marc in marcs)
            {

                var data = "{\"order\": " + marc.MARCHeader + ", \"marcContentType\": \"application/marc-in-json\"," + marc.MARCBody + "}";
                ExecuteILSRequest(marc.BasketLineItemID, data, accessToken, ILSBaseAddress, ILSOrderUrl, "Order", marc.BTKey, ILSLogs, SuccessOrders);
            }

            //Alert Failure message
            if (string.IsNullOrEmpty(ILSLogs.ErrorDescription))
                UserDAOManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSOrderValidationSuccess, ILSLogs.BasketSummaryId, tsUserId);
            else
                UserDAOManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSOrderValidationError, ILSLogs.BasketSummaryId, tsUserId);
        }

        /// <summary>
        /// Invoke ILS Api Validation
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="ILSGlobalSettings"></param>
        /// <param name="marcs"></param>
        /// <param name="ILSResponse"></param>
        private void InvokeILSApiValidation(string accessToken, string ilsBaseAddress, List<MARCJsonResponse> marcs, string tsUserId, ILSLog ILSLogs, List<ILSLineItemDetail> SuccessOrders)
        {
            GlobalConfigurationHelper ILSGlobalSettings = GlobalConfigurationHelper.Instance;
            var ILSBaseAddress = new Uri(ilsBaseAddress);
            var ILSValidationUrl = ILSGlobalSettings.Get(GlobalConfigurationKey.ILSOrderValidatePath);

            foreach (var marc in marcs)
            {
                ExecuteILSRequest(marc.BasketLineItemID, marc.MARCHeader, accessToken, ILSBaseAddress, ILSValidationUrl, "Validation", marc.BTKey, ILSLogs, SuccessOrders);
                if (ILSLogs.ILSStatus == "Authentication")
                {
                    break;
                }
            }

            if (string.IsNullOrEmpty(ILSLogs.ErrorDescription))
                UserDAOManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSValidationSucess, ILSLogs.BasketSummaryId, tsUserId);
            else
                UserDAOManager.Instance.InsertILSUserAlerts(UserAlertTemplateID.ILSValidationError, ILSLogs.BasketSummaryId, tsUserId);
        }

        /// <summary>
        /// Execute ILS Request
        /// </summary>
        /// <param name="lineItemId"></param>
        /// <param name="data"></param>
        /// <param name="accessToken"></param>
        /// <param name="ILSBaseAddress"></param>
        /// <param name="ILSEndPoint"></param>
        /// <param name="validations"></param>
        private void ExecuteILSRequest(string lineItemId, string data, string accessToken, Uri ILSBaseAddress, string ILSEndPoint, string ILSPhase, string btkey, ILSLog ILSLogs, List<ILSLineItemDetail> SuccessOrders)
        {
            int retryMaxCount = 0;
            int retryDelay = 0;

            if (!int.TryParse(BT.TS360API.Common.Configrations.AppSettings.ILSMaxRetryCount, out retryMaxCount))
                retryMaxCount = 3;
            if (!int.TryParse(BT.TS360API.Common.Configrations.AppSettings.ILSRetryDelayInSecond, out retryDelay))
                retryDelay = 5;

            TimeSpan delay = TimeSpan.FromSeconds(retryDelay);

            int currentRetry = -1;

            do
            {
                currentRetry++;
                try
                {
                    if (string.IsNullOrEmpty(ILSEndPoint))
                    {
                        ILSErrorParse(lineItemId, data, string.Format("Invalid ILS {0} endpoint", ILSPhase), ILSPhase, btkey, ILSLogs, "Others");
                        currentRetry = retryMaxCount; // Exit Loop - No Retry
                        break;
                    }


                    var ILSUrl = new Uri(ILSBaseAddress, ILSEndPoint);

                    // Create a WebClient to POST the request
                    using (var client = new WebClient())
                    {
                        client.Encoding = Encoding.UTF8;
                        // Set the header so it knows we are sending JSON
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        client.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken;
                        // Make the request
                        var response = client.UploadString(ILSUrl, data);
                        if (ILSPhase == "Order")
                        {
                            var jSerializer = new JavaScriptSerializer();
                            ILSOrderSucessResponse iLSResponse = jSerializer.Deserialize(response, typeof(ILSOrderSucessResponse)) as ILSOrderSucessResponse;
                            SuccessOrders.Add(new ILSLineItemDetail() { BasketLineItemID = lineItemId, ILSBIBNumber = iLSResponse.bibId, ILSOrderNumber = iLSResponse.orderId });
                        }
                    }

                    currentRetry = retryMaxCount; // Exit Loop - No Retry
                }
                catch (WebException webEx)
                {
                    if (webEx.Status == WebExceptionStatus.ProtocolError)
                    {
                        // Errors Caught that are not retried 
                        // 1. Invalid Login Id
                        // 2. Token Expired
                        // 3. Invalid Location Code
                        // 4. Invalid Find Code
                        // 5. Wrong Validation End Point
                        // 6. Invalid Vendor
                        // 7. JSON object missing field or field has invalid data

                        string errorDetails = string.Empty;
                        using (StreamReader streamReader = new StreamReader(webEx.Response.GetResponseStream()))
                        {
                            errorDetails = streamReader.ReadToEnd();
                            string phase = errorDetails.Contains("login is not valid") || errorDetails.Contains("invalid_grant") ? "Authentication" : ILSPhase;

                            string errorType = errorDetails.Contains("login is not valid") || errorDetails.Contains("invalid_grant") ? "Login" : string.Empty;
                            errorType = errorDetails.Contains("location") ? "FundLocation" : errorType;
                            errorType = errorDetails.Contains("fund") ? "FundLocation" : errorType;
                            errorType = errorDetails.Contains("vendor is not valid") ? "InvalidVendorCode" : errorType;
                            errorType = string.IsNullOrEmpty(errorType) ? "Others" : errorType;


                            var jSerializer = new JavaScriptSerializer();
                            var errorDescription = string.Empty;
                            ILSResponse iLSResponse = new ILSResponse();
                            try
                            {
                                iLSResponse = jSerializer.Deserialize(errorDetails, typeof(ILSResponse)) as ILSResponse;
                                errorDescription = iLSResponse.description;
                            }
                            catch (Exception)
                            {
                                errorDescription = errorDetails;
                            }

                            ILSErrorParse(lineItemId, data, errorDescription, phase, btkey, ILSLogs, errorType);

                            currentRetry = retryMaxCount; // Exit Loop - No Retry
                        }
                    }
                    else if (webEx.Status == WebExceptionStatus.ConnectFailure || webEx.Status == WebExceptionStatus.Timeout || webEx.Status == WebExceptionStatus.NameResolutionFailure)
                    {
                        // Errors Caught and retried 
                        // 1. Connection Failure
                        // 2. TimeOut
                        // 3. NameResolutionFailure
                        string errorDetails = OrderResources.ILS_ConnectFailureError;
                        ILSErrorParse(lineItemId, data, errorDetails, ILSPhase, btkey, ILSLogs, "TimeOut");
                        Task.Delay(delay);
                    }
                    else
                    {
                        string errorDetails = OrderResources.ILS_ErrorOccured;
                        ILSErrorParse(lineItemId, data, errorDetails, ILSPhase, btkey, ILSLogs, "Others");
                    }

                    // Validation error - skip logging to ELMAH, as we are showing error on the UI
                    if (!string.IsNullOrEmpty(ILSLogs.BasketSummaryId))
                    {
                        Logger.LogException("ExecuteILSRequest_DEBUG - " + webEx.Message + " - CartId : " + ILSLogs.BasketSummaryId, "ExecuteILSRequest_DEBUG", webEx);
                    }
                }
                catch (Exception ex)
                {

                    string errorDetails = OrderResources.ILS_ErrorOccured;
                    ILSErrorParse(lineItemId, data, errorDetails, ILSPhase, btkey, ILSLogs, "Others");

                    // Validation error - skip logging to ELMAH, as we are showing error on the UI
                    if (!string.IsNullOrEmpty(ILSLogs.BasketSummaryId))
                    {
                        var errMsg = ex.Message;
                        if (ex.Message.Equals("The remote server returned an error: (400) Bad Request.", StringComparison.InvariantCultureIgnoreCase))
                            errMsg += " " + ILSLogs.ErrorDescription;

                        Logger.LogException("ExecuteILSRequest_DEBUG -- " + errMsg + " - CartId : " + ILSLogs.BasketSummaryId, "ExecuteILSRequest_DEBUG", ex);
                    }
                }
            }
            while (currentRetry < retryMaxCount);

        }

        /// <summary>
        /// Convert response from ILS and adds to error list
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Response"></param>
        /// <param name="ILSPhase"></param>
        /// <param name="validations"></param>
        private void ILSErrorParse(string LineItemID, string Request, string Response, string ILSPhase, string btkey, ILSLog ILSLog, string errorType)
        {
            ILSLog.ErrorType = errorType;
            ILSLog.ILSStatus = ILSPhase;
            ILSLineItemLog LineItemLog = ILSLog.ILSLineItemLogs.FirstOrDefault(p => p.LineItemId == LineItemID);
            if (LineItemLog != null)
            {
                LineItemLog.ILSAPIRequest = Request;
                LineItemLog.ILSAPIResponse = Response;
                LineItemLog.LineItemId = LineItemID;
                LineItemLog.BTKey = btkey;
                LineItemLog.ErrorType = errorType;
            }
            else
            {
                ILSLog.ILSLineItemLogs.Add(new ILSLineItemLog() { ILSAPIRequest = Request, ILSAPIResponse = Response, LineItemId = LineItemID, BTKey = btkey, ErrorType = errorType });
            }

            switch (errorType)
            {
                case "TimeOut":
                    {
                        ILSLog.ErrorDescription = OrderResources.ILS_ConnectFailureError;
                        break;
                    }
                case "Others":
                    {
                        ILSLog.ErrorDescription = OrderResources.ILS_ErrorOccured;
                        break;
                    }
                case "Login":
                    {
                        ILSLog.ErrorDescription = OrderResources.ILS_InvalidLoginError;
                        break;
                    }
                case "FundLocation":
                    {
                        ILSLog.ErrorDescription = OrderResources.ILS_InvalidFundAndLocationCodeError;
                        break;
                    }
                case "InvalidVendorCode":
                    {
                        ILSLog.ErrorDescription = OrderResources.ILS_InvalidVendorCode;
                        break;
                    }
                default:
                    {
                        ILSLog.ErrorDescription = Response;
                        break;
                    }
            }

        }

        private MARCJsonRequest GetMarcDetails(string marcProfileId, string userId, string cartId, string ilsVendorCode)
        {

            GlobalConfigurationHelper ILSGlobalSettings = GlobalConfigurationHelper.Instance;
            var profile = ProfileService.Instance.GetUserById(userId);
            var org = ProfileService.Instance.GetOrganizationById(profile.OrgId);

            var MARCJsonRequest = new MARCJsonRequest
            {
                SortColumn = profile.CartSortBy==null?string.Empty: profile.CartSortBy,
                SortDirection = profile.CartSortOrder==null ? string.Empty : profile.CartSortOrder,
                IsOCLCEnabled = org.IsOCLCCatalogingPlusEnabled == null ? false: org.IsOCLCCatalogingPlusEnabled.Value,
                FullIndicator = org.IsFullMarcProfile==null? "A":(org.IsFullMarcProfile.Value? "F" : "A"),
                BasketSummaryID = cartId,
                OrgId = profile.OrgId,
                IsOrdered = false, // TODO
                ProfileID = marcProfileId,
                IsBTEmployee = profile.IsBTEmployee == null ? false: profile.IsBTEmployee.Value,
                MarketType = org.WebMarketType,
                TsUserId = userId,
                IsCancelled = false, // TODO     
                ILSAcquisitionsApiKey = org.ILSAcquisitionsApiKey,
                ILSAcquisitionsApiPassphrase = org.ILSAcquisitionsApiPassphrase,
                ILSBaseAddress = org.ILSAcquisitionsApiURL,
                IlsVendor = ilsVendorCode,
                IlsUserId = org.ILSAcquisitionsUserId
            };

            bool marcGridSort;
            List<MARCProfile> marcProfileList = MARCProfilerDAO.Instance.GetMARCProfiles(profile.OrgId, out marcGridSort);
            var selectedProfile = marcProfileList.Where(x => x.MARCProfileId == marcProfileId).FirstOrDefault();
            if (selectedProfile != null)
            {
                MARCJsonRequest.HasInventoryRules = selectedProfile.HasInventoryRules;
            }

            return MARCJsonRequest;
        }
        #endregion

        #region ILSValidation
        
        public string ValidateILSDetail(ILSValidationRequest request)
        {
            string returnMessage = string.Empty;
            ILSLog ILSRespLog = new ILSLog()
            {
                BasketSummaryId = string.Empty,
                RequestDateTime = DateTime.Now,
                ResponseDateTime = DateTime.Now,
                UserId = request.TSUserId
            };

            List<ILSLineItemDetail> SuccessOrders = new List<ILSLineItemDetail>();
            try
            {
                // Get access token 
                string accessToken = RequestValidationILSToken(ILSRespLog, request.ILSApiKey, request.ILSApiSecret, request.ILSUrl);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    //Validate all MARC records
                    InvokeILSApiValidation(request, accessToken, ILSRespLog, SuccessOrders);
                }

            }
            catch (Exception)
            {
                // Validation error - skip logging to ELMAH, as we are showing error on the UI
                //Logger.LogException(ex.Message + " - ValidateILSDetail : ", ExceptionCategory.ILS.ToString(), ex);
                returnMessage = OrderResources.ILS_ValidationUnexpectedError;
            }
            returnMessage = HandleErrorforILSValidation(ILSRespLog);
            return returnMessage;
        }

        public ServiceContracts.ILS.PAPIError ValidatePolarisILSDetail(ILSValidationRequest request)
        {
            var returnError = new ServiceContracts.ILS.PAPIError();

            ILSLog ILSRespLog = new ILSLog()
            {
                BasketSummaryId = string.Empty,
                RequestDateTime = DateTime.Now,
                ResponseDateTime = DateTime.Now,
                UserId = request.TSUserId
            };


            List<ILSLineItemDetail> SuccessOrders = new List<ILSLineItemDetail>();
            StaffUserResponse staffUserResponseJson = new StaffUserResponse();
            try
            {
                String dateTimeFormatRFC = DateTime.UtcNow.ToString("R");

                var papiURL = request.ILSUrl;

                if (papiURL.EndsWith("/") || papiURL.EndsWith("\\"))
                {
                    papiURL = papiURL.Substring(0, papiURL.Length - 1);
                }

                papiURL += "/protected/v1/1033/100/1/authenticator/staff";

                // Hashing 
                string signature = GetPAPIHash(request.ILSApiKey, "POST", papiURL, dateTimeFormatRFC, "");
                StaffUserRequest staffUserRequest = new StaffUserRequest();
                staffUserRequest.domain = request.ILSUserDomain;
                staffUserRequest.username = request.ILSUserAccount;
                staffUserRequest.password = request.ILSApiSecret;
                string staffUserRequestJson = JsonConvert.SerializeObject(staffUserRequest);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(papiURL);

                // append Header 
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", string.Format("PWS {0}:{1}", request.ILSLogin, signature));
                httpWebRequest.Headers.Add("PolarisDate", dateTimeFormatRFC);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(staffUserRequestJson);
                    streamWriter.Flush();
                    streamWriter.Close();
                }


                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    staffUserResponseJson = JsonConvert.DeserializeObject<StaffUserResponse>(result);
                }
                bool isAuthorize = (staffUserResponseJson.PAPIErrorCode == 0);

                var returnMessage = (isAuthorize ? "" : staffUserResponseJson.ErrorMessage);

                if (!isAuthorize && string.IsNullOrEmpty(returnMessage))
                {
                    if (staffUserResponseJson.PAPIErrorCode == -8001)
                        returnMessage = "Invalid User Account or Password.";
                    else
                        returnMessage = OrderResources.ILS_ValidationUnexpectedError;
                }

                returnError.ErrorMessage = returnMessage;
                returnError.PAPIErrorCode = staffUserResponseJson.PAPIErrorCode.ToString();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (ex.Message.Contains("(401) Unauthorized"))
                    {
                        returnError.ErrorMessage = "Unauthorized to access Polaris API.";
                        returnError.PAPIErrorCode = "-401";
                    }
                    else if (ex.Message.Contains("(404) Not Found"))
                    {
                        returnError.ErrorMessage = "The URL is not found.";
                        returnError.PAPIErrorCode = "-404";
                    }
                }
                else
                {
                    returnError.ErrorMessage = OrderResources.ILS_ValidationUnexpectedError + " " + ex.Message;
                    returnError.PAPIErrorCode = "-1";
                }
            }
            catch (Exception ex)
            {
                returnError.ErrorMessage = OrderResources.ILS_ValidationUnexpectedError + " " + ex.Message;
                returnError.PAPIErrorCode = "-1";
            }

            return returnError;
        }

        public static string GetPAPIHash(string strAccessKey, string strHTTPMethod, string strURI, string strHTTPDate, string strPatronPassword)
        {
            byte[] secretBytes = UTF8Encoding.UTF8.GetBytes(strAccessKey);
            HMACSHA1 hmac = new HMACSHA1(secretBytes);
            // Computed hash is based on different elements defined by URI 

            byte[] dataBytes = null;
            if (strPatronPassword.Length > 0)
                dataBytes = UTF8Encoding.UTF8.GetBytes(strHTTPMethod + strURI + strHTTPDate + strPatronPassword);
            else
                dataBytes = UTF8Encoding.UTF8.GetBytes(strHTTPMethod + strURI + strHTTPDate);
            byte[] computedHash = hmac.ComputeHash(dataBytes);
            string computedHashString = Convert.ToBase64String(computedHash);
            return computedHashString;

        } 
        private void InvokeILSApiValidation(ILSValidationRequest validationRequest, string accessToken, ILSLog ILSLogs, List<ILSLineItemDetail> SuccessOrders)
        {
            GlobalConfigurationHelper ILSGlobalSettings = GlobalConfigurationHelper.Instance;
            var ILSBaseAddress = new Uri(validationRequest.ILSUrl);
            var ILSValidationUrl = ILSGlobalSettings.Get(GlobalConfigurationKey.ILSOrderValidatePath);
            var ILSVendor = ILSGlobalSettings.Get(GlobalConfigurationKey.ILSVendor);
            List<MARCJsonResponse> marcs = new List<MARCJsonResponse>();
            marcs = GetTestMarc(validationRequest, ILSVendor);

            foreach (var marc in marcs)
            {
                ExecuteILSRequest(marc.BasketLineItemID, marc.MARCHeader, accessToken, ILSBaseAddress, ILSValidationUrl, "ILSValidation", marc.BTKey, ILSLogs, SuccessOrders);
                if (ILSLogs.ILSStatus == "Authentication")
                {
                    break;
                }
            }
        }

        private List<MARCJsonResponse> GetTestMarc(ILSValidationRequest testRequest, string vendor)
        {
            List<MARCJsonResponse> marc = new List<MARCJsonResponse>();
            marc.Add(new MARCJsonResponse()
            {
                MARCHeader = "{\"login\": \"" + testRequest.ILSLogin + "\",\"copies\": 4,\"allocation\": [{\"location\": \"00\",\"fund\": \"acfic\"}],\"vendor\": \"" + vendor + "\"}"
            });
            return marc;

        }

        private string HandleErrorforILSValidation(ILSLog ILSRespLog)
        {
            string message = OrderResources.ILS_ValidationSucess;
            if (ILSRespLog.ILSLineItemLogs.Count > 0)
            {
                switch (ILSRespLog.ErrorType)
                {
                    case "TimeOut":
                        {
                            message = OrderResources.ILS_ValidationInvalidURLError; // invalid url
                            break;
                        }
                    case "Unauthorized":
                        {
                            message = OrderResources.ILS_ValidationInvalidAPISecretError; // invalid pass phrase or key
                            break;
                        }
                    case "Login":
                        {
                            message = OrderResources.ILS_ValidationInvalidLoginError;
                            break;
                        }
                    case "Others":
                        {
                            message = OrderResources.ILS_ValidationUnexpectedError;
                            break;
                        }
                }

            }
            return message;
        }
        private WebServerClient InitializeILSValidationWebServerClient(string OrgId, string ILSAcquisitionsApiKey, string ILSAcquisitionsApiPassphrase, string authorizationServerUri)
        {
            GlobalConfigurationHelper ILSGlobalSettings = GlobalConfigurationHelper.Instance;
            WebServerClient webServerClient;
            string ILSAuthorizePath = ILSGlobalSettings.Get(GlobalConfigurationKey.ILSAuthorizePath);
            var ILSTokenPath = ILSGlobalSettings.Get(GlobalConfigurationKey.ILSTokenPath);
            var authorizationServer = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(new Uri(authorizationServerUri), ILSAuthorizePath),
                TokenEndpoint = new Uri(new Uri(authorizationServerUri), ILSTokenPath)
            };

            //var apikey = APIEncryptionHelper.Decrypt(ILSAcquisitionsApiKey);
            //var apiphrase = APIEncryptionHelper.Decrypt(ILSAcquisitionsApiPassphrase);
            webServerClient = new WebServerClient(authorizationServer, ILSAcquisitionsApiKey, ILSAcquisitionsApiPassphrase);

            return webServerClient;
        }
        private string RequestValidationILSToken(ILSLog ILSLogs, string ILSAcquisitionsApiKey, string ILSAcquisitionsApiPassphrase, string ILSBaseAddress)
        {

            string accessToken = string.Empty;

            try
            {
                WebServerClient _webServerClient = InitializeILSValidationWebServerClient(string.Empty, ILSAcquisitionsApiKey, ILSAcquisitionsApiPassphrase, ILSBaseAddress);
                var state = _webServerClient.GetClientAccessToken(new[] { "order" });
                accessToken = state.AccessToken;
            }
            catch (DotNetOpenAuth.Messaging.ProtocolException webEx)
            {

                var response = ((System.Exception)(webEx)).InnerException == null ? ((System.Exception)(webEx)).Message : (((System.Exception)(webEx)).InnerException).Message;

                if (response.Contains("The remote name could not be resolved")
                    || response.Contains("Unable to connect to the remote server")
                    || response.Contains("The underlying connection was closed")
                    )
                {
                    // Errors Caught and retried 
                    // 1. The remote name could not be resolved
                    // 2. Unable to connect to the remote server
                    // 3. Timeout
                    ILSErrorParse("Authentication", "AccessToken", response, "Authentication", "", ILSLogs, "TimeOut");
                }
                else if (response.Contains("Unauthorized"))
                {

                    // Errors Caught that are not retried 
                    // 1. Unauthorized error 401 or error in api key or pass phrase
                    ILSErrorParse("AccessToken", "AccessToken", response, "Authentication", "", ILSLogs, "Unauthorized");
                }
                else
                {
                    // Errors Caught that are not retried 
                    // 2. The remote server returned an error: (404) Not Found. when Authorization or Token end point is not valid
                    // 4. This message can only be sent over HTTPS.
                    // 5. Unexpected response Content-Type text/html : When Authorization or Token end point is Empty
                    // 6. The remote name could not be resolved : When no network 
                    ILSErrorParse("AccessToken", "AccessToken", response, "Authentication", "", ILSLogs, "Others");
                }

                // Validation error - skip logging to ELMAH, as we are showing error on the UI
                if (!string.IsNullOrEmpty(ILSLogs.BasketSummaryId))
                {
                    Logger.LogException("Method- RequestValidationILSToken() - " + webEx.Message + " - CartId : " + ILSLogs.BasketSummaryId, ExceptionCategory.ILS.ToString(), webEx);
                }
            }
            catch (Exception webEx)
            {
                // Errors Caught that are not retried 
                // 1. When api key or pass phrase is Empty
                // 2. Invalid URI: The URI is empty.  When III base adderess is Empty
                var response = webEx.Message;
                ILSErrorParse("AccessToken", "AccessToken", response, "Authentication", "", ILSLogs, "Others");

                // Validation error - skip logging to ELMAH, as we are showing error on the UI
                if (!string.IsNullOrEmpty(ILSLogs.BasketSummaryId))
                {
                    Logger.LogException("Method- RequestValidationILSToken() - " + webEx.Message + " - CartId : " + ILSLogs.BasketSummaryId, ExceptionCategory.ILS.ToString(), webEx);
                }
            }

            return accessToken;
        }
        #endregion
    }
}
