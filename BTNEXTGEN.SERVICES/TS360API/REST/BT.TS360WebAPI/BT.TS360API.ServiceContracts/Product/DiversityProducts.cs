using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BT.TS360API.ServiceContracts.Product
{
    [DataContract]
    public class DiversityProduct
    {
         [DataMember]
        public string BTKey { get; set; }

         [DataMember]
        public List<string> ClassificationName { get; set; }

    }

     [DataContract]
    public class DiversityProductsRequest
    {
        [DataMember]
       public List<string> BTKeys { get; set; }


    }

     [DataContract]
     public class DiversityProductsResponse
     {
         [DataMember]
         public List<DiversityProduct> DiversityProducts  { get; set; }


     }
}
