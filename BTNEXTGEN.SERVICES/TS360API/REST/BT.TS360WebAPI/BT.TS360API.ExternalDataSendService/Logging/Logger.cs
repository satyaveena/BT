using System;
using Elmah;
using System.Web;
using BT.TS360API.ExternalDataSendService.Constants;

namespace BT.TS360API.ExternalDataSendService.Logging
{
    public class Logger
    {
        public static void WriteLog(Exception ex, string category = "")
        {
            var errSource = string.IsNullOrEmpty(ex.Source) ? category : string.Format("{0}, {1}", category, ex.Source);
            ex.Source = errSource;

            var error = new Error(ex);
            error.ApplicationName = "ExternalDataSendService";

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                error.ServerVariables.Add(HttpContext.Current.Request.ServerVariables);


            //ErrorSignal.FromCurrentContext().Raise(ex);
            ErrorLog.GetDefault(null).Log(error);
        }

        public static void Write(string category, string message)
        {
            var exception = new Exception(message);
            WriteLog(exception, category);
        }

        public static void RaiseException(Exception exception, ExceptionCategory search)
        {
            WriteLog(exception, search.ToString());
        }

        public static void LogException(Exception exception)
        {
            WriteLog(exception, ExceptionCategory.General.ToString());
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
