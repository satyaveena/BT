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
    /// <summary>
    /// 
    /// </summary>
    /// 
    

   [DataContract]
   public class VerifyUserCredentials

    {
       [DataMember]
       public string VerifyUserid { get; set; }


   }


}