using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    public class SimpleLineItem
    {
        public string LineItemID { get; set; }
        public string BasketOriginalEntryID { get; set; }
        public string BTKey { get; set; }
    }
}
