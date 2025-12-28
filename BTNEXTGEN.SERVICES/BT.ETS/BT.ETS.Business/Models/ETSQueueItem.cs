
using BT.ETS.Business.Constants;
using BT.ETS.Business.Helpers;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace BT.ETS.Business.Models
{
    [BsonIgnoreExtraElements]

    public class ETSQueueItem : QueueItem
    {
        [BsonIgnoreIfNull]
        public DupCheckRequest DupCheckRequest { get; set; }
        [BsonIgnoreIfNull]
        public DupCheckResult DupCheckResponse { get; set; }
        [BsonIgnoreIfNull]
        public PricingRequest PricingRequest { get; set; }
        [BsonIgnoreIfNull]
        public ProductPricingResult PricingResponse { get; set; }
        [BsonIgnoreIfNull]
        public CartReceivedRequest CartReceivedRequest { get; set; }
        [BsonIgnoreIfNull]
        public InsertedCartResult CartReceivedResponse { get; set; }
        [BsonIgnoreIfNull]
        public string ETSRequestStatusMessage { get; set; }
        [BsonIgnoreIfNull]
        public string ETSRequestStatusCode { get; set; }
        [BsonIgnoreIfNull]
        public string ETSResponseStatusMessage { get; set; }
        [BsonIgnoreIfNull]
        public string ETSResponseStatusCode { get; set; }

        public ETSQueueItem()
        {
        }

        public ETSQueueItem(string jobType)
        {
            var now = DateTime.Now;
            var footprintInformation = new FootprintInformation();
            footprintInformation.CreatedDate = footprintInformation.UpdatedDate = now;
            footprintInformation.CreatedBy = footprintInformation.UpdatedBy = "ETS API";
            FootprintInformation = footprintInformation;
            JobType = jobType;
            Priority = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.BackgroundQueuePriority);
            InProcessState = 0;
        }

        public ETSQueueItem(string jobType, int state)
        {
            var now = DateTime.Now;
            var footprintInformation = new FootprintInformation();
            footprintInformation.CreatedDate = footprintInformation.UpdatedDate = now;
            footprintInformation.CreatedBy = footprintInformation.UpdatedBy = "ETS API";
            FootprintInformation = footprintInformation;
            JobType = jobType;
            Priority = AppConfigHelper.RetriveAppSettings<int>(AppConfigHelper.BackgroundQueuePriority);
            InProcessState = state;
        }
    }
}