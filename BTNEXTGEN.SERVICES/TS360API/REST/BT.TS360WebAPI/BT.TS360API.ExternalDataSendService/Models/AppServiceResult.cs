using BT.TS360API.ExternalDataSendService.Constants;

namespace BT.TS360API.ExternalDataSendService.Models
{
    public class AppServiceResult<T>
    {
        public T Data { get; set; }

        public AppServiceStatus Status { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public AppServiceResult()
        {
            Status = AppServiceStatus.Success;
        }
    }
}
