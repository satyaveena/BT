using BT.ETS.Business.Constants;
using System;

namespace BT.ETS.Business.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public string ErrorCode { get; set; }

        public BusinessException(string errorCode)
            : base(BusinessExceptionConstants.Message(errorCode))
        {
            ErrorCode = errorCode;
        }

        public BusinessException(int errorCode)
            : base(BusinessExceptionConstants.Message(errorCode.ToString()))
        {
            ErrorCode = errorCode.ToString();
        }

        public BusinessException(int errorCode, string errorMessage)
            : base(errorMessage)
        {
            ErrorCode = errorCode.ToString();
        }

        public BusinessException(string message, Exception innerException, string errorCode)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}