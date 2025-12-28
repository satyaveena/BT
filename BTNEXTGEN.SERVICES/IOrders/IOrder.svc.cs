using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.CommerceServer;
using Microsoft.CommerceServer.Marketing;
using Microsoft.CommerceServer.Profiles;
using Microsoft.CommerceServer.Orders;
using System.Data;
using Microsoft.CommerceServer.Internal.Marketing.XmlData;
using System.Globalization;
using System.Net;
using Microsoft.CommerceServer.Runtime.Orders;
using System.Xml;
using System.IO;
using System.Configuration;
// reference "common" project
using BTNextGen.Services.Common;
using BTNextGen.Commerce.Portal.Common;

namespace BTNextGen.Services.IOrders
{

    public class IOrder : IOrdersService
    {

        # region Public Methods

        public BasketDetailsResponseCollection CreateLegacyBaskets(BasketDetailsCollection basketDetails)
        {
            BasketDetailsResponseCollection response = new BasketDetailsResponseCollection();

            foreach (var basket in basketDetails)
            {
                response.Add(CreateLegacyBasket(basket));
            }

            return response;
        }

        public BasketDetailsResponse CreateLegacyBasket(BasketDetails basketDetails)
        {

            // build response message
            BasketDetailsResponse basketResponse = new BasketDetailsResponse();
            basketResponse.LegacyBasketId = basketDetails.LegacyBasketId;
            basketResponse.LegacySourceSystem = basketDetails.LegacySourceSystem;

            try
            {

                // Create basket for CS user
                Guid soldToId = new Guid(basketDetails.CSUserId);
               // Guid basketSummaryId = new Guid(basketDetails.BasketSummaryId);

                string myBasketName = string.Format("{0}{1}", basketDetails.LegacySourceSystem, basketDetails.LegacyBasketName);

                // CREATE NEW BASKET
                // =================

                // NOTE: the Stored Procedure must produce a unique name e.g with TS3 prefix
                OrderContext.Create(Config.CSSiteName);
                Microsoft.CommerceServer.Runtime.Orders.Basket newBasket = OrderContext.Current.GetBasket(soldToId, basketDetails.LegacyBasketName);
                newBasket.Name = basketDetails.LegacyBasketName;
                newBasket.Status = "InProcess";
               
                // CREATE NEW ORDERFORM
                // ====================
                OrderFormEx orderForm = new OrderFormEx();
                orderForm.Name = "Default"; 
                //orderForm.Name = "Default";
                orderForm.SubTotal = basketDetails.BasketTotal;
                orderForm.Total = orderForm.SubTotal;
                orderForm.ModifiedBy = soldToId.ToString();
                orderForm.Status = basketDetails.BasketStatus;
                
                //orderForm.FolderId =                                                              // ADVN: Provide legacy cart folder so it can be hardcoded in the Web.Config here
                orderForm.BTStatus = basketDetails.BasketStatus;                                    // ADVN: NOTE: redundant - same as "Status" discuss
                orderForm.IsArchived = 0;                                                           // ADVN: change data type, should be bit
                orderForm.IsPrimary = 0;                                                            // ADVN: change data type, should be bit
                orderForm.PONumber = basketDetails.BasketPONumber;
                orderForm.BookAccountId = basketDetails.CSBookAccountId;                            // BTDEV: Stored Proc should determine value depending on applicable ERP Account
                orderForm.EntertainmentAccountId = basketDetails.CSEntertainmentAccountId;          // BTDEV: Stored Proc should determine value depending on applicable ERP Account
                orderForm.SpecialInstructions = basketDetails.BasketSpecialInstructions;
                orderForm.IsHomeDeliveryIndicator = basketDetails.IsHomeDeliveryIndicator;
                orderForm.HomeDeliveryAccountId = basketDetails.HomeDeliveryAccountId; 
                orderForm.HomeDeliveryAddressType = basketDetails.HomeDeliveryAddressType;
                                                                                        // ADVN: Confirm the purpose of "IsBackOrder" and "ShippingMethodExId"
                //orderForm.BTGiftWrapCode = basketDetails.BasketGiftWrapCode;
                //orderForm.BTGiftWrapMessage = basketDetails.BasketGiftWrapMessage;
                //orderForm.StoreShippingFee = basketDetails.BasketStoreShippingFee;                 // ADVN: change data type - it should be bit?
                //orderForm.StoreGiftWrapFee = basketDetails.BasketStoreGiftWrapFee;                 // ADVN: change data type - it should be bit?
                //orderForm.StoreProccessingFee = basketDetails.BasketStoreProccessingFee;           // ADVN: change data type - it should be bit?
                //*orderForm.StoreOrderFee = basketDetails.BasketStoreOrderFee;                       // ADVN: change data type, shouldn't be bit?
                //orderForm.IsBTCart = basketDetails.BasketIsBTCart;                                 // ADVN: change data type - it should be bit
                orderForm.BTNote = basketDetails.BasketNote;                                        // ADVN: change data size (confirm constraint for orders with Marvin)
                //*orderForm.AccountType = "";                                          // ADVN: confirm what goes in here (domain of values)
                orderForm.InventoryReserveNumber = basketDetails.AccountInventoryReserveNumber;    // BTDEV: stored proc. should join to ERP tables for the given SAN/SAN-Suffix number and provide this value (TOLAS Only)
                orderForm.AccountInventoryType = basketDetails.AccountInventoryType;               // BTDEV: stored proc. should join to ERP tables for the given SAN/SAN-Suffix number and provide this value (SOP Only)
                //*orderForm.CheckReserveFlag 
                //    = ((orderForm.IsTolas && string.IsNullOrEmpty(orderForm.InventoryReserveNumber)) || (!orderForm.IsTolas && string.IsNullOrEmpty(orderForm.accountInventoryType))) ? false: true;
                orderForm.WarehouseList = basketDetails.AccountWarehouseList;                      // BTDEV: stored proc. should join to ERP accounts for this one
                //orderForm.IsTolas = basketDetails.BasketIsTolas;
                //orderForm.LegacySourceSystem = basketDetails.LegacySourceSystem;                  // ADVN: rename from "LegacySource" to "LegacySourceSystem" for clarity
                orderForm.LegacyBasketId = basketDetails.LegacyBasketId;
                //orderForm.BTTargetERP = (orderForm.IsTolas || orderForm.IsHomeDeliveryIndicator) ? "BTE" : "BTB";

                //*orderForm.TransmissionNumber = new Guid().ToString();                // ADVN: provide logic for deriving this value
                //orderForm.LegacyCreated = basketDetails.BasketCreatedDate;         // ADVN: add new attribute; the orderform/basket "created" property is "read-only" and cannot be overriden
                //orderForm.LegacyModified = basketDetails.BasketModifiedDate;       // ADVN: add new attribute; the orderform/basket "lastmodified" property is "read-only" and cannot be overriden


                // ADD LINEITEMS TO ORDERFORM
                // ==============================

                if (basketDetails.LineItemList != null)
                {
                    
                    foreach (LineItemDetails newLineDetail in basketDetails.LineItemList)
                    {
                        //LineItem newItem = new LineItem("supplies", "BCD-890", "", 250);
                        LineItemEx newLineItem = new LineItemEx();
                        newLineItem.ProductCatalog = newLineDetail.ProductCatalog;
                        newLineItem.ProductId = newLineDetail.BTKey;
                        newLineItem.Quantity = newLineDetail.Quantity;
                        newLineItem.ListPrice = newLineDetail.ListPrice;
                        newLineItem.PlacedPrice = newLineDetail.PlacedPrice;
                        newLineItem.DisplayName = newLineDetail.BTTitle;
                        newLineItem.Description = newLineDetail.BTTitle;
                        newLineItem.Status = "InProcess";
                        // add others based on NG Schema
                        newLineItem.BTKey = newLineDetail.BTKey;
                        //newLineItem.ProductType = newLineDetail.ProductType;
                        //newLineItem.BTGiftWrapCode = newLineDetail.BTGiftWrapCode;
                        //newLineItem.BTGiftWrapMessage = newLineDetail.BTGiftWrapMessage;
                        newLineItem.BTItemType = newLineDetail.BTItemType;
                        newLineItem.BTVolumeSet = newLineDetail.BTVolumeSet;
                        newLineItem.BTTitleEditionVersion = newLineDetail.BTTitleEditionVersion;
                        newLineItem.BTBibNumber = newLineDetail.BTBibNumber;
                        newLineItem.BTPOLineItemNumber = newLineDetail.LinePONumber;
                        newLineItem.LegacyBasketLineId = int.Parse(newLineDetail.LegacyLineItemId);
                        newLineItem.LegacyBasketId = int.Parse(newLineDetail.LegacyBasketId);
                        //newLineItem.LegacyCreatedDate = newLineDetail.LegacyCreatedDate;
                        //newLineItem.LegacyModifiedDate = newLineDetail.LegacyModifiedDate;

                        orderForm.LineItems.Add(newLineItem);
                    }
                }
                // Add OrderForm to Basket
                newBasket.OrderForms.Add(orderForm);
                // Save the Basket
                newBasket.Save();


                basketResponse.LoadStatus = "Loaded";

                return basketResponse;

            }
            catch (Exception ex)
            {
                // build response message
                basketResponse.LoadStatus = "Failed";
                basketResponse.ErrorMessage = ex.Message;
                
                // log exception
                return basketResponse;

                //throw new Exception(ex.Message);

            }

        }

