using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.Business.DAO;
using BT.ILSQueue.Business.Models;
using BT.ILSQueue.Business.Helpers;
using BT.ILSQueue.Business.MongDBLogger.ELMAHLogger;

namespace BT.ILSQueue.Business.Manager
{
    public class BTAlertManager
    {
        #region Private Member

        private static volatile BTAlertManager _instance;
        private static readonly object SyncRoot = new Object();
        public static BTAlertManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new BTAlertManager();
                }

                return _instance;
            }
        }

        #endregion

        public void InsertILSUserAlerts(UserAlertTemplateID msgTemplateId, string cartId, string userId, string cartName)
        {
            string alertMsgTemp, configReferenceValue;
            string cartDetailPageUrl = string.Format(CommonHelper.GetCartManagerURL(), cartId);

            BTAlertDAO.Instance.GetUserAlertMessageTemplate((int)msgTemplateId, out alertMsgTemp, out configReferenceValue);
            try
            {
                alertMsgTemp = alertMsgTemp.Replace("@cartname", cartName);
                alertMsgTemp = alertMsgTemp.Replace("@URL", cartDetailPageUrl);

                BTAlertDAO.Instance.InsertUserAlerts(alertMsgTemp, userId, (int)msgTemplateId, "ILSAlert");
            }
            catch (Exception ex)
            {
                ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));
            }
        }
    }
}
