using BT.TS360API.WebAPI.Common.Configuration;
using BT.TS360API.WebAPI.Common.Constants;
using BT.TS360API.WebAPI.Common.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BT.TS360API.WebAPI.Common.DataAccess
{
    public class OAuthDAO
    {
        private SqlConnection _sqlConn;

        public OAuthDAO()
        {
            _sqlConn = new SqlConnection(AppSetting.ProfilesDatabaseConnectionString);
        }
        
        /// <summary>
        /// Get Primary Cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetCryptoKey(string bucket, string handle)
        {
            try
            {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@Bucket", bucket));
            alParams.Add(new SqlParameter("@Handle", handle));
            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            string sqlError = "";

            DataSet result = DatabaseHelper.ExecuteProcedure(StoredProcedure.ProcGetSymmetricCryptoKey, arr, _sqlConn, out sqlError);

            if (!string.IsNullOrEmpty(sqlError))
                throw new Exception(sqlError);
               return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            return null;
        }

        public DataSet GetCryptoKeysByBucket(string bucket)
        {
            try
            { 
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@Bucket", bucket));
            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);

            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
            string sqlError = "";

            DataSet result = DatabaseHelper.ExecuteProcedure(StoredProcedure.ProcGetSymmetricCryptoKeyByBucket, arr, _sqlConn, out sqlError);

            if (!string.IsNullOrEmpty(sqlError))
                throw new Exception(sqlError);
            return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            return null;
        }

        public void AddNonce(string context, string code, DateTime timestamp)
        {
            try
            {
                ArrayList alParams = new ArrayList();
                alParams.Add(new SqlParameter("@Context", context));
                alParams.Add(new SqlParameter("@Code", code));
                alParams.Add(new SqlParameter("@Timestamp", SqlDbType.DateTime) { Value = timestamp });

                SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
                alParams.Add(paramErrorMessage);

                SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
                string sqlError = "";

                DatabaseHelper.ExecuteNonQuery(Constants.StoredProcedure.ProcInsertNonce, arr, _sqlConn, out sqlError);

                if (!string.IsNullOrEmpty(sqlError))
                {
                    throw new Exception(sqlError);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }            
        }

        public void AddCryptoKey(string bucket, string handle, byte[] secret, DateTime expiresUtc)
        {
            try
            {
                ArrayList alParams = new ArrayList();
                alParams.Add(new SqlParameter("@Bucket", bucket));
                alParams.Add(new SqlParameter("@Handle", handle));
                alParams.Add(new SqlParameter("@ExpiresUtc", SqlDbType.DateTime) { Value = expiresUtc });
                alParams.Add(new SqlParameter("@Secret", SqlDbType.VarBinary) { Value = secret });

                SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
                alParams.Add(paramErrorMessage);

                SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
                string sqlError = "";

                DatabaseHelper.ExecuteNonQuery(Constants.StoredProcedure.ProcInsertSymmetricCryptoKey, arr, _sqlConn, out sqlError);

                if (!string.IsNullOrEmpty(sqlError))
                {
                    throw new Exception(sqlError);
                }
            }            
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }            
        }

        public void DeleteCryptokey(string bucket, string handle)
        {
            try
            {


                ArrayList alParams = new ArrayList();
                alParams.Add(new SqlParameter("@Bucket", bucket));
                alParams.Add(new SqlParameter("@Handle", handle));

                SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
                alParams.Add(paramErrorMessage);

                SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
                string sqlError = "";

                DatabaseHelper.ExecuteNonQuery(Constants.StoredProcedure.ProcDeleteSymmetricCryptoKey, arr, _sqlConn, out sqlError);

                if (!string.IsNullOrEmpty(sqlError))
                {
                    throw new Exception(sqlError);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }    
        }

        public List<string> GetScopes(string clientIdentifier, DateTime issuedUtc, string userName)
        {
            ArrayList alParams = new ArrayList();
            alParams.Add(new SqlParameter("@ClientIdentifier", clientIdentifier));
            alParams.Add(new SqlParameter("@IssuedUtc", SqlDbType.DateTime) { Value = issuedUtc });
            alParams.Add(new SqlParameter("@UserName", userName));

            SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            alParams.Add(paramErrorMessage);
            SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));

            var reader = DatabaseHelper.ExecuteReader(StoredProcedure.ProcGetScope, arr, _sqlConn);

            var result = new List<string>();
            
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(reader["Scope"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }    
            finally
            {
                reader.Close();
            }

            return result;
        }

        public DataSet GetClient(string clientIdentifider)
        {
            try
            {
                ArrayList alParams = new ArrayList();
                alParams.Add(new SqlParameter("@ClientIdentifier", clientIdentifider));
                SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
                alParams.Add(paramErrorMessage);

                SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
                string sqlError = "";

                DataSet result = DatabaseHelper.ExecuteProcedure(StoredProcedure.ProcGetClient, arr, _sqlConn, out sqlError);

                if (!string.IsNullOrEmpty(sqlError))
                    throw new Exception(sqlError);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            return null;
        }

        public DataSet GetClientAuthorization(string clientIdentifider, string userName)
        {
            try
            {
                ArrayList alParams = new ArrayList();
                alParams.Add(new SqlParameter("@ClientIdentifier", clientIdentifider));
                alParams.Add(new SqlParameter("@UserName", userName));
                SqlParameter paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
                alParams.Add(paramErrorMessage);

                SqlParameter[] arr = (SqlParameter[])alParams.ToArray(typeof(SqlParameter));
                string sqlError = "";

                DataSet result = DatabaseHelper.ExecuteProcedure(StoredProcedure.ProcGetClientAuthorization, arr, _sqlConn, out sqlError);

                if (!string.IsNullOrEmpty(sqlError))
                    throw new Exception(sqlError);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            return null;
        }
    }
}
