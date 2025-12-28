using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.ILSQueue.Business.Models;
using BT.ILSQueue.Business.Helpers;
using BT.ILSQueue.Business.Constants;

namespace BT.ILSQueue.Business.DAO
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
            get
            {
                return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.Profiles_ConnectionString);
            }
        #endregion

        }

        public void GetUserAlertMessageTemplate(int alertMessageTemplateID, out string alertMessageTemplate, out string configReferenceValue)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage(StoredProcedureName.PROC_GET_ALERT_MESSAGE_TEMPLATE, dbConnection);
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
            var command = CreateSqlSpCommandNoErrorMessage(StoredProcedureName.PROC_INSERT_ALERT_USER_MESSAGE, dbConnection);

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
                //Logger.Write("BTAlert", sqlException.Message);
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
