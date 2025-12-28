using BT.Auth.Business.DataAccess.Interface;
using BT.Auth.Business.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Configuration;

namespace BT.Auth.Business.DataAccess
{
    /// <summary>
    /// Class AuthConfigDAO
    /// </summary>
    public class AuthConfigDAO:IAuthConfigDAO 
    {
        #region Private Member
        private string mongodbConnStr = ConfigurationManager.ConnectionStrings["MongoDBConnStr"].ToString();
        #endregion

        #region Method

        /// <summary>
        /// GetAuthConfigs
        /// </summary>
        /// <returns>List<AuthConfig></returns>
        public List<ApplicationKeys> GetAuthConfigs()
        {
            List<ApplicationKeys> portalConfigs = new List<ApplicationKeys>();
            var Client = new MongoClient(mongodbConnStr);
            var DB = Client.GetDatabase("AdminPortal");
            var collection = DB.GetCollection<BsonDocument>("ApplicationKeys");
            FilterDefinition<BsonDocument> filter = new BsonDocument();
            List<BsonDocument> portalConfigsDoc = collection.Find(filter).ToList();
            if(portalConfigsDoc != null)
            {
                foreach (BsonDocument document in portalConfigsDoc)
                {
                    ApplicationKeys portalConfig = new ApplicationKeys();
                    portalConfig.ApiKey = document["ApiKey"].ToString();
                    portalConfig.ApiPassphrase = document["ApiPassphrase"].ToString();
                    portalConfig.ApiDescription = document["ApiDescription"].ToString();
                    portalConfigs.Add(portalConfig);

                }
            }
            return portalConfigs;
        }
        #endregion
    }
}
 