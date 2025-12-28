using BT.ETS.Business.Constants;
using BT.ETS.Business.Exceptions;
using BT.ETS.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.DAO
{
    class ProfileDAO : BaseDAO
    {

        private static volatile ProfileDAO _instance;
        private static readonly object SyncRoot = new Object();

        public static ProfileDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new ProfileDAO();
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

        public async Task<DataSet> GetUserAccounts(string userId)
        {
            var dbConnection = CreateSqlConnection();

            var command = CreateSqlSpCommand(StoredProcedureName.GetUserShipToAccounts, dbConnection);
            //<Parameter>
            var sqlParamaters = CreateSqlParamaters(1);
            sqlParamaters[0] = new SqlParameter("@UserID", userId);
            //</Parameter>
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
    }
}
