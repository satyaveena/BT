using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO; 
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Reflection;
using BT.TS360API.WebAPI.Services;
using BT.TS360API.WebAPI.Models;
using BT.TS360API.WebAPI.Common;
using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.DataAccess;
using BT.TS360API.WebAPI.ServiceReferenceListTransferAPI;
using BT.TS360API.Security;
using System.Xml.Serialization; 


namespace BT.TS360API.WebAPI.Controllers
{
    public class CreateController : ApiController
    {


   


        /// <summary>
        /// Create a cart
        /// </summary>

        public HttpResponseMessage Post(CreateCart createcart)

        {

            CreateCartResponse results = null;
            results = new CreateCartResponse();

            results.IsSuccessful = true;
            results.PurchaseURL = null;
            string apiKey = null; 
            HttpStatusCode httpStatusCode = HttpStatusCode.Accepted;
            string logRequestLog = AppSetting.CreateEnableTraceLogFile;
            string logRequestFile = AppSetting.CreateEnableTraceRequestFile;
            string guid = Guid.NewGuid().ToString();

            try
            {
                // Logging and Inital check if MODEL is valid 
                //Log Entry into controller to file
                #region FileLogging DEV ONLY
                //if (logRequestLog == "ON")
                //{   FileLogRepository CREATE_LOGFILE = new FileLogRepository(AppSetting.CreateLogFolder, AppSetting.CreateLogFilePrefix);
                //    CREATE_LOGFILE.Write("Initial: " + guid, FileLogRepository.Level.INFO); }
                #endregion

                //Abort when model comes across NULL
                if (createcart == null )
                {
                    results.IsSuccessful = false;
                    results.ErrorType = "1010";
                    results.ErrorMessage = "General Exception: No Content Received";
                    CreateRepository.LogAPIMessage("CreateCart v2","n/a", Request.Content.Headers.ContentType.MediaType.ToString() + " " + guid,"n/a","Exception1 - Invalid Content, Model is invalid OR NULL");
                    #region FileLogging DEV ONLY 
                    //if (logRequestLog == "ON")
                    //{
                    //    FileLogRepository CREATE_LOGFILE = new FileLogRepository(AppSetting.CreateLogFolder, AppSetting.CreateLogFilePrefix);
                    //    CREATE_LOGFILE.Write("Error ... create cart model is NULL: " + guid, FileLogRepository.Level.ERROR); }
                    #endregion
                    return Request.CreateResponse(HttpStatusCode.BadRequest, results);
                }
              
                #region FileLogging 
                if (logRequestLog == "ON")
                {
                    FileLogRepository CREATE_LOGFILE = new FileLogRepository(AppSetting.CreateLogFolder, AppSetting.CreateLogFilePrefix);
                    CREATE_LOGFILE.Write("CreateCart v2" + " " + Request.Content.Headers.ContentType.MediaType.ToString() + " "  + createcart.ToString(), FileLogRepository.Level.INFO);


                    if (logRequestFile == "ON")
                    {

                        CREATE_LOGFILE.Write("Saving request to file for GUID: " + guid, FileLogRepository.Level.INFO);
                        FileReqRepository CREATE_REQFILE = new FileReqRepository(AppSetting.CreateREQFolder, AppSetting.CreateREQFilePrefix, guid);
                        CREATE_REQFILE.Write(createcart.ToString(), FileReqRepository.Level.INFO);
                    }
                }
                #endregion

                //Check cartname is valid 
                if (createcart.CartName == null)
                {
                    results.IsSuccessful = false;
                    results.ErrorType = "1010";
                    results.ErrorMessage = "General Exception: No Content Received";
                    CreateRepository.LogAPIMessage("CreateCart v2", "n/a", Request.Content.Headers.ContentType.MediaType.ToString() + " " + guid, "n/a", "Exception2 - Invalid Content, Cart Name is NULL");

                    System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
                    appLog.Source = "TS360WebAPI";
                    appLog.WriteEntry("CartName is null");


                    #region FileLogging DEV ONLY
                    //if (logRequestLog == "ON")
                    // {
                    //    FileLogRepository CREATE_LOGFILE = new FileLogRepository(AppSetting.CreateLogFolder, AppSetting.CreateLogFilePrefix);
                    //    CREATE_LOGFILE.Write("Error ... create cart name is NULL: " + guid, FileLogRepository.Level.ERROR); }
                    #endregion         
                    return Request.CreateResponse(HttpStatusCode.BadRequest, results);
                    
                 }

                //Validate APIKEY      
                SecurityRepository sp = new SecurityRepository();
                //bool isValidVendor = sp.ValidateAPIKey(Request, out apiKey);
                bool isValidVendor = sp.ValidateAPIKeyDB(Request, out apiKey);
                //Log Entry into database
                CreateRepository.LogAPIMessage("CreateCart v2", createcart.ToString(), Request.Content.Headers.ContentType.MediaType.ToString() + " " + guid, apiKey, "...");

                if (isValidVendor)
                {

                    // check for empty cart -name
                    if  (createcart.CartName == "")
                    {
                        createcart.CartName = "cart:" + DateTime.Now.ToString("yyyyMMdd");
                    }

                    if (string.IsNullOrEmpty(createcart.CartGroup))
                     {
                        createcart.CartGroup = "API";
                     }


                    results.CartName = createcart.CartName;
                    results.Items = new Collection<ItemResponseList>();

                    // check if a list of items have been provided
                    if (createcart.Items.Count <= 0)
                    {
                        results.IsSuccessful = false;
                        results.ErrorType = "1008";
                        results.ErrorMessage = "Missing Item List";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, results);
                    }

                    // determine the target system
                    // TS3 Specific PROCESSING
                    #region TS3Only
                    if (createcart.TargetSystem == "TS3")
                    {
                        // send to the BizTalk Web Service

                        // determine if the required username and password fields are supplied
                        if (string.IsNullOrEmpty(createcart.UserPassword))
                        {
                            results.IsSuccessful = false;
                            results.ErrorType = "1004";
                            results.ErrorMessage = "Invalid TS3 User Credentials: Password, Email and Username are required fields";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, results);
                        }
                        if (string.IsNullOrEmpty(createcart.UserName) || string.IsNullOrEmpty(createcart.UserEmail))
                        {
                            results.IsSuccessful = false;
                            results.ErrorType = "1004";
                            results.ErrorMessage = "Invalid TS3 User Credentials: Password, Email and Username are required fields";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, results);
                        }

                        var rawPassword = string.Empty;
                        try
                        {
                            // decrypt the encrypted password here'
                            rawPassword = DecryptTS3Password(createcart.UserPassword);
                        }
                        catch (Exception ex)
                        {
                            results.IsSuccessful = false;
                            results.ErrorType = "1010";
                            results.ErrorMessage = "Generic Error: " + ex.Message;

                            #region FileLogging DEV ONLY
                            //if (logRequestLog == "ON")
                            //{ FileLogRepository CREATE_LOGFILE = new FileLogRepository(AppSetting.CreateLogFolder, AppSetting.CreateLogFilePrefix);
                            //  CREATE_LOGFILE.Write("Error ... Password related:" + guid + ex.Message, FileLogRepository.Level.ERROR); }
                            #endregion
                            CreateRepository.LogAPIMessage("CreateCart v2", createcart.ToString(), Request.Content.Headers.ContentType.MediaType.ToString() + " " + guid, "n/a", "Exception3 - Generic Error, " + ex.Message);



                            return Request.CreateResponse(HttpStatusCode.InternalServerError, results);
                        }
                        ListTransferAPIClient bizAPI = new ListTransferAPIClient();
                        try
                        {
                            ListTransferParameters ts3Params = new ListTransferParameters();
                            //stTransferParameters ts3Params = new ListTransferParameters(); 

                            // assign request variables to BizTalk object
                            ts3Params.UserName = createcart.UserName;
                            ts3Params.UserEmail = createcart.UserEmail;
                            ts3Params.UserEmailCC = createcart.UserEmailCC;
                            ts3Params.UserPassword = rawPassword;
                            ts3Params.CartNote = createcart.CartNote;
                            ts3Params.CartName = createcart.CartName;
                            ts3Params.CartGroup = createcart.CartGroup;

                            DataTable dtBtKeys = new DataTable();
                            dtBtKeys.Columns.Add("BTKey");


                            // Get ISBNs by BTKey: fill BTKey Table first
                            foreach (var item in createcart.Items)
                            {
                                //DataRow workRow = dtBtKeys.NewRow();
                                //workRow[0] = item.ItemID;
                                //dtBtKeys.Rows.Add(workRow);

                                dtBtKeys.Rows.Add(item.ItemID);
                                
                            }


                            // call stored proc
                            DataTable dtISBNs = new DataTable();

                            dtISBNs = CreateRepository.GetISBNListDAL(dtBtKeys);
                            dtISBNs.Columns.Add("LineNumber");

                            int LineNumber = 0; 
                            
                            foreach (DataRow dr in dtISBNs.Rows)
                            {
                                
                                dtISBNs.Rows[LineNumber].SetField("LineNumber", LineNumber);
                                dr["isbnverified"] = dr["isbnverified"].ToString().Trim();
                                dr["btkey"] = dr["btkey"].ToString().Trim();
                                LineNumber = LineNumber + 1;
                            }

                            
                            dtISBNs.PrimaryKey = new DataColumn[] { dtISBNs.Columns["LineNumber"] };
                            

                            

                            int i = 0;

                            ListTransferParametersItems[] ts3ItemParams = new ListTransferParametersItems[createcart.Items.Count];

                            // process each item in the supplied list
                            foreach (var item in createcart.Items)
                            {
                                //foundRows = dataSet1.Tables["Customers"].Select("CompanyName Like 'A%'");

                                ItemResponseList responseItem = new ItemResponseList(); // API response goes here

                                if (string.IsNullOrEmpty(item.ItemID))
                                {
                                    responseItem.IsSuccessful = false;
                                    responseItem.ErrorType = "2001";
                                    responseItem.Errormessage = "Invalid Item ID";
                                }
                                else
                                {

                                    string isbnverified = "";
                                    ts3ItemParams[i] = new ListTransferParametersItems();

                                    object[] findTheseVals = new object[1];
                                    //findTheseVals[0] = item.ItemID;
                                    findTheseVals[0] = i; 
                                    // get from dataset returned from stored proc
                                    DataRow searchRow = dtISBNs.Rows.Find(findTheseVals);
                                    if (searchRow != null)
                                    {
                                        isbnverified = searchRow["isbnverified"].ToString();
                                    } 


                                    ts3ItemParams[i].ItemID = isbnverified; // change to ISBN after Jamie changes BizTalk
                                    ts3ItemParams[i].Quantity = item.Quantity;
                                    ts3ItemParams[i].PurchaseOrderLineText = item.PurchaseOrderLineText;
                                    responseItem.ItemID = item.ItemID;
                                    responseItem.IsSuccessful = true;

                                }
                                i = i + 1;

                                // add API response
                                results.Items.Add(responseItem);
                            }

                            ts3Params.Items = ts3ItemParams;
                            // call the BizTalk WCF Service
                            bizAPI.SendTS3(ts3Params);

                            results.IsSuccessful = true;
                        }
                        catch (Exception ex)
                        {
                            results.IsSuccessful = false;
                            results.ErrorType = "1010";
                            results.ErrorMessage = "Generic Exception: " + ex.Message;

                            #region FileLogging DEV ONLY
                            //if (logRequestLog == "ON")
                            // {
                            //    FileLogRepository CREATE_LOGFILE = new FileLogRepository(AppSetting.CreateLogFolder, AppSetting.CreateLogFilePrefix);
                            //    CREATE_LOGFILE.Write("Error ... error sending to TS3: " + guid + ex.Message, FileLogRepository.Level.ERROR); }
                            #endregion

                            CreateRepository.LogAPIMessage("CreateCart v2", createcart.ToString(), Request.Content.Headers.ContentType.MediaType.ToString() + " " + guid, "n/a", "Exception4 - Generic Error, " + ex.Message);

                            return Request.CreateResponse(HttpStatusCode.InternalServerError, results);
                        }
                        finally
                        {
                            bizAPI.Close();
                        }

                    }
                    #endregion

