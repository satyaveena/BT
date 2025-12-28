using System;

namespace BT.TS360API.ExternalServices
{
    public class MarketingService
    {
        private static volatile MarketingService _instance;
        private static readonly object SyncRoot = new Object();

        private MarketingService()
        {
        }

        public static MarketingService Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new MarketingService();
                }

                return _instance;
            }
        }

        public void GetAllDiscounts(string productId)
        {
            //var ma = new MarketingServiceAgent("http://localhost:100/TS360_MarketingWebService/MarketingWebService.asmx");
            //var marketingSystem = MarketingContext.Create(ma);

            //var discountObj = marketingSystem.CampaignItems.GetCampaignItem(52);
        }
    }
}
