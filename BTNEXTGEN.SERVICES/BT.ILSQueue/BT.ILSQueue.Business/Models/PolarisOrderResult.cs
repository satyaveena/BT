using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
    public class PolarisOrderResult
    {
        public string PAPIErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ExternalID { get; set; }
        public string PONumber { get; set; }
        public string PurchaseOrderID { get; set; }

        public List<ILSAckLineItem> LineItems { get; set; }

        public List<ILSAckLineItemError> LineItemErrors { get; set; }

        public List<string> ItemRecordCreateErrors { get; set; }
    }

    public class ILSAckLineItem
    {
        public string POLineItemID { get; set; }

        public string LineNumber { get; set; }
        public string ExternalLineItemID { get; set; }
        public string BibRecordID { get; set; }
        public List<ILSAckLineItemSegment> LineItemSegments { get; set; }
    }

    public class ILSAckLineItemSegment
    {
        public string POLineItemSegmentID { get; set; }

        public string POLISegmentNumber { get; set; }
        public string EDIPOLISegNum { get; set; }
        public string Location { get; set; }
        public string Fund { get; set; }
        public string Collection { get; set; }
        public string CallNumber { get; set; }
        public int Copies { get; set; }
        public List<ILSAckLineItemSegment> LineItemSegments { get; set; }
    }

    public class ILSAckLineItemError
    {
        public string ExternalLineItemID { get; set; }

        public string ErrorMessage { get; set; }
      
        public List<ILSAckLineItemSegmentError> LineItemSegmentErrors { get; set; }
    }

    public class ILSAckLineItemSegmentError
    {
        public string ErrorMessage { get; set; }

        public string Organization { get; set; }
        public string Collection { get; set; }
        public string Fund { get; set; }
        public int Quantity { get; set; }
        public string CallNumber { get; set; }
     
    }
}
