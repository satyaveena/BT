using System;
using System.Data;
using System.Data.SqlClient;
using BT.Auth.Business.Helper;
using BT.Auth.Business.Logger;

namespace BT.Auth.Business.DataAccess
{
    /// <summary>
    /// Abstract Class BaseDAO
    /// </summary>
    public abstract class BaseDAO
    {

        #region Public Property
        public abstract string ConnectionString
        {
            get;
        }
        #endregion

        #region Protected Method

        /// <summary>
        /// CreateSqlConnection
        /// </summary>
        /// <returns>SqlConnection</returns>
        protected virtual SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// CreateSqlCommand
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="sqlConnection"></param>
        /// <returns>SqlCommand</returns>
        protected virtual SqlCommand CreateSqlCommand(string commandName, SqlConnection sqlConnection)
        {
            return new SqlCommand(commandName, sqlConnection);
        }

        /// <summary>
        /// CreateSqlSpCommand
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="sqlConnection"></param>
        /// <returns>SqlCommand</returns>
        protected virtual SqlCommand CreateSqlSpCommand(string spName, SqlConnection sqlConnection)
        {
            var command = new SqlCommand(spName, sqlConnection) { CommandType = CommandType.StoredProcedure };

            var paramErrorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            var paramReturnValue = new SqlParameter("returnVal", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };

            command.Parameters.Add(paramErrorMessage);
            command.Parameters.Add(paramReturnValue);

            return command;
        }

        /// <summary>
        /// CreateSqlSpCommandNoErrorMessage
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="sqlConnection"></param>
        /// <returns>SqlCommand</returns>
        protected virtual SqlCommand CreateSqlSpCommandNoErrorMessage(string spName, SqlConnection sqlConnection)
        {
            var command = new SqlCommand(spName, sqlConnection) { CommandType = CommandType.StoredProcedure };
            return command;
        }

        /// <summary>
        /// HandleException
        /// </summary>
        /// <param name="command"></param>
        /// <param name="procName"></param>
        protected void HandleException(SqlCommand command, string procName = "")
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
            ILogger logger = new Logger.Logger();
            logger.Log(Logger.Enum.LogLevel.Error, errorMessage);
            throw exception;
        }
        #endregion
    }
}
