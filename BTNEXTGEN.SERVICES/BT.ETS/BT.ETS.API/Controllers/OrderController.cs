using BT.ETS.API.Services;
using BT.ETS.Business.Constants;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BT.ETS.API.Controllers
{
    /// <summary>
    /// Product and Cart Methods
    /// </summary>
   [Authorize]
    public class OrderController : ApiController
    {
        #region Private Member
        private readonly OrderServices _orderServices;
        #endregion

        #region Constructor

        /// <summary>
        /// Order Constructor
        /// </summary>
        public OrderController()
        {
            this._orderServices = new OrderServices();
        }
        #endregion

        #region Public Method

        /// <summary>
        /// Post received distributed cart to TS360.
        /// </summary>
        [HttpPost]
        [Route("cart/cartReceived/")]
        public async Task<EtsServiceQueueRealTimeResult> InsertEtsCart(CartReceivedRequestInput inputCart)
        {
            var result = new EtsServiceQueueRealTimeResult();
            try
            {
                result.JobId = await _orderServices.InsertEtsCartToBackgroundQueue(inputCart);
                return result;
            }
            catch (BusinessException be)
            {
                result.StatusMessage = be.Message;
                result.StatusCode = be.ErrorCode;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.Cart);
                result.StatusCode = "500";
                result.StatusMessage = BusinessExceptionConstants.Message(result.StatusCode);
                return result;
            }
        }

        /// <summary>
        /// Insert the product duplicate check status.
        /// </summary>
        [HttpPost]
        [Route("product/dupCheck/")]
        public async Task<EtsServiceQueueRealTimeResult> GetProductDupChecks(DupCheckRequest inputData)
        {
            var result = new EtsServiceQueueRealTimeResult();
            try
            {
                result.JobId = await _orderServices.InsertDupChecksToBackgroundQueue(inputData);
                return result;
            }
            catch (BusinessException be)
            {
                result.StatusMessage = be.Message;
                result.StatusCode = be.ErrorCode;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.ETSDupCheck);
                result.StatusCode = "500";
                result.StatusMessage = BusinessExceptionConstants.Message(result.StatusCode);
                return result;
            }
        }

        /// <summary>
        /// Return the product duplicate check details.
        /// </summary>
        [HttpPost]
        [Route("product/dupCheckDetails/")]
        public async Task<EtsServiceResult<DupCheckDetailResult>> GetProductDupCheckDetails(DupCheckDetailRequest inputData)
        {
            var result = new EtsServiceResult<DupCheckDetailResult>();
            try
            {
                result.Data = await _orderServices.GetDupCheckDetails(inputData);
                return result;
            }
            catch (BusinessException be)
            {
                result.StatusMessage = be.Message;
                result.StatusCode = be.ErrorCode;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.ETSDupCheck);
                result.StatusCode = "500";
                result.StatusMessage = BusinessExceptionConstants.Message(result.StatusCode);
                return result;
            }
        }

        /// <summary>
        /// Insert the product pricing.
        /// </summary>
        [HttpPost]
        [Route("product/pricing/")]
        public async Task<EtsServiceQueueRealTimeResult> GetProductPricing(PricingRequest inputData)
        {
            var result = new EtsServiceQueueRealTimeResult();
            try
            {
                result.JobId = await _orderServices.InsertPricingToBackgroundQueue(inputData);
                return result;
            }
            catch (BusinessException be)
            {
                result.StatusMessage = be.Message;
                result.StatusCode = be.ErrorCode;
                return result;
            }
            catch (Exception ex)
            {
                Logger.RaiseException(ex, ExceptionCategory.ETSPricing);
                result.StatusCode = "500";
                result.StatusMessage = BusinessExceptionConstants.Message(result.StatusCode);
                return result;
            }
        }
        #endregion
    }
}