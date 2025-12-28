using BT.ETS.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BT.ETS.Business.Models
{
    public class CartReceivedRequest
    {
        [BsonIgnoreIfNull] 
        public string ESPLibraryId { get; set; }
        [BsonIgnoreIfNull] 
        public string ETSCartId { get; set; }
        [BsonIgnoreIfNull] 
        public string CartName { get; set; }
        [BsonIgnoreIfNull] 
        public string CartNote { get; set; }
        [BsonIgnoreIfNull] 
        public string UserId { get; set; }
        //public List<LineItemInput> Items { get; set; }
    }

    public class CartReceivedRequestInput
    {
        [BsonIgnoreIfNull] 
        public string ESPLibraryId { get; set; }
        [BsonIgnoreIfNull] 
        public string ETSCartId { get; set; }
        [BsonIgnoreIfNull] 
        public string CartName { get; set; }
        [BsonIgnoreIfNull] 
        public string CartNote { get; set; }
        [BsonIgnoreIfNull] 
        public string UserId { get; set; }
        [BsonIgnoreIfNull] 
        public List<LineItemInput> Items { get; set; }
    }
}