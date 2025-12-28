using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.Authentication.Models
{
    [BsonIgnoreExtraElements]
    public class MongoAuthenticationLog
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonIgnoreIfNull]
        public string UserID { get; set; }
        [BsonIgnoreIfNull]
        public string AppID { get; set; }
        [BsonIgnoreIfNull]
        public string AuthCode { get; set; }
        [BsonIgnoreIfNull]
        public string AccessToken { get; set; }
        [BsonIgnoreIfNull]
        public string RefreshToken { get; set; }

        [BsonIgnoreIfNull]
        public DateTime AuthCodeExpirationDate { get; set; }
        [BsonIgnoreIfNull]
        public DateTime AccessTokenExpirationDate { get; set; }

        [BsonIgnoreIfNull]
        public FootprintInformation FootprintInformation { get; set; }

    }
}