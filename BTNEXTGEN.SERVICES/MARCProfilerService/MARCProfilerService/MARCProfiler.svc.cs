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
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using BT.TS360.NoSQL.API.Models;
using Newtonsoft.Json;
using BT.TS360API.ServiceContracts; 
using BT.TS360Constants;


// reference "common" project
//using BTNextGen.Services.Common;
// 9/21/17     JS     Modified to get inventory data from mongo.  This includes adding new external and internal parameters. create routine to call mongo, call sql to get associated
//                    btkeys for a basket.  Aded a routine to build the call to sql as well. 

namespace MARCProfilerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class MARCProfiler : IMARCProfiler
    {
        //Constants
        #region Constants

        //HEX value for Subfield indicator
        public const char SUBFIELD_INDICATOR = '\x1F';
        public const string SUBFIELD_INDICATORFTP = "#####";

        //HEX value for End of Field
        public const char END_OF_FIELD = '\x1E';
        public const string END_OF_FIELDFTP = "@@@@@";

        //HEX value for End of Record
        public const char END_OF_RECORD = '\x1D';
        public const string END_OF_RECORDFTP = "$$$$$";

        //Length of the Directory
        public const int DIRECTORY_ENTRY_LEN = 12;

        //Length of the Leader
        public const int LEADER_LEN = 24;

        //Maximum record length
        public const int MAX_RECORD_LENGTH = 99999;

        #endregion

        public class Global
        {
            public static string FTP_LOGFILE = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Config.FTPLocalFolder + "\\" + Config.FTPLogFileName;
            public static string DOWNLOAD_LOGFILE = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Config.DOWNLOADLocalFolder + "\\" + Config.DOWNLOADLogFileName;
            public static string SP_LOGFILE = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Config.SPLocalFolder + "\\" + Config.SPLogFileName;
            public static string SP_MARCFILE = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Config.SPLocalFolder + "\\" + Guid.NewGuid().ToString() + Config.SPMarcFileName;

            public static string InventoryServiceUrl = ConfigurationManager.AppSettings["InventoryServiceUrl"].ToString();
            public static string txnID = Guid.NewGuid().ToString(); 

        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string GetMARCFileOld(Collection<BTKeySequence> btkeySeq, Int32 MARCProfileID, string Indiciator, String BasketSummaryID, bool isOrdered, bool isCancelled, bool isOCLCEnabled)
        {
            //string filePath = "Sample.mrc1";

            try
            {
                DataTable dtTableValue = new DataTable();
                DataTable dtTableMARCResults = new DataTable();
                dtTableValue.Columns.Add("BTKey", typeof(string));
                dtTableValue.Columns.Add("SortSequence", typeof(Int32));
                string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                StringBuilder sbMARCFile = new StringBuilder();
                string bkey = String.Empty;
                string returnTemp = String.Empty;


                foreach (BTKeySequence singleBTKey in btkeySeq)
                {
                    bkey = singleBTKey.btKey;
                    int seq = singleBTKey.sortSequence;
                    dtTableValue.Rows.Add(bkey, seq);

                }
                DataAccess da = new DataAccess();
                //da.InsertTVPRecords(dtTableValue, conn);
                dtTableMARCResults = da.GetMARCRecordsOld(dtTableValue, "strMARCProfileID", 'F', "BasketSummaryID", conn);

                //get distinct BTKey from the stored procedure resultset.
                DataTable dtDistinctBTKeys = dtTableMARCResults.DefaultView.ToTable(true, "BTKey");

                //loop through the unique btkeys
                foreach (DataRow drSingleBTKey in dtDistinctBTKeys.Rows)
                {

                    //Initialize the variables
                    String Prd_Type = String.Empty;
                    String category = String.Empty;
                    String strLeader = String.Empty;

                    bkey = drSingleBTKey[0].ToString();
                    String strData = String.Empty;
                    String strTag = String.Empty;
                    String strIndicator = String.Empty;
                    var BtKeys = from MARCRecord in dtTableMARCResults.AsEnumerable()
                                 where MARCRecord.Field<string>("BTKey") == bkey
                                 select MARCRecord;

                    DataTable dtMARCRecord = BtKeys.CopyToDataTable<DataRow>();

                    #region startdebug
                    //dtMARCRecord.TableName = "DistinctDataTable";
                    //System.IO.StringWriter writer1 = new System.IO.StringWriter();
                    //dtMARCRecord.WriteXml(writer1, XmlWriteMode.WriteSchema, false);
                    //string xmlFromDataTable = writer1.ToString();
                    //System.Diagnostics.EventLog.WriteEntry("xml", xmlFromDataTable);
                    #endregion

                    //Extract Leader

                    IEnumerable<DataRow> queryLeader = from r in dtMARCRecord.AsEnumerable()
                                                       where r.Field<string>("000Tag") != null
                                                       select r;

                    foreach (DataRow dr in queryLeader)
                    {
                        strLeader = dr.Field<string>("000Tag").ToString().Trim();
                        break;
                    }

                    //Extract ProductType,Category and Leader(if present)

                    //914-w for Books and 943-J for Entertainment
                    // if 001 has BK then Product is Book 
                    // If 001 has BE then Product is Entertainment

                    IEnumerable<DataRow> queryCategory = from r in dtMARCRecord.AsEnumerable()
                                                         where r.Field<string>("Tag") == "001"
                                                         select r;

                    foreach (DataRow dr in queryCategory)
                    {
                        category = dr.Field<string>("Data").ToString().Trim();
                        break;
                    }


                    if (category == null || category == String.Empty)
                    {
                        throw new Exception("Data for 001 is empty for " + bkey);

                    }
                    // Set the category depending on the 001 data
                    if (category.Contains("BK"))
                        category = "Books";
                    if (category.Contains("BE"))
                        category = "Entertainment";

                    // Extract ProductType
                    if (category == "Books")
                    {
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("914wTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("914wTag").ToString().Trim();
                            break;
                        }


                    }

                    else
                    {

                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("943jTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("943jTag").ToString().Trim();
                            break;
                        }

                    }

                    returnTemp = BuildMARCFile(dtMARCRecord, strLeader, Prd_Type);
                    sbMARCFile.Append(returnTemp);
                    dtMARCRecord = null;
                }


                //FileStream fs = new FileStream("c:\\output.mrc", FileMode.Create);
                //BinaryWriter writer = new BinaryWriter(fs, Encoding.Default);
                //writer.Write(sbMARCFile.ToString().ToCharArray());
                //writer.Close();
                //fs.Close();

                return sbMARCFile.ToString();

            }

            catch (FaultException faultEx)
            {
                throw new FaultException(faultEx.Message, new FaultCode(faultEx.Code.Name));
            }

            catch (Exception ex)
            {
                throw new FaultException(ex.Message, new FaultCode("GET_MARC_FILE"));
            }

        }

        public string GetMARCFile(String sortColumn, String basketSummaryID, String sortDirection, String ProfileID, string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, bool hasInventoryRules, string marketType)
        {
            //string filePath = "Sample.mrc1";

            try
            {
                DataTable dtTableValue = new DataTable();
                DataTable dtTableMARCResults = new DataTable();
                DataTable dtTableMarcBTKeyResults = new DataTable();
                DataTable dtTableMarcInventory = new DataTable(); 
                dtTableValue.Columns.Add("BTKey", typeof(string));
                dtTableValue.Columns.Add("SortSequence", typeof(Int32));
                string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                string connOrders = ConfigurationManager.ConnectionStrings["OrdersConnectionString"].ConnectionString;
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                StringBuilder sbMARCFile = new StringBuilder();
                string bkey = String.Empty;
                string returnTemp = String.Empty;



                WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "\r\n" + "***Parameters***" + "\r\n" +
                "sort   : " + sortColumn + "\r\n" +
                "bask id: " + basketSummaryID + "\r\n" +
                "prof id: " + ProfileID + "\r\n" +
                "sortdir: " + sortDirection + "\r\n" +
                "fullind: " + FullIndicator + "\r\n" +
                "isordrd: " + isOrdered + "\r\n" +
                "iscancl: " + isCancelled + "\r\n" +
                "isoclc : " + isOCLCEnabled + "\r\n" +
                "isbtemployee: " + isBTEmployee + "\r\n" + 
                "hasinventoryrule: " + hasInventoryRules + "\r\n" + 
                 "markettype: " + marketType, 
                 Config.LogDetails);

                //foreach (BTKeySequence singleBTKey in btkeySeq)
                //{
                //    bkey = singleBTKey.btKey;
                //    int seq = singleBTKey.sortSequence;
                //    dtTableValue.Rows.Add(bkey, seq);

                //}
                
                //new for 3.2.14 get inventory for the cart when flag is equal to true
                if (hasInventoryRules == true)
                {
                    WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "inventory route ", Config.LogDetails);
                    //call stored procedure to get the btkeys
                    DataAccess da5 = new DataAccess();
                    dtTableMarcBTKeyResults = da5.GetMARCRecordsBTKeys(basketSummaryID, connOrders);
                    if (dtTableMarcBTKeyResults.Rows.Count > 0)
                    {

                        //build the request to mongo
                        InventoryDemandRequest inventorydemandrequest = new InventoryDemandRequest();
                        InventoryDemandResponse inventorydemandresponse = new InventoryDemandResponse();
                        inventorydemandrequest = BuildMongoRequest(dtTableMarcBTKeyResults, marketType);

                        //call mongo inventory service
                        var response = GetInventoryResults(inventorydemandrequest);

                        //build the udt variable to the main marc stored procedure
                        dtTableMarcInventory = BuildInventoryRequest(response);


                     }
                     else 
                     {
                       //This probably is not necessary for prod, but dev triggers this a lot and was looking to avoid
                       WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "BasketSummary passed had not btkeys, bypassing get inventory", Config.LogDetails);
                       //create dummy result set so sql server doesn't trip out due to no data. 
                       dtTableMarcInventory.Columns.Add("Sequence", typeof(Int32));
                       dtTableMarcInventory.Columns.Add("BTKey", typeof(String));
                       dtTableMarcInventory.Columns.Add("Warehouse", typeof(String));
                       dtTableMarcInventory.Columns.Add("QuantityOnHand", typeof(Int32));
                       dtTableMarcInventory.Columns.Add("QuantityOnOrder", typeof(Int32));
                        
                     }

                    
                }
                else
                { 
                //create dummy result set so sql server doesn't trip out due to no data. 
                dtTableMarcInventory.Columns.Add("Sequence",typeof(Int32));
                dtTableMarcInventory.Columns.Add("BTKey", typeof(String));
                dtTableMarcInventory.Columns.Add("Warehouse", typeof(String));
                dtTableMarcInventory.Columns.Add("QuantityOnHand", typeof(Int32));
                dtTableMarcInventory.Columns.Add("QuantityOnOrder", typeof(Int32));

                WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "no inventory route " , Config.LogDetails);


                }

                //foreach (DataRow drmarcrows in dtTableMarcInventory.Rows)
                //{
                //    string xxxxx  = drmarcrows[0].ToString();
                //    string xxxxx2 = drmarcrows[1].ToString();
                //    string xxxxx3 = drmarcrows[2].ToString();

                    
                //    WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "row 1 " + xxxxx, Config.LogDetails);
                //    WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "row 2" + xxxxx2, Config.LogDetails);
                //    WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "row 3" + xxxxx3, Config.LogDetails);

                //}

 

                DataAccess da = new DataAccess();
                //da.InsertTVPRecords(dtTableValue, conn);
                dtTableMARCResults = da.GetMARCRecords(sortColumn, basketSummaryID, sortDirection, ProfileID, FullIndicator, isOrdered, isCancelled, isOCLCEnabled, isBTEmployee, dtTableMarcInventory, conn);


                //var distinctBTKeys = (from row in dtTableMARCResults.AsEnumerable()
                //select row.Field<string>("BTKeys")).Distinct();

                //get distinct BTKey from the stored procedure resultset.
                DataTable dtDistinctBTKeys = dtTableMARCResults.DefaultView.ToTable(true, "BTKey");

                //loop through the unique btkeys
                foreach (DataRow drSingleBTKey in dtDistinctBTKeys.Rows)
                {
                    //Initialize the variables
                    String Prd_Type = String.Empty;
                    String category = String.Empty;
                    String strLeader = String.Empty;

                    bkey = drSingleBTKey[0].ToString();
                    String strData = String.Empty;
                    String strTag = String.Empty;
                    String strIndicator = String.Empty;
                    //var BtKeys = from MARCRecord in dtTableMARCResults.AsEnumerable()
                    //             where MARCRecord.Field<string>("BTKey") == bkey
                    //             select MARCRecord;
                    //
                    //DataTable dtMARCRecord = BtKeys.CopyToDataTable<DataRow>();
                    string expressionsql = "BTKey = '" + bkey + "'";
                    DataTable dtMARCRecord = dtTableMARCResults.Select(expressionsql).CopyToDataTable();


                    #region startdebug
                    //dtMARCRecord.TableName = "DistinctDataTable";
                    //System.IO.StringWriter writer1 = new System.IO.StringWriter();
                    //dtMARCRecord.WriteXml(writer1, XmlWriteMode.WriteSchema, false);
                    //string xmlFromDataTable = writer1.ToString();
                    //System.Diagnostics.EventLog.WriteEntry("xml", xmlFromDataTable);
                    #endregion

                    //Extract Leader

                    IEnumerable<DataRow> queryLeader = from r in dtMARCRecord.AsEnumerable()
                                                       where r.Field<string>("000Tag") != null
                                                       select r;

                    foreach (DataRow dr in queryLeader)
                    {
                        strLeader = dr.Field<string>("000Tag").ToString().Trim();
                        break;
                    }

                    //Extract ProductType,Category and Leader(if present)

                    //914-w for Books and 943-J for Entertainment
                    // if 001 has BK then Product is Book 
                    // If 001 has BE then Product is Entertainment

                    IEnumerable<DataRow> queryCategory = from r in dtMARCRecord.AsEnumerable()
                                                         where r.Field<string>("Tag") == "001"
                                                         select r;

                    foreach (DataRow dr in queryCategory)
                    {
                        category = dr.Field<string>("Data").ToString().Trim();
                        break;
                    }


                    //if (category == null || category == String.Empty)
                    //{
                    //    throw new Exception("Data for 001 is empty for " + bkey);
                    //
                    //}
                    // Set the category depending on the 001 data
                    if (category.Contains("BK"))
                        category = "Books";
                    if (category.Contains("BE"))
                        category = "Entertainment";

                    // Extract ProductType
                    if (category == "Books")
                    {
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("914wTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("914wTag").ToString().Trim();
                            break;
                        }
                    }
                    else if (category == "Entertainment")
                    {
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("943jTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("943jTag").ToString().Trim();
                            break;
                        }

                    }
                    //CHECK BOOK 914 TAG WHEN CATEGORY IS NULL
                    else
                    {
                        //WriteLogFile(Global.DOWNLOAD_LOGFILE, "category missing - checking 914", "", "true");
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("914wTag") != null 
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("914wTag").ToString().Trim();
                            break;
                        }

                    }

                    //CHECK ENTERTAINMENT 943 TAG WHEN PROD TYPE IS NULL
                    if (Prd_Type == null || Prd_Type == String.Empty)
                    {
                        //WriteLogFile(Global.DOWNLOAD_LOGFILE, "category missing - checking 943", "", "true");
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("943jTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("943jTag").ToString().Trim();
                            break;
                        }


                    }   


                    returnTemp = BuildMARCFile(dtMARCRecord, strLeader, Prd_Type);
                    sbMARCFile.Append(returnTemp);
                    dtMARCRecord = null;
                }

                //FOR VIEWING FILE - TESTING
                //FileStream fs = new FileStream("c:\\output.mrc", FileMode.Create);
                //BinaryWriter writer = new BinaryWriter(fs, Encoding.Default);
                //writer.Write(sbMARCFile.ToString().ToCharArray());


                //writer.Close();
                // fs.Close();
                //FOR VIEWING FILE - TESTING


                // this one works
                //ONLY DOING THIS NOW IN THE NEW METHOD
                //sbMARCFile = sbMARCFile.Replace("\u001F", "######");
                return sbMARCFile.ToString();



            }

            catch (FaultException faultEx)
            {
                WriteLogFile(Global.DOWNLOAD_LOGFILE, "GET_MARC_FILE_FTP Fault: ", faultEx.Message.ToString(), "true");

                SendEmailException("GET_MARC_FILE Fault: " + faultEx.Message.ToString(), "GETMarcFile", "true");

                WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "\r\n" + "***Parameters***" + "\r\n" +
                "sort   : " + sortColumn + "\r\n" +
                "bask id: " + basketSummaryID + "\r\n" +
                "prof id: " + ProfileID + "\r\n" +
                "sortdir: " + sortDirection + "\r\n" +
                "fullind: " + FullIndicator + "\r\n" +
                "isordrd: " + isOrdered + "\r\n" +
                "iscancl: " + isCancelled + "\r\n" +
                "isoclc : " + isOCLCEnabled + "\r\n" +
                "isbtemployee: " + isBTEmployee,
                Config.LogDetails);

                throw new FaultException(faultEx.Message, new FaultCode(faultEx.Code.Name));
            }

            catch (Exception ex)
            {
                WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "\r\n" + "***Parameters***" + "\r\n" +
                "sort   : " + sortColumn + "\r\n" +
                "bask id: " + basketSummaryID + "\r\n" +
                "prof id: " + ProfileID + "\r\n" +
                "sortdir: " + sortDirection + "\r\n" +
                "fullind: " + FullIndicator + "\r\n" +
                "isordrd: " + isOrdered + "\r\n" +
                "iscancl: " + isCancelled + "\r\n" +
                "isoclc : " + isOCLCEnabled + "\r\n" +
                "isbtemployee: " + isBTEmployee,
                Config.LogDetails);

                WriteLogFile(Global.DOWNLOAD_LOGFILE, "GET_MARC_FILE_FTP Fault: ", ex.Message.ToString(), "true");

                SendEmailException("GET_MARC_FILE Fault: " + ex.Message.ToString(), "GETMarcFile", "true");

                bool custom001;
                custom001 = ex.Message.Contains("Data for 001 is empty for");
                if (custom001)
                { throw new FaultException(ex.Message, new FaultCode("1001")); }
                else
                { throw new FaultException(ex.Message, new FaultCode("GET_MARC_FILE")); }
            }

        }

        public void SendMARCFileFTP(String sortColumn, String basketSummaryID, String sortDirection, String ProfileID, string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, bool hasInventoryRules, string marketType, string FTPServer, string FTPUserID, string FTPPassword, string FTPFolder, string FTPFilePrefix, string TS360UserID, string TS360CartName)
        {
            //string filePath = "Sample.mrc1";
            string connOrders = ConfigurationManager.ConnectionStrings["OrdersConnectionString"].ConnectionString;
            try
            {
                DataTable dtTableValue = new DataTable();
                DataTable dtTableMARCResults = new DataTable();
                DataTable dtTableMarcBTKeyResults = new DataTable();
                DataTable dtTableMarcInventory = new DataTable(); 
                dtTableValue.Columns.Add("BTKey", typeof(string));
                dtTableValue.Columns.Add("SortSequence", typeof(Int32));
                string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                StringBuilder sbMARCFile = new StringBuilder();
                string bkey = String.Empty;
                string returnTemp = String.Empty;


                //foreach (BTKeySequence singleBTKey in btkeySeq)
                //{
                //    bkey = singleBTKey.btKey;
                //    int seq = singleBTKey.sortSequence;
                //    dtTableValue.Rows.Add(bkey, seq);

                //}

                WriteLogFile(Global.FTP_LOGFILE, "FTP", "\r\n" + "***Parameters***" + "\r\n" +
                "sort   : " + sortColumn + "\r\n" +
                "bask id: " + basketSummaryID + "\r\n" +
                "prof id: " + ProfileID + "\r\n" +
                "sortdir: " + sortDirection + "\r\n" +
                "fullind: " + FullIndicator + "\r\n" +
                "isordrd: " + isOrdered + "\r\n" +
                "iscancl: " + isCancelled + "\r\n" +
                "isoclc : " + isOCLCEnabled + "\r\n" +
                "isbtemployee: " + isBTEmployee + "\r\n" +
                "hasinventoryrule: " + hasInventoryRules + "\r\n" +
                "markettype: " + marketType + "\r\n" +
                "FTPsrvr: " + FTPServer + "\r\n" +
                "FTPuser: " + FTPUserID + "\r\n" +
                "FTPpass: " + FTPPassword + "\r\n" +
                "FTPfold: " + FTPFolder + "\r\n" +
                "FTPpref: " + FTPFilePrefix + "\r\n" +
                "TS360userid: " + TS360UserID + "\r\n" +
                "TS360cart: " + TS360CartName, Config.LogDetails);


                //new for 3.2.14 get inventory for the cart when flag is equal to true
                if (hasInventoryRules == true)
                {
                    WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "inventory route ", Config.LogDetails);
                    //call stored procedure to get the btkeys
                    DataAccess da5 = new DataAccess();
                    dtTableMarcBTKeyResults = da5.GetMARCRecordsBTKeys(basketSummaryID, connOrders);
                    if (dtTableMarcBTKeyResults.Rows.Count > 0)
                    {

                        //build the request to mongo
                        InventoryDemandRequest inventorydemandrequest = new InventoryDemandRequest();
                        InventoryDemandResponse inventorydemandresponse = new InventoryDemandResponse();
                        inventorydemandrequest = BuildMongoRequest(dtTableMarcBTKeyResults, marketType);

                        //call mongo inventory service
                        var response = GetInventoryResults(inventorydemandrequest);

                        //build the udt variable to the main marc stored procedure
                        dtTableMarcInventory = BuildInventoryRequest(response);


                    }
                    else
                    {
                        //This probably is not necessary for prod, but dev triggers this a lot and was looking to avoid
                        WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "BasketSummary passed had not btkeys, bypassing get inventory", Config.LogDetails);
                        //create dummy result set so sql server doesn't trip out due to no data. 
                        dtTableMarcInventory.Columns.Add("Sequence", typeof(Int32));
                        dtTableMarcInventory.Columns.Add("BTKey", typeof(String));
                        dtTableMarcInventory.Columns.Add("Warehouse", typeof(String));
                        dtTableMarcInventory.Columns.Add("QuantityOnHand", typeof(Int32));
                        dtTableMarcInventory.Columns.Add("QuantityOnOrder", typeof(Int32));

                    }


                }
                else
                {
                    //create dummy result set so sql server doesn't trip out due to no data. 
                    dtTableMarcInventory.Columns.Add("Sequence", typeof(Int32));
                    dtTableMarcInventory.Columns.Add("BTKey", typeof(String));
                    dtTableMarcInventory.Columns.Add("Warehouse", typeof(String));
                    dtTableMarcInventory.Columns.Add("QuantityOnHand", typeof(Int32));
                    dtTableMarcInventory.Columns.Add("QuantityOnOrder", typeof(Int32));

                    WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "no inventory route ", Config.LogDetails);


                }




                DataAccess da = new DataAccess();
                //da.InsertTVPRecords(dtTableValue, conn);
                dtTableMARCResults = da.GetMARCRecordsFTP(sortColumn, basketSummaryID, sortDirection, ProfileID, FullIndicator, isOrdered, isCancelled, isOCLCEnabled, isBTEmployee, dtTableMarcInventory, conn);

                //WriteLogFile(Global.FTP_LOGFILE, "10 ", "10", "true");

                //var distinctBTKeys = (from row in dtTableMARCResults.AsEnumerable()
                //select row.Field<string>("BTKeys")).Distinct();

                //get distinct BTKey from the stored procedure resultset.
                DataTable dtDistinctBTKeys = dtTableMARCResults.DefaultView.ToTable(true, "BTKey");

                //loop through the unique btkeys
                foreach (DataRow drSingleBTKey in dtDistinctBTKeys.Rows)
                {
                    //Initialize the variables
                    String Prd_Type = String.Empty;
                    String category = String.Empty;
                    String strLeader = String.Empty;

                    bkey = drSingleBTKey[0].ToString();
                    String strData = String.Empty;
                    String strTag = String.Empty;
                    String strIndicator = String.Empty;
                    //var BtKeys = from MARCRecord in dtTableMARCResults.AsEnumerable()
                    //             where MARCRecord.Field<string>("BTKey") == bkey
                    //             select MARCRecord;
                    //DataTable dtMARCRecord = BtKeys.CopyToDataTable<DataRow>();

                    string expressionsql = "BTKey = '" + bkey + "'";
                    DataTable dtMARCRecord = dtTableMARCResults.Select(expressionsql).CopyToDataTable();


                    #region startdebug
                    //dtMARCRecord.TableName = "DistinctDataTable";
                    //System.IO.StringWriter writer1 = new System.IO.StringWriter();
                    //dtMARCRecord.WriteXml(writer1, XmlWriteMode.WriteSchema, false);
                    //string xmlFromDataTable = writer1.ToString();
                    //System.Diagnostics.EventLog.WriteEntry("xml", xmlFromDataTable);
                    #endregion

                    //Extract Leader

                    IEnumerable<DataRow> queryLeader = from r in dtMARCRecord.AsEnumerable()
                                                       where r.Field<string>("000Tag") != null
                                                       select r;

                    foreach (DataRow dr in queryLeader)
                    {
                        strLeader = dr.Field<string>("000Tag").ToString().Trim();
                        break;
                    }

                    //Extract ProductType,Category and Leader(if present)

                    //914-w for Books and 943-J for Entertainment
                    // if 001 has BK then Product is Book 
                    // If 001 has BE then Product is Entertainment

                    IEnumerable<DataRow> queryCategory = from r in dtMARCRecord.AsEnumerable()
                                                         where r.Field<string>("Tag") == "001"
                                                         select r;

                    foreach (DataRow dr in queryCategory)
                    {
                        category = dr.Field<string>("Data").ToString().Trim();
                        break;
                    }


                    //if (category == null || category == String.Empty)
                    //{
                    //    throw new Exception("Data for 001 is empty for " + bkey);
                    //
                    //}
                    // Set the category depending on the 001 data
                    if (category.Contains("BK"))
                        category = "Books";
                    if (category.Contains("BE"))
                        category = "Entertainment";

                    // Extract ProductType
                    if (category == "Books")
                    {
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("914wTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("914wTag").ToString().Trim();
                            break;
                        }
                    }
                    else if (category == "Entertainment")

                    {
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("943jTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("943jTag").ToString().Trim();
                            break;
                        }

                    }
                    //CHECK BOOK 914 TAG WHEN CATEGORY IS NULL
                    else
                    {
                        //WriteLogFile(Global.FTP_LOGFILE, "category missing - checking 914", "", "true");
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("914wTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("914wTag").ToString().Trim();
                            break;
                        }

                    }

                    //CHECK ENTERTAINMENT 943 TAG WHEN PROD TYPE IS NULL
                    if (Prd_Type == null || Prd_Type == String.Empty)
                    {
                        //WriteLogFile(Global.FTP_LOGFILE, "category missing - checking 943", "", "true");
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("943jTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("943jTag").ToString().Trim();
                            break;
                        }


                    }   

                    returnTemp = BuildMARCFileFTP(dtMARCRecord, strLeader, Prd_Type, basketSummaryID, connOrders);
                    sbMARCFile.Append(returnTemp);
                    dtMARCRecord = null;
                }



                //Assign folder and file name variables
                string FTPGUID = Guid.NewGuid().ToString();
                string FTPFileName = "marc-" + FTPGUID + ".mrc";
                string XMLFileName = "marc-" + FTPGUID + ".xml";

                string FTPDropoffFolder = Config.FTPDropoffFolder;

                //Write out to c drive in case web.config is missing entry
                if (FTPDropoffFolder == null)
                {
                    FTPDropoffFolder = "C:\\";
                }
                string FTPDropoffFolderFileName = FTPDropoffFolder + FTPFileName;
                string XMLDropoffFolderFileName = FTPDropoffFolder + XMLFileName;

                // Write out settings file
                XmlTextWriter xwriter = new XmlTextWriter(XMLDropoffFolderFileName, null);
                xwriter.WriteStartDocument(true);
                xwriter.Formatting = System.Xml.Formatting.Indented;
                xwriter.Indentation = 2;
                xwriter.WriteStartElement("MarcFTP");
                xwriter.WriteStartElement("Server");
                xwriter.WriteString(FTPServer);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("UserID");
                xwriter.WriteString(FTPUserID);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("Password");
                xwriter.WriteString(FTPPassword);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("Folder");
                xwriter.WriteString(FTPFolder);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("FilePrefix");
                xwriter.WriteString(FTPFilePrefix);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("TS360UserID");
                xwriter.WriteString(TS360UserID);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("TS360CartName");
                xwriter.WriteString(TS360CartName);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("BasketSummaryID");
                xwriter.WriteString(basketSummaryID);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("FTPProfileID");
                xwriter.WriteString("");
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("Source");
                xwriter.WriteString("");
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("MarcProfileID");
                xwriter.WriteString(ProfileID );
                xwriter.WriteEndDocument();
                xwriter.Close();

                //Write out marc file
                FileStream fs = new FileStream(FTPDropoffFolderFileName, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(fs, Encoding.Default);
                writer.Write(sbMARCFile.ToString().ToCharArray());

                writer.Close();
                fs.Close();



            }

            catch (FaultException faultEx)
            {
                //throw new FaultException(faultEx.Message, new FaultCode(faultEx.Code.Name));

                DataAccess da = new DataAccess();
                //Update stored procedure of failure;
                da.UpdateMARCFTPFailure(basketSummaryID, faultEx.Message, "ok", connOrders);
                WriteLogFile(Global.FTP_LOGFILE, "SEND_MARC_FILE_FTP Fault: ", faultEx.Message.ToString(), "true");

                SendEmailException("SEND_MARC_FILE_FTP Fault: " + faultEx.Message.ToString(), "SendMarcFileFTP", "true");
                throw new FaultException(faultEx.Message, new FaultCode(faultEx.Code.Name));

            }

            catch (Exception ex)
            {
                DataAccess da = new DataAccess();
                da.UpdateMARCFTPFailure(basketSummaryID, ex.Message, "ok", connOrders);
                WriteLogFile(Global.FTP_LOGFILE, "SEND_MARC_FILE_FTP Fault: ", ex.Message.ToString(), "true");

                SendEmailException("SEND_MARC_FILE_FTP Fault: " + ex.Message.ToString(), "SendMarcFileFTP", "true");

                throw new FaultException(ex.Message, new FaultCode("GET_MARC_FILE"));
            }

        }

        public void SendMARCFileFTPTest(String FTPServer, String FTPUserID, String FTPPassword, String FTPFolder, String TS360UserID, String MarcProfilerID)
        {
            //string filePath = "Sample.mrc1";
            string connOrders = ConfigurationManager.ConnectionStrings["ProductCatalogConnectionString"].ConnectionString;
            try
            {
                DataTable dtTableValue = new DataTable();
                DataTable dtTableMARCResults = new DataTable();
                dtTableValue.Columns.Add("BTKey", typeof(string));
                dtTableValue.Columns.Add("SortSequence", typeof(Int32));
                string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                StringBuilder sbMARCFile = new StringBuilder();
                string bkey = String.Empty;
                string returnTemp = String.Empty;


                //foreach (BTKeySequence singleBTKey in btkeySeq)
                //{
                //    bkey = singleBTKey.btKey;
                //    int seq = singleBTKey.sortSequence;
                //    dtTableValue.Rows.Add(bkey, seq);

                //}

                WriteLogFile(Global.FTP_LOGFILE, "FTP", "\r\n" + "***Parameters***" + "\r\n" +
                "FTPsrvr: " + FTPServer + "\r\n" +
                "FTPuser: " + FTPUserID + "\r\n" +
                "FTPpass: " + FTPPassword + "\r\n" +
                "FTPfold: " + FTPFolder + "\r\n" +
                "MarcProfileID: " + MarcProfilerID + "\r\n" +
                "TS360userid: " + TS360UserID, Config.LogDetails);

                //Assign folder and file name variables
                string FTPGUID = Guid.NewGuid().ToString();
                string FTPFileName = "marc-" + FTPGUID + ".mrc";
                string XMLFileName = "marc-" + FTPGUID + ".xml";

                string FTPDropoffFolder = Config.FTPDropoffFolder;

                //Write out to c drive in case web.config is missing entry
                if (FTPDropoffFolder == null)
                {
                    FTPDropoffFolder = "C:\\";
                }
                string FTPDropoffFolderFileName = FTPDropoffFolder + FTPFileName;
                string XMLDropoffFolderFileName = FTPDropoffFolder + XMLFileName;

                // Write out settings file
                XmlTextWriter xwriter = new XmlTextWriter(XMLDropoffFolderFileName, null);
                xwriter.WriteStartDocument(true);
                xwriter.Formatting = System.Xml.Formatting.Indented;
                xwriter.Indentation = 2;
                xwriter.WriteStartElement("MarcFTP");
                xwriter.WriteStartElement("Server");
                xwriter.WriteString(FTPServer);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("UserID");
                xwriter.WriteString(FTPUserID);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("Password");
                xwriter.WriteString(FTPPassword);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("Folder");
                xwriter.WriteString(FTPFolder);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("FilePrefix");
                xwriter.WriteString("marctest");
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("TS360UserID");
                xwriter.WriteString(TS360UserID);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("TS360CartName");
                xwriter.WriteString("");
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("BasketSummaryID");
                xwriter.WriteString("");
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("FTPProfileID");
                xwriter.WriteString(MarcProfilerID);
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("Source");
                xwriter.WriteString("TEST");
                xwriter.WriteEndElement();
                xwriter.WriteStartElement("MarcProfileID");
                xwriter.WriteString("");
                xwriter.WriteEndDocument();
                xwriter.Close();

                //Write out marc file

                System.IO.File.Copy(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Config.MarcFTPTESTSourceFile, FTPDropoffFolderFileName);




            }

            catch (FaultException faultEx)
            {
                //throw new FaultException(faultEx.Message, new FaultCode(faultEx.Code.Name));

                DataAccess da = new DataAccess();
                //Update stored procedure of failure;
                da.UpdateMARCFTPFailureTEST(MarcProfilerID , faultEx.Message, 3, connOrders);
                WriteLogFile(Global.FTP_LOGFILE, "SEND_MARC_FILE_FTP Fault: ", faultEx.Message.ToString(), "true");

                SendEmailException("SEND_MARC_FILE_FTP Fault: " + faultEx.Message.ToString(), "SendMarcFileFTP", "true");
                throw new FaultException(faultEx.Message, new FaultCode(faultEx.Code.Name));

            }

            catch (Exception ex)
            {
                DataAccess da = new DataAccess();
                da.UpdateMARCFTPFailureTEST(MarcProfilerID, ex.Message, 3, connOrders);
                WriteLogFile(Global.FTP_LOGFILE, "SEND_MARC_FILE_FTP Fault: ", ex.Message.ToString(), "true");

                SendEmailException("SEND_MARC_FILE_FTP Fault: " + ex.Message.ToString(), "SendMarcFileFTP", "true");

                throw new FaultException(ex.Message, new FaultCode("GET_MARC_FILE"));
            }

        }



        public void SendMARCFileSP(String sortColumn, String basketSummaryID, String sortDirection, String ProfileID, string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, bool hasInventoryRules, string marketType, string TS360UserID, string TS360CartName, string ProfileName)
        {
            WriteLogFile(Global.SP_LOGFILE, "Download", "\r\n" + "***Parameters***" + "\r\n" +
               "sort   : " + sortColumn + "\r\n" +
               "bask id: " + basketSummaryID + "\r\n" +
               "prof id: " + ProfileID + "\r\n" +
               "sortdir: " + sortDirection + "\r\n" +
               "fullind: " + FullIndicator + "\r\n" +
               "isordrd: " + isOrdered + "\r\n" +
               "iscancl: " + isCancelled + "\r\n" +
               "isoclc : " + isOCLCEnabled + "\r\n" +
               "isbtemployee: " + isBTEmployee + "\r\n" +
               "hasinventoryrule: " + hasInventoryRules + "\r\n" +
               "markettype: " + marketType + "\r\n" +
               "ts360userid: " + TS360UserID + "\r\n" +
               "cartname: " + TS360CartName + "\r\n" +
               "profilename: " + ProfileName,
               Config.LogDetails);

            try
            {
                DataTable dtTableValue = new DataTable();
                DataTable dtTableMARCResults = new DataTable();
                DataTable dtTableMarcInventory = new DataTable();
                DataTable dtTableMarcBTKeyResults = new DataTable();
                dtTableValue.Columns.Add("BTKey", typeof(string));
                dtTableValue.Columns.Add("SortSequence", typeof(Int32));
                string conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                string connOrders = ConfigurationManager.ConnectionStrings["OrdersConnectionString"].ConnectionString;
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                StringBuilder sbMARCFile = new StringBuilder();
                string bkey = String.Empty;
                string returnTemp = String.Empty;
                string xxx = string.Empty;
                string zzz = string.Empty;


                WriteLogFile(Global.SP_LOGFILE, "Download", "\r\n" + "***Parameters***" + "\r\n" +
                "sort   : " + sortColumn + "\r\n" +
                "bask id: " + basketSummaryID + "\r\n" +
                "prof id: " + ProfileID + "\r\n" +
                "sortdir: " + sortDirection + "\r\n" +
                "fullind: " + FullIndicator + "\r\n" +
                "isordrd: " + isOrdered + "\r\n" +
                "iscancl: " + isCancelled + "\r\n" +
                "isoclc : " + isOCLCEnabled + "\r\n" +
                "isbtemployee: " + isBTEmployee +"\r\n" +
                "isbtemployee: " + TS360UserID + "\r\n" +
                "cartname: " + TS360CartName + "\r\n" +
                "profilename: " + ProfileName,
                Config.LogDetails);


                //new for 3.2.14 get inventory for the cart when flag is equal to true
                if (hasInventoryRules == true)
                {
                    WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "inventory route ", Config.LogDetails);
                    //call stored procedure to get the btkeys
                    DataAccess da5 = new DataAccess();
                    dtTableMarcBTKeyResults = da5.GetMARCRecordsBTKeys(basketSummaryID, connOrders);
                    if (dtTableMarcBTKeyResults.Rows.Count > 0)
                    {

                        //build the request to mongo
                        InventoryDemandRequest inventorydemandrequest = new InventoryDemandRequest();
                        InventoryDemandResponse inventorydemandresponse = new InventoryDemandResponse();
                        inventorydemandrequest = BuildMongoRequest(dtTableMarcBTKeyResults, marketType);

                        //call mongo inventory service
                        var response = GetInventoryResults(inventorydemandrequest);

                        //build the udt variable to the main marc stored procedure
                        dtTableMarcInventory = BuildInventoryRequest(response);


                    }
                    else
                    {
                        //This probably is not necessary for prod, but dev triggers this a lot and was looking to avoid
                        WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "BasketSummary passed had not btkeys, bypassing get inventory", Config.LogDetails);
                        //create dummy result set so sql server doesn't trip out due to no data. 
                        dtTableMarcInventory.Columns.Add("Sequence", typeof(Int32));
                        dtTableMarcInventory.Columns.Add("BTKey", typeof(String));
                        dtTableMarcInventory.Columns.Add("Warehouse", typeof(String));
                        dtTableMarcInventory.Columns.Add("QuantityOnHand", typeof(Int32));
                        dtTableMarcInventory.Columns.Add("QuantityOnOrder", typeof(Int32));

                    }


                }
                else
                {
                    //create dummy result set so sql server doesn't trip out due to no data. 
                    dtTableMarcInventory.Columns.Add("Sequence", typeof(Int32));
                    dtTableMarcInventory.Columns.Add("BTKey", typeof(String));
                    dtTableMarcInventory.Columns.Add("Warehouse", typeof(String));
                    dtTableMarcInventory.Columns.Add("QuantityOnHand", typeof(Int32));
                    dtTableMarcInventory.Columns.Add("QuantityOnOrder", typeof(Int32));

                    WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "no inventory route ", Config.LogDetails);


                }


                //foreach (BTKeySequence singleBTKey in btkeySeq)
                //{
                //    bkey = singleBTKey.btKey;
                //    int seq = singleBTKey.sortSequence;
                //    dtTableValue.Rows.Add(bkey, seq);

                //}
                DataAccess da = new DataAccess();
                //da.InsertTVPRecords(dtTableValue, conn);
                dtTableMARCResults = da.GetMARCRecords(sortColumn, basketSummaryID, sortDirection, ProfileID, FullIndicator, isOrdered, isCancelled, isOCLCEnabled, isBTEmployee, dtTableMarcInventory, conn);


                //var distinctBTKeys = (from row in dtTableMARCResults.AsEnumerable()
                //select row.Field<string>("BTKeys")).Distinct();

                //get distinct BTKey from the stored procedure resultset.
                DataTable dtDistinctBTKeys = dtTableMARCResults.DefaultView.ToTable(true, "BTKey");

                //loop through the unique btkeys
                foreach (DataRow drSingleBTKey in dtDistinctBTKeys.Rows)
                {
                    //Initialize the variables
                    String Prd_Type = String.Empty;
                    String category = String.Empty;
                    String strLeader = String.Empty;

                    bkey = drSingleBTKey[0].ToString();
                    String strData = String.Empty;
                    String strTag = String.Empty;
                    String strIndicator = String.Empty;
                    //var BtKeys = from MARCRecord in dtTableMARCResults.AsEnumerable()
                    //             where MARCRecord.Field<string>("BTKey") == bkey
                    //             select MARCRecord;
                    //DataTable dtMARCRecord = BtKeys.CopyToDataTable<DataRow>();

                    string expressionsql = "BTKey = '" + bkey + "'";
                    DataTable dtMARCRecord = dtTableMARCResults.Select(expressionsql).CopyToDataTable();


                    #region startdebug
                    //dtMARCRecord.TableName = "DistinctDataTable";
                    //System.IO.StringWriter writer1 = new System.IO.StringWriter();
                    //dtMARCRecord.WriteXml(writer1, XmlWriteMode.WriteSchema, false);
                    //string xmlFromDataTable = writer1.ToString();
                    //System.Diagnostics.EventLog.WriteEntry("xml", xmlFromDataTable);
                    #endregion

                    //Extract Leader

                    IEnumerable<DataRow> queryLeader = from r in dtMARCRecord.AsEnumerable()
                                                       where r.Field<string>("000Tag") != null
                                                       select r;

                    foreach (DataRow dr in queryLeader)
                    {
                        strLeader = dr.Field<string>("000Tag").ToString().Trim();
                        break;
                    }

                    //Extract ProductType,Category and Leader(if present)

                    //914-w for Books and 943-J for Entertainment
                    // if 001 has BK then Product is Book 
                    // If 001 has BE then Product is Entertainment

                    IEnumerable<DataRow> queryCategory = from r in dtMARCRecord.AsEnumerable()
                                                         where r.Field<string>("Tag") == "001"
                                                         select r;

                    foreach (DataRow dr in queryCategory)
                    {
                        category = dr.Field<string>("Data").ToString().Trim();
                        break;
                    }


                    //if (category == null || category == String.Empty)
                    //{
                    //    throw new Exception("Data for 001 is empty for " + bkey);
                    //
                    //}
                    // Set the category depending on the 001 data
                    if (category.Contains("BK"))
                        category = "Books";
                    if (category.Contains("BE"))
                        category = "Entertainment";

                    // Extract ProductType
                    if (category == "Books")
                    {
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("914wTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("914wTag").ToString().Trim();
                            break;
                        }
                    }
                    else if (category == "Entertainment")
                    {
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("943jTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("943jTag").ToString().Trim();
                            break;
                        }

                    }
                    //CHECK BOOK 914 TAG WHEN CATEGORY IS NULL
                    else
                    {
                        //WriteLogFile(Global.SP_LOGFILE, "category missing - checking 914", "", "true");
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("914wTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("914wTag").ToString().Trim();
                            break;
                        }

                    }

                    //CHECK ENTERTAINMENT 943 TAG WHEN PROD TYPE IS NULL
                    if (Prd_Type == null || Prd_Type == String.Empty)
                    {
                        //WriteLogFile(Global.SP_LOGFILE, "category missing - checking 943", "", "true");
                        IEnumerable<DataRow> queryPrdType = from r in dtMARCRecord.AsEnumerable()
                                                            where r.Field<string>("943jTag") != null
                                                            select r;
                        foreach (DataRow dr in queryPrdType)
                        {
                            Prd_Type = dr.Field<string>("943jTag").ToString().Trim();
                            break;
                        }


                    }   


                    returnTemp = BuildMARCFile(dtMARCRecord, strLeader, Prd_Type);
                    sbMARCFile.Append(returnTemp);
                    dtMARCRecord = null;
                }

                //
                //Sharepoint directory writes
                //

                //Assign folder and file name variables
                string siteUrlUpload = Config.SPSiteURLUpload;
                string siteUrlDownload = Config.SPSiteURLDownload;
                string spMarcFileName = Config.SPMarcFileName;
                string spMarcLogilename = Config.SPLogFileName;
                string spDocLibraryName = Config.SPDocLibraryName;
                string spFileGUID = Guid.NewGuid().ToString();
                string targetSPFilePath = ("/" + spDocLibraryName + "/" + spFileGUID + spMarcFileName);
                string SPMarcLocalFolder = Config.SPLocalFolder;

                string SPUser = Config.SPUser;
                string SPPass = Config.SPPassword;
                string SPDomain = Config.SPDomain;
                string SourceSystem = Config.SourceSystem;
                string SPMessageTemplateNum = Config.SPMessageTemplateID;
                string SPLocalFolderFileName = Global.SP_MARCFILE;
                string SPLogFolderFileName = Global.SP_LOGFILE;

                //Write out marc file
                FileStream fs = new FileStream(SPLocalFolderFileName, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(fs, Encoding.Default);
                writer.Write(sbMARCFile.ToString().ToCharArray());
                writer.Close();
                fs.Close();

                targetSPFilePath = targetSPFilePath.Replace("-", ""); //// don't use "-" in SP Library name throws 409 conflict exception
                string completetargetSPFilePath = siteUrlDownload + targetSPFilePath; 
                
                ClientContext context = new ClientContext(siteUrlUpload);
                context.Credentials = new NetworkCredential(SPUser, SPPass, SPDomain);


                if (System.IO.File.Exists(SPLocalFolderFileName))
                {
                    try
                    {
                        using (FileStream fs2 = new FileStream(SPLocalFolderFileName, FileMode.Open))
                        {
                            Microsoft.SharePoint.Client.File.SaveBinaryDirect(context, targetSPFilePath, fs2, true);
                            //WriteLogFile(this.GetType().Name, "SendMarcFile SP XMIT Successful", "Successful XMIT");
                            //WriteLogFile(Global.SP_LOGFILE, "Success", basketSummaryID, "true");


                        }
                        // write out to log here indicating successful xmit: return siteUrl + targetSPFilePath;
                    }
                    catch (Exception ex)
                    {
                        // 
                        //WriteLogFile(this.GetType().Name, "SendMarcFile SP XMIT Exception", ex.Message.ToString());
                        WriteLogFile(Global.SP_LOGFILE, "xmit exception", ex.Message.ToString(), "true");

                        SendEmailException("SEND_MARC_FILE_SP" + ex.Message.ToString(), "SendMarcFileSP", "true");

                        throw new FaultException(ex.Message, new FaultCode("SEND_MARC_FILE_SP"));

                        // write out to log here indicating exception xmit: "Exception" + ex.Message;
                    }
                }
                else
                {
                    // write out to log here indication exception with file "Exception: The source file doesn't exist.";
                    //WriteLogFile(this.GetType().Name, "SendMarcFile SP Missing File", "Missing file, it doesn't exist");
                    WriteLogFile(Global.SP_LOGFILE, "missing file exception", "", "true");
                }

                if (System.IO.File.Exists(SPLocalFolderFileName))
                {
                    System.IO.File.Delete(SPLocalFolderFileName);

                }


                // 
                //WriteUserAlert("Marc File is available for viewing: " + siteUrl + targetSPFilePath, TS360UserID, "1", SourceSystem, SPLogFolderFileName);
                WriteUserAlert(SPMessageTemplateNum, TS360UserID, SourceSystem, TS360CartName, ProfileName, completetargetSPFilePath, SPLogFolderFileName);

                //for testing
                //SendEmailException("SEND_MARC_FILE sUCCESS " + "Marc File is available for viewing: " + siteUrl + targetSPFilePath , "SendMarcFileSP", "true");



            }

            catch (FaultException faultEx)
            {
                WriteLogFile(Global.SP_LOGFILE, "SEND_MARC_FILE_SP Fault: ", faultEx.Message.ToString(), "true");

                SendEmailException("SEND_MARC_FILE_SP Fault: " + faultEx.Message.ToString(), "SendMarcFileSP", "true");

                throw new FaultException(faultEx.Message, new FaultCode(faultEx.Code.Name));

            }

            catch (Exception ex)
            {
                WriteLogFile(Global.SP_LOGFILE, "SEND_MARC_FILE_SP", ex.Message.ToString(), "true");

                SendEmailException("SEND_MARC_FILE_SP" + ex.Message.ToString(), "SendMarcFileSP", "true");

                throw new FaultException(ex.Message, new FaultCode("SEND_MARC_FILE_SP"));
            }


        }


        private string BuildMARCFile(DataTable dtMARCRecord, string existingLeader, string ProductType)
        {

            try
            {
                String Leader = String.Empty;
                String strDirectory = String.Empty;
                String VariableData = String.Empty;

                int UpperBound = dtMARCRecord.Rows.Count;


                // Loop through tags
                //
                for (int Loop = 0; Loop < UpperBound; Loop++)
                {
                    // Array information
                    //
                    DataRow TagInfo = dtMARCRecord.Rows[Loop];

                    // Tag Data
                    //
                    string TempVarData;
                    if (Convert.ToInt32(TagInfo["Tag"]) < 10)
                    {
                        TempVarData = TagInfo["Data"].ToString() + END_OF_FIELD;
                    }
                    else
                    {
                        // Indicators
                        //
                        TempVarData = TagInfo["Indicator"].ToString();

                        TempVarData += TagInfo["Data"];

                        TempVarData += END_OF_FIELD;
                    }

                    // Directory Data
                    //
                    string TempDir;
                    //TempDir.Format("%03d%04d%05d", TagInfo["Tag"].ToString(), TempVarData.Length, VariableData.Length);

                    TempDir = TagInfo["Tag"].ToString().PadLeft(3, '0') + TempVarData.Length.ToString().PadLeft(4, '0') + VariableData.Length.ToString().PadLeft(5, '0');
                    strDirectory += TempDir;



                    // Variable Data
                    //
                    VariableData += TempVarData;
                }


                // Create a writer and open the file:
                //StreamWriter log;

                //if (!File.Exists("c:\\mrclogfile.txt"))
                //{
                //  log = new StreamWriter("c:\\mrclogfile.txt");
                //}
                //else
                //{
                //  log = File.AppendText("c:\\mrclogfile.txt");
                //} 

                // Write to the file:
                //log.WriteLine(DateTime.Now);
                //log.WriteLine(strDirectory.Length);
                //log.WriteLine(END_OF_FIELD.ToString().Length);
                //log.WriteLine(VariableData.Length);
                //log.WriteLine(END_OF_RECORD.ToString().Length);
                //log.WriteLine(existingLeader);
                //log.WriteLine(ProductType);


                //log.WriteLine();
                //log.Close();




                string strLeader = BuildLeader(strDirectory.Length, END_OF_FIELD.ToString().Length, VariableData.Length, END_OF_RECORD.ToString().Length, existingLeader, ProductType);

                //strLeader = strLeader.Replace("&#x1F;", "######");


                return strLeader + strDirectory + END_OF_FIELD + VariableData + END_OF_RECORD;


            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message, new FaultCode("BUILD_DIRECTORY"));
            }

        }


        private string BuildLeader(int DirLength, int fieldTermLength, int VariableField, int recordTermLength, string existingLeader, string ProductType)
        {
            try
            {

                StringBuilder sbLeader = new StringBuilder(24);
                int TotalRecLength = DirLength + VariableField + fieldTermLength + recordTermLength + 24;
                int DataLoc = DirLength + 24 + fieldTermLength;
                string strTotalRecLength = String.Format("{0:00000}", TotalRecLength);
                string strStart = String.Format("{0:00000}", DataLoc);

                if (Convert.ToInt32(strTotalRecLength) > 99999)
                {
                    strTotalRecLength = "99999";
                }

                if (existingLeader != null && existingLeader != String.Empty)
                {
                    sbLeader.Append(existingLeader);
                    sbLeader.Replace(existingLeader.ToString().Substring(0, 5), strTotalRecLength, 0, 5);
                    sbLeader.Replace(existingLeader.ToString().Substring(12, 5), strStart, 12, 5);
                    sbLeader.Replace(existingLeader.ToString().Substring(10, 1), "2", 10, 1);
                    sbLeader.Replace(existingLeader.ToString().Substring(22, 1), "0", 22, 1);

                }
                else
                {
                    if (ProductType.Substring(0, 3).Equals("AUD"))
                    {
                        //existingLeader = String.Format("%05dnjm  22%05d5  4500", strTotalRecLength, strStart);
                        existingLeader = String.Format("{0:00000}njm  22{1:00000}5  4500", strTotalRecLength, strStart);
                    }
                    else
                        if (ProductType.Substring(0, 3).Equals("VID"))
                        {
                            existingLeader = String.Format("{0:00000}ngm  22{1:00000}5  4500", strTotalRecLength, strStart);
                        }

                        else
                            if (ProductType.Length > 5)
                            {

                                if (ProductType.Substring(0, 6).Equals("SPOKEN"))
                                {
                                    existingLeader = String.Format("{0:00000}nim  22{1:00000}5  4500", strTotalRecLength, strStart);
                                }
                            }

                            else
                            //existingLeader = String.Format("%05dnam  22%05d5  4500", strTotalRecLength, strStart);
                            {
                                existingLeader = String.Format("{0:00000}nam  22{1:00000}5  4500", strTotalRecLength, strStart);
                            }

                    sbLeader.Append(existingLeader);
                }

                return sbLeader.ToString();
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message, new FaultCode("BUILD_LEADER"));
            }
        }

        private string BuildMARCFileFTP(DataTable dtMARCRecord, string existingLeader, string ProductType, string BasketSummaryID, string connOrders)
        {

            try
            {
                String Leader = String.Empty;
                String strDirectory = String.Empty;
                String VariableData = String.Empty;

                int UpperBound = dtMARCRecord.Rows.Count;


                // Loop through tags
                //
                for (int Loop = 0; Loop < UpperBound; Loop++)
                {
                    // Array information
                    //
                    DataRow TagInfo = dtMARCRecord.Rows[Loop];

                    // Tag Data
                    //
                    string TempVarData;
                    if (Convert.ToInt32(TagInfo["Tag"]) < 10)
                    {
                        TempVarData = TagInfo["Data"].ToString() + END_OF_FIELD;
                    }
                    else
                    {
                        // Indicators
                        //
                        TempVarData = TagInfo["Indicator"].ToString();

                        TempVarData += TagInfo["Data"];

                        TempVarData += END_OF_FIELD;
                    }

                    // Directory Data
                    //
                    string TempDir;
                    //TempDir.Format("%03d%04d%05d", TagInfo["Tag"].ToString(), TempVarData.Length, VariableData.Length);

                    TempDir = TagInfo["Tag"].ToString().PadLeft(3, '0') + TempVarData.Length.ToString().PadLeft(4, '0') + VariableData.Length.ToString().PadLeft(5, '0');
                    strDirectory += TempDir;



                    // Variable Data
                    //
                    VariableData += TempVarData;
                }


                // Create a writer and open the file:
                //StreamWriter log;

                //if (!File.Exists("c:\\mrclogfile.txt"))
                //{
                //  log = new StreamWriter("c:\\mrclogfile.txt");
                //}
                //else
                //{
                //  log = File.AppendText("c:\\mrclogfile.txt");
                //} 

                // Write to the file:
                //log.WriteLine(DateTime.Now);
                //log.WriteLine(strDirectory.Length);
                //log.WriteLine(END_OF_FIELD.ToString().Length);
                //log.WriteLine(VariableData.Length);
                //log.WriteLine(END_OF_RECORD.ToString().Length);
                //log.WriteLine(existingLeader);
                //log.WriteLine(ProductType);


                //log.WriteLine();
                //log.Close();




                string strLeader = BuildLeaderFTP(strDirectory.Length, END_OF_FIELD.ToString().Length, VariableData.Length, END_OF_RECORD.ToString().Length, existingLeader, ProductType, BasketSummaryID, connOrders);

                //strLeader = strLeader.Replace("&#x1F;", "######");


                return strLeader + strDirectory + END_OF_FIELD + VariableData + END_OF_RECORD;


            }
            catch (Exception ex)
            {
                //DataAccess da = new DataAccess();
                //da.UpdateMARCFTPFailure(BasketSummaryID, ex.Message, "ok", connOrders);
                WriteLogFile(Global.FTP_LOGFILE, "Build_MarcFileFTP Fault: ", ex.Message.ToString(), "true");

                //SendEmailException("Build_MarcFileFTP Fault: " + ex.Message.ToString(), "Build_MarcFileFTP", "true");

                throw new FaultException(ex.Message, new FaultCode("BUILD_MarcFileFTP"));
            }

        }


        private string BuildLeaderFTP(int DirLength, int fieldTermLength, int VariableField, int recordTermLength, string existingLeader, string ProductType, string BasketSummaryID, string connOrders)
        {
            try
            {

                StringBuilder sbLeader = new StringBuilder(24);
                int TotalRecLength = DirLength + VariableField + fieldTermLength + recordTermLength + 24;
                int DataLoc = DirLength + 24 + fieldTermLength;
                string strTotalRecLength = String.Format("{0:00000}", TotalRecLength);
                string strStart = String.Format("{0:00000}", DataLoc);

                if (Convert.ToInt32(strTotalRecLength) > 99999)
                {
                    strTotalRecLength = "99999";
                }

                if (existingLeader != null && existingLeader != String.Empty)
                {
                    sbLeader.Append(existingLeader);
                    sbLeader.Replace(existingLeader.ToString().Substring(0, 5), strTotalRecLength, 0, 5);
                    sbLeader.Replace(existingLeader.ToString().Substring(12, 5), strStart, 12, 5);
                    sbLeader.Replace(existingLeader.ToString().Substring(10, 1), "2", 10, 1);
                    sbLeader.Replace(existingLeader.ToString().Substring(22, 1), "0", 22, 1);

                }
                else
                {
                    if (ProductType.Substring(0, 3).Equals("AUD"))
                    {
                        //existingLeader = String.Format("%05dnjm  22%05d5  4500", strTotalRecLength, strStart);
                        existingLeader = String.Format("{0:00000}njm  22{1:00000}5  4500", strTotalRecLength, strStart);
                    }
                    else
                        if (ProductType.Substring(0, 3).Equals("VID"))
                        {
                            existingLeader = String.Format("{0:00000}ngm  22{1:00000}5  4500", strTotalRecLength, strStart);
                        }

                        else
                            if (ProductType.Length > 5)
                            {

                                if (ProductType.Substring(0, 6).Equals("SPOKEN"))
                                {
                                    existingLeader = String.Format("{0:00000}nim  22{1:00000}5  4500", strTotalRecLength, strStart);
                                }
                            }

                            else
                            //existingLeader = String.Format("%05dnam  22%05d5  4500", strTotalRecLength, strStart);
                            {
                                existingLeader = String.Format("{0:00000}nam  22{1:00000}5  4500", strTotalRecLength, strStart);
                            }

                    sbLeader.Append(existingLeader);
                }

                return sbLeader.ToString();
            }
            catch (Exception ex)
            {
                //DataAccess da = new DataAccess();
                //da.UpdateMARCFTPFailure(BasketSummaryID, ex.Message, "ok", connOrders);
                WriteLogFile(Global.FTP_LOGFILE, "Build_LeaderFTP Fault: ", ex.Message.ToString(), "true");

                //SendEmailException("Build_LeaderFTP Fault: " + ex.Message.ToString(), "Build_LeaderFTP", "true");

                throw new FaultException(ex.Message, new FaultCode("BUILD_LEADERFTP"));
            }
        }



        private InventoryDemandRequest BuildMongoRequest(DataTable dtBTKEYRecord, string marketType)
        {
            try
            {
                string leindicator = string.Empty ; 

                int UpperBound = dtBTKEYRecord.Rows.Count;
                InventoryDemandRequest inventorydemandrequest = new InventoryDemandRequest(); 
                List<BTKeys> btKeyList = new List<BTKeys>();

                // Loop through tags
                //
                for (int Loop = 0; Loop < UpperBound; Loop++)
                {
                    // Array information
                    //
                    DataRow TagInfo = dtBTKEYRecord.Rows[Loop];

                    //COUNT 
                    //int totalColumns = dtBTKEYRecord.Columns.Count;
                    //WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "#x1 count: " + totalColumns.ToString(), Config.LogDetails);

                    // BTKey Data
                    //
                    string tempBTKey;
                    tempBTKey = TagInfo["BTKEY"].ToString(); 

                    // ProductType Data 
                    string tempProductID;
                    tempProductID = TagInfo["ProductTypeID"].ToString(); 


                    if (marketType =="1" && tempProductID =="4" ) 
                    { leindicator = "1";}
                    else 
                    { leindicator = "0";} 


                    //List<BTKeys> btKeyList = new List<BTKeys>();
                    btKeyList.Add(new BTKeys { BTKey = tempBTKey, LEIndicator = leindicator  });

                   }
                inventorydemandrequest.MarketType = marketType ; 
                inventorydemandrequest.OnItemDetail = false; 
                inventorydemandrequest.VIPEnabled = "1";
                inventorydemandrequest.CountryCode = "US"; 
                List<InventoryDemandWareHouses> warehouseList = new List<InventoryDemandWareHouses>();
                warehouseList.Add(new InventoryDemandWareHouses { WarehouseID = "COM" });
                warehouseList.Add(new InventoryDemandWareHouses { WarehouseID = "SOM" });
                warehouseList.Add(new InventoryDemandWareHouses { WarehouseID = "MOM" });
                warehouseList.Add(new InventoryDemandWareHouses { WarehouseID = "RNO" });
                warehouseList.Add(new InventoryDemandWareHouses { WarehouseID = "VIE" });
                warehouseList.Add(new InventoryDemandWareHouses { WarehouseID = "VIM" });
                inventorydemandrequest.BTKeys = btKeyList.ToArray();
                inventorydemandrequest.Warehouses = warehouseList.ToArray();

                return inventorydemandrequest ;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message, new FaultCode("BUILD_MONGOSVC_REQUEST"));
            }
        }

        private DataTable BuildInventoryRequest(NoSqlServiceResult<InventoryDemandResponse> response)
         {
             try
             {
                 DataTable tblInventoryUDTRequest = new DataTable("InventoryMark");
                 DataTable tblInventoryUDTRequestTemp = new DataTable();
                 Int32 sequence = 0; 
                 
                 tblInventoryUDTRequest.Columns.Add("Sequence");
                 tblInventoryUDTRequest.Columns.Add("BTKey");
                 tblInventoryUDTRequest.Columns.Add("Warehouse");
                 tblInventoryUDTRequest.Columns.Add("QuantityOnHand");
                 tblInventoryUDTRequest.Columns.Add("QuantityOnOrder");

                 //WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "#x1 count: " + tblInventoryUDTRequest.Rows.Count.ToString(), Config.LogDetails);
                 

           if (response != null)
            {

                if (response.Status == NoSqlServiceStatus.Success)
                {

                    if (response.Data != null && response.Data.InventoryResults != null &&
                        response.Data.InventoryResults.Any())
                    {


                         //WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "#2afgrow 4 count: " + tblInventoryUDTRequest.Rows.Count.ToString(), Config.LogDetails);


                        foreach (var res in response.Data.InventoryResults)
                        {
                            //DataRow datarow = tblInventoryUDTRequest.NewRow();
                            //datarow["Sequence"] = sequence;
                            //datarow["BTKey"] = res.BTKey;
                            var responsedtl = res.Warehouses;

                            foreach (var resdtl in responsedtl)
                            {
                                DataRow datarow = tblInventoryUDTRequest.NewRow();
                                sequence = sequence + 1;
                                datarow["Sequence"] = sequence;
                                datarow["BTKey"] = res.BTKey;
                                //var responsedtl = res.Warehouses;

                                datarow["Warehouse"] = resdtl.WarehouseId;
                                datarow["QuantityOnHand"] = resdtl.InStockForRequest;
                                datarow["QuantityOnOrder"] = resdtl.OnOrderQuantity;
                                //WriteLogFile(Global.DOWNLOAD_LOGFILE, "aaa", "warehouse "+ resdtl.WarehouseId , "true");
                                //WriteLogFile(Global.DOWNLOAD_LOGFILE, "aaa", "warehouse " + resdtl.InStockForRequest , "true");
                                //WriteLogFile(Global.DOWNLOAD_LOGFILE, "aaa", "warehouse " + resdtl.OnOrderQuantity , "true");
                                //tblInventoryUDTRequest.ImportRow(datarow);
                                tblInventoryUDTRequest.Rows.Add(datarow); 

                            }

                        }

                    }
                }
                else
                {
                    String exception = string.Format("MongoDb WebAPI Call For GetInventory {0}, Error Code: {1}, Error Message {2}", response.Status,
                            response.ErrorCode, response.ErrorMessage);    
                    throw new Exception(exception );
                }



           }

           //WriteLogFile(Global.DOWNLOAD_LOGFILE, "Download", "#2afgrow 7 count: " + tblInventoryUDTRequest.Rows.Count.ToString(), Config.LogDetails);

           tblInventoryUDTRequestTemp = tblInventoryUDTRequest; 
           return tblInventoryUDTRequestTemp;
            
             }
             catch (Exception ex)
             {
                 throw new FaultException(ex.Message, new FaultCode("BUILD_SQL_Inventory_UDT_REQUEST"));
             }
         }




       private static NoSqlServiceResult<InventoryDemandResponse> GetInventoryResults(InventoryDemandRequest data)
       {
           try
           {
               // Create a WebClient to POST the request
               using (var client = new WebClient())
               {
                   // Set the header so it knows we are sending JSON
                   client.Headers[HttpRequestHeader.ContentType] = "application/json";

                   // Serialise the data we are sending in to JSON
                   string serialisedData = JsonConvert.SerializeObject(data);

                   Global.txnID = Guid.NewGuid().ToString(); 

                   ArchiveMessage(serialisedData, "Request", Global.txnID); 

                   // Make the request
                   var response = client.UploadString(Global.InventoryServiceUrl, serialisedData);

                   ArchiveMessage(response.ToString(), "Response", Global.txnID);


                   return JsonConvert.DeserializeObject <NoSqlServiceResult<InventoryDemandResponse>>(response);

               }
           }
           catch (Exception ex)
           {
               throw new FaultException(ex.Message, new FaultCode("GetInventoryResults"));
           }
           //return null;
       }

        private static void ArchiveMessage(string Message, string Type, string FileNameTag)
        {

            if (Config.ArchiveInventoryMessage  == "true" ) 
            {
            String AppPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath; 
            StreamWriter textwriter; 
            textwriter = new StreamWriter(AppPath + "\\Logs\\" + Type + "Message_" + FileNameTag + ".txt"); 
            textwriter.WriteLine(Message); 
            textwriter.Close(); 
            }

        
        }

                        

        private static void WriteLogFile(string fileName, string methodName, string message, string LogFlag)
        {
            if (LogFlag == "true")
            {
                try
                {  
                    string fileName_complete = string.Format("{0}{1}.txt", fileName, DateTime.Now.ToString("yyyyMMdd"));
                    //Create a writer and open the file:
                    StreamWriter log;

                    if (!System.IO.File.Exists(fileName_complete))
                    {
                        log = new StreamWriter(fileName_complete);
                    }
                    else
                    {
                        log = System.IO.File.AppendText(fileName_complete);
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
                    message.Subject = Config.Environment + "WCF MarcProfiler - " + MethodName;
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


        

        private static void WriteUserAlert(string MessageTemplateNum, string User, string Source, string CartName, string ProfileName, string SPURL, string logfilename)
        {

            try
            {
                #region startdebug
                //WriteLogFile(logfilename, "WriteUserAlert", "1", "true");
                #endregion
                string AlertMessageTemplate = string.Empty;
                string AlertMessageComplete = string.Empty;
                string ConfigReferenceValue = string.Empty;
                // This portion calls service to get the message template

                ServiceReference1.UserAlerts.UserAlertsClient svc1Get = new ServiceReference1.UserAlerts.UserAlertsClient();
                ServiceReference1.UserAlerts.GetUserAlertMessageTemplateResponse svc1Getresp = new ServiceReference1.UserAlerts.GetUserAlertMessageTemplateResponse();
                //ServiceReference1.UserAlertsClient svc1Get = new ServiceReference1.UserAlertsClient();
                //ServiceReference1.GetUserAlertMessageTemplateResponse svc1Getresp = new ServiceReference1.GetUserAlertMessageTemplateResponse();
                svc1Getresp = svc1Get.GetUserAlertMessageTemplate(ServiceReference1.UserAlerts.AlertMessageTemplateIDEnum.MarcDownloadSP);

                #region startdebug
                //WriteLogFile(logfilename, "WriteUserAlert", "1.1", "true");
                //WriteLogFile(logfilename, "WriteUserAlert", svc1Getresp.Status, "true");
                //WriteLogFile(logfilename, "WriteUserAlert", svc1Getresp.AlertMessageTemplate, "true"); 
                #endregion


                if (svc1Getresp.Status == "OK")
                {
                    #region startdebug
                    //WriteLogFile(logfilename, "WriteUserAlert", "1.2", "true");
                    #endregion

                    AlertMessageComplete = svc1Getresp.AlertMessageTemplate;
                    ConfigReferenceValue = svc1Getresp.ConfigReferenceValue;
                    WriteLogFile(logfilename, "GetUserAlert", "Success - GETUserAlert", "true");
                }

                else
                {
                    #region startdebug
                    //WriteLogFile(logfilename, "WriteUserAlert", "1.3", "true");
                    #endregion

                    WriteLogFile(logfilename, "GetUserAlert-Failure", svc1Getresp.ErrorMessage , "true");
                    throw new Exception("GetUserAlert: " + svc1Getresp.ErrorMessage);
                }

                #region startdebug
                //WriteLogFile(logfilename, "WriteUserAlert", "2", "true");
                //WriteLogFile(logfilename, "WriteUserAlert", ProfileName.Length.ToString(), "true");
                // This portion replaces the placeholders in the message template to create the completed message
                #endregion

                AlertMessageComplete = AlertMessageComplete.Replace("@cartname", CartName);

                if (ProfileName == null || ProfileName == String.Empty )
                { ProfileName = "n/a"; } 
                 
                  
                AlertMessageComplete = AlertMessageComplete.Replace("@profilename", ProfileName);
                AlertMessageComplete = AlertMessageComplete.Replace("@URL", SPURL);

                WriteLogFile(logfilename, "WriteUserAlert", AlertMessageComplete, "true");

                #region startdebug
                //AlertMessageComplete.r
                //CompleteMessage = CompleteMessage.Replace(“@profilename”,ProfileID)
                //CompleteMessage = CompleteMessage.Replace(“@URL”,SPSiteURL + TargetSPFilePath)
                //WriteLogFile(logfilename, "WriteUserAlert", "3", "true");
                // This portion calls service which writes to sql
                #endregion 


                ServiceReference1.UserAlerts.UserAlertsClient svc1 = new ServiceReference1.UserAlerts.UserAlertsClient();
                ServiceReference1.UserAlerts.CreateUserAlertMessageResponse svc1resp = new ServiceReference1.UserAlerts.CreateUserAlertMessageResponse();
                svc1resp = svc1.CreateUserAlertMessage(AlertMessageComplete, User, ServiceReference1.UserAlerts.AlertMessageTemplateIDEnum.MarcDownloadSP, Source);
                if (svc1resp.Status == "OK")
                {
                    WriteLogFile(logfilename, "WriteUserAlert", "Success - ADDUserAlert", "true");
                }

                else
                {
                    WriteLogFile(logfilename, "WriteUserAlert", "Failure " + svc1resp.ErrorMessage, "true");
                    throw new Exception("WriteUserAlert: " + svc1resp.ErrorMessage);
                }



            }

            catch (Exception ex)

            { throw new FaultException(ex.Message, new FaultCode("WRITEUSERALERT")); }

        }

    }

}
