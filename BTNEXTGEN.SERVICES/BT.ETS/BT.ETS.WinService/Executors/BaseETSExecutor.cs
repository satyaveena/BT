using BT.ETS.Business.Constants;
using BT.ETS.Business.DAO;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Models;
using BT.ETS.Business.MongDBLogger.ELMAHLogger;
using BT.ETS.WinService.Helper;
using Elmah;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Executors
{
    public abstract class BaseETSExecutor
    {
        protected Dictionary<string, string> APIKey;

        private ErrorLog _logger;

        protected ErrorLog logger
        {
            get
            {
                if (_logger == null)
                    _logger = new ELMAHMongoLogger();
                return _logger;
            }
        }

        public BaseETSExecutor()
        {
            APIKey = new Dictionary<string, string>
                    {
                        { "X-API-KEY", AppSettings.ESPVendorKey }
                    };
        }

        public abstract string MongoResponseFieldName { get; }

        public abstract string ETSPostBackServiceUrl { get; }

        /// <summary>
        /// 1) Update Request Status. 2) Send result to ETS service. 3) Update Response Status.
        /// 4) Send email notification if exception happens.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobID"></param>
        /// <param name="postbackResult"></param>
        protected async Task SendResultToESTService<T>(MongoDB.Bson.ObjectId jobID, EtsServiceQueueBackgroundResult<T> postbackResult)
        {
            if (jobID == null || postbackResult == null)
                return;

            string requestString = JsonConvert.SerializeObject(postbackResult);

            // log request text
            TextFileLogger.LogDebug(string.Format(" ETS Postback Request: {0}.", requestString));

            // Call ETS API (send result to EST Team)
            var retryWaitTimesList = AppSettings.RetryPeriods.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            int retryCount = 0;
            var maxRetries = retryWaitTimesList.Count;

            // RETRY LOGIC
            while (retryCount < maxRetries)
            {
                try
                {
                    // Save ETS request to MongDB
                    await CommonDAO.Instance.SetETSRequestStatus(jobID, postbackResult.StatusCode, postbackResult.StatusMessage);

                    // Send result to ETS service
                    var etsServiceResponse = HTTP.PostQueue(ETSPostBackServiceUrl, requestString, APIKey);

                    // log response text
                    TextFileLogger.LogDebug(" ETS Postback Response", etsServiceResponse);

                    // Save ETS respose to MongDB
                    await CommonDAO.Instance.SetETSResponseStatus(jobID, etsServiceResponse.StatusCode, etsServiceResponse.StatusMessage);
                    break;
                }
                // RETRY x times and then exit program
                catch (Exception ex)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        var errorMesasge = string.Format("Unable to send Results to ETS service. Url: {0}. {1}", ETSPostBackServiceUrl, ex.Message);
                        var exception = new PostBackResultException(errorMesasge, ex);
                        throw exception;
                    }
                }
            }
        }
    }
}
