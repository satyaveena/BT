using BT.TS360API.Common.DataAccess;
using BT.TS360API.Common.Grid.Cart;
using BT.TS360API.Common.Helpers;
using BT.TS360API.Common.Inventory;
using BT.TS360API.Common.Models;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Inventory;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using CommonAppSettings = BT.TS360API.Common.Configrations.AppSettings;

namespace BT.TS360API.Common.Business
{
    public class MARCProfilerManager
    {
        private static readonly object SyncRoot = new Object();

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

        /// <summary>
        /// MARCProfilerManager
        /// </summary>
        private MARCProfilerManager()
        {

        }

        /// <summary>
        /// Instance
        /// </summary>
        public static MARCProfilerManager Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    MARCProfilerManager _instance = HttpContext.Current == null ? new MARCProfilerManager() : HttpContext.Current.Items["MARCProfilerManager"] as MARCProfilerManager;
                    if (_instance == null)
                    {
                        _instance = new MARCProfilerManager();
                        HttpContext.Current.Items.Add("MARCProfilerManager", _instance);
                    }

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Get Json for marc profile
        /// </summary>
        /// <param name="sortColumn"></param>
        /// <param name="basketSummaryID"></param>
        /// <param name="sortDirection"></param>
        /// <param name="ProfileID"></param>
        /// <param name="FullIndicator"></param>
        /// <param name="isOrdered"></param>
        /// <param name="isCancelled"></param>
        /// <param name="isOCLCEnabled"></param>
        /// <param name="isBTEmployee"></param>
        /// <param name="hasInventoryRules"></param>
        /// <param name="marketType"></param>
        /// <returns></returns>
        public List<MARCJsonResponse> GetMarcJson(MARCJsonRequest request)
        {
            try
            {

                List<MARCJsonResponse> MARCJson = new List<MARCJsonResponse>();
                List<MARCRecord> marcResults = new List<MARCRecord>();
                List<string> distinctBTKeys = new List<string>();
                List<string> distinctLineItemIds = new List<string>();
                DataTable dtTableMarcInventory = new DataTable();
                DataTable dtTableMarcBTKeyResults = new DataTable();

                //new for 3.2.14 get inventory for the cart when flag is equal to true
                if (request.HasInventoryRules)
                {
                    //call stored procedure to get the btkeys
                    dtTableMarcBTKeyResults = OrdersDAO.Instance.GetMARCRecordsBTKeys(request.BasketSummaryID);

                    if (dtTableMarcBTKeyResults.Rows.Count > 0)
                    {

                        NoSqlServiceResult<InventoryDemandResponse> response = InventoryHelper4MongoDb.GetInstance(request.BasketSummaryID).GetInventoryDemandForMarc(dtTableMarcBTKeyResults, request.MarketType);
                        //build the udt variable to the main marc stored procedure
                        dtTableMarcInventory = BuildInventoryRequest(response);
                    }
                }

                if (dtTableMarcInventory.Columns.Count == 0)
                {
                    //create dummy result set so sql server doesn't trip out due to no data. 
                    dtTableMarcInventory.Columns.Add("Sequence", typeof(Int32));
                    dtTableMarcInventory.Columns.Add("BTKey", typeof(String));
                    dtTableMarcInventory.Columns.Add("Warehouse", typeof(String));
                    dtTableMarcInventory.Columns.Add("QuantityOnHand", typeof(Int32));
                    dtTableMarcInventory.Columns.Add("QuantityOnOrder", typeof(Int32));
                }

                marcResults = MARCProfilerDAO.Instance.GetMARCRecords(request.SortColumn, request.BasketSummaryID, request.SortDirection, request.ProfileID, request.FullIndicator, request.IsOrdered, request.IsCancelled, request.IsOCLCEnabled, request.IsBTEmployee, dtTableMarcInventory);

                //get distinct BTKey from the stored procedure resultset.
                distinctBTKeys = marcResults.Select(p => p.BTKey).Distinct().ToList();
                distinctLineItemIds = marcResults.Select(p => p.BasketLineItemID).Distinct().ToList();
                Dictionary<string, List<CommonCartGridLine>> allCartGridLines = CartGridManager.Instance.LoadCartLineItemGridLinesIII(request.TsUserId,request.OrgId, distinctLineItemIds);

                // Get ILS mapped grid fields
                var ilsGridFields = OrganizationDAO.Instance.GetILSGridFields(request.OrgId);
                var strILSFundCodeFieldID = "";
                var strILSLocationCodeFieldID = "";
                
                if (ilsGridFields.Tables.Count != 0 && ilsGridFields.Tables[0].Rows.Count != 0)
                {
                    var ilsInfoRow = ilsGridFields.Tables[0].Rows[0];

                    strILSFundCodeFieldID = ilsInfoRow.Table.Columns.Contains("ILSFundCodeFieldID")
                            ? DataAccessHelper.ConvertToString(ilsInfoRow["ILSFundCodeFieldID"])
                            : "";

                    strILSLocationCodeFieldID = ilsInfoRow.Table.Columns.Contains("ILSLocationCodeFieldID")
                            ? DataAccessHelper.ConvertToString(ilsInfoRow["ILSLocationCodeFieldID"])
                            : "";
                }

                //loop through the unique btkeys

                foreach (string bTKey in distinctBTKeys)
                {
                    string returnTemp = String.Empty;

                    //Initialize the variables
                    String Prd_Type = String.Empty;
                    String category = String.Empty;
                    String strLeader = String.Empty;
                    String strBasketLineItemID = String.Empty;

                    String strData = String.Empty;
                    String strTag = String.Empty;
                    String strIndicator = String.Empty;

                    List<MARCRecord> marcRecords = marcResults.Where(p => p.BTKey == bTKey).ToList();

                    //Extract BasketLineItemID
                    strBasketLineItemID = marcRecords != null ? marcRecords.FirstOrDefault().BasketLineItemID.Trim() : string.Empty;

                    //Extract Leader
                    strLeader = marcRecords != null ? marcRecords.FirstOrDefault(p => !string.IsNullOrEmpty(p.Tag000)).Tag000.Trim() : string.Empty;

                    //Extract ProductType,Category and Leader(if present)
                    // 914-w for Books and 943-J for Entertainment
                    // if 001 has BK then Product is Book 
                    // If 001 has BE then Product is Entertainment

                    var cat001 = marcRecords != null ? marcRecords.FirstOrDefault(p => p.Tag == "001") : null;
                    var cat001Data = cat001 != null ? cat001.Data.Trim() : string.Empty;

                    category = cat001Data;

                    // Set the category depending on the 001 data
                    if (category.Contains("BK"))
                        category = "Books";
                    if (category.Contains("BE"))
                        category = "Entertainment";

                    // Extract ProductType
                    if (category == "Books")
                    {
                        var Tag914wRecord = marcRecords != null ? marcRecords.FirstOrDefault(p => !string.IsNullOrEmpty(p.Tag914w)) : null;
                        Prd_Type = Tag914wRecord != null ? Tag914wRecord.Tag914w.Trim() : string.Empty;
                    }
                    else if (category == "Entertainment")
                    {
                        var Tag943jRecord = marcRecords != null ? marcRecords.FirstOrDefault(p => !string.IsNullOrEmpty(p.Tag943j)) : null;
                        Prd_Type = Tag943jRecord != null ? Tag943jRecord.Tag943j.Trim() : string.Empty;
                    }
                    //CHECK BOOK 914 TAG WHEN CATEGORY IS NULL
                    else
                    {
                        var Tag914wRecord = marcRecords != null ? marcRecords.FirstOrDefault(p => !string.IsNullOrEmpty(p.Tag914w)) : null;
                        Prd_Type = Tag914wRecord != null ? Tag914wRecord.Tag914w.Trim() : string.Empty;
                    }

                    //CHECK ENTERTAINMENT 943 TAG WHEN PROD TYPE IS NULL
                    if (Prd_Type == null || Prd_Type == String.Empty)
                    {
                        var Tag943jRecord = marcRecords != null ? marcRecords.FirstOrDefault(p => !string.IsNullOrEmpty(p.Tag943j)) : null;
                        Prd_Type = Tag943jRecord != null ? Tag943jRecord.Tag943j.Trim() : string.Empty;
                    }

                    string body = BuildMarcBody(marcRecords, strLeader, Prd_Type, strBasketLineItemID, bTKey);
                    
                    string header = BuildMarcHeader(request.TsUserId, request.OrgId, strBasketLineItemID, request.IlsUserId, request.IlsVendor, allCartGridLines,
                                        strILSFundCodeFieldID, strILSLocationCodeFieldID);

                    MARCJson.Add(new MARCJsonResponse() { BTKey = bTKey, BasketLineItemID = strBasketLineItemID, MARCHeader = header, MARCBody = body });

                    try
                    {
                        if (CommonAppSettings.RetriveAppSettings<bool>(CommonAppSettings.ILSDebugMode))
                        {
                            var filename = string.Format("{0}_{1}_{2:MMddyyyy_HHmmssfffffff}.{3}", strBasketLineItemID, bTKey, DateTime.Now, CommonAppSettings.RetriveAppSettings<string>(CommonAppSettings.ILSMarcJsonExtName));
                            var jsonData = "{\"order\": " + header + ", \"marcContentType\": \"application/marc-in-json\"," + body + "}";
                            SaveTextJsonMarcProfile(jsonData , filename);
                        }
                    }
                    catch (Exception debugEx)
                    {
                        var msg = string.Format("GetMarcJson()-ILSDebugMode- {0}. CartId = {1}", debugEx.Message, request.BasketSummaryID);
                        Logging.Logger.LogException(msg, ExceptionCategory.ILS.ToString(), debugEx);
                    }
                    
                    marcRecords = null;
                }
                return MARCJson;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task SaveBinaryMarcProfileFileToLocal(string inputContent, string filename)
        {
            var fullpath = string.Format(@"{0}/{1}", CommonAppSettings.RetriveAppSettings<string>(CommonAppSettings.ILSLogFolderPath), filename);
            //// Comment out because BinaryWriter doesn't support WriteAsync() method.
            ////FileStream fs = new FileStream(fullpath, FileMode.Create);
            ////using (BinaryWriter writer = new BinaryWriter(fs, Encoding.Default))
            ////{
            ////    writer.Write(inputContent.ToString().ToCharArray());
            ////} 

            //save file to local first
            using (StreamWriter writer = new StreamWriter(fullpath))
            {
                await writer.WriteAsync(inputContent);
            }

            string msg1 = string.Format("Save MarcProfile to local sucessfully - '{0}'. Continue copying to FTP.", fullpath);
            Logging.Logger.LogDebug("ILS_MarcProfile_Debug", msg1);
        }
        private async Task SaveBinaryMarcProfileFileToFTP(string inputContent, string filename)
        {
            try
            {
                var ftpServer = CommonAppSettings.RetriveAppSettings<string>(CommonAppSettings.FTPServer);
                var ftpUserID = CommonAppSettings.RetriveAppSettings<string>(CommonAppSettings.FTPUserID);
                var ftpPassword = CommonAppSettings.RetriveAppSettings<string>(CommonAppSettings.FTPPassword);
                var ftpFolder = CommonAppSettings.RetriveAppSettings<string>(CommonAppSettings.FTPFolder);


                var ftpFullpath = string.Format(@"ftp://{0}/{1}/{2}", ftpServer, ftpFolder, filename);

                //Write a string to a file on FTP server
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFullpath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                byte[] fileContents = Encoding.ASCII.GetBytes(inputContent);
                request.ContentLength = fileContents.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    await requestStream.WriteAsync(fileContents, 0, fileContents.Length);
                }
                FtpStatusCode ftpStatus = FtpStatusCode.Undefined;
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    ftpStatus = response.StatusCode;
                }

                string msg = string.Format("Copying file to FTP completed -'{0}'", ftpFullpath);
                Logging.Logger.LogDebug("ILS_MarcProfile_Debug", msg);
            }
            catch (Exception ex)
            {
                string errorMsg = string.Format("Failed to copy file to FTP - Error:'{0}'", ex.Message);
                Logging.Logger.LogException(errorMsg, "ILS_MarcProfile_Debug", ex);
            }
        }

