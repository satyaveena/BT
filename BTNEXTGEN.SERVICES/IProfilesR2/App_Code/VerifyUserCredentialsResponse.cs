using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace IProfilesR2

{
    


    [DataContract]
    public class VerifyUserCredentialsResponse


    {
        [DataMember]
        public string VerifyStatus { get; set; }
        
        [DataMember]
        public string UserGUID { get; set; }

        [DataMember]
        public string Message { get; set; }

    }
}