using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Linq;
using System.Web;

using BT.TS360API.WebAPI.Common.Constants;
using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.Helper;
using BT.TS360API.WebAPI.Models;

namespace BT.TS360API.WebAPI.Common.DataAccess
{
    public class NextGenProfilesDAO
    {
        private SqlConnection _sqlConn;

        public NextGenProfilesDAO()
        {
            _sqlConn = new SqlConnection(AppSetting.NextGenProfilesDatabaseConnectionString);
        }

        public SSOOAuthResponse GetUserDetails(string userToken, string loginPage, string ssoOAuthPage, int SSOOAUTHExpirationInDays)
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@CIPUserToken", userToken));

            /*SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);*/

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            //string sqlError = "";

            DataSet result = DatabaseHelper.ExecuteProcedure(StoredProcedure.GET_USER_DETAILS_BY_TOKEN, arr, _sqlConn);

            /*if (!string.IsNullOrEmpty(sqlError))
                throw new Exception(sqlError);*/

            if (result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                throw new Exception(StoredProcedure.GET_USER_DETAILS_BY_TOKEN + ": Invalid User");

            SSOOAuthUser user = new SSOOAuthUser
            {
                UserToken = userToken,
                UserName = result.Tables[0].Rows[0]["UserName"].ToString(),
                UserAlias = result.Tables[0].Rows[0]["UserAlias"].ToString(),
                EmailAddress = result.Tables[0].Rows[0]["EmailAddress"].ToString(),
                OrganizationName = result.Tables[0].Rows[0]["OrganizationName"].ToString(),
                MarketType = result.Tables[0].Rows[0]["MarketType"].ToString(),
                BillToAccountID = result.Tables[0].Rows[0]["BillToFirst6"].ToString(),
                CIPEnabled = result.Tables[0].Rows[0]["CIPEnabled"].ToString(),
                CIPLastLogin = (result.Tables[0].Rows[0]["CIPLastLoginDateTime"] == null) ? DateTime.MinValue : Convert.ToDateTime(result.Tables[0].Rows[0]["CIPLastLoginDateTime"])
            };

            bool isActiveUser = (user.CIPLastLogin.AddDays(SSOOAUTHExpirationInDays) >= DateTime.Now); 
            string ssoUrl = isActiveUser ? ssoOAuthPage : loginPage;

            SSOOAuthResponse response = new SSOOAuthResponse { User = user, SSOUrl = ssoUrl };

            return response;

        }

        public SSOUserProfileResponse GetUserProfile(string userName, string userToken)
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@UserName", userName));
            alParams.Add(new SqlParameter("@CIPUserToken", userToken));

            /*SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);*/

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            //string sqlError = "";

            DataSet result = DatabaseHelper.ExecuteProcedure(StoredProcedure.GET_USER_PROFILE_BY_NAME, arr, _sqlConn);

            /*if (!string.IsNullOrEmpty(sqlError))
                throw new Exception(sqlError);*/

            if (result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                throw new Exception(StoredProcedure.GET_USER_PROFILE_BY_NAME + ": Invalid User");

            SSOUserProfile user = new SSOUserProfile
            {
                UserName = result.Tables[0].Rows[0]["UserName"].ToString(),
                UserAlias = result.Tables[0].Rows[0]["UserAlias"].ToString(),
                EmailAddress = result.Tables[0].Rows[0]["EmailAddress"].ToString(),
                OrganizationName = result.Tables[0].Rows[0]["OrganizationName"].ToString(),
                MarketType = result.Tables[0].Rows[0]["MarketType"].ToString(),
                BillToAccountID = result.Tables[0].Rows[0]["BillToFirst6"].ToString(),
                CIPEnabled = result.Tables[0].Rows[0]["CIPEnabled"].ToString(),
                CIPLastLogin = (result.Tables[0].Rows[0]["CIPLastLoginDateTime"] == null) ? DateTime.MinValue : Convert.ToDateTime(result.Tables[0].Rows[0]["CIPLastLoginDateTime"])
            };

            SSOUserProfileResponse response = new SSOUserProfileResponse { User = user};

            return response;
        }

        public SSOUserInfoResponse GetUserInfo(string userName)
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@UserName", userName));

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            //string sqlError = "";

            DataSet result = DatabaseHelper.ExecuteProcedure(StoredProcedure.GET_USER_PROFILE_BY_NAME, arr, _sqlConn);

            /*if (!string.IsNullOrEmpty(sqlError))
                throw new Exception(sqlError);*/

            if (result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                throw new Exception(StoredProcedure.GET_USER_PROFILE_BY_NAME + ": Invalid User");

            SSOUserInfoResponse response = new SSOUserInfoResponse
            {
                sub = result.Tables[0].Rows[0]["UserId"].ToString(),
                user_id = result.Tables[0].Rows[0]["UserName"].ToString(),
                name = result.Tables[0].Rows[0]["UserAlias"].ToString(),
                email = result.Tables[0].Rows[0]["EmailAddress"].ToString(),
                organization_id = result.Tables[0].Rows[0]["OrganizationName"].ToString(),
                email_verified = true
                //id_token = result.Tables[0].Rows[0]["CIPUserToken"] != null? result.Tables[0].Rows[0]["CIPUserToken"].ToString() : "" 
            };
            response.id_token = result.Tables[0].Rows[0]["CIPUserToken"] != null ? result.Tables[0].Rows[0]["CIPUserToken"].ToString() : "";

            return response;
        }
    }
}