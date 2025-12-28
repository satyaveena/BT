using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Authentication.DataAccess
{
    public class AuthDAOManager
    {
        public static async Task AddAuthCodeInfo(AuthenticationTokenCreateContext context)
        {
            if (context == null)
                throw new ArgumentNullException("AuthenticationTokenCreateContext is null");

            if (!string.IsNullOrEmpty(context.Token))
            {
                var userId = context.Request.Query["user_id"];
                var clientId = context.Request.Query["client_id"];
                var authCode  = context.Token;
                var expiration = context.Ticket.Properties.ExpiresUtc.Value;

                // save to MongoDB
                await AuthenticationLogDAO.Instance.AddAuthCodeInfo(userId, clientId, authCode, expiration);
            }
        }

        public static async Task UpdateAccessTokenByAuthCode(string authCode, AuthenticationTokenCreateContext context)
        {
            if (string.IsNullOrEmpty(authCode) || context == null)
                throw new ArgumentNullException("authCode and new accessToken are required");

            var newAccessToken = context.Token;
            var expiration = context.Ticket.Properties.ExpiresUtc.Value;

            // save to MongoDB
            await AuthenticationLogDAO.Instance.UpdateAccessTokenByAuthCode(authCode, newAccessToken, expiration);
        }

        public static async Task UpdateRefreshTokenByAuthCode(string authCode, AuthenticationTokenCreateContext context)
        {
            if (string.IsNullOrEmpty(authCode) || context == null)
                throw new ArgumentNullException("authCode and new accessToken are required");

            var refreshToken = context.Token;
            var expiration = context.Ticket.Properties.ExpiresUtc.Value;

            // save to MongoDB
            await AuthenticationLogDAO.Instance.UpdateRefreshTokenByAuthCode(authCode, refreshToken);
        }

        public static async Task<string> GetUserIdByAccessToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException("accessToken is required");

            var userId = string.Empty;

            var item = await AuthenticationLogDAO.Instance.GetAuthLogItemByAccessToken(accessToken);
            if (item != null)
                userId = item.UserID;

            return userId;
        }
    }
}