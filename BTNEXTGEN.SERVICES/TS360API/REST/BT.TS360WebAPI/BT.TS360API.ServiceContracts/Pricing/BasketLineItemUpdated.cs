using System.Text;

namespace BT.TS360API.ServiceContracts.Pricing
{
    public class BasketLineItemUpdated
    {
        public string SoldToId { get; set; }

        public string BasketSummaryId { get; set; }

        public string BasketLineItemId { get; set; }

        public string ISBN { get; set; }

        public string BTKey { get; set; }

        public string AccountId { get; set; }

        public string ProductType { get; set; }

        public decimal? ListPrice { get; set; }

        public decimal? ExtendedPrice { get; set; }

        public int TotalLineQuantity { get; set; }

        public int? TotalOrderQuantity { get; set; }

        public decimal? PromotionPrice { get; set; }

        public decimal? ContractPrice { get; set; }

        public decimal ContractDiscountPercent { get; set; }

        public bool ProductPriceChangedIndicator { get; set; }

        public bool ContractChangedIndicator { get; set; }

        public bool PromotionChangedIndicator { get; set; }

        public bool PromotionActiveIndicator { get; set; }

        public string PriceKey { get; set; }

        public string AccountPricePlan { get; set; }

        public decimal AcceptableDiscount { get; set; }

        public string ProductLine { get; set; }

        public bool ReturnFlag { get; set; }

        public string AccountERPNumber { get; set; }

        public string PrimaryWarehouse { get; set; }

        public string ProductCatalog { get; set; }

        public string MarketType { get; set; }

        public string AudienceType { get; set; }

        public bool IsHomeDelivery { get; set; }

        public decimal? SalePrice { get; set; }

        public bool QuantityChanged { get; set; }

        public string EMarket { get; set; }

        public string ETier { get; set; }

        public string ESupplier { get; set; }

        public string PromotionCode { get; set; }

        public decimal PromotionDiscountPercent { get; set; }

        public decimal DiscountPercent { get; set; }

        public string Upc { get; set; }

        public string Pig { get; set; }

        public string SiteBranding { get; set; }

        public string OrgId { get; set; }

        public string OrgName { get; set; }

        public int NumberOfBuildings { get; set; }

        public string PurchaseOption { get; set; }

        public decimal ProcessingCharges { get; set; }

        public float SalesTax { get; set; }

        public bool IsVIPAccount { get; set; }

        public bool IsOneBoxAccount { get; set; }

        public override string ToString()
        {
            var itemString = new StringBuilder();
            itemString.AppendFormat("BasketLineItemUpdated:ID = {0}", this.BasketLineItemId);
            itemString.AppendFormat("SoldToId:{0} - ", this.SoldToId);
            itemString.AppendFormat("ISBN:{0} - ", this.ISBN);
            itemString.AppendFormat("BTKey:{0} - ", this.BTKey);
            itemString.AppendFormat("UPC:{0} - ", this.Upc);
            itemString.AppendFormat("BasketSummaryId:{0} - ", this.BasketSummaryId);
            itemString.AppendFormat("ProductType:{0} - ", this.ProductType);
            itemString.AppendFormat("ListPrice:{0} - ", this.ListPrice);
            itemString.AppendFormat("ContractPrice:{0} - ", this.ContractPrice);
            itemString.AppendFormat("ContractDiscountPercent:{0} - ", this.ContractDiscountPercent);
            itemString.AppendFormat("PromotionPrice:{0} - ", this.PromotionPrice);
            itemString.AppendFormat("PromotionDiscountPercent:{0} - ", this.PromotionDiscountPercent);
            itemString.AppendFormat("DiscountPercent:{0} - ", this.DiscountPercent);
            itemString.AppendFormat("ProductPriceChangedIndicator:{0} - ", this.ProductPriceChangedIndicator);
            itemString.AppendFormat("ContractChangedIndicator:{0} - ", this.ContractChangedIndicator);
            itemString.AppendFormat("PromotionChangedIndicator:{0} - ", this.PromotionChangedIndicator);
            itemString.AppendFormat("PromotionActiveIndicator:{0} - ", this.PromotionActiveIndicator);
            itemString.AppendFormat("AccountPricePlan:{0} - ", this.AccountPricePlan);
            itemString.AppendFormat("TotalLineQuantity:{0} - ", this.TotalLineQuantity);
            itemString.AppendFormat("TotalOrderQuantity:{0} - ", this.TotalOrderQuantity);
            itemString.AppendFormat("PriceKey:{0} - ", this.PriceKey);
            itemString.AppendFormat("AccountERPNumber:{0} - ", this.AccountERPNumber);
            itemString.AppendFormat("PrimaryWarehouse:{0} - ", this.PrimaryWarehouse);
            itemString.AppendFormat("IsHomeDelivery:{0} - ", this.IsHomeDelivery);
            itemString.AppendFormat("AccountId:{0} - ", this.AccountId);
            itemString.AppendFormat("MarketType:{0} - ", this.MarketType);
            itemString.AppendFormat("AudienceType:{0} - ", this.AudienceType);
            itemString.AppendFormat("ProductCatalog:{0} - ", this.ProductCatalog);
            itemString.AppendFormat("SalePrice:{0} - ", this.SalePrice);
            itemString.AppendFormat("ExtendedPrice:{0} - ", this.ExtendedPrice);
            itemString.AppendFormat("PromotionCode:{0} - ", this.PromotionCode);
            itemString.AppendFormat("ESupplier:{0} - ", this.ESupplier);
            itemString.AppendFormat("EMarket:{0} - ", this.EMarket);
            itemString.AppendFormat("ETier:{0} - ", this.ETier);
            itemString.AppendFormat("PIG:{0} - ", this.Pig);
            itemString.AppendFormat("SiteBranding:{0} - ", this.SiteBranding);
            itemString.AppendFormat("OrgId:{0} - ", this.OrgId);
            itemString.AppendFormat("OrgName:{0} - ", this.OrgName);
            itemString.AppendFormat("NumberOfBuildings:{0} - ", this.NumberOfBuildings);
            itemString.AppendFormat("PurchaseOption:{0} - ", this.PurchaseOption);
            itemString.AppendFormat("ProcessingCharges:{0} - ", this.ProcessingCharges);
            itemString.AppendFormat("SalesTax:{0} - ", this.SalesTax);
            itemString.AppendFormat("IsVIPAccount:{0} - ", this.IsVIPAccount);
            return itemString.ToString();
        }
    }
}
