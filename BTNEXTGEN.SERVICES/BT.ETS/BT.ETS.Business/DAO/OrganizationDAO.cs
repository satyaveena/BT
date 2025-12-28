using BT.ETS.Business.Constants;
using BT.ETS.Business.DAO.Interface;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
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
    /// <summary>
    /// Class OrganizationDAO
    /// </summary>
    public class OrganizationDAO : BaseDAO
    {

        private static volatile OrganizationDAO _instance;
        private static readonly object SyncRoot = new Object();
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

        #region Public Property
        public override string ConnectionString
        {
            get { return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.Profiles_ConnectionString); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// SearchByOrganization
        /// </summary>
        /// <param name="OrgName"></param>
        /// <returns></returns>
        public async Task<DataSet> GetEspOrgsByDate(DateTime? since)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.GetEspOrgs, dbConnection);
            var sqlParamaters = CreateSqlParamaters(1);
            sqlParamaters[0] = new SqlParameter("@ESPEnabledDate", SqlDbType.DateTime) { SqlValue = since };
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
                    Logger.RaiseException(ex, ExceptionCategory.Organization);
                    throw new BusinessException(-2);
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

        /// <summary>
        /// GetLoginIDsByOrgID
        /// </summary>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public async Task<DataSet> GetLoginIDsByOrgId(string orgId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.GetLoginIdsByOrgId, dbConnection);
            var paramUser = new SqlParameter("OrgID", SqlDbType.NVarChar) { SqlValue = orgId };
            command.Parameters.Add(paramUser);
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
                    throw new BusinessException(-2);
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

       
        #endregion
    }
}
