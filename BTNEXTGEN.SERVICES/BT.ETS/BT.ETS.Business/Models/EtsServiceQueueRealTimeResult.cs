
using BT.ETS.Business.Constants;
namespace BT.ETS.Business.Models
{
    /// <summary>
    /// Class EtsServiceQueueRealTimeResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EtsServiceQueueRealTimeResult
    {
        #region Public Member
        public string JobId  { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        #endregion

        #region Constructor
        public EtsServiceQueueRealTimeResult()
        {
            StatusCode = "0";
            StatusMessage = "";
        }
        #endregion
    }
}