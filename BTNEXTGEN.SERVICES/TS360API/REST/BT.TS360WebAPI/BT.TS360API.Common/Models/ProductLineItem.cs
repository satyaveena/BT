using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Models
{
    public class ProductLineItem
    {
        public string LineItemId { get; set; }
        public string BTKey { get; set; }
        public int Quantity { get; set; }
        public string PONumber { get; set; }
        public string Note { get; set; }
        public string BibNumber { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
