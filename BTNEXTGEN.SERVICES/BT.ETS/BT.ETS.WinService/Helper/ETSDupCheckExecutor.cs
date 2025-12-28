using BT.ETS.Business.Constants;
using BT.ETS.Business.DAO;
using BT.ETS.Business.Models;
using BT.ETS.WinService.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Helper
{
    public class ETSDupCheckExecutor : BaseETSExecutor, IQueueItemExecutor
    {
        public override string MongoResponseFieldName { get { return "DupCheckResponse"; } }

        public void ExecuteRequest(ETSQueueItem etsQueueItem)
        {
            // call DLL to get respose
            var dupCheckResult = _orderServices.GetDupChecks(etsQueueItem.DupCheckRequest).Result;

            if (dupCheckResult == null)
            {
                CommonDAO.Instance.UpdateETSQueueStatus(etsQueueItem.JobID, (int)QueueProcessState.InProcess, (int)QueueProcessState.Failed);
                return;
            }

            // Update ETS Queue with DupCheck Result
            CommonDAO.Instance.UpdateETSQueueItem(etsQueueItem.JobID, this.MongoResponseFieldName, dupCheckResult);

            string requestString = Newtonsoft.Json.JsonConvert.SerializeObject(dupCheckResult);

            // Call ETS API (send result to EST Team)
            var retryWaitTimesList = AppSettings.RetryPeriods.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            int retryCount = 0;
            var maxRetries = retryWaitTimesList.Count;

            ////RETRY LOGIC
            dynamic ESTResult = null;
            while (retryCount < maxRetries)
            {
                try
                {
                    ESTResult = HTTP.PostQueue(AppSettings.EspReceiveDupCheckUrl, requestString, APIKey);

                    // Save ETS respose to MongDB
                    CommonDAO.Instance.UpdateETSQueueItem(etsQueueItem.JobID, "ETSResponseStatusMessage", ESTResult);
                    break;
                }

                //RETRY x times and then exit program
                catch (Exception ex)
                {
                    retryCount++;
                    Thread.Sleep(Convert.ToInt32(retryWaitTimesList[retryCount - 1]));
                    if (retryCount >= maxRetries)
                    {
                        logger.Log(new Elmah.Error(ex));
                        CommonDAO.Instance.UpdateETSQueueStatus(etsQueueItem.JobID, (int)QueueProcessState.InProcess, (int)QueueProcessState.Failed);

                        // send email
                        var emailHelper = new EmailHelper();
                        emailHelper.Send(ex.Message);
                        
                        throw;
                    }
                }
            }


            // Save ETS respose to MongDB
            CommonDAO.Instance.UpdateETSQueueItem(etsQueueItem.JobID, "ESPResponseStatusMessage", ESTResult);

            CommonDAO.Instance.UpdateETSQueueStatus(etsQueueItem.JobID, (int)QueueProcessState.InProcess, (int)QueueProcessState.Success);
        }
    }
}
