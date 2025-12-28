using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.ETS.Business.Models
{ 
    /// <summary>
    /// Duplicate Check Details Request
    /// </summary>
    public class DupCheckDetailRequest
    {
        /// <summary>
        /// TS360 Organization Id - GUID
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// TS360 UserId - GUID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// BTKey
        /// </summary>
        public string BTKey { get; set; }
        /// <summary>
        /// Values should be “C” – Cart,  “O” – Order, “S” – Series
        /// </summary>
        public string DupCheckStatusType { get; set; }
        
        public string DupCheckPreference { get; set; }
        
        public string DupCheckDownloadCartType { get; set; }

    }

}