                    // TS360 SPECIFIC PROCESSING
                    #region TS360only

                    else if (createcart.TargetSystem == "TS360")
                    {

                        if (string.IsNullOrEmpty(createcart.UserName) || string.IsNullOrEmpty(createcart.UserEmail))
                        {
                            results.IsSuccessful = false;
                            results.ErrorType = "1005";
                            results.ErrorMessage = "TS360 User account not found";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, results);

                        }

                        // get TS360 UserGUID
                        var userId = CreateRepository.ValidateUserDAL(createcart.UserName, createcart.UserEmail);
                        if (!string.IsNullOrEmpty(userId))
                        {

                            // setup cart header table
                            DataTable utblListTransferBasketsType = new DataTable();
                            utblListTransferBasketsType.Columns.Add("CartName");
                            utblListTransferBasketsType.Columns.Add("GroupName");
                            utblListTransferBasketsType.Columns.Add("CartNote");
                            utblListTransferBasketsType.Columns.Add("CSGuid");
                            utblListTransferBasketsType.Columns.Add("SourceSystemx");
                            utblListTransferBasketsType.Columns.Add("BasketLineSplitLimit");
                            utblListTransferBasketsType.Columns.Add("BasketLineOverallLimit");

                            DataRow headerRow = utblListTransferBasketsType.NewRow();
                            headerRow["CartName"] = createcart.CartName; //[CartName] [nvarchar](80) : perform some character trimming to enforce DB constraints
                            headerRow["GroupName"] = createcart.CartGroup; //[GroupName] [nvarchar](50) : perform some character trimming to enforce DB constraints
                            headerRow["CartNote"] = createcart.CartNote; //[CartNote] [nvarchar](200) 
                            headerRow["CSGuid"] = userId; //[CSGuid] [nvarchar](50) 
                            headerRow["SourceSystemx"] = "API"; //[SourceSystemx] [nvarchar](50) : get from vendor translation table
                            headerRow["BasketLineSplitLimit"] = AppSetting.BasketLineSplitLimit; //[BasketLineSplitLimit] [nvarchar](10)  : get from web.config
                            headerRow["BasketLineOverallLimit"] = AppSetting.BasketLineOverallLimit; //[BasketLineOverallLimit] [nvarchar](10) : get from web.config
                            utblListTransferBasketsType.Rows.Add(headerRow);

                            // setup line item list table
                            DataTable utblListTransferBasketDetailsType = new DataTable();
                            utblListTransferBasketDetailsType.Columns.Add("ItemCode");
                            utblListTransferBasketDetailsType.Columns.Add("Isbn");
                            utblListTransferBasketDetailsType.Columns.Add("Upc");
                            utblListTransferBasketDetailsType.Columns.Add("BtKey");
                            utblListTransferBasketDetailsType.Columns.Add("Quantity");
                            utblListTransferBasketDetailsType.Columns.Add("Popl");

                            // Get ISBNs by BTKey: fill BTKey Table first
                            foreach (var item in createcart.Items) 
                            {
                                DataRow itemRow = utblListTransferBasketDetailsType.NewRow();
                                itemRow["ItemCode"] = ""; //[ItemCode] [nvarchar](20) NULL,
                                itemRow["Isbn"] = ""; // item.Isbn; //[Isbn] [nvarchar](20) NULL,
                                itemRow["Upc"] = "";  //item.Upc; //[Upc] [nvarchar](20) NULL,
                                itemRow["BtKey"] = item.ItemID; //[BtKey] [nvarchar](20) NULL,
                                itemRow["Quantity"] = int.Parse(item.Quantity);//[Quantity] [int] NULL,
                                itemRow["Popl"] = item.PurchaseOrderLineText; //[Popl] [nvarchar](50) NULL
                                utblListTransferBasketDetailsType.Rows.Add(itemRow);
                            }

                            DataTable lineItemStatus = new DataTable();


                            var basketGUID = string.Empty;
                            var dbReturn = string.Empty;
                            var dbMessage = string.Empty;

                            // call Stored Proc.
                            lineItemStatus = CreateRepository.CreateCart(utblListTransferBasketsType, utblListTransferBasketDetailsType, out basketGUID, out dbReturn, out dbMessage);
                            //lineItemStatus.PrimaryKey = new DataColumn[] { lineItemStatus.Columns["BTkey"] };
                            //changed because some vendors are supplying duplicate keys
                            lineItemStatus.PrimaryKey = new DataColumn[] { lineItemStatus.Columns["LineNumber"] };


                            if (dbReturn == "50026")
                            {
                                results.IsSuccessful = false;
                                results.ErrorType = "1010";
                                results.ErrorMessage = "Generic Exception: " + dbMessage;

                                return Request.CreateResponse(HttpStatusCode.BadRequest, results);

                            }

                            // the DB output is not always trimmed this is just a precautionary measure
                            foreach (DataRow dr in lineItemStatus.Rows)
                            {
                                //listItemErrors.Add(dr);
                                dr["BTkey"] = dr["BTkey"].ToString().Trim();
                                dr["LoadError"] = dr["LoadError"].ToString().Trim();
                            }


                            // set these values based on stored procedure response
                            int currentlinenumber = 0; 
                            foreach (var item in createcart.Items)
                            {
                                ItemResponseList responseItem = new ItemResponseList();
                                responseItem.ItemID = item.ItemID;
                                currentlinenumber = currentlinenumber + 1; 
                                if ((lineItemStatus.Rows.Count == 0) && (dbReturn == "0"))
                             
                                {
                                    // all line items were successful
                                    responseItem.IsSuccessful = true;

                                }
                                else
                                {

                                    if (string.IsNullOrEmpty(item.ItemID) || dbReturn == "50017")
                                    {
                                        responseItem.IsSuccessful = false;
                                        responseItem.ErrorType = "2001";
                                        responseItem.Errormessage = "Invalid Item ID";
                                    }
                                    else if (dbReturn == "50004")
                                    {
                                        responseItem.IsSuccessful = false;
                                        responseItem.ErrorType = "2001";
                                        responseItem.Errormessage = "Error Inserting Into Temp Table basketlineitems";
                                    }
                                    else
                                    {

                                        object[] findTheseVals = new object[1];


                                        //}
                                        //findTheseVals[0] = item.ItemID;
                                        findTheseVals[0] = currentlinenumber;

                                        //}
                                        // get from dataset returned from stored proc
                                        DataRow searchRow = lineItemStatus.Rows.Find(findTheseVals);

                                        if (searchRow != null)
                                        {
                                            var liError = searchRow["LoadError"].ToString();

                                            if (liError == "Invalid")
                                            {
                                                responseItem.IsSuccessful = false;
                                                responseItem.ErrorType = "2002";
                                                responseItem.Errormessage = "Item not found";
                                            }
                                            else if (liError == "Excluded")
                                            {
                                                responseItem.IsSuccessful = false;
                                                responseItem.ErrorType = "2003";
                                                responseItem.Errormessage = "Restricted Item";
                                            }
                                            else if (liError == "Duplicate")
                                            {
                                                responseItem.IsSuccessful = false;
                                                responseItem.ErrorType = "2004";
                                                responseItem.Errormessage = "Duplicate Item";
                                            }
                                            else
                                            {
                                                responseItem.IsSuccessful = false;
                                                responseItem.ErrorType = "2001";
                                                responseItem.Errormessage = "Invalid Item ID";

                                            }
                                        }
                                        else
                                        {
                                            // item was added successfully
                                            responseItem.IsSuccessful = true;
                                        }
                                    }
                                }
                                results.Items.Add(responseItem);
                            }
                            // added this part in so that it will return false and bad http response code when all invalid items
                            // or error inserting into line items.  

                            if (dbReturn == "50004" || dbReturn == "50017")
                            {

                                results.IsSuccessful = false;
                                results.ErrorType = "1010";
                                results.ErrorMessage = "Generic Exception: " + dbMessage;
                                return Request.CreateResponse(HttpStatusCode.BadRequest, results);

                            }
                            else
                            {
                                results.PurchaseURL = AppSetting.baseURL + basketGUID;
                                results.IsSuccessful = true;
                            }

                        }
                        else
                        {
                            results.IsSuccessful = false;
                            results.ErrorType = "1005";
                            results.ErrorMessage = "TS360 User account not found";
                            return Request.CreateResponse(HttpStatusCode.BadRequest, results);
                        }
                    }
                    #endregion


