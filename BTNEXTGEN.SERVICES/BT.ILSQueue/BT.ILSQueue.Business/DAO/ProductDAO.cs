using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BT.ILSQueue.Business.Models;
using BT.ILSQueue.Business.Helpers;
using BT.ILSQueue.Business.Constants;

namespace BT.ILSQueue.Business.DAO
{
    public class ProductDAO : BaseDAO
    {

        private static volatile ProductDAO _instance;
        private static readonly object SyncRoot = new Object();
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

        #region Public Property
        public override string ConnectionString
        {
            get { return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.ProductCatalog_ConnectionString); }
        }

        public List<MARCProfile> GetMARCProfiles(string orgId, out bool marcGridSort)
        {
            var resultSet = new List<MARCProfile>();

            marcGridSort = true;

            if (string.IsNullOrEmpty(orgId)) return null;

            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                using (var command = CreateSqlSpCommand(StoredProcedureName.PROC_MARC_GET_PROFILES, dbConnection))
                {
                    var paramOrgId = new SqlParameter("@OrgID", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Input, Value = orgId };
                    var paramMarcGridSort = new SqlParameter("@MarcGridSort", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(paramOrgId);
                    command.Parameters.Add(paramMarcGridSort);

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
        #endregion
    }
}
