using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.WinService.Helper
{
    public class TextFileLogger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TextFileLogger));

        public static bool IsDebugEnabled { get { return Log.IsDebugEnabled; } }

        public static void LogDebug(string message)
        {
            Log.Debug(message);
            
        }

        public static void LogDebug(string message, object jsonSerializeObject)
        {
            if (IsDebugEnabled && jsonSerializeObject != null)
            {
                try
                {
                    var serializedValue = JsonConvert.SerializeObject(jsonSerializeObject);

                    var logMsg = string.Format("{0} : {1}.", message, serializedValue);
                    Log.Debug(logMsg);
                }
                catch (Exception)
                {
                    Log.Debug(message + ". SerializeObject failed");
                }
            }
        }

        public static void LogInfo(string message, Exception exception = null)
        {
            Log.Info(message, exception);
        }

        public static void LogException(string message, Exception exception)
        {
            Log.Error(message, exception);
        }
    }
}
