using BT.TS360API.ExternalDataSendService.Constants;
using BT.TS360API.ExternalDataSendService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using BT.TS360API.ExternalDataSendService.Caching;

namespace BT.TS360API.ExternalDataSendService.DataAccess
{
    /// <summary>
    /// AuthConfigDAO
    /// </summary>
    public class AuthConfigDAO : IAuthConfigDAO
    {
        #region Private Member
        private string mongodbConnStr = ConfigurationManager.ConnectionStrings["MongoDBConnStr"].ToString();
        #endregion

        #region Method
        public async Task<List<ExternalApiInfoEx>> GetExternalApiInfoList()
        {
            var cacheKey = "ApplicationAuthKeys_ExternalApiInfo";
            var cacheInstance = WFECachingController.Instance;

            // read from server cache
            var results = cacheInstance.Read(cacheKey) as List<ExternalApiInfoEx>;
            if (results == null)
            {
                results = new List<ExternalApiInfoEx>();

                var client = new MongoClient(mongodbConnStr);
                var db = client.GetDatabase("Common");
                var collection = db.GetCollection<BsonDocument>("ApplicationAuthKeys");

                var filterBuilder = Builders<BsonDocument>.Filter;
                var filter = filterBuilder.Exists("ExternalApiInfo")
                                & filterBuilder.Exists("ExternalApiInfo.ApiUrl")
                                & filterBuilder.Exists("ExternalApiInfo.SendOrgFields.0");

                List<BsonDocument> portalConfigsDoc = await collection.Find(filter).ToListAsync();

                if (portalConfigsDoc != null)
                {
                    foreach (BsonDocument document in portalConfigsDoc)
                    {
                        string premiumCode = null;
                        if (document.Contains("PremiumServiceCode"))
                            premiumCode = document["PremiumServiceCode"].AsString;

                        var externalApiDoc = document["ExternalApiInfo"].AsBsonDocument;
                        // get external Url and fields to be sent
                        var apiInfo = new ExternalApiInfoEx();
                        apiInfo.PremiumServiceCode = premiumCode;
                        apiInfo.ApiUrl = externalApiDoc["ApiUrl"].AsString;
                        apiInfo.SendOrgFields = externalApiDoc["SendOrgFields"].AsBsonArray.Select(b => b.AsString).ToList();

                        // get access token info
                        if (externalApiDoc.Contains("ApiAccessToken"))
                        {
                            var bsonAccessToken = externalApiDoc["ApiAccessToken"].AsBsonDocument;
                            if (bsonAccessToken.Contains("ClientId") && bsonAccessToken.Contains("ClientSecret") && bsonAccessToken.Contains("EndpointUrl"))
                            {
                                var accessTokenInfo = new ApiAccessToken
                                {
                                    ClientId = bsonAccessToken["ClientId"].AsString,
                                    ClientSecret = bsonAccessToken["ClientSecret"].AsString,
                                    EndpointUrl = bsonAccessToken["EndpointUrl"].AsString
                                };

                                apiInfo.ApiAccessToken = accessTokenInfo;
                            }
                        }

                        results.Add(apiInfo);
                    }

                    // write to cache
                    cacheInstance.Write(cacheKey, results);
                }
            }

            return results;
        }

        
        #endregion
    }
}
 