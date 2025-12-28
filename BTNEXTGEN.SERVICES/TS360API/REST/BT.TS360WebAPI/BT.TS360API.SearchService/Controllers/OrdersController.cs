using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using System.Web.Http;

namespace BT.TS360API.SearchService.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly Services.OrdersService _orderActionService;

        public OrdersController()
        {
            this._orderActionService = new Services.OrdersService();
        }

        [HttpPost]
        [Route("Orders/SubmitILSOrder")]
        public AppServiceResult<bool> SubmitILSOrder(ILSOrderRequest request)
        {
            return _orderActionService.SubmitILSOrder(request);
        }

        [HttpPost]
        [Route("appservice/searchservice/ValidateILSDetail")]
        public AppServiceResult<string> ValidateILSDetail(ILSValidationRequest request)
        {
            return _orderActionService.ValidateILSDetail(request);
        }
    }
}
