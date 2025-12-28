using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Cybersource_rest_samples_dotnet.Samples.Reporting.CoreServicesPB;
using Cybersource_rest_samples_dotnet.Samples.Reporting.CoreServicesTR;
using BT.TS360.Services.Cybersource.Common.FileLogging;
using BT.TS360.Services.Cybersource.Common.Configuration;
using BT.TS360.Services.Cybersource.Common;


//JS 2019-02-18  This project is used to download report files from cybersource. The core code is from cybersource.  This is what resides in the api and model folders. 
//               For future reference, this project uses nlog as a reference.  It looks like it is part of the dll authenticatesdk dll.  This project will not work if removed. 



namespace Cybersource_maincode
{
    public class MainCode
    {


        public static void Main(string[] args)
        {
            string reportdate_s;
            DateTime reportdate_d;
            Int32 daysback;
            string secretkey_btpp;
            string secretkey_bt;
            string secretkey_majors;
            string secretkeyid_btpp;
            string secretkeyid_bt;
            string secretkeyid_majors;
            string runenvironment;
            string programarg;
            string trflag = "true";
            string pbflag = "false";
            string trreportname = "";
            string pbreportname = "";

            FileLogRepository fileLog = new FileLogRepository(AppSetting.LogFolder, AppSetting.LogFilePrefix);

            try
            {



                fileLog.Write("Starting BTNextGen.Services.Cybersource Main", FileLogRepository.Level.INFO);

                //Check arguments supplied.  If none abort.  
                if (args.Length == 0)
                {
                    System.Console.WriteLine("Please enter an argument, none supplied");
                    fileLog.Write("Please enter an argument, none supplied", FileLogRepository.Level.INFO);
                    throw new System.InvalidProgramException("Please enter an argument, none supplied");
                }

                if (args.Length > 1)
                {
                    System.Console.WriteLine("Please provide only 1 argument");
                    fileLog.Write("Please provide only 1 argument", FileLogRepository.Level.INFO);
                    throw new System.InvalidProgramException("Please provide only 1 argument");
                }

                programarg = args.GetValue(0).ToString();

                //Create report date
                daysback = (Int32.Parse(AppSetting.DaysBack) * -1);
                reportdate_d = DateTime.Today.AddDays(daysback);
                reportdate_s = reportdate_d.ToString("yyyy-MM-dd");

                // Get secretkey and keyid
                secretkey_btpp = AppSetting.merchantsecretKey_btpp;
                secretkey_bt = AppSetting.merchantsecretKey_bt;
                secretkey_majors = AppSetting.merchantsecretKey_majors;
                secretkeyid_btpp = AppSetting.merchantKeyid_btpp;
                secretkeyid_bt = AppSetting.merchantKeyid_bt;
                secretkeyid_majors = AppSetting.merchantKeyid_majors;
                runenvironment = AppSetting.runEnvironment;
                trreportname = AppSetting.TRReportName;
                pbreportname = AppSetting.PBReportName;

                //TransactionRequestReport btpp
                if (AppSetting.RunTransactionRequest_btpp == "true" && programarg == "bt_pp")
                {
                    fileLog.Write("runtransaction btpp flagged to true, running", FileLogRepository.Level.INFO);
                    DownloadReportTR downloadReport = new DownloadReportTR();
                    downloadReport.Run("bt_pp", trreportname, reportdate_s, secretkey_btpp, secretkeyid_btpp, runenvironment);
                    trflag = "true";
                }

                //PaymentBatchDetailReport btpp
                if (AppSetting.RunPaymentBatchRequest_btpp == "true" && programarg == "bt_pp")
                {
                    fileLog.Write("runpayment btpp flagged to true, running", FileLogRepository.Level.INFO);
                    DownloadReportPB downloadReport = new DownloadReportPB();
                    downloadReport.Run("bt_pp", pbreportname, reportdate_s, secretkey_btpp, secretkeyid_btpp, runenvironment);
                    pbflag = "true";
                }



                //TransactionRequestReport bt
                if (AppSetting.RunTransactionRequest_bt == "true" && programarg == "bt")
                {
                    fileLog.Write("runtransaction bt flagged to true, running", FileLogRepository.Level.INFO);
                    DownloadReportTR downloadReport = new DownloadReportTR();
                    downloadReport.Run("bt", trreportname, reportdate_s, secretkey_bt, secretkeyid_bt, runenvironment);
                    trflag = "true";
                }

                //PaymentBatchDetail bt
                if (AppSetting.RunPaymentBatchRequest_bt == "true" && programarg == "bt")
                {
                    fileLog.Write("runpayment bt flagged to true, running", FileLogRepository.Level.INFO);
                    DownloadReportPB downloadReport = new DownloadReportPB();
                    downloadReport.Run("bt", pbreportname, reportdate_s, secretkey_bt, secretkeyid_bt, runenvironment);
                    pbflag = "true";
                }




                //TransactionRequestReport majors
                if (AppSetting.RunTransactionRequest_majors == "true" && programarg == "majors")
                {
                    fileLog.Write("runtransaction majors flagged to true, running", FileLogRepository.Level.INFO);
                    DownloadReportTR downloadReport = new DownloadReportTR();
                    downloadReport.Run("majors", trreportname, reportdate_s, secretkey_majors, secretkeyid_majors, runenvironment);
                    trflag = "true";

                }

                //PaymentBatchDetail majors
                if (AppSetting.RunPaymentBatchRequest_majors == "true" && programarg == "majors")
                {
                    fileLog.Write("runpayment majors flagged to true, running", FileLogRepository.Level.INFO);
                    DownloadReportPB downloadReport = new DownloadReportPB();
                    downloadReport.Run("majors", pbreportname, reportdate_s, secretkey_majors, secretkeyid_majors, runenvironment);
                    pbflag = "true";
                }



                fileLog.Write("Stopping BTNextGen.Services.Cybersource Main", FileLogRepository.Level.INFO);
                EmailSomebody.SendEmail("Cybersource Load Successful(" + programarg + ")" + "\n...TR: " + trflag + "\n...PB: " + pbflag, "Main", "true", null);

            }
            catch (Exception ex)
            {
                EmailSomebody.SendEmail("Cybersource Load Failure: " + ex.ToString(), "Main", "true", "2");
                fileLog.Write("Stopping Download (Generic exception: " + ex.Message, FileLogRepository.Level.INFO);

            }
        }



        private static void SetNetworkSettings()
        {
            // setting servicepointmanager configs for SSL/TSL / Proxy issues
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
        }
    }
}