using BT.TS360API.ServiceContracts.Search;

namespace BT.TS360API.ServiceContracts
{
    public class ESPRankDetailInfo
    {
        public ProductSearchResultItem SearchResultInfo { get; set; }
        public RankDetail[] RankDetails { get; set; }


        private bool _hasESPRanking = false;
        public class RankDetail
        {
            private decimal _rankDetailValue;

            public string Description { get; set; }
            public decimal Value
            {
                get { return _rankDetailValue * 10; }
                set { _rankDetailValue = value; }
            }
        }

        public decimal? OverallRank { get; set; }
        public decimal? BisacRank { get; set; }
        public string DetailUrl { get; set; }
        public string ESPCategoryType { get; set; }
        public int ESPDetailWidth { get; set; }
        public int ESPDetailHeight { get; set; }

        //HasESPRanking logic in implemented in database
        public bool HasESPRanking
        {
            get { return _hasESPRanking; }
            set { _hasESPRanking = value; }
        }
    }
}
