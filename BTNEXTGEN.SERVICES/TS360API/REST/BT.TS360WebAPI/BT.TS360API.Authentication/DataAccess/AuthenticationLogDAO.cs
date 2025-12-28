using BT.TS360API.Authentication.Constants;
using BT.TS360API.Authentication.Helpers;
using BT.TS360API.Authentication.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Authentication.DataAccess
{
    public class AuthenticationLogDAO : BaseMongoDAO
    {
        private static AuthenticationLogDAO _instance = null;
        private static readonly object SyncRoot = new Object();
        readonly IMongoDatabase _mongoDb;
        readonly IMongoCollection<MongoAuthenticationLog> _authenticationLogs;

        #region Singleton

        public static AuthenticationLogDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new AuthenticationLogDAO();
                }

                return _instance;
            }
        }

        #endregion

        public override string ConnectionString
        {
            get { return AppSettings.MongoDBConnectionString; }
        }

        public override string DatabaseName
        {
            get { return "Common"; }
        }

        public AuthenticationLogDAO()
        {
            var client = new MongoClient(ConnectionString);
            _mongoDb = client.GetDatabase(DatabaseName);
            _authenticationLogs = _mongoDb.GetCollection<MongoAuthenticationLog>("SSOAuthenticationLog");
        }

        public async Task AddAuthCodeInfo(string userId, string clientId, string authCode, DateTimeOffset expiration)
        {
            var item = new MongoAuthenticationLog
            {
                Id = ObjectId.GenerateNewId(),
                UserID = userId,
                AppID = clientId,
                AuthCode = authCode,
                AuthCodeExpirationDate = expiration.UtcDateTime
            };

            int retries = base.maxRetries;

            while (retries > 0)
            {
                try
                {
                    await _authenticationLogs.InsertOneAsync(item);
                    break;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(base.retryWaitTime);
                    if (retries < 1)
                    {
                        throw;
                    }
                }
            }
        }

        public async Task UpdateAccessTokenByAuthCode(string authCode, string newAccessToken, DateTimeOffset expiration)
        {
            int retries = base.maxRetries;

            while (retries > 0)
            {
                try
                {
                    var filter = Builders<MongoAuthenticationLog>.Filter.Eq(FieldNames.AUTH_CODE, authCode);
                    var update = Builders<MongoAuthenticationLog>.Update.Set(s => s.AccessToken, newAccessToken)
                                                                        .Set(s => s.AccessTokenExpirationDate, expiration.UtcDateTime);
                    await _authenticationLogs.UpdateOneAsync(filter, update);

                    break;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(base.retryWaitTime);
                    if (retries < 1)
                    {
                        throw;
                    }
                }
            }
        }

        public async Task UpdateRefreshTokenByAuthCode(string authCode, string refreshToken)
        {
            int retries = base.maxRetries;

            while (retries > 0)
            {
                try
                {
                    var filter = Builders<MongoAuthenticationLog>.Filter.Eq(FieldNames.AUTH_CODE, authCode);
                    var update = Builders<MongoAuthenticationLog>.Update.Set(s => s.RefreshToken, refreshToken);

                    await _authenticationLogs.UpdateOneAsync(filter, update);

                    break;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(base.retryWaitTime);
                    if (retries < 1)
                    {
                        throw;
                    }
                }
            }
        }

        public async Task<MongoAuthenticationLog> GetAuthLogItemByAccessToken(string accessToken)
        {
            int retries = base.maxRetries;

            while (retries > 0)
            {
                try
                {
                    // filter
                    var filter = Builders<MongoAuthenticationLog>.Filter;
                    var query = filter.And(filter.Eq(FieldNames.ACCESS_TOKEN, accessToken), filter.Exists(FieldNames.AUTH_CODE));

                    var item =  _authenticationLogs.Find(query).Limit(1).FirstOrDefault();
                    return item;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(base.retryWaitTime);
                    if (retries < 1)
                    {
                        throw;
                    }
                }
            }

            return null;
        }
    }
}