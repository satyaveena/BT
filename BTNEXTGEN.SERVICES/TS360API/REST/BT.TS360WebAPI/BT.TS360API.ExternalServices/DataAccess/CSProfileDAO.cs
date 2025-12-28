using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360Constants;

namespace BT.TS360API.ExternalServices.DataAccess
{
    public class CSProfileDAO : BaseDAO
    {
        private static volatile CSProfileDAO _instance;
        private static readonly object SyncRoot = new Object();

        private CSProfileDAO()
        { // prevent init object outside
        }

        public static CSProfileDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CSProfileDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["NextGenProfilesConnectionString"]; }
        }

        public DataSet GetAccounts(List<string> accountIds)
        {
            var ds = new DataSet();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand("procTS360GetAccounts", dbConnection);

            var accountIDsTable = GenerateDataRecords(accountIds, "GUID", 50);

            var sqlParameter = new SqlParameter("@AccountIDs", SqlDbType.Structured) { Value = accountIDsTable };
            command.Parameters.Add(sqlParameter);

            var da = new SqlDataAdapter(command);
            dbConnection.Open();
            try
            {
                da.Fill(ds);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
        }

        public DataSet GetUserReviewTypes(List<string> reviewTypeIds)
        {
            var ds = new DataSet();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand("procTS360GetUserReviewTypes", dbConnection);

            var reviewTypeIDsTable = GenerateDataRecords(reviewTypeIds, "GUID", 50);

            var sqlParameter = new SqlParameter("@ReviewTypeIDs", SqlDbType.Structured) { Value = reviewTypeIDsTable };
            command.Parameters.Add(sqlParameter);

            var da = new SqlDataAdapter(command);
            dbConnection.Open();
            try
            {
                da.Fill(ds);
            }
            finally
            {
                dbConnection.Close();
            }

            return ds;
        }
    }
}