                    else
                    {

                        results.IsSuccessful = false;
                        results.ErrorType = "1002";
                        results.ErrorMessage = "Invalid Target System";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, results);
                    }
                }
                else
                {
                    results.IsSuccessful = false;
                    results.ErrorType = "1001";
                    results.ErrorMessage = "Invalid API Key" + apiKey;
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, results);


                }

                //Build the response message
                //var response = Request.CreateResponse<CreateCart>(httpStatusCode, createcart);
                return Request.CreateResponse(HttpStatusCode.OK, results);
            } 

            catch (Exception ex)
            {
                //Log 2 DB, File and Email Exception... 

                #region FileLogging DEV ONLY
                //if (logRequestLog == "ON")
                //{   FileLogRepository CREATE_LOGFILE = new FileLogRepository(AppSetting.CreateLogFolder, AppSetting.CreateLogFilePrefix);
                //    CREATE_LOGFILE.Write("Error: " + ex.Message + " guid: " + guid, FileLogRepository.Level.ERROR); }
                #endregion 

                System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
                appLog.Source = "TS360WebAPI";
                appLog.WriteEntry("Error in CreateCart Method: " + ex.Message);

                EmailExceptions.SendEmailException("Exception occurred: " + ex.Message, "CreateCart v2", AppSetting.CreateEmailExceptionFlag.ToLower(), guid);
      
                CreateRepository.LogAPIMessage("CreateCart v2", createcart.ToString(), Request.Content.Headers.ContentType.MediaType.ToString() + " " + guid, "n/a", "Exception5 - Generic Error, " + ex.Message);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
            
            }

        }


        // Private method 

        private string DecryptTS3Password(string textToDecrypt)
        {

            string plainText = string.Empty;       // decrypted text
            string passPhrase = AppSetting.passPhrase;     // can be any string
            string initVector = AppSetting.initVector; // must be 16 bytes


            RijndaelEnhanced rijndaelKey =
                new RijndaelEnhanced(passPhrase, initVector);
            plainText = rijndaelKey.Decrypt(textToDecrypt);

            return plainText;
        }




    }
}


