using BT.TS360API.ExternalDataSendService.DataAccess;
using BT.TS360API.ExternalDataSendService.Helpers;
using BT.TS360API.ExternalDataSendService.Logging;
using BT.TS360API.ExternalDataSendService.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BT.TS360API.ExternalDataSendService.ExternalSend
{
    public class ExternalApiSendManager
    {
        private List<ExternalApiInfoEx> _externalApiList;
        const string IS_DISABLED = "IsDisabled";
        const string DISABLED_REASON = "DisabledReason";

        public ExternalApiSendManager(List<ExternalApiInfoEx> ExternalApiList)
        {
            _externalApiList = ExternalApiList;
        }

        public void SendOrgInfo(DataTable orgData)
        {
            if (orgData == null || orgData.Rows.Count == 0 || _externalApiList == null)
            {
                // TODO: throw exception
                return;
            }

            var orgRow = orgData.Rows[0];

            foreach (var externalAPI in _externalApiList)
            {
                var premiumFieldName = externalAPI.PremiumServiceCode; // should be BTCATEnabled or SSPAPIEnabled
                if (string.IsNullOrWhiteSpace(externalAPI.ApiUrl) || string.IsNullOrWhiteSpace(premiumFieldName))
                    continue;

                // validate if premium option was set to Org
                if (!orgData.Columns.Contains(premiumFieldName) || BaseDAO.ConvertToBoolean(orgRow[premiumFieldName]) == false)
                    continue;

                var jObject = new JObject();
                string parentProperty;
                string childProperty;
                // get field values to send
                foreach (var fieldName in externalAPI.SendOrgFields)
                {
                    /* fieldName may be in parent-child format like "Contact.ContactName" */
                    var hasParentProperty = HasParentProperty(fieldName, out parentProperty, out childProperty);

                    // get value from data row
                    var colValue = string.Empty;
                    var colName = hasParentProperty ? childProperty : fieldName;
                    if (orgData.Columns.Contains(colName))
                    {
                        colValue = BaseDAO.ConvertToString(orgRow[colName]);
                    }

                    // add json property
                    if (hasParentProperty)
                    {
                        // parentProperty already exists
                        if (jObject.ContainsKey(parentProperty))
                        {
                            // add child to parent
                            ((JObject)jObject[parentProperty]).Add(childProperty, colValue);
                        }
                        else
                        {
                            // child property
                            var childJObject = new JObject();
                            childJObject.Add(childProperty, colValue);

                            // parent property
                            jObject.Add(parentProperty, childJObject);
                        }
                    }
                    else
                    {
                        jObject.Add(fieldName, colValue);
                    }
                }

                // check DisabledReason if any
                CheckAndUpdateForDisabledReason(jObject);

                // add other fields for special cases
                var clientId = externalAPI.ApiAccessToken != null ? externalAPI.ApiAccessToken.ClientId : string.Empty;
                AddOtherFieldsToPostData(clientId, jObject);

                // send Org data to external API
                var response = SendToExternalAPI(externalAPI.ApiUrl, externalAPI.ApiAccessToken, jObject);
            }
        }

        private string SendToExternalAPI(string apiUrl, ApiAccessToken tokenRequest, JObject payload)
        {
            try
            {
                // get oauth access token
                var dictHeader = new Dictionary<string, string>();
                if (tokenRequest != null)
                {
                    if (string.IsNullOrWhiteSpace(tokenRequest.EndpointUrl) &&
                        !string.IsNullOrWhiteSpace(tokenRequest.ClientId) && !string.IsNullOrWhiteSpace(tokenRequest.ClientSecret))
                    {
                        dictHeader.Add(tokenRequest.ClientId, tokenRequest.ClientSecret); // special for BTCAT
                    }
                    else
                    {
                        var accessToken = ApiServiceHelper.GetApiAccessToken(tokenRequest);
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            dictHeader.Add("Authorization", "Bearer " + accessToken);
                        }
                    }
                }

                var response = ApiServiceHelper.UploadString(apiUrl, "POST", payload.ToString(), dictHeader);

                return response;
            }
            catch (Exception ex)
            {
                var errMsg = string.Format("{0}. Url: {1}. Data: {2}.", ex.Message, apiUrl, payload);
                var exception = new Exception(errMsg, ex);
                Logger.WriteLog(exception, "SendToExternalAPI");

                var error = string.Format("Unable to send data to {0}. Error is {1}.", apiUrl, ex.Message);
                EmailHelper.NotifyException(error);
            }

            return string.Empty;
        }

        private bool HasParentProperty(string fieldName, out string parentProperty, out string childProperty)
        {
            /* fieldName may be "Contact.ContactName" */

            bool hasParentProperty = false;
            parentProperty = string.Empty;
            childProperty = string.Empty;

            if (!string.IsNullOrEmpty(fieldName))
            {
                char[] charSeparators = new char[] { '.' };
                var splitedFields = fieldName.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (splitedFields.Length > 1)
                {
                    hasParentProperty = true;
                    parentProperty = splitedFields[0];
                    childProperty = splitedFields[splitedFields.Length - 1];    // support 1 level only
                }
            }

            return hasParentProperty;
        }

        private void CheckAndUpdateForDisabledReason(JObject jObject)
        {
            if (jObject.ContainsKey(IS_DISABLED) && jObject.ContainsKey(DISABLED_REASON))
            {
                var isDisabled = (string)jObject[IS_DISABLED];
                var disabledReasonCode = (string)jObject[DISABLED_REASON];

                if (!string.IsNullOrEmpty(disabledReasonCode))
                {
                    bool flag;
                    if (isDisabled == "1" || bool.TryParse(isDisabled, out flag) && flag == true)
                    {
                        // get DisabledReason Text from SiteTerm
                        var disabledReasonText = NoSqlServiceHelper.GetDisabledReasonText(disabledReasonCode);
                        jObject[DISABLED_REASON] = disabledReasonText;
                    }
                    else
                    {
                        jObject[DISABLED_REASON] = string.Empty;    // reset Disabled Reason if the Org is Enabled
                    }
                }
            }
        }

        private void AddOtherFieldsToPostData(string clientId, JObject jObject)
        {
            if (!string.IsNullOrWhiteSpace(clientId) && jObject != null)
            {
                // special fields for SSP
                if (clientId == "SSP")
                {
                    jObject.Add("Source", "TS360");
                }
            }
        }
    }
}