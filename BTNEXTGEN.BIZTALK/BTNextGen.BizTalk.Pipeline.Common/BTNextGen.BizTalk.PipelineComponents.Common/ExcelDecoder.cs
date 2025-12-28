using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.OleDb;
using System.Data;


namespace BTNextGen.BizTalk.PipelineComponents.Common
{

    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Decoder)]
    [System.Runtime.InteropServices.Guid("0D3CB56E-DB90-42EB-90D4-EAB0ADF72B1B")]
    public class ExcelDecoder : IBaseComponent,
                                      IComponentUI,
                                      IComponent,
                                      IPersistPropertyBag
    {
        string filePropertiesNamespace = @"http://schemas.microsoft.com/BizTalk/2003/file-properties";
        string systemPropertiesNamespace = @"http://schemas.microsoft.com/BizTalk/2003/system-properties";
        #region Constructors

        public ExcelDecoder()
        {

        }

        #endregion

        #region IBaseComponent Members

        public string Description
        {
            get { return "Convert Excel to XML"; }
        }

        public string Name
        {
            get { return "Excel Decoder"; }
        }

        public string Version
        {
            get { return "1.0.0.0"; }
        }

        #endregion

        #region IPersistPropertyBag Members
        private string connectionString = null;
        [System.ComponentModel.Description("Excel Connection String")]
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        private string filter = null;
        [System.ComponentModel.Description("Filter for Select Statement")]
        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }


        private string sqlStatement = null;
        [System.ComponentModel.Description("Select Statement to Read ODBC Files.")]
        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; }
        }

        private string tempDropFolderLocation = null;
        [System.ComponentModel.Description("Temp Folder for Dropping ODBC Files.")]
        public string TempDropFolderLocation
        {
            get { return tempDropFolderLocation; }
            set { tempDropFolderLocation = value; }
        }
        

        private bool deleteTempMessages;
        [System.ComponentModel.Description("Delete Temp Messages after processing")]
        public bool DeleteTempMessages
        {
            get { return deleteTempMessages; }
            set { deleteTempMessages = value; }
        }

        private string fnamespace = null;
        [System.ComponentModel.Description("NameSpace for resultant XML Message, for example:")]
        public string NameSpace
        {
            get { return fnamespace; }
            set { fnamespace = value; }
        }

        private string rootNode = null;
        [System.ComponentModel.Description("Root Node Name for resultant XML Message")]
        public string RootNodeName
        {
            get { return rootNode; }
            set { rootNode = value; }
        }

        private string dataNode = null;
        [System.ComponentModel.Description("Data Node Name for resultant XML Message rows")]
        public string DataNodeName
        {
            get { return dataNode; }
            set { dataNode = value; }
        }

        public void GetClassID(out Guid classID)
        {
            classID = new Guid("0D3CB56E-DB90-42EB-90D4-EAB0ADF72B1B");
        }

        public void InitNew()
        {
        }

         void IPersistPropertyBag.Load(IPropertyBag propertyBag, int errorLog)
        {


            object valConnectionString = null,
                    valtempDropFolderLocation = null,
                    valSqlStatement = null,
                    valDeleteTempMessages = null,                    
                    valRootNodeName = null,
                    valNameSpace = null,
                    valDataNodeName = null,
                    valFilter = null;    
                    

            try
            {
                propertyBag.Read("ConnectionString", out valConnectionString, 0);
                propertyBag.Read("TempDropFolderLocation", out valtempDropFolderLocation, 0);
                propertyBag.Read("SqlStatement", out valSqlStatement, 0);
                propertyBag.Read("DeleteTempMessages", out valDeleteTempMessages, 0);                
                propertyBag.Read("RootNodeName", out valRootNodeName, 0);
                propertyBag.Read("NameSpace", out valNameSpace, 0);
                propertyBag.Read("DataNodeName", out valDataNodeName, 0);
                propertyBag.Read("Filter", out valFilter, 0);
                
            }
            catch (ArgumentException argEx)
            {
               // throw argEx;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error reading propertybag: " + ex.Message);
            }

            if (valFilter != null)
                Filter = (string)valFilter;
            else
                Filter = "";

            if (valConnectionString != null)
                ConnectionString = (string)valConnectionString;
            else
                ConnectionString = "";

            if (valtempDropFolderLocation != null)
                TempDropFolderLocation = (string)valtempDropFolderLocation;
            else
                TempDropFolderLocation = "";

            if (valSqlStatement != null)
                SqlStatement = (string)valSqlStatement;
            else
                SqlStatement = "";

            if (valDeleteTempMessages != null)
                DeleteTempMessages = (bool)valDeleteTempMessages;
            else
                DeleteTempMessages = true;

            if (valRootNodeName != null)
                RootNodeName = (string)valRootNodeName;
            else
                RootNodeName = "";

            if (valNameSpace != null)
                NameSpace = (string)valNameSpace;
            else
                NameSpace = "";


            if (valDataNodeName != null)
                DataNodeName = (string)valDataNodeName;
            else
                DataNodeName = "";
            
        }

        void IPersistPropertyBag.Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {


        
            object valConnectionString = (object)ConnectionString;
            propertyBag.Write("ConnectionString", ref valConnectionString);

            object valtempDropFolderLocation = (object)TempDropFolderLocation;
            propertyBag.Write("TempDropFolderLocation", ref valtempDropFolderLocation);

            object valSqlStatement = (object)SqlStatement;
            propertyBag.Write("SqlStatement", ref valSqlStatement);

            object valDeleteTempMessages = (object)DeleteTempMessages;
            propertyBag.Write("DeleteTempMessages", ref valDeleteTempMessages);   

            object valRootNodeName = (object)RootNodeName;
            propertyBag.Write("RootNodeName", ref valRootNodeName);

            object valNameSpace = (object)NameSpace;
            propertyBag.Write("NameSpace", ref valNameSpace);

            object valDataNodeName = (object)DataNodeName;
            propertyBag.Write("DataNodeName", ref valDataNodeName);

            object valFilter = (object)Filter;
            propertyBag.Write("Filter", ref valFilter);


        }

        #endregion

        #region IComponentUI Members

        public IntPtr Icon
        {
            get { return System.IntPtr.Zero; }
        }

        public System.Collections.IEnumerator Validate(object projectSystem)
        {
            return null;
        }

        #endregion

        #region IComponent

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            IBaseMessagePart bodyPart = pInMsg.BodyPart;
            string msgtype = String.Empty;
            if (bodyPart != null)
            {
                try
                {

                    //set default extension
                    string extension = ".xls";
                    //get the filename and extension
                    string rcvFilename = pInMsg.Context.Read("ReceivedFileName", filePropertiesNamespace).ToString();
                    if (rcvFilename.EndsWith(".xls"))
                    {
                        extension = ".xls";
                    }
                    if (rcvFilename.EndsWith(".xlsx"))
                    {
                        extension = ".xlsx";
                    }  
                    // First write the ODBC file to disk so can query it.
                    BinaryReader binaryReader = new BinaryReader(bodyPart.Data);
                    string folderName = this.TempDropFolderLocation;
                    if (folderName.Substring(folderName.Length - 1, 1) != "\\")
                        folderName += "\\";
                
                    string filename = System.IO.Path.GetRandomFileName();
                    filename = filename.Remove(8);
                    filename += extension;
                    string folderNameAndFileName = folderName + filename;
                    FileStream fileStream = new FileStream(folderNameAndFileName, FileMode.CreateNew);
                    BinaryWriter binaryWriter = new BinaryWriter(fileStream);
                    binaryWriter.Write(binaryReader.ReadBytes(Convert.ToInt32(binaryReader.BaseStream.Length)));
                    binaryWriter.Close();
                    binaryReader.Close();

                    // Create the Connection String for the ODBC File
                    string dataSource;
                   // string odbcConnectionString = this.connectionString;
                    string odbcConnectionString = String.Empty;
                    dataSource = "Data Source=" + folderNameAndFileName + ";";
                   if (extension.Equals(".xls"))
                    {
                    odbcConnectionString= "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\MyExcel.xls;Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                    }

                    if (extension.Equals(".xlsx"))
                    {
                   // odbcConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\MyExcel.xls;Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\"";

                    odbcConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\MyExcel.xlsx;Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
                    }

                  

                    if (odbcConnectionString.Substring(odbcConnectionString.Length - 1, 1) != ";")
                        odbcConnectionString += ";";
                    odbcConnectionString += dataSource;
                    OleDbConnection oConn = new OleDbConnection();
                    oConn.ConnectionString = odbcConnectionString;

                    // Create the Select Statement for the ODBC File
                    OleDbDataAdapter oCmd;
                    // Get the filter if there is one
                    string whereClause = "";
                    if (Filter.Trim() != "")
                        whereClause = " Where " + Filter.Trim();
                    oCmd = new OleDbDataAdapter(this.SqlStatement.Trim() + whereClause, oConn);
                    oConn.Open();
                    // Perform the Select statement from above into a dataset, into a DataSet. 
                    DataSet odbcDataSet = new DataSet();
                    oCmd.Fill(odbcDataSet, this.DataNodeName);
                    oConn.Close();
                    // Delete the message 
                    if (this.DeleteTempMessages)
                        System.IO.File.Delete(folderNameAndFileName);

                    // Write the XML From this DataSet into a String Builder
                    System.Text.StringBuilder stringBuilder = new StringBuilder();
                    System.IO.StringWriter stringWriter = new System.IO.StringWriter(stringBuilder);
                    odbcDataSet.Tables[0].WriteXml(stringWriter);

                    System.Xml.XmlDocument fromDataSetXMLDom = new System.Xml.XmlDocument();
                    fromDataSetXMLDom.LoadXml(stringBuilder.ToString());

                    // Create the Final XML Document. Root Node Name and Target Namespace
                    // come from properties set on the pipeline
                    System.Xml.XmlDocument finalMsgXmlDom = new System.Xml.XmlDocument();
                    System.Xml.XmlElement xmlElement;
                    xmlElement = finalMsgXmlDom.CreateElement("ns0", this.RootNodeName, this.NameSpace);
                    finalMsgXmlDom.AppendChild(xmlElement);

                    // Add the XML to the finalMsgXmlDom from the DataSet XML, 
                    // After this the XML Message will be complete
                    finalMsgXmlDom.FirstChild.InnerXml = fromDataSetXMLDom.FirstChild.InnerXml;
                     // finalMsgXmlDom.FirstChild.FirstChild.RemoveAll();

                    Stream strm = new MemoryStream();
                    // Save final XML Document to Stream
                       finalMsgXmlDom.Save(strm);
                    strm.Position = 0;
                    bodyPart.Data = strm;
                    pContext.ResourceTracker.AddResource(strm);
                    msgtype = this.NameSpace + "#" + this.RootNodeName;
                   pInMsg.Context.Promote("MessageType", systemPropertiesNamespace, msgtype);
                }
                catch (System.Exception ex)
                {
                    throw ex;

                }

            }
            return pInMsg;
        }

        #endregion
    }
}

