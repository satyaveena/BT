using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BT.TS360API.ServiceContracts.Request
{
    public class OrderSearchLinesResponseResult
    {
        public long LinesCount { get; set; }
        public List<OrderSearchLine> OrderSearchLineList { get; set; }
    }

    public class OrderSearchLine
    {
        public string AccountNumber { get; set; }

        public string OrderDate { get; set; }
        public string Title { get; set; }
        public string CustomerItemNumber { get; set; }
        public string ArtistAuthor { get; set; }
        public string Format { get; set; }
        public string ISBN { get; set; }
        public string UPC { get; set; }
        public string POOrderNumber { get; set; }

        public int Quantity { get; set; }
        public int ShippedQuantity { get; set; }
        public int InProcessQuantity { get; set; }
        public int OnSaleDateHoldQuantity { get; set; }
        public int ReservedAwaitingReleaseQuantity { get; set; }
        public int BackOrderQuantity { get; set; }
        public int CancelledQuantity { get; set; }

        public Decimal NetPrice { get; set; }
        public string PubDate { get; set; }
        public string OrderNumber { get; set; }
        public string Warehouse { get; set; }
        public string POLine { get; set; }
        public string Status { get; set; }
        public string LineItemStatus { get; set; }
        public string CancelledDate { get; set; }
        public string CancelledReasonLiteral { get; set; }
        public string ShipTrackingURL { get; set; }
        public string ShipTrackingNumber { get; set; }
        public string BackorderedReason { get; set; }
        public string BackorderPolicyDaysToCancel { get; set; }
        
        public bool ShipmentDelivered { get; set; }
        public string DeliveryDate { get; set; }


        public bool IsPartialOrderLine { get; set; }
        public List<TrackingInformationItem> TrackingInformation { get; set; }
    }
    public class TrackingInformationItem
    {
        public int Quantity { get; set; }
        public string ShipTrackingNumber { get; set; }
        public string ShipTrackingURL { get; set; }
        public string CarrierCode { get; set; }
    }
}