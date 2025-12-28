using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BT.TS360Constants;

namespace BT.TS360API.Marketing.DataAccess
{
    public class MarketingDAO : BaseDAO
    {
        private static volatile MarketingDAO _instance;
        private static readonly object SyncRoot = new Object();

        private MarketingDAO()
        { // prevent init object outside
        }

        public static MarketingDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new MarketingDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["MarketingConnectionString"]; }
        }

        public DataSet GetApprovedDiscounts()
        {
            var ds = new DataSet();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage(DBStores.mktg_spRuntimeLoadDiscounts, dbConnection);

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

        public DataSet GetApprovedAds()
        {
            var ds = new DataSet();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage(DBStores.mktg_spRuntimeLoadAdvertisements, dbConnection);

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

        public string GetExpressionBodyById(int exprId)
        {
            var ds = new DataSet();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage(DBStores.mktg_spGetExpression, dbConnection);

            var sqlParamaters = CreateSqlParamaters(1);
            sqlParamaters[0] = new SqlParameter("@expr_id", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = exprId };
            command.Parameters.AddRange(sqlParamaters);

            var da = new SqlDataAdapter(command);
            dbConnection.Open();
            try
            {
                da.Fill(ds);

                return ds.Tables.Count == 0 ? "" : DataAccessHelper.ConvertToString(ds.Tables[0].Rows[0]["u_expr_body"]);
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
