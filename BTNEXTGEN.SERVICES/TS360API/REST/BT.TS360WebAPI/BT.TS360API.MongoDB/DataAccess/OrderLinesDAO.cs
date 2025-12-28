using BT.TS360API.Logging;
using BT.TS360API.MongoDB.Common;
using BT.TS360API.MongoDB.Contracts;
using BT.TS360API.ServiceContracts;
using BT.TS360API.ServiceContracts.Request;
using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BT.TS360API.MongoDB.DataAccess
{
    public class OrderLinesDAO : BaseMongoDAO
    {
        private static OrderLinesDAO _instance = null;
        private static readonly object SyncRoot = new Object();
        readonly IMongoDatabase _orderLinesDatabase;

        #region Singleton

        public static OrderLinesDAO Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new OrderLinesDAO();
                }

                return _instance;
            }
        }

        #endregion

        public override string ConnectionString
        {
            get { return Common.AppSettings.MongoDBConnectionString; }
        }

        public override string DatabaseName
        {
            get { return Constants.OrdersDatabaseName; }
        }

        public OrderLinesDAO()
        {
            var client = new MongoClient(ConnectionString);
            _orderLinesDatabase = client.GetDatabase(DatabaseName);
        }

        internal async Task<List<BsonDocument>> GetRecentOrders(List<string> accountNumbers, int maxResultCount)
        {
            var bsonDocs = new List<BsonDocument>();
            var collection = _orderLinesDatabase.GetCollection<BsonDocument>(Constants.OrderLinesCollectionName);

            // filter
            var filter = Builders<BsonDocument>.Filter;
            var query = filter.And(filter.In(FieldNames.SHIP_TO_ACCOUNT_NUMBER, accountNumbers));

            // sort
            var sort = new SortExpression
            {
                SortField = FieldNames.ORDER_DATE,
                SortDirection = ServiceContracts.Search.SortDirection.Descending
            };
            var sortDefinition = OrderHelper.BuildSortDefinition(sort);

            int retries = base.maxRetries;
            while (retries > 0)
            {
                try
                {
                    bsonDocs = await collection.Aggregate<BsonDocument>()
                                    .Match(query)
                                    .Group(new BsonDocument{ { FieldNames._ID, "$" + FieldNames.ORDER_NUMBER },
                                                             { FieldNames.ORDER_DATE, new BsonDocument("$first", "$" + FieldNames.ORDER_DATE) },
                                                             { FieldNames.CUSTOMER_PO_NUMBER, new BsonDocument("$first", "$" + FieldNames.CUSTOMER_PO_NUMBER) },
                                                             { FieldNames.INVOICE_NUMBER, new BsonDocument("$first", "$" + FieldNames.INVOICE_NUMBER) },
                                                             { FieldNames.ORDER_STATUS, new BsonDocument("$first", "$" + FieldNames.ORDER_STATUS) } })
                                    .Sort(sortDefinition)
                                    .Limit(maxResultCount)
                                    .ToListAsync();

                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw ex;
                    }
                }
            }
            return bsonDocs;
        }

        internal async Task<OrderHistoryStatusInfo> OrderHistoryViewStatus(OrderHistoryStatusRequest request)
        {
            var result = new OrderHistoryStatusInfo();

            var orderDateRange = MongoDBHelper.Instance.CreateDateRange(request.OrderDate);

            if (orderDateRange.Count > 0)
            {
                var fromDate = orderDateRange[0];
                var toDate = orderDateRange[1];

                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    var collection = _orderLinesDatabase.GetCollection<OrderHistoryStatusInfo>(Constants.OrderLinesCollectionName);

                    var filter = MongoDBHelper.Instance.FilterForOrderCharts(request.AccountNumbers, fromDate, toDate);

                    int retries = base.maxRetries;
                    while (retries > 0)
                    {
                        try
                        {
                            result = await collection.Aggregate()
                                .Match(filter)
                                .Group(x => 1,
                                       g => new OrderHistoryStatusInfo()
                                       {

                                           OrderedQuantity = g.Sum(x => x.OrderedQuantity),
                                           ShippedQuantity = g.Sum(x => x.ShippedQuantity),
                                           BackOrderQuantity = g.Sum(x => x.BackOrderQuantity),
                                           CancelledQuantity = g.Sum(x => x.CancelledQuantity),
                                           OnSaleHoldQuantity = g.Sum(x => x.OnSaleHoldQuantity),
                                           InProcessQuantity = g.Sum(x => x.InProcessQuantity),
                                           InReserveQuantity = g.Sum(x => x.InReserveQuantity)
                                       }
                                ).FirstOrDefaultAsync();
                            break;
                        }
                        catch (Exception)
                        {
                            retries--;
                            Thread.Sleep(retryWaitTime);
                            if (retries < 1)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            return result;
        }

        internal async Task<OrderHistoryShowMonthlyInfo> OrderHistoryShowMonthly(OrderHistoryShowMonthlyRequest request)
        {
            var result = new OrderHistoryShowMonthlyInfo();
            result.MonthlyInfoList = new List<OrderHistoryStatusInfo>();
            result.MonthYearList = new List<string>();

            var orderDateRange = MongoDBHelper.Instance.CreateDateRange(request.OrderDate);

            if (orderDateRange.Count > 0)
            {
                var fromDate = orderDateRange[0];
                var toDate = orderDateRange[1];

                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    var collection = _orderLinesDatabase.GetCollection<OrderHistoryStatusInfo>(Constants.OrderLinesCollectionName);

                    var filter = MongoDBHelper.Instance.FilterForOrderCharts(request.AccountNumbers, fromDate, toDate);

                    var monthYearList = MongoDBHelper.Instance.GetMonthYearListInDateRange(fromDate, toDate);
                    if (monthYearList != null)
                        result.MonthYearList = monthYearList;

                    int retries = base.maxRetries;
                    while (retries > 0)
                    {
                        try
                        {
                            result.MonthlyInfoList = await collection.Aggregate()
                                .Match(filter)
                                .Group(x => new { x.OrderDate.Month, x.OrderDate.Year },
                                       g => new OrderHistoryStatusInfo()
                                       {
                                           OrderDateMonth = g.Min(x => x.OrderDate.Month),
                                           OrderDateYear = g.Min(x => x.OrderDate.Year),
                                           OrderedQuantity = g.Sum(x => x.OrderedQuantity),
                                           ShippedQuantity = g.Sum(x => x.ShippedQuantity),
                                           BackOrderQuantity = g.Sum(x => x.BackOrderQuantity),
                                           CancelledQuantity = g.Sum(x => x.CancelledQuantity),
                                           OnSaleHoldQuantity = g.Sum(x => x.OnSaleHoldQuantity),
                                           InProcessQuantity = g.Sum(x => x.InProcessQuantity),
                                           InReserveQuantity = g.Sum(x => x.InReserveQuantity)
                                       }
                                )
                                .ToListAsync();
                            break;
                        }
                        catch (Exception)
                        {
                            retries--;
                            Thread.Sleep(retryWaitTime);
                            if (retries < 1)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<BsonDocument>> GetSearchLines(OrderSearchLinesRequest request)
        {
            var bsonDocs = new List<BsonDocument>();
            var collection = _orderLinesDatabase.GetCollection<BsonDocument>(Constants.OrderLinesCollectionName);

            var filter = BuildersSearchLineFilter(request);
            var sort = OrderHelper.BuildCommonSort(request.SortBy, request.SortByDirection);

            int retries = base.maxRetries;
            var findOptions = new FindOptions() { Collation = new Collation("en", strength: CollationStrength.Secondary) };
            while (retries > 0)
            {
                try
                {
                    if (request.PageSize == -1)
                    {
                        bsonDocs = await collection.Find<BsonDocument>(filter, findOptions).Sort(sort).ToListAsync();
                    }
                    else
                    {
                        bsonDocs = await collection.Find<BsonDocument>(filter, findOptions).Sort(sort).Skip(request.PageSize * (request.PageNumber <= 0 ? 0 : request.PageNumber - 1)).Limit(request.PageSize).ToListAsync();
                    }
                    var searchLinesCount = await collection.CountDocumentsAsync(filter);
                    bsonDocs.Add(new BsonDocument(FieldNames.Lines_Count, BsonDouble.Create(searchLinesCount)));
                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw ex;
                    }
                }
            }
            return bsonDocs;
        }

        public async Task<List<BsonDocument>> GetSearchOrders(OrderSearchLinesRequest request)
        {
            var bsonDocs = new List<BsonDocument>();
            var collection = _orderLinesDatabase.GetCollection<BsonDocument>(Constants.OrderLinesCollectionName);

            // var filter = BuildersSearchLineFilter(request);
            var sort = new BsonDocument();
            var pipeline = new List<BsonDocument> { GetSearchOrdersMatch(request), GetSearchOrdersGroup() };

            if (request.SortBy != null)
            {
                if (string.Equals(request.SortBy, FieldNames.SHIPPED_PERCENT, StringComparison.OrdinalIgnoreCase))
                {
                    pipeline.Add(GetSearchOrdersShippedPercentProjection());
                }
                sort = GetSearchOrdersSort(request.SortBy, request.SortByDirection);
                pipeline.Add(sort);
            }
            var skip = new BsonDocument();
            var limit = new BsonDocument();
            if (request.PageSize > 0)
            {
                skip = OrderHelper.BuildSkip(request.PageSize, request.PageNumber);
                limit = OrderHelper.BuildLimit(request.PageSize);
                pipeline.Add(skip);
                pipeline.Add(limit);
            }

            int retries = base.maxRetries;
            while (retries > 0)
            {
                try
                {
                    //bsonDocs = await collection.Find<BsonDocument>(filter).Sort(sort).ToListAsync();
                    bsonDocs = await collection.Aggregate<BsonDocument>(pipeline, new AggregateOptions { AllowDiskUse = true }).ToListAsync();
                    //var searchLinesCount = await collection.CountDocumentsAsync(filter);
                    var searchLinesCount = bsonDocs.Count;
                    bsonDocs.Add(new BsonDocument(FieldNames.Lines_Count, BsonDouble.Create(searchLinesCount)));
                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw ex;
                    }
                }
            }
            return bsonDocs;
        }

        public async Task<long> GetSearchLinesResultCount(OrderSearchLinesRequest request)
        {
            long resultCount = 0;
            var collection = _orderLinesDatabase.GetCollection<BsonDocument>(Constants.OrderLinesCollectionName);

            var filter = BuildersSearchLineFilter(request);

            int retries = base.maxRetries;
            while (retries > 0)
            {
                try
                {
                    // count based on filter only, no need Sort and Paging
                    resultCount = await collection.CountDocumentsAsync(filter);
                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw ex;
                    }
                }
            }

            return resultCount;
        }

        public async Task<long> GetSearchOrdersResultCount(OrderSearchLinesRequest request)
        {
            long resultCount = 0;

            BsonDocument bsonDocResult = null;
            var collection = _orderLinesDatabase.GetCollection<BsonDocument>(Constants.OrderLinesCollectionName);

            // match filter
            var bsonMatch = GetSearchOrdersMatch(request);

            // group by $OrderNumber only
            var bsonSearchOrdersGroup = GetSearchOrdersGroup(groupByOrderNumberOnly: true);

            // count OrderNumber grouped items
            var bsonCount = new BsonDocument { { "$count", "id" } };

            // pipeline with $match, $group & $count
            var pipeline = new List<BsonDocument> { bsonMatch, bsonSearchOrdersGroup, bsonCount };

            int retries = base.maxRetries;
            while (retries > 0)
            {
                try
                {
                    bsonDocResult = await collection.Aggregate<BsonDocument>(pipeline, new AggregateOptions { AllowDiskUse = true })
                                                    .FirstOrDefaultAsync();
                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw ex;
                    }
                }
            }

            if (bsonDocResult != null)
                resultCount = Convert.ToInt64(bsonDocResult["id"]);

            return resultCount;
        }

        private static BsonDocument GetSearchOrdersMatch(OrderSearchLinesRequest request)
        {
            return new BsonDocument("$match", OrderLinesDAO.Instance.BuildSearchOrdersFilter(request));
        }

        private static BsonDocument GetSearchOrdersProjection()
        {
            return new BsonDocument("$project", new BsonDocument{
                    { "PublicationDate", 1 },
                    { "OrderNumber", 1 },
                    { "InProcessQuantity", 1 },
                    { "OrderedQuantity", 1 },
                    { "CancelledQuantity", 1 },
                    { "OnSaleHoldQuantity", 1 },
                    { "InReserveQuantity", 1 },
                    { "ShippedQuantity", 1 },
                    { "BackOrderQuantity", 1 },
            });
        }

        private static BsonDocument GetSearchOrdersSort(string fieldname, string direction)
        {
            var sortDirection = (direction == "desc") ? -1 : 1;
            if (string.Equals(fieldname, FieldNames._ID))
            {
                return new BsonDocument("$sort", new BsonDocument{
                    { fieldname, sortDirection }
                });
            }
            else
            {
                return new BsonDocument("$sort", new BsonDocument{
                    { fieldname, sortDirection },  { FieldNames._ID, 1 }
                });
            }
        }

        private static BsonDocument GetSearchOrdersGroup(bool groupByOrderNumberOnly = false)
        {
            var bsonGroupValues =  new BsonDocument{{ "_id", "$OrderNumber" }};

            if (!groupByOrderNumberOnly)
            {
                bsonGroupValues.AddRange(new BsonDocument{
                    { FieldNames.OPEN_QUANTITY, 
                        new BsonDocument("$sum", 
                        new BsonDocument("$add", 
                        new BsonArray
                                        {
                                            "$InProcessQuantity",
                                            "$OnSaleHoldQuantity",
                                            "$InReserveQuantity",
                                            "$BackOrderQuantity"
                    })) }, 
                    { FieldNames.IN_PROCESS_QUANTITY, new BsonDocument("$sum", "$InProcessQuantity") }, 
                    { FieldNames.ORDERED_QUANTITY,  new BsonDocument("$sum", "$OrderedQuantity") }, 
                    { FieldNames.CANCALLED_QUANTITY, new BsonDocument("$sum", "$CancelledQuantity") }, 
                    { FieldNames.ONSALE_HOLD_QUANTITY, new BsonDocument("$sum", "$OnSaleHoldQuantity") }, 
                    { FieldNames.IN_RESERVE_QUANTITY, new BsonDocument("$sum", "$InReserveQuantity") }, 
                    { FieldNames.SHIPPED_QUANTITY, new BsonDocument("$sum", "$ShippedQuantity") }, 
                    { FieldNames.BACK_ORDER_QUANTITY, new BsonDocument("$sum", "$BackOrderQuantity") },
                    { FieldNames.ORDER_DATE, new BsonDocument("$first", "$OrderDate")  },
                    { FieldNames.CUSTOMER_PO_NUMBER, new BsonDocument("$first", "$CustomerPONumber")  },
                    { FieldNames.SHIP_TO_ACCOUNT_NUMBER, new BsonDocument("$first", "$ShipToAccountNumber") }
                });
            }

            var bsonGroup = new BsonDocument("$group", bsonGroupValues);
            return bsonGroup;
        }

        private static BsonDocument GetSearchOrdersShippedPercentProjection()
        {
            return new BsonDocument("$project", new BsonDocument{
                { "_id", 1 }, 
                { FieldNames.OPEN_QUANTITY, 1}, 
                { FieldNames.IN_PROCESS_QUANTITY, 1 }, 
                { FieldNames.ORDERED_QUANTITY, 1 }, 
                { FieldNames.CANCALLED_QUANTITY, 1 }, 
                {FieldNames.ONSALE_HOLD_QUANTITY, 1 }, 
                {FieldNames.IN_RESERVE_QUANTITY, 1 }, 
                { FieldNames.SHIPPED_QUANTITY, 1 }, 
                { FieldNames.BACK_ORDER_QUANTITY, 1 },
                { FieldNames.ORDER_DATE, 1 },
                { FieldNames.CUSTOMER_PO_NUMBER, 1  },
                { FieldNames.SHIP_TO_ACCOUNT_NUMBER, 1  },
                { FieldNames.SHIPPED_PERCENT, new BsonDocument("$cond", 
    new BsonArray
                {
                    new BsonDocument("$eq", 
                    new BsonArray
                        {
                            "$OrderedQuantity",
                            0
                        }),
                    0,
                    new BsonDocument("$divide", 
                    new BsonArray
                        {
                            "$ShippedQuantity",
                            "$OrderedQuantity"
                        })
                }) }
                
            });
        }

        internal BsonDocument BuildSearchOrdersFilter(OrderSearchLinesRequest request)
        {
            var filter = new BsonDocument();
            if (request.ViewPoOption == 1)
            {
                filter.AddRange(new BsonDocument(FieldNames.ORDER_STATUS, new BsonDocument("$eq", "O")));
            }
            if (request.AccountNumbers != null && request.AccountNumbers.Count > 0)
            {
                filter.AddRange(new BsonDocument(FieldNames.SHIP_TO_ACCOUNT_NUMBER, new BsonDocument("$in", new BsonArray(request.AccountNumbers))));
            }

            if (!string.IsNullOrEmpty(request.POOrder))
            {
                filter.AddRange(new BsonDocument(FieldNames.CUSTOMER_PO_NUMBER, new BsonDocument("$eq", request.POOrder)));
            }
            if (request.OrderDateRange != null && !string.IsNullOrEmpty(request.OrderDateRange.FromDate) && !string.IsNullOrEmpty(request.OrderDateRange.ToDate))
            {
                try
                {
                    var fromdate = DateTime.ParseExact(request.OrderDateRange.FromDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                    var todate = DateTime.ParseExact(request.OrderDateRange.ToDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                    fromdate = fromdate.Date + new TimeSpan(0, 0, 0);
                    todate = (todate.AddDays(1).Date) + new TimeSpan(0, 0, 0);

                    fromdate = DateTime.SpecifyKind(fromdate, DateTimeKind.Utc);
                    todate = DateTime.SpecifyKind(todate, DateTimeKind.Utc);

                    filter.AddRange(new BsonDocument(FieldNames.ORDER_DATE, new BsonDocument("$gte", fromdate).Add(new BsonDocument("$lte", todate))));
                }
                catch (Exception ex)
                {
                    var msg = string.Format("Can not convert request.OrderDateRange (from: '{0}', to: '{1}') to date time value. {2}. {3}", request.OrderDateRange.FromDate, request.OrderDateRange.ToDate, ex.Message, ex.StackTrace);
                    Logger.WriteLog(ex, "BuildSearchLineFilter");
                }
            }



            return filter;
        }

        internal BsonDocument BuildSearchLineFilter(OrderLineRequest request)
        {
            var filter = new BsonDocument();

            if (request.Titles != null && request.Titles.Count > 0)
            {
                filter.AddRange(new BsonDocument("Title", new BsonDocument("$in", new BsonArray(request.Titles))));
            }

            if (request.ViewPoOption == 1)
            {
                filter.AddRange(new BsonDocument(FieldNames.BACK_ORDER_QUANTITY, new BsonDocument("$gt", 0)));
                filter.AddRange(new BsonDocument(FieldNames.IN_PROCESS_QUANTITY, new BsonDocument("$gt", 0)));
                filter.AddRange(new BsonDocument(FieldNames.ONSALE_HOLD_QUANTITY, new BsonDocument("$gt", 0)));
                filter.AddRange(new BsonDocument(FieldNames.IN_RESERVE_QUANTITY, new BsonDocument("$gt", 0)));
            }

            if (request.AccountNumbers != null && request.AccountNumbers.Count > 0)
            {
                filter.AddRange(new BsonDocument(FieldNames.SHIP_TO_ACCOUNT_NUMBER, new BsonDocument("$in", new BsonArray(request.AccountNumbers))));
            }

            if (request.ArtistAuthors != null && request.ArtistAuthors.Count > 0)
            {
                filter.AddRange(new BsonDocument(FieldNames.PRIMARYRE_SPONSIBLE_PARTY, new BsonDocument("$in", new BsonArray(request.ArtistAuthors))));
            }

            if (request.BTOrderNumbers != null && request.BTOrderNumbers.Count > 0)
            {
                filter.AddRange(new BsonDocument(FieldNames.ORDER_NUMBER, new BsonDocument("$in", new BsonArray(request.BTOrderNumbers))));
            }

            if (request.ISBNs != null && request.ISBNs.Count > 0)
            {
                filter.AddRange(new BsonDocument("$or", new BsonArray
                    {
                        new BsonDocument("ISBN", 
                            new BsonDocument("$in", 
                            new BsonArray(request.ISBNs))),
                        new BsonDocument("ISBN10", 
                            new BsonDocument("$in", 
                            new BsonArray(request.ISBNs)))
                    }));
            }

            if (request.CustomerItemNumbers != null && request.CustomerItemNumbers.Count > 0)
            {
                filter.AddRange(new BsonDocument(FieldNames.CUSTOMER_ITEM_NUMBER, new BsonDocument("$in", new BsonArray(request.CustomerItemNumbers))));
            }

            if (request.Formats != null && request.Formats.Count > 0)
            {
                filter.AddRange(new BsonDocument(FieldNames.FORMAT, new BsonDocument("$in", new BsonArray(request.Formats))));
            }

            if (request.Warehouses != null && request.Warehouses.Count > 0)
            {
                filter.AddRange(new BsonDocument(FieldNames.WAREHOUSE, new BsonDocument("$in", new BsonArray(request.Warehouses))));
            }

            if (request.Statuses != null && request.Statuses.Count > 0)
            {
                filter.AddRange(new BsonDocument(FieldNames.ORDER_STATUS, new BsonDocument("$in", new BsonArray(request.Statuses))));
            }

            if (request.OrderDateRange != null && !string.IsNullOrEmpty(request.OrderDateRange.FromDate) && !string.IsNullOrEmpty(request.OrderDateRange.ToDate))
            {
                try
                {
                    var fromdate = DateTime.ParseExact(request.OrderDateRange.FromDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    var todate = DateTime.ParseExact(request.OrderDateRange.ToDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    fromdate = fromdate.Date + new TimeSpan(0, 0, 0);
                    todate = (todate.AddDays(1).Date) + new TimeSpan(0, 0, 0);

                    fromdate = DateTime.SpecifyKind(fromdate, DateTimeKind.Utc);
                    todate = DateTime.SpecifyKind(todate, DateTimeKind.Utc);

                    filter.AddRange(new BsonDocument(FieldNames.ORDER_DATE, new BsonDocument("$gte", fromdate).Add(new BsonDocument("$lte", todate))));
                }
                catch (Exception ex)
                {
                    var msg = string.Format("Can not convert request.OrderDateRange (from: '{0}', to: '{1}') to date time value. {2}. {3}", request.OrderDateRange.FromDate, request.OrderDateRange.ToDate, ex.Message, ex.StackTrace);
                    Logger.WriteLog(ex, "BuildSearchLineFilter");
                }
            }
            if (request.PubDateRanges != null && !string.IsNullOrEmpty(request.PubDateRanges.FromDate) && !string.IsNullOrEmpty(request.PubDateRanges.ToDate))
            {
                try
                {
                    var fromdate = DateTime.ParseExact(request.PubDateRanges.FromDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    var todate = DateTime.ParseExact(request.PubDateRanges.ToDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    fromdate = fromdate.Date + new TimeSpan(0, 0, 0);
                    todate = (todate.AddDays(1).Date) + new TimeSpan(0, 0, 0);

                    fromdate = DateTime.SpecifyKind(fromdate, DateTimeKind.Utc);
                    todate = DateTime.SpecifyKind(todate, DateTimeKind.Utc);

                    filter.AddRange(new BsonDocument(FieldNames.PUBLICATION_DATE, new BsonDocument("$gte", fromdate).Add(new BsonDocument("$lte", todate))));
                }
                catch (Exception ex)
                {
                    var msg = string.Format("Can not convert request.PubDateRanges (from: '{0}', to: '{1}') to date time value. {2}. {3}", request.PubDateRanges.FromDate, request.PubDateRanges.ToDate, ex.Message, ex.StackTrace);
                    Logger.WriteLog(ex, "BuildSearchLineFilter");
                }
            }

            if (!string.IsNullOrEmpty(request.POLine))
            {
                filter.AddRange(new BsonDocument(FieldNames.CUSTOMER_PO_NUMBER, new BsonDocument("$in", new BsonArray(request.POLine))));
            }

            if (request.CustomerPONumber != null && request.CustomerPONumber.Count > 0)
            {
                filter.AddRange(new BsonDocument(FieldNames.CUSTOMER_PO_NUMBER, new BsonDocument("$in", new BsonArray(request.CustomerPONumber))));
            }

            if (request.UPC != null && request.UPC.Count > 0)
            {
                filter.AddRange(new BsonDocument(FieldNames.UPC, new BsonDocument("$in", new BsonArray(request.UPC))));
            }

            return filter;
        }

        internal FilterDefinition<BsonDocument> ContainFilter(string fieldName, List<string> requestList, FilterDefinition<BsonDocument> filterDef)
        {
            FilterDefinition<BsonDocument> titleFilter = null;
            var builder = Builders<BsonDocument>.Filter;
            requestList.ForEach(t =>
            {
                if (titleFilter == null)
                    titleFilter = builder.Regex(fieldName, new BsonRegularExpression(".*" + t + ".*", "i"));
                else
                    titleFilter = titleFilter | builder.Regex(fieldName, new BsonRegularExpression(".*" + t + ".*", "i"));
            });
            filterDef = filterDef != null ? filterDef & titleFilter : titleFilter;

            return filterDef;
        }
        internal FilterDefinition<BsonDocument> OrContainsFilter(string fieldName, string fieldName2, List<string> requestList, FilterDefinition<BsonDocument> filterDef)
        {
            FilterDefinition<BsonDocument> titleFilter = null;
            var builder = Builders<BsonDocument>.Filter;
            requestList.ForEach(t =>
            {
                if (titleFilter == null)
                {
                    titleFilter = builder.Regex(fieldName, new BsonRegularExpression(".*" + t + ".*", "i"));
                    titleFilter = titleFilter | builder.Regex(fieldName2, new BsonRegularExpression(".*" + t + ".*", "i"));
                }
                else
                {
                    titleFilter = titleFilter | builder.Regex(fieldName, new BsonRegularExpression(".*" + t + ".*", "i"));
                    titleFilter = titleFilter | builder.Regex(fieldName2, new BsonRegularExpression(".*" + t + ".*", "i"));
                }
            });
          
            filterDef = filterDef != null ? filterDef & titleFilter : titleFilter;

            return filterDef;
        }

        internal FilterDefinition<BsonDocument> BuildersSearchLineFilter(OrderLineRequest request)
        {
            var builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = null;

            if (request.AccountNumbers != null && request.AccountNumbers.Count > 0)
            {
                filter = builder.In(FieldNames.SHIP_TO_ACCOUNT_NUMBER, request.AccountNumbers);
            }

            if (request.Titles != null && request.Titles.Count > 0)
            {
                filter = ContainFilter(FieldNames.TITLE, request.Titles, filter);
            }

            //if (request.Titles != null && request.Titles.Count > 0)
            //{
            //    FilterDefinition<BsonDocument> titleFilter = null;
            //    request.Titles.ForEach(t => {
            //        if (titleFilter == null)
            //            titleFilter = builder.Regex(FieldNames.TITLE, new BsonRegularExpression(".*" + t + ".*", "i"));
            //        else
            //            titleFilter = titleFilter | builder.Regex(FieldNames.TITLE, new BsonRegularExpression(".*" + t + ".*", "i"));
            //    });
            //    filter = filter != null ? filter & titleFilter : titleFilter;
            //}

            if (request.ViewPoOption == (int)ViewPOOption.ViewOpenPos)
            {
                if (request.IsOrderLineSearch)
                {
                    FilterDefinition<BsonDocument> titleFilter = null;
                    titleFilter = builder.Gt(FieldNames.BACK_ORDER_QUANTITY, 0);
                    titleFilter = titleFilter | builder.Gt(FieldNames.IN_PROCESS_QUANTITY, 0);
                    titleFilter = titleFilter | builder.Gt(FieldNames.ONSALE_HOLD_QUANTITY, 0);
                    titleFilter = titleFilter | builder.Gt(FieldNames.IN_RESERVE_QUANTITY, 0);
                    filter = filter != null ? filter & titleFilter : titleFilter;

                }
                else
                {
                    var statusFilter = builder.Eq(FieldNames.ORDER_STATUS, "O");
                    filter = filter != null ? filter & statusFilter : statusFilter;

                }

            }

            if (request.ArtistAuthors != null && request.ArtistAuthors.Count > 0)
            {
                filter = ContainFilter(FieldNames.PRIMARYRE_SPONSIBLE_PARTY, request.ArtistAuthors, filter);
            }

            if (request.BTOrderNumbers != null && request.BTOrderNumbers.Count > 0)
            {
                filter = ContainFilter(FieldNames.ORDER_NUMBER, request.BTOrderNumbers, filter);
            }

            if (request.ISBNs != null && request.ISBNs.Count > 0)
            {
                filter = OrContainsFilter(FieldNames.ISBN, FieldNames.ISBN10, request.ISBNs, filter);
            }

            if (request.CustomerItemNumbers != null && request.CustomerItemNumbers.Count > 0)
            {
                filter = ContainFilter(FieldNames.CUSTOMER_ITEM_NUMBER, request.CustomerItemNumbers, filter);
            }

            if (request.Formats != null && request.Formats.Count > 0)
            {
                filter = ContainFilter(FieldNames.FORMAT, request.Formats, filter);
            }

            if (request.Warehouses != null && request.Warehouses.Count > 0)
            {
                filter = ContainFilter(FieldNames.WAREHOUSE, request.Warehouses, filter);
            }

            if (request.Statuses != null && request.Statuses.Count > 0)
            {
                filter = ContainFilter(FieldNames.LINE_ITEM_STATUS, request.Statuses, filter);
                //filter = filter != null ? filter & builder.In(FieldNames.LINE_ITEM_STATUS, request.Statuses) :
                //   builder.In(FieldNames.LINE_ITEM_STATUS, request.Statuses);
            }

            if (!string.IsNullOrEmpty(request.POOrder))
            {
                switch (request.POOrderSearchType)
                {
                    case SearchType.Contains:
                        filter = ContainFilter(FieldNames.CUSTOMER_PO_NUMBER, new List<string> { request.POOrder }, filter);
                        break;
                    case SearchType.Equals:
                        var equalsFilter = builder.Eq(FieldNames.CUSTOMER_PO_NUMBER, request.POOrder);
                        filter = filter != null ? filter & equalsFilter : equalsFilter;
                        break;
                }
            }
           
            if (request.OrderDateRange != null)
            {
                try
                {
                    if(!string.IsNullOrEmpty(request.OrderDateRange.FromDate)){
                        var fromdate = DateTime.ParseExact(request.OrderDateRange.FromDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                        fromdate = fromdate.Date + new TimeSpan(0, 0, 0);
                        fromdate = DateTime.SpecifyKind(fromdate, DateTimeKind.Utc);

                        filter = filter != null ? filter & builder.Gte(FieldNames.ORDER_DATE, fromdate) : builder.Gte(FieldNames.ORDER_DATE, fromdate);
                    }
                    if (!string.IsNullOrEmpty(request.OrderDateRange.ToDate))
                    {
                        var todate = DateTime.ParseExact(request.OrderDateRange.ToDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                        todate = (todate.AddDays(1).Date) + new TimeSpan(0, 0, 0);
                        todate = DateTime.SpecifyKind(todate, DateTimeKind.Utc);

                        filter = filter != null ? filter & builder.Lte(FieldNames.ORDER_DATE, todate) : builder.Lte(FieldNames.ORDER_DATE, todate);
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Format("Can not convert request.OrderDateRange (from: '{0}', to: '{1}') to date time value. {2}. {3}", request.OrderDateRange.FromDate, request.OrderDateRange.ToDate, ex.Message, ex.StackTrace);
                    Logger.WriteLog(ex, "BuildersSearchLineFilter");
                }
            }
            
            if (request.PubDateRanges != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(request.PubDateRanges.FromDate))
                    {
                        var fromdate = DateTime.ParseExact(request.PubDateRanges.FromDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                        fromdate = fromdate.Date + new TimeSpan(0, 0, 0);
                        fromdate = DateTime.SpecifyKind(fromdate, DateTimeKind.Utc);

                        filter = filter != null ? filter & builder.Gte(FieldNames.PUBLICATION_DATE, fromdate) : builder.Gte(FieldNames.PUBLICATION_DATE, fromdate);
                    }
                    if (!string.IsNullOrEmpty(request.PubDateRanges.ToDate))
                    {
                        var todate = DateTime.ParseExact(request.PubDateRanges.ToDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                        todate = (todate.AddDays(1).Date) + new TimeSpan(0, 0, 0);
                        todate = DateTime.SpecifyKind(todate, DateTimeKind.Utc);

                        filter = filter != null ? filter & builder.Lte(FieldNames.PUBLICATION_DATE, todate) : builder.Lte(FieldNames.PUBLICATION_DATE, todate);
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Format("Can not convert request.PubDateRanges (from: '{0}', to: '{1}') to date time value. {2}. {3}", request.PubDateRanges.FromDate, request.PubDateRanges.ToDate, ex.Message, ex.StackTrace);
                    Logger.WriteLog(ex, "BuildersSearchLineFilter");
                }
            }

            if (!string.IsNullOrEmpty(request.POLine))
            {
                filter = ContainFilter(FieldNames.PO_LINE, new List<string> { request.POLine }, filter);
            }

            if (request.CustomerPONumber != null && request.CustomerPONumber.Count > 0)
            {
                filter = ContainFilter(FieldNames.CUSTOMER_PO_NUMBER, request.CustomerPONumber, filter);
            }

            if (request.UPC != null && request.UPC.Count > 0)
            {
                filter = ContainFilter(FieldNames.UPC, request.UPC, filter);
            }

            if (request.ISBNsOrUPCs != null && request.ISBNsOrUPCs.Count > 0)
            {
                var result = OrderHelper.CheckViewByIsbnUPC(request.ISBNsOrUPCs);
                if (!result)
                {
                    var filterISBN = ContainFilter(FieldNames.ISBN, request.ISBNsOrUPCs, filter);
                    var filterUPC = ContainFilter(FieldNames.UPC, request.ISBNsOrUPCs, filter);
                    var filterISBN10 = ContainFilter(FieldNames.ISBN10, request.ISBNsOrUPCs, filter);
                    filter = filterISBN | filterUPC | filterISBN10;
                }
            }

            return filter;
        }

        internal async Task<BsonDocument> GetOrderSearchSummary(FilterDefinition<BsonDocument> match, BsonDocument group)
        {
            var orderLinesCollection = _orderLinesDatabase.GetCollection<BsonDocument>(Constants.OrderLinesCollectionName);
            var result = new BsonDocument();
            int retries = base.maxRetries;

            while (retries > 0)
            {
                try
                {
                    result = await orderLinesCollection.Aggregate<BsonDocument>(new AggregateOptions { AllowDiskUse = true }).Match(match).Group(group).FirstOrDefaultAsync();
                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw ex;
                    }
                }
            }

            return result;
        }

        internal async Task<BsonDocument> GetLineStatus(string orderLineId)
        {
            var orderLinesCollection = _orderLinesDatabase.GetCollection<BsonDocument>(Constants.OrderLinesCollectionName);
            var result = new BsonDocument();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(orderLineId));

            var projection = Builders<BsonDocument>.Projection.Include("OrderLineId").Include("LineItemStatus");
            projection = projection.Include("ShipTrackingNumber").Include("CarrierCode");
            projection = projection.Include("CancelReasonCode").Include("CancelledDate");
            projection = projection.Include("BackorderPolicyDaysToCancel").Include("BackorderedReason");
            //.Include("ShippedExpectedDate") // provided by carrier partner

            int retries = base.maxRetries;
            while (retries > 0)
            {
                try
                {
                    result = await orderLinesCollection.Find(filter).Project(projection).FirstOrDefaultAsync();
                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw ex;
                    }
                }
            }

            return result;
        }

        internal async Task<AggregateFacetResults> GetSearchOrderFacets(OrderLineRequest request)
        {
            var match = BuildersSearchLineFilter(request);
            var orderLinesCollection = _orderLinesDatabase.GetCollection<BsonDocument>(Constants.OrderLinesCollectionName);

            var todayDate = DateTime.Today.AddDays(1).AddTicks(-1);//last time of today.
            var date15ago = todayDate.AddDays(-16).AddTicks(2);
            var date30ago = todayDate.AddDays(-31).AddTicks(2);  //AddDays(-800)
            var date90ago = todayDate.AddDays(-91).AddTicks(2); //AddDays(-3600)


            var orderDateRangeDocument = new BsonDocument("$group",
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
                                                            new BsonDocument("$lte",
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
                                                            new BsonDocument("$lte",
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
                                    });
            var facetPipelineOrderDateRange = AggregateFacet.Create<BsonDocument, BsonDocument>("OrderDateRange", PipelineDefinition<BsonDocument, BsonDocument>.Create(orderDateRangeDocument));
            int retries = base.maxRetries;
            AggregateFacetResults result = null;
            while (retries > 0)
            {
                try
                {
                    result = await orderLinesCollection.Aggregate<BsonDocument>(new AggregateOptions { AllowDiskUse = true }).Match(match).Group(GetSearchOrdersFacetGroup()).Facet(facetPipelineOrderDateRange).FirstOrDefaultAsync();
                   break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw ex;
                    }
                }
            }

            return result;
        }

        internal async Task<AggregateFacetResults> GetSearchLineFacets(OrderLineRequest request)
        {
            var match = BuildersSearchLineFilter(request);
            var orderLinesCollection = _orderLinesDatabase.GetCollection<BsonDocument>(Constants.OrderLinesCollectionName);

            var todayDate = DateTime.Today.AddDays(1).AddTicks(-1);//last time of today.
            var date15ago = todayDate.AddDays(-16).AddTicks(2);
            var date30ago = todayDate.AddDays(-31).AddTicks(2);  //AddDays(-800)
            var date90ago = todayDate.AddDays(-91).AddTicks(2); //AddDays(-3600)

            var orderDateRangeDocument = new BsonDocument("$group",
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
                                                            new BsonDocument("$lte",
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
                                                            new BsonDocument("$lte",
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
                                    });
            var pubDateRangeDocument = new BsonDocument("$group",
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
                                                            new BsonDocument("$lte",
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
                                                            new BsonDocument("$lte",
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
                                    });
            var pipelineLineItemStatus = PipelineDefinition<BsonDocument, AggregateSortByCountResult<string>>.Create(new IPipelineStageDefinition[] { PipelineStageDefinitionBuilder.SortByCount<BsonDocument, string>("$LineItemStatus") });
            var pipelineWarehouse = PipelineDefinition<BsonDocument, AggregateSortByCountResult<string>>.Create(new IPipelineStageDefinition[] { PipelineStageDefinitionBuilder.SortByCount<BsonDocument, string>("$Warehouse") });
            var pipelineFormat = PipelineDefinition<BsonDocument, AggregateSortByCountResult<string>>.Create(new IPipelineStageDefinition[] { PipelineStageDefinitionBuilder.SortByCount<BsonDocument, string>("$Format") });

            var facetPipelineOrderDateRange = AggregateFacet.Create<BsonDocument, BsonDocument>("OrderDateRange", PipelineDefinition<BsonDocument, BsonDocument>.Create(orderDateRangeDocument));
            var facetPipelinePubDateRange = AggregateFacet.Create<BsonDocument, BsonDocument>("PubDateRange", PipelineDefinition<BsonDocument, BsonDocument>.Create(pubDateRangeDocument));
            var facetPipelineLineItemStatus = AggregateFacet.Create("LineItemStatus", pipelineLineItemStatus);
            var facetPipelineWarehouse = AggregateFacet.Create("Warehouse", pipelineWarehouse);
            var facetPipelineFormat = AggregateFacet.Create("Format", pipelineFormat);

            AggregateFacetResults result = null;
            int retries = base.maxRetries;
            while (retries > 0)
            {
                try
                {
                    result = await orderLinesCollection.Aggregate<BsonDocument>(new AggregateOptions { AllowDiskUse = true }).Match(match).Facet(facetPipelineFormat, facetPipelineOrderDateRange, facetPipelinePubDateRange, facetPipelineLineItemStatus, facetPipelineWarehouse).FirstOrDefaultAsync();
                    break;
                }
                catch (Exception ex)
                {
                    retries--;
                    Thread.Sleep(retryWaitTime);
                    if (retries < 1)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }

        private static BsonDocument GetSearchOrdersFacetGroup()
        {
            return new BsonDocument
            {
                { "_id", "$OrderNumber" }, 
                { "OrderDate", new BsonDocument("$first", "$OrderDate") }
            };
        }

        public async Task UpdateDeliveryDetails(string ShipTrackingNumber, bool ShipmentDelivered, DateTimeOffset DeliveryDate)
        {
            var collection = _orderLinesDatabase.GetCollection<OrderLine>(Constants.OrderLinesCollectionName);

            int retries = base.maxRetries;

            while (retries > 0)
            {
                try
                {
                    var filter = Builders<OrderLine>.Filter.Eq("ShipTrackingNumber", ShipTrackingNumber);
                    var update = Builders<OrderLine>.Update.Set(s => s.ShipmentDelivered, ShipmentDelivered)
                                                                        .Set(s => s.DeliveryDate, DeliveryDate.UtcDateTime);
                    await collection.UpdateManyAsync(filter, update);

                    break;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(base.retryWaitTime);
                    if (retries < 1)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
