using BT.CDMS.Business.DataAccess.Interface;
using BT.CDMS.Business.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Configuration;

namespace BT.CDMS.Business.DataAccess
{
    /// <summary>
    /// Class CDMSConfigDAO
    /// </summary>
    public class CDMSConfigDAO:ICDMSConfigDAO 
    {
        #region Private Member
        private string mongodbConnStr = ConfigurationManager.ConnectionStrings["MongoDBConnStr"].ToString();
        #endregion

        #region Method

        /// <summary>
        /// GetCDMSConfigs
        /// </summary>
        /// <returns>List<CDMSConfig></returns>
        public List<CDMSConfig> GetCDMSConfigs()
        {
            List<CDMSConfig> portalConfigs = new List<CDMSConfig>();
            var Client = new MongoClient(mongodbConnStr);
            var DB = Client.GetDatabase("AdminCDMS");
            var collection = DB.GetCollection<BsonDocument>("CDMSConfiguration");
            FilterDefinition<BsonDocument> filter = new BsonDocument();
            List<BsonDocument> portalConfigsDoc = collection.Find(filter).ToList();
            if(portalConfigsDoc != null)
            {
                foreach (BsonDocument document in portalConfigsDoc)
                {
                    CDMSConfig portalConfig = new CDMSConfig();
                    portalConfig.Key = document["Key"].ToString();
                    portalConfig.Value = document["Value"].ToString();
                    portalConfig.Desc = document["Desc"].ToString();
                    portalConfigs.Add(portalConfig);

                }
            }
            return portalConfigs;
        }
        #endregion
    }
}
 