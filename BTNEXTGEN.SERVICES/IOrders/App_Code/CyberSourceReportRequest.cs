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
    public class CyberSourceReportRequest
    {
        [DataMember]
        public string CyberSourceServerName { get; set; }

        [DataMember]
        public string CyberSourceUserName { get; set; }

        [DataMember]
        public string CyberSourcePassword { get; set; }

        [DataMember]
        public string CyberSourceMerchantId { get; set; }

        [DataMember]
        public string ReportName { get; set; }

        [DataMember]
        public string ReportFormat { get; set; }

        [DataMember]
        public string Year { get; set; }

        [DataMember]
        public string Month { get; set; }

        [DataMember]
        public string Day { get; set; }

    }
}