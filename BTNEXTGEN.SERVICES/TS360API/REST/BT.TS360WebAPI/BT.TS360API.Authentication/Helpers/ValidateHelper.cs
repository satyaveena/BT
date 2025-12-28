using BT.TS360API.Authentication.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.Authentication.Helpers
{
    public class ValidateHelper
    {
        public static bool ValidateAuthClient(string clientId, string clientSecret)
        {
            var authConfigDAO = new AuthConfigDAO();
            var application = authConfigDAO.GetApplicationIdentity(clientId);
            if (application != null)
            {
                if (clientId == application.ApiKey && clientSecret == application.ApiPassphrase)
                {
                    return true;
                }
            }

            return false;
        }
    }
}