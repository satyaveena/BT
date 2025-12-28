using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using BT.TS360API.ServiceContracts.Request;

namespace BT.TS360API.MongoDB.Contracts
{
    [BsonIgnoreExtraElements]
    public class BackgroundQueue
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId BackgroundQueueID { get; set; }
        [BsonIgnoreIfNull]
        public int Priority { get; set; }
        [BsonIgnoreIfNull]
        public string InProcessState { get; set; }
        [BsonIgnoreIfNull]
        public string JobType { get; set; }
        [BsonIgnoreIfNull]
        public FootprintInformation FootprintInformation { get; set; }
        [BsonIgnoreIfNull]
        public ReportSettings ReportSettings { get; set; }

    }


    [BsonIgnoreExtraElements]
    public class ReportSettings
    {
        [BsonIgnoreIfNull]
        public string ReportType { get; set; }
         
        [BsonIgnoreIfNull]
        public string UserID { get; set; }

        [BsonIgnoreIfNull]
        public string UserName { get; set; }

        [BsonIgnoreIfNull]
        public OrderSearchLinesRequest OrderSearchLinesRequestDetails { get; set; } 
         

    }
}

