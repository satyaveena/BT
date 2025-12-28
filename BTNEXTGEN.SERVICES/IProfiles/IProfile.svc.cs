using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Data;
using Microsoft.CommerceServer.Runtime.Profiles;
using Microsoft.CommerceServer.Runtime.Diagnostics;
using Microsoft.CommerceServer.Profiles;
using BTNextGen.Services.Common;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;
using System.Configuration;
using System.IO;

namespace IProfiles
{
    public class IProfile : IIProfile
    {
        #region CONSTANTS

        /// <summary>
        /// Root Element of Profile Document
        /// </summary>
        const string XmlRootName = "ProfileDocument";

        #endregion

        #region Public Web Methods

        /// <summary>
        /// Service Web Method for Updating Profiles
        /// </summary>
        /// <param name="profileXml"></param>
        /// <param name="profileKey"></param>
        /// <param name="profileObjectName"></param>
        /// <returns></returns>
        public string UpdatePartialProfile(XmlElement profileXml, string profileKey, string profileObjectName)
        {
            try
            {

                if (profileObjectName.Contains("ErpCnt"))
                {

                    profileObjectName = profileObjectName.Substring(0, profileObjectName.Length - 6);

                }

                // Diagnostic
                //1//System.Diagnostics.EventLog.WriteEntry("WCF IProfile Log",  " Step1");

                //2//
                //XmlDocument xmldocold = new XmlDocument();
                //xmldocold.LoadXml(profileXml.InnerXml);
                //xmldocold.Save(@"C:\Users\csadmin\Documents\ERPAccMsgSent\" + "OldAcclist" + Guid.NewGuid().ToString() + ".xml");

                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);

                XmlElement csProfileXml = context.GetProfile(profileKey, profileObjectName);


                var oldProfileXml = XmlConversions.ToXElement(profileXml);
                var newProfileXml = XmlConversions.ToXElement(csProfileXml);

                var doc = new XmlDocument();
                var updatedProfileXml = doc.CreateElement(XmlRootName);
                var profileObjectElement = doc.CreateElement(profileObjectName);
                XmlElement updatedSubElement;


                IEnumerable<XElement> profileSectionElements = from nfieldElement in newProfileXml.Elements(profileObjectName)
                                                               select nfieldElement;


                // iterate through each profile property group section
                foreach (var profileSection in profileSectionElements.Elements())
                {
                    var profileSectionElement = doc.CreateElement(profileSection.Name.ToString());


                    IEnumerable<XElement> newProfileElements = from nfieldElement in profileSectionElements.Elements()
                                                               select nfieldElement;

                    IEnumerable<XElement> oldProfileElements = from ofieldElement in oldProfileXml.Elements(profileObjectName).Elements(profileSection.Name)
                                                               select ofieldElement;

                    IEnumerable<XElement> unionElements = newProfileElements.Elements().Union<XElement>(oldProfileElements.Elements());


                    // perform updates to XML document
                    foreach (XElement subNewElement in unionElements.Distinct(new XElementNameComparer()))
                    {
                        if (profileSection.Name == subNewElement.Parent.Name)
                        {

                            updatedSubElement = doc.CreateElement(subNewElement.Name.ToString());
                            updatedSubElement.InnerText = subNewElement.Value;
                            profileSectionElement.AppendChild(updatedSubElement);
                            string prevElement = String.Empty;

                            foreach (var subOldElement in oldProfileElements.Elements())
                            {

                                if (subNewElement.Name == subOldElement.Name)
                                {
                                    var iUpdatedSubElement = doc.CreateElement(subOldElement.Name.ToString());
                                    iUpdatedSubElement.InnerText = subOldElement.Value;
                                    if (subOldElement.Name.ToString() != prevElement)
                                    {
                                        profileSectionElement.RemoveChild(updatedSubElement);
                                    }
                                    profileSectionElement.AppendChild(iUpdatedSubElement);
                                    prevElement = subOldElement.Name.ToString();
                                }
                            }
                        }
                    }

                    profileObjectElement.AppendChild(profileSectionElement);

                }

                updatedProfileXml.AppendChild(profileObjectElement);
                //XmlDocument xmldoc = new XmlDocument();
                //xmldoc.LoadXml(updatedProfileXml.InnerXml.ToString());
                //xmldoc.Save(@"C:\Users\csadmin\Documents\ERPAccMsgSent\" + "ToComSrvMsg" + Guid.NewGuid().ToString() + ".xml");
                //System.Diagnostics.EventLog.WriteEntry("WCF IProfile Log", " Step14");

                context.UpdateProfile(updatedProfileXml, true);

                return profileKey;

            }
            catch (Exception ex)
            {
                return profileKey + " : Failed - " + ex.Message;
            }
        }

