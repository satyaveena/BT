using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Constants
{
   public class ApplicationConstants
    {
       #region Public Member

       public const String POLARIS_VENDOR_CODE = "Polaris";

       public const string CommonDatabaseName = "Common";
       public const string ILSAPIRequestCollectionName = "ILSAPIRequestLog";

       //public const String APPLICATION_NAME_ILSQUEUE_BACKGROUND = "ILS Queue Background Service";
       public const String APPLICATION_NAME = "ILS Queue";
      
       public const String ELMAH_ERROR_SOURCE_ILS_BACKGROUND = "ILS Queue Service";
       public const String ELMAH_ERROR_SOURCE_ILS_API = "ILS API";

       public const String ELMAH_MONGODB = "ApplicationLog";
       public const String LOG_TABLE_EXCEPTIONS = "ExceptionLog";

       #endregion
    }

    public class ILSProcessingStatus
    {
        //ILSQueue ProcessingStatus
        public const string ValidationRequest = "Validation Request";
        public const string ValidationResponse = "Validation Response";
        public const string OrderingRequest = "Ordering Request";
        public const string OrderingResponse = "Ordering Response";
        public const string OrderingResponseReady = "Ordering Response Ready";
        public const string OrderingResultRequest = "Ordering Result Request";
        public const string OrderingResultResponse = "Ordering Result Response";
        // Failed status
        public const string ValidationFailed = "Validation Failed";
        public const string OrderValidationFailed = "Ordering Validation Failed";
    }
}
