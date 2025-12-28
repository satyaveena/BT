using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
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

        public static List<SqlDataRecord> GetutblUserGroupMemberDataRecords(List<ShareGroupMember> listMember)
        {
            if (listMember == null || listMember.Count == 0)
            {
                return null;
            }

            var dataRecords = new List<SqlDataRecord>();
            var sqlMetaDatas = new SqlMetaData[7];

            //Generating collumns metadata
            sqlMetaDatas[0] = new SqlMetaData("BasketUserGroupMemberId", SqlDbType.NVarChar, 50);
            sqlMetaDatas[1] = new SqlMetaData("UserId", SqlDbType.NVarChar, 50);
            sqlMetaDatas[2] = new SqlMetaData("HasContribution", SqlDbType.Bit);
            sqlMetaDatas[3] = new SqlMetaData("HasRequisition", SqlDbType.Bit);
            sqlMetaDatas[4] = new SqlMetaData("HasReview", SqlDbType.Bit);
            sqlMetaDatas[5] = new SqlMetaData("HasAcquisition", SqlDbType.Bit);
            sqlMetaDatas[6] = new SqlMetaData("IsOwner", SqlDbType.Bit);

            //Binding rows
            foreach (var member in listMember)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                if (!string.IsNullOrEmpty(member.ShareGroupMemberId))
                {
                    dataRecord.SetString(0, member.ShareGroupMemberId);
                }
                else
                {
                    dataRecord.SetString(0, Guid.NewGuid().ToString("b"));
                }

                if (!string.IsNullOrEmpty(member.UserId))
                {
                    dataRecord.SetString(1, member.UserId);
                }
                else
                {
                    dataRecord.SetDBNull(1);
                }

                dataRecord.SetBoolean(2, member.HasContributionStage);
                dataRecord.SetBoolean(3, member.HasRequisitionStage);
                dataRecord.SetBoolean(4, member.HasReviewStage);
                dataRecord.SetBoolean(5, member.HasAcquisitionStage);
                dataRecord.SetBoolean(6, member.IsOwner);

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

        internal static CartFolderType ConvertToFolderType(object value)
        {
            int folderType = DataAccessHelper.ConvertToInt(value);
            return (CartFolderType)folderType;
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

        public static List<SqlDataRecord> GetUtblESPCodes(List<ESPCode> espCode)
        {
            if (espCode == null || espCode.Count == 0)
            {
                return null;
            }

            var dataRecords = new List<SqlDataRecord>();

            var sqlMetaDatas = new SqlMetaData[2];
            sqlMetaDatas[0] = new SqlMetaData("GridFieldID", SqlDbType.NVarChar, 50);
            sqlMetaDatas[1] = new SqlMetaData("ESPMatchIndicator", SqlDbType.Bit);

            //Binding rows
            foreach (var code in espCode)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);

                dataRecord.SetString(0, code.GridCodeID);
                dataRecord.SetBoolean(1, code.ESPMatchIndicator);

                dataRecords.Add(dataRecord);
            }

            return dataRecords;
        }
    }

    // Custom comparer for the SqlDataRecordComparerByBTKey class. 
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
