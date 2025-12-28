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
using System.Collections.Generic;

using BT.TS360.Services.Cybersource.Common.Generic;
using CyberSource.Api;
using Cybersource_rest_samples_dotnetXXX;

namespace Cybersource_rest_samples_dotnet.Samples.Reporting.CoreServicesTR
{
    public class DownloadReportTR
    {
        public void Run(string organizationid, string reportname, string reportdate, string secretkey, string secretkeyid, string runenvironment)
        {
            // File will be created with the Data received in the Response Body
            string reportabbr = "";

            Generic generic = new Generic();

            reportabbr = "TR";


            string guid = Guid.NewGuid().ToString();
            string downloadFile1Path = AppSetting.ArchiveFolder;
            string downloadFile1Name = "Archive1-" + reportabbr + "_" + organizationid + "RawResponse_" + guid + ".txt";
            string downloadFile1 = downloadFile1Path + downloadFile1Name;
            string downloadFile2Name = "Archive2-" + reportabbr + "_" + organizationid + "XMLResponse_" + guid + ".txt";
            string downloadFile2 = downloadFile1Path + downloadFile2Name;
            string downloadFile3Name = "Archive3-" + reportabbr + "_" + organizationid + "TOLAS_" + guid + ".txt";
            string downloadFile3 = downloadFile1Path + downloadFile3Name;
            String downloadFile2Path = AppSetting.TolasFolderTR;
            string downloadfile4 = downloadFile2Path + downloadFile3Name;
            string downloadFile4Name = "Tolas-" + reportabbr + "_" + organizationid + "TOLAS_" + guid + ".txt";
            string results;
            FileLogRepository fileLog = new FileLogRepository(AppSetting.LogFolder, AppSetting.LogFilePrefix);

            try
            {
                fileLog.Write("Starting Download for:  " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate, FileLogRepository.Level.INFO);
                var configDictionary = new Configuration().GetConfiguration(organizationid, secretkey, secretkeyid, runenvironment);
                var clientConfig = new CyberSource.Client.Configuration(merchConfigDictObj: configDictionary);
                var apiInstance = new ReportDownloadsApi(clientConfig);
                var result = apiInstance.DownloadReportWithHttpInfo(reportdate, reportname, organizationid);

                Console.WriteLine(result);
                results = result.Data.ToString();

                // Archive the response
                File.WriteAllText(downloadFile1, result.Data.ToString());
                File.WriteAllText(downloadFile2, generic.CreateXml(result.Data));

                // Load the csv data into a list dictionary file
                var CSVList = generic.LoadCsvAsDictionary(downloadFile1);

                // Check count of list dictionary
                if (CSVList.Count > 0)
                {
                    WriteTolasTR(CSVList, downloadFile3);

                    // This path is necessary in the event we come out of writetolastr and we are unable to create a file. 
                    // This would happen if all of the lines in the report are subscriptions
                    if (!File.Exists(downloadFile3))
                    {
                        FileTolas fileTolas = new FileTolas(downloadFile3);
                        fileTolas.Create();
                        fileLog.Write("Main Download (no valid rows of data, empty file created) for :  " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate, FileLogRepository.Level.INFO);
                        //Removing, this happens already... File.Copy(downloadFile3, downloadfile4);

                    }

                }
                else
                {
                    FileTolas fileTolas = new FileTolas(downloadFile3);
                    fileTolas.Create();
                    fileLog.Write("Main Download (no downloaded rows of data, empty file created) for :  " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate, FileLogRepository.Level.INFO);
                }

                // Copy file to TOLAS folder. 
                File.Copy(downloadFile3, downloadfile4);

                Console.WriteLine("\nFile downloaded ... :");
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
                    fileLog.Write("Stopping Download (Generic exception: " + e.Message + " ) for :  " + "\n...org:" + organizationid + "\n...rpt:" + reportname + "\n...dte:" + reportdate, FileLogRepository.Level.INFO);
                    throw new System.Exception("Exception on calling the API: " + e.Message);
                }
            }
        }


