using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace BT.TS360API.Authentication.Models
{
        [BsonIgnoreExtraElements]
        public class FootprintInformation
        {
            [BsonIgnoreIfNull]
            public string CreatedBy { get; set; }
            [BsonIgnoreIfNull]
            public string CreatedByUserID { get; set; }
            [BsonIgnoreIfNull]
            public DateTime CreatedDate { get; set; }
            [BsonIgnoreIfNull]
            public string UpdatedBy { get; set; }
            [BsonIgnoreIfNull]
            public string UpdatedByUserID { get; set; }
            [BsonIgnoreIfNull]
            public DateTime UpdatedDate { get; set; }
        }
}
