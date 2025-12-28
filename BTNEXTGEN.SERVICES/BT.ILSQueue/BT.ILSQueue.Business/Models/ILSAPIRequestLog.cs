using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using BT.TS360.NoSQL.Data;

namespace BT.ILSQueue.Business.Models
{
    [BsonIgnoreExtraElements]
    public class ILSAPIRequestLog
    {
        public ILSAPIRequestLog ()
        {
            Priority = 0;
        }

        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId ILSQueueID { get; set; }
         [BsonIgnoreIfNull]
        public string ILSVendorID { get; set; }
        [BsonIgnoreIfNull]
        public string ExternalID { get; set; }
        [BsonIgnoreIfNull]
        public int Priority { get; set; }
        [BsonIgnoreIfNull]
        public string Vendor { get; set; }
        [BsonIgnoreIfNull]
        public string OrderedAtLocation { get; set; }
        [BsonIgnoreIfNull]
        public int OrderType { get; set; }

        [BsonIgnoreIfNull]
        public int PaymentMethod { get; set; }
        [BsonIgnoreIfNull]
        public int Copies { get; set; }
        [BsonIgnoreIfNull]
        public string ProcessingStatus { get; set; }
        [BsonIgnoreIfNull]
        public string PONumber { get; set; }
        [BsonIgnoreIfNull]
        public string PostbackURL { get; set; }

        [BsonIgnoreIfNull]
        public ValidationRequest ValidationRequest { get; set; }

        [BsonIgnoreIfNull]
        public PolarisPOResponse ValidatonResponse { get; set; }

        [BsonIgnoreIfNull]
        public OrderRequest OrderRequest { get; set; }

        [BsonIgnoreIfNull]
        public PolarisPOResponse OrderResponse { get; set; }

        [BsonIgnoreIfNull]
        public PolarisOrderResult OrderResult { get; set; }

        [BsonIgnoreIfNull]
        public FootprintInformation FootprintInformation { get; set; }

        // support fields
        public string OrganizationID { get; set; }
        public string MARCProfileID { get; set; }
        public string UpdatedBy { get; set; }
        public string OrderedDownloadedUserID { get; set; }
        public string BasketName { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class ValidationRequest
    {
        [BsonIgnoreIfNull]
        public List<ILSOrderLineItem> LineItems { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class OrderRequest
    {
        [BsonIgnoreIfNull]
        public List<MARCLineItem> MARCLineItems { get; set; }
    }

   
}
