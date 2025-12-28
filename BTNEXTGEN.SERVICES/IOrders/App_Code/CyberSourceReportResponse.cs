using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net;


namespace BTNextGen.Services.IOrders
{
    [DataContract]
    public class CyberSourceReportResponse
    {
        [DataMember]
        HttpWebResponse ReportResponse { get; set; }


        [DataMember]
        public string ErrorMessage { get; set; }

    }
}