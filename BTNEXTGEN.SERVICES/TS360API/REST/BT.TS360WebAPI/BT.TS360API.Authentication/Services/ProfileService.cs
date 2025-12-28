using BT.TS360API.Authentication.DataAccess;
using BT.TS360API.Authentication.ExternalServices;
using BT.TS360API.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Authentication.Services
{
    public class ProfileService
    {
        public UserProfile GetTS360UserProfile(string accessToken)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                // get user id from Auth Log using accesstoken
                var userId = AuthDAOManager.GetUserIdByAccessToken(accessToken).Result;

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    // get user profile from Commerce Service
                    //var userProfile = CSProfileService.Instance.GetUserProfileById(userId);
                    var userProfile = ProfileDAO.Instance.GetUserProfileById(userId);

                    return userProfile;
                }
            }

            return null;
        }
    }
}