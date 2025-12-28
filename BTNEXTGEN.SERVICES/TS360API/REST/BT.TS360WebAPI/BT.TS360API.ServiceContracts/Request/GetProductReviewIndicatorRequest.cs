
using System.Collections.Generic;


namespace BT.TS360API.ServiceContracts.Request
{
    public class GetProductReviewIndicatorRequest : BaseRequest
    {        
        public List<string> BTKeyList{ get; set; }
        //public string url { get; set; }

        public string UserId { get; set; }
    }
}
