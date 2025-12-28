using BT.TS360API.ExternalDataSendService.Configration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.ExternalDataSendService.DataAccess
{
    public class OrganizationDAO : BaseDAO
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
        /// Retrieve Organization by ID.
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public DataTable GetOrganizationById(string orgId)
        {
            using (var dbConnection = CreateSqlConnection())
            {
                using (var command = CreateSqlSpCommand("procTS360GetOrganizationByIDForExternalPush", dbConnection))
                {
                    var sqlParamaters = CreateSqlParamaters(1);
                    sqlParamaters[0] = new SqlParameter("@OrgID", orgId);

                    command.Parameters.AddRange(sqlParamaters);

                    // Create Data Adapter & Data Set & Fill
                    var da = new SqlDataAdapter(command);
                    var ds = new DataSet();

                    dbConnection.Open();
                    da.Fill(ds);

                    // Handle Error
                    HandleException(command);

                    return ds.Tables[0];
                }

            }
        }
    }
}
