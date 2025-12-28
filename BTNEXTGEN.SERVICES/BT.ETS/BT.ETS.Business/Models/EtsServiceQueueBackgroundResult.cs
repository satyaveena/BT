
using BT.ETS.Business.Constants;
namespace BT.ETS.Business.Models
{
    /// <summary>
    /// Class EtsServiceQueueBackgroundResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EtsServiceQueueBackgroundResult<T> : EtsServiceResult<T>
    {
        #region Public Member
        public string JobId { get; set; }
        #endregion
    }
}