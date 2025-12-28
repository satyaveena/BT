using System.Data;
using System.Data.SqlClient;
using BT.TS360API.Common.Configrations;
using BT.TS360Constants;
using AppSettings = BT.TS360API.Common.Configrations.AppSettings;

namespace BT.TS360API.Common.DataAccess
{
    public class InventoryDAO
    {
        #region Private properties

        private static string _connectionString = string.Empty;
        private const int DefaultCacheMinutes = 60;

        #endregion

        //#region Public Methods (SQL Helper)

        /// <summary>
        /// Connection String
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (_connectionString.ToLower().Contains(GeneralConstants.ProviderName))
                {
                    int firstDelimeter = _connectionString.IndexOf(GeneralConstants.Semicolon);
                    _connectionString = _connectionString.Substring(firstDelimeter + 1);
                }
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public static SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static SqlCommand CreateSqlCommand(string commandName, SqlConnection sqlConnection)
        {
            return new SqlCommand(commandName, sqlConnection);
        }

        public static SqlCommand CreateSqlSpCommand(string spName, SqlConnection sqlConnection)
        {
            return new SqlCommand(spName, sqlConnection) { CommandType = CommandType.StoredProcedure };
        }

        public static SqlParameter[] CreateSqlParamaters(int numberOfParams)
        {
            return new SqlParameter[numberOfParams];
        }

        //#endregion

        //#region Public Methods (DAO Helper)

        ///// <summary>
        ///// Convert an object to GUID
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static Guid ConvertToGUID(object obj)
        //{
        //    if (null != obj)
        //    {
        //        return new Guid(obj.ToString());
        //    }
        //    else
        //        return Guid.Empty;
        //}


        ///// <summary>
        ///// Convert an object to string
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static string ConvertToString(object obj)
        //{
        //    if (null != obj)
        //    {
        //        return obj.ToString();
        //    }
        //    return string.Empty;
        //}

        ///// <summary>
        ///// Convert an object to Int
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static int ConvertToInt(object obj)
        //{
        //    int returnValue = 0;
        //    if (null != obj)
        //    {
        //        int.TryParse(obj.ToString(), out returnValue);
        //    }
        //    return returnValue;
        //}

        ///// <summary>
        ///// Convert an object to byte
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static byte ConvertToByte(object obj)
        //{
        //    byte returnValue = 0;
        //    if (null != obj)
        //    {
        //        byte.TryParse(obj.ToString(), out returnValue);
        //    }
        //    return returnValue;
        //}

        ///// <summary>
        ///// Convert an object to bool
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static bool ConvertToBool(object obj)
        //{
        //    bool returnValue = false;
        //    if (null != obj)
        //    {
        //        bool.TryParse(obj.ToString(), out returnValue);
        //    }
        //    return returnValue;
        //}

        ///// <summary>
        ///// Convert an object to decimal
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static decimal ConvertToGetDecimal(object obj)
        //{
        //    decimal returnValue = 0;
        //    if (null != obj)
        //    {
        //        decimal.TryParse(obj.ToString(), out returnValue);
        //    }
        //    return returnValue;
        //}

        ///// <summary>
        ///// Convert an object to datetime
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static DateTime? ConvertToDateTime(object obj)
        //{
        //    DateTime? returnValue = null;
        //    if (System.DBNull.Value != obj)
        //    {
        //        DateTime temp;
        //        DateTime.TryParse(obj.ToString(), out temp);
        //        returnValue = temp;
        //    }
        //    return returnValue;
        //}

        ///// <summary>
        ///// Convert an object to long
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static long ConvertToLong(object obj)
        //{
        //    long returnValue = 0;
        //    if (null != obj)
        //    {
        //        long.TryParse(obj.ToString(), out returnValue);
        //    }
        //    return returnValue;
        //}

        ///// <summary>
        ///// Convert an object to double
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static double ConvertTodouble(object obj)
        //{
        //    double returnValue = 0;
        //    if (null != obj)
        //    {
        //        double.TryParse(obj.ToString(), out returnValue);
        //    }
        //    return returnValue;
        //}

        ///// <summary>
        ///// Convert DataReader To DataSet
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <returns></returns>
        //public static DataSet ConvertDataReaderToDataSet(SqlDataReader reader)
        //{
        //    var dataSet = new DataSet();
        //    do
        //    {
        //        // Create new data table
        //        var schemaTable = reader.GetSchemaTable();
        //        var dataTable = new DataTable();

        //        if (schemaTable != null)
        //        {
        //            for (var i = 0; i < schemaTable.Rows.Count; i++)
        //            {
        //                var dataRow = schemaTable.Rows[i];
        //                var columnName = (string)dataRow[PropertyName.ColumnName];
        //                var column = new DataColumn(columnName, (Type)dataRow[PropertyName.DataType]);
        //                dataTable.Columns.Add(column);
        //            }
        //            dataSet.Tables.Add(dataTable);

        //            while (reader.Read())
        //            {
        //                var dataRow = dataTable.NewRow();
        //                for (var i = 0; i < reader.FieldCount; i++)
        //                    dataRow[i] = reader.GetValue(i);
        //                dataTable.Rows.Add(dataRow);
        //            }
        //        }
        //        else
        //        {
        //            // No data
        //            var column = new DataColumn(PropertyName.RowsAffected);
        //            dataTable.Columns.Add(column);
        //            dataSet.Tables.Add(dataTable);
        //            var dataRow = dataTable.NewRow();
        //            dataRow[0] = reader.RecordsAffected;
        //            dataTable.Rows.Add(dataRow);
        //        }
        //    }
        //    while (reader.NextResult());

        //    return dataSet;
        //}

        public static string CatalogConnectionString
        {
            get
            {
                //var catalogConnectionString = GetCache(GeneralConstants.CatalogCacheKey) as string;
                //if (!string.IsNullOrEmpty(catalogConnectionString))
                //    return catalogConnectionString;

                var catalogConnectionString = AppSettings.CsproductcatalogConnectionstring;// GlobalConfiguration.ReadAppSetting(GlobalConfigurationKey.CsproductcatalogConnectionstring).Value;
                //WriteCache(GeneralConstants.CatalogCacheKey, catalogConnectionString);

                return catalogConnectionString;
            }
        }


        //#endregion

        //#region Private Methods (Cache)

        ///// <summary>
        ///// Get Cache
        ///// </summary>
        ///// <param name="cacheKey"></param>
        ///// <returns></returns>
        //private static object GetCache(string cacheKey)
        //{
        //    return HttpContext.Current.Cache.Get(cacheKey);
        //}

        ///// <summary>
        ///// Writes the specified cache key.
        ///// </summary>
        ///// <param name="cacheKey">The cache key.</param>
        ///// <param name="value">The object to cache.</param>
        //private static void WriteCache(string cacheKey, object value)
        //{
        //    HttpContext.Current.Cache.Add(
        //           cacheKey,
        //           value,
        //           null,
        //           DateTime.Now.AddMinutes(DefaultCacheMinutes),
        //           System.Web.Caching.Cache.NoSlidingExpiration,
        //           CacheItemPriority.Normal,
        //           null);
        //}

        //#endregion
    }
}
