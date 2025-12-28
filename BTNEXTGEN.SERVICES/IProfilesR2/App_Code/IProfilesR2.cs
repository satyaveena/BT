using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace IProfilesR2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProfilesR2" in both code and config file together.
    [ServiceContract]
    public interface IProfilesR2Service
    {


        [OperationContract]
        VerifyUserCredentialsResponse VerifyUser(VerifyUserCredentials VerifyCredentialsList);

        [OperationContract]
        VerifyCredentialsResponse VerifyUserPassword(VerifyCredentials VerifyCredentialsList);


        [OperationContract]
        XElement GetBasketDatasetXML(Guid request);
        
    }
}
