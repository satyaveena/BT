using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using BT.TS360API.ServiceContracts;
using System.Globalization;
using MongoDB.Driver;
using BT.TS360API.MongoDB.Contracts;
using BT.TS360Constants;

namespace BT.TS360API.MongoDB
{
    public class MongoDBHelper
    {        
        private static volatile MongoDBHelper _instance;
        private static readonly object SyncRoot = new Object();

        private MongoDBHelper() { }

        public static MongoDBHelper Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new MongoDBHelper();
                }

                return _instance;
            }
        }

        public List<string> GetMonthYearListInDateRange(string fromDateRange, string toDateRange)
        {
            var result = new List<string>();

            var fromDate = DateTime.ParseExact(fromDateRange, CommonConstants.Search_DateTimeRange_Format, CultureInfo.InvariantCulture);
            var toDate = DateTime.ParseExact(toDateRange, CommonConstants.Search_DateTimeRange_Format, CultureInfo.InvariantCulture);

            result.Add(fromDate.ToString("yyyy-M"));

            while (fromDate.Month < toDate.Month || fromDate.Year < toDate.Year)
            {
                fromDate = fromDate.AddMonths(1);
                result.Add(fromDate.ToString("yyyy-M"));
            }
            return result;
        }

        public FilterDefinition<OrderHistoryStatusInfo> FilterForOrderCharts(List<string> accountNumbers, string fromDate, string toDate)
        {
            var fromdate = DateTime.ParseExact(fromDate, CommonConstants.Search_DateTimeRange_Format, CultureInfo.InvariantCulture);
            var todate = DateTime.ParseExact(toDate, CommonConstants.Search_DateTimeRange_Format, CultureInfo.InvariantCulture);                

            fromdate = fromdate.Date + new TimeSpan(0, 0, 0);
            todate = (todate.AddDays(1).Date) + new TimeSpan(0, 0, 0);

            fromdate = DateTime.SpecifyKind(fromdate, DateTimeKind.Utc);
            todate = DateTime.SpecifyKind(todate, DateTimeKind.Utc);

            var filter = Builders<OrderHistoryStatusInfo>.Filter.In("ShipToAccountNumber", accountNumbers);
            filter = filter & Builders<OrderHistoryStatusInfo>.Filter.Gte("OrderDate", fromdate) &
                                Builders<OrderHistoryStatusInfo>.Filter.Lt("OrderDate", todate);

            return filter;
        }

        public List<string> CreateDateRange(string orderDate)
        {
            // orderDate = "Prev30days" or orderDate = "06102019|07252019"
 
            var newOrderDate = new List<string>();
            var fromDate = string.Empty;
            var toDate = string.Empty;

            if (!string.IsNullOrEmpty(orderDate))
            {
                string[] arr = orderDate.Split('|');
                

                if (arr.Length < 2)
                {
                    DateTime beginDate = DateTime.Now.ToUniversalTime();
                    DateTime nextDate = DateTime.Now.ToUniversalTime();
                    switch (orderDate)
                    {
                        case QueryStringName.Prev30days:
                            nextDate = AddDateTo(-30);
                            break;
                        case QueryStringName.Prev60days:
                            nextDate = AddDateTo(-60);
                            break;
                        case QueryStringName.Prev90days:
                            nextDate = AddDateTo(-90);
                            break;
                        case QueryStringName.Prev120days:
                            nextDate = AddDateTo(-120);
                            break;
                        case QueryStringName.Prev180days:
                            nextDate = AddDateTo(-180);
                            break;
                        case QueryStringName.Prev365days:
                            nextDate = AddDateTo(-365);
                            break;
                        case QueryStringName.Prev12months:
                            nextDate = GetPreviousMonths(-12);
                            beginDate = DateTime.SpecifyKind(beginDate, DateTimeKind.Utc);
                            break;
                        case QueryStringName.Prev24months:
                            nextDate = GetPreviousMonths(-24);
                            beginDate = DateTime.SpecifyKind(beginDate, DateTimeKind.Utc);
                            break;
                    }
                    if (nextDate != beginDate)
                    {
                        if (beginDate > nextDate)
                        {
                            var tempDate = nextDate;
                            nextDate = beginDate;
                            beginDate = tempDate;
                        }
                    }

                    fromDate = beginDate.ToString(CommonConstants.Search_DateTimeRange_Format);
                    toDate = nextDate.ToString(CommonConstants.Search_DateTimeRange_Format);
                }
                else
                {
                    fromDate = arr[0];
                    toDate = arr[1];
                }
            }
            
            newOrderDate.Add(fromDate);
            newOrderDate.Add(toDate);

            return newOrderDate;
        }

        private DateTime AddDateTo(int p)
        {
            return DateTime.Now.AddDays(p).ToUniversalTime();
        }

        private DateTime AddMonthTo(int p)
        {
            return DateTime.Now.AddMonths(p).ToUniversalTime();
        }

        private DateTime GetPreviousMonths(int p)
        {
            var nextDate = AddMonthTo(p);
            nextDate = new DateTime(nextDate.Year, nextDate.Month, 1);
            nextDate = DateTime.SpecifyKind(nextDate, DateTimeKind.Utc);

            return nextDate;
        }
    }
}
