using BT.TS360API.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ETS.Business.Models
{
    public class ProductPricingResult
    {
        public List<ProductPricing> Products { get; set; }
        public List<ErrorItem> ErrorItems { get; set; }
        public ProductPricingResult()
        {
            Products = new List<ProductPricing>();
            ErrorItems = new List<ErrorItem>();
        }

    }

    public class ProductPricing
    {
        public string BTKey { get; set; }
        public decimal NetPrice { get; set; }
        public decimal BookDigitalProcessingCharge { get; set; }
        public decimal AdditionalPaperbackCharge { get; set; }
        public decimal SpokenWordCharge { get; set; }
        public decimal SalesTaxPercentage { get; set; }
    }

    public class PricingRequests
    {
        public SearchRequest SearchRequest { get; set; }
        public Dictionary<string, ProductPricing> ProductVasList { get; set; }
        public List<ErrorItem> ErrorItems { get; set; }

        public PricingRequests()
        {
            ProductVasList = new Dictionary<string, ProductPricing>();
            ErrorItems = new List<ErrorItem>();
        }
    }

}
