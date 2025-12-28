using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Helpers;
using BT.TS360API.ServiceContracts.Profiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Common.Business
{
    public class CdmsDAOManager
    {
        private static volatile CdmsDAOManager _instance;
        private static readonly object SyncRoot = new Object();

        private CdmsDAOManager()
        {
        }

        public static CdmsDAOManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CdmsDAOManager();
                }

                return _instance;
            }
        }

        //public List<UserProfile> GetAdditionalUsers(string cdmsListId, string keyword)
        public List<UserProfileCdms> GetAdditionalUsers(string cdmsListId, string keyword)
        {
            //var result = new List<UserProfile>();
            var result = new List<UserProfileCdms>();
            
            var userCdmsListDs = CdmsDAO.Instance.GetAdditionalUsers(cdmsListId, keyword);
            if (userCdmsListDs.Tables.Count > 0)
            {
                var userCdmsListDt = userCdmsListDs.Tables[0];
                result = GetUsersFromDataTable(userCdmsListDt);
            }

            return result;
        }

        //private List<UserProfile> GetUsersFromDataTable(DataTable dataTable)
        private List<UserProfileCdms> GetUsersFromDataTable(DataTable dataTable)
        {
            //var result = new List<UserProfile>();
            var result = new List<UserProfileCdms>();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    //var userProfile = new UserProfile();
                    var userProfile = new UserProfileCdms();
                    userProfile.Id = DataAccessHelper.ConvertToString(dataRow["UserID"]);
                    userProfile.CdmsUserLoginName = DataAccessHelper.ConvertToString(dataRow["UserName"]);
                    userProfile.UserName = DataAccessHelper.ConvertToString(dataRow["UserAlias"]);
                    userProfile.OrganizationName = DataAccessHelper.ConvertToString(dataRow["OrganizationName"]);

                    result.Add(userProfile);
                }
            }

            //result = new List<UserProfileCdms>();
            //result.Add(new UserProfileCdms() { Id = "Id1", CdmsUserLoginName = "CdmsUserLoginName1", UserName = "UserName1", OrganizationName = "OrganizationName1" });
            //result.Add(new UserProfileCdms() { Id = "Id2", CdmsUserLoginName = "CdmsUserLoginName2", UserName = "UserName2", OrganizationName = "OrganizationName2" });
            //result.Add(new UserProfileCdms() { Id = "Id3", CdmsUserLoginName = "CdmsUserLoginName3", UserName = "UserName3", OrganizationName = "OrganizationName3" });

            return result;
        }

        public void SendCdmsList(string cdmsListId, string additionalUserIds, string checkedUserIds, bool allIndicator)
        {
            CdmsDAO.Instance.SendCdmsList(cdmsListId, additionalUserIds, checkedUserIds, allIndicator);
        }

        //public List<UserProfile> GetUserList(string cdmsListId, int pageSize, int pageIndex, string sortBy, string sortDirection)
        public List<UserProfileCdms> GetUserList(string cdmsListId, int pageSize, int pageIndex, string sortBy, string sortDirection)
        {
            //var result = new List<UserProfile>();
            var result = new List<UserProfileCdms>();

            var userCdmsListDs = CdmsDAO.Instance.GetUserList(cdmsListId, pageSize, pageIndex, sortBy, sortDirection);
            if (userCdmsListDs.Tables.Count > 0)
            {
                var userCdmsListDt = userCdmsListDs.Tables[0];
                result = GetUsersFromDataTable(userCdmsListDt);
            }
            return result;
        }

    }
}
