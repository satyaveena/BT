using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using Microsoft.SqlServer.Server;

namespace BT.TS360API.ExternalServices.DataAccess
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

        public static List<SqlDataRecord> GenerateDataRecords<T>(List<T> dataList, string columnName, int maxLength = 255)
        {
            if (dataList == null)
                return null;
            if (dataList.Count == 0)
                return null;

            var type = typeof(T);
            var sqlMetaDatas = new SqlMetaData[1];
            sqlMetaDatas[0] = ParseSqlMetaData(columnName, type, maxLength);

            var dict = new Dictionary<T, bool>();

            //fill data from dataset to data record
            var dataRecords = new List<SqlDataRecord>();
            foreach (var item in dataList)
            {
                if (dict.ContainsKey(item)) continue;

                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                dataRecord.SetValue(0, item);
                dataRecords.Add(dataRecord);

                dict.Add(item, true);
            }
            return dataRecords;
        }

        public static SqlMetaData ParseSqlMetaData(String columnName, Type type, Int64 maxLength)
        {
            var sqlParameter = new SqlParameter();
            sqlParameter.DbType = (DbType)TypeDescriptor.GetConverter(sqlParameter.DbType).ConvertFrom(type.Name);

            if (sqlParameter.SqlDbType == SqlDbType.Char || sqlParameter.SqlDbType == SqlDbType.NChar ||
                sqlParameter.SqlDbType == SqlDbType.NVarChar || sqlParameter.SqlDbType == SqlDbType.VarChar)
            {
                if (maxLength > 8000)
                {
                    maxLength = -1;
                }
                return new SqlMetaData(columnName, sqlParameter.SqlDbType, maxLength);
            }
            else if (sqlParameter.SqlDbType == SqlDbType.Text || sqlParameter.SqlDbType == SqlDbType.NText)
            {
                return new SqlMetaData(columnName, sqlParameter.SqlDbType, -1);
            }
            else
            {
                return new SqlMetaData(columnName, sqlParameter.SqlDbType);
            }
        }
    }
}
