using System.Collections.Generic;
using System.Data;
using System.Linq;
using BT.TS360API.Common.DataAccess;
using BT.TS360API.ServiceContracts.Pricing;

namespace BT.TS360API.Common.Business
{
    public class PricingDAOManager
    {
        public static List<BasketLineItemUpdated> ConvertToBasketLineItemUpdated(DataSet ds)
        {
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            var rs = new List<BasketLineItemUpdated>();
            rs.AddRange(from DataRow row in ds.Tables[0].Rows
                        select new BasketLineItemUpdated
                        {
                            SoldToId = BaseDAO.ConvertTo<string>(row, "SoldToId"),
                            BasketSummaryId = BaseDAO.ConvertTo<string>(row, "BasketSummaryID"),
                            BasketLineItemId = BaseDAO.ConvertTo<string>(row, "BasketLineItemId"), // not in design
                            BTKey = BaseDAO.ConvertTo<string>(row, "BTKey"),
                            Upc = BaseDAO.ConvertTo<string>(row, "UPC"),
                            ISBN = BaseDAO.ConvertTo<string>(row, "ISBN"),
                            AccountId = BaseDAO.ConvertTo<string>(row, "AccountId"),
                            ProductType = BaseDAO.ConvertTo<string>(row, "ProductType"),
                            ListPrice = BaseDAO.ConvertTo<decimal>(row, "ListPrice"),
                            TotalLineQuantity = BaseDAO.ConvertTo<int>(row, "TotalLineQuantity"),
                            TotalOrderQuantity = BaseDAO.ConvertTo<int>(row, "TotalOrderQuantity"),
                            PromotionPrice = BaseDAO.ConvertTo<decimal>(row, "PromotionPrice"),
                            ContractPrice = BaseDAO.ConvertTo<decimal>(row, "ContractPrice"),
                            DiscountPercent = BaseDAO.ConvertTo<decimal>(row, "ContractDiscountPercent"),
                            SalePrice = BaseDAO.ConvertTo<decimal>(row, "SalePrice"),
                            ProductPriceChangedIndicator = BaseDAO.ConvertTo<bool>(row, "ProductPriceChangedIndicator"),
                            ContractChangedIndicator = BaseDAO.ConvertTo<bool>(row, "ContractChangedIndicator"),
                            PromotionChangedIndicator = BaseDAO.ConvertTo<bool>(row, "PromotionChangedIndicator"),
                            PromotionActiveIndicator = BaseDAO.ConvertTo<bool>(row, "PromotionActiveIndicator"),
                            PriceKey = BaseDAO.ConvertTo<string>(row, "PriceKey"),
                            AccountPricePlan = BaseDAO.ConvertTo<string>(row, "AccountPricePlan"),
                            AcceptableDiscount = BaseDAO.ConvertTo<decimal>(row, "AcceptableDiscount"),
                            ProductLine = BaseDAO.ConvertTo<string>(row, "ProductLine"),
                            ReturnFlag = BaseDAO.ConvertTo<bool>(row, "ReturnFlag"),
                            AccountERPNumber = BaseDAO.ConvertTo<string>(row, "AccountERPNumber"),
                            PrimaryWarehouse = BaseDAO.ConvertTo<string>(row, "PrimaryWarehouse"),
                            ProductCatalog = BaseDAO.ConvertTo<string>(row, "ProductCatalog"),
                            MarketType = BaseDAO.ConvertTo<string>(row, "Market_Type"),
                            AudienceType = BaseDAO.ConvertTo<string>(row, "Audience_Type"),
                            ESupplier = BaseDAO.ConvertTo<string>(row, "eSupplier"),
                            EMarket = BaseDAO.ConvertTo<string>(row, "eMarket"),
                            ETier = BaseDAO.ConvertTo<string>(row, "eTier"),
                            IsHomeDelivery = BaseDAO.ConvertTo<bool>(row, "IsHomeDelivery"),
                            PromotionCode = BaseDAO.ConvertTo<string>(row, "PromotionCode"),
                            Pig = BaseDAO.ConvertTo<string>(row, "PIG"),
                            SiteBranding = BaseDAO.ConvertTo<string>(row, "SiteBranding"),
                            OrgId = BaseDAO.ConvertTo<string>(row, "OrgId"),
                            OrgName = BaseDAO.ConvertTo<string>(row, "OrgName"),
                            NumberOfBuildings = BaseDAO.ConvertTo<int>(row, "NumberOfBuildings"),
                            PurchaseOption = BaseDAO.ConvertTo<string>(row, "PurchaseOption"),
                            ProcessingCharges = BaseDAO.ConvertTo<decimal>(row, "ProcessingCharges"),
                            SalesTax = (float)BaseDAO.ConvertTo<decimal>(row, "SalesTax"),//To do: salesTax
                            IsVIPAccount = BaseDAO.ConvertTo<bool>(row, "IsVIPAccount"),
                            IsOneBoxAccount = BaseDAO.ConvertTo<string>(row, "AccountType") == "OneBox"
                        });

            return rs;
        }
    }
}
