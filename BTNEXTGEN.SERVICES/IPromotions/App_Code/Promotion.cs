using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BTNextGen.Services.IPromotions
{
    /// <summary>
    /// Data structure definition
    /// </summary>
    [DataContract]
    public class Promotion
    {
        [DataMember]
        public string PromoName {get; set;}

        [DataMember]
        public string PromoDescription { get; set; }

        [DataMember]
        public string PromoType { get; set; }

        [DataMember]
        public string PromoStartDate { get; set; }

        [DataMember]
        public string PromoEndDate { get; set; }  
    }

}