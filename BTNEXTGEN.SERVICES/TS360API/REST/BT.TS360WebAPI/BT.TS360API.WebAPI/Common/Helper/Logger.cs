using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Common.Helper
{
    public class Logger
    {
        public static void Info(string message)
        {
            WriteLog(message, FileLogRepository.Level.INFO);
        }

        public static void Error(string message)
        {
            WriteLog(message, FileLogRepository.Level.ERROR);
        }

        private static void WriteLog(string message, FileLogRepository.Level level)
        {
            FileLogRepository fileLog = new FileLogRepository(AppSetting.SSOOAUTHLogFolder, AppSetting.SSOOAUTHLogFilePrefix);
            string logFileMessage = string.Empty;
            bool enableTrace = (AppSetting.SSOOAUTHEnableTrace.ToUpper() == "ON");

            if (enableTrace)
            {
                fileLog.Write(message, level);
            }
        }
    }
}