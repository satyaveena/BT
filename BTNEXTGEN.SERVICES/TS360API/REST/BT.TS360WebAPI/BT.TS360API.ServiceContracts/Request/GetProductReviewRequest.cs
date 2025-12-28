
namespace BT.TS360API.ServiceContracts.Request
{
    public class GetProductReviewRequest : BaseRequest
    {        
        public string key { get; set; }

        public string UserId { get; set; }
        public string OrgId { get; set; }
    }
}
