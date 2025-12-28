namespace OAuthResourceServer {
	using System.Linq;
	using System.Security.Principal;
	using System.ServiceModel;

	using Code;
    using OAuthResourceServer.TS360OAuthService;

	/// <summary>
	/// The WCF service API.
	/// </summary>
	/// <remarks>
	/// Note how there is no code here that is bound to OAuth or any other
	/// credential/authorization scheme.  That's all part of the channel/binding elsewhere.
	/// And the reference to OperationContext.Current.ServiceSecurityContext.PrimaryIdentity 
	/// is the user being impersonated by the WCF client.
	/// In the OAuth case, it is the user who authorized the OAuth access token that was used
	/// to gain access to the service.
	/// </remarks>
	public class DataApi : IDataApi {
		private IIdentity User {
			get { return OperationContext.Current.ServiceSecurityContext.PrimaryIdentity; }
		}

		public int? GetAge() {
			// We'll just make up an age personalized to the user by counting the length of the username.
			return this.User.Name.Length;
		}

		public string GetName() {
            return this.User.Name;
		}

		public string[] GetFavoriteSites() {
			// Just return a hard-coded list, to avoid having to have a database in a sample.
			return new string[] {
				"http://www.dotnetopenauth.net/",
				"http://www.oauth.net/",
				"http://www.openid.net/",
			};
		}

        public SSOOAuthResponse GetUserInfo()
        {
            var userName = this.User.Name;
            var oAuthServiceClient = new OAuthServiceClient("BasicHttpBinding_OAuthService");
            var oAuthRes = oAuthServiceClient.GetUserInfo(userName);
            if (oAuthRes != null && oAuthRes.Status == "1") //success
            {
                var ssoOAuthResponse = new SSOOAuthResponse()
                {
                    UserName = oAuthRes.UserName,
                    UserAlias = oAuthRes.UserAlias,
                    OrganizationName = oAuthRes.OrganizationName,
                    MarketType = oAuthRes.MarketType,
                    EmailAddress = oAuthRes.EmailAddress,                    
                    Status = "1"
                };
                return ssoOAuthResponse;
            }
            return new SSOOAuthResponse() {Status = "-1"};
        }
    }
}