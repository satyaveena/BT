using System.Collections.ObjectModel;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SqlServer.Server;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360API.ServiceContracts.Search;

namespace BT.TS360API.Common.DataAccess
{
    public class ProductCatalogDAO : BaseDAO
    {
        private static volatile ProductCatalogDAO _instance;
        private static readonly object SyncRoot = new Object();

        private ProductCatalogDAO()
        {
        }

        public static ProductCatalogDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProductCatalogDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["ProductCatalog_ConnectionString"]; }
        }

        public List<string> GetRelatedProductIds(string productId, SortExpression sortExpression = null)
        {
            List<string> btKeys = new List<string>();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage(DBStores.BTNG_GetFamilyKeys, dbConnection);

            //<Parameter>
            if (sortExpression != null)
            {
                var sqlParamaters = CreateSqlParamaters(3);
                sqlParamaters[0] = new SqlParameter("@productId", SqlDbType.NVarChar);
                sqlParamaters[0].Direction = ParameterDirection.Input;
                sqlParamaters[0].Value = productId;
                sqlParamaters[1] = new SqlParameter("@SortBy", SqlDbType.NVarChar);
                sqlParamaters[1].Direction = ParameterDirection.Input;
                sqlParamaters[1].Value = sortExpression.SortField == "ngformatliteral" ? "Format" : "Pubdate";
                sqlParamaters[2] = new SqlParameter("@Direction", SqlDbType.Bit);
                sqlParamaters[2].Direction = ParameterDirection.Input;
                sqlParamaters[2].Value = sortExpression.SortDirection;
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                //            
            }
            else 
            {
                var sqlParamaters = CreateSqlParamaters(1);
                sqlParamaters[0] = new SqlParameter("@productId", SqlDbType.NVarChar);
                sqlParamaters[0].Direction = ParameterDirection.Input;
                //sqlParamaters[0].TypeName = DBCustomTypeName.udtCSVarChar;
                sqlParamaters[0].Value = productId;
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                //            
            }
           
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
                            string btKey = DataAccessHelper.ConvertToString(reader["BTKEY"]);
                            if (!string.IsNullOrEmpty(btKey) && !btKeys.Contains(btKey))
                            {
                                btKeys.Add(btKey);
                            }
                        }
                    }
                }
            }
            finally
            {
                dbConnection.Close();
            }

            return btKeys;
        }

        public async Task<Collection<AdditionalVersion>> GetAdditionalVersions(string btKey, string eMarketType, string eTier)
        {
            var result = new Collection<AdditionalVersion>();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(DBStores.BTNG_GetAdditionalVersions, dbConnection);

            //<Parameter>
            var sqlParamaters = CreateSqlParamaters(3);
            sqlParamaters[0] = new SqlParameter("@BTKey", SqlDbType.Char) { Direction = ParameterDirection.Input, Value = btKey };
            object objEMarketType = DBNull.Value;
            if (!string.IsNullOrEmpty(eMarketType)) objEMarketType = eMarketType;
            sqlParamaters[1] = new SqlParameter("@eMarket", SqlDbType.NVarChar) { Direction = ParameterDirection.Input, Value = objEMarketType };

            object objETier = DBNull.Value;
            if (!string.IsNullOrEmpty(eTier)) objETier = eTier;
            sqlParamaters[2] = new SqlParameter("@eTier", SqlDbType.NVarChar) { Direction = ParameterDirection.Input, Value = objETier };

            //</Parameter>
            command.Parameters.AddRange(sqlParamaters);
            //            
            try
            {
                await dbConnection.OpenAsync();
                command.CommandTimeout = 360;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var eSupplier = DataAccessHelper.ConvertToString(reader["eSupplierLiteral"]);
                            var physicalFormat = DataAccessHelper.ConvertToString(reader["FormatLiteral"]);
                            var formDetail = DataAccessHelper.ConvertToString(reader["FormDetailLiteral"]);
                            var listPrice = DataAccessHelper.ConvertTodecimal(reader["ListPrice"]);
                            var additionalVersion = new AdditionalVersion(eSupplier, physicalFormat, formDetail, CommonHelper.GetCurrencyFormat(listPrice));
                            result.Add(additionalVersion);
                        }
                    }
                }
            }
            finally
            {
                dbConnection.Close();
            }

            //if (result.Count < 1)//EBooks TODO: remove fake data when real data is available
            //{
            //    var additionalVersion1 = new AdditionalVersion("eSupplier1", "physicalFormat1", "formDetail1", 15);
            //    var additionalVersion2 = new AdditionalVersion("eSupplier2", "physicalFormat2", "formDetail2", 16);
            //    var additionalVersion3 = new AdditionalVersion("eSupplier3", "physicalFormat3", "formDetail3", 17);
            //    result.Add(additionalVersion1);
            //    result.Add(additionalVersion2);
            //    result.Add(additionalVersion3);
            //}
            return result;
        }

        public List<BtKeyParentCategoryObject> GetParentCategories(List<BtKeyCatalogObject> listProducts, bool isVirtualCatalog)
        {
            var results = new List<BtKeyParentCategoryObject>();

            if (listProducts == null || listProducts.Count == 0) return results;

            var dataRecords = GetDataRecordsForProductCatalog(listProducts);

            var dbConnection = new SqlConnection(Configrations.AppSettings.CsproductcatalogConnectionstring);
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

        public async Task<bool> UpdateHitCount(string suggestionText)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage(DBStores.BTNG_UpdateQueryTerm, dbConnection);

            //<Parameter>
            var suggestionTextParameter = "@ProductQueryTerm";
            var sqlParamaters = CreateSqlParamaters(1);
            sqlParamaters[0] = new SqlParameter(suggestionTextParameter, SqlDbType.NVarChar);
            sqlParamaters[0].Direction = ParameterDirection.Input;
            sqlParamaters[0].Value = suggestionText;

            //</Parameter>
            command.Parameters.AddRange(sqlParamaters);
            //            
            try
            {
                //dbConnection.Open();
                await dbConnection.OpenAsync();
                command.CommandTimeout = 360;
                //command.ExecuteNonQuery();
                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                dbConnection.Close();
            }

            return true;
        }

        // PRODUCT CATALOG 
        public List<KeyValuePair<string, string>> GetReviewType(IList<string> reviewSourceTexts)
        {
            var result = new List<KeyValuePair<string, string>>();
            var reviewSourceText = string.Join(";", reviewSourceTexts.ToArray());

            using (var dbConnection = new SqlConnection(Configrations.AppSettings.ProductCatalogConnectionString))
            {
                var command = CreateSqlSpCommand(DBStores.GetReviewPublicationType, dbConnection);
                var sqlParamaters = CreateSqlParamaters(1);
                sqlParamaters[0] = new SqlParameter("@ReviewSourceLiteralList", reviewSourceText);
                command.Parameters.AddRange(sqlParamaters);

                var ds = new DataSet();
                var sqlDa = new SqlDataAdapter(command);
                dbConnection.Open();
                sqlDa.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    var reviewSourceDt = ds.Tables[0];
                    if (reviewSourceDt != null && reviewSourceDt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in reviewSourceDt.Rows)
                        {
                            result.Add(new KeyValuePair<string, string>(dr["ReviewSourceType"].ToString(), dr["ReviewSourceCode"].ToString()));
                        }
                    }
                }
            }

            return result;
        }
    }
}
