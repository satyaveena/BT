using BT.TS360API.Authentication.DataAccess;
using BT.TS360API.Authentication.Models;
using System;
using System.Collections.Generic;

namespace BT.TS360API.Services.Services
{
    public class MongoService
    {
        public string GetPremiumServiceCode(string url, out bool domainUrlFound)
        {
            var authConfigDAO = new AuthConfigDAO();
            var result = authConfigDAO.GetMongoPremiumServiceCode(url, out domainUrlFound);
            return result;
        }

        public List<RefererBranding> GetAllSiteBrandings()
        {
            var authConfigDAO = new AuthConfigDAO();
            var result = authConfigDAO.GetAllSiteBrandings();
            return result;
        }
    }
}