using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections.ObjectModel;

using Microsoft.CommerceServer;
using Microsoft.CommerceServer.Runtime;
using Microsoft.CommerceServer.Inventory;
using Microsoft.CommerceServer.Catalog;
using System.Data;
using System.Net;

// reference "common" project
using BTNextGen.Services.Common;


namespace BTNextGen.Services.ICatalogs
{

    /// <summary>
    /// UNDER CONSTRUCTION
    /// </summary>
    public class Catalogs : ICatalogService
    {
        public string CreateNewProducts(BTProductCollection newProducts)
        {
            foreach (var product in newProducts)
            {
                AddNewProduct(product);
            }
            return "Success";
        }

        public string UpdateProducts(BTProductCollection updateProducts)
        {
            foreach (BTProduct product in updateProducts)
            {
                //
            }
            return "Success";
        }

        private void AddNewProduct(BTProduct newProduct)
        {
            try
            {

                CatalogContext context = CSContext.CreateCatalogContextFromServiceAgent();

                // Get the catalog
                BaseCatalog baseCatalog = (BaseCatalog)context.GetCatalog(newProduct.CatalogName);

                // Get product definition from web.config
                CatalogDefinition productDefinition = context.GetDefinition(Config.ProductDefinition);

                // CategoryName is set to empty string
                Product btProduct = baseCatalog.CreateProduct(productDefinition.Name, newProduct.BTKey, newProduct.BTListPrice, "", newProduct.BTTitle);

                // Set the properties of the product and save the record
                btProduct["BTKey"] = newProduct.BTKey;
                btProduct["BTListPrice"] = newProduct.BTListPrice;
                btProduct["BTISBN"] = newProduct.BTISBN;
                btProduct["BTGTIN"] = newProduct.BTGTIN;
                btProduct["BTBlowOutInactiveFlag"] = newProduct.BTBlowOutInactiveFlag;
                btProduct["BTItemGroup"] = newProduct.BTItemGroup;
                btProduct["BTProductType"] = newProduct.BTProductType;
                btProduct["BTListPrice"] = newProduct.BTListPrice;
                btProduct.Save();

                if (newProduct.RequiresExpressSetup)
                {
                    // TODO: Clear Site Cache gracefully
                };

            }
            catch (EntityAlreadyExistsException e)
            {
                // handle errors here
                Console.WriteLine(string.Format("Exception: {0}", e.Message));
            }

        }

        public CategoryRespCollection GetCategoryDetails(string catalogName)
        {
            // Get all the categories for this catalog passed by the client application.
                       
            CategoryRespCollection respColl = new CategoryRespCollection();
            CatalogServiceAgent csAgent = new CatalogServiceAgent(Config.CatalogServiceUrl);
            CatalogContext context = CatalogContext.Create(csAgent);
          
            ProductCatalog catalog = context.GetCatalog(catalogName);// get the catalog details
            CatalogSearchOptions searchOptions = new CatalogSearchOptions(); // prepare the search clause
            searchOptions.PropertiesToReturn = "CategoryName"; 
            CatalogSearch catalogSearch = context.GetCatalogSearch();
             catalogSearch.SqlWhereClause = "CategoryName is not null";// only pull the category details using the filter
            catalogSearch.CatalogNames = catalogName;// This is a comma separated list of catalogs to search eg "Catalog1,catalog2"
            catalogSearch.Recursive = true;  // This property searches the catalog recursively. If not set, the child categories wont be pulled in the results. 
            int totalRecords = 0;
            CatalogItemsDataSet catalogItems = catalogSearch.Search(out totalRecords);
            foreach (CatalogItemsDataSet.CatalogItem catalogItem in catalogItems.CatalogItems) // loop individual category
            {
                string catName = catalogItem.CategoryName.ToString();
                Category singleCategory = catalog.GetCategory(catName);
                ProductCollection prodCollection = singleCategory.ChildProducts; // get all products within a category
                foreach (Product Prod in prodCollection) // loop individual products
                {
                    int rank = Prod.Information.CatalogItems[0].Rank; // pull the rank details from the product
                    CategoryDetailsResponse singResp = new CategoryDetailsResponse(); // Populate the response object wie th product and rank detail
                    singResp.BTKey = Prod.ProductId;
                    singResp.CatalogName = catalogName;
                    singResp.Category = catName;
                    singResp.Rank = rank;
                    respColl.Add(singResp);// add the details in the collection object.
                    
                }

            }

            return respColl;
        }


    }
}
