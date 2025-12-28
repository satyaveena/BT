using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Order
{

    public class OrderSearchExportResponse
    {
        public string FileContent { get; set; }
        public string FileName { get; set; }
        public long LineCount { get; set; }
    }

}
