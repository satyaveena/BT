using BT.TS360API.ExternalDataSendService.DataAccess;
using BT.TS360API.ExternalDataSendService.ExternalSend;
using BT.TS360API.ExternalDataSendService.Helpers;
using BT.TS360API.ExternalDataSendService.Interfaces;
using BT.TS360API.ExternalDataSendService.Logging;
using BT.TS360API.ExternalDataSendService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BT.TS360API.ExternalDataSendService.Services
{
    public class OrganizationSendService : IOrganizationSendService
    {
        public OrganizationSendService()
        {
        }

        public async Task<bool> SendOrganizationAsync(string orgId)
        {
            bool isSuccess = false;
            try
            {
                // get org information from SQL
                var tblOrgInfo = OrganizationDAO.Instance.GetOrganizationById(orgId);

                // get list of external API info from MongoDB
                var authConfigDAO = new AuthConfigDAO();
                var externalAPIs = await authConfigDAO.GetExternalApiInfoList();

                // send org info to external APIs
                var sendManager = new ExternalApiSendManager(externalAPIs);
                sendManager.SendOrgInfo(tblOrgInfo);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                // log error
                Logger.LogException(ex);

                // send email
                var error = string.Format("There is exception when sending Organization information to external APIs. Error is {0}.", ex.Message);
                EmailHelper.NotifyException(ex.Message);

                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
