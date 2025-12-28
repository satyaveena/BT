using System;
using System.Collections.Generic;
using BT.TS360Constants;

namespace BT.TS360API.ServiceContracts
{
    public class InventoryStatusArgRequest : BaseRequest
    {
        public InventoryStatusArgContract arg { get; set; }
        public MarketType? MarketType { get; set; }
        public string UserId { get; set; }
        public string CountryCode { get; set; }
        public string OrgId { get; set; }
    }

    public class InventoryStatusArgContract 
    {
        public string CatalogName;
        
        public string Flag;
        
        public string BTKey;
      
        public string CartID;
      
        public decimal Quantity;
        
        public string ProductType;
        
        public string VariantId;
        
        public DateTime PublishDate;
        
        public string MerchandiseCategory;
        
        public string MarketType;
        
        public string PubCodeD;
        
        public string ESupplier;
      
        public string ReportCode;
        
        public string SupplierCode;
      
        public string BlockedExportCountryCodes { get; set; }

        public string ISBN;
        public string Title { get; set; }
        public string Author { get; set; }
        public string FormatLiteral { get; set; }
        public string PublishDateString { get; set; }
    }

    public class InventoryStatusClientArg
    {
        public string BTKey { get; set; }
        public string VarId { get; set; }
        public string Pub { get; set; }
        public string BTType { get; set; }
        public string Quantity { get; set; }
        public string Catalog { get; set; }
        public string PublishDate { get; set; }
        public string MerchandiseCategory { get; set; }
        public string MarketType { get; set; }
        public string PubCodeD { get; set; }
        public string ESupplier { get; set; }
        public string ReportCode { get; set; }
    }

    public class InventoryStatusClientRequest : BaseRequest
    {
        public List<InventoryStatusClientArg> inventoryStatusArgList { get; set; }

        public MarketType? MarketType { get; set; }
        public string UserId { get; set; }
        public string CountryCode { get; set; }
        public string OrgId { get; set; }
    }
}
