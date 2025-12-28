using System;
using System.Diagnostics;
using System.Linq;
using Elmah;
using log4net;
using BT.Auth.Business.Logger.Enum;

namespace BT.Auth.Business.Logger
{
    /// <summary>
    /// Class Logger
    /// </summary>
    public class Logger : ILogger
    {
        #region Property
        private static ILog _logger;
        public Type LoggerType
        {
            get; set;
        }
        #endregion

        #region Method
        # region Public
        public Guid Log(LogLevel level, String message)
        {
            return Log(level, null, message);
        }
        public Guid Log(LogLevel level, Exception exception)
        {
            return Log(level, exception, null);
        }
        public Guid Log(LogLevel level, Exception exception, String message)
        {
            GetLoggerInstance();
            Guid logId = Guid.Empty;
            switch (level)
            {
                case LogLevel.Error:
                    if (_logger.IsErrorEnabled)
                        logId = Guid.NewGuid();
                    break;
                case LogLevel.Fatal:
                    if (_logger.IsFatalEnabled)
                        logId = Guid.NewGuid();
                    break;
                case LogLevel.Warning:
                    if (_logger.IsWarnEnabled)
                        logId = Guid.NewGuid();
                    break;
            }

            String idMessage = logId != Guid.Empty ? $"MessageId:{logId} Message: {message}" : message;

            if (idMessage?.Length > 2000)
            {
                idMessage = idMessage.Substring(0, 2000);
            }
            switch (level)
            {
                case LogLevel.Debug:
                    if (_logger.IsDebugEnabled)
                        _logger.Debug(idMessage, exception);
                    break;
                case LogLevel.Error:

                    //Log to elmah in case of error
                    if (_logger.IsErrorEnabled)
                    {
                        _logger.Error(idMessage, exception);
                        LogToElmah(idMessage, exception);
                    }
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(idMessage, exception);
                    break;
                case LogLevel.Information:
                    _logger.Info(idMessage, exception);
                    break;
                case LogLevel.Warning:
                    _logger.Warn(idMessage, exception);
                    break;
                default:
                    _logger.Info(idMessage, exception);
                    break;
            }
            return logId;
        }
        #endregion

        #region Private
        private void GetLoggerInstance()
        {
            Type caller = LoggerType ??
                (
                    from frame in new StackTrace().GetFrames()
                    let type = frame.GetMethod().ReflectedType
                    where type != typeof(Logger)
                    select type
                ).First();

            _logger = LogManager.GetLogger(caller);
        }
        private void LogToElmah(String message, Exception exception)
        {
            Exception completeException = new Exception(message + (exception != null ? exception.StackTrace : String.Empty));
            ErrorSignal.FromCurrentContext().Raise(completeException);
        }
        #endregion
        #endregion
    }
}
