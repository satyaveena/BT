using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

using BT.ILSQueue.Business.Constants;

namespace BT.ILSQueue.Business.Helpers
{
    public class DataAccessHelper
    {
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

        public static bool ConvertToBool(object obj)
        {
            if (obj == null || obj == DBNull.Value) return false;

            var resultString = obj.ToString();
            switch (resultString)
            {
                case "1":
                    resultString = "True";
                    break;
                case "0":
                    resultString = "False";
                    break;
            }

            bool result;
            return bool.TryParse(resultString, out result) && result;
        }

        /// <summary>
        /// Convert an object to decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ConvertTodecimal(object obj)
        {
            decimal returnValue = 0;
            if (null != obj)
            {
                Decimal.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        public static string ConvertCartStatus(object obj)
        {
            var returnValue = String.Empty;
            if (obj != null && obj != DBNull.Value)
            {
                var cartStatus = obj.ToString();
                var enumCartStatus = (CartStatus)Enum.Parse(typeof(CartStatus), cartStatus);
                if (enumCartStatus == CartStatus.Downloaded)
                    returnValue = CartStatus.Downloaded.ToString();
                else if (enumCartStatus == CartStatus.Open)
                    returnValue = CartStatus.Open.ToString();
                else if (enumCartStatus == CartStatus.Ordered)
                    returnValue = CartStatus.Ordered.ToString();
                else if (enumCartStatus == CartStatus.Submitted)
                    returnValue = CartStatus.Submitted.ToString();
                else if (enumCartStatus == CartStatus.Deleted)
                    returnValue = CartStatus.Deleted.ToString();
                else if (enumCartStatus == CartStatus.Archived)
                    returnValue = CartStatus.Archived.ToString();
                else if (enumCartStatus == CartStatus.Quote_Submitted)
                    returnValue = CartStatus.Quote_Submitted.ToString();
                else if (enumCartStatus == CartStatus.Quoted)
                    returnValue = CartStatus.Quoted.ToString();
                else if (enumCartStatus == CartStatus.Quote_Transmitted)
                    returnValue = CartStatus.Quote_Transmitted.ToString();
                else if (enumCartStatus == CartStatus.Ordered_Quote)
                    returnValue = CartStatus.Ordered_Quote.ToString();
                else if (enumCartStatus == CartStatus.Processing)
                    returnValue = CartStatus.Processing.ToString();
                else if (enumCartStatus == CartStatus.VIP_Submitted)
                    returnValue = CartStatus.VIP_Submitted.ToString();
                else if (enumCartStatus == CartStatus.VIP_Ordered)
                    returnValue = CartStatus.VIP_Ordered.ToString();
            }
            returnValue = returnValue.Replace('_', ' ');

            return returnValue;
        }

       

        public static T SqlDataConvertTo<T>(DataRow row, string columnName)
        {
            return SqlDataConvertTo<T>(row, columnName, true);
        }

        public static T SqlDataConvertTo<T>(DataRow row, string columnName, bool throwExceptionIfColNotFound)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                if (!row.IsNull(columnName))
                {
                    return row.Field<T>(columnName);
                }
            }
            else
            {
                if (throwExceptionIfColNotFound)
                    throw new Exception(string.Format("Column name '{0}' not found."));
            }
            return default(T);
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

    }

    public class SqlDataRecordComparerByBTKey : IEqualityComparer<SqlDataRecord>
    {
        // SqlDataRecord are equal if their BTKey are the same. 
        public bool Equals(SqlDataRecord x, SqlDataRecord y)
        {
            // Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(x, y)) return true;

            // Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            // Check whether the SqlDataRecordComparerByBTKey are equal. 
            return ((string)x["BTKey"]).Trim() == ((string)y["BTKey"]).Trim();
        }

        // If Equals() returns true for a pair of objects, 
        // GetHashCode must return the same value for these objects. 
        public int GetHashCode(SqlDataRecord r)
        {
            var v = ((string)r["BTKey"]).Trim();
            int hash = string.IsNullOrEmpty(v) ? 0 : v.GetHashCode();

            return hash;
        }
    }
}
