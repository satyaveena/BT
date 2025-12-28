using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BT.CDMS.Business.Constants;
using BT.CDMS.Business.DataAccess.Interface;
using BT.CDMS.Business.Models;
using BT.CDMS.Business.Helper;

namespace BT.CDMS.Business.DataAccess
{
    /// <summary>
    /// Class GridDAO
    /// </summary>
    public class GridDAO : BaseDAO, IGridDAO
    {

        #region Public Property
        public override string ConnectionString
        {
            get { return ConfigurationManager.AppSettings["Orders_ConnString"]; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// GetGridTemplatesByOrgId
        /// </summary>
        /// <param name="OrgId"></param>
        /// <returns></returns>
        public DataSet GetGridTemplatesByOrgId(string orgId)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.ProcTS360GetGridTemplatesByOrgID, dbConnection);
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
        /// CheckIfTemplateIsAccessibleToListOfUsers
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataSet CheckIfTemplateIsAccessibleToListOfUsers(CheckGridTemplateAccessRequest request)
        {
            var dbConnection = CreateSqlConnection();
            var command = CreateSqlSpCommand(StoredProcedureName.ProcTS360VerifyTemplateAccessibility, dbConnection);
            var sqlParamaters = CreateSqlParamaters(2);
            sqlParamaters[0] = new SqlParameter("GridTemplateID", SqlDbType.NVarChar) { Value = request.GridTemplateId };

            var userIDTable = DataAccessHelper.GenerateDataRecords(request.UserIdsList, "utblCSGUIDS", 50);
            sqlParamaters[1] = new SqlParameter("UserID", SqlDbType.Structured) { Value = userIDTable };

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

        #endregion
    }
}