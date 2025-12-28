using System;
using System.Collections.Generic;
using System.Configuration;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360Constants;

namespace BT.TS360API.Marketing.DataAccess
{
    public class ProfileDAO : BaseDAO
    {
        private static volatile ProfileDAO _instance;
        private static readonly object SyncRoot = new Object();

        private ProfileDAO()
        { // prevent init object outside
        }

        public static ProfileDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProfileDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["NextGenProfilesConnectionString"]; }
        }

        public List<BTProductInterestGroup> GetProductInterestGroup()
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var result = new List<BTProductInterestGroup>();
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.BTNG_ProductInterestGroup, dbConnection);

                //Open Connection
                dbConnection.Open();

                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //
                        while (reader.Read())
                        {
                            var pigObj = new BTProductInterestGroup
                            {
                                Id = DataAccessHelper.ConvertToString(reader["ID"]),
                                PIGName = DataAccessHelper.ConvertToString(reader["Name"]),
                                Description = DataAccessHelper.ConvertToString(reader["Description"]),
                                MarketTypeList = DataAccessHelper.ConvertToStringArray(reader["MarketType"].ToString()),
                                ProductTypeList = DataAccessHelper.ConvertToStringArray(reader["ProductType"].ToString())
                            };

                            result.Add(pigObj);
                        }
                    }
                }

                return result;
            }
        }
    }
}
