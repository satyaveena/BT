namespace OAuthClient {
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Net;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Security;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using DotNetOpenAuth.OAuth2;

	using SampleResourceServer;
    using DotNetOpenAuth.Messaging;
    using OAuthClient.Code;
    using DotNetOpenAuth.OAuth;
    using System.Configuration;

	public partial class SampleWebApi : System.Web.UI.Page {
		/// <summary>
		/// The OAuth 2.0 client object to use to obtain authorization and authorize outgoing HTTP requests.
		/// </summary>
        private static readonly UserAgentClient Client;		

        private static string CallbackUrl
        {
            get
            {
                //"http://localhost:59722/SampleWebApi.aspx"
                return ConfigurationManager.AppSettings["CallbackUrl"];
            }
        }

        private static string WebApiUrl
        {
            get
            {
                //"http://localhost:8087/cart/ssouserprofile"
                return ConfigurationManager.AppSettings["WebApiUrl"];
            }
        }

        private static string ClientIdentifider
        {
            get
            {
                //"sampleconsumer"
                return ConfigurationManager.AppSettings["ClientIdentifider"];
            }
        }

        private static string ClientSecret
        {
            get
            {
                //"samplesecret"
                return ConfigurationManager.AppSettings["ClientSecret"];
            }
        }

        private static string Scope
        {
            get
            {
                return ConfigurationManager.AppSettings["Scope"];
            }
        }
        /// <summary>
		/// Initializes static members of the <see cref="SampleWcf2"/> class.
		/// </summary>
        static SampleWebApi()
        {
            var authEP = ConfigurationManager.AppSettings["TS360AuthEndpoint"];
            var tokenEP = ConfigurationManager.AppSettings["TS360TokenEndpoint"];
            var authServerDescription = new AuthorizationServerDescription
            {
                AuthorizationEndpoint = new Uri(authEP),
                TokenEndpoint = new Uri(tokenEP)
            };
            Client = new UserAgentClient(authServerDescription, ClientIdentifider, ClientSecret);
            Authorization = new AuthorizationState();
            Authorization.Callback = new Uri(CallbackUrl);
		}

		/// <summary>
		/// Gets or sets the authorization details for the logged in user.
		/// </summary>
		/// <value>The authorization details.</value>
		/// <remarks>
		/// Because this is a sample, we simply store the authorization information in memory with the user session.
		/// A real web app should store at least the access and refresh tokens in this object in a database associated with the user.
		/// </remarks>
        public static IAuthorizationState Authorization { get; private set; }
		protected void Page_Load(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(Request.QueryString["code"]) && !IsPostBack)
            {
                try
                {
                    Client.ProcessUserAuthorization(Request.Url, Authorization);
                    var valueString = string.Empty;
                    if (!string.IsNullOrEmpty(Authorization.AccessToken))
                    {
                        valueString = CallAPI();
                        authorizationLabel.Text = "Authenication Received. User Info: " + valueString;
                    }
                    
                }
                catch (DotNetOpenAuth.Messaging.ProtocolException ex)
                {
                    this.authorizationLabel.Text = ex.ToString();
                }
            }
		}

		protected void getAuthorizationButton_Click(object sender, EventArgs e) {
            string redirectURL = "";
            if (Session["AccessToken"] == null)
            {
                Authorization.Scope.AddRange(OAuthUtilities.SplitScopes(Scope));
                Uri authorizationUrl = Client.RequestUserAuthorization(Authorization);
                redirectURL = authorizationUrl.AbsoluteUri;
                Response.Redirect(redirectURL);
            }            
		}

		protected void getUserInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.userInfoLabel.Text = CallAPI();
            }
            catch (SecurityAccessDeniedException)
            {
                this.userInfoLabel.Text = "Access denied!";
            }
            catch (MessageSecurityException)
            {
                this.userInfoLabel.Text = "Access denied!";
            }
            catch(Exception ex)
            {
                this.userInfoLabel.Text = ex.ToString();
            }
        }

        private string CallAPI()
        {
            if (!string.IsNullOrEmpty(Authorization.AccessToken))
            {
                var webClient = new WebClient();
                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["X-JavaScript-User-Agent"] = "Demo";
                webClient.Headers[HttpRequestHeader.Authorization] = string.Format(CultureInfo.InvariantCulture, "Bearer {0}", Authorization.AccessToken);
                var valueString = webClient.DownloadString(WebApiUrl);

                return valueString;
            }
            return "Web Api call failed!";
        }
	}
}