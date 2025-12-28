using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
//using CyberSource.Api;
using System.Net; 
using BT.TS360.Services.Cybersource.Common.FileLogging;
using BT.TS360.Services.Cybersource.Common.FileTolas;
using BT.TS360.Services.Cybersource.Common.Configuration;
using BT.TS360.Services.Cybersource.Common;
using System.Collections.Generic ;
using BT.TS360.Services.Cybersource.Common.Generic; 
using CyberSource.Api;
using Cybersource_rest_samples_dotnetXXX; 

namespace Cybersource_rest_samples_dotnet.Samples.Reporting.CoreServicesPB
{
    public class DownloadReportPB
    {
        public void Run(string organizationid, string reportname, string reportdate, string secretkey, string secretkeyid, string runenvironment)
        {
            // File will be created with the Data received in the Response Body
            string reportabbr = "" ; 

            Generic generic = new Generic(); 
            reportabbr = "PB";

            string guid = Guid.NewGuid().ToString();
            string downloadFile1Path = AppSetting.ArchiveFolder; 
            string downloadFile1Name = "Archive1-" + reportabbr + "_" + organizationid + "RawResponse_" + guid + ".txt"; 
            string downloadFile1 = downloadFile1Path + downloadFile1Name ;
            string downloadFile2Name = "Archive2-" + reportabbr + "_" + organizationid + "XMLResponse_" + guid + ".txt"; 
            string downloadFile2 = downloadFile1Path + downloadFile2Name;
            string downloadFile3Name = "Archive3-" + reportabbr + "_" + organizationid + "TOLAS_" + guid + ".txt";
            string downloadFile3 = downloadFile1Path + downloadFile3Name;
            String downloadFile2Path = AppSetting.TolasFolderPB;
            string downloadfile4 = downloadFile2Path + downloadFile3Name;
            string downloadFile4Name = "Tolas-" + reportabbr + "_" + organizationid + "TOLAS_" + guid + ".txt";
            string results;
            FileLogRepository fileLog = new FileLogRepository(AppSetting.LogFolder, AppSetting.LogFilePrefix);
            
            try
            {
                fileLog.Write("Starting Download for1:  " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate , FileLogRepository.Level.INFO);
                var configDictionary = new Configuration().GetConfiguration(organizationid, secretkey, secretkeyid, runenvironment  );
                var clientConfig = new CyberSource.Client.Configuration(merchConfigDictObj: configDictionary);
                fileLog.Write("Starting Download for2:  " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate, FileLogRepository.Level.INFO);
 
                var apiInstance = new ReportDownloadsApi(clientConfig);
                fileLog.Write("Starting Download for3:  " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate, FileLogRepository.Level.INFO);
         
                var result = apiInstance.DownloadReportWithHttpInfo(reportdate, reportname, organizationid);

                Console.WriteLine(result);
                results = result.Data.ToString(); 
                
                // Archive the response
                File.WriteAllText(downloadFile1, result.Data.ToString() ); 
                File.WriteAllText(downloadFile2, generic.CreateXml(result.Data));

                // Load the csv data into a list dictionary file
                var CSVList = generic.LoadCsvAsDictionary(downloadFile1);

                // Check count of list dictionary
                if (CSVList.Count > 0 )  
                {
                    // Write the output to a flat file for TOLAS 
                        WriteTolasPB(CSVList, downloadFile3, reportdate ); 
                    
               }
                else
                {
                    FileTolas fileTolas = new FileTolas(downloadFile3);
                    fileTolas.Create() ;
                    fileLog.Write("Main Download (no valid rows of data, empty file created) for :  " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate, FileLogRepository.Level.INFO);
                }

                // Copy file to TOLAS folder. 
                File.Copy(downloadFile3, downloadfile4); 
                Console.WriteLine("\nFile downloaded at the below location:");
                fileLog.Write("Stopping Download for:  " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate, FileLogRepository.Level.INFO);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found: Kindly verify the path");
            }
            catch (Exception e)
            {
                string exmessage = e.Message.ToString(); 
                //Check if 404 message triggered. This indicates no data exists for that day. 
                if (exmessage.Contains("The requested resource is not found. Please try again later"))
                {
                    string testx = e.Message.ToString(); 
                    FileTolas fileTolas = new FileTolas(downloadFile3);
                    fileTolas.Create();
                    File.Copy(downloadFile3, downloadfile4); 
                    fileLog.Write("Stopping Download (EXC resource not found, empty file created) for :  " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate, FileLogRepository.Level.INFO);
                }
                else
                {
                    Console.WriteLine("Exception on calling the API: " + e.Message);
                    fileLog.Write("Stopping Download (Generic exception: " + e.Message + " ) for :   " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate, FileLogRepository.Level.INFO);
                    throw new System.Exception("Exception on calling the API: " + e.Message);
                }
            }
        }


        public void WriteTolasPB(List<Dictionary<string, string>> result, string tolasfile, string reportdate)
        {
            string merchantref = "";
            string batchdate = "";
            string requestid = "";
            string amount = "";
            string currencycode = "USD";
            string merchantid = "";
            string status = "";
            string batchid = "";
            string transactionref = "";
            string appname = "";
            string cardtype = ""; 

            FileTolas fileTolas = new FileTolas(tolasfile);
            FileLogRepository fileLog = new FileLogRepository(AppSetting.LogFolder, AppSetting.LogFilePrefix);
            fileLog.Write("WriteTolas:  " + "\n...List Count: " + result.Count.ToString(), FileLogRepository.Level.INFO);

            if (result.Count > 0)
            {

                foreach (var dictionary in result)
                {
                    foreach (var keyValue in dictionary)
                    {
                        if (keyValue.Key == "request_id")
                        //this is the first element, reset variable values
                       // { merchantref = ""; status = ""; batchid = ""; merchantid = ""; batchdate = ""; requestid = ""; merchantref = ""; transactionref = ""; amount = ""; appname = ""; cardtype = "";  }
                        if (keyValue.Key == "Status") { status  = keyValue.Value; }
                        if (keyValue.Key == "batch_id") { batchid  = keyValue.Value; }
                        if (keyValue.Key == "merchant_ref_number") { merchantref = keyValue.Value; }
                        if (keyValue.Key == "merchant_id") { merchantid = keyValue.Value; }
                        if (keyValue.Key == "Batch_Date")
                        {
                            batchdate = keyValue.Value;
                            batchdate = batchdate.Replace("Z", "-08:00");
                        }
                        if (keyValue.Key == "request_id") { requestid = keyValue.Value; }
                        if (keyValue.Key == "card_type") { cardtype  = keyValue.Value; }
                        if (keyValue.Key == "TransactionReferenceNumber") { transactionref = keyValue.Value; }
                        if (keyValue.Key == "amount") { amount = keyValue.Value; }
                        if (keyValue.Key == "ics_applications") 
                        { 
                           if (keyValue.Value.Contains("ics_credit"))
                            {appname = "ics_credit";}
                           else
                            {appname = "ics_bill";}
                        }
                      
                        // this is the last element, write to tolas file 
                        if (keyValue.Key == "currency")
                        {
                            currencycode = keyValue.Value; 
                            fileTolas.Write(batchid  + "|" + merchantid + "|" + reportdate   + "|" + requestid  + "|" + merchantref  + "|" + transactionref + "|" + cardtype  + "|" + currencycode  + "|" + amount  + "|" + appname  );
                            merchantid = ""; batchdate = ""; requestid = ""; merchantref = ""; transactionref = ""; amount = ""; appname = ""; cardtype = ""; currencycode = "";  ; 
                        }



                    }
                }
            }

        }

    }
}
