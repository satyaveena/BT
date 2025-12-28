using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BT.ETS.Business.Models
{
    public class LineItemInput
    {
        [BsonId]
        public ObjectId _id { get; set; }

        [BsonIgnoreIfDefault]
        public ObjectId RequestID { get; set; }

        [BsonIgnoreIfNull] 
        public string BTKey { get; set; }
        public int Quantity { get; set; }
        [BsonIgnoreIfNull] 
        public string Note { get; set; }
        [BsonIgnoreIfNull] 
        public List<GridInput> Grids { get; set; }
        [BsonIgnoreIfNull] 
        public RankingInput Ranking { get; set; }
    }

    public class GridInput
    {
        public int Quantity { get; set; }
        [BsonIgnoreIfNull] 
        public string AgencyCode { get; set; }
        [BsonIgnoreIfNull] 
        public string AgencyId { get; set; }
        
        [BsonIgnoreIfNull] 
        public string ItemTypeCode { get; set; }
        [BsonIgnoreIfNull] 
        public string ItemTypeId { get; set; }
        [BsonIgnoreIfNull] 
        public string CollectionCode { get; set; }
        [BsonIgnoreIfNull] 
        public string CollectionId { get; set; }
        [BsonIgnoreIfNull] 
        public string CallNumberText { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode1Code { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode1Id { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode2Code { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode2Id { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode3Code { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode3Id { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode4Code { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode4Id { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode5Code { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode5Id { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode6Code { get; set; }
        [BsonIgnoreIfNull] 
        public string UserCode6Id { get; set; }
    } 

    public class RankingInput
    {
        [BsonIgnoreIfNull] 
        public string Overall { get; set; }
        [BsonIgnoreIfNull] 
        public string Genre_Score { get; set; }
        [BsonIgnoreIfNull] 
        public string Genre_Score_Description { get; set; }
        [BsonIgnoreIfNull] 
        public string Detail_URL { get; set; }
        public int DetailHeight { get; set; }
        public int DetailWidth { get; set; }
        [BsonIgnoreIfNull] 
        public string OveralScoreType { get; set; }

    }
}
