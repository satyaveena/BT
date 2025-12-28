using System;
using BT.Auth.Business.Logger.Enum;

namespace BT.Auth.Business.Logger
{
    public interface ILogger
    {
        Type LoggerType
        {
            get; set;
        }

        Guid Log(LogLevel level, String message);
        Guid Log(LogLevel level, Exception exception);
        Guid Log(LogLevel level, Exception exception, String message);
    }
}
