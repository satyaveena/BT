using System;
using System.Collections.Generic;

namespace BT.TS360API.ServiceContracts
{
    public class CheckRealTimeInventoryForQuickCartDetailsInfoReponse
    {
        //public List<PricingReturn4ClientObject> Pricing { get; set; }
        //public List<SiteTermObject> InventoryStatus { get; set; }
        //public List<PromotionReturn4ClientObject> Promotion { get; set; }
        //public List<SiteTermObject> Duplicate { get; set; }
        //public List<SiteTermObject> ContentIndicator { get; set; }
        //public List<PrimaryCartTitleDetail> PrimaryCartTitleDetails { get; set; }
        //public string ContentIndicatorHtml { get; set; }
        //public string Message { get; set; }
        //public List<NoteClientObject> NotesList { get; set; }

        public List<InventoryResults> InventoryResultsList { get; set; }
        public string StockCheckInventoryStatus { get; set; }

        public CheckRealTimeInventoryForQuickCartDetailsInfoReponse()
        {
            InventoryResultsList = new List<InventoryResults>();
        }
    }

}
