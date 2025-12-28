using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using BT.TS360API.Authentication.ExternalServices.CSProfileServiceReference;
using BT.TS360API.Authentication.Constants;
using BT.TS360API.Authentication.Models;

namespace BT.TS360API.Authentication.ExternalServices
{
    public class CSProfileService
    {
        private static ProfilesWebServiceSoapClient _serviceClient;

        private static volatile CSProfileService _instance;
        private static readonly object SyncRoot = new Object();

        private CSProfileService()
        {
            
        }

        public static CSProfileService Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _serviceClient = new ProfilesWebServiceSoapClient();
                        _instance = new CSProfileService();
                    }
                        
                }

                return _instance;
            }
        }

        public UserProfile GetUserProfileById(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;

            var userObjectXml = _serviceClient.GetProfile(userId, ProfileEntity.UserObject.ToString());

            if (userObjectXml == null) return null;

            var userProfile = new UserProfile();
            userProfile.UserId = userId;

            GetValuesForUserProfile(userObjectXml, userProfile);

            userProfile.OrgName = GetOrgNameById(userProfile.OrgId);

            return userProfile;
        }

        public string GetOrgNameById(string orgId)
        {
            var orgName = string.Empty;

            if (!string.IsNullOrEmpty(orgId))
            {
                var orgObjectXml = _serviceClient.GetProfile(orgId, ProfileEntity.Organization.ToString());
                if (orgObjectXml != null)
                {
                    var xml = orgObjectXml.SelectNodes("Organization/GeneralInfo");
                    foreach (XmlNode node in xml)
                    {
                        if (node.Attributes == null) continue;

                        orgName = GetSingleNodeText(node, "name");
                    }
                }
            }

            return orgName;
        }

        private void GetValuesForUserProfile(XmlElement userObjectXml, UserProfile userProfile)
        {
            // get values in <AccountInfo> element
            var xmlAccountInfo = userObjectXml.SelectNodes("/UserObject/AccountInfo");
            if (xmlAccountInfo != null)
            {
                foreach (XmlNode node in xmlAccountInfo)
                {
                    if (node.Attributes == null) continue;

                    userProfile.OrgId = GetSingleNodeText(node, "org_id");
                }
            }

            // get values in <BTNextGen> element
            var xmlBtNextGen = userObjectXml.SelectNodes("/UserObject/BTNextGen");
            if (xmlBtNextGen != null)
            {
                foreach (XmlNode node in xmlBtNextGen)
                {
                    if (node.Attributes == null) continue;

                    userProfile.UserName = GetSingleNodeText(node, "user_name");
                    userProfile.UserAlias = GetSingleNodeText(node, "user_alias");
                }
            }

            // get values in <xmlAccountInfo> element
            var xmlGeneralInfo = userObjectXml.SelectNodes("/UserObject/GeneralInfo");
            if (xmlGeneralInfo != null)
            {
                foreach (XmlNode node in xmlGeneralInfo)
                {
                    if (node.Attributes == null) continue;

                    userProfile.UserEmail = GetSingleNodeText(node, "email_address");
                }
            }
        }

        private string GetSingleNodeText(XmlNode parentNode, string nodeName)
        {
            var node = parentNode.SelectNodes(nodeName);
            if (node == null || node.Count == 0) return "";

            return node[0].InnerText;
        }
    }
}
