using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace BT.ETS.Business.Models
{
    [BsonIgnoreExtraElements]
    public class QueueItem
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId JobID { get; set; }
        [BsonIgnoreIfNull]
        public int Priority { get; set; }
        [BsonIgnoreIfNull]
        public int InProcessState { get; set; } //0 = new, 1 = in process, 2 = success, 3 = failed 
        [BsonIgnoreIfNull]
        public string JobType { get; set; } //DupCheck, Pricing, CartReceived 
        [BsonIgnoreIfNull]
        public FootprintInformation FootprintInformation { get; set; }
    }
}