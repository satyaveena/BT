using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Models
{
   public class PolarisPOResponse
    {
        //"{\"PAPIErrorCode\":-6,\"ErrorMessage\":\"JobsPurchaseOrderCreateData object is null.\",\"JobGuid\":null,\"JobStatusID\":0,\"JobStatusDescription\":null,\"ExternalID\":null,\"LineItemValidationErrors\":null}"
       public string PAPIErrorCode { get; set; }
       public string ErrorMessage { get; set; }
       public string JobGuid { get; set; }
       public string JobStatusID { get; set; }
       public string JobStatusDescription { get; set; }
       public string ExternalID { get; set; }
       public List<LineItemValidationErrors> LineItemValidationErrors { get; set; }
   }

    public class LineItemValidationErrors
    {
        public string ExternalLineItemID { get; set; }
        public List<LineItemValidationErrorDetail> Errors { get; set; }

    }

    public class LineItemValidationErrorDetail
    {
        public string PAPIErrorCode { get; set; }
        public string ErrorMessage { get; set; }

    }
}
