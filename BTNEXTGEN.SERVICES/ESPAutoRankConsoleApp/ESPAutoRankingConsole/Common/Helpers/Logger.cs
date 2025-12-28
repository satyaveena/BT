using System;
using Elmah;
using System.Web;
namespace ESPAutoRankingConsole.Common.Helpers
{
    public class Logger
    {
        public static void WriteLog(Exception ex, string category = "", bool writeToLogFile = false)
        {
            if (writeToLogFile)
            {
                TextFileLogger.LogException("", ex);
            }

            var errSource = string.IsNullOrEmpty(ex.Source) ? category : string.Format("{0}, {1}", category, ex.Source);
            ex.Source = errSource;

            var errorLog = ErrorLog.GetDefault(null);
            errorLog.ApplicationName = "ESPAuToRankBackground";

            var error = new Error(ex);
            error.ApplicationName = "ESPAuToRankBackground";

            errorLog.Log(error);
        }

        public static void Write(string category, string message)
        {
            var exception = new Exception(message);
            WriteLog(exception, category);
        }
        public static void Write(string category, string message, bool writeToDb = true)
        {
            //UlsLoggingService.Log(category, message);

            //if (writeToDb)
            //{
            //    var exception = new Exception(message);
            //    exception.Source = category +
            //                       (exception.Source == null ? string.Empty : (CategoryGrouped + exception.Source));
            //    ErrorSignal.FromCurrentContext().Raise(exception);
            //}
        }
        public static void RaiseException(Exception exception, ExceptionCategory category, bool writeToLogFile = true)
        {
            WriteLog(exception, category.ToString(), writeToLogFile);
        }

        public static void LogException(Exception exception, bool writeToLogFile = true)
        {
            WriteLog(exception, ExceptionCategory.ESPGetAutoRankRequests.ToString(), writeToLogFile);
        }

        public static void LogDebug(string logCategory, string calculatepriceRealtimeBegin)
        {
            Write(logCategory, calculatepriceRealtimeBegin);
        }
        public static void LogException(string message, string category, Exception innerException)
        {
            Exception ex = new Exception(message, innerException.InnerException);
            WriteLog(ex, category);
        }
    }
}