               /// <summary>
        /// <summary>
        /// Takes input from the calling application and returns an XML report from CyberSource
        /// </summary>
        /// <param name="reportRequest"></param>
        /// <returns>XML Response from CyberSource; along with an ErrorMessage if one is encountered in calling the CyberSource Service</returns>
        public string GetCyberSourceReport(CyberSourceReportRequest reportRequest)
        {
            // CyberSourceReportResponse ReportResponse = new CyberSourceReportResponse();

            CyberSourceReportRequest request = new CyberSourceReportRequest();

            string baseUrl = Config.CyberSourceReportsURL;
            string completeUrl;
            string sResults = "";

            HttpWebResponse iResponse = null;


            completeUrl = baseUrl;

            completeUrl = completeUrl.Replace("{CyberSourceServerName}", reportRequest.CyberSourceServerName);
            completeUrl = completeUrl.Replace("{Year}", reportRequest.Year);
            completeUrl = completeUrl.Replace("{Month}", reportRequest.Month);
            completeUrl = completeUrl.Replace("{Day}", reportRequest.Day);
            completeUrl = completeUrl.Replace("{CyberSourceMerchantId}", reportRequest.CyberSourceMerchantId);
            completeUrl = completeUrl.Replace("{ReportName}", reportRequest.ReportName);
            completeUrl = completeUrl.Replace("{ReportFormat}", reportRequest.ReportFormat);

            try
            {

                NetworkCredential myCredentials = new NetworkCredential("", "", "");
                myCredentials.Domain = "";
                myCredentials.UserName = reportRequest.CyberSourceUserName;
                myCredentials.Password = reportRequest.CyberSourcePassword;

                // Create a WebRequest with the specified URL. 
                WebRequest myWebRequest = WebRequest.Create(completeUrl);
                myWebRequest.Credentials = myCredentials;

                // Send the request and wait for a response.
                WebResponse myWebResponse = myWebRequest.GetResponse();

                // Process the response.
                StreamReader oReader = new StreamReader(myWebResponse.GetResponseStream());
                sResults = oReader.ReadToEnd();
                oReader.Close();

                // Release the resources of the response object.
                myWebResponse.Close();


                return sResults;


            }
            catch (Exception ex)
            {
                // build response message
                //iResponse.ErrorMessage = ex.Message;

                // log exception
                return ex.Message;
            }

        }
        /// <summary>
        /// STILL IN DEV. IGNORE
        /// </summary>
        /// <param name="purgeBasketThresholdInDays"></param>
        /// <param name="maxBatchSize"></param>
        /// <returns></returns>
        public DataSet GetAgedBaskets(string purgeBasketThresholdInDays, string maxBatchSize)
        {
            //Collection<BasketDetails> getBaskets = new Collection<BasketDetails>();
            var getBaskets = new DataSet();

            try
            {
                //PASSED BY CALLER
                double daysToPurgeBaskets = double.Parse(purgeBasketThresholdInDays);

                OrderServiceAgent msoAgent = new OrderServiceAgent(Config.OrdersServiceUrl);
                msoAgent.Credentials = CredentialCache.DefaultNetworkCredentials;
                OrderManagementContext context = OrderManagementContext.Create(msoAgent);
                BasketManager basketManager = context.BasketManager;

                // Create a search clause
                DataSet searchableProperties = basketManager.GetSearchableProperties(CultureInfo.CurrentUICulture.ToString());
                SearchClauseFactory searchClauseFactory = basketManager.GetSearchClauseFactory(searchableProperties, "Basket");

                // Check criteria for purging
                SearchClause clause = searchClauseFactory.CreateClause(ExplicitComparisonOperator.GreaterThan, "LastModified", DateTime.Now.AddDays(-(daysToPurgeBaskets)));

                //only get the firts "maxBatchSize" 

                // sample for testing only
                // getBaskets = basketManager.GetBasketAsDataSet(Guid.Parse("{d2596d73-33c1-4bbb-b31d-b25f4b576ca4}"));

            }
            catch 
            {
                throw;
            }

            return getBaskets;
        }

