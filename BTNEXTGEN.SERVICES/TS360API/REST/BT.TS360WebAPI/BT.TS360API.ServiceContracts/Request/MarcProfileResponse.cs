using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Request
{
    public class MarcProfileResponse
    {
        public List<ItemDataContract> ListMarcProfile { get; set; }
        public List<ItemDataContract> ListMarcFtpProfile { get; set; }
        public string DefaultMarcProfileId { get; set; }
        public string DefaultMarcFtpProfileId { get; set; }
        public bool IsMarcEnabled { get; set; }
        public bool IsBtEmployees { get; set; }
        public bool IsOCLCEnabled { get; set; }
        public int MARCCartLineThreshold { get; set; }

    }

    public class MARCJsonResponse
    {
        public string BTKey { get; set; }
        public string BasketLineItemID { get; set; }
        public string MARCBody { get; set; }
        public string MARCHeader { get; set; }
    }

    public class MARCJsonRequest
    {
        public String SortColumn { get; set; }
        public String BasketSummaryID { get; set; }
        public String SortDirection { get; set; }
        public String ProfileID { get; set; }
        public String FullIndicator { get; set; }
        public Boolean IsOrdered { get; set; }
        public Boolean IsCancelled { get; set; }
        public Boolean IsOCLCEnabled { get; set; }
        public Boolean IsBTEmployee { get; set; }
        public Boolean HasInventoryRules { get; set; }
        public String MarketType { get; set; }
        public String TsUserId { get; set; }
        public String OrgId { get; set; }
        public String IlsUserId { get; set; }
        public String IlsVendor { get; set; }
        public string ILSAcquisitionsApiKey { get; set; }
        public string ILSAcquisitionsApiPassphrase { get; set; }
        public string ILSBaseAddress { get; set; }
    }

    public class ILSResponse
    {
        public int code { get; set; }
        public int specificCode { get; set; }
        public int httpStatus { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class ILSOrderSucessResponse
    {
        public string controlNumber { get; set; }
        public string orderId { get; set; }
        public string bibId { get; set; }
        public string legacyOrderId { get; set; }

    }

    public class ILSLineItemLog
    {
        public string LineItemId { get; set; }
        public string ILSAPIRequest { get; set; }
        public string ILSAPIResponse { get; set; }
        public string BTKey { get; set; }
        //Timout,Other,Fund Code
        public string ErrorType { get; set; }

    }

    public class ILSLog
    {
        public ILSLog()
        {
            ILSLineItemLogs = new List<ILSLineItemLog>();
        }
        public string ID { get; set; }
        public DateTime RequestDateTime { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public string BasketSummaryId { get; set; }
        public List<ILSLineItemLog> ILSLineItemLogs { get; set; }
        /// <summary>
        /// Authentication, Validation or Order
        /// </summary>
        public string ErrorType { get; set; }
        /// <summary>
        /// Error Description
        /// </summary>
        public string ErrorDescription { get; set; }
        /// <summary>
        /// New Cart ID in case of split cart
        /// </summary>
        public string NewCartId { get; set; }
        /// <summary>
        /// Authentication, Validation or Order
        /// </summary>
        public string ILSStatus { get; set; }

        public string UserId { get; set; }

    }

    public class ILSAPIRequestResponse
    {
        public DateTime RequestDateTime { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public string BasketSummaryId { get; set; }
        public List<LineItemAPILogDetails> ILSLineItemLogs { get; set; }
        public string ErrorType { get; set; }
        public string ErrorDescription { get; set; }
        public string NewCartId { get; set; }
        public string ILSStatus { get; set; }
    }

    public class LineItemAPILogDetails
    {
        public string LineItemId { get; set; }
        public string ILSAPIRequest { get; set; }
        public string ILSAPIResponse { get; set; }
        public FootprintInformation FootprintInformation { get; set; }
    }

    public class FootprintInformation  //1:1 Relationship
    {
        public string CreatedBy { get; set; }
        public string CreatedByUserID { get; set; }  //2016-07-27 Ralph requested 
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedByUserID { get; set; }  //2016-07-27 Ralph requested
        public DateTime UpdatedDate { get; set; }
    }

    public class CartOrderedDownloadedUser
    {
        public string CartId { get; set; }
        public string OrderedDownloadedUser { get; set; }

    }
  
    public class ILSLineItemDetail
    {
        public string BasketLineItemID { get; set; }
        public string ILSOrderNumber { get; set; }
        public string ILSBIBNumber { get; set; }
    }

    public class ILSOrderRequest
    {
        public string UserId { get; set; }
        public string CartId { get; set; }       
        public string MarcProfileId { get; set; }
        public Int64 IlsVendorId { get; set; }
        public string IlsVendorCode { get; set; }
        public string OrderedDownloadedUserId { get; set; }
    }

   
}
