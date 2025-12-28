using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Data;
using Microsoft.CommerceServer;
using Microsoft.CommerceServer.Runtime.Profiles;
using Microsoft.CommerceServer.Runtime.Diagnostics;
using Microsoft.CommerceServer.Profiles;
using Microsoft.CommerceServer.Orders;

using BTNextGen.Services.Common;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;
using System.Globalization;
using System.Net;
using Microsoft.CommerceServer.Runtime;
using System.IO;
using Microsoft.CommerceServer.Runtime.Orders;

using BTNextGen.Commerce.Portal.Common;




namespace IProfilesR2
{





    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProfilesR2" in code, svc and config file together.
    public class ProfilesR2 : IProfilesR2Service
    {


        // Name of the user profile.
        const string UserProfileName = "UserObject";

        // Property that holds the logon name.
        //const string VerifyUserLoginPropertyName = "GeneralInfo.email_address";
        const string UserLoginPropertyName = "BTNextGen.user_name";

        // Property that holds the password.
        const string UserPasswordPropertyName = "GeneralInfo.user_security_password";

        // Property that holds the user id GUID
        const string UserGUIDName = "GeneralInfo.user_id";



        private const int SaltValueSize = 4;

        // Hashing algorithms used to verify one-way-hashed passwords:
        // MD5 is used for backward compatibility with Commerce Server 2002. If you have no legacy data, MD5 can be removed.
        // SHA256 is used on Windows Server 2003.
        // SHA1 should be used on Windows XP (SHA256 is not supported).
        private static string[] HashingAlgorithms = new string[] { "SHA256", "MD5" };

        public VerifyCredentialsResponse VerifyUserPassword(VerifyCredentials VerifyCredentials)
        {
            Boolean verifyStatusFlag = true;

            VerifyCredentialsResponse verifyResponse = new VerifyCredentialsResponse();
            VerifyCredentials verifyidentify = new VerifyCredentials();
            verifyidentify.VerifyPassword = VerifyCredentials.VerifyPassword;
            verifyidentify.VerifyUserid = VerifyCredentials.VerifyUserid;
            ProfileContext oProfileSystem = null;
            if (null == CommerceContext.Current)
                //oProfileSystem = new ProfileContext("xxx", new ConsoleDebugContext(DebugMode.Checked));
                oProfileSystem = new ProfileContext(Config.CSSqlConnString, new ConsoleDebugContext(DebugMode.Checked));
            else
                oProfileSystem = CommerceContext.Current.ProfileSystem;

            //Profile userProfile = oProfileSystem.GetProfile(UserLoginPropertyName, userid, UserProfileName);



            //Profile userProfile = CommerceContext.Current.ProfileSystem.GetProfile(UserLoginPropertyName, VerifyCredentials.VerifyUserid, UserProfileName);

            Profile userProfile = oProfileSystem.GetProfile(UserLoginPropertyName, VerifyCredentials.VerifyUserid, UserProfileName);

            try
            {



                if (userProfile != null)
                {
                    ProfileProperty passwordProperty = userProfile[UserPasswordPropertyName];
                    string userPassword = passwordProperty.Value.ToString();

                    ProfileProperty guidProperty = userProfile[UserGUIDName];
                    string userGUID = guidProperty.Value.ToString();


                    bool isHashed = false;
                    ProfilePropertyAttribute encryptionType = passwordProperty.Attributes["encryptiontype"];
                    if (encryptionType != null)
                        isHashed = string.Equals(encryptionType.Value.ToString(), "1", StringComparison.Ordinal);

                    if (isHashed)
                    {
                        verifyStatusFlag = VerifyHashedPassword(verifyidentify.VerifyPassword, userPassword);
                    }
                    else
                    {
                        verifyStatusFlag = string.Equals(verifyidentify.VerifyPassword, userPassword, StringComparison.Ordinal);

                    }

                    verifyResponse.UserGUID = userGUID;

                    if (verifyStatusFlag == false)
                    {
                        verifyResponse.VerifyStatus = "Invalid Password";
                    }
                    else
                    {
                        verifyResponse.VerifyStatus = "Valid";
                    }

                }

                else
                {

                    verifyResponse.VerifyStatus = "Invalid UserID";

                }


                return verifyResponse;
            }

            catch (Exception exlax)
            {


                verifyResponse.VerifyStatus = "Error";
                verifyResponse.Message = exlax.Message;
                return verifyResponse;


            }
        }

