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
    [CollectionDataContract]
    public class BTProductCollection : List<BTProduct>
    {
        [DataMember]
        public List<BTProduct> ProductCollection {get; set; }
    }

}