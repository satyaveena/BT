using System.Collections.Generic;
using System;
namespace BT.ETS.Business.Models
{
    /// <summary>
    /// OrganizationInfo
    /// </summary>
    public class OrganizationInfo
    {
        public string OrganizationId { get; set; }
        
        public string OrganizationName { get; set; }
        
        public string ESPLibraryId { get; set; }
        
        public string IsRankEnabled { get; set; }
        
        public string IsDistributionEnabled { get; set; }
        
        public DateTime? LastUpdatedDate { get; set; } 
    }
}