        public bool VerifyHashedPassword(string password, string profilePassword)
        {
            int saltLength = SaltValueSize * UnicodeEncoding.CharSize;

            if (string.IsNullOrEmpty(profilePassword) ||
                string.IsNullOrEmpty(password) ||
                profilePassword.Length < saltLength)
            {
                return false;
            }

            // Strip the salt value off the front of the stored password.
            string saltValue = profilePassword.Substring(0, saltLength);

            foreach (string hashingAlgorithmName in HashingAlgorithms)
            {
                HashAlgorithm hash = HashAlgorithm.Create(hashingAlgorithmName);
                string hashedPassword = HashPassword(password, saltValue, hash);
                if (profilePassword.Equals(hashedPassword, StringComparison.Ordinal))
                    return true;
            }

            // None of the hashing algorithms could verify the password.
            return false;
        }



        private static string GenerateSaltValue()
        {
            UnicodeEncoding utf16 = new UnicodeEncoding();

            if (utf16 != null)
            {
                // Create a random number object seeded from the value
                // of the last random seed value. This is done
                // interlocked because it is a static value and we want
                // it to roll forward safely.

                Random random = new Random(unchecked((int)DateTime.Now.Ticks));

                if (random != null)
                {
                    // Create an array of random values.

                    byte[] saltValue = new byte[SaltValueSize];

                    random.NextBytes(saltValue);

                    // Convert the salt value to a string. Note that the resulting string
                    // will still be an array of binary values and not a printable string. 
                    // Also it does not convert each byte to a double byte.

                    string saltValueString = utf16.GetString(saltValue);

                    // Return the salt value as a string.

                    return saltValueString;
                }
            }

            return null;
        }

        private static string HashPassword(string clearData, string saltValue, HashAlgorithm hash)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();

