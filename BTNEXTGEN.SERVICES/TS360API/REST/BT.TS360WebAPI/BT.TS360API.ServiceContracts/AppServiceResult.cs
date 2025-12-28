using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts
{
    public class AppServiceResult<T>
    {
        public T Data { get; set; }

        public AppServiceStatus Status { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string ImageUrl { get; set; }

        public string NavigateUrl { get; set; }

        public string ProductTypeLink { get; set; }

        public AppServiceResult()
        {
            Status = AppServiceStatus.Success;
        }
    }
}
