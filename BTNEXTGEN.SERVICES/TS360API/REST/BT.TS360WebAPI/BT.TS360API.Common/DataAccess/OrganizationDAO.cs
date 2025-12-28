using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.TS360API.Cache;
using BT.TS360API.Common.Configrations;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts.Profiles;
using BT.TS360Constants;
using AppSettings = BT.TS360API.Common.Configrations.AppSettings;
using BT.TS360API.ServiceContracts;

namespace BT.TS360API.Common.DataAccess
{
    public class OrganizationDAO: BaseDAO
    {
        private static volatile OrganizationDAO _instance;
        private static readonly object SyncRoot = new Object();

        private const char DEFAULT_ORDER_STATUS = 'A';

        private OrganizationDAO()
        {
        }

        public static OrganizationDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new OrganizationDAO();
                }

                return _instance;
            }
        }

        public override string ConnectionString
        {
            get
            {
                return AppSettings.NextGenProfilesConnectionString;
            }
        }

        /// <summary>
        /// Retrieve Organizations by search key
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public ICollection<Organization> GetOrganizations(string searchKey)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var result = new Collection<Organization>();
                var command = CreateSqlSpCommand(DBStores.CONST_GET_ORGANIZATION_EXTENTION, dbConnection);
                //Create SQL Paremeter
                var sqlParamaters = CreateSqlParamaters(1);
                sqlParamaters[0] = new SqlParameter("@searchKeyword", searchKey);
                command.Parameters.AddRange(sqlParamaters);
                //Open Connection
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //var profileController = ProfileController.Current;
                        while (reader.Read())
                        {
                            var orgStatus = SiteTermHelper.Instance.GetSiteTermName(SiteTermName.OrganizationStatus,
                                DataAccessHelper.ConvertToString(reader["Status"]));
                            var webMarketTypeName = SiteTermHelper.Instance.GetSiteTermName(SiteTermName.MarketType,
                                DataAccessHelper.ConvertToString(reader["WebMarketType"]));

                            var orgObj = new Organization
                            {
                                OrgId = DataAccessHelper.ConvertToString(reader["Id"]),
                                Name = DataAccessHelper.ConvertToString(reader["Name"]),
                                Alias = DataAccessHelper.ConvertToString(reader["Alias"]),
                                BTOrganizationId = DataAccessHelper.ConvertToString(reader["BTOrganizationId"]),
                                Status = orgStatus,
                                WebMarketTypeName = webMarketTypeName,
                                AccountCount = DataAccessHelper.ConvertToInt(reader["AccountCount"]),
                                UsersUseCount = DataAccessHelper.ConvertToInt(reader["UserCount"]),
                                AdminContact = DataAccessHelper.ConvertToString(reader["AdminContact"]),
                                AllowedUsersCount = DataAccessHelper.ConvertToInt(reader["AllowedUsersCount"])
                            };
                            result.Add(orgObj);
                        }
                    }
                }
                return result;
            }
        }

        public List<Organization> GetOrganizations(string searchKeywords, string salesRepID, string userId, string marketTypeValue, int pageIndex, int pageSize, string sortExpression, int sortOrder, out int totalItems, bool isESPOnly)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var result = new List<Organization>();
                var command = CreateSqlSpCommand(DBStores.CONST_GET_ORGANIZATIONS_BY_SEARCH, dbConnection);
                //<Parameter>
                var sqlParamaters = CreateSqlParamaters(9);
                sqlParamaters[0] = new SqlParameter("@marketType", marketTypeValue ?? string.Empty);
                sqlParamaters[1] = new SqlParameter("@searchKey", searchKeywords ?? string.Empty);
                //sqlParamaters[2] = new SqlParameter("@salesRepID", salesRepID ?? string.Empty);
                sqlParamaters[2] = new SqlParameter("@userId", userId ?? string.Empty);
                sqlParamaters[3] = new SqlParameter("@selectedPage", pageIndex);
                sqlParamaters[4] = new SqlParameter("@pageSize", pageSize);
                sqlParamaters[5] = new SqlParameter("@orderBy", sortExpression);
                sqlParamaters[6] = new SqlParameter("@direction", sortOrder);
                sqlParamaters[7] = new SqlParameter("@totalItems", SqlDbType.Int) { Direction = ParameterDirection.Output };
                sqlParamaters[8] = new SqlParameter("@ESPOnly", isESPOnly ? 1 : 0);
                //</Parameter>
                command.Parameters.AddRange(sqlParamaters);
                //Open Connection


                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    //Fetching
                    if (reader.HasRows)
                    {
                        //
                        while (reader.Read())
                        {
                            var orgObj = new Organization
                            {
                                OrgId = DataAccessHelper.ConvertToString(reader["Id"]),
                                Name = DataAccessHelper.ConvertToString(reader["Name"]),
                                Alias = DataAccessHelper.ConvertToString(reader["Alias"]),
                                BTOrganizationId = DataAccessHelper.ConvertToString(reader["BTOrganizationId"]),
                                WebMarketType = DataAccessHelper.ConvertToString(reader["WebMarketType"]),
                                WebMarketTypeName = DataAccessHelper.ConvertToString(reader["WebMarketTypeName"]),
                                AccountCount = DataAccessHelper.ConvertToInt(reader["AccountCount"]),
                                UsersUseCount = DataAccessHelper.ConvertToInt(reader["UsersUseCount"]),
                                AllUsersCount = DataAccessHelper.ConvertToInt(reader["AllUsersCount"]),
                                AllActiveUsersCount = DataAccessHelper.ConvertToInt(reader["ActiveUserCount"]),
                                AdminContact = DataAccessHelper.ConvertToString(reader["AdminContact"]),
                                OrganizationType = DataAccessHelper.ConvertToString(reader["OrganizationType"]),
                                OrganizationTypeName = DataAccessHelper.ConvertToString(reader["OrganizationTypeName"]),
                                IsBTEmployee = DataAccessHelper.ConvertToBool(reader["IsBTEmployee"]),
                                ContactName = DataAccessHelper.ConvertToString(reader["ContactName"]),
                                City = DataAccessHelper.ConvertToString(reader["City"]),
                                State = DataAccessHelper.ConvertToString(reader["State"]),
                                ZipCode = DataAccessHelper.ConvertToString(reader["ZipCode"]),
                                SalesRep = DataAccessHelper.ConvertToString(reader["SalesRepList"]),
                                BTUsersCount = DataAccessHelper.ConvertToInt(reader["BTUsersCount"])
                            };
                            result.Add(orgObj);
                        }
                    }
                }

                dbConnection.Close();

                totalItems = DataAccessHelper.ConvertToInt(command.Parameters["@totalItems"].Value);
                return result;
            }
        }


        //public List<Organization> GetOrganizationsSearchHome(string searchKeywords, string searchBy, string marketTypeValue, int pageIndex, int pageSize, string sortExpression, int sortOrder, out int totalItems)
        //{
        //    using (var dbConnection = CreateSqlConnection())
        //    {
        //        var result = new List<Organization>();
        //        var command = CreateSqlSpCommand(DBStores.CONST_GET_ORGANIZATIONS_BY_SEARCH_HOME, dbConnection);
        //        //<Parameter>
        //        var sqlParamaters = CreateSqlParamaters(8);
        //        sqlParamaters[0] = new SqlParameter("@marketType", marketTypeValue ?? string.Empty);
        //        sqlParamaters[1] = new SqlParameter("@searchKey", searchKeywords ?? string.Empty);
        //        sqlParamaters[2] = new SqlParameter("@searchBy", searchBy ?? SearchByOrgMode.OrgName.ToString());
        //        sqlParamaters[3] = new SqlParameter("@selectedPage", pageIndex);
        //        sqlParamaters[4] = new SqlParameter("@pageSize", pageSize);
        //        sqlParamaters[5] = new SqlParameter("@orderBy", sortExpression);
        //        sqlParamaters[6] = new SqlParameter("@direction", sortOrder);
        //        sqlParamaters[7] = new SqlParameter("@totalItems", SqlDbType.Int) { Direction = ParameterDirection.Output };
        //        //</Parameter>
        //        command.Parameters.AddRange(sqlParamaters);
        //        //Open Connection


        //        dbConnection.Open();
        //        using (var reader = command.ExecuteReader())
        //        {
        //            //Fetching
        //            if (reader.HasRows)
        //            {
        //                //
        //                while (reader.Read())
        //                {
        //                    var orgObj = new Organization
        //                    {
        //                        Id = DataAccessHelper.ConvertToString(reader["Id"]),
        //                        Name = DataAccessHelper.ConvertToString(reader["Name"]),
        //                        Alias = DataAccessHelper.ConvertToString(reader["Alias"]),
        //                        BTOrganizationId = DataAccessHelper.ConvertToString(reader["BTOrganizationId"]),
        //                        WebMarketType = DataAccessHelper.ConvertToString(reader["WebMarketType"]),
        //                        WebMarketTypeName = DataAccessHelper.ConvertToString(reader["WebMarketTypeName"]),
        //                        AccountCount = DataAccessHelper.ConvertToInt(reader["AccountCount"]),
        //                        UsersUseCount = DataAccessHelper.ConvertToInt(reader["UsersUseCount"]),
        //                        AllUsersCount = DataAccessHelper.ConvertToInt(reader["AllUsersCount"]),
        //                        AdminContact = DataAccessHelper.ConvertToString(reader["AdminContact"]),
        //                        OrganizationType = DataAccessHelper.ConvertToString(reader["OrganizationType"]),
        //                        OrganizationTypeName = DataAccessHelper.ConvertToString(reader["OrganizationTypeName"]),
        //                        IsBTEmployee = DataAccessHelper.ConvertToBool(reader["IsBTEmployee"]),
        //                        ContactName = DataAccessHelper.ConvertToString(reader["ContactName"]),
        //                        City = DataAccessHelper.ConvertToString(reader["City"]),
        //                        State = DataAccessHelper.ConvertToString(reader["State"]),
        //                        ZipCode = DataAccessHelper.ConvertToString(reader["ZipCode"]),
        //                        SalesRep = DataAccessHelper.ConvertToString(reader["SalesRepList"]),
        //                        BTUsersCount = DataAccessHelper.ConvertToInt(reader["BTUsersCount"])
        //                    };
        //                    result.Add(orgObj);
        //                }
        //            }
        //        }

        //        dbConnection.Close();

        //        totalItems = DataAccessHelper.ConvertToInt(command.Parameters["@totalItems"].Value);
        //        return result;
        //    }
        //}
        //public ICollection<Organization> GetOrganizations(out int totalItemReturn, string marketTypeValue)
        //{
        //    using (var dbConnection = CreateSqlConnection())
        //    {
        //        var result = new Collection<Organization>();
        //        var command = CreateSqlSpCommand(DBStores.ConstGetAllOrganizations, dbConnection);
        //        var marketType = new SqlParameter("@MarketType", SqlDbType.NVarChar)
        //        {
        //            Value = marketTypeValue,
        //            Direction = ParameterDirection.Input
        //        };
        //        var output = new SqlParameter("@TotalItemReturn", SqlDbType.BigInt)
        //        {
        //            Direction = ParameterDirection.Output
        //        };
        //        command.Parameters.Add(output);
        //        command.Parameters.Add(marketType);
        //        //Open Connection
        //        dbConnection.Open();
        //        using (var reader = command.ExecuteReader())
        //        {
        //            //Fetching
        //            if (reader.HasRows)
        //            {
        //                var profileController = ProfileController.Current;
        //                //
        //                while (reader.Read())
        //                {
        //                    var orgObj = new Organization
        //                    {
        //                        Id = DataAccessHelper.ConvertToString(reader["Id"]),
        //                        Name = DataAccessHelper.ConvertToString(reader["Name"]),
        //                        Alias = DataAccessHelper.ConvertToString(reader["Alias"]),
        //                        BTOrganizationId = DataAccessHelper.ConvertToString(reader["BTOrganizationId"]),
        //                        Status = DataAccessHelper.ConvertToString(reader["Status"]),
        //                        StatusName = profileController.GetSiteTermName(SiteTermName.OrganizationStatus, DataAccessHelper.ConvertToString(reader["Status"])),
        //                        WebMarketType = DataAccessHelper.ConvertToString(reader["WebMarketType"]),
        //                        WebMarketTypeName =
        //                            profileController.GetSiteTermName(SiteTermName.MarketType,
        //                            DataAccessHelper.ConvertToString(reader["WebMarketType"])),
        //                        ERPMarketType = DataAccessHelper.ConvertToString(reader["ERPMarketType"]),
        //                        ERPMarketTypeName = profileController.GetSiteTermName(SiteTermName.MarketType,
        //                            DataAccessHelper.ConvertToString(reader["ERPMarketType"])),
        //                        AccountCount = DataAccessHelper.ConvertToInt(reader["AccountCount"]),
        //                        UsersUseCount = DataAccessHelper.ConvertToInt(reader["UsersUseCount"]),
        //                        AdminContact = DataAccessHelper.ConvertToString(reader["AdminContact"]),
        //                        OrganizationType = DataAccessHelper.ConvertToString(reader["OrganizationType"]),
        //                        OrganizationTypeName = profileController.GetSiteTermName(SiteTermName.OrganizationType, DataAccessHelper.ConvertToString(reader["OrganizationType"])),
        //                        ContactName = DataAccessHelper.ConvertToString(reader["ContactName"]),
        //                    };
        //                    result.Add(orgObj);
        //                }
        //            }
        //        }
        //        totalItemReturn = result.Count;
        //        return result;
        //    }
        //}

        //public List<Organization> SearchForOrganizations(string keyword, string salesRep, string marketType)
        //{
        //    try
        //    {
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            var result = new List<Organization>();
        //            using (var command = CreateSqlSpCommand(DBStores.CONST_SEARCH_FOR_ORGANIZATION, dbConnection))
        //            {
        //                var sqlParamaters = CreateSqlParamaters(7);

        //                sqlParamaters[0] = new SqlParameter("@UserID", DBNull.Value);

        //                sqlParamaters[1] = string.IsNullOrEmpty(keyword)
        //                                       ? new SqlParameter("@keyword", DBNull.Value)
        //                                       : new SqlParameter("@Keyword", keyword);
        //                sqlParamaters[2] = string.IsNullOrEmpty(salesRep)
        //                                       ? new SqlParameter("@SalesRep", DBNull.Value)
        //                                       : new SqlParameter("@SalesRep", salesRep);
        //                sqlParamaters[3] = string.IsNullOrEmpty(marketType) || marketType == "-1"
        //                                       ? new SqlParameter("@MarketType", DBNull.Value)
        //                                       : new SqlParameter("@MarketType", marketType);

        //                sqlParamaters[4] = new SqlParameter("@LoginName", DBNull.Value);
        //                sqlParamaters[5] = new SqlParameter("@AssignedOnly", false);
        //                sqlParamaters[6] = new SqlParameter("@IsCustomerOrgOnly", true);

        //                command.Parameters.AddRange(sqlParamaters);

        //                // Create Data Adapter & Data Set & Fill
        //                var da = new SqlDataAdapter(command);
        //                var ds = new DataSet();

        //                dbConnection.Open();
        //                da.Fill(ds);

        //                // Handle Error
        //                HandleException(command);

        //                // Fetching
        //                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
        //                {
        //                    result.AddRange(from DataRow row in ds.Tables[0].Rows
        //                                    select new Organization
        //                                    {
        //                                        Id = DataAccessHelper.ConvertToString(row["OrgID"]),
        //                                        Name = DataAccessHelper.ConvertToString(row["OrgName"]),
        //                                        Alias = DataAccessHelper.ConvertToString(row["OrgAlias"]),
        //                                        SalesRep = DataAccessHelper.ConvertToString(row["SalesRepList"]),
        //                                        AllUsersCount = DataAccessHelper.ConvertToInt(row["AllUsersCount"]),
        //                                        WebMarketType = DataAccessHelper.ConvertToString(row["MarketType"])
        //                                    });
        //                }
        //            }
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}

        //public List<Organization> SearchForAssignedOrganizations(string userId, string keyword, string salesRep,
        //    string marketType, string loginName, bool isAssignedOnly, bool isCustomerOrgOnly)
        //{
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        throw new ArgumentException("User id cannot be null");
        //    }
        //    try
        //    {

        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            var result = new List<Organization>();

        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_SEARCH_FOR_ORGANIZATION, dbConnection);

        //            var sqlParamaters = CreateSqlParamaters(7);

        //            sqlParamaters[0] = new SqlParameter("@UserID", userId);

        //            sqlParamaters[1] = string.IsNullOrEmpty(keyword) ? new SqlParameter("@Keyword", DBNull.Value) : new SqlParameter("@Keyword", keyword);
        //            sqlParamaters[2] = string.IsNullOrEmpty(salesRep) ? new SqlParameter("@SalesRep", DBNull.Value) : new SqlParameter("@SalesRep", salesRep);
        //            sqlParamaters[3] = string.IsNullOrEmpty(marketType) ? new SqlParameter("@MarketType", DBNull.Value) : new SqlParameter("@LoginName", loginName);
        //            sqlParamaters[4] = string.IsNullOrEmpty(loginName) ? new SqlParameter("@LoginName", DBNull.Value) : new SqlParameter("@LoginName", loginName);

        //            sqlParamaters[5] = new SqlParameter("@AssignedOnly", isAssignedOnly);
        //            sqlParamaters[6] = new SqlParameter("@IsCustomerOrgOnly", isCustomerOrgOnly);

        //            command.Parameters.AddRange(sqlParamaters);

        //            // Create Data Adapter & Data Set & Fill
        //            var da = new SqlDataAdapter(command);
        //            var ds = new DataSet();

        //            dbConnection.Open();
        //            da.Fill(ds);
        //            // Handle Error
        //            HandleException(command);

        //            if (ds.Tables.Count > 0 &&
        //            ds.Tables[0].Rows.Count != 0)
        //            {
        //                result.AddRange(from DataRow row in ds.Tables[0].Rows
        //                                select new Organization
        //                                {
        //                                    Id = DataAccessHelper.ConvertToString(row["OrgID"]),
        //                                    Name = DataAccessHelper.ConvertToString(row["OrgName"]),
        //                                    Alias = DataAccessHelper.ConvertToString(row["OrgAlias"]),
        //                                    SalesRep = DataAccessHelper.ConvertToString(row["SalesRepList"]),
        //                                    AllUsersCount = DataAccessHelper.ConvertToInt(row["AllUsersCount"]),
        //                                    WebMarketType = DataAccessHelper.ConvertToString(row["MarketType"])
        //                                });
        //            }

        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}

        //public Dictionary<string, int> GetNumberOfOrganizations(string[] userIDs)
        //{
        //    try
        //    {
        //        var result = new Dictionary<string, int>();
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            var userIDsTable = DataAccessHelper.GetGUIDTable(userIDs);
        //            //                
        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_GET_NUMBER_OF_ORGS, dbConnection);
        //            //
        //            var sqlParamaters = CreateSqlParamaters(1);
        //            sqlParamaters[0] = new SqlParameter("@UserIDs", SqlDbType.Structured) { Direction = ParameterDirection.Input, Value = userIDsTable };
        //            command.Parameters.AddRange(sqlParamaters);
        //            //
        //            var da = new SqlDataAdapter(command);
        //            var ds = new DataSet();

        //            dbConnection.Open();
        //            da.Fill(ds);

        //            // Handle Error
        //            HandleException(command);

        //            if (ds.Tables.Count > 0 &&
        //            ds.Tables[0].Rows.Count != 0)
        //            {
        //                foreach (DataRow row in ds.Tables[0].Rows.Cast<DataRow>().Where(row => row != null))
        //                {
        //                    var userId = DataAccessHelper.ConvertToString(row["UserID"]);
        //                    if (!result.ContainsKey(userId))
        //                    {
        //                        result.Add(userId, DataAccessHelper.ConvertToInt(row["NumberOfOrg"]));
        //                    }
        //                }
        //            }

        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}


        ///// <summary>
        ///// Get Organization Name By ID
        ///// </summary>
        ///// <param name="orgId"></param>
        ///// <returns></returns>
        //public string GetOrganizationName(string orgId)
        //{
        //    using (var dbConnection = CreateSqlConnection())
        //    {
        //        //
        //        var result = string.Empty;
        //        var command = CreateSqlSpCommand(DBStores.CONST_GET_ORGANIZATION_NAME, dbConnection);
        //        //
        //        var sqlParamater = new SqlParameter("@OrgId", orgId);
        //        command.Parameters.Add(sqlParamater);
        //        //
        //        dbConnection.Open();
        //        using (var reader = command.ExecuteReader())
        //        {
        //            //
        //            if (reader.HasRows && reader.Read())
        //            {
        //                result = DataAccessHelper.ConvertToString(reader["OrganizationName"]);
        //            }
        //        }
        //        return result;
        //    }
        //}

        //public int GetBTUserCount(string orgId)
        //{
        //    try
        //    {
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            //
        //            var result = 0;
        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_GET_BTUSERS_COUNT, dbConnection);
        //            //
        //            var sqlParamater = new SqlParameter("@OrgId", orgId);
        //            command.Parameters.Add(sqlParamater);
        //            //
        //            // Create DataAdapter & DataSet & Fill
        //            var da = new SqlDataAdapter(command);
        //            var ds = new DataSet();

        //            dbConnection.Open();
        //            da.Fill(ds);

        //            // Handle Error
        //            HandleException(command);

        //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
        //            {
        //                foreach (DataRow row in ds.Tables[0].Rows)
        //                {
        //                    result = DataAccessHelper.ConvertToInt(row["BTUsersCount"]);
        //                    break;
        //                }
        //            }
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Get the assigned BT Users of an Organzation
        ///// </summary>
        ///// <param name="organizationID">ID of Organization</param>
        ///// <param name="sortBy"></param>
        ///// <param name="direction"></param>
        ///// <returns>List of users</returns>
        //public List<UserProfileDTO> GetAssignedBTUsers(string organizationID, string sortBy, bool direction)
        //{
        //    try
        //    {
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            //
        //            var result = new List<UserProfileDTO>();
        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_GET_ASSIGNED_BTUSERS, dbConnection);
        //            //
        //            var sqlParamaters = CreateSqlParamaters(3);
        //            sqlParamaters[0] = new SqlParameter("@OrgId", string.IsNullOrEmpty(organizationID) ? string.Empty : organizationID);

        //            var sortByParam = new SqlParameter("@SortBy", SqlDbType.NVarChar);
        //            if (string.IsNullOrEmpty(sortBy))
        //                sortByParam.Value = DBNull.Value;
        //            else
        //                sortByParam.Value = sortBy;

        //            sqlParamaters[1] = sortByParam;

        //            sqlParamaters[2] = new SqlParameter("@Direction", SqlDbType.Bit) { Value = direction };

        //            command.Parameters.AddRange(sqlParamaters);
        //            //
        //            // create data Adapter  & Data Set & Fill
        //            var da = new SqlDataAdapter(command);
        //            var ds = new DataSet();

        //            dbConnection.Open();
        //            da.Fill(ds);

        //            // Handle Error
        //            HandleException(command);

        //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
        //            {
        //                result.AddRange(from DataRow row in ds.Tables[0].Rows
        //                                select new UserProfileDTO
        //                                {
        //                                    UserID = DataAccessHelper.ConvertToString(row["UserID"]),
        //                                    UserAlias = DataAccessHelper.ConvertToString(row["UserAlias"]),
        //                                    UserName = DataAccessHelper.ConvertToString(row["UserName"]),
        //                                    SalesRepName = DataAccessHelper.ConvertToString(row["SalesRepName"]),
        //                                    AssociationType = string.Compare(DataAccessHelper.ConvertToString(row["AssociationType"]), "I", StringComparison.OrdinalIgnoreCase) == 0
        //                                        ? AssociationType.Indirect : AssociationType.Direct
        //                                }
        //                );
        //            }
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}

        //public List<string> GetSalesRepForOrganizaions(string orgID)
        //{
        //    try
        //    {
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            //
        //            var result = new List<string>();
        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_GET_SALES_REP_FOR_ORGANIZATION, dbConnection);
        //            //
        //            var sqlParamater = new SqlParameter("@OrgID", orgID);
        //            command.Parameters.Add(sqlParamater);
        //            //
        //            // Create Data Adapter & Data Set & Fill
        //            var da = new SqlDataAdapter(command);
        //            var ds = new DataSet();

        //            dbConnection.Open();
        //            da.Fill(ds);

        //            // Handle Error
        //            HandleException(command);
        //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
        //            {
        //                result.AddRange(from DataRow row in ds.Tables[0].Rows
        //                                select DataAccessHelper.ConvertToString(row["SalesRepName"]));
        //            }

        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}

        //public void UpdateAssignedBTUsers(string organizationID, string userID, string[] userIDs)
        //{
        //    try
        //    {
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            var userIDsTable = DataAccessHelper.GetGUIDTable(userIDs);
        //            //                
        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_UPDATE_ASSIGNED_BTUSERS, dbConnection);
        //            //
        //            var sqlParamaters = CreateSqlParamaters(3);
        //            sqlParamaters[0] = new SqlParameter("@OrgID", organizationID);
        //            sqlParamaters[1] = new SqlParameter("@UserID", userID);
        //            sqlParamaters[2] = new SqlParameter("@UserIDs", SqlDbType.Structured) { Direction = ParameterDirection.Input, Value = userIDsTable };
        //            command.Parameters.AddRange(sqlParamaters);
        //            //
        //            dbConnection.Open();

        //            command.ExecuteNonQuery();

        //            // Handle Error
        //            HandleException(command);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}

        ////R2.6 search for sales rep used in Assign Sales Rep popup
        //public List<SalesRep> SearchForSalesRep(string keyword)
        //{
        //    try
        //    {
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            //
        //            var result = new List<SalesRep>();

        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_SEARCH_FOR_SALES_REP, dbConnection);
        //            //
        //            var sqlParamater = new SqlParameter("@Keyword", keyword);
        //            command.Parameters.Add(sqlParamater);
        //            //
        //            // create DataAdapter &  Data Set & Fill execute
        //            var ds = new DataSet();
        //            var da = new SqlDataAdapter(command);

        //            dbConnection.Open();
        //            da.Fill(ds);

        //            // Handle Error
        //            HandleException(command);

        //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
        //            {
        //                result.AddRange(from DataRow row in ds.Tables[0].Rows
        //                                select new SalesRep
        //                                {
        //                                    Id = DataAccessHelper.ConvertToString(row["u_id"]),
        //                                    SalesRepId = DataAccessHelper.ConvertToString(row["u_salesrep_id"]),
        //                                    SalesRepName = DataAccessHelper.ConvertToString(row["u_salesrep_name"])
        //                                });
        //            }

        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}


        public OrganizationPremiumServices GetOrganizationPremiumServices(string orgID)
        {
            if (string.IsNullOrEmpty(orgID))
                return new OrganizationPremiumServices();
            //
            var cacheKey = string.Format(CacheKeyConstant.OrganizationPremiumService_CACHE_KEY_PREFIX, orgID);
            var result = CachingController.Instance.Read(cacheKey) as OrganizationPremiumServices;

            if (result != null)
            {
                return result;
            }

            using (var dbConnection = CreateSqlConnection())
            {
                using (var command = CreateSqlSpCommand(DBStores.CONST_GET_ORG_PREMIUM_SERVICES, dbConnection))
                {
                    //
                    var sqlParamater = new SqlParameter("@OrgID", orgID);
                    command.Parameters.Add(sqlParamater);
                    //
                    // Create Data Adapter & Data Set & Fill
                    var da = new SqlDataAdapter(command);
                    var ds = new DataSet();

                    dbConnection.Open();
                    da.Fill(ds);

                    // Handle Error
                    HandleException(command);

                    if (ds.Tables[0].Rows.Count == 0)
                        return new OrganizationPremiumServices();

                    result = GetOrganizationPremiumServices(ds);

                    // write result to cache
                    CachingController.Instance.Write(cacheKey, result);

                    return result;
                }
            }
        }

        public DataSet GetILSGridFields(string orgID)
        {
            DataSet ds = new DataSet();

            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(DBStores.CONST_GET_ILS_GRID_FIELDS, dbConnection);
                //
                var sqlParamater = new SqlParameter("@OrgID", orgID);
                command.Parameters.Add(sqlParamater);
                //
                // Create Data Adapter & Data Set & Fill
                var da = new SqlDataAdapter(command);
                ds = new DataSet();

                dbConnection.Open();
                da.Fill(ds);

                // Handle Error
                HandleException(command);

                if (ds.Tables[0].Rows.Count == 0)
                    return new DataSet();


                return ds;
            }
        }

        private static OrganizationPremiumServices GetOrganizationPremiumServices(DataSet ds)
        {
            if (ds.Tables.Count == 0 && ds.Tables[0].Rows.Count == 0) return new OrganizationPremiumServices();

            var orgInfoRow = ds.Tables[0].Rows[0];
            var result = new OrganizationPremiumServices()
            {
                orgid =
                    orgInfoRow.Table.Columns.Contains("u_org_id")
                        ? DataAccessHelper.ConvertToString(orgInfoRow["u_org_id"])
                        : "",
                espEnabled =
                    orgInfoRow.Table.Columns.Contains("b_esp_enabled") &&
                    DataAccessHelper.ConvertToBool(orgInfoRow["b_esp_enabled"]),
                espRanking =
                    orgInfoRow.Table.Columns.Contains("b_esp_ranking") &&
                    DataAccessHelper.ConvertToBool(orgInfoRow["b_esp_ranking"]),
                espDistribution =
                    orgInfoRow.Table.Columns.Contains("b_esp_distribution") &&
                    DataAccessHelper.ConvertToBool(orgInfoRow["b_esp_distribution"]),
                espFundMonitoring =
                    orgInfoRow.Table.Columns.Contains("b_esp_fund_monitoring") &&
                    DataAccessHelper.ConvertToBool(orgInfoRow["b_esp_fund_monitoring"]),
                espLibraryId =
                    orgInfoRow.Table.Columns.Contains("u_esp_library_id")
                        ? DataAccessHelper.ConvertToString(orgInfoRow["u_esp_library_id"])
                        : "",
                espLibraryName =
                    orgInfoRow.Table.Columns.Contains("ESPLibraryName")
                        ? DataAccessHelper.ConvertToString(orgInfoRow["ESPLibraryName"])
                        : "",
                vipEnabled =
                    orgInfoRow.Table.Columns.Contains("VIPEnabled") &&
                    DataAccessHelper.ConvertToBool(orgInfoRow["VIPEnabled"]),
                ESPCollectionHQLibraryID =
                    orgInfoRow.Table.Columns.Contains("ESPCollectionHQLibraryID")
                        ? DataAccessHelper.ConvertToString(orgInfoRow["ESPCollectionHQLibraryID"])
                        : ""
            };
            return result;
        }

        public async Task<List<PPCSubscription>> GetPPCSubscriptions()
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(DBStores.CONST_GET_ACTIVE_PPC_SUBSCRIPTIONS, dbConnection);

                // Create Data Adapter & Data Set & Fill
                var da = new SqlDataAdapter(command);
                var ds = new DataSet();
                await dbConnection.OpenAsync();
                await Task.Run(() => da.Fill(ds));

                // Handle Error
                HandleException(command);

                if (ds.Tables.Count == 0 && ds.Tables[0].Rows.Count == 0)
                    return null;

                var result = new List<PPCSubscription>();

                result.AddRange(from DataRow row in ds.Tables[0].Rows
                                select new PPCSubscription
                                {
                                    ID = DataAccessHelper.ConvertToString(row["PayPerCirculationID"]),
                                    AuxCode = DataAccessHelper.ConvertToString(row["AuxCode"]),
                                    AuxDescription = DataAccessHelper.ConvertToString(row["AuxDescription"])
                                }
                                );

                return result;
            }
        }

        public async Task<List<string>> GetOrgSelectedPPCSubscriptions(string orgId)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                var command = CreateSqlSpCommand(DBStores.CONST_GET_ORG_PPC_SUBSCRIPTIONS, dbConnection);
                //
                var sqlParamater = new SqlParameter("@OrgID", orgId);
                command.Parameters.Add(sqlParamater);
                //
                // Create Data Adapter & Data Set & Fill
                var da = new SqlDataAdapter(command);
                var ds = new DataSet();
                await dbConnection.OpenAsync();
                await Task.Run(() => da.Fill(ds));

                // Handle Error
                HandleException(command);

                if (ds.Tables.Count == 0 && ds.Tables[0].Rows.Count == 0)
                    return null;

                var result = new List<string>();

                result.AddRange(from DataRow row in ds.Tables[0].Rows
                                select DataAccessHelper.ConvertToString(row["PayPerCirculationID"]));

                return result;
            }
        }

        //public void SetOrganizationPremiumServices(OrganizationPremiumServices orgPremiumServicesStatus)
        //{
        //    try
        //    {
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            //                
        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_SET_ORG_PREMIUM_SERVICES, dbConnection);
        //            //
        //            var sqlParamaters = CreateSqlParamaters(7);
        //            sqlParamaters[0] = new SqlParameter("@OrgID", orgPremiumServicesStatus.orgid);
        //            sqlParamaters[1] = new SqlParameter("@ESPEnabled", orgPremiumServicesStatus.espEnabled);
        //            sqlParamaters[2] = new SqlParameter("@ESPRanking", orgPremiumServicesStatus.espRanking);
        //            if (orgPremiumServicesStatus.espLibraryId == "")
        //                sqlParamaters[3] = new SqlParameter("@ESPLibraryId", DBNull.Value);
        //            else
        //                sqlParamaters[3] = new SqlParameter("@ESPLibraryId", orgPremiumServicesStatus.espLibraryId);
        //            sqlParamaters[4] = new SqlParameter("@ESPDistribution", orgPremiumServicesStatus.espDistribution);
        //            sqlParamaters[5] = new SqlParameter("@ESPFundMonitoring", orgPremiumServicesStatus.espFundMonitoring);
        //            sqlParamaters[6] = new SqlParameter("@VIPEnabled", orgPremiumServicesStatus.vipEnabled);
        //            command.Parameters.AddRange(sqlParamaters);
        //            //
        //            dbConnection.Open();

        //            command.ExecuteNonQuery();

        //            // Handle Error
        //            HandleException(command);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}
        //public List<ESPLibrary> getESPLibraries()
        //{
        //    try
        //    {
        //        var cacheKey = string.Format("__espLibraries");
        //        var ds = VelocityCacheManager.Read(cacheKey, CommonCacheContant.Ts360FarmCacheName) as DataSet;

        //        if (ds != null)
        //        {
        //            return GetEspLibrariesFromDs(ds);
        //        }

        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            //
        //            var result = new List<ESPLibrary>();

        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_GET_ESP_LIBRARIES, dbConnection);
        //            //
        //            ds = new DataSet();
        //            var da = new SqlDataAdapter(command);

        //            dbConnection.Open();
        //            da.Fill(ds);

        //            // Handle Error
        //            HandleException(command);

        //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
        //            {
        //                result = GetEspLibrariesFromDs(ds);
        //                // App Fabric
        //                VelocityCacheManager.Write(cacheKey, ds, CommonCacheContant.Ts360FarmCacheName);
        //            }

        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}

        //private static List<ESPLibrary> GetEspLibrariesFromDs(DataSet ds)
        //{
        //    var result = new List<ESPLibrary>();
        //    result.AddRange(from DataRow row in ds.Tables[0].Rows
        //                    select new ESPLibrary
        //                    {
        //                        ESPLibraryID = DataAccessHelper.ConvertToString(row["ESPLibraryID"]),
        //                        ESPLibraryName = DataAccessHelper.ConvertToString(row["ESPLibraryName"]),
        //                    });
        //    return result;
        //}

        //public void SaveOrganizationPropertyById(string orgid, bool isHideDisabledUsers)
        //{
        //    try
        //    {
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            //                
        //            var command = CreateSqlSpCommandWithErrorHandling("procTS360SaveOrganizationPropertyByID", dbConnection);
        //            //
        //            var sqlParamaters = CreateSqlParamaters(2);
        //            sqlParamaters[0] = new SqlParameter("@OrgID", orgid);
        //            sqlParamaters[1] = new SqlParameter("@HideDisabledUsers", isHideDisabledUsers);

        //            command.Parameters.AddRange(sqlParamaters);
        //            //
        //            dbConnection.Open();

        //            command.ExecuteNonQuery();

        //            // Handle Error
        //            HandleException(command);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}

        //public void SetOrganizationByID(OrganizationGeneralInformation OrgGeneralInformationStatus)
        //{
        //    try
        //    {
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            //                
        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_SET_ORG_GENERAL_INFORMATION, dbConnection);
        //            //
        //            var sqlParamaters = CreateSqlParamaters(3);
        //            sqlParamaters[0] = new SqlParameter("@OrgID", OrgGeneralInformationStatus.orgid);
        //            sqlParamaters[1] = new SqlParameter("@QuotationsEnabled", OrgGeneralInformationStatus.QuotationsEnabled);
        //            sqlParamaters[2] = new SqlParameter("@CIPEnabled", OrgGeneralInformationStatus.CIPEnabled);
        //            command.Parameters.AddRange(sqlParamaters);
        //            //
        //            dbConnection.Open();

        //            command.ExecuteNonQuery();

        //            // Handle Error
        //            HandleException(command);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }
        //}

        //public OrganizationGeneralInformation GetOrganizationByID(string orgID)
        //{
        //    try
        //    {
        //        using (var dbConnection = CreateSqlConnection())
        //        {
        //            //
        //            var command = CreateSqlSpCommandWithErrorHandling(DBStores.CONST_GET_ORG_GENERAL_INFORMATION, dbConnection);
        //            //
        //            var sqlParamater = new SqlParameter("@OrgID", orgID);
        //            command.Parameters.Add(sqlParamater);
        //            //
        //            // Create Data Adapter & Data Set & Fill
        //            var da = new SqlDataAdapter(command);
        //            var ds = new DataSet();

        //            dbConnection.Open();
        //            da.Fill(ds);

        //            // Handle Error
        //            HandleException(command);

        //            if (ds == null || ds.Tables[0].Rows.Count == 0)
        //                return new OrganizationGeneralInformation();
        //            var OrgInfoRow = ds.Tables[0].Rows[0];
        //            OrganizationGeneralInformation result = new OrganizationGeneralInformation()
        //            {
        //                orgid = OrgInfoRow.Table.Columns.Contains("u_org_id") ? DataAccessHelper.ConvertToString(OrgInfoRow["u_org_id"]) : "",
        //                QuotationsEnabled = OrgInfoRow.Table.Columns.Contains("QuotationsEnabled") && DataAccessHelper.ConvertToBool(OrgInfoRow["QuotationsEnabled"]),
        //                CIPEnabled = OrgInfoRow.Table.Columns.Contains("CIPEnabled") && DataAccessHelper.ConvertToBool(OrgInfoRow["CIPEnabled"])

        //            };
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.RaiseException(ex, ExceptionCategory.Admin);
        //        throw;
        //    }

        //}
    }
}
