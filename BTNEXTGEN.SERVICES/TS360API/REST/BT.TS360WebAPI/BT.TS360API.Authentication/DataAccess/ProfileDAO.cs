using BT.TS360API.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BT.TS360API.Authentication.DataAccess
{
    public class ProfileDAO
    {
        private static volatile ProfileDAO _instance;
        private static readonly object SyncRoot = new Object();
        private string _connectionString;
        private ProfileDAO()
        { // prevent init object outside
            _connectionString = ConfigurationManager.AppSettings["NextGenProfilesConnectionString"];
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

        public UserProfile GetUserProfileById(string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = new UserProfile();
                var command = new SqlCommand("procTS360GetUserProfile", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserID", userId));

                var paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
                var paramReturnValue = new SqlParameter("returnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

                command.Parameters.Add(paramErrorMessage);
                command.Parameters.Add(paramReturnValue);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                result.UserId = userId;
                                result.OrgId = ConvertToString(reader["u_org_id"]);
                                result.OrgName = ConvertToString(reader["u_Name"]);
                                result.UserAlias = ConvertToString(reader["u_user_alias"]);
                                result.UserEmail = ConvertToString(reader["u_email_address"]);
                                result.UserName = ConvertToString(reader["u_user_name"]);

                                if (reader.HasColumn("is_customer_admin"))
                                {
                                    result.IsCustomerAdmin = reader["is_customer_admin"].ToString() == "1";
                                }
                            }
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }

                return result;
            }
        }

        private string ConvertToString(object obj)
        {
            if (null != obj && DBNull.Value != obj)
            {
                return obj.ToString();
            }
            return String.Empty;
        }
    }

    public static class DataRecordExtensions
    {
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}