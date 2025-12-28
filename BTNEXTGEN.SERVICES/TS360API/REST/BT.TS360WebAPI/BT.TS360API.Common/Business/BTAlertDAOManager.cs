using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Common.DataAccess;

namespace BT.TS360API.Common.Business
{
    public class BTAlertDAOManager
    {
        #region Singleton
        private static volatile BTAlertDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        private BTAlertDAOManager()
        { // prevent init object outside
        }

        public static BTAlertDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new BTAlertDAOManager();
                }

                return _instance;
            }
        }
        #endregion

        public void GetUserAlertsCount(string userId, out int unReadAlertsCount, out int hasReadAlertsCount)
        {
            BTAlertDAO.Instance.GetUserAlertTotalCount(userId, out unReadAlertsCount, out hasReadAlertsCount);
        }
        public void GetUserAlertMessageTemplate(int alertMessageTemplateID, out string alertMessageTemplate, out string configReferenceValue)
        {
            BTAlertDAO.Instance.GetUserAlertMessageTemplate(alertMessageTemplateID, out alertMessageTemplate, out configReferenceValue);
        }
        public void InsertUserAlerts(string alertMsg, string userID, int msgTemplateId, string sourceSystem)
        {
            BTAlertDAO.Instance.InsertUserAlerts(alertMsg, userID, msgTemplateId, sourceSystem);
        }
    }
}
