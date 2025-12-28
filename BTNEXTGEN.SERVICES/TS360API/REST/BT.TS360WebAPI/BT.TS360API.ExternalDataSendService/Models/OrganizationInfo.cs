using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BT.TS360API.ExternalDataSendService.Models
{
    public class OrganizationInfo
    {
        [Required(ErrorMessage = "Organization ID is required.")]
        public string OrgId { get; set; }
        [Required(ErrorMessage = "Organization Name is required.")]
        public string OrgName { get; set; }
        public bool? IsDisabled { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsBTOrg { get; set; }
        public string DisabledReason { get; set; }
        public string OrgAlias { get; set; }
        public string SourceOrgAlias { get; set; }
        public string TimeZone { get; set; }
        public ContactInfo Contact { get; set; }
        public ContactInfo Address { get; set; }
        public string SalesRepId { get; set; }
        [Required]
        public string Source { get; set; }
        public ContactInfo CustomerServiceContact { get; set; }
        public string WeedCycle { get; set; }
        public string PreferredCreditType { get; set; }
        public bool IsCHQCustomer { get; set; }
        public int EstimatedUnitsPerYear { get; set; }
        public int NumberOfBranches { get; set; }
    }
}
