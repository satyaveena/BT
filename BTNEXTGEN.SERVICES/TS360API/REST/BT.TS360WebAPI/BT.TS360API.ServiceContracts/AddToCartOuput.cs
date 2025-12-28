using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class AddToCartOuput
    {
        public string CartId { get; set; }
        public string PermissionViolationMessage { get; set; }
        public int totalAddingQtyForGridDistribution { get; set; }
    }
}
