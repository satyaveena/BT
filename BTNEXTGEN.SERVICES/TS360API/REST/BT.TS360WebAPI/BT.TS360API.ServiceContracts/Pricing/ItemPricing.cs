using System.Text;

namespace BT.TS360API.ServiceContracts.Pricing
{
    public class ItemPricing
    {
        /// <summary>
        /// Basket Id
        /// </summary>
        public string BasketSummaryId { get; set; }

        /// <summary>
        /// Product BTKey
        /// </summary>
        public string BTKey { get; set; }

        /// <summary>
        /// Contract Price
        /// </summary>
        public decimal? ContractPrice { get; set; }

        /// <summary>
        /// List Price
        /// </summary>
        public decimal? ListPrice { get; set; }

        /// <summary>
        /// Promotion Price
        /// </summary>
        public decimal? PromotionPrice { get; set; }

        /// <summary>
        /// Promotion Price
        /// </summary>
        public decimal? SalePrice { get; set; }

        /// <summary>
        /// Promotion Price
        /// </summary>
        public decimal? ExtendedPrice { get; set; }

        /// <summary>
        /// Discount Percentage
        /// </summary>
        public decimal? DiscountPercent { get; set; }

        /// <summary>
        /// Contract Discount Percentage
        /// </summary>
        public decimal? ContractDiscountPercent { get; set; }

        /// <summary>
        /// Promotion Discount Percentage
        /// </summary>
        public decimal? PromotionDiscountPercent { get; set; }

        public decimal? SurchargeAmount { get; set; }

        public string ESupplier { get; set; }

        public string PromotionCode { get; set; }
        public string BasketDisplay { get; set; }

        public override string ToString()
        {
            var itemString = new StringBuilder();
            itemString.Append("ItemPricing:");
            itemString.AppendFormat("BTKey:{0} - ", this.BTKey);
            itemString.AppendFormat("BasketSummaryId:{0} - ", this.BasketSummaryId);
            itemString.AppendFormat("DiscountPercent:{0} - ", this.DiscountPercent);
            itemString.AppendFormat("ListPrice:{0} - ", this.ListPrice);
            itemString.AppendFormat("ContractPrice:{0} - ", this.ContractPrice);
            itemString.AppendFormat("PromotionPrice:{0} - ", this.PromotionPrice);
            itemString.AppendFormat("SalePrice:{0} - ", this.SalePrice);
            itemString.AppendFormat("ExtendedPrice:{0} - ", this.ExtendedPrice);
            itemString.AppendFormat("DiscountPercent:{0} - ", this.DiscountPercent);
            itemString.AppendFormat("SurchargeAmount:{0} - ", this.SurchargeAmount);

            return itemString.ToString();
        }
    }
}
