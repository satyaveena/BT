using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public class DefaultAccountObject
    {
        public string EntAccountId { get; set; }
        public string BookAccountId { get; set; }
        public string VIPAccountId { get; set; }
        public Dictionary<string, string> DefaultESupplierAccountIds { get; set; }
    }
}
