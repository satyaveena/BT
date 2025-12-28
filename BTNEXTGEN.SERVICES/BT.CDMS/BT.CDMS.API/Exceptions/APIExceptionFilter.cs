using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using BT.CDMS.Business.Helper;
using BT.CDMS.Business.Constants;
using BT.CDMS.Business.Helpers;
using BT.CDMS.Business.Exceptions;
using Unity;
using Elmah;
using System;

namespace BT.CDMS.API.Exceptions
{
    public class APIExceptionFilter : ExceptionFilterAttribute
    {
        private ErrorLog _logger;
        private ErrorLog Logger
        {
            get
            {
                if (_logger == null)
                    _logger = UnityHelper.Container.Resolve<ErrorLog>();
                return _logger;
            }
        }
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception.GetType() == typeof(BusinessException))
            {
                BusinessException businessException = context.Exception as BusinessException;
                var errorModel = new AppServiceResult<string>()
                {ErrorCode=businessException.ErrorCode ,ErrorMessage = businessException.Message, Status = AppServiceStatus.Fail };
                context.Response = context.Request.CreateResponse(HttpStatusCode.OK, errorModel);
            }
            else
            {
                try
                {
                    CommonHelper.SendEmail(context.Exception);
                    Logger.Log(new Elmah.Error(context.Exception));
                    context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, "Internal Server Error.");
                }
                catch(Exception ex)
                {
                    Logger.Log(new Elmah.Error(ex));
                }
                
            }

        }
    }
}