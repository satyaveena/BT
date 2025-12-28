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
    public class FeaturedTitlesController : NRCControllerBase
    {
        public FeaturedTitlesController(string[] productTypes, string calendarView, string month, string year)
            : base(productTypes, calendarView, month, year)
        {
            
        }

        public override string GetCacheKey(ProductTypeEx productType)
        {
            return string.Format("Featured_{0}{1}_{2}", this.Month, this.Year, productType);
        }

        public Dictionary<string, List<NRCList>> GetSharePointItems(List<string> productTypesList)
        {
            var resultDic = new Dictionary<string, List<NRCList>>();

            var requestDomainName = AppSetting.Ts360SiteUrl;
            var spFeaturedDic = ContentManagementController.Current.GetNRCFeaturedTitlesItems(Month, Year, productTypesList, requestDomainName);

            if (spFeaturedDic.Count > 0)
            {
                var listItemsNRC = new List<NRCList>();
                foreach (var groupItem in spFeaturedDic)
                {
                    var listFeaturedTitle = new List<NRCList>();
                    foreach (var item in groupItem.Value)
                    {
                        var featuredTitle = new NRCList
                        {
                            ProductType = groupItem.Key, //item.ProductType,
                            StreetDate = item.StreetDate,
                            PreOrderDate = item.PreOrderDate,
                            BTKeys = item.BTKeys
                        };

                        listFeaturedTitle.Add(featuredTitle);
                    }

                    // add to result dictionary
                    resultDic.Add(groupItem.Key, listFeaturedTitle);
                }
            }

            return resultDic;
        }
    }
}
