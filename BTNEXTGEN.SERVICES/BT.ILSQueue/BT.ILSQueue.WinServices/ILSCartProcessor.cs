using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BT.ILSQueue.WinServices.Helper;
using BT.ILSQueue.Business.Manager;
using BT.ILSQueue.Business.Models;

namespace BT.ILSQueue.WinServices
{
    public class ILSCartProcessor
    {
        public Dictionary<ILSQueueRequest, bool> cartRequests = null;
        private static object lockProcess = new object();

        private int NumberOfILSThreads = 0;
        private int MaxQueueJobStatus = 0;
        private int MaxQueueJobResult = 0;

        public ILSCartProcessor()
        {
            cartRequests = new Dictionary<ILSQueueRequest, bool>();
            NumberOfILSThreads = Convert.ToInt32(AppSettings.NumberOfILSThreads);
            MaxQueueJobStatus = Convert.ToInt32(AppSettings.MaxQueueJobStatus);
            MaxQueueJobResult = Convert.ToInt32(AppSettings.MaxQueueJobResult);
        }

        public void ProcessCarts()
        {
            TextFileLogger.LogInfo("=========== Process Cart BEGIN ===========");
          
            TextFileLogger.LogInfo("=========== Process Cart END ===========");

        }

        public void InitializeILSRequest()
        {
            var pendingOrderCount = OrderManager.Instance.GetILSQueuePendingOrderCount();
            TextFileLogger.LogInfo("Pending Orders: " + pendingOrderCount);

            if (pendingOrderCount > 0)
            {
                List<ILSQueueRequest> ilsRequestList = null;
                if (cartRequests.Where(xx => xx.Value == false).Count() < NumberOfILSThreads
                    && pendingOrderCount > 0)
                {
                    ilsRequestList = OrderManager.Instance.GetILSQueuePendingOrder();
                    foreach (ILSQueueRequest ilsRequest in ilsRequestList)
                    {
                        if (!cartRequests.Any(qry => qry.Key.ExternalID.Equals(ilsRequest.ExternalID)))
                        {
                            cartRequests.Add(ilsRequest, false);
                        }
                    }
                }
            }

            var inProcessCarts = cartRequests.Where(xx => xx.Value == true).ToList();
            var pendingIlsCarts = cartRequests.Where(qry => qry.Value == false).ToList();

            if (inProcessCarts.Count() < NumberOfILSThreads)
            {
                int pendingQueue = NumberOfILSThreads - inProcessCarts.Count();
                for (int ctr = 0; ctr < pendingIlsCarts.Count(); ctr++)
                {
                    if (ctr < pendingQueue)
                    {
                        ILSQueueRequest ilsOrderRequest = pendingIlsCarts.ElementAt(ctr).Key;
                        cartRequests[ilsOrderRequest] = true;
                     
                        lock (lockProcess)
                        {
                            TextFileLogger.LogInfo("Start Validation - BasketSummaryID: " + ilsOrderRequest.ExternalID);
                            cartRequests.Remove(ilsOrderRequest);
                            Task.Factory.StartNew(() => OrderManager.Instance.SubmitILSValidation(ilsOrderRequest));
                        }
                       
                    }
                   
                }

            }
        }

        public void InitializeILSOrder()
        {

            TextFileLogger.LogInfo("Start Retreiving ILS Order Result ");

            Task.Factory.StartNew(() => OrderManager.Instance.GetILSOrderResult(MaxQueueJobResult));
         
        }

        public void InitializeILSJobStatus()
        {
            TextFileLogger.LogInfo("Start checking ILS Job Status");

            Task.Factory.StartNew(() => OrderManager.Instance.CheckJobStatus(MaxQueueJobStatus));
        }
    }
}
