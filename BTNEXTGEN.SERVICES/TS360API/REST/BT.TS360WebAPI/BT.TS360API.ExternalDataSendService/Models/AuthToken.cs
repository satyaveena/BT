using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace BT.TS360API.ExternalDataSendService.Models
{
    public class AuthToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        public string Token { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Expires In Second.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }

    //public class SSPUserProfileResponse
    //{
    //    public string UserId { get; set; }
    //    public string Token { get; set; }
    //}
}
