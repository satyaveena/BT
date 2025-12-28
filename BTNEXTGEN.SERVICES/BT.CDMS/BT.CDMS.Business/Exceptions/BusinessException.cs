using System;

namespace BT.CDMS.Business.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public string ErrorCode { get; set; }

        public BusinessException(string message,string errorCode):base(message)
        {
            ErrorCode = errorCode;
        }
        public BusinessException(string message, Exception innerException, string errorCode) : base(message,innerException)
        {
            ErrorCode = errorCode;
        }
    }
}