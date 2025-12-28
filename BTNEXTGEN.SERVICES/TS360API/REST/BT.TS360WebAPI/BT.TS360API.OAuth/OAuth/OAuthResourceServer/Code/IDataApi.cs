namespace OAuthResourceServer.Code {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.ServiceModel;
	using System.ServiceModel.Web;
	using System.Text;

	[ServiceContract]
	public interface IDataApi {
		[OperationContract, WebGet(UriTemplate = "/age", ResponseFormat = WebMessageFormat.Json)]
		int? GetAge();

		[OperationContract, WebGet(UriTemplate = "/name", ResponseFormat = WebMessageFormat.Json)]
		string GetName();

		[OperationContract, WebGet(UriTemplate = "/favoritesites", ResponseFormat = WebMessageFormat.Json)]
		string[] GetFavoriteSites();

        [OperationContract, WebGet(UriTemplate = "/userinfo", ResponseFormat = WebMessageFormat.Json)]
        SSOOAuthResponse GetUserInfo();
	}

    
    public class SSOOAuthResponse
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string OrganizationName { get; set; }

        [DataMember]
        public string MarketType { get; set; }

        [DataMember]
        public string UserAlias { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }
    }
}