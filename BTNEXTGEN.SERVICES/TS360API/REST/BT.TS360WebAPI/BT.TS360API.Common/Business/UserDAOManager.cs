using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360Constants;
using System;
using System.Web;

namespace BT.TS360API.Common.Business
{
    public class UserDAOManager
    {
        #region Singleton
        private static readonly object SyncRoot = new Object();

        private UserDAOManager()
        { // prevent init object outside
        }

        public static UserDAOManager Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    UserDAOManager _instance = HttpContext.Current == null ? new UserDAOManager() : HttpContext.Current.Items["UserDAOManager"] as UserDAOManager;
                    if (_instance == null)
                    {
                        _instance = new UserDAOManager();
                        HttpContext.Current.Items.Add("UserDAOManager", _instance);
                    }

                    return _instance;
                }
            }
        }
        #endregion

       public void InsertILSUserAlerts(UserAlertTemplateID msgTemplateId, string cartId, string userId)
        {

            var cart = new CartDAOManager().GetCartById(cartId, userId);
            string cartDetailPageUrl = string.Format(CommonHelper.GetCartManagerURL(), cartId);
            string alertMsgTemp, configReferenceValue;
            BTAlertDAOManager.Instance.GetUserAlertMessageTemplate((int)msgTemplateId, out alertMsgTemp, out configReferenceValue);
            try
            {
                alertMsgTemp = alertMsgTemp.Replace("@cartname", cart.CartName);
                alertMsgTemp = alertMsgTemp.Replace("@URL", cartDetailPageUrl);

                BTAlertDAOManager.Instance.InsertUserAlerts(alertMsgTemp, userId, (int)msgTemplateId, "ILSAlert");
            }
            catch (Exception ex)
            {
                Logging.Logger.WriteLog(ex, "ILS Alert");
            }
        }
    }
}
