using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace BTNextGen.Services.ICatalogs
{
  
    [DataContract]
    public class CategoryDetailsResponse
    {
        [DataMember]
        public string CatalogName { get; set; }
        [DataMember]
        public string BTKey { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public int Rank { get; set; }
    }
   

    [CollectionDataContract]
    public class CategoryRespCollection : List<CategoryDetailsResponse>
    {
        [DataMember]
        public List<CategoryDetailsResponse> CategoryResponseCollection { get; set; }

    }
}
