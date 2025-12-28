using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Request
{
    public class ILSValidationRequest
    {
        public string ILSType { get; set; }
        public string TSUserId { get; set; }
        public string TSOrgId { get; set; }
        public string ILSUrl { get; set; }
        public string ILSApiKey { get; set; }
        public string ILSApiSecret { get; set; }
        public string ILSLogin { get; set; }
        public int ILSValidationStatusId { get; set; }
        public string ILSValidationStatus { get; set; }
        public DateTime? ILSValidationDateTime { get; set; }
        public string ILSValidationDateTimeInString
        {
            get
            {
                return ILSValidationDateTime.HasValue ? ILSValidationDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : "";
            }
        }
        public string ILSValidationErrorMessage { get; set; }

        public string ILSUserDomain { get; set; }

        public string ILSUserAccount { get; set; }
    }

    public class VendorCodesRequest
    {
        public string OrganizationId { get; set; }
        public string SearchKeyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int SortDirection { get; set; }
        public List<VendorCode> VendorCodes { get; set; }
        public int LastResultset { get; set; }
        public int IsImport { get; set; }
        public int OrderingType { get; set; }
        public string VendorID { get; set; }
    }

    public class VendorCodesDeleteRequest
    {
        public string OrganizationId { get; set; }
        public List<Int64> IdList { get; set; }
        public string VendorID { get; set; }
    }

    public class VendorCodeAddRequest
    {
        public int IsImport { get; set; }
        public string OrganizationId { get; set; }
        public string UserId { get; set; } 
        public List<string> CodeList { get; set; }
        public int OrderingType { get; set; }
        public string VendorID { get; set; }
    }

    public class VendorCode
    {
        public string Code { get; set; }
        public int Id { get; set; }
    }

    public class StaffUserRequest
    {
        public string domain { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }



    public class StaffUserResponse
    {
        public int PAPIErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
        public int PolarisUserID { get; set; }
        public int BranchID { get; set; }
        public string AuthExpDate { get; set; }
    } 
}
