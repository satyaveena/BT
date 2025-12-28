using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Models;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;

namespace BT.TS360API.Common.DataAccess
{
    public class SavedSearchDAO : BaseDAO
    {
        #region Singleton
        private static volatile SavedSearchDAO _instance;
        private static readonly object SyncRoot = new Object();

        private SavedSearchDAO()
        { // prevent init object outside
        }

        public static SavedSearchDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new SavedSearchDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["NextGenProfilesConnectionString"]; }
        }
        #endregion
        public async Task<Result> Delete(string userId, string savedSearchId)
        {
            var result = new Result();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(DBStores.ProcSavedSearchDelete, dbConnection);
            var sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@UserId", SqlDbType.NVarChar, 50) { Value = userId };
            sqlParameters[1] = new SqlParameter("@SavedSearchId", SqlDbType.NVarChar, 50) { Value = savedSearchId };

            command.Parameters.AddRange(sqlParameters);
            command.CommandTimeout = 600;
            //dbConnection.Open();
            try
            {
                await dbConnection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                //command.ExecuteNonQuery();
                result.ErrorMessage = HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            return result;
        }
        public async Task<Result<string>> Insert(SavedSearchEntity entity)
        {
            var result = new Result<string>();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(DBStores.ProcSavedSearchInsert, dbConnection);
            var sqlParameters = new SqlParameter[7];
            sqlParameters[0] = new SqlParameter("@UserId", SqlDbType.NVarChar, 50) { Value = entity.UserId };
            sqlParameters[1] = new SqlParameter("@SavedSearchId", SqlDbType.NVarChar, 50) { Value = entity.SavedSearchId };
            sqlParameters[2] = new SqlParameter("@SavedSearchName", SqlDbType.NVarChar, 50) { Value = entity.SavedSearchName };
            sqlParameters[3] = new SqlParameter("@Criteria", SqlDbType.NText) { Value = entity.SearchCriteria };
            sqlParameters[4] = new SqlParameter("@DateCreated", SqlDbType.DateTime) { Value = entity.DateCreated };
            sqlParameters[5] = new SqlParameter("@From", SqlDbType.Int) { Value = entity.SearchFrom };
            //Output
            sqlParameters[6] = new SqlParameter("@CurrentId", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };

            command.Parameters.AddRange(sqlParameters);
            command.CommandTimeout = 600;
            //dbConnection.Open();
            try
            {
                await dbConnection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                //command.ExecuteNonQuery();

                result.ErrorMessage = HandleException(command);
                result.Data = DataAccessHelper.ConvertToString(command.Parameters["@CurrentId"].Value);
            }
            finally
            {
                dbConnection.Close();
            }
            return result;
        }
        public async Task<Result> Update(SavedSearchEntity entity)
        {
            var result = new Result();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(DBStores.ProcSavedSearchUpdate, dbConnection);
            var sqlParameters = new SqlParameter[4];
            sqlParameters[0] = new SqlParameter("@SavedSearchId", SqlDbType.NVarChar, 50) { Value = entity.SavedSearchId };
            sqlParameters[1] = new SqlParameter("@Criteria", SqlDbType.NText) { Value = entity.SearchCriteria };
            sqlParameters[2] = new SqlParameter("@DateModified", SqlDbType.DateTime) { Value = entity.DateModified };
            sqlParameters[3] = new SqlParameter("@From", SqlDbType.Int) { Value = entity.SearchFrom };

            command.Parameters.AddRange(sqlParameters);
            command.CommandTimeout = 600;
            //dbConnection.Open();
            try
            {
                await dbConnection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                //command.ExecuteNonQuery();
                result.ErrorMessage = HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            return result;
        }
        //public async Task<ResultSet<List<SavedSearchEntity>>> Get(string userId, string sortBy, bool sortASC, int pageSize, int pageNo)
        //{
        //    var result = new ResultSet<List<SavedSearchEntity>>();
        //    result.Count = 0;
        //    var dbConnection = CreateSqlConnection();
        //    var command = CreateSqlSpCommand(DBStores.ProcSavedSearchGet, dbConnection);
        //    var sqlParameters = new SqlParameter[6];
        //    sqlParameters[0] = new SqlParameter("@UserId", SqlDbType.NVarChar, 50) { Value = userId };
        //    sqlParameters[1] = new SqlParameter("@SortBy", SqlDbType.NVarChar) { Value = sortBy };
        //    sqlParameters[2] = new SqlParameter("@SortOrder", SqlDbType.Bit) { Value = sortASC };
        //    sqlParameters[3] = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
        //    sqlParameters[4] = new SqlParameter("@PageNo", SqlDbType.Int) { Value = pageNo };
        //    //Output
        //    sqlParameters[5] = new SqlParameter("@TotalCount", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };

        //    command.Parameters.AddRange(sqlParameters);
        //    command.CommandTimeout = 600;

        //    var ds = new DataSet();
        //    var da = new SqlDataAdapter(command);

        //    //dbConnection.Open();
        //    try
        //    {
        //        await dbConnection.OpenAsync();
        //        await Task.Run(() => da.Fill(ds));
        //        //da.Fill(ds);
        //        result.ErrorMessage = HandleException(command);
        //    }
        //    finally
        //    {
        //        dbConnection.Close();
        //    }
        //    if (!result.Success) return result;
        //    if (ds.Tables.Count == 0)
        //    {
        //        result.Data = null;
        //        result.ErrorMessage = "Cannot get saved searches.";
        //    }
        //    else
        //    {
        //        result.Data = new List<SavedSearchEntity>();
        //        foreach (DataRow dataRow in ds.Tables[0].Rows)
        //        {
        //            result.Data.Add(new SavedSearchEntity
        //            {
        //                SavedSearchId = DataAccessHelper.ConvertTo<string>(dataRow, "SavedSearchId"),
        //                SavedSearchName = DataAccessHelper.ConvertTo<string>(dataRow, "SavedSearchName"),
        //                SearchCriteria = DataAccessHelper.ConvertTo<string>(dataRow, "Criteria"),
        //                DateModified = DataAccessHelper.ConvertTo<DateTime>(dataRow, "DateModified"),
        //                SearchFrom = DataAccessHelper.ConvertTo<int>(dataRow, "From")
        //            });
        //        }
        //        result.Count = DataAccessHelper.ConvertToInt(command.Parameters["@TotalCount"].Value);
        //    }
        //    return result;
        //}
        
        public async Task<Result<SavedSearchEntity>> GetByID(string id)
        {
            var result = new Result<SavedSearchEntity>();
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(DBStores.ProcSavedSearchGetByID, dbConnection);
            var sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@SavedSearchId", SqlDbType.NVarChar, 50) { Value = id };

            command.Parameters.AddRange(sqlParameters);
            command.CommandTimeout = 600;

            var ds = new DataSet();
            var da = new SqlDataAdapter(command);

            //dbConnection.Open();
            try
            {
                await dbConnection.OpenAsync();
                await Task.Run(() => da.Fill(ds));
                //da.Fill(ds);
                result.ErrorMessage = HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }

            if (!result.Success) return result;
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                result.Data = null;
                result.ErrorMessage = "Cannot get the saved search.";
            }
            else
            {
                var dataRow = ds.Tables[0].Rows[0];
                result.Data = new SavedSearchEntity
                {
                    SavedSearchId = DataAccessHelper.ConvertTo<string>(dataRow, "SavedSearchId")
                    ,
                    SavedSearchName = DataAccessHelper.ConvertTo<string>(dataRow, "SearchName")
                    ,
                    SearchCriteria = DataAccessHelper.ConvertTo<string>(dataRow, "Criteria")
                    ,
                    DateModified = DataAccessHelper.ConvertTo<DateTime>(dataRow, "DateModified")
                    ,
                    SearchFrom = DataAccessHelper.ConvertTo<int>(dataRow, "From")
                };
            }
            return result;
        }
        private string HandleException(SqlCommand command)
        {
            var paramValue = command.Parameters["returnVal"].Value;
            var returnCode = -1;

            if (paramValue != null)
            {
                returnCode = DataAccessHelper.ConvertToInt(paramValue);
            }

            if (returnCode == 0)
                return string.Empty;

            //Error
            paramValue = command.Parameters["@ErrorMessage"].Value;
            return paramValue != null ? paramValue.ToString() : string.Empty;
        }
    }
}
