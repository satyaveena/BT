using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.ILS
{
    public class ILSValidationRequestResponse
    {
        public ILSValidationRequest ValidationRequest { get; set; }
        public ILSOrderValidationResponseStatus ValidationResponse { get; set; }
    }

    /// <summary>
    /// "ValidationRequest" object in Common.ILSAPIRequestLog collection.
    /// </summary>
    public class ILSValidationRequest
    {
        public List<ILSRequestLineItem> LineItems { get; set; }
    }

    /// <summary>
    /// "ValidationResponse" or "OrderResponse" object in Common.ILSAPIRequestLog collection.
    /// </summary>
    public class ILSOrderValidationResponseStatus
    {
        public string PAPIErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string JobGuid { get; set; }
        public string JobStatusID { get; set; }
        public string JobStatusDescription { get; set; }

        /// <summary>
        /// BasketSummaryID.
        /// </summary>
        public string ExternalID { get; set; }

        public List<LineItemValidationError> LineItemValidationErrors { get; set; }
    }

    public class ILSRequestLineItem
    {
        public string ExternalLineItemID { get; set; }
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

    public class LineItemValidationError
    {
        public string ExternalLineItemID { get; set; }
        public List<PAPIError> Errors { get; set; }
    }

    public class PAPIError
    {
        public string PAPIErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