            if (clearData != null && hash != null && encoding != null)
            {
                // If the salt string is null or the length is invalid then
                // create a new valid salt value.

                if (saltValue == null)
                {
                    // Generate a salt string.
                    saltValue = GenerateSaltValue();
                }

                // Convert the salt string and the password string to a single
                // array of bytes. Note that the password string is Unicode and
                // therefore may or may not have a zero in every other byte.

                byte[] binarySaltValue = new byte[SaltValueSize];

                binarySaltValue[0] = byte.Parse(saltValue.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[1] = byte.Parse(saltValue.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[2] = byte.Parse(saltValue.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[3] = byte.Parse(saltValue.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);

                byte[] valueToHash = new byte[SaltValueSize + encoding.GetByteCount(clearData)];
                byte[] binaryPassword = encoding.GetBytes(clearData);

                // Copy the salt value and the password to the hash buffer.

                binarySaltValue.CopyTo(valueToHash, 0);
                binaryPassword.CopyTo(valueToHash, SaltValueSize);

                byte[] hashValue = hash.ComputeHash(valueToHash);

                // The hashed password is the salt plus the hash value (as a string).

                string hashedPassword = saltValue;

                foreach (byte hexdigit in hashValue)
                {
                    hashedPassword += hexdigit.ToString("X2", CultureInfo.InvariantCulture.NumberFormat);
                }

                // Return the hashed password as a string.

                return hashedPassword;
            }

            return null;
        }


        // Name of the user profile.
        const string VerifyUserProfileName = "UserObject";

        // Property that holds the logon name.
        const string VerifyUserLoginPropertyName = "BTNextGen.user_name";


        // Property that holds the password.
        const string VerifyUserPasswordPropertyName = "GeneralInfo.user_security_password";

        // Property that holds the user id GUID
        const string VerifyUserGUIDName = "GeneralInfo.user_id";



        //private const int SaltValueSize = 4;

        // Hashing algorithms used to verify one-way-hashed passwords:
        // MD5 is used for backward compatibility with Commerce Server 2002. If you have no legacy data, MD5 can be removed.
        // SHA256 is used on Windows Server 2003.
        // SHA1 should be used on Windows XP (SHA256 is not supported).
        //private static string[] HashingAlgorithms = new string[] { "SHA256", "MD5" };

        public VerifyUserCredentialsResponse VerifyUser(VerifyUserCredentials VerifyUserCredentials)
        {
            VerifyUserCredentialsResponse verifyUserResponse = new VerifyUserCredentialsResponse();
            VerifyUserCredentials verifyUseridentify = new VerifyUserCredentials();
            verifyUseridentify.VerifyUserid = VerifyUserCredentials.VerifyUserid;

            //ProfilesServiceAgent msoAgent = new ProfilesServiceAgent(Config.ProfilesServiceUrl);
            //msoAgent.Credentials = CredentialCache.DefaultNetworkCredentials;

            //ProfileManagementContext context = ProfileManagementContext.Create(msoAgent);
            ProfileContext oProfileSystem = null;
            if (null == CommerceContext.Current)
                oProfileSystem = new ProfileContext(Config.CSSqlConnString, new ConsoleDebugContext(DebugMode.Checked));
            else
                oProfileSystem = CommerceContext.Current.ProfileSystem;

            //Profile userProfile = oProfileSystem.GetProfile(UserLoginPropertyName, userid, UserProfileName);



            //Profile userProfile = CommerceContext.Current.ProfileSystem.GetProfile(UserLoginPropertyName, VerifyCredentials.VerifyUserid, UserProfileName);

            Profile useronlyProfile = oProfileSystem.GetProfile(VerifyUserLoginPropertyName, VerifyUserCredentials.VerifyUserid, VerifyUserProfileName);

            try
            {

                if (useronlyProfile != null)
                {

                    ProfileProperty guidProperty = useronlyProfile[UserGUIDName];
                    string userGUID = guidProperty.Value.ToString();


                    verifyUserResponse.UserGUID = userGUID;
                    verifyUserResponse.VerifyStatus = "Valid";

                }
                else
                {

                    verifyUserResponse.VerifyStatus = "Invalid UserID";

                }

                return verifyUserResponse;
            }

            catch (Exception exlax)
            {

                verifyUserResponse.VerifyStatus = "Error";
                verifyUserResponse.Message = exlax.Message;

                return verifyUserResponse;



            }


        }



        //public Collection<AllBaskets> GetBasketDataset(Guid xxx)
        //{

        //    //OrderSiteAgent ordersAgent = new OrderSiteAgent(Config.OrdersServiceUrl);

        //    //OrderManagementContext context = OrderManagementContext.Create(ordersAgent);


        //    //Create object of OrderServiceAgent which will be used to create OrderManagementContext object
        //    //string orderWebServiceURL = “http://localhost:100/TS360_ordersWebService/orderswebservice.asmx”;
        //    //OrderServiceAgent ordersAgent = new OrderServiceAgent(orderWebServiceURL);
        //    // or create OrderSiteAgent object. In the following, replace “StarterSite” with the name of your site.
        //    // Faster but code must run in local server.

        //    Collection<AllBaskets> collAllBaskets = new Collection<AllBaskets>();

        //    OrderSiteAgent ordersAgent = new OrderSiteAgent("TS360");

        //    //Create OrderManagementContext object using OrderAgent
        //    OrderManagementContext orderManagementContext = OrderManagementContext.Create(ordersAgent);

        //    //Create the instance of the PurchaseOrderManager used to retrieve and store the PurchaseOrder in Commerce Server.
        //    BasketManager basketmanager = orderManagementContext.BasketManager;

        //    //Create a search clause to find purchase orders whose Product name is Boot
        //    //DataSet searchableProperties = basketmanager.GetSearchableProperties(CultureInfo.CurrentUICulture.ToString());
        //    //SearchClauseFactory searchClauseFactory = basketmanager.GetSearchClauseFactory(searchableProperties, "Basket");

        //    //S/earchClause productClause = searchClauseFactory.CreateClause(ExplicitComparisonOperator.Equal, "Name", "Default");

        //    //Specify the optional parameters to control the search results
        //    //SearchOptions options = new SearchOptions();

        //    //specify the properties you want to return by the DataSet
        //    //options.PropertiesToReturn = "LastModified, Created, SubTotal";

        //    //Specify the property by which the result will be sorted
        //    //options.SortProperties = "LastModified";

        //    //Specify the maximum number of records to return
        //    //options.NumberOfRecordsToReturn = 100;
        //    //Get the DataSet as result by passing the parameters to the SearchPurchaseOrders() method
        //    DataSet searchResults = basketmanager.GetBasketAsDataSet(xxx);

        //    AllBaskets objAllBasket = new AllBaskets();
        //    //objAllBasket = searchResults;
        //    //return searchResults;

        //    //searchResults.WriteXml; 

            

        //    // Apply the WriteXml method to write an XML document



        //    string strLogFile = "C:\\logfile.xml";
        //    StreamWriter swLog;

        //    if (!File.Exists(strLogFile))
        //    {
        //        swLog = new StreamWriter(strLogFile);
        //    }
        //    else
        //    {
        //        swLog = File.AppendText(strLogFile);
        //    }

        //    searchResults.WriteXml(swLog);

        //    swLog.Close();


            
        //    return collAllBaskets;

        //}



        //took out xelement
        public XElement GetBasketDatasetXML(Guid xxx)
        {


            OrderSiteAgent ordersAgent = new OrderSiteAgent(Config.CSSiteName);

            //Create OrderManagementContext object using OrderAgent
            OrderManagementContext orderManagementContext = OrderManagementContext.Create(ordersAgent);

            //Create the instance of the PurchaseOrderManager used to retrieve and store the PurchaseOrder in Commerce Server.
            BasketManager basketmanager = orderManagementContext.BasketManager;

            DataSet searchResults = basketmanager.GetBasketAsDataSet(xxx);
            XElement container = new XElement("container");
            using (XmlWriter w = container.CreateWriter())
            {
                searchResults.WriteXml(w);
                w.Close();


            }


            return container;







        } 

        

    }
}


