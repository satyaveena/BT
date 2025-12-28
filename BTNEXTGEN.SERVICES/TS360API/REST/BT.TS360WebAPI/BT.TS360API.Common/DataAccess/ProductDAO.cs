using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Common.Configrations;
using BT.TS360API.Common.Constants;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using Microsoft.SqlServer.Server;
using BT.TS360API.ServiceContracts;
using AppSettings = BT.TS360API.Common.Configrations.AppSettings;

namespace BT.TS360API.Common.DataAccess
{
    public class ProductDAO: BaseDAO
    {
        private static volatile ProductDAO _instance;
        private static readonly object SyncRoot = new Object();

        private ProductDAO()
        {
        }

        public static ProductDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProductDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return AppSettings.ProductCatalogConnectionString; }
        }
        public async Task<Dictionary<string, bool>> CheckFamilyKeysAsync(List<string> btKeys)
        {
            var result = new Dictionary<string, bool>();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommandNoErrorMessage(DBStores.CONST_CHECK_FAMILY_KEY, dbConnection);
            var btKeysTable = DataAccessHelper.GenerateDataRecords(btKeys, "GUID", 50);

            var sqlParamaters = CreateSqlParamaters(1);
            sqlParamaters[0] = new SqlParameter("@BTKeys", SqlDbType.Structured) { Direction = ParameterDirection.Input, Value = btKeysTable };
            command.Parameters.AddRange(sqlParamaters);

            try
            {
                await dbConnection.OpenAsync();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var btKey = DataAccessHelper.ConvertToString(reader["BTKEY"]);
                            if (!result.ContainsKey(btKey))
                                result.Add(btKey, DataAccessHelper.ConvertToBool(reader["HasFamilyKeys"]));
                        }
                    }
                }
            }
            finally
            {
                dbConnection.Close();
            }
            return result;
        }

        public Dictionary<string, bool> CheckProductReviewsFromODS(string[] listProductId, string[] reviewSubdIds)
        {
            if (listProductId == null || reviewSubdIds == null || reviewSubdIds.Length == 0 || listProductId.Length == 0)
            {
                return new Dictionary<string, bool>();
            }
            var results = new Dictionary<string, bool>();
            var btKeyRecords = GetBtKeyDataRecords(listProductId);
            var dataRecords = GetReviewDataRecords(reviewSubdIds);
            //
            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.CheckProductReviews, dbConnection);
                var sqlParamaters = CreateSqlParamaters(3);
                sqlParamaters[0] = new SqlParameter("@BTKeys", SqlDbType.Structured);
                sqlParamaters[0].Direction = ParameterDirection.Input;
                sqlParamaters[0].TypeName = DBCustomTypeName.udtBTKeys;
                sqlParamaters[0].Value = btKeyRecords;
                sqlParamaters[1] = new SqlParameter("@SubsIds", SqlDbType.Structured);
                sqlParamaters[1].Direction = ParameterDirection.Input;
                sqlParamaters[1].TypeName = DBCustomTypeName.udtCSReviewTypes;
                sqlParamaters[1].Value = dataRecords;
                sqlParamaters[2] = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1);
                sqlParamaters[2].Direction = ParameterDirection.Output;
                command.Parameters.AddRange(sqlParamaters);

                dbConnection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string btKey = DataAccessHelper.ConvertToString(reader["BTKey"]);
                            if (!results.ContainsKey(btKey))
                            {
                                bool hasReview = DataAccessHelper.ConvertToBool(reader["HasReview"]);
                                results.Add(btKey, hasReview);
                            }
                        }
                    }
                }
            }
            return results;
        }

        public DataSet GetInventory(string skuList, string warehouseId, string checkLEReserve,
            string accountInventoryType, string inventoryReserveNumber, out int last30DaysDemand)
        {
            var tvpWhsIds = ConvertToTvpParamForWarehouse(warehouseId);
            var tvpSkus = ConvertToTvpParamForSkus(skuList, checkLEReserve, accountInventoryType, inventoryReserveNumber);

            return GetInventoryData(tvpSkus, tvpWhsIds, out last30DaysDemand);
        }

        private DataSet GetInventoryData(List<SqlDataRecord> tvpSkus, List<SqlDataRecord> tvpWhsIds, out int last30DaysDemand)
        {
            last30DaysDemand = 0;
            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.GetInventory, dbConnection);
                var sqlParameters = new SqlParameter[3];

                if (tvpSkus == null || tvpSkus.Count == 0)
                {
                    sqlParameters[0] = new SqlParameter("@Skus", SqlDbType.Structured);
                }
                else
                {
                    sqlParameters[0] = new SqlParameter("@Skus", SqlDbType.Structured) { Value = tvpSkus };
                }

                if (tvpWhsIds == null || tvpWhsIds.Count == 0)
                {
                    sqlParameters[1] = new SqlParameter("@WarehouseId", SqlDbType.Structured);
                }
                else
                {
                    sqlParameters[1] = new SqlParameter("@WarehouseId", SqlDbType.Structured) { Value = tvpWhsIds };
                }

                sqlParameters[2] = new SqlParameter("@Last30DaysDemand", SqlDbType.Int);
                sqlParameters[2].Direction = ParameterDirection.Output;

                command.Parameters.AddRange(sqlParameters);
                dbConnection.Open();

                var dataSet = new DataSet();
                var sqlDataAdapter = new SqlDataAdapter(command);
                sqlDataAdapter.Fill(dataSet);

                if (sqlParameters[2].Value == null) return dataSet;

                if (!int.TryParse(sqlParameters[2].Value.ToString(), out last30DaysDemand))
                {
                    last30DaysDemand = 0;
                }

                return dataSet;
            }
        }

        public string GetProductExcerpts(string productId)
        {
            string excerptsString = string.Empty;

            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.GetProductExcerpts, dbConnection);
                var sqlParamater = new SqlParameter("@BTKey", SqlDbType.Char, 10);
                sqlParamater.Direction = ParameterDirection.Input;
                sqlParamater.Value = productId;
                command.Parameters.Add(sqlParamater);
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        excerptsString = reader.GetString(0) ?? string.Empty;
                    }
                }
            }
            return excerptsString;
        }
        /// <summary>
        /// Gets first annotation only.
        /// </summary>
        /// <param name="btkey"></param>
        /// <returns></returns>
        public async Task<string> GetFirstProductAnnotation(string btkey)
        {
            var firstAnno = string.Empty;
            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.GetProductAnnos, dbConnection);
                var sqlParamater = new SqlParameter("@BTKey", SqlDbType.Char, 10);
                sqlParamater.Direction = ParameterDirection.Input;
                sqlParamater.Value = btkey;
                command.Parameters.Add(sqlParamater);
                
                await dbConnection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        firstAnno = DataAccessHelper.ConvertToString(reader[1]);
                    }
                }
            }

            return firstAnno;
        }
        public async Task<ProductDetailsObject> GetProductInformation(string btKey)
        {
            var result = new ProductDetailsObject();

            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.GetProductInformation, dbConnection);
                command.CommandTimeout = 240;//fix timeout defect.

                var sqlParamater = new SqlParameter("@BTKey", SqlDbType.Char, 10) { Direction = ParameterDirection.Input, Value = btKey };
                command.Parameters.Add(sqlParamater);

                await dbConnection.OpenAsync();
                var dataSet = new DataSet();
                var sqlDataAdapter = new SqlDataAdapter(command);
                //sqlDataAdapter.Fill(dataSet);
                await Task.Run(() => sqlDataAdapter.Fill(dataSet));
                if (dataSet != null && dataSet.Tables.Count > 14)
                {
                    // get general info
                    DataTable generalDT = dataSet.Tables[0];
                    if (generalDT != null && generalDT.Rows.Count > 0)
                    {
                        result.GeneralInfo = new Dictionary<string, string>();
                        foreach (DataColumn dr in generalDT.Columns)
                        {
                            string fieldName = dr.ColumnName ?? string.Empty;
                            string fieldValue = DataAccessHelper.ConvertToString(generalDT.Rows[0][fieldName]);

                            if (fieldValue != string.Empty)
                            {
                                if (!result.GeneralInfo.ContainsKey(fieldName))
                                {
                                    if (fieldName.Equals("PPCPrice"))
                                        fieldValue = CommonHelper.FormatPrice(Decimal.Parse(fieldValue));
                                    result.GeneralInfo.Add(fieldName, fieldValue);
                                }
                            }
                        }
                    }
                    // Get attributes
                    DataTable attsDT = dataSet.Tables[1];
                    if (attsDT != null && attsDT.Rows.Count > 0)
                    {
                        result.Attributes = new List<string>();
                        foreach (DataRow dr in attsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.Attributes.Add(value);
                        }
                    }

                    // Get Platforms
                    DataTable platformsDT = dataSet.Tables[2];
                    if (platformsDT != null && platformsDT.Rows.Count > 0)
                    {
                        result.Platforms = new List<string>();
                        foreach (DataRow dr in platformsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.Platforms.Add(value);
                        }
                    }
                    // Get Versions
                    DataTable versionsDT = dataSet.Tables[3];
                    if (versionsDT != null && versionsDT.Rows.Count > 0)
                    {
                        result.Versions = new List<string>();
                        foreach (DataRow dr in versionsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.Versions.Add(value);
                        }
                    }
                    // Get region
                    DataTable regionDT = dataSet.Tables[4];
                    if (regionDT != null && regionDT.Rows.Count > 0)
                    {
                        result.Regions = new List<string>();
                        foreach (DataRow dr in regionDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.Regions.Add(value);
                        }
                    }

                    // Get SoundTypes
                    DataTable soundTypeDT = dataSet.Tables[5];
                    if (soundTypeDT != null && soundTypeDT.Rows.Count > 0)
                    {
                        result.SoundTypes = new List<string>();
                        foreach (DataRow dr in soundTypeDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.SoundTypes.Add(value);
                        }
                    }

                    // Get BISACSubjects
                    DataTable bisacSubjectsDT = dataSet.Tables[6];
                    if (bisacSubjectsDT != null && bisacSubjectsDT.Rows.Count > 0)
                    {
                        result.BISACSubjects = new List<string>();
                        foreach (DataRow dr in bisacSubjectsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.BISACSubjects.Add(value);
                        }
                    }

                    // Get LibrarySubjects
                    DataTable librarySubjectsDT = dataSet.Tables[7];
                    if (librarySubjectsDT != null && librarySubjectsDT.Rows.Count > 0)
                    {
                        result.LibrarySubjects = new List<string>();
                        foreach (DataRow dr in librarySubjectsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.LibrarySubjects.Add(value);
                        }
                    }

                    // Get academicSubjects
                    DataTable academicSubjectsDT = dataSet.Tables[8];
                    if (academicSubjectsDT != null && academicSubjectsDT.Rows.Count > 0)
                    {
                        result.AcademicSubjects = new List<string>();
                        foreach (DataRow dr in academicSubjectsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.AcademicSubjects.Add(value);
                        }
                    }

                    // Get ContinuationsSeries
                    DataTable continuationsSeriesDT = dataSet.Tables[9];
                    if (continuationsSeriesDT != null && continuationsSeriesDT.Rows.Count > 0)
                    {
                        result.ContinuationsSeries = new Dictionary<string, string>();
                        foreach (DataRow dr in continuationsSeriesDT.Rows)
                        {
                            string seriesId = DataAccessHelper.ConvertToString(dr[0]);
                            string seriesTitle = DataAccessHelper.ConvertToString(dr[1]);

                            if (seriesId != string.Empty && seriesTitle != string.Empty
                                && !result.ContinuationsSeries.ContainsKey(seriesId))
                                result.ContinuationsSeries.Add(seriesId, seriesTitle);
                        }
                    }

                    // Get Awards
                    DataTable AwardsDT = dataSet.Tables[10];
                    if (AwardsDT != null && AwardsDT.Rows.Count > 0)
                    {
                        result.Awards = new List<string>();
                        foreach (DataRow dr in AwardsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.Awards.Add(value);
                        }
                    }

                    // Get Bibliographies
                    DataTable bibliographiesDT = dataSet.Tables[11];
                    if (bibliographiesDT != null && bibliographiesDT.Rows.Count > 0)
                    {
                        result.Bibliographies = new List<string>();
                        foreach (DataRow dr in bibliographiesDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.Bibliographies.Add(value);
                        }
                    }

                    // Get ReviewCitations
                    DataTable reviewCitationsDT = dataSet.Tables[12];
                    if (reviewCitationsDT != null && reviewCitationsDT.Rows.Count > 0)
                    {
                        result.ReviewCitations = new List<ReviewCitationObject>();
                        foreach (DataRow dr in reviewCitationsDT.Rows)
                        {
                            string mReviewCitation = DataAccessHelper.ConvertToString(dr[0]);
                            string mReviewId = DataAccessHelper.ConvertToString(dr["ReviewPublication"]);
                            if (mReviewCitation != string.Empty && mReviewId != string.Empty)
                                result.ReviewCitations.Add(
                                    new ReviewCitationObject
                                    {
                                        ReviewCitation = mReviewCitation,
                                        ReviewId = mReviewId
                                    }
                                    );
                            //ConvertToString(dr[0])
                        }
                    }

                    // Get OtherCitations
                    DataTable otherCitationsDT = dataSet.Tables[13];
                    if (otherCitationsDT != null && otherCitationsDT.Rows.Count > 0)
                    {
                        result.OtherCitations = new List<string>();
                        foreach (DataRow dr in otherCitationsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.OtherCitations.Add(value);
                        }
                    }

                    // Get AcceleratedTopics
                    DataTable acceleratedTopicsDT = dataSet.Tables[14];
                    if (acceleratedTopicsDT != null && acceleratedTopicsDT.Rows.Count > 0)
                    {
                        result.AcceleratedTopics = new List<string>();
                        foreach (DataRow dr in acceleratedTopicsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.AcceleratedTopics.Add(value);
                        }
                    }

                    // Get BT Programs
                    DataTable btProgramsDT = dataSet.Tables[15];
                    if (btProgramsDT != null && btProgramsDT.Rows.Count > 0)
                    {
                        result.BTPrograms = new List<string>();
                        foreach (DataRow dr in btProgramsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.BTPrograms.Add(value);
                        }
                    }

                    // Get BT Programs
                    DataTable btPublicationsDT = dataSet.Tables[16];
                    if (btPublicationsDT != null && btPublicationsDT.Rows.Count > 0)
                    {
                        result.BTPublications = new List<string>();
                        foreach (DataRow dr in btPublicationsDT.Rows)
                        {
                            string value = DataAccessHelper.ConvertToString(dr[0]);
                            if (value != string.Empty)
                                result.BTPublications.Add(value);
                        }
                    }

                    // Get Pay Per Circulation Collections
                    DataTable ppcCircDT = dataSet.Tables[17];
                    if (ppcCircDT != null && ppcCircDT.Rows.Count > 0)
                    {
                        result.PayPerCirCollections = new List<PPCSubscription>();
                        foreach (DataRow dr in ppcCircDT.Rows)
                        {
                            string auxCode = DataAccessHelper.ConvertToString(dr[0]);
                            string auxDescription = DataAccessHelper.ConvertToString(dr["AuxDescription"]);
                            if (auxCode != string.Empty && auxDescription != string.Empty)
                                result.PayPerCirCollections.Add(
                                    new PPCSubscription
                                    {
                                        AuxCode = auxCode,
                                        AuxDescription = auxDescription
                                    }
                                );
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Get reviews of Product from ODS
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="reviewSubdIds">format: 1,2,3,4</param>
        /// <returns></returns>
        public List<AdditionContent> GetProductReviews(string productId, string[] reviewSubdIds)
        {
            if (reviewSubdIds == null || reviewSubdIds.Length == 0)
            {
                return new List<AdditionContent>();
            }
            var lstReviews = new List<AdditionContent>();
            //
            var dataRecords = GetReviewDataRecords(reviewSubdIds);
            //
            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.GetProductReviews, dbConnection);
                var sqlParamaters = CreateSqlParamaters(2);
                sqlParamaters[0] = new SqlParameter("@BTKey", SqlDbType.Char, 10) { Direction = ParameterDirection.Input, Value = productId };
                sqlParamaters[1] = new SqlParameter("@ReviewTypes", SqlDbType.Structured)
                {
                    Direction = ParameterDirection.Input,
                    TypeName = DBCustomTypeName.udtCSReviewTypes,
                    Value = dataRecords
                };
                command.Parameters.AddRange(sqlParamaters);
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var reviewContent = new AdditionContent
                        {
                            ReviewTypeId =
                                DataAccessHelper.ConvertToString(reader["ReviewPublicationCode"]),
                            Title = DataAccessHelper.ConvertToString(reader[3]),
                            Content = DataAccessHelper.ConvertToString(reader[4])
                        };
                        lstReviews.Add(reviewContent);
                    }
                }
            }

            return lstReviews;
        }

        public List<AdditionContent> GetProductAnnos(string productId)
        {
            var listContent = new List<AdditionContent>();
            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.GetProductAnnos, dbConnection);
                var sqlParamater = new SqlParameter("@BTKey", SqlDbType.Char, 10);
                sqlParamater.Direction = ParameterDirection.Input;
                sqlParamater.Value = productId;
                command.Parameters.Add(sqlParamater);
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contentObject = new AdditionContent();
                        contentObject.Title = DataAccessHelper.ConvertToString(reader[0]);
                        contentObject.Content = DataAccessHelper.ConvertToString(reader[1]);
                        listContent.Add(contentObject);
                    }
                }
            }

            return listContent;
        }

        public List<AdditionContent> GetProductFlapCopy(string productId)
        {
            var listContent = new List<AdditionContent>();
            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.GetProductFlapCopy, dbConnection);
                var sqlParamater = new SqlParameter("@BTKey", SqlDbType.Char, 10);
                sqlParamater.Direction = ParameterDirection.Input;
                sqlParamater.Value = productId;
                command.Parameters.Add(sqlParamater);
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var flapObject = new AdditionContent { Title = DataAccessHelper.ConvertToString(reader[0]), Content = DataAccessHelper.ConvertToString(reader[1]) };
                        listContent.Add(flapObject);
                    }
                }
            }

            return listContent;
        }

        public List<AdditionContent> GetProductBiographies(string productId)
        {
            var listContent = new List<AdditionContent>();
            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                var command = CreateSqlSpCommandNoErrorMessage(DBStores.GetProductBiographies, dbConnection);
                var sqlParamater = new SqlParameter("@BTKey", SqlDbType.Char, 10);
                sqlParamater.Direction = ParameterDirection.Input;
                sqlParamater.Value = productId;
                command.Parameters.Add(sqlParamater);
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contentObject = new AdditionContent { Title = DataAccessHelper.ConvertToString(reader[0]), Content = DataAccessHelper.ConvertToString(reader[1]) };
                        listContent.Add(contentObject);
                    }
                }
            }

            return listContent;
        }

        //public async Task<Dictionary<string, ProductContent>> CheckProductContentFromODS(string[] listProductId, string[] reviewTypeIDs)
        //{
        //    if (listProductId == null || listProductId.Length == 0 || reviewTypeIDs == null || reviewTypeIDs.Length == 0)
        //    {
        //        return new Dictionary<string, ProductContent>();
        //    }
        //    var results = new Dictionary<string, ProductContent>();
        //    var dataRecords = GetReviewDataRecords(reviewTypeIDs);
        //    //
        //    using (var dbConnection = new SqlConnection(ConnectionString))
        //    {
        //        var command = CreateSqlSpCommand(DBStores.CheckProductContent, dbConnection);
        //        //
        //        var sqlParamaters = CreateSqlParamaters(8);
        //        sqlParamaters[0] = new SqlParameter("@BTKey", SqlDbType.NVarChar, 10);
        //        sqlParamaters[0].Direction = ParameterDirection.Input;

        //        sqlParamaters[1] = new SqlParameter("@HasAnnotation", SqlDbType.Bit);
        //        sqlParamaters[1].Direction = ParameterDirection.Output;

        //        sqlParamaters[2] = new SqlParameter("@HasExcerpts", SqlDbType.Bit);
        //        sqlParamaters[2].Direction = ParameterDirection.Output;

        //        sqlParamaters[3] = new SqlParameter("@HasReviews", SqlDbType.Bit);
        //        sqlParamaters[3].Direction = ParameterDirection.Output;

        //        sqlParamaters[4] = new SqlParameter("@HasTOC", SqlDbType.Bit);
        //        sqlParamaters[4].Direction = ParameterDirection.Output;

        //        sqlParamaters[5] = new SqlParameter("@HasReturnKey", SqlDbType.Bit);
        //        sqlParamaters[5].Direction = ParameterDirection.Output;

        //        sqlParamaters[6] = new SqlParameter("@ErrorString", SqlDbType.NVarChar, 255);
        //        sqlParamaters[6].Direction = ParameterDirection.Output;

        //        sqlParamaters[7] = new SqlParameter("@SubsIds", SqlDbType.Structured);
        //        sqlParamaters[7].Direction = ParameterDirection.Input;
        //        sqlParamaters[7].TypeName = DBCustomTypeName.udtCSReviewTypes;
        //        sqlParamaters[7].Value = dataRecords;
        //        command.Parameters.AddRange(sqlParamaters);
        //        //
        //        await dbConnection.OpenAsync();

        //        bool prepareFlag = true;
        //        foreach (string productId in listProductId)
        //        {
        //            sqlParamaters[0].Value = productId;
        //            if (prepareFlag)
        //            {
        //                command.Prepare();
        //                prepareFlag = false;
        //            }
        //            await command.ExecuteNonQueryAsync();
        //            // Check in case we don't have Product for this BTKey
        //            if (sqlParamaters[6].Value.ToString() != string.Empty)
        //            {
        //                throw new Exception(); // will define Exception later
        //            }

        //            if (!results.ContainsKey(productId))
        //            {
        //                // retrieve output value
        //                var productContent = new ProductContent
        //                {
        //                    HasAnnotation = bool.Parse(sqlParamaters[1].Value.ToString()),
        //                    HasExcerpts = bool.Parse(sqlParamaters[2].Value.ToString()),
        //                    HasReviews = bool.Parse(sqlParamaters[3].Value.ToString()),
        //                    HasTOC = bool.Parse(sqlParamaters[4].Value.ToString()),
        //                    HasReturnKey = bool.Parse(sqlParamaters[5].Value.ToString())
        //                };
        //                results.Add(productId, productContent);
        //            }
        //        }
        //    }
        //    return results;
        //}

        #region private

        private List<SqlDataRecord> ConvertToTvpParamForWarehouse(string warehouseIds)
        {
            if (string.IsNullOrEmpty(warehouseIds)) return null;

            var whsIds = warehouseIds.Split(';');
            var listWhsId = whsIds.Where(whsId => !string.IsNullOrEmpty(whsId)).ToList();
            return ConvertToTvpParamForWarehouse(listWhsId);
        }

        private List<SqlDataRecord> ConvertToTvpParamForWarehouse(List<string> warehouseIds)
        {
            if (warehouseIds == null || warehouseIds.Count == 0) return null;

            var dataRecords = new List<SqlDataRecord>();
            var sqlMetaDatas = new SqlMetaData[1];

            foreach (var whsId in warehouseIds)
            {
                if (string.IsNullOrEmpty(whsId)) continue;

                sqlMetaDatas[0] = new SqlMetaData("WarehouseID", SqlDbType.VarChar, 50);
                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                dataRecord.SetString(0, whsId.Trim());
                dataRecords.Add(dataRecord);
            }
            return dataRecords;
        }

        private List<SqlDataRecord> ConvertToTvpParamForSkus(string skus, string leIndicator, string accountInventoryType,
            string inventoryReserveNumber)
        {
            if (string.IsNullOrEmpty(skus)) return null;

            var skuItems = skus.Split(',');
            var listSkus = (from skuItem in skuItems
                            where skuItem.Length == 10
                            select string.Format("{0}@{1}@{2}@{3}", skuItem, leIndicator, accountInventoryType, inventoryReserveNumber)).ToList();

            return listSkus.Count == 0 ? null : ConvertToTvpParamForSkus(listSkus);
        }

        private List<SqlDataRecord> ConvertToTvpParamForSkus(List<string> Skus)
        {
            if (Skus == null || Skus.Count == 0) return null;

            var dataRecords = new List<SqlDataRecord>();

            var pagePosition = 1;
            foreach (var sku in Skus)
            {
                var sqlMetaDatas = new SqlMetaData[5];

                var skuItems = sku.Split('@');
                if (skuItems.Length == 4)
                {
                    if (skuItems[0].Length == 10)
                    {
                        sqlMetaDatas[0] = new SqlMetaData("PagePosition", SqlDbType.Int);
                        sqlMetaDatas[1] = new SqlMetaData("BTKey", SqlDbType.Char, 10);
                        sqlMetaDatas[2] = new SqlMetaData("LEIndicator", SqlDbType.Char, 1);
                        sqlMetaDatas[3] = new SqlMetaData("AccountInventoryType", SqlDbType.VarChar, 50);
                        sqlMetaDatas[4] = new SqlMetaData("InventoryReserveNumber", SqlDbType.VarChar, 50);

                        var dataRecord = new SqlDataRecord(sqlMetaDatas);
                        dataRecord.SetInt32(0, pagePosition);
                        dataRecord.SetString(1, skuItems[0]);
                        dataRecord.SetString(2, skuItems[1]);
                        dataRecord.SetString(3, skuItems[2]);
                        dataRecord.SetString(4, skuItems[3]);

                        dataRecords.Add(dataRecord);
                        pagePosition += 1;
                    }
                }
            }
            return dataRecords;
        }

        private static List<SqlDataRecord> GetBtKeyDataRecords(IEnumerable<string> input)
        {
            var dataRecords = new List<SqlDataRecord>();
            SqlMetaData[] sqlMetaDatas = { new SqlMetaData("BTKey", SqlDbType.Char, 10) };
            foreach (var value in input)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                dataRecord.SetString(0, value);
                dataRecords.Add(dataRecord);
            }
            return dataRecords;
        }

        public static List<SqlDataRecord> GetReviewDataRecords(string[] reviewTypeIDs, string colName)
        {
            var dataRecords = new List<SqlDataRecord>();
            SqlMetaData[] sqlMetaDatas = { new SqlMetaData(colName, SqlDbType.NVarChar, -1) };
            foreach (string id in reviewTypeIDs)
            {
                var dataRecord = new SqlDataRecord(sqlMetaDatas);
                dataRecord.SetString(0, id);
                dataRecords.Add(dataRecord);
            }
            return dataRecords;
        }

        public static List<SqlDataRecord> GetReviewDataRecords(string[] reviewTypeIDs)
        {
            return GetReviewDataRecords(reviewTypeIDs, "[ReviewTypes]");
        }
        #endregion
    }
}
