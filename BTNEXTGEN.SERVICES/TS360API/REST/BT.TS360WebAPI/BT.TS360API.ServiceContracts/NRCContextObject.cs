using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts
{
    public class NRCContextObject
    {
        public List<NRCProductType> ProductTypesList { get; set; }
        public string CalendarView { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
    }
}
