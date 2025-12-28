using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts.Request
{
    public class DataFixSendEmailToBtRequest : BaseRequest
    {
        public string btKey { get; set; }
        public string userNote { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string LoginId { get; set; }
        public string UserEmail { get; set; }
        public MarketType? MarketType { get; set; }
        public string OrganizationName { get; set; }
    }
}
