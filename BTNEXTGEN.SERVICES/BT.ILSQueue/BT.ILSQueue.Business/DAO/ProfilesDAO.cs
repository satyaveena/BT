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
    public class ProfilesDAO: BaseDAO 
    {
        private static volatile ProfilesDAO _instance;
        private static readonly object SyncRoot = new Object();

        public static ProfilesDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProfilesDAO();
                }

                return _instance;
            }
        }

        #region Public Property
        public override string ConnectionString
        {
            get { return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.NextGenProfiles_ConnectionString); }
        }

        public ILSValidationRequest GetILSConfiguration(string orgId)
        {
            using (var dbConnection = CreateSqlConnection())
            {
               var command = CreateSqlSpCommand(StoredProcedureName.PROC_GET_ILS_CONFIGURATION, dbConnection);
                //<Parameter>
                var sqlParamaters = CreateSqlParamaters(1);
                sqlParamaters[0] = new SqlParameter("@OrgId", orgId);
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                //Open Connection
                ILSValidationRequest orgObj = null;

                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //
                        while (reader.Read())
                        {
                            orgObj = new ILSValidationRequest
                            {
                                TSOrgId = DataAccessHelper.ConvertToString(reader["u_org_id"]),
                                ILSUrl = DataAccessHelper.ConvertToString(reader["u_ILS_acquisitions_api_url"]),
                                ILSApiKey = DataAccessHelper.ConvertToString(reader["u_ILS_acquisitions_api_key"]),
                                ILSApiSecret = DataAccessHelper.ConvertToString(reader["u_ILS_acquisitions_api_passphrase"]),
                                ILSLogin = DataAccessHelper.ConvertToString(reader["u_ILS_login"]),
                                ILSValidationStatus = DataAccessHelper.ConvertToString(reader["ILSValidationStatusLiteral"]),
                                ILSValidationStatusId = DataAccessHelper.ConvertToInt(reader["ILSValidationStatusID"]),
                                ILSValidationDateTime = DataAccessHelper.ConvertToDateTime(reader["ILSValidationUpdatedDateTime"]),
                                ILSValidationErrorMessage = DataAccessHelper.ConvertToString(reader["ILSValidationLogError"]),
                                ILSUserAccount = DataAccessHelper.ConvertToString(reader["u_ils_user_account"]),
                                ILSUserDomain = DataAccessHelper.ConvertToString(reader["u_ils_user_domain"])
                            };

                        }
                    }
                }

                dbConnection.Close();
                return orgObj;
            }
        }


        public MARCJsonRequest GetMARCRequestParameter(string userID)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(StoredProcedureName.PROC_GET_ILS_ORG_USER_PROFILE, dbConnection);
                //<Parameter>
                var sqlParamaters = CreateSqlParamaters(1);
                sqlParamaters[0] = new SqlParameter("@UserID", userID);
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                //Open Connection
                MARCJsonRequest marcJsonRequest = null;

                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //
                        while (reader.Read())
                        {
                            marcJsonRequest = new MARCJsonRequest();
                           
                            marcJsonRequest.SortColumn = DataAccessHelper.ConvertToString(reader["u_cart_sort_by"]) ; 
                            marcJsonRequest.SortDirection = DataAccessHelper.ConvertToString(reader["u_cart_sort_order"]) ; 
                            
                            marcJsonRequest.IsOCLCEnabled = DataAccessHelper.ConvertToBool(reader["b_oclc_cataloging_plus_enabled"]);

                            string FullIndicator = DataAccessHelper.ConvertToString(reader["b_is_full_marc_profile"]);
                            marcJsonRequest.FullIndicator =  string.IsNullOrEmpty(FullIndicator)? "A": (Convert.ToBoolean(FullIndicator)? "F" : "A");

                            marcJsonRequest.IsBTEmployee = DataAccessHelper.ConvertToBool(reader["b_is_bt_employee"]);
                            marcJsonRequest.MarketType = DataAccessHelper.ConvertToString(reader["u_web_market_type"]);
                                
                        };
                    }
                }

                dbConnection.Close();
                return marcJsonRequest;
            }
        }

        

        #endregion
    }
}
