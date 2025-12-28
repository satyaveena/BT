using BT.CDMS.Business.Constants;

namespace BT.CDMS.Business.Helper
{
    /// <summary>
    /// Class AppServiceResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppServiceResult<T>
    {
        #region Public Member
        public T Data { get; set; }
        public AppServiceStatus Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Constructor
        public AppServiceResult()
        {
            Status = AppServiceStatus.Success;
        }
        #endregion
    }
}