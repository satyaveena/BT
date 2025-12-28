using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using System;
using System.Web.Http;

namespace BT.TS360API.Services.Controllers
{
    public class CartController : ApiController
    {
        private readonly Services.CartService _cartService;

        public CartController()
        {
            this._cartService = new Services.CartService();
        }

        [HttpPost]
        [Route("appservice/searchservice/cart/esprank/HideEspAutoRankMessage")]
        public AppServiceResult<bool> HideEspAutoRankMessage(HideEspAutoRankMessageRequest request)
        {
            var result = new AppServiceResult<bool> { Status = AppServiceStatus.Success };
            try
            {
                if (string.IsNullOrEmpty(request.CartId) || string.IsNullOrEmpty(request.UserId))
                {
                    result.Data = false;
                    result.Status = AppServiceStatus.Fail;
                    result.ErrorMessage = "CartId, UserId are required.";
                }
                else
                {
                    result.Data = _cartService.HideEspAutoRankMessage(request);
                }
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Status = AppServiceStatus.Fail;
                result.ErrorMessage = ex.Message;
                Logger.RaiseException(ex, ExceptionCategory.General);
            }
            return result;
        }
    }
}
