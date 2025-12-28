using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
    public class ILSLineItemDetail
    {
        public string BasketLineItemID { get; set; }
        public string BTKey { get; set; }
        public string ILSOrderNumber { get; set; }
        public string ILSBIBNumber { get; set; }
        public string ILSPolySegID { get; set; }
        public string LocationCode { get; set; }
        public string FundCode { get; set; }
        public string CollectionCode { get; set; }
    }
}