        private async Task SaveTextJsonMarcProfile(string inputContent, string filename)
        {
            var fullpath = string.Format(@"{0}/{1}", CommonAppSettings.RetriveAppSettings<string>(CommonAppSettings.ILSLogFolderPath), filename);
            using (StreamWriter writer = new StreamWriter(fullpath))
            {
                await writer.WriteAsync(inputContent);
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

                if (response != null)
                {
                    if (response.Status == NoSqlServiceStatus.Success)
                    {
                        if (response.Data != null && response.Data.InventoryResults != null &&
                            response.Data.InventoryResults.Any())
                        {
                            foreach (var res in response.Data.InventoryResults)
                            {
                                var responsedtl = res.Warehouses;

                                foreach (var resdtl in responsedtl)
                                {
                                    DataRow datarow = tblInventoryUDTRequest.NewRow();
                                    sequence = sequence + 1;
                                    datarow["Sequence"] = sequence;
                                    datarow["BTKey"] = res.BTKey;

                                    datarow["Warehouse"] = resdtl.WarehouseId;
                                    datarow["QuantityOnHand"] = resdtl.InStockForRequest;
                                    datarow["QuantityOnOrder"] = resdtl.OnOrderQuantity;
                                    tblInventoryUDTRequest.Rows.Add(datarow);
                                }
                            }
                        }
                    }
                    else
                    {
                        String exception = string.Format("MongoDb WebAPI Call For GetInventory {0}, Error Code: {1}, Error Message {2}", response.Status,
                                response.ErrorCode, response.ErrorMessage);
                        throw new Exception(exception);
                    }
                }
                tblInventoryUDTRequestTemp = tblInventoryUDTRequest;
                return tblInventoryUDTRequestTemp;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message, new FaultCode("BUILD_SQL_Inventory_UDT_REQUEST"));
            }
        }
        private string BuildMarcHeader(string tsUserId, string orgId, string lineItemId, string ilsUserId, string ilsVendor, Dictionary<string, List<CommonCartGridLine>> allCartGridLines,
            string strILSFundCodeFieldID, string strILSLocationCodeFieldID)
        {

            StringBuilder sbOrder = new StringBuilder();
            StringBuilder sbAllocation = new StringBuilder();
            List<DCGridLine> GridLines = GetGridLinesByLineItem(tsUserId, orgId, lineItemId, allCartGridLines);
            int copies = 0;
            int seperator = 0;
            foreach (DCGridLine gridLine in GridLines)
            {
                if (seperator > 0)
                    sbAllocation.Append(",");
                seperator++;
                int quantity = 0;
                int.TryParse(gridLine.Quantity, out quantity);
                copies += quantity;

                var locationGridCode = "";
                var fundGridCode = "";

                if (gridLine != null && gridLine.DCGridFieldCodes != null)
                {
                    DCGridFieldCode location = gridLine.DCGridFieldCodes.Where(p => p.GridFieldId == strILSLocationCodeFieldID).FirstOrDefault();
                    DCGridFieldCode fund = gridLine.DCGridFieldCodes.Where(p => p.GridFieldId == strILSFundCodeFieldID).FirstOrDefault();

                    if (location != null) locationGridCode = location.GridCode;
                    if (fund != null) fundGridCode = fund.GridCode;
                }                

                sbAllocation.Append("{\"location\":\"" + locationGridCode + "\",\"fund\": \"" + fundGridCode + "\",\"copies\": " + quantity + "}");
            }

            sbOrder.Append("{\"login\": \"" + ilsUserId + "\",");
            if (copies > 1)
            {
                sbOrder.Append("\"copies\": " + copies + ",");
            }
            sbOrder.Append("\"allocation\": [");
            sbOrder.Append(sbAllocation.ToString());
            sbOrder.Append("],\"vendor\": \"" + ilsVendor + "\"}");

            return sbOrder.ToString();
        }
        private List<DCGridLine> GetGridLinesByLineItem(string userId, string orgId, string lineItemId, Dictionary<string, List<CommonCartGridLine>> allCartGridLines)
        {
            var dcGridLines = new List<DCGridLine>();
            //var dict = CartGridManager.Instance.LoadCartLineItemGridLines(userId, orgId, new List<string> { lineItemId });
            if (allCartGridLines.ContainsKey(lineItemId))
                dcGridLines = GridHelper.ConvertCartGridLinesToDCGridLines(allCartGridLines[lineItemId]);
            return dcGridLines;
        }
        /// <summary>
        /// Build Json formatted MARC
        /// </summary>
        /// <param name="marcRecords"></param>
        /// <param name="existingLeader"></param>
        /// <param name="ProductType"></param>
        /// <returns></returns>
        private string BuildMarcBody(List<MARCRecord> marcRecords, string existingLeader, string ProductType, string cartId, string btkey)
        {
            String MarcRecord = string.Empty;
            String Leader = String.Empty;
            String strDirectory = String.Empty;
            String VariableData = String.Empty;
            //string fields = string.Empty;
            Dictionary<string, List<string>> fields = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> Indicator = new Dictionary<string, List<string>>();

            try
            {
                // Loop through tags
                foreach (MARCRecord TagInfo in marcRecords)
                {

                    // Array information
                    string tag = TagInfo.Tag;
                    string tagData = TagInfo.Data;
                    string tagDataIndicator = TagInfo.Indicator;
                    
                    // Add data to tag dictionary
                    if (fields.ContainsKey(tag))
                    {
                        fields[tag].Add(tagData);
                    }
                    else
                    {
                        List<string> data = new List<string>();
                        data.Add(tagData);
                        fields.Add(tag, data);

                    }

                    // Add data to Indicator dictionary
                    if (Indicator.ContainsKey(tag))
                    {
                        Indicator[tag].Add(tagDataIndicator);
                    }
                    else
                    {
                        if (Convert.ToInt32(tag) >= 10)
                        {
                            List<string> IndiData = new List<string>();
                            IndiData.Add(tagDataIndicator);
                            Indicator.Add(tag, IndiData);
                        }
                    }

                    // Tag Data
                    string TempVarData;
                    if (Convert.ToInt32(TagInfo.Tag) < 10)
                    {
                        TempVarData = TagInfo.Data + END_OF_FIELD;
                    }
                    else
                    {
                        // Indicators
                        TempVarData = TagInfo.Indicator;
                        TempVarData += TagInfo.Data;
                        TempVarData += END_OF_FIELD;
                    }

                    // Directory Data
                    string TempDir;

                    TempDir = TagInfo.Tag.PadLeft(3, '0') + TempVarData.Length.ToString().PadLeft(4, '0') + VariableData.Length.ToString().PadLeft(5, '0');
                    strDirectory += TempDir;

                    // Variable Data
                    VariableData += TempVarData;

                }

                string strLeader = BuildLeader(strDirectory.Length, END_OF_FIELD.ToString().Length, VariableData.Length, END_OF_RECORD.ToString().Length, existingLeader, ProductType);
                try
                {
                    if (CommonAppSettings.RetriveAppSettings<bool>(CommonAppSettings.ILSDebugMode))
                    {
                        var fileContent = strLeader + strDirectory + END_OF_FIELD + VariableData + END_OF_RECORD;
                        var filename = string.Format("{0}_{1}_{2:MMddyyyy_HHmmssfffffff}.{3}", cartId, btkey, DateTime.Now, CommonAppSettings.RetriveAppSettings<string>(CommonAppSettings.ILSMarcProfileExtName));
                        
                        var task1 = SaveBinaryMarcProfileFileToLocal(fileContent, filename);
                        var task2 = SaveBinaryMarcProfileFileToFTP(fileContent, filename);
                        Task.WhenAll(task1, task2);
                    }
                }
                catch (Exception debugEx)
                {
                    var msg = string.Format("BuildMarcBody()-ILSDebugMode- {0}.", debugEx.Message);
                    Logging.Logger.LogException(msg, ExceptionCategory.ILS.ToString(), debugEx);
                }
                
                string markFieldJson = "\"fields\": [";
                int outerCtr = 1;
                foreach (var marcField in fields)
                {
                    if (marcField.Key == "000")
                        continue;

                    if (outerCtr != 1)
                    {
                        markFieldJson += ",";
                    }
                    outerCtr = 2;
                    if (marcField.Value.Count == 1 && Convert.ToInt32(marcField.Key) < 10) // only fields and tag <10 and subfields if >10 
                    {
                        markFieldJson += "{ \"" + marcField.Key + "\":\"" + marcField.Value[0].Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"}";
                    }
                    else
                    {
                        int ctr = 1;
                        int tagSequence = 0;
                        foreach (var data in marcField.Value)
                        {
                            string[] subfieldsSplitted = data.Split(new string[] { "" }, StringSplitOptions.RemoveEmptyEntries);
                            string subFields = string.Empty;
                            if (marcField.Key == "960")
                            {
                                var containsS = false;
                                var sField = "";
                                foreach (var split in subfieldsSplitted)
                                {
                                    if (split.Substring(0, 1) == "s")
                                    {
                                        sField = split;
                                        containsS = true;
                                    }
                                }
                                if (!containsS)
                                    continue;
                                subfieldsSplitted = new string[] { sField };
                            }
                        
                           
                                
                            if (ctr != 1)
                            {
                                markFieldJson += ",";
                            }
                            ctr = 2;

                            int innerCtr = 1;                         
                            subFields += "{ \"" + marcField.Key + "\": {\"subfields\": [ ";

                            foreach (var split in subfieldsSplitted)
                            {
                                if (innerCtr != 1)
                                {
                                    subFields += ",";
                                }
                                innerCtr = 2;

                                subFields += "{ \"" + split.Substring(0, 1) + "\": \"" + split.Substring(1, split.Length - 1).Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"}";
                            }
                            subFields += "]";

                            if (Indicator.ContainsKey(marcField.Key))
                            {
                                var indicator = Indicator[marcField.Key];
                                var item = indicator[tagSequence];
                                if (!string.IsNullOrEmpty(item))
                                {
                                    if (item.Length == 1)
                                    {
                                        subFields += ", \"ind1\": \"" + item + "\"";
                                        subFields += ", \"ind2\": \" \"";
                                    }
                                    else if (item.Length >= 2)
                                    {
                                        subFields += ", \"ind1\": \"" + item.Substring(0, 1) + "\"";
                                        subFields += ", \"ind2\": \"" + item.Substring(1, 1) + "\"";
                                    }
                                }
                                else
                                {
                                    subFields += ", \"ind1\": \" \"";
                                    subFields += ", \"ind2\": \" \"";
                                }

                                tagSequence++;
                            }

                            markFieldJson += subFields;
                            markFieldJson += "}}";
                        }

                    }

                }
                markFieldJson += "]";

                MarcRecord += "\"marc\" : {" + "\"leader\":\"" + strLeader + "\"," + markFieldJson + "}";
                return MarcRecord;
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }
        private string RemoveIllegalChar(string inputString)
        {
            return Regex.Replace(inputString, @"[^0-9a-zA-Z:\s,'{}!@#$%&*()\/]+", "");
        }

        private string RemoveIllegalCharForHeader(string inputString)
        {
            return Regex.Replace(inputString, @"[^0-9a-zA-Z:,'{}!@#$%&*()\/]+", "");
        }
    }
}
