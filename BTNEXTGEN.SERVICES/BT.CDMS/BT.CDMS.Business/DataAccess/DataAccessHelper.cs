using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace BT.CDMS.Business.DataAccess
{
    /// <summary>
    /// DataAccessHelper
    /// </summary>
    public class DataAccessHelper
    {
        #region Public Method
        /// <summary>
        /// ConvertToInt
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>int</returns>
        public static int ConvertToInt(object obj)
        {
            int returnValue = 0;
            if (null != obj)
            {
                Int32.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Generate a list of sql data record from list of data item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="columnName"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static List<SqlDataRecord> GenerateDataRecords<T>(List<T> dataList, string columnName, int maxLength = 255)
        {
            if (dataList == null)
                return null;
            if (dataList.Count == 0)
                return null;

            var type = typeof(T);
            var sqlMetaDatas = new SqlMetaData[1];
            sqlMetaDatas[0] = ParseSqlMetaData(columnName, type, maxLength);
            //fill data from dataset to data record
            var dataRecords = new List<SqlDataRecord>();
            foreach (var item in dataList)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                dataRecord.SetValue(0, item);
                dataRecords.Add(dataRecord);
            }
            return dataRecords;
        }

        /// <summary>  
        ///<para>This method takes a column name, type and maximum length, returning the column definition as SqlMetaData.</para>  
        /// </summary>
        /// <param name="String">A column name to be used in the returned Microsoft.SqlServer.Server.SqlMetaData.</param>
        /// <param name="System.Type">A column data type to be used in the returned Microsoft.SqlServer.Server.SqlMetaData.</param>
        /// <param name="System.Int32">The maximum length of the column to be used in the returned Microsoft.SqlServer.Server.SqlMetaData.</param>
        ///<param name="columnName"></param>
        ///<param name="type"></param>
        ///<param name="maxLength"></param>
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
        #endregion
    }
}
