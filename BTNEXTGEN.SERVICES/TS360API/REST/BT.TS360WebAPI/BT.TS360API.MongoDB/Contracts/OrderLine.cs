using BT.TS360API.ServiceContracts.Request;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.MongoDB.Contracts
{
    [BsonIgnoreExtraElements]
    public class OrderLine
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId OrderLineID { get; set; }
        [BsonIgnoreIfNull]
        public string CustomerPONumber { get; set; }
        [BsonIgnoreIfNull]
        public string OrderStatus { get; set; }
        [BsonIgnoreIfNull]
        public string TransmissionNumber { get; set; }
        [BsonIgnoreIfNull]
        public string IMSSegmentID { get; set; }
        [BsonIgnoreIfNull]
        public string AccountName { get; set; }
        [BsonIgnoreIfNull]
        public string ShipToAccountNumber { get; set; }
        [BsonIgnoreIfNull]
        public string ShipToAccount8Number { get; set; }
        [BsonIgnoreIfNull]
        public string OrganizationID { get; set; }
        [BsonIgnoreIfNull]
        public string CompanyCode { get; set; }
        [BsonIgnoreIfNull]
        public DateTime OrderDate { get; set; }
        [BsonIgnoreIfNull]
        public string CustomerPOLineNumber { get; set; }
        [BsonIgnoreIfNull]
        public string CustomerItemNumber { get; set; }
        [BsonIgnoreIfNull]
        public string BTKey { get; set; }
        [BsonIgnoreIfNull]
        public string OrderNumber { get; set; }
        [BsonIgnoreIfNull]
        public string Title { get; set; }
        [BsonIgnoreIfNull]
        public string PrimaryResponsibleParty { get; set; }
        [BsonIgnoreIfNull]
        public string ISBN { get; set; }
        [BsonIgnoreIfNull]
        public string UPC { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? PublicationDate { get; set; }
        [BsonIgnoreIfNull]
        public string Format { get; set; }
        [BsonIgnoreIfNull]
        public double NetPrice { get; set; }
        [BsonIgnoreIfNull]
        public double DiscountPercent { get; set; }
        [BsonIgnoreIfNull]
        public string Warehouse { get; set; }
        [BsonIgnoreIfNull]
        public string LineItemStatus { get; set; }
        [BsonIgnoreIfNull]
        public string InvoiceNumber { get; set; }
        [BsonIgnoreIfNull]
        public string ShipTrackingNumber { get; set; }
        [BsonIgnoreIfNull]
        public string CarrierCode { get; set; }
        [BsonIgnoreIfNull]
        public string CancelReasonCode { get; set; }
        [BsonIgnoreIfNull]
        public int BackorderPolicyDaysToCancel { get; set; }
        [BsonIgnoreIfNull]
        public FootprintInformation FootprintInformation { get; set; }
        [BsonIgnoreIfNull]
        public bool NeedsDupeCheckUpdate { get; set; }
        [BsonIgnoreIfNull]
        public int OrderedQuantity { get; set; }
        [BsonIgnoreIfNull]
        public int ShippedQuantity { get; set; }
        [BsonIgnoreIfNull]
        public int CancelledQuantity { get; set; }
        [BsonIgnoreIfNull]
        public int BackOrderQuantity { get; set; }
        [BsonIgnoreIfNull]
        public int InReserveQuantity { get; set; }

        [BsonIgnoreIfNull]
        public bool ShipmentDelivered { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DeliveryDate { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class OrderHistoryStatusInfo
    {
        [BsonIgnoreIfNull]
        public DateTime OrderDate { get; set; }
        [BsonIgnoreIfNull]
        public int OrderDateMonth { get; set; }
        [BsonIgnoreIfNull]
        public string OrderDateMonthLiteral { get; set; }
        [BsonIgnoreIfNull]
        public int OrderDateYear { get; set; }
        [BsonIgnoreIfNull]
        public int OrderedQuantity { get; set; }
        [BsonIgnoreIfNull]
        public int ShippedQuantity { get; set; }
        [BsonIgnoreIfNull]
        public int CancelledQuantity { get; set; }
        [BsonIgnoreIfNull]
        public int BackOrderQuantity { get; set; }
        [BsonIgnoreIfNull]
        public int OnSaleHoldQuantity { get; set; }
        [BsonIgnoreIfNull]
        public int InReserveQuantity { get; set; }
        [BsonIgnoreIfNull]
        public int InProcessQuantity { get; set; }
    }

    public class OrderHistoryShowMonthlyInfo
    {
        public List<OrderHistoryStatusInfo> MonthlyInfoList { get; set; }
        public List<string> MonthYearList { get; set; }
    }
}
