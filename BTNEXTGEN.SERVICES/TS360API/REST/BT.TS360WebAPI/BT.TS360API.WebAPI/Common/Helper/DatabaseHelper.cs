using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BT.TS360API.WebAPI.Common.Helper
{
    public class DatabaseHelper
    {
        public enum ResultCode { Succeed, Warning, Error };

        public static DataSet ExecuteCommand(string commandText, SqlConnection conn)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(commandText, conn);
            da.SelectCommand.CommandType = CommandType.Text;

            try
            {
                conn.Open();

                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ds;

        }

        public static DataSet ExecuteCommand(string commandText, SqlParameter[] parameters, SqlConnection conn)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(commandText, conn);
            da.SelectCommand.CommandType = CommandType.Text;

            foreach (SqlParameter param in parameters)
                da.SelectCommand.Parameters.Add(param);

            try
            {
                conn.Open();

                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ds;
        }

        public static SqlDataReader ExecuteReader(string procedureName, SqlParameter[] parameters, SqlConnection conn)
        {
            SqlDataReader dr = null;

            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = procedureName;
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandTimeout = 0; //18 hours - 64800 to no limit 

            foreach (SqlParameter param in parameters)
                cm.Parameters.Add(param);

            try
            {
                conn.Open();

                dr = cm.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dr;

        }

        public static DataSet ExecuteProcedure(string procedureName, SqlParameter[] parameters, SqlConnection conn, out string errorMessage)
        {
            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(procedureName, conn);
            da.SelectCommand.CommandTimeout = 0; //18 hours - 64800 to no limit 
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter param in parameters)
                da.SelectCommand.Parameters.Add(param);

            try
            {
                conn.Open();

                da.Fill(ds);

                object paramValue = da.SelectCommand.Parameters["@ErrorMessage"].Value;
                errorMessage = paramValue != null ? paramValue.ToString() : "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ds;
        }

        public static DataSet ExecuteProcedure(string procedureName, SqlParameter[] parameters, SqlConnection conn)
        {
            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(procedureName, conn);
            da.SelectCommand.CommandTimeout = 0; //18 hours - 64800 to no limit 
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter param in parameters)
                da.SelectCommand.Parameters.Add(param);

            try
            {
                conn.Open();

                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return ds;
        }

        public static int ExecuteScaler(string procedureName, SqlParameter[] parameters, SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand(procedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter param in parameters)
                cmd.Parameters.Add(param);

            int retVal = 0;

            try
            {
                conn.Open();

                object o = cmd.ExecuteScalar();

                if (o != null && o != DBNull.Value)
                    retVal = Int32.Parse(o.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return retVal;
        }

        public static int ExecuteProcedureRowCount(string procedureName, SqlParameter[] parameters, SqlConnection conn)
        {
            int rows = 0;
            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter(procedureName, conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter param in parameters)
                da.SelectCommand.Parameters.Add(param);

            try
            {
                conn.Open();

                da.Fill(ds);

                if (ds != null && ds.Tables.Count > 0)
                {
                    rows = ds.Tables[0].Rows.Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }

            return rows;
        }


        public static void ExecuteNonQuery(string procedureName, SqlParameter[] parameters, SqlConnection conn)
        {
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = procedureName;
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandTimeout = 0; //18 hours - 64800 to no limit 
            foreach (SqlParameter param in parameters)
                cm.Parameters.Add(param);

            try
            {
                conn.Open();

                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public static void ExecuteNonQuery(string procedureName, SqlParameter[] parameters, SqlConnection conn, out string errorMessage)
        {
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = procedureName;
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandTimeout = 0; //18 hours - 64800 to no limit 
            foreach (SqlParameter param in parameters)
                cm.Parameters.Add(param);

            try
            {
                conn.Open();

                cm.ExecuteNonQuery();

                object paramValue = cm.Parameters["@ErrorMessage"].Value;
                errorMessage = paramValue != null ? paramValue.ToString() : "";

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        public static SqlCommand ExecuteNonQueryOutput(string procedureName, SqlParameter[] parameters, SqlConnection conn)
        {
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = procedureName;
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandTimeout = 0; //18 hours - 64800 to no limit 
            foreach (SqlParameter param in parameters)
                cm.Parameters.Add(param);

            try
            {
                conn.Open();

                cm.ExecuteNonQuery();

                return cm;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
             
            }
        }
        public static void ExecuteNonQueryProcedure(string procedureName, SqlConnection conn)
        {
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = procedureName;
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandTimeout = 0;

            try
            {
                conn.Open();

                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }


        public static void ExecuteNonQueryText(string cmdText, SqlConnection conn)
        {
            SqlCommand cm = conn.CreateCommand();
            cm.CommandText = cmdText;
            cm.CommandType = CommandType.Text;

            try
            {
                conn.Open();

                cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }
        public static string ConvertStringFromDB(DataRow dr, string columnName)
        {
            try
            {
                return (dr[columnName] != null) ? dr[columnName].ToString().Trim() : "";
            }
            catch (ArgumentException)
            {
                return "";
            }
        }

        public static SqlParameter CreateSqlOutputParameter(string parameterName, SqlDbType sqlType, int size)
        {
            SqlParameter outputParam = new SqlParameter(string.Format("@{0}", parameterName), SqlDbType.Int, 0);
            outputParam.Direction = ParameterDirection.Output;

            return outputParam;
        }

        /// <summary>
        /// Convert an object to string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertToString(object obj)
        {
            if (null != obj && DBNull.Value != obj)
            {
                return obj.ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// Convert an object to Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ConvertToInt(object obj)
        {
            int returnValue = 0;
            if (null != obj)
            {
                Int32.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        public static long ConvertToInt64(object obj)
        {
            long returnValue = 0;
            if (null != obj)
            {
                Int64.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Convert an object to datetime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ConvertToDateTime(object obj)
        {
            DateTime? returnValue = null;
            if (DBNull.Value != obj)
            {
                DateTime temp;
                DateTime.TryParse(obj.ToString(), out temp);
                returnValue = temp;
            }
            return returnValue;
        }
    }
}