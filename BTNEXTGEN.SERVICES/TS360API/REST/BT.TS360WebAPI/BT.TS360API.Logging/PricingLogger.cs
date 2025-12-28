using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace BT.TS360API.Logging
{
    public class PricingLogger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PricingLogger));

        private static bool? _debugTrace = null;
        private static bool DebugTrace
        {
            get
            {
                if (_debugTrace.HasValue)
                {
                    return _debugTrace.Value;
                }

                bool temp = false;
                if (bool.TryParse(ConfigurationManager.AppSettings["DebugTrace"], out temp))
                {
                    _debugTrace = temp;
                    return _debugTrace.Value;
                }
                _debugTrace = false;
                return _debugTrace.Value;
            }
        }

        public static void LogDebug(string category, string message)
        {
            LogDebugToLog4Net(message);
        }

        public static void LogInfo(string category, string message)
        {
            Log.Info(message);
        }

        private static void LogDebugToLog4Net(string message, Exception exception = null)
        {
            if (DebugTrace)
            {
                Log.Debug(message, exception);
            }
        }
    }
}
