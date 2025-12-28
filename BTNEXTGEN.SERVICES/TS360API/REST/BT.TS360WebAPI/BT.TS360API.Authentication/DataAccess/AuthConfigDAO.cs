using BT.TS360API.Authentication.Constants;
using BT.TS360API.Authentication.Helpers;
using BT.TS360API.Authentication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace BT.TS360API.Authentication.DataAccess
{
    /// <summary>
    /// AuthConfigDAO
    /// </summary>
    public class AuthConfigDAO : IAuthConfigDAO
    {
        #region Private Member
        private string mongodbConnStr = ConfigurationManager.ConnectionStrings["MongoDBConnStr"].ToString();
        private const string COMMON_MONGODB = "Common";
        private const string APPLICATION_AUTH_KEYS_COLLECTION = "ApplicationAuthKeys";
        #endregion

        #region Method
        /// <summary>
        /// Gets Application Identity.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public ApplicationAuthKey GetApplicationIdentity(string apiKey)
        {
            ApplicationAuthKey result = null;

            var appConfigs = GetAuthConfigs();
            if (appConfigs != null)
            {
                result = appConfigs.FirstOrDefault(xx => xx.ApiKey == apiKey);
            }

            return result;
        }

        /// <summary>
        /// Gets list of application keys from MongoDB.
        /// </summary>
        /// <returns></returns>
        public List<ApplicationAuthKey> GetAuthConfigs()
        {
            // read from cache
            var results = ServerCache.Instance.Read(CacheKeys.MONGO_APPLICATION_KEYS) as List<ApplicationAuthKey>;
            if (results == null)
            {
                results = new List<ApplicationAuthKey>();
//#if DEBUG
//                results.Add(new ApplicationAuthKey { ApiKey = "CDMS", ApiPassphrase = "xVNz8J2aOvLJQpjSoNl6XV5l9NpJhPcZJ3gmS9PvqH0=" });
//                results.Add(new ApplicationAuthKey { ApiKey = "TS360API", ApiPassphrase = "Qo17FyMc98Zit4N0SeCg62Xdb3W5Tps8D9Bkx5APm46" });
//                results.Add(new ApplicationAuthKey { ApiKey = "SSP", ApiPassphrase = "kgfhldkfKDkhdfkhglskhglXCjhihrs", DomainURL = "sspsit1.baker-taylor.com", PremiumServiceCode = "SSPAPIEnabled" });
//                results.Add(new ApplicationAuthKey { ApiKey = "BTCAT", ApiPassphrase = "kgfhldkfKDkhdfkhglskhglXCjhihrd", DomainURL = "btcatdev2.azurewebsites.net", PremiumServiceCode = "BTCATEnabled" });
//#else
                var Client = new MongoClient(mongodbConnStr);
                var DB = Client.GetDatabase(COMMON_MONGODB);
                var collection = DB.GetCollection<BsonDocument>(APPLICATION_AUTH_KEYS_COLLECTION);
                FilterDefinition<BsonDocument> filter = new BsonDocument();
                List<BsonDocument> portalConfigsDoc = collection.Find(filter).ToList();
                if (portalConfigsDoc != null)
                {
                    foreach (BsonDocument document in portalConfigsDoc)
                    {
                        var portalConfig = new ApplicationAuthKey()
                        {
                            ApiKey = document["ApiKey"].ToString(),
                            ApiPassphrase = document["ApiPassphrase"].ToString(),
                            ApiDescription = document["ApiDescription"].ToString(),
                        };

                        if (document.Contains(FieldNames.DOMAIN_URL))
                            portalConfig.DomainURL = document[FieldNames.DOMAIN_URL].ToString();

                        if (document.Contains("PremiumServiceCode"))
                        portalConfig.PremiumServiceCode = document["PremiumServiceCode"].ToString();

                        results.Add(portalConfig);
                    }
                }
//#endif
                // write results to cache
                ServerCache.Instance.Write(CacheKeys.MONGO_APPLICATION_KEYS, results);
            }

            return results;
        }

        /// <summary>
        /// Gets premium service code from MongoDB.
        /// </summary>
        /// <returns></returns>
        public string GetMongoPremiumServiceCode(string url, out bool domainUrlFound)
        {
            domainUrlFound = false;
            var premiumFieldName = string.Empty;

            var appConfigs = GetAuthConfigs();
            if (appConfigs != null)
            {
                var result = appConfigs.FirstOrDefault(xx => xx.DomainURL == url);
                if (result != null)
                {
                    premiumFieldName = result.PremiumServiceCode;
                    domainUrlFound = true;
                }
            }

            return premiumFieldName;
        }

        public List<RefererBranding> GetAllSiteBrandings()
        {
            var results = new List<RefererBranding>();

//#if DEBUG
//            results.Add(new RefererBranding
//                        {
//                            Id = "605348b38406c373922a080d",
//                            DomainURL = "sspsit1.baker-taylor.com",
//                            ApplicationName = "Sustainable Shelves Program",
//                            CssInlineStyles = ".btn-primary,.btn{background-color:#279549;border-color:#279549;} .btn-primary:hover,.btn:hover{background-color:#067951;border-color:#067951;} a{color:#279549;} a:hover{color:#067951;}",
//                            RefererHeaderImageUrl = "/_layouts/1033/IMAGES/ssp-logo-text_md.png",
//                            RefererFooterHTML = "<a href='http://www.baker-taylor.com/privacy.cfm' target='_blank'>Privacy Policy</a>",
//                        });

//            results.Add(new RefererBranding
//                        {
//                            Id = "6053489f8406c373922a080c",
//                            DomainURL = "btcatdev2.azurewebsites.net",
//                            ApplicationName = "BTCAT",
//                            CssInlineStyles = ".logoWrap img{width:75%;} .btn-primary,.btn{background-color:#1088c0;border-color:#1088c0;} .btn-primary:hover,.btn:hover{background-color:#0069d9;border-color:#0069d9;} a{color:#1088c0;} a:hover{color:#0056b3;}",
//                            RefererHeaderImageUrl = "https://btcatdev2.azurewebsites.net/assets/images/BTCAT_logo.png",
//                            RefererFooterHTML = "<a href='http://www.baker-taylor.com/privacy.cfm' target='_blank'>Privacy Policy</a>",
//                        });

//            return results;
//#endif

            var client = new MongoClient(mongodbConnStr);
            var db = client.GetDatabase(COMMON_MONGODB);
            var collection = db.GetCollection<BsonDocument>(APPLICATION_AUTH_KEYS_COLLECTION);

            var filterBuilder = Builders<BsonDocument>.Filter;

            // required fields
            var filter = filterBuilder.Exists(FieldNames.DOMAIN_URL)
                            & filterBuilder.Exists(FieldNames.APPLICATION_NAME)
                            & filterBuilder.Exists(FieldNames.CSS_INLINE_STYLES)
                            & filterBuilder.Exists(FieldNames.REFERER_HEADER_IMAGE_URL);

            List<BsonDocument> portalConfigsDoc = collection.Find(filter).ToList();

            if (portalConfigsDoc != null)
            {
                foreach (BsonDocument document in portalConfigsDoc)
                {
                    var refererBranding = new RefererBranding
                    {
                        Id = document[FieldNames._ID].AsObjectId.ToString(),
                        DomainURL = document[FieldNames.DOMAIN_URL].AsString,
                        ApplicationName = document[FieldNames.APPLICATION_NAME].AsString,
                        CssInlineStyles = document[FieldNames.CSS_INLINE_STYLES].AsString,
                        RefererHeaderImageUrl = document[FieldNames.REFERER_HEADER_IMAGE_URL].AsString
                    };

                    if (document.Contains(FieldNames.REFERER_FOOTER_HTML))
                        refererBranding.RefererFooterHTML = document[FieldNames.REFERER_FOOTER_HTML].AsString;

                    if (document.Contains(FieldNames.REFERER_LOGIN_HEADER_TEXT))
                        refererBranding.RefererLoginHeaderText = document[FieldNames.REFERER_LOGIN_HEADER_TEXT].AsString;

                    if (document.Contains(FieldNames.FORGOT_PWD_HEADER_TEXT))
                        refererBranding.ForgotPasswordHeaderText = document[FieldNames.FORGOT_PWD_HEADER_TEXT].AsString;

                    if (document.Contains(FieldNames.FORGOT_PWD_MAIN_BODY_TEXT))
                        refererBranding.ForgotPasswordMainBodyText = document[FieldNames.FORGOT_PWD_MAIN_BODY_TEXT].AsString;

                    if (document.Contains(FieldNames.FORGOT_PWD_SUB_BODY_TEXT))
                        refererBranding.ForgotPasswordSubBodyText = document[FieldNames.FORGOT_PWD_SUB_BODY_TEXT].AsString;

                    if (document.Contains(FieldNames.FORGOT_LOGINID_HEADER_TEXT))
                        refererBranding.ForgotLoginIDHeaderText = document[FieldNames.FORGOT_LOGINID_HEADER_TEXT].AsString;

                    if (document.Contains(FieldNames.FORGOT_LOGINID_MAIN_BODY_TEXT))
                        refererBranding.ForgotLoginIDMainBodyText = document[FieldNames.FORGOT_LOGINID_MAIN_BODY_TEXT].AsString;

                    if (document.Contains(FieldNames.FORGOT_LOGINID_SUB_BODY_TEXT))
                        refererBranding.ForgotLoginIDSubBodyText = document[FieldNames.FORGOT_LOGINID_SUB_BODY_TEXT].AsString;

                    results.Add(refererBranding);
                }
            }

            return results;
        }
        #endregion
    }
}
 