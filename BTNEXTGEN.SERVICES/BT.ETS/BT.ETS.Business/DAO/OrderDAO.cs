using BT.ETS.Business.Constants;
using BT.ETS.Business.DAO.Interface;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using BT.ETS.Business.Models;
using Microsoft.SqlServer.Server;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.DAO
{
    class OrderDAO : BaseDAO
    {

        private static volatile OrderDAO _instance;
        private static readonly object SyncRoot = new Object();

        public static OrderDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new OrderDAO();
                }

                return _instance;
            }
        }

        #region Public Property
        public override string ConnectionString
        {
            get { return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.Orders_ConnectionString); }
        }
        #endregion

        #region Private
        private static void CreateStructedInputData(List<LineItemInput> inputLines
            , out List<SqlDataRecord> itemInputData, out List<SqlDataRecord> gridInputData, out List<SqlDataRecord> rankingInputData)
        {
            itemInputData = new List<SqlDataRecord>();
            gridInputData = new List<SqlDataRecord>();
            rankingInputData = new List<SqlDataRecord>();

            SqlMetaData[] itemSchema =
            {
                new SqlMetaData("BTKey", SqlDbType.Char, 10), 
                new SqlMetaData("Note", SqlDbType.NVarChar, SqlMetaData.Max)
            };

            SqlMetaData[] gridSchema =
            {
                new SqlMetaData("BTKey", SqlDbType.Char, 10), 
                new SqlMetaData("Quantity", SqlDbType.Int),
                new SqlMetaData("AgencyCodeID", SqlDbType.NVarChar, 50),
                new SqlMetaData("ItemTypeCodeID", SqlDbType.NVarChar, 50),
                new SqlMetaData("CollectionCodeID", SqlDbType.NVarChar, 50),
                new SqlMetaData("CallNumberText", SqlDbType.NVarChar, 50),
                new SqlMetaData("UserCode1ID", SqlDbType.NVarChar, 50),
                new SqlMetaData("UserCode2ID", SqlDbType.NVarChar, 50),
                new SqlMetaData("UserCode3ID", SqlDbType.NVarChar, 50),
                new SqlMetaData("UserCode4ID", SqlDbType.NVarChar, 50),
                new SqlMetaData("UserCode5ID", SqlDbType.NVarChar, 50),
                new SqlMetaData("UserCode6ID", SqlDbType.NVarChar, 50)
            };

            SqlMetaData[] rankingSchema =
            {
                new SqlMetaData("BTKey", SqlDbType.Char, 10), 
                new SqlMetaData("OverallRanking", SqlDbType.Decimal, 18, 1),
                new SqlMetaData("GenreScore", SqlDbType.Decimal, 18, 1),
                new SqlMetaData("GenreScoreDescription", SqlDbType.VarChar, 200),
                new SqlMetaData("DetailURL", SqlDbType.VarChar, 1000),
                new SqlMetaData("DetailHeight", SqlDbType.Int),
                new SqlMetaData("DetailWidth", SqlDbType.Int),
                new SqlMetaData("OverallScoreType", SqlDbType.VarChar, 50)
            };
             
            foreach (var line in inputLines)
            {
                var btkey = line.BTKey;
                if (string.IsNullOrEmpty(btkey))
                    continue;

                var itemRecord = new SqlDataRecord(itemSchema);
                itemRecord.SetString(0, line.BTKey ?? "");
                itemRecord.SetString(1, line.Note ?? "");
                itemInputData.Add(itemRecord);

                if (line.Grids != null)
                {
                    foreach (var grid in line.Grids)
                    {
                        var gridRecord = new SqlDataRecord(gridSchema);
                        gridRecord.SetString(0, line.BTKey ?? "");
                        gridRecord.SetInt32(1, grid.Quantity);
                        gridRecord.SetString(2, grid.AgencyId ?? "");
                        gridRecord.SetString(3, grid.ItemTypeId ?? "");
                        gridRecord.SetString(4, grid.CollectionId ?? "");
                        gridRecord.SetString(5, grid.CallNumberText ?? "");
                        gridRecord.SetString(6, grid.UserCode1Id ?? "");
                        gridRecord.SetString(7, grid.UserCode2Id ?? "");
                        gridRecord.SetString(8, grid.UserCode3Id ?? "");
                        gridRecord.SetString(9, grid.UserCode4Id ?? "");
                        gridRecord.SetString(10, grid.UserCode5Id ?? "");
                        gridRecord.SetString(11, grid.UserCode6Id ?? "");
                        gridInputData.Add(gridRecord);
                    }
                }

                if (line.Ranking != null)
                {
                    var rankingRecord = new SqlDataRecord(rankingSchema);
                    rankingRecord.SetString(0, line.BTKey ?? "");
                    if (!string.IsNullOrEmpty(line.Ranking.Overall))
                        rankingRecord.SetDecimal(1, Convert.ToDecimal(line.Ranking.Overall));
                    if (!string.IsNullOrEmpty(line.Ranking.Genre_Score))
                        rankingRecord.SetDecimal(2, Convert.ToDecimal(line.Ranking.Genre_Score));
                    rankingRecord.SetString(3, line.Ranking.Genre_Score_Description ?? "");
                    rankingRecord.SetString(4, line.Ranking.Detail_URL ?? "");
                    rankingRecord.SetInt32(5, line.Ranking.DetailHeight);
                    rankingRecord.SetInt32(6, line.Ranking.DetailWidth);
                    rankingRecord.SetString(7, line.Ranking.OveralScoreType ?? "");
                    rankingInputData.Add(rankingRecord);
                }
            }
        }

        private static List<SqlDataRecord> CreateBTKeyListStructedData(List<string> inputBtKeys)
        {
            var items = new List<SqlDataRecord>();
            SqlMetaData[] itemSchema =
            {
                new SqlMetaData("BTKey", SqlDbType.Char, 10)
            };

            foreach (var btkey in inputBtKeys)
            {
                if (string.IsNullOrEmpty(btkey))
                    continue;

                var itemRecord = new SqlDataRecord(itemSchema);
                itemRecord.SetString(0, btkey);
                items.Add(itemRecord);
            }

            return items;
        }

        #endregion

        #region Public Methods

        public async Task<DataSet> InsertEtsCart(string espLibraryId, string etsCartId, string cartName, string cartNote, string userId, List<LineItemInput> lines)
        {
            var itemInputData = new List<SqlDataRecord>();
            var gridInputData = new List<SqlDataRecord>();
            var rankingInputData = new List<SqlDataRecord>();

            CreateStructedInputData(lines, out itemInputData, out gridInputData, out rankingInputData);

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.InsertEtsCart, dbConnection);
            var sqlParamaters = CreateSqlParamaters(8);
            sqlParamaters[0] = new SqlParameter("@ESPLibraryID", SqlDbType.NVarChar) { SqlValue = espLibraryId };
            sqlParamaters[1] = new SqlParameter("@ETSCartID", SqlDbType.VarChar) { SqlValue = etsCartId };
            sqlParamaters[2] = new SqlParameter("@CartName", SqlDbType.NVarChar) { SqlValue = cartName };
            sqlParamaters[3] = new SqlParameter("@CartNote", SqlDbType.NVarChar) { SqlValue = cartNote };
            sqlParamaters[4] = new SqlParameter("@UserID", SqlDbType.NVarChar) { SqlValue = userId };
            sqlParamaters[5] = new SqlParameter("@Items", SqlDbType.Structured)
            {
                Direction =
                    ParameterDirection.Input,
                TypeName =
                    "utblETSItems",
                Value = itemInputData
            };

            sqlParamaters[6] = new SqlParameter("@Grids", SqlDbType.Structured)
            {
                Direction =
                    ParameterDirection.Input,
                TypeName =
                    "utblETSGrids",
                Value = gridInputData.Count == 0 ? null : gridInputData
            };

            sqlParamaters[7] = new SqlParameter("@Ranking", SqlDbType.Structured)
            {
                Direction =
                    ParameterDirection.Input,
                TypeName =
                    "utblETSRanking",
                Value = rankingInputData
            };
            command.Parameters.AddRange(sqlParamaters);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            try
            {
                await dbConnection.OpenAsync();
                await Task.Run(() => sqlDa.Fill(ds));
                HandleException(command);
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2)
                {
                    Logger.RaiseException(ex, ExceptionCategory.Order);
                    throw new BusinessException(510);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }

        public async Task<DupCheckDataResult> GetCheckDups(string userId, List<string> btKeyList, string dupCheckC, string dupCheckDownloadCart, string dupCheckH)
        {
            var itemInputData = CreateBTKeyListStructedData(btKeyList);
            var result = new DupCheckDataResult();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.GetDupChecks, dbConnection);
            var sqlParamaters = CreateSqlParamaters(9);
            sqlParamaters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar) { SqlValue = userId };
            sqlParamaters[1] = new SqlParameter("@DupCheckCart", SqlDbType.NVarChar) { SqlValue = dupCheckC };
            sqlParamaters[2] = new SqlParameter("@DupCheckOrder", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            sqlParamaters[3] = new SqlParameter("@DupCheckDownloadCart", SqlDbType.NVarChar) { SqlValue = dupCheckDownloadCart };
            sqlParamaters[4] = new SqlParameter("@DupCheckHolding", SqlDbType.NVarChar) { SqlValue = dupCheckH };
            sqlParamaters[5] = new SqlParameter("@BTKeys", SqlDbType.Structured)
            {
                Direction = ParameterDirection.Input,
                TypeName = "utblBTKeys",
                Value = itemInputData
            };
            sqlParamaters[6] = new SqlParameter("@OrganizationID", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            sqlParamaters[7] = new SqlParameter("@SeriesDupeCheckType", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };
            sqlParamaters[8] = new SqlParameter("@DupCheckPreferenceDownloadCart", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };


            command.Parameters.AddRange(sqlParamaters);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            try
            {
                await dbConnection.OpenAsync();
                await Task.Run(() => sqlDa.Fill(ds));
                HandleException(command);
                result.OrgId = command.Parameters["@OrganizationID"].Value.ToString();
                result.SeriesDupeCheckType = command.Parameters["@SeriesDupeCheckType"].Value.ToString();
                result.OrdersDupeCheckType = command.Parameters["@DupCheckOrder"].Value.ToString();
                result.DupCheckPreferenceDownloadCart = command.Parameters["@DupCheckPreferenceDownloadCart"].Value.ToString();
            }
            catch (SqlException ex)
            {
                if(ex.Number == -2)
                {
                    Logger.RaiseException(ex, ExceptionCategory.Order);
                    throw new BusinessException(510);
                }
                else
                {
                    throw;
                }
            }
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            finally
            {
                dbConnection.Close();
            }
            result.Data = ds;
            return result;
        }

        public async Task<DataSet> GetCheckDupDetails(string userId, string btKey, string dupCheckStatusType, string dupCheckPreference, string dupCheckDownloadCartType)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.GetDupCheckDetails, dbConnection);
            var sqlParamaters = CreateSqlParamaters(5);
            sqlParamaters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar) { SqlValue = userId };
            sqlParamaters[1] = new SqlParameter("@BTKey", SqlDbType.Char, 10) { SqlValue = btKey };
            sqlParamaters[2] = new SqlParameter("@DupCheckStatusType", SqlDbType.Char, 1) { SqlValue = dupCheckStatusType };
            //37969: Override Dup preference
            sqlParamaters[3] = new SqlParameter("@DupCheckPreferences", SqlDbType.NVarChar) { SqlValue = dupCheckPreference };
            sqlParamaters[4] = new SqlParameter("@DupCheckDownloadCartType", SqlDbType.NVarChar) { SqlValue = dupCheckDownloadCartType };

            command.Parameters.AddRange(sqlParamaters);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            try
            {
                await dbConnection.OpenAsync();
                await Task.Run(() => sqlDa.Fill(ds));
                HandleException(command);
                if (ds == null || ds.Tables == null || ds.Tables.Count == 0)
                {
                    var msg = string.Format("{0}: No data returns, userId='{1}', btkey='{2}', dupCheckStatusType='{3}'"
                                                        , StoredProcedureName.GetDupCheckDetails, userId, btKey, dupCheckStatusType);
                    throw new Exception(msg);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2)
                {
                    Logger.RaiseException(ex, ExceptionCategory.Order);
                    throw new BusinessException(510);
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }

        public async Task<DataSet> GetGridTemplatesByUserId(string userId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.GetGridTemplate, dbConnection);
            var sqlParamaters = CreateSqlParamaters(1);
            sqlParamaters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar) { SqlValue = userId };

            command.Parameters.AddRange(sqlParamaters);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            try
            {
                await dbConnection.OpenAsync();
                await Task.Run(() => sqlDa.Fill(ds));
                HandleException(command);
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2)
                {
                    Logger.RaiseException(ex, ExceptionCategory.Order);
                    throw new BusinessException(510);
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnection.Close();
            } 
            return ds;
        }

        public async Task<DataSet> GetProductPricingPreferences(string userId, List<string> btKeyList)
        {
            var itemInputData = CreateBTKeyListStructedData(btKeyList);

            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.GetPricingRequests, dbConnection);
            var sqlParamaters = CreateSqlParamaters(2);
            sqlParamaters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar) { SqlValue = userId };
            sqlParamaters[1] = new SqlParameter("@BTKeys", SqlDbType.Structured)
            {
                Direction = ParameterDirection.Input,
                TypeName = "utblBTKeys",
                Value = itemInputData
            };

            command.Parameters.AddRange(sqlParamaters);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            try
            {
                await dbConnection.OpenAsync();
                await Task.Run(() => sqlDa.Fill(ds));
                HandleException(command);
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2)
                {
                    Logger.RaiseException(ex, ExceptionCategory.Order);
                    throw new BusinessException(510);
                }
                else
                {
                    throw;
                }
            }
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }

        public async Task<bool> ValidateUserAndOrg(string userId, string orgId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.ValidateOrganizationUser, dbConnection);
            var sqlParamaters = CreateSqlParamaters(2);
            sqlParamaters[0] = new SqlParameter("@UserID", SqlDbType.NVarChar) { SqlValue = userId };
            sqlParamaters[1] = new SqlParameter("@OrgID", SqlDbType.NVarChar) { SqlValue = orgId };
       
            command.Parameters.AddRange(sqlParamaters);
          
            try
            {
                await dbConnection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                HandleException(command);
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2)
                {
                    Logger.RaiseException(ex, ExceptionCategory.Order);
                    throw new BusinessException(510);
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConnection.Close();
            }
            return true;
        }

        #endregion
    }
}
