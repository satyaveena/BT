using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Results;

using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.DataAccess;
using BT.TS360API.WebAPI.Common;
using BT.TS360API.WebAPI.Models;
using BT.TS360API.WebAPI.Services;

namespace BT.TS360API.WebAPI.Controllers
{
    //[RoutePrefix("cart/distribute")]
    public class DistributedController : ApiController
    {
        private DistributedRepository distributedRepository;

        public DistributedController()
        {
            this.distributedRepository = new DistributedRepository();
        }

        /// <summary>
        /// Gets all distributed items
        /// </summary>
        //[HttpGet]
        public DistributedItem[] Get()
        {
            bool enableCheckCache = (AppSetting.ESPDistEnableCheckCache.ToUpper() == "ON");

            if (enableCheckCache)
                return distributedRepository.GetAllDistributedItems();
            else
                return new DistributedItem[] { };
        }




        /// <summary>
        ///Updates distributed overall qty, branch qty
        /// </summary>
        public HttpResponseMessage Post(DistributedItem distributedItem)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.Accepted;
            try
            {
            //..log request and confirm APIKEY is valid...   
                string distError = "";
                string apiKey;

                if (distributedItem == null)
                {
                    distError = "Invalid Request - Check JSON";
                    throw new Exception(distError);

                }


                SecurityRepository sp = new SecurityRepository();
                bool isValidVendor = sp.ValidateAPIKeyDB(Request, out apiKey);


                string strDistributedItem = distributedItem.ToString();
                string requestMessage = strDistributedItem;

            //..create and save the request log...    
                var request = Request.CreateResponse<DistributedItem>(httpStatusCode, distributedItem);
                string webMethod = "DistributeCart";

            //..initialize the file logging and tracing. 
                bool hasLoggedToFile = false;
                FileLogRepository fileLog = new FileLogRepository(AppSetting.ESPDistLogFolder, AppSetting.ESPDistLogFilePrefix);
                string logFileMessage = string.Empty;
                bool enableTrace = (AppSetting.ESPDistEnableTrace.ToUpper() == "ON");

                if (enableTrace)
                {
                    logFileMessage = string.Format("[{0}] {1}", "INIT", strDistributedItem);
                    fileLog.Write(logFileMessage, FileLogRepository.Level.INFO);
                }


            //..save request, request to TS360APILog table  
            //    try
            //    {
            //        ExceptionLoggingDAO logger = new ExceptionLoggingDAO();
            //        logger.LogRequest(webMethod, requestMessage, "", apiKey, "request");
            //    }
            //    catch (Exception ex)
            //    {
            //        //Log distribution to file if WEBAPI has errors. 
            //        logFileMessage = string.Format("[{0}] [{1}] [{2}] [{3}] {4}", "", apiKey, distError, ex.Message, strDistributedItem);
            //        fileLog.Write(logFileMessage, FileLogRepository.Level.ERROR);
            //        hasLoggedToFile = true;
            //    }



            //..check boolean flag to see if valid key
                if (!isValidVendor)
                {
                    distError = "Invalid API Key";
                    throw new Exception(distError); 
                }



            //..main create, save response to DB, change http status, send alert
                this.distributedRepository.SaveDist(distributedItem);
                httpStatusCode = HttpStatusCode.Created;
                this.distributedRepository.SendAlert(distributedItem, out distError);

            //..create response
                var response = Request.CreateResponse<DistributedItem>(httpStatusCode, distributedItem);
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string responseCode = response.StatusCode.ToString();
                string responseReason = response.ReasonPhrase;
                string responseMessage = string.Format("Version:{0} | ResponseCode:{1} | ResponseReason:{2}", version, responseCode, responseReason);

            //..save request / response, request to TS360APILog table  
                try
                {
                    ExceptionLoggingDAO logger = new ExceptionLoggingDAO();
                    logger.LogRequest(webMethod, requestMessage, responseMessage, apiKey, "");
                }
                catch (Exception ex)
                {
                    //Log distribution to file if WEBAPI has errors. 
                    logFileMessage = string.Format("[{0}] [{1}] [{2}] [{3}] {4}", "", apiKey, distError, ex.Message, strDistributedItem);
                    fileLog.Write(logFileMessage, FileLogRepository.Level.ERROR);
                    hasLoggedToFile = true;
                }


            //..log distribution response to file if the file log flag is ON
                if (enableTrace && !hasLoggedToFile)
                {
                    logFileMessage = string.Format("[{0}] {1}", responseCode, strDistributedItem);
                    fileLog.Write(logFileMessage, FileLogRepository.Level.INFO);
                 
                }

                return response;
            }
            catch (Exception ex)
            {
                
                System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog();
                appLog.Source = "TS360WebAPI";
                appLog.WriteEntry("Error in DistributeCart Method: " + ex.Message);

                EmailExceptions.SendEmail(AppSetting.ESPEmailTo, "DistributeCart", "Exception occurred: " + ex.Message, AppSetting.ESPEmailServer, AppSetting.ESPEmailFlag.ToLower());

                if (ex.Message == "Invalid Request - Check JSON")
                    { CreateRepository.LogAPIMessage("DistributeCart", ex.Message, "", "n/a", ex.Message);      }
                else 
                    { CreateRepository.LogAPIMessage("DistributeCart", distributedItem.ToString(), Request.Content.Headers.ContentType.MediaType.ToString(), "n/a", "Exception - Generic Error, " + ex.Message); }
                
                FileLogRepository fileLogEXC = new FileLogRepository(AppSetting.ESPDistLogFolder, AppSetting.ESPDistLogFilePrefix);
                fileLogEXC.Write(ex.Message  , FileLogRepository.Level.ERROR);

                if (ex.Message == "distributed qty <> branch qty")
                {
                    var response = Request.CreateResponse<DistributedItem>(HttpStatusCode.BadRequest, distributedItem);
                    return response; 
                }
                else 
                if (ex.Message == "Invalid API Key")
                {
                    var response = Request.CreateResponse<DistributedItem>(HttpStatusCode.Unauthorized, distributedItem);
                    return response; 
                }
                else
                {
                    var response = Request.CreateResponse<DistributedItem>(HttpStatusCode.InternalServerError, distributedItem);
                    return response;  
                }
            }

        }
    }



}

