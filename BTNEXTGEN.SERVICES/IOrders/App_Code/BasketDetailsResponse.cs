using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BTNextGen.Services.IOrders
{
    [DataContract]
    public class BasketDetailsResponse
    {
        [DataMember]
        public string LegacyBasketId { get; set; }

        [DataMember]
        public string LegacySourceSystem { get; set; }

        [DataMember]
        public string LoadStatus { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

    }

    /// <summary>
    /// Data structure definition
    /// </summary>
    [CollectionDataContract]
    public class BasketDetailsResponseCollection : List<BasketDetailsResponse>
    {
        [DataMember]
        public List<BasketDetailsResponse> BasketResponseCollection { get; set; }
    }
}