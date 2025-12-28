using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Xml;


namespace MARCProfilerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IMARCProfiler
    {

        //[OperationContract]
        //string GetMARCFileOld(Collection<BTKeySequence> btkeySeq, Int32 MARCProfileID,string Indiciator, String BasketSummaryID, bool isOrdered, bool isCancelled, bool isOCLCEnabled);

        [OperationContract]
        //XmlCDataSection GetMARCFile(String sortColumn, String basketSummaryID, String sortDirection, String ProfileID, string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled);
        //this is the old way
        string GetMARCFile(String sortColumn, String basketSummaryID, String sortDirection, String ProfileID, string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, bool hasInventoryRules, string marketType);


        [OperationContract]

        void SendMARCFileFTP(String sortColumn, String basketSummaryID, String sortDirection, String ProfileID, string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, bool hasInventoryRules, string marketType, string FTPServer, string FTPUserID, string FTPPassword, string FTPFolder, string FTPFilePrefix, string TS360UserID, string TS360CartName );


        [OperationContract]

        void SendMARCFileFTPTest(String FTPServer, String FTPUserID, String FTPPassword, String FTPFolder, String TS360UserID, String MarcProfilerID );


        [OperationContract(IsOneWay = true)]

        void SendMARCFileSP(String sortColumn, String basketSummaryID, String sortDirection, String ProfileID, string FullIndicator, bool isOrdered, bool isCancelled, bool isOCLCEnabled, bool isBTEmployee, bool hasInventoryRules, string marketType, string TS360UserID, string TS360CartName, string ProfileName);


        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
   [DataContract]
    public class BTKeySequence
    {
        [DataMember]
        public string btKey { get; set; }
        [DataMember]
        public Int32 sortSequence { get; set; }
   }
}
