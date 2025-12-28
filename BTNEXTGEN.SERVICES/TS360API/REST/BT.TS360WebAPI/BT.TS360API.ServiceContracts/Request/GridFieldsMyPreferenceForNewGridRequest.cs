using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts.Request
{
    public class GridFieldsMyPreferenceForNewGridRequest : BaseRequest
    {
        public List<UserGridFieldObject> userGridFieldObjects  { get; set; }
        public string defaultQuantity { get; set; }

        public string UserId { get; set; }
    }
}
