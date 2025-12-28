using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Microsoft.CommerceServer.Marketing;
using Microsoft.CommerceServer;
using Microsoft.CommerceServer.Runtime;
using System.Net;

// reference "common" project
using BTNextGen.Services.Common;

namespace BTNextGen.Services.IPromotions
{

    public class IPromotion : IPromotionsService
    {

        # region public methods
        /// <summary>
        /// Output a Promotion Collection Object
        /// </summary>
        /// <returns>Promotions Collection Object</returns>
        public Collection<Promotion> GetPromotions()
        {
            Collection<Promotion> getPromotions = new Collection<Promotion>();

            // convert the datatable to a promotion object
            foreach (DataRow promoRow in GetPromotionsTable().Rows)
            {
                getPromotions.Add(ConvertToPromoObject(promoRow));
            }

            return getPromotions;
        }

        /// <summary>
        /// Output a Promotions DataTable
        /// </summary>
        /// <returns>Promotions DaTatable</returns>
        public DataTable GetPromotionsTable()
        {

            // Make sure your user account has sufficient Marketing permissions to perform these operations.
            DataTable promoTable = new DataTable();

            try
            {
                // Get the Commerce Server marketing webservice URL from the web.config
                // Create an instance of the Marketing System in Agent-mode
                MarketingServiceAgent msAgent = new MarketingServiceAgent(Config.MarketingServiceUrl);
               
                /// Use default credentials to authenticate to the web service
                msAgent.Credentials = CredentialCache.DefaultNetworkCredentials;

                /// Get Context of the CS arketing System
                MarketingContext marketingSystemContext = MarketingContext.Create(msAgent);

                /// Get only a subset of data needed for this service 
                SearchOptions searchOptions = new SearchOptions();
                searchOptions.StartRecord = 1;
                searchOptions.PropertiesToReturn = string.Format("{0},{1},{2},{3},{4}", "Name", "Description", "DiscountType", "StartDate", "EndDate");
                searchOptions.SortProperties = string.Format("{0},{1}", "StartDate", "EndDate");
                searchOptions.SortDescending = true;

                // Get only discounts; no ads or direct mail campaign items
                SearchClauseFactory searchFactory = marketingSystemContext.CampaignItems.GetSearchClauseFactory(CampaignItemType.Discount);
                //SearchClauseFactory searchFactory = marketingSystemContext.CampaignItems.GetSearchClauseFactory(); // use if you don't need to filter by discounts only
                
                // Send all promotions per ERP requirements : To Filter by "Approved discounts" use the "Active" flag
                // SearchClause searchClause = searchFactory.CreateClause(Microsoft.CommerceServer.ImplicitComparisonOperator.IsTrue, "Active");
                SearchClause searchClause = searchFactory.CreateClause();

                promoTable = marketingSystemContext.CampaignItems.Search(searchClause, searchOptions, false).Tables[0];
            }
            catch (Exception ex)
            {
                // TODO: add exception logging here
                // Console.WriteLine("Exception: {0}\r\nMessage: {1}", ex.GetType(), ex.Message);
                if (ex.InnerException != null)
                {
                    // Console.WriteLine("\r\nInner Exception: {0}\r\nMessage: {1}", ex.InnerException.GetType(), ex.InnerException.Message);
                }
            }

            return promoTable;
        }

        # endregion

        # region private methods
        private Promotion ConvertToPromoObject(DataRow promoRow)
        {
            Promotion promo = new Promotion();

            try
            {
                //promo.PromoId = promoRow.Field<int>("ID");
                promo.PromoName = promoRow.Field<string>("Name");
                promo.PromoDescription = promoRow.Field<string>("Description");
                promo.PromoType = promoRow.Field<string>("DiscountType");
                promo.PromoStartDate = DateTime.Parse(promoRow.Field<DateTime>("StartDate").ToString()).ToString("yyyy-MM-dd-HH.mm.ss.000000");
                promo.PromoEndDate = DateTime.Parse(promoRow.Field<DateTime>("EndDate").ToString()).ToString("yyyy-MM-dd-HH.mm.ss.000000");

            }
            catch(Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Console.WriteLine("\r\nInner Exception: {0}\r\nMessage: {1}", ex.InnerException.GetType(), ex.InnerException.Message);
                }
            }
            return promo;
        }
        # endregion
    }
}
