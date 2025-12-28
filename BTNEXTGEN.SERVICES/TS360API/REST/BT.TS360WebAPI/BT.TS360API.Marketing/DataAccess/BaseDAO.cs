using System;
using System.Data;
using System.Data.SqlClient;
using BT.TS360API.Logging;
using BT.TS360Constants;

namespace BT.TS360API.Marketing.DataAccess
{
    public abstract class BaseDAO
    {
        public abstract string ConnectionString
        {
            get;
        }

        protected virtual SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected virtual SqlCommand CreateSqlCommand(string commandName, SqlConnection sqlConnection)
        {
            return new SqlCommand(commandName, sqlConnection);
        }

        protected virtual SqlCommand CreateSqlSpCommand(string spName, SqlConnection sqlConnection)
        {
            var command = new SqlCommand(spName, sqlConnection) { CommandType = CommandType.StoredProcedure };

            var paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            var paramReturnValue = new SqlParameter("returnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            command.Parameters.Add(paramErrorMessage);
            command.Parameters.Add(paramReturnValue);

            return command;
        }

        protected virtual SqlCommand CreateSqlSpCommandNoErrorMessage(string spName, SqlConnection sqlConnection)
        {
            var command = new SqlCommand(spName, sqlConnection) { CommandType = CommandType.StoredProcedure };
            return command;
        }

        protected SqlParameter[] CreateSqlParamaters(int numberOfParams)
        {
            return new SqlParameter[numberOfParams];
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
            return string.Empty;
        }

        public static char ConvertToChar(object obj)
        {
            char returnValue = ' ';
            if (null != obj && DBNull.Value != obj)
            {
                char.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
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
                int.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Convert an object to bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ConvertToBoolean(object obj)
        {
            bool returnValue = false;
            if (null != obj && obj != DBNull.Value)
            {
                bool.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        public static T ConvertTo<T>(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                if (!row.IsNull(columnName))
                {
                    return row.Field<T>(columnName);
                }
            }
            return default(T);
        }

        public static decimal ConvertTodecimal(object obj)
        {
            decimal returnValue = 0;
            if (null != obj)
            {
                decimal.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        protected void HandleException(SqlCommand command)
        {
            var paramValue = command.Parameters["returnVal"].Value;
            var returnCode = -1;

            if (paramValue != null)
            {
                returnCode = DataAccessHelper.ConvertToInt(paramValue);
            }

            if (returnCode == 0)
                return;

            //Error
            paramValue = command.Parameters["@ErrorMessage"].Value;
            var errorMessage = paramValue != null ? paramValue.ToString() : "";

            var exception = new Exception(errorMessage);
            Logger.WriteLog(exception, ExceptionCategory.Order.ToString());

            throw exception;
        }
    }
}
