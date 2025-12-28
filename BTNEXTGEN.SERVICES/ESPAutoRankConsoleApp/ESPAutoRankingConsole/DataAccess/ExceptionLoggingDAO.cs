using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using ESPAutoRankingConsole.Common;
using ESPAutoRankingConsole.Common.Helpers;

namespace ESPAutoRankingConsole.DataAccess
{
    public class ExceptionLoggingDAO
    {
        private SqlConnection _sqlConn;

        public ExceptionLoggingDAO()
        {
            _sqlConn = new SqlConnection(AppSettings.ExceptionLoggingConnectionString);
        }

        public void LogRequest(string webMethod, string requestMessage, string responseMessage, string vendorAPIKey, string exceptionMessage) 
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@webMethod", webMethod));
            alParams.Add(new SqlParameter("@requestMessage", requestMessage));
            alParams.Add(new SqlParameter("@responseMessage", responseMessage));
            alParams.Add(new SqlParameter("@vendorAPIKey", vendorAPIKey));
            alParams.Add(new SqlParameter("@createdOn", DateTime.Now));
            alParams.Add(new SqlParameter("@createdBy", "TS360WebAPI"));
            alParams.Add(new SqlParameter("@exceptionMessage", exceptionMessage));

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));

            DatabaseHelper.ExecuteNonQuery(StoredProcedure.UPDATE_LOG_REQUEST, arr, _sqlConn);
        }
    }
}