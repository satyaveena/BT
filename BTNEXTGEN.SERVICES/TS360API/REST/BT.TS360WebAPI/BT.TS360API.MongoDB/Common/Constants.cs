using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.MongoDB.Common
{
    public class Constants
    {
        // Mongo database names
        public const string OrdersDatabaseName = "Orders";

        public const string CommonDatabaseName = "Common";

        public const string ProductsDatabaseName = "Products";


        // Mongo Collection names
        public const string DupeCheckCollectionName = "DupeCheck";

        // OrderLines
        public const string OrderLinesCollectionName = "OrderLines";

        public const string BackgroundQueueCollectionName = "BackgroundQueue";

        // Products
        public const string ProductsCollectionName = "Products";
    }

    public class FieldNames
    {
        public const string _ID = "_id";
        public const string BTKey = "BTKey";
        public const string ORDER_STATUS = "OrderStatus";
        public const string INVOICE_NUMBER = "InvoiceNumber";
        public const string SHIP_TO_ACCOUNT_NUMBER = "ShipToAccountNumber";
        public const string PRIMARYRE_SPONSIBLE_PARTY = "PrimaryResponsibleParty";
        public const string CUSTOMER_ITEM_NUMBER = "CustomerItemNumber";
        public const string FORMAT = "Format";
        public const string ISBN = "ISBN";
        public const string ISBN10 = "ISBN10";
        public const string NET_PRICE = "NetPrice";
        public const string ORDER_DATE = "OrderDate";
        public const string ORDER_NUMBER = "OrderNumber";
        public const string PO_LINE = "CustomerPOLineNumber";
        public const string CUSTOMER_PO_NUMBER = "CustomerPONumber";
        public const string PUBLICATION_DATE = "PublicationDate";
        public const string ORDERED_QUANTITY = "OrderedQuantity";
        public const string SHIPPED_QUANTITY = "ShippedQuantity";
        public const string STATUS = "OrderStatus";
        public const string TITLE = "Title";
        public const string LINE_ITEM_STATUS = "LineItemStatus";
        public const string WAREHOUSE = "Warehouse";
        public const string UPC = "UPC";
        public const string Lines_Count = "LinesCount";

        public const string IN_PROCESS_QUANTITY = "InProcessQuantity";
        public const string CANCALLED_QUANTITY = "CancelledQuantity";
        public const string ONSALE_HOLD_QUANTITY = "OnSaleHoldQuantity";
        public const string IN_RESERVE_QUANTITY = "InReserveQuantity";
        public const string BACK_ORDER_QUANTITY = "BackOrderQuantity";
        public const string OPEN_QUANTITY = "OPEN_QUANTITY";

        public const string SHIP_TRACKING_NUMBER = "ShipTrackingNumber";
        public const string CARRIER_CODE = "CarrierCode";
        public const string CANCELLED_DATE = "CancelledDate";
        public const string CANCEL_REASON_CODE = "CancelReasonCode";
        public const string CANCEL_REASON_LITERAL = "CancelledReasonLiteral";
        public const string BACKORDERED_REASON = "BackorderedReason";
        public const string BACKORDER_POLICY_DAYS_TO_CANCEL = "BackorderPolicyDaysToCancel";
        public const string SHIP_TRACKING_URL = "ShipTrackingURL";
        
        public const string DELIVERY_DATE = "DeliveryDate";
        public const string SHIPMENT_DELIVERED = "ShipmentDelivered";
        public const string IS_PARTIAL_ORDER_LINE = "IsPartialOrderLine";
        public const string SHIPPED_PERCENT = "ShippedPercent";
        public const string TRACKING_INFORMATION = "TrackingInformation";
    }
}
