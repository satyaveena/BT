using BT.TS360API.Cache;
using BT.TS360API.ExternalServices.NoSqlAPI;
using BT.TS360API.Logging;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Services
{
    public abstract class NRCControllerBase
    {
        public string[] ProductTypes { get; private set; }
        public string CalendarView { get; private set; }
        public string Month { get; private set; }
        public string Year { get; private set; }

        public NRCControllerBase(string[] productTypes, string calendarView, string month, string year)
        {
            this.ProductTypes = productTypes;
            this.CalendarView = calendarView;
            this.Month = month;
            this.Year = year;
        }

        public abstract string GetCacheKey(ProductTypeEx productType);

        /// <summary>
        /// Gets cached data by product types.
        /// </summary>
        /// <param name="notInCacheProductTypes"></param>
        /// <returns></returns>
        public List<NRCList> GetCachedData(out List<string> notInCacheProductTypes)
        {
            var cachedData = new List<NRCList>();
            var listQuery = new List<string>();

            foreach (var productTypeId in ProductTypes)
            {
                ProductTypeEx productType;
                if (Enum.TryParse<ProductTypeEx>(productTypeId, out productType))
                {
                    var cacheResult = GetCachedItemsByProductType(productType);
                    if (cacheResult != null)
                    {
                        cachedData.AddRange(cacheResult);
                    }
                    else
                    {
                        listQuery.Add(productType.ToString());
                    }
                }
            }

            notInCacheProductTypes = listQuery;

            return cachedData;
        }

        private List<NRCList> GetCachedItemsByProductType(ProductTypeEx productType)
        {
            var cacheKey = GetCacheKey(productType);
            var cacheResult = CachingController.Instance.Read(cacheKey) as List<NRCList>;
            return cacheResult;
        }

        public void WriteItemsToCache(List<NRCList> items, ProductTypeEx productType)
        {
            var cacheKey = GetCacheKey(productType);
            CachingController.Instance.Write(cacheKey, items);
        }

        public List<NRCListFilterView> FilterByCalendarView(List<NRCList> items)
        {
            var result = new List<NRCListFilterView>();

            foreach (var item in items)
            {
                DateTime itemDate;
                if (this.CalendarView == NewReleaseCalendarConst.STREET_DATE_VIEW_TITLE)
                    itemDate = item.StreetDate;
                else
                    itemDate = item.PreOrderDate;

                if (itemDate.Month.ToString() == this.Month && itemDate.Year.ToString() == this.Year)
                {
                    var nrcItem = new NRCListFilterView()
                    {
                        ProductType = item.ProductType,
                        ActiveDate = itemDate,
                        BTKeys = item.BTKeys
                    };
                    result.Add(nrcItem);
                }
            }

            return result;
        }

        public List<NRCListFilterView> FilterByCalendarView(List<NRCList> items, int maxBtKeysPerType)
        {
            var result = new List<NRCListFilterView>();
            var groupByTypes = items.GroupBy(u => u.ProductType).ToList();

            foreach (var groupByItem in groupByTypes)
            {
                var listBTKeys = new List<string>();

                foreach (var item in groupByItem)
                {
                    DateTime itemDate;
                    if (this.CalendarView == NewReleaseCalendarConst.STREET_DATE_VIEW_TITLE)
                        itemDate = item.StreetDate;
                    else
                        itemDate = item.PreOrderDate;

                    if (itemDate.Month.ToString() == this.Month && itemDate.Year.ToString() == this.Year)
                    {
                        foreach (var btKey in item.BTKeys)
                        {
                            listBTKeys.Add(btKey);
                            if (listBTKeys.Count == maxBtKeysPerType)
                                break;
                        }
                    }
                }

                var featuredItem = new NRCListFilterView()
                {
                    ProductType = groupByItem.Key,
                    BTKeys = listBTKeys
                };
                result.Add(featuredItem);
            }
            return result;
        }
    }
}
