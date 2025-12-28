using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using ListItem = Microsoft.SharePoint.Client.ListItem;

namespace BT.TS360SP
{
    /// <summary>
    /// Managed Content for BTNG Content Management
    /// </summary>
    public partial class NRCFeaturedTitlesItem : MultipleProductsBaseItem
    {
        public DateTime StreetDate { get; set; }
        public DateTime PreOrderDate { get; set; }
        public string ProductType { get; set; }
        public override void SPListItemMapping(ListItem item)
        {
            StreetDate = ((DateTime)item[CMFieldNameConstants.ReleaseDate]).ToLocalTime();
            PreOrderDate = ((DateTime)item[CMFieldNameConstants.PreOrderDate]).ToLocalTime();
            BTKeyList = item[CMFieldNameConstants.BTKeyList] as string;
            ProductType = item[CMFieldNameConstants.ProductType] as string;
        }
    }
}