        public void WriteTolasTR(List<Dictionary<string, string>> result, string tolasfile)
        {

            string merchantref = "";
            string requestdate = "";
            string requestid = "";
            string accountsuffix = "";
            string currencycode = "USD";
            string cardtype = "";
            string amount = "";
            string appname = "";
            string authcode = "";
            string merchantid = "";
            string authrcode = "";
            string billrcode = "";
            string credrcode = "";
            string rcode = "";
            string authauthavs = "";
            FileTolas fileTolas = new FileTolas(tolasfile);
            FileLogRepository fileLog = new FileLogRepository(AppSetting.LogFolder, AppSetting.LogFilePrefix);

            Generic generic = new Generic();

            fileLog.Write("WriteTolas:  " + "\n...List Count: " + result.Count.ToString(), FileLogRepository.Level.INFO);

            if (result.Count > 0)
            {

                foreach (var dictionary in result)
                {
                    foreach (var keyValue in dictionary)
                    {
                        if (keyValue.Key == "request_id")
                        //this is the first element, reset variable values
                        { merchantref = ""; requestdate = ""; requestid = ""; accountsuffix = ""; cardtype = ""; amount = ""; appname = ""; authcode = ""; merchantid = ""; rcode = ""; authrcode = ""; billrcode = ""; credrcode = ""; authauthavs = ""; }
                        if (keyValue.Key == "ics_rcode")
                        {
                            rcode = keyValue.Value;

                            appname = appname.Replace("\"", "");
                            var rcodehead = generic.SplitCsv(appname);
                            rcode = rcode.Replace(",\"", ", ");
                            rcode = rcode.Replace(",,", ", ,");
                            rcode = rcode.Replace("\"", "");
                            var rcodevals = generic.SplitCsv(rcode);

                            for (int i = 0; i < rcodehead.Count; i++)
                            {
                                if (rcodehead[i] == "ics_auth") { authrcode = rcodevals[i]; }
                                if (rcodehead[i] == ("ics_bill")) { billrcode = rcodevals[i]; }
                                if (rcodehead[i] == ("ics_credit")) { credrcode = rcodevals[i]; }
                            }

                        }
                        if (keyValue.Key == "merchant_ref_number") { merchantref = keyValue.Value; }
                        if (keyValue.Key == "transaction_date")
                        {

                            requestdate = keyValue.Value;
                            requestdate = requestdate.Replace("Z", "-08:00");
                        }

                        if (keyValue.Key == "request_id") { requestid = keyValue.Value; }
                        if (keyValue.Key == "account_no") { accountsuffix = keyValue.Value; }
                        if (keyValue.Key == "card_type") { cardtype = keyValue.Value; }
                        if (keyValue.Key == "amount") { amount = keyValue.Value; }
                        if (keyValue.Key == "ics_applications")
                        {
                            appname = keyValue.Value;

                        }
                        if (keyValue.Key == "auth_request_amount_currency") { currencycode = keyValue.Value; }
                        if (keyValue.Key == "auth_auth_avs") { authauthavs = keyValue.Value; }
                        if (keyValue.Key == "auth_code") { authcode = keyValue.Value; }
                        if (keyValue.Key == "merchant_id") { merchantid = keyValue.Value; }


                        // this is the last element, write to tolas file 
                        if (keyValue.Key == "LocalizedRequestDate")
                        {

                            {

                                appname = appname.Replace("\"", "");

                                fileTolas.Write(merchantref + "|" + requestdate + "|" + requestid + "|" + accountsuffix + "|" + cardtype + "|" + amount + "|" + currencycode + "|" + authauthavs + "|" + authrcode + "|" + credrcode + "|" + appname + "|" + authcode + "|" + merchantid);


                            }



                        }

                    }

                }

            }

        }
    }
}
