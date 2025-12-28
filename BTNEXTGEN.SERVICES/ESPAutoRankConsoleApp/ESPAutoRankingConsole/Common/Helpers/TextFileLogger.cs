using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPAutoRankingConsole.Common.Helpers
{
    public class TextFileLogger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TextFileLogger));

        public static void LogDebug(string message, Exception exception = null)
        {
            Log.Debug(message, exception);
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
