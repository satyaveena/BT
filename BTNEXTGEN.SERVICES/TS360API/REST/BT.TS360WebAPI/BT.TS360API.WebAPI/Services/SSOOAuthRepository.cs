using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using BT.TS360API.WebAPI.Models;
using BT.TS360API.WebAPI.Common;
using BT.TS360API.WebAPI.Common.DataAccess;

namespace BT.TS360API.WebAPI.Services
{
    public class SSOOAuthRepository
    {
        public SSOOAuthResponse Authorize(string userToken, string loginPage, string ssoOAuthPage, int SSOOAUTHExpirationInDays)
        {
            SSOOAuthResponse response;

            try
            {
                NextGenProfilesDAO profilesDAO = new NextGenProfilesDAO();
                response = profilesDAO.GetUserDetails(userToken, loginPage, ssoOAuthPage, SSOOAUTHExpirationInDays);
            }
            catch (Exception ex)
            {
                response = new SSOOAuthResponse
                {
                    User = new SSOOAuthUser
                    {
                        UserToken = userToken
                    },
                    SSOUrl = loginPage,
                    ErrorMessage = ex.Message

                };
            }

           return response;
        }

        public SSOUserProfileResponse GetUserProfile(string userName, string userToken)
        {
            SSOUserProfileResponse response;

            try
            {
                NextGenProfilesDAO profilesDAO = new NextGenProfilesDAO();
                response = profilesDAO.GetUserProfile(userName, userToken);
            }
            catch (Exception ex)
            {
                response = new SSOUserProfileResponse
                {
                    User = new SSOUserProfile
                    {
                        UserName = userName
                    },
                    ErrorMessage = ex.Message

                };
            }

            return response;
        }

        public SSOUserInfoResponse GetUserInfo(string userName)
        {
            SSOUserInfoResponse response = new SSOUserInfoResponse();

            
            NextGenProfilesDAO profilesDAO = new NextGenProfilesDAO();
            response = profilesDAO.GetUserInfo(userName);
            

            return response;
        }
    }
}