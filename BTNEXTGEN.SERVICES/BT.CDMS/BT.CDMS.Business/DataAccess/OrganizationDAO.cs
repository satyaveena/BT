using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BT.CDMS.Business.Constants;
using BT.CDMS.Business.DataAccess.Interface;

namespace BT.CDMS.Business.DataAccess
{
    /// <summary>
    /// Class OrganizationDAO
    /// </summary>
    public class OrganizationDAO : BaseDAO, IOrganizationDAO
    {

        #region Public Property
        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["Organization_ConnString"]; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// SearchByOrganization
        /// </summary>
        /// <param name="OrgName"></param>
        /// <returns></returns>
        public DataSet SearchByOrgName(string orgName)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.GetOrgIDUsingOrgName, dbConnection);
            var sqlParamaters = CreateSqlParamaters(2);
            sqlParamaters[0] = new SqlParameter("OrgName", SqlDbType.NVarChar) { SqlValue = orgName };
            var maxPageSize = ConfigurationManager.AppSettings["MaxPageSize"];
            sqlParamaters[1] = new SqlParameter("MaxPageSize", SqlDbType.Int) { SqlValue = maxPageSize };
            command.Parameters.AddRange(sqlParamaters);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            try
            {
                dbConnection.Open();
                sqlDa.Fill(ds);
                HandleException(command);
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
        public DataSet GetLoginIDsByOrgId(string orgId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.GetLoginIDsByOrgID, dbConnection);
            var paramUser = new SqlParameter("OrgID", SqlDbType.NVarChar) { SqlValue = orgId };
            command.Parameters.Add(paramUser);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            try
            {
                dbConnection.Open();
                sqlDa.Fill(ds);
                HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }

        /// <summary>
        /// SearchByLoginId
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public string SearchByLoginId(string loginId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.ProcTS360GetUserIDByUserName, dbConnection);
            var sqlParamaters = CreateSqlParamaters(2);
            sqlParamaters[0] = new SqlParameter("UserName", SqlDbType.NVarChar) { SqlValue = loginId };
            sqlParamaters[1] = new SqlParameter("UserID", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
            command.Parameters.RemoveAt("@ErrorMessage");
            command.Parameters.AddRange(sqlParamaters);
            var ds = new DataSet();
            var sqlDa = new SqlDataAdapter(command);
            try
            {
                dbConnection.Open();
                sqlDa.Fill(ds);
                HandleException(command);
            }
            finally
            {
                dbConnection.Close();
            }
            var responseValue = command.Parameters["UserID"].Value;
            return responseValue != null ? responseValue.ToString() : "";
        }

        #endregion
    }
}