        /// <summary>
        /// Purge Old Baskets where LastModifiedDate > 90 days
        /// </summary>
        public void PurgeAgedBaskets(string purgeBasketThresholdInDays)
        {
            try
            {

                //PASSED BY CALLER
                double daysToPurgeBaskets = double.Parse(purgeBasketThresholdInDays);

                OrderServiceAgent msoAgent = new OrderServiceAgent(Config.OrdersServiceUrl);
                msoAgent.Credentials = CredentialCache.DefaultNetworkCredentials;

                // run CS service in agent-mode to get access to the CS API
                OrderManagementContext context = OrderManagementContext.Create(msoAgent);
                BasketManager basketManager = context.BasketManager;

                // Create a search clause
                DataSet searchableProperties = basketManager.GetSearchableProperties(CultureInfo.CurrentUICulture.ToString());
                SearchClauseFactory searchClauseFactory = basketManager.GetSearchClauseFactory(searchableProperties, "Basket");

                // Check criteria for purging :: look for a case where "IsPendingPurge = True" to perform delete
                SearchClause clause = searchClauseFactory.CreateClause(ExplicitComparisonOperator.GreaterThan, "LastModified", DateTime.Now.AddDays(-(daysToPurgeBaskets)));

                // Delete baskets that match the conditions
                int recordsDeleted;
                basketManager.DeleteBaskets(clause, out recordsDeleted);

                // For debugging only::
                //Console.WriteLine("Deleted " + recordsDeleted.ToString() + " records.");
                //Console.ReadLine();
            }
            catch
            {
                // log exception
            }

        }

        public OrderDetailResponseCollection GetOrderDetails(string accountNum)
        {
            try
            {
                OrderDetailResponseCollection orderColl = new OrderDetailResponseCollection();
                string conn = ConfigurationManager.ConnectionStrings["OrdersConnectionString"].ConnectionString;
                DataTable dt = new DataTable();
                // add logic here 
                DataAccess da = new DataAccess();
                dt = da.GetOrderDetails(accountNum, conn);
                foreach (DataRow dr in dt.Rows)
                {
                    OrderDetailResponse resp = new OrderDetailResponse();
                    resp.AccountNum = Convert.ToString( dr[0]);
                    resp.ISBN = Convert.ToString(dr[1]);
                    resp.PONumber = Convert.ToString(dr[2]);
                    resp.Quantity = Convert.ToString(dr[3]);
                    orderColl.Add(resp);
                }
                return orderColl;
            }

            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }


        #endregion

        
    }
}
