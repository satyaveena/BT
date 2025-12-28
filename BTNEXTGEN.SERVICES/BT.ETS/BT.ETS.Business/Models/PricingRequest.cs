using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.ETS.Business.Models
{
    /// <summary>
    /// Pricing Request
    /// </summary>
    /// 
    public class PricingRequest
    {
        // OrganizationId not used
        //public string OrganizationId { get; set; }
        
        /// <summary>
        /// TS360 User Id {GUID}
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// List of BTKeys
        /// </summary>
        public List<string> BTKeys { get; set; }
    }

}