using BT.TS360API.MongoDB.Common;
using BT.TS360API.MongoDB.Contracts;
using BT.TS360API.MongoDB.DataAccess;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Order;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.MongoDB
{
    public class OrderLinesDAOManager
    {
        public async static Task<List<RecentOrder>> GetRecentOrders(List<string> accountNumbers, int maxResultCount)
        {
            var results = new List<RecentOrder>();

            var bsonDocs = await OrderLinesDAO.Instance.GetRecentOrders(accountNumbers, maxResultCount);
            foreach (var bsonDoc in bsonDocs)
            {
                if (bsonDoc != null)
                {
                    var item = new RecentOrder
                    {
                        OrderNumber = bsonDoc[FieldNames._ID].AsString,
                        OrderDate = bsonDoc[FieldNames.ORDER_DATE].ToUniversalTime().ToString("MM/dd/yyyy"),
                        POOrderNumber = bsonDoc[FieldNames.CUSTOMER_PO_NUMBER].AsStringEx(),
                        InvoiceNumber = bsonDoc[FieldNames.INVOICE_NUMBER].AsStringEx(),
                        OrderStatus = bsonDoc[FieldNames.ORDER_STATUS].AsStringEx()
                    };

                    results.Add(item);
                }
            }
            return results;
        }

        public async static Task<OrderHistoryStatusResponse> OrderHistoryViewStatus(OrderHistoryStatusRequest request)
        {
            var results = new OrderHistoryStatusResponse();
            results.UnitStatusItems = new Dictionary<string, int>();

            var orderInfo = await OrderLinesDAO.Instance.OrderHistoryViewStatus(request);
            if (orderInfo != null)
            {
                results.UnitStatusItems.Add(OrderUnitStatus.Shipped.ToString(), orderInfo.ShippedQuantity);
                results.UnitStatusItems.Add(OrderUnitStatus.Cancelled.ToString(), orderInfo.CancelledQuantity);
                results.UnitStatusItems.Add(OrderUnitStatus.BackOrder.ToString(), orderInfo.BackOrderQuantity);
                results.UnitStatusItems.Add(OrderUnitStatus.OnSaleHold.ToString(), orderInfo.OnSaleHoldQuantity);
                results.UnitStatusItems.Add(OrderUnitStatus.InReserve.ToString(), orderInfo.InReserveQuantity);
                results.UnitStatusItems.Add(OrderUnitStatus.InProcess.ToString(), orderInfo.InProcessQuantity);
                results.TotalUnits = orderInfo.OrderedQuantity;
            }
            return results;
        }

        public async static Task<List<OrderHistoryStatusInfo>> OrderHistoryShowMonthly(OrderHistoryShowMonthlyRequest request)
        {
            var results = new List<OrderHistoryStatusInfo>();

            var orderInfo = await OrderLinesDAO.Instance.OrderHistoryShowMonthly(request);
            var monthsInString = new string[]{"", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul","Aug","Sep","Oct","Nov","Dec"};
            if (orderInfo != null && orderInfo.MonthlyInfoList != null && orderInfo.MonthlyInfoList.Count > 0 &&
                orderInfo.MonthYearList != null && orderInfo.MonthYearList.Count > 0)
            {
                foreach (var monthYear in orderInfo.MonthYearList)
                {
                    var month = Convert.ToInt32(monthYear.Split('-')[1]);
                    var year = Convert.ToInt32(monthYear.Split('-')[0]);
                    var item = orderInfo.MonthlyInfoList.Find(x => x.OrderDateMonth == month && x.OrderDateYear == year);
                    if (item == null)
                    {
                        orderInfo.MonthlyInfoList.Add(new OrderHistoryStatusInfo
                        {
                            OrderDateMonth = month,
                            OrderDateMonthLiteral = monthsInString[month],
                            OrderDateYear = year,
                            OrderedQuantity = 0,
                            ShippedQuantity = 0,
                            CancelledQuantity = 0,
                            BackOrderQuantity = 0,
                            OnSaleHoldQuantity = 0,
                            InProcessQuantity = 0,
                            InReserveQuantity = 0
                        });
                    }
                    else
                    {
                        item.OrderDateMonthLiteral = monthsInString[month];
                    }
                }
                results = orderInfo.MonthlyInfoList.OrderBy(x => x.OrderDateYear).ThenBy(t => t.OrderDateMonth).ToList();
            }
            return results;
        }

        public static async Task<OrderSearchLinesResponseResult> GetSearchLines(OrderSearchLinesRequest request)
        {
            var result = new OrderSearchLinesResponseResult();
            result.OrderSearchLineList = new List<OrderSearchLine>();

            var getResult = await OrderLinesDAO.Instance.GetSearchLines(request);
            if (getResult != null)
            {
                getResult.ForEach(x =>
                {
                    if (x.Contains(FieldNames.Lines_Count))
                    {
                        result.LinesCount = (long)x[FieldNames.Lines_Count].AsDouble; 
                    }else
                        result.OrderSearchLineList.Add(MapBSonDocumentToSearchLines(x));
                });

            }
            //result.LinesCount = OrderLinesDAO.Instance.linesCount;
            return result;
        }

        public static async Task<SearchOrdersResult> GetSearchOrders(OrderSearchLinesRequest request)
        {
            var result = new SearchOrdersResult();
            result.SearchOrdersRespondList = new List<SearchOrdersRespond>();

            var getResult = await OrderLinesDAO.Instance.GetSearchOrders(request);
            if (getResult != null)
            {
                getResult.ForEach(x =>
                {
                    if (x.Contains(FieldNames.Lines_Count))
                    {
                        result.LinesCount = (long)x[FieldNames.Lines_Count].AsDouble;
                    }
                    else
                        result.SearchOrdersRespondList.Add(MapBSonDocumentToOrderLine(x));
                });
                result.POOrder = request.POOrder;
            }
            return result;
        }

        public static async Task<long> GetSearchLinesResultCount(OrderSearchLinesRequest request)
        {
            var linesCount = await OrderLinesDAO.Instance.GetSearchLinesResultCount(request);

            return linesCount;
        }

        public static async Task<long> GetSearchOrdersResultCount(OrderSearchLinesRequest request)
        {
            var ordersCount = await OrderLinesDAO.Instance.GetSearchOrdersResultCount(request);

            return ordersCount;
        }

        private static SearchOrdersRespond MapBSonDocumentToOrderLine(BsonDocument data)
        {
            var dtOrderDate = data.Contains(FieldNames.ORDER_DATE) ? data[FieldNames.ORDER_DATE].AsNullableDateTime : null;
            var item = new SearchOrdersRespond
            {
                AccountNumber = data.Contains(FieldNames.SHIP_TO_ACCOUNT_NUMBER) ? data[FieldNames.SHIP_TO_ACCOUNT_NUMBER].AsString : "",

                OrderDate = dtOrderDate.HasValue ? dtOrderDate.Value.ToString("MM/dd/yyyy") : "",
                OrderNumber = data.Contains("_id") ? data["_id"].AsString : "",
                OrderedQuantity = data.Contains(FieldNames.ORDERED_QUANTITY) ? Convert.ToInt32(data[FieldNames.ORDERED_QUANTITY].ToString()) : 0,
                ShippedQuantity = data.Contains(FieldNames.SHIPPED_QUANTITY) ? Convert.ToInt32(data[FieldNames.SHIPPED_QUANTITY].ToString()) : 0,

                BackOrderQuantity = data.Contains(FieldNames.BACK_ORDER_QUANTITY) ? Convert.ToInt32(data[FieldNames.BACK_ORDER_QUANTITY].ToString()) : 0,
                CancelledQuantity = data.Contains(FieldNames.CANCALLED_QUANTITY) ? Convert.ToInt32(data[FieldNames.CANCALLED_QUANTITY].ToString()) : 0,
                InProcessQuantity = data.Contains(FieldNames.IN_PROCESS_QUANTITY) ? Convert.ToInt32(data[FieldNames.IN_PROCESS_QUANTITY].ToString()) : 0,
                InReserveQuantity = data.Contains(FieldNames.IN_RESERVE_QUANTITY) ? Convert.ToInt32(data[FieldNames.IN_RESERVE_QUANTITY].ToString()) : 0,
                OnSaleHoldQuantity = data.Contains(FieldNames.ONSALE_HOLD_QUANTITY) ? Convert.ToInt32(data[FieldNames.ONSALE_HOLD_QUANTITY].ToString()) : 0,
                OpenQuantity = data.Contains(FieldNames.OPEN_QUANTITY) ? Convert.ToInt32(data[FieldNames.OPEN_QUANTITY].ToString()) : 0,
            };
            return item;
        }

        private static OrderSearchLine MapBSonDocumentToSearchLines(BsonDocument data)
        {//pd
            var dtOrderDate = data.Contains(FieldNames.ORDER_DATE) ? data[FieldNames.ORDER_DATE].AsNullableDateTime : null;
            var dtCancelledDate = data.Contains(FieldNames.CANCELLED_DATE) ? data[FieldNames.CANCELLED_DATE].AsNullableDateTime : null;
            var dtPubDate = data.Contains(FieldNames.PUBLICATION_DATE) ? data[FieldNames.PUBLICATION_DATE].AsNullableDateTime : null;
            var dtDeliveryDate = data.Contains(FieldNames.DELIVERY_DATE) ? data[FieldNames.DELIVERY_DATE].AsNullableDateTime : null;
            var item = new OrderSearchLine
            {
                AccountNumber = data.Contains(FieldNames.SHIP_TO_ACCOUNT_NUMBER) ? data[FieldNames.SHIP_TO_ACCOUNT_NUMBER].AsString : "",
                ArtistAuthor = data.Contains(FieldNames.PRIMARYRE_SPONSIBLE_PARTY) ? data[FieldNames.PRIMARYRE_SPONSIBLE_PARTY].AsString : "",
                CustomerItemNumber = data.Contains(FieldNames.CUSTOMER_ITEM_NUMBER) ? data[FieldNames.CUSTOMER_ITEM_NUMBER].ToString() : "",
                Format = data.Contains(FieldNames.FORMAT) ? data[FieldNames.FORMAT].AsString : "",
                ISBN = data.Contains(FieldNames.ISBN) ? data[FieldNames.ISBN].AsString : "",
                UPC = data.Contains(FieldNames.UPC) ? data[FieldNames.UPC].AsString : "",
                NetPrice = data.Contains(FieldNames.NET_PRICE) ? data[FieldNames.NET_PRICE].ToDecimal() : 0,
                OrderDate = dtOrderDate.HasValue ? dtOrderDate.Value.ToString("MM/dd/yyyy") : "",
                OrderNumber = data.Contains(FieldNames.ORDER_NUMBER) ? data[FieldNames.ORDER_NUMBER].AsString : "",
                POLine = data.Contains(FieldNames.PO_LINE) ? data[FieldNames.PO_LINE].AsString : "",
                POOrderNumber = data.Contains(FieldNames.CUSTOMER_PO_NUMBER) ? data[FieldNames.CUSTOMER_PO_NUMBER].AsString : "",
                PubDate = dtPubDate.HasValue ? dtPubDate.Value.ToString("MM/dd/yyyy") : "",
                Quantity = data.Contains(FieldNames.ORDERED_QUANTITY) ? Convert.ToInt32(data[FieldNames.ORDERED_QUANTITY].ToString()) : 0,
                ShippedQuantity = data.Contains(FieldNames.SHIPPED_QUANTITY) ? Convert.ToInt32(data[FieldNames.SHIPPED_QUANTITY].ToString()) : 0,
                InProcessQuantity = data.Contains(FieldNames.IN_PROCESS_QUANTITY) ? Convert.ToInt32(data[FieldNames.IN_PROCESS_QUANTITY].ToString()) : 0,
                CancelledQuantity = data.Contains(FieldNames.CANCALLED_QUANTITY) ? Convert.ToInt32(data[FieldNames.CANCALLED_QUANTITY].ToString()) : 0,
                OnSaleDateHoldQuantity = data.Contains(FieldNames.ONSALE_HOLD_QUANTITY) ? Convert.ToInt32(data[FieldNames.ONSALE_HOLD_QUANTITY].ToString()) : 0,
                ReservedAwaitingReleaseQuantity = data.Contains(FieldNames.IN_RESERVE_QUANTITY) ? Convert.ToInt32(data[FieldNames.IN_RESERVE_QUANTITY].ToString()) : 0,
                BackOrderQuantity = data.Contains(FieldNames.BACK_ORDER_QUANTITY) ? Convert.ToInt32(data[FieldNames.BACK_ORDER_QUANTITY].ToString()) : 0,
                Status = data.Contains(FieldNames.STATUS) ? data[FieldNames.STATUS].AsString : "",
                Title = data.Contains(FieldNames.TITLE) ? data[FieldNames.TITLE].AsString : "",
                LineItemStatus = data.Contains(FieldNames.LINE_ITEM_STATUS) ? data[FieldNames.LINE_ITEM_STATUS].AsString : "",
                Warehouse = data.Contains(FieldNames.WAREHOUSE) ? data[FieldNames.WAREHOUSE].AsString : "",
                CancelledDate = dtCancelledDate.HasValue ? dtCancelledDate.Value.ToString("MM/dd/yyyy") : "",
                CancelledReasonLiteral = data.Contains(FieldNames.CANCEL_REASON_LITERAL) ? data[FieldNames.CANCEL_REASON_LITERAL].AsString : "",
                ShipTrackingURL = data.Contains(FieldNames.SHIP_TRACKING_URL) ? data[FieldNames.SHIP_TRACKING_URL].AsString : "",
                ShipTrackingNumber = data.Contains(FieldNames.SHIP_TRACKING_NUMBER) ? data[FieldNames.SHIP_TRACKING_NUMBER].AsString : "",
                BackorderedReason = data.Contains(FieldNames.BACKORDERED_REASON) ? data[FieldNames.BACKORDERED_REASON].AsString : "",
                DeliveryDate = dtDeliveryDate.HasValue ? dtDeliveryDate.Value.ToLocalTime().ToString("MM/dd/yyyy") : "",
                ShipmentDelivered = data.Contains(FieldNames.SHIPMENT_DELIVERED) ? data[FieldNames.SHIPMENT_DELIVERED].ToBoolean() : false,
                IsPartialOrderLine = data.Contains(FieldNames.IS_PARTIAL_ORDER_LINE) ? data[FieldNames.IS_PARTIAL_ORDER_LINE].ToBoolean() : false
            };
            if (data.Contains(FieldNames.BACKORDER_POLICY_DAYS_TO_CANCEL) && dtOrderDate.HasValue && string.Equals(item.LineItemStatus, "Backordered", StringComparison.OrdinalIgnoreCase))
            {
                var backorderCancelDays = data[FieldNames.BACKORDER_POLICY_DAYS_TO_CANCEL].AsInt32;
                var currentOrderDays = (DateTime.Now.Date - dtOrderDate.Value.Date).TotalDays;
                item.BackorderPolicyDaysToCancel = (backorderCancelDays - currentOrderDays).ToString();

            }
            if (data.Contains(FieldNames.TRACKING_INFORMATION))
            {
                var trackingItemList = new List<TrackingInformationItem>();
                BsonArray trackingInformation = data[FieldNames.TRACKING_INFORMATION].AsBsonArray;
                foreach (BsonDocument trackingInformationItemDocument in trackingInformation)
                {
                    var trackingInformationItem = new TrackingInformationItem();
                    if (trackingInformationItemDocument.Contains(FieldNames.CARRIER_CODE))
                    {
                        trackingInformationItem.CarrierCode = trackingInformationItemDocument[FieldNames.CARRIER_CODE].AsString;
                    }
                    if (trackingInformationItemDocument.Contains("Quantity"))
                    {
                        trackingInformationItem.Quantity = trackingInformationItemDocument["Quantity"].AsInt32;
                    }
                    if (trackingInformationItemDocument.Contains(FieldNames.SHIP_TRACKING_NUMBER))
                    {
                        trackingInformationItem.ShipTrackingNumber = trackingInformationItemDocument[FieldNames.SHIP_TRACKING_NUMBER].AsString;
                    }
                    if (trackingInformationItemDocument.Contains(FieldNames.SHIP_TRACKING_URL))
                    {
                        trackingInformationItem.ShipTrackingURL = trackingInformationItemDocument[FieldNames.SHIP_TRACKING_URL].AsString;
                    }
                    trackingItemList.Add(trackingInformationItem);
                }
                if(trackingItemList.Count > 0)
                {
                    item.TrackingInformation = trackingItemList.OrderByDescending(x=> x.Quantity).ToList();
                }
            }
            return item;
        }

        public async static Task<OrderSearchSummaryResponse> GetOrderSearchSummary(OrderLineRequest request)
        {
            var group = GetSummaryGroup();
            var match = OrderLinesDAO.Instance.BuildersSearchLineFilter(request);
            var orderSearchSummaryDoc = await OrderLinesDAO.Instance.GetOrderSearchSummary(match, group);
            var orderSearchSummaryResponse = new OrderSearchSummaryResponse();
            if (orderSearchSummaryDoc != null)
            {
                orderSearchSummaryResponse.BackOrderQuantity = orderSearchSummaryDoc.Contains("BackOrderQuantity") ? orderSearchSummaryDoc["BackOrderQuantity"].AsInt32 : 0;
                orderSearchSummaryResponse.CancelledQuantity = orderSearchSummaryDoc.Contains("CancelledQuantity") ? orderSearchSummaryDoc["CancelledQuantity"].AsInt32 : 0;
                orderSearchSummaryResponse.InProcessQuantity = orderSearchSummaryDoc.Contains("InProcessQuantity") ? orderSearchSummaryDoc["InProcessQuantity"].AsInt32 : 0;
                orderSearchSummaryResponse.InReserveQuantity = orderSearchSummaryDoc.Contains("InReserveQuantity") ? orderSearchSummaryDoc["InReserveQuantity"].AsInt32 : 0;
                orderSearchSummaryResponse.OnSaleHoldQuantity = orderSearchSummaryDoc.Contains("OnSaleHoldQuantity") ? orderSearchSummaryDoc["OnSaleHoldQuantity"].AsInt32 : 0;
                orderSearchSummaryResponse.OrderedQuantity = orderSearchSummaryDoc.Contains("OrderedQuantity") ? orderSearchSummaryDoc["OrderedQuantity"].AsInt32 : 0;
                orderSearchSummaryResponse.ShippedQuantity = orderSearchSummaryDoc.Contains("ShippedQuantity") ? orderSearchSummaryDoc["ShippedQuantity"].AsInt32 : 0;
            }
            return orderSearchSummaryResponse;
        }

        private static BsonDocument GetSummaryGroup()
        {
            return new BsonDocument{
            { "_id", "null" }, 
            { "InProcessQuantity", new BsonDocument("$sum", "$InProcessQuantity") }, 
            { "OrderedQuantity",  new BsonDocument("$sum", "$OrderedQuantity") }, 
            { "CancelledQuantity", new BsonDocument("$sum", "$CancelledQuantity") }, 
            { "OnSaleHoldQuantity", new BsonDocument("$sum", "$OnSaleHoldQuantity") }, 
            { "InReserveQuantity", new BsonDocument("$sum", "$InReserveQuantity") }, 
            { "ShippedQuantity", new BsonDocument("$sum", "$ShippedQuantity") }, 
            { "BackOrderQuantity", new BsonDocument("$sum", "$BackOrderQuantity") }};
        }

        private static BsonDocument GetSummaryMatch(OrderLineRequest request)
        {
            BsonElement result = new BsonElement("$match", OrderLinesDAO.Instance.BuildersSearchLineFilter(request).ToBsonDocument());
            return new BsonDocument(result);
        }

        public async static Task<LineStatusResponse> GetLineStatus(string orderLineId)
        {
            var bsonData = await OrderLinesDAO.Instance.GetLineStatus(orderLineId);

            LineStatusResponse returnData = new LineStatusResponse();
            if (bsonData != null)
            {
                returnData.OrderLineId = bsonData.Contains("_id") ? bsonData["_id"].ToString() : "";
                returnData.LineItemStatus = bsonData.Contains(FieldNames.LINE_ITEM_STATUS) ? bsonData[FieldNames.LINE_ITEM_STATUS].AsString : "";
                returnData.ShippedTrackingNumber = bsonData.Contains(FieldNames.SHIP_TRACKING_NUMBER) ? bsonData[FieldNames.SHIP_TRACKING_NUMBER].AsString : "";
                returnData.ShippedCarrier = bsonData.Contains(FieldNames.CARRIER_CODE) ? bsonData[FieldNames.CARRIER_CODE].AsString : "";
                returnData.CancelledDate = bsonData.Contains(FieldNames.CANCELLED_DATE) ? bsonData[FieldNames.CANCELLED_DATE].AsString : "";
                returnData.CancelledReason = bsonData.Contains(FieldNames.CANCEL_REASON_CODE) ? bsonData[FieldNames.CANCEL_REASON_CODE].AsString : "";
                returnData.BackOrderedReason = bsonData.Contains(FieldNames.BACKORDERED_REASON) ? bsonData[FieldNames.BACKORDERED_REASON].AsString : "";
                returnData.BackOrderedNumOfDates = bsonData.Contains(FieldNames.BACKORDER_POLICY_DAYS_TO_CANCEL) ? bsonData[FieldNames.BACKORDER_POLICY_DAYS_TO_CANCEL].AsInt32.ToString() : "";
                returnData.PartialOrderedQty = bsonData.Contains(FieldNames.ORDERED_QUANTITY) ? Convert.ToInt32(bsonData[FieldNames.ORDERED_QUANTITY].ToString()) : 0;
                returnData.PartialShippedQty = bsonData.Contains(FieldNames.SHIPPED_QUANTITY) ? Convert.ToInt32(bsonData[FieldNames.SHIPPED_QUANTITY].ToString()) : 0;
                returnData.PartialBackOrderQty = bsonData.Contains(FieldNames.BACK_ORDER_QUANTITY) ? Convert.ToInt32(bsonData[FieldNames.BACK_ORDER_QUANTITY].ToString()) : 0;
                returnData.PartialCancelledQty = bsonData.Contains(FieldNames.CANCALLED_QUANTITY) ? Convert.ToInt32(bsonData[FieldNames.CANCALLED_QUANTITY].ToString()) : 0;
                returnData.PartialInProcessQty = bsonData.Contains(FieldNames.IN_PROCESS_QUANTITY) ? Convert.ToInt32(bsonData[FieldNames.IN_PROCESS_QUANTITY].ToString()) : 0;
            }
            return returnData;
        }

        private static BsonDocument[] CreateLineFacetPipeline()
        {
            var todayDate = DateTime.Today.AddDays(1).AddTicks(-1);//last time of today.
            var date15ago = todayDate.AddDays(-2729).AddTicks(2); //-16
            var date30ago = todayDate.AddDays(-2533).AddTicks(2); //-31
            var date90ago = todayDate.AddDays(-3650).AddTicks(2); //-91

            var facets = new[]
                {new BsonDocument("$facet", 
                    new BsonDocument
                        {
                            { "Format", 
                    new BsonArray
                            {
                                new BsonDocument("$group", 
                                new BsonDocument
                                    {
                                        { "_id", "$Format" }, 
                                        { "Count", 
                                new BsonDocument("$sum", 1) }
                                    }),
                                new BsonDocument("$sort", 
                                new BsonDocument("_id", 1))
                            } }, 
                            { "LineItemStatus", 
                    new BsonArray
                            {
                                new BsonDocument("$group", 
                                new BsonDocument
                                    {
                                        { "_id", "$LineItemStatus" }, 
                                        { "Count", 
                                new BsonDocument("$sum", 1) }
                                    }),
                                new BsonDocument("$sort", 
                                new BsonDocument("_id", 1))
                            } }, 
                            { "Warehouse", 
                    new BsonArray
                            {
                                new BsonDocument("$group", 
                                new BsonDocument
                                    {
                                        { "_id", "$Warehouse" }, 
                                        { "Count", 
                                new BsonDocument("$sum", 1) }
                                    }),
                                new BsonDocument("$sort", 
                                new BsonDocument("_id", 1))
                            } }, 
                            { "OrderDateRange", 
                    new BsonArray
                            {
                                new BsonDocument("$group", 
                                new BsonDocument
                                    {
                                        { "_id", BsonNull.Value }, 
                                        { "Day15", 
                                new BsonDocument("$sum", 
                                new BsonDocument("$cond", 
                                new BsonArray
                                                {
                                                    new BsonDocument("$and", 
                                                    new BsonArray
                                                        {
                                                            new BsonDocument("$gt", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    date15ago
                                                                }),
                                                            new BsonDocument("$lt", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    todayDate
                                                                })
                                                        }),
                                                    1,
                                                    0
                                                })) }, 
                                        { "Day30", 
                                new BsonDocument("$sum", 
                                new BsonDocument("$cond", 
                                new BsonArray
                                                {
                                                    new BsonDocument("$and", 
                                                    new BsonArray
                                                        {
                                                            new BsonDocument("$gt", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    date30ago
                                                                }),
                                                            new BsonDocument("$lt", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    todayDate
                                                                })
                                                        }),
                                                    1,
                                                    0
                                                })) }, 
                                        { "Day90", 
                                new BsonDocument("$sum", 
                                new BsonDocument("$cond", 
                                new BsonArray
                                                {
                                                    new BsonDocument("$and", 
                                                    new BsonArray
                                                        {
                                                            new BsonDocument("$gt", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    date90ago
                                                                }),
                                                            new BsonDocument("$lte", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    todayDate
                                                                })
                                                        }),
                                                    1,
                                                    0
                                                })) }
                                    })
                            } },
                            { "PubDateRange", 
                    new BsonArray
                            {
                                new BsonDocument("$group", 
                                new BsonDocument
                                    {
                                        { "_id", BsonNull.Value }, 
                                        { "Day15", 
                                new BsonDocument("$sum", 
                                new BsonDocument("$cond", 
                                new BsonArray
                                                {
                                                    new BsonDocument("$and", 
                                                    new BsonArray
                                                        {
                                                            new BsonDocument("$gt", 
                                                            new BsonArray
                                                                {
                                                                    "$PublicationDate",
                                                                    date15ago
                                                                }),
                                                            new BsonDocument("$lt", 
                                                            new BsonArray
                                                                {
                                                                    "$PublicationDate",
                                                                    todayDate
                                                                })
                                                        }),
                                                    1,
                                                    0
                                                })) }, 
                                        { "Day30", 
                                new BsonDocument("$sum", 
                                new BsonDocument("$cond", 
                                new BsonArray
                                                {
                                                    new BsonDocument("$and", 
                                                    new BsonArray
                                                        {
                                                            new BsonDocument("$gt", 
                                                            new BsonArray
                                                                {
                                                                    "$PublicationDate",
                                                                    date30ago
                                                                }),
                                                            new BsonDocument("$lt", 
                                                            new BsonArray
                                                                {
                                                                    "$PublicationDate",
                                                                    todayDate
                                                                })
                                                        }),
                                                    1,
                                                    0
                                                })) }, 
                                        { "Day90", 
                                new BsonDocument("$sum", 
                                new BsonDocument("$cond", 
                                new BsonArray
                                                {
                                                    new BsonDocument("$and", 
                                                    new BsonArray
                                                        {
                                                            new BsonDocument("$gt", 
                                                            new BsonArray
                                                                {
                                                                    "$PublicationDate",
                                                                    date90ago
                                                                }),
                                                            new BsonDocument("$lte", 
                                                            new BsonArray
                                                                {
                                                                    "$PublicationDate",
                                                                    todayDate
                                                                })
                                                        }),
                                                    1,
                                                    0
                                                })) }
                                    })
                            } }
                        })
                };
                 

            return facets;
        }

        private static BsonDocument[] CreateOrderFacetPipeline()
        {
            var todayDate = DateTime.Today.AddDays(1).AddTicks(-1);//last time of today.
            var date15ago = todayDate.AddDays(-16).AddTicks(2);
            var date30ago = todayDate.AddDays(-31).AddTicks(2);
            var date90ago = todayDate.AddDays(-3651).AddTicks(2);

            var facets = new[]
                {new BsonDocument("$facet",
                    new BsonDocument
                        {
                            { "OrderDateRange", 
                    new BsonArray
                            {
                                new BsonDocument("$group", 
                                new BsonDocument
                                    {
                                        { "_id", BsonNull.Value }, 
                                        { "Day15", 
                                new BsonDocument("$sum", 
                                new BsonDocument("$cond", 
                                new BsonArray
                                                {
                                                    new BsonDocument("$and", 
                                                    new BsonArray
                                                        {
                                                            new BsonDocument("$gt", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    date15ago
                                                                }),
                                                            new BsonDocument("$lt", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    todayDate
                                                                })
                                                        }),
                                                    1,
                                                    0
                                                })) }, 
                                        { "Day30", 
                                new BsonDocument("$sum", 
                                new BsonDocument("$cond", 
                                new BsonArray
                                                {
                                                    new BsonDocument("$and", 
                                                    new BsonArray
                                                        {
                                                            new BsonDocument("$gt", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    date30ago
                                                                }),
                                                            new BsonDocument("$lt", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    todayDate
                                                                })
                                                        }),
                                                    1,
                                                    0
                                                })) }, 
                                        { "Day90", 
                                new BsonDocument("$sum", 
                                new BsonDocument("$cond", 
                                new BsonArray
                                                {
                                                    new BsonDocument("$and", 
                                                    new BsonArray
                                                        {
                                                            new BsonDocument("$gt", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    date90ago
                                                                }),
                                                            new BsonDocument("$lte", 
                                                            new BsonArray
                                                                {
                                                                    "$OrderDate",
                                                                    todayDate
                                                                })
                                                        }),
                                                    1,
                                                    0
                                                })) }
                                    })
                            } }
                        })
                };
            return facets;
        }

        public async static Task<SearchLineFacetsResponse> SearchLineFacets(OrderLineRequest request)
        {
            AggregateFacetResults result = await OrderLinesDAO.Instance.GetSearchLineFacets(request);
            SearchLineFacetsResponse facetList = new SearchLineFacetsResponse();
            if (result != null && result.Facets != null)
            {
                foreach (var aggFacet in result.Facets)
                {
                    switch (aggFacet.Name)
                    {
                        case FacetNameConstants.Format:
                            List<TextFacetData> data = BsonSerializer.Deserialize<List<TextFacetData>>(aggFacet.Output<AggregateSortByCountResult<string>>().ToJson());
                            facetList.Format = new TextFacet() { Name = aggFacet.Name, Data = data };
                            break;
                        case FacetNameConstants.OrderDateRange:
                            List<DateRangeFacetData> data3 = BsonSerializer.Deserialize<List<DateRangeFacetData>>(aggFacet.Output<BsonDocument>().ToJson());
                            if (data3 != null && data3.Count > 0)
                                facetList.OrderDate = new DateRangeFacet() { Name = aggFacet.Name, Data = data3[0] };
                            break;
                        case FacetNameConstants.PubDateRange:
                            List<DateRangeFacetData> data4 = BsonSerializer.Deserialize<List<DateRangeFacetData>>(aggFacet.Output<BsonDocument>().ToJson());
                            if (data4 != null && data4.Count > 0)
                                facetList.PubDate = new DateRangeFacet() { Name = aggFacet.Name, Data = data4[0] };
                            break;
                        case FacetNameConstants.LineItemStatus:
                            List<TextFacetData> data1 = BsonSerializer.Deserialize<List<TextFacetData>>(aggFacet.Output<AggregateSortByCountResult<string>>().ToJson());
                            facetList.LineItemStatus = new TextFacet() { Name = aggFacet.Name, Data = data1 };
                            break;
                        case FacetNameConstants.Warehouse:
                            List<TextFacetData> data2 = BsonSerializer.Deserialize<List<TextFacetData>>(aggFacet.Output<AggregateSortByCountResult<string>>().ToJson());
                            facetList.Warehouse = new TextFacet() { Name = aggFacet.Name, Data = data2 };
                            break;
                    }
                }
            }          
            return facetList;
        }

        public async static Task<SearchOrderFacetsResponse> SearchOrderFacets(OrderLineRequest request)
        {
            AggregateFacetResults result = await OrderLinesDAO.Instance.GetSearchOrderFacets(request);
            SearchOrderFacetsResponse facetList = new SearchOrderFacetsResponse();
            if (result != null && result.Facets != null)
            {
                foreach (var aggFacet in result.Facets)
                {
                    switch (aggFacet.Name)
                    {
                        case FacetNameConstants.OrderDateRange:
                            List<DateRangeFacetData> data3 = BsonSerializer.Deserialize<List<DateRangeFacetData>>(aggFacet.Output<BsonDocument>().ToJson());
                            if (data3 != null && data3.Count > 0)
                                facetList.OrderDate = new DateRangeFacet() { Name = aggFacet.Name, Data = data3[0] };
                            break;
                    }
                }
            }
            return facetList;
        }

        public static async Task<OrderSearchExportResponse> SearchLineExport(OrderSearchLinesRequest request, bool skipHeader = false)
        {
            OrderSearchLinesResponseResult lines = await GetSearchLines(request);
            if (lines == null || lines.OrderSearchLineList == null)
                return null;
            OrderSearchExportResponse result = new OrderSearchExportResponse();
            string skipColumnName;
            result.FileName = GenerateExportFilename(request, out skipColumnName);
 
            result.FileContent = GenerateExportFileContent(lines.OrderSearchLineList, skipColumnName, skipHeader);
            result.LineCount = lines.LinesCount;
            return result;
        }

        public static async Task<OrderSearchExportResponse> SearchOrderExport(OrderSearchLinesRequest request, bool skipHeader = false)
        {
            SearchOrdersResult lines = await GetSearchOrders(request);
            if (lines == null || lines.SearchOrdersRespondList == null || lines.SearchOrdersRespondList.Count == 0)
                return null;
            OrderSearchExportResponse result = new OrderSearchExportResponse();
            result.FileName = string.Format("PO_{0}_{1}.csv", request.POOrder, DateTime.Now.ToString("MMddyyyy_HHmmss"));
            result.FileContent = GenerateExportFileContent_Orders(lines.SearchOrdersRespondList, skipHeader);
            result.LineCount = lines.LinesCount;

            return result;
        }

        private static string GenerateExportFilename(OrderSearchLinesRequest request, out string skipColumnName)
        {
            string filename = "";
            if (request.ISBNs != null && request.ISBNs.Count > 0)
            {
                if (request.ISBNsOrUPCs != null && request.ISBNsOrUPCs.Count > 0)
                {
                    var result = OrderHelper.CheckViewByIsbnUPC(request.ISBNsOrUPCs);
                    if (result)
                        filename += "ISBN_UPC_" + request.ISBNs[0] + "_" + request.ISBNsOrUPCs[0].Split(':')[1];
                    else
                        filename += "ISBN_" + request.ISBNs[0];
                }
                else
                    filename += "ISBN_" + request.ISBNs[0];

                //skipColumnName = "ISBN";
            }
            else if (!string.IsNullOrEmpty(request.POLine))
            {
                filename += "PO_" + request.POLine;
                //skipColumnName = "POLine";
            }
            else if (request.BTOrderNumbers != null && request.BTOrderNumbers.Count > 0)
            {
                filename += "Order_" + request.BTOrderNumbers[0];
                //skipColumnName = "BTOrderNumbers";
            }
            else
            {
                filename += "Orders";
                //skipColumnName = "";
            }

            skipColumnName = "";
            filename += DateTime.Now.ToString("_MMddyyyy_HHmmss");
            filename += ".csv";
            return filename;
        }

        private static string GenerateExportFileContent(List<OrderSearchLine> lines, string skipColumnName, bool skipHeader = false)
        {
            StringBuilder filecontent = new StringBuilder();
            var separateChar = ",";

            if (!skipHeader)
            {
                var header = "Account Number,Order Date,Title,Cust Item #,Author/Artist,Format, ISBN/UPC,PO Number,QTY,Net Price,Pub Date,Order #,Warehouse,PO Line,Status";

                // using the same header so comment out all the Switch code below - #37433
                //switch (skipColumnName)
                //{
                //    case "ISBN": header = header.Replace("ISBN/UPC,", "");
                //        break;
                //    case "POLine": header = header.Replace("PO Line,", "");
                //        break;
                //    case "BTOrderNumbers": header = header.Replace("Order #,", "");
                //        break;
                //}

                filecontent.AppendLine(header);
            }
            foreach (var line in lines)
            {
                var accountNumber = string.IsNullOrEmpty(line.AccountNumber) ? "" : line.AccountNumber;
                var orderDate = line.OrderDate;
                var title = (string.IsNullOrEmpty(line.Title) ? "" : line.Title.Replace(separateChar, ""));
                var customerItemNumber = string.IsNullOrEmpty(line.CustomerItemNumber) ? "" : line.CustomerItemNumber.Replace(separateChar, "");
                var author = string.IsNullOrEmpty(line.ArtistAuthor) ? "" : line.ArtistAuthor.Replace(separateChar, "");
                var format = string.IsNullOrEmpty(line.Format) ? "" : line.Format.Replace(separateChar, "");
                
                var isbn = string.IsNullOrEmpty(line.ISBN) ? "" : line.ISBN;

                var upc = string.IsNullOrEmpty(line.UPC) ? "" : line.UPC;                
                if (!string.IsNullOrEmpty(upc))
                    isbn = "\"" + isbn + "\n" + upc + "\"";

                var poOrder = string.IsNullOrEmpty(line.POOrderNumber) ? "" : line.POOrderNumber.Replace(separateChar, "");
                var qty = line.Quantity.ToString();
                var netPrice = line.NetPrice.ToString("#.00");
                var pubDate = line.PubDate;
                var orderNumber = string.IsNullOrEmpty(line.OrderNumber) ? "" : line.OrderNumber.Replace(separateChar, "");
                var wareHouse = string.IsNullOrEmpty(line.Warehouse) ? "" : line.Warehouse.Replace(separateChar, "");
                var poLine = string.IsNullOrEmpty(line.POLine) ? "" : line.POLine.Replace(separateChar, "");
                var status = string.IsNullOrEmpty(line.LineItemStatus) ? "" : line.LineItemStatus.Replace(separateChar, "");

                var insertLine = accountNumber + separateChar
                                + orderDate + separateChar
                                + title + separateChar
                                + customerItemNumber + separateChar
                                + author + separateChar
                                + format + separateChar
                                + isbn + separateChar //(skipColumnName == "ISBN" ? "" : isbn + separateChar)
                                + poOrder + separateChar
                                + qty + separateChar
                                + netPrice + separateChar
                                + pubDate + separateChar
                                + orderNumber + separateChar //(skipColumnName == "BTOrderNumbers" ? "" : orderNumber + separateChar)
                                + wareHouse + separateChar
                                + poLine + separateChar //(skipColumnName == "POLine" ? "" : poLine + separateChar)
                                + status + separateChar;

                filecontent.AppendLine(insertLine);
            }
            return filecontent.ToString();
        }

        private static string GenerateExportFileContent_Orders(List<SearchOrdersRespond> lines, bool skipHeader = false)
        {
            StringBuilder filecontent = new StringBuilder();
            var separateChar = ",";
            if (!skipHeader)
            {
                filecontent.AppendLine("Account Number,Order Date,Order #,Open,In Process,In Reserve,On Sale Date Hold,Backordered,Cancelled,Shipped,Total units ordered,Shipping%");
            }
            foreach (var line in lines)
            {
                var shipPercent = "0.00";
                if (line.OrderedQuantity > 0)
                {
                    shipPercent = ((double)line.ShippedQuantity / (double)line.OrderedQuantity * 100).ToString("0.00");
                }
                var insertLine = line.AccountNumber + separateChar
                    + line.OrderDate + separateChar
                    + line.OrderNumber + separateChar
                    + line.OpenQuantity + separateChar
                    + line.InProcessQuantity + separateChar
                    + line.InReserveQuantity + separateChar
                    + line.OnSaleHoldQuantity + separateChar
                    + line.BackOrderQuantity + separateChar
                    + line.CancelledQuantity + separateChar
                    + line.ShippedQuantity + separateChar
                    + line.OrderedQuantity + separateChar
                    + shipPercent + "%";

                filecontent.AppendLine(insertLine);
            }

            return filecontent.ToString();
        }

        public static async Task UpdateDeliveryDetails(string ShipTrackingNumber, bool ShipmentDelivered, string DeliveryDate)
        {
            if (string.IsNullOrEmpty(DeliveryDate))
                throw new ArgumentNullException("Delivey date is not available");

            // Update to MongoDB
            await OrderLinesDAO.Instance.UpdateDeliveryDetails(ShipTrackingNumber, ShipmentDelivered, DateTimeOffset.Parse(DeliveryDate));
        }
    }
}
