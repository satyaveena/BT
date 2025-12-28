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



namespace MARCProfilerService
{
    public class DataAccess
    {
        private SqlConnection sqlConn = null;

        public class Global
        {
            public static string FTP_LOGFILE = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Config.FTPLocalFolder + "\\" + Config.FTPLogFileName;

        }

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
                throw new FaultException(ex.Message, new FaultCode("MARC_DB_CONNECT"));
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
                throw new FaultException(ex.Message, new FaultCode("MARC_DB_CONNECT"));
            }
        }
        
        public DataTable GetMARCRecordsOld(DataTable dtBTKeySeq, string strMARCProfileID, char FullBriefIndicator, string BasketSummaryID, string conn)
        {
            try
            {
                OpenConnection(conn);

                DataTable dt = new DataTable();
                string storedProcName = "[dbo].[procGetMARCBasket]";// the stored procedure and the input values used are only for test purpose.

                using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    // Input Paramter

                    SqlParameter tvpparam = sqlCmd.Parameters.AddWithValue("@BTKeys", dtBTKeySeq);
                    tvpparam.SqlDbType = SqlDbType.Structured;

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@MARCProfileID";
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Value = strMARCProfileID;
                    sqlCmd.Parameters.Add(param);

                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@FullBriefIndicator";
                    param1.SqlDbType = SqlDbType.Char;
                    param1.Value = FullBriefIndicator;
                    sqlCmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@BasketSummaryID";
                    param2.SqlDbType = SqlDbType.VarChar;
                    param2.Value = BasketSummaryID;
                    sqlCmd.Parameters.Add(param2);


                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    CloseConnection(conn);
                }
                return dt;
            }

            catch( FaultException ex )
            {
                throw new FaultException(ex.Message, new FaultCode(ex.Code.Name));
            }
            
            catch (SqlException sqlEx)
            {
                throw new FaultException(sqlEx.Message, new FaultCode("MARC_DB_READ"));
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


        public DataTable GetMARCRecords(String sortColumn, String basketSummaryID, String sortDirection, String ProfileID, string FullIndicator,bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, DataTable  dtTableMarcInventory, string conn)
        {
            try
            {
                OpenConnection(conn);

                int SQLCommandTimeout; 
                
                DataTable dt = new DataTable();
                string storedProcName = "[dbo].[procMARCGetBasket]";// the stored procedure and the input values used are only for test purpose.
                SQLCommandTimeout = Convert.ToInt32(Config.SQLCommandTimeout);
                using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    // Input Paramter

                    SqlParameter paramSortColumn = new SqlParameter();
                    paramSortColumn.ParameterName = "@SortColumn";
                    paramSortColumn.SqlDbType = SqlDbType.VarChar;
                    paramSortColumn.Value = sortColumn;
                    sqlCmd.Parameters.Add(paramSortColumn);

                    SqlParameter paramBasketSummaryID = new SqlParameter();
                    paramBasketSummaryID.ParameterName = "@BasketSummaryID";
                    paramBasketSummaryID.SqlDbType = SqlDbType.VarChar;
                    paramBasketSummaryID.Value = basketSummaryID;
                    sqlCmd.Parameters.Add(paramBasketSummaryID);

                    SqlParameter paramSortDirection = new SqlParameter();
                    paramSortDirection.ParameterName = "@sortDirection";
                    paramSortDirection.SqlDbType = SqlDbType.VarChar;
                    paramSortDirection.Value = sortDirection;
                    sqlCmd.Parameters.Add(paramSortDirection);

                    SqlParameter paramProfileID = new SqlParameter();
                    paramProfileID.ParameterName = "@MARCProfileID";
                    paramProfileID.SqlDbType = SqlDbType.VarChar;
                    paramProfileID.Value = ProfileID;
                    sqlCmd.Parameters.Add(paramProfileID);

                    SqlParameter paramFullIndicator = new SqlParameter();
                    paramFullIndicator.ParameterName = "@FullAcquisitionIndicator";
                    paramFullIndicator.SqlDbType = SqlDbType.Char;
                    paramFullIndicator.Value = FullIndicator;
                    sqlCmd.Parameters.Add(paramFullIndicator);

                    SqlParameter paramIsOrdered = new SqlParameter();
                    paramIsOrdered.ParameterName = "@IsOrdered";
                    paramIsOrdered.SqlDbType = SqlDbType.Bit;
                    paramIsOrdered.Value = isOrdered;
                    sqlCmd.Parameters.Add(paramIsOrdered);

                    SqlParameter paramIsCancelled = new SqlParameter();
                    paramIsCancelled.ParameterName = "@IsCancelled";
                    paramIsCancelled.SqlDbType = SqlDbType.Bit;
                    paramIsCancelled.Value = isCancelled;
                    sqlCmd.Parameters.Add(paramIsCancelled);

                    SqlParameter paramIsOCLCEnabled = new SqlParameter();
                    paramIsOCLCEnabled.ParameterName = "@OCLCEnabled";
                    paramIsOCLCEnabled.SqlDbType = SqlDbType.Bit;
                    paramIsOCLCEnabled.Value = isOCLCEnabled;
                    sqlCmd.Parameters.Add(paramIsOCLCEnabled);

                    SqlParameter paramIsBTEmployee = new SqlParameter();
                    paramIsBTEmployee.ParameterName = "@IsBTEmployee";
                    paramIsBTEmployee.SqlDbType = SqlDbType.Bit;
                    paramIsBTEmployee.Value = isBTEmployee;
                    sqlCmd.Parameters.Add(paramIsBTEmployee);

                    SqlParameter paramdtTableMarcInventory = new SqlParameter();
                    paramdtTableMarcInventory.ParameterName = "@utblBasketInventoryMarc";
                    paramdtTableMarcInventory.SqlDbType = SqlDbType.Structured;
                    paramdtTableMarcInventory.Value = dtTableMarcInventory;
                    sqlCmd.Parameters.Add(paramdtTableMarcInventory);


                    sqlCmd.CommandTimeout = SQLCommandTimeout; 
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    dt.Load(reader);
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
                throw new FaultException(sqlEx.Message, new FaultCode("MARC_DB_READ"));
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



        public DataTable GetMARCRecordsFTP(String sortColumn, String basketSummaryID, String sortDirection, String ProfileID, string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, DataTable dtTableMarcInventory, string conn)

        {
            
            //string connOrders = ConfigurationManager.ConnectionStrings["OrdersConnectionString"].ConnectionString;
            try
            {
                OpenConnection(conn);

                int SQLCommandTimeout;

                DataTable dt = new DataTable();
                string storedProcName = "[dbo].[procMARCGetBasket]";// the stored procedure and the input values used are only for test purpose.
                SQLCommandTimeout = Convert.ToInt32(Config.SQLCommandTimeout);
                using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    // Input Paramter

                    SqlParameter paramSortColumn = new SqlParameter();
                    paramSortColumn.ParameterName = "@SortColumn";
                    paramSortColumn.SqlDbType = SqlDbType.VarChar;
                    paramSortColumn.Value = sortColumn;
                    sqlCmd.Parameters.Add(paramSortColumn);

                    SqlParameter paramBasketSummaryID = new SqlParameter();
                    paramBasketSummaryID.ParameterName = "@BasketSummaryID";
                    paramBasketSummaryID.SqlDbType = SqlDbType.VarChar;
                    paramBasketSummaryID.Value = basketSummaryID;
                    sqlCmd.Parameters.Add(paramBasketSummaryID);

                    SqlParameter paramSortDirection = new SqlParameter();
                    paramSortDirection.ParameterName = "@sortDirection";
                    paramSortDirection.SqlDbType = SqlDbType.VarChar;
                    paramSortDirection.Value = sortDirection;
                    sqlCmd.Parameters.Add(paramSortDirection);

                    SqlParameter paramProfileID = new SqlParameter();
                    paramProfileID.ParameterName = "@MARCProfileID";
                    paramProfileID.SqlDbType = SqlDbType.VarChar;
                    paramProfileID.Value = ProfileID;
                    sqlCmd.Parameters.Add(paramProfileID);

                    SqlParameter paramFullIndicator = new SqlParameter();
                    paramFullIndicator.ParameterName = "@FullAcquisitionIndicator";
                    paramFullIndicator.SqlDbType = SqlDbType.Char;
                    paramFullIndicator.Value = FullIndicator;
                    sqlCmd.Parameters.Add(paramFullIndicator);

                    SqlParameter paramIsOrdered = new SqlParameter();
                    paramIsOrdered.ParameterName = "@IsOrdered";
                    paramIsOrdered.SqlDbType = SqlDbType.Bit;
                    paramIsOrdered.Value = isOrdered;
                    sqlCmd.Parameters.Add(paramIsOrdered);

                    SqlParameter paramIsCancelled = new SqlParameter();
                    paramIsCancelled.ParameterName = "@IsCancelled";
                    paramIsCancelled.SqlDbType = SqlDbType.Bit;
                    paramIsCancelled.Value = isCancelled;
                    sqlCmd.Parameters.Add(paramIsCancelled);

                    SqlParameter paramIsOCLCEnabled = new SqlParameter();
                    paramIsOCLCEnabled.ParameterName = "@OCLCEnabled";
                    paramIsOCLCEnabled.SqlDbType = SqlDbType.Bit;
                    paramIsOCLCEnabled.Value = isOCLCEnabled;
                    sqlCmd.Parameters.Add(paramIsOCLCEnabled);

                    SqlParameter paramIsBTEmployee = new SqlParameter();
                    paramIsBTEmployee.ParameterName = "@IsBTEmployee";
                    paramIsBTEmployee.SqlDbType = SqlDbType.Bit;
                    paramIsBTEmployee.Value = isBTEmployee;
                    sqlCmd.Parameters.Add(paramIsBTEmployee);

                    SqlParameter paramdtTableMarcInventory = new SqlParameter();
                    paramdtTableMarcInventory.ParameterName = "@utblBasketInventoryMarc";
                    paramdtTableMarcInventory.SqlDbType = SqlDbType.Structured;
                    paramdtTableMarcInventory.Value = dtTableMarcInventory;
                    sqlCmd.Parameters.Add(paramdtTableMarcInventory);


                    sqlCmd.CommandTimeout = SQLCommandTimeout;
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    
                    dt.Load(reader);
                    reader.Close();
                    CloseConnection(conn);
                   
                }
                return dt;
            }

            catch (FaultException ex)
            {

                WriteLogFile(Global.FTP_LOGFILE, "GetMarcRecords_FTP: ", ex.Message.ToString(), "true");
                SendEmailException("GetMarcRecords_FTP Fault: " + ex.Message.ToString(), "GetMarcRecords_FTP", "true");

                throw new FaultException(ex.Message, new FaultCode(ex.Code.Name));

            }

            catch (SqlException sqlEx)
            {

                WriteLogFile(Global.FTP_LOGFILE, "GetMarcRecords_FTP Fault: ", sqlEx.Message.ToString(), "true");
                //SendEmailException("GetMarcRecords_FTP Fault: " + sqlEx.Message.ToString(), "GetMarcRecords_FTP", "true");


                throw new FaultException(sqlEx.Message, new FaultCode("MARC_DB_READ"));
            }
            catch (Exception ex)
            {

                WriteLogFile(Global.FTP_LOGFILE, "GetMarcRecords_FTP Fault: ", ex.Message.ToString(), "true");
                //SendEmailException("GetMarcRecords_FTP Fault: " + ex.Message.ToString(), "GetMarcRecords_FTP", "true");


                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }

        }

        public DataTable GetMARCRecordsBTKeys(String basketSummaryID, string conn)
        {
            try
            {
                OpenConnection(conn);

                int SQLCommandTimeout;

                DataTable dt = new DataTable();
                string storedProcName = "[dbo].[procMARCGetBasketBTKeys]";// the stored procedure and the input values used are only for test purpose.
                SQLCommandTimeout = Convert.ToInt32(Config.SQLCommandTimeout);
                using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    // Input Paramter

                    SqlParameter paramBasketSummaryID = new SqlParameter();
                    paramBasketSummaryID.ParameterName = "@BasketSummaryID";
                    paramBasketSummaryID.SqlDbType = SqlDbType.VarChar;
                    paramBasketSummaryID.Value = basketSummaryID;
                    sqlCmd.Parameters.Add(paramBasketSummaryID);

                    sqlCmd.CommandTimeout = SQLCommandTimeout;
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    dt.Load(reader);
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
                throw new FaultException(sqlEx.Message, new FaultCode("MARC_DB_READ"));
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




public void UpdateMARCFTPFailure(String basketSummaryID, String  ftpErrorMessage, String ErrorMessage, string conn)
        {
            try
            {
                OpenConnection(conn);

                //WriteLogFile(Global.FTP_LOGFILE, "5 ", "5", "true");

                int SQLCommandTimeout; 
                
                DataTable dt = new DataTable();
                string storedProcName = "[dbo].[procTS360SetBasketOneClickMARC]";// the stored procedure and the input values used are only for test purpose.
                SQLCommandTimeout = Convert.ToInt32(Config.SQLCommandTimeout);
                using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    // Input Paramter

                    SqlParameter paramBasketSummaryID = new SqlParameter();
                    paramBasketSummaryID.ParameterName = "@BasketSummaryID";
                    paramBasketSummaryID.SqlDbType = SqlDbType.VarChar;
                    paramBasketSummaryID.Value = basketSummaryID;
                    sqlCmd.Parameters.Add(paramBasketSummaryID);

                    SqlParameter paramFTPErrorMessage = new SqlParameter();
                    paramFTPErrorMessage.ParameterName = "@FTPErrorMessage";
                    paramFTPErrorMessage.SqlDbType = SqlDbType.VarChar;
                    paramFTPErrorMessage.Value = ftpErrorMessage;
                    sqlCmd.Parameters.Add(paramFTPErrorMessage);

                    SqlParameter paramErrorMessage = new SqlParameter();
                    paramErrorMessage.ParameterName = "@ErrorMessage";
                    paramErrorMessage.SqlDbType = SqlDbType.VarChar;
                    paramErrorMessage.Value = "ok";
                    paramErrorMessage.SqlDbType = SqlDbType.VarChar;
                    paramErrorMessage.Direction = ParameterDirection.InputOutput;

                    sqlCmd.Parameters.Add(paramErrorMessage);
                    
                    sqlCmd.CommandTimeout = SQLCommandTimeout;

                    //WriteLogFile(Global.FTP_LOGFILE, "6 ", conn, "true");
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    //WriteLogFile(Global.FTP_LOGFILE, "6 ", "6.1", "true");
                    dt.Load(reader);
                    reader.Close();
                    //WriteLogFile(Global.FTP_LOGFILE, "6 ", "6.2", "true");
                    CloseConnection(conn);
                    //WriteLogFile(Global.FTP_LOGFILE, "6 ", "6.3", "true");
                    //WriteLogFile(Global.FTP_LOGFILE, "7 ", "7", "true");
                }
                //return dt;
            }

            catch (FaultException ex)
            {
                //WriteLogFile(Global.FTP_LOGFILE, "7 ", "7.1", "true");
                WriteLogFile(Global.FTP_LOGFILE, "UpdateMarcRecords_FTP Fault1: ", ex.Message.ToString(), "true");
                //SendEmailException("UpdateMarcRecords_FTP Fault: " + ex.Message.ToString(), "UpdateMarcRecords_FTP", "true");

                throw new FaultException(ex.Message, new FaultCode(ex.Code.Name));
            }

            catch (SqlException sqlEx)
            {
                //WriteLogFile(Global.FTP_LOGFILE, "8 ", "8", "true");
                WriteLogFile(Global.FTP_LOGFILE, "UpdateMarcRecords_FTP Fault2: ", sqlEx.Message.ToString(), "true");
                //SendEmailException("UpdateMarcRecords_FTP Fault: " + sqlEx.Message.ToString(), "UpdateMarcRecords_FTP", "true");
                throw new FaultException(sqlEx.Message, new FaultCode("MARC_DB_UPDATE"));
            }
            catch (Exception ex)
            {

                //WriteLogFile(Global.FTP_LOGFILE, "9 ", "9", "true");
                WriteLogFile(Global.FTP_LOGFILE, "UpdateMarcRecords_FTP Fault3: ", ex.Message.ToString(), "true");
                //SendEmailException("UpdateMarcRecords_FTP Fault: " + ex.Message.ToString(), "UpdateMarcRecords_FTP", "true");
                               
                throw ex;
            }
            finally
            {
                CloseConnection(conn);
            }

        }



public void UpdateMARCFTPFailureTEST(String marProfileID, String ftpErrorMessage, int statusCode, string conn)
{
    try
    {
        OpenConnection(conn);

        //WriteLogFile(Global.FTP_LOGFILE, "5 ", "5", "true");

        int SQLCommandTimeout;

        DataTable dt = new DataTable();
        string storedProcName = "[dbo].[procMarcSetOneClickTest]";// the stored procedure and the input values used are only for test purpose.
        SQLCommandTimeout = Convert.ToInt32(Config.SQLCommandTimeout);
        using (SqlCommand sqlCmd = new SqlCommand(storedProcName, this.sqlConn))
        {
            sqlCmd.CommandType = CommandType.StoredProcedure;
            // Input Paramter

            SqlParameter paramMarcProfileID = new SqlParameter();
            paramMarcProfileID.ParameterName = "@MARCProfileID";
            paramMarcProfileID.SqlDbType = SqlDbType.VarChar;
            paramMarcProfileID.Value = marProfileID ;
            sqlCmd.Parameters.Add(paramMarcProfileID);

            SqlParameter paramOneClickStatus = new SqlParameter();
            paramOneClickStatus.ParameterName = "@OneClickTestStatus";
            paramOneClickStatus.SqlDbType = SqlDbType.Int ;
            paramOneClickStatus.Value = statusCode ;
            sqlCmd.Parameters.Add(paramOneClickStatus);

            SqlParameter paramOneClickTestError = new SqlParameter();
            paramOneClickTestError.ParameterName = "@OneClickTestError";
            paramOneClickTestError.SqlDbType = SqlDbType.VarChar;
            paramOneClickTestError.Value = ftpErrorMessage;
            paramOneClickTestError.SqlDbType = SqlDbType.VarChar;
            paramOneClickTestError.Direction = ParameterDirection.Input;

            SqlParameter paramErrorMessage = new SqlParameter();
            paramErrorMessage.ParameterName = "@ErrorMessage";
            paramErrorMessage.SqlDbType = SqlDbType.VarChar;
            paramErrorMessage.Value = "ok" ;
            paramErrorMessage.SqlDbType = SqlDbType.VarChar;
            paramErrorMessage.Direction = ParameterDirection.InputOutput;

            sqlCmd.Parameters.Add(paramErrorMessage);

            sqlCmd.CommandTimeout = SQLCommandTimeout;

            //WriteLogFile(Global.FTP_LOGFILE, "6 ", conn, "true");
            SqlDataReader reader = sqlCmd.ExecuteReader();
            //WriteLogFile(Global.FTP_LOGFILE, "6 ", "6.1", "true");
            dt.Load(reader);
            reader.Close();
            //WriteLogFile(Global.FTP_LOGFILE, "6 ", "6.2", "true");
            CloseConnection(conn);
            //WriteLogFile(Global.FTP_LOGFILE, "6 ", "6.3", "true");
            //WriteLogFile(Global.FTP_LOGFILE, "7 ", "7", "true");
        }
        //return dt;
    }

    catch (FaultException ex)
    {
        //WriteLogFile(Global.FTP_LOGFILE, "7 ", "7.1", "true");
        WriteLogFile(Global.FTP_LOGFILE, "UpdateMarcRecords_FTP Fault1: ", ex.Message.ToString(), "true");
        //SendEmailException("UpdateMarcRecords_FTP Fault: " + ex.Message.ToString(), "UpdateMarcRecords_FTP", "true");

        throw new FaultException(ex.Message, new FaultCode(ex.Code.Name));
    }

    catch (SqlException sqlEx)
    {
        //WriteLogFile(Global.FTP_LOGFILE, "8 ", "8", "true");
        WriteLogFile(Global.FTP_LOGFILE, "UpdateMarcRecords_FTP Fault2: ", sqlEx.Message.ToString(), "true");
        //SendEmailException("UpdateMarcRecords_FTP Fault: " + sqlEx.Message.ToString(), "UpdateMarcRecords_FTP", "true");
        throw new FaultException(sqlEx.Message, new FaultCode("MARC_DB_UPDATE"));
    }
    catch (Exception ex)
    {

        //WriteLogFile(Global.FTP_LOGFILE, "9 ", "9", "true");
        WriteLogFile(Global.FTP_LOGFILE, "UpdateMarcRecords_FTP Fault3: ", ex.Message.ToString(), "true");
        //SendEmailException("UpdateMarcRecords_FTP Fault: " + ex.Message.ToString(), "UpdateMarcRecords_FTP", "true");

        throw ex;
    }
    finally
    {
        CloseConnection(conn);
    }

}

private static void WriteLogFile(string fileName, string methodName, string message, string LogFlag)
{
    if (LogFlag == "true")
    {
        try
        {
            //Create a writer and open the file:
            StreamWriter log;

            if (!System.IO.File.Exists(fileName))
            {
                log = new StreamWriter(fileName);
            }
            else
            {
                log = System.IO.File.AppendText(fileName);
            }

            // Write to the file:
            log.WriteLine(DateTime.Now + " , " + methodName + " , " + message);
            log.WriteLine();

            // Close the stream:
            log.Close();



        }

        catch { }
    }

}


private static void SendEmailException(string EmailMessage, string MethodName, string EmailFlag)
{

    try
    {

        if (EmailFlag == "true")
        {
            string to = Config.EmailToExceptions;
            string from = "no-reply@baker-taylor.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "WCF MarcProfiler - " + MethodName;
            message.Body = EmailMessage;


            SmtpClient client = new SmtpClient(Config.EmailServer);
            // Credentials are necessary if the server requires the client  
            // to authenticate before it will send e-mail on the client's behalf.
            client.UseDefaultCredentials = true;
            client.Send(message);
        }
    }
    // empty catch block here 
    catch (Exception ex)
    {
        throw new FaultException(ex.Message, new FaultCode("EMAILEXCEPTION"));

    }


}


    }
}