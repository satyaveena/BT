using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Newtonsoft.Json;
using System.Net;
using System.IO;

using BT.ILSQueue.Business.Models;
using BT.ILSQueue.Business.Helpers;

namespace BT.ILSQueue.Business.Manager
{
    public class AuthHashManager
    {
        #region Private Member

        private static volatile AuthHashManager _instance;
        private static readonly object SyncRoot = new Object();
        public static AuthHashManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new AuthHashManager();
                }

                return _instance;
            }
        }

        #endregion

        public StaffUserResponse Authenicate(PolarisProfile polarisProfile)
        {
           /*
            string papiURL = "https://qa-polaris.polarislibrary.com/PAPIService/rest";
            string papiID = "TS360API";
            string papiAccesskey = "C987543D-8D2E-4C59-8B6F-4D9E01C70B97";
            string domain = "QA-Polaris";
            string account = "VendorAccount";
            string password = "VendorTesting01!"; */

            string papiURL = polarisProfile.papiURL;
            string papiID = polarisProfile.papiID;
            string papiAccesskey = polarisProfile.papiAccesskey;
            string domain = polarisProfile.domain;
            string account = polarisProfile.account;
            string password = polarisProfile.password;

            String dateTimeFormatRFC = DateTime.UtcNow.ToString("R");

            papiURL = CommonHelper.GetPolarisAppRelativePath(papiURL);

            papiURL += "/authenticator/staff";

            // Hashing 
            string signature = GetPAPIHash(papiAccesskey, "POST", papiURL, dateTimeFormatRFC, "");

            StaffUserRequest staffUserRequest = new StaffUserRequest();

            staffUserRequest.Domain = domain;
            staffUserRequest.Username = account;
            staffUserRequest.Password = password;

            string staffUserRequestJson = JsonConvert.SerializeObject(staffUserRequest);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(papiURL);

            // append Header 

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", string.Format("PWS {0}:{1}", papiID, signature));
            httpWebRequest.Headers.Add("PolarisDate", dateTimeFormatRFC);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(staffUserRequestJson);
                streamWriter.Flush();
                streamWriter.Close();

            }

            StaffUserResponse staffUserResponseJson = new StaffUserResponse();

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();

                staffUserResponseJson = JsonConvert.DeserializeObject<StaffUserResponse>(result);
            }

            return staffUserResponseJson;
        }

        public  string GetPAPIHash(string strAccessKey, string strHTTPMethod, string strURI, string strHTTPDate, string strPatronPassword)
        {
            byte[] secretBytes = UTF8Encoding.UTF8.GetBytes(strAccessKey);
            HMACSHA1 hmac = new HMACSHA1(secretBytes);

            // Computed hash is based on different elements defined by URI
            byte[] dataBytes = null;

            if (strPatronPassword.Length > 0)
                dataBytes = UTF8Encoding.UTF8.GetBytes(strHTTPMethod + strURI + strHTTPDate + strPatronPassword);
            else
                dataBytes = UTF8Encoding.UTF8.GetBytes(strHTTPMethod + strURI + strHTTPDate);

            byte[] computedHash = hmac.ComputeHash(dataBytes);
            string computedHashString = Convert.ToBase64String(computedHash);

            return computedHashString;
        }
    }
}
