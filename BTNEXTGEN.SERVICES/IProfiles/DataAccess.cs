using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using BTNextGen.Services.Common;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
//using BTNextGen.Services.Common;
using System.Configuration;



namespace IProfiles
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
                throw new FaultException(ex.Message, new FaultCode("PROFILES_DB_CONNECT"));
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
                throw new FaultException(ex.Message, new FaultCode("PROFILES_DB_CONNECT"));
            }
        }





        public void GetERPAccountNum(String CreditCardGUID, string conn, out string ERPAccountnum)
        {
            try
            {
                OpenConnection(conn);

                //WriteLogFile(Global.FTP_LOGFILE, "5 ", "5", "true");

                int SQLCommandTimeout;

                DataTable dt = new DataTable();
                string storedProcName = "[dbo].[GetERPAccountWithCreditCardGuid]";// the stored procedure and the input values used are only for test purpose.
                SQLCommandTimeout = Convert.ToInt32(Config.SQLCommandTimeout);
                using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    // Input Paramter

                    SqlParameter paramCreditCardGUID = new SqlParameter();
                    paramCreditCardGUID.ParameterName = "@CreditCardGUID";
                    paramCreditCardGUID.SqlDbType = SqlDbType.VarChar;
                    paramCreditCardGUID.Value = CreditCardGUID;
                    sqlCmd.Parameters.Add(paramCreditCardGUID);

                    SqlParameter paramERPAccount = new SqlParameter();
                    paramERPAccount.ParameterName = "@ERPAccountnum";
                    paramERPAccount.SqlDbType = SqlDbType.VarChar;
                    paramERPAccount.Direction = ParameterDirection.Output;
                    paramERPAccount.Size = 500;
                    sqlCmd.Parameters.Add(paramERPAccount);


                    SqlParameter paramReturn = new SqlParameter();
                    paramReturn.ParameterName = "@Return";
                    paramReturn.SqlDbType = SqlDbType.Int;
                    paramReturn.Direction = ParameterDirection.Output;
                    sqlCmd.Parameters.Add(paramReturn);

                    SqlParameter paramErrorMessage = new SqlParameter();
                    paramErrorMessage.ParameterName = "@ErrorMessage";
                    paramErrorMessage.SqlDbType = SqlDbType.VarChar;
                    paramErrorMessage.Direction = ParameterDirection.Output;
                    paramErrorMessage.Size = 500;
                    sqlCmd.Parameters.Add(paramErrorMessage);

                    sqlCmd.CommandTimeout = SQLCommandTimeout;

                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();

                    CloseConnection(conn);

                    ERPAccountnum = (sqlCmd.Parameters["@ERPAccountnum"].Value.ToString());


                }

            }

            catch (FaultException ex)
            {




                throw new FaultException(ex.Message, new FaultCode(ex.Code.Name));
            }

            catch (SqlException sqlEx)
            {


                throw new FaultException(sqlEx.Message, new FaultCode("GETERPAcctNUM"));
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