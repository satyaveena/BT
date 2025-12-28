using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using BTNextGen.Services.Common;


namespace ICommerce
{
    public class DataAccess
    {
        private SqlConnection sqlConn = null;

        public void OpenConnection(string connectionString)
        {
            try
            {
                sqlConn = new SqlConnection();
                sqlConn.ConnectionString = connectionString;
                sqlConn.Open();

            }

            catch (SqlException ex)
            {
                throw new FaultException(ex.Message, new FaultCode("IUSR_DB_CONNECT"));
            }
        }

        public void CloseConnection(string connectionString)
        {
            try
            {
                sqlConn.Close();
            }
            catch (SqlException ex)
            {
                throw new FaultException(ex.Message, new FaultCode("IUSR_DB_CONNECT"));
            }
        }




        //public DataTable GetUserAlertMessageTemplateSQL(String AlertMessageTemplateID, string conn, out string AlertMessageTemplate,  out string ResponseMessage, out string ResponseValue)
      public DataTable GetMarketTypeSQL(string EmailAddress, string conn, out string MarketType, out string ResponseMessage, out string ResponseValue)

        {
            try
            {
                OpenConnection(conn);

                int SQLCommandTimeout;

                DataTable dt = new DataTable();
                string storedProcName = "[dbo].[procGetMarketType]";// the stored procedure and the input values used are only for test purpose.
                SQLCommandTimeout = Convert.ToInt32(Config.SQLCommandTimeout);
                using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    // Input Paramter

                    SqlParameter paramEmailAddress = new SqlParameter();
                    paramEmailAddress.ParameterName = "@EmailAddress";
                    paramEmailAddress.SqlDbType = SqlDbType.VarChar;
                    paramEmailAddress.Value = EmailAddress;
                    sqlCmd.Parameters.Add(paramEmailAddress);
                    paramEmailAddress.Size = 100;


                    SqlParameter paramMarketType = new SqlParameter();
                    paramMarketType.ParameterName = "@MarketType";
                    paramMarketType.SqlDbType = SqlDbType.VarChar;
                    paramMarketType.Direction = ParameterDirection.Output;
                    sqlCmd.Parameters.Add(paramMarketType);
                    paramMarketType.Size = 50;


                    SqlParameter paramReturn = new SqlParameter();
                    paramReturn.ParameterName = "@Return";
                    paramReturn.SqlDbType = SqlDbType.Int;
                    paramReturn.Direction = ParameterDirection.Output;

                    sqlCmd.Parameters.Add(paramReturn);

                    SqlParameter paramMessage = new SqlParameter();
                    paramMessage.ParameterName = "@Message";
                    paramMessage.SqlDbType = SqlDbType.VarChar;
                    paramMessage.Direction = ParameterDirection.Output;
                    paramMessage.Size = 500;
                    sqlCmd.Parameters.Add(paramMessage);


                    sqlCmd.CommandTimeout = SQLCommandTimeout;
                    SqlDataReader reader = sqlCmd.ExecuteReader();



                    dt.Load(reader);


                    MarketType = (sqlCmd.Parameters["@MarketType"].Value.ToString());
                    ResponseMessage = (sqlCmd.Parameters["@Message"].Value.ToString());
                    ResponseValue = (sqlCmd.Parameters["@Return"].Value.ToString());


                    //ReturnID = paramReturn;
                    //ReturnMessage = paramMessage; 

                    reader.Close();
                    CloseConnection(conn);
                }
                return dt;
            }

            catch (FaultException ex)
            {
                throw new FaultException(ex.Message, new FaultCode(ex.Code.Name));
            }

            catch (SqlException sqlEx)
            {
                throw new FaultException(sqlEx.Message, new FaultCode("IUSR_DB_READ"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }

        }

    }
}