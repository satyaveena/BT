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

using BT.TS360API.WebAPI.Common;
using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.DataAccess;
using BT.TS360API.WebAPI.Models;
using BT.TS360API.WebAPI.Services;

namespace BT.TS360API.WebAPI.Controllers
{
    //[RoutePrefix("cart/ranked")]
    public class RankedController : ApiController
    {
        private RankedRepository rankedRepository;

        public RankedController()
        {
            this.rankedRepository = new RankedRepository();
        }

        /// <summary>
        /// Gets all ranked items
        /// </summary>
        /// Gets All Ranked Items
        //[Route("cart/ranked/getall")]
        //[HttpGet]
        public RankedItem[] Get()
        {
            bool enableCheckCache = (AppSetting.ESPRankEnableCheckCache.ToUpper() == "ON");
         
            if (enableCheckCache)
                return rankedRepository.GetAllRankedItems();
            else
                return new RankedItem[] { };
        }



        /// <summary>
        /// Gets a specific ranked item
        /// </summary>
        /// Gets ranked item
        public JsonResult<RankedItem> GetRankItem(int id)
        {
            return Json(new RankedItem()
            {
                ESPLibraryId = "1",
                CartId = "2"
            });

           
        }

        /// Creates a ranked item
        /// <summary>
        /// Creates a ranked item 
        /// </summary>
        public HttpResponseMessage Post(RankedItem rankItem)
        {
            try {

            HttpStatusCode httpStatusCode = HttpStatusCode.Accepted;
            string rankError = "";
            string apiKey;
            string strRankedItem = rankItem.ToString();

            SecurityRepository sp = new SecurityRepository();
            //bool isValidVendor = sp.ValidateAPIKey(Request, out apiKey);
            bool isValidVendor = sp.ValidateAPIKeyDB(Request, out apiKey);

            if (!isValidVendor)
            {
                rankError = "Invalid API Key";
                httpStatusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                rankError = this.rankedRepository.SaveRank(rankItem);
                httpStatusCode = (rankError == "") ? HttpStatusCode.Created : HttpStatusCode.InternalServerError;

                if (rankError == "" && rankItem.CartId.StartsWith("AutoRank") == false)
                    this.rankedRepository.SendAlert(rankItem, out rankError);
            }

            if (!string.IsNullOrEmpty(rankError))
            {
                EmailExceptions.SendEmail(AppSetting.ESPEmailTo, "RankCart", "Exception occurred: " + rankError, AppSetting.ESPEmailServer, AppSetting.ESPEmailFlag.ToLower());
            }

            var response = Request.CreateResponse<RankedItem>(httpStatusCode, rankItem);
            string webMethod = response.RequestMessage.RequestUri.LocalPath;
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
            string responseCode = response.StatusCode.ToString();
            string responseReason = response.ReasonPhrase;
            string responseMessage = string.Format("Version:{0} | ResponseCode:{1} | ResponseReason:{2}",version, responseCode, responseReason);
            string requestMessage = strRankedItem;

            //var requestRankItem = new ObjectContent<RankedItem>(rankItem, Configuration.Formatters.JsonFormatter);

            bool hasLoggedToFile = false;
            FileLogRepository fileLog = new FileLogRepository(AppSetting.ESPRankLogFolder, AppSetting.ESPRankLogFilePrefix);
            string logFileMessage = string.Empty;
            bool enableTrace = (AppSetting.ESPRankEnableTrace.ToUpper() == "ON");

            try
            {
                ExceptionLoggingDAO logger = new ExceptionLoggingDAO();
                logger.LogRequest(webMethod, requestMessage, responseMessage, apiKey, rankError);
            }
            catch (Exception ex)
            {
                // Log rank to file if WEBAPI cannot connect to database
                logFileMessage = string.Format("[{0}] [{1}] [{2}] [{3}] {4}", responseCode, apiKey, rankError, ex.Message, strRankedItem);
                fileLog.Write(logFileMessage, FileLogRepository.Level.ERROR);
                hasLoggedToFile = true;
            }
                
            // Log rank to file if the file log flag is ON
            if (enableTrace && !hasLoggedToFile)
            {
                logFileMessage = string.Format("[{0}] {1}", responseCode, strRankedItem);
                fileLog.Write(logFileMessage, FileLogRepository.Level.INFO);
            }

            return response;
            
            }
            catch (Exception ex)
            {
                EmailExceptions.SendEmail(AppSetting.ESPEmailTo, "RankCart", "Exception occurred: " + ex.Message, AppSetting.ESPEmailServer, AppSetting.ESPEmailFlag.ToLower());

                if (rankItem == null)
                {
                    rankItem = new RankedItem();
                }

                var response = Request.CreateResponse<RankedItem>(HttpStatusCode.InternalServerError, rankItem);
                return response;
            }
        }

       
       
    }
}
