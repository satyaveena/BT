using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using BT.ILSQueue.Business.Helpers;
using BT.ILSQueue.Business.Exceptions;

namespace BT.ILSQueue.Business.DAO
{
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
            command.CommandTimeout = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.SqlCommandTimeOut, 30); //default is 30s

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
            var returnCode = Convert.ToInt16(paramValue);

            if (returnCode == 0)
                return;  // valid case.
            if (returnCode == -1)
            {
                var paramError = command.Parameters["@ErrorMessage"].Value;
                string sqlErrorMessage = "Exception from SQL. ";
                if (paramError != null && !string.IsNullOrEmpty(paramError.ToString()))
                    sqlErrorMessage += paramError.ToString();

                throw new Exception(sqlErrorMessage);
            }

           throw new BusinessException(returnCode);

        }

        protected SqlParameter[] CreateSqlParamaters(int numberOfParams)
        {
            return new SqlParameter[numberOfParams];
        }
        #endregion
    }
}
