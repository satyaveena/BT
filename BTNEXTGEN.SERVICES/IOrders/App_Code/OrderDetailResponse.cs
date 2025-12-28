using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace BTNextGen.Services.IOrders
{
    [DataContract]
    public class OrderDetailResponse
    {

        [DataMember]
        public string PONumber { get; set; }

        [DataMember]
        public string AccountNum { get; set; }

        [DataMember]
        public string ISBN { get; set; }

        [DataMember]
        public string Quantity { get; set; }

    }

    [CollectionDataContract]
    public class OrderDetailResponseCollection : List<OrderDetailResponse>
    {
        [DataMember]
        public List<OrderDetailResponse> OrderResponseCollection { get; set; }

    }
}