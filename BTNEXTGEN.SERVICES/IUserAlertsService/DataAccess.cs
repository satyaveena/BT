using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using BTNextGen.Services.Common;


namespace IUserAlerts
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



      public DataTable CreateUserAlertMessageSQL(String Message, String UserID, Int64 MessageTemplateID, String SourceSystem, string conn, out string ResponseMessage, out string ResponseValue )

        {
            try
            {
                OpenConnection(conn);

                int SQLCommandTimeout; 
                
                DataTable dt = new DataTable();
                string storedProcName = "[dbo].[procInsertAlertUserMessage]";// the stored procedure and the input values used are only for test purpose.
                SQLCommandTimeout = Convert.ToInt32(Config.SQLCommandTimeout);
                using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    // Input Paramter

                    SqlParameter paramMessageName = new SqlParameter();
                    paramMessageName.ParameterName = "@AlertMessage";
                    paramMessageName.SqlDbType = SqlDbType.VarChar;
                    paramMessageName.Value = Message;
                    paramMessageName.Size = 500; 
                    sqlCmd.Parameters.Add(paramMessageName);

                    SqlParameter paramUserID = new SqlParameter();
                    paramUserID.ParameterName = "@UserID";
                    paramUserID.SqlDbType = SqlDbType.VarChar;
                    paramUserID.Value = UserID;
                    
                    sqlCmd.Parameters.Add(paramUserID);

                    SqlParameter paramAlertMessageTemplateID = new SqlParameter();
                    paramAlertMessageTemplateID.ParameterName = "@MessageTemplateID";
                    paramAlertMessageTemplateID.SqlDbType = SqlDbType.Int;
                    paramAlertMessageTemplateID.Value = MessageTemplateID;
                    
                    sqlCmd.Parameters.Add(paramAlertMessageTemplateID);

                    SqlParameter paramSourceSystem = new SqlParameter();
                    paramSourceSystem.ParameterName = "@SourceSystem";
                    paramSourceSystem.SqlDbType = SqlDbType.VarChar;
                    paramSourceSystem.Value = SourceSystem;

                    sqlCmd.Parameters.Add(paramSourceSystem);

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

                    reader.Close();
                    CloseConnection(conn);

                    ResponseMessage = (sqlCmd.Parameters["@Message"].Value.ToString());
                    ResponseValue = (sqlCmd.Parameters["@Return"].Value.ToString());



                    
                }
                return dt ;
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


        //public DataTable GetUserAlertMessageTemplateSQL(String AlertMessageTemplateID, string conn, out string AlertMessageTemplate,  out string ResponseMessage, out string ResponseValue)
      public DataTable GetUserAlertMessageTemplateSQL(Int64 AlertMessageTemplateID, string conn, out string AlertMessageTemplate, out string ConfigReferenceValue, out string ResponseMessage, out string ResponseValue)

        {
            try
            {
                OpenConnection(conn);

                int SQLCommandTimeout;

                DataTable dt = new DataTable();
                string storedProcName = "[dbo].[procGetAlertMessageTemplate]";// the stored procedure and the input values used are only for test purpose.
                SQLCommandTimeout = Convert.ToInt32(Config.SQLCommandTimeout);
                using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    // Input Paramter

                    SqlParameter paramAlertMessageTemplateID = new SqlParameter();
                    paramAlertMessageTemplateID.ParameterName = "@AlertMessageTemplateID";
                    paramAlertMessageTemplateID.SqlDbType = SqlDbType.BigInt;
                    paramAlertMessageTemplateID.Value = AlertMessageTemplateID;


                    sqlCmd.Parameters.Add(paramAlertMessageTemplateID);

                    SqlParameter paramAlertMessageTemplate = new SqlParameter();
                    paramAlertMessageTemplate.ParameterName = "@AlertMessageTemplate";
                    paramAlertMessageTemplate.SqlDbType = SqlDbType.VarChar;
                    paramAlertMessageTemplate.Size = 500;
                    paramAlertMessageTemplate.Direction = ParameterDirection.Output;

                    sqlCmd.Parameters.Add(paramAlertMessageTemplate);

                    SqlParameter paramReference = new SqlParameter();
                    paramReference.ParameterName = "@ConfigReferenceValue";
                    paramReference.SqlDbType = SqlDbType.VarChar;
                    paramReference.Size = 500;
                    paramReference.Direction = ParameterDirection.Output;

                    sqlCmd.Parameters.Add(paramReference);

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


                    AlertMessageTemplate = (sqlCmd.Parameters["@AlertMessageTemplate"].Value.ToString());
                    ConfigReferenceValue = (sqlCmd.Parameters["@ConfigReferenceValue"].Value.ToString());
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