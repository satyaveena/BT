using BT.TS360API.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;

namespace BT.TS360API.WebAPI.Controllers
{
    public class SSOTokenController : ApiController
    {
        protected string CreateJsonWebToken(IdentityTokenRequest token, X509SigningCredentials credentials)
        {
            var unixSecondAuthTime = (token.AuthTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            var authTime = unixSecondAuthTime.ToString("0");

            var unixSecondExp = (token.ExpirationTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            var exp = unixSecondExp.ToString("0");

            var unixSecondIat = (token.ExpirationTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            var iat = unixSecondIat.ToString("0");
            var claims = new List<Claim>(){
                new Claim(SSOClaimTypes.Subject, Guid.NewGuid().ToString()),
                new Claim(SSOClaimTypes.IssuedAt, iat),
                new Claim(SSOClaimTypes.AuthenticationTime, authTime)
            };

            var jwt = new JwtSecurityToken(
                token.Issuer,
                token.Audience,
                claims,
                null,
                token.ExpirationTime,
                credentials);

            //var x509credential = credentials;
            //if (x509credential != null)
            //{
            //    jwt.Header.Add("kid", Base64Url.Encode(x509credential.Certificate.GetCertHash()));
            //}
            //jwt.Header.Remove("x5t");
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(jwt);
        }

        public string Post(IdentityTokenRequest request)
        {
            try
            {
                var credentials = new X509SigningCredentials(Cert.Load(), new SecurityKeyIdentifier(
                    new NamedKeySecurityKeyIdentifierClause(
                        "kid",
                        "68BB0C7B0BB8D72C765C07B8D9B3887D8472C7B1")));
                return CreateJsonWebToken(request, credentials);
            }
            catch
            {
                return "";
            }
        }
    }

    public static class SSOClaimTypes
    {
        // core oidc claims
        public const string Subject = "sub";
        public const string Name = "name";
        public const string GivenName = "given_name";
        public const string FamilyName = "family_name";
        public const string MiddleName = "middle_name";
        public const string NickName = "nickname";
        public const string PreferredUserName = "preferred_username";
        public const string Profile = "profile";
        public const string Picture = "picture";
        public const string WebSite = "website";
        public const string Email = "email";
        public const string EmailVerified = "email_verified";
        public const string Gender = "gender";
        public const string BirthDate = "birthdate";
        public const string ZoneInfo = "zoneinfo";
        public const string Locale = "locale";
        public const string PhoneNumber = "phone_number";
        public const string PhoneNumberVerified = "phone_number_verified";
        public const string Address = "address";
        public const string Audience = "aud";
        public const string Issuer = "iss";
        public const string NotBefore = "nbf";
        public const string Expiration = "exp";

        // more standard claims
        public const string UpdatedAt = "updated_at";
        public const string IssuedAt = "iat";
        public const string AuthenticationMethod = "amr";
        public const string AuthenticationContextClassReference = "acr";
        public const string AuthenticationTime = "auth_time";
        public const string AuthorizedParty = "azp";
        public const string AccessTokenHash = "at_hash";
        public const string AuthorizationCodeHash = "c_hash";
        public const string Nonce = "nonce";
        public const string JwtId = "jti";

        // more claims
        public const string ClientId = "client_id";
        public const string Scope = "scope";
        public const string Id = "id";
        public const string Secret = "secret";
        public const string IdentityProvider = "idp";
        public const string Role = "role";
        public const string ReferenceTokenId = "reference_token_id";

        // claims for authentication controller partial logins
        public const string AuthorizationReturnUrl = "authorization_return_url";
        public const string PartialLoginReturnUrl = "partial_login_return_url";

        // internal claim types
        // claim type to identify external user from external provider
        public const string ExternalProviderUserId = "external_provider_user_id";
        public const string PartialLoginResumeId = "partial_login_resume_id:{0}";
    }
    
    internal class Cert
    {
        public static X509Certificate2 Load()
        {
            var assembly = typeof(Cert).Assembly;
            using (var stream = assembly.GetManifestResourceStream("BT.TS360API.WebAPI.OAuthExtensions.ts360.pfx"))
            {
                return new X509Certificate2(ReadStream(stream), "idsrv3test");
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
    internal class Base64Url
    {
        public static string Encode(byte[] arg)
        {
            string s = Convert.ToBase64String(arg); // Regular base64 encoder
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding
            return s;
        }
        public static byte[] Decode(string arg)
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
