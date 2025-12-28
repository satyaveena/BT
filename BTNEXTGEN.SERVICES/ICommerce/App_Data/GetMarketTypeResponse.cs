using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ICommerce
{

    [ServiceContract]
    public interface ICommerceService
    {

        [OperationContract]
        //[WebInvoke(Method="POST",
        //           RequestFormat=WebMessageFormat.Xml,
        //           ResponseFormat=WebMessageFormat.Xml,
        //           UriTemplate="MarketType/{EmailAddress}") ]
        GetMarketTypeResponse GetMarketType(string EmailAddress, string VendorAPIKey);



    }



   
    [DataContract]
    public class GetMarketTypeResponse
    {
        [DataMember]
        public string MarketType { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

    }


    
}
