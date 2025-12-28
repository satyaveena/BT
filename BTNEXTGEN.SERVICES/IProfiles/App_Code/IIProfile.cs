using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Data;
using Microsoft.CommerceServer.Profiles;



namespace IProfiles
{

    [ServiceContract]
    public interface IIProfile
    {
        [OperationContract]
        string UpdatePartialProfile(XmlElement profileXml, string profileKey, string profileObjectName);

        [OperationContract]
        Collection<ActiveCreditCards> GetCreditCardDetails(int NoOfCards);

        [OperationContract]
        Collection<ExpiredCreditCards> GetExpiredCards(int currentYear, int month, bool isPrimaryCard);

        [OperationContract]
       CreditCardToken GetCreditCardToken(string cardID);


        [OperationContract]
        OrgAccounts GetOrgIDFromBillTo(string billtoGuid, string shipTo);

        [OperationContract]
        Collection<ProfilesResponse> ImportMultipleProfiles(Collection<ProfilesRequest> xmlProfiles);

        [OperationContract  ]
        AccountListAndCount RemoveAssociatedAccounts(String accounList, String strOrgID);

        [OperationContract]
        OrgName CheckAssociatedAccounts(String singleAccount, String strOrgID, string legacyOrg);

       
    }

    /// <summary>
    /// Data structure definition
    /// </summary>
    [DataContract]
    public class ActiveCreditCards
    {
        [DataMember]
        public string cardID { get; set; }
        [DataMember]
        public string erp_account_id { get; set; }
        [DataMember]
        public string last_4_digits { get; set; }
        [DataMember]
        public string expiration_month { get; set; }
        [DataMember]
        public string expiration_year { get; set; }
        [DataMember]
        public string alias { get; set; }
        [DataMember]
        public string card_type { get; set; }
        [DataMember]
        public string is_Tolas { get; set; }

    }


    /// <summary>
    /// Data structure definition
    /// </summary>
    [DataContract]
    public class AccountDetails
    {

        [DataMember]
        public string erp_account_num { get; set; }

        [DataMember]
        public string is_Tolas { get; set; }

    }

    [DataContract]
    public class ExpiredCreditCards
    {
        [DataMember]
        public string contact_user { get; set; }
        [DataMember]
        public string card_id { get; set; }
        [DataMember]
        public string alias_name { get; set; }
        [DataMember]
        public string expiration_month { get; set; }
        [DataMember]
        public string expiration_year { get; set; }
        [DataMember]
        public string last_4_digits { get; set; }
        [DataMember]
        public string first_name { get; set; }
        [DataMember]
        public string last_name { get; set; }
        [DataMember]
        public string email_address { get; set; }
        [DataMember]
        public string erp_account_id { get; set; }



    }

    [DataContract]
    public class UserOfExpiredCards
    {
        [DataMember]
        public string first_name { get; set; }
        [DataMember]
        public string last_name { get; set; }
        [DataMember]
        public string email_address { get; set; }
    }

    [DataContract]
    public class CreditCardToken
    {
        [DataMember]
        public string cardID { get; set; }
        [DataMember]
        public string cardToken { get; set; }
        [DataMember]
        public string email_address { get; set; }

    }

    [DataContract]
    public class OrgAccounts
    {
        [DataMember]
        public string orgid { get; set; }
        [DataMember]
        public XmlElement u_accounts { get; set; }
        [DataMember]
        public string shipTo { get; set; }

    }

    [DataContract]
    public class ProfilesRequest
    {
        [DataMember]
        public XmlElement xmlProfile { get; set; }
    }

    [DataContract]
    public class ProfilesResponse
    {
        [DataMember]
        public ProfileOperationResponse xmlProfile { get; set; }
        [DataMember]
        public string LoadStatus { get; set; }

    }

    /// <summary>
    /// Data structure definition
    /// </summary>
    [DataContract]
    public class AccountListAndCount
    {
        [DataMember]
        public string accountList { get; set; }
        [DataMember]
        public string accountCount { get; set; }
        [DataMember]
        public string existingOrg { get; set; }

    }

       /// <summary>
    /// Data structure definition
    /// </summary>
    [DataContract]
    public class OrgName
    {
        [DataMember]
        public string orgName { get; set; }
        [DataMember]
        public bool IsAssociated { get; set; }
    }

  }
