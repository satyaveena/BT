using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BT.ILSQueue.Business.Constants;
using BT.ILSQueue.Business.Helpers;

using BT.ILSQueue.Business.Models;
using BT.ILSQueue.Business.MARCProfilerReference;
using BT.ILSQueue.Business.MongDBLogger.ELMAHLogger;

using MARC;

namespace BT.ILSQueue.Business.Manager
{
    public class MarcManager
    {
        #region Private Member

        private static volatile MarcManager _instance;
        private static readonly object SyncRoot = new Object();
        public static MarcManager Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new MarcManager();
                }

                return _instance;
            }
        }

        #endregion

        public string PurchaseOrderFolderPath
        {
            get
            {
                return AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.LogFolder) +
                  AppConfigHelper.RetriveAppSettings<string>(AppConfigHelper.PurchaseOrderDirectory);
            }
        }
        
        /*public List<MARC21> GenerateOrderMARC(string sortColumn, string basketSummaryID, string sortDirection, string ProfileID,
            string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, bool hasInventoryRules,
            string marketType, DateTime requestDateTime)*/
        public SortedDictionary<string, MARC21> GenerateOrderMARC(string sortColumn, string basketSummaryID, string sortDirection, string ProfileID,
            string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, bool hasInventoryRules,
            string marketType, DateTime requestDateTime)
        {
            try
            {
                MARCProfiler mp = new MARCProfilerReference.MARCProfiler();

                string MARCContents = mp.GetMARCFile(sortColumn, basketSummaryID, sortDirection, ProfileID, FullIndicator,
                    isOrdered, true, isCancelled, true, isOCLCEnabled, true, isBTEmployee, true, hasInventoryRules, true, marketType);

                const char groupSeparotor = '\x1D';
                const char recordSeparotor = '\x1E';
                const char unitSeparotor = '\x1F';
                MARCContents = MARCContents.Replace("@@@@@@", recordSeparotor.ToString())
                    .Replace("######", unitSeparotor.ToString()).Replace("$$$$$$", groupSeparotor.ToString());

                string folderName = PurchaseOrderFolderPath + "\\";
                string fileName = basketSummaryID + "_" + requestDateTime.ToString("yyyyMMddHHmmss") + "_marc.mrc";

                CreateMARCFile(folderName, fileName, MARCContents);

                return RenderMARC(folderName, fileName);

            }
            catch (Exception ex)
            {
                ex.Source = ApplicationConstants.ELMAH_ERROR_SOURCE_ILS_BACKGROUND;
                ELMAHMongoLogger.Instance.Log(new Elmah.Error(ex));
            }

           // return new List<MARC21>();
            return new SortedDictionary<string, MARC21>();
        }

        private void CreateMARCFile(string folderName, string fileName, string MARCProfilerContent)
        {
            var fs = new FileStream(folderName + fileName, FileMode.Create);
            try
            {
                using (var writer = new BinaryWriter(fs, Encoding.Default))
                {
                    writer.Write(MARCProfilerContent.ToCharArray());
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

       // private List<MARC21> RenderMARC(string folderName, string MARCFileName)
        private SortedDictionary<string, MARC21> RenderMARC(string folderName, string MARCFileName)
        {
            List<MARC21> lstMARC = new List<MARC21>();

            FileMARCReader reader = new FileMARCReader(folderName + MARCFileName);

            //MARCFile mfile = new MARCFile();

            SortedDictionary<string, MARC21> dictMARC21 = new SortedDictionary<string, MARC21>();

            foreach (Record marc in reader)
            {
                //  MARCRecord mrecord = new MARCRecord();
                string BTKey = "";
                MARC21 marc21 = new MARC21();

                marc21.controlFields = new List<MARC21ControlField>();
                marc21.dataFields = new List<MARC21DataField>();

                marc21.leader = marc.Leader;

                foreach (Field field in marc.Fields)
                {
                    string tag = field.Tag;
                    char ind1 = ' ';
                    char ind2 = ' ';
                    string data = "";

                    if (field.IsDataField())
                    {
                        MARC21DataField marcDataField = new MARC21DataField();

                        DataField dataField = (DataField)field;

                        ind1 = dataField.Indicator1;
                        ind2 = dataField.Indicator2;

                        marcDataField.tag = field.Tag;
                        marcDataField.ind1 = ind1;
                        marcDataField.ind2 = ind2;
                        marcDataField.subFields = new List<MARC21SubField>();

                        List<Subfield> lstSubFields = dataField.GetSubfields();
                        List<MARC21SubField> marc21SubFields = new List<MARC21SubField>();

                        foreach (Subfield subField in lstSubFields)
                        {
                            //data += string.Format("${0}{1}", subField.Code, subField.Data);
                            MARC21SubField marcSubField = new MARC21SubField();

                            marcSubField.code = subField.Code;
                            marcSubField.data = subField.Data;

                            marc21SubFields.Add(marcSubField);
                        }

                        marcDataField.subFields.AddRange(marc21SubFields);
                        marc21.dataFields.Add(marcDataField);

                    }
                    else if (field.IsControlField())
                    {
                        ControlField controlField = (ControlField)field;

                        data = controlField.Data;

                        MARC21ControlField marcControlField = new MARC21ControlField();
                        marcControlField.tag = field.Tag;
                        marcControlField.data = data;

                        marc21.controlFields.Add(marcControlField);

                        if (field.Tag == "001")
                        {
                           BTKey = data.Substring(2);
                        }
                    }

                    // MARCField mfield = new MARCField(tag, ind1, ind2, data);

                    //                    mrecord.Add(mfield);

                } // end each record

                //              mfile.Add(mrecord);
                //lstMARC.Add(marc21);
                dictMARC21.Add(BTKey, marc21);
            } //end all records
            reader.Dispose();

           // string marcJson = Newtonsoft.Json.JsonConvert.SerializeObject(lstMARC);

           // int x = 1;
            return dictMARC21;
        }
    }
}
