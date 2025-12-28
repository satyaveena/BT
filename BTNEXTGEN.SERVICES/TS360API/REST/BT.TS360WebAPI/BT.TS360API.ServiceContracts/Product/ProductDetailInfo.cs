using BT.TS360API.ServiceContracts.Search;
using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts.Product
{
    public class ProductDetailInfo
    {
        /// <summary>
        /// Gets or sets SearchResultInfo
        /// </summary>
        public ProductSearchResultItem SearchResultInfo { get; set; }

        /// <summary>
        /// Gets or sets MarketType
        /// </summary>
        public MarketType MarketType { get; set; }

        public bool IsOriginalEntry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public ProductDetailInfo()
        //{
        //    this.MarketType = SiteContext.Current.MarketType ?? MarketType.Any;
        //}
        public ProductDetailInfo(MarketType marketType = MarketType.Any)
        {
            this.MarketType = marketType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchResultInfo"></param>
        public ProductDetailInfo(ProductSearchResultItem searchResultInfo,MarketType marketType)
            : this(marketType)
        {
            this.SearchResultInfo = searchResultInfo;
        }
    }
}
