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

namespace ICommerce
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Commerce : ICommerceService
    {

        public class Global
        {
            public static string COMM_LOGFILE = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Config.DOWNLOADLocalFolder + "\\" + Config.DOWNLOADLogFileName;
            
        }

     public GetMarketTypeResponse GetMarketType(string EmailAddress, string VendorAPIKey ) 

        {

            // build response message
            GetMarketTypeResponse GetMTResponse = new GetMarketTypeResponse();
            string ReturnValueIn = string.Empty;
            string ReturnMessageIn = string.Empty;
            string MarketTypeIn = string.Empty;
            

            try
            {

                DataTable dtTableValue = new DataTable();
                DataTable dtTableIUserResults = new DataTable();
                string spAlertLogilename = Config.SPLogFileName;

                
                WriteLogFile(Global.COMM_LOGFILE, "Commerce_FILE Param", EmailAddress,Config.LogDetails );
                string vendorapikey = Config.VendorAPIKey;

                if (vendorapikey != VendorAPIKey)
                {
                    GetMTResponse.Status = "Failed";
                    GetMTResponse.ErrorMessage = "Invalid VendorAPIKey Passed";
                    WriteLogFile(Global.COMM_LOGFILE, "Commerce_FILE Key", EmailAddress + ":" + VendorAPIKey, Config.LogDetails);
                    return GetMTResponse;
                }

                string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                DataAccess da = new DataAccess();

                dtTableIUserResults = da.GetMarketTypeSQL(EmailAddress, conn, out MarketTypeIn, out ReturnMessageIn, out ReturnValueIn);
              
                if (ReturnValueIn == "0")
                {
                    GetMTResponse.Status = "OK";
                    GetMTResponse.MarketType = MarketTypeIn;
                    return GetMTResponse;
                }
                else
                {
                    GetMTResponse.Status = "Failed";
                    GetMTResponse.ErrorMessage = ReturnValueIn + ": " + ReturnMessageIn;
                    WriteLogFile(Global.COMM_LOGFILE, "Commerce_FILE Fail:" + ReturnValueIn + ": " + ReturnMessageIn, EmailAddress, Config.LogDetails);
                    return GetMTResponse;

                }
            }


            catch (FaultException faultEx)
            {
                GetMTResponse.Status = "Failed";
                GetMTResponse.ErrorMessage = faultEx.Message;
                WriteLogFile(Global.COMM_LOGFILE, "Commerce_FILE Fault:" + faultEx.Message, EmailAddress, Config.LogDetails);
                return GetMTResponse;
                throw new FaultException(faultEx.Message, new FaultCode(faultEx.Code.Name));
  
            
            }


            catch (Exception ex)
            {
                //throw new FaultException(ex.Message, new FaultCode("CREATE_ALERT_FILE"));
                GetMTResponse.Status = "Failed";
                GetMTResponse.ErrorMessage = ex.Message;
                WriteLogFile(Global.COMM_LOGFILE, "Commerce_FILE Ex:" + ex.Message, EmailAddress, Config.LogDetails);
                return GetMTResponse;
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

