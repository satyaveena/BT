using System.Collections.Generic;
using BT.TS360API.ServiceContracts.Product;
using BT.TS360API.ServiceContracts.Search;

namespace BT.TS360API.ServiceContracts
{
    public class ItemDetailArg : BaseRequest
    {

        public string Author { get; set; }

     
        public string BTKey { get; set; }

       
        public string CartId { get; set; }

     
        public InventoryStatusArgContract InventoryArg { get; set; }

      
        public string Catalog { get; set; }

       
        public string LineItemId { get; set; }

    
        public string ESupplier { get; set; }

        public bool AllowBTEmployee { get; set; }
        
        public RecordIndexArgs RecordIndexArg { get; set; }
    }
    
    public class ItemDetailReturn
    {
        
        public RelatedProductListContract RelatedProductData { get; set; }

        
        public string InventoryData { get; set; }

        
        public ProductDetail DetailInfo { get; set; }

        
        public NoteClientObject Note { get; set; }

        
        public string TitleDetailSelectingTab { get; set; }

        
        //public List<SiteTermObject> DuplicateIndicator { get; set; }

        
        public string InventoryStatus { get; set; }

        
        public PromotionContract PromotionInfo { get; set; }

        
        public PricingContract PricingInfo { get; set; }

        
        public ESPRankContract ESPRankInfo { get; set; }

        
        public string PrimaryCartGridLink { get; set; }

        
        public List<AdditionalVersionContract> AdditionalVersion { get; set; }

        
        public string RecordIndexHtmlContent { get; set; }

        
        public UpdateItemResult UpdateItemResult { get; set; }
    }
    public class PromotionContract
    {
        
        public bool HasPromotion { get; set; }

        
        public string PromoText { get; set; }

       
        public string PromoLink { get; set; }
    }
    public class PricingContract
    {
        
        public string NetPrice { get; set; }

        
        public string ListPrice { get; set; }

        
        public string DiscountPercentage { get; set; }

        
        public bool IsRetail { get; set; }

        
        public string QuotedPriceIndicator { get; set; }
    }
    public class ESPRankContract
    {
       
        public decimal? OverallRank { get; set; }

        //[DataMember]
        //public string ESPRankingText { get; set; }

        public decimal? BisacRank { get; set; }

       
        public string ESPDetailUrl { get; set; }

        public int ESPDetailWidth { get; set; }
        public int ESPDetailHeight { get; set; }
       
        public bool HasESPRanking { get; set; }

        public string ESPCategoryName { get; set; }
    }
    public class AdditionalVersionContract
    {
        
        public string ESupplier { get; set; }

        
        public string PhysicalFormat { get; set; }

   
        public string FormDetail { get; set; }

       
        public string ListPrice { get; set; }

        public AdditionalVersionContract(string eSupplier, string physicalFormat, string formDetail, string listPrice)
        {
            ESupplier = eSupplier;
            PhysicalFormat = physicalFormat;
            FormDetail = formDetail;
            ListPrice = listPrice;
        }
    }

    public class ItemDetailPrimaryInfoReturn
    {
        public int GridLineCount { get; set; }
        public string InventoryData { get; set; }
        public PricingContract PricingInfo { get; set; }
        public string TitleDetailSelectingTab { get; set; }        
        public string InventoryStatus { get; set; }
        public int Last30DaysDemandInfo { get; set; }
        public bool HasDemand { get; set; }
        public ProductDetail DetailInfo { get; set; }
    }

    public class PrimaryInfoItemDetailArg: BaseRequest
    {
        public string BTKey { get; set; }
        public string CartId { get; set; }
        public string LineItemId { get; set; }
        public bool IsVIP { get; set; }
        public InventoryStatusArgContract InventoryArg { get; set; }
    }

    public class ItemDetailSecondaryInfoReturn
    {
        public RelatedProductListContract RelatedProductData { get; set; }
        public ProductDetail DetailInfo { get; set; }
        public NoteClientObject Note { get; set; }
        public PromotionContract PromotionInfo { get; set; }
        public ESPRankContract ESPRankInfo { get; set; }
        public string PrimaryCartGridLink { get; set; }
        public List<AdditionalVersionContract> AdditionalVersion { get; set; }
        public string RecordIndexHtmlContent { get; set; }
        public RecordIndexCartArgs RecordIndexCart { get; set; }
        public UpdateItemResult UpdateItemResult { get; set; }
        //public List<SiteTermObject> DuplicateIndicator { get; set; }
    }

    public class GetPrimaryCartGridDataRequest
    {
        public List<string> BTKeyList { get; set; }
        public string PrimaryCartId { get; set; }
    }

    public class RecordIndexCartArgs
    {
        public bool IsPrimary { get; set; }
        public bool IsPricing { get; set; }
        public int LineItemCount { get; set; }
        public int TotalOrderQuantity { get; set; }
        public string Price { get; set; }
    }

    public class SecondaryInfoItemDetailArg
    {
        public string BTKey { get; set; }
        public string CartId { get; set; }
        public string LineItemId { get; set; }
        public string Author { get; set; }
        public bool IsPrimaryCartSet { get; set; }
        public string Catalog { get; set; }
        public string ESupplier { get; set; }
        public RecordIndexArgs RecordIndexArg { get; set; }
        public string Tab { get; set; }
        public string HdfTabLoaded { get; set; }
        public bool AllowBTEmployee { get; set; }
    }

    public class AdditionalVersion
    {
        public string ESupplier { get; set; }
        public string PhysicalFormat { get; set; }
        public string FormDetail { get; set; }
        public string ListPrice { get; set; }

        public AdditionalVersion(string eSupplier, string physicalFormat, string formDetail, string listPrice)
        {
            ESupplier = eSupplier;
            PhysicalFormat = physicalFormat;
            FormDetail = formDetail;
            ListPrice = listPrice;
        }
    }
}
