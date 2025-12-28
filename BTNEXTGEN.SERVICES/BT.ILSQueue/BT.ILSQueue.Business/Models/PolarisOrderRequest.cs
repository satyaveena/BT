using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
    public class PolarisOrderRequest: PolarisPORequest
    {
      /*  public string ExternalID { get; set; }
        public string Vendor { get; set; }
        public string OrderedAtLocation { get; set; }
        public int OrderType { get; set; }
        public int PaymentMethod { get; set; }*/

        public string PONumber { get; set; }
        public string PostbackURL { get; set; }
        public List<MARCLineItem> MARCLineItems { get; set; }
    }

    public class MARCLineItem
    {
        public string ExternalLineItemID { get; set; }
        //public string BasketSummaryID { get; set; }
        public int Copies { get; set; }

        public MARC21 MARC { get; set; }
    }
}
