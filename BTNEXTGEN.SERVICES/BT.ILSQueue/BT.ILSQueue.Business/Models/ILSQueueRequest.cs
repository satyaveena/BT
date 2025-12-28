using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
    public class ILSQueueRequest
    {
        // pre-v
        public string ExternalID { get; set; }
        public string Vendor { get; set; }
        public string OrderedAtLocation { get; set; }
        public int OrderType { get; set; }
        public int PaymentMethod { get; set; }
        public int Copies { get; set; }
        public List<ILSOrderLineItem> LineItems { get; set; }

        // order
        public string PONumber { get; set; }
        public string PostbackURL { get; set; }

        // support fields
        public string OrganizationID { get; set; }
        public string MARCProfileID { get; set; }
        public string UpdatedBy { get; set; }
        public string OrderedDownloadedUserID { get; set; }
        public string BasketName { get; set; }
    }

    public class ILSOrderLineItem
    {
        public string ExternalLineItemID { get; set; }
        //public string BasketSummaryID { get; set; }
        public int Copies { get; set; }   
        public string BTKey { get; set; }
        public List<ILSLineItemSegment> LineItemSegments { get; set; }
    }

    public class ILSLineItemSegment
    {
        public string Location { get; set; }
        public string Fund { get; set; }
        public string Collection { get; set; }
        public int Copies { get; set; }
    }
}
