using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using BT.TS360API.WebAPI.Common.DataAccess;
using BT.TS360API.WebAPI.Common.Helper;
using BT.TS360API.WebAPI.Models;
using BT.TS360API.WebAPI.Controllers;

namespace BT.TS360API.WebAPI.OAuth
{
    internal class AccessTokenManager
    {
        public static string GenerateIdToken(string currentUrl, string clientId)
        {
            var tokenReq = new IdentityTokenRequest()
                           {
                               Audience = clientId,
                               AuthTime = DateTime.UtcNow,
                               CreationTime = DateTime.UtcNow,
                               ExpirationTime = DateTime.UtcNow.AddDays(1),
                               Issuer = currentUrl
                           };
            var ssoTokenController = new SSOTokenController();
            return ssoTokenController.Post(tokenReq);
        }

        private static void GetAuthTimeAndExpTime(string clientId, string userName, out string exp, out string authTime)
        {
            var authDt = DateTime.UtcNow;
            var expDt = DateTime.UtcNow.AddDays(1);
            var client = GetClientAuthorization(clientId, userName);
            if (client != null)
            {
                authDt = client.CreatedOnUtc;
                expDt = client.ExpirationDateUtc.HasValue ? client.ExpirationDateUtc.Value : DateTime.UtcNow.AddDays(1);
            }
            var unixSecondAuthTime = (authDt - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            authTime = unixSecondAuthTime.ToString("0");

            var unixSecondExp = (expDt - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            exp = unixSecondExp.ToString("0");
        }

        private static ClientAuthorization GetClientAuthorization(string clientIdentifier, string userName)
        {
            if (string.IsNullOrEmpty(clientIdentifier) || string.IsNullOrEmpty(userName)) return null;
            try
            {
                var oAuthDao = new OAuthDAO();
                var ds = oAuthDao.GetClientAuthorization(clientIdentifier, userName);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];
                    var client = new ClientAuthorization();

                    var createdOnUtc = DatabaseHelper.ConvertToDateTime(row["CreatedOnUtc"]);
                    var expirationDateUtc = DatabaseHelper.ConvertToDateTime(row["ExpirationDateUtc"]);

                    client.CreatedOnUtc = createdOnUtc.HasValue ? createdOnUtc.Value : DateTime.UtcNow;
                    client.ExpirationDateUtc = expirationDateUtc;

                    return client;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
    }

    internal class JWTConverter
    {
        // URL Encode the string, according to
        // http://tools.ietf.org/html/draft-ietf-jose-json-web-signature-08#appendix-A.1, page 35
        public string Base64UrlEncode(byte[] arg)
        {
            string s = Convert.ToBase64String(arg); // Regular base64 encoder
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding
            return s;
        }
        public byte[] Base64UrlDecode(string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default: throw new System.Exception(
                "Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder
        }
    }
}
