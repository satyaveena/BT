using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Request
{
    public class SendCdmsListDistributionRequest : BaseRequest
    {
        public string cdmsListId { get; set; }
        public List<string> additionalUserIds { get; set; }

        public List<string> checkedUserIds { get; set; }

        public bool allIndicator { get; set; }
    }
}
