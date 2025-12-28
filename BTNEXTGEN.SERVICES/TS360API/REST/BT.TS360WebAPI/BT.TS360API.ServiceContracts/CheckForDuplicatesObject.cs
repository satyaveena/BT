using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public class CheckForDuplicatesObject
    {
        public CheckForDuplicatesObject()
        {
            IsDuplication = false;
            ItemsCheckOrder = "";
            ItemsCheckCart = "";
            HoldingsFlag = "";
            ItemsCheckHoldings = new Dictionary<string, bool>();
        }

        public bool IsDuplication { get; set; }
        public string ItemsCheckOrder { get; set; }
        public string ItemsCheckCart { get; set; }
        public string HoldingsFlag { get; set; }

        public Dictionary<string, bool> ItemsCheckHoldings { get; set; }
    }
}
