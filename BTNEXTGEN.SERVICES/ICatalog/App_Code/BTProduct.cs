using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BTNextGen.Services.ICatalogs
{
    /// <summary>
    /// Data structure definition
    /// </summary>
    [DataContract]
    public class BTProduct
    {
        [DataMember]
        public string CatalogName { get; set; }
        [DataMember]
        public bool RequiresExpressSetup { get; set; }
        [DataMember]
        public string BTTitle { get; set; }
        [DataMember]
        public string BTKey { get; set; }
        [DataMember]
        public string BTISBN { get; set; }
        [DataMember]
        public string BTGTIN { get; set; }
        [DataMember]
        public bool BTBlowOutInactiveFlag { get; set; }
        [DataMember]
        public string BTItemGroup { get; set; }
        [DataMember]
        public string BTProductType { get; set; }
        [DataMember]
        public decimal BTListPrice { get; set; }
    }

}