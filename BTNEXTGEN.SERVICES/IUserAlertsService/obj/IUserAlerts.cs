using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace IUserAlerts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    [ServiceKnownType(typeof(AlertMessageTemplateIDEnum))]

    public interface IUserAlerts
    {

        [OperationContract] // (IsOneWay = true)]
        CreateUserAlertMessageResponse CreateUserAlertMessage(String AlertMessage, String UserID, AlertMessageTemplateIDEnum AlertMessageTemplateID, String SourceSystem);

        [OperationContract]
        GetUserAlertMessageTemplateResponse GetUserAlertMessageTemplate(AlertMessageTemplateIDEnum AlertMessageTemplateID);

        // TODO: Add your service operations here
    }


    [DataContract(Name = "AlertMessageTemplateIDEnum")]
    public enum AlertMessageTemplateIDEnum
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        BTFirstLookCDMS = 1,
        [EnumMember]
        MarcDownloadSP = 2,
        [EnumMember]
        SlipReport = 3,
        [EnumMember]
        HoldingsFile = 4,
        [EnumMember]
        BTFirstLooksCATS = 5,
        [EnumMember]
        MassQuantUpdate = 6,
        [EnumMember]
        HoldingsDupChkInactive = 7,
        [EnumMember]
        HoldingsDupChkActive  = 8,
        [EnumMember]
        QuotationsExpireNotice = 9,
        [EnumMember]
        ESPRankComplete = 10,
        [EnumMember]
        ESPRankFailLibrary = 11,
        [EnumMember]
        ESPRankFailSystem = 12,
        [EnumMember]
        DownloadCartComplete = 13,
        [EnumMember]
        DownloadCartFail = 14,
        [EnumMember]
        QuotationReport = 15,
        [EnumMember]
        SingleLineReport = 16,
        [EnumMember]
        SplitCart = 17, 
        [EnumMember]
        ApplyDuplicates = 18, 
        [EnumMember] 
        BatchEntryComplete = 19, 
        [EnumMember] 
        BatchEntryFail = 20, 
        [EnumMember ] 
        BatchEntryCompleteErrors = 21,
        [EnumMember ] 
        CopyCart = 22,
        [EnumMember]
        MergeCart = 23,
        [EnumMember]
        MoveCart = 24,
        [EnumMember]
        ApplyGridTemplate = 25,
        [EnumMember]
        ESPDistWFundComplete = 26,
        [EnumMember]
        ESPDistWFundFail = 27,
        [EnumMember]
        ESPDistWOFundComplete = 28,
        [EnumMember]
        ESPDistWOFundFail = 29,
        [EnumMember]
        ESPFundComplete = 30,
        [EnumMember]
        ESPFundFail = 31


    }
}