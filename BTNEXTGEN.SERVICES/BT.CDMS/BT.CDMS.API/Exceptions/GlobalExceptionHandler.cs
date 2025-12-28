using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Unity;
using Elmah;
using BT.CDMS.Business.Helpers;

namespace BT.CDMS.API.Exceptions
{
    public class GlobalExceptionHandler : ExceptionHandler
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
        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            const string errorMessage = "An unexpected error occured";
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new
                {
                    Message = errorMessage
                });
            response.Headers.Add("X-Error", errorMessage);
            Logger.Log(new Elmah.Error(context.Exception));
            await Task.FromResult(context.Result = new ResponseMessageResult(response));
        }
    }
}