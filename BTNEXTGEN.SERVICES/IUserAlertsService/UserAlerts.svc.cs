using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using BTNextGen.Services.Common;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace IUserAlerts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class UserAlerts : IUserAlerts
    {

        public class Global
        {
            public static string ALERT_LOGFILE = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Config.SPLocalFolder + "\\" + Config.AlertLogFileName;
            
        }

        public CreateUserAlertMessageResponse CreateUserAlertMessage(String AlertMessage, String UserID, AlertMessageTemplateIDEnum AlertMessageTemplateID, String SourceSystem)


        {

            // build response message
            CreateUserAlertMessageResponse alertResponse = new CreateUserAlertMessageResponse();
            string ReturnValueIn = string.Empty;
            string ReturnMessageIn = string.Empty;
            string AlertTemplateIn = string.Empty;
             

            try
            {



                DataTable dtTableValue = new DataTable();
                DataTable dtTableIUserResults = new DataTable();

                string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                WriteLogFile(Global.ALERT_LOGFILE, "Alert_FILE-Parameters",
                                                   "AlertMessage" + AlertMessage + "\r\n" +
                                                   "UserID" + UserID + "\r\n" +
                                                   "MessageID" + Convert.ToString( ((int)AlertMessageTemplateID))  + "\r\n" + 
                                                   "SourceSystem" + SourceSystem, "true");

                DataAccess da = new DataAccess();
                //da.InsertTVPRecords(dtTableValue, conn);
                dtTableIUserResults = da.CreateUserAlertMessageSQL(AlertMessage, UserID, (int)AlertMessageTemplateID, SourceSystem, conn, out ReturnMessageIn, out ReturnValueIn);

  


                if ( ReturnValueIn == "0" )
                {
                    alertResponse.Status = "OK";

                    return alertResponse;
                }
                else
                {
                    alertResponse.Status = "Failed";
                    alertResponse.ErrorMessage = ReturnValueIn + ": " + ReturnMessageIn;
                    //alertResponse.ErrorMessage = "xxx";
                    return alertResponse;

                }
            }


            catch (Exception ex)
            {
                //throw new FaultException(ex.Message, new FaultCode("CREATE_ALERT_FILE"));
                alertResponse.Status = "Failed";
                alertResponse.ErrorMessage = ex.Message;
                return alertResponse;
            }

        }


      public GetUserAlertMessageTemplateResponse GetUserAlertMessageTemplate(AlertMessageTemplateIDEnum AlertMessageTemplateID) 

        {

            // build response message
            GetUserAlertMessageTemplateResponse alertResponse = new GetUserAlertMessageTemplateResponse();
            string ReturnValueIn = string.Empty;
            string ReturnMessageIn = string.Empty;
            string AlertTemplateIn = string.Empty;
            string ConfigReferenceIn = string.Empty; 


            try
            {



                DataTable dtTableValue = new DataTable();
                DataTable dtTableIUserResults = new DataTable();
                string spAlertLogilename = Config.SPLogFileName;
                int convertedAlertID = 0;

                convertedAlertID = (int)AlertMessageTemplateID;
                WriteLogFile(Global.ALERT_LOGFILE, "Alert_FILE", convertedAlertID.ToString(), "true");

                //WriteLogFile(Global.ALERT_LOGFILE, "Alert_FILE", "1...", "true");
                string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                DataAccess da = new DataAccess();

                //WriteLogFile(Global.ALERT_LOGFILE, "Alert_FILE", "x" + convertedAlertID.ToString() + "Z", "true");
    
                //dtTableIUserResults = da.GetUserAlertMessageTemplateSQL(convertedAlertID.ToString(), conn, out AlertTemplateIn, out ConfigReferenceIn, out ReturnMessageIn, out ReturnValueIn);
                dtTableIUserResults = da.GetUserAlertMessageTemplateSQL((int)AlertMessageTemplateID, conn, out AlertTemplateIn, out ConfigReferenceIn, out ReturnMessageIn, out ReturnValueIn);

                //WriteLogFile(Global.ALERT_LOGFILE, "Alert_FILE", "2...", "true");
                if (ReturnValueIn == "0")
                {
                    alertResponse.Status = "OK";
                    alertResponse.AlertMessageTemplate = AlertTemplateIn;
                    alertResponse.ConfigReferenceValue = ConfigReferenceIn; 

                    return alertResponse;
                }
                else
                {
                    alertResponse.Status = "Failed";
                    alertResponse.ErrorMessage = ReturnValueIn + ": " + ReturnMessageIn;
                    //alertResponse.ErrorMessage = "xxx";
                    return alertResponse;

                }
            }


            catch (FaultException faultEx)
            {
                alertResponse.Status = "Failed";
                alertResponse.ErrorMessage = faultEx.Message;
                return alertResponse;
                throw new FaultException(faultEx.Message, new FaultCode(faultEx.Code.Name));
  
            
            }


            catch (Exception ex)
            {
                //throw new FaultException(ex.Message, new FaultCode("CREATE_ALERT_FILE"));
                alertResponse.Status = "Failed";
                alertResponse.ErrorMessage = ex.Message;
                return alertResponse;
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

    }
 }

