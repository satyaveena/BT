using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Logging;
using BT.TS360Constants;

namespace BT.TS360API.Common.DataAccess
{
    public class BTAlertDAO : BaseDAO
    {
        #region Singleton
        private static volatile BTAlertDAO _instance;
        private static readonly object SyncRoot = new Object();

        private BTAlertDAO()
        { // prevent init object outside
        }

        public static BTAlertDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new BTAlertDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["Profiles_ConnectionString"]; }
        }
        #endregion

        public void GetUserAlertTotalCount(string userId, out int unreadAlerts, out int hasReadAlerts)
        {
            unreadAlerts = 0;
            hasReadAlerts = 0;
            if (string.IsNullOrEmpty(userId)) return;

            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.procGetUserAlertTotalCount, dbConnection);

                var paramUserId = new SqlParameter("@UserId", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = userId };
                var paramUnRead = new SqlParameter("@UnReadCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var paramHasRead = new SqlParameter("@HasReadCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var errorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

                command.Parameters.AddRange(new[] { paramUserId, paramUnRead, paramHasRead, errorMessage });

                dbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();

                    if (!int.TryParse(paramUnRead.Value.ToString(), out unreadAlerts))
                    {
                        unreadAlerts = 0;
                    }

                    if (!int.TryParse(paramHasRead.Value.ToString(), out hasReadAlerts))
                    {
                        hasReadAlerts = 0;
                    }
                }
                finally
                {
                    dbConnection.Close();
                }
            }
        }
        public void GetUserAlertMessageTemplate(int alertMessageTemplateID, out string alertMessageTemplate, out string configReferenceValue)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage(DBStores.procGetAlertMessageTemplate, dbConnection);
            alertMessageTemplate = null;
            configReferenceValue = null;
            //<Parameter>
            var AlertMessageTemplateID = new SqlParameter("@AlertMessageTemplateID", alertMessageTemplateID);
            var AlertMessageTemplate = new SqlParameter("@AlertMessageTemplate", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            var ConfigReferenceValue = new SqlParameter("@ConfigReferenceValue", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            var Return = new SqlParameter("@Return", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var Message = new SqlParameter("@Message", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

            command.Parameters.Add(AlertMessageTemplateID);
            command.Parameters.Add(AlertMessageTemplate);
            command.Parameters.Add(ConfigReferenceValue);
            command.Parameters.Add(Return);
            command.Parameters.Add(Message);
            dbConnection.Open();
            command.ExecuteNonQuery();
            alertMessageTemplate = (command.Parameters["@AlertMessageTemplate"].Value.ToString());
            configReferenceValue = (command.Parameters["@ConfigReferenceValue"].Value.ToString());

            dbConnection.Close();
        }
        public void InsertUserAlerts(string alertMsg, string userID, int msgTemplateId, string sourceSystem)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage(DBStores.procInsertAlertUserMessage, dbConnection);

            //<Parameter>
            var sqlParameters = new SqlParameter[6];
            sqlParameters[0] = new SqlParameter("@AlertMessage", alertMsg);
            sqlParameters[1] = new SqlParameter("@UserID", userID);
            sqlParameters[2] = new SqlParameter("@MessageTemplateID", msgTemplateId);
            sqlParameters[3] = new SqlParameter("@SourceSystem", sourceSystem);
            sqlParameters[4] = new SqlParameter("@Return", SqlDbType.Int) { Direction = ParameterDirection.Output };
            sqlParameters[5] = new SqlParameter("@Message", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

            command.Parameters.AddRange(sqlParameters);
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException sqlException)
            {
                Logger.Write("BTAlert", sqlException.Message);
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }

    }
}
