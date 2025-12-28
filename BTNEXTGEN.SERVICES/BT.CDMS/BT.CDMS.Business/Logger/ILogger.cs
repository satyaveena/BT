using System;
using BT.CDMS.Business.Logger.Enum;

namespace BT.CDMS.Business.Logger
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
