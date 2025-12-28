using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace IUserAlerts
{
    [DataContract]
    public class CreateUserAlertMessageResponse

    {

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

    }

    [DataContract]
    public class GetUserAlertMessageTemplateResponse
    {
        [DataMember]
        public string AlertMessageTemplate { get; set; }

        [DataMember]
        public string ConfigReferenceValue { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

    }


 
    
}