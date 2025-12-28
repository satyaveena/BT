using BT.ETS.Business.Constants;
using BT.ETS.Business.DAO;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Models;
using BT.ETS.Business.MongDBLogger.ELMAHLogger;
using BT.ETS.WinService.Constants;
using BT.ETS.WinService.Executors;
using BT.ETS.WinService.Helper;
using Elmah;
using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BT.ETS.WinService
{
    public partial class ETSQueueService : ServiceBase
    {
        static System.Timers.Timer _serviceTimer = new System.Timers.Timer(AppSettings.TimerInterval * 1000);
        public static System.Diagnostics.EventLog _etsServiceLog;

        private static ErrorLog _mongoLogger;

        public static ErrorLog MongoLogger
        {
            get
            {
                if (_mongoLogger == null)
                    _mongoLogger = new ELMAHMongoLogger();
                return _mongoLogger;
            }
        }

        private static object locker = new object();

        public ETSQueueService()
        {
            InitializeComponent();

            _etsServiceLog = etsServiceLog;
        }

        protected override void OnStart(string[] args)
        {
            etsServiceLog.WriteEntry("############### ETS service STARTED ###############");

            log4net.Config.BasicConfigurator.Configure();

            _serviceTimer.AutoReset = true;
            _serviceTimer.Enabled = true;
            _serviceTimer.Elapsed += TimerElapsed; // fire in different thread
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                TextFileLogger.LogInfo("=========== Timer fired Elapsed BEGIN ===========");

                await ExecuteETSQueueRequests();

                TextFileLogger.LogInfo("=========== Timer fired Elapsed END ===========");
            }
            catch (Exception ex)
            {
                TextFileLogger.LogException("ExecuteETSQueueRequests Failed.", ex);

                MongoLogger.Log(new Elmah.Error(ex));
            }
        }

        static async Task ExecuteETSQueueRequests()
        {
            int queueInProcess = await CommonDAO.Instance.GetQueueItemCount((int)QueueProcessState.InProcess);

            TextFileLogger.LogInfo(string.Format("Queue Item in Process ({0}) items.", queueInProcess));

            int queueItemNumber = AppSettings.MaxQueueItemNumber - queueInProcess;

            if (queueItemNumber <= 0) return;

            // Read Data from ETS Queue
            var queueItems = await CommonDAO.Instance.GetQueueItems(queueItemNumber);

            if (queueItems != null)
            {
                TextFileLogger.LogInfo(string.Format("GetQueueItems(max {0}): {1} items.", AppSettings.MaxQueueItemNumber, queueItems.Count));

                var allTasks = new List<Task>();
                foreach (var item in queueItems)
                {
                    // each item should be executed in different thread
                    allTasks.Add(ExecuteETSQueueItem(item));
                    //ThreadPool.QueueUserWorkItem(ExecuteETSQueueItem, item);
                }

                await Task.WhenAll(allTasks);
            }
        }

        static async Task ExecuteETSQueueItem(ETSQueueItem queueItem)
        {
            // Text File logging
            TextFileLogger.LogInfo(string.Format("Start processing JobID: {0}, JobType: {1}", queueItem.JobID, queueItem.JobType));
            TextFileLogger.LogDebug(" ETSQueueItem", queueItem);

            string exceptionMessage = string.Empty;
            bool isPostBackResultException = false;
            try
            {
                // update item status to InProcess
                await CommonDAO.Instance.UpdateETSQueueStatus(queueItem.JobID, (int)QueueProcessState.New, (int)QueueProcessState.InProcess);

                var queueItemExecutorFactory = new QueueItemExecutorFactory(queueItem.JobType);
                var queueItemExecutor = queueItemExecutorFactory.CreateExecutor();

                // Execute item
                await queueItemExecutor.ExecuteRequest(queueItem);

            }
            catch (Exception ex)
            {
                TextFileLogger.LogException(string.Format("ExecuteETSQueueItem exception JobID: {0}", queueItem.JobID), ex);

                exceptionMessage = ex.Message;
                MongoLogger.Log(new Elmah.Error(ex));

                // send notification email with error message
                var emailHelper = new EmailHelper();
                var emailBody = string.Format("JobID: {0}. Error: {1}", queueItem.JobID, ex.Message);
                emailHelper.Send(emailBody);

                if (ex is PostBackResultException)
                {
                    isPostBackResultException = true;
                }
            }

            // to resolve "Cannot await in the body of a catch clause"
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                // update item status to Failed
                await CommonDAO.Instance.UpdateETSQueueStatus(queueItem.JobID, (int)QueueProcessState.InProcess, (int)QueueProcessState.Failed);

                if (isPostBackResultException)
                {
                    // Save unexpected ETS exception to MongDB response
                    await CommonDAO.Instance.SetETSResponseStatus(queueItem.JobID, BusinessExceptionConstants.UNEXPECTED_EXCEPTION, exceptionMessage);
                }
            }

            TextFileLogger.LogInfo("End processing JobID: " + queueItem.JobID);
        }

        protected override void OnStop()
        {
            try
            {
                _serviceTimer.Stop();
                _serviceTimer.Dispose();

                etsServiceLog.WriteEntry("############### ETS service STOPPED ###############");
            }
            catch (Exception ex)
            {
                TextFileLogger.LogException("Exception", ex);
            }
        }


    }
}