        /// <summary>
        /// Service Web Method for getting ERP account Number
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        public Collection<ActiveCreditCards> GetCreditCardDetails(int NoOfCards)
        {
            Collection<ActiveCreditCards> collCreditCards = new Collection<ActiveCreditCards>();

            string cardID = System.String.Empty;
            string erpAccNo = System.String.Empty;
            XmlElement profileXml;
            try
            {
                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);
                Microsoft.CommerceServer.SearchClause searchClause = null;
                Microsoft.CommerceServer.SearchClause searchClause1 = null;
                Microsoft.CommerceServer.SearchClause searchClause2 = null;
                Microsoft.CommerceServer.SearchOptions searchOptions = new Microsoft.CommerceServer.SearchOptions();
                //Build the dataset of searchable entities for a profile
                DataSet searchableEntities = context.GetSearchableEntities();

                // Build the search clause for the Accounts Object

                Microsoft.CommerceServer.SearchClauseFactory factory = context.GetSearchClauseFactory(searchableEntities, "CreditCard");
                //searchClause = factory.CreateClause(xmlQuery);
                searchClause1 = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "primary_indicator", "true");
                searchClause2 = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "transmitted_to_erp", "false");

                searchClause = factory.IntersectClauses(searchClause1, searchClause2);
                searchOptions.NumberOfRecordsToReturn = NoOfCards;
                searchOptions.PropertiesToReturn = "id";
                DataSet searchResults = context.ExecuteSearch("CreditCard", searchClause, searchOptions);
                //returnValue = searchResults.GetXml();

                if (searchResults.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in searchResults.Tables[0].Rows)
                    {
                        Collection<AccountDetails> acctsDetails = new Collection<AccountDetails>();
                        cardID = Convert.ToString(row[0]);
                        acctsDetails = GetAccountNumber(cardID);
                        foreach (AccountDetails accDetails in acctsDetails)
                        {
                            ActiveCreditCards objCreditCards = new ActiveCreditCards();
                            profileXml = context.GetProfile(cardID, "CreditCard");
                            objCreditCards.cardID = cardID;
                            objCreditCards.erp_account_id = accDetails.erp_account_num;
                            objCreditCards.is_Tolas = accDetails.is_Tolas;
                            objCreditCards.alias = profileXml.SelectSingleNode("//alias").InnerText;
                            objCreditCards.expiration_month = profileXml.SelectSingleNode("//expiration_month").InnerText;
                            objCreditCards.expiration_year = profileXml.SelectSingleNode("//expiration_year").InnerText;
                            objCreditCards.last_4_digits = profileXml.SelectSingleNode("//last_4_digits").InnerText;
                            objCreditCards.card_type = profileXml.SelectSingleNode("//credit_card_type").InnerText;
                            collCreditCards.Add(objCreditCards);
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return collCreditCards;
        }



        /// <summary>
        /// Service Web Method to get Expired Credit Cards
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        public Collection<ExpiredCreditCards> GetExpiredCards(int currentYear, int month, bool isPrimaryCard)
        {
            Collection<ExpiredCreditCards> collCreditCards = new Collection<ExpiredCreditCards>();

            string cardID = System.String.Empty;
            string connProfiles = ConfigurationManager.ConnectionStrings["CSProfilesConnectionString"].ConnectionString;
            XmlElement profileXml;
            try
            {
                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);
                Microsoft.CommerceServer.SearchClause searchClause = null;
                Microsoft.CommerceServer.SearchClause searchClause1 = null;
                Microsoft.CommerceServer.SearchClause searchClause2 = null;
                Microsoft.CommerceServer.SearchClause searchClause3 = null;
                Microsoft.CommerceServer.SearchClause searchClauseFinal = null;
                Microsoft.CommerceServer.SearchOptions searchOptions = new Microsoft.CommerceServer.SearchOptions();
                //Build the dataset of searchable entities for a profile
                DataSet searchableEntities = context.GetSearchableEntities();

                // Build the search clause for the Accounts Object

                Microsoft.CommerceServer.SearchClauseFactory factory = context.GetSearchClauseFactory(searchableEntities, "CreditCard");
                //searchClause = factory.CreateClause(xmlQuery);
                searchClause1 = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "expiration_month", month);
                searchClause2 = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "expiration_year", currentYear);
                searchClause3 = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "primary_indicator", isPrimaryCard);
                searchClause = factory.IntersectClauses(searchClause1, searchClause2);
                searchClauseFinal = factory.IntersectClauses(searchClause, searchClause3);
                // searchOptions.NumberOfRecordsToReturn = 1;
                searchOptions.PropertiesToReturn = "id";
                DataSet searchResults = context.ExecuteSearch("CreditCard", searchClauseFinal, searchOptions);
                //returnValue = searchResults.GetXml();

                #region startdebug
                //// Create a writer and open the file:
                //StreamWriter log;

                //if (!File.Exists("c:\\prfllogfile.txt"))
                //{
                //  log = new StreamWriter("c:\\prfllogfile.txt");
                //}
                //else
                //{
                //  log = File.AppendText("c:\\prfllogfile.txt");
                //} 

                //// Write to the file:
                //log.WriteLine("1"); 
                //log.WriteLine(DateTime.Now);
                //log.WriteLine();
                //log.Close(); 

                ////
                #endregion 


                if (searchResults.Tables[0].Rows.Count > 0)
                {

                    #region startdebug
                    // Create a writer and open the file:
                    //StreamWriter log;

                    //if (!File.Exists("c:\\prfllogfile.txt"))
                    //{
                    //    log = new StreamWriter("c:\\prfllogfile.txt");
                    //}
                    //else
                    //{
                    //    log = File.AppendText("c:\\prfllogfile.txt");
                    //}

                    //// Write to the file:
                    //log.WriteLine("2");
                    //log.WriteLine(DateTime.Now);
                    //log.WriteLine();
                    //log.Close();
                    #endregion
                    //
                    foreach (DataRow row in searchResults.Tables[0].Rows)
                    {
                        #region startdebug
                        // Create a writer and open the file:

                        //if (!File.Exists("c:\\prfllogfile.txt"))
                        //{
                        //    log = new StreamWriter("c:\\prfllogfile.txt");
                        //}
                        //else
                        //{
                        //    log = File.AppendText("c:\\prfllogfile.txt");
                        //}

                        //// Write to the file:
                        //log.WriteLine("3");
                        //log.WriteLine(DateTime.Now);
                        //log.WriteLine();
                        //log.Close();
                        #endregion
                        //
                        ExpiredCreditCards objExpiredCards = new ExpiredCreditCards();
                        cardID = Convert.ToString(row[0]);
                        #region startdebug
                        //if (!File.Exists("c:\\prfllogfile.txt"))
                        //{
                        //    log = new StreamWriter("c:\\prfllogfile.txt");
                        //}
                        //else
                        //{
                        //    log = File.AppendText("c:\\prfllogfile.txt");
                        //}

                        //// Write to the file:
                        //log.WriteLine("4");
                        //log.WriteLine(DateTime.Now);
                        //log.WriteLine();
                        //log.Close();
                        #endregion
                        //
                        string ERPAccountnum = System.String.Empty;
                        DataAccess da = new DataAccess();
                        da.GetERPAccountNum(cardID, connProfiles, out ERPAccountnum);

                        #region startdebug
                        //if (!File.Exists("c:\\prfllogfile.txt"))
                        //{
                        //    log = new StreamWriter("c:\\prfllogfile.txt");
                        //}
                        //else
                        //{
                        //    log = File.AppendText("c:\\prfllogfile.txt");
                        //}

                        //// Write to the file:
                        //log.WriteLine("5");
                        //log.WriteLine(DateTime.Now);
                        //log.WriteLine(cardID);
                        //log.WriteLine(ERPAccountnum); 
                        //log.Close();
                        #endregion
                        //
                        profileXml = context.GetProfile(cardID, "CreditCard");

                        #region startdebug
                        //if (!File.Exists("c:\\prfllogfile.txt"))
                        //{
                        //    log = new StreamWriter("c:\\prfllogfile.txt");
                        //}
                        //else
                        //{
                        //    log = File.AppendText("c:\\prfllogfile.txt");
                        //}

                        //// Write to the file:
                        //log.WriteLine("6");
                        //log.WriteLine(DateTime.Now);
                        //log.WriteLine();
                        //log.Close();
                        //
                        #endregion

                        if (profileXml.OuterXml.Contains("card_contact_user"))
                        {
                            string card_contact_user = profileXml.SelectSingleNode("//card_contact_user").InnerText;
                            UserOfExpiredCards userDetails = new UserOfExpiredCards();
                            userDetails = GetUserDetails(card_contact_user);
                            objExpiredCards.contact_user = card_contact_user;
                            objExpiredCards.first_name = userDetails.first_name;
                            objExpiredCards.last_name = userDetails.last_name;
                            objExpiredCards.email_address = userDetails.email_address;
                             
                        }
                        objExpiredCards.card_id = cardID;
                        objExpiredCards.erp_account_id = ERPAccountnum; 
                        objExpiredCards.alias_name = profileXml.SelectSingleNode("//alias").InnerText; ;
                        objExpiredCards.expiration_month = profileXml.SelectSingleNode("//expiration_month").InnerText;
                        objExpiredCards.expiration_year = profileXml.SelectSingleNode("//expiration_year").InnerText;
                        objExpiredCards.last_4_digits = profileXml.SelectSingleNode("//last_4_digits").InnerText;
                        collCreditCards.Add(objExpiredCards);
                    }
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return collCreditCards;
        }


        /// <summary>
        /// WCF Service Web Method to get Expired Credit Cards
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        public CreditCardToken GetCreditCardToken(string cardID)
        {
            //Collection<CreditCardToken> collCreditCards = new Collection<CreditCardToken>();
            CreditCardToken objCreditCardToken = new CreditCardToken();
            string ccId = System.String.Empty;
            string strToken = System.String.Empty;
            XmlElement profileXml;
            try
            {
                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);
                Microsoft.CommerceServer.SearchClause searchClause = null;
                Microsoft.CommerceServer.SearchOptions searchOptions = new Microsoft.CommerceServer.SearchOptions();
                //Build the dataset of searchable entities for a profile
                DataSet searchableEntities = context.GetSearchableEntities();

                // Build the search clause for the Accounts Object

                Microsoft.CommerceServer.SearchClauseFactory factory = context.GetSearchClauseFactory(searchableEntities, "CreditCard");
                //searchClause = factory.CreateClause(xmlQuery);
                searchClause = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "id", cardID);
                searchOptions.NumberOfRecordsToReturn = 1;
                searchOptions.PropertiesToReturn = "id";
                DataSet searchResults = context.ExecuteSearch("CreditCard", searchClause, searchOptions);

                if (searchResults.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in searchResults.Tables[0].Rows)
                    {

                        ccId = Convert.ToString(row[0]);
                        profileXml = context.GetProfile(ccId, "CreditCard");
                        if (profileXml.OuterXml.Contains("card_contact_user"))
                        {
                            string card_contact_user = profileXml.SelectSingleNode("//card_contact_user").InnerText;
                            // reusing the method to get user contact by passing the card_contact_user
                            UserOfExpiredCards userDetails = new UserOfExpiredCards();
                            userDetails = GetUserDetails(card_contact_user);
                            objCreditCardToken.email_address = userDetails.email_address;
                        }
                        objCreditCardToken.cardID = ccId;
                      strToken = GetDecryptedID(ccId, "cc_number", "GeneralInfo", "CreditCard");
                        objCreditCardToken.cardToken = strToken;
                        //collCreditCards.Add(objCreditCardToken);

                    }
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return objCreditCardToken;
        }

        /// <summary>
        /// WCF Service Web Method to get Expired Credit Cards
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        public OrgAccounts GetOrgIDFromBillTo(string billtoGuid, string shipTo)
        {
            //Collection<OrgAccounts> colOrgAccounts = new Collection<OrgAccounts>();
            string org_id = System.String.Empty;
            OrgAccounts objOrgAccounts = new OrgAccounts();
            XmlDocument csOrg = new XmlDocument();
            try
            {
                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);
                Microsoft.CommerceServer.SearchClause searchClause = null;
                Microsoft.CommerceServer.SearchOptions searchOptions = new Microsoft.CommerceServer.SearchOptions();
                //Build the dataset of searchable entities for a profile
                DataSet searchableEntities = context.GetSearchableEntities();

                // Build the search clause for the Accounts Object

                Microsoft.CommerceServer.SearchClauseFactory factory = context.GetSearchClauseFactory(searchableEntities, "BTAccount");
                //searchClause = factory.CreateClause(xmlQuery);
                searchClause = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "account_id", billtoGuid);
                searchOptions.NumberOfRecordsToReturn = 1;
                searchOptions.PropertiesToReturn = "org_id";
                DataSet searchResults = context.ExecuteSearch("BTAccount", searchClause, searchOptions);

                if (searchResults.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in searchResults.Tables[0].Rows)
                    {

                        org_id = Convert.ToString(row[0]);
                        string accList = String.Empty;
                        if (org_id.Trim() != String.Empty)
                        {
                            //accList = GetAccountList(org_id,shipTo);
                            csOrg = GetAccountList(org_id, shipTo);
                        }

                        
                        objOrgAccounts.orgid = org_id;
                        objOrgAccounts.u_accounts = csOrg.DocumentElement;
                        objOrgAccounts.shipTo = shipTo;


                    }
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return objOrgAccounts;
        }

        public Collection<ProfilesResponse> ImportMultipleProfiles(Collection<ProfilesRequest> xmlProfiles)
        {
            ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
            ProfileManagementContext context = ProfileManagementContext.Create(psAgent);
            Collection<ProfilesResponse> clcResponse = new Collection<ProfilesResponse>();


            foreach (ProfilesRequest singleProfile in xmlProfiles)
            {
                string strIdentifier = string.Empty;
                string profileType = string.Empty;
                string keyName = string.Empty;
                ProfilesResponse Response = new ProfilesResponse();

                try
                {

                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(singleProfile.xmlProfile.OuterXml);

                    // get the profile object
                    profileType = xdoc.DocumentElement.FirstChild.Name;
                    if (profileType.Equals("BTAccount"))
                    {
                        strIdentifier = xdoc.DocumentElement.SelectSingleNode("//account_id").InnerText;
                        keyName = "u_bt_account_id";
                    }

                    if (profileType.Equals("Address"))
                    {
                        strIdentifier = xdoc.DocumentElement.SelectSingleNode("//address_id").InnerText;
                        keyName = "u_address_id";
                    }


                    ProfileOperationResponse profResp = context.CreateProfile(singleProfile.xmlProfile);
                    Response.LoadStatus = "Success";
                    Response.xmlProfile = profResp;
                    clcResponse.Add(Response);

                }


                catch (Exception ex)
                {
                    ProfileOperationResponse profResp = new ProfileOperationResponse();
                    profResp.PrimaryKeyName = keyName;
                    profResp.PrimaryKeyValue = strIdentifier;
                    profResp.ProfileType = profileType;
                    Response.LoadStatus = ex.Message;
                    Response.xmlProfile = profResp;
                    clcResponse.Add(Response);
                    // return clcResponse;

                }

            }
            return clcResponse;
        }

        public AccountListAndCount RemoveAssociatedAccounts(String accounList, String strOrgID)
        {
            string updatedAccountList = String.Empty;
            accounList = accounList.Trim();
            AccountListAndCount accLstCnt = new AccountListAndCount();
            StringBuilder StrBldrAccountList = new StringBuilder();
            StringBuilder StrBldrOrgList = new StringBuilder();
            int iCounter = 0;
            char[] accountDelimiter = { ';' };
            try
            {
                String[] accountList = accounList.Split(accountDelimiter);
                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);


                foreach (string singleAccount in accountList)
                {
                    Microsoft.CommerceServer.SearchClause searchClause = null;
                    Microsoft.CommerceServer.SearchOptions searchOptions = new Microsoft.CommerceServer.SearchOptions();
                    //Build the dataset of searchable entities for a profile
                    DataSet searchableEntities = context.GetSearchableEntities();

                    // Build the search clause for the Accounts Object

                    Microsoft.CommerceServer.SearchClauseFactory factory = context.GetSearchClauseFactory(searchableEntities, "BTAccount");
                    //searchClause = factory.CreateClause(xmlQuery);
                    searchClause = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "account_id", singleAccount);
                    searchOptions.NumberOfRecordsToReturn = 1;
                    searchOptions.PropertiesToReturn = "org_id";
                    DataSet searchResults = context.ExecuteSearch("BTAccount", searchClause, searchOptions);
                    if (searchResults.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in searchResults.Tables[0].Rows)
                        {
                            String org_id = Convert.ToString(row[0]);
                            org_id = org_id.Trim();
                            if (org_id == strOrgID || org_id.Equals(String.Empty) || org_id== null)
                            {
                                StrBldrAccountList = StrBldrAccountList.Append(singleAccount);
                                StrBldrAccountList = StrBldrAccountList.Append(';');
                                iCounter = iCounter + 1;
                            }

                            else
                            {
                                StrBldrOrgList = StrBldrOrgList.Append(org_id);
                                StrBldrOrgList = StrBldrOrgList.Append(';');
                            }

                        }
                    }
                }

            }

            catch (Exception ex)
            {

            }

            updatedAccountList = StrBldrAccountList.ToString();
            accLstCnt.accountList = updatedAccountList;
            accLstCnt.accountCount = iCounter.ToString();
            accLstCnt.existingOrg = StrBldrOrgList.ToString();

            return accLstCnt;
        }


        public OrgName CheckAssociatedAccounts(String singleAccount, string strOrgID, string legacyOrgID)
        {
            OrgName orgAcct = new OrgName();
            orgAcct.IsAssociated= false; // default is false( not associated)
            bool checkAssociation;
            try
            {
                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);
                Microsoft.CommerceServer.SearchClause searchClause = null;
                Microsoft.CommerceServer.SearchOptions searchOptions = new Microsoft.CommerceServer.SearchOptions();
                //Build the dataset of searchable entities for a profile
                DataSet searchableEntities = context.GetSearchableEntities();
               

                // Build the search clause for the Accounts Object

                Microsoft.CommerceServer.SearchClauseFactory factory = context.GetSearchClauseFactory(searchableEntities, "BTAccount");
                //searchClause = factory.CreateClause(xmlQuery);
                searchClause = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "account_id", singleAccount);
                searchOptions.NumberOfRecordsToReturn = 1;
                searchOptions.PropertiesToReturn = "org_id,is_billing_account,legacy_org_id";
                DataSet searchResults = context.ExecuteSearch("BTAccount", searchClause, searchOptions);
                if (searchResults.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in searchResults.Tables[0].Rows)
                    {
                        string org_id = Convert.ToString(row[0]);
                        bool is_billing = Convert.ToBoolean(row[1]);
                        string legacy_org_id = Convert.ToString(row[2]);
                        org_id = org_id.Trim();

                        // if it is a billing account, then check if the legacy orgID is already updated.
                        // if legacyOrgID is populated with the current legacyOrgID , then do check for association
                        if (is_billing)
                        {
                            if (legacy_org_id.Equals(legacyOrgID))
                            {
                                checkAssociation = false;
                            }
                            else
                            {
                                checkAssociation = true;
                            }

                        }
                       // if it is a shipping account, then always check for association
                        else
                        {
                            checkAssociation = true;
                        }


                        // if the flag is true do the orgID comparision and determine the orgName.

                        if (checkAssociation)
                        {

                            if (org_id != strOrgID && org_id != String.Empty && org_id != null)
                            {

                                orgAcct.orgName = GetOrgName(org_id);
                                orgAcct.IsAssociated = true;
                            }


                            else
                            {
                                orgAcct.IsAssociated = false;
                            }
                        }
                    }
                }

            }
          
            catch( Exception ex)
            {
                throw ex;
            }
            return orgAcct;

        }

        #endregion

        #region Private Methods


        /// <summary>
        /// Service Web Method for getting ERP account Number
        /// </summary>
        /// <param name="cardID"></param>
        /// <returns></returns>
        private Collection<AccountDetails> GetAccountNumber(string cardID)
        {
            Collection<AccountDetails> objAcctsDetails = new Collection<AccountDetails>();
            string returnValue = System.String.Empty;
            try
            {
                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);
                Microsoft.CommerceServer.SearchClause searchClause = null;
                Microsoft.CommerceServer.SearchClause searchClause1 = null;
                Microsoft.CommerceServer.SearchClause searchClause2 = null;
                //Microsoft.CommerceServer.SearchClause searchClause3 = null;
                //Microsoft.CommerceServer.SearchClause searchClause4 = null;
                Microsoft.CommerceServer.SearchOptions searchOptions = new Microsoft.CommerceServer.SearchOptions();
                //Build the dataset of searchable entities for a profile
                DataSet searchableEntities = context.GetSearchableEntities();

                // Build the search clause for the Accounts Object

                //Since UnionClauses are not supported by Profiles Subsystem, we do 2 separate searches in order
                //  to account for the cc_transmitted_to_erp flag being EITHER empty OR Not True (NULL does not evaluate as NOT True)
                //  - First check if it is NULL
                Microsoft.CommerceServer.SearchClauseFactory factory = context.GetSearchClauseFactory(searchableEntities, "BTAccount");
                searchClause1 = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "preferred_credit_card", cardID);
                searchClause2 = factory.CreateClause(Microsoft.CommerceServer.ImplicitComparisonOperator.IsEmpty, "cc_transmitted_to_erp");
                searchClause = factory.IntersectClauses(searchClause1, searchClause2);
                searchOptions.PropertiesToReturn = "erp_account_number,is_TOLAS";
                DataSet searchResults = context.ExecuteSearch("BTAccount", searchClause, searchOptions);
                if (searchResults.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in searchResults.Tables[0].Rows)
                    {
                        AccountDetails objAccDetails = new AccountDetails();
                        objAccDetails.erp_account_num = Convert.ToString(row["BTNextGen.erp_account_number"]);
                        objAccDetails.is_Tolas = Convert.ToString(row["BTNextGen.is_TOLAS"]);
                        objAcctsDetails.Add(objAccDetails);
                    }
                }

                //  - Then check if it is NOT true.
                searchClause1 = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "preferred_credit_card", cardID);
                searchClause2 = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.NotEqual, "cc_transmitted_to_erp", "true");
                searchClause = factory.IntersectClauses(searchClause1, searchClause2);
                searchOptions.PropertiesToReturn = "erp_account_number,is_TOLAS";
                DataSet searchResults2 = context.ExecuteSearch("BTAccount", searchClause, searchOptions);
                if (searchResults2.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in searchResults2.Tables[0].Rows)
                    {
                        AccountDetails objAccDetails = new AccountDetails();
                        objAccDetails.erp_account_num = Convert.ToString(row["BTNextGen.erp_account_number"]);
                        objAccDetails.is_Tolas = Convert.ToString(row["BTNextGen.is_TOLAS"]);
                        objAcctsDetails.Add(objAccDetails);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objAcctsDetails;
        }

        private UserOfExpiredCards GetUserDetails(string user_id)
        {
            UserOfExpiredCards objUserDetails = new UserOfExpiredCards();
            string returnValue = System.String.Empty;
            try
            {
                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);
                Microsoft.CommerceServer.SearchClause searchClause = null;
                Microsoft.CommerceServer.SearchOptions searchOptions = new Microsoft.CommerceServer.SearchOptions();
                //Build the dataset of searchable entities for a profile
                DataSet searchableEntities = context.GetSearchableEntities();

                // Build the search clause for the Accounts Object

                Microsoft.CommerceServer.SearchClauseFactory factory = context.GetSearchClauseFactory(searchableEntities, "UserObject");
                searchClause = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "user_id", user_id);
                searchOptions.NumberOfRecordsToReturn = 1;
                searchOptions.PropertiesToReturn = "first_name,last_name,email_address";
                DataSet searchResults = context.ExecuteSearch("UserObject", searchClause, searchOptions);
                if (searchResults.Tables[0].Rows.Count > 0)
                {
                    objUserDetails.first_name = Convert.ToString(searchResults.Tables[0].Rows[0]["GeneralInfo.first_name"]);
                    objUserDetails.last_name = Convert.ToString(searchResults.Tables[0].Rows[0]["GeneralInfo.last_name"]);
                    objUserDetails.email_address = Convert.ToString(searchResults.Tables[0].Rows[0]["GeneralInfo.email_address"]);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objUserDetails;
        }

        private XmlDocument GetAccountList(string orgid, string shipTo)
        {
            string accountList = String.Empty;

            try
            {
                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);

                XmlElement csProfileXml = context.GetProfile(orgid, "Organization");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml("<Accounts ></Accounts>");
                XmlNode root = xmlDoc.DocumentElement;

                XmlNodeList accListNode = csProfileXml.SelectNodes("//account_list");
                string list = string.Empty;

                foreach (XmlNode xnode in accListNode)
                {
                    if (xnode.InnerText.Trim() != string.Empty && xnode.InnerText != null)
                    {
                        if (!(xnode.InnerText.Trim().Equals(shipTo)))
                        {
                            XmlElement acclist = xmlDoc.CreateElement("account_list");
                            acclist.InnerText = xnode.InnerText;
                            root.AppendChild(acclist);
                        }
                        else
                        {
                            xmlDoc = null;
                            xmlDoc.LoadXml("<Accounts ></Accounts>");
                            break;
                        }
                    }
                }


                return xmlDoc;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            // return accountList;
        }

        private string GetDecryptedID(string id, string profileProperty, string profileGroup, string ProfileName)
        {
            string strReturn = String.Empty;
            string registerKey = Config.RegistryKey;
            try
            {
                // publicKey and privateKey values generated by ProfileKeyManager.exe.
                string publicKey = "";
                string privateKey = "";
               // string registerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Commerce Server 2007 Keys\TS360";

               // System.Diagnostics.EventLog.WriteEntry("the reg path is ", registerKey);

                publicKey = GetRegistryValue(registerKey, "PublicKey");
               // System.Diagnostics.EventLog.WriteEntry("public ", publicKey);
                privateKey = GetRegistryValue(registerKey, "PrivateKey");
               // System.Diagnostics.EventLog.WriteEntry("private", privateKey);
                bool cbDecrypt = true; 

                // Profile Connection String - value can be retrieved from Commerce Server.
                // Manager, Console Root -> Commerce Server Manager -> Global Resources -> 
                // Profiles, right-click and select Properties, highlight 
                // s_ProfileServiceConnectionString.
                //string connString = "Provider=CSOLEDB;Data Source=<computername>;Initial Catalog=<CS Site>_commerce;Integrated Security=SSPI;";
                //string connString = @"Provider=CSOLEDB;Data Source=128.11.98.245\COMMERCESERVER;Initial Catalog=Nextgen_profiles;Integrated Security=SSPI;";

                string connString = Config.CSSqlConnString;

                // To decrypt, specify PrivateKey1= and KeyIndex1= if you encrypted 
                // using /i 1 with ProfileKeyManager, or PrivateKey2= and KeyIndex2= if you specified 
                // /i 2 with ProfileKeyManager. 
                if (cbDecrypt)
                    connString += "PublicKey=" + publicKey + ";KeyIndex=1;PrivateKey1=" + privateKey + ";";

                ConsoleDebugContext debugContext = new ConsoleDebugContext(DebugMode.Debug);
                ProfileContext rtCtxt = new ProfileContext(connString, debugContext);
                Profile p = rtCtxt.GetProfile(id, ProfileName);
                foreach (ProfilePropertyGroup ppg in p.PropertyGroups)
                {
                    if (ppg.Name == profileGroup)
                    {
                        foreach (ProfileProperty pp in ppg.Properties)
                        {
                            if (pp.Name == profileProperty)
                            {
                                strReturn = pp.Value.ToString();
                                break;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return strReturn;
        }

        private string GetRegistryValue(string keyName, string valueName)
        {

            byte[] cipherText = Registry.GetValue(keyName, valueName, null) as byte[];

            return DecryptString(cipherText);

        }

        private string DecryptString(byte[] cipherText)
        {

            string str;

            byte[] bytes = Encoding.UTF8.GetBytes("Commerce Server Registry Secrets");

            byte[] buffer2 = ProtectedData.Unprotect(cipherText, bytes, DataProtectionScope.LocalMachine);

            str = Encoding.UTF8.GetString(buffer2);

            return str;

        }
        private string AppendCount(string list)
        {

            int icount = list.Length - list.Replace("}", "").Length;
            list = icount + ";" + list;
            return list;

        }

          private string GetOrgName(string org_id)
        {

            string orgName = System.String.Empty;
            try
            {
                ProfilesServiceAgent psAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
                ProfileManagementContext context = ProfileManagementContext.Create(psAgent);
                Microsoft.CommerceServer.SearchClause searchClause = null;
                Microsoft.CommerceServer.SearchOptions searchOptions = new Microsoft.CommerceServer.SearchOptions();
                //Build the dataset of searchable entities for a profile
                DataSet searchableEntities = context.GetSearchableEntities();

                // Build the search clause for the Accounts Object

                Microsoft.CommerceServer.SearchClauseFactory factory = context.GetSearchClauseFactory(searchableEntities, "Organization");
                searchClause = factory.CreateClause(Microsoft.CommerceServer.ExplicitComparisonOperator.Equal, "org_id", org_id);
                searchOptions.NumberOfRecordsToReturn = 1;
                searchOptions.PropertiesToReturn = "name";
                DataSet searchResults = context.ExecuteSearch("Organization", searchClause, searchOptions);
                if (searchResults.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in searchResults.Tables[0].Rows)
                    {
                        orgName = Convert.ToString(row[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return orgName;

        }

        #endregion

    }
}
