
using BT.ETS.Business.Constants;
namespace BT.ETS.Business.Models
{
    /// <summary>
    /// Class AppServiceResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EtsServiceResult<T>
    {
        #region Public Member
        public T Data { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        #endregion

        #region Constructor
        public EtsServiceResult()
        {
            StatusCode = BusinessExceptionConstants.SUCCEED;
            StatusMessage = "";
        }
        #endregion
    }
}
