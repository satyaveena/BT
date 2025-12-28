using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BT.ETS.Business.Models
{
    public class DuplicateSeriesTitleResponse
    {
        public List<SeriesDuplicateTitle> SeriesDuplicateTitleList { get; set; }
    }

    public class SeriesDuplicateTitle
    {
        public string BTKey { get; set; }
        public string DuplicateProfiledSeriesIdList { get; set; }
        public List<string> SeriesIdList { get; set; }
    }

    public class ProfiledSeries
    {
        public string ProfiledSeriesID { get; set; }
        
        public string ProfileID { get; set; }
        
        public string SeriesID { get; set; }
        
        public string Note { get; set; }  //This is populated from TS360
        
        public string RequestStatus { get; set; }
        
        public string ChangeRequestUserID { get; set; }  //PENDING
        //
        //public string Status { get; set; }   //2016-07-22  DHB There is no ProfiledSeries Status
        
        public int TotalPrimaryQuantity { get; set; }  //2016-07-22 Ralph requires for Dup Check Results
        
        public FootprintInformation FootprintInformation { get; set; }
        
        public List<PO> PurchaseOrders { get; set; }
        
        public RedundantSeriesData RedundantSeriesInformation { get; set; }
        
        public RedundantProfileData RedundantProfileInformation { get; set; } //2016-02-11 Cerisa Required for report
    }

    public class PO
    {

        public string PurchaseOrderID { get; set; }
        
        public string POLineNumber { get; set; }
                                    //PENDING
        public string POLineNumberProposed { get; set; }//PENDING
        //
        //
        //public int? NonGridQuantity { get; set; }  //2016-07-22 DHB  There ain't no such thing.
        
        
        public string StartDate { get; set; }  //2016-07-25 DHB  StartData is now known as StartDate
        
        
        public string ShippingPreference { get; set; }  //2016-07-25 DHB  Cycle is now known as shipping preference
        
        
        public string Status { get; set; }
        
        
        public string FormatPreferencePrimary { get; set; }  //2016-02-18  Systems requirement
        
        
        public int FormatPreferencePrimaryQuantity { get; set; }  //2016-03-03  New Business Requirement
        
        
        public string FormatPreferenceSecondary { get; set; } //2016-02-18  Systems requirement
        
        
        public int FormatPreferenceSecondaryQuantity { get; set; } //2016-03-03  New Business Requirement
        
        
        public string FormatPreferenceString { get; set; } //2016-07-25 DHB Corrected the name
        
        
        public List<POGrid> PurchaseOrderGrids { get; set; }  //2016-07-26  Re-evaluated the requirements
        
        
        public FootprintInformation FootprintInformation { get; set; }

        
        
        public bool StartNextAvailableTitle { get; set; }


        public class PORecords
        {
            public PORecords()
            {
                POData = new List<PO>();
            }
            public List<PO> POData { get; set; }
        }

    }

    public class POGrid
    {

        public string PurchaseOrderGridID { get; set; }
    
        public string AgencyCodeID { get; set; }
        
        public string ItemTypeID { get; set; }
        
        public string CollectionID { get; set; }
        
        public string UserCode1ID { get; set; }
        
        public string UserCode2ID { get; set; }
        
        public string UserCode3ID { get; set; }
        
        public string UserCode4ID { get; set; }
        
        public string UserCode5ID { get; set; }
        
        public string UserCode6ID { get; set; }
        
        public string CallNumberText { get; set; }
        
        public int Sequence { get; set; }
        
        public int? Quantity { get; set; }

        public FootprintInformation FootprintInformation { get; set; }

        public class POGridRecords
        {
            public POGridRecords()
            {
                POGridData = new List<POGrid>();
            }
            public List<POGrid> POGridData { get; set; }
        }
    }

    public class RedundantSeriesData  //1:1 Relationship
    {
        
        public string Name { get; set; }
        
        public string Author { get; set; }  //2016-02-11 Cerisa Required for report
        
        public string Format { get; set; }
        
        public string Audience { get; set; }
        
        public string Publisher { get; set; }
        
        public string Distributor { get; set; }                 //2017-09-20 CDM TFS 26484
        
        public string Frequency { get; set; }  //2016-02-18 Clarified by Nicole
        
        public List<string> Programs { get; set; }
        
        public List<string> AreasOfInterest { get; set; }
        
        public string RequestStatus { get; set; }
        
        public string Status { get; set; }  //2016-03-03 Cerisa required for report
        
        public LatestIssueInformation LatestIssueInformation { get; set; } //2016-02-11 A better name
        
        public List<LatestIssueInformation> ForthcomingTitles { get; set; }
        
        public FootprintInformation FootprintInformation { get; set; }

        
        public bool HasRelatedSeries { get; set; } //2017-05-10 CDM TFS 25131
        
        public List<string> RelatedSeriesIDs { get; set; }          //2017-11-14 CDM TFS 27466
        
        public List<BindingPreference> BindingPreferences { get; set; }
        
        public bool HasBindingPreferences { get; set; }
    }

    public class BindingPreference
    {
        
        public string Literal { get; set; }
        
        public string PrimaryPreference { get; set; }
        
        public string SecondaryPreference { get; set; }
        
        public bool HasMultiplePreference { get; set; }
    }

    public class LatestIssueInformation //1:1 Relationship
    {
        
        public string BTKey { get; set; }  //2016-02-23 Ralph required for UI
        
        public string ISBN { get; set; }
        
        public decimal? ListPrice { get; set; }
        
        public DateTime? PublicationDate { get; set; }
        
        public string Edition { get; set; }
        
        public string Title { get; set; }
        
        public string Author { get; set; }
        
        public string Format { get; set; }
    }

    public class RedundantProfileData  //2016-02-11 Cerisa Required for report
    {
        
        public string CompassAccountNumber { get; set; }  //2016-02-18  Jamie 
        
        public string ShippingAccountNumber { get; set; }
        
        public string Name { get; set; }
        
        public string OrganizationID { get; set; }
        
        public int? TotalSeries { get; set; } //2016-03-10 Cerisa required for report 
        
        public int? TotalCopies { get; set; }  //2016-03-03 Cerisa required for report
        
        public string Status { get; set; }  //2016-03-04  Ralph request
        
        public string ProfileType { get; set; } //2016-03-04  Ralph request
        
        public List<string> Programs { get; set; }    //2016-04-08  Ralph 
        
        public string SalesTerritory { get; set; }  //2016-07-26  Ralph 
    }

    [BsonIgnoreExtraElements]
    public class FootprintInformation  //1:1 Relationship
    {
        [BsonIgnoreIfNull]
        public string CreatedBy { get; set; }
        [BsonIgnoreIfNull]
        public string CreatedByUserID { get; set; }  //2016-07-27 Ralph requested 
        [BsonIgnoreIfDefault]
        public DateTime CreatedDate { get; set; }
        [BsonIgnoreIfNull]
        public string UpdatedBy { get; set; }
        [BsonIgnoreIfNull]
        public string UpdatedByUserID { get; set; }  //2016-07-27 Ralph requested 
        [BsonIgnoreIfDefault]
        public DateTime UpdatedDate { get; set; }
    }
}