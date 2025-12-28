using BT.ETS.Business.Handler;
using BT.ETS.Business.Constants;
using BT.ETS.Business.DAO;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Models;
using BT.ETS.WinService.Constants;
using BT.ETS.WinService.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Executors
{
    public class ETSPricingExecutor : BaseETSExecutor, IQueueItemExecutor
    {
        public override string MongoResponseFieldName { get { return "PricingResponse"; } }

        public override string ETSPostBackServiceUrl { get { return AppSettings.EspReceivePricingUrl; } }

        public async Task ExecuteRequest(ETSQueueItem etsQueueItem)
        {
            TextFileLogger.LogInfo("Pricing Executing. JobID: " + etsQueueItem.JobID);

            try
            {
                // use to sent to EST service
                var etsResultPostBack = new EtsServiceQueueBackgroundResult<ProductPricingResult>();
                etsResultPostBack.JobId = etsQueueItem.JobID.ToString();

                var pricingResult = new ProductPricingResult();
                var isError = false;
                try
                {
                    // call DLL to get respose 123
                    pricingResult = await ETSRequestHandler.GetProductPricing(etsQueueItem.PricingRequest);
                    etsResultPostBack.Data = pricingResult;
                    if (pricingResult.ErrorItems != null && pricingResult.ErrorItems.Count > 0)
                    {
                        etsResultPostBack.StatusCode = BusinessExceptionConstants.PRODUCTS_VALIDATION_FAILED;
                        etsResultPostBack.StatusMessage = BusinessExceptionConstants.Message(etsResultPostBack.StatusCode);
                    }
                }
                catch (BusinessException ex)
                {
                    TextFileLogger.LogInfo(string.Format("Pricing BusinessException - GetProductPricing: {0}. StackTrace: {1}", ex.Message, ex.StackTrace));

                    etsResultPostBack.StatusCode = ex.ErrorCode;
                    etsResultPostBack.StatusMessage = ex.Message;
                    etsResultPostBack.Data = new ProductPricingResult(); //TFS36707
                }
                catch (Exception ex)
                {
                    isError = true;
                    TextFileLogger.LogException(string.Format("Pricing Exception JobID: {0} - GetProductPricing: {1}", etsQueueItem.JobID, ex.Message), ex);

                    etsResultPostBack.StatusCode = BusinessExceptionConstants.UNEXPECTED_EXCEPTION;
                    etsResultPostBack.StatusMessage = BusinessExceptionConstants.Message(etsResultPostBack.StatusCode);
                    etsResultPostBack.Data = new ProductPricingResult(); //TFS36707

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

                TextFileLogger.LogInfo("Pricing got Result successfully. JobID: " + etsQueueItem.JobID);

                // Update ETS Queue with Pricing Result
                await CommonDAO.Instance.UpdateETSQueueItem(etsQueueItem.JobID, this.MongoResponseFieldName, pricingResult);

                // send Result To EST Service
                await SendResultToESTService(etsQueueItem.JobID, etsResultPostBack);

                await CommonDAO.Instance.UpdateETSQueueStatus(etsQueueItem.JobID, (int)QueueProcessState.InProcess, (int)QueueProcessState.Success);
            }
            catch (Exception ex)
            {
                TextFileLogger.LogException(string.Format("Pricing ExecuteRequest Exception JobID: {0}", etsQueueItem.JobID), ex);
                throw;
            }

            TextFileLogger.LogInfo("Pricing Finished. JobID: " + etsQueueItem.JobID);
        }
    }
}
