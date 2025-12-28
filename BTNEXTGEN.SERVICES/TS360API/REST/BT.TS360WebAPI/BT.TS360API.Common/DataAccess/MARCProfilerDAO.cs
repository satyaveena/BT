using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace BT.TS360API.Common.DataAccess
{
    public class MARCProfilerDAO : BaseDAO
    {
        private static readonly object SyncRoot = new Object();

        /// <summary>
        /// MARCProfilerDAO
        /// </summary>
        private MARCProfilerDAO()
        {
        }

        /// <summary>
        /// Instance
        /// </summary>
        public static MARCProfilerDAO Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    MARCProfilerDAO _instance = HttpContext.Current == null ? new MARCProfilerDAO() : HttpContext.Current.Items["MARCProfilerDAO"] as MARCProfilerDAO;
                    if (_instance == null)
                    {
                        _instance = new MARCProfilerDAO();
                        HttpContext.Current.Items.Add("MARCProfilerDAO", _instance);
                    }

                    return _instance;
                }
            }


        }

        /// <summary>
        /// ConnectionString
        /// </summary>
        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["ProductCatalog_ConnectionString"]; }
        }

       public List<MARCRecord> GetMARCRecords(String sortColumn, String basketSummaryID, String sortDirection, String ProfileID, string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, DataTable dtTableMarcInventory)
        {
            try
            {
                List<MARCRecord> resultSet = new List<MARCRecord>();

                using (var dbConnection = new SqlConnection(ConnectionString))
                {
                    using (var command = CreateSqlSpCommandNoErrorMessage(DBStores.procMARCGetBasket, dbConnection))
                    {
                        SqlParameter paramSortColumn = new SqlParameter("@SortColumn", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = sortColumn };
                        command.Parameters.Add(paramSortColumn);

                        SqlParameter paramBasketSummaryID = new SqlParameter("@BasketSummaryID", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = basketSummaryID };
                        command.Parameters.Add(paramBasketSummaryID);

                        SqlParameter paramSortDirection = new SqlParameter("@sortDirection", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = sortDirection };
                        command.Parameters.Add(paramSortDirection);

                        SqlParameter paramProfileID = new SqlParameter("@MARCProfileID", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = ProfileID };
                        command.Parameters.Add(paramProfileID);

                        SqlParameter paramFullIndicator = new SqlParameter("@FullAcquisitionIndicator", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = FullIndicator };
                        command.Parameters.Add(paramFullIndicator);

                        SqlParameter paramIsOrdered = new SqlParameter("@IsOrdered", SqlDbType.Bit) { Direction = ParameterDirection.Input, Value = isOrdered };
                        command.Parameters.Add(paramIsOrdered);

                        SqlParameter paramIsCancelled = new SqlParameter("@IsCancelled", SqlDbType.Bit) { Direction = ParameterDirection.Input, Value = isCancelled };
                        command.Parameters.Add(paramIsCancelled);

                        SqlParameter paramIsOCLCEnabled = new SqlParameter("@OCLCEnabled", SqlDbType.Bit) { Direction = ParameterDirection.Input, Value = isOCLCEnabled };
                        command.Parameters.Add(paramIsOCLCEnabled);

                        SqlParameter paramIsBTEmployee = new SqlParameter("@IsBTEmployee", SqlDbType.Bit) { Direction = ParameterDirection.Input, Value = isBTEmployee };
                        command.Parameters.Add(paramIsBTEmployee);

                        SqlParameter paramdtTableMarcInventory = new SqlParameter("@utblBasketInventoryMarc", SqlDbType.Structured) { Direction = ParameterDirection.Input, Value = dtTableMarcInventory };
                        command.Parameters.Add(paramdtTableMarcInventory);

                        dbConnection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            resultSet = GetMarcRecordsFromDs(reader);
                        }

                        return resultSet;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<MARCRecord> GetMarcRecordsFromDs(SqlDataReader reader)
        {
            var resultSet = new List<MARCRecord>();
            if (reader == null) return resultSet;

            while (reader.Read())
            {
                var resultItem = new MARCRecord
                {
                    BasketLineItemID = reader["BasketLineItemID"].ToString(),
                    Data = reader["Data"].ToString(),
                    Indicator = reader["Indicator"].ToString(),
                    SortSequence = reader["SortSequence"].ToString(),
                    BTKey = reader["BTKey"].ToString(),
                    Tag = reader["Tag"].ToString(),
                    Tag000 = reader["000Tag"].ToString(),
                    Tag914w = reader["914wTag"].ToString(),
                    Tag943j = reader["943jTag"].ToString()
                };

                resultSet.Add(resultItem);
            }

            return resultSet;
        }

        public List<MARCProfile> GetMARCProfiles(string orgId, out bool marcGridSort)
        {
            var resultSet = new List<MARCProfile>();

            marcGridSort = true;

            if (string.IsNullOrEmpty(orgId)) return null;

            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                using (var command = CreateSqlSpCommand(DBStores.procMARCGetProfiles, dbConnection))
                {
                    var paramOrgId = new SqlParameter("@OrgID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = orgId };
                    var paramMarcGridSort = new SqlParameter("@MarcGridSort", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                    //var errorMessage = new SqlParameter("@ErrorMessage", SqlDbType.VarChar, -1) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(paramOrgId);
                    command.Parameters.Add(paramMarcGridSort);
                    //command.Parameters.Add(errorMessage);

                    dbConnection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        resultSet = GetMarcProfilesFromDs(reader);
                    }
                    marcGridSort = (bool)paramMarcGridSort.Value;

                    return resultSet.Count > 0 ? resultSet : null;
                }
            }
        }

        private List<MARCProfile> GetMarcProfilesFromDs(SqlDataReader reader)
        {
            var resultSet = new List<MARCProfile>();
            if (reader == null) return resultSet;

            while (reader.Read())
            {
                var resultItem = new MARCProfile
                {
                    Name = reader["Name"].ToString(),
                    MARCProfileId = reader["MARCProfileID"].ToString(),
                    Description = reader["Description"].ToString(),
                    Sequence = Convert.ToInt32(reader["Sequence"]),
                    UpdatedBy = reader["UpdatedBy"].ToString(),
                    OrgId = reader["u_org_id"].ToString(),
                    UpdatedDateTime = reader["UpdatedDatetime"].ToString(),
                    HasInventoryRules = Convert.ToBoolean(reader["HasInventoryRules"])

                };

                resultSet.Add(resultItem);
            }

            return resultSet;
        }
    }
}
