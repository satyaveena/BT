using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Elmah;
using log4net;

using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.WinServices.Helper;
using BT.ILSQueue.Business.MongDBLogger.ELMAHLogger;

namespace BT.ILSQueue.WinServices
{
    public partial class ILSQueueService : ServiceBase
    {
        static System.Timers.Timer _servicePickupTimer = new System.Timers.Timer(AppSettings.ILSOrderPickupTimer * 1000);
        static System.Timers.Timer _serviceResultTimer = new System.Timers.Timer(AppSettings.ILSOrderResultTimer * 1000);
        static System.Timers.Timer _serviceJobStatusTimer = new System.Timers.Timer(AppSettings.ILJobStatusTimer * 1000);
        
        
        public static System.Diagnostics.EventLog _ilsQueueServiceLog;

        ILSCartProcessor iLSCartValidation;
        //bool InProcess = false;
        bool InProcessPickup = false;
        bool InProcessResult = false;
        bool InProcessJobStatus = false;

        /*private static ErrorLog _mongoLogger;

        public static ErrorLog MongoLogger
        {
            get
            {
                if (_mongoLogger == null)
                    _mongoLogger = new ELMAHMongoLogger();
                return _mongoLogger;
            }
        }*/

        public ILSQueueService()
        {
            InitializeComponent();

            _ilsQueueServiceLog = ilsQueueServiceLog;
            iLSCartValidation = new ILSCartProcessor();
        }

        protected override void OnStart(string[] args)
        {
            TextFileLogger.LogInfo("=========== OnStarted ===========");
            _ilsQueueServiceLog.WriteEntry("############### ILS Queue Service STARTED ###############");

            log4net.Config.BasicConfigurator.Configure();

            _servicePickupTimer.AutoReset = true;
            _servicePickupTimer.Enabled = AppSettings.EnableOrderPickupTimer;
            _servicePickupTimer.Elapsed += PickupTimerElapsed; // fire in different thread

            _serviceResultTimer.AutoReset = true;
            _serviceResultTimer.Enabled = AppSettings.EnableOrderResultTimer;
            _serviceResultTimer.Elapsed += ResultTimerElapsed; // fire in different thread

            _serviceJobStatusTimer.AutoReset = true;
            _serviceJobStatusTimer.Enabled = AppSettings.EnableOrderStatusTimer;
            _serviceJobStatusTimer.Elapsed += JobStatusTimerElapsed; // fire in different thread

            var emailHelper = new EmailHelper();
            var emailBody = string.Format("OnStarted");
            emailHelper.Send(emailBody);
            
        }

        protected override void OnStop()
        {
            var emailHelper = new EmailHelper();

            try
            {
                _servicePickupTimer.Stop();
                _servicePickupTimer.Dispose();

                _serviceJobStatusTimer.Stop();
                _serviceJobStatusTimer.Dispose();

                _serviceResultTimer.Stop();
                _serviceResultTimer.Dispose();

                TextFileLogger.LogInfo("=========== OnStopped ===========");
                this.ilsQueueServiceLog.WriteEntry("############### ILS Queue service STOPPED ###############");

                var emailBody = string.Format("OnStopped");
                emailHelper.Send(emailBody);

            }
            catch (Exception ex)
            {
                TextFileLogger.LogException("Exception", ex);

                ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));

                var emailBody = string.Format(ex.Message);
                emailHelper.Send(emailBody);
            }
        }

        private async void PickupTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (InProcessPickup == true)
                    return;
                else
                {
                    TextFileLogger.LogInfo("=========== Pickup Timer Elapsed BEGIN ===========");

                    InProcessPickup = true;
                    iLSCartValidation.InitializeILSRequest();
                    InProcessPickup = false;

                    TextFileLogger.LogInfo("=========== Pickup Timer Elapsed END ===========");
                }
            }
            catch (Exception ex)
            {
                InProcessPickup = false;
                TextFileLogger.LogException("PickupTimerElapsed Failed.", ex);

                ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));

                var emailHelper = new EmailHelper();
                var emailBody = string.Format(ex.Message);
                emailHelper.Send(emailBody);

            }
        }

        private async void ResultTimerElapsed(object sender, ElapsedEventArgs e)
        {

            try
            {
                if (InProcessResult == true)
                    return;
                else
                {
                    TextFileLogger.LogInfo("=========== Result Timer Elapsed BEGIN ===========");

                    InProcessResult = true;
                    iLSCartValidation.InitializeILSOrder();
                    InProcessResult = false;

                    TextFileLogger.LogInfo("=========== Result Timer Elapsed END ===========");
                }
            }
            catch (Exception ex)
            {
                InProcessResult = false;

                TextFileLogger.LogException("ResultTimerElapsed Failed.", ex);

                ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));

                var emailHelper = new EmailHelper();
                var emailBody = string.Format(ex.Message);
                emailHelper.Send(emailBody);

            }

        }

        private async void JobStatusTimerElapsed(object sender, ElapsedEventArgs e)
        {

            try
            {
                if (InProcessJobStatus == true)
                    return;
                else
                {
                    TextFileLogger.LogInfo("=========== Job Status Timer Elapsed BEGIN ===========");

                    InProcessJobStatus = true;
                    iLSCartValidation.InitializeILSJobStatus();
                    InProcessJobStatus = false;
                }
            }
            catch (Exception ex)
            {
                InProcessJobStatus = false;
                TextFileLogger.LogException("JobStatusTimerElapsed Failed - " + ex.Message, ex);

                ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));

                var emailHelper = new EmailHelper();
                var emailBody = string.Format(ex.Message);
                emailHelper.Send(emailBody);

            }
  
        }
       
    }
}
