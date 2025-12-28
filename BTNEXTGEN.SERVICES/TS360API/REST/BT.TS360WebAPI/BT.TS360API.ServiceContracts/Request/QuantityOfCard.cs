

namespace BT.TS360API.ServiceContracts
{
    public class QuantityOfCartResponse
    {
    }

    public class QuantityOfCartRequest : BaseRequest
    {
        public string CartName { get; set; }
        public string CartId { get; set; }
        
    }
}
