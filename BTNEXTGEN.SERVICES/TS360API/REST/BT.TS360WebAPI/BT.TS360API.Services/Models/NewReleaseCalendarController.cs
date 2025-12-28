using BT.TS360API.Services.Common.Configuration;
using BT.TS360API.ServiceContracts;
using BT.TS360Constants;
using BT.TS360SP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Services
{
    public class NewReleaseCalendarController : NRCControllerBase
    {
        public NewReleaseCalendarController(string[] productTypes, string calendarView, string month, string year)
            : base(productTypes, calendarView, month, year)
        {
            
        }

        public Dictionary<string, List<NRCList>> GetSharePointItems(List<string> productTypesList)
        {
            var resultDic = new Dictionary<string, List<NRCList>>();

            var requestDomainName = AppSetting.Ts360SiteUrl;
            var spNRCItems = ContentManagementController.Current.GetNewReleaseCalendarItems(Month, Year, productTypesList, requestDomainName);

            if (spNRCItems.Count > 0)
            {
                // group items by ProductType
                var groupItems = spNRCItems.GroupBy(u => u.ProductType).ToDictionary(r => r.Key, r => r.ToList());

                var listItemsNRC = new List<NRCList>();
                foreach (var groupItem in groupItems)
                {
                    var listItemNRC = new List<NRCList>();
                    foreach (var item in groupItem.Value)
                    {
                        var itemNRC = new NRCList
                        {
                            ProductType = item.ProductType,
                            StreetDate = item.StreetDate,
                            PreOrderDate = item.PreOrderDate,
                            BTKeys = item.BTKeys
                        };
                        listItemNRC.Add(itemNRC);
                    }

                    // add to result dictionary
                    resultDic.Add(groupItem.Key, listItemNRC);
                }
            }

            return resultDic;
        }

        public override string GetCacheKey(ProductTypeEx productType)
        {
            return string.Format("NRC_{0}{1}_{2}", this.Month, this.Year, productType);
        }
    }
}
