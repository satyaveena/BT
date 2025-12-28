using BT.TS360.NoSQL.Data.Common.Constants;
using BT.TS360.NoSQL.Data.Common.Helper;
using ILSWinService.Helper;
using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace ILSWinService
{
    public partial class ILSWinService : ServiceBase
    {
        Timer Timer1;
        bool InProcess = false;
        bool ServiceStarted = true;
        ILSCartProcessor iLSCartValidation;
        ThreadedLogger _threadedLogger;
        static Emailer _emailer;
        ExceptionLogger _exceptionLogger;
        static string _emailTo;
        static string _currentEnvironment;
        public ILSWinService()
        {
            InitializeComponent();
            Timer1 = new Timer();
            Timer1.Elapsed += Timer1_Elapsed;
            InitAppSettings();
            iLSCartValidation = new ILSCartProcessor();
        }

        private void InitAppSettings()
        {
            _threadedLogger = new ThreadedLogger(AppSettings.LogFolder, AppSettings.LogFilePrefix);
            _emailer = new Emailer(AppSettings.EmailSMTPServer);
            _emailTo = AppSettings.EmailTo;
            _currentEnvironment = AppSettings.CurrentEnvironment;
            _exceptionLogger = new ExceptionLogger(AppSettings.ExceptionLoggingConnectionString);
        }
        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (InProcess == true)
                    return;
                else
                {
                    InProcess = true;
                    if (ServiceStarted)
                    {
                        ILSCartFile.Instance.ResetILSFile();
                        ServiceStarted = false;
                    }
                    iLSCartValidation.ProcessCarts();
                    InProcess = false;
                }
            }
            catch (Exception ex)
            {
                InProcess = false;
                _threadedLogger.Write(ex.Message, FileLoggingLevel.ERROR);
                _exceptionLogger.LogError(ex, "ILSWinService", "Timer1_Elapsed");
                _emailer.Send(_emailTo, string.Format("ILS Windows Service Error: " + _currentEnvironment + " [" + Environment.MachineName + "]"), ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Timer1.Interval = Convert.ToDouble(ConfigurationManager.AppSettings["Timer1IntervalSeconds"]) * 1000;
                Timer1.Enabled = true;
                Timer1.AutoReset = true;
                Timer1.Start();
                _threadedLogger.Write("ILS Windows Service is Starting", FileLoggingLevel.INFO);
                _emailer.Send(_emailTo, string.Format("ILS Windows Service is Starting: " + _currentEnvironment + " [" + Environment.MachineName + "]"), "Service Started " + DateTime.Now.ToString());
            }
            catch (Exception ilsEx)
            {
                _threadedLogger.Write(ilsEx.Message, FileLoggingLevel.ERROR);
                _exceptionLogger.LogError(ilsEx, "ILSWinService", "OnStart");
                _emailer.Send(_emailTo, string.Format("ILS Windows Service is Starting: " + _currentEnvironment + " [" + Environment.MachineName + "]"), ilsEx.Message);
            }
        }

        protected override void OnStop()
        {
            Timer1.Enabled = false;
            _threadedLogger.Write("ILS Windows Service STOPPED", FileLoggingLevel.INFO);
            _emailer.Send(_emailTo, string.Format("ILS Windows Service: " + _currentEnvironment + " [" + Environment.MachineName + "]"), "Service Stopped " + DateTime.Now.ToString());
        }
    }
}
