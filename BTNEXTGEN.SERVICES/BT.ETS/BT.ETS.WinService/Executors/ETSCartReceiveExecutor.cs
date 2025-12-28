using BT.ETS.Business.Handler;
using BT.ETS.Business.Constants;
using BT.ETS.Business.DAO;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Models;
using BT.ETS.WinService.Constants;
using BT.ETS.WinService.Helper;
using Elmah;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Executors
{
    public class ETSCartReceiveExecutor : BaseETSExecutor, IQueueItemExecutor
    {
        public override string MongoResponseFieldName { get { return "CartReceivedResponse"; } }

        public override string ETSPostBackServiceUrl { get { return AppSettings.EspReceiveCartReceivedUrl; } }

        public async Task ExecuteRequest(ETSQueueItem etsQueueItem)
        {
            TextFileLogger.LogInfo("CartReceived Executing. JobID: " + etsQueueItem.JobID);

            try
            {
                // use to sent to EST service
                var etsResultPostBack = new EtsServiceQueueBackgroundResult<InsertedCartResult>();
                etsResultPostBack.JobId = etsQueueItem.JobID.ToString();

                var cartReceivedResult = new InsertedCartResult();
                var isError = false;
                try
                {
                    // insert cart and get response
                    cartReceivedResult = await ETSRequestHandler.InsertEtsCart(etsQueueItem.CartReceivedRequest, etsQueueItem.JobID);
                    etsResultPostBack.Data = cartReceivedResult;
                    if (cartReceivedResult.ErrorItems != null && cartReceivedResult.ErrorItems.Count > 0)
                    {
                        etsResultPostBack.StatusCode = BusinessExceptionConstants.ITEMS_VALIDATION_FAILED;
                        etsResultPostBack.StatusMessage = BusinessExceptionConstants.Message(etsResultPostBack.StatusCode);
                    }
                }
                catch (BusinessException ex)
                {
                    TextFileLogger.LogInfo(string.Format("CartReceived BusinessException - Insert Cart: {0}. StackTrace: {1}", ex.Message, ex.StackTrace));

                    etsResultPostBack.StatusCode = ex.ErrorCode;
                    etsResultPostBack.StatusMessage = ex.Message;
                    etsResultPostBack.Data = new InsertedCartResult(); //TFS36707
                }
                catch (Exception ex)
                {
                    isError = true;
                    TextFileLogger.LogException(string.Format("CartReceived Exception JobID: {0} - Insert Cart: {1}", etsQueueItem.JobID, ex.Message), ex);

                    etsResultPostBack.StatusCode = BusinessExceptionConstants.UNEXPECTED_EXCEPTION;
                    etsResultPostBack.StatusMessage = BusinessExceptionConstants.Message(etsResultPostBack.StatusCode);
                    etsResultPostBack.Data = new InsertedCartResult(); //TFS36707

                    ETSQueueService.MongoLogger.Log(new Elmah.Error(ex));

                    // send notification email with error message
                    var emailHelper = new EmailHelper();
                    var emailBody = string.Format("JobID: {0}. Error: {1}", etsQueueItem.JobID, ex.Message);
                    emailHelper.Send(emailBody);
                }

                // handle exception
                if (isError)
                {
                    // Set item Failed
                    await CommonDAO.Instance.SetETSQueueItemRequestStatusFailed(etsQueueItem.JobID, etsResultPostBack.StatusCode, etsResultPostBack.StatusMessage);

                    // send Empty Result To EST Service with error code
                    await SendResultToESTService(etsQueueItem.JobID, etsResultPostBack);

                    return;
                }

                TextFileLogger.LogInfo("CartReceived got Result successfully. JobID: " + etsQueueItem.JobID);

                // Update ETS Queue with DupCheck Result
                await CommonDAO.Instance.UpdateETSQueueItem(etsQueueItem.JobID, this.MongoResponseFieldName, cartReceivedResult);

                // send Result To EST Service
                await SendResultToESTService(etsQueueItem.JobID, etsResultPostBack);

                await CommonDAO.Instance.UpdateETSQueueStatus(etsQueueItem.JobID, (int)QueueProcessState.InProcess, (int)QueueProcessState.Success);
            }
            catch (Exception ex)
            {
                TextFileLogger.LogException(string.Format("CartReceived ExecuteRequest Exception JobID: {0}", etsQueueItem.JobID), ex);
                throw;
            }

            TextFileLogger.LogInfo("CartReceived Finished. JobID: " + etsQueueItem.JobID);
        }
    }
}
