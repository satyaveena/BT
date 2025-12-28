using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.ETS.Business.Models
{
    /// <summary>
    /// Duplicate Check Request
    /// </summary>
    public class DupCheckRequest
    {
        /// <summary>
        /// TS360 UserId - GUID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Array of BTKeys
        /// </summary>
        public List<string> Products { get; set; }//list of BTKey
        /// <summary>
        /// Value should be “default”, “mycarts”, “allcarts”, “none”
        /// </summary>
        public string DupCheckC { get; set; }
        /// <summary>
        /// Value should be “default” , “myaccounts”, “allaccounts”, “none”
        /// </summary>
        public string DupCheckO { get; set; }
        /// <summary>
        /// Value should be “default”, “includeworders” – included with orders, “includewcarts” – included with carts
         /// </summary>
        public string DupCheckDownloadCart { get; set; }
        /// <summary>
        /// Value should be “default”, “series”,  “none”
        /// </summary>
        public string DupCheckS { get; set; }
        /// <summary>
        /// Value should be “default”, “againstmyholdings”, “againstorganizationholdings”, “none”
        /// </summary>
        public string DupCheckH { get; set; } 
    }

}