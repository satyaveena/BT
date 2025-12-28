using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace BT.TS360API.Common.Helpers
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

        /// <summary>
        /// Generate a list of sql data record from a dataset
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public static List<SqlDataRecord> GenerateDataRecords(DataSet dataSet)
        {
            if (dataSet == null)
                return null;

            var dataTable = dataSet.Tables[0];
            if (dataTable == null) return null;
            //create meta data
            var sqlMetaDatas = (from DataColumn col in dataTable.Columns
                                select ParseSqlMetaData(col.ColumnName, col.DataType, col.MaxLength)).ToList();
            var sqlMetaDataArray = sqlMetaDatas.ToArray();
            //fill data from dataset to data record
            var dataRecords = new List<SqlDataRecord>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDataArray);
                dataRecord.SetValues(row.ItemArray);
                dataRecords.Add(dataRecord);
            }
            return dataRecords;
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

        public static List<SqlDataRecord> GenerateDataRecords(Dictionary<string, Dictionary<string, string>> dict)
        {
            if (dict == null) return null;

            var dataRecords = new List<SqlDataRecord>();

            var firstRow = dict.Values.First();
            if (firstRow == null) return null;

            var sqlMetaDatas = new SqlMetaData[firstRow.Keys.Count];
            var listColumnNames = new Dictionary<string, int>();

            //Generating collumns metadata
            var colIndex = 0;
            foreach (var columnName in firstRow.Keys)
            {
                sqlMetaDatas[colIndex] = ParseSqlMetaData(columnName, typeof(string), 255);
                listColumnNames.Add(columnName, colIndex);
                colIndex++;
            }

            //Binding rows
            foreach (var row in dict.Values)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);

                foreach (var cell in row)
                {
                    var ordinary = listColumnNames[cell.Key];

                    if (cell.Value != null)
                        dataRecord.SetString(ordinary, cell.Value);
                }

                dataRecords.Add(dataRecord);
            }

            return dataRecords;
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

        public static DateTime? ConvertToDateTimeNull(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName))
            {
                if (!row.IsNull(columnName))
                {
                    return row.Field<DateTime>(columnName);
                }
            }
            return null;
        }
        /// <summary>
        /// Convert an object to GUID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Guid ConvertToGUID(object obj)
        {
            if (null != obj)
            {
                return new Guid(obj.ToString());
            }
            else
                return Guid.Empty;
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

        public static int? ConvertToIntNullable(object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;
            return ConvertToInt(obj);
        }

        /// <summary>
        /// Convert an object to byte
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte ConvertToByte(object obj)
        {
            byte returnValue = 0;
            if (null != obj)
            {
                Byte.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Convert an object to bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        public static bool? ConvertToBoolNullable(object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;
            return ConvertToBool(obj);
        }

        /// <summary>
        /// Convert an object to decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ConvertToGetDecimal(object obj)
        {
            decimal returnValue = 0;
            if (null != obj)
            {
                Decimal.TryParse(obj.ToString(), out returnValue);
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

        /// <summary>
        /// Convert an object to long
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ConvertToLong(object obj)
        {
            long returnValue = 0;
            if (null != obj)
            {
                Int64.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
        }

        /// <summary>
        /// Convert an object to double
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ConvertTodouble(object obj)
        {
            double returnValue = 0;
            if (null != obj)
            {
                Double.TryParse(obj.ToString(), out returnValue);
            }
            return returnValue;
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

        public static decimal? ConvertToDecimalNullable(object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;

            decimal returnValue;
            if (!Decimal.TryParse(obj.ToString(), out returnValue))
            {
                return null;
            }
            return returnValue;
        }
        public static float? ConvertToFloatNullable(object obj)
        {
            if (obj == null) return null;

            float returnValue;
            if (!float.TryParse(obj.ToString(), out returnValue))
            {
                return null;
            }
            return returnValue;
        }
        /// <summary>
        /// ConvertToMultipleValue
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static string[] ConvertToStringArray(string item)
        {
            // invalid string
            if (String.IsNullOrEmpty(item)) return null;

            var results = new List<string>();

            var temp = item.Split(';');

            // invalid string
            if (temp.Length == 0 || temp.Length == 1) return null;

            for (var i = 1; i < temp.Length; i++)
            {
                results.Add(temp[i]);
            }

            return results.Count == 0 ? null : results.ToArray();
        }

        public static object GetDbNullValueIfAny(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }
        public static List<SqlDataRecord> GetGUIDTable(string[] guids)
        {
            var dataRecords = new List<SqlDataRecord>();

            if (guids == null || guids.Length <= 0)
            {
                return null;
            }
            var sqlMetaDatas = new SqlMetaData[1];

            //Generating collumns metadata
            sqlMetaDatas[0] = new SqlMetaData("GUID", SqlDbType.NVarChar, 50);

            foreach (var guid in guids)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);

                dataRecord.SetString(0, guid);

                dataRecords.Add(dataRecord);
            }

            return dataRecords;
        }

        public static SqlParameter CreateTableParameter(string parameterName, string parameterTypeName, DataTable value)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                SqlDbType = SqlDbType.Structured,
                TypeName = parameterTypeName,
                Value = value
            };
        }

        public static DataTable CreateArgumentTable()
        {
            var dt = new DataTable("utblCSGuids");
            dt.Columns.Add("GUID", typeof(string));
            return dt;
        }

        public static DataTable ConvertToListArgumentTable(IEnumerable<string> items, DataTable dt)
        {
            if (items != null)
            {
                var list = items.ToList();
                if (list.Any())
                    list.ForEach(r => dt.Rows.Add(r));
            }
            return dt;
        }

    }
}
