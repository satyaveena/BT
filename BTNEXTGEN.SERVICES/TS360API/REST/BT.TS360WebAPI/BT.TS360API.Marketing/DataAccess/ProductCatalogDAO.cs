using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using Microsoft.SqlServer.Server;

namespace BT.TS360API.Marketing.DataAccess
{
    public class CsProductCatalogDAO : BaseDAO
    {
        private static volatile CsProductCatalogDAO _instance;
        private static readonly object SyncRoot = new Object();

        private CsProductCatalogDAO()
        {
        }

        public static CsProductCatalogDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CsProductCatalogDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["CSProductCatalog_ConnectionString"]; }
        }

        public List<BtKeyParentCategoryObject> GetParentCategories(List<BtKeyCatalogObject> listProducts, bool isVirtualCatalog)
        {
            var results = new List<BtKeyParentCategoryObject>();

            if (listProducts == null || listProducts.Count == 0) return results;

            var dataRecords = GetDataRecordsForProductCatalog(listProducts);

            var dbConnection = new SqlConnection(ConnectionString);
            var command = CreateSqlSpCommandNoErrorMessage("procTS360GetParentCategories", dbConnection);

            //<Parameter>
            var sqlParamaters = CreateSqlParamaters(2);

            sqlParamaters[0] = new SqlParameter("@BTKeyCatalogs", SqlDbType.Structured)
            {
                Direction =
                    ParameterDirection.Input,
                TypeName =
                    "utblBTKeyCatalog",
                Value = dataRecords
            };

            sqlParamaters[1] = new SqlParameter("@IsVirtualCatalog", SqlDbType.Bit)
            {
                Direction =
                    ParameterDirection.Input,
                Value = isVirtualCatalog
            };

            //</Parameter>
            command.Parameters.AddRange(sqlParamaters);
            //            
            try
            {
                dbConnection.Open();
                command.CommandTimeout = 360;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var parentItem = new BtKeyParentCategoryObject();
                            parentItem.BTKey = DataAccessHelper.ConvertToString(reader["BTKey"]);
                            parentItem.CatalogName = DataAccessHelper.ConvertToString(reader["CatalogName"]);
                            parentItem.ParentCategory = DataAccessHelper.ConvertToString(reader["ParentCategory"]);
                            parentItem.PrimaryParentCategory = DataAccessHelper.ConvertToString(reader["PrimaryParentCategory"]);

                            results.Add(parentItem);
                        }
                    }
                }
            }
            catch (SqlException sqlException)
            {
                Logger.RaiseException(sqlException, ExceptionCategory.Catalog);
            }
            finally
            {
                dbConnection.Close();
            }

            return results;
        }

        private static List<SqlDataRecord> GetDataRecordsForProductCatalog(List<BtKeyCatalogObject> listProducts)
        {
            var dataRecords = new List<SqlDataRecord>();
            SqlMetaData[] sqlMetaDatas =
            {
                new SqlMetaData("BTKey", SqlDbType.NVarChar, 100), 
                new SqlMetaData("CatalogName", SqlDbType.NVarChar, 100),
                new SqlMetaData("BaseCatalogName", SqlDbType.NVarChar, 100)
            };

            foreach (var btKeyCatalogObject in listProducts)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                dataRecord.SetString(0, btKeyCatalogObject.BTKey ?? "");
                dataRecord.SetString(1, btKeyCatalogObject.CatalogName ?? "");
                dataRecord.SetString(2, btKeyCatalogObject.BaseCatalogName ?? "");
                dataRecords.Add(dataRecord);
            }
            return dataRecords;
        }

        public List<string> GetBTKeysFromCategoryName(string promoCode)
        {
            var results = new List<string>();

            if (string.IsNullOrEmpty(promoCode)) return results;

            //var dbConnection = new SqlConnection(CsNextGenProductCatalogConnectionString);
            //var command = CreateSqlSpCommand("procTS360GetBTKeysFromCategoryName", dbConnection);
            
            var dbConnection = new SqlConnection(ConnectionString);
            var command = CreateSqlSpCommandNoErrorMessage("procTS360GetBTKeysFromCategoryName", dbConnection);

            //<Parameter>
            var sqlParamaters = CreateSqlParamaters(1);

            sqlParamaters[0] = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 128)
            {
                Direction =
                    ParameterDirection.Input,
                Value = promoCode
            };

            //</Parameter>
            command.Parameters.AddRange(sqlParamaters);
            //            
            try
            {
                dbConnection.Open();
                command.CommandTimeout = 360;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            results.Add(DataAccessHelper.ConvertToString(reader["BTKey"]));
                        }
                    }
                }
            }
            catch (SqlException sqlException)
            {
                Logger.RaiseException(sqlException, ExceptionCategory.Catalog);
            }
            finally
            {
                dbConnection.Close();
            }

            return results;
        }
    }
}
