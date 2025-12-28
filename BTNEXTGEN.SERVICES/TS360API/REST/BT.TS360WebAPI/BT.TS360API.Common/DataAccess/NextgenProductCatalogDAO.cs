using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Common.Configrations;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using Microsoft.SqlServer.Server;

namespace BT.TS360API.Common.DataAccess
{
    public class NextgenProductCatalogDAO: BaseDAO
    {
        private static volatile NextgenProductCatalogDAO _instance;
        private static readonly object SyncRoot = new Object();

        private NextgenProductCatalogDAO()
        {
        }

        public static NextgenProductCatalogDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new NextgenProductCatalogDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return AppSettings.CsproductcatalogConnectionstring; }
        }

        public List<BtKeyParentCategoryObject> GetParentCategories(List<BtKeyCatalogObject> listProducts, bool isVirtualCatalog)
        {
            var results = new List<BtKeyParentCategoryObject>();

            if (listProducts == null || listProducts.Count == 0) return results;

            var dataRecords = GetDataRecordsForProductCatalog(listProducts);

            var dbConnection = new SqlConnection(ConnectionString);
            var command = CreateSqlSpCommand("procTS360GetParentCategories", dbConnection);

            //<Parameter>
            var sqlParamaters = CreateSqlParamaters(2);

            sqlParamaters[0] = new SqlParameter("@BTKeyCatalogs", SqlDbType.Structured)
            {
                Direction = ParameterDirection.Input,
                TypeName = "utblBTKeyCatalog",
                Value = dataRecords
            };

            sqlParamaters[1] = new SqlParameter("@IsVirtualCatalog", SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
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

        #region Private
        public static List<SqlDataRecord> GetDataRecordsForProductCatalog(List<BtKeyCatalogObject> listProducts)
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
        #endregion
    }
}
